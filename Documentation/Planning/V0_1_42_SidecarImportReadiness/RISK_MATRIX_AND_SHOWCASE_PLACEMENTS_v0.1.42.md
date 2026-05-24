# V0.1.42 Risk Matrix And Showcase Placements

Purpose: identify import risks and propose presentation-only placements for CorridorKitSet02, EncounterEnemySet02, and WeaponViewmodelSet03. These placements are suggestions for main-lane `V0SceneBuilder.cs` work after package import validation passes.

## Risk Matrix

| Risk | Likelihood | Impact | Mitigation | Validation owner |
| --- | --- | --- | --- | --- |
| Parallel manifest edits collide with the three new local package references | Medium | High | Add only the three v0.1.42 lines after checking `git status --short`; let Unity resolve before touching scenes. | Main lane |
| Package version formatting differs across roots | Medium | Low | Assert UPM name and manifest path first. Treat `0.1.41`, `0.1.41-p001`, and manifest `build_id: p001` as review metadata unless release policy requires an exact suffix. | Tools owner |
| CorridorKitSet02 modules are mistaken for gameplay-ready level collision | Medium | High | Keep SCK2 assets in the quarantine showcase only; do not use them for nav, occlusion, doors, cover, collision, or progression blockers. | Level owner |
| Corridor modules inflate renderer counts or crowd existing route readability | Medium | Medium | Place small representative modules at corridor edges and alcoves first; raise renderer thresholds only after visual review. | Level owner |
| SCK2 door visuals imply interaction state that does not exist | Medium | Medium | Label as visual-only; avoid placing door modules over real transitions or pressure-gate gameplay. | Design owner |
| EncounterEnemySet02 visual poses are confused with rigged combat enemies | Low | High | Keep EE02 instances detached from AI, damage, nav, hit proxies, animation, and combat spawners. | Gameplay owner |
| EE02 tells are visually stronger than current gameplay timing | Medium | Medium | Review in Unity at combat camera distance before promoting any silhouette to a rigging task. | Combat owner |
| WeaponViewmodelSet03 runtime assembly introduces compile/import issues | Low | High | Import WVM03 in the first manifest/validator batch and block scene work until Unity compiles cleanly. | Tools owner |
| WVM03 candidates are used before first-person socket review | Medium | High | Showcase only until hand scale, muzzle alignment, recoil clearance, reload clearance, and animation sockets pass. | Weapon owner |
| Package-local materials are procedural lookdev proxies | High | Medium | Validate contrast in Unity lighting; plan final material/texture replacement before production promotion. | Art integration |
| Rollback leaves generated scenes referencing removed packages | Medium | High | Keep all new instances under named showcase roots and rehearse removal of package refs plus showcase entries before signoff. | Main lane |

## Showcase Rules

- Put every imported visual under `Sidecar Quarantine Showcase - <LevelXX>`.
- Keep using `StripSidecarPresentationPhysics` or equivalent cleanup so showcase instances add no colliders, rigidbodies, or autonomous audio sources.
- Do not place new visuals on combat lanes, pickups, real doors, objective interactables, triggers, or level transitions.
- Prefer small representative placements over full-kit dumps; the goal is import/readiness confidence, not full level art replacement.
- Keep labels and required names consistent with the current `SidecarVisual_<Level>_<Name>` and `SidecarMaterialSwatch_<Level>_<Name>` validator patterns.

## Proposed Placements

