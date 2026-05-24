# FloorDrainGratePrototype Production Brief v0.1.27

Promotion version: v0.1.27  
Component name: FloorDrainGratePrototype  
Geometry source: Unity-owned geometry only  
Production state: integrated; implemented; full_matrix_passed

## Purpose

`FloorDrainGratePrototype` is a compact route-safe floor and wet-drain dressing component for Brassworks Breach steampunk corridors. It gives intake floors, pipeworks runs, and foundry-adjacent corridors a believable drainage and maintenance detail without changing player movement, collision, combat reads, traversal affordances, or route width.

The promoted asset should read as a low-profile industrial drain grate set into worn stone or metal floor plating: useful, bolted-in, damp, and mechanically maintained, but never hazardous or physically obstructive.

## North-Star Style

The drain grate should feel like steampunk plant infrastructure assembled from blackened iron, aged brass, wet stone, and visible hardware:

- Blackened iron grate frame with soot-darkened edges, worn bevels, and darker grime in the seams.
- Aged brass drain trim or lip around the frame, with tarnish in recesses and brighter rubbed edges.
- Slotted blackened iron grate bars with clear parallel slot language and a shadowed drain void below.
- Rivets, bolts, or screw heads at the corners and along side plates so the drain feels installed, not decorative.
- Thin oil and wet stone stain plates around the grate footprint, with subtle sheen and irregular pooling.
- Optional small pale steam seep puffs rising through a few slots, only when they remain non-damaging, low-opacity, and route-safe.
- Slight asymmetry in grime, oil, and water marks while keeping the silhouette clean enough to repeat across corridors.

The shape language should remain compact and floor-safe: drain fixture, not pit; ambience, not hazard; dressing that reinforces the industrial setting without interrupting navigation.

## Unity Ownership And Gameplay Contract

`FloorDrainGratePrototype` is Unity-owned geometry only.

- No external mesh asset dependency is required for acceptance.
- No colliders.
- No trigger volumes.
- No NavMeshObstacle components.
- No blocking behavior.
- No route narrowing, lane-width reduction, or step-up geometry.
- No gameplay interaction, loot, damage, slipping, fire, steam hazard, or traversal affordance.
- No authored animation dependency for acceptance.
- Optional steam seep puffs are ambient visual dressing only and must not imply damage, heat pressure, poison gas, knockback, or visibility denial.

Placement must preserve the existing walkable envelope. The grate may visually mark drainage, floor wear, or wet machinery zones, but any future gameplay hazard must be owned by a separate gameplay system and receive separate validation.

## Expected Placement Roles

The v0.1.27 implementation should support these role identifiers exactly:

- `intake_floor_drain_grate`
- `pipeworks_floor_drain_grate`
- `foundry_floor_drain_grate`

Each role should share the same compact route-safe contract, while varying wear, trim polish, grime density, and stain shape to match its destination:

- `intake_floor_drain_grate`: damp intake-floor drainage, mineral staining, cleaner brass trim, water sheen around the outer stone plates.
- `pipeworks_floor_drain_grate`: oil and condensate streaks under pipe runs, darker slot voids, serviceable bolts, slightly cleaner traffic-worn bars.
- `foundry_floor_drain_grate`: sootier blackened iron, darker wet stone plates, heat-tinted brass edges, heavier grime near bolts, no lava or burn-hazard read.

## Named Hierarchy Target

Every promoted drain grate variant should use a named hierarchy that is easy to inspect in Unity. Suggested structure:

```text
FloorDrainGratePrototype_[role]
  geometry
    frame_blackened_iron
    trim_aged_brass_drain_lip
    bars_blackened_iron_slotted
    void_shadowed_drain_recess
    hardware_rivets_and_bolts
    stain_plates_oil_wet_stone
    ambience_steam_seep_puffs_optional
  metadata
```

The root object name must include `FloorDrainGratePrototype` and the exact placement role. Child names must expose material or part intent clearly enough for review without opening meshes one by one.

## Material-Role Coverage

At minimum, the promoted prototype must visibly cover these material roles:

- `blackened_iron_frame`
- `aged_brass_drain_trim`
- `blackened_iron_slotted_bars`
- `shadowed_drain_void`
- `dark_hardware_rivets_bolts`
- `oil_wet_stone_stain_plates`
- `pale_steam_seep_ambient_optional`

Material assignments may use existing Brassworks Breach material conventions or Unity-owned generated materials, but the role coverage should remain inspectable from hierarchy names, mesh names, material names, or reviewer-facing metadata.

