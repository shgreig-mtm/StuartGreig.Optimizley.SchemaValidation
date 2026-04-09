using Microsoft.AspNetCore.Mvc;

namespace SchemaInspector.ViewComponents;

/// <summary>
/// View component that injects Schema Inspector button into CMS pages
/// </summary>
public class SchemaInspectorButtonViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
