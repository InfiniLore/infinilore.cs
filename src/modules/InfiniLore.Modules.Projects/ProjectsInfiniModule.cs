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

    protected override void Configure() {
        Services.RegisterServicesFromInfiniLoreModulesProjects();
        
        AddSubModule<ProjectsSharedInfiniModule>();
    }
}
