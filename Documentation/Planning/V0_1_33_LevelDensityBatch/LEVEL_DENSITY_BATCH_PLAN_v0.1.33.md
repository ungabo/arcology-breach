# Brassworks Breach - v0.1.33 Level Density Batch Plan

Created: `2026-05-24 11:59 -04:00`

## Purpose

This packet gives the parallel level crew an ambitious one-compile density plan for `Level01` through `Level05`. It is not a sequential asset list. The target is a coordinated placement pass where the same route-dressing families stack around objective reads, route thresholds, combat landmarks, hazards, secrets, and exits so the next compile feels visibly more authored across the full five-level run.

Write scope for this side packet is only:

`D:\__MY APPS\Unity Doom\Documentation\Planning\V0_1_33_LevelDensityBatch\`

The main lane owns Unity code, scene builder edits, generated scenes, validators, tests, shared status docs, release docs, commits, and pushes. This packet does not edit any of those files.

## Inputs Reviewed

- `Documentation/LEVEL_DESIGN_AND_MAPS.md`
- `Documentation/PARALLEL_LEVEL_PRODUCTION_MAPS.md`
- `Documentation/Planning/V0_1_33_BatchPlan/THRESHOLD_ROUTE_DRESSING_BATCH_PLAN_v0.1.33.md`
- `Documentation/Planning/V0_1_33_BatchPlan/THRESHOLD_ROUTE_DRESSING_BATCH_STATUS.json`
- `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.32.md`
- `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.32.md`
- `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.32.md`
- `Documentation/Planning/V0_1_29_RouteTriage/ROUTE_TRIAGE_PLAN_v0.1.29.md`

Current deterministic baseline: `v0.1.32` reports no route-blocking scene composition issues. The same QA packet still calls for human checks on movement comfort, route readability, hazards, boss clarity, secrets, and audio mix. This batch should improve the human first-read without touching gameplay authority.

## One-Compile Visible Leap

The compile should not feel like six isolated props appeared. It should feel like the brassworks became denser, more legible, and more physically connected.

The visible leap comes from four layered moves:

1. Every level gets at least one hero threshold stack that combines threshold framing, pipe arteries, route-state signals, safe-lane floor language, grime, and local story debris.
2. Every critical objective or lock gets an "attention constellation": amber approach cues, red locked/unsafe state, green resolved/exit cue, pressure pipes that visually explain what changed, and floor/wall trim that makes the objective space read from normal play distance.
3. Every combat or hazard arena gets readable edge dressing that clarifies safe movement and enemy silhouettes without adding new blockers or authority.
4. Secrets get a consistent cross-level clue grammar so optional spaces feel authored but do not compete with the main route.

Density target for one compile:

| Density band | Target per level | Gameplay purpose |
| --- | ---: | --- |
| Hero anchor stack | `1-2` | Make the main lock or exit visibly upgraded from first-person height. |
| Objective/lock clusters | `2-4` | Make key, valve, regulator, Warden lock, and transition state easier to parse. |
| Route breadcrumb runs | `3-6` | Tie spawn, branch, return, and exit together every `6m` to `10m` of travel. |
| Combat/hazard edge clusters | `2-4` | Clarify cover, safe lanes, heat/steam boundaries, and heavy enemy silhouette reads. |
| Secret micro-clues | `1-3` | Make optional discovery language consistent without over-signaling. |

If scope must be cut, preserve the stacks at the main route thresholds and objective locks first. That is where the compile will look most improved.

## Route-Dressing Families

Use these as placement families, not as implementation order. The best result is overlapping families in each route beat.

| ID | Family | Exact gameplay intent | Visible density contribution | Primary route-safety risks | Targeted validation gates |
| --- | --- | --- | --- | --- | --- |
| LDF-001 | Threshold Landmark Frames | Make gates, lifts, hoists, and boss exits read as major route state machines before interaction. | Big silhouette frames, piston braces, sill plates, cable drums, side-wall mass. | Narrowing transition apertures, hiding lock signals, crowding spawn/exit comfort space. | No colliders or authority; center aperture remains visually clear; first-person screenshot confirms exit/lock is readable; no dressing root inside transition volumes. |
| LDF-002 | Pressure Pipe Arteries | Visually explain that pressure moves from objective to lock to exit. | Copper/blackened pipe runs, clamp collars, feed/return labels, short overhead bundles. | Pipes crossing walkable or projectile lanes, visual clutter over enemy tells, false route direction. | Pipe runs wall/ceiling-bound; no walkable lane below `2.2m` width is reduced; color/state matches objective progression. |
| LDF-003 | Objective Machine Constellations | Make keys, valves, regulators, weapon displays, and Warden locks feel like mechanical systems. | Gauge banks, control plates, nameplates, wheel frames, plinth crowns, status bulbs. | Hiding pickups/prompts, implying extra interactions, masking valve or pickup state. | Prompt/pickup remains visible from approach; no extra interactables; metadata marks visual-only support. |
| LDF-004 | Route-State Signal Chains | Teach amber objective, red locked/unsafe, green restored/exit language repeatedly across the run. | Amber enamel plates, red lock lamps, green exit lamps, small repeating signal beads. | Conflicting with existing red/green state, making secrets look mandatory, over-lighting combat arenas. | Color audit per level; amber never marks final exit; green reserved for restored route/exit; no new dynamic lights unless main lane accepts them. |
| LDF-005 | Safe-Lane Floor Grammar | Show where the player can stand, turn, retreat, and board lifts without reading UI text. | Flush brass strips, blackened kick plates, heat-safe edge bands, worn traffic paths. | Snag geometry, fake ramps/cover, misleading hazard-safe zones, VR comfort loss. | Trim height target `0.03m` max; no colliders; hazard-edge markings match actual damage zones; lift boarding areas preserve `4m` turning space where planned. |
| LDF-006 | Secret Clue Motifs | Make optional caches discoverable through a consistent language across Level01, Level02, Level04, and future Level05. | Chalk marks, missing rivets, cold pipe labels, coal dust trails, clerk misfile labels. | Pulling players off critical route during combat, over-signaling secrets, blocking cache entrances. | Secret clue appears outside active combat/hazard lane; main route signage remains stronger than secret clue; secret reward remains visible after entry. |
| LDF-007 | Combat Read Cover Dressing | Make cover and arena edges look intentional while preserving enemy movement and player escape routes. | Skins on existing benches, pipe elbows, regulator pylons, tool racks, broken cradle frames. | Creating new collision, changing cover strength, hiding Scrapper/Lancer/Bulwark/Warden silhouettes. | Visual-only unless attached to existing collision; enemy approach and projectile lanes remain readable; boss pylons do not trivialize line-of-sight. |
| LDF-008 | Hazard Tell Amplifiers | Make steam and furnace hazards readable before damage and after shutdown. | Warning collars, heat shields, damper plates, safe-state relief lamps, steam baffle frames. | Masking active hazard VFX, implying inactive hazards are safe too early, crowding dodge lanes. | Warning cue visible before hazard; shutdown/vented visual state differs from active; no dressing placed inside damage ownership volumes. |
| LDF-009 | Skyline Machinery Mass | Add large background mechanical identity without changing route geometry. | High wall gantries, rotating silhouettes, governor drums, hammer presses, distant pipe racks. | Performance cost, visual noise behind enemies, accidental route affordances. | Static or cheap motion only; collision disabled; visible from route but not mistaken for traversal; quality downshift can simplify detail. |
| LDF-010 | Local Crew Evidence | Add story density that makes each space feel worked-in and abandoned under pressure. | Tool trays, work orders, failed repair plates, soot trails, plaque staging, maintenance clutter. | Hiding pickups, cluttering floors, drawing attention away from objective text. | Floor clutter outside main lanes; pickups and plaques remain unobstructed; no small objects in first combat retreat paths. |

## Level Placement Matrix

### Level01 - Brassworks Intake

Goal: the first level should immediately read as a functioning service entrance, then teach the player to trust amber key cues, red locked pressure, green restored lift, and secret clue language.

| Zone | Coordinates / area | Density families | Gameplay intent | Route-safety risk | Targeted gate |
| --- | --- | --- | --- | --- | --- |
| Soot Service Entry to Maintenance Throat | `x -6..6, z -24..-16` into `x -5..5, z -16..-8` | LDF-002, LDF-004, LDF-005, LDF-010 | Establish forward pressure-pipe pull and low-risk amber breadcrumbing before the first fight. | Over-cluttering the first movement lane or making the route look narrower than it is. | Player can face north from spawn and identify the main path within `3s`; no trim/cable narrows the throat. |
| Repair Bay combat read | `x -18..6, z -18..-6`; first Scrapper near `x -10, z -11` | LDF-007, LDF-010, LDF-002 | Make benches and boilers look like real work stations while keeping fallback routes obvious. | Visual clutter around the first Scrapper or retreat path. | First combat should still avoid spawn-behind reads; at least two side routes around bench dressing remain visible. |
| Gate Overlook and Pressure Gate hero stack | Gate Overlook `x -16..4, z -4..6`; Pressure Gate `x -4..8, z 3..7` | LDF-001, LDF-002, LDF-003, LDF-004, LDF-005 | Make the locked route impossible to miss before the key, then make green restored pressure feel physical after unlock. | Piston/pipe mass narrowing the aperture or confusing red/green lock state. | Locked gate screenshot from overlook shows red state, gate aperture, and amber key branch; open-state screenshot shows green route/lift continuation. |
| Gear-Key Workshop objective branch | `x 8..24, z -8..6`; dormant Scrapper near `x 18, z -1` | LDF-003, LDF-004, LDF-010, LDF-006 | Crown the key plinth as a purpose-built machine part and seed optional clue language nearby. | Key plinth dressing hiding the pickup burst or making side clutter look interactive. | Gear key remains visible from approach and acquisition VFX remains unobstructed. |
| Secret Intake Cache clue and reward | Secret bounds `x 16..28, z -22..-14`; target point `x 21, z -18` | LDF-006, LDF-010, LDF-002 | Introduce the secret grammar through chalk/missing rivets/hairline green steam without competing with the key route. | Secret clue too strong during first-run route; pipe rack blocking standing-height access. | Secret is discoverable but not mandatory-looking; reward pickup line is clear once inside. |
| Furnace Control Room and Service Lift | Control `x -8..14, z 8..18`; Lift `x -3..7, z 18..24` | LDF-001, LDF-002, LDF-004, LDF-005, LDF-010 | Make the exit-side fight and service lift feel like the first successful pressure restoration. | Lift comfort area or transition trigger visually crowded. | Preserve `4m` lift turning space; green lift language visible after gate. |

### Level02 - Pipeworks Annex

Goal: turn the route into a pipe logic puzzle the player can read by sight: locked Boilerheart lift, west routing valve, east ranged threat, then green pressure return.

| Zone | Coordinates / area | Density families | Gameplay intent | Route-safety risk | Targeted gate |
| --- | --- | --- | --- | --- | --- |
| Arrival Lift and Baffle Corridor | Arrival `x -5..7, z -32..-24`; Baffle `x -8..8, z -20..-8` | LDF-002, LDF-004, LDF-005, LDF-007 | Pull the player north through alternating pipe baffles while teaching cover rhythm. | Pipe density creating gaps under `2.2m` or catching backing movement. | Baffle pass keeps readable side-step lanes and does not snag melee fallback. |
| Pump Side Room resource branch | `x 10..26, z -22..-8` | LDF-003, LDF-010, LDF-002 | Make optional resources feel like a maintained pump room, not random pickups. | Branch looks like the critical objective path. | Amber objective chain remains stronger toward valve/lobby; pickups remain visible. |
| Cartridge Cache secret | `x -24..-10, z -22..-12`; target `x -18, z -17` | LDF-006, LDF-010, LDF-002 | Use cold-blue pipe section and worker chalk to sell an optional cache. | Secret cue hidden in pipe shadow or placed too close to the critical valve. | Cache clue readable from baffle corridor side but not from locked lift as main objective. |
| Condensate Spine and Lancer read | Spine `x -12..14, z -8..8`; Lancer Bridge `x 8..30, z 2..18`; Lancer near `x 18, z 9, Y +1.2m` | LDF-002, LDF-007, LDF-009, LDF-004 | Add overhead pipe-wall density while keeping the first Lancer silhouette and cover options readable. | Background pipes hiding projectile tells or making bridge feel like a mandatory climb. | First Lancer is visible from spine; cover elbows at planned points still read as cover. |
| Routing Valve Gallery objective stack | `x -30..-8, z 4..18` | LDF-003, LDF-002, LDF-004, LDF-008, LDF-010 | Make the valve a noisy pressure-routing machine whose effect explains the lift unlock. | Gauge/pipe dressing hides valve interaction or implies extra buttons. | Valve prompt readable at chest-height approach; green pressure return path appears after completion. |
| Locked Lift Lobby and Boilerheart Lift | Lobby `x -12..16, z 16..26`; Lift `x -4..8, z 26..34` | LDF-001, LDF-002, LDF-004, LDF-005 | Stack red locked lift, then green restored Boilerheart route, so the return feels changed. | Framing blocks lift transition read or fight space after valve. | Locked-lift rejection still readable; after valve, player can identify lift within `5s` from lobby. |

### Level03 - Boilerheart Core

Goal: make the Boilerheart feel circular, hot, and mechanically legible: steam hazards first, central boiler landmark, optional scattergun, Bellows Node priority, pressure valve, foundry lift.

| Zone | Coordinates / area | Density families | Gameplay intent | Route-safety risk | Targeted gate |
| --- | --- | --- | --- | --- | --- |
| Arrival and Steam-Baffle Approach | Arrival `x -5..7, z -36..-28`; Approach `x -12..12, z -24..-10` | LDF-008, LDF-002, LDF-004, LDF-005 | Teach steam rhythm through warning frames and safe-lane strips before higher pressure. | Dressing masks steam puffs or suggests damage where none exists. | Steam warning visible before damage zones; safe lane matches actual passable route. |
| Gauge Service Alcove | `x -30..-14, z -24..-12` | LDF-003, LDF-010, LDF-006 | Add optional pressure-gauge lore and seed future Gaugekeeper secret language. | Alcove looks like the main route or hides pickups/plaque. | Main route to Boilerheart Ring remains stronger; plaque/pickups unobstructed. |
| Condensate Side Loop | `x 14..30, z -24..-8` | LDF-002, LDF-007, LDF-005 | Make the alternate route read as a safe repositioning loop. | Pipe bundles narrowing VR-friendly loop. | Loop retains comfortable lateral movement and does not become a dead end. |
| Boilerheart Ring hero landmark | `x -22..22, z -10..14` | LDF-009, LDF-002, LDF-004, LDF-007, LDF-010 | Create a central boiler identity visible from multiple angles, with readable circular cover. | Dense machinery hiding Lancer/Scrapper reads or confusing path around ring. | From ring entry, player sees boiler landmark, scattergun side, and lift/catwalk direction. |
| Scattergun Display branch | `x 12..34, z 0..16` | LDF-003, LDF-004, LDF-010, LDF-005 | Make the weapon display feel valuable and make the return fight prove close-range utility. | Display dressing hides weapon pickup or acquisition smoke/audio. | Weapon silhouette and pickup burst visible; no dressing sits inside pickup route automation. |
| Bellows Node Chamber | `x -34..-12, z 2..18`; required open space about `16m x 16m` | LDF-007, LDF-008, LDF-003, LDF-009 | Amplify the support-machine priority read with warning circle, piston-lung silhouette, and pulse-safe edges. | Crowding dodge lanes or hiding pulse VFX. | Chamber keeps dodge space; pulse warning circle remains clear from entry. |
| Pressure Valve Catwalk and Foundry Lift | Catwalk `x -20..20, z 16..28`; Lift `x -8..10, z 28..36` | LDF-001, LDF-002, LDF-003, LDF-004, LDF-005, LDF-008 | Make valve venting visibly release pressure and make the foundry lift a restored exit. | Catwalk clutter during objective defense, lift aperture crowding, shutdown state confusion. | Valve interaction readable; vented state visibly differs; foundry lift route clear after valve. |

### Level04 - Furnace Foundry

Goal: make Level04 the heat-and-heavy-combat showcase, with hazard-safe edges, furnace scale, Bulwark staging, coal cache clues, regulator density, and emergency hoist clarity.

| Zone | Coordinates / area | Density families | Gameplay intent | Route-safety risk | Targeted gate |
| --- | --- | --- | --- | --- | --- |
| Arrival Hoist and Quench Tank prep | Arrival `x -8..10, z -36..-26`; Quench candidate `x -12..6, z -18..-8` | LDF-009, LDF-002, LDF-005, LDF-010 | Establish huge furnace space while offering readable prep/resources. | Large silhouettes obscure first landmark or route direction. | Player sees furnace row glow and forward route from arrival. |
| Foundry Gantry and Slag Side Loop | Gantry `x 18..40, z -22..4`; Slag `x -40..-18, z -20..0` | LDF-007, LDF-008, LDF-009, LDF-005 | Show alternate movement choices and hazard preview without forcing narrow ledge fighting. | Gantry looks mandatory; hazard edge bands do not match damage timing. | At least two routes into Furnace Row remain visible; no rail-less narrow fighting cues. |
| Furnace Row main arena | `x -34..34, z -2..20` | LDF-008, LDF-009, LDF-002, LDF-007, LDF-005 | Make heat pulses, safe lateral movement, and cover rhythm immediately readable. | Heat shimmer/dressing hides Lancer or Scrapper movement. | Warning lamps/edge marks visible before damage; enemy silhouettes not lost against furnace glow. |
| Coal Cache Secret | `x -38..-22, z 12..26`; target `x -30, z 18` | LDF-006, LDF-010, LDF-008 | Use coal dust, cooler pipe, and stuck panel language to make the secret feel earned. | Secret path crosses lethal heat or pulls player into Bulwark pressure. | Secret reachable without active lethal heat; clue visible from safe alcove. |
| Bulwark Hammer Bay | `x 18..40, z 8..28` | LDF-007, LDF-009, LDF-010, LDF-008 | Stage the Bulwark as a readable heavy unit with broken rescue frames and hammer bay scale. | Cover/dressing hiding windup silhouette or reducing circle space. | Bulwark windup visible at eye level; broad lanes remain open. |
| Cooling Regulator Lock and Emergency Hoist | Regulator `x -18..18, z 20..32`; Hoist `x -6..8, z 32..40` | LDF-001, LDF-002, LDF-003, LDF-004, LDF-005, LDF-008 | Make the regulator and hoist feel connected: red/orange hazard pressure drops into green emergency exit. | Confusing current route if regulator lock is not implemented yet; hoist trigger crowding. | If no new lock exists, dressing must read as supportive, not required; green hoist route clear. |

### Level05 - Governor Core

Goal: make the finale feel like a cathedral-scale pressure bureaucracy: ordered approach, regulator arms, readable Warden arena, and a final hoist that clearly unlocks after shutdown.

| Zone | Coordinates / area | Density families | Gameplay intent | Route-safety risk | Targeted gate |
| --- | --- | --- | --- | --- | --- |
| Arrival Hoist and Pressure Chapel | Arrival `x -7..9, z -40..-30`; Chapel `x -18..18, z -28..-10` | LDF-009, LDF-004, LDF-005, LDF-010, LDF-002 | Shift tone from foundry chaos to ordered final machine space while preparing the player. | Over-decorating final prep and hiding health/ammo/archive plaque. | Final resources and plaque remain readable; route points forward into Core Ring. |
| Core Ring orientation landmark | `x -18..18, z -10..8` | LDF-009, LDF-002, LDF-003, LDF-004, LDF-007 | Make the Master Governor logic bank a central read before the Warden. | Rotating/drum silhouettes hide mixed-machine encounter reads. | Player can identify north arena route and side arms from ring. |
| West Regulator Arm | `x -42..-20, z -8..18`; future secret behind `x -38, z 10` | LDF-003, LDF-006, LDF-007, LDF-010, LDF-002 | Make this optional arm a resource/risk route and seed Governor Clerk Void clue language. | Secret cue competes during boss approach or arm dead-ends during combat. | Arm loops back visibly; secret clue only pulls after immediate combat read is resolved. |
| East Regulator Arm | `x 20..42, z -8..18` | LDF-003, LDF-007, LDF-002, LDF-004 | Give the mirrored arm a different pressure identity and support Lancer placement. | Symmetry makes orientation confusing; Lancer silhouette lost in pipe density. | East/west labels and silhouette differences readable from Core Ring. |
| Governor Warden Arena | `x -24..24, z 4..30`; Warden center `x 0, z 17`; cover pylons spaced at least `8m` apart | LDF-007, LDF-008, LDF-009, LDF-004, LDF-005 | Clarify boss attack lanes, cover pylons, red hostile pressure feeds, and shutdown release. | Cover trivializes fight, red/green cues conflict, Warden attacks hidden by machinery mass. | Warden visible on reveal; cover pylons do not fully block boss pressure; boss HUD/readability unaffected. |
| Master Override Hoist final stack | `x -8..8, z 30..40`; Warden-to-exit baseline `4.5m` | LDF-001, LDF-002, LDF-003, LDF-004, LDF-005 | Make the final locked hoist unmistakable before and after Warden defeat. | Hoist is very close to Warden; dressing may crowd boss arena edge or hide lock signals. | Locked hoist rejection readable before fight; green unlock visible after Warden death; no dressing inside final exit trigger. |

## Cross-Level Density Rules

- Put density at route decisions, not evenly everywhere.
- Keep every dressing root visual-only unless the main lane explicitly attaches it to existing collision.
- Preserve existing route triggers, pickup authority, valve authority, hazards, Warden lock, final win, enemy placements, and objective text.
- Do not place decorative roots inside transition, pickup, hazard, enemy spawn, boss, or prompt ownership volumes.
- Amber means attention/objective direction. Red means locked, hostile, or unsafe. Green means restored, safe, or exit. Do not mix these casually.
- Keep floor pieces flush or near-flush with no gameplay collision. They should guide feet, not become ramps or cover.
- On combat lanes, dressing should frame enemies rather than sit behind their silhouettes in the same value/color range.
- On hazards, warning dressing should amplify existing tells, not replace them.
- On secrets, clue density should be weaker than main route signage but consistent enough for repeatable discovery.
- On VR-forward spaces, preserve turning space near lifts/hoists and avoid narrow catwalk combat pressure.

## Batch Validation Gates

Run targeted gates before the full V0 matrix:

1. Compile/editor validation after any implementation changes.
2. Generated scene rebuild for `Level01` through `Level05`.
3. Batch validator confirms every level has at least one hero anchor stack and no level receives only grime/story clutter.
4. Family validator confirms expected roots, metadata, material roles, named child groups, and zero unauthorized colliders/gameplay components.
5. Route-state color audit confirms amber/red/green usage matches current objective/lock/hazard state.
6. Transition safety check confirms no dressing root sits inside lift, hoist, gate, final exit, or level transition volumes.
7. Pickup/prompt visibility check confirms gear key, routing valve, Boilerheart valve, scattergun pickup, regulator if present, Warden lock, plaques, health/ammo, and final hoist remain readable.
8. Hazard readability check confirms steam/furnace VFX and warning timings are not masked by dressing.
9. Combat readability check confirms first Scrapper, first Lancer, Bellows Node, Bulwark, and Warden silhouettes remain clear from intended approach angles.
10. Targeted route smokes:
    - Level01: spawn to key, gate, lift, and intake secret clue.
    - Level02: locked lift, valve, restored return, first Lancer read, cartridge cache clue.
    - Level03: steam approach, scattergun pickup, Bellows Node chamber, valve vent, foundry lift.
    - Level04: heat pulse lanes, coal cache approach, Bulwark Hammer Bay, emergency hoist.
    - Level05: Core Ring orientation, side arms, Warden fight, locked/unlocked final hoist.
11. First-person screenshot review from spawn, objective approach, and exit approach for each level.
12. Route audit reports no blockers.
13. Full V0 matrix passes only after targeted checks are clean.

## Handoff Recommendation

For the next compile, implement this as a single density batch with shared naming and validation rather than separate component promotions. The strongest one-compile result is:

- `Level01`: pressure gate and service lift become the tutorial standard for state language.
- `Level02`: pipe arteries visually connect routing valve to Boilerheart lift.
- `Level03`: Boilerheart Ring, scattergun, Bellows Node, valve, and foundry lift become one circular pressure system.
- `Level04`: furnace hazards and Bulwark staging read as one foundry showcase.
- `Level05`: Warden lock and final hoist become the route finale, with ordered regulator arms and strong boss readability.

This keeps the route mechanics unchanged while making the whole run look deliberately authored in one compile.
