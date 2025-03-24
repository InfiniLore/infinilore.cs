// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Contracts.Services.Cqrs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.CQRS.PipelineBehaviours;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ValidateRequestBehaviour<TRequest, TResponse>(
    ILogger<ValidateRequestBehaviour<TRequest, TResponse>> logger,
    IValidator<TRequest>? validator = null// Optional service, because not all requests have validation logic.
)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMediatorRequest 
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct) {
        // If a Validator hasn't been defined, we skip any logic all together.
        //      In the long run everything should be validated, so we add warning logs.
        if (validator is null) {
            logger.LogWarning("No validator defined for request type {RequestType}", typeof(TRequest).Name);
            return await next();
        }

        ValidationResult validationResult = await validator.ValidateAsync(request, ct);
        if (validationResult.IsValid) return await next();

        // TODO Create a system that logs failed validations to a db or something
        logger.LogWarning("Validation failed: {@Reason}", validationResult.Errors);

        // Handle non-generic MediatorResponse case.
        if (!typeof(TResponse).MatchesGenericType(typeof(MediatorResponse<>)))
            return (TResponse)(object)validationResult.ToMediatorResponse();

        // Handle validation failure by checking the expected TResponse 
        Type responseType = typeof(MediatorResponse<>).MakeGenericType(typeof(TResponse).GetGenericArguments()[0]);
        string errors = validationResult.Errors.Select(e => e.ErrorMessage).Aggregate((a, b) => a + ", " + b) ?? string.Empty;
        return (TResponse)Activator.CreateInstance(responseType, errors)!;
    }
}
