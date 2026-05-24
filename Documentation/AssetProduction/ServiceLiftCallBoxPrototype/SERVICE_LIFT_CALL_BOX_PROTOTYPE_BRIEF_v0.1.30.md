# ServiceLiftCallBoxPrototype Production Brief v0.1.30

Promotion version: v0.1.30  
Component name: ServiceLiftCallBoxPrototype  
Geometry source: Unity-owned geometry only  
Production state: documentation_ready; implementation_pending; validation_pending

## Purpose

`ServiceLiftCallBoxPrototype` is a compact, route-safe transition dressing component for Brassworks Breach service lifts, hoists, and final-exit machinery. It gives each lift-adjacent route a readable steampunk control fixture without changing route geometry, triggers, objectives, enemy pressure, combat balance, player interaction, or level-transition authority.

The promoted asset should read as a worn industrial call box bolted beside a lift frame or pressure wall: blackened iron backplate, aged brass lever, cream gauge, colored service lamps, short pressure pipes, stamped label, visible rivets, and subtle oil or scorch wear. It must remain visual dressing only. The existing transition trigger remains the only gameplay owner.

## North-Star Style

The call box should feel like a heavy brassworks control fixture that has survived heat, soot, condensation, and repeated maintenance:

- Blackened riveted-iron backplate with worn bright edges, dark seams, and mounted hardware.
- Aged brass pull lever, guard ring, hinge pins, trim, screws, and mounting collars.
- Cream enamel lift-pressure or hoist-status gauge with a dark needle.
- Amber ready lamp, red locked or pressure-denied lamp, and green service or exit lamp language where the nearby transition supports it.
- Short copper, brass, or blackened iron pressure pipes entering the box from a wall, conduit, or lift frame.
- Stamped label plate such as `LIFT PRESSURE`, `HOIST CALL`, `SERVICE RELIEF`, or `MASTER HOIST`.
- Oil streaks, soot rubs, scorch marks, and grime plates around mounting bolts and lower pipe unions.
- Compact chunky silhouette that reads from player height but never becomes a route blocker, cover object, wall puzzle, or implied required interaction.

Avoid digital panels, neon sci-fi screens, floating UI icons, clean plastic buttons, fragile decorative filigree, or anything that contradicts the worn steampunk machinery language.

## Unity Ownership And Gameplay Contract

`ServiceLiftCallBoxPrototype` must be Unity-owned generated geometry for this promotion.

- No external mesh asset dependency.
- No colliders.
- No trigger volumes.
- No `NavMeshObstacle`.
- No route blocker, cover object, climb affordance, pickup, puzzle, or new interaction.
- No change to existing lift, hoist, final-exit, objective, or route-transition scripts.
- Existing route triggers remain the only gameplay authority.
- Lamps should prefer emissive or readable materials rather than extra dynamic lights.
- Optional spark, steam, or glow detail must remain ambient, low opacity, and non-obscuring.

The prop may visually imply lift readiness or pressure state, but it must not add a button prompt, alternate route, locked-door logic, damage hazard, pressure puzzle, or interactable control state.

## Expected Placement Roles

The v0.1.30 implementation should support these role identifiers exactly:

- `intake_service_lift_call_box`
- `pipeworks_service_lift_call_box`
- `boilerheart_service_lift_call_box`
- `foundry_emergency_hoist_call_box`
- `governor_master_hoist_call_box`

Recommended placement intent:

- `intake_service_lift_call_box`: mounted beside the service lift or lift-facing wall after the pressure-gate route, with clear amber/green readiness language and mineral-stained lower pipes.
- `pipeworks_service_lift_call_box`: mounted beside the Pipeworks lift to Boilerheart, outside the combat lane, with darker oil grime and denser pipe tie-ins.
- `boilerheart_service_lift_call_box`: mounted near the Boilerheart lift transition, with heat-scorched backplate edges and strong red/amber pressure-status language.
- `foundry_emergency_hoist_call_box`: mounted beside the emergency hoist path, on a foundry-safe wall or lift frame, with emergency red lamp language and heavier soot.
- `governor_master_hoist_call_box`: mounted beside the master override hoist or final exit, with more ceremonial brass trim, cleaner gauge face, and master-hoist label language.

