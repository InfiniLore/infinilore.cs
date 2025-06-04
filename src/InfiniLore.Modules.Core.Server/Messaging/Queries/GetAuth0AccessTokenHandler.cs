// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Credentials.Auth0;
using InfiniLore.Modules.Core.Server.Auth;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Modules.Core.Shared;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetAuth0AccessTokenHandler(
    IReadonlyUnitOfWorkFactory factory,
    ILogger<GetAuth0AccessTokenHandler> logger,
    IAccessProtectionRules protectionRules,
    IAuth0AccessTokenEncryptionService encryptionService
) : AccessProtectedCommandHandler<GetAuth0AccessTokenQuery, IAuth0AccessToken>(logger) {

    protected override Outcome<IAuth0AccessToken> AccessDeniedOutcome => throw new NotImplementedException();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<Outcome<IAuth0AccessToken>> HandleCommandAsync(GetAuth0AccessTokenQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var keyValueEntryRepository = await unitOfWork.GetRepositoryAsync<IKeyValueEntryRepository>(ct);

        RepoOutcome<KeyValueEntryModel> storeOutcome = await keyValueEntryRepository.TryGetByKeyAsync("Auth0AccessToken", ct);
        if (storeOutcome.IsError) {
            logger.Warning("Failed to retrieve Auth0 access token. Key not found.");
            return Outcome<IAuth0AccessToken>.FromError("Cannot get auth0 access token. Key not found.");
        }

        KeyValueEntryModel store = storeOutcome.AsData;
        if (store.Value.IsNullOrEmpty()) {
            logger.Warning("Auth0 access token value is empty.");
            return Outcome<IAuth0AccessToken>.FromError("Cannot get auth0 access token. Value is empty.");
        }

        store.Value = encryptionService.Decrypt(store.Value);

        if (!store.TryGetConvertJsonValueToObject(out Auth0AccessTokenJsonDto? dto)) {
            logger.Error("Failed to convert Auth0 access token JSON to object.");
            return Outcome<IAuth0AccessToken>.FromError("Cannot get auth0 access token. Json conversion failed.");
        }

        logger.Information("Successfully retrieved and parsed Auth0 access token.");
        return dto;
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetAuth0AccessTokenQuery command, CancellationToken ct = default) 
        => protectionRules.IsServerAsync(command.AccessingUser, ct);
}
