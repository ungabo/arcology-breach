# Brassworks Breach - v0.1.33 Visible Threshold And Route Dressing Milestone

Created: `2026-05-24T11:52:04-04:00`

Updated: `2026-05-24T12:04:00-04:00`

Owned scope: `Documentation/Planning/V0_1_33_BatchPlan/`

## Purpose

Plan `v0.1.33` as a visible milestone batch after the parent lane finishes `v0.1.32`. This is intentionally not a one-prop release. The batch should make the current five-level route feel meaningfully more authored in a single version by dressing pressure gates, service lifts, route valves, hoists, route returns, and final thresholds with a coordinated Unity-only steampunk environment pass.

The implementation posture is "big leaps per compile": build several related dressing families and grouped placements before each compile/validation checkpoint, use targeted checks during development, and run one full V0 matrix only after the batch is coherent.

This packet is documentation-only. The parent lane owns all Unity code, generated scenes, validators, shared status docs, release docs, test evidence, packaging, commits, and pushes.

## Inputs Reviewed

- `Documentation/ProductionManagement/BATCH_PRODUCTION_MODE.md`
- `Documentation/AssetProduction/PistonDoorBracePrototype/PistonDoorBracePrototype_ProductionBrief.md`
- `Documentation/AssetProduction/PistonDoorBracePrototype/PistonDoorBracePrototype_Status.json`
- `Documentation/VERSION_MICRO_ROADMAP.md`
- `Documentation/WORK_LEDGER.md`
- `Documentation/ArtDirection/UNITY_CONCEPT_MATCH_PRODUCTION_STANDARD.md`
- `Documentation/ArtDirection/UNITY_ASSET_ACCEPTANCE_GATES.md`
- Existing asset-promotion patterns from recent route-safe environment components

## Batch Goals

- Ship a broad 10-family environment/route-dressing milestone in one version, with a minimum acceptable reduction to 8 families only if targeted validation exposes route crowding.
- Make the route thresholds visibly heavier: pressure-door braces, sill plates, overhead clamps, wall repairs, grime, warning tags, and diegetic indicators should work together rather than feel like isolated props.
- Improve route comprehension through material language and placement rhythm, not new gameplay prompts or new route logic.
- Keep all components Unity-only: primitives, procedural mesh helpers, ProBuilder-style geometry if already available, generated quads/decals, and existing in-project materials. Do not use Blender or any new external DCC requirement.
- Preserve existing gameplay authority: no route-state ownership, no trigger volumes, no pickup authority, no transition logic, no enemy blockers, no damage behavior, and no nav obstacles.
- Validate in batches during development and run one route audit plus one full V0 matrix at the end.

## Milestone Component Families

The planned batch contains 10 component families or grouped placement families. These are meant to be implemented together as one milestone pass:

