// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using Old.InfiniLore.Server.Types;
using JetBrains.Annotations;

namespace Old.Tests.InfiniLore.Server.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(RepoResult))]
[TestSubject(typeof(RepoResult<>))]
public class RepoResultTests {
    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task RepoResult_ImplicitBoolConversion_ReturnsCorrectResult(bool value) {
        // Arrange & Act
        RepoResult repoResult = value;

        // Assert
        await Assert.That(repoResult).IsEqualTo(value);
    }

    [Test]
    [Arguments("error", false)]
    public async Task RepoResult_ImplicitStringConversion_ReturnsFailure(string input, bool expected) {
        // Arrange & Act
        RepoResult repoResult = input;

        // Assert
        if (expected) await Assert.That(repoResult.IsSuccess).IsEqualTo(expected);
        if (!expected) await Assert.That(repoResult.IsFailure).IsEqualTo(!expected);
    }

    [Test]
    public async Task RepoResultOfT_ImplicitConversionFromString_Failure() {
        // Arrange
        string error = "Error occurred";

        // Act
        RepoResult<object> result = error;
        bool isSuccess = result.TryGetAsSuccess(out object? value);

        // Assert
        await Assert.That(isSuccess).IsFalse();
        await Assert.That(value).IsNull();
    }

    [Test]
    public async Task RepoResultOfT_ImplicitConversionFromT_Success() {
        // Arrange
        const int entity = 123456789;

        // Act
        RepoResult<int> result = entity;
        bool isSuccess = result.TryGetAsSuccess(out int value);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(isSuccess).IsTrue();
        await Assert.That(value).IsEqualTo(entity);
    }

    [Test]
    public async Task RepoResultOfT_ToSuccessOrFailure_Success() {
        // Arrange
        const int entity = 123456789;
        RepoResult<int> result = entity;

        // Act
        SuccessOrFailure<int> successOrFailure = result.ToSuccessOrFailure();

        // Assert
        await Assert.That(successOrFailure.IsSuccess).IsTrue();
        await Assert.That(successOrFailure.AsSuccess.Value).IsEqualTo(entity);
    }

    [Test]
    public async Task RepoResultOfT_ToSuccessOrFailure_Failure() {
        // Arrange
        const string error = "Error";
        RepoResult<int[]> result = error;

        // Act
        SuccessOrFailure<int[]> successOrFailure = result.ToSuccessOrFailure();

        // Assert
        await Assert.That(successOrFailure.IsFailure).IsTrue();
        await Assert.That(successOrFailure.AsFailure.Value).IsEqualTo(error);
    }
}
