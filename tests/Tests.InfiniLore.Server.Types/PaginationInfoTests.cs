// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Types;
using JetBrains.Annotations;

namespace Tests.InfiniLore.Server.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(PaginationInfo))]
public class PaginationInfoTests {
    [Test]
    [Arguments(1, 10, true, null)]
    [Arguments(0, 10, false, PaginationInfo.PageNumberErrorMessage)]
    [Arguments(1, 0, false, PaginationInfo.PageSizeErrorMessage)]
    [Arguments(-1, 10, false, PaginationInfo.PageNumberErrorMessage)]
    [Arguments(1, -10, false, PaginationInfo.PageSizeErrorMessage)]
    public async Task IsValid_ReturnsExpectedResult(int pageNumber, int pageSize, bool expectedIsValid, string? expectedErrorMessage) {
        // Arrange
        var paginationInfo = new PaginationInfo(pageNumber, pageSize);
        
        // Act
        bool isValid = paginationInfo.IsValid(out Failure<string> error);

        // Assert
        await Assert.That(isValid).IsEqualTo(expectedIsValid);
        await Assert.That(error.Value).IsEqualTo(expectedErrorMessage);
    }

    [Test]
    [Arguments(0, 10, PaginationInfo.PageNumberErrorMessage)]
    [Arguments(1, 0, PaginationInfo.PageSizeErrorMessage)]
    public async Task IsNotValid_ReturnsInvalid(int pageNumber, int pageSize, string expectedErrorMessage) {
        // Arrange
        var paginationInfo = new PaginationInfo(pageNumber, pageSize);

        // Act
        bool isNotValid = paginationInfo.IsNotValid(out Failure<string> error);

        // Assert
        await Assert.That(isNotValid).IsTrue();
        await Assert.That(error.Value).IsEqualTo(expectedErrorMessage);
    }
    
    [Test]
    [Arguments(1, 10)]
    public async Task IsNotValid_ReturnsValid(int pageNumber, int pageSize) {
        // Arrange
        var paginationInfo = new PaginationInfo(pageNumber, pageSize);

        // Act
        bool isNotValid = paginationInfo.IsNotValid(out Failure<string> _);

        // Assert
        await Assert.That(isNotValid).IsFalse();
    }

    [Test]
    [Arguments(1, 10, 0)]
    [Arguments(2, 10, 10)]
    [Arguments(3, 10, 20)]
    [Arguments(1, 1, 0)]
    [Arguments(2, 1, 1)]
    public async Task SkipAmount_ReturnsExpectedResult(int pageNumber, int pageSize, int expectedSkipAmount) {
        // Arrange
        var paginationInfo = new PaginationInfo(pageNumber, pageSize);

        // Act
        int skipAmount = paginationInfo.SkipAmount;

        // Assert
        await Assert.That(skipAmount).IsEqualTo(expectedSkipAmount);
    }
}
