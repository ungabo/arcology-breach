# V0.1.48 Risk And Authority Matrix

Purpose: define what must remain visual-only until a future owned slice grants gameplay, collision, VFX, lighting, animation, audio, or weapon authority.

## Current Authority Boundary

| Boundary | Current status | Promotion rule |
| --- | --- | --- |
| Package import | 15 sidecar packages imported through `Packages/manifest.json` | Import alone grants loadability only |
| Showcase placement | `V0SceneBuilder` places sidecar instances under `Sidecar Quarantine Showcase - LevelXX` | Showcase placement grants visual-review authority only |
| Physics | Showcase instances are passed through `StripSidecarPresentationPhysics` | No sidecar collider or rigidbody should be treated as authoritative |
| Gameplay logic | v0.1.45 notes deny gameplay authority, AI, pickups, damage, interactable state, and autonomous audio | Existing game systems remain the source of truth |
| Build validation | v0.1.45 passed QA with sidecars quarantined | Any promotion requires new QA evidence |

## Risk Matrix

| Promotion area | Must remain visual-only until | Main risks if promoted early | Required evidence before graduation |
| --- | --- | --- | --- |
| Collision setpieces | Level design owns collider proxies, route metrics, camera clearance, and stuck-point tests | Player snagging, unreachable pickups, broken auto-playthrough, AI path breakage, hidden combat imbalance | Collider prefab review, level route audit, runtime movement tests, auto-playthrough pass |
| Doors and gates | Gameplay objectives own door state, key requirements, animation timing, blockers, and save-state assumptions | Softlocks, misleading locked/unlocked language, broken level transitions | Existing `LockedDoor` or transition logic mapped to visual shell, route-blocked feedback pass |
| Objective props | Objective systems own interaction radius, prompt text, state changes, inventory hooks, and completion logic | Interactable-looking props that do nothing, duplicated objective state, unclear mission route | Interaction checklist, objective copy review, success/failure event tests |
| Pickups and weapon props | Inventory/weapon systems own pickup kind, amount, collect radius, bob/spin behavior, and respawn policy | Ammo economy drift, pickups hidden by scale, duplicate weapon grants, collision mismatch | Pickup component mapping, balance run, readability screenshots |
| VFX sockets | VFX/feedback systems own event routing, lifetime, pooling, and performance budgets | Looping effects, particle overdraw, hidden enemies, incorrect damage implication | Socket map, performance profile, event trigger tests |
| Lighting | Lighting owner defines intensity, range, baked/realtime policy, color script, and accessibility limits | Washed-out HUD/world labels, glare, frame spikes, false route affordances | Lighting comparison captures, readability test, performance pass |
| Enemy visuals | Combat owner maps visual shell to archetype, hit volumes, hurt boxes, attack tells, and animation states | Hitbox lies, unreadable attacks, scale mismatch, unfair cover interactions | Archetype mapping, hit volume overlay review, combat route pass |
| Enemy animation proxies | Animation/combat owner replaces or controls proxy state changes | Static poses mistaken for production animation, attack timing desync, repeated pose popping | Animation state plan, tell timing review, enemy combat tests |
| Weapon viewmodels | Weapon owner controls muzzle origin, recoil, reload timing, hand pose, camera FOV, and occlusion | Shots from wrong point, clipping, motion sickness, reload mismatches | Muzzle socket tests, viewmodel screenshots, weapon runtime tests |
| Materials | Art direction owns shared palette, shader settings, emission, transparency, and build size | Inconsistent readability, too many material variants, shader fallback issues | Material palette review, build size check, lighting screenshots |
| Audio feedback | Audio/UX owner routes clips through mixers, settings, volume policy, cooldowns, and spatialization | Repeated spam, inaccessible feedback, pause/menu leaks, mix clipping | Mixer routing, settings tests, runtime audio mix test |

## Package Authority Notes

| Package | Keep visual-only until | Safe first authority to grant |
| --- | --- | --- |
| `feedback-fx-audio` | Audio mixer/event routing is owned | Event-bound feedback cues |
| `corridor-kit-set02` | Route and collider ownership is assigned | Proxy collision for a single test route |
| `enemy-animation-proxy-set01` | Animation/combat owns state timing | Pose reference library |
| `encounter-enemy-set02` | Combat owns archetype tells | Readability shell for existing enemies |
| `level-atmosphere-set03` | VFX/lighting/headroom ownership is assigned | Non-colliding ambient sockets |
| `level-dressing-set01` | Level dressing and performance budgets are assigned | Non-colliding dressing pass |
| `materials-set01` | Art direction approves material consolidation | Shared promoted material palette |
| `mechanical-enemies` | Combat owns visual-to-hitbox mapping | Coarse enemy shell swap |
| `mechanical-enemy-visual-set01` | Combat/animation owns variant mapping | Enemy visual variant library |
| `objective-props-set02` | Objective systems own interaction state | Existing objective visual shells |
| `steam-vfx-set02` | VFX/feedback owns event sockets | Event-triggered steam, impact, and muzzle effects |
| `steampunk-weapons` | Weapon systems own sockets and pickups | Legacy pressure pistol prop references |
| `steamworks-level-kit` | Level design owns collision and cover metrics | Route shell and cover proxy test |
| `weapon-viewmodel-set03` | Weapon/viewmodel systems own camera-space behavior | Pressure pistol/scattergun visual replacement |
| `weapon-props-set02` | Weapon/pickup systems own inventory semantics | Ammo, rack, and pickup dressing |

## Hard Stops

- Do not let sidecar prefabs own damage, health, enemy AI, objective state, inventory, transitions, or route gating by default.
- Do not use sidecar doors, locks, levers, lifts, or secret caches as authoritative interactables without explicit component mapping.
- Do not enable colliders from visual prefabs wholesale; use reviewed proxy colliders.
- Do not attach looping audio or VFX directly in scenes without event routing and settings control.
- Do not promote enemy pose proxies as finished animation.
- Do not ship viewmodel assemblies without muzzle-origin, recoil, reload, and clipping validation.
