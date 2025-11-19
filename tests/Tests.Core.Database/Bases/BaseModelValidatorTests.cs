// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;
using FluentValidation.Results;
using Tests.Core.Database.TestData;

namespace Tests.Core.Database.Bases;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiDataSource]
public class BaseModelValidatorTests(IValidator<SimpleModel> validator) {

    public static IEnumerable<Func<(SimpleModel, bool)>> TestCases() {
        yield return () => (new SimpleModel(), true);
        yield return () => (new SimpleModel { Id = Guid.Empty }, false);
    }

    [Test]
    [MethodDataSource(nameof(TestCases))]
    public async Task Validate_ShouldReturnExpected(SimpleModel model, bool expected) {
        // Arrange
        
        // Act
        ValidationResult? result = await validator.ValidateAsync(model);

        // Assert
        await Assert.That(result.IsValid).IsEqualTo(expected);
    }
}
