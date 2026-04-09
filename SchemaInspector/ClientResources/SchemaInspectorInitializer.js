define([
    "dojo/_base/declare",
    "epi/_Module",
    "epi/dependency",
    "epi/routes"
], function (
    declare,
    _Module,
    dependency,
    routes
) {
    return declare([_Module], {
        initialize: function () {
            this.inherited(arguments);

            var componentRegistry = dependency.resolve("epi.cms.ComponentRegistry");

            // Register Schema Inspector widget in the right panel
            componentRegistry.add({
                id: "schema-inspector-widget",
                name: "Schema Inspector",
                description: "View and validate JSON-LD schema",
                widgetType: "SchemaInspector/component/SchemaInspectorWidget",
                plugInAreas: ["epi-cms.component.contentRightPanel"],
                settings: {
                    order: 100
                }
            });

            console.log("✅ Schema Inspector widget registered");
        }
    });
});
