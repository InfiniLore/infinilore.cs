// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace InfiniLore.WasmClient;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    private async static Task Main(string[] args) {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        await builder.Build().RunAsync();
    }
}
