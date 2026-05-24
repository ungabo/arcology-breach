# Brassworks Breach Objective Interactables Set 05

Visual-only Unity sidecar package for large objective/interactable prop families.

## Contents

- Runtime prefabs, materials, reusable mesh assets, and metadata catalog generated under `Runtime/`.
- Package-local manifest under `Documentation~/Manifest/`.
- Editor-only generator, preview renderer, and validator under `Editor/`.
- Isolated validation project under `ValidationProject~/`.

## Safety Contract

- Visual-only assets.
- No gameplay authority, trigger logic, inventory logic, damage logic, door/lift/boss state, or autonomous audio.
- Generated runtime prefabs omit colliders, rigidbodies, cameras, lights, particle systems, audio sources, and runtime MonoBehaviours.
- Any later gameplay hooks must remain in the primary lane and target these prefabs as decorative children only.

## Unity Commands

- `Brassworks Breach/Sidecar Packs/Objective Interactables Set 05 v0.1.49/Generate Package Assets`
- `Brassworks Breach/Sidecar Packs/Objective Interactables Set 05 v0.1.49/Render Preview PNGs`
- `Brassworks Breach/Sidecar Packs/Objective Interactables Set 05 v0.1.49/Generate, Render, Validate`

This package is a sidecar only. It does not modify primary gameplay scenes, shared scripts, build settings, or the main project package manifest.
