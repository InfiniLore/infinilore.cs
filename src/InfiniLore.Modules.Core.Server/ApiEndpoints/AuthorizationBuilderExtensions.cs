// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class AuthorizationBuilderExtensions {
    public static AuthorizationBuilder AddJwtProtectedPolicy(this AuthorizationBuilder builder) 
        => builder.AddPolicy(ApiPolicies.JwtProtected, configurePolicy: policy => {
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser(); // Enforce authentication
        });
}
