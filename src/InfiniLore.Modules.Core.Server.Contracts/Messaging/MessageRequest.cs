// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;

namespace InfiniLore.Modules.Core.Server.Messaging;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record MessageRequest : CommonRequestData, ICommand<Result>;

// ReSharper disable once UnusedTypeParameter
public abstract record MessageRequest<TResponse> : CommonRequestData, ICommand<Result<TResponse>>;
