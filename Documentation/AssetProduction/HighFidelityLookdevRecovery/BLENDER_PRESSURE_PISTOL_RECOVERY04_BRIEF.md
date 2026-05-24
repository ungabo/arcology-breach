# Blender Pressure Pistol Recovery04 Brief

Status: ready to run after Blender unblock  
Subject: pressure pistol only  
Script: `Documentation/AssetProduction/HighFidelityLookdevRecovery/PressurePistolProof/blender_pressure_pistol_recovery04_scene.py`  
Expected render: `Documentation/ConceptRenders/RENDER_HFLD_Recovery04_pressure_pistol_blender_proof.jpg`

## Mission

Recovery04 exists to replace the failed Recovery03 Python/Pillow fallback with a true Blender proof that can demonstrate 3D bevels, layered mechanical depth, PBR-like material response, glass, smoky practical lighting, and first-person composition.

This is not a Unity integration pass. Do not touch Unity scenes, gameplay scripts, shared status docs, or existing ConceptRenders during this handoff.

## North-Star Target

Use the lower-right pressure-pistol crop from:

`Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`

Primary read:

- chunky 3/4 first-person pressure pistol
- muzzle points left/left-forward
- leather glove/grip mass anchors lower-right
- blackened barrel and lower pressure tank read as separate bevelled cylinders
- brass frame, brackets, plates, collars, and dense rivets sit on top of the dark metal
- top gauge is readable with cream face, red needle, brass bezel, and glass highlight
- exposed coil window has a warm copper/hot accent
- background is dark, smoky, and warm-practical-lit without hiding the silhouette

## Scene Script Contents

The Recovery04 Blender script builds the scene procedurally with no external asset dependency:

- bevelled main blackened barrel
- nested bevelled muzzle stack
- separate lower pressure tank with brass straps
- 18+ plates/brackets/rails
- 135+ generated rivets/studs/fasteners
- 8-turn copper coil in a dark recessed window
- top pressure gauge with brass bezel, cream face, glass lens, red needle, 44 tick marks, and highlight arc
- 3 visible front pressure ports
- 3 top valves/caps
- trigger, brass trigger guard, angled leather grip
- lower-right leather glove mass with palm, thumb, fingers, and brass knuckle studs
- translucent smoke puffs and dark background wall
- warm amber key, warm rim, low cool fill, and warm practical bulbs
- 1920x1080 default Cycles render with Filmic contrast

## Exact Render Command

After Blender is installed and available:

```powershell
$root = 'D:\__MY APPS\Unity Doom'
$blender = (Get-Command blender -ErrorAction Stop).Source
$script = Join-Path $root 'Documentation\AssetProduction\HighFidelityLookdevRecovery\PressurePistolProof\blender_pressure_pistol_recovery04_scene.py'
$out = Join-Path $root 'Documentation\ConceptRenders\RENDER_HFLD_Recovery04_pressure_pistol_blender_proof.jpg'
& $blender --background --factory-startup --python $script -- --output $out --samples 128 --res-x 1920 --res-y 1080
```

Quick lower-sample smoke test:

```powershell
$root = 'D:\__MY APPS\Unity Doom'
$blender = (Get-Command blender -ErrorAction Stop).Source
$script = Join-Path $root 'Documentation\AssetProduction\HighFidelityLookdevRecovery\PressurePistolProof\blender_pressure_pistol_recovery04_scene.py'
$out = Join-Path $root 'Documentation\ConceptRenders\RENDER_HFLD_Recovery04_pressure_pistol_blender_proof_smoketest.jpg'
& $blender --background --factory-startup --python $script -- --output $out --samples 32 --res-x 1280 --res-y 720
```

Use the smoke test only to confirm the script runs. The acceptance render remains the 1920x1080 JPG.

## Review Gates

The render is acceptable only if it passes all of these:

1. Blender command exits 0 and writes a JPG at the expected path.
2. Render is at least 1920x1080.
3. Gauge, coil, lower tank, muzzle stack, trigger/guard, and leather grip/glove are all readable at normal review size.
4. At least 60 fasteners and 8 plates/brackets are visible.
5. Coil window shows 6+ turns; the script generates 8.
6. Materials separate blackened iron, aged brass, darker pipe metal, hot copper, cream gauge face, glass, leather, and soot/smoke.
7. Camera reads as first-person 3/4 with muzzle left/left-forward, not flat side-view.
8. Warm practical lighting creates brass/gauge/coil highlights while keeping a dark smoky background.
9. Human review says it is closer to the north-star pistol than Recovery03.

## Failure Handling

If Recovery04 fails, do not promote it to Unity. Write a focused note naming the failing gate:

- silhouette/camera failure
- component-density failure
- material/PBR failure
- lighting/contrast failure
- gauge/coil readability failure
- north-star identity failure

Then revise only the Blender script or create the next isolated recovery brief. Gameplay and production-lane files remain off limits.
