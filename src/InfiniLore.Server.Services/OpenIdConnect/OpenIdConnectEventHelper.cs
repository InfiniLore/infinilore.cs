// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services.OpenIdConnectEventHelper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace InfiniLore.Server.Services.OpenIdConnect;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class OpenIdConnectEventHelper {

    [DebuggerStepThrough]
    public static Func<TContext, Task> HandleWith<TService, TContext>()
            where TContext : BaseContext<OpenIdConnectOptions> 
            where TService : IOpenIdConnectEventHelper<TContext> 
        => static context => {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var handler = serviceProvider.GetRequiredService<TService>();
            return handler.HandleAsync(context);
        };
}
