// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using Old.InfiniLore.Database.Models.Content.Account;
using Old.InfiniLore.Server.Services.CQRS.Requests.Commands.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Old.InfiniLore.Server.Services.CQRS.Handlers.Commands.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoginCommandHandler(SignInManager<InfiniLoreUser> signInManager) : IRequestHandler<LoginCommand, SuccessOrFailure<InfiniLoreUser>> {

    public async Task<SuccessOrFailure<InfiniLoreUser>> Handle(LoginCommand request, CancellationToken cancellationToken) {
        if (await signInManager.UserManager.FindByNameAsync(request.Username) is not {} user) return "Invalid username";
        if (!await signInManager.CanSignInAsync(user)) return "Unable to sign in.";

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        return signInResult switch {
            { Succeeded: false, IsLockedOut: false } => "Invalid username or password.",
            { Succeeded: false, IsLockedOut: true } => "User is locked out.",
            _ => user
        };
    }
}
