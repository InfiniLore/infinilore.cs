// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Messaging;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record MessageRequest : CommonRequestData, ICommand<Outcome>;

public abstract record MessageRequest<TResponse> : CommonRequestData, ICommand<Outcome<TResponse>>;

public abstract record PaginatedMessageRequest<TResponse> : CommonRequestData, ICommand<PaginatedOutcome<TResponse>> 
    where TResponse : class;
