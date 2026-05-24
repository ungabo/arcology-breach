# Brassworks Breach Mechanical Enemies Sidecar

Version: `v0.1.37-p001`

This is a Unity-only asset-factory package for the Brassworks Breach mechanical enemy family. It is intentionally isolated from the primary game project so the main lane can quarantine-import the generated content without accepting gameplay behavior, project settings, package changes, or scene edits.

## Contents

- `Runtime/Prefabs`: generated enemy prefab outputs.
- `Runtime/Materials`: generated steampunk material library.
- `Runtime/Meshes`: generated procedural mesh accents used by the prefabs.
- `Editor`: Unity Editor generator and preview-render tooling.
- `Samples~/PreviewScene`: optional preview-scene notes for sidecar validation.

## Generation Menu

After importing this package into a Unity project, run:

- `Brassworks Breach/Sidecars/Mechanical Enemies/Generate v0.1.37 Enemy Package`
- `Brassworks Breach/Sidecars/Mechanical Enemies/Render v0.1.37 Preview PNGs`

The generator creates visual-only prefabs for:

- `SCENM_SawScrapper`
- `SCENM_RivetLancer`
- `SCENM_BulwarkFurnace`
- `SCENM_WardenSentinel`
- `SCENM_FoundryOverseerBust`

Each prefab keeps rigging-ready children named `Root`, `Hips`, `Body`, `Head`, `LeftArm`, `RightArm`, `LeftLeg`, `RightLeg`, `WeaponMounts`, `VFXAnchors`, and `Hitboxes`.

## Design Rules

- No AI, combat, spawn, damage, route, or progression scripts are included.
- All geometry is built from Unity primitives or procedural mesh data.
- Amber materials are weak-point/furnace-eye language.
- Cyan-blue materials are charge, ranged tell, or dangerous tool energy.
- Hitbox markers are visual/collider guidance only and should be reviewed by the gameplay lane before promotion.
- Prefab scale is first-person corridor compatible for the current main-game proportions.

## Quarantine Import

This package is ready for quarantine import after these checks:

1. `package.json` parses as valid JSON.
2. No conflict markers appear inside the package or v0.1.37 documentation folders.
3. Unity imports the local package without console errors.
4. The generation menu creates the prefab, material, and mesh assets.
5. The preview render menu writes PNGs to `Documentation/ConceptRenders/V0_1_37_MechanicalEnemiesSidecar`.

See `Documentation/AssetProduction/V0_1_37_MechanicalEnemiesSidecar/ACCEPTANCE_REPORT_v0.1.37-p001.md` for the current validation state.
