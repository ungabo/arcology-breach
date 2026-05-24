# Brassworks Breach - Unity Art Production Breakdown V1

Status: production planning breakdown  
Scope: map the north-star concept art into buildable Unity asset packages and implementation order  
Companion docs:

- `Documentation/ArtDirection/UNITY_CONCEPT_MATCH_PRODUCTION_STANDARD.md`
- `Documentation/ArtDirection/UNITY_ASSET_ACCEPTANCE_GATES.md`
- `Documentation/STEAMPUNK_NORTH_STAR.md`
- `Documentation/AAA_ASSET_CATALOG.md`
- `Documentation/PARALLEL_ASSET_PACK_PRODUCTION_PLAN.md`
- `Documentation/PARALLEL_LEVEL_PRODUCTION_MAPS.md`

## Production Intent

This breakdown turns the steampunk north-star into side-agent-safe Unity asset packages. It should let multiple agents produce materials, modular kits, props, UI, VFX, audio, and enemy assets without guessing what "concept match" means.

Unity is the review authority. Lookdev, test renders, screenshots, and acceptance proof must be generated inside Unity. Do not use Blender or external render tools.

## Implementation Strategy

Build from shared visual language outward:

1. Materials and import settings.
2. Corridor bay and pressure door vertical slice.
3. Pressure pistol and Scrapper because they are the most visible early-game hero assets.
4. Signage, HUD, VFX, and audio because they make the existing route readable and tactile.
5. Pipeworks/Boilerheart environment packages.
6. Bulwark/foundry packages.
7. Warden/Governor Core packages.
8. Platform reduction packages for Android, WebGL, and VR.

The first production victory should be one Unity screenshot from Level01 or a test bay that looks like the top panel of the concept sheet while also showing the first-person pressure pistol and a Scrapper-ready encounter lane.

## Concept Elements To Unity Packages

| Concept element | Package ID | Buildable Unity package | Primary contents | Acceptance gates |
| --- | --- | --- | --- | --- |
| Wet brassworks corridor | UAP-01 | Material implementation and trim/decal foundation | FinalMaterialsV1 Unity `.mat` candidates, trim sheet plan, decal atlas setup, import presets, platform overrides | COM, COR, SIGN |
| Soot brick, riveted walls, wet stone floor | UAP-02 | Modular corridor core kit | 2m/4m/8m wall, floor, ceiling, corner, doorway, arch, trim, grate, railing, floor guide strips | COR |
| Overhead pipes, tanks, gauges, gas lamps | UAP-03 | Pipe/valve/gauge/gaslight dressing kit | Pipe straights/elbows/T-joints, valve wheels, wall gauges, pressure tanks, rail pieces, lamp fixtures, brackets | COR, VFXAUD |
| Gear-driven pressure door | UAP-04 | Pressure gate and lock language | Large pressure gate, standard pressure door, keyed socket, gear wheel, pressure gauge, red/green lamps, steam vent sockets | GATE, VFXAUD, SIGN |
| Service lift/hoist exit language | UAP-05 | Lift and hoist kit | Service lift frame, platform, call plate, pulley, cable drum, green glass lamp, master override hoist variant | GATE, COR, SIGN |
| First-person brass pressure pistol | UAP-06 | Pressure Pistol hero pack | Viewmodel, world pickup, display pose, gauge, coil, valve lever, muzzle/vent anchors, material map, platform variants | PISTOL, VFXAUD |
| Bottom-left cutter machine | UAP-07 | Scrapper final machinery pack | Scrapper mesh/rig candidate, cutter arms, boiler body, piston limbs, furnace eye, VFX sockets, LODs/collision | SCRAP, VFXAUD |
| Warning plates, work orders, labels | UAP-08 | Signage/decal production kit | Objective plates, lock warnings, route arrows, chevrons, lore plaques, secret marks, scorch/oil decals, floor strips | SIGN |
| Mechanical instrument HUD | UAP-09 | HUD/UI instrument kit | Health/pressure/ammo gauges, gear-key lamp, objective plate, boss gauge, prompt panels, menu panels, sprite import settings | HUD |
| Steam puffs, pressure sparks, heat | UAP-10 | VFX pressure and hazard kit | Steam vent, muzzle puff, impact sparks, pressure ring, gate release, lift activation, heat shimmer, machine death variants | VFXAUD |
| Brass latch, valve dump, ambience | UAP-11 | Audio feedback kit | Weapon snaps, gate/lift cues, valve turns, gear ticks, steam vents, furnace beds, machine tells, pickup cues, UI ticks | VFXAUD |
| Boilerheart machinery and Bellows | UAP-12 | Boilerheart machine package | Central boiler, valve catwalk dressing, steam curtain modules, Bellows Node shell, scattergun display stand support props | COR, VFXAUD, SIGN |
| Heavy foundry enemy | UAP-13 | Bulwark and foundry package | Bulwark mesh/rig candidate, hammer arms, furnace plates, rescue-frame bay props, heat hazard decals, cooling regulator | BUL, VFXAUD, SIGN |
| Final boss and core machinery | UAP-14 | Governor Warden and core package | Warden boss shell, pressure cannon, furnace heart, regulator pylons, governor drums, punch-card belts, shutdown relief language | WARDEN, VFXAUD, HUD |
| Lower-spec shipping variants | UAP-15 | Platform reduction package | Texture downscales, material merges, LOD tuning, VFX quality tiers, compressed audio, VR comfort notes | COM plus relevant asset gates |

