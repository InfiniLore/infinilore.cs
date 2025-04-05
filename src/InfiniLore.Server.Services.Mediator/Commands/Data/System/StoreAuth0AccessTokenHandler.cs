// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using InfiniLore.Server.Contracts.Database.Repositories.Data.System;
using InfiniLore.Server.Contracts.Services.Auth0;
using InfiniLore.Server.Database.Models.Data.System;

namespace InfiniLore.Server.Services.Mediator.Commands.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class StoreAuth0AccessTokenHandler {
    public static async Task<MediatorResponse<bool>> HandleAsync(
        // Message
        StoreAuth0AccessTokenMediatorRequest message,
        // Services
        IUnitOfWorkFactory unitOfWorkFactory, IAuth0AccessTokenEncryptionService encryptionService, IValidator<KeyValueStore> validator,
        // CT
        CancellationToken ct
    ) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var keyValueStoreRepository = await unitOfWork.GetRepositoryAsync<IKeyValueStoreRepository>(ct);

        Auth0AccessTokenJsonDto token = Auth0AccessTokenJsonDto.FromToken(message.Token);

        Result<KeyValueStore> storeResult = await keyValueStoreRepository.TryGetByKeyAsync("Auth0AccessToken", ct);
        KeyValueStore store = storeResult.TryGetAsSuccess(out KeyValueStore? foundStore)
            ? foundStore
            : new KeyValueStore { Key = "Auth0AccessToken" };

        if (!store.CanSetObjectAsValueJson(token) || !store.TrySetObjectAsJsonValue(token)) return MediatorResponse<bool>.FromErrorString("Cannot store auth0 access token. Json conversion failed.");

        store.Value = encryptionService.Encrypt(store.Value);
        if (!(await validator.ValidateAsync(store, ct)).IsValid) return MediatorResponse<bool>.FromErrorString("Cannot store auth0 access token. Validation failed.");


        Result result = await keyValueStoreRepository.TryAddOrUpdateAsync(store, ct);
        if (!result.TryGetState(out bool state)) return result.AsError;

        return state;
    }
}
