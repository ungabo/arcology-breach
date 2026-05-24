# V0.1.36 Sidecar Project Architecture

Purpose: define a safe architecture for separate Unity projects/instances that produce importable asset packs while the primary game lane stays open.

## Operating Model

A sidecar is a disposable or semi-disposable Unity project used to build, preview, validate, and package a bounded asset family. It is not a fork of the game. It should mirror only the minimum Unity version, render pipeline assumptions, package dependencies, folder conventions, and naming rules needed to author importable content.

Primary project authority remains in `D:\__MY APPS\Unity Doom`. Sidecars should live outside the primary repo when eventually created, for example under `D:\__MY APPS\Unity Doom Sidecars\`, so their `Library/`, `Temp/`, `Logs/`, `UserSettings/`, and generated caches never enter the primary workspace.

## Recommended Sidecar Root Layout

Example future machine-local layout:

```text
D:\__MY APPS\Unity Doom Sidecars\
  UD-SC-WPN-WeaponPropLookdev\
    Assets\
      SidecarContent\
        com.brassworks.sidecar.weapon-prop-lookdev\
          package.json
          README.md
          CHANGELOG.md
          Runtime\
            Art\
            Audio\
            Materials\
            Prefabs\
            VFX\
          Samples~\
            PreviewScene\
            ContactSheets\
          Documentation~
            Manifest\
            ImportNotes\
    Packages\
      manifest.json
    ProjectSettings\
    Library\
  UD-SC-ENM-MechanicalEnemyVisuals\
  UD-SC-LVL-LevelModulesPrefabs\
  UD-SC-FBK-FeedbackUIAudioVFX\
