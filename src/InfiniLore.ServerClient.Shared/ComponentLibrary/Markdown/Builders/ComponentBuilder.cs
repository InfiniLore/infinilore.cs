// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.Rendering;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.Builders;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ComponentBuilder(string componentName) {
    private string ComponentName { get; set; } = componentName;
    private readonly Dictionary<string, object> _componentProperties = new();
    private readonly Queue<ComponentBuilder> ChildComponents = new();
    private string Content { get; set; } = "";

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ComponentBuilder AddClass(string className) {
        if (!_componentProperties.TryAdd("class", className)) {
            _componentProperties["class"] += $" {className}";
        }

        return this;
    }

    public ComponentBuilder AddContent(string content) {
        Content = content;
        return this;
    }

    public ComponentBuilder AddChildComponent(string className) {
        var componentBuilder = new ComponentBuilder(className);
        ChildComponents.Enqueue(componentBuilder);
        return componentBuilder;
    }
    
    public void ToRenderFragment(RenderTreeBuilder builder, ref int sequence){
        builder.OpenElement(sequence++, ComponentName);
        builder.AddMultipleAttributes(sequence++, _componentProperties);
        builder.AddContent(sequence++, Content);
        
        while (ChildComponents.TryDequeue(out ComponentBuilder? cb)) {
            cb.ToRenderFragment(builder, ref sequence);
        }
        
        builder.CloseElement();
    }
}
