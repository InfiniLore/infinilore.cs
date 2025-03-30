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
[KeyedInjectableService<IMultiLineSectionParser>("listUnordered", ServiceLifetime.Singleton)]
public class ListUnorderedSectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    private readonly Lazy<IMarkdownParser> _markdownParser = new(provider.GetRequiredService<IMarkdownParser>);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer) {
        if (!group.TryGetValue(out string? listUnorderedBody)) return;

        List<Match> matchCollection = MarkdownRegexLib.ListItemBodyRegex.Matches(listUnorderedBody).ToList();
        int matchCount = matchCollection.Count;

        writer.Write("<ul>");
        for (int index = 0; index < matchCount; index++) {
            Match match = matchCollection[index];
            GroupCollection groups = match.Groups;

            writer.Write("<li>");
            if (groups["lHead"].TryGetValue(out string? listHeader)) {
                _markdownParser.Value.ParseSingleline(listHeader, writer);
            }

            if (groups["lBody"].TryGetValue(out string? listBody)) {
                string normalizedBody = NormalizationHelper.NormalizeIndentation(listBody);
                _markdownParser.Value.ParseMultiline(normalizedBody, writer);
            }

            writer.Write("</li>");
        }

        writer.Write("</ul>");
    }
}
