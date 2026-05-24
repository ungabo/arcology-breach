# v0.1.35 Performance Budgets

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_35_PerformanceImportBundle/`

## Target Hardware Tiers

| Tier | Target | Working assumption |
| --- | --- | --- |
| Windows low PC | Playable and readable at 720p to 900p, reduced shadows/effects. | Older 4-core CPU, 8 GB RAM, integrated or low-end discrete GPU. |
| Windows mid PC | Smooth default target at 1080p. | 6-core CPU, 16 GB RAM, mainstream discrete GPU. |
| Deferred Android | Keep assets reducible for mobile thermal and memory limits. | TBD after Windows v0 pipeline is stable. |
| Deferred WebGL | Keep import paths compatible with compressed textures, fewer materials, and fewer runtime allocations. | TBD after browser delivery path is active. |
| Deferred SteamVR / Meta | Preserve strong silhouettes, low overdraw, and predictable frame timing. | TBD after non-VR combat readability is stable. |

## Windows Low PC Budgets

| Category | Budget | Notes |
| --- | --- | --- |
| Frame target | 30 FPS floor, no repeated stalls above 50 ms during normal play. | Prefer stable frame pacing over high visual density. |
| Camera-visible triangles | 180k to 250k practical scene target, with spikes reviewed manually. | Includes level modules, enemies, weapon view, pickups, and feedback. |
| Hero first-person weapon | LOD0 under 12k triangles; LOD1 under 5k; LOD2 under 1.5k. | Decorative coils, rivets, labels, needles, and soot cards are first reduction candidates. |
| Mechanical enemy on screen | Common enemies under 8k triangles LOD0 each; heavy/boss under 18k LOD0. | Enemies must keep attack tells and weak lamps after reduction. |
| Level module kit | Repeated modules under 6k triangles each at LOD0; trims under 1k. | Build rooms from repeated pieces with batching-friendly materials. |
| Materials per prefab | 1 to 4 preferred, 6 maximum for hero/boss assets. | Use shared brass/iron/glass/emissive/hazard recipes. |
| Texture memory per staged pack | 128 MB review threshold, 256 MB hard hold until justified. | Prefer atlases, shared masks, and capped preview textures. |
| Texture dimensions | 2048 max for hero, 1024 max for standard props/enemies, 512 for trim/decal/sprite where readable. | Downscale before shipping low tier if detail is not readable in play. |
| Realtime lights | 0 dynamic per decorative prop; 1 local effect light maximum per active high-value moment. | Prefer emissive materials and baked/static light contribution. |
| Shadow casters | Only player-important silhouettes, large enemies, and major blockers. | Disable shadows on rivets, small coils, gauges, wires, sprites, particles, and pickup glints. |
| Colliders | Primitive-only for staging; 1 to 5 per prop; enemy proxy body plus required simple tool/weak volumes. | Mesh colliders are rejected unless main lane explicitly owns and validates the exception. |
| Audio voices | Keep burst events short; avoid stacked duplicate loops. | One-shot confirmations should not mask enemy tells or route feedback. |
| Particle systems | 1 to 3 active systems per event, capped emission, no collision by default. | Disable soft particles/expensive overdraw on low settings where possible. |

## Windows Mid PC Budgets

| Category | Budget | Notes |
| --- | --- | --- |
| Frame target | 60 FPS target at 1080p in representative flow. | Brief heavy-combat dips require review if readability worsens. |
| Camera-visible triangles | 350k to 500k practical scene target. | Boss/setpiece scenes may spike with documented LOD fallbacks. |
| Hero first-person weapon | LOD0 under 20k triangles; LOD1 under 8k; LOD2 under 2.5k. | Viewmodel crops still matter more than raw detail. |
| Mechanical enemy on screen | Common enemies under 12k LOD0 each; heavy/boss under 30k LOD0. | Material count and shadow cost are usually bigger risks than mesh alone. |
| Level module kit | Repeated modules under 10k LOD0, with trim variants instanced/shared. | Do not make every placed module unique. |
| Materials per prefab | 1 to 5 preferred, 8 maximum for hero/boss review. | Use MaterialPropertyBlock-style variation later instead of duplicate materials. |
| Texture memory per staged pack | 256 MB review threshold, 512 MB hard hold until justified. | Includes albedo, normal, mask, emission, decals, sprites, and UI preview assets. |
| Realtime lights | Use sparingly for active events only. | Decorative glow should be emissive, baked, or disabled on low quality. |
| Shadow casters | Allow major silhouettes and active threats. | Keep small detail non-shadowing. |
| Audio voices | Mix remains intelligible in combat, pickup, objective, and pause/settings states. | Prioritize player weapon, enemy tells, route, low health, then polish. |

## Deferred Platform Notes

### Android

- Plan for 512 to 1024 textures on most world/enemy assets and aggressive audio compression.
- Avoid shader features that require desktop-only lighting assumptions.
- Keep material slots low; mobile batching suffers when decorative pieces all have unique materials.
- Particle overdraw, transparent steam, and emissive cards need low-quality fallbacks.
- Preserve primitive collider policy because physics overhead will be more visible on mobile CPUs.

### WebGL

- Keep import packages deterministic and manifest-driven so browser builds can strip unused variants.
- Avoid large texture sets and long uncompressed audio clips.
- Prefer shared materials and atlased sprites to reduce WebGL state changes.
- Treat runtime decompression/allocation spikes as likely browser stutter risks.
- Keep missing-asset fallback behavior safe because browser delivery can expose case/path mistakes.

### SteamVR / Meta

- VR readiness depends on stable frame time, strong silhouettes, and low overdraw.
- Avoid screen-filling steam, flash, and pickup bursts that would be uncomfortable in stereo.
- Keep viewmodel weapons and enemy attack tells readable from both eyes and at different IPDs later.
- Do not bake gameplay understanding into tiny gauges, labels, or color-only cues.
- Meta/mobile VR will inherit Android-like constraints plus stricter frame pacing needs.

## Budget Review Rules

- A staged package may exceed one budget only with a clear reduction path and a human-readable reason.
- Any pack with unknown triangle counts, unknown material count, unknown texture memory, or unknown collider count is not ready for prefab promotion.
- Final asset swaps must be compared against proxy budgets before replacing working placeholders.
- Visual proof sheets are not performance evidence; they must be paired with import stats and real-scene readability review.

