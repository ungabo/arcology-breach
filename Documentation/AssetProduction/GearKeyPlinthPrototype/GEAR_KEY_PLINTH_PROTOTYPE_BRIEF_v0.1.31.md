# GearKeyPlinthPrototype Production Brief v0.1.31

Promotion version: v0.1.31  
Component name: GearKeyPlinthPrototype  
Geometry source: Unity-owned generated geometry only  
Production state: documentation_ready; implementation_pending; validation_pending

## Purpose

`GearKeyPlinthPrototype` is a route-safe presentation plinth for the existing GearKey pickup in Brassworks Breach. It should make the key area feel intentional and valuable without becoming gameplay authority. The current GearKey pickup remains the only pickup object, the only interaction owner, and the only source of collection state.

The promoted asset should read as a heavy steampunk display pedestal: blackened iron base, aged brass gear cradle, cream enamel label or gauge detail, amber key lamp, visible rivets, and oil/scorch grime. It must be generated and owned inside Unity for this promotion. It must not introduce pickup logic, colliders, triggers, `NavMeshObstacle` components, route blocking, cover behavior, or unreachable geometry around the GearKey.

## North-Star Style

The plinth should look like an old brassworks reliquary built for an industrial key:

- Blackened iron pedestal body with worn edges, soot-dark seams, and heavy riveted construction.
- Aged brass gear cradle shaped like a partial toothed ring or nested gear bracket around, beside, or below the existing GearKey.
- Cream enamel label plate, small pressure gauge, or inset maker's dial that reads as old factory instrumentation.
- Amber key lamp with warm emissive material to draw attention to the pickup without adding required dynamic light behavior.
- Visible rivets, bolts, screw heads, brass collars, and plate seams.
- Oil streaks, scorch smudges, grime plates, and rubbed metal highlights around the base and cradle.
- Compact readable silhouette from player height, with enough negative space that the actual GearKey remains visible and reachable.

Avoid sci-fi terminals, clean museum vitrines, glass cases, floating UI markers, magical effects, ornate fantasy altar language, or anything that implies a new puzzle, lock, trap, or alternate interaction.

## Unity Ownership And Gameplay Contract

`GearKeyPlinthPrototype` must be Unity-owned generated geometry only for this promotion.

- No external mesh asset dependency.
- No new pickup logic.
- No new interactable scripts.
- No colliders.
- No trigger volumes.
- No `NavMeshObstacle`.
- No blocking or narrowing of the player route.
- No step-up, climb, cover, physics, damage, puzzle, lock, or objective behavior.
- No change to GearKey scripts, pickup state, objective state, inventory state, route scripts, or combat logic.
- The existing GearKey pickup remains gameplay authority and must remain reachable by the player.
- The plinth may visually frame the GearKey, but it must not enclose, occlude, hide, lift out of reach, or move the pickup unless the main lane explicitly verifies reachability.
- Any lamp, glow, steam, spark, or grime effect must remain ambient, non-obscuring, and non-interactive.

## Intended Placement

The required first placement is:

- `level01_gear_key_plinth`: Level01 existing gear-key pickup area, aligned to support the current GearKey pickup without changing pickup behavior or route clearance.

Placement intent:

- Anchor the plinth under or immediately beside the existing GearKey pickup so the key reads as a deliberate brassworks route item.
- Keep the GearKey visible from the approach path and reachable at the existing player interaction distance.
- Keep all plinth geometry outside the pickup's required access volume.
- Preserve route readability, enemy movement assumptions, and player retreat paths in the existing Level01 gear-key area.

Optional future placements are documentation-only and not required for v0.1.31:

- `pipeworks_spare_gear_key_plinth`: an inactive or empty plinth for future environmental storytelling.
- `foundry_master_key_plinth`: a larger ceremonial variant if a later slice introduces a foundry key beat.
- `governor_archive_key_plinth`: a cleaner late-game variant for archive or control-room dressing.

Future placements must remain separate production work unless the main lane explicitly expands scope.

## Proposed Metadata Fields

Each promoted plinth root should expose metadata equivalent to:

```text
componentName: GearKeyPlinthPrototype
promotionVersion: v0.1.31
placementRole: level01_gear_key_plinth
geometrySource: UnityGenerated
gameplayAuthority: ExistingGearKeyPickup
pickupLogicAdded: false
collidersAllowed: false
triggersAllowed: false
navMeshObstacleAllowed: false
requiresGearKeyReachabilityCheck: true
logPrefix: v041
```

Recommended optional metadata:

- `intendedScene`: `Level01`
- `pickupAuthorityObject`: exact existing GearKey pickup object name after implementation discovery.
- `routeSafetyNotes`: short note confirming clearance and reachability.
- `materialRoles`: array of required material role identifiers.
- `validatorName`: `ValidateGearKeyPlinthPrototype`

## Named Hierarchy Target

The v0.1.31 root should use this pattern:

```text
GearKeyPlinthPrototype_[role]
  geometry
    pedestal_blackened_riveted_iron
    cradle_aged_brass_gear_ring
    label_cream_enamel_key_plate
    lamp_amber_key_beacon
    hardware_rivets_bolts_screws
    trim_brass_collars_and_edges
    grime_oil_scorch_wear
  metadata
```

Required child roots:

- `Pedestal Root`
- `Gear Cradle Root`
- `Label Gauge Root`
- `Amber Lamp Root`
- `Rivet Hardware Root`
- `Brass Trim Root`
- `Grime Wear Root`
- `Metadata Root`

Example named parts:

