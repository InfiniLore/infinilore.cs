// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.CQRS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RequestExceptionHandler<TRequest, TException, T>(
    ILogger logger
) : IRequestExceptionHandler<TRequest, MediatorResponse<T>, TException> 
    where TRequest : notnull 
    where TException : Exception
{
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<MediatorResponse<T>> state, CancellationToken cancellationToken) {
        logger.Error(exception, $"Exception caught in request handler for request type {typeof(TRequest).Name}: {exception.Message}");
        state.SetHandled(MediatorResponse<T>.FromFailureString(exception.Message));
        return Task.CompletedTask;
    }
}
