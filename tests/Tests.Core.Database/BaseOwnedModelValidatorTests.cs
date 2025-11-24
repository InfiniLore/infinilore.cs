// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Core.Database;
using System.Diagnostics.CodeAnalysis;

namespace Tests.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("Usage", "TUnit0300:Generic type or method may not be AOT-compatible")]
[SuppressMessage("Usage", "TUnit0059:Abstract test class with data sources requires [InheritsTests]")]
public abstract class BaseOwnedModelValidatorTests<TModel, TOwner>(IValidator<TModel> validator) 
    where TModel : BaseOwnedModel<TOwner>, new() 
    where TOwner : BaseModel {

    public abstract IEnumerable<Func<(TModel, bool)>> TestCases();

    // -----------------------------------------------------------------------------------------------------------------
    // Test Methods
    // -----------------------------------------------------------------------------------------------------------------

    [Test]
    [InstanceMethodDataSource(nameof(TestCases))]
    public async Task Validate_ShouldReturnExpected(TModel model, bool expected) {
        // Arrange

        // Act
        ValidationResult? result = await validator.ValidateAsync(model);

        // Assert
        await Assert.That(result.IsValid).IsEqualTo(expected);
    }
}
