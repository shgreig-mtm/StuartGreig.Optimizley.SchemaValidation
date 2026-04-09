using EPiServer.Shell.ViewComposition;

namespace SchemaInspector.UI;

/// <summary>
/// Component that appears in the editing toolbar (top bar) in CMS edit mode
/// </summary>
//[Component]  // Disabled - causes CMS to break
public class SchemaInspectorAssetPaneComponent : ComponentDefinitionBase
{
    public SchemaInspectorAssetPaneComponent() 
        : base("epi-cms.component.IFrameComponent")
    {
        Categories = ["content"];
        Title = "Schema Inspector";
        Description = "View and validate Schema.org JSON-LD markup";
        SortOrder = 100;
        PlugInAreas = ["/episerver/cms/action"];

        // The URL that will be loaded in the iframe - {contentLink} is replaced automatically
        Settings["url"] = "/modules/my-schema-plugin/schemainspector/inspector/{contentLink}";
    }
}
