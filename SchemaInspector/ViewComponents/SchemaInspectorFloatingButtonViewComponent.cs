using Microsoft.AspNetCore.Mvc;

namespace SchemaInspector.ViewComponents;

/// <summary>
/// Adds a floating Schema Inspector button that only appears in CMS edit mode
/// </summary>
public class SchemaInspectorFloatingButtonViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        // Only show in edit mode (check if we're in the CMS context)
        var isEditMode = HttpContext.Request.Path.StartsWithSegments("/episerver/cms");
        
        if (!isEditMode)
        {
            return Content(string.Empty);
        }

        return View();
    }
}