## Implementation Order

| Order | Slice | Why first | Required output |
| ---: | --- | --- | --- |
| 1 | UAP-01 Material implementation | Every later package needs the same brass/iron/stone/brick/walnut/enamel/glass vocabulary. | Unity material candidates, import settings, preview scene, platform texture table. |
| 2 | UAP-02 Corridor core kit | Converts greybox route into the top-panel concept language and sets scale for all levels. | 8m test corridor bay with floor/wall/ceiling/corner/doorway modules. |
| 3 | UAP-03 Dressing kit | Pipes, gauges, lamps, tanks, rails, and valves create the dense brassworks identity. | Dressing prefabs with LOD/collision, placed in the corridor bay. |
| 4 | UAP-04 Pressure gate | The first major objective reads through this asset and it anchors Level01. | Locked/open gate prefab with red/green states, sockets, VFX/audio hooks. |
| 5 | UAP-06 Pressure Pistol | It is always on screen and must match the concept before broad art polish. | First-person Unity proof, world pickup, material map, VFX/audio anchors. |
| 6 | UAP-07 Scrapper | Most common early enemy; locks melee machine language for later enemies. | In-game Scrapper proof with attack tell hooks, LOD/collision, shutdown sockets. |
| 7 | UAP-08 and UAP-09 Signage/HUD | Readability accelerates all existing levels and reduces gameplay ambiguity. | World signage/decal atlas and instrument HUD screenshots at target resolutions. |
| 8 | UAP-10 and UAP-11 VFX/audio pass | Makes pressure systems tactile and validates combat/objective feedback. | Pressure gate, pistol, Scrapper, pickup, lift, and ambient cues covered. |
| 9 | UAP-12 Boilerheart package | Supports Level03 steam/hazard identity and Bellows Node/scattergun staging. | Boiler/valve/steam curtain/Bellows/display props with quality tiers. |
| 10 | UAP-13 Bulwark/foundry package | Escalates heavy combat and heat hazard visuals in Level04. | Bulwark proof, foundry bay props, heat hazard and cooling regulator language. |
| 11 | UAP-14 Warden/core package | Boss art depends on established material, VFX, HUD, and arena language. | Warden proof, regulator pylons, boss states, shutdown relief, hoist unlock read. |
| 12 | UAP-15 Platform reductions | Reduction should happen after source art is accepted, not before. | Android/WebGL/VR overrides, lower LODs, particle tiers, compressed audio notes. |

## First Vertical Slice Definition

The first slice should prove the concept sheet in Unity with minimum breadth and maximum signal.

Required contents:

