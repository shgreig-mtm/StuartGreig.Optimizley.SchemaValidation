define([
    "dojo/_base/declare",
    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "epi/dependency"
], function (
    declare,
    _WidgetBase,
    _TemplatedMixin,
    dependency
) {
    return declare([_WidgetBase, _TemplatedMixin], {
        
        templateString: '<div class="schema-inspector-widget">' +
            '<div style="padding: 20px; text-align: center;">' +
            '<button data-dojo-attach-point="inspectButton" class="epi-primary" style="padding: 12px 24px; font-size: 14px;">' +
            '<span style="font-size: 20px; margin-right: 8px;">📊</span> Inspect Schema' +
            '</button>' +
            '<p style="margin-top: 15px; font-size: 12px; color: #666;">View JSON-LD structured data for this page</p>' +
            '</div>' +
            '</div>',
        
        postCreate: function () {
            this.inherited(arguments);
            this.own(
                this.inspectButton.onclick = this._openInspector.bind(this)
            );
        },
        
        _openInspector: function () {
            var contentContext = dependency.resolve("epi.storeregistry").get("epi.cms.contentdata");
            var currentContext = contentContext.currentContext || {};
            var contentLink = currentContext.id;
            
            if (contentLink) {
                window.open(
                    "/modules/my-schema-plugin/schemainspector/inspector/" + contentLink,
                    "SchemaInspector",
                    "width=1200,height=900,scrollbars=yes,resizable=yes"
                );
            } else {
                alert("Please select a content item first");
            }
        }
    });
});
