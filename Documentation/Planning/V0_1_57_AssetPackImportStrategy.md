# V0.1.57 Asset Pack Import Strategy

Date: 2026-05-24  
Scope: docs-only strategy for testing locally available Unity assets without polluting main scenes, package manifests, or runtime code.

## Goal

Use Unity-only local packages to produce one credible premium steampunk corridor lookdev slice faster, while preserving the main game and current parallel work. The import test must happen in `roomtest` or a disposable sidecar validation project first.

## Import Rules

- Do not edit `Packages/manifest.json`, `Packages/packages-lock.json`, main scenes, shared source, or asset pack roots during the test.
- Do not copy assets into `Assets/_Project` during the first pass.
- Do not promote any prefab to gameplay until it passes visual review, shader/material review, scale/camera review, performance/overdraw review, and rollback review.
- Prefer a disposable Unity validation project or `roomtest` with local package references.
- Keep all render evidence under documentation or `roomtest/Renders`; keep runtime imports quarantined.

## Minimal Test Stack

### Pass 1 - Corridor Mood Cell

Import into an isolated test only:

1. `com.brassworks.sidecar.room-material-set10`
2. `com.brassworks.sidecar.room-surface-relief-set11`
3. `com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11`
4. `com.brassworks.sidecar.steam-lighting-fixture-set12`
5. `com.brassworks.sidecar.hero-vault-door-set12`

Target scene:

- New `roomtest` scene or sidecar validation scene, not a main campaign scene.
- Small first-person corridor cell: wet floor, dark masonry walls/ceiling, one hero pressure door, two wall fixture zones, one ceiling pipe zone, one sidewall boiler/gauge cluster.
- Use player-height camera framing and one weapon/HUD overlay check if possible.

Acceptance bar:

- No magenta materials.
- No missing meshes/textures.
- Warm fixture pools read clearly on wet floor.
- Door is a visible destination, not buried under pipes.
- Sidewalls have depth from player height, not just flat surface noise.
- Scene remains readable with a weapon silhouette present.

### Pass 2 - History And Dirt Layer

Add only after Pass 1 looks coherent:

1. `com.brassworks.sidecar.surface-breakup-decal-set12`
2. `com.brassworks.sidecar.steam-atmosphere-vfx-set10`
3. `com.brassworks.sidecar.corridor-prop-cluster-set12`

Test requirements:

- Place grime and wetness around believable causes: lamp soot, pipe leaks, floor traffic, door threshold puddles, rivet rust, corner darkness.
- Use no more than one composed clutter cluster in the first shot; keep combat path and door silhouette clean.
- Test fog/steam from at least three camera positions for sorting and readability.
- Capture before/after renders so art review can identify whether the layer improves premium feel or just adds noise.

### Pass 3 - Optional Background Depth

Add only if the cell still feels shallow:

1. `com.brassworks.sidecar.industrial-machinery-set12`
2. `com.brassworks.sidecar.brassworks-door-mechanism-set10`
3. `com.brassworks.sidecar.pipe-tank-gauge-set10`

Use these as background/sidewall silhouettes, not as a new focal point. The door remains the composition anchor.

## Representative Asset Paths For First Test

Use these as the first smoke-load set in Unity:

| Package | Asset Path |
|---|---|
| `com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11` | `Packages/com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11/Runtime/Prefabs/SCDHF11_PREFAB_WallPipeRunLayered_A.prefab` |
| `com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11` | `Packages/com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11/Runtime/Prefabs/SCDHF11_PREFAB_CagedGaslight_Long_A.prefab` |
| `com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11` | `Packages/com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11/Runtime/Prefabs/SCDHF11_PREFAB_GaugeCluster_Triple_A.prefab` |
| `com.brassworks.sidecar.room-material-set10` | `Packages/com.brassworks.sidecar.room-material-set10/Runtime/Materials/RMS10_MAT_DarkWetBrickWall.mat` |
| `com.brassworks.sidecar.room-material-set10` | `Packages/com.brassworks.sidecar.room-material-set10/Runtime/Materials/RMS10_MAT_WetUnevenFlagstoneFloor.mat` |
| `com.brassworks.sidecar.room-surface-relief-set11` | `Packages/com.brassworks.sidecar.room-surface-relief-set11/Runtime/Prefabs/RSR11_PREFAB_01_WallPanel_ModularBrick_A.prefab` |
| `com.brassworks.sidecar.room-surface-relief-set11` | `Packages/com.brassworks.sidecar.room-surface-relief-set11/Runtime/Prefabs/RSR11_PREFAB_05_FloorSlab_WetFlagstone_A.prefab` |
| `com.brassworks.sidecar.steam-lighting-fixture-set12` | `Packages/com.brassworks.sidecar.steam-lighting-fixture-set12/Runtime/Prefabs/SLF12_PREFAB_01_WallGaslightCage.prefab` |
| `com.brassworks.sidecar.steam-lighting-fixture-set12` | `Packages/com.brassworks.sidecar.steam-lighting-fixture-set12/Runtime/Prefabs/SLF12_PREFAB_07_SootHaloDecalPlane.prefab` |
| `com.brassworks.sidecar.hero-vault-door-set12` | `Packages/com.brassworks.sidecar.hero-vault-door-set12/Runtime/Prefabs/HVDS12_HeroVaultDoor_Assembly.prefab` |

