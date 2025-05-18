// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Users.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace InfiniLore.Server.Modules.Core;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// This might look a little bit janky, but it solves some overhead when it comes to DI
public static class OpenIdConnectEventHelper {
    public static readonly Func<TokenValidatedContext, Task> OnTokenValidated = HandleWith<TokenValidatedContext>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    [DebuggerStepThrough]
    private static Func<TContext, Task> HandleWith<TContext>()
        where TContext : BaseContext<OpenIdConnectOptions>
        => static context => context.HttpContext.RequestServices
            .GetRequiredService<IOpenIdConnectEventHelper<TContext>>()
            .HandleAsync(context);
}
