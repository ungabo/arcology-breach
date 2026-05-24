# HFLD Recovery04 Unity Pressure Pistol Proof Report

Status: rejected full-gun Unity proof; not accepted final art
Date/time: 2026-05-24 00:06:54 -04:00
Unity version: 6000.4.6f1
Subject: pressure pistol only
Tool lane: Unity editor batchmode, temporary in-memory scene, Camera plus RenderTexture JPG export
Batchmode command entrypoint: `UnityPressurePistolProofRenderer.RenderBatch`
Superseded direction: the prior non-Unity lane is forbidden by current project direction.

## Outputs

| File | Purpose | Dimensions |
| --- | --- | ---: |
| `Documentation/ConceptRenders/RENDER_HFLD_Recovery04_pressure_pistol_unity_proof.jpg` | Hero Unity proof render | 1920x1080 |
| `Documentation/ConceptRenders/CONTACTSHEET_HFLD_Recovery04_pressure_pistol_unity_proof.jpg` | Contact sheet comparing north-star crop and Unity proof | 2200x1400 |
| `Documentation/AssetProduction/HighFidelityLookdevRecovery/UnityPressurePistolProof/unity_pressure_pistol_proof_metrics.json` | Unity-generated component/frame metrics | n/a |

## Methodology

- Created a new unsaved editor scene in memory and did not add anything to build settings.
- Built the pressure pistol from Unity primitives only: cylinders, boxes, spheres, transparent quads, and layered collar/strap geometry to approximate bevel highlights.
- Used Standard-compatible material setup with URP Lit fallback detection, procedural base/normal/occlusion textures, metallic/smoothness values, transparent gauge glass, emissive copper coil, walnut grip, and dark leather glove mass.
- Lit the scene with warm amber key, hot coil/gauge practicals, low cool fill, brass rim light, dark fog, and soft smoke billboards from visible ports.
- Captured the hero image through a Unity Camera into a RenderTexture, then captured a Unity-rendered contact sheet with the north-star pressure-pistol crop.

## Component Metrics

| Check | Result |
| --- | ---: |
| Visible coil turns | 8 |
| Fasteners/rivets/bolts | 106 |
| Plates/brackets/straps | 32 |
| Pressure ports/sockets | 3 |
| Top valves/caps | 3 |
| Gauge tick marks | 28 |
| Steam/smoke billboards | 28 |
| Body occupancy | 85.2% width x 88.9% height |
| Body mask bbox at 640x360 | x 95-639, y 0-319 |

## Acceptance Gates

| Gate | Status | Evidence |
| --- | --- | --- |
| Gate 0: Scope and label | Pass | New intentional files are in the isolated editor script path, Unity proof documentation folder, and allowed ConceptRenders names. The scene is temporary and unsaved. |
| Gate 1: Component count | Pass | Barrel, lower tank, readable gauge, coil window, muzzle stack, trigger/guard, grip/glove mass, ports, valves, plates, and 60+ fasteners are procedurally present. |
| Gate 2: Material and texture detail | Partial/fail | Unity materials use procedural base/normal/occlusion maps plus metallic/smoothness response, but this remains primitive geometry with no authored UV asset pass or production texture bake. |
| Gate 3: Camera and composition | Pass/partial | 3/4 first-person framing; muzzle left-forward, grip/glove lower-right, gauge and coil kept on visible near side. Body occupancy lands in the target range if the mask metric is trusted. |
| Gate 4: Lighting and contrast | Partial | Warm key, low cool fill, rim/specular accents, coil/gauge practicals, fog, and smoke are present, but there is no post stack or authored reflection setup. |
| Gate 5: Resolution and file checks | Pass | Hero is 1920x1080, contact sheet is above 1536x1024, names include Recovery04 and pressure_pistol, and this report/metrics file name the method and gaps. |
| Gate 6: Human review | Fail until reviewed | This is a stronger Unity proof workflow and should read closer than the rejected blockout, but it is not accepted final art without human visual approval against the north-star concept. |

## Honest Visual Read

The Unity proof should communicate the target silhouette better than prior blockouts: chunky first-person pistol, left-forward muzzle, large gauge, hot coil window, brass/iron layering, lower pressure tank, walnut grip, leather hand mass, visible ports, and smoke. It is still not good enough to promote as final high-fidelity weapon art because primitive cylinders/boxes cannot fully match the north-star concept's sculpted bevels, dense occlusion, worn hand-authored surface breakup, and physically richer metal/glass response.

## PM/user review rejection

Recovery04 is retained as a rejected full-gun proof. It failed visual review because smoke/steam rendered as opaque paper-like slabs, the camera was too cropped and side-on, materials skewed too orange, and several component silhouettes remained too boxy for the north-star concept target.
