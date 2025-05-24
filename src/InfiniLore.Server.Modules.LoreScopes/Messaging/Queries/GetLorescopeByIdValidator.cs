// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IValidator<GetLorescopeByIdQuery>>]
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
