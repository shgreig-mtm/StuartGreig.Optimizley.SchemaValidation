using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace SchemaInspector.Initialization;

/// <summary>
/// Initialization module for Schema Inspector
/// </summary>
[InitializableModule]
[ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
public class SchemaInspectorInitialization : IInitializableModule
{
    public void Initialize(InitializationEngine context)
    {
        System.Diagnostics.Debug.WriteLine("✅ Schema Inspector module initialized");
    }

    public void Uninitialize(InitializationEngine context)
    {
        // Nothing to uninitialize
    }
}
