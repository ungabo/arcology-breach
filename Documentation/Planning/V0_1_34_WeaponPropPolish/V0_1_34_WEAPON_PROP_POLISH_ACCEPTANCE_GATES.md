# v0.1.34 Weapon/Prop Polish Acceptance Gates

Status: staging gates for main-lane integration

Applies to:

- `Documentation/Planning/V0_1_34_WeaponPropPolish/`
- `Assets/_Project/ArtStaging/V0_1_34_WeaponPropPolish/`

## Package Scope Gates

- All authored files stay inside the two owned v0.1.34 paths.
- No edits to generated scenes, shared validators, `BUILD_STATUS`, `WORK_LEDGER`, `SESSION_LOG`, release notes, git state, gameplay scripts, or prefab authority.
- The package remains Unity-only for review and integration planning. Blender is not required or referenced as an asset dependency.
- OBJs, MTL, materials, and previews import as loose staging assets only.

## Unity Import Gates

- `Meshes/V134WPP_001_PressurePistolSightlinePolish_Staged.obj` imports with named pistol polish subobjects.
- `Meshes/V134WPP_002_SteamScattergunDisplayPolish_Staged.obj` imports with named display subobjects.
- `Meshes/V134WPP_003_AmmoPickupReadabilityPolish_Staged.obj` imports with named ammo-family subobjects.
- `Meshes/V134WPP_004_RouteConsolePropPolish_Staged.obj` imports with named route-console subobjects.
- `Meshes/V134WPP_Brassworks_PolishPalette.mtl` resolves all `MAT_V134WPP_*` material slots used by the staged OBJs.
- `Materials/M_V134WPP_*.mat` remain local staged proxies and are not moved into shared material libraries during smoke check.
- Preview SVGs open outside Unity and can be used for quick review without requiring a generated scene.

## Batch Readability Gates

- The integrated result reads as one coordinated weapon/prop polish batch, not four isolated props.
- Pressure pistol remains smaller, tighter, and more precise than the Steam Scattergun.
- Steam Scattergun display reads heavier, broader, and more soot-blackened than the pistol.
- Ammo pickups read as pressure cartridges before HUD feedback confirms collection.
- Route-console props read as industrial route dressing unless a separate interaction task explicitly owns behavior.
- Material roles remain separated: blackened iron, aged brass, copper lines, cream gauge faces, amber glass, warning red, walnut/leather, oil/soot, polished wear, route green, and heat steel.

## Target-Specific Gates

### Pressure Pistol

- First-person silhouette keeps receiver, pressure chamber, muzzle crown, coil pack, gauge, relief lever, and grip readable.
- Gauge needle, red pressure mark, dump lever, and vent details reinforce secondary-fire identity without changing weapon behavior.
- Brass and copper accents support the form but do not turn the pistol into a single gold mass.

### Steam Scattergun Display

- World pickup/display keeps yoke, barrel mass, shell rack, slug canister, top rib, walnut pump, and amber nameplate readable at route distance.
- Display stand does not block pickup reachability or confuse the unlock authority.
- Soot, blackened iron, and heat-stained steel carry the heavy weapon read.

### Ammo Pickups

- Ammo family supports at least two visual variants: pressure-cell bundle and scattergun slug/shell canister.
- Red pressure seals and brass cartridge bodies survive gameplay-distance inspection.
- Pickup visuals do not obscure pickup VFX, pickup audio timing, or existing pickup definitions.

### Route Console Props

- Valve wheel, gauge cluster, pilot lamps, pipe couplers, rivets, and oil grime align with existing boiler-control and valve-wheel console language.
- Console polish introduces no trigger volumes, interactables, prompts, puzzle state, route-state logic, or objective authority.
- Lamp colors do not contradict existing route-state language.

## Fail Conditions

- Any staged file is integrated one at a time without preserving the batch relationship.
- Any staged prop introduces gameplay authority, collision blockers, route blockers, or fake prompts.
- Any weapon polish reduces existing primary/secondary fire readability.
- Any route-console polish implies a required interaction that does not exist.
- Any material assignment collapses brass, copper, walnut, grime, and iron into a single brown/dark mass.

## Suggested Main-Lane Validation

- Unity import smoke for all v0.1.34 staged assets.
- Gameplay-distance visual review for pistol, scattergun pickup, ammo pickups, and console placements.
- Targeted weapon-switch and pickup-route smoke after the batch is integrated.
- Route audit before the full V0 matrix, especially around Level03 scattergun pickup and route-console placements.
- One full V0 matrix only after the batch is coherent and route-crowding fixes are complete.
