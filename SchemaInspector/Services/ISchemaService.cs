using EPiServer.Core;

namespace SchemaInspector.Services;

/// <summary>
/// Service interface for generating Schema.org JSON-LD markup.
/// </summary>
public interface ISchemaService
{
    /// <summary>
    /// Generates JSON-LD structured data for the given content.
    /// </summary>
    /// <param name="content">The content to generate JSON-LD for</param>
    /// <returns>JSON-LD markup as a string</returns>
    string GetJsonLd(IContent content);
}

/// <summary>
/// Default implementation of ISchemaService.
/// This is a placeholder - consumers should implement their own logic.
/// </summary>
public class DefaultSchemaService : ISchemaService
{
    public string GetJsonLd(IContent content)
    {
        // Placeholder implementation - returns a basic example
        return $$"""
        {
          "@context": "https://schema.org",
          "@type": "WebPage",
          "name": "{{content.Name}}",
          "description": "Implement ISchemaService to provide custom JSON-LD markup"
        }
        """;
    }
}
