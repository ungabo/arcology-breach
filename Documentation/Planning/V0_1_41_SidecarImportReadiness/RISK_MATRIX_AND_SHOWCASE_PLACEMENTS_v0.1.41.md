# V0.1.41 Risk Matrix And Showcase Placements

Purpose: identify import risks and propose presentation-only placements for the four accepted sidecars. These placements are suggestions for main-lane `V0SceneBuilder.cs` work after package import validation passes.

## Risk Matrix

| Risk | Likelihood | Impact | Mitigation | Validation owner |
| --- | --- | --- | --- | --- |
| Manifest edit causes package resolution or compile failure | Medium | High | Add only the four local package references first, let Unity compile, then extend validators. Rollback by removing the new manifest lines. | Main lane |
| Material texture importer roles are wrong, especially `*_NRM.png` | Medium | Medium | Extend validator or manual QA to inspect `TextureImporter.textureType` for normal maps and material texture slots. | Main lane art integration |
| Materials use Standard/Lit fallback and may need render-pipeline conversion | Medium | Medium | Keep first import in quarantine; validate loadability and visual contrast before replacing existing primary materials. | Art integration |
| Level dressing prefabs increase renderer count and scene clutter | Medium | Medium | Place only curated showcase prefabs first; defer gameplay collision/nav blocking until promotion. | Level owner |
| Soot/grime decal planes do not match final decal system | Medium | Low | Treat as visual markers during quarantine; remap or rebuild through primary decal tooling during promotion. | Art integration |
| Enemy visual candidates are mistaken for gameplay-ready enemies | Low | High | Keep zero gameplay authority: no AI, damage, movement, nav, colliders, or rigidbodies from sidecar prefabs. | Gameplay owner |
| Weapon props are used before hand scale, muzzle alignment, and sockets are checked | Medium | High | Showcase only until first-person camera, recoil clearance, and animation socket review pass. | Weapon owner |
| Showcase validator only requires one asset name per level today | High | Medium | Update `V0LevelValidator` to require one representative from each newly imported package family per intended level, not only a single legacy asset. | Main lane |
| Package versions use `p001` build IDs inconsistently | Low | Low | Validator should assert package name and manifest path, not block on exact version string unless release policy requires it. | Tools owner |
| Rollback leaves generated scene references to removed packages | Medium | High | Validate rollback by removing package refs in a throwaway branch/worktree and opening all generated scenes after rebuild. | Main lane |

## Showcase Rules

- Put every imported visual under `Sidecar Quarantine Showcase - <LevelXX>`.
- Continue using `StripSidecarPresentationPhysics` or equivalent checks so showcase instances have no colliders, rigidbodies, or autonomous audio sources.
- MaterialsSet01 has no prefabs; show it through simple non-colliding swatch cubes or by assigning materials to small review plinths under the showcase root.
- Keep sidecar visuals at corridor edges or alcoves, not on combat lanes, pickups, doors, or transition triggers.
- Label presentation roots with existing `WorldLabelBillboard` readability metadata.

## Proposed Placements