| ID | Component or grouped family | Batch role | Minimum target | Route-safe contract |
| --- | --- | --- | --- | --- |
| TRD-001 | `PistonDoorBracePrototype` | Heavy side braces at pressure gates, lift thresholds, hoists, and final route thresholds. | 5 threshold roles, reduce to 3 only if clearance fails. | Wall/threshold-adjacent visual geometry only; center aperture remains clear; no colliders or gameplay components. |
| TRD-002 | `PipeClampCouplerSetPrototype` | Extra pipe collars, flanges, short couplers, and clamp bands around existing route pipework. | 10 grouped placements across Level01-Level05. | Attach to existing wall, ceiling, or pipe dressing; no new pipe crosses walkable space. |
| TRD-003 | `OilSootGrimePanelSetPrototype` | Flat oil, soot, scorch, leak, and worn-edge panels near machinery, valve consoles, lift motors, and threshold seams. | 15 grouped placements, at least 3 per level. | Flush decals/quads or paper-thin panels; no collision; never hides pickups, hazards, enemies, prompts, or route-state cues. |
| TRD-004 | `AmberIndicatorPlatePrototype` | Small amber route-attention plates near pressure locks, valves, lifts, restored paths, and route returns. | 10 grouped placements across all levels. | Diegetic status dressing only; subtle emissive material if needed; no buttons, prompt ownership, or new dynamic lights. |
| TRD-005 | `BrassThresholdKickPlatePrototype` | Thin brass/blackened-iron sill and wall kick plates that sell repeated industrial traffic. | 8 threshold group placements. | Flush or near-flush trim; max visual rise target `0.03m`; no colliders, ramps, cover, or snag geometry. |
| TRD-006 | `RivetedPatchRepairPlatePrototype` | Bolted repair plates and stamped metal patches on route-adjacent wall panels. | 15 grouped placements. | Wall-bound only; noninteractive; no signage objective, prompt, route blocker, or route authority. |
| TRD-007 | `PressureSealGasketRingPrototype` | Rubberized/dark iron gasket rings and seal bands around locked gates, lift frames, and heavy machinery seams. | 6 grouped placements. | Surface-mounted dressing; does not change door frames or transition volumes. |
| TRD-008 | `RouteReturnPipeMarkerPrototype` | Short capped pipe stubs, elbow markers, and brass return-flow tags that rhythmically reinforce where pressure has been routed. | 8 grouped placements. | Uses existing route-state color language; no new interaction or objective logic. |
| TRD-009 | `SteamVentResidueCollarPrototype` | Soot-dark vent collars, heat halo plates, and condensation/oil marks near existing steam/furnace/relief machinery. | 8 grouped placements. | Visual residue only; does not add steam hazards, particles, damage, or visibility-blocking effects. |
| TRD-010 | `HoistChainAnchorPlatePrototype` | Wall/ceiling hoist anchor plates, chain brackets, pulley backers, and riveted mounts near lift/hoist thresholds. | 6 grouped placements. | Visual mount dressing only; no physics chains, no collision, no moving platforms, no transition authority. |

Minimum milestone cut line:

- Preferred ship target: all 10 families.
- Acceptable if route crowding appears: 8 families, preserving TRD-001 through TRD-006 plus at least 2 of TRD-007 through TRD-010.
- Not acceptable: fewer than 8 families, fewer than 4 levels touched, or any result that feels like a single isolated prop promotion.

## Suggested Placements Per Level

Use exact role identifiers where practical. The implementation can combine several small details under one grouped root when that reduces hierarchy noise, but each family still needs inspectable metadata and counts.

### Level01 - Brassworks Intake

- `PistonDoorBracePrototype_intake_pressure_gate_brace`: pressure-gate side walls after key acquisition, center aperture clear.
- `PipeClampCouplerSetPrototype_intake_gate_pipe_clamps`: pressure-gate feed pipes and service-lift pipework.
- `OilSootGrimePanelSetPrototype_intake_gate_lift_grime`: below call box, gate pipe seams, lift motor, and worn floor/wall contact points.
- `AmberIndicatorPlatePrototype_intake_key_gate_route_plates`: key-gate approach, gate state area, and service-lift ready line.
- `BrassThresholdKickPlatePrototype_intake_gate_lift_kick_plates`: pressure-gate threshold and lift apron.
- `RivetedPatchRepairPlatePrototype_intake_route_wall_patches`: noncritical wall returns and service alcove walls.
- `PressureSealGasketRingPrototype_intake_pressure_gate_seals`: existing gate frame seams, no aperture change.
- `RouteReturnPipeMarkerPrototype_intake_key_return_pipe_markers`: key-branch return-side pipe stubs and tags.
- `SteamVentResidueCollarPrototype_intake_relief_vent_residue`: near relief vent and gate pipe outlet details.
- `HoistChainAnchorPlatePrototype_intake_service_lift_anchors`: service-lift frame/upper wall, no moving or physics chain.

### Level02 - Pipeworks Annex

