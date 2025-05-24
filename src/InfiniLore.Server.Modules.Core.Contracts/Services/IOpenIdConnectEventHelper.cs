// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace InfiniLore.Server.Modules.Core;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IOpenIdConnectEventHelper<in TContext> where TContext : BaseContext<OpenIdConnectOptions> {
    Task HandleAsync(TContext context);
}
