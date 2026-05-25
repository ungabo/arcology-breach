# Unity-Owned Asset Quality Audit

Date: 2026-05-24  
Worker: Unity-only quality-pipeline audit  
Scope: local Unity project, Package Manager manifests, `AssetPacks`, project assets, Unity package cache, and local Asset Store cache. No imports or scene/package edits were performed.

## Executive Verdict

The fastest path toward the premium steampunk corridor is to use the existing Brassworks sidecar packages as a quarantined Unity package wave, then compose them in `roomtest` before touching main scenes. The local Asset Store cache has a few possible support packages, mostly postprocess, skybox, prototype industrial, and particle packs, but it does not contain an obvious premium steampunk or industrial dungeon kit that beats the project-owned sidecars.

The best immediate value is the Set11/Set12 corridor stack:

1. `SteamCorridorDressingHighFidelitySet11` for layered pipe/gauge/boiler/lamp depth.
2. `RoomMaterialSet10` plus `RoomSurfaceReliefSet11` for the dark wet masonry shell.
3. `SteamLightingFixtureSet12` for believable warm fixture anchors.
4. `HeroVaultDoorSet12` and/or `BrassworksDoorMechanismSet10` for the pressure-door focal point.
5. `SurfaceBreakupDecalSet12`, `SteamAtmosphereVfxSet10`, and `CorridorPropClusterSet12` for the history pass: grime, oil, haze, clutter, wet glints.

Important caveat: these are still Unity-generated/procedural sidecar assets. They can raise the current look quickly, but they do not fully close the AAA gap by themselves. The hard remaining gap is authored bevel/damage/sculpt quality, physically coherent wet lighting/reflection, and gameplay-safe volumetric atmosphere.

## Project And Cache Findings

- Unity version: `6000.4.6f1`, from `ProjectSettings/ProjectVersion.txt`.
- Active graphics settings appear to use the built-in pipeline (`m_CustomRenderPipeline: {fileID: 0}` in `ProjectSettings/GraphicsSettings.asset`), while editor lookdev scripts search for `Universal Render Pipeline/Lit` and `Universal Render Pipeline/Unlit`. Any import/lookdev pass must verify shader resolution in Unity before judging material quality.
- Main `Packages/manifest.json` already references 24 local sidecar packages under `../AssetPacks/...`.
- `Library/PackageCache` only showed built-in Unity modules plus `com.unity.ugui` and `com.unity.multiplayer.center`; no high-value art/render packages were found there.
- Local Asset Store cache exists at `C:/Users/Gabe/AppData/Roaming/Unity/Asset Store-5.x`.
- Main project already contains imported/staged materials and OBJ blockouts under `Assets/_Project/ArtStaging`, including `FinalMaterialsV1`, `MaterialsPBR`, `ModularKit`, enemy blockouts, weapon blockouts, signage decals, and HUD assets. These are useful as evidence/reference, but the current task should avoid editing or promoting them.

## Top Import Candidates

