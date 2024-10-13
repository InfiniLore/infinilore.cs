// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data.Models.Account;
using InfiniLoreLib.Results;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace InfiniLore.Server.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[RegisterService<IApiSignInService>(LifeTime.Scoped)]
public class ApiSignInService(SignInManager<InfiniLoreUser> signInManager, ILogger logger) : IApiSignInService {
    public async Task<IdentityUserResult<InfiniLoreUser>> SignInAsync(string username, string password, CancellationToken ct = default) {
        if (await signInManager.UserManager.FindByNameAsync(username) is not {} user) {
            logger.Warning("User {@Username} not found", username);
            return IdentityUserResult<InfiniLoreUser>.Failure("Invalid username");
        }

        if (!await signInManager.CanSignInAsync(user)) {
            logger.Warning("User {@Username} cannot sign in", username);
            return IdentityUserResult<InfiniLoreUser>.Failure("Unable to sign in.");
        }

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, password, false);
        switch (signInResult) {
            case { Succeeded: false, IsLockedOut: false }:
                logger.Warning("Invalid password for user {@Username}", username);
                return IdentityUserResult<InfiniLoreUser>.Failure("Invalid username or password.");
            case { Succeeded: false, IsLockedOut: true }:
                logger.Warning("User {@Username} is locked out", username);
                return IdentityUserResult<InfiniLoreUser>.Failure("User is locked out.");
        }

        logger.Information("User {@Username} signed in", username);
        return IdentityUserResult<InfiniLoreUser>.Success(user);
    }

}
