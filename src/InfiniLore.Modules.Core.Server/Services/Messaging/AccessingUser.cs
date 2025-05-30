// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace InfiniLore.Modules.Core.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record AccessingUser(
    [JsonProperty("user_id")] Guid UserId,
    [JsonProperty("roles")] ImmutableArray<string> Roles,
    [JsonProperty("permissions")] ImmutableArray<string> Permissions
) : IAccessingUser {
    [JsonProperty("metadata")] public FrozenDictionary<string, object> MetaData { get; init; } = FrozenDictionary<string, object>.Empty;
    
    public static AccessingUser Empty { get; } = new(Guid.Empty, ImmutableArray<string>.Empty, ImmutableArray<string>.Empty);
}
