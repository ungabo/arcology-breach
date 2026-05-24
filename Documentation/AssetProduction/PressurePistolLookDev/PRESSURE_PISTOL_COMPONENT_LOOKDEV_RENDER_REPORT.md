# Pressure Pistol Component Lookdev Render Report

Status: initial Unity-only component lookdev pass
Date: 2026-05-24 03:44:14 -04:00
Unity: 6000.4.6f1
Entrypoint: `PressurePistolLookDevRenderer.RenderBatch`

## Run Result

Final batch run completed and wrote the component PNGs, contact sheet, metrics JSON, and this report.

Command used:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod PressurePistolLookDevRenderer.RenderBatch -logFile 'D:\__MY APPS\Unity Doom\Documentation\AssetProduction\PressurePistolLookDev\pressure_pistol_component_renderer_batch.log'
```

Log read:

- Final run has no renderer exceptions and no C# compile errors.
- The log includes Unity licensing status lines containing `failed validation` and `Access token is unavailable`; Unity still completed batchmode with return code `0` and wrote all expected lookdev outputs.
- An earlier run exposed a contact-sheet lifetime issue after the two component PNGs were written. The owned renderer now reloads already-written PNGs from disk for contact sheet assembly, avoiding destroyed in-memory render textures during scene swaps.

## Outputs

- Coil pack: `Documentation/ConceptRenders/PressurePistolComponents/PPCOMP_001_copper_brass_coil_pack.png` (`1600x1000`, 808595 bytes)
- Gauge/dial: `Documentation/ConceptRenders/PressurePistolComponents/PPCOMP_002_pressure_gauge_dial.png` (`1600x1000`, 899165 bytes)
- Contact sheet: `Documentation/ConceptRenders/PressurePistolComponents/PPCOMP_CONTACTSHEET_001_coil_pack_gauge_dial.png` (`2200x1200`, 856914 bytes)
- Metrics: `Documentation/AssetProduction/PressurePistolLookDev/pressure_pistol_component_lookdev_metrics.json` (3878 bytes)
- Batch log: `Documentation/AssetProduction/PressurePistolLookDev/pressure_pistol_component_renderer_batch.log`

## Component Evidence

| Component | Geometry/detail evidence | Material evidence | Image check |
| --- | --- | --- | --- |
| Copper/brass coil pack | 11 coil turns, 0 ticks, 28 fasteners, 0 rings/collars, 14 wear marks | 7 visual roles, 5 FinalMaterialsV1 texture families loaded | avg luminance 0.217, near-black 32.6%, warm highlight 20.6% |
| Pressure gauge/dial | 0 coil turns, 36 ticks, 10 fasteners, 6 rings/collars, 8 wear marks | 6 visual roles, 5 FinalMaterialsV1 texture families loaded | avg luminance 0.273, near-black 28.3%, warm highlight 10.3% |

## Pixel Checks

| Image | Nonblank | Shader-magenta | Material separation | Evidence |
| --- | --- | --- | --- | --- |
| `PPCOMP_001_copper_brass_coil_pack.png` | Pass | Pass, `0` magenta samples | Pass | Avg luminance `0.2162`, near-black `32.98%`, warm metal `26.63%`, hot copper `22.11%`, red heat `12.37%`, dark material `28.42%` |
| `PPCOMP_002_pressure_gauge_dial.png` | Pass | Pass, `0` magenta samples | Pass | Avg luminance `0.2733`, near-black `28.32%`, warm metal `35.58%`, hot copper/brass highlights `15.93%`, red needle `1.11%`, dark material `20.62%` |
| `PPCOMP_CONTACTSHEET_001_coil_pack_gauge_dial.png` | Pass | Pass, `0` magenta samples | Pass | Avg luminance `0.1299`, near-black `66.07%`, warm metal `15.72%`, hot copper `9.51%`, red accents `3.21%`, dark material `11.72%` |

## Read

This pass deliberately avoids the whole gun. The coil and gauge are isolated components with separate nested Unity primitives, material-family assignments, rivets, scratches, grime, heat/glass accents, and camera/lighting proof. These renders are nonshipping lookdev only; they are intended to decide the component language before a later reassembly pass.

## Known Gaps

- Boiler chamber, barrel rings, grip, valves, screw/rivet atlas strategy, and full lighting/camera integration remain checklist-only in this pass.
- Geometry is temporary Unity primitive construction, not production mesh topology.
- No gameplay prefab, weapon definition, audio, tests, build matrix, or general project status document was touched.
