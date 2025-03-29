// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
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
        for (int i = 0; i < lines.Length; i++) {
            string line = lines[i];
            ReadOnlySpan<char> trimmed = line.AsSpan().TrimStart();
            if (trimmed.IsEmpty) continue;

            int leadingSpaces = line.Length - trimmed.Length;
            minIndent = Math.Min(minIndent, leadingSpaces);
        }

        if (minIndent == int.MaxValue) return input;

        // If the number of lines is small, use simple string concatenation
        if (lines.Length <= smallLineThreshold) {
            int totalLength = 0;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int index = 0; index < lines.Length; index++) {
                string line = lines[index];
                ReadOnlySpan<char> span = line.AsSpan();
                totalLength += Math.Max(span.Length - minIndent, 0) + 1;// Account for "\n"
            }

            char[] rentedBuffer = ArrayPool<char>.Shared.Rent(totalLength);
            try {
                Span<char> resultSpan = rentedBuffer.AsSpan(0, totalLength);
                int position = 0;

                // ReSharper disable once ForCanBeConvertedToForeach
                for (int i = 0; i < lines.Length; i++) {
                    ReadOnlySpan<char> span = lines[i].AsSpan();
                    ReadOnlySpan<char> trimmed = span.Length >= minIndent ? span[minIndent..] : span;

                    trimmed.CopyTo(resultSpan[position..]);
                    position += trimmed.Length;
                    resultSpan[position++] = '\n';
                }

                return resultSpan[..(position - 1)].ToString();
            }
            finally {
                ArrayPool<char>.Shared.Return(rentedBuffer);
            }
        }


        // Use StringBuilder for larger inputs
        StringBuilder resultBuilder = StringBuilderPool.Get();
        try {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < lines.Length; i++) {
                ReadOnlySpan<char> span = lines[i].AsSpan();
                resultBuilder.Append(span.Length >= minIndent ? span[minIndent..] : span);
                resultBuilder.AppendLine();
            }

            return resultBuilder.ToString();
        }
        finally {
            StringBuilderPool.Return(resultBuilder);
        }
    }
}
