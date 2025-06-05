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
        return storeOutcome.Match<Outcome<IAuth0AccessToken>>(
            model => {
                if (model.Value.IsNullOrEmpty()) {
                    logger.Warning("Auth0 access token value is empty.");
                    return Outcome<IAuth0AccessToken>.FromError("Cannot get auth0 access token. Value is empty.");
                }

                model.Value = encryptionService.Decrypt(model.Value);
                if (!model.TryGetConvertJsonValueToObject(out Auth0AccessTokenJsonDto? dto)) {
                    logger.Error("Failed to convert Auth0 access token JSON to object.");
                    return Outcome<IAuth0AccessToken>.FromError("Cannot get auth0 access token. Json conversion failed.");
                }
                
                logger.Information("Successfully retrieved and parsed Auth0 access token.");
                return Outcome<IAuth0AccessToken>.FromData(dto);
            },
            error => {
                logger.Warning("Failed to retrieve Auth0 access token. {reason}", error.Value);
                return Outcome<IAuth0AccessToken>.FromError($"Cannot get auth0 access token. {error.Value}");
            }
        );
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetAuth0AccessTokenQuery command, CancellationToken ct = default) 
        => protectionRules.IsServerAsync(command.AccessingUser, ct);
}
