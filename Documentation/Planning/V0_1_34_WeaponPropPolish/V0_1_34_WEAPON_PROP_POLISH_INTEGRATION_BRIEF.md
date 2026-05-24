# Brassworks Breach v0.1.34 Weapon/Prop Polish Integration Brief

Created: `2026-05-24`

Owned scope:

- `Documentation/Planning/V0_1_34_WeaponPropPolish/`
- `Assets/_Project/ArtStaging/V0_1_34_WeaponPropPolish/`

## Purpose

This package stages a batch-ready weapon and prop polish layer for the next main-lane playable-art leap. It is not a one-asset promotion. It groups pressure pistol readability, Steam Scattergun display language, ammo pickup identity, and route-console prop cues into one Unity-importable art packet that the main lane can integrate after route and gameplay authority remain stable.

## Inputs Reviewed

- `Assets/_Project/ArtStaging/WeaponPropBatch/`
- `Documentation/AssetProduction/WeaponPropBatch/`
- `Assets/_Project/ArtStaging/WeaponsProps/`
- `Documentation/AssetProduction/BoilerControlConsolePrototype/`
- `Documentation/AssetProduction/ValveWheelConsolePrototype/`
- `Documentation/AAA_ASSET_CATALOG.md`
- `Documentation/IMPLEMENTATION_TODO.md`

## Staging Contents

| Area | Files | Integration use |
| --- | --- | --- |
| Mesh kits | `Meshes/V134WPP_001_*` through `Meshes/V134WPP_004_*` | Loose OBJ material and silhouette boards for four grouped prop lanes. |
| Shared OBJ palette | `Meshes/V134WPP_Brassworks_PolishPalette.mtl` | Material-slot names shared by all staged OBJ files. |
| Material proxies | `Materials/M_V134WPP_*.mat` | Local Unity Standard-shader proxies for brass, iron, copper, gauges, glass, warning accents, grime, wear, route green, and heat steel. |
| Previews | `Previews/V134WPP_*_preview.svg` | Lightweight swatch and inventory previews for non-scene review. |
| Planning docs | This brief, manifest JSON, and acceptance gates | Main-lane merge guidance and validation checklist. |

## Batch Targets

### Pressure Pistol Polish

Use the staged pistol kit to merge the older blockout proportions with the newer `WeaponPropBatch` component vocabulary. The strongest integration targets are the pressure chamber, gauge cluster, copper coil pack, muzzle crown, relief lever, blackened receiver mass, and walnut grip contrast. This should read in first person as a compact precision weapon, with the red pressure needle and pressure-dump lever reserved for secondary-fire emphasis.

### Steam Scattergun Display

Use the staged scattergun kit to strengthen the Level03 pickup display and first-person identity without changing unlock authority. The display should keep the triple/twin barrel mass, heavy yoke, shell rack, slug canister, walnut pump grip, brass top rib, and amber nameplate as readable distance cues. It should feel heavier than the pistol, with soot and blackened iron doing more work than polished brass.

### Ammo Pickups

Use the staged ammo kit to establish a family, not a single pickup: small pressure-cell bundle, scattergun shell/slug canister variant, and route-readable red pressure seals. The pickup should communicate "cartridges" before the player sees HUD feedback. The existing pickup gameplay definition remains authoritative.

### Route Console Props

Use the staged console kit as a prop-polish bridge between `BoilerControlConsolePrototype` and `ValveWheelConsolePrototype`. Integrate only as visual dressing unless a separate gameplay task owns interaction. The valve wheel/gauge/lamp language should reinforce route readability near pipeworks and boilerheart consoles while avoiding fake prompts or new puzzle state.

## Main-Lane Integration Order

1. Import the v0.1.34 staging folder and confirm all OBJs, material proxies, and SVG previews load without missing references.
2. Promote the pressure pistol component substitutions first: chamber, muzzle crown, gauge/needle, relief lever, coil pack, and grip contrast.
3. Promote Steam Scattergun display polish second, keeping pickup/unlock scripts and existing display root as the only gameplay authority.
4. Promote ammo pickup visual variants as a grouped family, then verify pickup readability and route occlusion in the current gameplay levels.
5. Promote route-console prop polish last, because console dressing can confuse future interaction language if it is placed before material and lamp rules are final.

## Guardrails

- No generated scenes, prefabs, gameplay scripts, validators, release notes, status files, git state, or shared integration files are edited by this package.
- All staged meshes are visual/material boards, not final topology, colliders, rigs, animations, or gameplay-ready prefabs.
- Do not use Blender or any external DCC as an integration requirement.
- Keep the main lane in charge of scene placement, validators, build matrix, package evidence, release notes, and commits.
- If a staged prop looks like an interaction, either add clear noninteractive dressing context or defer it until interaction ownership is planned.

## Recommended Verification

Run Unity import review first, then targeted gameplay-distance inspection. The full V0 matrix should wait until the main lane has integrated a coherent batch rather than one staged piece at a time.
