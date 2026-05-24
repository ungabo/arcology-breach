# Brassworks Breach Weapon Props Set 02 v0.1.40

This is a self-contained Unity Package Manager sidecar package for weapon and interactable prop asset candidates. It expands the steampunk FPS visual library only; it does not integrate gameplay, input, inventory, damage, or pickup logic.

## Visual Target

Set 02 focuses on hard-surface brassworks props that can be promoted later into first-person weapons, world weapons, ammo pickups, and interactable wall props. The set uses aged brass, oxidized copper, blackened iron, edge-worn gunmetal, varnished walnut, worn leather, amber and green pressure glass, paper labels, and red safety seals.

## Included Generator

After importing this package in an isolated Unity project, use:

- `Brassworks Breach/Sidecar Packs/Generate Weapon Props Set 02 v0.1.40`
- `Brassworks Breach/Sidecar Packs/Render Weapon Props Set 02 Previews v0.1.40`

The generator creates real Unity `.mat`, `.asset`, and `.prefab` files under this package's `Runtime` folders. The renderer creates PNG previews under `Documentation/ConceptRenders/V0_1_40_WeaponPropsSet02` in the repository root when the package is kept under `AssetPacks`.

## Runtime Outputs

Generated prefabs:

- `Runtime/Prefabs/BB_WPS02_PressurePistol_Frame_A.prefab`
- `Runtime/Prefabs/BB_WPS02_PressurePistol_Overcoil_B.prefab`
- `Runtime/Prefabs/BB_WPS02_PressurePistol_Snub_C.prefab`
- `Runtime/Prefabs/BB_WPS02_PressurePistol_BarrelAssembly.prefab`
- `Runtime/Prefabs/BB_WPS02_Scattergun_Body_Single.prefab`
- `Runtime/Prefabs/BB_WPS02_Scattergun_Body_TwinBoiler.prefab`
- `Runtime/Prefabs/BB_WPS02_Scattergun_Body_SawedSteam.prefab`
- `Runtime/Prefabs/BB_WPS02_AmmoCartridge_Long.prefab`
- `Runtime/Prefabs/BB_WPS02_AmmoCartridge_Cluster.prefab`
- `Runtime/Prefabs/BB_WPS02_WallWeaponRack_ThreeSlot.prefab`
- `Runtime/Prefabs/BB_WPS02_AmmoCabinet_Shell_Open.prefab`
- `Runtime/Prefabs/BB_WPS02_GearKey_Housing.prefab`
- `Runtime/Prefabs/BB_WPS02_PressureCell_Canister.prefab`
- `Runtime/Prefabs/BB_WPS02_TuningDialGauge_Panel.prefab`
- `Runtime/Prefabs/BB_WPS02_RepairTool_Clutter_A.prefab`
- `Runtime/Prefabs/BB_WPS02_RepairTool_Clutter_B.prefab`

Generated materials: 12 package-local material assets under `Runtime/Materials`.

Generated reusable meshes:

- `Runtime/Meshes/WPS02_Mesh_BeveledBox.asset`
- `Runtime/Meshes/WPS02_Mesh_TaperedGrip.asset`
- `Runtime/Meshes/WPS02_Mesh_GaugeNeedle.asset`
- `Runtime/Meshes/WPS02_Mesh_HexBolt.asset`

## Import Rule

Do not add this package directly to the main game until it passes isolated import, generator run, preview render pass, and sidecar validation. This package is intentionally a candidate library, not a gameplay integration.
