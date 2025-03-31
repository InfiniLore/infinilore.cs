// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static partial class MarkdownRegexLib {
    [GeneratedRegex("""
          (?<escaped>\\[!"\#$%&'()*+,-./:;<=>?@[\\\]^_`{|}~])
        | (?<boldAndItalic>(?<bi>\*\*\*)(?<biText>.+?)(?<!\\)\k<bi>)
        | (?<bold>(?<b>\*\*)(?<bText>.+?(?:(?<iNested>\*|_)[^*]+?\k<iNested>)?)(?<!\\)\k<b>)
        | (?<italic>(?<i>\*)(?<iText>.+?)(?<!\\)\k<i>)
        | (?<strike>~~(?<sText>.+?)~~)
        | (?<underline>(?<u>_)(?<uText>.+?)(?<!\\)\k<u>)
        | (?<code>(?<open>`+)(?<codeText>(?>[^`\\]+|\\.|`(?!\k<open>))*?)\k<open>)
        | (?<emote>(?<e>:)(?<eText>[a-zA-Z0-9-_]+?)\k<e>)
        | (?<linkNested>
            (?<lnBang>!)?
            \[(?<lnText>!?\[.+?\]\(.+?\))\]
            \((?<lnHref>.+?)(?:\s?"(?<lnTitle>[^"]*)")?\)
          )
        | (?<linkRegular>
            (?<lrBang>!)?
            \[(?<lrText>[^\]]+?)\]
            \((?<lrHref>[^\)]+?)(?:\s?"(?<lrTitle>[^"]*)")?\)
          )
        | (?<script><script.*?>[\w\s\D]*?</script>)
        | (?<lookupDict>
            &copy;
            | &
            | <
            | >
        )
        """, RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex SinglelineStructuresRegex { get; }

    [GeneratedRegex("""
          (?<heading>^(?<hLevel>\#{1,6})\s(?<hText>.+))
        | (?<codeBlock>```(?<cLang>.+?)?\r?\n+?(?<cBody>[\s\S]+?)```\s*?$)
        | (?<headingSimple>^(?<hsText>.+?)\r?\n[\ ]*[-=]{3,})
        | (?<listUnordered>(?:^[^\S\r\n]*-\s+.+(?:(?:\n[^\S\r\n]*[-.]\d*\.?\s+.+)|(?:\n[^\S\r\n]+.+))*(?:[^\S\r\n]{0,2}(?![\r\n]))?)+)
        | (?<listOrdered>(?:^[^\S\r\n]*[-.]?\d+\.?\s+.+(?:(?:\n[^\S\r\n]*[-.]?\d+\.?\s+.+)|(?:\n[^\S\r\n]+.+))*(?:[^\S\r\n]{0,2}(?![\r\n]))?)+)
        | (?<table>
            ^\|(?<tHead>.+)\|\s*\r?\n
            ^\|(?<tSep>[:\-|\ ]+)\|\s*\r?\n
            (?<tBody>(?:^\|.+\|\s*)+)
          )
        | (?<blockQuote>^>\s+.+(?:\r?\n(?![*+-]\s|[-.]?\d|\s*[^>]).+)*)$
        | (?<htmlBody>
            <(?<tag>\w+)\b[^>]*>
            (?:
              [^<]+
              | <(?<open>\k<tag>)\b[^>]*>
              | </(?<-open>\k<tag>)>
              | <(?!/?\k<tag>\b)[^>]+>
            )*
            # (?:(?<-open>)(?!)) # This should be a valuable check, but it doesnt work for me
            </\k<tag>>
          )  
        | (?<horizontalRule>^[*-_]{3,}\s*$)
        | (?<remainder>.+?(?:\r?\n|$))
        """, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex MultilineStructuresRegex { get; }

    [GeneratedRegex(@"^[ ]*[-.]?\d*\.?\s+(?<lTask>\[[\ xX]\])?(?<lHead>.+)(?<lBody>(?:\n[ ]+.+)*)", RegexOptions.Multiline | RegexOptions.ExplicitCapture)]
    public static partial Regex ListItemBodyRegex { get; }

    [GeneratedRegex("^>", RegexOptions.Multiline)]
    public static partial Regex NormalizeBlockQuoteRegex { get; }

    [GeneratedRegex("\r?\n")]
    public static partial Regex NormalizeNewlinesRegex { get; }

    public static MatchCollection SinglelineStructuresMatches(string markdown) => SinglelineStructuresRegex.Matches(markdown);
    public static MatchCollection MultilineStructuresMatches(string markdown) => MultilineStructuresRegex.Matches(markdown);
}
