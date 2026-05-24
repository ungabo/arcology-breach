# Brassworks Breach - v0.1.35 Level Setpiece Placement Plan

Created: `2026-05-24`

## Purpose

This is a batch-ready placement plan for integrating `V0_1_35_LevelModuleSetpieces` across all five existing levels. It is intended for a large `v0.1.35` or `v0.1.36` visual leap, not serial room-by-room polish.

Coordinate convention follows existing planning docs: `+Z` moves deeper/north, `+X` is east/right, and `1 Unity unit = 1 meter`.

## Placement Strategy

- Use module families in grouped beats: corridor shell + trim + pipe gallery + state fixture, not isolated single props.
- Keep route-state color language consistent: amber attention/objective, red locked/hazard/hostile, green restored/exit.
- Place bulky setpieces on walls, lintels, corners, and perimeter zones. Avoid new floor blockers.
- Use furnace and vault pieces as landmark silhouettes, not generic filler.
- Treat all staged prefabs as visual-only until route smoke and collider review are complete.

## Level01 - Intake Gate / Service Lift

| ID | Priority | Approx position | Rotation | Suggested grouped assets | Role |
| --- | --- | --- | --- | --- | --- |
| LMS-01 | P0 | `x 2, y 0, z 5` | `0, 0, 0` | Pressure door, trim pack, caged gaslight | Upgrade Pressure Gate threshold with gear/pressure read while preserving center aperture. |
| LMS-02 | P1 | `x -5, y 0, z 11` | `0, 90, 0` | Pipe gallery, corridor bay slice, gaslight | Side-wall tutorial machinery near first route turn; supports pressure-network theme. |
| LMS-03 | P1 | `x 4, y 0, z 14` | `0, -90, 0` | Trim pack, catwalk rail segment, grouped dressing sampler child pieces | Repair Bay perimeter dressing that frames first combat without blocking retreat. |
| LMS-04 | P2 | `x 1, y 0, z 21` | `0, 0, 0` | Pressure door lintel language, green/amber state fixtures | Service Lift frame reinforcement; avoid lift trigger/boarding footprint. |

## Level02 - Condensate Spine / Routing Valve

| ID | Priority | Approx position | Rotation | Suggested grouped assets | Role |
| --- | --- | --- | --- | --- | --- |
| LMS-05 | P0 | `x -3, y 0, z 8` | `0, 90, 0` | Pipe gallery, trim pack | Baffle Corridor side wall machinery; keep baffle gaps and projectile lanes open. |
| LMS-06 | P0 | `x 4, y 0, z 14` | `0, -90, 0` | Catwalk rail, pipe gallery, caged gaslight | Lancer Bridge support read; place behind/side of threat lane, not directly behind Lancer tell. |
| LMS-07 | P1 | `x 0, y 0, z 22` | `0, 180, 0` | Pipe gallery, pressure door frame accents, route green tab | Routing Valve Gallery objective wall; make restored pressure visible without new interactables. |
| LMS-08 | P2 | `x 2, y 0, z 29` | `0, 0, 0` | Trim pack, gaslight, grouped dressing child pieces | Locked Lift Lobby return-state dressing; preserve lift trigger and cartridge cache access. |

## Level03 - Boilerheart / Bellows Node

| ID | Priority | Approx position | Rotation | Suggested grouped assets | Role |
| --- | --- | --- | --- | --- | --- |
| LMS-09 | P0 | `x 0, y 0, z 9` | `0, 0, 0` | Furnace alcove, trim pack, warning red strip | Boilerheart approach landmark; keep steam hazard VFX unobscured. |
| LMS-10 | P0 | `x -5, y 0, z 16` | `0, 90, 0` | Vault door, pipe gallery, caged gaslight | Scattergun Display backdrop and side machinery; do not hide pickup silhouette or acquisition effects. |
| LMS-11 | P0 | `x 0, y 0, z 24` | `0, 0, 0` | Furnace alcove side pieces, grouped dressing pulse ring, trim pack | Bellows Node perimeter hazard frame; keep central dodge footprint clear. |
| LMS-12 | P1 | `x 6, y 0, z 31` | `0, -90, 0` | Catwalk rail, pipe gallery, green state tab | Pressure Valve Catwalk release machinery; avoid valve prompt radius. |

## Level04 - Furnace Row / Emergency Hoist

| ID | Priority | Approx position | Rotation | Suggested grouped assets | Role |
| --- | --- | --- | --- | --- | --- |
| LMS-13 | P0 | `x -4, y 0, z 8` | `0, 90, 0` | Furnace alcove, warning red strip, trim pack | Furnace Row hazard identity; place on high wall and side lips, not inside heat lanes. |
| LMS-14 | P0 | `x 4, y 0, z 18` | `0, -90, 0` | Vault door, furnace alcove side piers, caged gaslights | Bulwark Hammer Bay stage mass; keep arena circle and heavy enemy movement clear. |
| LMS-15 | P1 | `x -6, y 0, z 23` | `0, 90, 0` | Pipe gallery, trim pack, catwalk rail short segment | Coal Cache reward frame; avoid pulling the main route through heat. |
| LMS-16 | P1 | `x 1, y 0, z 32` | `0, 0, 0` | Pressure door frame, route green glass, gaslight | Emergency Hoist frame; side/lintel-only placement around boarding zone. |

## Level05 - Core Ring / Warden / Master Override

| ID | Priority | Approx position | Rotation | Suggested grouped assets | Role |
| --- | --- | --- | --- | --- | --- |
| LMS-17 | P0 | `x 0, y 0, z 7` | `0, 0, 0` | Vault door, pipe gallery, trim pack | Pressure Chapel entry landmark; establish final machinery scale. |
| LMS-18 | P0 | `x -7, y 0, z 15` | `0, 90, 0` | Pipe gallery, caged gaslight, heat-blue steel accents | West Regulator Arm identity panel; preserve side-arm loop width. |
| LMS-19 | P0 | `x 7, y 0, z 15` | `0, -90, 0` | Pipe gallery, caged gaslight, warning/green state tabs | East Regulator Arm identity panel; differentiate from west without confusing route colors. |
| LMS-20 | P0 | `x 0, y 0, z 22` | `0, 180, 0` | Setpiece dressing sampler Warden feed, vault/pipe high-wall pieces | Warden Arena high-wall feeds and crown framing; keep boss center and projectile lanes clear. |
| LMS-21 | P1 | `x 0, y 0, z 34` | `0, 0, 0` | Pressure door frame, route green tab, caged gaslights | Master Override Hoist final state stack; keep Warden-to-exit path at least `4.5m`. |

## Integration Recommendations

1. Integrate `P0` placements as grouped visual chunks first, then run first-person smoke screenshots from approach and objective-read angles.
2. Add `P1` and `P2` placements only after route widths and enemy readability remain clean.
3. Convert repeated proxy primitives into optimized final meshes or combined prefabs before broad all-level shipping placement.