- `PistonDoorBracePrototype_pipeworks_boiler_lift_brace`: Boilerheart lift threshold, clear of locked-lift readability.
- `PipeClampCouplerSetPrototype_pipeworks_routing_valve_couplers`: routing valve, valve return pipes, and pipe canopy joints.
- `OilSootGrimePanelSetPrototype_pipeworks_valve_lift_grime`: below valve, Lancer-side machinery, and lift motor.
- `AmberIndicatorPlatePrototype_pipeworks_valve_route_plates`: locked lift to routing valve to restored lift route.
- `BrassThresholdKickPlatePrototype_pipeworks_lift_kick_plates`: lift apron and valve platform edge trim.
- `RivetedPatchRepairPlatePrototype_pipeworks_service_wall_patches`: side walls outside combat lanes.
- `PressureSealGasketRingPrototype_pipeworks_locked_lift_seals`: locked-lift frame seams.
- `RouteReturnPipeMarkerPrototype_pipeworks_restored_flow_markers`: routed pressure return line and restored path.
- `SteamVentResidueCollarPrototype_pipeworks_pipe_leak_residue`: harmless residue around non-damaging pipe leaks/vents.
- `HoistChainAnchorPlatePrototype_pipeworks_lift_anchors`: lift frame upper mount plates.

### Level03 - Boilerheart Core

- `PistonDoorBracePrototype_boilerheart_foundry_lift_brace`: Foundry lift threshold, set back from steam hazard lanes.
- `PipeClampCouplerSetPrototype_boilerheart_pressure_valve_couplers`: pressure-valve feed/return pipes and boiler ring pipework.
- `OilSootGrimePanelSetPrototype_boilerheart_valve_heat_grime`: valve machinery, boiler seams, scattergun display base edges, and lift motor, without obscuring pickup readability.
- `AmberIndicatorPlatePrototype_boilerheart_valve_lift_plates`: pressure-valve return and lift-ready language.
- `BrassThresholdKickPlatePrototype_boilerheart_lift_ring_kick_plates`: lift threshold and boiler ring threshold edges.
- `RivetedPatchRepairPlatePrototype_boilerheart_boiler_wall_patches`: scorched boiler walls and safe side panels.
- `PressureSealGasketRingPrototype_boilerheart_pressure_valve_seals`: valve housing and heavy pipe seals.
- `RouteReturnPipeMarkerPrototype_boilerheart_valve_return_markers`: valve-return route and foundry-lift lead.
- `SteamVentResidueCollarPrototype_boilerheart_steam_hazard_residue`: near existing steam hazard machinery only, never inside hazard read silhouettes.
- `HoistChainAnchorPlatePrototype_boilerheart_lift_anchors`: lift frame anchor plates.

### Level04 - Furnace Foundry

- `PistonDoorBracePrototype_foundry_emergency_hoist_brace`: emergency hoist approach, outside Bulwark and furnace hazard movement lanes.
- `PipeClampCouplerSetPrototype_foundry_heat_bypass_couplers`: heat-bypass pipework and pressure-relief connections.
- `OilSootGrimePanelSetPrototype_foundry_furnace_hoist_grime`: furnace machinery, emergency hoist, secret-cache machinery, and scorched wall seams.
- `AmberIndicatorPlatePrototype_foundry_hoist_route_plates`: emergency-hoist route and restored/exit cue walls.
- `BrassThresholdKickPlatePrototype_foundry_hoist_kick_plates`: hoist apron and safe service edge trim.
- `RivetedPatchRepairPlatePrototype_foundry_battered_wall_patches`: side walls, furnace service alcoves, and noncritical foundry panels.
- `PressureSealGasketRingPrototype_foundry_heat_gate_seals`: furnace-side pressure seams and hoist frame seals.
- `RouteReturnPipeMarkerPrototype_foundry_emergency_flow_markers`: safe hoist route and heat-bypass return details.
- `SteamVentResidueCollarPrototype_foundry_furnace_residue`: existing furnace/heat residue, never adding damage or obscuring hazard tells.
- `HoistChainAnchorPlatePrototype_foundry_emergency_hoist_anchors`: emergency-hoist frame and overhead anchor plates.

