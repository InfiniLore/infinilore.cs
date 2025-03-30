// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Buffers;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.MultiLine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<IMultiLineSectionParser>("table", ServiceLifetime.Singleton)]
public class TableSectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    private readonly Lazy<IMarkdownParser> _markdownParser = new(provider.GetRequiredService<IMarkdownParser>);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer) {
        // Extract header, separator, and rows
        ReadOnlySpan<char> header = entireMatch.Groups["tHead"].ValueSpan;
        Span<Range> headerColumns = stackalloc Range[header.Length];
        int headerColumnCount = header.Split(headerColumns, '|', StringSplitOptions.TrimEntries);

        ReadOnlySpan<char> separator = entireMatch.Groups["tSep"].ValueSpan;
        Span<Range> separatorColumns = stackalloc Range[separator.Length];
        int _ = separator.Split(separatorColumns, '|', StringSplitOptions.TrimEntries);

        ReadOnlySpan<char> rows = entireMatch.Groups["tBody"].ValueSpan;
        Span<Range> rowRanges = stackalloc Range[rows.Length];
        int rowCount = rows.Split(rowRanges, '\n', StringSplitOptions.TrimEntries);

        // Construct table HTML
        writer.Write("<table>");

        // Add headers
        writer.Write("<thead><tr>");
        for (int index = 0; index < headerColumnCount; index++) {
            writer.Write("<th>");
            ReadOnlySpan<char> column = header[headerColumns[index]];
            _markdownParser.Value.ParseSingleline(column.ToString(), writer);
            writer.Write("</th>");
        }

        writer.Write("</tr></thead>");

        // Add rows
        writer.Write("<tbody>");
        ArrayPool<Range> bufferPool = ArrayPool<Range>.Shared;
        const int maxExpectedRowLength = 512;// Based on expected data characteristics
        Range[] rowColumnRanges = bufferPool.Rent(maxExpectedRowLength);

        for (int rowIndex = 0; rowIndex < rowCount; rowIndex++) {
            Range rowRange = rowRanges[rowIndex];
            ReadOnlySpan<char> row = rows[rowRange].Trim();
            if (row.IsEmpty) continue;

            // Split the row
            int rowColumnCount = row.Split(rowColumnRanges.AsSpan(0, row.Length), '|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            writer.Write("<tr>");
            for (int columnIndex = 0; columnIndex < rowColumnCount; columnIndex++) {
                writer.Write("<td>");
                Range columnRange = rowColumnRanges[columnIndex];
                ReadOnlySpan<char> column = row[columnRange];
                _markdownParser.Value.ParseSingleline(column.ToString(), writer);
                writer.Write("</td>");
            }

            writer.Write("</tr>");
        }

        writer.Write("</tbody>");

        writer.Write("</table>");
    }
}
