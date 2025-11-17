// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetsSharedInfiniModule : InfiniModule {
    protected override void Configure() {
        Services.RegisterServicesFromInfiniLoreModulesAssetsShared();
    }
}
