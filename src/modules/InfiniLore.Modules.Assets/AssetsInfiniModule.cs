// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;

namespace InfiniLore.Modules.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetsInfiniModule : InfiniModule {
    protected override void Configure() {
        Services.RegisterServicesFromInfiniLoreModulesAssets();
        
        AddSubModule<AssetsSharedInfiniModule>();
    }
}
