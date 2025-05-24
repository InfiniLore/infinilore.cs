// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.ApiEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Test.InfiniLore.Server.Modules.Core.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ApiPolicyTests {
    [Test]
    public async Task AddJwtProtectedPolicy_AddsCorrectPolicy() {
        // Arrange
        var services = new ServiceCollection();
        services.AddAuthorizationBuilder().AddJwtProtectedPolicy();
        ServiceProvider provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IAuthorizationPolicyProvider>();
        
        // Act
        AuthorizationPolicy? policy = await options.GetPolicyAsync(ApiPolicies.JwtProtected);

        // Assert
        await Assert.That(policy).IsNotNull();
        await Assert.That(policy?.AuthenticationSchemes).ContainsOnly(scheme => scheme == JwtBearerDefaults.AuthenticationScheme);
        await Assert.That(policy?.Requirements).ContainsOnly(requirement => requirement is DenyAnonymousAuthorizationRequirement);
    }
}
