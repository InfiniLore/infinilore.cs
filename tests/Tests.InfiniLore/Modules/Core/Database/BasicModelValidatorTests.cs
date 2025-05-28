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
public class BasicModelValidatorTests {
    [ClassDataSource<ServiceProviderDataSource>(Shared = SharedType.PerTestSession)]
    public required ServiceProviderDataSource ServiceProvider { get; init; }

    private IValidator<BasicModel> Validator => ServiceProvider.GetRequiredService<IValidator<BasicModel>>();

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
