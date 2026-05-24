# Brassworks Breach - Parallel Workstream Status

Last updated: `2026-05-23 22:33 -04:00`

Purpose: track side-agent work that can advance independently from the main Unity implementation lane. Side agents own separate documentation, art-staging, and view-only render scopes; code, generated scenes, and shared status docs remain in the main integration lane until their output is reviewed and merged.

## Main Integration Lane

Owner: primary Codex thread

Current focus:

- Keep the playable Windows build moving through versioned slices.
- Preserve scene-generation determinism and validation coverage.
- Integrate side-agent outputs only after review.
- Commit and push verified slices regularly.

Current verified local build:

- `v0.0.90`
- Build path: `Builds/Windows/v0.0.90/BrassworksBreach_v0.0.90.exe`
- Matrix result: `V0_BUILD_MATRIX_PASS`

## Active Side Agents

| Agent | ID | Scope | Allowed Write Files | Started | Status |
| --- | --- | --- | --- | --- | --- |
| Hilbert | `019e579b-41ba-7843-8c5f-17bc62683ca7` | AAA steampunk asset-pack production plan | `Documentation/PARALLEL_ASSET_PACK_PRODUCTION_PLAN.md`, `Documentation/PARALLEL_ASSET_ACCEPTANCE_CHECKLIST.md` | `2026-05-23 21:31 -04:00` | completed |
| Copernicus | `019e579b-7160-7b51-87ab-5bd7da355535` | Production map plans for current/future levels | `Documentation/PARALLEL_LEVEL_PRODUCTION_MAPS.md` | `2026-05-23 21:31 -04:00` | completed |
| Noether | `019e579b-965a-71c2-8141-51a85513a3bb` | Combat roster and weapon-family design spec | `Documentation/PARALLEL_COMBAT_ROSTER_AND_WEAPONS.md` | `2026-05-23 21:31 -04:00` | completed |
| Helmholtz | `019e579b-bc0a-7ba0-861f-a8bea9f75173` | Android/WebGL/SteamVR/Meta Quest readiness | `Documentation/PARALLEL_PLATFORM_PORTS_AND_VR_PLAN.md` | `2026-05-23 21:31 -04:00` | completed |
| Chandrasekhar | `019e57a1-724c-7971-8722-d635910c6f85` | Local Unity/Asset Store pack inventory | `Documentation/PARALLEL_LOCAL_ASSET_PACK_INVENTORY.md` | `2026-05-23 21:37 -04:00` | completed |
| Beauvoir | `019e57a1-a809-7f20-9ead-228500ae4ad9` | Concrete asset generation/import briefs | `Documentation/PARALLEL_ASSET_GENERATION_BRIEFS.md` | `2026-05-23 21:37 -04:00` | completed |
| Nietzsche | `019e57a8-8d30-7543-af03-13e33acbdd3d` | Asset viewing guide and preview swatches | `Documentation/ASSET_VIEWING_GUIDE.md`, `Documentation/AssetPreviews/` | `2026-05-23 21:45 -04:00` | completed |
| Curie | `019e57ac-ba96-75f2-a362-cc3af0f1d0cd` | High-fidelity north-star lookdev for corridor/door, pressure pistol, and Scrapper-like monster | `Documentation/AssetProduction/HighFidelityLookdev/`, `Documentation/ConceptRenders/`, optional `Assets/_Project/ArtStaging/HighFidelityLookdev/` | `2026-05-23 22:33 -04:00` | in-progress |
| Poincare | `019e57ad-10a2-71d3-b094-4c469c95ca42` | Staged modular environment kit meshes | `Assets/_Project/ArtStaging/ModularKit/`, `Documentation/AssetProduction/ModularKit/` | `2026-05-23 21:49 -04:00` | completed |
| Rawls | `019e57ad-3c85-71f3-b860-9187f4e58b2e` | Staged weapon and gameplay prop meshes | `Assets/_Project/ArtStaging/WeaponsProps/`, `Documentation/AssetProduction/WeaponsProps/` | `2026-05-23 21:49 -04:00` | completed |
| Linnaeus | `019e57ad-64ad-73c3-94e0-eb3c6980117e` | Staged mechanical enemy blockout meshes | `Assets/_Project/ArtStaging/Enemies/`, `Documentation/AssetProduction/Enemies/` | `2026-05-23 21:49 -04:00` | completed |
| Hooke | `019e57ae-0c70-74e2-9627-20c741592f05` | View-only object and room concept renders | `Documentation/ConceptRenders/` | `2026-05-23 21:50 -04:00` | completed |

## Completed Side-Agent Outputs

