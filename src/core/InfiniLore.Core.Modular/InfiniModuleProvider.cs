// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using System.Collections.Immutable;
using System.Reflection;

namespace InfiniLore.Core.Modular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniModuleProvider(IEnumerable<InfiniModule> modules) : IInfiniModuleProvider {
    public ImmutableArray<FrozenInfiniModule> Modules { get; } =  modules.Select(FrozenInfiniModule.FromModule).ToImmutableArray();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void StartupModules(WebApplication app) {
        foreach (FrozenInfiniModule infiniModule in Modules) {
            infiniModule.Startup(app);
        }
    }
    
    public IEnumerable<Assembly> GetRegisteredAssemblies() {
        IEnumerable<Assembly> assemblies = Modules
            .SelectMany(m => m.Assemblies)
            .Distinct();
        
        return assemblies;
    }
}