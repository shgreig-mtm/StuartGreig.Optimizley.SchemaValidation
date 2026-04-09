using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchemaInspector.Controllers;

/// <summary>
/// Admin controller for Schema Inspector tool
/// </summary>
[Authorize(Roles = "CmsAdmins,WebAdmins,Administrators")]
[Route("episerver/[controller]")]
public class SchemaInspectorAdminController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
