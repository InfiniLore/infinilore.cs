// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.Core.Shared.Database;

namespace InfiniLore.Modules.Core.Wasm.Services.InteractiveApi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IUserInteractiveApi>]
public class UserInteractiveApiWasm : IUserInteractiveApi {

    public ValueTask<Result<IInfiniLoreUserModel>> GetUserAsync(string userId, CancellationToken ct = default) {
        throw new NotImplementedException();
    }
}
