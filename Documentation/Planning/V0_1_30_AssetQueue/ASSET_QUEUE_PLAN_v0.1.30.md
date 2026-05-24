# Brassworks Breach - v0.1.30 Asset Queue Packet

Created: `2026-05-24T10:12:31-04:00`

Owned scope: `Documentation/Planning/V0_1_30_AssetQueue/`

## Purpose

This sidecar packet recommends the next route-safe Unity-owned modular prop slice for the main lane after the `v0.1.29` distribution-hardening pass. It is docs-only and does not authorize edits outside this planning folder.

Version targeting rule: use this packet for `v0.1.30` if `v0.1.29` lands the Windows distribution fallback. If the main roadmap keeps `v0.1.30` for more distribution work, use this packet for the first open asset-promotion slice immediately after that.

## Inputs Reviewed

- `Documentation/VERSION_MICRO_ROADMAP.md`
- `Documentation/WORK_LEDGER.md`
- `Documentation/AAA_ASSET_CATALOG.md`
- `Documentation/STEAMPUNK_NORTH_STAR.md`
- `Documentation/PARALLEL_ASSET_ACCEPTANCE_CHECKLIST.md`
- `Documentation/ArtDirection/UNITY_CONCEPT_MATCH_PRODUCTION_STANDARD.md`
- `Documentation/ArtDirection/UNITY_ASSET_ACCEPTANCE_GATES.md`
- `Documentation/Planning/V0_1_29_RouteTriage/ROUTE_TRIAGE_PLAN_v0.1.29.md`
- Recent asset-production briefs for `CatwalkRailPrototype`, `FloorDrainGratePrototype`, and `PressureTankRackPrototype`

## Queue Recommendation

| Rank | Candidate | Best use | Route risk | Recommendation |
| --- | --- | --- | --- | --- |
| 1 | `ServiceLiftCallBoxPrototype` | Wall-adjacent lift/hoist control dressing beside level transitions | Low | Selected next implementation slice |
| 2 | `GearKeyPlinthPrototype` | Objective-key presentation pedestal that frames the existing gear key pickup | Medium-low | Good follow-up after lift call boxes |
| 3 | `BoilerInspectionPanelPrototype` | Dense wall-only gauge/label/sight-glass dressing for boiler rooms and machinery corridors | Low | Safest fallback if transition-adjacent work is delayed |

## Selected Slice: ServiceLiftCallBoxPrototype

### Why This Is The Best Next Slice

`ServiceLiftCallBoxPrototype` gives high visual and gameplay-readability payoff while keeping the same low-risk contract as recent prop promotions. Every current level relies on service lifts, hoists, or final lift-style exits. A compact analog call box beside each transition will make those exits feel more authored and more steampunk without touching route geometry, enemy pressure, combat balance, or transition mechanics.

It also supports the long-term VR path: the prop can later become a hand-scale physical interaction surface with lever, gauge, and lamp states. For this slice, it remains visual dressing only; existing lift triggers and objective scripts continue to own gameplay.

### North-Star Style

The call box should feel like a worn industrial control fixture bolted to a lift frame or nearby pressure wall:

- Blackened riveted-iron backplate with soot-dark seams and worn bright edges.
- Aged brass lever, guard ring, hinge pins, trim, and screw heads.
- Cream enamel pressure dial or lift-status gauge with a dark needle.
- Amber ready lamp, red locked/pressure-denied lamp, and green service/exit lamp language where placement supports it.
- Short copper or brass pressure pipes entering the box from the wall or lift frame.
- Small stamped label plate such as `LIFT PRESSURE`, `HOIST CALL`, or `SERVICE RELIEF`.
- Oil streaks or scorch rubs around mounting bolts and lower pipe unions.
- Compact chunky silhouette that reads from player height but does not become a wall blocker or interactable puzzle.

Avoid digital panels, neon, clean sci-fi buttons, screen UI, floating icons, or fragile decorative filigree that would not survive a working brassworks.

### Unity Ownership And Gameplay Contract

`ServiceLiftCallBoxPrototype` should be Unity-owned generated geometry only for this promotion.

- No external mesh dependency.
- No colliders.
- No trigger volumes.
- No `NavMeshObstacle`.
- No new route blocker, cover object, climb affordance, pickup, or puzzle.
- No change to existing lift, hoist, final-exit, objective, or route-transition scripts.
- No new required player interaction.
- Existing route triggers remain the only gameplay authority.
- New lamps should prefer emissive/readable materials over extra dynamic lights.
- Optional steam or spark detail must remain ambient, low opacity, and non-obscuring.

