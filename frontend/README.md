# Prelegal Frontend — Mutual NDA Creator

ASP.NET Core MVC web app that turns the shared **Mutual NDA** template
(`data/templates/mutual-nda.json`) into a finished document. The user fills a
form, previews the filled agreement, and downloads it as a PDF.

Implements Jira **KAN-6** (*Prototype of Mutual NDA creator*).

## How it works

```
GET  /                → form seeded with the template's default values
POST /Nda/Preview     → validate input, fill {{placeholders}}, show the NDA
POST /Nda/Download    → render the filled NDA to PDF (QuestPDF) and stream it
```

- **Template is the single source of truth.** `TemplateService` reads
  `data/templates/mutual-nda.json` at runtime (path is configurable via the
  `TemplatesPath` setting) and caches it. The dataset is not duplicated here.
- **One fill path.** `DocumentRenderer` substitutes `{{variable_key}}`
  placeholders once; both the HTML preview and the PDF use its output, so the
  download always matches the preview.
- **Typed form + validation.** `NdaFormModel` binds the 11 template variables
  with DataAnnotations; jQuery unobtrusive validation runs client-side.

## Project layout

| Path | Purpose |
| --- | --- |
| `Models/NdaTemplate.cs` | DTOs mirroring `data/templates/schema.json` |
| `Models/NdaFormModel.cs` | Bound form fields → template-key value map |
| `Services/TemplateService.cs` | Loads + caches the template JSON |
| `Services/DocumentRenderer.cs` | Fills `{{placeholders}}` |
| `Services/NdaPdfDocument.cs` | QuestPDF layout for the PDF export |
| `Controllers/NdaController.cs` | Form → preview → download flow |
| `Views/Nda/*.cshtml` | Bootstrap form + rendered document |

## Run

```bash
cd frontend
dotnet run
```

Then open the URL printed by Kestrel (e.g. `https://localhost:5001`).

## Configuration

`appsettings.json`:

```json
{ "TemplatesPath": "../data/templates" }
```

Resolved relative to the app content root. Point it elsewhere to load templates
from another location.

## Notes

- **Tech:** ASP.NET Core MVC (net9.0), Bootstrap 5, jQuery, QuestPDF (Community license).
- Output is an informational prototype, **not legal advice** — have an attorney
  review any document before use.
