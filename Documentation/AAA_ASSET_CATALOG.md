# AAA-Style Asset Catalog

This catalog lists the assets that would be needed to evolve the project from a greybox proof of concept into a high-quality first-person action game. It is intentionally broad. Items should be generated or implemented in passes, not all at once.

Status values:

- `planned`: needed later
- `prototype`: rough placeholder exists or can be generated quickly
- `in-progress`: actively being made
- `review`: ready for quality check
- `approved`: accepted for current milestone
- `deferred`: not needed for the current milestone

Priority values:

- `P0`: required for next playable milestone
- `P1`: required for vertical slice
- `P2`: important for production quality
- `P3`: polish or expansion

## 1. Environment Materials and Textures

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| MAT-001 | Greybox wall material | P0 | prototype | Existing flat grey wall material for layout work. |
| MAT-002 | Greybox floor material | P0 | prototype | Existing dark floor material for layout work. |
| MAT-003 | Industrial wall panel set | P1 | planned | Tileable metal panel textures with bolts, seams, rust, and grime. |
| MAT-004 | Industrial floor concrete | P1 | planned | Low/noise concrete floor with cracks, scuffs, and readable tone. |
| MAT-005 | Metal grate floor | P1 | planned | Dark grate or ribbed metal floor used for tech corridors. |
| MAT-006 | Gothic stone block wall | P1 | planned | Repeating stone blocks for chapel/reliquary areas. |
| MAT-007 | Ritual stone floor | P2 | planned | Worn stone floor with subtle carved lines and stains. |
| MAT-008 | Hazard trim | P1 | planned | Yellow/black or amber/black striping for doors and danger areas. |
| MAT-009 | Red key door material | P1 | planned | Door material with red indicator lights and lock icon area. |
| MAT-010 | Green exit material | P1 | planned | Glowing green slab/portal/switch material for exits. |
| MAT-011 | Ceiling dark panel | P2 | planned | Dark ceiling tile that frames the play space without stealing attention. |
| MAT-012 | Broken metal edge trim | P2 | planned | Modular damaged trim for room borders and broken tech walls. |
| MAT-013 | Wet grime/decal set | P2 | planned | Decals for stains, leaks, soot, and scorch marks. |
| MAT-014 | Occult energy emissive | P2 | planned | Red/green/amber emissive strips, runes, vents, and portals. |
| MAT-015 | Blood/scorch impact decals | P2 | planned | Small decals for combat impact readability. |

## 2. Modular Level Geometry

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| GEO-001 | Basic wall module | P0 | prototype | Existing cube walls generated into Level01. |
| GEO-002 | Basic floor module | P0 | prototype | Existing large floor primitive. |
| GEO-003 | Straight corridor kit | P1 | planned | Modular corridor pieces with wall, floor, ceiling variants. |
| GEO-004 | Corner corridor kit | P1 | planned | 90-degree corridor segments for maze flow. |
| GEO-005 | Door frame kit | P1 | planned | Frames for standard and locked doors. |
| GEO-006 | Stair/ramp module | P2 | planned | Optional vertical connection pieces. Use carefully to preserve classic flow. |
| GEO-007 | Combat arena shell kit | P1 | planned | Larger room modules with columns, cover, and loop paths. |
| GEO-008 | Chapel arch set | P2 | planned | Gothic arches for final-room identity. |
| GEO-009 | Industrial pillar set | P2 | planned | Cover and visual rhythm props. |
| GEO-010 | Secret wall module | P2 | planned | Visually plausible wall segment that can open or slide. |
| GEO-011 | Window/grate insert | P2 | planned | Non-passable visibility inserts for spatial depth. |
| GEO-012 | Ceiling vent/beam kit | P3 | planned | Overhead detail modules. |

## 3. Props and World Objects

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| PROP-001 | Red locked door | P0 | prototype | Existing sliding red door placeholder. |
| PROP-002 | Green exit marker | P0 | prototype | Existing green exit trigger placeholder. |
| PROP-003 | Health pickup | P0 | prototype | Existing red cube pickup placeholder. |
| PROP-004 | Ammo pickup | P0 | prototype | Existing blue cube pickup placeholder. |
| PROP-005 | Key pickup | P0 | prototype | Existing yellow cube pickup placeholder. |
| PROP-006 | Styled health pack | P1 | planned | Red/white med box, vial, or injector. |
| PROP-007 | Styled ammo box | P1 | planned | Compact box with bullet or energy cell markings. |
| PROP-008 | Red keycard/skull-key | P1 | planned | Distinct objective pickup linked visually to red door. |
| PROP-009 | Door switch panel | P1 | planned | Wall-mounted panel for doors, lifts, and exits. |
| PROP-010 | Exit portal/switch | P1 | planned | Final objective object with green illumination. |
| PROP-011 | Explosive barrel | P2 | planned | Combat prop with risk/reward behavior. |
| PROP-012 | Broken machinery | P2 | planned | Environmental dressing and cover. |
| PROP-013 | Hanging cables/pipes | P3 | planned | Background dressing. |
| PROP-014 | Altar/machine hybrid | P2 | planned | Landmark prop for chapel spaces. |
| PROP-015 | Crates and supply boxes | P2 | planned | Cover, resource markers, and dressing. |
| PROP-016 | Light fixtures | P1 | planned | Industrial and gothic lighting assets. |

