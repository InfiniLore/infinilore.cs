// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using FluentValidation;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Users.Services;
using JetBrains.Annotations;

namespace InfiniLore.Server.Modules.Users.Messaging.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class StoreAuth0AccessTokenHandler(IUnitOfWorkFactory unitOfWorkFactory, IAuth0AccessTokenEncryptionService encryptionService, IValidator<IKeyValueEntry> validator) : CommandHandler<StoreAuth0AccessTokenRequest, MessageResponse<bool>> {
    public override async Task<MessageResponse<bool>> ExecuteAsync(StoreAuth0AccessTokenRequest command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var keyValueEntryRepository = await unitOfWork.GetRepositoryAsync<IKeyValueEntryRepository>(ct);

        Auth0AccessTokenJsonDto token = Auth0AccessTokenJsonDto.FromToken(command.Token);

        Result<IKeyValueEntry> storeResult = await keyValueEntryRepository.TryGetByKeyAsync("Auth0AccessToken", ct);
        IKeyValueEntry store = storeResult.TryGetAsSuccess(out IKeyValueEntry? foundStore)
            ? foundStore
            : new KeyValueEntry { Key = "Auth0AccessToken" };

        if (!KeyValueEntry.CanSetObjectAsValueJson(token) || !store.TrySetObjectAsJsonValue(token)) return MessageResponse<bool>.FromErrorString("Cannot store auth0 access token. Json conversion failed.");

        store.Value = encryptionService.Encrypt(store.Value);
        if (!(await validator.ValidateAsync(store, ct)).IsValid) return MessageResponse<bool>.FromErrorString("Cannot store auth0 access token. Validation failed.");

        Result result = await keyValueEntryRepository.TryAddOrUpdateAsync(store, ct);
        if (!result.TryGetState(out bool state)) return result.AsError;

        return state;
    }
}
