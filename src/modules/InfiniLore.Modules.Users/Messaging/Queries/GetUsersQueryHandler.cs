// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Messaging;
using InfiniLore.Core.Outcomes;
using InfiniLore.Core.Pagination;
using InfiniLore.Modules.Users.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Users.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetUsersQueryHandler(IServiceScopeFactory serviceScopeFactory) : BasePaginatedQueryHandler<GetUsersQuery, UserModel>(serviceScopeFactory) {

    protected override async Task<PaginatedOutcome<UserModel>> ExecuteAsync(IServiceProvider provider, GetUsersQuery query, CancellationToken ct) {
        var readonlyUnitOfWorkFactory = provider.GetRequiredService<IReadonlyUnitOfWorkFactory<InfiniLoreDb>>();
        var logger = provider.GetRequiredService<ILogger<GetUsersQueryHandler>>();

        await using IReadonlyUnitOfWork<InfiniLoreDb> uow = readonlyUnitOfWorkFactory.Create();

        try {
            var repo = await uow.GetRepositoryAsync<UserModelRepository>(ct);

            PaginatedRepoOutcome<UserModel> outcome = await repo.GetAllAsync(query.Pagination, query.Config, ct);

            // ReSharper disable once InvertIf
            if (!outcome.TryGetAsData(out PaginatedData<UserModel>? data)) {
                logger.Warning("Failed to get users: {error}", outcome.AsError);
                return PaginatedOutcome.Failure;
            }
            
            return data;

        }
        catch (Exception ex) {
            logger.Error(ex, "Failed to execute GetUsersQuery");
            return PaginatedOutcome.Failure;
        }
    }
}
