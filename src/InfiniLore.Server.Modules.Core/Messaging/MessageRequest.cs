// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using Newtonsoft.Json;

namespace InfiniLore.Server.Modules.Core.Messaging;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record CommonRequestData : ICommonRequestData {
    [JsonProperty("access_data")] public IAccessData AccessData { get; init; } = RequestAccessData.Empty;
    [JsonProperty("created_at")] public DateTime CreatedAt { get; } = DateTime.UtcNow;
}

public record MessageRequest : CommonRequestData, ICommand<MessageResponse>;

// ReSharper disable once UnusedTypeParameter
public record MessageRequest<TResponse> : CommonRequestData, ICommand<MessageResponse<TResponse>>;
