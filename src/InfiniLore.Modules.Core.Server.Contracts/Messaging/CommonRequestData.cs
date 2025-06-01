// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Newtonsoft.Json;

namespace InfiniLore.Modules.Core.Server.Messaging;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record CommonRequestData : ICommonRequestData {
    [JsonProperty("access_data")] public required IAccessingUser AccessingUser { get; init; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
