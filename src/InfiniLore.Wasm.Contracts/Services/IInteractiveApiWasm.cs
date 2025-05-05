// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Kiota;
using InfiniLore.Shared.Services.InteractiveApi;
using System.Text.Json;

namespace InfiniLore.Wasm.Contracts.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInteractiveApiWasm : IInteractiveApi{
    InfiniLoreApiClient ApiClient { get; }
    JsonSerializerOptions JsonOptions { get; }
}
