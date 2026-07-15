# Prelegal

Prelegal turns machine-readable legal document templates into finished,
downloadable documents. A user fills in a few key fields and gets a completed
agreement — currently a **Mutual NDA**.

> ⚠️ Output is an informational prototype, **not legal advice**. Have a
> qualified attorney review any document before use.

## Status

🚧 **In progress** — under active development.

Delivered:

- ✅ **KAN-5** — legal document template dataset (`data/templates/`)
- ✅ **KAN-6** — Mutual NDA creator web app (`frontend/`)

## Components

| Path | What |
| --- | --- |
| `data/templates/` | Machine-readable legal document template dataset (KAN-5) |
| `frontend/` | ASP.NET Core MVC app — Mutual NDA creator (KAN-6). See [frontend/README.md](frontend/README.md) |

## Quick start

Run the Mutual NDA creator locally (requires the .NET SDK):

```bash
cd frontend
dotnet run
```

Open the URL Kestrel prints, fill the form, preview the NDA, and download it as
a PDF. See [frontend/README.md](frontend/README.md) for details.
