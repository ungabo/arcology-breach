# WeaponPropBatch Integration Checklist

Status: upcoming integration checklist
Applies to: `Assets/_Project/ArtStaging/WeaponPropBatch/**`

## Import Guardrails

- Keep this package in the art staging path until review passes.
- Do not move the staged materials into shared material libraries during smoke check.
- Do not create scenes, prefabs, gameplay scripts, animation clips, or collider assets from this package without a separate integration task.
- Treat all meshes as visual proofs. They are scale/silhouette/material references, not final optimized production geometry.

## Unity Smoke Check

- Open Unity and let `Assets/_Project/ArtStaging/WeaponPropBatch` import.
- Confirm the seven `WPB_###_*_Staged.obj` meshes appear without magenta materials.
- Confirm `Meshes/WPB_Brassworks_BatchPalette.mtl` resolves the named OBJ material slots.
- Confirm `Materials/M_WPB_*.mat` load as local staged materials.
- Confirm the four draft textures and three preview PNGs import as textures.
- Inspect mesh bounds in the Model preview and verify +Y up, +Z forward, meter scale.

## Art Review Gates

- Coil pack: copper loops must stay visually separate from brass rails and blackened backing.
- Pressure chamber/barrel: muzzle bore, collar stack, tank mass, and pressure fittings must read at viewmodel scale.
- Gauge/dial: cream face, tick marks, red needle, brass bezel, glass rim, and side ports must remain readable.
- Grip plates: walnut, leather, brass tang, trigger guard, screws, and grain/crease direction must not collapse into one brown mass.
- Fastener plates: slotted screws, rivets, straps, gasket shadow, patina, and oil runs must establish reusable repair language.
- Ammo cartridge: case, red seal, copper neck, primer, side rounds, and cradle must communicate pickup/ammo identity.
- Scattergun pieces: twin barrel collars, slug canister, shell row, stock socket, soot, and patina must work as display vocabulary.

## Integration Readiness

- Ready: loose Unity import review.
- Ready: material-role comparison against existing Brassworks material language.
- Ready: first-person silhouette discussion for pressure pistol components.
- Ready: pickup/display prop scale discussion for ammo and scattergun pieces.
- Not ready: final prefab hookup.
- Not ready: gameplay behavior, muzzle effects, animation, rigging, colliders, LODs, lightmap UVs, or production mesh replacement.

## Handoff Notes

- Use `weapon_prop_batch_manifest.json` for mesh counts, file paths, and preview locations.
- Use `Previews/WPB_mesh_inventory_preview.png` for quick non-Unity inventory review.
- Use `Previews/WPB_material_swatch_preview.png` to compare material-role spread before applying project-wide materials.
- Use the staged OBJ subobject names to split or replace components during final art pass.