| Agent | Completed | Output | Top Integration Signal |
| --- | --- | --- | --- |
| Hilbert | `2026-05-23 21:35 -04:00` | `Documentation/PARALLEL_ASSET_PACK_PRODUCTION_PLAN.md`, `Documentation/PARALLEL_ASSET_ACCEPTANCE_CHECKLIST.md` | Start with material bible, modular corridor/pipe/gate/lift kit, gameplay props, final weapons, and Scrapper/Lancer packages. |
| Copernicus | `2026-05-23 21:36 -04:00` | `Documentation/PARALLEL_LEVEL_PRODUCTION_MAPS.md` | Turn Level03 into a cohesive scattergun/Bellows Node loop, build signage/decal kits, and make Level05 boss readability the map priority. |
| Noether | `2026-05-23 21:35 -04:00` | `Documentation/PARALLEL_COMBAT_ROSTER_AND_WEAPONS.md` | Lock weapon/enemy data contracts, improve Scrapper readability, and define prefab sockets/material/LOD standards before final asset import. |
| Helmholtz | `2026-05-23 21:35 -04:00` | `Documentation/PARALLEL_PLATFORM_PORTS_AND_VR_PLAN.md` | Move toward input/aim-provider abstractions, reusable HUD data, scalable VFX, and quality-tiered assets for mobile/WebGL/VR. |
| Beauvoir | `2026-05-23 21:41 -04:00` | `Documentation/PARALLEL_ASSET_GENERATION_BRIEFS.md` | First asset production batch should start with aged brass, blackened iron, soot/wet stone, trim sheet, corridor kit, pipe/valve/gauge kit, decals, final weapons, and Scrapper. |
| Chandrasekhar | `2026-05-23 21:44 -04:00` | `Documentation/PARALLEL_LOCAL_ASSET_PACK_INVENTORY.md` | Found 64 local Unity Asset Store `.unitypackage` files totaling about 18.83 GB; best sandbox candidates include Snaps Prototype Sci-Fi Industrial, Unity Particle Pack, Hovl fire packs, VR packages, and Japanese Alley. |
| Nietzsche | `2026-05-23 21:48 -04:00` | `Documentation/ASSET_VIEWING_GUIDE.md`, `Documentation/AssetPreviews/` | Created an asset viewing guide plus 7 preview/contact-sheet images; confirmed `.meta` files are Unity sidecar metadata, not the assets themselves. |
| Curie | `2026-05-23 22:01 -04:00` | `Assets/_Project/ArtStaging/MaterialsPBR/`, `Documentation/AssetProduction/MaterialsPBR/` | Created the first staged PBR material batch: 8 material IDs, 24 texture PNG maps at 1024px, manifests, acceptance checklist, and preview contact sheets. |
| Linnaeus | `2026-05-23 21:58 -04:00` | `Assets/_Project/ArtStaging/Enemies/`, `Documentation/AssetProduction/Enemies/` | Created enemy OBJ blockouts for Scrapper, Lancer, Sentinel Turret, and reusable mechanical parts, plus manifests and preview sheets. |
| Rawls | `2026-05-23 22:00 -04:00` | `Assets/_Project/ArtStaging/WeaponsProps/`, `Documentation/AssetProduction/WeaponsProps/` | Created weapon/prop OBJ blockouts for Pressure Pistol, Steam Scattergun, ammo pickups, pressure station, and crank lever, plus manifests and preview sheets. |
| Hooke | `2026-05-23 22:06 -04:00` | `Documentation/ConceptRenders/` | Created 13 view-only JPG concept renders/contact sheets plus index, render plan, and work log. Best first file: `Documentation/ConceptRenders/CONTACTSHEET_staged_assets_current.jpg`. |
| Poincare | `2026-05-23 22:11 -04:00` | `Assets/_Project/ArtStaging/ModularKit/`, `Documentation/AssetProduction/ModularKit/` | Created first modular corridor kit with 12 OBJ meshes, 8 base-color texture PNGs, material library, manifests, previews, and contact sheet. |
| Hooke | `2026-05-23 22:29 -04:00` | `Documentation/ConceptRenders/` | Added batch 02 staged room/object JPGs for modular corridor, pressure-gate control alcove, enemy lineup, weapon/prop lineup, and a combined contact sheet. |

## Active Reassigned Work

| Agent | Reassigned | Output Scope | Target |
| --- | ---:| --- | --- |
| Curie | `2026-05-23 22:33 -04:00` | `Documentation/AssetProduction/HighFidelityLookdev/`, `Documentation/ConceptRenders/`, optional `Assets/_Project/ArtStaging/HighFidelityLookdev/` | Push static material/model lookdev toward the north-star image: realistic wet dark stone, riveted bronze/iron corridor and pressure door, chunky pressure pistol, and Scrapper-like mechanical monster. Rigging and gameplay integration are deferred. |

## Integration Rules

- Side agents do not edit Unity scenes, generated scene-builder code, existing roadmap/status docs, or shared gameplay scripts.
- Art-production side agents may write only to their assigned `Assets/_Project/ArtStaging/` folders and matching `Documentation/AssetProduction/` folders.
- View-only render side agents may write only to `Documentation/ConceptRenders/`; those JPGs are for review and are intentionally outside Unity build assets.
- Main lane continues implementation while side agents work.
- When side-agent output returns, review it before merging into the main docs.
- Convert accepted side-agent output into concrete implementation slices, asset tasks, and validation requirements.
- If a side-agent recommendation conflicts with current playable build needs, keep the playable build stable and move the recommendation to a later task.

## Good Parallel Lanes

- Asset-pack planning, generation prompts, acceptance criteria, and import rules.
- Level paper maps, encounter beats, production footprint estimates, and secrets.
- Combat role specs, tells, animation/VFX/audio requirements, and test hooks.
- Platform-port constraints, VR comfort requirements, input abstraction notes, and quality tiers.
- QA checklists and manual playtest forms.

## Reserved For Main Lane

- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- Generated `.unity` scenes.
- Shared gameplay scripts.
- Build/version docs that reflect current verified executable state.
- Git commits/pushes to `main`.
