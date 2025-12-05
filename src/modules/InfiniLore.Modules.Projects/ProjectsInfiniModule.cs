// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Projects;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ProjectsInfiniModule : InfiniModule {

    protected override void OnModuleRegister(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesProjects();
        
        AddSubModule<ProjectsSharedInfiniModule>();
    }
}
