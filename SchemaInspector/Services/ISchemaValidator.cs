using System.Text.Json;

namespace SchemaInspector.Services;

/// <summary>
/// Service interface for validating Schema.org JSON-LD markup.
/// </summary>
public interface ISchemaValidator
{
    /// <summary>
    /// Validates the given JSON-LD markup.
    /// </summary>
    /// <param name="jsonLd">The JSON-LD markup to validate</param>
    /// <returns>Validation result containing status and any errors/warnings</returns>
    SchemaValidationResult Validate(string jsonLd);
}

/// <summary>
/// Result of schema validation.
/// </summary>
public class SchemaValidationResult
{
    public bool IsValidJson { get; set; }
    public bool IsValidSchema { get; set; }
    public List<string> Errors { get; set; } = [];
    public List<string> Warnings { get; set; } = [];
    public JsonDocument? ParsedJson { get; set; }
    
    public bool IsValid => IsValidJson && IsValidSchema && Errors.Count == 0;
}

/// <summary>
/// Default implementation of schema validator.
/// Validates JSON syntax and basic Schema.org structure.
/// </summary>
public class SchemaValidator : ISchemaValidator
{
    // Common Schema.org types and their required properties
    private static readonly Dictionary<string, string[]> RequiredProperties = new()
    {
        { "Article", ["headline"] },
        { "NewsArticle", ["headline"] },
        { "BlogPosting", ["headline"] },
        { "Product", ["name"] },
        { "Event", ["name", "startDate"] },
        { "Organization", ["name"] },
        { "Person", ["name"] },
        { "Place", ["name"] },
        { "Review", ["itemReviewed", "reviewRating"] },
        { "Recipe", ["name"] },
        { "VideoObject", ["name", "description", "thumbnailUrl", "uploadDate"] },
        { "ImageObject", ["contentUrl"] },
        { "WebPage", ["name"] },
        { "WebSite", ["name", "url"] },
        { "BreadcrumbList", ["itemListElement"] },
        { "FAQPage", ["mainEntity"] },
        { "HowTo", ["name", "step"] },
        { "JobPosting", ["title", "description"] },
        { "LocalBusiness", ["name"] }
    };

    public SchemaValidationResult Validate(string jsonLd)
    {
        var result = new SchemaValidationResult();

        if (string.IsNullOrWhiteSpace(jsonLd))
        {
            result.IsValidJson = false;
            result.IsValidSchema = false;
            result.Errors.Add("JSON-LD markup is empty");
            return result;
        }

        // Step 1: Validate JSON syntax
        try
        {
            result.ParsedJson = JsonDocument.Parse(jsonLd);
            result.IsValidJson = true;
        }
        catch (JsonException ex)
        {
            result.IsValidJson = false;
            result.IsValidSchema = false;
            result.Errors.Add($"Invalid JSON syntax: {ex.Message}");
            return result;
        }

        // Step 2: Validate Schema.org structure
        try
        {
            ValidateSchemaStructure(result);
        }
        catch (Exception ex)
        {
            result.IsValidSchema = false;
            result.Errors.Add($"Schema validation error: {ex.Message}");
        }

        return result;
    }

    private void ValidateSchemaStructure(SchemaValidationResult result)
    {
        if (result.ParsedJson == null)
        {
            result.IsValidSchema = false;
            result.Errors.Add("JSON document is null");
            return;
        }

        var root = result.ParsedJson.RootElement;

        // Check if it's an array of schemas or a single schema
        if (root.ValueKind == JsonValueKind.Array)
        {
            bool hasValidSchema = false;
            foreach (var item in root.EnumerateArray())
            {
                ValidateSingleSchema(item, result);
                if (result.IsValidSchema)
                {
                    hasValidSchema = true;
                }
            }
            result.IsValidSchema = hasValidSchema;
        }
        else if (root.ValueKind == JsonValueKind.Object)
        {
            ValidateSingleSchema(root, result);
        }
        else
        {
            result.IsValidSchema = false;
            result.Errors.Add("JSON-LD must be an object or array");
        }
    }

    private void ValidateSingleSchema(JsonElement schema, SchemaValidationResult result)
    {
        result.IsValidSchema = true;

        // Check for @context
        if (!schema.TryGetProperty("@context", out var context))
        {
            result.Errors.Add("Missing required '@context' property");
            result.IsValidSchema = false;
        }
        else
        {
            var contextValue = context.ValueKind == JsonValueKind.String 
                ? context.GetString() 
                : null;
            
            if (contextValue == null || !contextValue.Contains("schema.org"))
            {
                result.Warnings.Add("@context should reference 'https://schema.org'");
            }
        }

        // Check for @type
        if (!schema.TryGetProperty("@type", out var typeProperty))
        {
            result.Errors.Add("Missing required '@type' property");
            result.IsValidSchema = false;
            return;
        }

        string? schemaType = typeProperty.ValueKind == JsonValueKind.String 
            ? typeProperty.GetString() 
            : null;

        if (string.IsNullOrEmpty(schemaType))
        {
            result.Errors.Add("@type property must be a non-empty string");
            result.IsValidSchema = false;
            return;
        }

        // Check for required properties based on type
        if (RequiredProperties.TryGetValue(schemaType, out var requiredProps))
        {
            foreach (var requiredProp in requiredProps)
            {
                if (!schema.TryGetProperty(requiredProp, out _))
                {
                    result.Errors.Add($"Missing required property '{requiredProp}' for type '{schemaType}'");
                    result.IsValidSchema = false;
                }
            }
        }
        else
        {
            // Unknown type - add a warning but don't fail validation
            result.Warnings.Add($"Schema type '{schemaType}' is not in the common types list. Ensure it's a valid Schema.org type.");
        }

        // Validate nested objects recursively
        ValidateNestedObjects(schema, result);

        // Additional recommendations
        AddRecommendations(schema, schemaType, result);
    }

    private void ValidateNestedObjects(JsonElement element, SchemaValidationResult result)
    {
        foreach (var property in element.EnumerateObject())
        {
            var value = property.Value;
            
            // If it's an object with @type, validate it as a nested schema
            if (value.ValueKind == JsonValueKind.Object && value.TryGetProperty("@type", out _))
            {
                ValidateSingleSchema(value, result);
            }
            else if (value.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in value.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.Object && item.TryGetProperty("@type", out _))
                    {
                        ValidateSingleSchema(item, result);
                    }
                }
            }
        }
    }

    private void AddRecommendations(JsonElement schema, string? schemaType, SchemaValidationResult result)
    {
        // Type-specific recommendations
        switch (schemaType)
        {
            case "Article":
            case "NewsArticle":
            case "BlogPosting":
                if (!schema.TryGetProperty("author", out _))
                    result.Warnings.Add("Consider adding 'author' property for better SEO");
                if (!schema.TryGetProperty("datePublished", out _))
                    result.Warnings.Add("Consider adding 'datePublished' property for better SEO");
                if (!schema.TryGetProperty("image", out _))
                    result.Warnings.Add("Consider adding 'image' property for rich results");
                break;

            case "Product":
                if (!schema.TryGetProperty("image", out _))
                    result.Warnings.Add("Consider adding 'image' property for rich results");
                if (!schema.TryGetProperty("offers", out _))
                    result.Warnings.Add("Consider adding 'offers' property for product rich results");
                break;

            case "Event":
                if (!schema.TryGetProperty("location", out _))
                    result.Warnings.Add("Consider adding 'location' property for event rich results");
                break;
        }
    }
}
