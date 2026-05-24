# Unity Pressure Pistol Recovery04 Brief

Status: active  
Subject: pressure pistol only  
Tool: Unity editor batchmode  
Expected render: `Documentation/ConceptRenders/RENDER_HFLD_Recovery04_pressure_pistol_unity_proof.jpg`

## Mission

Recovery04 replaces the failed flat fallback path with a true Unity in-engine proof. It must show whether the actual game renderer can carry the north-star pressure pistol look: layered brass hardware, blackened iron, walnut/leather grip mass, readable pressure gauge, exposed copper coil, warm lamps, smoke, and a first-person 3/4 camera.

This is not a gameplay integration pass. Do not touch gameplay scenes, generated level scenes, player controllers, combat scripts, build settings, or runtime managers.

## North-Star Target

Use the lower-right pressure-pistol crop from:

`Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`

Primary read:

- chunky 3/4 first-person pressure pistol
- muzzle points left/left-forward
- leather/walnut grip mass anchors lower-right
- blackened barrel and lower pressure tank read as separate cylinders
- brass frame, brackets, plates, collars, and dense rivets sit on top of dark metal
- top gauge is readable with cream face, red needle, brass bezel, and glass highlight
- exposed coil window has warm copper/hot accent
- background is dark, smoky, and warm-practical-lit without hiding the silhouette

## Unity Render Script Requirements

The Unity script should procedurally create the temporary proof setup:

- main blackened barrel
- nested muzzle stack
- lower pressure tank with straps
- 8+ plates/brackets/rails
- 60+ generated rivets/studs/fasteners
- 6+ copper coil turns in a recessed window
- top pressure gauge with bezel, face, lens, red needle, and tick marks
- visible pressure ports and top valves
- trigger and trigger guard
- angled walnut/leather grip mass
- smoke/steam planes or particle-like billboards
- warm amber key/rim lights and low cool fill
- RenderTexture capture to JPG/PNG

## Command

```powershell
$unity = 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe'
$project = 'D:\__MY APPS\Unity Doom'
& $unity -batchmode -quit -projectPath $project -executeMethod UnityPressurePistolProofRenderer.RenderProof -logFile "$project\Logs\unity_pressure_pistol_proof.log"
```

## Review Gates

The render is acceptable only if it passes all of these:

1. Unity command exits 0 and writes a review image.
2. Render is at least 1920x1080.
3. Gauge, coil, lower tank, muzzle stack, trigger/guard, and grip mass are readable at normal review size.
4. At least 60 fasteners and 8 plates/brackets are present.
5. Coil window shows 6+ turns.
6. Materials separate blackened iron, aged brass, dark pipe metal, hot copper, cream gauge face, glass, walnut/leather, soot, and smoke.
7. Camera reads as first-person 3/4 with muzzle left/left-forward, not flat side-view.
8. Warm practical lighting creates brass/gauge/coil highlights while keeping a dark smoky background.
9. Human review says it is closer to the north-star pistol than Recovery03.

## Failure Handling

If Recovery04 fails, do not promote it into gameplay. Write a focused note naming the failing gate:

- silhouette/camera failure
- component-density failure
- material-response failure
- lighting/contrast failure
- gauge/coil readability failure
- north-star identity failure

Then revise the Unity render script or create a focused Unity-only Recovery05 brief.
