# V0.1.44 Risk Matrix And Showcase Placements

Purpose: identify import risks and propose presentation-only placements for ObjectivePropsSet02 and SteamVFXSet02. These placements are suggestions for main-lane `V0SceneBuilder.cs` work after package import validation passes.

## Risk Matrix

| Risk | Likelihood | Impact | Mitigation | Validation owner |
| --- | --- | --- | --- | --- |
| Parallel manifest or validator edits collide with v0.1.44 additions | Medium | High | Check `git status --short`; add only the two new package refs and two validator entries after coordination. | Main lane |
| OPS02 interactable props are mistaken for gameplay-ready objectives | Medium | High | Keep all instances quarantine-only; do not wire prompts, inventory, door state, bridge state, hoist state, triggers, or objective completion. | Design owner |
| OPS02 props imply false interaction on the critical route | Medium | Medium | Place near route edges, alcoves, or showcase plinths; avoid placing over real doors, lifts, secrets, and boss controls. | Level owner |
| OPS02 procedural proxy materials read too flat under final lighting | High | Medium | Run Unity lookdev in the actual generated levels and track texture/material authoring needs separately. | Art integration |
| Steam VFX particles occlude combat reads or pickup visibility | Medium | High | Use small counts in quarantine placements; avoid combat lanes and pickup silhouettes until tuning is owned. | Gameplay owner |
| Steam VFX particle timing/scale does not match sockets | Medium | Medium | Review muzzle, wall-hit, and boss accents from Unity camera positions before any gameplay binding. | VFX owner |
| Transparent steam materials render differently after pipeline changes | Medium | Medium | Validate shader/material fallback in the main project after import; keep shader remap as a promotion task. | Tools owner |
| New runtime identity scripts create compile/import issues | Low | High | Import both packages in the first manifest/validator batch and block scene work until Unity compiles cleanly. | Tools owner |
| Rollback leaves generated scenes referencing removed packages | Medium | High | Keep all new instances under named showcase roots and remove package refs plus matching placements/validator entries together. | Main lane |

## Showcase Rules

- Put every imported visual under `Sidecar Quarantine Showcase - <LevelXX>`.
- Keep using `StripSidecarPresentationPhysics` or equivalent cleanup so showcase instances add no colliders, rigidbodies, autonomous audio sources, triggers, or gameplay authority scripts.
- Do not place new objective props on real doors, locks, lifts, bridge controls, pickups, secrets, boss controls, or level transitions.
- Do not place steam VFX across active sightlines, combat lanes, pickups, damage telegraphs, or navigation choke points.
- Keep labels and required names consistent with current `SidecarVisual_<Level>_<Name>` and `SidecarMaterialSwatch_<Level>_<Name>` validator patterns.

## Proposed Placements

| Level | Placement name | Asset path | Position | Rotation | Scale | Reason |
| --- | --- | --- | --- | --- | ---: | --- |
| Level01 | `OPS02KeyedLockTriGearVault` | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_KeyedLock_TriGearVault.prefab` | `(-5.10, 0.10, 10.80)` | `(0, 48, 0)` | 0.38 | Early objective-lock readability without binding to progression. |
| Level01 | `BBSVFX02SteamVentSoftColumn` | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_SteamVent_SoftColumn.prefab` | `(5.15, 0.08, 12.30)` | `(0, -90, 0)` | 0.42 | Gentle steam column visibility at route edge. |
| Level02 | `OPS02ValvePanelTwinPressurePuzzle` | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_ValvePanel_TwinPressurePuzzle.prefab` | `(-5.22, 0.55, 9.70)` | `(0, 90, 0)` | 0.44 | Pipeworks valve puzzle language next to noninteractive wall space. |
| Level02 | `BBSVFX02PressureLeakRuptureCone` | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_PressureLeak_RuptureCone.prefab` | `(5.05, 0.70, 14.20)` | `(0, -90, 0)` | 0.34 | Pressure leak scale check without blocking the lane. |
| Level03 | `OPS02LiftCallStationBrassCage` | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_LiftCallStation_BrassCageUpDown.prefab` | `(5.18, 0.75, 15.30)` | `(0, -90, 0)` | 0.46 | Lift-call affordance comparison without lift authority. |
| Level03 | `BBSVFX02FurnaceBlastDoorBelch` | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_FurnaceBlast_DoorBelch.prefab` | `(-5.10, 0.42, 18.40)` | `(0, 90, 0)` | 0.30 | Furnace burst brightness and opacity review. |
| Level04 | `OPS02ActuatorBridgeThrowLever` | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_Actuator_BridgeThrowLever.prefab` | `(-5.18, 0.12, 18.60)` | `(0, 52, 0)` | 0.42 | Actuator silhouette review away from real bridge/door logic. |
| Level04 | `BBSVFX02SparkRicochetWallHit` | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_SparkRicochet_WallHit.prefab` | `(5.15, 1.05, 21.70)` | `(0, -90, 0)` | 0.36 | Wall-hit feedback read under Foundry lighting. |
| Level05 | `OPS02GovernorOverrideBossKillSwitch` | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_GovernorOverride_BossKillSwitch.prefab` | `(-5.20, 0.22, 22.40)` | `(0, 58, 0)` | 0.40 | Final objective control language near, but not on, boss route. |
| Level05 | `BBSVFX02BossPhaseGovernorOvercrank` | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_BossPhase_GovernorOvercrank.prefab` | `(5.08, 0.82, 23.60)` | `(0, -58, 0)` | 0.32 | Boss phase accent scale and opacity review. |

## Suggested `V0LevelValidator` Required Names

Add these after placements are finalized:

- Level01: `OPS02KeyedLockTriGearVault`, `BBSVFX02SteamVentSoftColumn`
- Level02: `OPS02ValvePanelTwinPressurePuzzle`, `BBSVFX02PressureLeakRuptureCone`
- Level03: `OPS02LiftCallStationBrassCage`, `BBSVFX02FurnaceBlastDoorBelch`
- Level04: `OPS02ActuatorBridgeThrowLever`, `BBSVFX02SparkRicochetWallHit`
- Level05: `OPS02GovernorOverrideBossKillSwitch`, `BBSVFX02BossPhaseGovernorOvercrank`

Keep existing required names for earlier sidecars. Raise minimum renderer counts only after Unity confirms the new placements render correctly and do not overcrowd each showcase root.
