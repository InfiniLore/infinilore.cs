// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Frozen;
using System.Reflection;

namespace InfiniLore.Core.Modular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class FrozenInfiniModule(InfiniModule module) {
    private readonly Lazy<FrozenSet<Assembly>> _assemblies = new(() => module.Assemblies.ToFrozenSet());
    public FrozenSet<Assembly> Assemblies => _assemblies.Value;
    
    internal InfiniModule UnderlyingModule => module;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static FrozenInfiniModule FromModule(InfiniModule module) {
        var frozen = new FrozenInfiniModule(module);
        
        return frozen;
    }
}
