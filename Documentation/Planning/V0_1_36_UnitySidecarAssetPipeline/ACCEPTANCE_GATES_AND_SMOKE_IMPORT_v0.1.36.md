# V0.1.36 Acceptance Gates And Smoke Import

Purpose: define the gates a sidecar asset pack must pass before anything is imported into the primary Unity project.

## Gate 0: Scope Gate

Pass conditions:

- Pack is asset-only.
- Pack has one isolated package root.
- Pack does not require new scripts, scenes, validators, build settings, or package changes.
- Pack includes a manifest and import report template.
- Required primary-project changes are either empty or explicitly documented for later owner review.

Stop conditions:

- Pack requires editing `Packages/manifest.json`.
- Pack requires changing tags, layers, input, quality, render pipeline, physics, or build settings.
- Pack contains primary game scenes or changes existing primary prefabs.
- Pack cannot be deleted cleanly by removing one isolated root.

## Gate 1: Naming And Boundary Gate

Pass conditions:

- Asset paths sit under the canonical package root.
- Asset names use the lane prefix.
- No path collisions with existing primary project assets.
- `.meta` files are present and stable in the exported package folder.
- The `.unitypackage`, if produced, includes only the sidecar content root.

Stop conditions:

- Assets are scattered across generic folders.
- Same-name material/prefab candidates are proposed as direct replacements.
- Sidecar preview scenes or editor-only helpers are mixed with runtime import content.

## Gate 2: Dependency Gate

Pass conditions:

- Manifest lists every dependency outside the package root.
- Pack works with only built-in Unity modules and confirmed primary dependencies.
- Any script-based dependency is already present in the primary project and listed by type/name.
- Preview-only dependencies remain under `Samples~` or `Documentation~`.

Stop conditions:

- Missing shader, material, script, font, audio mixer, or VFX dependency is discovered during clean import.
- Sidecar package depends on local absolute file paths.
- Pack assumes package versions not present in the primary project.

## Gate 3: Clean Throwaway Import Gate

Before primary intake, import into a clean throwaway Unity project that matches the primary editor version and minimal package baseline.

Required checks:

1. Open Unity with no compile errors.
2. Import the sidecar package folder or `.unitypackage`.
3. Confirm no missing scripts.
4. Confirm no missing materials.
5. Confirm no missing meshes/textures/audio/VFX references.
6. Open preview sample scene if included.
7. Instantiate every runtime prefab candidate in an empty scene.
8. Confirm all prefabs can be selected without inspector errors.
9. Record console warnings and classify them as block/fix/defer.
10. Export an import report with pass/fail status and screenshots.

Pass condition:

- No blocking console errors, no missing references, no package dependency drift, and all runtime prefabs instantiate.

## Gate 4: Content-Specific Gates

Weapon/prop pack:

- Viewmodel candidates preserve plausible muzzle alignment.
- Pickup and display pivots are stable.
- Trigger/collision guidance uses simple primitive intent.
- Red/green/amber material meanings remain consistent.
- No ammo, reload, unlock, damage, or input behavior is included.

Mechanical enemy visual pack:

- Enemy families remain identifiable from silhouette alone.
- Weak points and charge tells are distinct from furnace-eye glow.
- Socket markers are present and consistently named.
- LODs preserve role-defining geometry.
- No spawn, route, combat, AI, or damage behavior is included.

Level module/prefab pack:

- Module dimensions and pivots are documented.
- Collision guidance uses simple primitives.
- First-person scale reads correctly in preview.
- Modules do not imply new route blockers or objective progression.
- No primary scene placement is included.

Feedback UI/audio/VFX pack:

- Content is event-ready but not event-wired.
- Audio files have normalized naming and loudness notes.
- VFX prefabs do not assume gameplay event timing.
- HUD/UI sprites do not require new input or settings work.
- Accessibility risks are documented.

## Gate 5: Primary Quarantine Import Gate

Only the primary lane performs this gate.

Required steps:

1. Import into an isolated quarantine root.
2. Confirm file paths match the manifest.
3. Confirm no existing assets were overwritten.
4. Confirm no `ProjectSettings/`, `Packages/`, scenes, validators, or scripts changed.
5. Open a minimal test scene or temporary review scene.
6. Instantiate candidates and inspect missing references.
7. Run only targeted smoke checks appropriate to the pack.
8. Capture import notes and decide promote/defer/reject.

Promotion is not automatic. Passing quarantine means the pack is eligible for integration work, not already integrated.

## Suggested Smoke Checks

General:

- Console is free of blocking errors after import.
- Asset database refresh completes.
- Every prefab opens in Prefab Mode.
- Materials render with expected shaders.
- Textures/audio/VFX clips import with expected settings.
- Removing the package root removes all imported content.

Weapon/prop:

- Instantiate weapon candidates at origin and in first-person preview camera.
- Check muzzle, grip, pickup, and display pivots.
- Verify no mesh collider is required for decorative details.

Enemy visual:

- Instantiate all enemy families in a lineup.
- Toggle weak-point, furnace-eye, and charge materials.
- Inspect LOD transitions from expected combat distance.

Level module:

- Place modules on a grid.
- Check floor contact, wall thickness, doorway dimensions, and player-height readability.
- Verify proposed collision primitives do not block route-critical spaces without main-lane review.

Feedback:

- Play one-shot audio candidates at neutral volume.
- Trigger VFX manually in a blank scene.
- Confirm sprites/icons are readable on light and dark backgrounds.

## Acceptance Record

Each pack should produce a short report:

```text
Pack:
Version:
Build ID:
Unity version:
Sidecar project:
Export method:
Clean import status:
Primary quarantine status:
Blocking issues:
Deferred issues:
Required primary changes:
Rollback path:
Reviewer:
Decision: promote / defer / reject
```

## Stop Conditions

- Any import modifies primary project settings, packages, build settings, scenes, scripts, validators, or shared status files.
- Any sidecar pack overwrites existing assets by path.
- Any prefab has missing scripts or missing core references after clean import.
- Any package requires unapproved dependencies.
- Any asset is impossible to roll back by deleting one isolated root.
- Any sidecar lane asks the primary lane to accept undocumented content.
