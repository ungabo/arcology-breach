# SCLVL Steamworks Level Kit Acceptance Report

Version: `v0.1.37-p001`

## Decision

Status: `ready_for_throwaway_import_after_generator_run`

The package is a self-contained UPM-style Unity asset factory. It is ready for quarantine or throwaway-project import to run the generator, render previews, and inspect generated prefab output.

## Required Validation Sequence

1. Add `AssetPacks/BrassworksBreach.SteamworksLevelKit` as a local package in a throwaway Unity project using Unity `6000.4.6f1` or later in the same line.
2. Run `Brassworks/Sidecars/Steamworks Level Kit v0.1.37/Generate Package Assets`.
3. Confirm `Runtime/Prefabs`, `Runtime/Materials`, `Runtime/Meshes`, and `Runtime/Metadata/SCLVL_SteamworksLevelKit_GeneratedManifest.json` populate.
4. Run `Brassworks/Sidecars/Steamworks Level Kit v0.1.37/Render Preview PNGs`.
5. Confirm three PNG files appear in `Documentation/ConceptRenders/V0_1_37_SteamworksLevelKitSidecar/`.
6. Import into a primary-project quarantine root only after the throwaway import is clean.

## Acceptance Gates

| Gate | Status | Notes |
| --- | --- | --- |
| UPM package structure | Pass | `package.json`, `README`, `CHANGELOG`, `Runtime`, `Editor`, `Samples~`, and `Documentation~` included. |
| Unity-only construction | Pass | Generator uses Unity mesh data and generated materials; no Blender or external DCC dependency. |
| Modular prefab list | Pass pending generator run | 13 required level-kit prefabs are defined in the generator and manifest. |
| Scale metadata | Pass | Scale and snap rules are documented in package and planning docs. |
| Preview/render path | Pass pending generator run | Menu item writes PNG files to the requested docs concept-render folder. |
| Primary project safety | Pass | Package does not edit main scenes, gameplay scripts, project settings, or manifest dependencies. |
| Cheap JSON validation | Pass | Package JSON, asmdef JSON, metadata JSON, and manifest JSON parse. |
| Conflict marker scan | Pass | No merge conflict markers found in owned scopes. |
| C# static plausibility | Pass | Editor scripts passed delimiter balance checks. |
| Collision readiness | Hold | Collision remains a main-lane route/collider task after visual review. |
| Final art readiness | Hold | Assets are procedural Unity proxies intended for lookdev, layout, and kit validation before final art promotion. |

## Import Risks

- Package paths in the manifest use the expected UPM runtime path `Packages/com.brassworks.sidecar.steamworks-level-kit/...`; local embedded-package path behavior should be confirmed in the throwaway import.
- Generated prefabs intentionally avoid gameplay scripts, so all interactability, doors, smoke, collision, nav blocking, and route-state behavior must be added by the main lane later.
- Dense rivets and pipe details are acceptable for lookdev but should be combined, instanced, or LOD-reduced before broad placement.
- Transparent steam is represented only as an anchor/socket; actual steam effects need platform-specific overdraw budgets.

## Unity Validation Note

Unity import/generation validation was not run in the primary project because this sidecar package is not referenced by `Packages/manifest.json`, and this lane is not allowed to edit that file. The correct next validation is a throwaway local-package import or a main-lane-approved quarantine import.