### Level05 - Governor Core

- `PistonDoorBracePrototype_governor_final_hoist_brace`: final hoist/master route threshold, outside Warden combat footprint.
- `PipeClampCouplerSetPrototype_governor_final_pressure_couplers`: final pressure lines, governor regulator dressing, and master hoist pipework.
- `OilSootGrimePanelSetPrototype_governor_warden_hoist_grime`: machinery bases and final-hoist supports without muddying boss readability.
- `AmberIndicatorPlatePrototype_governor_final_route_plates`: final-hoist and route-state walls; green only where existing restored/exit language supports it.
- `BrassThresholdKickPlatePrototype_governor_final_hoist_kick_plates`: final-hoist apron and master threshold trim.
- `RivetedPatchRepairPlatePrototype_governor_regulator_wall_patches`: noninteractive governor machinery walls.
- `PressureSealGasketRingPrototype_governor_master_hoist_seals`: master hoist frame and regulator seal bands.
- `RouteReturnPipeMarkerPrototype_governor_final_flow_markers`: final route pressure return and master override pipe markers.
- `SteamVentResidueCollarPrototype_governor_regulator_residue`: regulator residue and harmless vent collars.
- `HoistChainAnchorPlatePrototype_governor_final_hoist_anchors`: final-hoist support plates and upper brackets.

## Unity-Only Development Path

- Use Unity primitives, procedural mesh generation, ProBuilder-style geometry if already present, renderer naming, material instances, decals/quads, and editor-only helpers.
- Do not use Blender, Maya, Substance, Houdini, external mesh baking, or a new external art tool as a blocker for this version.
- Prefer repeated grouped roots and shared helper builders so the batch is a leap in visible density without becoming a hand-authored one-off maze of objects.
- Prefer emissive material accents over new dynamic lights. If a dynamic light is needed, it should be rare, justified, and included in validation.
- Use existing project material language: blackened iron, aged brass, copper pipe, cream/hazard enamel, amber glass, oil, soot, scorch, wet stone, and worn edges.
- Keep transparent grime/steam residue inexpensive and non-obscuring for later Android/WebGL reductions.

## Big-Leap Compile Cadence

Do not compile after every individual prop. The suggested cadence is:

1. **Compile A - Foundation families:** metadata batch ID, shared helper methods, validators, `OilSootGrimePanelSetPrototype`, `RivetedPatchRepairPlatePrototype`, and `PipeClampCouplerSetPrototype`.
2. **Compile B - Route language families:** add `AmberIndicatorPlatePrototype`, `BrassThresholdKickPlatePrototype`, `PressureSealGasketRingPrototype`, and `RouteReturnPipeMarkerPrototype`.
3. **Compile C - Threshold mass families:** add `PistonDoorBracePrototype`, `SteamVentResidueCollarPrototype`, and `HoistChainAnchorPlatePrototype`; rebuild all five scenes.
4. **Targeted validation pass:** run level validation and targeted route smokes, then tune/remove crowded placements.
5. **Final verification pass:** route audit and one full V0 matrix after the batch is coherent.

## Component Acceptance Gates

All promoted roots should follow the existing asset-promotion pattern:

- Root name contains the exact component name and placement role.
- Metadata exists on or clearly associated with each root, including component name, placement role, promotion version `v0.1.33`, and batch ID `v0.1.33_threshold_route_dressing_milestone`.
- Each component has a named hierarchy exposing major material and detail roles.
- All rendered geometry is Unity-owned.
- No hierarchy contains colliders, trigger colliders, `NavMeshObstacle`, route-state controllers, interactables, pickup scripts, damage scripts, transition scripts, objective scripts, physics chain components, or prompt ownership.
- No item implies a new required player interaction. Amber plates and lamps are diegetic route dressing, not buttons or quest markers.
- Material-role coverage includes blackened iron, aged brass, copper or pressure-pipe metal where relevant, amber glass/enamel where relevant, dark hardware, oil/soot/scorch wear, and worn edges.
- Detail counts are inspectable through child names, renderer names, materials, or metadata.

