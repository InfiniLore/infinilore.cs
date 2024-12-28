// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Types;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;

namespace InfiniLore.Server.Contracts.Services.Auth.Authentication;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtTokenParsingService {
    JwtSecurityToken? Jwt { get; }
    bool TryParseJwt([NotNullWhen(true)] out JwtSecurityToken? jwt);
    bool TryGetPermissions([NotNullWhen(true)] out string[]? roles);
    bool TryGetRoles([NotNullWhen(true)] out string[]? permissions);
    bool TryGetUserId(out Guid userId);
    
    bool TryGetAsJwtTokenRequestData([NotNullWhen(true)] out JwtTokenRequestData? data);
}
