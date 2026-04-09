define([
    "dojo/_base/declare",
    "epi/dependency",
    "epi/shell/command/_Command"
], function (
    declare,
    dependency,
    _Command
) {
    return declare([_Command], {
        name: "schemaInspector",
        label: "Schema",
        tooltip: "View Schema.org JSON-LD validation",
        iconClass: "epi-iconObjectReport",
        canExecute: true,

        _execute: function () {
            try {
                // Get the current content link from the context
                var context = this.model;
                var contentLink = context && context.contentLink;

                if (!contentLink) {
                    // Try alternative method
                    var contentData = dependency.resolve("epi.storeregistry").get("epi.cms.contentdata");
                    if (contentData && contentData.currentContext) {
                        contentLink = contentData.currentContext.id;
                    }
                }

                if (!contentLink) {
                    console.warn("Schema Inspector: Could not determine content link");
                    alert("Unable to determine current page. Please try again.");
                    return;
                }

                // Open Schema Inspector in new window
                var url = "/modules/my-schema-plugin/schemainspector/inspector/" + contentLink;
                var win = window.open(url, "SchemaInspector", 
                    "width=1200,height=900,scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no");

                if (!win) {
                    alert("Popup blocked. Please allow popups for this site.");
                }
            } catch (error) {
                console.error("Schema Inspector error:", error);
                alert("Error opening Schema Inspector: " + error.message);
            }
        },

        _onModelChange: function () {
            this.inherited(arguments);
            this.set("isAvailable", true);
            this.set("canExecute", true);
        }
    });
});
