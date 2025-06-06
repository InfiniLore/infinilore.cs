// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Modules.Core.Server.Database;

namespace Tests.InfiniLore.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BasicModelValidatorTests {
    private static IValidator<BasicModel> Validator => new BasicModelValidator();

    // -----------------------------------------------------------------------------------------------------------------
    // Tests
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    public async Task BasicModel_ShouldHaveValidEmptyConstructor() {
        // Arrange
        var entity = new BasicModel();
        
        // Act
        ValidationResult? result = await Validator.ValidateAsync(entity);

        // Assert
        await Assert.That(result.IsValid).IsTrue();
    }

    [Test]
    public async Task BasicModel_FailsOnEmptyGuid() {
        // Arrange
        var entity = new BasicModel {
            Id = Guid.Empty
        };
        
        // Act
        ValidationResult? result = await Validator.ValidateAsync(entity);
    
        // Assert
        await Assert.That(result.IsValid).IsFalse();

    }
}
