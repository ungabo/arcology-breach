# HFLD Recovery03 Pressure Pistol Proof Report

Status: proof attempt failed acceptance  
Date/time: 2026-05-23 23:23:13 -04:00  
Subject: pressure pistol only  
Label: non-shipping fallback proof, not accepted final art

## Outputs

| File | Purpose | Dimensions |
| --- | --- | ---: |
| `Documentation/ConceptRenders/RENDER_HFLD_Recovery03_pressure_pistol_proof_python_fallback.jpg` | Hero proof attempt, no annotation layer | 1920x1080 |
| `Documentation/ConceptRenders/CONTACTSHEET_HFLD_Recovery03_pressure_pistol_proof_python_fallback.jpg` | Review contact sheet with source crop, Batch01 comparison, and measurements | 2200x1500 |
| `Documentation/AssetProduction/HighFidelityLookdevRecovery/PressurePistolProof/generate_pressure_pistol_proof.py` | Procedural fallback renderer | n/a |
| `Documentation/AssetProduction/HighFidelityLookdevRecovery/PressurePistolProof/pressure_pistol_proof_metrics.json` | Measured output counts | n/a |

## Tool Path

Blender was preferred but was not available in the local environment:

- `where.exe blender` returned no executable.
- `Get-Command blender` returned no executable.
- `C:\Program Files\Blender Foundation` did not contain `blender.exe`.
- A limited recursive search on `D:\` timed out without finding a usable Blender executable.
- ImageMagick and POV-Ray were not available.
- Local Python had Pillow 12.2 and NumPy available; common 3D render packages such as `trimesh`, `pyrender`, `moderngl`, `scipy`, `skimage`, and `cv2` were not installed.

Fallback used: Python/Pillow/NumPy procedural raster construction. This is a real proof attempt in the sense that it creates a standalone gun render and counts visible parts, but it is not a true 3D/PBR proof.

## Measured Checks

| Check | Result |
| --- | --- |
| Hero dimensions | 1920x1080 |
| Contact sheet dimensions | 2200x1500 |
| Gauge | 1 visible top gauge, cream face, brass bezel, red needle, 44 tick marks |
| Coil window | 1 visible coil window, 8 visible hot-copper coil turns |
| Muzzle assembly | 5 generated nested ring/stepped forms |
| Lower pressure tank | 1 separate lower dark cylinder |
| Trigger/guard | 1 trigger and 1 trigger guard |
| Glove/grip mass | 1 lower-right leather hand/grip mass |
| Pressure ports | 3 generated visible ports/sockets |
| Top valves/caps | 3 generated top valves/caps |
| Plates/brackets/straps | 36 generated distinct pieces |
| Fasteners | 151 generated screws/rivets/bolts |
| Material roles | 8 visual roles: blackened iron, aged brass, darker pipe metal, hot copper coil, cream gauge face, glass highlight, dark leather, soot/grime |
| Logical material slots | 6 simulated slots: blackened iron, aged brass, dark pipe metal, hot copper, gauge/glass face, leather/soot/grime |
| Body occupancy | 71.7% image width, 64.1% image height using alpha >= 190 body mask |
| Silhouette bbox | x 262-1638, y 201-893 |

## Gate Score

| Gate | Score | Evidence |
| --- | --- | --- |
| Gate 0: Scope and label | Pass | New files are in the allowed proof folder and allowed ConceptRenders names. Subject is pressure-pistol-only. Render/contact sheet are labeled proof attempt/non-shipping. No gameplay files or Unity scenes were edited. |
| Gate 1: Component count | Pass by count, partial visually | Required nouns are present: barrel, lower tank, gauge, coil, muzzle stack, trigger/guard, glove/grip, ports, top caps, plates, and 60+ fasteners. Visual credibility is only partial because many fasteners and brackets still read as drawn symbols. |
| Gate 2: Material and texture detail | Fail | The render simulates the required material roles and six logical slots, but it does not contain real 3D materials, UVs, base color/normal/roughness/metallic/AO maps, bevel geometry, or PBR lighting. Texture detail remains painterly/procedural and too flat. |
| Gate 3: Camera and composition | Partial/fail | Muzzle direction, gauge visibility, coil visibility, glove scale, dark background, and 55-80%/45-70% occupancy are satisfied. However, the camera still reads too much like a side-view construction and not enough like the source concept's chunky 3/4 first-person object. |
| Gate 4: Lighting and contrast | Partial | Warm key, dark/cool fill, rim accents, coil glow, readable gauge, and non-obstructive steam are present. The result lacks true dimensional specular response and practical-light depth, so it does not reach north-star lighting quality. |
| Gate 5: Resolution and file checks | Pass | Hero proof is 1920x1080, contact sheet is above 1536x1024, filenames include `Recovery03` and `pressure_pistol`, outputs are in `Documentation/ConceptRenders/`, and this companion report names the tool/source/material/camera/lighting gaps. |
| Gate 6: Human review | Fail | Without labels, the result is closer than Batch01 in part density and checklist coverage, but it is still not close enough to the source concept. It remains too flat, too graphic, and not convincingly high-fidelity. |

Overall verdict: fail. This artifact is useful as a pressure-pistol-only density/count proof and fallback blocker record. It should not be promoted as accepted high-fidelity direction.

## Silhouette And Visual Notes

- The body mask is within the target frame occupancy range at 71.7% width and 64.1% height.
- The top gauge breaks the upper silhouette, and the lower tank creates a second cylinder below the main barrel.
- The coil window is readable and warm, but its loops are too even and graphic compared with the source.
- The pistol still lacks the source concept's layered depth, beveled chunkiness, and occluded overlapping mechanical masses.
- The leather glove/grip anchors the lower-right frame, but the glove shape is still too angular and not material-rich enough.
- Lighting has a warm/cool split and smoke, but the proof lacks true 3D shadowing and metal reflections.

## Blocker And Next Exact Tool

The next credible proof needs Blender or an equivalent local 3D renderer. The fallback raster path cannot prove geometry, bevels, PBR channels, or real material response.

Exact unblock command:

```powershell
winget install --id BlenderFoundation.Blender -e --source winget
```

After Blender is available on `PATH`, the next pass should be a Blender offline scene with bevelled cylinders, extruded brass plates, 60+ mesh rivets, a real coil mesh, glass/gauge geometry, procedural PBR materials, and a camera/light setup matching the north-star crop. The hero output should replace this fallback with a true `RENDER_HFLD_Recovery03_pressure_pistol_proof*.jpg` or a new Recovery04 proof if this file is kept as the failed fallback record.
