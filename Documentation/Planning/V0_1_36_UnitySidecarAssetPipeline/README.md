# V0.1.36 Unity Sidecar Asset-Pack Pipeline

Date: 2026-05-24
Package state: docs-only architecture and decision bundle
Scope: sidecar Unity project strategy, import boundaries, resource assessment, lane recommendations, and acceptance gates

This bundle answers whether separate Unity projects and Unity Editor instances can produce importable asset packs while the primary game lane continues. Recommendation: yes, but only for visual/content packs with strict package boundaries and a staged import gate. Sidecars should accelerate asset production, lookdev, prefab assembly, previews, and smoke import tests; they should not own gameplay scripts, scenes, build settings, validators, or primary-project package changes.

## Owned Scope

- `Documentation/Planning/V0_1_36_UnitySidecarAssetPipeline/`
- `Documentation/AssetProduction/V0_1_36_UnitySidecarAssetPipeline/`

## Files

- `SIDECAR_ARCHITECTURE_v0.1.36.md` - concrete project layout, package boundaries, export/import methods, versioning, manifests, and ownership rules.
- `RESOURCE_SPEED_ASSESSMENT_v0.1.36.md` - practical speedups, slowdowns, machine limits, Library cache costs, Unity license/session risks, merge risks, and lane count guidance.
- `RECOMMENDED_FIRST_LANES_v0.1.36.md` - first sidecar lane candidates and recommended start order for this project.
- `ACCEPTANCE_GATES_AND_SMOKE_IMPORT_v0.1.36.md` - import gates, smoke validation, stop conditions, and promotion steps before primary-project intake.
- Asset-production handoff files under `Documentation/AssetProduction/V0_1_36_UnitySidecarAssetPipeline/`.

## Decision Summary

- Use sidecars for asset-only packs that can be imported as staged content under a single root folder.
- Prefer UPM-style package layout for clean boundaries and repeatable import review, with `.unitypackage` snapshots only as human-friendly handoff artifacts.
- Avoid direct staged folder copy into the primary repo until a pack has passed sidecar import smoke checks and primary-lane approval.
- Start with two sidecar lanes, then move to three only if RAM, disk, import time, and reviewer bandwidth remain healthy.
- Keep the primary game lane authoritative for gameplay prefabs, scene placement, scripts, packages, project settings, validators, builds, and release status.

## Guardrails

- No scripts.
- No scenes.
- No validators.
- No build settings.
- No package files.
- No generated builds.
- No shared status, ledger, release, or session files.
- No Unity project scaffold in this pass.
