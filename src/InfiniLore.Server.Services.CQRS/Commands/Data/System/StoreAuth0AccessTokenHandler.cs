// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Data.System;
using InfiniLore.Server.Contracts.Services.Auth0;
using InfiniLore.Server.Database.Models.Data.System;
using MediatR;

namespace InfiniLore.Server.Services.CQRS.Commands.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class StoreAuth0AccessTokenHandler(IUnitOfWorkFactory unitOfWorkFactory, IAuth0AccessTokenEncryptionService encryptionService, IValidator<KeyValueStore> validator) : IRequestHandler<StoreAuth0AccessTokenRequest, MediatorResponse<bool>> {

    public async Task<MediatorResponse<bool>> Handle(StoreAuth0AccessTokenRequest request, CancellationToken ct) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var keyValueStoreRepository = await unitOfWork.GetRepositoryAsync<IKeyValueStoreRepository>(ct);

        Auth0AccessTokenJsonDto token = Auth0AccessTokenJsonDto.FromToken(request.Token);
        
        RepoResult<KeyValueStore> storeResult = await keyValueStoreRepository.TryGetByKeyAsync("Auth0AccessToken", ct);
        KeyValueStore store = storeResult.TryGetAsSuccess(out KeyValueStore? foundStore) 
            ? foundStore 
            : new KeyValueStore { Key = "Auth0AccessToken" };
        
        if (!store.CanSetObjectAsValueJson(token) || !store.TrySetbOjectAsJsonValue(token)) return MediatorResponse<bool>.FromFailureString("Cannot store auth0 access token. Json conversion failed.");
        
        store.Value = encryptionService.Encrypt(store.Value);
        if (!(await validator.ValidateAsync(store, ct)).IsValid) return MediatorResponse<bool>.FromFailureString("Cannot store auth0 access token. Validation failed.");
        
        
        RepoResult result = await keyValueStoreRepository.TryAddOrUpdateAsync(store, ct);
        return result.IsSuccess;
    }
}
