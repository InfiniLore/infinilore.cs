// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Buffers;
using System.Text;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.MultiLine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<IMultiLineSectionParser>("table", ServiceLifetime.Singleton)]
public class TableSectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    public void ParseToStringBuilder(Match entireMatch, Group group, StringBuilder builder) {
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
        builder.Append("<table>");

        // Add headers
        builder.Append("<thead><tr>");
        for (int index = 0; index < headerColumnCount; index++) {
            builder.Append("<th>");
            ReadOnlySpan<char> column = header[headerColumns[index]];
            provider.GetRequiredService<IMarkdownParser>().ParseSingleline(column.ToString(), builder);
            builder.Append("</th>");
        }

        builder.Append("</tr></thead>");

        // Add rows
        builder.Append("<tbody>");
        ArrayPool<Range> bufferPool = ArrayPool<Range>.Shared;
        const int maxExpectedRowLength = 512;// Based on expected data characteristics
        Range[] rowColumnRanges = bufferPool.Rent(maxExpectedRowLength);

        for (int rowIndex = 0; rowIndex < rowCount; rowIndex++) {
            Range rowRange = rowRanges[rowIndex];
            ReadOnlySpan<char> row = rows[rowRange].Trim();
            if (row.IsEmpty) continue;

            // Split the row
            int rowColumnCount = row.Split(rowColumnRanges.AsSpan(0, row.Length), '|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            builder.Append("<tr>");
            for (int columnIndex = 0; columnIndex < rowColumnCount; columnIndex++) {
                builder.Append("<td>");
                Range columnRange = rowColumnRanges[columnIndex];
                ReadOnlySpan<char> column = row[columnRange];
                provider.GetRequiredService<IMarkdownParser>().ParseSingleline(column.ToString(), builder);
                builder.Append("</td>");
            }

            builder.Append("</tr>");
        }

        builder.Append("</tbody>");

        builder.Append("</table>");
    }
}
