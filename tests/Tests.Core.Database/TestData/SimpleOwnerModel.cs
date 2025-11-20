// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using InfiniLore.Core.Database;

namespace Tests.Core.Database.TestData;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed record SimpleOwnerModel : BaseModel;

public sealed class SimpleOwnerModelConfiguration : BaseModelConfiguration<SimpleOwnerModel>;

[InjectableScoped<IValidator<SimpleOwnerModel>>]
public sealed class SimpleOwnerModelValidator : BaseModelValidator<SimpleOwnerModel>;