### Expected Placement Roles

Use these exact role identifiers if promoted:

- `intake_service_lift_call_box`
- `pipeworks_service_lift_call_box`
- `boilerheart_service_lift_call_box`
- `foundry_emergency_hoist_call_box`
- `governor_master_hoist_call_box`

Recommended placements:

- Level01: beside the service lift or lift-facing wall after the pressure-gate route.
- Level02: beside the Pipeworks lift to Boilerheart, outside the combat lane.
- Level03: beside the Boilerheart lift transition to Foundry.
- Level04: beside the emergency hoist path, mounted on a foundry-safe wall or lift frame.
- Level05: beside the master override hoist/final exit, with a more ceremonial governor-grade trim variant.

### Named Hierarchy Target

Every promoted variant should expose a reviewable hierarchy:

```text
ServiceLiftCallBoxPrototype_[role]
  geometry
    backplate_blackened_riveted_iron
    lever_aged_brass
    gauge_cream_enamel_pressure
    lamps_amber_red_green_service
    pipes_copper_pressure_lines
    label_stamped_enamel_or_brass
    hardware_rivets_and_screws
    grime_oil_scorch_marks
  metadata
```

Recommended child roots:

- `Backplate Root`
- `Lever Root`
- `Gauge Root`
- `Lamp Root`
- `Pipe Root`
- `Label Root`
- `Rivet Root`
- `Grime Root`

Example named parts:

- `Blackened Iron Call Box Backplate`
- `Aged Brass Pull Lever`
- `Aged Brass Lever Guard`
- `Cream Enamel Lift Pressure Gauge`
- `Dark Lift Gauge Needle`
- `Amber Lift Ready Lamp`
- `Red Lift Locked Lamp`
- `Green Service Lift Lamp`
- `Copper Pressure Feed Pipe A`
- `Stamped Brass Hoist Call Label`
- `Brass Call Box Rivet 00`
- `Oil Streak Plate Low`

### Material-Role Coverage

At minimum, each role variant should visibly cover:

- `blackened_riveted_iron_backplate`
- `aged_brass_lever_and_trim`
- `cream_enamel_gauge_or_label`
- `amber_red_green_lamp_language`
- `copper_or_brass_pressure_pipes`
- `dark_hardware_rivets_screws`
- `oil_scorch_grime_marks`

Material names, renderer names, hierarchy names, or metadata should make these roles inspectable without subjective guessing.

### Minimum Counts

For each accepted role variant:

- Backplate: at least 1.
- Lever or switch handle: at least 1.
- Gauge or dial: at least 1.
- Lamps: at least 2, preferably amber plus green or red depending on placement state.
- Pressure pipes: at least 2 short pipe runs or pipe stubs.
- Label plate: at least 1.
- Rivets, screws, or mounting bolts: at least 8.
- Grime, oil, scorch, or wear plates: at least 2 subtle surface details.
- Colliders: exactly 0.
- Trigger volumes: exactly 0.
- `NavMeshObstacle` components: exactly 0.

### Acceptance Gates

- Metadata component or clearly associated metadata exists on the root.
- Metadata `promotionVersion` matches the implementation version chosen by the main lane.
- Root object name includes `ServiceLiftCallBoxPrototype` and the exact placement role.
- All five expected placement roles are represented unless the main lane explicitly scopes the slice to three or fewer levels and records that reduction.
- Required child roots or equivalent named children are present.
- Minimum counts pass for backplate, lever, gauge, lamps, pipes, label, fasteners, grime, and zero colliders/triggers.
- Material-role coverage includes blackened iron, aged brass, cream enamel, lamp language, copper/brass pipes, hardware, and wear.
- Placement is wall-adjacent or lift-frame-adjacent and does not narrow route width, cover access, doorway clearance, combat lanes, or final-exit readability.
- The prop does not imply an unimplemented button prompt, puzzle, or new interaction.
- If lamps show red/green state language, they must support the nearby transition's existing route state rather than contradict it.
- Full V0 matrix passes after implementation.

### Validation Checks For Main Lane

Add a dedicated validator similar to recent prop validators:

```text
ValidateServiceLiftCallBoxPrototype(sceneName, objectName, expectedRole)
```

Recommended checks:

