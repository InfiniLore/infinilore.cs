// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;

namespace InfiniLore.Server.Contracts.Services.Auth.Authorization;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserContentAuthorizationService {
    ValueTask<bool> InDevelopmentAsync();

    ValueTask<bool> ValidateHttpContextAsync<T>(T model, AccessKind accessKind, CancellationToken ct = default) where T : UserContent;
    ValueTask<bool> ValidateHttpContextAsync(Guid contentId, AccessKind accessKind, CancellationToken ct = default);
    ValueTask<bool> ValidateHttpContextIsOwnerAsync(Guid ownerId, CancellationToken ct = default);

    ValueTask<bool> HttpContextHasAccessRead(Guid contentId, CancellationToken ct = default);
    ValueTask<bool> HttpContextHasAccessWrite(Guid contentId, CancellationToken ct = default);
    ValueTask<bool> HttpContextHasAccessDelete(Guid contentId, CancellationToken ct = default);
}
