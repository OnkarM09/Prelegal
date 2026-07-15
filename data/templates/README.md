# Legal Document Template Dataset

A machine-readable dataset of legal document templates. Each template is a JSON
document with **placeholder variables** the Prelegal system can fill or let a
user modify, and **sections** that make up the document body.

> ⚠️ These templates are informational starting points, not legal advice. Have a
> qualified attorney review any document before use.

## Layout

| File | Purpose |
| --- | --- |
| `schema.json` | JSON Schema (draft-07) every template validates against |
| `index.json` | Catalog of all templates (id, name, category, file) |
| `*.json` | One template per file |

## Template model

Each template file has:

- **Metadata** — `id`, `name`, `category`, `version`, `jurisdiction`, `source`, `license`.
- **`variables`** — the fields the system fills or the user edits. Each has a
  `key`, `label`, `type`, `required` flag, optional `description`, `default`, and
  (for `select`) `options`.
- **`sections`** — ordered document body. Section `body` text embeds
  placeholders as `{{variable_key}}`, matching a `variables[].key`.

### Filling a template

Replace every `{{key}}` in each section's `body` with the value for that `key`.
Example: `{{party_1_name}}` → `Acme, Inc.`

## Templates

| ID | Name | Category |
| --- | --- | --- |
| `mutual-nda` | Mutual Non-Disclosure Agreement (MNDA) | Confidentiality |
| `unilateral-nda` | One-Way (Unilateral) Non-Disclosure Agreement | Confidentiality |
| `independent-contractor-agreement` | Independent Contractor Agreement | Services |
| `service-agreement` | Master Services Agreement (MSA) | Services |
| `employment-offer-letter` | Employment Offer Letter | Employment |

## Adding a template

1. Create `data/templates/<id>.json` conforming to `schema.json`.
2. Add an entry to `index.json` and bump its `count`.
3. Keep placeholder keys and `variables[].key` in sync.

## License

The Mutual NDA is derived from the [Common Paper MNDA](https://commonpaper.com/standards/mutual-NDA/1.0),
licensed under [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/). All
templates in this dataset are distributed under CC BY 4.0.
