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
[KeyedInjectableService<IMultiLineSectionParser>("headingSimple", ServiceLifetime.Singleton)]
public class HeadingSimpleSectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    private readonly Lazy<IMarkdownParser> _markdownParser = new(provider.GetRequiredService<IMarkdownParser>);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer) {
        if (!entireMatch.Groups["hsText"].TryGetValue(out string? headerSimpleText)) return;

        writer.Write("<h1>");
        _markdownParser.Value.ParseSingleline(headerSimpleText, writer);
        writer.Write("</h1>");
    }
}
