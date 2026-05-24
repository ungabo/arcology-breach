# Brassworks Breach Steamworks Level Kit

Version: `0.1.37-p001`

This is a UPM-style Unity sidecar package for generating modular steampunk level-kit assets. It is an isolated asset factory output, not a direct main-game integration. The package is designed to be copied into a throwaway Unity project or referenced as a local package for review before the main Brassworks Breach project imports anything into quarantine.

## What It Builds

Use the Unity menu item:

`Brassworks/Sidecars/Steamworks Level Kit v0.1.37/Generate Package Assets`

The generator creates visual-only prefab candidates under this package's `Runtime/` folders:

- `SCLVL_CorridorStraight_4m`
- `SCLVL_CorridorCorner_4m`
- `SCLVL_TJunction_4m`
- `SCLVL_BoilerAlcove_4m`
- `SCLVL_GaugeWall_4m`
- `SCLVL_RivetedVaultDoor_4m`
- `SCLVL_PressureLockDoorFrame_4m`
- `SCLVL_PipeRailing_4m`
- `SCLVL_CatwalkFloor_4m`
- `SCLVL_WallColumn_3m`
- `SCLVL_CeilingPipeCluster_4m`
- `SCLVL_ValveConsole`
- `SCLVL_VentSmokeEmitterAnchor`

Use the Unity menu item:

`Brassworks/Sidecars/Steamworks Level Kit v0.1.37/Render Preview PNGs`

That command renders Unity-only preview PNGs to:

`Documentation/ConceptRenders/V0_1_37_SteamworksLevelKitSidecar/`

## Package Boundary

This package intentionally contains only editor-side generation tooling plus generated visual assets. It does not add gameplay code, input changes, tags, layers, project settings, render pipeline settings, or scene placements.

The main project should intake the generated kit by quarantine import first, then promote selected assets only after route, readability, collision, and performance checks pass.

## Scale Contract

- Unity unit scale: `1 unit = 1 meter`
- Primary grid snap: `4m`
- Fine snap for trim: `0.5m`
- Corridor clear width target: `3.2m`
- Corridor visual shell width: `4m`
- Corridor clear height target: `2.65m`
- Door aperture target: `2.6m high x 2.4m wide`
- Combat loop target: `6m to 10m` usable diameter for skirmish spaces
- VR future note: avoid head-height bars, screen-filling steam, and narrow high-speed turns.

## Quarantine Import Recommendation

Ready state is `package-ready / generated-assets-pending` until the Unity menu generator has been run inside a sidecar or quarantine project. After generation, run preview rendering and the acceptance report checks before promotion.
