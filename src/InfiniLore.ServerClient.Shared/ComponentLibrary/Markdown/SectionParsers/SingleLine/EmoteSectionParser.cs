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
public class EmoteSectionParser(ILogger<EmoteSectionParser> logger, IServiceProvider provider) : ISingleLineSectionParser{
    public SingleLineOrigin SkipOnOrigin => SingleLineOrigin.Emote;
    
    // TODO Requires some sort of Emote service
    private FrozenDictionary<EmoteKey, string> EmoteDict { get; } = new Dictionary<EmoteKey, string> {
        { 
            EmoteKey.FromKeys("flag-transgender", "flag-trans", "flag_trans", "flag_transgender"),
            "\ud83c\udff3\ufe0f\u200d\u26a7\ufe0f" // Transgender flag
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

public record EmoteKey(params FrozenSet<string> Keys) {
    public static EmoteKey FromKeys(params FrozenSet<string> keys) => new(keys);
}

public class EmoteKeyComparer :  IEqualityComparer<EmoteKey>, IAlternateEqualityComparer<string, EmoteKey> {
    public bool Equals(EmoteKey? x, EmoteKey? y)
        => x is not null && y is not null && x.Equals(y) ;
    
    public int GetHashCode(EmoteKey obj) 
        => obj.GetHashCode();
    
    public bool Equals(string alternate, EmoteKey other) 
        => other.Keys.Contains(alternate);
    
    public int GetHashCode(string alternate)
        => alternate.GetHashCode();
    
    public EmoteKey Create(string alternate) => throw new NotSupportedException("Cannot create registration based on partial key");
}














