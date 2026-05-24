# V0.1.38 Quarantine Import Prep

Purpose: prepare the primary lane to intake sidecar Unity asset packages quickly without letting package import work alter the live game by accident.

This bundle is tooling and documentation only. It does not import packages, edit `Packages/manifest.json`, touch `ProjectSettings`, change scenes, or promote sidecar assets into `Assets/_Project`.

## Outputs

- Static quarantine readiness report script: `Tools/SidecarValidation/New-QuarantineReadinessReport.ps1`
- Primary-lane quarantine import runbook: `QUARANTINE_IMPORT_RUNBOOK_v0.1.38.md`
- Batch import speed guidance: `BATCH_IMPORT_SPEED_GUIDANCE_v0.1.38.md`
- QA templates for weapon, enemy, level-kit, and feedback-FX package reviews.

## Recommended Use

Run the report before every quarantine import cycle:

```powershell
cd "D:\__MY APPS\Unity Doom"
.\Tools\SidecarValidation\New-QuarantineReadinessReport.ps1
```

Then choose one of three outcomes per package:

- `ready_for_primary_quarantine`: proceed to clean throwaway import evidence review and primary quarantine import planning.
- `needs_generation_or_remediation`: send back to the sidecar lane or run the sidecar generator in an isolated Unity project.
- `blocked_static_errors`: do not import until the package structure or manifest is fixed.

Next-step directive: continue immediately with the next highest-impact unfinished task.
