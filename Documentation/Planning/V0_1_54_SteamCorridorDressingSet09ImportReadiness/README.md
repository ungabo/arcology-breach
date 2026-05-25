# Steam Corridor Dressing Set 09 Import Readiness Plan

## Recommended Flow

1. Import `AssetPacks/BrassworksBreach.SteamCorridorDressingSet09` into a quarantine Unity project as an embedded/local package.
2. Confirm the package compiles with the runtime and editor assemblies.
3. Run `Brassworks Breach/Sidecars/Steam Corridor Dressing Set 09/Generate Package Assets`.
4. Inspect `Runtime/Generated/Materials`, `Runtime/Generated/Meshes`, and `Runtime/Generated/Prefabs`.
5. Place `SCD09_PREFAB_000_FullCorridorDressingPalette.prefab` in a temporary review scene.
6. Compare family coverage against `Documentation/ConceptRenders/V0_1_54_SteamCorridorDressingSet09/SCD09_contact_sheet.png`.
7. Promote only approved generated prefabs/materials through the normal asset gate.

## What Can Be Integrated

- 20 visual-only dressing prefabs after Unity generation.
- 1 full-family palette prefab for internal review.
- 12 generated materials for blackened stone, soot brick, oily iron, brass, copper, verdigris, glass, gauge, warning paint, steam marker, and cable rubber treatments.
- 6 generated mesh assets used as modular construction primitives.
- Runtime metadata and catalog code for tooling, QA, and future regeneration.

## What Must Stay Out Of Main Game Integration

- Do not add package paths directly to `Packages/manifest.json` until the package has passed quarantine review.
- Do not promote generated assets directly into production scenes.
- Do not treat steam marker geometry as VFX.
- Do not assume generated prefabs include collision, nav, occlusion, lighting, audio, or interactable behavior.

## Placement Guidance

- Use wall pieces to increase density along long corridor runs and room margins.
- Use floor drains/grates where wet floor reflection and runoff silhouettes help readability.
- Use ceiling pipe clusters and lamps to build vertical compression in boiler-room corridors.
- Use doorway pieces around pressure doors, locks, and transition points so thresholds feel engineered.
