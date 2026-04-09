define([
    "dojo/_base/declare",
    "dijit/form/Button",
    "epi-cms/component/command/_GlobalToolbarCommandProvider",
    "SchemaInspector/SchemaInspectorToolbarCommand"
], function (
    declare,
    Button,
    _GlobalToolbarCommandProvider,
    SchemaInspectorCommand
) {
    return declare([_GlobalToolbarCommandProvider], {
        constructor: function () {
            this.inherited(arguments);

            try {
                // Add Schema Inspector button to the right side (trailing area) of the toolbar
                this.addToTrailing(new SchemaInspectorCommand(), {
                    showLabel: true,
                    widget: Button
                });

                console.log("✅ Schema Inspector button added to global toolbar");
            } catch (error) {
                console.error("❌ Failed to add Schema Inspector to toolbar:", error);
            }
        }
    });
});
