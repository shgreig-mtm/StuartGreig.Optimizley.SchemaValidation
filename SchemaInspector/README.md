# Schema Inspector for Optimizely CMS 13

A Schema.org Inspector add-on for Optimizely CMS 13 that allows editors to view, test, and validate JSON-LD structured data directly from the Assets panel.

## Features

- 🎨 **Dark-themed UI** matching CMS 13 Visual Builder aesthetic
- ✅ **JSON validation** - instantly checks if your JSON-LD is valid
- 🔍 **Schema.org validation** - verifies structure and required properties
- 📋 **Copy to clipboard** for easy JSON-LD sharing
- 🌐 **Google Rich Results Test** integration (optional external validation)
- 🔄 **Auto-refresh** when content is saved
- 🎯 **Assets panel integration** for seamless editing experience
- ⚠️ **Error and warning display** with actionable feedback

## Installation

### Via NuGet (when published)

```bash
dotnet add package SchemaInspector.Optimizely
```

### Via Package Reference

```xml
<PackageReference Include="SchemaInspector.Optimizely" Version="1.0.0" />
```

## Quick Start

### 1. Register the Service

In your `Startup.cs` (or `Program.cs` for .NET 6+), add the Schema Inspector:

```csharp
using SchemaInspector.Extensions;

public void ConfigureServices(IServiceCollection services)
{
    // ... existing CMS configuration ...
    
    services.AddSchemaInspector();
}
```

### 2. Implement Your Schema Service (Optional)

The default implementation provides a basic example. To generate your own JSON-LD markup, implement the `ISchemaService` interface:

```csharp
using EPiServer.Core;
using SchemaInspector.Services;

public class MySchemaService : ISchemaService
{
    public string GetJsonLd(IContent content)
    {
        // Your custom logic to generate JSON-LD
        if (content is MyPageType page)
        {
            return $$"""
            {
              "@context": "https://schema.org",
              "@type": "Article",
              "headline": "{{page.Heading}}",
              "description": "{{page.Description}}",
              "author": {
                "@type": "Person",
                "name": "{{page.Author}}"
              }
            }
            """;
        }
        
        return string.Empty;
    }
}
```

### 3. Register Your Custom Service

Update your service registration to use your custom implementation:

```csharp
services.AddSchemaInspector<MySchemaService>();
```

## Usage

1. Open any page in the Optimizely CMS editor
2. Navigate to the **Assets** panel
3. Find the **Schema Inspector** component
4. View your JSON-LD markup with real-time validation:
   - ✅ **JSON Syntax Valid** - Your markup is valid JSON
   - ✅ **Schema.org Valid** - Your markup follows Schema.org specifications
   - **Errors** - Critical issues that need to be fixed
   - **Warnings** - Recommendations for better SEO and rich results
5. Use the **Copy JSON-LD** button to copy the markup
6. Click **Test with Google** to validate with Google's Rich Results Test (optional)

## Validation

The Schema Inspector provides two levels of validation:

### 1. JSON Syntax Validation
Ensures your generated JSON-LD is valid JSON that can be parsed by browsers and search engines.

### 2. Schema.org Structure Validation
Checks for:
- Required `@context` property referencing schema.org
- Required `@type` property with a valid Schema.org type
- Type-specific required properties (e.g., `headline` for Articles, `name` for Products)
- Nested object validation
- SEO recommendations (e.g., adding images, author information)

The validator supports common Schema.org types including:
- Article, NewsArticle, BlogPosting
- Product, Offer
- Event
- Organization, Person
- Place, LocalBusiness
- Review, Recipe
- VideoObject, ImageObject
- WebPage, WebSite
- BreadcrumbList, FAQPage, HowTo
- JobPosting
- And more...

## Auto-Refresh

The Schema Inspector automatically listens for the `epi-content-saved` message from the CMS. When content is saved, the panel will refresh to show the updated schema.

## Development

### Building the Package

```bash
cd SchemaInspector
dotnet build
dotnet pack
```

### Local Testing

1. Build the project
2. Reference it in your Optimizely project
3. Call `services.AddSchemaInspector()` in Startup.cs
4. Run your CMS application

## Architecture

### Components

- **SchemaInspectorComponent**: Registers the IFrame component in the Assets panel
- **SchemaInspectorController**: MVC controller with explicit routing for RCL compatibility
- **ISchemaService**: Interface for generating JSON-LD markup
- **DefaultSchemaService**: Placeholder implementation
- **ISchemaValidator**: Interface for validating JSON-LD markup
- **SchemaValidator**: Validates JSON syntax and Schema.org structure
- **ServiceCollectionExtensions**: Easy service registration

### Routing

The controller uses explicit routing to ensure it works correctly when hosted in a Razor Class Library:

```csharp
[Route("modules/my-schema-plugin/[controller]")]
public class SchemaInspectorController : Controller
{
    [HttpGet("Inspector/{contentLink}")]
    public IActionResult Index(ContentReference contentLink)
    {
        // ...
    }
}
```

This creates the route: `/modules/my-schema-plugin/inspector/{contentLink}`

## Requirements

- Optimizely CMS 13.x
- .NET 10.0
- Modern browser with Clipboard API support

## License

MIT

## Contributing

Contributions welcome! Please submit pull requests or issues.

## Support

For issues or questions, please create an issue in the repository.
