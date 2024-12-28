// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using InfiniLore.Server.Contracts.Services.Auth.Authentication;
using InfiniLore.Server.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace InfiniLore.Server.Services.Authentication;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IJwtTokenParsingService>(ServiceLifetime.Scoped)]
public class JwtTokenParsingService(IHttpContextAccessor contextAccessor, ILogger logger) : IJwtTokenParsingService {
    private readonly ILogger _logger = logger.ForSectionProperty("JWT parsing");
    private readonly JwtSecurityTokenHandler _handler = new();
    private bool _gotValidToken;
    private JwtSecurityToken? _jwt;
    public JwtSecurityToken? Jwt {
        get {
            if (_gotValidToken) return _jwt;
            if (!TryParseJwt(out JwtSecurityToken? jwt)) return _jwt;

            _jwt = jwt;
            _gotValidToken = true;
            return _jwt;
        }
        private set => _jwt = value;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryParseJwt([NotNullWhen(true)] out JwtSecurityToken? jwt) {
        jwt = null;

        if (contextAccessor.HttpContext is not {} httpContext) return false;
        if (!httpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationHeader)) return false;

        string token = authorizationHeader
            .ToString()
            .Replace("Bearer ", string.Empty);

        if (!_handler.CanReadToken(token)) return false;

        return (Jwt = jwt = _handler.ReadJwtToken(token)) is not null;
    }

    public bool TryGetPermissions([NotNullWhen(true)] out string[]? permissions) => TryGetPayloadData("permissions", out permissions);
    public bool TryGetRoles([NotNullWhen(true)] out string[]? permissions) => TryGetPayloadData("roles", out permissions);
    public bool TryGetUserId(out Guid userId) => TryGetPayloadData(ClaimTypes.NameIdentifier, out userId);
    
    public bool TryGetAsAuthRequestData([NotNullWhen(true)] out AuthRequestData? data) {
        data = null;
        if (Jwt is null) return false;

        if (!TryGetPermissions(out string[]? permissions)) {
            return false;
        }
        if (!TryGetRoles(out string[]? roles)) return false;
        if (!TryGetUserId(out Guid userId)) return false;

        data = new AuthRequestData(
            userId,
            permissions,
            roles
        );
        return true;
    }

    private bool TryGetPayloadData<T>(string key, [NotNullWhen(true)] out T? value) {
        value = default;

        if (Jwt is null) return false;
        if (!Jwt.Payload.TryGetValue(key, out object? objectValue)) return false;

        try {
            switch (objectValue) {
                case T objectString when typeof(T) == typeof(string): {
                    value = objectString;
                    return true;
                }

                case JsonElement permissionsJson: {
                    value = JsonSerializer.Deserialize<T>(permissionsJson.GetRawText());
                    return value is not null;
                }

                default:
                    return false;
            }
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error parsing for {Key} at {Value}", key, objectValue);
            return false;
        }
    }
}
