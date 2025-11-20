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
public class BaseOwnedModelValidatorTests(IValidator<SimpleOwnedModel> validator) {

    public static IEnumerable<Func<(SimpleOwnedModel, bool)>> TestCases() {
        yield return () => (new SimpleOwnedModel { OwnerId = Guid.CreateVersion7() }, true);
        yield return () => (new SimpleOwnedModel { Id = Guid.Empty, OwnerId = Guid.CreateVersion7() }, false);
        yield return () => (new SimpleOwnedModel { Id = Guid.Empty, OwnerId = Guid.Empty }, false);
    }

    [Test]
    [MethodDataSource(nameof(TestCases))]
    public async Task Validate_ShouldReturnExpected(SimpleOwnedModel model, bool expected) {
        // Arrange

        // Act
        ValidationResult? result = await validator.ValidateAsync(model);

        // Assert
        await Assert.That(result.IsValid).IsEqualTo(expected);
    }
}
