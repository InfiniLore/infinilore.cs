// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.MultiLine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<IMultiLineSectionParser>("listOrdered", ServiceLifetime.Singleton)]
public class ListOrderedSectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    private readonly Lazy<IMarkdownParser> _markdownParser = new(provider.GetRequiredService<IMarkdownParser>);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer) {
        if (!group.TryGetValue(out string? listOrderedBody)) return;
        
        writer.Write("<ol>");
        foreach (Match lineMatch in MarkdownRegexLib.ListItemBodyRegex.Matches(listOrderedBody)) {
            writer.Write("<li>");

            if (lineMatch.Groups["lHead"].TryGetValue(out string? listHeader)) {
                _markdownParser.Value.ParseSingleline(listHeader, writer);
            }

            if (lineMatch.Groups["lBody"].TryGetValue(out string? listBody)) {
                string normalizedBody = NormalizationHelper.NormalizeIndentation(listBody);
                _markdownParser.Value.ParseMultiline(normalizedBody, writer);
            }

            writer.Write("</li>");
        }

        writer.Write("</ol>");
    }
}