Second-pass smoke-load set:

| Package | Asset Path |
|---|---|
| `com.brassworks.sidecar.surface-breakup-decal-set12` | `Packages/com.brassworks.sidecar.surface-breakup-decal-set12/Runtime/Prefabs/SBD12_PREFAB_03_WetOilTrail_BlackSlick.prefab` |
| `com.brassworks.sidecar.surface-breakup-decal-set12` | `Packages/com.brassworks.sidecar.surface-breakup-decal-set12/Runtime/Prefabs/SBD12_PREFAB_17_CornerDarkness_CoolMold.prefab` |
| `com.brassworks.sidecar.steam-atmosphere-vfx-set10` | `Packages/com.brassworks.sidecar.steam-atmosphere-vfx-set10/Runtime/Prefabs/SAV10_PREFAB_BacklitDoorFog.prefab` |
| `com.brassworks.sidecar.steam-atmosphere-vfx-set10` | `Packages/com.brassworks.sidecar.steam-atmosphere-vfx-set10/Runtime/Prefabs/SAV10_PREFAB_WarmGaslightHaze.prefab` |
| `com.brassworks.sidecar.corridor-prop-cluster-set12` | `Packages/com.brassworks.sidecar.corridor-prop-cluster-set12/Runtime/Prefabs/CPC12_PREFAB_CorridorClutterCluster_A.prefab` |

## Sidecar Setup Options

Preferred option:

1. Duplicate or create a disposable Unity validation project outside the main project.
2. Add local package references from `D:/__MY APPS/Unity Doom/AssetPacks/...`.
3. Build the corridor cell and capture renders.
4. Delete the validation project after artifacts are reviewed, or keep it as a clearly named quarantine project.

Acceptable option:

1. Use `D:/__MY APPS/Unity Doom/roomtest`.
2. Add package references only to the roomtest-side manifest if it has one, or use Unity Package Manager in that isolated project context.
3. Save scenes under `roomtest/Assets/RoomTest/Scenes`.
4. Save renders under `roomtest/Renders`.

Do not use the main project manifest as the first import surface for Set12 packages that are not already referenced.

## Render/Lookdev Checks

- Built-in-vs-URP check: confirm shader names resolve before blaming materials. Project settings currently indicate built-in render pipeline, while some lookdev code searches for URP shaders.
- Camera check: render at standing player height, crouch-ish height, and weapon-present FPS framing.
- Lighting check: compare warm lamp pools with and without wet floor materials; the north-star needs readable amber reflections without turning the scene orange.
- Density check: door silhouette, path clearance, enemy silhouette space, and objective/interactable readability must survive prop clutter.
- Transparency check: steam, fog, wet glints, and soot planes must not sort incorrectly during a simple forward/back camera move.
- Performance sanity: count lights, transparent planes, and high-density prefabs before promotion. Hero stills can be expensive; playable corridors cannot.

## Promotion Gate

Only after the isolated test passes:

1. Create a narrow main-lane import readiness packet listing the accepted package names, exact representative paths, and rollback steps.
2. Add only the minimal local packages to the main manifest in a separate implementation task.
3. Extend `SidecarQuarantineImportValidator` with representative asset checks.
4. Bind assets into one noncritical showcase corridor or roomtest-derived integration scene first.
5. Capture comparison screenshots against the north-star sheet and the previous rejected render.

## Explicit Gaps

- If the sidecar stack still reads as procedural, do not keep layering more procedural props. The missing ingredient is authored surface damage and mesh variation.
- If wetness still looks like cards, the next task is Unity-native material/lighting setup, not more decal imports.
- If steam looks good in stills but fails in motion, treat it as concept render support only.
- If Asset Store packages are considered, import only into a disposable project first. The cache has no obvious steampunk corridor pack, and old shader/effect packages may create more cleanup work than visual value.

