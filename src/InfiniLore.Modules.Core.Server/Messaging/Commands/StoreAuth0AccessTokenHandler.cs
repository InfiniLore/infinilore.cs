// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using InfiniLore.Modules.Core.Server.Auth;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.Messaging.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class StoreAuth0AccessTokenHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    IAuth0AccessTokenEncryptionService encryptionService, 
    IValidator<KeyValueEntryModel> validator,
    ILogger<StoreAuth0AccessTokenHandler> logger,
    IAccessProtectionRules protectionRules   
) : AccessProtectedCommandHandler<StoreAuth0AccessTokenRequest, bool>(logger) {
    protected override async Task<Shared.Outcome<bool>> HandleCommandAsync(StoreAuth0AccessTokenRequest command, CancellationToken ct = default) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var keyValueEntryRepository = await unitOfWork.GetRepositoryAsync<IKeyValueEntryRepository>(ct);

        Auth0AccessTokenJsonDto token = Auth0AccessTokenJsonDto.FromToken(command.Token);

        AterraEngine.Unions.Result<KeyValueEntryModel> storeResult = await keyValueEntryRepository.TryGetByKeyAsync("Auth0AccessToken", ct);
        KeyValueEntryModel store = storeResult.TryGetAsData(out KeyValueEntryModel? foundStore)
            ? foundStore
            : new KeyValueEntryModel { Key = "Auth0AccessToken" };

        if (!KeyValueEntryModel.CanSetObjectAsValueJson(token) || !store.TrySetObjectAsJsonValue(token)) return Shared.Outcome<bool>.FromError("Cannot store auth0 access token. Json conversion failed.");

        store.Value = encryptionService.Encrypt(store.Value);
        if (!(await validator.ValidateAsync(store, ct)).IsValid) return Shared.Outcome<bool>.FromError("Cannot store auth0 access token. Validation failed.");

        AterraEngine.Unions.Result result = await keyValueEntryRepository.TryAddOrUpdateAsync(store, ct);
        if (!result.TryGetAsState(out bool state)) return result.AsError;

        return state;
    }
    
    protected override ValueTask<bool> ValidateAccessAsync(StoreAuth0AccessTokenRequest command, CancellationToken ct = default) 
        => protectionRules.IsServerAsync(command.AccessingUser, ct);  
}
