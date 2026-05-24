# Brassworks Breach Mechanical Enemy Elite Set 05

Version: `v0.1.49-p001`

This isolated Unity sidecar package provides large mechanical enemy visual upgrades that can later replace placeholder enemies. It contains generated visual-only prefabs, reusable mesh assets, materials, preview renders, and import-readiness documentation.

## Contents

- `Runtime/Prefabs`: 25 generated visual-only prefab candidates.
- `Runtime/Materials`: 18 generated material assets.
- `Runtime/Meshes`: 12 reusable simple mesh assets.
- `Editor`: package generator and validation entry point.
- `Documentation~/Manifest`: package-local manifest JSON.
- `Samples~/PreviewScene`: quarantine preview notes.
- `ValidationProject~`: isolated Unity project for generation and smoke validation.

## Generation

Batch entry point:

`BrassworksBreach.MechanicalEnemyEliteSet05.Editor.MechanicalEnemyEliteSet05Generator.GenerateValidateAndQuit`

Menu items:

- `Brassworks Breach/Sidecars/Mechanical Enemy Elite Set 05/Generate Package`
- `Brassworks Breach/Sidecars/Mechanical Enemy Elite Set 05/Render Preview PNGs`

## Scope

Visual-only pose proxies only. No AI, damage, movement, nav authority, colliders, runtime scripts, animation clips, or animator controllers are included.
