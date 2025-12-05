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
            var provider = app.Services.GetRequiredService<InfiniModuleProvider>();
            Assembly[] assemblies = provider.Assemblies.ToArray();
            
            builder.AddAdditionalAssemblies(assemblies);
            return builder;
        }
    }
}
