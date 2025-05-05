// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Newtonsoft.Json;

namespace InfiniLore.Server.Modules.Core.Messaging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record CommonRequestData : ICommonRequestData {
    [JsonProperty("access_data")] public required IMessageAccess Access { get; init; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
