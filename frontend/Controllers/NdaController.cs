using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Prelegal.Frontend.Models;
using Prelegal.Frontend.Services;
using QuestPDF.Fluent;

namespace Prelegal.Frontend.Controllers;

/// <summary>
/// Drives the Mutual NDA creator: show form → preview filled document → download PDF.
/// </summary>
public sealed class NdaController : Controller
{
    private readonly TemplateService _templates;

    public NdaController(TemplateService templates) => _templates = templates;

    /// <summary>GET / — blank form seeded with the template's declared defaults.</summary>
    [HttpGet]
    public IActionResult Index()
    {
        var template = _templates.GetMutualNda();
        ViewBag.Template = template;
        return View(NdaFormModel.FromTemplateDefaults(template));
    }

    /// <summary>POST /Nda/Preview — validate input and render the filled NDA.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Preview(NdaFormModel form)
    {
        if (TryRenderNda(form, out var rendered))
        {
            ViewBag.Form = form;
            return View(rendered);
        }

        return RedisplayForm(form);
    }

    /// <summary>POST /Nda/Download — generate and stream the filled NDA as a PDF.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Download(NdaFormModel form)
    {
        if (!TryRenderNda(form, out var rendered))
            return RedisplayForm(form);

        var pdf = new NdaPdfDocument(rendered).GeneratePdf();
        return File(pdf, "application/pdf", "Mutual-NDA.pdf");
    }

    /// <summary>Fills the template if the form is valid; returns false when validation fails.</summary>
    private bool TryRenderNda(NdaFormModel form, out RenderedDocument rendered)
    {
        if (!ModelState.IsValid)
        {
            rendered = null!;
            return false;
        }

        rendered = DocumentRenderer.Render(_templates.GetMutualNda(), form.ToValues());
        return true;
    }

    /// <summary>Re-renders the form view with the template needed for its help text.</summary>
    private IActionResult RedisplayForm(NdaFormModel form)
    {
        ViewBag.Template = _templates.GetMutualNda();
        return View(nameof(Index), form);
    }

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
    });
}
