# Unity Render Toolchain Plan

Status: active
Date: 2026-05-23  
Lane: Unity-only high-fidelity asset proof, pressure pistol first

## Direction

Use Unity for lookdev and test renders. External renderer work is superseded for this project unless the user explicitly reopens it later.

The active proof target is the pressure pistol from:

`Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`

The goal is a credible in-engine proof that the game renderer can move toward the north-star realism: layered steampunk geometry, brass/iron/walnut material separation, readable gauge and coil, warm practical lighting, smoke/steam mood, and first-person framing.

## Unity Tooling

Preferred editor:

`C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe`

Project:

`D:\__MY APPS\Unity Doom`

The render lane should use an editor-only batchmode script, temporary objects, a Unity camera, and a `RenderTexture`. Output review images to `Documentation/ConceptRenders/`; do not add proof content to Build Settings.

## Isolation Rules

- Do not modify gameplay scenes, generated level scenes, or `MainMenu.unity`.
- Do not add lookdev scenes to Build Settings.
- Do not reference player controllers, save systems, combat managers, or scene-generation scripts.
- Put editor-only rendering code under `Assets/_Project/Editor/`.
- Put proof reports and metrics under `Documentation/AssetProduction/HighFidelityLookdevRecovery/UnityPressurePistolProof/`.
- Put JPG/PNG review output under `Documentation/ConceptRenders/`.
- Treat proof renders as non-shipping review files until they pass acceptance gates.

## Batchmode Shape

The renderer should:

1. Create a temporary scene in memory.
2. Build a first-person pressure pistol from Unity primitives and generated materials.
3. Add barrel, lower pressure tank, muzzle collars, top gauge, glass, needle, coil window, valves, side plates, visible fasteners, grip/hand mass, steam/smoke planes, and warm amber lights.
4. Use Unity material properties for metalness, smoothness, normal/noise, emission, and alpha where available.
5. Render through a camera into a `RenderTexture`.
6. Encode the image to JPG or PNG in `Documentation/ConceptRenders/`.
7. Write a report with component counts, render settings, and acceptance-gate result.
8. Exit Unity batchmode with a clear pass/fail log marker.

Example command:

```powershell
$unity = 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe'
$project = 'D:\__MY APPS\Unity Doom'
& $unity -batchmode -quit -projectPath $project -executeMethod UnityPressurePistolProofRenderer.RenderProof -logFile "$project\Logs\unity_pressure_pistol_proof.log"
```

## Acceptance Gates

Gate 1: Unity run

- Unity exits 0.
- Proof script writes at least one review image.
- Proof script writes a report and metric summary.

Gate 2: output file

- Hero proof is at least 1920x1080.
- Contact sheet or report identifies source concept, render time, and gate status.
- Output is outside Unity build assets.

Gate 3: component density

- Main blackened barrel and lower pressure tank are separate.
- Muzzle has nested/stepped rings.
- Gauge has brass bezel, cream face, glass highlight, red needle, and tick marks.
- Coil window has at least 6 turns.
- Grip/hand mass anchors the lower-right frame.
- At least 60 fasteners and 8 plates/brackets are visible or generated.

Gate 4: material and lighting

- Blackened iron, aged brass, dark pipe metal, hot copper, cream gauge face, glass, leather/walnut, smoke, and soot read as separate roles.
- Materials show roughness/specular variation instead of flat color.
- Warm amber highlights land on brass, gauge rim, muzzle, tank, and coil frame.
- Low fill preserves silhouette and shadows.

Gate 5: composition

- Camera reads as first-person 3/4.
- Muzzle points left/left-forward.
- Gauge and coil remain unobstructed.
- Gun occupies roughly 55-80% of image width and 45-70% of image height.
- Smoke/background support the object without hiding it.

Gate 6: review

- Without labels, the asset reads as the pressure pistol from the north-star sheet.
- If it fails, the report names the specific geometry, material, lighting, or framing gap and assigns the next Unity-only revision step.
