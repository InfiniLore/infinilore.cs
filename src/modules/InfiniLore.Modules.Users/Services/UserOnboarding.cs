// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Core.Database;
using InfiniLore.Core.Pagination;
using InfiniLore.Modules.Users.Database;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfiniLore.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<UserOnboarding>]
public class UserOnboarding(IReadonlyUnitOfWorkFactory<InfiniLoreDb> readonlyUnitOfWorkFactory ) {

    public async Task<bool> IsOnboardingRequiredAsync(CancellationToken ct = default) {
        await using IReadonlyUnitOfWork<InfiniLoreDb> uow = readonlyUnitOfWorkFactory.Create();
        var repo = await uow.GetRepositoryAsync<UserModelRepository>(ct);
        bool any = await repo.AnyAsync(ct: ct);
        return !any;
    }

    public async Task<bool> IsCorrectUserSignedIn(ClaimsPrincipal user, CancellationToken ct = default) {
        if (user.Identity?.IsAuthenticated == false) return false;
        
        Claim? knownIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (knownIdClaim is null) return false;

        if(!Guid.TryParse(knownIdClaim.Value, out Guid knownId)) return false;
        
        await using IReadonlyUnitOfWork<InfiniLoreDb> uow = readonlyUnitOfWorkFactory.Create();
        var repo = await uow.GetRepositoryAsync<UserModelRepository>(ct);
        
        UserModel? knownUser = await repo.GetByIdAsync(knownId, ct:ct); 
        return knownUser is not null;

    }

    public async Task<bool> TryAutoSignInSingleUserAsync(IUserAuthentication authService, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork<InfiniLoreDb> uow = readonlyUnitOfWorkFactory.Create();
        var repo = await uow.GetRepositoryAsync<UserModelRepository>(ct);

        // This only works if there's exactly one user
        PaginatedData<UserModel> users = await repo.GetAllAsync(PaginationData.Default, ct: ct);

        // Only auto sign-in if there's exactly one user
        if (users.TotalCount != 1) {
            return false;
        }

        UserModel user = users.Items.First();
        return await authService.SignInAsync(user.Id);
    }
    
}
