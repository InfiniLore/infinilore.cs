// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Immutable;
using System.Reflection;

namespace InfiniLore.Core.Modular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniModuleProvider(ImmutableArray<InfiniModule> modules, ImmutableArray<Assembly> assemblies) {
    public IEnumerable<InfiniModule> Modules => modules;
    public IEnumerable<Assembly> Assemblies => assemblies;
}