## 4. Weapons

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| WPN-001 | Blocky pistol placeholder | P0 | prototype | Existing camera-mounted primitive weapon. |
| WPN-002 | Iron Pistol final placeholder | P1 | planned | Centered sprite or simple model with chunky silhouette. |
| WPN-003 | Shotgun | P1 | planned | Close-range burst weapon with strong recoil and reload beat. |
| WPN-004 | Machine pistol/rifle | P2 | planned | Sustained fire weapon for mid-range pressure. |
| WPN-005 | Heavy energy weapon | P2 | planned | High-cost weapon for elite enemies or crowd clearing. |
| WPN-006 | Melee/quick strike | P3 | planned | Emergency low-damage fallback, optional. |
| WPN-007 | Weapon pickup models | P2 | planned | World pickup visuals for each weapon. |
| WPN-008 | Ammo family models | P1 | planned | Bullets, shells, cells, rockets or equivalent resource types. |

## 5. Weapon Animations

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| ANIM-WPN-001 | Pistol idle bob | P1 | prototype | Existing simple recoil return can be expanded. |
| ANIM-WPN-002 | Pistol fire | P1 | planned | Quick recoil and muzzle movement. |
| ANIM-WPN-003 | Pistol reload/check | P2 | planned | Optional if ammo system supports reloads. |
| ANIM-WPN-004 | Shotgun fire | P2 | planned | Heavy kick and pump/cycle motion. |
| ANIM-WPN-005 | Weapon switch in/out | P2 | planned | Clear transition animation for weapon inventory. |
| ANIM-WPN-006 | Empty/fire fail | P2 | planned | Dry trigger or lowered weapon feedback. |

## 6. Enemies and Monsters

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| ENEMY-001 | Primitive melee enemy | P0 | prototype | Existing capsule/body with eye markers. |
| ENEMY-002 | Ash Imp | P1 | planned | Basic melee pressure enemy, red/orange body, readable swipe. |
| ENEMY-003 | Ranged cult soldier | P1 | planned | Mid-range shooter that teaches projectile dodging or line-of-fire breaks. |
| ENEMY-004 | Heavy bruiser | P2 | planned | Slow durable enemy that controls space. |
| ENEMY-005 | Fast leaper/charger | P2 | planned | Mobility enemy used sparingly to disrupt comfort. |
| ENEMY-006 | Floating caster | P2 | planned | Airborne or elevated threat with slow projectile patterns. |
| ENEMY-007 | Mini-boss guardian | P3 | planned | Larger encounter enemy for level finales. |
| ENEMY-008 | Boss prototype | P3 | deferred | Multi-phase set piece enemy for later content. |

## 7. Enemy Animations

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| ANIM-EN-001 | Idle loop | P1 | planned | Subtle breathing/stance for each enemy type. |
| ANIM-EN-002 | Walk/chase | P1 | planned | Clear movement state. |
| ANIM-EN-003 | Attack tell | P1 | planned | Anticipation frame or windup before damage. |
| ANIM-EN-004 | Attack release | P1 | planned | Actual swipe/projectile/fire frame. |
| ANIM-EN-005 | Hit reaction | P1 | planned | Short flinch or color flash. |
| ANIM-EN-006 | Stagger | P2 | planned | Heavier reaction for high-impact weapons. |
| ANIM-EN-007 | Death | P1 | planned | Clear enemy removal or corpse transition. |
| ANIM-EN-008 | Corpse/ash fade | P2 | planned | Cleanup animation or persistent body variant. |

## 8. Player, HUD, and UI

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| UI-001 | Text HUD | P0 | prototype | Existing health/ammo/key HUD. |
| UI-002 | Crosshair | P0 | prototype | Existing text crosshair. |
| UI-003 | Damage flash | P0 | prototype | Existing red overlay flash. |
| UI-004 | Doom-like HUD frame | P1 | planned | Compact bottom HUD with readable health/ammo/key/armor. |
| UI-005 | Weapon icon set | P2 | planned | Icons for weapon inventory. |
| UI-006 | Pickup icon set | P2 | planned | Icons for health, ammo, key, armor. |
| UI-007 | Main menu | P2 | planned | Play, settings, quit. |
| UI-008 | Pause menu | P1 | prototype | Existing text pause can be replaced with panel. |
| UI-009 | Death screen | P1 | prototype | Existing text screen can be styled. |
| UI-010 | Win/level complete screen | P1 | prototype | Existing text screen can be styled. |
| UI-011 | Settings menu | P2 | planned | Mouse sensitivity, resolution, fullscreen, volume. |
| UI-012 | Accessibility options | P2 | planned | Crosshair options, flash intensity, subtitles, color readability. |

