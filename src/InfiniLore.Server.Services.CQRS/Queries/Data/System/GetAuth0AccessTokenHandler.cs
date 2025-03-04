// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Credentials.Auth0;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Data.System;
using InfiniLore.Server.Contracts.Services.Auth0;
using InfiniLore.Server.Database.Models.Data.System;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.CQRS.Queries.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetAuth0AccessTokenHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetAuth0AccessTokenHandler> logger,  IAuth0AccessTokenEncryptionService encryptionService) : IRequestHandler<GetAuth0AccessTokenRequest, MediatorResponse<IAuth0AccessToken>> {
    
    public async Task<MediatorResponse<IAuth0AccessToken>> Handle(GetAuth0AccessTokenRequest request, CancellationToken ct) {
        
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var keyValueStoreRepository = await unitOfWork.GetRepositoryAsync<IKeyValueStoreRepository>(ct);
        
        RepoResult<KeyValueStore> storeResult = await keyValueStoreRepository.TryGetByKeyAsync("Auth0AccessToken", ct);
        if (storeResult.IsFailure) return MediatorResponse<IAuth0AccessToken>.FromFailureString("Cannot get auth0 access token. Key not found.");
        
        KeyValueStore store = storeResult.AsSuccess;
        if (store.Value.IsNullOrEmpty()) return MediatorResponse<IAuth0AccessToken>.FromFailureString("Cannot get auth0 access token. Value is empty.");
        store.Value = encryptionService.Decrypt(store.Value);
        
        // TODO value should be protected, so we need to decrypt it in some way.
        if (!store.TryGetConvertJsonValueToObject(out Auth0AccessTokenJsonDto? dto)) 
            return MediatorResponse<IAuth0AccessToken>.FromFailureString("Cannot get auth0 access token. Json conversion failed.");
        
        
        return dto;
    }
}
