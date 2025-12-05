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
    public ImmutableArray<InfiniModule> Modules => modules;
    public ImmutableArray<Assembly> Assemblies => assemblies;
}