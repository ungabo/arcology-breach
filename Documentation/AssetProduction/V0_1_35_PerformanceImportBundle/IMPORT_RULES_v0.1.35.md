# v0.1.35 Staged Asset Import Rules

Created: `2026-05-24`

Owned scope: `Documentation/AssetProduction/V0_1_35_PerformanceImportBundle/`

## Applies To

- `V0_1_35_WeaponArsenal`
- `V0_1_35_MechanicalEnemyPack`
- `V0_1_35_LevelModuleSetpieces`
- `V0_1_35_FeedbackPolish`

These rules cover OBJ, MTL, material, texture, audio, and sprite staging packages before gameplay-scene promotion.

## Naming

Use stable package prefixes:

| Package | Prefix | Example |
| --- | --- | --- |
| Weapon arsenal | `V0135WA_` | `V0135WA_001_PressurePistol_FinalComponentPack.obj` |
| Mechanical enemies | `V0135ME_` | `V0135ME_Scrapper_LOD0.obj` |
| Level modules | `V0135LM_` | `V0135LM_BoilerLiftFrame_A_LOD0.obj` |
| Feedback polish | `V0135FB_` | `V0135FB_WeaponImpact_SteamPuff.sprite` |

Rules:

- Keep asset ids stable once listed in a manifest.
- Include role and LOD in mesh names: `_LOD0`, `_LOD1`, `_LOD2`.
- Use `SOCK_` for sockets, `COL_` for collider guide meshes, `FX_` for visual-only effects, and `PREVIEW_` for non-gameplay proof assets.
- Do not use spaces, versionless duplicate names, or case-only filename differences.
- Do not rename existing main-lane authority objects from an asset package.

## Scale, Orientation, And Pivot

| Asset type | Scale | Pivot |
| --- | --- | --- |
| First-person weapons | 1 Unity unit = 1 meter; preserve authored real-world length in manifest. | Grip/hand root or receiver root, documented per weapon. |
| Weapon pickups / props | 1 Unity unit = 1 meter. | Bottom-center for placed pickup, or mount socket for wall display. |
| Mechanical enemies | 1 Unity unit = 1 meter; match manifest heights. | Grounded between feet at body/root center. |
| Level modules | 1 Unity unit = 1 meter; snap dimensions use grid-friendly increments. | Bottom-left/back or module snap origin, documented in manifest. |
| Feedback sprites/VFX | Match target event scale in meters. | Center for bursts, bottom for ground puffs, muzzle/socket for weapon effects. |

Import expectations:

- Forward/up orientation must be documented before prefab conversion.
- No negative scale in promoted prefabs.
- Do not rely on parent scaling to fix wrong mesh scale.
- Keep component hierarchy/object names for staged weapons and enemies so sockets and tools remain selectable.

## OBJ And MTL

- Import OBJ meshes with scale factor `1.0` unless the manifest explicitly says otherwise.
- Preserve hierarchy and material slot names on first import.
- Treat MTL files as staging references, not final material authority.
- Replace generated import materials with shared Unity recipe materials before promotion.
- Reject OBJ packages that require external absolute texture paths.
- Recalculate normals only when the source normals are visibly broken; record the choice.

## Material Replacement

Preferred shared recipes:

- `AgedBrass`
- `BlackenedIron`
- `OilyLeather`
- `GrimyGlass`
- `SootWear`
- `AmberWeakPoint`
- `CyanAttackTell`
- `HazardTrim`
- `RouteGreen`
- `RouteAmber`
- `RouteRed`

Rules:

- Keep material count low: 1 to 4 preferred for most props, 6 maximum for hero/boss review.
- Use shared recipes plus later per-renderer variation instead of unique duplicate materials.
- Pink/magenta output is an automatic hold.
- Emission must support readability, not become a fake gameplay state.
- Route colors are reserved: green restored/safe/exit, amber attention/objective/charged, red locked/hostile/danger.

## Texture Rules

- Prefer PNG/TGA for authored staging textures and document final compression intent.
- Cap hero textures at 2048 unless specifically justified; use 1024 or 512 for common props, trims, sprites, and decals.
- Use consistent suffixes: `_Albedo`, `_Normal`, `_Mask`, `_Emission`, `_Opacity`, `_Sprite`.
- Avoid unique 2k textures for small repeated details like rivets, labels, soot cards, and gauge needles.
- Alpha textures require a written overdraw/fallback note.
- Texture names must match manifest references exactly.

## Audio Rules

- Use clear event-role names: `WeaponFire`, `WeaponEmpty`, `WeaponSwitch`, `Pickup`, `EnemyHit`, `EnemyShutdown`, `ObjectiveState`, `SecretFound`, `PauseConfirm`.
- Short one-shots are preferred for staged feedback. Loops require start/stop ownership notes.
- Provide expected compression intent later: high-priority combat cues can stay higher quality; ambient/polish cues should compress more aggressively.
- Audio must not mask enemy tells, low-health warnings, pickup confirmation, objective feedback, pause/settings confirmation, or boss HUD cues.
- Missing audio must fall back silently without breaking gameplay state.

## Sprite And Feedback Rules

- Sprites require pixels-per-unit, pivot, alpha mode, intended event, and low-quality fallback notes.
- Sprite sheets require named cells or frame ranges.
- Feedback sprites/VFX are presentation-only unless the main lane separately owns gameplay authority.
- No feedback asset may include collision, trigger, damage, pickup, route, objective, transition, save, boss-lock, or nav authority.
- Screen-space feedback must not hide HUD, crosshair, prompts, enemy tells, hazard warnings, boss HUD, final exit, or pause/settings controls.

## Collider Policy

Default policy is primitive-only:

- Weapons: boxes/capsules for receiver, barrel, grip, stock, tanks; pickup trigger is main-lane-owned only.
- Enemies: body capsule/box plus simple tool/weak-point guides only when combat authority owns them.
- Level modules: no collision for dress-only props; simplified boxes for large static blockers only when route ownership is explicit.
- Feedback polish: no colliders, no triggers.

Reject mesh colliders on decorative coils, rivets, pipes, gauges, labels, wires, soot cards, glows, steam cards, pickup glints, and preview props.

## LOD Policy

Every repeated or combat-relevant asset needs a reduction story:

- LOD0: full staged silhouette and readable gameplay markers.
- LOD1: remove micro detail such as rivets, gauge needles, tiny coils, wires, labels, soot cards, and duplicate trims.
- LOD2: collapse to broad class silhouette while preserving route state, weak point, pickup identity, hazard language, and major attack tells.

Do not promote assets whose only reduction plan is "use LOD0 everywhere."

## Light And Shadow Policy

- Decorative glow should use emissive materials, not realtime lights.
- Realtime lights are reserved for active high-value moments and must have low-quality fallbacks.
- Disable shadows on small decorative detail, particles, sprites, gauges, labels, rivets, wires, and pickup glints.
- Preserve shadows for large enemies, meaningful blockers, and major silhouettes only.
- Transparent steam/glass/sprites need overdraw review in representative scenes.

## Batching And Instancing Notes

- Reuse shared materials across packs wherever possible.
- Keep repeated trim/module meshes identical when they can be instanced.
- Avoid unique material instances for color/state variants that can be driven later by properties.
- Avoid splitting a single static prop into many tiny renderers unless sockets or animation require it.
- Level-module kits should favor repeatable snap pieces over bespoke single-use meshes.
- Weapon and enemy component separation is allowed where sockets, animation, or damage readability require it.

