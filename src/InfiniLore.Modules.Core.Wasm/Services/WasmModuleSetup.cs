// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Core.Wasm.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class WasmModuleSetup {
    public virtual void SetupBuilder(WebAssemblyHostBuilder builder) {}
    public virtual void SetupServices(IServiceCollection services) {}
}
