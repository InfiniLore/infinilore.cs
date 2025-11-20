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
public sealed record SimpleOwnedModel : BaseOwnedModel<SimpleOwnerModel>;

public sealed class SimpleOwnedModelConfiguration : BaseOwnedModelConfiguration<SimpleOwnedModel, SimpleOwnerModel>;

[InjectableScoped<IValidator<SimpleOwnedModel>>]
public sealed class SimpleOwnedModelValidator : BaseOwnedModelValidator<SimpleOwnedModel, SimpleOwnerModel>;