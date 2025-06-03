// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Kiota;
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Wasm.Contracts.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInteractiveApiWasm : IInteractiveApi{
    InfiniLoreApiClient ApiClient { get; }
    
    string DefaultApiError { get; }
}
