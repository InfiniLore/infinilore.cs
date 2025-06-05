// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Core.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class ServerModuleSetup {
    public virtual void SetupBuilder(WebApplicationBuilder builder) {}
    public virtual void SetupServices(IServiceCollection services) {}
}
