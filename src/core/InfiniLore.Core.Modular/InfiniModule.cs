// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfiniLore.Core.Modular;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class InfiniModule {
    private readonly List<Assembly> _assemblies = [];
    public IEnumerable<Assembly> Assemblies => _assemblies.AsReadOnly();

    private List<InfiniModule> _subModules = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    internal void Configure(IServiceCollection services) {
        _subModules = new List<InfiniModule>();

        _assemblies.Add(GetType().Assembly);
        OnConfiguring(services);
        
        foreach (InfiniModule subModule in _subModules) {
            subModule.Configure(services);
            
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (Assembly assembly in subModule._assemblies) {
                if (_assemblies.Contains(assembly)) continue;
                _assemblies.Add(assembly);
            }
        }

        _subModules = null!;
    }
    
    internal void Startup(WebApplication app) {
        OnStartup(app);
    }

    /// <summary>
    /// Called when the module is loaded.
    /// This is during the service registration phase.
    /// </summary>
    /// <param name="services"></param>
    protected abstract void OnConfiguring(IServiceCollection services);

    /// <summary>
    /// Called when the module is loaded up fully.
    /// This is during the stage where the application is fully bootstrapped.
    /// </summary>
    /// <param name="app"></param>
    protected abstract void OnStartup(WebApplication app);
    
    protected void AddSubModule<TModule>() where TModule : InfiniModule, new() {
        _subModules.Add(new TModule());
    }
}
