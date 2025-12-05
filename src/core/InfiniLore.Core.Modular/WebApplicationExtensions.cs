// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Core.Modular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class WebApplicationExtensions {
    extension(WebApplication webApplication) {
        public WebApplication UseInfiniLoreModules() {
            var services = webApplication.Services;
            var provider = services.GetRequiredService<IInfiniModuleProvider>();
            
            provider.StartModuleLoad();
            
            return webApplication;
        }
    }
}
