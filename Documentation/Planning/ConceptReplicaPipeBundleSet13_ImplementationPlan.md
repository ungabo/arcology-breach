# Concept Replica Pipe Bundle Set13 Implementation Plan

Generated: 2026-05-25 00:23:33 -04:00

## Target
Create a Unity-only modular pipe bundle proof matching the north-star crop: aged copper/brass pipes, elbow bends, blackened collars, flanges, wall brackets, small valves, dark wet masonry, grime, steam hints, and warm gaslight reflections.

## Method
- Build reusable mesh primitives in Unity: straight cylinders on all axes, quarter elbow tube, torus rings, rivet domes, planes, and box primitives.
- Generate procedural texture maps before material creation: albedo, normal, metallic/smoothness, and occlusion for copper/brass, blackened iron, wet grime, dark masonry, amber highlights, and red valve enamel.
- Save visual-only prefabs for the module kit, then compose an assembled pipe bundle against masonry to judge crop match.
- Render three PNGs in Unity with warm point/spot lighting and low ambient fill: beauty, modular breakdown, and grazing-light material closeup.

## Integration Notes
The modules are ready for a future quarantined playable visual import after a sidecar validator pass. They should remain visual-only until collision/nav blockers are authored in the main project.
