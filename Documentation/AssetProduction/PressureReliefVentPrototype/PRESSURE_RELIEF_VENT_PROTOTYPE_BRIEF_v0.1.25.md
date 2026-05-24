# PressureReliefVentPrototype Production Brief

Promotion version: v0.1.25  
Component name: PressureReliefVentPrototype  
Geometry source: Unity-owned geometry only  
Production state: promoted; implementation_complete; validation_passed

## North-Star Style

PressureReliefVentPrototype is a compact steampunk route-dressing component for Brassworks Breach. It should read as a pressure-management fixture bolted onto industrial walls, floors, pipe runs, and foundry-adjacent surfaces without changing player routing or combat affordances.

The visual target is:

- Blackened iron wall or floor mount plate with worn edges and heat-darkened grime.
- Aged brass vent stack or louvered exhaust face with simple Unity-owned primitive construction.
- Small copper relief pipe tied into the mount or vent body.
- Visible rivets or bolts around the plate and pipe brackets.
- Amber pressure tag or pointer that gives the asset an immediately readable pressure-relief identity.
- Optional pale steam puff geometry for non-damaging ambience only.

The asset should feel useful, bolted-in, and mechanical rather than decorative. It should support Brassworks Breach's brass, iron, copper, pressure, steam, and foundry language while staying small enough to repeat across industrial spaces.

## Route-Safe Dressing Rules

PressureReliefVentPrototype is route-safe dressing.

- No colliders on the promoted prefab or child geometry.
- Non-blocking by design.
- Must not narrow navigation, doorways, ramps, combat lanes, cover access, or traversal reads.
- Must not imply player damage, forced venting, knockback, burn zones, poison gas, or pressure hazards.
- Any future damaging or reactive behavior must be owned by a separate hazard script and separate validation pass.
- Steam puff geometry, if included, is pale ambient dressing only and must not communicate a live damage volume.

## Placement Roles

Expected placement roles for v0.1.25:

- intake_pressure_relief_vent: small vents mounted near intake machinery, air feeds, and service pipe clusters.
- pipeworks_pressure_relief_vent: repeated pipe-run dressing on corridor walls, risers, catwalk-adjacent pipe banks, and junction plates.
- foundry_pressure_relief_vent: hotter, sootier variants near foundry equipment, furnace-adjacent walls, and heavy machinery bays.

## Recommended Hierarchy

The promoted prefab hierarchy should be named and inspectable:

```text
PressureReliefVentPrototype
  Mount_BlackenedIron_Plate
  Vent_AgedBrass_StackOrLouver
  Pipe_Copper_Relief
  Hardware_RivetsBolts
  Indicator_Amber_PressureTagPointer
  Ambience_PaleSteamPuff_Optional
```

Child names may vary slightly to fit Unity conventions, but the hierarchy must preserve clear material roles and functional intent.

## Material Roles

Minimum material role coverage:

- blackened_iron_mount
- aged_brass_vent
- copper_relief_pipe
- dark_hardware_rivets_bolts
- amber_pressure_indicator
- pale_steam_ambient_optional

Materials may be shared project materials or Unity-owned generated materials, but the prefab should not require external mesh assets.

## Minimum Counts

Minimum visible composition counts for validation:

- Mount plate: at least 1.
- Vent stack or louvered exhaust: at least 1.
- Copper relief pipe: at least 1.
- Rivets or bolts: at least 4.
- Amber pressure tag or pointer: at least 1.
- Optional pale steam puff geometry: 0 or more, ambience only.
- Colliders: exactly 0.

## Acceptance Gates

The v0.1.25 promotion is accepted only when all gates pass:

- Metadata component present on the promoted prefab.
- Metadata promotion version is exactly `v0.1.25`.
- Root object is named `PressureReliefVentPrototype`.
- Hierarchy is named and separates mount, vent, pipe, hardware, indicator, and optional ambience roles.
- Geometry source is Unity-owned geometry only.
- Material roles are present and legible.
- Minimum counts are met.
- No colliders are present anywhere in the hierarchy.
- Route safety is preserved in representative placements.
- Placement roles include `intake_pressure_relief_vent`, `pipeworks_pressure_relief_vent`, and `foundry_pressure_relief_vent`.

## Implementation Notes

The v0.1.25 main lane promoted this component into generated Unity scenes as non-blocking route dressing. The full V0 matrix passed on `2026-05-24 08:56 -04:00`, including route audit, level validation, Windows build, runtime smoke, auto-playthrough, combat/readability/hazard/secret/settings coverage, Windows packaging, QA packet generation, issue-triage packet generation, and candidate-readiness evidence.

Verified placements:

- `North Star Intake Pressure Relief Vent` with `intake_pressure_relief_vent`.
- `North Star Pipeworks Pressure Relief Vent` with `pipeworks_pressure_relief_vent`.
- `North Star Foundry Pressure Relief Vent` with `foundry_pressure_relief_vent`.

Verification artifacts:

- `Builds/Windows/v0.1.25/BrassworksBreach_v0.1.25.exe`
- `Builds/WindowsPackages/v0.1.25/BrassworksBreach_v0.1.25_Windows.zip`
- `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.25.md`
- `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.25.md`
- `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.25.md`
- `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.25.md`

Package SHA-256: `97065409C8CDDA23686B307963DAA3973F6AC6672E85FC20D5B4EF481E6C0EB6`
