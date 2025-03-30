// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.MarkdownWriters;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Frozen;
using System.Text;
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

    private static readonly string[] MultilineGroupNames = [
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

    private static readonly string[] SinglelineGroupNames = [
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
    public string ParseMultiline(string markdown) {
        StringBuilderMarkdownWriter writer = StringBuilderMarkdownWriterPool.Get();
        try {
            ParseMultiline(markdown, writer);
            return writer.ToString();
        }
        finally {
            StringBuilderMarkdownWriterPool.Return(writer);
        }
    }

    public void ParseMultiline(string markdown, IMarkdownWriter writer) {
        MatchCollection collection = MarkdownRegexLib.MultilineStructuresMatches(markdown);
        for (int index = 0; index < collection.Count; index++) {
            Match match = collection[index];
            foreach ((string groupName, IMultiLineSectionParser sectionParser) in MultilineGroupToParserDictionary) {
                if (match.Groups[groupName] is not { Success: true } group) continue;

                sectionParser.ParseToStringBuilder(match, group, writer);
            }
        }
    }

    public string ParseSingleline(string markdown) {
        StringBuilderMarkdownWriter writer = StringBuilderMarkdownWriterPool.Get();
        try {
            ParseSingleline(markdown, writer);
            return writer.ToString();
        }
        finally {
            StringBuilderMarkdownWriterPool.Return(writer);
        }
    }

    public void ParseSingleline(string markdown, IMarkdownWriter writer, SingleLineOrigin origin = SingleLineOrigin.Undefined) {
        MatchCollection collection = MarkdownRegexLib.SinglelineStructuresMatches(markdown);
        int currentIndex = 0;// Track the position in the string we're currently at
        ReadOnlySpan<char> markdownSpan = markdown.AsSpan();

        for (int index = 0; index < collection.Count; index++) {
            Match match = collection[index];

            // Add unmatched text before the current match
            if (match.Index > currentIndex) {
                ReadOnlySpan<char> unmatchedText = markdownSpan.Slice(currentIndex, match.Index - currentIndex);
                writer.Write(unmatchedText);
            }

            // Process matched text using parsers
            foreach ((string groupName, ISingleLineSectionParser sectionParser) in SinglelineGroupToParserDictionary) {
                if (origin.HasFlag(sectionParser.SkipOnOrigin)) continue;
                if (match.Groups[groupName] is not { Success: true } group) continue;

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
}
