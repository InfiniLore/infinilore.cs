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
    extension(WebApplication app) {
        public WebApplication UseInfiniLoreModules() {
            var services = app.Services;
            var provider = services.GetRequiredService<InfiniModuleProvider>();
            
            provider.StartupModules(app);
            
            return app;
        }
    }
}
