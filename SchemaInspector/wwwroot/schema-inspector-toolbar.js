// Simple inline toolbar button for Schema Inspector
(function() {
    if (window.location.pathname.indexOf('/episerver/cms') === -1) {
        return; // Only run in CMS
    }

    console.log("🔍 Schema Inspector: Attempting to add toolbar button...");

    function addToolbarButton() {
        try {
            require([
                "dojo/_base/declare",
                "dijit/form/Button",
                "epi-cms/component/command/_GlobalToolbarCommandProvider",
                "epi/shell/command/_Command",
                "epi/dependency"
            ], function(declare, Button, _GlobalToolbarCommandProvider, _Command, dependency) {
                
                var SchemaCommand = declare([_Command], {
                    label: "Schema",
                    tooltip: "View Schema.org validation",
                    iconClass: "epi-iconObjectReport",
                    
                    _execute: function() {
                        var contentData = dependency.resolve("epi.storeregistry").get("epi.cms.contentdata");
                        var contentLink = contentData && contentData.currentContext && contentData.currentContext.id;
                        
                        if (!contentLink) {
                            alert("Cannot determine current page");
                            return;
                        }
                        
                        var url = "/modules/my-schema-plugin/schemainspector/inspector/" + contentLink;
                        window.open(url, "SchemaInspector", "width=1200,height=900");
                    }
                });
                
                var Provider = declare([_GlobalToolbarCommandProvider], {
                    constructor: function() {
                        this.inherited(arguments);
                        this.addToTrailing(new SchemaCommand(), {
                            showLabel: true,
                            widget: Button
                        });
                        console.log("✅ Schema Inspector button added!");
                    }
                });
                
                new Provider();
            });
        } catch (error) {
            console.error("❌ Schema Inspector error:", error);
        }
    }

    // Wait for Dojo to be available
    var checkInterval = setInterval(function() {
        if (typeof require !== 'undefined') {
            clearInterval(checkInterval);
            addToolbarButton();
        }
    }, 100);
    
    // Give up after 10 seconds
    setTimeout(function() {
        clearInterval(checkInterval);
    }, 10000);
})();
