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
[KeyedInjectableService<IMultiLineSectionParser>("heading", ServiceLifetime.Singleton)]
public class HeadingSectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    private readonly Lazy<IMarkdownParser> _markdownParser = new(provider.GetRequiredService<IMarkdownParser>);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer) {
        if (!entireMatch.Groups["hLevel"].TryGetLength(out int headingLevel)) return;
        if (!entireMatch.Groups["hText"].TryGetValue(out string? headerText)) return;

        writer.Write("<h").Write(headingLevel).Write('>');
        _markdownParser.Value.ParseSingleline(headerText, writer);
        writer.Write("</h").Write(headingLevel).Write('>');
    }
}