Component-specific minimums:

- `PistonDoorBracePrototype`: left and right brace assemblies; target center clearance at least `1.8m` wide and `2.4m` tall unless the existing threshold is already smaller; at least 2 cylinders, 2 brass rods, 12 rivets/bolts, 2 amber indicators, and 2 grime details.
- `PipeClampCouplerSetPrototype`: at least 4 clamp/coupler details and 8 fasteners per grouped placement.
- `OilSootGrimePanelSetPrototype`: at least 3 grime panels per grouped placement, with oil/soot/scorch placed according to gravity, heat, and mechanical contact.
- `AmberIndicatorPlatePrototype`: at least 2 plates and 4 fasteners per grouped placement; color language must match existing red locked, amber attention, and green restored/exit states.
- `BrassThresholdKickPlatePrototype`: at least 2 trim pieces and 6 fasteners per threshold role; near-flush and no snag geometry.
- `RivetedPatchRepairPlatePrototype`: at least 3 repair plates and 12 rivets/bolts per grouped placement.
- `PressureSealGasketRingPrototype`: at least 2 seal/gasket bands per placement; no aperture shrink and no route-state behavior.
- `RouteReturnPipeMarkerPrototype`: at least 2 pipe stubs/elbows/tags per placement; no objective logic.
- `SteamVentResidueCollarPrototype`: at least 2 residue collars or heat halo plates per placement; no particles, damage, or visibility-blocking steam unless a later VFX slice owns it.
- `HoistChainAnchorPlatePrototype`: at least 2 anchor plates/brackets and 8 fasteners per placement; no physics chain, no moving platform behavior.

## Batch Acceptance Gates

- The implementation visibly reads as a milestone route-dressing pass, not a single asset promotion.
- Preferred: all 10 families ship. Acceptable: 8 families if targeted validation proves crowding, preserving TRD-001 through TRD-006 and at least 2 of TRD-007 through TRD-010.
- Level01 through Level05 each receive at least 7 grouped placements from the batch.
- Piston braces appear on at least 3 current route thresholds, preferably all 5 listed threshold roles.
- Existing route triggers, pickups, locked gates, lift transitions, Warden defeat gating, enemy placement, weapon pickups, and hazard behavior remain authoritative and unchanged.
- Route audit reports no blockers after placement.
- Level validation checks expected roles, material roles, detail counts, metadata, and zero-collider/no-authority constraints.
- Targeted route smoke tests pass before the full V0 matrix.
- One full V0 matrix passes after the complete batch is assembled and tuned.

## Validation Strategy

Add validation as a batch-aware wrapper around component-specific checks:

```text
ValidateThresholdRouteDressingMilestone(sceneName, expectedRoles)
ValidatePistonDoorBracePrototype(sceneName, objectName, expectedRole)
ValidatePipeClampCouplerSetPrototype(sceneName, objectName, expectedRole)
ValidateOilSootGrimePanelSetPrototype(sceneName, objectName, expectedRole)
ValidateAmberIndicatorPlatePrototype(sceneName, objectName, expectedRole)
ValidateBrassThresholdKickPlatePrototype(sceneName, objectName, expectedRole)
ValidateRivetedPatchRepairPlatePrototype(sceneName, objectName, expectedRole)
ValidatePressureSealGasketRingPrototype(sceneName, objectName, expectedRole)
ValidateRouteReturnPipeMarkerPrototype(sceneName, objectName, expectedRole)
ValidateSteamVentResidueCollarPrototype(sceneName, objectName, expectedRole)
ValidateHoistChainAnchorPlatePrototype(sceneName, objectName, expectedRole)
```

Recommended validator coverage:

