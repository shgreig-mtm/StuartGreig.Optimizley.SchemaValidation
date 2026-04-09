define([
    "dojo/_base/declare",
    "dijit/_WidgetBase",
    "epi/dependency",
    "epi/shell/command/_Command"
], function (
    declare,
    _WidgetBase,
    dependency,
    _Command
) {
    return declare([_Command], {
        label: "Schema Inspector",
        tooltip: "Inspect Schema.org JSON-LD for this page",
        iconClass: "epi-iconObjectReport",
        
        _execute: function () {
            var contentContext = dependency.resolve("epi.storeregistry").get("epi.cms.contentdata");
            if (!contentContext) {
                alert("Could not get content context");
                return;
            }
            
            var currentContext = contentContext.currentContext || {};
            var contentLink = currentContext.id;
            
            if (!contentLink) {
                alert("Could not detect current content");
                return;
            }
            
            var url = "/modules/my-schema-plugin/schemainspector/inspector/" + contentLink;
            window.open(url, "SchemaInspector", "width=1200,height=900,scrollbars=yes,resizable=yes,toolbar=no,location=no");
        },
        
        _onModelChange: function () {
            this.inherited(arguments);
            this.set("isAvailable", true);
        }
    });
});
