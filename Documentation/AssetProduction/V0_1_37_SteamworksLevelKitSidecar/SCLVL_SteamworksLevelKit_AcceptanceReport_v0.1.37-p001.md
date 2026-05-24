# SCLVL Steamworks Level Kit Acceptance Report

Version: `v0.1.37-p001`

## Decision

Status: `ready_for_throwaway_import_after_generator_run`

The sidecar package is structurally ready. It should not be promoted into the main game until Unity has generated the package runtime assets and a throwaway import has confirmed the menu tooling, prefab paths, and render outputs.

## Completed

- UPM package structure created in `AssetPacks/BrassworksBreach.SteamworksLevelKit`.
- Unity Editor generator defines all required modular corridor, room, door, trim, catwalk, pipe, gauge, valve, and steam-anchor prefabs.
- Unity-only preview renderer menu command added.
- Manifest includes prefab paths, dimensions, setpiece roles, dependencies, and import risks.
- Scale and snap metadata documented for Windows, Android, WebGL, and future VR considerations.
- Cheap validation passed for JSON parsing, conflict-marker scan, diff whitespace check, and C# delimiter balance.

## Pending Unity Validation

- Run local-package import in a throwaway Unity project.
- Execute generator menu item.
- Confirm generated prefabs, meshes, materials, and runtime manifest.
- Execute preview render menu item.
- Inspect PNG renders for steampunk identity, route readability, and scale.
- Run primary-project quarantine import only after throwaway import is clean.

Unity validation was not run from the primary project because this sidecar package is intentionally not referenced by `Packages/manifest.json`, and this lane is not allowed to mutate that file.

## Quarantine Import Gate

Quarantine import can start after the package is added as a local package and the generator succeeds. Direct production promotion is not approved yet.
