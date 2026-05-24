# V0.1.48 Promotion Matrix

Purpose: group every currently imported sidecar package by the safest future gameplay-promotion path. These are candidates only; all assets remain visual-only until a later slice owns implementation, QA, rollback, and authority.

## Imported Sidecar Inventory

| Package | Prefabs | Materials | Meshes | Audio | Anim clips | Primary future path |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| `com.brassworks.sidecar.feedback-fx-audio` | 15 | 12 | 3 | 15 | 0 | Audio/feedback |
| `com.brassworks.sidecar.corridor-kit-set02` | 32 | 18 | 6 | 0 | 0 | Environment collision setpieces |
| `com.brassworks.sidecar.enemy-animation-proxy-set01` | 16 | 15 | 8 | 0 | 4 | Enemy visuals/animation |
| `com.brassworks.sidecar.encounter-enemy-set02` | 16 | 16 | 12 | 0 | 0 | Enemy visuals/animation |
| `com.brassworks.sidecar.level-atmosphere-set03` | 28 | 16 | 8 | 0 | 0 | VFX sockets, lighting, atmosphere |
| `com.brassworks.sidecar.level-dressing-set01` | 30 | 16 | 5 | 0 | 0 | Environment dressing and sockets |
| `com.brassworks.sidecar.materials-set01` | 0 | 16 | 0 | 0 | 0 | Materials |
| `com.brassworks.sidecar.mechanical-enemies` | 5 | 10 | 7 | 0 | 0 | Enemy visuals |
| `com.brassworks.sidecar.mechanical-enemy-visual-set01` | 13 | 15 | 8 | 0 | 0 | Enemy visuals |
| `com.brassworks.sidecar.objective-props-set02` | 24 | 17 | 11 | 0 | 0 | Objective props |
| `com.brassworks.sidecar.steam-vfx-set02` | 20 | 16 | 8 | 0 | 0 | VFX sockets |
| `com.brassworks.sidecar.steampunk-weapons` | 7 | 7 | 0 | 0 | 0 | Weapon/viewmodel pieces |
| `com.brassworks.sidecar.steamworks-level-kit` | 24 | 16 | 4 | 0 | 0 | Environment collision setpieces |
| `com.brassworks.sidecar.weapon-viewmodel-set03` | 20 | 14 | 7 | 0 | 0 | Weapon/viewmodel pieces |
| `com.brassworks.sidecar.weapon-props-set02` | 16 | 12 | 4 | 0 | 0 | Weapon props and pickups |

## Environment Collision Setpieces

| Candidate sidecars | Representative families | Safe graduation target | Required owner before promotion |
| --- | --- | --- | --- |
| `corridor-kit-set02` | Corridor straights, corners, junctions, door frames, floor panels, wall panels, ceiling modules | Collision-authoritative modular route pieces after proxy collider pass, nav/route validation, camera clearance, and stuck-point testing | Level design plus gameplay collision |
| `steamworks-level-kit` | Corridors, arched doors, vault doors, catwalk floors, floor grates, boiler alcoves, cover crates, pipe railings | Real route shell, cover silhouettes, floor traversal, door-frame occluders, and non-interactive machinery | Level design plus combat readability |
| `level-dressing-set01` | Trim plates, drain channels, service panels, valve clusters, caged lamps, pressure tanks, vents, warning placards | Non-authoritative dressing first; selected floor panels, tanks, and cover-like props can later receive simple colliders | Level dressing plus performance |
| `level-atmosphere-set03` | overhead pipe canopies, drain covers, hanging chains, pulley silhouettes, wall grime panels | Mostly non-colliding atmosphere; selected drain covers and overhead canopies can become collision blockers only after headroom tests | Environment art plus traversal QA |

Recommended first graduation: promote a batch of corridor shells and cover crates into one test route, not individual decorative pieces. This creates player-facing progress while avoiding one-object churn.

## Objective Props

| Candidate sidecars | Representative families | Safe graduation target | Required owner before promotion |
| --- | --- | --- | --- |
| `objective-props-set02` | Keyed locks, valve panels, lift call stations, pressure regulators, secret caches, bridge actuators, governor override switches | Interactable skin layer for existing key, door, lift, secret, bridge, and boss objective systems | Gameplay objectives plus UI/audio feedback |
| `weapon-props-set02` | Gear key housing, ammo cartridge clusters, pressure cells, wall racks, ammo cabinets | Pickup shell, readable weapon-rack set dressing, secret reward visuals | Inventory/pickup systems plus level design |
| `steampunk-weapons` | Pressure cartridge, ammo cabinet shell, wall weapon display | Pickup display and arsenal dressing once scale and collision are owned elsewhere | Weapon systems plus pickup QA |

Recommended first graduation: replace greybox objective markers with one cohesive `key -> pressure gate -> lift call -> secret cache` visual language pass. Keep logic on existing components.

