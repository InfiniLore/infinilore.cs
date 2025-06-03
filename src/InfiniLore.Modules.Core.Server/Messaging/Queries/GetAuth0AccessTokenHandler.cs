// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Credentials.Auth0;
using InfiniLore.Modules.Core.Server.Auth;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
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

    protected override Result<IAuth0AccessToken> AccessDeniedResult => throw new NotImplementedException();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<Result<IAuth0AccessToken>> HandleCommandAsync(GetAuth0AccessTokenQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var keyValueEntryRepository = await unitOfWork.GetRepositoryAsync<IKeyValueEntryRepository>(ct);

        AterraEngine.Unions.Result<KeyValueEntryModel> storeResult = await keyValueEntryRepository.TryGetByKeyAsync("Auth0AccessToken", ct);
        if (storeResult.IsError) {
            logger.Warning("Failed to retrieve Auth0 access token. Key not found.");
            return Result<IAuth0AccessToken>.FromError("Cannot get auth0 access token. Key not found.");
        }

        KeyValueEntryModel store = storeResult.AsSuccess;
        if (store.Value.IsNullOrEmpty()) {
            logger.Warning("Auth0 access token value is empty.");
            return Result<IAuth0AccessToken>.FromError("Cannot get auth0 access token. Value is empty.");
        }

        store.Value = encryptionService.Decrypt(store.Value);

        if (!store.TryGetConvertJsonValueToObject(out Auth0AccessTokenJsonDto? dto)) {
            logger.Error("Failed to convert Auth0 access token JSON to object.");
            return Result<IAuth0AccessToken>.FromError("Cannot get auth0 access token. Json conversion failed.");
        }

        logger.Information("Successfully retrieved and parsed Auth0 access token.");
        return dto;
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetAuth0AccessTokenQuery command, CancellationToken ct = default) 
        => protectionRules.IsServerAsync(command.AccessingUser, ct);
}
