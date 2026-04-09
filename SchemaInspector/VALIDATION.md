# Schema Validation Implementation

This document describes the validation functionality added to the Schema Inspector.

## Overview

The Schema Inspector now validates JSON-LD markup in two ways:
1. **JSON Syntax Validation** - Ensures the generated markup is valid JSON
2. **Schema.org Structure Validation** - Verifies compliance with Schema.org specifications

## Implementation

### Services

#### `ISchemaValidator` Interface
```csharp
public interface ISchemaValidator
{
    SchemaValidationResult Validate(string jsonLd);
}
```

#### `SchemaValidator` Implementation
The validator performs the following checks:

**JSON Syntax Validation:**
- Attempts to parse the JSON string
- Returns detailed syntax errors if parsing fails

**Schema.org Structure Validation:**
- Verifies presence of required `@context` property
- Checks that `@context` references schema.org
- Validates presence of `@type` property
- Type-specific validation for required properties
- Recursive validation of nested objects
- SEO recommendations based on schema type

### Supported Schema Types

The validator includes built-in validation for these common Schema.org types:

| Type | Required Properties |
|------|-------------------|
| Article, NewsArticle, BlogPosting | headline |
| Product | name |
| Event | name, startDate |
| Organization, Person | name |
| Place, LocalBusiness | name |
| Review | itemReviewed, reviewRating |
| Recipe | name |
| VideoObject | name, description, thumbnailUrl, uploadDate |
| ImageObject | contentUrl |
| WebPage | name |
| WebSite | name, url |
| BreadcrumbList | itemListElement |
| FAQPage | mainEntity |
| HowTo | name, step |
| JobPosting | title, description |

### Validation Result

The `SchemaValidationResult` class contains:

```csharp
public class SchemaValidationResult
{
    public bool IsValidJson { get; set; }
    public bool IsValidSchema { get; set; }
    public List<string> Errors { get; set; }
    public List<string> Warnings { get; set; }
    public JsonDocument? ParsedJson { get; set; }
    public bool IsValid => IsValidJson && IsValidSchema && Errors.Count == 0;
}
```

## UI Display

### Validation Status Panel
The validation results are displayed in a dedicated panel showing:
- ✅ JSON Syntax Valid/Invalid status
- ✅ Schema.org Valid/Invalid status
- Error messages (if any) with red styling
- Warning messages (if any) with yellow styling

### Visual Indicators
- **Green checkmark (✓)** for valid
- **Red X (✕)** for invalid
- **⚠ icon** for errors
- **ℹ icon** for warnings

## Example Errors and Warnings

### Common Errors
```
- Missing required '@context' property
- Missing required '@type' property
- Missing required property 'headline' for type 'Article'
- Invalid JSON syntax: Unexpected character at position 42
```

### Common Warnings
```
- @context should reference 'https://schema.org'
- Consider adding 'author' property for better SEO
- Consider adding 'image' property for rich results
- Schema type 'CustomType' is not in the common types list
```

## Usage

The validation happens automatically when the Schema Inspector panel loads. Developers don't need to do anything special - just implement `ISchemaService` as usual.

### Custom Validation

If you need custom validation logic, you can implement your own `ISchemaValidator`:

```csharp
public class CustomSchemaValidator : ISchemaValidator
{
    public SchemaValidationResult Validate(string jsonLd)
    {
        // Your custom validation logic
        var result = new SchemaValidationResult();
        // ... add your checks
        return result;
    }
}
```

Then register it before calling `AddSchemaInspector()`:

```csharp
services.AddSingleton<ISchemaValidator, CustomSchemaValidator>();
services.AddSchemaInspector<MySchemaService>();
```

## Benefits

1. **Immediate Feedback** - Editors see validation results without leaving the CMS
2. **No External Dependencies** - Validation happens server-side
3. **Developer-Friendly** - Clear error messages help fix issues quickly
4. **SEO Optimization** - Warnings suggest improvements for better search results
5. **Type Safety** - Validates structure based on Schema.org specifications

## Testing

To test the validation:

1. Generate invalid JSON (e.g., missing closing brace)
   - Should show "JSON Syntax Invalid" with error details

2. Generate valid JSON but missing required properties
   - Should show "Schema.org Invalid" with missing property errors

3. Generate valid schema with all required properties
   - Should show both "JSON Syntax Valid" and "Schema.org Valid"

4. Add optional but recommended properties (like `image` or `author`)
   - Warnings should disappear when added
