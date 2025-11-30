// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Core.Database;
using InfiniLore.Modules.Users.Database;
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
            || path.StartsWithSegments("/api");

        if (isSignup || isFramework) {
            await next();
            return;
        }

        await using AsyncServiceScope scope = context.RequestServices.CreateAsyncScope();
        var onboarding = scope.ServiceProvider.GetRequiredService<OnboardingService>();
        if (!await onboarding.IsOnboardingRequiredAsync(context.RequestAborted)) {
            await next();
            return;
        }

        context.Response.Redirect("/signup");
    }

    private async Task<bool> IsOnboardingRequiredAsync(CancellationToken ct = default) {
        await using IReadonlyUnitOfWork<InfiniLoreDb> uow = readonlyUnitOfWorkFactory.Create();
        var repo = await uow.GetRepositoryAsync<UserModelRepository>(ct);
        bool any = await repo.AnyAsync(ct: ct);
        return !any;
    }
}
