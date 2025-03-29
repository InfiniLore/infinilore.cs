// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.SingleLine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<IMultiLineSectionParser>("boldAnditalic", ServiceLifetime.Singleton)]
public class BoldAndItalicSectionParser(IParserSwitcher parserSwitcher) : ISingleLineSectionParser {
    public SingleLineOrigin SkipOnOrigin => SingleLineOrigin.BoldAndItalic;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, StringBuilder builder, SingleLineOrigin origin) {
        if (!entireMatch.Groups["boldAndItalic"].Success) return;
        if (!entireMatch.Groups["biText"].TryGetValue(out string? boldAndItalicValue)) return;

        builder.Append("<strong><em>");
        parserSwitcher.ParseSingleline(boldAndItalicValue, builder, origin | SingleLineOrigin.BoldAndItalic);
        builder.Append("</em></strong>");
    }
}
