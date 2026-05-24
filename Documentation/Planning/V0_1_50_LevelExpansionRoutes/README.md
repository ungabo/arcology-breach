# V0.1.50 Level Expansion Routes

Created: `2026-05-24`

This packet defines three implementation-ready route modules for the next Brassworks Breach gameplay leap. It is documentation-only and owns no Unity source, scenes, packages, build scripts, or existing docs.

## Owned Roots

- `Documentation/Planning/V0_1_50_LevelExpansionRoutes/`
- `Documentation/QA/V0_1_50_LevelExpansionRoutes/`
- `Documentation/LevelDesign/V0_1_50_LevelExpansionRoutes/`

## Deliverables

- `IMPLEMENTATION_PACKET_v0.1.50.md` - shared build contract, budgets, naming, collision authority, sidecar placement rules, and integration order.
- `LEVEL02_ROUTE_MODULE_v0.1.50.md` - pressure bypass combat loop and exploration valve route.
- `LEVEL03_ROUTE_MODULE_v0.1.50.md` - foundry gantry, coolant detour, and furnace hazard loop.
- `LEVEL04_ROUTE_MODULE_v0.1.50.md` - observatory pumpworks route, vertical arena, and secret return branch.
- `SCENE_OBJECT_EXPECTATIONS_v0.1.50.md` - expected scene container/object names and validation tables.
- `QA_CHECKLIST_v0.1.50.md` - manual QA checklist for route, combat, hazard, objective, and performance checks.
- `VALIDATION_ACCEPTANCE_CRITERIA_v0.1.50.md` - promotion gates and rejection conditions.

## Coordinate Contract

- Unity units are meters.
- Coordinates are local to each receiving level root unless that scene already has a documented level-local origin.
- Player floor is `Y=0` unless specified.
- Grid snap is `0.5m`; primary structural dimensions use `1m`, `2m`, or `4m` increments.
- Door/corridor clear width is never below `3.0m`; combat corridors target `4.0m`.
- Main-lane collision, triggers, nav links, objectives, and enemy spawn logic stay authoritative in the main Unity project.
- Visual-only sidecar instances remain decoration only and must not own colliders, rigidbodies, runtime scripts, cameras, lights, audio, or particles.

## Route Module Summary

| Level | Module | Main Expansion Goal | Route Type | Target Added Playtime |
| --- | --- | --- | --- | --- |
| Level02 | `L02_Route_PressureBypass` | Convert a linear pressure-door segment into a flankable valve bypass with one secret return. | Combat/exploration loop | 4-6 min |
| Level03 | `L03_Route_FoundryGantry` | Add a heated vertical gantry around the foundry with optional coolant objective. | Vertical combat loop | 5-7 min |
| Level04 | `L04_Route_ObservatoryPumpworks` | Add a late-route pumpworks wing with lock/key objective, hazard toggles, and secret ammo cache. | Objective branch with rejoin | 6-8 min |

## Integration Stance

Use this packet as a build recipe for a later main-lane batch. Do not import sidecars or edit the main scene from this packet alone. The later implementer should first block out gameplay volumes with main-lane primitives, validate traversal/combat, and only then place visual-only sidecar dressing under the containers named in `SCENE_OBJECT_EXPECTATIONS_v0.1.50.md`.
