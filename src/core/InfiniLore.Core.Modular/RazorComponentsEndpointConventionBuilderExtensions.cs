// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfiniLore.Core.Modular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class RazorComponentsEndpointConventionBuilderExtensions {
    extension(RazorComponentsEndpointConventionBuilder builder) {
        public RazorComponentsEndpointConventionBuilder AddInfiniLoreModuleAssemblies(WebApplication app) {
            var provider = app.Services.GetRequiredService<IInfiniModuleProvider>();
            Assembly[] assemblies = provider.GetRegisteredAssemblies().ToArray();
            
            builder.AddAdditionalAssemblies(assemblies);
            return builder;
        }
    }
}
