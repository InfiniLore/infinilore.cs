// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Core.Database;
using InfiniLore.Core.Pagination;
using InfiniLore.Modules.Users.Database;
using InfiniLore.Modules.Users.Services;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Application.Services.Onboarding;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<OnboardingService>]
public class OnboardingService(IReadonlyUnitOfWorkFactory<InfiniLoreDb> readonlyUnitOfWorkFactory) {
    public static async Task Middleware(HttpContext context, Func<Task> next) {
        PathString path = context.Request.Path;

        // Allow signup and static/framework resources
        bool isSignup = path.StartsWithSegments("/signup", StringComparison.OrdinalIgnoreCase);
        bool isFramework = path.StartsWithSegments("/_framework")
            || path.StartsWithSegments("/_content")
            || path.StartsWithSegments("/css")
            || path.StartsWithSegments("/js")
            || path.StartsWithSegments("/favicon")
            || path.StartsWithSegments("/assets")
            || path.StartsWithSegments("/swagger")
            || path.StartsWithSegments("/swagger.json")
            || path.StartsWithSegments("/api")
            || path.StartsWithSegments("/_blazor");

        if (isSignup || isFramework) {
            await next();
            return;
        }

        // Check if user is already authenticated
        if (context.User.Identity?.IsAuthenticated == true) {
            await next();
            return;
        }

        await using AsyncServiceScope scope = context.RequestServices.CreateAsyncScope();
        var onboarding = scope.ServiceProvider.GetRequiredService<OnboardingService>();

        // Check if we need onboarding (no users exist)
        if (await onboarding.IsOnboardingRequiredAsync(context.RequestAborted)) {
            context.Response.Redirect("/signup");
            return;
        }

        // User exists but not authenticated - auto sign in single user
        var authService = scope.ServiceProvider.GetRequiredService<UserAuthentication>();
        bool autoSignedIn = await onboarding.TryAutoSignInSingleUserAsync(authService, context.RequestAborted);

        if (autoSignedIn) {
            // Redirect to the same page to refresh with authenticated context
            context.Response.Redirect(context.Request.Path);
            return;
        }

        await next();
    }

    private async Task<bool> IsOnboardingRequiredAsync(CancellationToken ct = default) {
        await using IReadonlyUnitOfWork<InfiniLoreDb> uow = readonlyUnitOfWorkFactory.Create();
        var repo = await uow.GetRepositoryAsync<UserModelRepository>(ct);
        bool any = await repo.AnyAsync(ct: ct);
        return !any;
    }

    private async Task<bool> TryAutoSignInSingleUserAsync(UserAuthentication authService, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork<InfiniLoreDb> uow = readonlyUnitOfWorkFactory.Create();
        var repo = await uow.GetRepositoryAsync<UserModelRepository>(ct);

        var outcome = await repo.GetAllAsync(PaginationData.Default, ct:ct);
        if (!outcome.TryGetAsSuccess(out PaginatedData<UserModel>? users)) {
            return false;
        }

        // Only auto sign-in if there's exactly one user
        if (users.TotalCount != 1) {
            return false;
        }

        UserModel user = users.Items.First();
        return await authService.SignInAsync(user);
    }
}
