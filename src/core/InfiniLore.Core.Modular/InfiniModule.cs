// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfiniLore.Core.Modular;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class InfiniModule {
    private readonly List<Assembly> _assemblies = [];
    public IEnumerable<Assembly> Assemblies => _assemblies.AsReadOnly();
    protected IServiceCollection Services { get; private set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    internal void Load(IServiceCollection serviceCollection) {
        Services = serviceCollection;
        _assemblies.Add(GetType().Assembly);
        Configure();
    }

    protected abstract void Configure();
    
    protected void AddSubModule<TModule>() where TModule : InfiniModule, new() {
        var module = new TModule();
        module.Load(Services);

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (Assembly assembly in module._assemblies) {
            if (_assemblies.Contains(assembly)) continue;
            _assemblies.Add(assembly);
        }
    }
}
