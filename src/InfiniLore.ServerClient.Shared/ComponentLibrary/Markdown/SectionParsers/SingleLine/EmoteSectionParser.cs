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
[KeyedInjectableService<ISingleLineSectionParser>("emote", ServiceLifetime.Singleton)]
public class EmoteSectionParser(ILogger<EmoteSectionParser> logger) : ISingleLineSectionParser{
    public SingleLineOrigin SkipOnOrigin => SingleLineOrigin.Emote;
    
    // TODO Requires some sort of Emote service
    private FrozenDictionary<EmoteKey, string> EmoteDict { get; } = new Dictionary<EmoteKey, string> {
        { 
            EmoteKey.FromKeys("flag-transgender", "flag-trans", "flag_trans", "flag_transgender"),
            "\ud83c\udff3\ufe0f\u200d\u26a7\ufe0f" // Transgender flag,
        },
        {
            EmoteKey.FromKeys("li-signature"),
            "<span style=\"display: inline-block;vertical-align: middle;\"><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" class=\"lucide lucide-signature-icon lucide-signature\"><path d=\"m21 17-2.156-1.868A.5.5 0 0 0 18 15.5v.5a1 1 0 0 1-1 1h-2a1 1 0 0 1-1-1c0-2.545-3.991-3.97-8.5-4a1 1 0 0 0 0 5c4.153 0 4.745-11.295 5.708-13.5a2.5 2.5 0 1 1 3.31 3.284\"/><path d=\"M3 21h18\"/></svg></span>" // Transgender flag
        }
    }.ToFrozenDictionary(comparer:new EmoteKeyComparer());
    
    private FrozenDictionary<EmoteKey, string>.AlternateLookup<string>? _emoteLookup;
    private FrozenDictionary<EmoteKey, string>.AlternateLookup<string> EmoteLookup => _emoteLookup ??= EmoteDict.GetAlternateLookup<string>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer, SingleLineOrigin origin) {
        if (!entireMatch.Groups["eText"].TryGetValue(out string? lookupValue)) return;
        if (!EmoteLookup.TryGetValue(lookupValue, out string? value)) {
            logger.LogWarning("Lookup emote not found: {LookupValue}", lookupValue);
            writer.Write(group.Value);
            return;
        }
        writer.Write(value);
    }
}
