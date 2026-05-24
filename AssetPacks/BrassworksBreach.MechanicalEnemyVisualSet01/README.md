# Brassworks Breach Mechanical Enemy Visual Set 01

Version: `v0.1.40-p001`

This package is a large, self-contained Unity sidecar for visual-only mechanical enemy candidates. It is not gameplay content: no AI, damage, movement, colliders, nav obstacles, rigging, or runtime scripts are included.

## Contents

- `Runtime/Prefabs`: generated visual candidate prefabs.
- `Runtime/Materials`: generated steampunk material library.
- `Runtime/Meshes`: reusable procedural mesh accents used by the prefabs.
- `Editor`: Unity editor generator and batch validation entry points.
- `Documentation~/Manifest`: package-local sidecar manifest.
- `Samples~/PreviewScene`: preview-scene notes.
- `ValidationProject~`: package-local Unity project for isolated generation and smoke validation.

## Generation Menu

After referencing this package in a Unity project, run:

- `Brassworks Breach/Sidecars/Mechanical Enemy Visual Set 01/Generate Package`
- `Brassworks Breach/Sidecars/Mechanical Enemy Visual Set 01/Render Preview PNGs`

Batch entry point:

`BrassworksBreach.MechanicalEnemyVisualSet01.Editor.MechanicalEnemyVisualSet01Generator.GenerateValidateAndQuit`

## Visual Families

- Saw Scrapper variants: low, fast boiler bodies with large saw silhouettes.
- Rivet Lancer variants: tall narrow lance silhouettes with readable cyan pressure tells.
- Bulwark Furnace variants: broad furnace blockers with shield armor and warning lamps.
- Bellows support node variants: compact pressure-support silhouettes with leather bellows and smoke stacks.
- Warden/Overseer studies: tall command silhouettes for elite/boss readability.

Every prefab includes named visual part groups for `chassis`, `boiler`, `lens`, `saw_limb`, `pressure_lines`, `rivets`, `warning_lamps`, `smoke_stacks`, and `armor_plates`.

## Scope Rules

- Visual-only prefabs and generated assets only.
- No runtime assembly and no gameplay components.
- Unity primitives and procedural meshes only.
- Preview PNGs are rendered by Unity from generated prefabs.
- Primary project manifests and gameplay scenes are intentionally untouched.
