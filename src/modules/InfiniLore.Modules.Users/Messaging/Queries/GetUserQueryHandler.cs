// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Messaging;
using InfiniLore.Core.Outcomes;
using InfiniLore.Modules.Users.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Users.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetUserQueryHandler(IServiceScopeFactory serviceScopeFactory) : BaseQueryHandler<GetUserQuery, UserModel>(serviceScopeFactory) {

    protected override async Task<Outcome<UserModel>> ExecuteAsync(IServiceProvider provider, GetUserQuery query, CancellationToken ct) {
        var readonlyUnitOfWorkFactory = provider.GetRequiredService<IReadonlyUnitOfWorkFactory<InfiniLoreDb>>();
        var logger = provider.GetRequiredService<ILogger<GetUserQueryHandler>>();

        await using IReadonlyUnitOfWork<InfiniLoreDb> uow = readonlyUnitOfWorkFactory.Create();

        try {
            var repo = await uow.GetRepositoryAsync<UserModelRepository>(ct);

            if (query.UserId != Guid.Empty) {
                UserModel? userModel = await repo.GetByIdAsync(query.UserId, ct: ct);

                // ReSharper disable once InvertIf
                if (userModel is null) {
                    logger.Warning("Failed to get user by id {id}", query.UserId);
                    return ErrorOutcome.Failure;
                }
                return userModel;
            }

            if (query.Username.IsNotNullOrWhiteSpace()) {
                UserModel? userModel = await repo.GetByUserNameAsync(query.Username, ct: ct);

                // ReSharper disable once InvertIf
                if (userModel is null) {
                    logger.Warning("Failed to get user by username {username}", query.Username);
                    return ErrorOutcome.Failure;
                }
                return userModel;
            }

            logger.Warning("No user id or username provided.");
            return ErrorOutcome.Failure;
        }
        catch (Exception ex) {
            logger.Error(ex, "Failed to execute GetUserQuery");
            return ErrorOutcome.Failure;
        }
    }
}
