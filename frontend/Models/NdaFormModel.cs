using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Prelegal.Frontend.Models;

/// <summary>
/// User-entered values for the Mutual NDA. Property names are PascalCase for
/// binding; <see cref="ToValues"/> maps them to the snake_case template keys
/// used by {{placeholder}} substitution.
/// </summary>
public sealed class NdaFormModel
{
    [Required(ErrorMessage = "Effective date is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Effective Date")]
    public DateTime? EffectiveDate { get; set; }

    [Required(ErrorMessage = "Party 1 legal name is required.")]
    [Display(Name = "Party 1 Legal Name")]
    public string? Party1Name { get; set; }

    [Display(Name = "Party 1 Entity Type")]
    public string? Party1EntityType { get; set; }

    [Required(ErrorMessage = "Party 1 address is required.")]
    [Display(Name = "Party 1 Address")]
    public string? Party1Address { get; set; }

    [Required(ErrorMessage = "Party 2 legal name is required.")]
    [Display(Name = "Party 2 Legal Name")]
    public string? Party2Name { get; set; }

    [Display(Name = "Party 2 Entity Type")]
    public string? Party2EntityType { get; set; }

    [Required(ErrorMessage = "Party 2 address is required.")]
    [Display(Name = "Party 2 Address")]
    public string? Party2Address { get; set; }

    [Required(ErrorMessage = "Purpose is required.")]
    [Display(Name = "Purpose")]
    public string? Purpose { get; set; }

    [Required(ErrorMessage = "Agreement term is required.")]
    [Range(1, 1200, ErrorMessage = "Term must be between 1 and 1200 months.")]
    [Display(Name = "Agreement Term (months)")]
    public int? TermMonths { get; set; }

    [Required(ErrorMessage = "Confidentiality period is required.")]
    [Range(1, 1200, ErrorMessage = "Confidentiality period must be between 1 and 1200 months.")]
    [Display(Name = "Confidentiality Period (months)")]
    public int? ConfidentialityPeriodMonths { get; set; }

    [Required(ErrorMessage = "Governing law is required.")]
    [Display(Name = "Governing Law")]
    public string? GoverningLaw { get; set; }

    /// <summary>Maps entered values to the template's variable keys for substitution.</summary>
    public IReadOnlyDictionary<string, string> ToValues() => new Dictionary<string, string>
    {
        ["effective_date"] = EffectiveDate?.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture) ?? "",
        ["party_1_name"] = Party1Name?.Trim() ?? "",
        ["party_1_entity_type"] = Blank(Party1EntityType, "corporation"),
        ["party_1_address"] = Party1Address?.Trim() ?? "",
        ["party_2_name"] = Party2Name?.Trim() ?? "",
        ["party_2_entity_type"] = Blank(Party2EntityType, "corporation"),
        ["party_2_address"] = Party2Address?.Trim() ?? "",
        ["purpose"] = Purpose?.Trim() ?? "",
        ["term_months"] = TermMonths?.ToString(CultureInfo.InvariantCulture) ?? "",
        ["confidentiality_period_months"] = ConfidentialityPeriodMonths?.ToString(CultureInfo.InvariantCulture) ?? "",
        ["governing_law"] = GoverningLaw?.Trim() ?? "",
    };

    /// <summary>Builds a form pre-seeded with the template's declared defaults.</summary>
    public static NdaFormModel FromTemplateDefaults(NdaTemplate template)
    {
        string? Def(string key) => template.Variables.FirstOrDefault(v => v.Key == key)?.Default;

        return new NdaFormModel
        {
            EffectiveDate = DateTime.Today,
            Party1EntityType = Def("party_1_entity_type"),
            Party2EntityType = Def("party_2_entity_type"),
            Purpose = Def("purpose"),
            TermMonths = ParseInt(Def("term_months")),
            ConfidentialityPeriodMonths = ParseInt(Def("confidentiality_period_months")),
            GoverningLaw = Def("governing_law"),
        };
    }

    private static string Blank(string? value, string fallback)
        => string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

    private static int? ParseInt(string? s)
        => int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) ? n : null;
}
