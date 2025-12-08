// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfiniLore.Core.Modular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniModuleCollection(IServiceCollection services) {
    private readonly List<InfiniModule> _modules = [];
    private readonly HashSet<Assembly> _assemblies = [];
    
    public IReadOnlyCollection<Assembly> Assemblies => _assemblies;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public InfiniModuleCollection AddModule<TModule>() where TModule : InfiniModule, new() {
        var module = new TModule();
        module.Configure(services);
        
        _modules.Add(module);
        
        foreach (Assembly assembly in _modules.SelectMany(m => m.Assemblies)) {
            _assemblies.Add(assembly);
        }
        
        return this;
    }
    
    public InfiniModuleProvider Build() {
        // sort the modules depending on load order
        // TODO Create a sorter depending on application configuration
        return new InfiniModuleProvider(_modules);
    }
}
