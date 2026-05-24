# PressureTankRackPrototype Production Brief v0.1.28

Promotion version: v0.1.28  
Component name: PressureTankRackPrototype  
Geometry source: Unity-owned geometry only  
Production state: integrated; implemented; full_matrix_passed

## Purpose

`PressureTankRackPrototype` is a compact route-safe industrial pressure tank rack dressing component for Brassworks Breach steampunk corridors. It gives intake machinery bays, pipeworks service runs, and governor-pressure corridors a readable cluster of stored pressure without changing player movement, collision, combat reads, traversal affordances, or route width.

The promoted asset should read as a wall-adjacent or alcove-adjacent rack of maintained pressure tanks: useful, bolted down, blackened by age and heat, fitted with brass service hardware, and clearly part of the pressure network. It must remain visual dressing only. The rack can suggest stored pressure, feeder routing, and maintenance access, but it must not read as an explosive pickup, destructible gameplay object, cover object, valve puzzle, or live hazard.

## North-Star Style

The rack should feel like steampunk plant infrastructure assembled from blackened iron, dark pressure cylinders, aged brass, soot-darkened hardware, and small pressure-service details:

- Blackened iron rack frame with worn edges, darker grime at joints, and visible bolted feet or wall brackets.
- Multiple dark pressure tanks held in the rack, with slightly varied soot, oil, and rub-wear so the cluster feels maintained rather than decorative.
- Aged brass bands, collars, and end caps on the tanks, with tarnish in recesses and brighter rubbed edges on service-facing surfaces.
- Small aged brass valves, stopcocks, or wheel handles mounted where feeder lines meet the tanks.
- Pressure tag, gauge, or small dial that identifies the cluster as pressure infrastructure at a glance.
- Rivets, bolts, and bracket hardware across the frame, feet, tank bands, and pipe clamps.
- Feeder pipes routed into or out of the rack, using aged brass, darkened copper, or blackened iron depending on placement.
- Optional pale ambient steam seep from one joint, clamp, or pipe union, only when it remains non-damaging, low-opacity, and route-safe.
- Slight asymmetry in grime, tank spacing, tag angle, valve orientation, and pipe bends while keeping the silhouette compact and repeatable.

The shape language should remain compact and corridor-safe: pressure storage rack, not barricade; ambience, not hazard; infrastructure dressing that reinforces the industrial setting without interrupting navigation.

## Unity Ownership And Gameplay Contract

`PressureTankRackPrototype` is Unity-owned geometry only.

- No external mesh asset dependency is required for acceptance.
- No colliders.
- No trigger volumes.
- No NavMeshObstacle components.
- No blocking behavior.
- No route narrowing, lane-width reduction, or step-up geometry.
- No cover behavior or intentional combat obstruction.
- No gameplay interaction, valve puzzle, pickup, loot, damage, explosion, pressure blast, gas leak, burn, poison, or traversal affordance.
- No authored animation dependency for acceptance.
- Optional steam seep puffs are ambient visual dressing only and must not imply damage, forced venting, heat pressure, poison gas, knockback, explosion risk, or visibility denial.

Placement must preserve the existing walkable envelope. The rack may visually mark pressure equipment, maintenance zones, or corridor utility infrastructure, but any future gameplay hazard, interaction, explosion, destructibility, or cover behavior must be owned by a separate gameplay system and receive separate validation.

## Expected Placement Roles

The v0.1.28 implementation should support these role identifiers exactly:

- `intake_pressure_tank_rack`
- `pipeworks_pressure_tank_rack`
- `governor_pressure_tank_rack`

Each role should share the same compact route-safe contract, while varying tank finish, brass polish, grime density, pipe routing, and pressure-reading details to match its destination:

