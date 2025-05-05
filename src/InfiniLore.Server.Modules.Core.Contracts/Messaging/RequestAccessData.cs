// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace InfiniLore.Server.Modules.Core.Messaging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record RequestAccessData(
    [JsonProperty("user_id")] Guid UserId,
    [JsonProperty("roles")] ImmutableArray<string> Roles,
    [JsonProperty("permissions")] ImmutableArray<string> Permissions
) : IAccessData {
    public static readonly IAccessData Empty = new RequestAccessData(Guid.Empty, [], []);
}
