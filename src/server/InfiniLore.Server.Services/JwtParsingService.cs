// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace InfiniLore.Server.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[RegisterService<IJwtParsingService>(LifeTime.Scoped)]
public class JwtParsingService(IHttpContextAccessor contextAccessor, ILogger  logger) : IJwtParsingService {
    // ReSharper disable once RedundantDefaultMemberInitializer
    private bool? _gotValidToken = null;
    private JwtSecurityToken? _jwt;
    public JwtSecurityToken? Jwt {
        get {
            switch (_gotValidToken) {
                case true: return _jwt;
                case false: return null;

                default: {
                    _gotValidToken = TryParseJwt(out JwtSecurityToken? jwt);
                    return _jwt = jwt;
                }
            }
        }
        private set => _jwt = value;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private bool TryGetPayloadData<T>(string key, [NotNullWhen(true)] out T? value) {
        value = default;
        
        if (Jwt is null) return false;
        if (!Jwt.Payload.TryGetValue(key, out object? objectValue)) return false;

        switch (objectValue) {
            case T objectString when typeof(T) == typeof(string): {
                value = objectString;
                return true;
            }
            case JsonElement permissionsJson: {
                try {
                    value = JsonSerializer.Deserialize<T>(permissionsJson.GetRawText());
                    return value is not null;
                }
                catch (Exception ex) {
                    logger.Error(ex, "Error parsing permissions");
                    return false;
                }
            }
            default:
                return false;
        }
    }
    
    public bool TryParseJwt([NotNullWhen(true)] out JwtSecurityToken? jwt) {
        jwt = null;

        if (contextAccessor.HttpContext is not { } httpContext ) return false;
        if (!httpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationHeader) ) return false;

        var handler = new JwtSecurityTokenHandler();
        
        try {
            Jwt = jwt = handler.ReadJwtToken(authorizationHeader.ToString().Replace("Bearer ", string.Empty));
            return jwt is not null;
        }
        catch (Exception ex) {
            logger.Error(ex, "Error parsing JWT");
            return false;
        }

    }

    public bool TryGetPermissions([NotNullWhen(true)] out string[]? permissions) =>
        TryGetPayloadData("permissions", out permissions);
}















