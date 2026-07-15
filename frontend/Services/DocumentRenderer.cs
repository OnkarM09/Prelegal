using System.Text.RegularExpressions;
using Prelegal.Frontend.Models;

namespace Prelegal.Frontend.Services;

/// <summary>A template section with its {{placeholders}} resolved to values.</summary>
public sealed record RenderedSection(string Heading, string Body);

/// <summary>The full filled document, ready to display or export.</summary>
public sealed record RenderedDocument(string Title, IReadOnlyList<RenderedSection> Sections);

/// <summary>
/// Fills a template's {{variable_key}} placeholders with user-supplied values.
/// Substitution logic is shared by the HTML preview and the PDF export so the
/// downloaded document always matches what the user previewed.
/// </summary>
public static class DocumentRenderer
{
    // Matches {{ key }} with optional surrounding whitespace; key is snake_case.
    private static readonly Regex Placeholder = new(@"\{\{\s*(?<key>[a-z0-9_]+)\s*\}\}", RegexOptions.Compiled);

    public static RenderedDocument Render(NdaTemplate template, IReadOnlyDictionary<string, string> values)
    {
        var sections = template.Sections
            .Select(s => new RenderedSection(s.Heading, Fill(s.Body, values)))
            .ToList();

        return new RenderedDocument(template.Name, sections);
    }

    /// <summary>Replaces every {{key}} in <paramref name="text"/>. Unknown keys are left intact.</summary>
    private static string Fill(string text, IReadOnlyDictionary<string, string> values)
        => Placeholder.Replace(text, match =>
        {
            var key = match.Groups["key"].Value;
            return values.TryGetValue(key, out var value) ? value : match.Value;
        });
}
