// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Buffers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IMarkdownParser>(ServiceLifetime.Singleton)]
public partial class MarkdownParser : IMarkdownParser {
    [GeneratedRegex("""
          (?<escaped>\\[!"\#$%&'()*+,-./:;<=>?@[\\\]^_`{|}~])
        | (?<boldAndItalic>(?<bi>\*\*\*)(?<biText>.+?)(?<!\\)\k<bi>)
        | (?<bold>(?<b>\*\*)(?<bText>.+?(?:(?<iNested>\*|_)[^*]+?\k<iNested>)?)(?<!\\)\k<b>)
        | (?<italic>(?<i>\*)(?<iText>.+?)(?<!\\)\k<i>)
        | (?<strike>~~(?<sText>.+?)~~)
        | (?<code>(?<open>`+)(?<codeText>(?>[^`\\]+|\\.|`(?!\k<open>))*?)\k<open>)
        | (?<linkNested>
            (?<lnBang>!)?
            \[(?<lnText>!?\[.+?\]\(.+?\))\]
            \((?<lnHref>.+?)(?:\s?"(?<lnTitle>[^"]*)")?\))
        | (?<linkRegular>
            (?<lrBang>!)?
            \[(?<lrText>[^\]]+?)\]
            \((?<lrHref>[^\)]+?)(?:\s?"(?<lrTitle>[^"]*)")?\))
        | (?<copyright>&copy;)
        | (?<amp>&)
        | (?<script><script.*?>[\w\s\D]*?</script>)
        | (?<lessThan><)
        | (?<greaterThan>>)
        """, RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex SinglelineStructuresRegex { get; }

    [GeneratedRegex("""
          (?<heading>^(?<hLevel>\#{1,6})\s(?<hText>.+))
        | (?<codeBlock>```(?<cLang>.+?)?\r?\n+?(?<cBody>[\s\S]+?)```\s*?$)
        | (?<headingSimple>^(?<hsText>.+?)\r?\n[\ ]*[-=]{3,})
        | (?<listUnordered>(?:^[^\S\r\n]*-\s+.+(?:(?:\n[^\S\r\n]*[-.]\d*\.?\s+.+)|(?:\n[^\S\r\n]+.+))*(?:[^\S\r\n]{0,2}(?![\r\n]))?)+)
        | (?<listOrdered>(?:^[^\S\r\n]*[-.]?\d+\.?\s+.+(?:(?:\n[^\S\r\n]*[-.]?\d+\.?\s+.+)|(?:\n[^\S\r\n]+.+))*(?:[^\S\r\n]{0,2}(?![\r\n]))?)+)
        | (?<table>
            ^\|(?<tHead>.+)\|\s*\r?\n
            ^\|(?<tSep>[:\-|\ ]+)\|\s*\r?\n
            (?<tBody>(?:^\|.+\|\s*)+)
          )
        | (?<blockQuote>^>\s+(?:(?![*+-]\s+.+|[-.]?\d+).+(?:\r?\n|$)?)*)
        | (?<htmlBody>
            <(?<tag>\w+)\b[^>]*>
                (?:
                    [^<]+
                    | <(?<OPEN>\k<tag>)\b[^>]*>
                    | </(?<-OPEN>\k<tag>)>
                    | <(?!/?\k<tag>\b)[^>]+>
                )*
                # (?:(?<-OPEN>)(?!)) # This should be the end of the tag, but it doesnt work
                </\k<tag>>
          )  
        | (?<horizontalRule>^[*-_]{3,}\s*$)
        | (?<remainder>.+?(?:\r?\n|$))
        """, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.ExplicitCapture  | RegexOptions.Compiled)]
    private static partial Regex MultilineStructuresRegex { get; }

    [GeneratedRegex(@"^[ ]*[-.]?\d*\.?\s+(?<lHead>.+)(?<lBody>(?:\n[ ]+.+)*)", RegexOptions.Multiline | RegexOptions.ExplicitCapture)]
    private static partial Regex ListItemBodyRegex { get; }

    [GeneratedRegex(@"^>", RegexOptions.Multiline)]
    private static partial Regex NormalizeBlockQuoteRegex { get; }

    [GeneratedRegex("\r?\n")]
    private static partial Regex NormalizeNewlinesRegex { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public string Parse(string markdown)
        => MultilineStructuresRegex.Replace(markdown, MultilineStructuresEvaluator);

    public string ParseByMatches(string markdown) {
        StringBuilder builder = StringBuilderPool.Get();
        try {
            MatchCollection enumerable = MultilineStructuresRegex.Matches(markdown);
            foreach (Match match in enumerable) {
                string output = MultilineStructuresEvaluator(match);
                builder.AppendLine(output);
            }

            return builder.ToString();
        }
        finally {
            StringBuilderPool.Return(builder);
        }

    }

    private static string MultilineStructuresEvaluator(Match match) {
        if (match.Groups["remainder"].TryGetValue(out string? paragraph)) {
            if (paragraph.IsNullOrWhiteSpace()) return string.Empty;

            string output = SinglelineStructuresRegex.Replace(paragraph, evaluator: static m => SinglelineStructuresEvaluator(m));
            return $"<p>{output}</p>";
        }

        if (match.Groups["heading"].Success
            && match.Groups["hLevel"].TryGetLength(out int headingLevel)
            && match.Groups["hText"].TryGetValue(out string? headerText)
        ) {
            string output = SinglelineStructuresRegex.Replace(headerText, evaluator: static m => SinglelineStructuresEvaluator(m));
            return $"<h{headingLevel}>{output}</h{headingLevel}>";
        }

        if (match.Groups["codeBlock"].Success
            && match.Groups["cBody"].TryGetValue(out string? codeBlockBody)
        ) {
            string langName = match.Groups["cLang"].TryGetValue(out string? langNameValue) ? langNameValue : string.Empty;
            string output = HtmlEncoder.Default.Encode(codeBlockBody);
            string langClass = langName.IsNullOrWhiteSpace() ? string.Empty : $" class=\"language-{langName}\"";
            return $"<pre><code{langClass}>{output}</code></pre>";
        }

        if (match.Groups["headingSimple"].Success
            && match.Groups["hsText"].TryGetValue(out string? headerSimpleText)
        ) {
            string output = SinglelineStructuresRegex.Replace(headerSimpleText, evaluator: static m => SinglelineStructuresEvaluator(m));
            return $"<h1>{output}</h1>";
        }

        if (match.Groups["listUnordered"].TryGetValue(out string? listUnorderedBody)) {
            StringBuilder builder = StringBuilderPool.Get();
            try {
                builder.Append("<ul>");
                foreach (Match lineMatch in ListItemBodyRegex.Matches(listUnorderedBody)) {
                    builder.Append("<li>");
                    if (lineMatch.Groups["lHead"].TryGetValue(out string? listHeader)) {
                        string listHeaderOutput = SinglelineStructuresRegex.Replace(listHeader, evaluator: static m => SinglelineStructuresEvaluator(m));
                        builder.Append(listHeaderOutput);
                    }

                    if (lineMatch.Groups["lBody"].TryGetValue(out string? listBody)) {
                        string normalizedBody = NormalizeIndentation(listBody);
                        string listBodyOutput = MultilineStructuresRegex.Replace(normalizedBody, MultilineStructuresEvaluator);
                        builder.Append(listBodyOutput);
                    }

                    builder.Append("</li>");
                }

                builder.Append("</ul>");
                return builder.ToString();
            }
            finally {
                StringBuilderPool.Return(builder);
            }
        }

        if (match.Groups["listOrdered"].TryGetValue(out string? listOrderedBody)) {
            StringBuilder builder = StringBuilderPool.Get();
            try {
                builder.Append("<ol>");
                foreach (Match lineMatch in ListItemBodyRegex.Matches(listOrderedBody)) {
                    builder.Append("<li>");

                    if (lineMatch.Groups["lHead"].TryGetValue(out string? listHeader)) {
                        string listHeaderOutput = SinglelineStructuresRegex.Replace(listHeader, evaluator: static m => SinglelineStructuresEvaluator(m));
                        builder.Append(listHeaderOutput);
                    }

                    if (lineMatch.Groups["lBody"].TryGetValue(out string? listBody)) {
                        string normalizedBody = NormalizeIndentation(listBody);
                        string listBodyOutput = MultilineStructuresRegex.Replace(normalizedBody, MultilineStructuresEvaluator);
                        builder.Append(listBodyOutput);
                    }

                    builder.Append("</li>");
                }

                builder.Append("</ol>");
                return builder.ToString();
            }
            finally {
                StringBuilderPool.Return(builder);
            }
        }

        if (match.Groups["table"].Success) {
            // Extract header, separator, and rows
            ReadOnlySpan<char> header = match.Groups["tHead"].ValueSpan;
            Span<Range> headerColumns = stackalloc Range[header.Length];
            int headerColumnCount = header.Split(headerColumns, '|', StringSplitOptions.TrimEntries);

            ReadOnlySpan<char> separator = match.Groups["tSep"].ValueSpan;
            Span<Range> separatorColumns = stackalloc Range[separator.Length];
            int _ = separator.Split(separatorColumns, '|', StringSplitOptions.TrimEntries);

            ReadOnlySpan<char> rows = match.Groups["tBody"].ValueSpan;
            Span<Range> rowRanges = stackalloc Range[rows.Length];
            int rowCount = rows.Split(rowRanges, '\n', StringSplitOptions.TrimEntries);

            // Construct table HTML
            StringBuilder builder = StringBuilderPool.Get();
            try {
                builder.Append("<table>");

                // Add headers
                builder.Append("<thead><tr>");
                for (int index = 0; index < headerColumnCount; index++) {
                    builder.Append("<th>");
                    ReadOnlySpan<char> column = header[headerColumns[index]];
                    string output = SinglelineStructuresRegex.Replace(column.ToString(), evaluator: static m => SinglelineStructuresEvaluator(m));
                    builder.Append(output);
                    builder.Append("</th>");
                }

                builder.Append("</tr></thead>");

                // Add rows
                builder.Append("<tbody>");
                ArrayPool<Range> bufferPool = ArrayPool<Range>.Shared;
                const int maxExpectedRowLength = 512;// Based on expected data characteristics
                Range[] rowColumnRanges = bufferPool.Rent(maxExpectedRowLength);

                try {
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++) {
                        Range rowRange = rowRanges[rowIndex];
                        ReadOnlySpan<char> row = rows[rowRange];

                        // Split the row
                        int rowColumnCount = row.Split(rowColumnRanges.AsSpan(0, row.Length), '|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                        builder.Append("<tr>");
                        for (int columnIndex = 0; columnIndex < rowColumnCount; columnIndex++) {
                            builder.Append("<td>");
                            Range columnRange = rowColumnRanges[columnIndex];
                            ReadOnlySpan<char> column = row[columnRange];
                            string output = SinglelineStructuresRegex.Replace(column.ToString(), evaluator: static m => SinglelineStructuresEvaluator(m));
                            builder.Append(output);
                            builder.Append("</td>");
                        }

                        builder.Append("</tr>");
                    }
                }
                finally {
                    bufferPool.Return(rowColumnRanges);// Ensure the buffer is returned, avoiding leaks
                }

                builder.Append("</tbody>");

                builder.Append("</table>");
                return builder.ToString();
            }
            finally {
                StringBuilderPool.Return(builder);
            }
        }

        if (match.Groups["blockQuote"].TryGetValue(out string? blockQuoteBody)) {
            string normalized = NormalizeBlockQuoteRegex.Replace(blockQuoteBody, string.Empty);
            string adjustedBlockquote = NormalizeIndentation(normalized);
            string output = MultilineStructuresRegex.Replace(adjustedBlockquote, MultilineStructuresEvaluator);
            return $"<blockquote>{output}</blockquote>";
        }

        if (match.Groups["htmlBody"].Success) {
            return match.Value;
        }

        if (match.Groups["horizontalRule"].Success) {
            return "<hr>";
        }

        return string.Empty;
    }

    private static string SinglelineStructuresEvaluator(Match match, Origin origin = Origin.Undefined) {
        if (match.Groups["escaped"].TryGetValueSpan(out ReadOnlySpan<char> escapedCharSpan)
        ) {
            return escapedCharSpan[1].ToString(); // skip the `\` character
        }

        if (!origin.HasFlag(Origin.BoldAndItalic)
            && match.Groups["boldAndItalic"].Success
            && match.Groups["biText"].TryGetValue(out string? boldAndItalicValue)
        ) {
            string output = SinglelineStructuresRegex.Replace(boldAndItalicValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | Origin.BoldAndItalic));
            return $"<strong><em>{output}</em></strong>";
        }

        if (!origin.HasFlag(Origin.Bold)
            && match.Groups["bold"].Success
            && match.Groups["bText"].TryGetValue(out string? boldValue)
        ) {
            string output = SinglelineStructuresRegex.Replace(boldValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | Origin.Bold));
            return $"<strong>{output}</strong>";
        }

        if (!origin.HasFlag(Origin.Italic)
            && match.Groups["italic"].Success
            && match.Groups["iText"].TryGetValue(out string? italicValue)
        ) {
            string output = SinglelineStructuresRegex.Replace(italicValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | Origin.Italic));
            return $"<em>{output}</em>";
        }

        if (!origin.HasFlag(Origin.Strike)
            && match.Groups["strike"].Success
            && match.Groups["sText"].TryGetValue(out string? strikeValue)
        ) {
            string output = SinglelineStructuresRegex.Replace(strikeValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | Origin.Strike));
            return $"<s>{output}</s>";

        }