| Rank | Candidate | Immediate Value | Exact Local Path | UPM/Import Path |
|---:|---|---|---|---|
| 1 | Steam Corridor Dressing High Fidelity Set 11 | Highest direct corridor density: wall pipe runs, caged gaslights, boiler columns, valve banks, gauge clusters, grates, threshold trims, wet soot overlays. Prior north-star comparison says this fixes the flat-wall/depth problem. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.SteamCorridorDressingHighFidelitySet11` | `file:../AssetPacks/BrassworksBreach.SteamCorridorDressingHighFidelitySet11`; package name `com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11`; sample assets resolve as `Packages/com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11/Runtime/...` |
| 2 | Room Material Set 10 | Best accepted dark wet room material family: wet brick wall, sooted ceiling, wet flagstone, black mortar/grime, soot and dampness overlays. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.RoomMaterialSet10` | `file:../AssetPacks/BrassworksBreach.RoomMaterialSet10`; package name `com.brassworks.sidecar.room-material-set10` |
| 3 | Room Surface Relief Set 11 | Needed to move beyond flat material planes: modular brick wall relief, wet flagstone slabs, ceiling panels, mortar shadow cards, corner grime strips, raised trim. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.RoomSurfaceReliefSet11` | `file:../AssetPacks/BrassworksBreach.RoomSurfaceReliefSet11`; package name `com.brassworks.sidecar.room-surface-relief-set11` |
| 4 | Steam Lighting Fixture Set 12 | Best focused fixture pack: wall gaslight cage, hanging lantern, inspection lamp, gauge lamp mount, pipe bracket glow glass, switch box, soot halo plane. Lighting fixtures are essential for warm pools and wet reflections. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.SteamLightingFixtureSet12` | `file:../AssetPacks/BrassworksBreach.SteamLightingFixtureSet12`; package name `com.brassworks.sidecar.steam-lighting-fixture-set12` |
| 5 | Hero Vault Door Set 12 | Strongest single focal object for the north-star corridor: circular door slab, gear lock, radial braces, hinge pistons, riveted arch frame, gauges, couplers, assembled hero door. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.HeroVaultDoorSet12` | `file:../AssetPacks/BrassworksBreach.HeroVaultDoorSet12`; package name `com.brassworks.sidecar.hero-vault-door-set12` |
| 6 | Industrial Machinery Set 12 | Large silhouette machinery for background depth: boiler tank, flywheel generator, wall pump station, overhead pipe manifold, regulator console, machinery bay assembly. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.IndustrialMachinerySet12` | `file:../AssetPacks/BrassworksBreach.IndustrialMachinerySet12`; package name `com.brassworks.sidecar.industrial-machinery-set12` |
| 7 | Surface Breakup Decal Set 12 | High value polish layer: soot streaks, wet oil trails, damp puddles, light stains, leak grime, brass edge wear, chipped wall grime, scuffs, corner darkness, rust halos. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.SurfaceBreakupDecalSet12` | `file:../AssetPacks/BrassworksBreach.SurfaceBreakupDecalSet12`; package name `com.brassworks.sidecar.surface-breakup-decal-set12` |
| 8 | Corridor Prop Cluster Set 12 | Asymmetric corridor clutter: tool cart, coal crates, loose pipe bundle, oil cans, toolbox, cable coil, broken gear pile, placard, pressure canister rack, composed clutter cluster. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.CorridorPropClusterSet12` | `file:../AssetPacks/BrassworksBreach.CorridorPropClusterSet12`; package name `com.brassworks.sidecar.corridor-prop-cluster-set12` |
| 9 | Steam Atmosphere VFX Set 10 | Useful visual atmosphere cards: warm haze, backlit door fog, ceiling smoke, wet glints, light shafts, floor mist, pipe leak steam, heat shimmer proxy. Must be tested for sorting/overdraw. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.SteamAtmosphereVfxSet10` | `file:../AssetPacks/BrassworksBreach.SteamAtmosphereVfxSet10`; package name `com.brassworks.sidecar.steam-atmosphere-vfx-set10` |
| 10 | Brassworks Door Mechanism Set 10 | Good secondary door/mechanism kit: gear hubs, latch, crossbolts, piston braces, pressure lock, crank valve, hinge, rail, lamp capsule. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.BrassworksDoorMechanismSet10` | `file:../AssetPacks/BrassworksBreach.BrassworksDoorMechanismSet10`; package name `com.brassworks.sidecar.brassworks-door-mechanism-set10` |

## Other Useful Local Sidecar Packages

