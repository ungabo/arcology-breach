# BrassworksBreach.RoomSurfaceReliefSet11

Unity-only visual sidecar package for high-fidelity room and corridor surface relief in Brassworks Breach.

This bundle targets the roomtest v0.6 surface gap: shallow real brick/stone geometry over dark mortar, warmer wet flagstone response, sooted ceiling brick, corner grime, and helper cards for damp gaslight reflection. It does not edit roomtest and does not carry gameplay authority.

## Contents

- Prefabs: 28 visual-only modular surface assemblies across 7 families.
- Meshes: 17 native Unity mesh assets for bricks, slabs, cards, wedges, trim, and folded corner grime.
- Materials: 14 Unity Standard-shader materials.
- Runtime texture maps: 56 generated PNG maps at 1024x1024.
- Previews: 29 external concept PNGs plus package preview mirrors and contact sheets.
- Metadata: manifest and catalog JSON in Runtime/Metadata and Documentation~/Manifest.

## Visual-Only Contract

Runtime prefabs contain only GameObject, Transform, MeshFilter, and MeshRenderer records. They include no scripts, colliders, rigidbodies, lights, reflection probes, audio, animation, timeline, navmesh, scenes, or gameplay behavior.

## Import Notes

Import into a quarantine Unity project first. Use wall panels as shallow overlay modules, floor slabs as corridor or room surface inserts, ceiling panels as small-brick overhead inserts, and cards/strips as secondary layers. Main project owners should author real collision, lighting, reflection probes, decals, occlusion, and gameplay separately.
