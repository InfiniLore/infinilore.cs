// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using InfiniLore.Server.Contracts.Services.Auth0;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Services.CQRS.Queries.Data.User;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IValidator<GetLorescopeByIdQuery>>(ServiceLifetime.Scoped)]
public class GetLorescopeByIdValidator : AbstractValidator<GetLorescopeByIdQuery> {
    private readonly IJwtTokenHelper _tokenHelper;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    public GetLorescopeByIdValidator(IServiceProvider provider) {
        _tokenHelper = provider.GetRequiredService<IJwtTokenHelper>();
        
        RuleFor(x => x.LorescopeId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
        
        // Check if the user has access to this lorescope
        RuleFor(x => x)
            .CustomAsync(VerifyAccessAsync);
    }
    private Task VerifyAccessAsync(GetLorescopeByIdQuery query, ValidationContext<GetLorescopeByIdQuery> context, CancellationToken ct) {
        // TODO create a query handler that checks if the user in query.AccessData.UserId has access to the specific lorescope it is requesting
        return Task.CompletedTask;
    }
}