- Find each expected grouped root by exact object name.
- Confirm metadata component, component name, placement role, promotion version, and milestone batch ID.
- Confirm named child groups for structural plates, pipes/clamps, indicators, grime, fasteners, threshold trim, gasket/seal, return marker, vent residue, and hoist anchor roles as relevant.
- Count renderer-bearing children for required part types.
- Confirm material-role names through materials, renderer names, child names, or metadata.
- Confirm zero colliders, zero trigger colliders, zero `NavMeshObstacle`, and zero gameplay authority components under every dressing hierarchy.
- Confirm piston brace clearance markers or measured named bounds keep the center route visually and physically unobstructed.
- Confirm amber/green/red state language is consistent with existing route state and does not introduce conflicting signals.
- Confirm no dressing root is placed inside known transition, pickup, hazard, enemy, or boss-combat ownership volumes.
- Confirm the milestone family count and per-level grouped placement count meet the batch gates.

## Targeted Tests Before Full Matrix

Run these before the full matrix so the main lane can tune the batch cheaply:

1. Compile/editor validation after Compile A foundation families.
2. Compile/editor validation after Compile B route language families.
3. Compile/editor validation after Compile C threshold mass families and generated scene rebuild.
4. Level validation for Level01 through Level05 with milestone dressing checks enabled.
5. Targeted Level01 smoke: spawn to key gate to service lift, confirming gate/lift route readability and no snag geometry.
6. Targeted Level02 smoke: locked Boilerheart lift, routing valve, restored lift route, and Lancer lane visibility.
7. Targeted Level03 smoke: pressure valve, steam hazard lanes, scattergun pickup readability, Bellows Node space, and Foundry lift approach.
8. Targeted Level04 smoke: furnace hazard lanes, Bulwark combat footprint, secret-cache approach, and emergency hoist threshold.
9. Targeted Level05 smoke: Warden arena movement, locked final hoist, Warden defeat unlock, and final route readability.
10. Focused first-person visual review at each piston brace and each dense amber-plate cluster.
11. Route audit after targeted smoke passes.
12. One full V0 build matrix after the batch is stable.

## Implementation Order

1. Confirm `v0.1.32` is complete or intentionally superseded; do not start from a half-updated generated scene baseline.
2. Add the shared milestone batch ID, role registry, grouped placement table, and validator scaffolding.
3. Implement foundation families in one leap: grime panels, riveted repair plates, pipe clamps/couplers.
4. Implement route language families in one leap: amber plates, brass threshold kick plates, seal/gasket rings, return pipe markers.
5. Implement threshold mass families in one leap: piston door braces, steam vent residue collars, hoist chain anchor plates.
6. Rebuild Level01 through Level05 and run level validation.
7. Run targeted route smokes and first-person visual review.
8. Remove or thin any grouped placements that crowd route readability, hazard tells, pickup readability, combat movement, or final-hoist clarity.
9. Run route audit and one full V0 matrix.
10. Refresh release notes, candidate readiness, work ledger, roadmap, and session log in the main lane after the verified milestone batch is complete.

## Side-Agent Boundaries

- This side agent writes only inside `Documentation/Planning/V0_1_33_BatchPlan/`.
- This packet does not authorize edits to `Assets/`, `Tools/`, generated scenes, build scripts, release docs, shared status docs, or Git history.
- Parent/main lane owns actual Unity implementation, validators, generated scene files, test evidence, package artifacts, releases, commits, and pushes.
- Future side agents should receive disjoint folders or file sets and should prepare batch-ready briefs, acceptance gates, material recipes, or QA checklists without touching active implementation files.
- Do not revert or overwrite the parent agent's active `v0.1.32` work or unrelated user changes.

## Main-Lane Handoff Summary

Promote `v0.1.33` as a visible threshold-and-route dressing milestone: ship a Unity-only 10-family route dressing batch across Level01 through Level05, including `PistonDoorBracePrototype`, pipe clamps/couplers, oil/soot grime panels, amber indicator plates, brass threshold kick plates, riveted repair plates, seal gasket rings, route-return pipe markers, steam vent residue collars, and hoist chain anchor plates. Build in larger leaps per compile, run targeted validation during development, then run route audit and one full V0 matrix at the end.
