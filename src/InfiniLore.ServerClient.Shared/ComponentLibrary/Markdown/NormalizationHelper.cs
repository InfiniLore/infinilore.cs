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
        int lineCount = lines.Length;
        int minIndent = int.MaxValue;

        for (int index = 0; index < lineCount; index++) {
            string line = lines[index];
            ReadOnlySpan<char> trimmed = line.AsSpan().TrimStart();
            if (trimmed.IsEmpty) continue;
            
            minIndent = Math.Min(minIndent, line.Length - trimmed.Length);
        }

        if (minIndent == int.MaxValue) return input;
        if (lineCount <= smallLineThreshold) return ProcessSmallerInput(lines, minIndent, lineCount);
        return ProcessLargerInput(lines, minIndent, lineCount);
    }

    private static string ProcessLargerInput(string[] lines, int minIndent, int lineCount) {
        StringBuilder stringBuilder = StringBuilderPool.Get();
        
        try {
            for (int index = 0; index < lineCount; index++) {
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

    private static string ProcessSmallerInput(string[] lines, int minIndent, int lineCount) {
        int totalLength = 0;
        for (int index = 0; index < lineCount; index++) {
            int lineLength = lines[index].Length;
            if (lineLength > minIndent) totalLength += lineLength - minIndent + 1; // Include space for '\n'
            else totalLength += 1; // Only the '\n'
        }

        char[] buffer = ArrayPool<char>.Shared.Rent(totalLength);
        try {
            Span<char> resultSpan = buffer.AsSpan(0, totalLength);
            int position = 0;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int index = 0; index < lineCount; index++) {
                string line = lines[index];
                ReadOnlySpan<char> trimmed = line.Length >= minIndent 
                    ? line.AsSpan(minIndent) 
                    : line.AsSpan();

                trimmed.CopyTo(resultSpan[position..]);
                resultSpan[position + trimmed.Length] = '\n'; // Write the newline directly
                position += trimmed.Length + 1;

            }

            return resultSpan[..(position - 1)].ToString(); // Exclude trailing newline
        }
        finally {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }
}
