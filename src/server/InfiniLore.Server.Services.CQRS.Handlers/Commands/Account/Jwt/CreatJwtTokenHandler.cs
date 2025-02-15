// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Contracts.Services.Auth.Authentication;
using InfiniLore.Server.Services.CQRS.Requests.Commands.Account.Jwt;
using InfiniLore.Server.Types;
using MediatR;

namespace InfiniLore.Server.Services.CQRS.Handlers.Commands.Account.Jwt;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CreatJwtTokenHandler(IJwtTokenGenerationService jwtTokenService) : IRequestHandler<CreateJwtTokenCommand, SuccessOrFailure<JwtTokenData>> {

    public async Task<SuccessOrFailure<JwtTokenData>> Handle(CreateJwtTokenCommand request, CancellationToken ct) =>
        // Todo check if user has roles
        // Todo check if user has permissions
        await jwtTokenService.GenerateTokensAsync(
            request.User,
            request.Roles,
            request.Permissions,
            request.RefreshExpiresInDays,
            ct
        );
}
