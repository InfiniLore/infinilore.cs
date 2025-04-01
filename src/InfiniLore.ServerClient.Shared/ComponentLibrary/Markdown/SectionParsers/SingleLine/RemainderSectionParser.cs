// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Frozen;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.SingleLine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<ISingleLineSectionParser>("remainder", ServiceLifetime.Singleton)]
public class RemainderSectionParser : ISingleLineSectionParser{
    public SingleLineOrigin SkipOnOrigin => SingleLineOrigin.NotSkipped;
    private static readonly FrozenDictionary<string, string> LookupDict = new Dictionary<string, string>(5) {
        { "&copy;", "\u00a9" },
        { "<br/>", "<br/>" },
        { "&", "&amp;" },
        { "<", "&lt;" },
        { ">", "&gt;" }
    }.ToFrozenDictionary();
    private static readonly FrozenDictionary<string, string>.AlternateLookup<ReadOnlySpan<char>> AlternateLookup = LookupDict.GetAlternateLookup<ReadOnlySpan<char>>();
    private Regex LookupDictRegex { get; } = new(string.Join('|', LookupDict.Keys), RegexOptions.Compiled | RegexOptions.Singleline);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Obsolete("This parser is not meant to be used for direct single line parsing. Use ParseToStringBuilder(ref ReadOnlySpan<char> remainder, IMarkdownWriter writer) instead.")]
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer, SingleLineOrigin origin) {
        throw new NotSupportedException("This parser is not meant to be used for direct single line parsing. Use ParseToStringBuilder(ref ReadOnlySpan<char> remainder, IMarkdownWriter writer) instead.");
    }

    public void ParseToStringBuilder(ref ReadOnlySpan<char> remainder, IMarkdownWriter writer) {
        if (remainder.IsEmpty) return;
        int currentIndex = 0;

        foreach (ValueMatch match in LookupDictRegex.EnumerateMatches(remainder)) {
            int matchIndex = match.Index;
            int matchLength = match.Length;
            if (currentIndex < matchIndex) {
                writer.Write(remainder.Slice(currentIndex, matchIndex - currentIndex));
            }
            if (AlternateLookup.TryGetValue(remainder.Slice(matchIndex, matchLength), out string? replacement)) {
                writer.Write(replacement);
            }
            currentIndex = matchIndex + matchLength;
        }
        if (currentIndex < remainder.Length) {
            writer.Write(remainder[currentIndex..]);
        }
    }

}
