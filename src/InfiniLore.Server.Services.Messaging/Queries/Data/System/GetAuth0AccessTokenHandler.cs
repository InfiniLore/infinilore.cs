// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Credentials.Auth0;
using InfiniLore.Server.Contracts.Database.Repositories.Data.System;
using InfiniLore.Server.Contracts.Services.Auth0;
using InfiniLore.Server.Database.Models.Data.System;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.Messaging.Queries.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetAuth0AccessTokenHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetAuth0AccessTokenHandler> logger, IAuth0AccessTokenEncryptionService encryptionService) : CommandHandler<GetAuth0AccessTokenQuery, MessageResponse<IAuth0AccessToken>> {
    public override async Task<MessageResponse<IAuth0AccessToken>> ExecuteAsync(GetAuth0AccessTokenQuery command, CancellationToken ct = new CancellationToken()) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var keyValueEntryRepository = await unitOfWork.GetRepositoryAsync<IKeyValueEntryRepository>(ct);

        Result<KeyValueEntry> storeResult = await keyValueEntryRepository.TryGetByKeyAsync("Auth0AccessToken", ct);
        if (storeResult.IsError) {
            logger.Warning("Failed to retrieve Auth0 access token. Key not found.");
            return MessageResponse<IAuth0AccessToken>.FromErrorString("Cannot get auth0 access token. Key not found.");
        }

        KeyValueEntry store = storeResult.AsSuccess;
        if (store.Value.IsNullOrEmpty()) {
            logger.Warning("Auth0 access token value is empty.");
            return MessageResponse<IAuth0AccessToken>.FromErrorString("Cannot get auth0 access token. Value is empty.");
        }

        store.Value = encryptionService.Decrypt(store.Value);

        if (!store.TryGetConvertJsonValueToObject(out Auth0AccessTokenJsonDto? dto)) {
            logger.Error("Failed to convert Auth0 access token JSON to object.");
            return MessageResponse<IAuth0AccessToken>.FromErrorString("Cannot get auth0 access token. Json conversion failed.");
        }

        logger.Information("Successfully retrieved and parsed Auth0 access token.");
        return dto;
    }
}
