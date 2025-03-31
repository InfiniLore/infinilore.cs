// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.MarkdownWriters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IMarkdownParser>(ServiceLifetime.Singleton)]
public class MarkdownParser(IServiceProvider serviceProvider, ILogger<MarkdownParser> logger) : IMarkdownParser {
    private readonly FrozenDictionary<string, IMultiLineSectionParser> MultilineGroupToParsers = ToFrozenDictionary<IMultiLineSectionParser>(MultilineGroupNames, logger, serviceProvider);
    private readonly FrozenDictionary<string, ISingleLineSectionParser> SinglelineGroupToParsers = ToFrozenDictionary<ISingleLineSectionParser>(SinglelineGroupNames, logger, serviceProvider);

    private static ImmutableArray<string> MultilineGroupNames => [
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

    private static ImmutableArray<string> SinglelineGroupNames => [
        "escaped",
        "boldAndItalic",
        "bold",
        "italic",
        "supScript",
        "subScript",
        "strike",
        "code",
        "linkNested",
        "linkRegular",
        "script",
        "lookupDict",
        "underline",
        "emote"
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

    private static FrozenDictionary<string, T> ToFrozenDictionary<T>(ImmutableArray<string> keyNames, ILogger<MarkdownParser> logger, IServiceProvider serviceProvider) {
        int keyCount = keyNames.Length;
        var dictionaryBuilder = new Dictionary<string, T>(keyCount);

        for (int index = 0; index < keyCount; index++) {
            string groupName = keyNames[index];
            var service = serviceProvider.GetKeyedService<T>(groupName);
            if (service is null) {
                logger.LogWarning($"No service found for group name '{groupName}' for type '{typeof(T).Name}'.");
                continue;
            }

            dictionaryBuilder[groupName] = service;
        }

        return dictionaryBuilder.ToFrozenDictionary();

    }

    #region Parsing Methods
    public void ParseMultiline(string markdown, IMarkdownWriter writer) {
        List<Match> collection = MarkdownRegexLib.MultilineStructuresMatches(markdown).ToList();
        int collectionCount = collection.Count;

        for (int index = 0; index < collectionCount; index++) {
            Match match = collection[index];
            GroupCollection groups = match.Groups;
            int groupCount = groups.Count;

            for (int i = 0; i < groupCount; i++) {
                Group group = groups[i];
                if (!group.Success) continue;
                if (!MultilineGroupToParsers.TryGetValue(group.Name, out IMultiLineSectionParser? sectionParser)) continue;

                sectionParser.ParseToStringBuilder(match, group, writer);
            }
        }
    }

    public void ParseSingleline(string markdown, IMarkdownWriter writer, SingleLineOrigin origin = SingleLineOrigin.Undefined) {
        List<Match> collection = MarkdownRegexLib.SinglelineStructuresMatches(markdown).ToList();
        int collectionCount = collection.Count;

        int currentIndex = 0; // Track the position in the string we're currently at
        ReadOnlySpan<char> markdownSpan = markdown.AsSpan();

        for (int index = 0; index < collectionCount; index++) {
            Match match = collection[index];
            GroupCollection groups = match.Groups;
            int groupCount = groups.Count;

            // Add unmatched text before the current match
            if (match.Index > currentIndex) {
                ReadOnlySpan<char> unmatchedText = markdownSpan.Slice(currentIndex, match.Index - currentIndex);
                writer.Write(unmatchedText);
            }

            // Process matched text using parsers
            for (int i = 0; i < groupCount; i++) {
                Group group = groups[i];
                if (!group.Success) continue;
                if (!SinglelineGroupToParsers.TryGetValue(group.Name, out ISingleLineSectionParser? sectionParser)) continue;
                if (origin.HasFlag(sectionParser.SkipOnOrigin)) continue;

                sectionParser.ParseToStringBuilder(match, group, writer, origin);
            }

            // Update the current position to the end of the current match
            currentIndex = match.Index + match.Length;
        }

        // ReSharper disable once InvertIf
        // Append any remaining unmatched text after the last match
        if (currentIndex < markdown.Length) {
            ReadOnlySpan<char> remainingText = markdownSpan[currentIndex..];
            writer.Write(remainingText);
        }

    }
    #endregion
}
