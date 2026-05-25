# CAML10 Blunt North-Star Comparison

Reference: `Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png`.
These renders are concept/lookdev only, outside the game build.

## Verdict

The assembly is directionally useful but not production-ready. It passes the broad corridor read: dark wet shell, brass/black iron palette, warm lamps, steam haze, and sidewall mechanical language. It fails the north-star focal-detail bar: the pressure-door target is still weak/occluded, and the object silhouettes are too planar and too generator-clean next to the north-star paintover.

## Passes

- RSS07 gives a readable corridor footprint, turn, ribs, and pressure-door framing without touching main scenes.
- RMS10 materially moves the shell away from gray blockout toward dark wet masonry and glossy flagstone.
- GPD10 lamps are the best-performing new family in assembly: they give readable amber practicals and wall soot anchors.
- RSS07 plus Unity-only staging can establish a corridor frame without touching the HRS07 render scenes.
- SCD09 provides the right categories: pipes, gauges, tanks, valves, floor drains, vents, ceiling clusters, and doorway dressing.

## Fails

- SCD09 pieces read as flat kit cards in close view. The north-star needs deeper pipe offsets, elbows, valve wheels, overlapping brackets, gauge needles, and tank straps that break silhouette.
- The pressure-door/focal read is not good enough. The staged target is occluded by mid-corridor dressing and does not land like the north-star locked gate.
- RSS07 shell modules still expose slab-simple walls and ceiling ribs when the camera gets close. RMS10 helps the material read, but it cannot hide blockout-level geometry.
- Steam and wetness are lookdev staging, not a validated VFX/material integration pass.
- The assembly has corridor mood but not enough authored grime, decal history, damage variation, or hand-placed clutter.
- No player weapon/HUD integration is assessed here; this is only environmental corridor lookdev.

## Revise Next

Revise `com.brassworks.sidecar.steam-corridor-dressing-set09` next. The shell and material packages are serviceable enough to keep evaluating corridor concepts, and GPD10 passes as a lighting/fixture booster. The biggest remaining gap against the north-star is object-family depth: SCD09 must become more three-dimensional, less planar, and more mechanically layered before another corridor assembly pass will feel close.

## Shot Metrics

- `Documentation/ConceptRenders/V0_1_55_CorridorAssemblyLookdev10/CAML10_RENDER_01_corridor_assembly_hero.png`: avg luma 0.121, dark ratio 0.832, warm ratio 0.110, prefabs 26, staging primitives 193.
- `Documentation/ConceptRenders/V0_1_55_CorridorAssemblyLookdev10/CAML10_RENDER_02_corner_service_density.png`: avg luma 0.063, dark ratio 0.959, warm ratio 0.010, prefabs 9, staging primitives 171.
- `Documentation/ConceptRenders/V0_1_55_CorridorAssemblyLookdev10/CAML10_RENDER_03_material_fixture_detail.png`: avg luma 0.086, dark ratio 0.899, warm ratio 0.008, prefabs 5, staging primitives 28.

## Missing Assets
- None detected by the isolated render script.
