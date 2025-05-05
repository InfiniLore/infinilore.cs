// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace InfiniLore.Wasm.Modules.Core.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class WasmModuleBuilder {
    private WebAssemblyHostBuilder? WasmBuilder { get; init; }
    private IServiceCollection? Services { get; init; }
    public List<Assembly> ModuleAssemblies { get; } = new();

    private WasmModuleBuilder() {}

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static WasmModuleBuilder Create(WebAssemblyHostBuilder builder) {
        return new WasmModuleBuilder {
            WasmBuilder = builder,
            Services = builder.Services
        };
    }

    public static WasmModuleBuilder Create(IServiceCollection services) {
        return new WasmModuleBuilder {
            WasmBuilder = null,
            Services = services
        };
    }

    public WasmModuleBuilder AddModule<TAssemblyEntry>() {
        Assembly assembly = typeof(TAssemblyEntry).Assembly;
        
        TypeInfo? serverModuleSetupType = assembly.DefinedTypes.FirstOrDefault(t => t.IsAssignableTo(typeof(IWasmModuleSetup)));
        if (serverModuleSetupType is null) {
            Log.Logger.Error("Could not find a server module setup type in assembly {AssemblyName}.", assembly.GetName().Name);
            return this;
        }
        
        var serverModuleSetup = (IWasmModuleSetup)Activator.CreateInstance(serverModuleSetupType.AsType())!;
        if (WasmBuilder is not null) serverModuleSetup.SetupBuilder(WasmBuilder);
        if (Services is not null) serverModuleSetup.SetupServices(Services);
        
        ModuleAssemblies.Add(assembly);
        
        return this;
    }
}
