// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Unions;
using InfiniLore.Database.Models;
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Account;
using InfiniLore.Server.Contracts.Services.Auth.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Services.Authorization;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IUserContentAuthorizationService>(ServiceLifetime.Scoped)]
public class UserContentAuthorizationService(
    IUserContentAccessRepository userContentAccessRepository,
    IHttpContextAccessor contextAccessor,
    UserManager<InfiniLoreUser> userManager
) : IUserContentAuthorizationService {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValueTask<bool> InDevelopmentAsync() => new(true);

    public ValueTask<bool> ValidateAsync<T>(T model, AccessKind accessKind, CancellationToken ct = default) where T : UserContent
        => ValidateAsync(model.Id, accessKind, ct);

    public async ValueTask<bool> ValidateAsync(Guid contentId, AccessKind accessKind, CancellationToken ct = default) {
        if (!(await GetUserFromClaimsPrincipalAsync(ct)).TryGetAsSuccessValue(out InfiniLoreUser? accessorUser)) return false;

        return await userContentAccessRepository.UserHasKindAsync(contentId, accessorUser.Id, accessKind, ct);
    }

    public async ValueTask<bool> ValidateIsOwnerAsync(Guid ownerId, CancellationToken ct = default) {
        if (!(await GetUserFromClaimsPrincipalAsync(ct)).TryGetAsSuccessValue(out InfiniLoreUser? accessorUser)) return false;

        return accessorUser.Id == ownerId;
    }

    public ValueTask<bool> HasAccessRead(Guid contentId, CancellationToken ct = default) => ValidateAsync(contentId, AccessKind.Read, ct);

    public ValueTask<bool> HasAccessWrite(Guid contentId, CancellationToken ct = default) => ValidateAsync(contentId, AccessKind.Write, ct);
    public ValueTask<bool> HasAccessDelete(Guid contentId, CancellationToken ct = default) => ValidateAsync(contentId, AccessKind.Delete, ct);

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private async ValueTask<SuccessOrFailure<InfiniLoreUser, string>> GetUserFromClaimsPrincipalAsync(CancellationToken ct) {
        if (contextAccessor.HttpContext is not {} accessor) return new Failure<string>("No HttpContext found in IHttpContextAccessor");
        InfiniLoreUser? user = await userManager.GetUserAsync(accessor.User);
        if (user is null) return new Failure<string>("No user found in IHttpContextAccessor");
        
        ct.ThrowIfCancellationRequested();
        return user;
    }
}
