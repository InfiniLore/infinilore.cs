// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using DataSources.InfiniLore.Server;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Modules.Core.Database;

namespace Tests.InfiniLore.Modules.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiDataSource]
public class BasicModelValidatorTests(IValidator<BasicModel> validator) {
    [Test]
    public async Task BasicModel_ShouldHaveValidEmptyConstructor() {
        // Arrange
        var entity = new BasicModel();
        
        // Act
        ValidationResult? result = await validator.ValidateAsync(entity);

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
        ValidationResult? result = await validator.ValidateAsync(entity);
    
        // Assert
        await Assert.That(result.IsValid).IsFalse();

    }
}
