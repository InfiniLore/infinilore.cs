// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Wasm.Modules.Core.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class WasmModuleSetup {
    public virtual void SetupBuilder(WebAssemblyHostBuilder builder) {}
    public virtual void SetupServices(IServiceCollection services) {}
}
