// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;

namespace InfiniLore.Server.Modules.Core.Messaging;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record MessageRequest : CommonRequestData, ICommand<MessageResponse>;

// ReSharper disable once UnusedTypeParameter
public abstract record MessageRequest<TResponse> : CommonRequestData, ICommand<MessageResponse<TResponse>>;
