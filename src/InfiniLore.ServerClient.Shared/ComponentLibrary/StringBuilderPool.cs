// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class StringBuilderPool {
    private static readonly ObjectPool<StringBuilder> Pool =
        ObjectPool.Create(new DefaultPooledObjectPolicy<StringBuilder>());

    public static StringBuilder Get() => Pool.Get();

    public static void Return(StringBuilder builder) {
        builder.Clear();// Ensure the builder is cleared before reusing
        Pool.Return(builder);
    }
}