| Level | Placement name | Asset path | Position | Rotation | Scale | Reason |
| --- | --- | --- | --- | --- | ---: | --- |
| Level01 | `SCK2ArchedDoorFrame` | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_Door_Frame_ArchedRiveted.prefab` | `(-5.35, 0.06, 8.65)` | `(0, 90, 0)` | 0.48 | Compare new corridor door language without replacing the pressure gate. |
| Level01 | `SCK2WallPanelRiveted` | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_WallPanel_RivetedBrass_2m.prefab` | `(5.25, 0.62, 12.20)` | `(0, -90, 0)` | 0.58 | Intake wall-density check. |
| Level01 | `WVM03PressurePistolFullA` | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_PressurePistol_FullAssembly_A.prefab` | `(-5.05, 1.18, 11.25)` | `(0, 42, 0)` | 0.42 | First-person pistol silhouette comparison. |
| Level01 | `EE02AshcanIdle` | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_AshcanReclaimer_A_IdleSawScout.prefab` | `(-5.12, 0.06, 14.40)` | `(0, 55, 0)` | 0.36 | Low melee silhouette beside early Scrapper readability. |
| Level02 | `SCK2CorridorStraightNorthStar` | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorStraight_4m_NorthStar.prefab` | `(-5.35, 0.04, 6.90)` | `(0, 90, 0)` | 0.32 | Pipeworks corridor shell scale check. |
| Level02 | `SCK2CeilingPipeRack` | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CeilingModule_PipeRack_4m.prefab` | `(0.00, 2.35, 10.40)` | `(0, 0, 0)` | 0.44 | Overhead density review without floor blockage. |
| Level02 | `EE02PressureSpindleNeedleTell` | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_PressureSpindle_B_NeedleThrustTell.prefab` | `(5.08, 0.06, 14.60)` | `(0, -58, 0)` | 0.40 | Lancer/thrust tell comparison. |
| Level02 | `WVM03AmmoPressureCell` | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_AmmoPressureCell_Single.prefab` | `(3.20, 1.16, 9.90)` | `(0, -25, 0)` | 0.54 | Ammo prop readability beside existing pressure cartridge language. |
| Level03 | `SCK2TJunctionPipeSpine` | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorTJunction_PipeSpine.prefab` | `(-5.30, 0.04, 12.80)` | `(0, 90, 0)` | 0.30 | Boilerheart junction shape review. |
| Level03 | `SCK2RoomWallGaugeNest` | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_RoomWallPanel_GaugeNest.prefab` | `(5.18, 0.55, 16.20)` | `(0, -90, 0)` | 0.52 | Gauge wall detail review under warm lighting. |
| Level03 | `EE02GatehammerShieldedIdle` | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_GatehammerBastion_A_ShieldedIdle.prefab` | `(-4.95, 0.06, 18.10)` | `(0, 48, 0)` | 0.34 | Large blocker silhouette before Foundry escalation. |
| Level03 | `WVM03ScattergunFullA` | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_Scattergun_FullAssembly_A.prefab` | `(4.90, 1.14, 20.05)` | `(0, -52, 0)` | 0.40 | Scattergun viewmodel comparison near Level03 weapon language. |
| Level04 | `SCK2BulkheadRoundDoor` | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_Door_BulkheadRound_3m.prefab` | `(-5.30, 0.04, 11.10)` | `(0, 90, 0)` | 0.42 | Foundry door massing review without binding to a transition. |
| Level04 | `SCK2FloorGratedWet` | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_FloorPanel_GratedWet_2m.prefab` | `(5.05, 0.03, 15.80)` | `(0, -90, 0)` | 0.66 | Floor material/readability check outside route center. |
| Level04 | `EE02GatehammerHammerTell` | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_GatehammerBastion_B_HammerBackswingTell.prefab` | `(-5.00, 0.06, 20.60)` | `(0, 54, 0)` | 0.34 | Hammer tell silhouette for Foundry combat readability. |
| Level04 | `WVM03BoltThrowerFullA` | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_BoltThrower_FullAssembly_A.prefab` | `(4.95, 1.14, 22.65)` | `(0, -56, 0)` | 0.38 | Long weapon rail and muzzle clearance review. |
| Level05 | `SCK2CrossJunctionCompassHub` | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorCrossJunction_CompassHub.prefab` | `(-5.35, 0.04, 10.80)` | `(0, 90, 0)` | 0.26 | Governor Core hub motif review. |
| Level05 | `SCK2NorthStarSignage` | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_Signage_NorthStarWayfinding.prefab` | `(5.15, 1.08, 13.80)` | `(0, -90, 0)` | 0.58 | Wayfinding language near final route. |
| Level05 | `EE02GovernorWardenBell` | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_GovernorWarden_B_BellBeaconCastTell.prefab` | `(5.00, 0.06, 20.80)` | `(0, -58, 0)` | 0.36 | Governor elite support tell comparison. |
| Level05 | `WVM03GaugeClusterTriple` | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_GaugeCluster_Triple.prefab` | `(-5.20, 1.18, 23.20)` | `(0, 62, 0)` | 0.54 | Weapon/gauge detail read under final lighting. |

## Suggested `V0LevelValidator` Required Names

Add these after placements are finalized:

- Level01: `SCK2ArchedDoorFrame`, `WVM03PressurePistolFullA`, `EE02AshcanIdle`
- Level02: `SCK2CorridorStraightNorthStar`, `SCK2CeilingPipeRack`, `EE02PressureSpindleNeedleTell`, `WVM03AmmoPressureCell`
- Level03: `SCK2TJunctionPipeSpine`, `SCK2RoomWallGaugeNest`, `EE02GatehammerShieldedIdle`, `WVM03ScattergunFullA`
- Level04: `SCK2BulkheadRoundDoor`, `SCK2FloorGratedWet`, `EE02GatehammerHammerTell`, `WVM03BoltThrowerFullA`
- Level05: `SCK2CrossJunctionCompassHub`, `SCK2NorthStarSignage`, `EE02GovernorWardenBell`, `WVM03GaugeClusterTriple`

Keep the current existing required names for earlier sidecars. Raise minimum renderer counts only after Unity confirms the new placements render correctly and do not over-crowd each showcase root.