- `intake_pressure_tank_rack`: cleaner service rack near intake machinery, cooler condensation streaks, mineral staining around feeder pipes, readable brass caps, and a simple pressure tag or gauge.
- `pipeworks_pressure_tank_rack`: denser feeder pipe integration, darker oil and condensate grime, serviceable valves, clamp hardware, and more traffic-worn rack edges.
- `governor_pressure_tank_rack`: pressure-regulation dressing near governor or control corridors, stronger gauge/tag presence, more brass banding, soot-darkened iron frame, and slightly more formal maintenance labeling.

## Named Hierarchy Target

Every promoted pressure tank rack variant should use a named hierarchy that is easy to inspect in Unity. Suggested structure:

```text
PressureTankRackPrototype_[role]
  geometry
    rack_blackened_iron_frame
    tanks_dark_pressure_cylinders
    bands_aged_brass_caps
    valves_small_aged_brass
    gauge_pressure_tag
    hardware_rivets_and_bolts
    feeder_pipes_service_lines
    ambience_steam_seep_optional
  metadata
```

The root object name must include `PressureTankRackPrototype` and the exact placement role. Child names must expose material or part intent clearly enough for review without opening meshes one by one.

Recommended implementation child roots:

- `Rack Root`
- `Tank Root`
- `Brass Band Root`
- `Valve Root`
- `Pressure Tag Root`
- `Feeder Pipe Root`
- `Rivet Root`
- `Ambient Steam Root`

Example named parts:

- `Blackened Iron Rack Upright Left`
- `Blackened Iron Rack Rail Upper`
- `Dark Pressure Tank 00`
- `Aged Brass Tank Band 00 A`
- `Aged Brass Tank Cap 00 Front`
- `Small Brass Valve 00`
- `Amber Pressure Gauge Dial`
- `Stamped Pressure Service Tag`
- `Blackened Iron Rack Bolt 00`
- `Aged Brass Pipe Clamp 00`
- `Dark Feeder Pipe Intake`
- `Pale Steam Seep Ambient Optional`

## Material-Role Coverage

At minimum, the promoted prototype must visibly cover these material roles:

- `blackened_iron_rack_frame`
- `dark_pressure_tank_shells`
- `aged_brass_bands_caps`
- `aged_brass_small_valves`
- `pressure_tag_gauge`
- `dark_hardware_rivets_bolts`
- `feeder_pipes_aged_brass_or_blackened_iron`
- `pale_steam_seep_ambient_optional`

Material assignments may use existing Brassworks Breach material conventions or Unity-owned generated materials, but the role coverage should remain inspectable from hierarchy names, mesh names, material names, or reviewer-facing metadata.

## Minimum Counts

For each accepted role variant:

- Rack frame: at least 1 continuous rack frame or 4 named frame members, such as uprights and rails.
- Pressure tanks: at least 3 visible dark tank cylinders for the standard compact rack.
- Brass bands or collars: at least 2 visible brass retaining bands per tank, or at least 6 total brass band/collar elements.
- Brass caps: at least 1 visible cap per tank, or at least 3 total cap elements if the rear caps are hidden against a wall.
- Small valves, stopcocks, or wheel handles: at least 2.
- Pressure tag, gauge, dial, or readable service marker: at least 1.
- Rivets, bolts, or clamp fasteners: at least 12 visible fasteners across frame feet, brackets, tank bands, or pipe clamps.
- Feeder pipes or service lines: at least 2, with one visually entering and one visually leaving the rack where placement allows.
- Optional ambient steam seep puffs: 0 or more, ambience only.
- Colliders: exactly 0.

Short alcove or narrow-wall variants may reduce the pressure tank count to 2 only if they still include the blackened iron frame, aged brass bands/caps, valve hardware, pressure tag or gauge, feeder pipes, visible fasteners, and the zero-collider route-safe contract.

## Acceptance Gates

The v0.1.28 promotion gate should pass only when all of the following are true:

