// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Reflection;

namespace InfiniLore.Core.Modular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniModuleCollection {
    private readonly List<InfiniModule> _modules = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public InfiniModuleCollection AddModule<TModule>() where TModule : InfiniModule, new() {
        var module = new TModule();
        _modules.Add(module);
        
        return this;
    }
    
    public static InfiniModuleCollection Create() {
        var collection = new InfiniModuleCollection();
        return collection;
    }
    
    public InfiniModuleProvider Build(IServiceCollection services) {
        ImmutableArray<InfiniModule> modules = _modules.ToImmutableArray();

        foreach (InfiniModule module in modules) {
            module.Start(services);
        }
        
        ImmutableArray<Assembly> assemblies = _modules
            .SelectMany(module => module.Assemblies)
            .Distinct()
            .ToImmutableArray();
        
        return new InfiniModuleProvider(modules, assemblies);
    }
}
