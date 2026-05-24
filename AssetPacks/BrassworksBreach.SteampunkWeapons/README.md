# Brassworks Breach Steampunk Weapons v0.1.37

This is a self-contained Unity Package Manager sidecar package for weapon and hand-prop asset production. It is designed to be imported into a quarantine Unity project first, then promoted into the main Brassworks Breach project after validation.

## Visual Target

The package follows the north-star steampunk direction in `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`: aged brass, oxidized copper, blackened iron, varnished dark wood, worn leather, amber pressure glass, compact mechanical silhouettes, and visible pressure instrumentation.

## Included Generator

After importing the package, use:

- `Brassworks Breach/Sidecar Packs/Generate Steampunk Weapon Pack v0.1.37`
- `Brassworks Breach/Sidecar Packs/Render Steampunk Weapon Previews v0.1.37`

The first menu item creates real Unity `.mat` and `.prefab` assets under this package's `Runtime` folders. The second menu item renders isolated PNG previews without adding anything to the active game scenes.

## Runtime Outputs

Generated prefabs:

- `Runtime/Prefabs/BB_V0137_PressurePistolCore.prefab`
- `Runtime/Prefabs/BB_V0137_CopperCoilAssembly.prefab`
- `Runtime/Prefabs/BB_V0137_BrassDialGaugeAssembly.prefab`
- `Runtime/Prefabs/BB_V0137_LeatherGrip.prefab`
- `Runtime/Prefabs/BB_V0137_PressureCartridge.prefab`
- `Runtime/Prefabs/BB_V0137_AmmoCabinetShell.prefab`
- `Runtime/Prefabs/BB_V0137_WallWeaponDisplay.prefab`

Generated materials:

- `Runtime/Materials/BB_AgedBrass.mat`
- `Runtime/Materials/BB_OxidizedCopper.mat`
- `Runtime/Materials/BB_BlackenedIron.mat`
- `Runtime/Materials/BB_DarkVarnishedWood.mat`
- `Runtime/Materials/BB_WornLeather.mat`
- `Runtime/Materials/BB_GlowingAmberGlass.mat`
- `Runtime/Materials/BB_OilyWetStone.mat`

## Import Rule

Do not add this package directly to the main game until it passes a clean quarantine import, generator run, preview render pass, and cheap compile/import smoke.
