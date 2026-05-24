# v0.1.35 Staged Bundle Integration Checklist

Created: `2026-05-24`

Owned scope: `Documentation/AssetProduction/V0_1_35_PerformanceImportBundle/`

## Use

Run this checklist before promoting any `v0.1.35` staged bundle from documentation/art staging into gameplay scenes or prefabs.

## All Bundles

- [ ] Package lives under the expected staging path and has a matching manifest.
- [ ] Manifest lists package id, version, asset ids, source files, scale, material recipes, collider policy, LOD policy, and preview outputs.
- [ ] Preview/contact sheet exists and matches the manifest.
- [ ] Meshes import at the expected scale with correct orientation, pivots, normals, and hierarchy.
- [ ] MTL/generated materials have been replaced or mapped to shared Unity recipe materials.
- [ ] No pink/magenta material output appears in preview or staging.
- [ ] Texture paths are relative, supported, correctly named, and sized for target tier.
- [ ] Audio files have event-role names, short durations unless justified, and safe missing-reference fallback.
- [ ] Sprites define pivot, pixels-per-unit, alpha behavior, and intended event.
- [ ] Collider count and type match primitive-only policy.
- [ ] LOD0/LOD1/LOD2 intent is documented before repeated placement.
- [ ] Light/shadow settings follow the low/mid PC policy.
- [ ] Route-authority statement confirms visual-only or names the main-lane owner.

## Weapon Arsenal

- [ ] Pressure Pistol viewmodel crop preserves barrel direction, gauge/lamp read, and hand-fit assumptions.
- [ ] Steam Scattergun crop preserves triple-barrel read, pump/stock grip volumes, and heavy silhouette.
- [ ] Ammo cartridge family keeps standard, high-pressure, empty, and ruptured reads distinct.
- [ ] Wall display frame does not place backplate or hooks in front of existing wall/route collision.
- [ ] Ammo cabinet has one simple cabinet collider plan and a front interaction/pickup authority note.
- [ ] Future alt weapon silhouettes are marked prototype-only and cannot block current integration.
- [ ] Weapon LOD1 removes rivets, small coils, gauge needles, soot cards, and labels before silhouette detail.
- [ ] Weapon pickup/display cues use green/amber/red route language consistently.

## Mechanical Enemies

- [ ] Scrapper remains low/fast with saw/claw silhouette visible at combat distance.
- [ ] Lancer keeps tall lance and cyan charge tell readable against machinery backgrounds.
- [ ] Bulwark keeps shield-wall mass, hammer, and guard-break read after LOD reduction.
- [ ] Warden keeps command silhouette, twin coils, boss read, and phase/weak lamp identity.
- [ ] Foundry Overseer Elite remains clearly elite/miniboss and separable from Warden.
- [ ] `SOCK_` placeholders are preserved for hips, spine cage, tools, coil/backpack, weak lamp, and shutdown burst.
- [ ] Body collision is one simple capsule/box proxy unless main-lane combat validation owns exceptions.
- [ ] Tool and weak-point guide volumes are simple and cannot become unauthorized gameplay authority.
- [ ] Shutdown fragments are visual-only and cannot block movement, projectiles, lifts, hoists, or exits.

## Level Modules

- [ ] Modules use documented snap origins and grid-friendly dimensions.
- [ ] Decorative modules do not narrow required route apertures or hide prompts, exits, valves, lifts, hoists, hazards, pickups, or boss locks.
- [ ] Blocking collision is explicit, primitive, and owned by the route-authority/main-lane plan.
- [ ] Repeated modules share materials and meshes for batching/instancing readiness.
- [ ] Small trims, rivets, pipes, gauges, and labels do not cast realtime shadows.
- [ ] LOD2 keeps route state, hazard language, and destination readability.
- [ ] Placement candidates are validated from intended player approach direction, not only from editor orbit view.

## Feedback Polish

- [ ] Feedback assets are presentation-only by default.
- [ ] No feedback prefab contains collider, trigger, Rigidbody, NavMeshObstacle, damage, pickup, prompt, objective, transition, save, secret, boss-lock, or route-state authority.
- [ ] Weapon fire, hit, miss, empty, pickup, and switch feedback remain distinguishable.
- [ ] Enemy hit/death/shutdown feedback does not hide windups, projectiles, weak points, or boss HUD.
- [ ] Pickup feedback differentiates resource, route item, and capability acquisition.
- [ ] Objective feedback respects amber/red/green route language.
- [ ] Secret feedback is lower priority than main route feedback.
- [ ] Pause/settings/audio feedback remains readable and reversible.
- [ ] Missing final VFX/audio fails silently or falls back to safe placeholders.

## Final Hold Conditions

Hold integration if any item below is true:

- Manifest or preview evidence is missing.
- Import scale, pivot, orientation, hierarchy, or material mapping is unknown.
- Pink/magenta materials appear anywhere in the candidate.
- Collider policy is not primitive-only or contains unauthorized triggers.
- LOD policy is absent for repeated assets.
- Texture memory, material count, or shadow cost has no budget estimate.
- Asset package introduces route authority or hides existing route authority.
- Low PC settings make weapon, enemy, pickup, objective, hazard, boss, or exit reads worse.

