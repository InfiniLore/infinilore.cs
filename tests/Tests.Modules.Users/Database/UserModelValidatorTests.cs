// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Modules.Users.Database;

namespace Tests.Modules.Users.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiDataSource]
public class UserModelValidatorTests(IValidator<UserModel> validator) {

    public static IEnumerable<Func<(UserModel, bool)>> TestCases() {
        yield return () => (new UserModel {UserName = "AnnaSasDev"}, true);
        yield return () => (new UserModel { Id = Guid.Empty, UserName = string.Empty}, false);
        yield return () => (new UserModel { UserName = string.Empty }, false);
        yield return () => (new UserModel { UserName = "#INVALID!!!" }, false);
    }

    [Test]
    [MethodDataSource(nameof(TestCases))]
    public async Task Validate_ShouldReturnExpected(UserModel model, bool expected) {
        // Arrange
        
        // Act
        ValidationResult? result = await validator.ValidateAsync(model);

        // Assert
        await Assert.That(result.IsValid).IsEqualTo(expected);
    }
}