- One 8m to 12m brassworks corridor bay.
- Wet oil-dark stone floor, soot brick, blackened iron panels, brass/copper pipe bundles, railings, gauges, gas lamps, and decals.
- One pressure gate at end of bay with red locked and green open states.
- First-person pressure pistol visible in screenshot.
- One Scrapper stand-in or final candidate visible at combat distance.
- Amber/red/green route language.
- At least one ambient steam vent, one pressure release VFX, one brass spark or machine hit VFX, and one looped machinery ambience cue.
- HUD objective/key/pressure state in mechanical instrument style.
- Unity-only screenshots at `1920x1080` and `1280x720`.

This slice should use the acceptance gates before any full-level art replacement begins.

## Level Application Map

| Level | Primary art packages | Local focus |
| --- | --- | --- |
| Level01 Brassworks Intake | UAP-01 through UAP-11 | Teach corridor language, pressure pistol, Scrapper, gear key, pressure gate, service lift, secret clue decals. |
| Level02 Pipeworks Annex | UAP-01 through UAP-05, UAP-08, UAP-10, UAP-11 | Pipe spine, routing valve, gauge banks, condensate, cartridge cache, lift lock/readiness signage. |
| Level03 Boilerheart Core | UAP-03, UAP-08 through UAP-12 | Boiler silhouette, steam hazards, Bellows Node, scattergun display stand, valve catwalk, pressure relief feedback. |
| Level04 Furnace Foundry | UAP-01, UAP-03, UAP-08, UAP-10, UAP-11, UAP-13 | Heat rows, Bulwark reveal, rescue-frame machinery, cooling regulator, heat hazard decals, coal cache. |
| Level05 Governor Core | UAP-08 through UAP-11, UAP-14 | Cathedral machine core, Warden arena, regulator pylons, boss HUD, master override hoist, shutdown green relief. |
| Platform variants | UAP-15 | Android/WebGL texture and VFX reduction, VR comfort scaling, conservative LOD transitions. |

## Package Work Orders

### UAP-01 Material Implementation

Inputs:

- FinalMaterialsV1 material families.
- Existing Unity material conventions.
- Concept sheet corridor, enemy, and pistol panels.

Outputs:

- Unity material candidates for aged brass, blackened riveted iron, wet oil-dark stone, soot brick, copper pipe, greasy walnut, cream enamel gauge, amber glass, leather bellows, hazard enamel, and scorch/oil decals.
- Import settings for BaseColor, Normal, ORM, alpha helpers, mipmaps, compression, and platform overrides.
- Test spheres/planes and one corridor bay material proof.

Do not auto-replace active materials. Stage candidates for main integration.

### UAP-02 Corridor Core Kit

Outputs:

- Grid-aligned 2m/4m/8m wall, floor, ceiling, corner, doorway, arch, grate, rail, and trim pieces.
- Reference assembly matching corridor width `3.5m` to `5m`.
- LODs for repeated modules and simple collision.
- Lightmap/static batching notes.

Acceptance focus: no bare greybox, no texture stretch over `5m`, no snag collision.

### UAP-03 Dressing Kit

Outputs:

- Pipe straight, elbow, junction, bracket, valve wheel, wall gauge, tank, gas lamp, railing, vent, and pressure relief cap.
- Prefab variants for wall, ceiling, floor edge, and door-adjacent use.
- VFX sockets for steam leaks and pressure relief.

Acceptance focus: dense brassworks identity without blocking combat or route readability.

### UAP-04 Pressure Gate

Outputs:

- Large pressure gate prefab and standard pressure door variant.
- Keyed socket, gear wheel, gauge, pressure cylinders, warning lamps, open/closed collision, VFX/audio hooks.
- Red locked, amber key attention, and green restored states.

Acceptance focus: Level01 objective readability from `12m`.

### UAP-05 Lift And Hoist

Outputs:

- Service lift frame, platform, pulley, cable drum, call plate, green glass pressure lamp, and final master hoist variant.
- Safe-zone floor language and activation feedback sockets.

Acceptance focus: exit identity is different from lock identity but shares pressure-machine language.

### UAP-06 Pressure Pistol

Outputs:

- First-person viewmodel, world pickup, display prop, material assignment map, recoil/vent/gauge/lever anchors.
- Primary fire, alternate pressure burst, impact, pickup VFX/audio hook names.
- VR grip/display notes and low-spec world pickup variant.

Acceptance focus: bottom-right concept panel match in Unity first person.

