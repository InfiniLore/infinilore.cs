// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Projects;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ProjectsSharedInfiniModule : InfiniModule{

    protected override void OnConfiguring(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesProjectsShared();
    }
    
    protected override void OnStartup(WebApplication app) {
        
    }
}