- Find root by exact object name.
- Confirm metadata component, component name, placement role, and promotion version.
- Confirm required child roots or required named parts.
- Count renderer-bearing children for backplate, lever, gauge, lamps, pipes, label, rivets, and grime.
- Confirm zero colliders, zero trigger colliders, and zero `NavMeshObstacle` components under the hierarchy.
- Confirm material names or child names include required role keywords: iron, brass, enamel/gauge, lamp, pipe, rivet/bolt, grime/oil/scorch.
- Confirm each expected scene placement exists: Level01 through Level05 if all five roles are selected.
- Confirm no new object in the call-box hierarchy has a gameplay interaction component unless a future slice explicitly owns that work.

Expected broader verification:

- Scene rebuild log for the selected version.
- Route audit with no route-blocking scene composition issues.
- Level validation log including the call-box checks.
- Full V0 build matrix.
- Windows package and candidate-readiness evidence if the slice produces a packaged build.

## Candidate 2: GearKeyPlinthPrototype

Use this when the next art slice should improve objective pickup presentation.

Purpose: frame the existing gear-key pickup with a brass-and-iron pedestal, ring socket, enamel label, small gauge, and lamp language. It should improve first-objective readability without changing pickup trigger behavior.

Suggested roles:

- `intake_gear_key_plinth`
- `pipeworks_service_key_plinth`
- `foundry_cache_key_plinth`

Acceptance focus:

- No route blocking, no new trigger ownership, and no change to gear-key pickup logic.
- Existing pickup remains visible and reachable.
- Plinth does not look like cover or a required separate interaction.
- Minimum parts: base, brass socket ring, label, 8 fasteners, 2 lamp/status details, 2 pipe or conduit details, zero colliders.

Why it is not first: objective-pickup work sits closer to existing pickup trigger readability, so it carries slightly more gameplay-signaling risk than lift-side dressing.

## Candidate 3: BoilerInspectionPanelPrototype

Use this as the safest wall-only fallback.

Purpose: add dense steampunk wall machinery without touching transitions or pickups: inspection plate, sight glass, small pressure gauge, service labels, rivets, and short pipe stubs. This deepens corridor density while remaining noninteractive.

Suggested roles:

- `intake_boiler_inspection_panel`
- `pipeworks_boiler_inspection_panel`
- `boilerheart_boiler_inspection_panel`
- `foundry_heat_inspection_panel`

Acceptance focus:

- Wall-only placement, no collision, no trigger, no route-state contradiction.
- Must not duplicate the existing wall-pipe gauge cluster silhouette so closely that it reads as the same component.
- Minimum parts: backplate, sight glass, gauge, label, pipe stubs, fasteners, grime/wear, zero colliders.

Why it is fallback: it is very safe, but it adds less player-facing route clarity than the lift call box.

## Fallback Decision

If `ServiceLiftCallBoxPrototype` proves too route-adjacent, ambiguous, or costly during main-lane implementation, fall back to `BoilerInspectionPanelPrototype` for the same asset slice. It preserves the Unity-owned/no-collider validation pattern and can be placed on noncritical walls in Level01, Level02, and Level03 with minimal route exposure.

If both are blocked, keep the asset queue packet as planning evidence and let the main lane use the next version for docs-only roadmap reconciliation or distribution evidence hardening.

## Platform Notes

Windows source tier:

- Keep geometry compact and repeated-prop friendly.
- Prefer shared project materials and simple mesh primitives.
- Avoid new dynamic lights unless explicitly justified.

Android/WebGL reductions:

- Merge lamp/enamel/label details into fewer materials where possible.
- Use lower cylinder segment counts for pipes, rivets, gauges, and lever guards.
- Use emissive color instead of dynamic lights.
- Skip ambient steam if transparency budget is tight.

VR future:

- Keep call box at plausible hand height and scale.
- Avoid tiny progression-critical text.
- Do not require the player to look sharply behind them at exits.
- Preserve enough physical affordance for future hand interaction, but keep this slice noninteractive.

## Main-Lane Handoff

Recommended next implementation summary:

```text
Promote ServiceLiftCallBoxPrototype as Unity-owned, no-collider, wall/lift-frame-adjacent transition dressing in Level01 through Level05. Add metadata, named hierarchy, material-role coverage, detail-count validation, zero-collider validation, route audit, full V0 matrix, package, release notes, and status-doc updates for the selected version.
```

Next-step directive: once `v0.1.29` distribution hardening is complete, continue immediately with this selected asset slice unless a higher-priority route or release blocker appears.
