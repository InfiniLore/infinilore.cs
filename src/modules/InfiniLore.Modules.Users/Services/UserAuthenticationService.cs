// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Users.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;


namespace InfiniLore.Modules.Users.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<UserAuthentication>]
public class UserAuthentication(IHttpContextAccessor httpContextAccessor) {
    public async Task<bool> SignInAsync(UserModel user) {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) {
            return false;
        }
        
        var claims = new List<Claim> {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName)
        };
        
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
        };
        
        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );
        
        return true;
    }
    
    public async Task SignOutAsync() {
        HttpContext? httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) {
            return;
        }
        
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}