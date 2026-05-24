# V0.1.37 Sidecar Integration Gate Checklist

Purpose: promote sidecar asset packs into the primary project without letting speed create hidden project-setting, dependency, or rollback debt.

## Gate 0: Intake Identity

- Pack name:
- Pack version:
- Build ID:
- Sidecar project:
- Owner lane:
- Primary intake owner:
- Package root:
- Export artifact path:
- Manifest path:
- Import report path:
- Preview render/contact sheet path:

Pass conditions:

- The package has one isolated root under `AssetPacks/BrassworksBreach.*` or another explicitly approved quarantine source.
- The package is asset-only.
- The package can be removed by deleting one isolated root.
- No primary project file changes are required before quarantine import.

Stop conditions:

- The pack asks to modify `Packages/manifest.json`, `ProjectSettings/`, scenes, core scripts, validators, input, tags, layers, quality settings, or build settings.
- The pack includes undocumented runtime behavior.
- The pack is not tied to a manifest and rollback path.

## Gate 1: Static Package Validation

Run:

```powershell
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1
```

Required result:

- Zero errors.
- Warnings are reviewed and either fixed or logged as accepted.
- `package.json` parses and includes required fields.
- Manifest JSON parses and includes required fields.
- Required package folders are present.
- No conflict markers are present.
- No obvious `.meta`/source mismatches block import.

## Gate 2: Clean Throwaway Import

Required steps:

1. Create or open a throwaway Unity project matching the primary editor version.
2. Import or mount only the package under review.
3. Wait for AssetDatabase refresh to complete.
4. Confirm the console has no blocking errors.
5. Open package sample or preview content if provided.
6. Instantiate every runtime prefab candidate in an empty scene.
7. Inspect prefabs for missing scripts, missing meshes, missing materials, missing textures, missing audio, and broken VFX references.
8. Capture a preview render/contact sheet if the pack is visual.
9. Fill out the quarantine QA report template.

Pass conditions:

- All runtime prefab candidates instantiate.
- Missing references are zero or documented as preview-only non-blockers.
- Console has no blocking compile/import errors.
- Package dependencies match approved primary project dependencies.

## Gate 3: Primary Quarantine Import

Quarantine target:

```text
Assets/_Project/ArtStaging/SidecarImports/<PackageName>/
```

Required steps:

1. Confirm the main working tree is understood before import.
2. Copy or import into quarantine only.
3. Confirm no existing asset path is overwritten.
4. Confirm no edits appear outside the quarantine root, expected `.meta` files, and the integration report.
5. Run a targeted Unity import/compile validation.
6. Instantiate the candidates in a temporary review scene or existing editor-only review scene.
7. Record console warnings.
8. Decide promote, defer, or reject.

Stop conditions:

- Any scene, core script, validator, project setting, package manifest, build artifact, or unrelated asset changes unexpectedly.
- Any runtime prefab candidate imports with missing scripts.
- Any dependency cannot be satisfied without project-level changes.
- Rollback requires more than deleting the quarantine root and associated report.

## Gate 4: Live Promotion

Promotion may happen only after quarantine pass.

Required steps:

1. Choose the smallest live production root that matches project conventions.
2. Promote only assets needed by the current gameplay batch.
3. Preserve source pack identity in notes or metadata.
4. Wire assets through existing systems and prefabs rather than adding new gameplay scripts from the sidecar.
5. Run targeted editor validation.
6. Run the relevant player smoke tests after a meaningful batch is integrated.
7. Update the work ledger, parallel status, build status, and release notes as appropriate.

Promotion decision:

- Promote:
- Defer:
- Reject:
- Reason:
- Required follow-up:

Next-step directive: continue immediately with the next highest-impact unfinished task.