## VFX Sockets

| Candidate sidecars | Representative families | Safe graduation target | Required owner before promotion |
| --- | --- | --- | --- |
| `steam-vfx-set02` | Steam vents, wall jets, pressure leaks, muzzle flashes, ricochets, furnace blasts, boss phase effects, valve release puffs | Socketed effects triggered by existing hazards, weapons, impacts, doors, and boss phase events | Gameplay feedback plus performance |
| `level-atmosphere-set03` | Wall leakers, corner bleeds, dense ambience combos, warning gauges | Ambient socket anchors paired with existing scene timing and visibility rules | Level art plus VFX |
| `feedback-fx-audio` | Event cue prefabs for weapon fire, enemy hit/death, objectives, route blocked, pickup, secret, pause, settings | Event-bound feedback prefabs, never autonomous scene audio | Audio/feedback plus settings |

Recommended first graduation: create a socket map per existing event type before placing more visuals. The highest-impact set is weapon fire, enemy hit/death, objective completion, route blocked, and steam hazard.

## Enemy Visuals And Animation

| Candidate sidecars | Representative families | Safe graduation target | Required owner before promotion |
| --- | --- | --- | --- |
| `mechanical-enemies` | Saw Scrapper, Rivet Lancer, Bulwark Furnace, Warden Sentinel, Overseer bust | Coarse enemy silhouette swaps for existing controller archetypes | Combat systems plus enemy readability |
| `mechanical-enemy-visual-set01` | Scrapper variants, lancer variants, bulwark variants, bellows supports, warden overseers | Variant visual libraries mapped to current archetypes, with separate hit volumes and damage authority | Combat systems plus animation |
| `encounter-enemy-set02` | Ashcan reclaimer tells, pressure spindle tells, gatehammer tells, governor warden tells | Readability tell meshes and pose states for attacks, stagger, shield, enrage, and command behaviors | Combat design plus animation |
| `enemy-animation-proxy-set01` | Scrapper, lancer, bulwark, and warden pose proxies plus 4 pose-only clips | Animation planning references; later replace with rigged clips or controlled visual state prefabs | Animation plus AI/combat |

Recommended first graduation: batch by archetype across all five levels. Do Scrapper, Lancer, Bulwark, and Warden visual shells with shared hitbox rules instead of fully finishing one enemy at a time.

## Weapon And Viewmodel Pieces

| Candidate sidecars | Representative families | Safe graduation target | Required owner before promotion |
| --- | --- | --- | --- |
| `weapon-viewmodel-set03` | Pressure pistol, scattergun, bolt thrower, glove silhouettes, ammo cells, gauge clusters, muzzle modules | Viewmodel visual replacement, hand alignment reference, muzzle socket placement, reload pose planning | Weapon/viewmodel systems plus animation |
| `weapon-props-set02` | Pistol frames/barrels/coils, scattergun bodies, ammo cabinets, cartridges, repair tools, racks | World pickups, weapon rack dressing, ammo pickup visuals, prop scale references | Weapon systems plus pickup QA |
| `steampunk-weapons` | Pressure pistol core, coils, gauge assembly, leather grip, pressure cartridge | Legacy core visual references for current pressure pistol and ammo vocabulary | Weapon systems |

Recommended first graduation: promote pressure pistol and scattergun visuals together with consistent muzzle, casing, ammo, and audio sockets. Defer bolt thrower gameplay until an owned weapon slice exists.

## Materials

| Candidate sidecars | Representative families | Safe graduation target | Required owner before promotion |
| --- | --- | --- | --- |
| `materials-set01` | Aged brass, oily iron, riveted wall plate, gauge glass, leather, wood, hazard paint, soot grime, wet stone, copper | Shared material palette for promoted sidecar visuals and greybox replacement | Art direction plus performance |
| Package-local materials across all sidecars | Brass, copper, steel, glass, glow, enamel, steam, hazard, grime, leather, wood | Material consolidation after promoted families are selected; avoid duplicate near-identical materials | Art direction plus build size |

Recommended first graduation: establish a small approved material palette before broad prefab promotion. This reduces shader/material noise and improves readability consistency.

## Audio And Feedback

| Candidate sidecars | Representative families | Safe graduation target | Required owner before promotion |
| --- | --- | --- | --- |
| `feedback-fx-audio` | Weapon fired/empty/impact/switched, enemy hit/death, objective updated/completed, pickup, route blocked, secret, pause, settings, boss phase | Event-routed audio and micro-VFX feedback bound to existing controllers and player settings | Audio, UX, gameplay feedback |
| `steam-vfx-set02` | Muzzle flash, ricochet, pressure pop, valve turn, boss phase | Visual counterpart for feedback events | VFX plus feedback |

Recommended first graduation: prioritize event coverage that players feel every minute: weapon fire, impact, empty, enemy hit/death, pickup, objective complete, and route blocked.
