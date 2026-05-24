# V0.1.45 Risk Matrix And Showcase Placements

Purpose: identify import risks and propose presentation-only placements for LevelAtmosphereSet03 and EnemyAnimationProxySet01. These placements are suggestions for main-lane `V0SceneBuilder.cs` work after package import validation passes.

## Risk Matrix

| Risk | Likelihood | Impact | Mitigation | Validation owner |
| --- | --- | --- | --- | --- |
| Parallel import waves collide with v0.1.44 manifest or validator changes | Medium | High | Import after v0.1.44 is committed or consciously batch both waves into one coordinated main-lane edit. | Main lane |
| Atmosphere props overcrowd already readable v0 corridors | Medium | Medium | Use one or two compact showcase placements per level first; keep dense combo prefabs off critical combat sightlines. | Level owner |
| Hanging chains or overhead canopies imply collision or block headroom visually | Medium | Medium | Keep all instances showcase-only, above or beside the path, and validate from player camera height in Unity. | Level owner |
| Floor drains imply walkable/collision detail that does not exist | Low | Medium | Place as presentation surfaces only; do not use as holes, traps, pickups, or route blockers. | Design owner |
| Pressure lamps imply live lighting systems | Medium | Medium | Treat lamps as emissive material shells only until lighting pass owns real Light components and performance budgets. | Art integration |
| Enemy animation proxies are mistaken for gameplay-ready enemies | Medium | High | Keep proxies as static visual references; do not bind AI, colliders, hitboxes, nav, damage, spawn waves, or animator controllers. | Gameplay owner |
| Placeholder AnimationClips are mistaken for approved animation timing | Medium | Medium | Validate clips only for import loadability; track rigging and timing separately in animation production. | Animation owner |
| Proxy materials read too flat or too bright under final lighting | High | Medium | Review in Unity at level camera height and track material remap/polish tasks separately. | Art integration |
| Rollback leaves generated scenes with missing package references | Medium | High | Keep all new instances under named showcase roots and remove package refs plus matching placements/validator entries together. | Main lane |

## Showcase Rules

- Put every imported visual under `Sidecar Quarantine Showcase - <LevelXX>`.
- Keep using `StripSidecarPresentationPhysics` or equivalent cleanup so showcase instances add no colliders, rigidbodies, autonomous audio sources, triggers, AI controllers, damage scripts, hitboxes, pickups, or gameplay authority scripts.
- Do not place atmosphere props across doors, pickups, level transitions, combat telegraphs, route chokepoints, or enemy spawn reads.
- Do not place enemy proxies where the player can confuse them with live combatants unless labels and visual quarantine context are clear.
- Keep labels and required names consistent with current `SidecarVisual_<Level>_<Name>` and `SidecarMaterialSwatch_<Level>_<Name>` validator patterns.

## Proposed Placements

| Level | Placement name | Asset path | Position | Rotation | Scale | Reason |
| --- | --- | --- | --- | --- | ---: | --- |
| Level01 | `SCLAPressureLampWallCagedA` | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_PressureLamp_WallCaged_A.prefab` | `(-5.22, 1.15, 10.10)` | `(0, 90, 0)` | 0.52 | Early warm lamp read without adding real light authority. |
| Level01 | `EAP01ScrapperAshcanIdleBrace` | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_ScrapperAshcan_01_IdleBrace.prefab` | `(5.12, 0.12, 11.40)` | `(0, -45, 0)` | 0.44 | First small enemy silhouette comparison in a safe showcase alcove. |
| Level02 | `SCLASteamPipeWallLeakerA` | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_SteamPipeCluster_WallLeaker_A.prefab` | `(-5.15, 0.50, 12.70)` | `(0, 90, 0)` | 0.44 | Pipeworks steam-leak silhouette at route edge. |
| Level02 | `EAP01LancerPressureAimLine` | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_LancerPressureSpindle_01_AimLine.prefab` | `(5.18, 0.12, 14.10)` | `(0, -72, 0)` | 0.40 | Ranged windup pose readability without combat authority. |
| Level03 | `SCLAHangingChainsTripleSlack` | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_HangingChains_TripleSlack.prefab` | `(-4.95, 2.05, 15.80)` | `(0, 90, 0)` | 0.46 | Vertical factory clutter and headroom review. |
| Level03 | `EAP01BulwarkHammerRaise` | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_BulwarkGatehammer_02_HammerRaise.prefab` | `(5.05, 0.12, 16.90)` | `(0, -58, 0)` | 0.38 | Heavy enemy pre-attack silhouette review. |
| Level04 | `SCLAOverheadPipeValveRun` | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_OverheadPipeCanopy_ValveRun.prefab` | `(-3.20, 2.65, 20.10)` | `(0, 0, 0)` | 0.42 | Ceiling density check away from route blockers. |
| Level04 | `EAP01WardenGovernorSignalRaise` | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_WardenGovernor_02_SignalRaise.prefab` | `(5.18, 0.12, 21.10)` | `(0, -64, 0)` | 0.36 | Commander pose and halo silhouette review. |
| Level05 | `SCLADenseAmbienceCorridorBite` | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_DenseAmbienceCombo_CorridorBite.prefab` | `(-5.05, 0.45, 23.20)` | `(0, 90, 0)` | 0.34 | Stress-test dense atmosphere under final-route lighting. |
| Level05 | `EAP01ScrapperSawLunge` | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_ScrapperAshcan_03_SawLunge.prefab` | `(5.08, 0.12, 24.10)` | `(0, -52, 0)` | 0.42 | Attack-pose readability in the highest-pressure route context. |

## Suggested `V0LevelValidator` Required Names

Add these after placements are finalized:

- Level01: `SCLAPressureLampWallCagedA`, `EAP01ScrapperAshcanIdleBrace`
- Level02: `SCLASteamPipeWallLeakerA`, `EAP01LancerPressureAimLine`
- Level03: `SCLAHangingChainsTripleSlack`, `EAP01BulwarkHammerRaise`
- Level04: `SCLAOverheadPipeValveRun`, `EAP01WardenGovernorSignalRaise`
- Level05: `SCLADenseAmbienceCorridorBite`, `EAP01ScrapperSawLunge`

Keep existing required names for earlier sidecars. Raise minimum renderer counts only after Unity confirms the new placements render correctly and do not overcrowd each showcase root.