| Candidate | Use | Path |
|---|---|---|
| `PipeTankGaugeSet10` | Pipe/tank/gauge detail library if Set11 lacks specific silhouettes. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.PipeTankGaugeSet10` |
| `DoorVaultSet10` | Older pressure/vault door kit; useful if HeroVaultDoorSet12 has scale or shader issues. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.DoorVaultSet10` |
| `GrimeDecalWetnessSet10` | Previous wetness/grime pack; fallback or supplement to SurfaceBreakupDecalSet12. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.GrimeDecalWetnessSet10` |
| `GaslightPipeDressingSet10` | Older gaslight/pipe fixtures; fallback if Set12 fixture scale is wrong. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.GaslightPipeDressingSet10` |
| `SurfaceMaterialDetailSet08` | Broad material library: wet black stone, riveted iron, worn brass/copper, grime, enamel, oil, glass, furnace metal. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.SurfaceMaterialDetailSet08` |
| `PressurePistolHeroSet12` | Best visible first-person weapon candidate. Useful for render composition if the weapon appears in corridor screenshots. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.PressurePistolHeroSet12` |
| `MechanicalEnemyDetailSet12`, `MechanicalEnemyPartsSet07`, `ClockworkEnemyPartsSet09` | Mechanical enemy detail/parts if corridor screenshot includes an enemy silhouette. | `D:/__MY APPS/Unity Doom/AssetPacks/...` |
| `HudFeedbackOrnamentSet12` | HUD/UI polish only; not a corridor look fix. | `D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.HudFeedbackOrnamentSet12` |

## Local Asset Store Cache Candidates

No cached Asset Store package looks like a ready-made premium steampunk corridor/dungeon kit. These may still help in isolated tests:

| Candidate | Path | Value | Risk |
|---|---|---|---|
| Beautify 2 - Advanced Post Processing | `C:/Users/Gabe/AppData/Roaming/Unity/Asset Store-5.x/Kronnect/ShadersFullscreen Camera Effects/Beautify 2 - Advanced Post Processing.unitypackage` | Potential fast lookdev boost for bloom, tonemapping, sharpening, color grading. | Pipeline compatibility and licensing/project-policy check required; do not import into main project first. |
| Snaps Prototype Sci-Fi Industrial | `C:/Users/Gabe/AppData/Roaming/Unity/Asset Store-5.x/Asset Store Originals/3D ModelsEnvironmentsSci-Fi/Snaps Prototype Sci-Fi Industrial.unitypackage` | Blockout/proportion reference for industrial corridor kitbashing. | Prototype sci-fi style conflicts with steampunk target; likely too clean/futuristic. |
| Unity Particle Pack / Unity Particle Pack 5x | `C:/Users/Gabe/AppData/Roaming/Unity/Asset Store-5.x/Unity Technologies/Unity EssentialsSample Projects/Unity Particle Pack.unitypackage`; `.../Unity EssentialsAsset Packs/Unity Particle Pack 5x.unitypackage` | Steam/smoke/fire primitives for controlled Unity-only VFX tests. | Old package; may import legacy shaders/scripts and noise into project. Sidecar-only. |
| URP Vertical Fog | `C:/Users/Gabe/AppData/Roaming/Unity/Asset Store-5.x/LushkinR/Textures MaterialsShaders/URP Vertical Fog.unitypackage` | Potential fog layer reference if URP is adopted in an isolated project. | Current project settings show no custom render pipeline; direct import may not work. |
| Skybox packs | Multiple under `.../Textures MaterialsSkies/` | Background only, little value for enclosed corridor. | Low priority; not a corridor quality fix. |

## Gaps That Local Assets Do Not Fully Solve

- Authored AAA bevel/damage: local procedural meshes can stage the look, but the room shell still needs believable chipped masonry thickness, worn edges, dented metal, bent brackets, and non-repeating damage.
- Real wet-surface lighting: current sidecars include wetness cards and smooth materials; the premium read needs validated reflection probes/screen-space reflections or another Unity-native wet material setup.
- Gameplay-safe volumetric atmosphere: cards are useful for stills, but camera motion, transparency sorting, overdraw, enemy readability, and weapon/HUD overlap remain unproven.
- Unified shader pipeline: the project appears built-in, while multiple tools/packs mention URP shaders. This must be resolved in a sidecar validation project before judging material failures.
- Imported commercial art: no cached Asset Store steampunk/dungeon/industrial corridor kit was found. If the target is truly AAA, local project-owned procedural packs may need external authored Unity-compatible art later, but that is outside this Unity-only, docs-only audit.

