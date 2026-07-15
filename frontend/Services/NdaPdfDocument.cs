using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Prelegal.Frontend.Services;

/// <summary>
/// Renders a <see cref="RenderedDocument"/> to a print-ready PDF using QuestPDF.
/// </summary>
public sealed class NdaPdfDocument : IDocument
{
    private readonly RenderedDocument _doc;

    public NdaPdfDocument(RenderedDocument doc) => _doc = doc;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(2, Unit.Centimetre);
            page.DefaultTextStyle(t => t.FontSize(11).FontFamily(Fonts.Calibri).LineHeight(1.35f));

            page.Header().PaddingBottom(12).Column(col =>
            {
                col.Item().Text(_doc.Title).Bold().FontSize(16);
                col.Item().PaddingTop(2).LineHorizontal(1).LineColor(Colors.Grey.Medium);
            });

            page.Content().Column(col =>
            {
                col.Spacing(12);
                foreach (var section in _doc.Sections)
                {
                    col.Item().Column(s =>
                    {
                        s.Item().Text(section.Heading).Bold().FontSize(12);
                        s.Item().PaddingTop(3).Text(section.Body).Justify();
                    });
                }
            });

            page.Footer().AlignCenter().Text(t =>
            {
                t.Span("Page ");
                t.CurrentPageNumber();
                t.Span(" of ");
                t.TotalPages();
            });
        });
    }
}