```

The content root should always be one isolated package folder. Do not scatter sidecar output across `Assets/Materials`, `Assets/Prefabs`, `Assets/Scenes`, or project-level folders. A single content root makes diffing, reviewing, deleting, reimporting, and rollback realistic.

## Naming

Project naming:

- `UD-SC-WPN-WeaponPropLookdev`
- `UD-SC-ENM-MechanicalEnemyVisuals`
- `UD-SC-LVL-LevelModulesPrefabs`
- `UD-SC-FBK-FeedbackUIAudioVFX`

Package naming:

- `com.brassworks.sidecar.weapon-prop-lookdev`
- `com.brassworks.sidecar.mechanical-enemy-visuals`
- `com.brassworks.sidecar.level-modules-prefabs`
- `com.brassworks.sidecar.feedback-ui-audio-vfx`

Asset prefix naming:

- Weapons and props: `SCWPN_`
- Mechanical enemies: `SCENM_`
- Level modules: `SCLVL_`
- Feedback UI/audio/VFX: `SCFBK_`

Version suffix naming:

- Pack folder: `V0_1_36`
- Export artifact: `UD_SCWPN_WeaponPropLookdev_v0.1.36-p001.unitypackage`
- Manifest: `SCWPN_WeaponPropLookdev_Manifest_v0.1.36-p001.json`
- Import report: `SCWPN_WeaponPropLookdev_ImportReport_v0.1.36-p001.md`

Use incrementing pack build IDs for repeated attempts: `p001`, `p002`, `p003`. Do not overwrite prior handoff artifacts after review has started.

## Package Boundaries

Allowed inside sidecar packs:

- Meshes, materials, textures, sprites, audio clips, VFX Graph or ParticleSystem assets, animation clips, AnimatorControllers for visual preview only, prefabs, prefab variants, ScriptableObject data only if it uses already-approved runtime types from the primary project.
- Preview-only scenes under `Samples~/PreviewScene` in sidecar packaging, never promoted into primary scenes.
- Documentation, manifest, screenshots, contact sheets, metrics, and import notes.

Not allowed inside sidecar packs:

- New gameplay scripts.
- New editor validators.
- Project settings.
- Build settings.
- Input settings.
- Tags/layers unless explicitly documented as a required primary-lane change.
- Package dependency changes.
- Global render-pipeline settings.
- Primary game scenes or scene placements.
- Addressables or AssetBundle configuration unless the primary lane already owns that integration.

## Export/Import Method

Recommended default: UPM-style package folder plus manifest.

Use a Unity Package Manager compatible structure as the canonical handoff shape because it gives the cleanest boundary and can later be imported by local file path, Git URL, tarball, or copied package folder. Even if the primary project does not immediately consume it through UPM, the package shape forces clear ownership.

Supported handoff artifacts:

1. Canonical: package folder
   - Best for review, diffing, deletion, and repeatable local import.
   - Contains `package.json`, `README.md`, `CHANGELOG.md`, `Runtime/`, `Samples~/`, and `Documentation~`.
   - Primary-lane intake can copy the package into an approved staging path or add it as a local package only when package-file edits are explicitly owned by that integration task.

2. Snapshot: `.unitypackage`
   - Useful for a single manual import review.
   - Riskier because it can hide path collisions and asset moves until import time.
   - Must be exported from only the package content root, never from whole `Assets/`.

3. Quarantine option: staged folder copy
   - Only acceptable after sidecar validation and with primary-lane approval.
   - Copy into an isolated primary staging root such as `Assets/_Project/ArtStaging/SidecarImports/<PackName>/`.
   - Never copy into final production folders directly.

Do not use AssetBundles as the first pipeline. AssetBundles solve runtime delivery, not authoring handoff, and would add build pipeline complexity before the content boundaries are proven.

## Minimal `package.json` Shape

```json
{
  "name": "com.brassworks.sidecar.weapon-prop-lookdev",
  "version": "0.1.36-p001",
  "displayName": "Brassworks Sidecar Weapon Prop Lookdev",
  "description": "Visual-only weapon and prop lookdev pack for primary-project import review.",
  "unity": "6000.0",
  "dependencies": {},
  "samples": [
    {
      "displayName": "Preview Scene",
      "description": "Sidecar-only visual preview scene and contact sheet source.",
      "path": "Samples~/PreviewScene"
    }
  ]
}
```

Pin the exact Unity editor line to the primary project version once the main lane confirms it. Until then, sidecar scaffolding should be blocked rather than guessed.

## Manifest Contract

Each pack needs a manifest beside the export artifact and inside `Documentation~/Manifest/`.

Required manifest fields:

- `pack_id`
- `display_name`
- `version`
- `build_id`
- `unity_version`
- `sidecar_project`
- `owner_lane`
- `primary_intake_owner`
- `canonical_root`
- `export_artifacts`
- `asset_counts`
- `dependencies`
- `required_primary_changes`
- `path_collisions_checked`
- `guid_collisions_checked`
- `import_smoke_status`
- `known_risks`
- `rollback_path`

Example:

```json
{
  "pack_id": "SCWPN",
  "display_name": "Weapon Prop Lookdev",
  "version": "0.1.36",
  "build_id": "p001",
  "unity_version": "TBD_MATCH_PRIMARY",
  "sidecar_project": "UD-SC-WPN-WeaponPropLookdev",
  "owner_lane": "sidecar-weapon-prop-lookdev",
  "primary_intake_owner": "main-lane-art-integration",
  "canonical_root": "Packages/com.brassworks.sidecar.weapon-prop-lookdev",
  "export_artifacts": [],
  "asset_counts": {
    "prefabs": 0,
    "materials": 0,
    "textures": 0,
    "meshes": 0,
    "audio": 0,
    "vfx": 0
  },
  "dependencies": [],
  "required_primary_changes": [],
  "path_collisions_checked": false,
  "guid_collisions_checked": false,
  "import_smoke_status": "not_run",
  "known_risks": [],
  "rollback_path": "delete isolated imported package root"
}
```

## Ownership Rules

- One sidecar lane owns one package root.
- One package root has one active owner at a time.
- The primary lane is the only owner allowed to promote sidecar assets into gameplay prefabs, scenes, final production folders, package manifests, build settings, validators, or release notes.
- Sidecar lanes may provide previews, prefab candidates, material recipes, LOD guidance, collision guidance, and smoke reports.
- Sidecar lanes must not rename or replace existing primary-project assets. They submit new isolated assets and a mapping table if replacement is desired.
- Any asset that depends on a primary runtime type must list that dependency and provide a fallback visual-only prefab that does not require new scripts.

## Import Promotion Flow

1. Sidecar builds assets inside the isolated package root.
2. Sidecar exports a package folder snapshot and optional `.unitypackage`.
3. Sidecar validates in a clean throwaway import project, not the primary game.
4. Sidecar writes manifest, import report, preview sheet, and rollback notes.
5. Primary lane reviews artifact paths and dependency list.
6. Primary lane imports into quarantine staging.
7. Primary lane runs targeted smoke checks.
8. Primary lane decides whether to promote, defer, or reject.

## Versioning

Use `major.minor.patch-packBuild` where the game version remains `0.1.36` and the sidecar pack build increments independently.

- `0.1.36-p001`: first exportable sidecar candidate.
- `0.1.36-p002`: same content lane after fixes.
- `0.1.36-p003`: subsequent candidate after review.

Do not skip manifest updates when only art changes. Import validation depends on knowing exactly which build was tested.

## Delete/Rollback Rule

Every sidecar import must be removable by deleting one isolated root folder plus its `.meta` files. If a candidate cannot be rolled back that way, the pack boundary is too wide for sidecar intake.
