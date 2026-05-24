# Brassworks Breach - Parallel Asset Acceptance Checklist

Created: 2026-05-23 21:31 -04:00

Purpose: use this checklist before any independently produced asset enters the main `Brassworks Breach` Unity project.

This checklist is for the parallel asset-pack lane. It does not authorize edits to Unity scenes, scripts, README, existing roadmaps, or status docs.

## Asset Intake Summary

Fill this out for every candidate asset.

| Field | Value |
| --- | --- |
| Asset ID | TBD |
| Asset name | TBD |
| Category | Material / Environment / Prop / Weapon / Enemy / Animation / VFX / Audio / UI |
| Source | Procedural / Local asset pack / Unity account asset / External model / External audio / Other |
| License confirmed | Yes / No / TBD |
| Intended level or mechanic | TBD |
| North-star reference | Level sheet / Weapon sheet / Creative direction / Other |
| Platform targets included | Windows / Android / WebGL / SteamVR / Meta Quest |
| Status | briefed / generated / staged / accepted / integrated / rework / deferred |
| Reviewer | TBD |
| Date reviewed | TBD |

## Style Acceptance

- [ ] Reads as steampunk first: brass, copper, iron, stone, wood, pressure, gauges, valves, gears, steam, oil, or furnace logic.
- [ ] Uses stylized silhouette and readable forms rather than generic photoreal clutter.
- [ ] PBR surface detail is high fidelity without losing the bold `Brassworks Breach` identity.
- [ ] Avoids cyberpunk/neon city language, black chrome, clean sci-fi, holograms, sleek robots, and digital display styling.
- [ ] Important gameplay features are readable at normal play distance.
- [ ] Color coding matches the project language: amber useful, red/orange danger, green service/exit/restored machinery, cream enamel readable labels.
- [ ] Detail density does not obscure enemies, pickups, exits, hazards, or interactables.

## Technical Acceptance

- [ ] File names follow the project naming convention.
- [ ] Asset has a clear category prefix such as `Mat`, `T`, `SM`, `SK`, `PF`, `AN`, `VFX`, `AUD`, or `COL`.
- [ ] Unity scale is correct: 1 Unity unit = 1 meter.
- [ ] Pivot supports placement or interaction.
- [ ] Transforms are applied before export.
- [ ] Materials are assigned intentionally, with no orphan/default material slots.
- [ ] Texture maps use correct color space: sRGB for color/emission, linear for masks/normal data.
- [ ] Normal maps import as normal maps.
- [ ] Mipmaps are correct for usage.
- [ ] Read/Write is disabled unless specifically required.
- [ ] Mesh optimization settings are compatible with the asset.
- [ ] Static environment pieces have lightmap UVs or an explicit reason not to.
- [ ] No high-detail render mesh is used as gameplay collision for common assets.
- [ ] Asset has no missing scripts, missing materials, missing textures, or broken prefab links.

## LOD and Collision Acceptance

- [ ] Repeated environment meshes include LODs or meet the no-LOD budget exception.
- [ ] Hero props and enemies include LOD plans even if final LODs are pending.
- [ ] LOD transitions preserve silhouette and gameplay readability.
- [ ] Enemy LODs preserve attack-tell readability.
- [ ] Collision meshes are simple and named with `COL`.
- [ ] Trigger colliders are used for pickups/interactions where appropriate.
- [ ] Navigation footprint is documented for enemies and large props.
- [ ] VR close-range scale has been considered.

## Platform Variant Acceptance

Windows:

- [ ] Meets mid-to-low gaming PC target.
- [ ] Texture sizes are reasonable for 4 GB VRAM planning.
- [ ] Material count is controlled.
- [ ] VFX density is readable but not excessive.

Android:

- [ ] Reduced texture sizes exist or are planned.
- [ ] Shader/material complexity can be reduced.
- [ ] Mesh and LOD plan supports lower memory.
- [ ] Audio can be compressed.

WebGL:

- [ ] Download and memory footprint are considered.
- [ ] Avoids unnecessary unique large textures.
- [ ] Avoids heavy transparency and expensive shaders.
- [ ] Uses simplified VFX/audio variants where needed.

SteamVR:

- [ ] Readable at VR scale and distance.
- [ ] Avoids flicker, tiny critical text, and excessive near-face noise.
- [ ] LOD popping risk is considered.
- [ ] Interaction affordances can become world-space/hand-based later.

Meta Quest:

- [ ] Quest/mobile material fallback is plausible.
- [ ] Low VFX density variant is plausible.
- [ ] Texture and mesh reductions are plausible.
- [ ] Collision and scale support comfortable VR.

## Category-Specific Checks

### Materials and Textures

- [ ] Base color, normal, mask, and emission maps are present as needed.
- [ ] Roughness/metallic/AO packing is documented.
- [ ] Tileables loop cleanly.
- [ ] Trim sheets have documented texel density and intended use.
- [ ] Decals have transparent or mask data set correctly.
- [ ] Downscale variants remain readable.

### Modular Environment

- [ ] Snaps to the intended grid.
- [ ] Dimensions are documented in meters.
- [ ] Works with existing first-person scale and corridor width.
- [ ] Has clean seams and repeatable variations.
- [ ] Does not block player or enemy movement unexpectedly.
- [ ] Supports baked lighting.

### Props and Pickups

- [ ] Gameplay purpose is visually obvious.
- [ ] Pickup/interactable state is readable.
- [ ] Has simple trigger/collision setup.
- [ ] Has optional VFX/audio anchor points if needed.
- [ ] Does not rely on long text to communicate its purpose.

### Weapons

- [ ] First-person silhouette is readable and does not block too much screen.
- [ ] World pickup silhouette is distinct.
- [ ] Muzzle, pressure vent, shell/steam, and VFX anchor points are documented.
- [ ] Grip/hand alignment is documented for future VR support.
- [ ] Alternate fire visual distinction is supported.
- [ ] Animation clip list is included.

### Enemies

- [ ] Role is readable from silhouette.
- [ ] Attack tell is visually distinct.
- [ ] Weak/important regions are clear only if gameplay supports them.
- [ ] Rig plan and collider plan are included.
- [ ] VFX/audio sockets are documented.
- [ ] Shutdown/death state is readable.
- [ ] LODs preserve targetable mass and tell shapes.

### Animations

- [ ] Clip names are descriptive.
- [ ] Looping clips loop cleanly.
- [ ] Attack tell timing is documented.
- [ ] Hit/shutdown timing leaves gameplay readable.
- [ ] Root motion is disabled unless explicitly approved.
- [ ] Generic/Humanoid rig choice is documented.

### VFX

- [ ] Effect purpose is clear: weapon, impact, pickup, hazard, enemy state, objective, or UI.
- [ ] Particle count has low/medium/high budget notes.
- [ ] Lifetime is short enough for combat readability.
- [ ] Color matches gameplay language.
- [ ] Low-density mobile/VR variant is planned.
- [ ] Does not cause full-screen readability loss.

### Audio

- [ ] Clip category is documented.
- [ ] Mono/stereo choice is correct.
- [ ] Loop points are clean for ambience.
- [ ] One-shots are short and layered.
- [ ] Loudness is plausible relative to existing cue family.
- [ ] Compression settings are planned for platform variants.

## Acceptance Decision

Choose one:

- [ ] Accepted for main integration.
- [ ] Accepted for staging only.
- [ ] Rework required.
- [ ] Deferred.
- [ ] Rejected.

Required reviewer notes:

```text
TBD
```

Required follow-up if not accepted:

```text
TBD
```
