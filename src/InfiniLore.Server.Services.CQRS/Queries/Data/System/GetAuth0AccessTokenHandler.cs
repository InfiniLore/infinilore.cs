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
public class GetAuth0AccessTokenHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetAuth0AccessTokenHandler> logger, IAuth0AccessTokenEncryptionService encryptionService) : IRequestHandler<GetAuth0AccessTokenQuery, MediatorResponse<IAuth0AccessToken>> {

    public async Task<MediatorResponse<IAuth0AccessToken>> Handle(GetAuth0AccessTokenQuery request, CancellationToken ct) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var keyValueStoreRepository = await unitOfWork.GetRepositoryAsync<IKeyValueStoreRepository>(ct);

        RepoResult<KeyValueStore> storeResult = await keyValueStoreRepository.TryGetByKeyAsync("Auth0AccessToken", ct);
        if (storeResult.IsError) {
            logger.Warning("Failed to retrieve Auth0 access token. Key not found.");
            return MediatorResponse<IAuth0AccessToken>.FromErrorString("Cannot get auth0 access token. Key not found.");
        }

        KeyValueStore store = storeResult.AsSuccess;
        if (store.Value.IsNullOrEmpty()) {
            logger.Warning("Auth0 access token value is empty.");
            return MediatorResponse<IAuth0AccessToken>.FromErrorString("Cannot get auth0 access token. Value is empty.");
        }

        store.Value = encryptionService.Decrypt(store.Value);

        if (!store.TryGetConvertJsonValueToObject(out Auth0AccessTokenJsonDto? dto)) {
            logger.Error("Failed to convert Auth0 access token JSON to object.");
            return MediatorResponse<IAuth0AccessToken>.FromErrorString("Cannot get auth0 access token. Json conversion failed.");
        }

        logger.Information("Successfully retrieved and parsed Auth0 access token.");
        return dto;
    }
}
