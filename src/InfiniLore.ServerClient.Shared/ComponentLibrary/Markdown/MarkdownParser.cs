// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IMarkdownParser>(ServiceLifetime.Singleton)]
public class MarkdownParser(IParserSwitcher parserSwitcher) : IMarkdownParser {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public string Parse(string markdown)
        => parserSwitcher.ParseMultiline(markdown);
    

    private static string SinglelineStructuresEvaluator(Match match, SingleLineOrigin origin = SingleLineOrigin.Undefined) {
        // TODO CONTINUE FROM HERE!!!!
        if (!origin.HasFlag(SingleLineOrigin.Bold)
            && match.Groups["bold"].Success
            && match.Groups["bText"].TryGetValue(out string? boldValue)
        ) {
            string output = MarkdownRegexLib.SinglelineStructuresRegex.Replace(boldValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | SingleLineOrigin.Bold));
            return $"<strong>{output}</strong>";
        }

        if (!origin.HasFlag(SingleLineOrigin.Italic)
            && match.Groups["italic"].Success
            && match.Groups["iText"].TryGetValue(out string? italicValue)
        ) {
            string output = MarkdownRegexLib.SinglelineStructuresRegex.Replace(italicValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | SingleLineOrigin.Italic));
            return $"<em>{output}</em>";
        }

        if (!origin.HasFlag(SingleLineOrigin.Strike)
            && match.Groups["strike"].Success
            && match.Groups["sText"].TryGetValue(out string? strikeValue)
        ) {
            string output = MarkdownRegexLib.SinglelineStructuresRegex.Replace(strikeValue, evaluator: m => SinglelineStructuresEvaluator(m, origin | SingleLineOrigin.Strike));
            return $"<s>{output}</s>";

        }

        if (!origin.HasFlag(SingleLineOrigin.Code)
            && match.Groups["code"].Success
            && match.Groups["codeText"].TryGetValue(out string? codeValue)
        ) {
            string normalizedBackticks = codeValue.Replace("\\`", "`");
            string output = HtmlEncoder.Default.Encode(normalizedBackticks);
            return $"<code>{output}</code>";
        }

        if (match.Groups["linkNested"].Success
            && match.Groups["lnText"].TryGetValue(out string? linkText)
            && match.Groups["lnHref"].TryGetValue(out string? linkHref)
        ) {
            string titleText = match.Groups["lnTitle"].TryGetValue(out string? altTextValue) ? $" title=\"{altTextValue}\"" : string.Empty;

            if (match.Groups["lnBang"].Success) {
                return $"<img src=\"{linkHref}\" alt=\"{linkText}\"{titleText}>";
            }

            string output = MarkdownRegexLib.SinglelineStructuresRegex.Replace(linkText, evaluator: m => SinglelineStructuresEvaluator(m, origin));
            return $"<a href=\"{linkHref}\">{output}</a>";
        }

        if (!origin.HasFlag(SingleLineOrigin.Link) && match.Groups["linkRegular"].Success
            && match.Groups["lrText"].TryGetValue(out string? linkRegularText)
            && match.Groups["lrHref"].TryGetValue(out string? linkRegularHref)
        ) {
            string titleText = match.Groups["lrTitle"].TryGetValue(out string? altTextValue) ? $" title=\"{altTextValue}\"" : string.Empty;

            if (match.Groups["lrBang"].Success) {
                return $"<img src=\"{linkRegularHref}\" alt=\"{linkRegularText}\"{titleText}>";
            }

            string output = MarkdownRegexLib.SinglelineStructuresRegex.Replace(linkRegularText, evaluator: m => SinglelineStructuresEvaluator(m, origin | SingleLineOrigin.Link));
            return $"<a href=\"{linkRegularHref}\">{output}</a>";
        }

        if (match.Groups["copyright"].Success) {
            return "\u00a9";
        }

        if (match.Groups["amp"].Success) {
            return "&amp;";
        }

        if (match.Groups["script"].TryGetValue(out string? scriptBody)) {
            string output = HtmlEncoder.Default.Encode(scriptBody);
            return output;
        }

        if (match.Groups["lessThan"].Success) {
            return "&lt;";
        }

        if (match.Groups["greaterThan"].Success) {
            return "&gt;";
        }

        return match.Value;
    }

    
}
