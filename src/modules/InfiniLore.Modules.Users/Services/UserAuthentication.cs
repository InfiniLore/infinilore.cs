// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Core.Database;
using InfiniLore.Modules.Users.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfiniLore.Modules.Users;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IUserAuthentication>]
public class UserAuthentication(IHttpContextAccessor httpContextAccessor, IReadonlyUnitOfWorkFactory<InfiniLoreDb> uowFactory) : IUserAuthentication {
    public async Task<bool> SignInAsync(Guid userId) {
        await using  IReadonlyUnitOfWork<InfiniLoreDb> uow = uowFactory.Create();
        var repo = await uow.GetRepositoryAsync<UserModelRepository>();
        UserModel? user = await repo.GetByIdAsync(userId);
        if (user == null) {
            return false;
        }
        
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