- `Blackened Iron Gear Key Pedestal Body`
- `Blackened Iron Pedestal Base Plate`
- `Blackened Iron Upper Plinth Cap`
- `Aged Brass Gear Cradle Ring`
- `Aged Brass Gear Tooth 00`
- `Aged Brass Key Support Bracket Left`
- `Aged Brass Key Support Bracket Right`
- `Cream Enamel Gear Key Label`
- `Cream Enamel Mini Pressure Gauge`
- `Dark Gauge Needle`
- `Amber Gear Key Lamp Lens`
- `Amber Lamp Brass Bezel`
- `Brass Plinth Rivet 00`
- `Blackened Iron Mounting Bolt 00`
- `Oil Streak Plate Front Low`
- `Soot Scorch Smudge Rear`
- `Worn Bright Edge Highlight Left`

## Material-Role Coverage

At minimum, the accepted Level01 plinth must visibly cover these material roles:

- `blackened_riveted_iron_pedestal`
- `aged_brass_gear_cradle`
- `cream_enamel_label_or_gauge`
- `amber_key_lamp_emissive`
- `brass_trim_and_collars`
- `dark_hardware_rivets_bolts`
- `oil_scorch_grime_wear`

Material names, renderer names, hierarchy names, or metadata should make these roles inspectable without subjective guessing.

## Minimum Counts

For each accepted plinth variant:

- Pedestal body or base assembly: at least 1.
- Aged brass gear cradle ring, partial ring, or keyed bracket: at least 1.
- Gear teeth or gear-like brass accents: at least 8.
- Cream enamel label or gauge: at least 1.
- Amber lamp lens or beacon: at least 1.
- Brass trim, collars, or edge strips: at least 2.
- Rivets, bolts, or screws: at least 12.
- Oil, scorch, grime, soot, or worn-edge detail plates: at least 3.
- Existing GearKey pickup authority: exactly 1 reachable existing pickup in the placement area.
- New pickup components under the plinth hierarchy: exactly 0.
- Colliders under the plinth hierarchy: exactly 0.
- Trigger volumes under the plinth hierarchy: exactly 0.
- `NavMeshObstacle` components under the plinth hierarchy: exactly 0.

## Acceptance Gates

The v0.1.31 promotion gate should pass only when all of the following are true:

- Metadata component is present on the root or clearly associated metadata object.
- Metadata `promotionVersion` is exactly `v0.1.31`.
- Root object name includes `GearKeyPlinthPrototype` and the exact placement role.
- Required first placement `level01_gear_key_plinth` is represented in the existing Level01 gear-key pickup area.
- Optional future placements are not required for the v0.1.31 pass.
- Required child roots or equivalent named children are present.
- Minimum counts pass for pedestal, brass gear cradle, gear teeth, enamel label/gauge, amber lamp, trim, fasteners, and grime.
- Material-role coverage includes blackened riveted iron, aged brass, cream enamel, amber emissive lamp language, brass trim, dark hardware, and oil/scorch grime.
- Plinth hierarchy contains zero colliders, zero trigger colliders, zero `NavMeshObstacle` components, and zero pickup or interactable components.
- Existing GearKey pickup remains present, visible, reachable, and gameplay authoritative.
- The plinth does not move, replace, parent-disable, occlude, enclose, or otherwise invalidate the existing GearKey pickup.
- Player route clearance around the Level01 gear-key area is unchanged or explicitly verified as non-blocking.
- Full V0 matrix passes after implementation.

## Validation Plan

Add a dedicated validator similar to recent prop validators:

```text
ValidateGearKeyPlinthPrototype(sceneName, objectName, expectedRole)
```

Recommended checks:

- Find root by exact object name.
- Confirm metadata component, component name, placement role, promotion version, and `gameplayAuthority`.
- Confirm required child roots or required named parts.
- Count renderer-bearing children for pedestal, brass gear cradle, gear teeth, label/gauge, amber lamp, trim, rivets/hardware, and grime/wear.
- Confirm material names or child names include required role keywords: iron, brass, gear, enamel or gauge, amber or lamp, rivet or bolt, grime or oil or scorch.
- Confirm zero colliders, zero trigger colliders, and zero `NavMeshObstacle` components under the plinth hierarchy.
- Confirm zero pickup, interactable, objective, inventory, damage, physics, or route-control components under the plinth hierarchy.
- Confirm exactly one existing GearKey pickup remains in the Level01 placement area and is not owned by the plinth hierarchy.
- Confirm the GearKey pickup remains reachable by player interaction distance or equivalent existing pickup validation.
- Confirm the plinth does not narrow required route clearance, block retreat movement, create cover behavior, or affect enemy navigation assumptions.

## Expected Verification Artifacts

Expected validation artifacts after implementation enters the main lane:

- Scene rebuild: `Logs/v041-scene.log`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.31.md`
- Level validation: `Logs/v041-level-validation.log`
- GearKey plinth validation excerpt or dedicated log lines with prefix `v041`
- Full matrix: `V0_BUILD_MATRIX_PASS`
- Windows executable: `Builds/Windows/v0.1.31/BrassworksBreach_v0.1.31.exe`
- Windows package: `Builds/WindowsPackages/v0.1.31/BrassworksBreach_v0.1.31_Windows.zip`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.31.md`
- Release notes: `Documentation/Releases/RELEASE_NOTES_v0.1.31.md`
- Package SHA-256 recorded after build.

## Non-Goals

This slice does not define new pickup behavior, alternate key variants, inventory changes, objective changes, route unlock logic, puzzle logic, collider setup, dynamic lighting requirements, VFX scripting, audio cues, animation, external mesh production, authored prefabs outside generated Unity geometry, or changes to the existing GearKey pickup. Those concerns must remain separate gameplay, UX, VFX, audio, animation, or production slices if needed later.

## Handoff Status

- Documentation: documentation_ready.
- Implementation: implementation_pending.
- Validation: validation_pending.
