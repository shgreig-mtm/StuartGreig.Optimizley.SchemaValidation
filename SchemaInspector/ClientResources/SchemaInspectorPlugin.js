define([
    "dojo/_base/declare",
    "epi-cms/plugin-area/navigation-tree"
], function (
    declare,
    pluginArea
) {
    return declare(null, {
        
        postscript: function () {
            this.inherited(arguments);
            this._addSchemaInspectorButton();
        },

        _addSchemaInspectorButton: function () {
            // Add floating button to open Schema Inspector
            var style = document.createElement('style');
            style.textContent = `
                .schema-inspector-fab {
                    position: fixed;
                    bottom: 30px;
                    right: 30px;
                    width: 56px;
                    height: 56px;
                    border-radius: 50%;
                    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                    color: white;
                    border: none;
                    box-shadow: 0 4px 12px rgba(0,0,0,0.3);
                    cursor: pointer;
                    font-size: 24px;
                    z-index: 9999;
                    transition: all 0.3s ease;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                }
                .schema-inspector-fab:hover {
                    transform: scale(1.1);
                    box-shadow: 0 6px 20px rgba(102, 126, 234, 0.5);
                }
                .schema-inspector-tooltip {
                    position: absolute;
                    right: 70px;
                    background: #333;
                    color: white;
                    padding: 8px 12px;
                    border-radius: 4px;
                    white-space: nowrap;
                    opacity: 0;
                    pointer-events: none;
                    transition: opacity 0.3s;
                    font-size: 14px;
                }
                .schema-inspector-fab:hover .schema-inspector-tooltip {
                    opacity: 1;
                }
            `;
            document.head.appendChild(style);

            var button = document.createElement('button');
            button.className = 'schema-inspector-fab';
            button.innerHTML = '📊<span class="schema-inspector-tooltip">Schema Inspector</span>';
            button.title = 'Inspect Schema.org markup';
            
            button.onclick = function() {
                // Get current content reference from URL
                var contentMatch = window.location.href.match(/contentLink=([0-9_]+)/) || 
                                 window.location.href.match(/\/([0-9]+)\/edit/);
                var contentId = contentMatch ? contentMatch[1].replace(/_/g, '') : null;
                
                if (contentId) {
                    window.open('/modules/my-schema-plugin/schemainspector/inspector/' + contentId, 
                               '_blank', 
                               'width=1200,height=900,scrollbars=yes');
                } else {
                    alert('Could not detect content ID from current page');
                }
            };
            
            document.body.appendChild(button);
        }
    });
});
