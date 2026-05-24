# v0.1.35 Level Module Setpieces - Material, LOD, Collider, and Performance Guidance

## Material Guidance

- Treat blackened iron and soot brick as the dominant structural read. Brass and copper should be accents, not full-room color floods.
- Reserve `M_V0135_WarmAmberGlass` for objective attention, route reassurance, lamp lenses, and pressure-state hints.
- Reserve `M_V0135_WarningRedEnamel` for locked, hostile, overheated, or unsafe reads only.
- Reserve `M_V0135_RouteGreenGlass` for restored pressure and exit-ready state. Avoid mixing green with generic decorative lamps.
- Keep furnace glow below enemy readability intensity in combat spaces. The glow should frame hazards, not erase silhouettes.

## LOD Guidance

| Distance | Target | Notes |
| --- | --- | --- |
| `0-18m` | LOD0 | Full staged prefab, all visible proxy children, readable gauges/rivets/lamps. |
| `18-35m` | LOD1 | Remove 40-55 percent of small rivets, clamps, spokes, and tiny status tabs; keep primary silhouette. |
| `35m+` | LOD2 | Merge to simple wall/door/pipe/furnace blocks with emissive state colors only. |

For low/mid Windows PC, prefer static batching for wall/floor/trim shells and GPU instancing for repeated rivets, bolts, pipe clamps, valve caps, and lamp cages.

## Collider Guidance

- Default package intent is visual-only. Do not enable child primitive colliders wholesale.
- Corridor shells may use one simple floor collider and two wall box colliders only after route width is checked.
- Door frames should collide only on outer jambs/lintels. Central apertures must remain clear unless the existing door system owns the blocker.
- Vault doors may use one simple closed-door blocker only when placed as a non-route scenic closure.
- Pipe galleries, gauges, valve wheels, lamps, trim, and rail details should be non-colliding unless the main integration lane deliberately marks them reachable cover or blockers.
- Catwalk rails can use simple post/rail capsule or box colliders after movement QA, but never create hidden snag points along the player path.
- Furnace alcoves must not create new damage volumes. Heat gameplay remains owned by existing hazard systems.

## Performance Notes

- Target each full decorated corridor bay cluster at under roughly `18k` rendered triangles after final art conversion; current Unity primitive proxies are placeholders and should be consolidated before ship.
- Avoid unique material instances at integration time. Use the shared 12-material palette or project-wide material equivalents.
- Bake static lighting or use existing level lighting where possible. The caged gaslight point light is a proof light, not a required runtime light.
- Keep emissive surfaces small. Prefer a few readable lamps/gauges over broad glowing panels.
- Place setpieces at objective/hazard/enemy-read moments rather than evenly filling every wall. This keeps draw calls and visual noise under control.

## Acceptance Gates

- Unity imports the package without compile errors, magenta materials, or missing prefab dependencies.
- Preview sheets remain visibly nonblank and aligned with the north-star style.
- Route-smoke screenshots show no blocked pickup, prompt, transition, enemy spawn, boss lane, secret entrance, or final exit.
- Tight lanes remain at least `2.2m`; normal lanes remain at least `3.5m`; lift/hoist boarding zones keep about `4m` turn space.
- Red/green/amber state colors remain semantically consistent across all five levels.
- Enemy tell colors and pickup markers stay readable against nearby setpiece dressing.
