// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Credentials.Auth0;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Users.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.Users.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetAuth0AccessTokenHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetAuth0AccessTokenHandler> logger, IAuth0AccessTokenEncryptionService encryptionService) : CommandHandler<GetAuth0AccessTokenQuery, MessageResponse<IAuth0AccessToken>> {
    public override async Task<MessageResponse<IAuth0AccessToken>> ExecuteAsync(GetAuth0AccessTokenQuery command, CancellationToken ct = new()) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var keyValueEntryRepository = await unitOfWork.GetRepositoryAsync<IKeyValueEntryRepository>(ct);

        Result<KeyValueEntryModel> storeResult = await keyValueEntryRepository.TryGetByKeyAsync("Auth0AccessToken", ct);
        if (storeResult.IsError) {
            logger.Warning("Failed to retrieve Auth0 access token. Key not found.");
            return MessageResponse<IAuth0AccessToken>.FromErrorString("Cannot get auth0 access token. Key not found.");
        }

        KeyValueEntryModel store = storeResult.AsSuccess;
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
