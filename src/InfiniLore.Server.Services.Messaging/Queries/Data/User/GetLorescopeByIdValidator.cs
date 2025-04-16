// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Services.Messaging.Queries.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IValidator<GetLorescopeByIdQuery>>(ServiceLifetime.Scoped)]
public class GetLorescopeByIdValidator : AbstractValidator<GetLorescopeByIdQuery> {
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    public GetLorescopeByIdValidator() {
        RuleFor(x => x.LorescopeId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        // Check if the user has access to this lorescope
        RuleFor(x => x)
            .CustomAsync(VerifyAccessAsync);
    }
    private Task VerifyAccessAsync(GetLorescopeByIdQuery query, ValidationContext<GetLorescopeByIdQuery> context, CancellationToken ct) =>
        // TODO create a query handler that checks if the user in query.AccessData.UserId has access to the specific lorescope it is requesting
        Task.CompletedTask;
}
