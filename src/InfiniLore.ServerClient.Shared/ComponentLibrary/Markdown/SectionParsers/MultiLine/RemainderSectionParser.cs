// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.MultiLine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<IMultiLineSectionParser>("remainder", ServiceLifetime.Singleton)]
public class RemainderSectionParser(IParserSwitcher parserSwitcher) : IMultiLineSectionParser {
    public void ParseToStringBuilder(Match _, Group group, StringBuilder builder) {
        if (!group.TryGetValue(out string? paragraph)) return;
        if (paragraph.IsNullOrWhiteSpace()) return;

        builder.Append("<p>");
        parserSwitcher.ParseSingleline(paragraph, builder);
        builder.Append("</p>");
    }
}
