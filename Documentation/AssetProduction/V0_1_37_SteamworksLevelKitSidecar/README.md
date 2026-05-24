# v0.1.37 Steamworks Level Kit Sidecar

Created: `2026-05-24`

Owned package root: `AssetPacks/BrassworksBreach.SteamworksLevelKit/`

This lane creates a Unity-only modular level-kit asset factory for Brassworks Breach. The kit targets the steampunk north-star look: blackened riveted iron, aged brass, copper pressure pipes, oily stone, soot brick, amber lamps, green route-state glass, red pressure warnings, gauges, boiler silhouettes, vault machinery, and controlled steam sockets.

## Delivered Output

- UPM-style package: `AssetPacks/BrassworksBreach.SteamworksLevelKit`
- Package manifest: `SCLVL_SteamworksLevelKit_Manifest_v0.1.37-p001.json`
- Acceptance report: `SCLVL_SteamworksLevelKit_AcceptanceReport_v0.1.37-p001.md`
- Unity editor generator: `SteamworksLevelKitGenerator.cs`
- Unity preview renderer: `SteamworksLevelKitPreviewRenderer.cs`
- Planning docs for level scale, map expansion, platform simplification, and VR readiness.

## Required Unity Menu Flow

1. `Brassworks/Sidecars/Steamworks Level Kit v0.1.37/Generate Package Assets`
2. `Brassworks/Sidecars/Steamworks Level Kit v0.1.37/Render Preview PNGs`

The generator defines 13 prefabs and creates their mesh, material, prefab, and generated manifest assets inside the package runtime folders.

## Intended Intake

This pack should be tested as a local package in a throwaway Unity project first. If clean, the main game lane should import it into a quarantine staging area, review prefab dimensions and route clearance, then promote selected modules into production usage.
