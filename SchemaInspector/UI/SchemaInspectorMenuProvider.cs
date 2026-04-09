using EPiServer.Shell.Navigation;

namespace SchemaInspector.UI;

/// <summary>
/// Adds a Schema Inspector menu item to the CMS
/// </summary>
[MenuProvider]
public class SchemaInspectorMenuProvider : IMenuProvider
{
    public IEnumerable<MenuItem> GetMenuItems()
    {
        var menuItem = new UrlMenuItem("Schema Inspector", 
            "/global/cms/admin/schemainspector",
            "/episerver/schemainspectoradmin")
        {
            SortIndex = 100,
            IsAvailable = context => true
        };

        return new[] { menuItem };
    }
}