- Metadata component is present on the root or clearly associated metadata object.
- Metadata `promotionVersion` is exactly `v0.1.28`.
- Hierarchy is named and includes `PressureTankRackPrototype` plus the exact placement role.
- All expected placement roles are represented: `intake_pressure_tank_rack`, `pipeworks_pressure_tank_rack`, and `governor_pressure_tank_rack`.
- Material-role coverage includes blackened iron rack frame, dark pressure tank shells, aged brass bands/caps, aged brass valves, pressure tag or gauge, hardware fasteners, feeder pipes, and optional ambient steam if present.
- Minimum counts are met for rack frame, tanks, bands/caps, valves, gauge/tag, fasteners, feeder pipes, and zero colliders.
- No colliders are present anywhere in the prototype hierarchy.
- No trigger volumes or NavMeshObstacle components are present.
- The rack reads as compact corridor or wall-adjacent dressing and does not become cover, a barricade, a destructible/explosive prop, a valve puzzle, loot marker, route marker for a hidden interaction, or a hazard telegraph.
- Placement preserves route width and does not narrow navigation, cover access, doorway clearance, ramps, stairs, readable combat lanes, or traversal sightlines.
- Optional steam remains pale, low-opacity, non-directional ambient seep only and does not obscure player view or imply damaging pressure.

## Non-Goals

This slice does not define final collision, cover behavior, destructibility, explosive reactions, pressure hazards, gas leaks, steam damage, valve interactions, puzzle logic, pickup or loot behavior, audio, authored particle timing, lighting effects, animation, or route blocking. Those concerns must remain owned by separate gameplay, VFX, audio, or level-design systems if needed later.

## Implementation Notes

This v0.1.28 handoff has been implemented and verified. The main lane promoted `PressureTankRackPrototype` as compact Unity-owned generated geometry with three placement roles:

- `intake_pressure_tank_rack`
- `pipeworks_pressure_tank_rack`
- `governor_pressure_tank_rack`

The implementation should prefer primitive or procedurally generated cylinders, boxes, torus-like band approximations, small valve handles, simple gauge/tag planes, and pipe sections that remain easy to inspect in hierarchy. Cylinders should be visually dark and industrial, with brass bands/caps carrying the steampunk read. The frame should sit tight to walls, alcoves, or machinery edges and should not protrude into movement lanes.

If implementation uses optional steam, keep it as non-collider ambient visual geometry or approved non-gameplay VFX. Steam should be subtle enough that route readability, enemy silhouettes, and interactable clarity are unaffected.

## Verification Artifacts

Expected validation artifacts after implementation enters the main lane:

- Scene rebuild: `Logs/v038-scene.log`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.28.md`
- Level validation: `Logs/v038-level-validation.log`
- Full matrix: `V0_BUILD_MATRIX_PASS`
- Windows executable: `Builds/Windows/v0.1.28/BrassworksBreach_v0.1.28.exe`
- Windows package: `Builds/WindowsPackages/v0.1.28/BrassworksBreach_v0.1.28_Windows.zip`
- Package SHA-256: `5899590751B66471916AFB833E5EF0F6BD358E33858EF2BDF727B0938463FD72`

These artifacts are complete for the verified v0.1.28 slice.

## Validation Plan

Validation should include:

- Metadata review for component name, placement role, and exact `v0.1.28` promotion version.
- Hierarchy review for named material and part roles.
- Material-role review for blackened iron, dark tanks, aged brass bands/caps, valves, gauge/tag, hardware, feeder pipes, and optional ambient steam.
- Count review for rack frame, pressure tanks, bands/caps, valves, gauge/tag, fasteners, feeder pipes, and zero colliders.
- Route audit in representative intake, pipeworks, and governor placements.
- Visual readability pass confirming the rack reads as compact pressure infrastructure dressing, not a destructible explosive object, interactable valve puzzle, cover object, or live steam hazard.
- Full validation matrix once implementation enters the main lane.

Validation passed for the v0.1.28 Unity implementation.

## Handoff Status

- Documentation: integrated.
- Implementation: implemented.
- Validation: full_matrix_passed.
