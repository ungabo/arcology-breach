# V0.1.52 Unity Weapon Assembly Lookdev

Quarantined Unity-only replacement for the rejected procedural Weapon Component Set 07 proof. This pass tests whether a pressure-pistol-like steampunk weapon assembly can hit the Brassworks Breach north-star material read using only Unity primitives, Unity-generated meshes, Unity materials, Unity lighting, and Unity `RenderTexture` PNG capture.

## Scope

- Render output: `Documentation/ConceptRenders/V0_1_52_UnityWeaponAssemblyLookdev/`
- QA manifest: `Documentation/QA/V0_1_52_UnityWeaponAssemblyLookdev/qa_manifest.json`
- Isolated Unity project: `AssetPacks/BrassworksBreach.UnityWeaponAssemblyLookdev08/`
- Renderer script: `AssetPacks/BrassworksBreach.UnityWeaponAssemblyLookdev08/Assets/Editor/UnityWeaponAssemblyLookdevRenderer.cs`

No main gameplay scenes, gameplay scripts, shared status docs, or main project manifests were edited.

## Render Set

1. `01_full_assembly_three_quarter.png` - complete pressure pistol read.
2. `02_copper_coil_closeup.png` - isolated copper coil around dark iron barrel.
3. `03_gauge_dial_closeup.png` - amber-rimmed pressure gauge/dial.
4. `04_muzzle_cluster_closeup.png` - muzzle crown and clustered crown ports.
5. `05_partial_assembled_exploded.png` - partial assembly/exploded review.
6. `06_first_person_scale_proxy.png` - first-person scale proxy with simple forearms/gloves.
7. `07_material_stress_warm_light.png` - warm lighting material stress.
8. `08_material_stress_cool_light.png` - cool lighting material stress.
9. `09_contact_sheet.png` - Unity-composited contact sheet of the first eight renders.

## What Passed

- Unity-only render path worked in batchmode with Unity 6000.4.6f1.
- The core weapon silhouette reads as a compact pressure pistol: dark receiver, long muzzle, amber tube, exposed copper coil, gauge, valve, rivets, grip, and brass crown details.
- Material intent is directionally useful. The warm/cool stress renders show that aged brass, copper, amber glass, and blackened iron separate under lighting changes.
- The gauge, coil, and muzzle each have a closer inspection render that can guide final asset priority.
- The first-person proxy suggests the assembly can sit in a hand-scale composition without immediately feeling like a rifle-sized prop.

## What Failed / Needs Revision

- Geometry is still blockout/lookdev, not final asset quality. The receiver and grip need authored bevels, better trigger-guard shape, and production mesh density.
- Procedural material noise is useful for tone but not enough for final grime. Final art should use authored masks for edge wear, soot, and oil accumulation.
- Amber glass reads strongly, but the gauge closeup shows glass opacity/refraction needs proper shader treatment before final promotion.
- The muzzle crown direction is promising but too simplified; it needs a more intentional crown, vents, bore detail, and soot pattern.
- The contact sheet is a Unity-composited review artifact, not a designed presentation sheet.

## Final Product Influence

Carry forward the dark iron receiver plus aged brass trim, the exposed copper coil, the amber pressure glass, the pressure gauge language, and the crown-port muzzle idea. Treat this as a visual target board for proportions and material separation, not as importable production geometry.

## Unity-Only Verification

- Created and ran an isolated Unity project at `AssetPacks/BrassworksBreach.UnityWeaponAssemblyLookdev08/`.
- Rendered via:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\Gabe\Documents\Codex\2026-05-22\i-want-to-do-an-experiment\AssetPacks\BrassworksBreach.UnityWeaponAssemblyLookdev08' -executeMethod UnityWeaponAssemblyLookdevRenderer.RunBatch -logFile 'C:\Users\Gabe\Documents\Codex\2026-05-22\i-want-to-do-an-experiment\AssetPacks\BrassworksBreach.UnityWeaponAssemblyLookdev08.render.log'
```

- Unity log contains `UNITY_WEAPON_ASSEMBLY_LOOKDEV_RENDERED` with all nine PNG paths.
- Meshes were assembled from Unity primitives plus a torus mesh generated inside the Unity editor script.
- Textures/material variation were generated inside Unity using `Texture2D`, `Mathf.PerlinNoise`, and Standard shader material settings.
- PNGs were written by Unity using `Texture2D.EncodeToPNG` after camera `RenderTexture` capture.
- No Blender, Python, PIL, external DCC, or external image-generation tool was used for these renders.
