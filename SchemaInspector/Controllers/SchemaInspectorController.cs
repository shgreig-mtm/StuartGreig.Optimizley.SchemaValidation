using EPiServer;
using EPiServer.Core;
using Microsoft.AspNetCore.Mvc;
using SchemaInspector.Services;

namespace SchemaInspector.Controllers;

/// <summary>
/// Controller for the Schema Inspector panel.
/// Uses explicit routing to ensure the controller is accessible when hosted in an RCL.
/// </summary>
[Route("modules/my-schema-plugin/[controller]")]
public class SchemaInspectorController : Controller
{
    private readonly IContentLoader _contentLoader;
    private readonly ISchemaService _schemaService;
    private readonly ISchemaValidator _schemaValidator;

    public SchemaInspectorController(
        IContentLoader contentLoader,
        ISchemaService schemaService,
        ISchemaValidator schemaValidator)
    {
        _contentLoader = contentLoader;
        _schemaService = schemaService;
        _schemaValidator = schemaValidator;
    }

    /// <summary>
    /// Displays the Schema Inspector panel for a given content item.
    /// </summary>
    /// <param name="contentLink">The content reference to inspect</param>
    [HttpGet("Inspector/{contentLink}")]
    public IActionResult Index(ContentReference contentLink)
    {
        return GetSchemaView(contentLink);
    }

    /// <summary>
    /// Gadget view that shows a simple UI for selecting content to inspect
    /// </summary>
    [HttpGet("Gadget")]
    public IActionResult Gadget()
    {
        return View("Gadget");
    }

    private IActionResult GetSchemaView(ContentReference contentLink)
    {
        if (ContentReference.IsNullOrEmpty(contentLink))
        {
            return View(new SchemaInspectorViewModel
            {
                Error = "No content reference provided",
                JsonLd = null,
                ContentLink = null,
                ContentUrl = null
            });
        }

        try
        {
            if (!_contentLoader.TryGet<IContent>(contentLink, out var content))
            {
                return View(new SchemaInspectorViewModel
                {
                    Error = "Content not found",
                    JsonLd = null,
                    ContentLink = contentLink,
                    ContentUrl = null
                });
            }

            // Get JSON-LD markup for the page
            var jsonLd = _schemaService.GetJsonLd(content);

            // Validate the JSON-LD markup
            var validationResult = _schemaValidator.Validate(jsonLd);

            // Try to get the public URL
            string? contentUrl = null;
            if (content is PageData page)
            {
                var urlHelper = Url;
                contentUrl = urlHelper?.Content($"~/{page.LinkURL}");
            }

            return View(new SchemaInspectorViewModel
            {
                JsonLd = jsonLd,
                ContentLink = contentLink,
                ContentUrl = contentUrl,
                ContentName = content.Name,
                ValidationResult = validationResult
            });
        }
        catch (Exception ex)
        {
            return View(new SchemaInspectorViewModel
            {
                Error = $"Error loading content: {ex.Message}",
                JsonLd = null,
                ContentLink = contentLink,
                ContentUrl = null
            });
        }
    }
}

/// <summary>
/// View model for the Schema Inspector view.
/// </summary>
public class SchemaInspectorViewModel
{
    public string? JsonLd { get; set; }
    public ContentReference? ContentLink { get; set; }
    public string? ContentUrl { get; set; }
    public string? ContentName { get; set; }
    public string? Error { get; set; }
    public SchemaValidationResult? ValidationResult { get; set; }
}
