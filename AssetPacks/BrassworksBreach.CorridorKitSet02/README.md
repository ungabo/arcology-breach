# Brassworks Breach Corridor Kit Set 02

Corridor Kit Set 02 is a Unity-only sidecar package for v0.1.41 quarantine review. It contains generated visual meshes, procedural materials, and modular prefabs for a dense north-star steampunk corridor/door/room kit.

The package is intentionally isolated from the main game project. It does not declare runtime dependencies, gameplay scripts, autonomous audio, colliders, nav blockers, scene content, or changes to `Packages/manifest.json`.

## Generated Content

- Modular corridor modules, junctions, end caps, and arch supports.
- Pressure door, frame, threshold, iris, and control-column visuals.
- Room wall panels, corner pieces, alcoves, ceiling rings, floor panels, pipe runs, light runs, window grates, and wayfinding.
- Reusable procedural meshes and materials under `Runtime`.
- Package manifest and generated catalog JSON for intake review.

## Unity Menu

After importing or opening the validation project, use:

`Brassworks/Sidecars/Corridor Kit Set 02 v0.1.41/Generate and Render Preview`

Batchmode generation method:

`BrassworksBreach.Sidecars.CorridorKitSet02.CorridorKitSet02Generator.GenerateAllAndRenderPreview`

## Runtime Safety

All package prefabs are visual-only. They are built from `MeshFilter`, `MeshRenderer`, inactive metadata child objects, and occasional `TextMesh` labels. They omit colliders, rigidbodies, gameplay components, scene objects, and autonomous audio.
