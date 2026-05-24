# Enemy Readability Batch

Date: 2026-05-24
Package state: staged for upcoming Unity integration
Scope: `EnemyReadabilityBatch` only

This folder is the documentation side of a parallel readability/art staging package for Brassworks Breach mechanical enemies. The matching Unity asset payload lives at:

`Assets/_Project/ArtStaging/EnemyReadabilityBatch/`

## Contents

- `ENEMY_READABILITY_BATCH_MANIFEST.md` - asset index, import contract, and package coverage.
- `ENEMY_READABILITY_BATCH_ART_NOTES.md` - silhouette, tell, weak-point, shutdown-fragment, and material separation notes.
- `ENEMY_READABILITY_BATCH_INTEGRATION_CHECKLIST.md` - Unity import/review gates for the next integration pass.
- `ENEMY_READABILITY_BATCH_WORK_LOG.md` - work log and generation notes.
- `ERB_EnemyReadabilityBatch_Manifest.json` - machine-readable asset index duplicated from the Unity staging metadata folder.
- `Previews/` - PNG readability boards and contact sheet.
- `Tools/Generate-EnemyReadabilityBatch.ps1` - reproducible generator for the staged OBJ, material, metadata, and preview payload.

## Guardrails

- No gameplay code.
- No scene builder.
- No shared docs or shared material folders modified.
- No Blender dependency.
- No git operations.
- Write scope was limited to:
  - `D:\__MY APPS\Unity Doom\Documentation\AssetProduction\EnemyReadabilityBatch\`
  - `D:\__MY APPS\Unity Doom\Assets\_Project\ArtStaging\EnemyReadabilityBatch\`
