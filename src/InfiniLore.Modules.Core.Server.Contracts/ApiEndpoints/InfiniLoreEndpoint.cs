// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class InfiniLoreEndpoint<TRequest, TResponse, TMapper> : Endpoint<TRequest, Results<
    Ok<TResponse>,

    // Default Included Results
    NotFound,
    UnauthorizedHttpResult,
    BadRequest,
    ForbidHttpResult,
    ProblemDetails
>, TMapper>
    where TRequest : notnull
    where TResponse : notnull
    where TMapper : class, IMapper;

public abstract class InfiniLoreEndpointWithoutMapper<TRequest, TResponse> : Endpoint<TRequest, Results<
    Ok<TResponse>,
    
    // Default Included Results
    NotFound,
    UnauthorizedHttpResult,
    BadRequest,
    ForbidHttpResult,
    ProblemDetails
>>
    where TRequest : notnull
    where TResponse : notnull;

public abstract class InfiniLoreEndpointWithEmptyResponse<TRequest, TMapper> : Endpoint<TRequest, Results<
    Ok,
    
    // Default Included Results
    NotFound,
    UnauthorizedHttpResult,
    BadRequest,
    ForbidHttpResult,
    ProblemDetails
>>
    where TRequest : notnull
    where TMapper : class, IMapper;

public abstract class InfiniLoreEndpointWithEmptyResponse<TRequest> : Endpoint<TRequest, Results<
    Ok,
    
    // Default Included Results
    NotFound,
    UnauthorizedHttpResult,
    BadRequest,
    ForbidHttpResult,
    ProblemDetails
>>
    where TRequest : notnull;