## Named Hierarchy Target

Every promoted call box variant should expose a reviewable hierarchy:

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
- `Blackened Iron Backplate Edge Rail Left`
- `Aged Brass Pull Lever`
- `Aged Brass Lever Guard`
- `Cream Enamel Lift Pressure Gauge`
- `Dark Lift Gauge Needle`
- `Amber Lift Ready Lamp`
- `Red Lift Locked Lamp`
- `Green Service Lift Lamp`
- `Copper Pressure Feed Pipe A`
- `Copper Pressure Return Pipe B`
- `Stamped Brass Hoist Call Label`
- `Brass Call Box Rivet 00`
- `Oil Streak Plate Low`
- `Soot Scorch Plate Upper`

## Material-Role Coverage

At minimum, each role variant must visibly cover these material roles:

- `blackened_riveted_iron_backplate`
- `aged_brass_lever_and_trim`
- `cream_enamel_gauge_or_label`
- `amber_red_green_lamp_language`
- `copper_or_brass_pressure_pipes`
- `dark_hardware_rivets_screws`
- `oil_scorch_grime_marks`

Material names, renderer names, hierarchy names, or metadata should make these roles inspectable without subjective guessing.

## Minimum Counts

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

## Acceptance Gates

The v0.1.30 promotion gate should pass only when all of the following are true:

- Metadata component is present on the root or clearly associated metadata object.
- Metadata `promotionVersion` is exactly `v0.1.30`.
- Root object name includes `ServiceLiftCallBoxPrototype` and the exact placement role.
- All five expected placement roles are represented unless the main lane explicitly records a smaller scope.
- Required child roots or equivalent named children are present.
- Minimum counts pass for backplate, lever, gauge, lamps, pipes, label, fasteners, grime, and zero colliders/triggers.
- Material-role coverage includes blackened iron, aged brass, cream enamel, lamp language, copper or brass pipes, hardware, and wear.
- Placement is wall-adjacent or lift-frame-adjacent and does not narrow route width, cover access, doorway clearance, combat lanes, or final-exit readability.
- The prop does not imply an unimplemented button prompt, puzzle, lock override, required interaction, or hidden route.
- Red/green lamp language supports the nearby transition's existing route state and does not contradict it.
- Full V0 matrix passes after implementation.

## Validation Plan

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
- Confirm material names or child names include required role keywords: iron, brass, enamel or gauge, lamp, pipe, rivet or bolt, grime or oil or scorch.
- Confirm each expected scene placement exists in Level01 through Level05 if all five roles are selected.
- Confirm no new object in the call-box hierarchy has a gameplay interaction component unless a future slice explicitly owns that work.

## Expected Verification Artifacts

Expected validation artifacts after implementation enters the main lane:

- Scene rebuild: `Logs/v040-scene.log`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.30.md`
- Level validation: `Logs/v040-level-validation.log`
- Full matrix: `V0_BUILD_MATRIX_PASS`
- Windows executable: `Builds/Windows/v0.1.30/BrassworksBreach_v0.1.30.exe`
- Windows package: `Builds/WindowsPackages/v0.1.30/BrassworksBreach_v0.1.30_Windows.zip`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.30.md`
- Release notes: `Documentation/Releases/RELEASE_NOTES_v0.1.30.md`
- Package SHA-256 recorded after build.

## Non-Goals

This slice does not define final interactivity, lift control logic, physical buttons, VR hand interaction, audio state, dynamic light state, puzzle scripting, route unlock logic, damage, player prompts, new transition triggers, authored animation, or external mesh production. Those concerns must remain owned by separate gameplay, UX, VFX, audio, animation, or platform-specific systems if needed later.

## Handoff Status

- Documentation: documentation_ready.
- Implementation: implementation_pending.
- Validation: validation_pending.
