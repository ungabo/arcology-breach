# CatwalkRailPrototype Brief v0.1.26

## Purpose

`CatwalkRailPrototype` is a Unity-owned geometry slice for Brassworks Breach route-safe industrial safety rails. It exists as environmental dressing for catwalk edges, service walkways, and furnace-side maintenance paths. It must read as steampunk plant infrastructure without becoming gameplay blocking, collision, or navigation constraint.

This slice was promoted in v0.1.26. Implementation, scene placement, route audit, and the full Windows V0 matrix are complete.

## North-Star Style

The rail should feel like a durable foundry safety system assembled from blackened iron, aged brass, and visible fasteners:

- Blackened iron vertical posts with soot-darkened wear at corners and foot plates.
- Aged brass upper rail with handled, polished contact edges and tarnished underside.
- Lower iron rail that visually reinforces the route edge without closing the walkway.
- Brass post caps or collars that catch warm industrial light.
- Rivets, bolt heads, and bolted feet that make the component feel mechanically installed.
- Slight asymmetry in wear, grime, and edge highlights, while keeping silhouette clean and readable.

The shape language should remain industrial and route-safe: protective rail, not barricade; service fixture, not wall; dressing that clarifies the route edge, not dressing that narrows the route.

## Unity Ownership And Gameplay Contract

`CatwalkRailPrototype` is Unity-owned geometry only.

- No colliders.
- No trigger volumes.
- No NavMeshObstacle components.
- No blocking behavior.
- No route narrowing or lane-width reduction.
- No hidden gameplay affordance or traversal requirement.
- No authored animation dependency for acceptance.

Placement must preserve the existing walkable envelope. Rails may communicate safety and edge boundaries visually, but they must not become physical barriers unless a separate gameplay-owned blocker is intentionally placed by another system.

## Expected Placement Roles

The v0.1.26 implementation should support these role identifiers exactly:

- `foundry_catwalk_rail`
- `pipeworks_service_rail`
- `boilerheart_service_rail`

Each role should be visually related, but it may vary in wear, grime density, brass polish, and mounting details to match its destination:

- `foundry_catwalk_rail`: heavier soot, scorched iron feet, hot brass edge glints.
- `pipeworks_service_rail`: serviceable and slightly cleaner, with pipe-adjacent grime streaks.
- `boilerheart_service_rail`: darker heat staining, stronger brass/iron contrast, dense bolted hardware.

## Named Hierarchy Target

Every promoted rail variant should use a named hierarchy that is easy to inspect in Unity. Suggested structure:

```text
CatwalkRailPrototype_[role]
  geometry
    posts_blackened_iron
    rail_upper_aged_brass
    rail_lower_blackened_iron
    caps_aged_brass
    feet_bolted_iron
    rivets_and_bolts
  metadata
```

The root object name must include `CatwalkRailPrototype` and the exact placement role. Child names must expose material or part intent clearly enough for review without opening meshes one by one.

## Material-Role Coverage

At minimum, the promoted prototype must visibly cover these material roles:

- `blackened_iron_posts`
- `aged_brass_upper_rail`
- `blackened_iron_lower_rail`
- `aged_brass_caps`
- `dark_iron_bolted_feet`
- `rivet_or_bolt_detail`

Material assignments may use existing Brassworks Breach material conventions, but the role coverage should remain inspectable from hierarchy names, mesh names, material names, or reviewer-facing metadata.

## Minimum Counts

For each accepted rail run or role variant:

- At least 3 vertical posts for a standard straight run.
- At least 1 continuous or visually continuous aged brass upper rail.
- At least 1 lower blackened iron rail.
- At least 2 bolted feet or mounting plates.
- At least 2 brass caps or collars.
- At least 6 visible rivets or bolt heads across feet, collars, or plates.

Short end-cap or corner variants may use 2 posts, but they must still include the upper rail, lower rail, bolted feet, brass cap/collar language, and visible fasteners.

## Acceptance Gates

The v0.1.26 promotion gate should pass only when all of the following are true:

- Metadata component is present on the root or clearly associated metadata object.
- Metadata `promotionVersion` is exactly `v0.1.26`.
- Hierarchy is named and includes `CatwalkRailPrototype` plus the exact placement role.
- All expected placement roles are represented: `foundry_catwalk_rail`, `pipeworks_service_rail`, `boilerheart_service_rail`.
- Material-role coverage includes blackened iron posts, aged brass upper rail, lower iron rail, brass caps, bolted feet, and rivet/bolt detail.
- Minimum counts are met for posts, rails, feet, caps/collars, and rivets/bolts.
- No colliders are present anywhere in the prototype hierarchy.
- No trigger volumes or NavMeshObstacle components are present.
- Placement preserves route width and does not narrow navigation.

## Non-Goals

This slice does not define final collision, cover behavior, traversal behavior, damage response, destruction, interaction, or route blocking. Those concerns must remain owned by separate gameplay or level-design systems if needed later.

## Implementation Notes

The promoted Unity implementation uses `CatwalkRailPrototype` metadata on the root object and three placement roles:

- `pipeworks_service_rail`
- `boilerheart_service_rail`
- `foundry_catwalk_rail`

The generated hierarchy includes `Rail Root`, `Upright Root`, `Cap Root`, `Foot Root`, and `Rivet Root`, with named brass/iron parts such as `Aged Brass Upper Rail`, `Blackened Iron Lower Rail`, `Blackened Iron Upright 00`, `Aged Brass Post Cap 00`, `Blackened Iron Bolted Foot 00`, and `Brass Foot Rivet 00`.

## Verification Artifacts

- Scene rebuild: `Logs/v036-scene.log`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.26.md`
- Level validation: `Logs/v036-level-validation.log`
- Full matrix: `V0_BUILD_MATRIX_PASS`
- Windows executable: `Builds/Windows/v0.1.26/BrassworksBreach_v0.1.26.exe`
- Windows package: `Builds/WindowsPackages/v0.1.26/BrassworksBreach_v0.1.26_Windows.zip`
- Package SHA-256: `22DC845145667AFE6586502ED8EF2D189D6C572F437D302C4DBCE3A28750D149`

## Handoff Status

- Documentation: complete.
- Implementation: complete.
- Validation: complete.
