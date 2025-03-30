// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.MarkdownWriters;
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class StringBuilderMarkdownWriterPool {
    private static readonly ObjectPool<StringBuilderMarkdownWriter> Pool =
        ObjectPool.Create(new DefaultPooledObjectPolicy<StringBuilderMarkdownWriter>());

    public static StringBuilderMarkdownWriter Get() => Pool.Get();

    public static void Return(StringBuilderMarkdownWriter builder) {
        builder.Clear();
        Pool.Return(builder);
    }
}

public static class StringBuilderPool {
    private static readonly ObjectPool<StringBuilder> Pool =
        ObjectPool.Create(new DefaultPooledObjectPolicy<StringBuilder>());

    public static StringBuilder Get() => Pool.Get();

    public static void Return(StringBuilder builder) {
        builder.Clear();
        Pool.Return(builder);
    }
}