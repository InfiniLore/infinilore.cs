// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace InfiniLore.Server.Services.OpenIdConnect;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// This might look a little bit janky, but it solves some overhead when it comes to DI
public static class OpenIdConnectEventHelper {
    [DebuggerStepThrough]
    public static Func<TContext, Task> HandleWith<TContext>()
        where TContext : BaseContext<OpenIdConnectOptions>
        => static context => {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var handler = serviceProvider.GetRequiredService<IOpenIdConnectEventHelper<TContext>>();
            return handler.HandleAsync(context).AsTask();
        };
}
