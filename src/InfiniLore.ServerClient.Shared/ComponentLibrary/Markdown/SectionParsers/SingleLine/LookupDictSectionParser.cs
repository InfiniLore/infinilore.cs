// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Frozen;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.SingleLine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<ISingleLineSectionParser>("lookupDict", ServiceLifetime.Singleton)]
public class LookupDictSectionParser(ILogger<LookupDictSectionParser> logger) : ISingleLineSectionParser{
    public SingleLineOrigin SkipOnOrigin => SingleLineOrigin.NotSkipped;
    private FrozenDictionary<string, string> LookupDict { get; } = new Dictionary<string, string> {
        {"&", "&amp;"},
        {"<", "&lt;"},
        {">", "&gt;"},
        {"&copy;", "\u00a9"}
    }.ToFrozenDictionary();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer, SingleLineOrigin origin) {
        if (!group.TryGetValue(out string? lookupValue)) return;
        if (!LookupDict.TryGetValue(lookupValue, out string? value)) {
            logger.LogWarning("LookupDictSectionParser: Lookup value not found: {LookupValue}", lookupValue);
            return;
        }
        writer.Write(value);
    }
}
