// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using FluentValidation;
using InfiniLore.Server.Contracts.Database.Repositories.Data.System;
using InfiniLore.Server.Contracts.Services.Auth0;
using InfiniLore.Server.Database.Models.Data.System;
using JetBrains.Annotations;

namespace InfiniLore.Server.Services.Messaging.Commands.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class StoreAuth0AccessTokenHandler(IUnitOfWorkFactory unitOfWorkFactory, IAuth0AccessTokenEncryptionService encryptionService, IValidator<KeyValueEntry> validator) : CommandHandler<StoreAuth0AccessTokenRequest, MessageResponse<bool>> {
    public override async Task<MessageResponse<bool>> ExecuteAsync(StoreAuth0AccessTokenRequest command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var keyValueEntryRepository = await unitOfWork.GetRepositoryAsync<IKeyValueEntryRepository>(ct);

        Auth0AccessTokenJsonDto token = Auth0AccessTokenJsonDto.FromToken(command.Token);

        Result<KeyValueEntry> storeResult = await keyValueEntryRepository.TryGetByKeyAsync("Auth0AccessToken", ct);
        KeyValueEntry store = storeResult.TryGetAsSuccess(out KeyValueEntry? foundStore)
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
