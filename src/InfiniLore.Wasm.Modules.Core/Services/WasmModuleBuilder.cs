// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using System.Reflection;

namespace InfiniLore.Wasm.Modules.Core.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class WasmModuleBuilder {
    private WebAssemblyHostBuilder WasmBuilder { get; init; } = null!;
    private WasmModuleBuilder() {}

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static WasmModuleBuilder CreateFromBuilder(WebAssemblyHostBuilder builder) {
        return new WasmModuleBuilder {
            WasmBuilder = builder
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
        serverModuleSetup.Setup(WasmBuilder); 
        
        return this;
    }
}
