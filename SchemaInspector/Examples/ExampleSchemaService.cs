using EPiServer.Core;
using SchemaInspector.Services;

namespace SchemaInspector.Examples;

/// <summary>
/// Example implementation of ISchemaService showing how to generate
/// custom JSON-LD markup for different content types.
/// </summary>
/// <remarks>
/// Copy this class to your project and modify it to match your content model.
/// Then register it in Startup.cs:
/// services.AddSchemaInspector&lt;ExampleSchemaService&gt;();
/// </remarks>
public class ExampleSchemaService : ISchemaService
{
    public string GetJsonLd(IContent content)
    {
        // Basic WebPage schema for all content
        var schema = GenerateWebPageSchema(content);

        // Add type-specific schemas
        // Uncomment and modify these examples based on your content types:

        /*
        schema = content switch
        {
            ArticlePage article => GenerateArticleSchema(article),
            ProductPage product => GenerateProductSchema(product),
            EventPage eventPage => GenerateEventSchema(eventPage),
            _ => schema
        };
        */

        return schema;
    }

    private string GenerateWebPageSchema(IContent content)
    {
        var name = content.Name.Replace("\"", "\\\"");
        
        return $$"""
        {
          "@context": "https://schema.org",
          "@type": "WebPage",
          "name": "{{name}}",
          "description": "Page content from Optimizely CMS"
        }
        """;
    }

    // Example: Article schema
    /*
    private string GenerateArticleSchema(ArticlePage article)
    {
        var headline = article.Heading?.Replace("\"", "\\\"") ?? article.Name.Replace("\"", "\\\"");
        var description = article.TeaserText?.Replace("\"", "\\\"") ?? string.Empty;
        var author = article.Author?.Replace("\"", "\\\"") ?? "Unknown";
        var datePublished = article.StartPublish?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? string.Empty;
        var dateModified = article.Changed.ToString("yyyy-MM-ddTHH:mm:ssZ");
        
        return $$"""
        {
          "@context": "https://schema.org",
          "@type": "Article",
          "headline": "{{headline}}",
          "description": "{{description}}",
          "author": {
            "@type": "Person",
            "name": "{{author}}"
          },
          "datePublished": "{{datePublished}}",
          "dateModified": "{{dateModified}}"
        }
        """;
    }
    */

    // Example: Product schema
    /*
    private string GenerateProductSchema(ProductPage product)
    {
        var name = product.Name.Replace("\"", "\\\"");
        var description = product.Description?.Replace("\"", "\\\"") ?? string.Empty;
        var price = product.Price.ToString("F2");
        var currency = product.Currency ?? "USD";
        var availability = product.InStock ? "InStock" : "OutOfStock";
        
        return $$"""
        {
          "@context": "https://schema.org",
          "@type": "Product",
          "name": "{{name}}",
          "description": "{{description}}",
          "offers": {
            "@type": "Offer",
            "price": "{{price}}",
            "priceCurrency": "{{currency}}",
            "availability": "https://schema.org/{{availability}}"
          }
        }
        """;
    }
    */

    // Example: Event schema
    /*
    private string GenerateEventSchema(EventPage eventPage)
    {
        var name = eventPage.Name.Replace("\"", "\\\"");
        var description = eventPage.Description?.Replace("\"", "\\\"") ?? string.Empty;
        var startDate = eventPage.EventStartDate?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? string.Empty;
        var endDate = eventPage.EventEndDate?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? string.Empty;
        var location = eventPage.Location?.Replace("\"", "\\\"") ?? string.Empty;
        
        return $$"""
        {
          "@context": "https://schema.org",
          "@type": "Event",
          "name": "{{name}}",
          "description": "{{description}}",
          "startDate": "{{startDate}}",
          "endDate": "{{endDate}}",
          "location": {
            "@type": "Place",
            "name": "{{location}}"
          }
        }
        """;
    }
    */
}
