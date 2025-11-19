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
public sealed record SimpleModel : BaseModel;

public sealed class SimpleModelConfiguration : BaseModelConfiguration<SimpleModel>;

[InjectableScoped<IValidator<SimpleModel>>]
public sealed class SimpleModelValidator : BaseModelValidator<SimpleModel>;