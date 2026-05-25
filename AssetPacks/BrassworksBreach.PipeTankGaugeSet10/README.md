# Pipe Tank Gauge Set 10

Unity-only visual sidecar package for corridor machinery dressing.

This package contains isolated prefabs for wall and ceiling pipe runs, pressure tanks, gauge clusters, valve wheels, brass brackets, copper elbows, black iron supports, steam nozzle housings, and one assembled lookdev strip. Assets are visual-only: no colliders, rigidbodies, gameplay scripts, animation controllers, audio, or scene dependencies.

Preview PNGs and the contact sheet are generated both inside `Runtime/Previews` and to `Documentation/ConceptRenders/V0_1_55_PipeTankGaugeSet10`.

## Contents

- `Runtime/Prefabs`: isolated corridor machinery family prefabs plus a combined lookdev strip.
- `Runtime/Materials`: aged brass, bright brass edgewear, heat-stained copper, black iron, oily black support metal, ivory gauge enamel, amber glass, red enamel, and steam-soot materials.
- `Runtime/Textures`: procedural albedo, normal, and mask textures used by the materials.
- `Runtime/Meshes`: reusable Unity mesh assets generated procedurally in-editor.
- `Runtime/Metadata`: normalized package manifest and validation metadata.
- `Runtime/Previews`: import-facing PNG previews and contact sheet.
- `Documentation~/Manifest`: package-local copy of the normalized manifest.

## Rebuild

In a Unity project that references this package, run:

`Brassworks Breach/Sidecar Packs/Pipe Tank Gauge Set 10/Generate Assets And Renders`
