// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.Builders;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ComponentTreeBuilder {
    private readonly Queue<ComponentBuilder> _componentQueue = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ComponentBuilder AddComponent(string componentName) {
        var componentBuilder = new ComponentBuilder(componentName);
        _componentQueue.Enqueue(componentBuilder);
        return componentBuilder;
    }

    public RenderFragment ToRenderFragment()
        => builder => {
            int sequence = 0;

            while (_componentQueue.TryDequeue(out ComponentBuilder? cb)) {
                cb.ToRenderFragment(builder, ref sequence);
            }
        };
}
