// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using InfiniLore.Modules.Assets.Shared;

namespace InfiniLore.Modules.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetsSharedInfiniModule : InfiniModule {
    protected override void Configure() {
        Services.RegisterServicesFromInfiniLoreModulesAssetsShared();
    }
}
