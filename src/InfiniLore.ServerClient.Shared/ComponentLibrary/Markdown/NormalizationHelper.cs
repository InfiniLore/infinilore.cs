// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.Pools;
using System.Buffers;
using System.Text;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class NormalizationHelper {
    public static string NormalizeIndentation(string input) {
        const int smallLineThreshold = 10;
        string[] lines = MarkdownRegexLib.NormalizeNewlinesRegex.Split(input);
        int minIndent = int.MaxValue;

        // ReSharper disable once ForCanBeConvertedToForeach
        for (int index = 0; index < lines.Length; index++) {
            string line = lines[index];
            ReadOnlySpan<char> trimmed = line.AsSpan().TrimStart();
            if (trimmed.IsEmpty) continue;

            int leadingSpaces = line.Length - trimmed.Length;
            minIndent = Math.Min(minIndent, leadingSpaces);
        }

        if (minIndent == int.MaxValue) return input;

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (lines.Length <= smallLineThreshold) return ProcessSmallerInput(lines, minIndent);

        return ProcessLargerInput(lines, minIndent);
    }

    private static string ProcessLargerInput(string[] lines, int minIndent) {
        StringBuilder stringBuilder = StringBuilderPool.Get();
        try {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int index = 0; index < lines.Length; index++) {
                ReadOnlySpan<char> trimmed = lines[index].AsSpan();
                stringBuilder.Append(trimmed.Length >= minIndent ? trimmed[minIndent..] : trimmed);
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
        finally {
            StringBuilderPool.Return(stringBuilder);
        }
    }

    private static string ProcessSmallerInput(string[] lines, int minIndent) {
        int totalLength = 0;

        // ReSharper disable once ForCanBeConvertedToForeach
        for (int index = 0; index < lines.Length; index++) {
            string line = lines[index];
            ReadOnlySpan<char> span = line.AsSpan();
            totalLength += Math.Max(span.Length - minIndent, 0) + 1;// Include space for '\n'
        }

        char[] buffer = ArrayPool<char>.Shared.Rent(totalLength);
        try {
            Span<char> resultSpan = buffer.AsSpan(0, totalLength);
            int position = 0;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int index = 0; index < lines.Length; index++) {
                ReadOnlySpan<char> span = lines[index].AsSpan();
                ReadOnlySpan<char> trimmed = span.Length >= minIndent ? span[minIndent..] : span;

                trimmed.CopyTo(resultSpan[position..]);
                position += trimmed.Length;
                resultSpan[position++] = '\n';
            }

            return resultSpan[..(position - 1)].ToString();// Exclude trailing newline
        }
        finally {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }
}