## 9. VFX

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| VFX-001 | Muzzle flash | P0 | prototype | Existing primitive flash. |
| VFX-002 | Hit marker sphere | P0 | prototype | Existing temporary yellow sphere. |
| VFX-003 | Wall impact sparks | P1 | planned | Short sparks for metal/stone impacts. |
| VFX-004 | Enemy hit effect | P1 | planned | Blood/spark puff, depending enemy type. |
| VFX-005 | Enemy death effect | P1 | planned | Collapse, dissolve, or burst. |
| VFX-006 | Pickup collect flash | P1 | planned | Short glow or particles. |
| VFX-007 | Door open effect | P2 | planned | Dust, sparks, or light change. |
| VFX-008 | Exit portal effect | P1 | planned | Green distortion/glow. |
| VFX-009 | Projectile effects | P2 | planned | Tracer, fireball, plasma bolt, or equivalent. |
| VFX-010 | Environmental ambience | P3 | planned | Steam, embers, dust, flicker. |

## 10. Audio

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| AUD-001 | Pistol fire | P1 | planned | Short, punchy weapon sound. |
| AUD-002 | Empty weapon click | P1 | planned | Dry mechanical feedback. |
| AUD-003 | Enemy hurt | P1 | planned | Short impact vocal or synthetic hit. |
| AUD-004 | Enemy death | P1 | planned | Clear kill confirmation. |
| AUD-005 | Player hurt | P1 | planned | Quick low thud/breath feedback. |
| AUD-006 | Health pickup | P1 | planned | Bright positive blip. |
| AUD-007 | Ammo pickup | P1 | planned | Mechanical pickup sound. |
| AUD-008 | Key pickup | P1 | planned | More important objective cue. |
| AUD-009 | Door open | P1 | planned | Metal slide/clank. |
| AUD-010 | Locked door fail | P1 | planned | Denied buzz/clank. |
| AUD-011 | Exit/win cue | P1 | planned | Short success tone. |
| AUD-012 | Ambient loop | P2 | planned | Low industrial/ritual ambience. |
| AUD-013 | Combat music loop | P3 | planned | Optional high-energy combat track. |
| AUD-014 | UI sounds | P2 | planned | Pause, menu select, restart. |

## 11. Levels

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| LVL-001 | Level01 greybox | P0 | prototype | Existing generated blockout. |
| LVL-002 | Level01 combat slice | P1 | planned | Tuned version of current level with improved encounter flow. |
| LVL-003 | Level01 art pass | P1 | planned | First styled version using industrial/gothic kit. |
| LVL-004 | Level02 concept | P2 | planned | Second level proving content scalability. |
| LVL-005 | Tutorial/intake space | P2 | planned | Optional clearer onboarding area. |
| LVL-006 | Final arena prototype | P2 | planned | Bigger fight with multiple enemy roles. |
| LVL-007 | Secret-room set | P2 | planned | Optional exploration rewards. |

## 12. Animation and Cinematic Needs

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| CIN-001 | Intro camera moment | P3 | deferred | Very short in-engine opening shot, optional. |
| CIN-002 | Door reveal moment | P3 | planned | Small lighting/audio cue when final path opens. |
| CIN-003 | Exit activation moment | P2 | planned | Win feedback beyond text. |
| CIN-004 | Boss intro moment | P3 | deferred | Later set-piece need. |

## 13. Tools and Pipeline Assets

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| TOOL-001 | V0 scene builder | P0 | prototype | Existing editor utility that generates Level01. |
| TOOL-002 | Smoke test method | P0 | prototype | Existing editor/runtime smoke checks. |
| TOOL-003 | Asset import presets | P1 | planned | Standard texture, model, audio, and sprite import settings. |
| TOOL-004 | Material generator | P2 | planned | Script/tool for generating simple tileable prototype textures. |
| TOOL-005 | Level validation tool | P2 | planned | Checks missing colliders, unreachable exits, spawn issues. |
| TOOL-006 | Build automation | P1 | prototype | Existing editor method builds Windows player. |
| TOOL-007 | Playtest report template | P1 | planned | Structured manual playtest form. |
| TOOL-008 | Performance capture checklist | P2 | planned | Repeatable profiling procedure. |

## 14. Asset Generation Method

For each generated asset:

1. Add the item to this catalog or update its row.
2. Create a concise generation brief.
3. Generate or build the asset in a scratch/import folder.
4. Import into Unity using the correct folder and settings.
5. Place it in a test scene or target scene.
6. Verify it in-game.
7. Update status, notes, and dependencies.

Do not generate large batches blindly. Generate assets in milestone-sized sets tied to gameplay needs.
