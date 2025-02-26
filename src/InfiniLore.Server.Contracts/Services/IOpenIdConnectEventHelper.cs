// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace InfiniLore.Server.Contracts.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IOpenIdConnectEventHelper<in TContext> where TContext : BaseContext<OpenIdConnectOptions> {
    ValueTask HandleAsync(TContext context);
}
