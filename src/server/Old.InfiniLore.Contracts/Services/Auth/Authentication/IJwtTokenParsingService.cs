// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Server.Types;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;

namespace Old.InfiniLore.Contracts.Services.Auth.Authentication;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtTokenParsingService {
    JwtSecurityToken? Jwt { get; }
    bool TryParseJwtFromContext([NotNullWhen(true)] out JwtSecurityToken? jwt);
    string[] GetPermissions();
    string[] GetRoles();
    bool TryGetUserId(out Guid userId);

    bool TryGetAsAuthRequestData(out AuthRequestData data);
}
