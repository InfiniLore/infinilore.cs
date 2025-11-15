// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceExtensions {
    public static IServiceCollection AddInfiniLoreCore(this IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreCore();
        return services;
    }
}
