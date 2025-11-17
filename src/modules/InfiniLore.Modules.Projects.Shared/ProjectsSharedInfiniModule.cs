// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using InfiniLore.Modules.Projects.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Projects;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ProjectsSharedInfiniModule : InfiniModule{

    protected override void Configure() {
        Services.RegisterServicesFromInfiniLoreModulesProjectsShared();
    }
}