| Level | Placement name | Asset path or material | Position | Rotation | Scale | Reason |
| --- | --- | --- | --- | --- | ---: | --- |
| Level01 | `WPS02_PressurePistolFrame` | `Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_PressurePistol_Frame_A.prefab` | `(-4.85, 1.05, 10.75)` | `(0, 35, 0)` | 0.58 | Compare against current pressure pistol silhouette. |
| Level01 | `WPS02_GearKeyHousing` | `Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_GearKey_Housing.prefab` | `(-5.15, 1.18, 12.15)` | `(0, 70, 0)` | 0.72 | Objective prop readability near key/gate flow. |
| Level01 | `MSET01_AgedBrassSwatch` | `MSET01_MAT_AgedBrass.mat` on non-colliding cube | `(-5.35, 1.10, 13.20)` | `(0, 90, 0)` | 0.45 | Brass contrast benchmark. |
| Level01 | `MSET01_OilyIronSwatch` | `MSET01_MAT_OilyBlackenedIron.mat` on non-colliding cube | `(-5.35, 1.10, 13.75)` | `(0, 90, 0)` | 0.45 | Dark metal contrast benchmark. |
| Level02 | `SCLD_PipeJunctionT` | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_PipeJunction_T_2m.prefab` | `(-5.35, 0.20, 7.80)` | `(0, 90, 0)` | 0.65 | Pipeworks density candidate. |
| Level02 | `SCLD_PressureTankFloorLarge` | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_PressureTank_FloorLarge.prefab` | `(5.25, 0.10, 10.80)` | `(0, -90, 0)` | 0.75 | Large dressing scale check. |
| Level02 | `SCLD_ValveClusterWall` | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_ValveCluster_Wall_1m.prefab` | `(5.35, 1.10, 12.40)` | `(0, -90, 0)` | 0.80 | Valve readability near objective language. |
| Level02 | `MSET01_SteamPipePatinaSwatch` | `MSET01_MAT_SteamPipePatina.mat` on non-colliding cube | `(-5.35, 1.10, 14.00)` | `(0, 90, 0)` | 0.45 | Pipe material benchmark. |
| Level03 | `WPS02_ScattergunTwinBoiler` | `Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_Scattergun_Body_TwinBoiler.prefab` | `(4.95, 1.18, 12.70)` | `(0, -55, 0)` | 0.58 | Compare against steam scattergun pickup flow. |
| Level03 | `SCLD_BoilerGaugePedestal` | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_BoilerGaugePedestal.prefab` | `(-4.95, 0.10, 14.20)` | `(0, 70, 0)` | 0.75 | Boilerheart instrumentation candidate. |
| Level03 | `MEV01_BellowsPressureNode` | `Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_BellowsSupport_B_PressureNode.prefab` | `(-4.85, 0.08, 18.40)` | `(0, 55, 0)` | 0.62 | Support-node silhouette beside current Bellows gameplay. |
| Level03 | `MSET01_BoilerCeramicSwatch` | `MSET01_MAT_BoilerCeramic.mat` on non-colliding cube | `(4.90, 1.08, 19.35)` | `(0, -55, 0)` | 0.45 | Furnace/ceramic material benchmark. |
| Level04 | `MEV01_BulwarkShieldBoiler` | `Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_BulwarkFurnace_A_ShieldBoiler.prefab` | `(-5.00, 0.08, 21.80)` | `(0, 54, 0)` | 0.56 | Foundry blocker silhouette comparison. |
| Level04 | `SCLD_VentStackFloor` | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_VentStack_Floor.prefab` | `(5.05, 0.10, 16.20)` | `(0, -90, 0)` | 0.72 | Steam hazard dressing candidate. |
| Level04 | `SCLD_DrainChannel` | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_DrainChannel_2m.prefab` | `(5.05, 0.04, 18.10)` | `(0, -90, 0)` | 0.85 | Floor readability and path risk check. |
| Level04 | `MSET01_WetStoneSwatch` | `MSET01_MAT_WetStone.mat` on non-colliding cube | `(-5.20, 1.10, 23.10)` | `(0, 60, 0)` | 0.45 | Foundry floor material benchmark. |
| Level05 | `MEV01_WardenTall` | `Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_WardenOverseer_A_TallWarden.prefab` | `(5.05, 0.08, 18.25)` | `(0, -55, 0)` | 0.55 | Boss silhouette comparison. |
| Level05 | `MEV01_OverseerBell` | `Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_WardenOverseer_B_OverseerBell.prefab` | `(-5.25, 0.08, 21.25)` | `(0, 62, 0)` | 0.58 | Alternate elite silhouette comparison. |
| Level05 | `WPS02_TuningDialGaugePanel` | `Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_TuningDialGauge_Panel.prefab` | `(4.90, 1.20, 24.15)` | `(0, -55, 0)` | 0.75 | Governor instrumentation candidate. |
| Level05 | `MSET01_PressureGaugeGlassSwatch` | `MSET01_MAT_PressureGaugeGlass.mat` on non-colliding cube | `(-5.40, 1.12, 10.90)` | `(0, 90, 0)` | 0.45 | Gauge/glass material benchmark. |

## Suggested `V0LevelValidator` Assertions

Add per-level required showcase names after `V0SceneBuilder` placements are finalized:

- Level01: `SidecarVisual_Level01_WPS02_PressurePistolFrame`, `SidecarVisual_Level01_WPS02_GearKeyHousing`, and at least two `MSET01_` swatch objects.
- Level02: `SidecarVisual_Level02_SCLD_PipeJunctionT`, `SidecarVisual_Level02_SCLD_PressureTankFloorLarge`, and one `MSET01_` pipe material swatch.
- Level03: `SidecarVisual_Level03_WPS02_ScattergunTwinBoiler`, `SidecarVisual_Level03_SCLD_BoilerGaugePedestal`, `SidecarVisual_Level03_MEV01_BellowsPressureNode`.
- Level04: `SidecarVisual_Level04_MEV01_BulwarkShieldBoiler`, `SidecarVisual_Level04_SCLD_VentStackFloor`, `SidecarVisual_Level04_SCLD_DrainChannel`.
- Level05: `SidecarVisual_Level05_MEV01_WardenTall`, `SidecarVisual_Level05_MEV01_OverseerBell`, `SidecarVisual_Level05_WPS02_TuningDialGaugePanel`.

Keep the existing checks for minimum renderer count and zero colliders, rigidbodies, and audio sources.
