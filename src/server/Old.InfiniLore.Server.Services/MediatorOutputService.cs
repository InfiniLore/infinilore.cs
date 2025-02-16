// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using AterraEngine.Unions;
using FastEndpoints;
using Old.InfiniLore.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;

namespace Old.InfiniLore.Server.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IMediatorOutputService>(ServiceLifetime.Singleton)]
public class MediatorOutputService : IMediatorOutputService {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public Results<Ok<TResponse>, BadRequest<ProblemDetails>> ToHttpResults<TResponse, TModel>(
        SuccessOrFailure<TModel> successOrFailure,
        Func<TModel, TResponse> mapper
    ) {
        if (!successOrFailure.TryGetAsFailureValue(out string? failure)) return TypedResults.Ok(mapper(successOrFailure.AsSuccess.Value));

        return TypedResults.BadRequest(new ProblemDetails {
            Detail = failure
        });

    }

    public Results<Ok<TResponse>, BadRequest<ProblemDetails>> ToBadRequest<TResponse>(string? failure = null) =>
        TypedResults.BadRequest(new ProblemDetails {
            Detail = failure
        });
}