        if (!origin.HasFlag(Origin.Code)
            && match.Groups["code"].Success
            && match.Groups["codeText"].TryGetValue(out string? codeValue)
        ) {
            string normalizedBackticks = codeValue.Replace("\\`", "`");
            string output = HtmlEncoder.Default.Encode(normalizedBackticks);
            return $"<code>{output}</code>";
        }

        if (match.Groups["linkNested"].Success
            && match.Groups["lnText"].TryGetValue(out string? linkText)
            && match.Groups["lnHref"].TryGetValue(out string? linkHref)
        ) {
            string titleText = match.Groups["lnTitle"].TryGetValue(out string? altTextValue) ? $" title=\"{altTextValue}\"" : string.Empty;

            if (match.Groups["lnBang"].Success) {
                return $"<img src=\"{linkHref}\" alt=\"{linkText}\"{titleText}>";
            }

            string output = SinglelineStructuresRegex.Replace(linkText, evaluator: m => SinglelineStructuresEvaluator(m, origin));
            return $"<a href=\"{linkHref}\">{output}</a>";
        }

        if (!origin.HasFlag(Origin.Link) && match.Groups["linkRegular"].Success
            && match.Groups["lrText"].TryGetValue(out string? linkRegularText)
            && match.Groups["lrHref"].TryGetValue(out string? linkRegularHref)
        ) {
            string titleText = match.Groups["lrTitle"].TryGetValue(out string? altTextValue) ? $" title=\"{altTextValue}\"" : string.Empty;

            if (match.Groups["lrBang"].Success) {
                return $"<img src=\"{linkRegularHref}\" alt=\"{linkRegularText}\"{titleText}>";
            }

            string output = SinglelineStructuresRegex.Replace(linkRegularText, evaluator: m => SinglelineStructuresEvaluator(m, origin | Origin.Link));
            return $"<a href=\"{linkRegularHref}\">{output}</a>";
        }

        if (match.Groups["copyright"].Success) {
            return "\u00a9";
        }

        if (match.Groups["amp"].Success) {
            return "&amp;";
        }

        if (match.Groups["script"].TryGetValue(out string? scriptBody)) {
            string output = HtmlEncoder.Default.Encode(scriptBody);
            return output;
        }

        if (match.Groups["lessThan"].Success) {
            return "&lt;";
        }

        if (match.Groups["greaterThan"].Success) {
            return "&gt;";
        }

        return match.Value;
    }

    [Flags]
    private enum Origin {
        Undefined = 0,
        BoldAndItalic = 1 << 0,
        Bold = 1 << 1,
        Italic = 1 << 2,
        Strike = 1 << 3,
        Code = 1 << 4,
        Link = 1 << 5
    }

    private static string NormalizeIndentation(string input) {
        const int smallLineThreshold = 10;
        string[] lines = NormalizeNewlinesRegex.Split(input);
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
