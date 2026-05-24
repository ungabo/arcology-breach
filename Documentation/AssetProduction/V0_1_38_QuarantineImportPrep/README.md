# V0.1.38 Quarantine Import Prep Asset-Production Notes

Purpose: define what sidecar asset producers must provide so the primary lane can quarantine-import large batches without slowing down.

## Required Sidecar Package Evidence

Every package should include:

- `package.json`
- `README.md`
- `CHANGELOG.md`
- `Documentation~/Manifest/*.json`
- Runtime prefabs, materials, meshes, textures, audio, or VFX under `Runtime/`
- Preview content under `Samples~` or external concept renders under `Documentation/ConceptRenders/`
- Known risks and rollback path in the manifest

## Manifest Lists Used By The v0.1.38 Report

The report script reads these manifest arrays when present:

- `generated_prefabs`
- `generated_materials`
- `generated_meshes`
- `preview_renders`
- `prefabs[].path` as a fallback for level-kit packages

It compares them with files on disk and reports warnings for:

- Missing expected files.
- Placeholder references such as `generated_by_*`.
- Expected count greater than observed disk count.
- Missing package-local manifests.

## Producer Notes

- Keep generated assets inside the package root.
- Prefer package-local manifests over shared docs-only manifests.
- Use stable lane prefixes on every generated asset.
- Preserve `.meta` files for generated Unity assets.
- Put preview renders in documentation-only folders so they are not shipped accidentally.
- Do not request main project setting changes from a sidecar package.

Next-step directive: continue immediately with the next highest-impact unfinished task.
