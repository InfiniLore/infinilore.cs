// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Types;
using JetBrains.Annotations;
using Moq;
using System.Reflection;

namespace Tests.InfiniLore.Server.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(AsyncLazy<>))]
public class AsyncLazyTests {
    [Test]
    public async Task GetValueAsync_ShouldReturnValue_FromTestoryMethod() {
        // Arrange
        int expectedValue = 42;
        var lazy = new AsyncLazy<int>(_ => Task.FromResult(expectedValue));

        // Act
        int result = await lazy.GetValueAsync();

        // Assert
        await Assert.That(result).IsEqualTo(expectedValue);
    }

    [Test]
    public async Task GetValueAsync_ShouldInitializeValueOnlyOnce() {
        // Arrange
        int callCount = 0;
        var lazy = new AsyncLazy<int>(_ => {
            callCount++;
            return Task.FromResult(42);
        });

        // Act
        await lazy.GetValueAsync();
        await lazy.GetValueAsync();// Call again to ensure the factory is only called once

        // Assert
        await Assert.That(callCount).IsEqualTo(1);
    }

    [Test]
    public async Task GetValueAsync_ShouldHandleCancellationToken() {
        // Arrange
        var cts = new CancellationTokenSource();
        var lazy = new AsyncLazy<int>(static ct => {
            ct.ThrowIfCancellationRequested();
            return Task.FromResult(42);
        });

        // Act & Assert
        await cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(() => lazy.GetValueAsync(cts.Token));
    }

    [Test]
    public async Task DisposeAsync_ShouldDisposeIDisposableResource() {
        // Arrange
        var mockDisposable = new Mock<IDisposable>();
        var lazy = new AsyncLazy<IDisposable>(_ => Task.FromResult(mockDisposable.Object));

        // Act
        await lazy.GetValueAsync(); // We need to set the value before disposing, else we just skip disposing most of the time
        await lazy.DisposeAsync();

        // Assert
        mockDisposable.Verify(m => m.Dispose(), Times.Once);
    }

    [Test]
    public async Task DisposeAsync_ShouldDisposeIAsyncDisposableResource() {
        // Arrange
        var mockAsyncDisposable = new Mock<IAsyncDisposable>();
        mockAsyncDisposable
            .Setup(d => d.DisposeAsync())
            .Returns(ValueTask.CompletedTask);  // Changed this line

        var lazy = new AsyncLazy<IAsyncDisposable>(_ => Task.FromResult(mockAsyncDisposable.Object));

        // Act
        await lazy.GetValueAsync(); // We need to set the value before disposing, else we just skip disposing most of the time
        await lazy.DisposeAsync();

        // Assert
        mockAsyncDisposable.Verify(d => d.DisposeAsync(), Times.Once);
    }

    [Test]
    public async Task DisposeAsync_ShouldResetValueToNull() {
        // Arrange
        var lazy = new AsyncLazy<int>(_ => Task.FromResult(42));

        // Act
        await lazy.GetValueAsync();// Load the value
        await lazy.DisposeAsync();// Dispose to reset it

        // Use reflection to access the private `_value` field
        FieldInfo? privateField = typeof(AsyncLazy<int>).GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance);
        object? value = privateField?.GetValue(lazy);

        // Assert
        await Assert.That(value).IsNull();
    }
}