// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace InfiniLore.Server.Modules.Core.Messaging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record MessageAccess(
    [JsonProperty("user_id")] Guid UserId,
    [JsonProperty("roles")] ImmutableArray<string> Roles,
    [JsonProperty("permissions")] ImmutableArray<string> Permissions
) : IMessageAccess {
    public static readonly IMessageAccess Empty = new MessageAccess(Guid.Empty, [], []);
}
