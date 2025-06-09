// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Immutable;
using System.Reflection;

namespace InfiniLore.Modules;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ModuleProvider {
    public required ImmutableArray<IModuleSetup> Modules { private get; init; }
    public required ImmutableArray<Assembly> Assemblies { private get; init; }
    
    public IEnumerable<Assembly> GetAssemblies()
        => Assemblies;

    public IEnumerable<Assembly> GetComponentAssemblies()
        => Modules.Select(static m => m.ComponentAssembly);
}
