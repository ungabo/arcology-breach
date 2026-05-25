# BrassworksBreach.GaslightPipeDressingSet10

Visual-only steampunk wall gaslight and pipe-fixture dressing package for Brassworks Breach.

This package directly targets the weak lamp/fixture gap called out around roomtest v0.5: the room surfaces and lighting direction are working, but the remaining gains need purpose-built lamp models, fixture detail, wall plaques, and damp reflection helpers.

## Contents

- Prefabs: 24 visual-only Unity prefabs across 6 families.
- Materials: 14 Unity Standard-shader materials.
- Preview PNGs: 24 per-piece concept previews in the documentation root plus a package contact sheet in Runtime/Previews.
- Metadata: normalized sidecar manifest mirrored in Runtime/Metadata and Documentation~/Manifest.

## Visual-Only Contract

The prefabs contain only GameObject, Transform, MeshFilter, and MeshRenderer records. They contain no colliders, rigidbodies, lights, reflection probes, scripts, audio, animations, timelines, scenes, navmesh, or gameplay authority. Amber bulbs and glints are emissive materials only.

## Import Notes

Import into a quarantine Unity project first. Place the wall gaslight prefabs on the v0.5 brick surfaces, then layer pipe brackets, plaques, cages, and reflection helper strips around the damp floor/wall glints. Any real lighting, collision, reflection probe, damage, or interaction behavior must be authored by the integration owner in main project content.