# v0.1.35 Performance + Import Validation Gates

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_35_PerformanceImportBundle/`

## Purpose

Catch asset-import and performance-readiness problems before staged weapon, enemy, level-module, and feedback packages are promoted into gameplay scenes.

## Required Evidence Before Promotion

Each staged bundle should have:

- A manifest with package id, version, asset ids, source paths, expected scale, material recipes, collider policy, LOD policy, and preview outputs.
- At least one Unity-readable preview/contact sheet or equivalent proof path.
- Import stats for mesh count, triangle count, material slot count, texture count/resolution, audio count/duration, sprite count, collider plan, and LOD plan.
- A written route-authority statement: visual-only, presentation-only subscriber, or main-lane-owned gameplay authority.
- A missing-reference fallback statement for materials, textures, sprites, audio, and VFX.

## Import Error Gate

Fail or hold if any are true:

- OBJ, MTL, texture, audio, or sprite paths are missing, case-mismatched, duplicated with conflicting names, or outside the expected staging package.
- Meshes import with unexpected scale, rotation, pivot, negative scale, broken normals, inverted faces, or fragmented hierarchy that blocks component selection.
- Materials are orphaned, duplicated per object without need, or assigned to the wrong recipe.
- Texture references point to local-only absolute paths or unsupported file types.
- Audio files clip, loop unexpectedly, or have unknown channel/sample-rate/compression expectations.
- Sprite sheets lack cell size, pivot, pixels-per-unit, alpha, or naming guidance.

## Shader And Material Gate

Automatic hold:

- Any visible pink/magenta material in preview, prefab staging, or representative scene.
- Missing shader fallback for core recipes: aged brass, blackened iron, glass/lamp, soot/wear, hazard trim, cyan tell, amber weak point, route red/green/amber.
- Emissive materials overpower enemy tells, hazard language, route colors, pickup prompts, boss HUD, or final exit state.
- Transparent steam, glass, or sprite effects create heavy overdraw without low-quality fallback.

Targeted checks:

- View the asset under neutral light and darker route lighting.
- Verify metallic/smoothness ranges are plausible and do not flatten silhouettes.
- Verify color language: green means stocked/restored/safe/exit, amber means attention/objective/charged, red means locked/hostile/danger.
- Verify weak-point amber/red and attack-tell cyan/blue remain distinct after downscaling.

## Collider And Physics Gate

Automatic fail:

- Mesh collider on decorative coils, rivets, pipes, gauges, labels, wires, soot cards, preview-only props, pickup glints, or feedback VFX.
- Trigger, NavMeshObstacle, Rigidbody, damage, pickup, prompt, objective, save, transition, boss-lock, route-state, or enemy-hitbox authority added by a visual-only staged asset.
- Collider shape blocks a lift, gate, hoist, route aperture, prompt volume, pickup root, enemy tell, hazard warning, final exit, or boss arena flow.

Expected policy:

- Weapons: simple boxes/capsules for receiver, barrel, grip, stock, tank, and pickup trigger only when main lane owns the trigger.
- Enemies: one capsule/box for body proxy; simple tool/weak volumes only when main lane owns combat validation.
- Level modules: blocking collision uses existing route-authority owners; decorative setpieces use no collision or simplified primitives.
- Feedback polish: no colliders and no triggers by default.

## LOD And Shadow Gate

Hold if:

- LOD0 is the only defined mesh for a repeated prop, enemy, or level module.
- LOD1 does not remove small silhouette-noise detail.
- LOD2 destroys gameplay readability, attack tells, pickup identity, objective state, or route color language.
- Small decorative detail casts realtime shadows.
- LOD swaps create visible pop on route-critical reads.

Required LOD intent:

- LOD0: final readable silhouette and local detail.
- LOD1: remove rivets, gauge needles, tiny coils, wires, soot cards, micro labels, and duplicate trims.
- LOD2: preserve broad class silhouette, route state, weak point, pickup identity, and major hazard/exit language.

## Missing Preview Or Manifest Gate

Automatic hold:

- No manifest exists for the staged bundle.
- Manifest lists preview outputs that are missing or stale.
- Preview sheet does not show front/side/readability angles for weapons, enemies, or level modules.
- Feedback polish lacks representative stills or written fallback behavior.
- The manifest does not identify whether the package is visual-only or presentation-only.

## Route-Authority Gate

Reject any staged bundle that:

- Creates new route, pickup, damage, objective, prompt, save, transition, boss-lock, secret, spawn, or nav authority.
- Moves existing route-authority objects or obscures their prompts/cues.
- Adds a collider/trigger that changes player or projectile traversal.
- Requires final art/audio to make gameplay state valid.
- Conflicts with established amber/red/green route language.

Allowed:

- Visual children on existing authoritative roots.
- Presentation subscribers that respond to existing events.
- Missing-asset-safe placeholder materials, sprites, audio, and VFX.
- Non-authoritative preview objects kept out of gameplay scenes until promoted by the main lane.

## Human Review Gate

Before full matrix validation, a human first-person pass should confirm:

- Weapons crop correctly in first-person and pickup view.
- Enemy silhouettes and attack tells remain readable in active lighting.
- Level modules do not narrow movement lanes or hide route affordances.
- Feedback polish does not hide HUD, prompts, hazards, enemy tells, boss HUD, final exit, or pause/settings states.
- Low-quality settings still preserve the intended read.

