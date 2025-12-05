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
    
    private readonly List<InfiniModule> _subModules = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    internal void StartModuleRegister(IServiceCollection services) {
        _assemblies.Add(GetType().Assembly);
        OnModuleRegister(services);
        
        foreach (InfiniModule subModule in _subModules) {
            subModule.StartModuleRegister(services);
            
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (Assembly assembly in subModule._assemblies) {
                if (_assemblies.Contains(assembly)) continue;
                _assemblies.Add(assembly);
            }
        }
    }
    
    internal void StartModuleLoad() {
        OnModuleLoad();
    }

    /// <summary>
    /// Called when the module is loaded.
    /// This is during the service registration phase.
    /// </summary>
    /// <param name="services"></param>
    protected abstract void OnModuleRegister(IServiceCollection services);
    
    /// <summary>
    /// Called when the module is loaded up fully.
    /// This is during the stage where the application is fully bootstrapped.
    /// </summary>
    protected virtual void OnModuleLoad() {
        
    }
    
    protected void AddSubModule<TModule>() where TModule : InfiniModule, new() {
        _subModules.Add(new TModule());
    }
}
