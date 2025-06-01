// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.Core.Shared.Database;
using InfiniLore.Modules.Core.Wasm.Contracts.Services;

namespace InfiniLore.Modules.Core.Wasm.Services.InteractiveApi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiUsers>]
public class InteractiveApiWasmUsers(IInteractiveApiWasm interactiveApi) : IInteractiveApiUsers {

    public ValueTask<Result<IInfiniLoreUserModel>> GetUserAsync(string userId, CancellationToken ct = default) 
        => throw new NotImplementedException();

    public ValueTask<Result> UpsertProfileImageAsync(string userId, string contentType, Stream file, CancellationToken ct = default) 
        => throw new NotImplementedException();
}
