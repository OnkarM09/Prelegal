using System.Text.Json.Serialization;

namespace Prelegal.Frontend.Models;

/// <summary>
/// In-memory representation of a legal document template file
/// (data/templates/*.json), matching data/templates/schema.json.
/// Only the fields the frontend needs are mapped.
/// </summary>
public sealed class NdaTemplate
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Version { get; set; } = "";
    public string? Jurisdiction { get; set; }
    public string? Description { get; set; }
    public string License { get; set; } = "";
    public TemplateSource? Source { get; set; }
    public List<TemplateVariable> Variables { get; set; } = new();
    public List<TemplateSection> Sections { get; set; } = new();
}

public sealed class TemplateSource
{
    public string? Name { get; set; }
    public string? Url { get; set; }
}

/// <summary>A single fillable field. Maps to a {{key}} placeholder in sections.</summary>
public sealed class TemplateVariable
{
    public string Key { get; set; } = "";
    public string Label { get; set; } = "";

    /// <summary>text, textarea, date, number, currency, email, select, boolean.</summary>
    public string Type { get; set; } = "text";
    public bool Required { get; set; }
    public string? Description { get; set; }

    /// <summary>Optional default; may be a string or number in the JSON.</summary>
    [JsonConverter(typeof(ScalarStringConverter))]
    public string? Default { get; set; }

    public List<string>? Options { get; set; }
}

/// <summary>An ordered document section whose body may embed {{key}} placeholders.</summary>
public sealed class TemplateSection
{
    public string Id { get; set; } = "";
    public string Heading { get; set; } = "";
    public string Body { get; set; } = "";
}
