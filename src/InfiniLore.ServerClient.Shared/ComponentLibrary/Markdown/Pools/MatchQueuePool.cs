// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class MatchQueuePool {
    private const int InitialCapacity = 100;
    private static readonly ObjectPool<Queue<Match>> Pool =
        new DefaultObjectPool<Queue<Match>>(new DefaultPooledObjectPolicy<Queue<Match>>(), InitialCapacity);

    public static Queue<Match> Get() => Pool.Get();

    public static void Return(Queue<Match> builder) {
        builder.Clear();
        Pool.Return(builder);
    }
}