## Minimum Counts

For each accepted role variant:

- Drain frame: at least 1 continuous frame or 4 named frame rails.
- Brass drain trim or lip: at least 1 continuous trim element or 4 named trim rails.
- Slotted grate bars: at least 6 visible parallel bars.
- Shadowed drain void or recessed slot backing: at least 1.
- Rivets, bolts, or screw heads: at least 8 visible fasteners across corners or side plates.
- Oil or wet stone stain plates: at least 2 thin, non-collider stain planes or low-profile geometry plates.
- Optional steam seep puffs: 0 or more, ambience only.
- Colliders: exactly 0.

Short or narrow variants may reduce bar count to 4 only if they remain visibly slotted and still include the blackened iron frame, aged brass trim, hardware, stain plates, and zero-collider route-safe contract.

## Acceptance Gates

The v0.1.27 promotion gate should pass only when all of the following are true:

- Metadata component is present on the root or clearly associated metadata object.
- Metadata `promotionVersion` is exactly `v0.1.27`.
- Hierarchy is named and includes `FloorDrainGratePrototype` plus the exact placement role.
- All expected placement roles are represented: `intake_floor_drain_grate`, `pipeworks_floor_drain_grate`, and `foundry_floor_drain_grate`.
- Material-role coverage includes blackened iron frame, aged brass drain trim, slotted blackened iron bars, shadowed drain void, hardware, oil/wet stone stain plates, and optional ambient steam if present.
- Minimum counts are met for frame, trim, bars, shadowed recess, fasteners, stain plates, and zero colliders.
- No colliders are present anywhere in the prototype hierarchy.
- No trigger volumes or NavMeshObstacle components are present.
- The grate reads as flush or low-profile floor dressing and does not become a step, pit, snag, route marker for a hidden interaction, or hazard telegraph.
- Placement preserves route width and does not narrow navigation, cover access, doorway clearance, ramps, or combat lanes.

## Non-Goals

This slice does not define final collision, floor hazards, puddle slipping, damage response, steam damage, particle timing, sound design, destruction, interaction, loot, secret marking, or route blocking. Those concerns must remain owned by separate gameplay, VFX, audio, or level-design systems if needed later.

## Implementation Notes

The v0.1.27 main lane promoted `FloorDrainGratePrototype` as compact Unity-owned generated geometry with three placement roles:

- `intake_floor_drain_grate`
- `pipeworks_floor_drain_grate`
- `foundry_floor_drain_grate`

Implemented child roots are `Frame Root`, `Brass Trim Root`, `Slotted Grate Root`, `Rivet Root`, `Oil Stain Root`, and `Steam Seep Root`. Example named parts include `Blackened Iron Drain Frame North`, `Aged Brass Drain Trim North`, `Blackened Iron Slotted Grate Bar 00`, `Brass Drain Bolt 00`, `Oil Dark Stone Stain Plate A`, and `Pale Steam Seep Low`.

The asset should sit visually flush with the surrounding floor. If the implementation uses slightly raised bars or trim for readability, the height should remain cosmetic only and must not require collision, traversal logic, or navigation edits.

## Verification Artifacts

- Scene rebuild: `Logs/v037-scene.log`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.27.md`
- Level validation: `Logs/v037-level-validation.log`
- Full matrix: `V0_BUILD_MATRIX_PASS`
- Windows executable: `Builds/Windows/v0.1.27/BrassworksBreach_v0.1.27.exe`
- Windows package: `Builds/WindowsPackages/v0.1.27/BrassworksBreach_v0.1.27_Windows.zip`
- Package SHA-256: `A26328051AFE1CF0DE0E9A4B2B09507673E37249F322E70DA6A407C9F6AAE6A4`

## Validation Plan

Validation should include:

- Metadata review for component name, placement role, and exact `v0.1.27` promotion version.
- Hierarchy review for named material and part roles.
- Material-role review for blackened iron, aged brass, drain void, hardware, oil/wet stone staining, and optional ambient steam.
- Count review for frame, trim, bars, recess, fasteners, stain plates, and zero colliders.
- Route audit in representative intake, pipeworks, and foundry placements.
- Visual readability pass confirming the grate reads as compact drain dressing, not a pit, trap, secret, or live steam hazard.
- Full validation matrix once implementation enters the main lane.

Validation is complete for the promoted v0.1.27 Unity implementation.

## Handoff Status

- Documentation: complete.
- Implementation: complete.
- Validation: complete.
