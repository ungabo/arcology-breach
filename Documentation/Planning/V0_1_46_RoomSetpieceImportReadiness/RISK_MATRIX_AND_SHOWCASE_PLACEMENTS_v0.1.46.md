# V0.1.46 Risk Matrix And Showcase Placements

Purpose: identify import risks and propose presentation-only placements for RoomSetpieceKit04. These placements are suggestions for main-lane `V0SceneBuilder.cs` work after package import validation passes.

## Risk Matrix

| Risk | Likelihood | Impact | Mitigation | Validation owner |
| --- | --- | --- | --- | --- |
| Package is imported while another worker is editing manifests or validator entries | Medium | High | Import only after current main-lane package work is stable, or batch intentionally in one coordinated compile slice. | Main lane |
| Large room setpieces block routes or confuse level boundaries | Medium | High | Place only in showcase alcoves, wall edges, ceiling zones, or non-route corners; review from player-height camera. | Level owner |
| Door alcove and threshold assets imply gameplay doors, locks, routes, or transitions | Medium | High | Treat as static visual shells; do not bind door, key, objective, transition, blocker, lift, or bridge logic. | Design owner |
| Catwalk and stair silhouettes imply traversable geometry | Medium | High | Keep off reachable paths and avoid using them to signal alternate routes until collision/nav authority is authored by main lane. | Design owner |
| Regulator machinery or clutter clusters hide combat reads, pickups, signs, or exits | Medium | Medium | Use small scales in quarantine showcases and keep the main route, sightlines, pickups, enemy telegraphs, and signage unobstructed. | Level owner |
| Ceiling pipe galleries visually reduce headroom or camera clarity | Medium | Medium | Mount above or beside routes only; test camera clearance and readability in Unity. | Art integration |
| Furnace glow, steam wisps, and warning gauges imply live VFX, lighting, audio, hazards, or objectives | Medium | Medium | Document as material/geometry placeholders only; future VFX, lighting, audio, hazard, and objective systems remain separate. | Art integration |
| Materials need render-pipeline remap during promotion | Medium | Medium | Validate import and visual fallback now; track final material conversion separately. | Art integration |
| Rollback leaves missing package references in generated scenes | Medium | High | Keep instances under named showcase roots; remove package ref, validator entry, placements, and required names together. | Main lane |

## Showcase Rules

- Put every imported visual under `Sidecar Quarantine Showcase - <LevelXX>`.
- Keep every RoomSetpieceKit04 instance visual-only: no colliders, rigidbodies, triggers, autonomous audio sources, AI controllers, hitboxes, damage scripts, pickups, objective scripts, doors, lifts, bridges, transitions, route blockers, or gameplay authority scripts.
- Do not place setpieces across doors, pickups, route signs, level exits, combat telegraphs, enemy spawns, secrets, pressure gates, boss mechanics, or narrow route chokepoints.
- Keep labels and required names consistent with current `SidecarVisual_<Level>_<Name>` validator patterns.
- Prefer one focused setpiece per level first. Add density only after validator and player-height lookdev are green.

## Proposed Placements

| Level | Placement name | Asset path | Position | Rotation | Scale | Reason |
| --- | --- | --- | --- | --- | ---: | --- |
| Level01 | `RSK04BoilerChamberWallBayA` | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_BoilerChamberWallBay_A.prefab` | `(-5.35, 0.25, 10.85)` | `(0, 90, 0)` | 0.46 | Early wall-bay read beside the route without creating a blocker. |
| Level02 | `RSK04PressureVaultDoorAlcoveA` | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_PressureVaultDoorAlcove_A.prefab` | `(5.35, 0.10, 13.65)` | `(0, -90, 0)` | 0.42 | Vault-door visual language in a showcase bay, explicitly not a door or transition. |
| Level03 | `RSK04CatwalkBalconyModuleA` | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_CatwalkBalconyModule_A.prefab` | `(-4.80, 1.15, 16.40)` | `(0, 90, 0)` | 0.38 | Vertical factory silhouette review while staying off traversable geometry. |
| Level04 | `RSK04PipeGalleryCeilingClusterA` | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_PipeGalleryCeilingCluster_A.prefab` | `(-3.10, 2.70, 20.25)` | `(0, 0, 0)` | 0.38 | Ceiling density and headroom readability check away from route blockers. |
| Level05 | `RSK04RegulatorCoreMachineryA` | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_RegulatorCoreMachinery_A.prefab` | `(5.20, 0.10, 23.95)` | `(0, -60, 0)` | 0.36 | Boss-route machinery showcase with no collision, hazard, or objective authority. |

## Alternate Rotation Candidates

Use these if the primary five placements overcrowd a specific level:

| Level | Placement name | Asset path | Use case |
| --- | --- | --- | --- |
| Level01 | `RSK04BrassFloorTrimThresholdA` | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_BrassFloorTrimThreshold_A.prefab` | Low-profile floor trim at a showcase edge, not across a route. |
| Level02 | `RSK04FurnaceControlWallA` | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_FurnaceControlWall_A.prefab` | Wall-mounted glow language without live light, audio, VFX, or hazard behavior. |
| Level03 | `RSK04ServiceStairSilhouetteA` | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_ServiceStairSilhouette_A.prefab` | Background silhouette only; avoid any placement that reads as a usable stair. |
| Level04 | `RSK04LargeWarningGaugeWallA` | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_LargeWarningGaugeWall_A.prefab` | Gauge wall readability without objective, pressure, or warning-system authority. |
| Level05 | `RSK04RoomCornerClutterClusterA` | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_RoomCornerClutterCluster_A.prefab` | Corner clutter stress sample if regulator machinery blocks too much sightline. |

## Suggested `V0LevelValidator` Required Names

Add these after placements are finalized:

- Level01: `RSK04BoilerChamberWallBayA`
- Level02: `RSK04PressureVaultDoorAlcoveA`
- Level03: `RSK04CatwalkBalconyModuleA`
- Level04: `RSK04PipeGalleryCeilingClusterA`
- Level05: `RSK04RegulatorCoreMachineryA`

Keep existing required names for earlier sidecars. Raise minimum renderer counts only after Unity confirms the new placements render correctly and do not overcrowd each showcase root.
