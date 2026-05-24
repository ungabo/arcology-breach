# Concept Render Plan

## Purpose

Create a review-only JPG lane for Brassworks Breach object and room concepts without touching Unity build assets. This folder reads from `Assets/_Project/ArtStaging/` and `Documentation/AssetPreviews/`, then writes only to `Documentation/ConceptRenders/`.

## Current Inputs

- Baseline previews: `Documentation/AssetPreviews/`.
- Staged PBR materials: `Assets/_Project/ArtStaging/MaterialsPBR/Previews/` and `Textures/`.
- Staged enemy blockouts: `Assets/_Project/ArtStaging/Enemies/*.obj` plus `ENEMY_BlockoutMaterials.mtl`.
- Staged weapon/prop blockouts: `Assets/_Project/ArtStaging/WeaponsProps/*.obj` plus `WPN_PROP_PICKUP_WeaponsProps_BlockoutMaterials.mtl`.
- No staged `ModularKit` mesh files were present during this pass.

## Render Lanes

1. Source contact sheets
   - `CONTACTSHEET_preview_material_sources.jpg`
   - `CONTACTSHEET_staged_material_pbr_batch01.jpg`
   - `CONTACTSHEET_staged_assets_current.jpg`

2. Object renders
   - Mockup silhouettes for pipe/valve/gauge, Pressure Pistol, Steam Scattergun, and Scrapper.
   - Staged OBJ turntables for enemy blockouts and weapon/prop blockouts.

3. Room renders
   - Mockup Brassworks Intake corridor and Steam-Baffle Approach mood boards.
   - Staged-material corridor mood using ArtStaging PBR base colors and staged OBJ silhouettes.

## Final-Target Gates

Mark future JPGs as `final-target` only after these are present and accepted by the main asset workflow:

- Final or near-final textured weapon meshes with stable material assignments.
- Final or near-final enemy meshes with rig-aware silhouette checks.
- ModularKit wall/floor/ceiling/door/gate meshes for real corridor/room assembly.
- Accepted PBR material maps, including base color, normal, and ORM/mask sets.
- Clear source paths for every rendered asset and no dependency on Unity scenes.

## Next Pass

- Add `RENDER_OBJECT_staged_modularkit_turntable.jpg` once ModularKit OBJ/FBX files exist.
- Add `RENDER_ROOM_staged_modularkit_corridor.jpg` once wall/floor/arch modules exist.
- Add per-asset JPG turntables for final weapon and enemy meshes when their staged files move beyond blockout.
- Keep all outputs in this folder and update `INDEX.md` for every new JPG.
