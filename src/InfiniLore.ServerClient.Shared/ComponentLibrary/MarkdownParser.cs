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
          (?<escaped>\\([!"\#$%&'()*+,-./:;<=>?@[\\\]^_`{|}~]))
        | (?<boldAndItalic>\*\*\*([^*]+?)\*\*\*|___([^_]+?)___)
        | (?<bold>\*\*([^*]+?)\*\*|__([^_]+?)__)
        | (?<italic>\*([^*]+?)\*|_([^_]+?)_)
        | (?<strike>~~(.+?)~~)
        | (?<code>`((?:[^`\\]|\\`)+?)`)
        | (?<link>(!)?\[(.+?)\]\((.+?)\))
        | (?<copyright>&copy;)
        | (?<amp>&)
        | (?<script><script.*?>[\w\s\D]*?</script>)
        | (?<lessThan>(<)(?:(?=\s|[^a-z!/-])|$))
        | (?<greaterThan>(?:(?<=[^a-z!/-])|^)(>))
        """, RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex SinglelineStructuresRegex { get; }

    [GeneratedRegex("""
          (?<heading>^(\#{1,6})\s(.+))
        | (?<codeBlock>```(.+?)\n([\s\S]*?)```)
        | (?<headingSimple>^(.+?)\n\s*[-=]{3,})
        | (?<listUnordered>(?:^[^\S\r\n]*[*+-]\s+.+(?:(?:\n[^\S\r\n]*[*+-.]\d*\.?\s+.+)|(?:\n[^\S\r\n]+.+))*(?:[^\S\r\n]{0,2}(?![\r\n]))?)+)
        | (?<listOrdered>(?:^[^\S\r\n]*[-.]?\d+\.?\s+.+(?:(?:\n[^\S\r\n]*[-.]?\d+\.?\s+.+)|(?:\n[^\S\r\n]+.+))*(?:[^\S\r\n]{0,2}(?![\r\n]))?)+)
        | (?<table>
            ^\|(.+)\|\s*\r?\n
            ^\|([:\-|\ ]+)\|\s*\r?\n
            ((?:^\|.+\|\s*)+)
          )
        | (?<htmlTag>
            <(?<tag>\w+)(?:\s[^>]*)?>
            (?:(?!<\k<tag>>)[\s\S]*|<\k<tag>[\s\S]*?</\k<tag>>)*
            </\k<tag>> 
          )  
        | (?<remainder>.+?(?:\n|$))
        """, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline )]
    private static partial Regex MultilineStructuresRegex { get; }
    
    [GeneratedRegex(@"^[ ]*[*+-.]?\d*\.?\s+(.+)((?:(?:(?:\n[ ]+[*+-.]?\d*\.?)|(?:\n[ ]+))\s+.+)*)", RegexOptions.Multiline)]
    private static partial Regex ListItemBodyRegex { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public string Parse(string markdown) 
        => MultilineStructuresRegex.Replace(markdown, evaluator: MultilineStructuresEvaluator);

    private static string MultilineStructuresEvaluator(Match match) {
        if (match.Groups["remainder"].TryGetValue(out string? paragraph)) {
            string output = SinglelineStructuresRegex.Replace(paragraph, static m => SinglelineStructuresEvaluator(m));
            return $"<p>{output}</p>";
        }
        
        if (match.Groups["heading"].Success && match.Groups[1].TryGetLength(out int headingLevel) && match.Groups[2].TryGetValue(out string? headerText)) {
            string output = SinglelineStructuresRegex.Replace(headerText, static m => SinglelineStructuresEvaluator(m));
            return $"<h{headingLevel}>{output}</h{headingLevel}>";
        }

        if (match.Groups["codeBlock"].Success && match.Groups[3].TryGetValue(out string? langName) && match.Groups[4].TryGetValue(out string? codeBlockBody)) {
            string output = HtmlEncoder.Default.Encode(codeBlockBody);
            return $"<pre><code lang=\"{langName}\">{output}</code></pre>";
        }

        if (match.Groups["headingSimple"].Success && match.Groups[5].TryGetValue(out string? headerSimpleText)) {
            string output = SinglelineStructuresRegex.Replace(headerSimpleText, static m => SinglelineStructuresEvaluator(m));
            return $"<h1>{output}</h1>";
        }

        if (match.Groups["listUnordered"].TryGetValue(out string? listUnorderedBody)) {
            StringBuilder builder = StringBuilderPool.Get();
            try {
                builder.Append("<ul>");
                foreach (Match lineMatch in ListItemBodyRegex.Matches(listUnorderedBody)) {
                    builder.Append("<li>");
                    if (lineMatch.Groups[1].TryGetValue(out string? listHeader)) {
                        string listHeaderOutput = SinglelineStructuresRegex.Replace(listHeader, static m => SinglelineStructuresEvaluator(m));
                        builder.Append(listHeaderOutput);
                    }
                    if (lineMatch.Groups[2].TryGetValue(out string? listBody)) {
                        string listBodyOutput = MultilineStructuresRegex.Replace(listBody, MultilineStructuresEvaluator);
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

                    if (lineMatch.Groups[1].TryGetValue(out string? listHeader)) {
                        string listHeaderOutput = SinglelineStructuresRegex.Replace(listHeader, static m => SinglelineStructuresEvaluator(m));
                        builder.Append(listHeaderOutput);
                    }

                    if (lineMatch.Groups[2].TryGetValue(out string? listBody)) {
                        string listBodyOutput = MultilineStructuresRegex.Replace(listBody, MultilineStructuresEvaluator);
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
            ReadOnlySpan<char> header = match.Groups[6].ValueSpan;
            Span<Range> headerColumns = stackalloc Range[header.Length];
            int headerColumnCount = header.Split(headerColumns, '|', StringSplitOptions.TrimEntries);

            ReadOnlySpan<char> separator = match.Groups[7].ValueSpan;
            Span<Range> separatorColumns = stackalloc Range[separator.Length];
            int _ = separator.Split(separatorColumns, '|', StringSplitOptions.TrimEntries);

            ReadOnlySpan<char> rows = match.Groups[8].ValueSpan;
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
                    string output = SinglelineStructuresRegex.Replace(column.ToString(), static m => SinglelineStructuresEvaluator(m));
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
                            string output = SinglelineStructuresRegex.Replace(column.ToString(), static m => SinglelineStructuresEvaluator(m));
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

        if (match.Groups["htmlBody"].Success) {
            return match.Value;
        }

        return match.Value;
    }
    
    private static string SinglelineStructuresEvaluator(Match match, Origin origin = Origin.Undefined) {
        if (match.Groups["escaped"].Success && match.Groups[1].TryGetValue(out string? escapedChar)) {
            return escapedChar;
        }
        
        if (!origin.HasFlag(Origin.BoldAndItalic) && match.Groups["boldAndItalic"].Success) {
            if (match.Groups[2].TryGetValue(out string? boldAndItalicValue)) {
                string output = SinglelineStructuresRegex.Replace(boldAndItalicValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | Origin.BoldAndItalic));
                return $"<strong><em>{output}</em></strong>";
            }

            if (match.Groups[3].TryGetValue(out string? boldAndItalicUnderscoreValue)) {
                string output = SinglelineStructuresRegex.Replace(boldAndItalicUnderscoreValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | Origin.BoldAndItalic));
                return $"<strong>{output}</strong>";
            }
        }

        if (!origin.HasFlag(Origin.Bold) && match.Groups["bold"].Success) {
            if (match.Groups[4].TryGetValue(out string? boldValue)) {
                string output = SinglelineStructuresRegex.Replace(boldValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | Origin.Bold));
                return $"<strong>{output}</strong>";
            }

            if (match.Groups[5].TryGetValue(out string? boldUnderscoreValue)) {
                string output = SinglelineStructuresRegex.Replace(boldUnderscoreValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | Origin.Bold));
                return $"<strong>{output}</strong>";
            }
        }

        if (!origin.HasFlag(Origin.Italic) && match.Groups["italic"].Success) {
            if (match.Groups[6].TryGetValue(out string? italicValue)) {
                string output = SinglelineStructuresRegex.Replace(italicValue, evaluator:  m => SinglelineStructuresEvaluator(m, origin | Origin.Italic));
                return $"<em>{output}</em>";
            }

            if (match.Groups[7].TryGetValue(out string? italicUnderscoreValue)) {
                string output = SinglelineStructuresRegex.Replace(italicUnderscoreValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | Origin.Italic));
                return $"<em>{output}</em>";
            }
        }

        if (!origin.HasFlag(Origin.Strike) && match.Groups["strike"].Success && match.Groups[8].TryGetValue(out string? strikeValue)) {
            string output = SinglelineStructuresRegex.Replace(strikeValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | Origin.Strike));
            return $"<s>{output}</s>";

        }

        if (!origin.HasFlag(Origin.Code) && match.Groups["code"].Success && match.Groups[9].TryGetValue(out string? codeValue)) {
            string output = HtmlEncoder.Default.Encode(codeValue);
            return $"<code>{output}</code>";
        }

        if (!origin.HasFlag(Origin.Link) && match.Groups["link"].Success
            && match.Groups[11].TryGetValue(out string? linkText)
            && match.Groups[12].TryGetValue(out string? linkHref)
        ) {
            if (match.Groups[10].Success) {
                return $"<img src=\"{linkHref}\" alt=\"{linkText}\">";
            }
            string output = SinglelineStructuresRegex.Replace(linkText, evaluator: m => SinglelineStructuresEvaluator(m, origin | Origin.Link));
            return $"<a href=\"{linkHref}\">{output}</a>";
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
    

    // # ***boldAndItalic*** **bold** *italic* ~~strike~~ `code` [text](https://example.com)  ___boldAndItalic___ __bold__ _italic_
    // ***boldAndItalic*** **bold** *italic* ~~strike~~ `code` [text](https://example.com)  ___boldAndItalic___ __bold__ _italic_
    // ** [text](https://example.com) **
    // # heading
    // ## headin
    // ### headi
    // #### head
    // ##### hea
    // ###### he

    // TODO should be flags
    [Flags]
    private enum Origin {
        Undefined = 0,
        BoldAndItalic = 1 << 0,
        Bold = 1 << 1,
        Italic = 1 << 2,
        Strike = 1 << 3,
        Code = 1 << 4,
        Link = 1 << 5,
    }
}
