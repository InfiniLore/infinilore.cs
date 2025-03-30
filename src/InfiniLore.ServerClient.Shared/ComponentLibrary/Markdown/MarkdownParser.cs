// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.MarkdownWriters;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Frozen;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IMarkdownParser>(ServiceLifetime.Singleton)]
public class MarkdownParser(IServiceProvider serviceProvider) : IMarkdownParser {
    private FrozenDictionary<string, IMultiLineSectionParser> MultilineGroupToParserDictionary { get; } = MultilineGroupNames
        .Select(groupName => (GroupName: groupName, Service: serviceProvider.GetKeyedService<IMultiLineSectionParser>(groupName)))
        .Where(tuple => tuple.Service is not null)
        .ToFrozenDictionary(keySelector: tuple => tuple.GroupName, elementSelector: tuple => tuple.Service!);

    private FrozenDictionary<string, ISingleLineSectionParser> SinglelineGroupToParserDictionary { get; } = SinglelineGroupNames
        .Select(groupName => (GroupName: groupName, Service: serviceProvider.GetKeyedService<ISingleLineSectionParser>(groupName)))
        .Where(tuple => tuple.Service is not null)
        .ToFrozenDictionary(keySelector: tuple => tuple.GroupName, elementSelector: tuple => tuple.Service!);

    private static readonly FrozenSet<string> MultilineGroupNames = [
        "remainder",
        "heading",
        "codeBlock",
        "headingSimple",
        "listUnordered",
        "listOrdered",
        "table",
        "blockQuote",
        "htmlBody",
        "horizontalRule"
    ];

    private static readonly FrozenSet<string> SinglelineGroupNames = [
        "escaped",
        "boldAndItalic",
        "bold",
        "italic",
        "strike",
        "code",
        "linkNested",
        "linkRegular",
        "copyright",
        "amp",
        "script",
        "lessThan",
        "greaterThan",
        "remainder"
    ];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public string Parse(string markdown) {
        StringBuilderMarkdownWriter writer = StringBuilderMarkdownWriterPool.Get();
        try {
            ParseMultiline(markdown, writer);
            return writer.ToString();
        }
        finally {
            StringBuilderMarkdownWriterPool.Return(writer);
        }
    }
    
    public void Parse<T>(string markdown, T writer) where T : TextWriter {
        var markdownWriter = new TextWriterMarkdownWriter<T>(writer);
        ParseMultiline(markdown, markdownWriter);
    }
    
    #region Parsing Methods
    public void ParseMultiline(string markdown, IMarkdownWriter writer) {
        var collection = MarkdownRegexLib.MultilineStructuresMatches(markdown).ToList();
        for (int index = 0; index < collection.Count; index++) {
            Match match = collection[index];
            var groups = match.Groups;
            var groupCount = groups.Count;

            for (int i = 0; i < groupCount; i++) {
                Group group = groups[i];
                if (!group.Success) continue;
                if (!MultilineGroupToParserDictionary.TryGetValue(group.Name, out IMultiLineSectionParser? sectionParser)) continue;

                sectionParser.ParseToStringBuilder(match, group, writer);
            }
        }
    }
    
    public void ParseSingleline(string markdown, IMarkdownWriter writer, SingleLineOrigin origin = SingleLineOrigin.Undefined) {
        var collection = MarkdownRegexLib.SinglelineStructuresMatches(markdown).ToList();
        int currentIndex = 0;// Track the position in the string we're currently at
        ReadOnlySpan<char> markdownSpan = markdown.AsSpan();

        for (int index = 0; index < collection.Count; index++) {
            Match match = collection[index];
            var groups = match.Groups;
            var groupCount = groups.Count;

            // Add unmatched text before the current match
            if (match.Index > currentIndex) {
                ReadOnlySpan<char> unmatchedText = markdownSpan.Slice(currentIndex, match.Index - currentIndex);
                writer.Write(unmatchedText);
            }

            // Process matched text using parsers
            for (int i = 0; i < groupCount; i++) {
                Group group = groups[i];
                if (!group.Success) continue;
                if (!SinglelineGroupToParserDictionary.TryGetValue(group.Name, out ISingleLineSectionParser? sectionParser)) continue;
                if (origin.HasFlag(sectionParser.SkipOnOrigin)) continue;

                sectionParser.ParseToStringBuilder(match, group, writer, origin);
            }

            // Update the current position to the end of the current match
            currentIndex = match.Index + match.Length;
        }

        // Append any remaining unmatched text after the last match
        if (currentIndex < markdown.Length) {
            ReadOnlySpan<char> remainingText = markdownSpan[currentIndex..];
            writer.Write(remainingText);
        }

    }
    #endregion
}
