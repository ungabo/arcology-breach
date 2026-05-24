# Pressure Pistol Component Lookdev Render Report

Status: Unity-only component refinement pass
Date: 2026-05-24 04:20:03 -04:00
Unity: 6000.4.6f1
Entrypoint: `PressurePistolLookDevRenderer.RenderBatch`

## Run Result

Batch renderer completed and wrote six isolated component PNGs, an updated contact sheet, metrics JSON, and this report. The renderer uses Unity Editor/batch APIs plus procedural primitives/materials only; no Blender or external DCC is involved.

Command used:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod PressurePistolLookDevRenderer.RenderBatch -logFile 'D:\__MY APPS\Unity Doom\Documentation\AssetProduction\PressurePistolLookDev\pressure_pistol_component_renderer_batch.log'
```

## Outputs

- Aged copper/brass coil pack: `Documentation/ConceptRenders/PressurePistolComponents/PPCOMP_001_copper_brass_coil_pack.png`
- Pressure gauge/dial with fittings: `Documentation/ConceptRenders/PressurePistolComponents/PPCOMP_002_pressure_gauge_dial.png`
- Boiler pressure chamber: `Documentation/ConceptRenders/PressurePistolComponents/PPCOMP_003_boiler_pressure_chamber.png`
- Barrel and muzzle assembly: `Documentation/ConceptRenders/PressurePistolComponents/PPCOMP_004_barrel_muzzle_assembly.png`
- Valve and manifold block: `Documentation/ConceptRenders/PressurePistolComponents/PPCOMP_005_valve_manifold_block.png`
- Leather grip and trigger guard: `Documentation/ConceptRenders/PressurePistolComponents/PPCOMP_006_leather_grip_trigger_guard.png`
- Contact sheet: `Documentation/ConceptRenders/PressurePistolComponents/PPCOMP_CONTACTSHEET_002_six_component_refinement.png`
- Metrics: `Documentation/AssetProduction/PressurePistolLookDev/pressure_pistol_component_lookdev_metrics.json`
- Batch log: `Documentation/AssetProduction/PressurePistolLookDev/pressure_pistol_component_renderer_batch.log`

## Component Evidence

| Component | Detail evidence | Material/read evidence | Automated checks |
| --- | --- | --- | --- |
| Aged copper/brass coil pack | 12 coils, 0 ticks, 32 fasteners, 11 plates/brackets, 8 pipes/manifolds, 0 rings/collars, 18 wear marks | 8 visual roles, 7 FinalMaterialsV1 families, warm 3.8%, dark 27.9% | nonblank pass, magenta pass, separation pass, framing pass |
| Pressure gauge/dial with fittings | 0 coils, 40 ticks, 12 fasteners, 3 plates/brackets, 6 pipes/manifolds, 7 rings/collars, 10 wear marks | 7 visual roles, 7 FinalMaterialsV1 families, warm 5.9%, dark 23.4% | nonblank pass, magenta pass, separation pass, framing pass |
| Boiler pressure chamber | 0 coils, 0 ticks, 30 fasteners, 5 plates/brackets, 6 pipes/manifolds, 5 rings/collars, 18 wear marks | 6 visual roles, 7 FinalMaterialsV1 families, warm 0.7%, dark 23.4% | nonblank pass, magenta pass, separation pass, framing pass |
| Barrel and muzzle assembly | 0 coils, 0 ticks, 8 fasteners, 1 plates/brackets, 4 pipes/manifolds, 6 rings/collars, 15 wear marks | 6 visual roles, 7 FinalMaterialsV1 families, warm 1.0%, dark 18.7% | nonblank pass, magenta pass, separation pass, framing pass |
| Valve and manifold block | 0 coils, 0 ticks, 10 fasteners, 5 plates/brackets, 5 pipes/manifolds, 2 rings/collars, 8 wear marks | 7 visual roles, 7 FinalMaterialsV1 families, warm 0.6%, dark 18.6% | nonblank pass, magenta pass, separation pass, framing pass |
| Leather grip and trigger guard | 0 coils, 0 ticks, 12 fasteners, 5 plates/brackets, 0 pipes/manifolds, 4 rings/collars, 12 wear marks | 7 visual roles, 7 FinalMaterialsV1 families, warm 1.7%, dark 30.8% | nonblank pass, magenta pass, separation pass, framing pass |

## Final Verification

- Unity batch log exits with return code `0`.
- No `error CS`, `Exception`, `MissingReferenceException`, or build failure lines were found in the final renderer log.
- Unity licensing status lines mention `failed validation` and `Access token is unavailable`, but the batch still rendered all outputs and exited successfully.
- A shell-only PNG sample pass confirms all six component PNGs and the contact sheet are nonblank and have `0` shader-magenta samples.
- Renderer metrics confirm every component passes nonblank, no-magenta, material separation, and camera-framing checks.

## Per-Component Acceptance Notes

### Aged copper/brass coil pack

Refined darker coil: aged brass and blackened steel frame, separate oxidized copper turns, smaller heat core, oil runs, soot, patina, slotted screws, and visible pressure leads. Still a primitive proof rather than authored mesh detail.

- Framing occupancy: x `0.128` to `0.876`, y `0.254` to `0.765`.
- Pixel checks: avg luminance `0.057`, near-black `66.7%`, magenta samples `0`.

### Pressure gauge/dial with fittings

Refined gauge uses a wider camera, more breathing room around side fittings, blackened rear cup, oily yoke, brass collar stack, cream dial, red needle, glass glints, raised slotted screws, and polished rim bites.

- Framing occupancy: x `0.221` to `0.779`, y `0.209` to `0.864`.
- Pixel checks: avg luminance `0.066`, near-black `69.4%`, magenta samples `0`.

### Boiler pressure chamber

New boiler component: blackened pressure tank with domed caps, brass bands, fill and relief caps, feed/discharge pipes, bracket feet, rivets, soot scars, and oil runs. It reads chunky and mechanical but still needs real mesh bevels for production.

- Framing occupancy: x `0.144` to `0.852`, y `0.283` to `0.756`.
- Pixel checks: avg luminance `0.036`, near-black `75.3%`, magenta samples `0`.

### Barrel and muzzle assembly

New barrel/muzzle component: nested brass collars, dark blackened tube, visible bore darkness, lower pressure pipe, front sight, slotted rail screws, soot at muzzle, and oil streaks. It is component proof only, not the whole pistol barrel layout.

- Framing occupancy: x `0.193` to `0.825`, y `0.407` to `0.670`.
- Pixel checks: avg luminance `0.037`, near-black `79.8%`, magenta samples `0`.

### Valve and manifold block

New valve/manifold component: layered block, gasket, inlet/outlet pipes, bypass line, two valve wheels, caps, screws, oil drips, and patina. The functional pipe logic is readable, though real production would replace the segmented wheels with authored geometry.

- Framing occupancy: x `0.155` to `0.845`, y `0.270` to `0.735`.
- Pixel checks: avg luminance `0.033`, near-black `80.4%`, magenta samples `0`.

### Leather grip and trigger guard

New grip/trigger component: walnut core, dark leather wrap, brass tang and butt cap, segmented trigger guard, trigger blade, rivets, leather creases, and oil-dark wear. It anchors the future first-person lower-right read, but the grip silhouette remains blockout-like.

- Framing occupancy: x `0.152` to `0.768`, y `0.127` to `0.816`.
- Pixel checks: avg luminance `0.050`, near-black `66.0%`, magenta samples `0`.

## Candid North-Star Self-Critique

- Better: the pass is now component-first and broader: coil, gauge, boiler chamber, barrel/muzzle, valve/manifold, and leather grip all exist as independent Unity renders with denser rivets, soot/oil, collars, and fittings.
- Better: the gauge camera is no longer as claustrophobic; side pipes, top cap, yoke, and full bezel now have review breathing room.
- Better: the coil is darker and less toy-bright, with blackened steel backing, aged brass, patina, smaller heat, and more recessed grime.
- Still weak: all geometry is primitive lookdev. The north-star art has molded bevel continuity, chipped edges, surface dents, and authored silhouettes that these blocks/cylinders only approximate.
- Still weak: leather and walnut read as material roles, but the grip needs a real sculpted profile before it will match the concept's handmade Victorian feel.
- Still weak: the material response is useful but not final PBR art; true production needs mesh UVs, trim/atlas choices, baked normal detail, and hand-placed grime masks.
- Recommendation: keep this as a component acceptance/reference lane and do not promote it to gameplay viewmodel art. Next pass should refine one component at a time into authored Unity mesh assets or a procedural mesh builder inside the same scope.

## PM Visual Review

Status: integrate as reference-only lookdev, not as final weapon art.

- The component breakdown is the right production method and is much better scoped than whole-gun attempts.
- The six-up sheet is still too dark and primitive to match the north-star pressure pistol; silhouettes read as simple cylinders/boxes rather than authored machinery.
- The grip/trigger module is the weakest visual match and needs a real shaped handle before full-gun reassembly.
- Next accepted step should choose one hero component, preferably gauge or barrel/muzzle, and push it toward authored bevels, grime masks, and production mesh standards before attempting a complete weapon.

## Scope Guard

This lane touched only the owned renderer and lookdev documentation/render outputs. It did not edit gameplay scripts, audio files, build scripts, status docs, version docs, or the active weapon definition.
