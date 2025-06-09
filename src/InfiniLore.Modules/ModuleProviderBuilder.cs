// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Reflection;

namespace InfiniLore.Modules;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ModuleProviderBuilder {
    private IServiceCollection Services { get; init; } = null!;
    private List<IModuleSetup> Modules { get; } = new();
    private List<Assembly> Assemblies { get; } = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    private ModuleProviderBuilder() {}
    
    public static ModuleProviderBuilder Create(IServiceCollection services) {
        return new ModuleProviderBuilder {
            Services = services
        };
    }
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ModuleProviderBuilder AddModule<TModuleSetup>() where TModuleSetup : IModuleSetup, new() {
        var moduleSetup = new TModuleSetup();
        Assembly moduleAssembly = typeof(TModuleSetup).Assembly;
        
        moduleSetup.SetupServices(Services);
        
        Modules.Add(moduleSetup);
        Assemblies.Add(moduleAssembly);
        
        return this;
    }

    public ModuleProvider Build() {
        var provider =  new ModuleProvider {
            Modules = Modules.ToImmutableArray(),
            Assemblies = Assemblies.ToImmutableArray()
        };
        
        Services.AddSingleton(provider);
        return provider;
    }
}
