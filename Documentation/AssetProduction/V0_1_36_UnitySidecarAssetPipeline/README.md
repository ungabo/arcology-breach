# V0.1.36 Unity Sidecar Asset-Pack Pipeline Handoff

Date: 2026-05-24
Package state: docs-only asset production handoff

This folder contains production-facing templates and handoff notes for future Unity sidecar asset-pack lanes. No sidecar Unity project has been created in this pass.

## Files

- `SIDECAR_PACK_MANIFEST_TEMPLATE_v0.1.36.json` - manifest schema example for future sidecar pack exports.
- `SIDECAR_IMPORT_REPORT_TEMPLATE_v0.1.36.md` - import/smoke report template for clean throwaway import and primary quarantine review.
- `SIDECAR_HANDOFF_CHECKLIST_v0.1.36.md` - concise checklist for workers preparing a sidecar asset pack.

## Handoff Rule

Future sidecar output should not be imported into the primary Unity project until it has:

- One isolated package root.
- A completed manifest.
- A completed import report.
- Clean throwaway import proof.
- Primary-lane approval for quarantine import.
