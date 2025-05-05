// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;

namespace InfiniLore.Server.Modules.Core.Messaging;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record MessageRequest : CommonRequestData, ICommand<MessageResponse>;

// ReSharper disable once UnusedTypeParameter
public record MessageRequest<TResponse> : CommonRequestData, ICommand<MessageResponse<TResponse>>;
