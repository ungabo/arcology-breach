# Render Toolchain Unblock Plan

Status: blocked on local Blender availability  
Date: 2026-05-23  
Lane: high-fidelity asset proof, pressure pistol only

## Current Local Tooling Result

Blender is not currently available in this environment.

Commands checked:

```powershell
Get-Command blender -ErrorAction SilentlyContinue
Get-Command blender.exe -ErrorAction SilentlyContinue
Get-Command bforartists -ErrorAction SilentlyContinue
Get-Command bforartists.exe -ErrorAction SilentlyContinue
where.exe blender
```

Observed result:

- `where.exe blender` returned `INFO: Could not find files for the given pattern(s).`
- `Get-Command blender`, `blender.exe`, `bforartists`, and `bforartists.exe` returned no executable.
- Checked usual install folders under `C:\Program Files\Blender Foundation`, `C:\Program Files\Blender`, `C:\Program Files\Bforartists`, and targeted `D:\` tool folders; no `blender.exe` was found.
- `winget.exe` is available at `C:\Users\Gabe\AppData\Local\Microsoft\WindowsApps\winget.exe`.
- `python.exe` is available at `C:\Python313\python.exe`.
- Python has `PIL` and `numpy`, but not `bpy`, `trimesh`, `pyrender`, `moderngl`, `OpenGL`, `mitsuba`, `cv2`, `skimage`, or `scipy`.
- `magick`, `povray`, `pvengine`, `mitsuba`, `freecad`, `openscad`, and `oiiotool` were not found on PATH.

Verdict: the Recovery03 Python/Pillow fallback remains the only runnable local render path, and it cannot prove bevelled geometry, PBR response, glass, or true 3D composition. The next credible proof requires Blender or an equivalent 3D renderer to be installed first.

## Do Not Run During This Lane

Do not install software or generate ConceptRenders from this handoff unless the render lane is explicitly allowed to write the output JPG. This plan only documents the unblock and provides the ready-to-run scene script.

Do not edit:

- `Documentation/ConceptRenders/`
- Unity scenes
- gameplay scripts
- `README.md`
- `Documentation/BUILD_STATUS.md`
- `Documentation/WORK_LEDGER.md`
- shared status docs

## Exact Unblock Command

When installation is approved, use the noninteractive winget command:

```powershell
winget install --id BlenderFoundation.Blender -e --source winget --accept-package-agreements --accept-source-agreements
```

After install, open a fresh PowerShell or refresh PATH, then verify:

```powershell
$blender = (Get-Command blender -ErrorAction Stop).Source
& $blender --version
```

If PATH is not refreshed, locate the executable directly:

```powershell
Get-ChildItem -LiteralPath 'C:\Program Files\Blender Foundation' -Recurse -Filter blender.exe
```

Then use the returned full path in place of `$blender`.

## Recovery04 Headless Render Command

Default render target:

`D:\__MY APPS\Unity Doom\Documentation\ConceptRenders\RENDER_HFLD_Recovery04_pressure_pistol_blender_proof.jpg`

Script:

`D:\__MY APPS\Unity Doom\Documentation\AssetProduction\HighFidelityLookdevRecovery\PressurePistolProof\blender_pressure_pistol_recovery04_scene.py`

Run:

```powershell
$root = 'D:\__MY APPS\Unity Doom'
$blender = (Get-Command blender -ErrorAction Stop).Source
$script = Join-Path $root 'Documentation\AssetProduction\HighFidelityLookdevRecovery\PressurePistolProof\blender_pressure_pistol_recovery04_scene.py'
$out = Join-Path $root 'Documentation\ConceptRenders\RENDER_HFLD_Recovery04_pressure_pistol_blender_proof.jpg'
& $blender --background --factory-startup --python $script -- --output $out --samples 128 --res-x 1920 --res-y 1080
```

If using a direct executable path:

```powershell
$root = 'D:\__MY APPS\Unity Doom'
$blender = 'C:\Program Files\Blender Foundation\Blender 4.x\blender.exe'
$script = Join-Path $root 'Documentation\AssetProduction\HighFidelityLookdevRecovery\PressurePistolProof\blender_pressure_pistol_recovery04_scene.py'
$out = Join-Path $root 'Documentation\ConceptRenders\RENDER_HFLD_Recovery04_pressure_pistol_blender_proof.jpg'
& $blender --background --factory-startup --python $script -- --output $out --samples 128 --res-x 1920 --res-y 1080
```

Replace `Blender 4.x` with the actual folder returned by `Get-ChildItem`.

## Expected Script Output

The script prints the generated proof counts before rendering:

- visible fasteners: 135+
- brass plates/brackets/rails: 18+
- coil turns: 8
- gauge tick marks: 44
- pressure ports: 3
- top valves/caps: 3

The script default writes only the hero JPG. It has an optional `--save-blend` argument, but that should stay unused unless a future lane explicitly allows a `.blend` artifact.

## Acceptance Gates

Gate 1: toolchain

- `& $blender --version` exits 0.
- Blender runs the script headlessly with `--background --factory-startup`.
- No Unity files, gameplay files, build docs, or shared status docs are modified.

Gate 2: output file

- `Documentation/ConceptRenders/RENDER_HFLD_Recovery04_pressure_pistol_blender_proof.jpg` exists.
- JPG dimensions are 1920x1080 or greater.
- File is a hero proof render, not an annotated planning sheet.

Gate 3: component density

- Main blackened barrel is bevelled and capped.
- Lower pressure tank is separate and bevelled.
- Muzzle has nested/stepped rings.
- Top gauge has brass bezel, cream face, glass lens, red needle, and tick marks.
- Coil window has at least 6 turns; Recovery04 script generates 8.
- Grip/glove mass anchors the lower-right frame.
- At least 60 fasteners are visible; Recovery04 script generates 135+.
- At least 8 plates/brackets are visible; Recovery04 script generates 18+.

Gate 4: material and lighting

- Blackened iron, aged brass, dark pipe metal, hot copper, cream gauge face, glass, leather, and soot/smoke are all readable.
- Materials use procedural roughness/bump/noise where useful instead of flat color only.
- Warm amber key and practical lights create highlight islands on brass, barrel caps, gauge rim, muzzle, lower tank, and coil frame.
- Low cool fill preserves the silhouette without flattening the object.

Gate 5: composition

- Camera reads as 3/4 first-person, not a flat side view.
- Muzzle points left/left-forward.
- Gauge and coil remain unobstructed.
- Gun occupies roughly 55-80% of image width and 45-70% of image height.
- Smoky background supports silhouette without becoming the subject.

Gate 6: human review

- Without labels, reviewers can identify the asset as the north-star pressure pistol.
- It is materially and dimensionally closer than the Recovery03 Python fallback.
- If it still fails, the failure note must name the specific missing geometry, material, light, or camera issue before any Unity validation begins.