### UAP-07 Scrapper

Outputs:

- Scrapper model/rig candidate, LODs, simple collision, socket map, material map.
- Attack tell, hit, and shutdown VFX/audio hook names.
- In-game proof at Level01 combat distance.

Acceptance focus: cutter machine silhouette and pre-damage read.

### UAP-08 Signage And Decals

Outputs:

- Gameplay sign set: gear seal locked, key required, valve crew only, surge hazard, lift pressure, Warden active.
- Route arrows, chevrons, floor strips, work orders, plaques, secret clues, oil/scorch/soot/leak decals.
- Atlas/import notes and gameplay-distance proof.

Acceptance focus: short copy, amber/red/green language, readable at `6m` to `10m`.

### UAP-09 HUD/UI Instruments

Outputs:

- Health/pressure/ammo gauges, gear key lamp, objective plate, interact prompt, boss gauge, menu panels.
- Sprite atlas, nine-slice notes, import settings, screen proofs.

Acceptance focus: compact mechanical instrument style with no digital drift and no text overlap.

### UAP-10 VFX Pressure And Hazards

Outputs:

- Steam vent, pressure ring, muzzle puff, impact sparks, scorch/oil impact, gate release, lift activation, steam hazard states, furnace heat states, machine shutdown variants.
- Quality tiers for Windows, Android/WebGL, and VR.

Acceptance focus: tactile pressure identity without obscuring combat or UI.

### UAP-11 Audio Feedback

Outputs:

- Pressure pistol snap/tail, pressure burst valve dump, brass latch, gear movement, gate denied/open, lift activate, valve turn, steam vent, pipe knock, furnace bed, machine tells, pickup cues, HUD ticks.
- Loop and one-shot integration notes.
- Compression/downscale notes.

Acceptance focus: every gameplay-critical interaction has a corresponding audible cue.

### UAP-12 Boilerheart Package

Outputs:

- Central boiler shell, valve catwalk props, steam curtain modules, Bellows Node shell, scattergun display support props, relief pipe language.
- Steam hazard warning/active/safe visual set.

Acceptance focus: hot central machine scale and readable pressure relief.

### UAP-13 Bulwark And Foundry Package

Outputs:

- Bulwark model/rig candidate, slam tell sockets, foundry rescue-frame bay props, oversized tools, furnace row pieces, cooling regulator, coal cache props, heat warning decals.

Acceptance focus: heavy enemy readability in broad arena and low-overdraw heat hazard language.

### UAP-14 Warden And Governor Core Package

Outputs:

- Governor Warden boss candidate, regulator pylons, governor drums, punch-card belts, red hostile pressure lines, green relief lines, master override hoist presentation.
- Boss state VFX/audio/HUD hooks.

Acceptance focus: final guardian hierarchy, phase readability, and shutdown reward.

### UAP-15 Platform Reduction

Outputs:

- Android/WebGL texture override table.
- Material merge/atlas plan.
- Lower LOD validation.
- Particle quality tiers.
- Compressed audio variants.
- VR comfort notes for scale, labels, particles, and LOD popping.

Acceptance focus: lower-spec variants preserve route, enemy, weapon, and objective readability.

## Side-Agent Handoff Rules

- Work in versioned staging packages until the main integration lane accepts replacement.
- Include the relevant acceptance gate IDs in each package report.
- Report triangle counts, material slots, texture sizes, LODs, collision, VFX/audio hooks, Unity proof paths, and platform notes.
- Do not modify active prefabs, shared materials, scenes, or status ledgers unless explicitly assigned.
- Do not commit as part of asset package production unless the main agent asks for it.

## Highest-Priority Next Slices

1. UAP-01 plus UAP-02: material implementation and one modular corridor bay.
2. UAP-03 plus UAP-04: pipe/gauge dressing and pressure gate state language.
3. UAP-06: pressure pistol viewmodel and world pickup proof.
4. UAP-07: Scrapper final machinery proof with tell/shutdown sockets.
5. UAP-08 plus UAP-09: signage/decal and HUD readability pass for Level01.
6. UAP-10 plus UAP-11: VFX/audio coverage for pistol, gate, lift, pickup, Scrapper, and ambient machinery.
