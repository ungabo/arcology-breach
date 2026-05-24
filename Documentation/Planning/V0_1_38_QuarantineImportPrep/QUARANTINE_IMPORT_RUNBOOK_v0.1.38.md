# V0.1.38 Quarantine Import Runbook

Purpose: let the main Unity lane import sidecar output at speed while keeping rollback simple and preserving the primary game project.

## Preflight

Required before any primary-project import:

- Run `Tools/SidecarValidation\Test-SidecarAssetPacks.ps1`.
- Run `Tools/SidecarValidation\New-QuarantineReadinessReport.ps1`.
- Read the package-local manifest under `AssetPacks/<Package>/Documentation~/Manifest/`.
- Confirm package decision is not `blocked_static_errors`.
- Confirm the pack can be removed by deleting one isolated quarantine root.
- Confirm no package asks to edit `Packages/manifest.json`, `ProjectSettings`, generated scenes, build output, or core scripts.

## Clean Throwaway Import

Use a disposable Unity project matching the main editor version.

1. Open the throwaway project with no extra packages beyond Unity defaults.
2. Add the sidecar package as a local package or embedded copy.
3. Let Unity complete import and script compilation.
4. Run the package generator if the manifest contains placeholders or planned counts.
5. Open package preview content under `Samples~` if present.
6. Instantiate every runtime prefab candidate in a blank scene.
7. Check missing scripts, missing materials, missing meshes, missing textures, missing audio, and broken VFX references.
8. Capture preview renders/contact sheets when the package is visual.
9. Fill the matching v0.1.38 QA template.

Pass criteria:

- No blocking console errors.
- All runtime prefab candidates instantiate.
- All expected assets in the manifest exist on disk after generation.
- Dependencies are either built into Unity or already approved for the primary project.
- Rollback is delete-only.

## Primary Quarantine Copy

Only after static report and throwaway import are acceptable:

1. Confirm current git status so unrelated edits are understood.
2. Copy package output into a single quarantine root:

```text
Assets/_Project/ArtStaging/SidecarImports/<PackageName>/
```

3. Do not overwrite existing assets by path.
4. Do not edit package/project settings.
5. Let Unity import.
6. Save quarantine QA notes in `Documentation/QA/`.
7. If any file changes outside the quarantine root or QA report, stop and investigate before proceeding.

## Preview Scene Open

Review options, in preferred order:

- Existing editor-only review scene if one exists.
- Temporary local review scene that is not committed.
- Blank scene created only for QA and deleted after review.

Checks:

- Open prefabs in Prefab Mode.
- Instantiate prefabs at origin.
- For first-person props, view from a gameplay camera height of roughly 1.65 meters.
- For enemies, inspect silhouette at short, medium, and long corridor distances.
- For level modules, snap to the documented grid and confirm player doorway/floor scale.
- For feedback-FX, trigger manually without gameplay wiring.

## Prefab Instantiate Checks

Required for every runtime prefab candidate:

- Opens in Prefab Mode.
- Instantiates without console errors.
- Has no missing script components.
- Has no missing mesh, material, texture, audio, or VFX references.
- Pivot and scale are plausible for its role.
- Can be deleted cleanly.

## Rollback Path

Rollback must be simple:

1. Close Unity or let AssetDatabase settle.
2. Delete the single quarantine import root.
3. Delete the matching QA report if it only describes the failed import.
4. Reopen or refresh Unity.
5. Confirm no references remain in live scenes or live prefabs.
6. Confirm git status shows no unexpected files outside the quarantine root.

Do not promote any package that cannot be rolled back this way.

## Decision Criteria

Promote:

- Static report has no errors.
- Throwaway import passes.
- Primary quarantine import has no blocking console errors.
- All required prefab checks pass.
- Assets support the next main-lane gameplay batch.
- Promotion touches only planned live production roots.

Defer:

- Package is useful but has placeholder assets, incomplete renders, minor material/readability issues, or missing non-critical metadata.
- Package needs generation before import.
- Package is visually promising but not yet ready for gameplay-facing integration.

Reject:

- Package changes project settings or requires unapproved dependencies.
- Package overwrites existing assets.
- Prefabs have missing scripts or broken core references.
- Rollback is not delete-only.
- Visual quality is materially below the north-star steampunk target and not fixable in the current batch.

Next-step directive: continue immediately with the next highest-impact unfinished task.
