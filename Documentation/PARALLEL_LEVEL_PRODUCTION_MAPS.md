# Brassworks Breach - Parallel Level Production Maps

Timestamp: 2026-05-23 21:31 -04:00

Scope: side-agent planning document only. This file is intended to run ahead of Unity implementation without editing scenes, scripts, existing status docs, or generated assets.

## Purpose

This document turns the current five-level route into production-ready map notes that can be handed to level, art, encounter, audio, QA, and platform agents without blocking the main code path.

The current route is:

1. `Level01` - Brassworks Intake
2. `Level02` - Pipeworks Annex
3. `Level03` - Boilerheart Core
4. `Level04` - Furnace Foundry
5. `Level05` - Governor Core

These notes are deliberately practical for Unity. Coordinates are approximate greybox targets, not final authored positions. Use them to preserve scale, pacing, sightlines, encounter placement, and player readability when the production maps are rebuilt.

## Shared Coordinate And Scale Conventions

- Unity scale: `1 unit = 1 meter`.
- Coordinate convention for planning: `+Z = north/deeper into the facility`, `+X = east/right`, `Y = elevation`.
- Player height target: `1.8m`.
- Main corridor width: `3.5m` to `5m`.
- Tight service corridor width: `2.2m` minimum, used sparingly.
- Main combat room minimum: `12m x 12m`.
- Medium combat arena: `18m x 24m`.
- Large finale arena: `30m x 30m` usable floor with stable central landmarks.
- Standard door/gate: `3m` wide, `2.8m` high.
- Large pressure gate: `4m` to `6m` wide, `4m` high.
- VR-safe ramp slope: target `1:10` or gentler.
- Avoid mandatory jumps, crouch gaps, or rapid forced camera spins.
- Keep floor collision simple. Use raised detail meshes as non-blocking visual dressing unless they are deliberate cover.

## Production Readability Rules

- The golden path should be visible through light, signage, pipe direction, floor guide strips, and repeated green service-lift language.
- Objective locks should be seen before their keys or valves when possible.
- Secrets should look suspicious, not mandatory. Use missing rivets, warmer pipe glow, draft steam, unusual floor plates, or worker chalk marks.
- Each level needs one first-read landmark visible from at least two rooms.
- Each combat role should have a clear entry space:
  - `Scrapper`: short route into melee with windup space.
  - `Lancer`: elevated or long sightline position with cover breaks.
  - `Bulwark`: wide room, clear approach lane, cover around edges.
  - `Bellows Node`: visible central or side support device with readable pulse radius.
  - `Governor Warden`: boss arena with stable orientation landmarks.

## Parallel Production Rules

Work that can safely run ahead now:

- Paper maps and room manifests.
- Asset lists per room.
- Signage text, lore plaque text, warning stamp text.
- Encounter intent and spawn families without final balance values.
- Prop kits and modular wall/floor/pipe pieces.
- Secret-cache visual language.
- Platform budgets for LODs, collision, and draw-call grouping.
- VR comfort review notes.
- QA route scripts and validation expectations.

Work that should wait for main implementation:

- Exact enemy counts tied to current balance numbers.
- Final navmesh baking decisions.
- Scene edits or prefab placement in active Unity scenes.
- Final lighting values.
- Final asset memory budgets after import settings are chosen.
- Save/autosave placement until persistence rules are expanded.

## Level 01 - Brassworks Intake

### Production Target

- Purpose: teach movement, shooting, Scrapper pressure, gear key, pressure gate, secret language, and service lift.
- Target footprint: `68m x 46m`, compact and readable.
- Target room count: 8 primary rooms plus 1 secret cache.
- Expected completion time: 4 to 6 minutes for a first-time player.
- Combat density: low to moderate.
- Core feel: unsafe service entrance, soot-brick utility spaces, repair benches, small machinery, first pressure systems.

### Approximate Layout

```text
                         +Z north

       [Service Lift]
            z +20
              |
   [Furnace Control Room]
      x -8..14, z +8..18
              |
      [Pressure Gate]
          z +4
              |
 [Gate Overlook]--[Gear-Key Workshop]
 x -16..4       x +8..24
 z -4..+6       z -8..+6
      |              |
 [Repair Bay]---[Secret Intake Cache]
 x -18..6       x +16..28
 z -18..-6      z -22..-14
      |
 [Maintenance Throat]
      |
 [Soot Service Entry]
      z -22
```

### Room List

| Room | Approximate Bounds | Function | Notes |
| --- | --- | --- | --- |
| Soot Service Entry | `x -6..6`, `z -24..-16` | Spawn and first orientation | Player faces north toward warm pipe light and the first readable corridor. |
| Maintenance Throat | `x -5..5`, `z -16..-8` | First movement lane | Include low steam leaks as visual-only dressing, not damage yet. |
| Repair Bay | `x -18..6`, `z -18..-6` | First combat room | Scrapper tutorial with benches, waist-high boilers, and clear fallback space. |
| Gate Overlook | `x -16..4`, `z -4..6` | Show locked pressure gate | Player sees the locked route before owning the key. |
| Gear-Key Workshop | `x 8..24`, `z -8..6` | Key objective branch | Key is on a plinth with amber light and two paths back. |
| Secret Intake Cache | `x 16..28`, `z -22..-14` | Optional resource reward | Hidden behind service panel or suspicious pipe recess. |
| Pressure Gate | `x -4..8`, `z 3..7` | First lock | Large riveted gear gate with red locked state and green open state. |
| Furnace Control Room | `x -8..14`, `z 8..18` | Exit-side combat and reward | Add ammo, health, and the first archive plaque. |
| Service Lift | `x -3..7`, `z 18..24` | Level transition | Green restored-pressure language, pulley spinner, safe zone. |

### Route Beats

1. Spawn in the soot entry with the service route framed by amber pipe light.
2. Walk through maintenance throat and hear/see machinery ambience.
3. Enter repair bay and fight the first Scrapper.
4. Reach gate overlook and attempt or inspect the locked pressure gate.
5. Follow amber gear-key signage into the workshop branch.
6. Collect gear key from a plinth framed by brass gauges.
7. Loop back to the pressure gate from a second angle.
8. Open gate, get green pressure feedback, and fight one small escalation.
9. Read or pass the intake archive plaque.
10. Board the service lift to Level02.

### Combat Encounters

- `E01-A First Scrapper`
  - Position: Repair Bay, near `x -10`, `z -11`.
  - Goal: teach melee pressure without cornering the player.
  - Cover: two benches that block line of movement but leave wide side routes.

- `E01-B Key Workshop Ambush`
  - Position: Gear-Key Workshop, one Scrapper starts dormant near `x 18`, `z -1`.
  - Trigger: key pickup or entry threshold.
  - Goal: teach that objectives can wake machines.

- `E01-C Gate Open Response`
  - Position: Furnace Control Room, one Scrapper or weak Boiler Tick candidate later.
  - Trigger: pressure gate opens.
  - Goal: reward route completion with a short final encounter before lift.

### Secret Placement

- `Secret - Intake Pressure Cache`
  - Approximate location: `x 21`, `z -18`.
  - Access concept: side service panel behind a pipe rack in the repair bay or workshop return loop.
  - Visual clue: chalk arrow from the Locked Shift, hairline green steam, missing rivet pattern.
  - Rewards: small health, pressure cartridges, one lore scrap.
  - Implementation independence: can be blocked out as a simple trigger volume and resource alcove without touching later systems.

### Objective Locks

- Gear key unlocks pressure gate.
- Pressure gate controls access to furnace control room and service lift.
- No campaign-scoped lock in this level.

### Traversal

- No jump required.
- One gentle ramp may connect repair bay to gate overlook at `Y +0.6m`.
- Avoid stairs over `0.18m` risers if a ramp can do the same job.
- Provide at least `4m` wide turning space at the service lift for future VR.

### Signage And Readability

- Entry sign: `INTAKE SERVICE - AUTHORIZED PRESSURE CREWS ONLY`
- Gate sign: `GEAR SEAL LOCKED - KEY REQUIRED`
- Key branch sign: `REPAIR BAY - GEAR STORES`
- Lift sign: `SERVICE LIFT: PIPEWORKS ANNEX`
- Use amber lamps for key direction, red pressure lamps for locked gate, green lamps for active lift.

### Art Set Dressing

- Sooty brick wall modules with riveted iron braces.
- Copper pipe bundles entering low wall manifolds.
- Repair benches with small gears, wrenches, oil cans, broken pressure gauges.
- Small flywheels and belt drives, mostly non-interactive.
- Gate should be visibly gear-driven, with two or three readable rotating pieces.
- The service lift needs a brass call plate, pulley frame, cable drum, and green glass pressure lamp.

### Performance And LOD Notes

- Use simple combined wall/floor pieces where possible.
- Keep pipe bundles modular and reuse 3 to 5 variants.
- Small bench clutter can be baked into static prop clusters.
- Avoid transparent steam overuse in the first room; save heavier particles for objective feedback.
- Occlusion opportunity: dogleg between repair bay and gate overlook.

### VR Comfort Notes

- First combat should not spawn behind the player.
- Gate open feedback should avoid screen shake dependency.
- Service lift transition should fade or hold stable forward orientation.
- Keep collision uncluttered around workbenches.

### Independently Implementable Now

- Final paper blockout with room bounds.
- Modular intake wall/floor/pipe kit.
- Gear-key plinth prefab concept.
- Pressure gate art concept.
- Intake signage decals.
- Secret cache clue decals.
- Encounter validation checklist.

## Level 02 - Pipeworks Annex

### Production Target

- Purpose: introduce longer sightlines, first meaningful ranged combat, pipe routing objective, cartridge-cache secret, and deeper industrial scale.
- Target footprint: `76m x 48m`.
- Target room count: 9 primary rooms plus 1 secret.
- Expected completion time: 6 to 8 minutes.
- Combat density: moderate.
- Core feel: pipe spine, valve math, pressure baffles, condensate, stacked pipe racks, first Lancer threat.

### Approximate Layout

```text
                         +Z north

          [Boilerheart Lift]
          x -4..8, z +26..+34
                 |
        [Locked Lift Lobby]
          x -12..16, z +16..+26
           /                 \
 [Routing Valve Gallery]   [Lancer Pipe Bridge]
 x -30..-8,z +4..+18      x +8..+30,z +2..+18
           \                 /
           [Condensate Spine]
          x -12..14,z -8..+8
                 |
        [Baffle Corridor]
          x -8..8,z -20..-8
          /              \
 [Cartridge Cache]   [Pump Side Room]
 x -24..-10,z -22..-12 x +10..+26,z -22..-8
                 |
          [Arrival Lift]
          x -5..7,z -32..-24
```

### Room List

| Room | Approximate Bounds | Function | Notes |
| --- | --- | --- | --- |
| Arrival Lift | `x -5..7`, `z -32..-24` | Entry from Level01 | Keep player facing north with pipes pulling view forward. |
| Baffle Corridor | `x -8..8`, `z -20..-8` | Movement contrast | Alternating pipe baffles create cover and rhythm. |
| Pump Side Room | `x 10..26`, `z -22..-8` | Resource branch | Good spot for ammo, health, first readable pump machinery. |
| Cartridge Cache | `x -24..-10`, `z -22..-12` | Optional secret | Hidden maintenance storage behind pipe shadows. |
| Condensate Spine | `x -12..14`, `z -8..8` | Main combat lane | First longer line of sight, but cover breaks every `6m`. |
| Routing Valve Gallery | `x -30..-8`, `z 4..18` | Objective branch | Valve objective, pressure gauge bank, side ambush. |
| Lancer Pipe Bridge | `x 8..30`, `z 2..18` | Ranged threat branch | Lancer reads from distance with elevated pipe bridge. |
| Locked Lift Lobby | `x -12..16`, `z 16..26` | Show route lock | Boilerheart lift visible but inactive until routing valve. |
| Boilerheart Lift | `x -4..8`, `z 26..34` | Level transition | Service lift to Level03. |

### Route Beats

1. Arrive in a cramped lift bay and see the pipe spine beyond.
2. Move through baffles that introduce cover and line-of-sight breaks.
3. See the Boilerheart lift lobby locked by pressure-routing status.
4. Hear or see Lancer threat across a pipe bridge before reaching it.
5. Branch west to the routing valve gallery.
6. Activate routing valve, causing pressure lamps along the spine to turn green.
7. Return through a changed encounter state with a small pressure response.
8. Optional: find cartridge cache from pipe-shadow clues near the baffle corridor.
9. Board Boilerheart lift.

### Combat Encounters

- `E02-A Baffle Scrappers`
  - Position: Baffle Corridor, two staggered melee threats.
  - Goal: teach backing through cover without snagging.

- `E02-B First Lancer Read`
  - Position: Lancer Pipe Bridge, `x 18`, `z 9`, `Y +1.2m`.
  - Goal: introduce ranged projectile tell with visible cover options.
  - Cover: pipe elbows at `x -4`, `z 2`; `x 5`, `z 8`; `x -8`, `z 12`.

- `E02-C Routing Valve Defense`
  - Position: Routing Valve Gallery.
  - Trigger: valve interaction.
  - Enemies: one Scrapper from the entry side, one Lancer or later Boiler Tick candidate from the far side.
  - Goal: make objective completion feel noisy and mechanical.

- `E02-D Lift Lobby Pressure`
  - Position: Locked Lift Lobby after valve.
  - Enemies: one or two light machines.
  - Goal: verify that the return route has changed but remains readable.

### Secret Placement

- `Secret - Pipeworks Cartridge Cache`
  - Approximate location: `x -18`, `z -17`.
  - Access concept: crawl-sized visual hatch, but gameplay access should be standing-height for now. Use a narrow door or slid panel rather than crouch.
  - Visual clue: worker chalk text `SPARE BOLTS UNDER COLD PIPE`, a cold-blue pipe section, and a nonstandard wall plate.
  - Rewards: pressure cartridges, small health, optional archive note.
  - Keep the entrance away from the critical routing valve so it reads optional.

### Objective Locks

- Boilerheart lift starts locked.
- Routing valve unlocks Boilerheart lift.
- Lock state should be visible in the lift lobby and on repeating pipe lamps along the return route.

### Traversal

- Use mild elevation changes on the Lancer bridge, but keep alternate ground path available.
- Avoid pipe baffles that create `less than 2.2m` gaps.
- Route should loop rather than force a long backtrack.
- No damage hazards required in the main spine for the production first pass; visual condensate can imply danger.

### Signage And Readability

- Arrival sign: `PIPEWORKS ANNEX`
- Locked lift sign: `BOILERHEART FEED: PRESSURE ROUTE INCOMPLETE`
- Valve sign: `MAIN ROUTING VALVE`
- Secret clue: `LOCKED SHIFT STORES`
- Directional signs should be stamped on enamel plates mounted to pipe frames.

### Art Set Dressing

- Dense pipe walls with readable gaps.
- Condensate drip pans, brass drains, pressure dials.
- Overhead pipe bridge silhouette for first Lancer read.
- Rotating routing valve with gauge needles and pressure lamps.
- Cartridge crates with stamped brass caps.
- Color accents: amber objective, green routed pressure, red locked lift.

### Performance And LOD Notes

- Pipe density must be controlled through modular pipe-wall panels.
- Use fake depth on pipe racks instead of individually modeled background pipes.
- Use LODs for bridge valve wheels and gauge clusters.
- Occlusion opportunity: baffle corridor and side-room doglegs.
- Limit active particle leaks to 2 or 3 visible at once.

### VR Comfort Notes

- Lancer bridge should not require the player to look straight up.
- Projectile lanes should allow side-stepping rather than rapid backwards movement.
- Keep valve interaction at comfortable chest height, `Y 1.1m` to `1.4m`.
- Return route should avoid surprise enemies from directly behind.

### Independently Implementable Now

- Pipeworks modular pipe-wall kit and baffle kit.
- Valve gallery prop cluster.
- Lift lock/readiness signage.
- Lancer bridge greybox study.
- Cartridge cache visual language.
- Routing valve audio/VFX concept sheet.

## Level 03 - Boilerheart Core

### Production Target

- Purpose: introduce pressure-hazard management, the Steam Scattergun, the Bellows Node support role, and the foundry descent lock.
- Target footprint: `72m x 60m`.
- Target room count: 10 primary rooms plus 1 optional cache candidate.
- Expected completion time: 8 to 10 minutes.
- Combat density: moderate with one support-machine spike.
- Core feel: hot central boiler, circular pressure systems, steam curtains, brass valve cathedral, unstable heart of the works.

### Approximate Layout

```text
                         +Z north

             [Foundry Lift Lobby]
              x -8..10,z +28..+36
                      |
           [Pressure Valve Catwalk]
              x -20..20,z +16..+28
              /                  \
 [Bellows Node Chamber]      [Scattergun Display]
 x -34..-12,z +2..+18       x +12..+34,z +0..+16
              \                  /
              [Boilerheart Ring]
              x -22..22,z -10..+14
                      |
           [Steam-Baffle Approach]
              x -12..12,z -24..-10
             /                    \
 [Gauge Service Alcove]      [Condensate Side Loop]
 x -30..-14,z -24..-12      x +14..+30,z -24..-8
                      |
              [Arrival Lift]
              x -5..7,z -36..-28
```

### Room List

| Room | Approximate Bounds | Function | Notes |
| --- | --- | --- | --- |
| Arrival Lift | `x -5..7`, `z -36..-28` | Entry from Level02 | Player sees the boiler glow ahead. |
| Steam-Baffle Approach | `x -12..12`, `z -24..-10` | Introduce steam hazard language | Use short, readable puffs before damaging zones. |
| Gauge Service Alcove | `x -30..-14`, `z -24..-12` | Resource and lore branch | Archive plaque and first pressure-gauge cluster. |
| Condensate Side Loop | `x 14..30`, `z -24..-8` | Alternate route and cover | Supports route variety and VR-friendly loop. |
| Boilerheart Ring | `x -22..22`, `z -10..14` | Landmark arena | Central boiler column, circular cover, mixed enemies. |
| Scattergun Display | `x 12..34`, `z 0..16` | Optional weapon pickup | Must read as valuable before the foundry. |
| Bellows Node Chamber | `x -34..-12`, `z 2..18` | Support-machine encounter | Clear pulse radius and nearby Scrapper pressure. |
| Pressure Valve Catwalk | `x -20..20`, `z 16..28` | Objective branch | Valve unlocks foundry lift and shuts down linked steam hazards. |
| Foundry Lift Lobby | `x -8..10`, `z 28..36` | Locked exit | Visible from ring and valve catwalk. |
| Optional Gauge Cache | `x -31`, `z -2` candidate | Future secret | Hide behind rotating gauge shutters or maintenance void. |

### Route Beats

1. Arrive from Pipeworks and see the boiler silhouette through steam.
2. Learn steam-hazard visual rhythm in a safe or low-risk baffle lane.
3. Enter Boilerheart Ring, a readable circular landmark.
4. See the foundry lift locked by pressure and the valve catwalk above/behind it.
5. Detour to the Steam Scattergun display. Pickup should be visible enough to draw players but not required to progress.
6. Fight a close-range encounter that lets the scattergun matter.
7. Enter Bellows Node chamber and learn support-machine priority.
8. Reach pressure valve catwalk, vent the heart valve, and watch steam hazards reduce or shut down.
9. Return to foundry lift through safer vents and green route cues.
10. Descend to Level04.

### Combat Encounters

- `E03-A Steam Approach Scrappers`
  - Position: Steam-Baffle Approach.
  - Enemies: one or two Scrappers.
  - Goal: force movement around steam puffs without high damage pressure.

- `E03-B Boilerheart Ring Mix`
  - Position: Boilerheart Ring.
  - Enemies: Scrappers plus one Lancer on a partial catwalk.
  - Goal: combine circular movement and ranged line control.

- `E03-C Scattergun Trial`
  - Position: Scattergun Display return route.
  - Trigger: pickup or route threshold.
  - Enemies: two close Scrappers or one Scrapper plus Boiler Tick candidate later.
  - Goal: immediately prove why the close-range weapon exists.

- `E03-D Bellows Node Priority`
  - Position: Bellows Node Chamber.
  - Enemies: Bellows Node plus two Scrappers.
  - Goal: teach that the stationary device amplifies pressure and should be prioritized.

- `E03-E Valve Catwalk Defense`
  - Position: Pressure Valve Catwalk.
  - Trigger: valve interaction.
  - Enemies: one Lancer at distance, one Scrapper from lower return.
  - Goal: objective completion under pressure, followed by clear release.

### Secret Placement

- Future candidate: `Secret - Gaugekeeper Cache`
  - Approximate location: `x -31`, `z -2`.
  - Access concept: rotating gauge wall opens after shooting or interacting with a misaligned pressure gauge.
  - Visual clue: gauge needle stuck in green while nearby gauges are red.
  - Rewards: ammo, small health, possibly an early lore note about the Locked Shift manipulating sensors.
  - Independence: can be planned now; implementation should wait until secret-trigger rules are standardized beyond simple trigger volumes.

### Objective Locks

- Foundry lift starts locked.
- Boilerheart pressure valve unlocks foundry lift.
- Same valve shuts down or lowers linked steam hazards.
- Scattergun pickup is optional but should become a persistent weapon unlock.

### Traversal

- Main route should be a loop around the boiler, not a straight hallway.
- Catwalk height target: `Y +2.0m` to `+3.0m`, with ramps preferred over steep stairs.
- Do not make players cross narrow catwalks during heavy melee pressure.
- Bellows Node chamber needs at least `16m x 16m` open space to dodge pulses.

### Signage And Readability

- Arrival sign: `BOILERHEART CORE`
- Locked exit sign: `FOUNDRY LIFT PRESSURE-LOCKED`
- Valve sign: `VENT HEART PRESSURE`
- Scattergun display plate: `EMERGENCY STEAM SCATTERGUN - BREAK SEAL`
- Bellows warning: `OVERPRESSURE AMPLIFIER - KEEP CLEAR`
- Use red-orange steam hazard lamps before valve, green relief lamps after valve.

### Art Set Dressing

- Central boiler cylinder with furnace glow, riveted brass bands, gauge rings, and animated flywheels.
- Scattergun display stand with enamel plate, shell rack, walnut/brass weapon details, and acquisition steam.
- Bellows Node with piston lungs, expanding pressure rings, and floor warning circle.
- Valve catwalk with large wheel, pressure gauges, pressure relief pipes, and dripping condensate.
- Steam curtain modules that can swap between damaging and vented states.

### Performance And LOD Notes

- Central boiler can hide occlusion between quadrants.
- Use 6 to 8 repeated curved wall segments rather than unique ring pieces.
- Steam particles should have quality-scaled density.
- Bellows pulse VFX should be cheap enough for repeated use and readable on Android/WebGL later.
- Scattergun display prop can be high-detail on Windows but needs a low-poly variant.

### VR Comfort Notes

- Avoid forced circular strafing against the Bellows Node; provide side cover and stable retreat.
- Catwalks should have visual railings even if collision is simplified.
- Steam hazards need world-space tells, not just screen effects.
- Weapon pickup should not require looking down sharply.

### Independently Implementable Now

- Boilerheart circular greybox map.
- Scattergun display art blockout and signage.
- Bellows Node chamber layout variants.
- Steam hazard visual language sheet.
- Valve catwalk prop set.
- Gaugekeeper cache concept, held until trigger rules are chosen.

## Level 04 - Furnace Foundry

### Production Target

- Purpose: escalate hazard choreography, introduce heavy Bulwark combat as a recurring pressure point, and make the facility feel physically massive.
- Target footprint: `86m x 64m`.
- Target room count: 11 primary rooms plus 1 secret.
- Expected completion time: 9 to 12 minutes.
- Combat density: moderate to high.
- Core feel: industrial heat, hammer machinery, furnace rows, slag gutters, heavy rescue frames turned into containment machines.

### Approximate Layout

```text
                         +Z north

             [Emergency Hoist]
              x -6..8,z +32..+40
                    |
        [Cooling Regulator Lock]
          x -18..18,z +20..+32
          /                    \
 [Coal Cache Secret]       [Bulwark Hammer Bay]
 x -38..-22,z +12..+26    x +18..+40,z +8..+28
          \                    /
             [Furnace Row]
          x -34..34,z -2..+20
              |          |
      [Slag Side Loop] [Foundry Gantry]
      x -40..-18       x +18..+40
      z -20..+0        z -22..+4
              \          /
             [Arrival Hoist]
          x -8..10,z -36..-26
```

### Room List

| Room | Approximate Bounds | Function | Notes |
| --- | --- | --- | --- |
| Arrival Hoist | `x -8..10`, `z -36..-26` | Entry from Level03 | Strong furnace roar, clear first landmark. |
| Foundry Gantry | `x 18..40`, `z -22..4` | Elevated Lancer lane | Optional high route or visual bridge, not mandatory narrow combat. |
| Slag Side Loop | `x -40..-18`, `z -20..0` | Hazard preview and alternate path | Furnace heat pulses introduced with clear warning lamps. |
| Furnace Row | `x -34..34`, `z -2..20` | Main arena | Repeating furnace mouths, heat pulses, mixed combat. |
| Bulwark Hammer Bay | `x 18..40`, `z 8..28` | Heavy enemy focus | Wide space with big cover and clear windup silhouette. |
| Coal Cache Secret | `x -38..-22`, `z 12..26` | Optional resource reward | Current secret language can become richer production space. |
| Cooling Regulator Lock | `x -18..18`, `z 20..32` | Objective/control area | Can gate emergency hoist behind regulator or combat clear. |
| Emergency Hoist | `x -6..8`, `z 32..40` | Level transition | Large industrial hoist to Governor Core. |
| Quench Tank Alcove | `x -12..6`, `z -18..-8` candidate | Resource and story | Worker marks, health, cover from heat pulses. |
| Furnace Archive Nook | `x 2..16`, `z 6..14` candidate | Lore | Keep readable outside direct combat path. |

### Route Beats

1. Arrive below larger machines and immediately see furnace row glow.
2. Move through side loop or gantry while heat pulse tells are introduced.
3. Enter furnace row and learn alternating safe/unsafe floor or wall zones.
4. Fight mixed Scrapper/Lancer pressure around furnace cover.
5. Discover coal cache secret through slag-side visual clues.
6. Enter Bulwark Hammer Bay and read the heavy unit before it attacks.
7. Use scattergun or pistol burst to manage close pressure while avoiding hammer tells.
8. Activate or restore cooling regulator.
9. Emergency hoist powers green, heat pressure drops near exit.
10. Ride to Governor Core.

### Combat Encounters

- `E04-A Furnace Row Crossfire`
  - Position: Furnace Row.
  - Enemies: two Scrappers, one Lancer on gantry or far platform.
  - Goal: mix movement, cover, and heat timing.

- `E04-B Heat Pulse Pinch`
  - Position: Slag Side Loop.
  - Enemies: one light enemy with heat hazard nearby.
  - Goal: make hazard timing matter without creating a death trap.

- `E04-C Bulwark Reveal`
  - Position: Bulwark Hammer Bay.
  - Enemies: one Bulwark, optional Scrapper support.
  - Goal: clear heavy silhouette, readable windup, enough circle space.

- `E04-D Cooling Regulator Defense`
  - Position: Cooling Regulator Lock.
  - Trigger: regulator interaction or entering final control zone.
  - Enemies: one Lancer plus light melee pressure.
  - Goal: final pressure before hoist unlock.

### Secret Placement

- `Secret - Foundry Coal Cache`
  - Approximate location: `x -30`, `z 18`.
  - Access concept: coal chute wall panel behind a heat-safe alcove.
  - Visual clue: black coal dust footprints, cooler blue-gray quench pipe, worker chalk `COAL DOOR STICKS`.
  - Rewards: health, pressure cartridges, possibly higher ammo value for scattergun.
  - Secret should be reachable without crossing active lethal heat.

### Objective Locks

- Current route can allow the hoist after the main path.
- Production target should consider a cooling regulator objective:
  - Hoist is blocked by `heat surge unsafe`.
  - Cooling regulator lowers surge near exit and powers hoist.
- If the main code path does not support a new lock yet, preserve the room for later.

### Traversal

- Broad lanes are essential for Bulwark combat.
- Heat hazards should be avoidable by lateral movement, not tiny timing windows.
- Gantry height: `Y +2.5m`, with visible ramps and railings.
- Provide at least two routes into Furnace Row so the player can reposition.

### Signage And Readability

- Arrival sign: `FURNACE FOUNDRY`
- Hazard sign: `HEAT SURGE - WATCH PRESSURE LAMPS`
- Bulwark bay sign: `RESCUE FRAME MAINTENANCE`
- Regulator sign: `COOLING REGULATOR`
- Hoist sign: `EMERGENCY HOIST: GOVERNOR CORE`
- Use red-orange hazard language heavily, but keep green exit language sacred.

### Art Set Dressing

- Furnace mouths with animated glow cards, simple heat shimmer, and soot-black iron ribs.
- Hammer press silhouettes, chain hoists, slag gutters, quench tanks.
- Bulwark bay with broken rescue-frame cradles and oversized tool racks.
- Coal cache with stacked coal bins, stamped crates, spare cartridges, and worker markings.
- Cooling regulator: big wheel, pressure needles, water/steam pipes, green relief lamps.

### Performance And LOD Notes

- Use furnace row repetition to reduce unique geometry.
- Heat shimmer should be optional or quality-scaled.
- Large props can be static batched.
- Avoid too many dynamic lights. Use baked or emissive-style material cues where possible.
- Bulwark room should be visually rich but collision-simple.

### VR Comfort Notes

- Heat pulses need clear world-space warning at least `0.75s` before damage.
- No narrow ledge fighting on gantries.
- Heavy enemy windup should be readable at eye level.
- Avoid forcing the player to backpedal through active hazard zones.

### Independently Implementable Now

- Furnace row modular kit.
- Bulwark arena blockout variants.
- Heat hazard decal and warning-light set.
- Cooling regulator objective concept.
- Coal cache dressing set.
- Foundry signage and worker warning decals.

## Level 05 - Governor Core

### Production Target

- Purpose: deliver the current route finale, combine known mechanics, stage the Governor Warden, and make the Master Governor feel like a physical machine bureaucracy.
- Target footprint: `78m x 72m`.
- Target room count: 8 primary rooms plus optional lore alcoves.
- Expected completion time: 8 to 12 minutes.
- Combat density: high but controlled.
- Core feel: cathedral-machine core, pressure logic, stacked regulators, analog computation, final guardian.

### Approximate Layout

```text
                         +Z north

              [Master Override Hoist]
                x -8..8,z +30..+40
                       |
               [Governor Warden Arena]
                x -24..24,z +4..+30
             /            |             \
 [West Regulator Arm] [Core Ring] [East Regulator Arm]
 x -42..-20,z -8..+18 x -18..18,z -10..+8 x +20..+42,z -8..+18
             \            |             /
              [Pressure Chapel Approach]
                x -18..18,z -28..-10
                       |
                [Arrival Hoist]
                x -7..9,z -40..-30
```

### Room List

| Room | Approximate Bounds | Function | Notes |
| --- | --- | --- | --- |
| Arrival Hoist | `x -7..9`, `z -40..-30` | Entry from Level04 | Quiet before finale, strong forward orientation. |
| Pressure Chapel Approach | `x -18..18`, `z -28..-10` | Final prep | Ammo, health, archive plaque, view into core. |
| Core Ring | `x -18..18`, `z -10..8` | Central orientation landmark | Analog logic drums, punch plates, rotating governor arms. |
| West Regulator Arm | `x -42..-20`, `z -8..18` | Optional combat/resource route | Cover and side pressure hazards. |
| East Regulator Arm | `x 20..42`, `z -8..18` | Optional combat/resource route | Mirrored but not identical, good Lancer placement. |
| Governor Warden Arena | `x -24..24`, `z 4..30` | Boss fight | Circular but with strong north/south landmarks. |
| Master Override Hoist | `x -8..8`, `z 30..40` | Final win device | Locked until Warden defeat. |
| Archive Alcoves | side pockets | Optional lore | Keep outside boss path or lock them open before fight. |

### Route Beats

1. Arrive from foundry into a colder, more ordered machine space.
2. Pass through Pressure Chapel Approach with final resources and archive plaque.
3. See the Master Governor logic drums before seeing the Warden.
4. Enter Core Ring and clear a mixed pre-finale encounter.
5. Explore side regulator arms for resources or safer positions.
6. Trigger or approach the Governor Warden arena.
7. Fight Warden with known language: ranged bolt, stomp, enrage, shutdown burst.
8. Warden defeat unlocks the Master Override Hoist.
9. Player activates hoist/reset device and wins current route.

### Combat Encounters

- `E05-A Core Ring Mixed Machines`
  - Position: Core Ring.
  - Enemies: Scrapper, Lancer, optional Bulwark depending on balance.
  - Goal: final systems reminder before boss.

- `E05-B Regulator Arm Pressure`
  - Position: West/East Regulator Arms.
  - Enemies: one Lancer in one arm, one Scrapper/Bulwark pressure in the other.
  - Goal: optional resource risk and route control.

- `E05-C Governor Warden`
  - Position: Warden Arena, center `x 0`, `z 17`.
  - Boss language: pressure bolts, stomp, enraged half-health phase, shutdown burst.
  - Arena cover: 4 regulator pylons at diagonals, each wide enough to read but not enough to trivialize the fight.
  - Goal: boss reads as final guardian of a mechanical rule system, not a monster.

### Secret Placement

- Future candidate: `Secret - Governor Clerk Void`
  - Approximate location: behind west regulator arm, `x -38`, `z 10`.
  - Access concept: small archive maintenance void opened by interacting with a misfiled punch plate or hidden clerk lever.
  - Visual clue: neatly stamped labels that do not match the rest of the room, one green indicator where all others are amber.
  - Rewards: lore, high-value health/ammo, optional final score secret.
  - Keep it accessible before the boss or after boss defeat, not during boss combat.

### Objective Locks

- Master Override Hoist starts locked.
- Governor Warden defeat unlocks hoist.
- Future production target can add regulator arm disables before Warden vulnerability, but avoid overcomplicating the first release route.

### Traversal

- Boss arena should be mostly flat.
- Cover pylons should be large but spaced at least `8m` apart.
- Side regulator arms should loop back into the core, not dead-end during combat.
- No rail-less ledges or high-speed platform transitions.

### Signage And Readability

- Arrival sign: `GOVERNOR CORE`
- Core ring label: `MASTER GOVERNOR LOGIC BANK`
- Locked hoist sign: `MASTER OVERRIDE SEALED - WARDEN ACTIVE`
- Regulator signs: `DISTRICT HEAT`, `LIFT PRESSURE`, `WATER PUMP`, `CLOCKTOWER DRIVE`
- Warden warning: `CONTAINMENT FRAME ONLINE`

### Art Set Dressing

- Cathedral-scale governor drums, brass logic cylinders, punch-card belts, mercury-style glass tubes, pressure needles.
- Warden arena with four regulator pylons and visible master override hoist.
- Red/orange hostile pressure lines feeding the Warden, green relief lines after shutdown.
- Archive alcoves with clerk desks, stamped orders, failed worker notes, and analog calculation boards.
- Final hoist should be visually distinct from normal service lifts: master reset plate, green pressure glass, heavy chain frame.

### Performance And LOD Notes

- Use large repeating core panels and rotating hero machines sparingly.
- Keep boss arena particles controlled; Warden VFX should have quality tiers.
- Regulator arms can occlude side detail from the central arena.
- Boss cover pylons can be static batched with simple collision hulls.
- Avoid too many transparent glass tubes in one view on lower PCs.

### VR Comfort Notes

- Boss arena should preserve stable horizon and avoid camera-control effects.
- Boss attacks need strong body tells and ground indicators, not sudden close-range surprises.
- Keep Warden centered for initial reveal so the player is not forced into an immediate 180-degree turn.
- Final hoist interaction should be at comfortable height with a clear pause after the fight.

### Independently Implementable Now

- Warden arena cover layout variants.
- Governor Core signage set.
- Regulator pylon kit.
- Final hoist/reset-device concept.
- Boss arena readability checklist.
- Optional Clerk Void secret concept.

## Cross-Level Production Integration

### Objective Progression

| Level | Main Lock | Unlock Action | Exit |
| --- | --- | --- | --- |
| Level01 | Pressure Gate | Collect Gear Key | Service Lift to Pipeworks |
| Level02 | Boilerheart Lift | Route Pipeworks Pressure Valve | Lift to Boilerheart |
| Level03 | Foundry Lift | Vent Boilerheart Pressure Valve | Lift to Foundry |
| Level04 | Emergency Hoist candidate lock | Restore Cooling Regulator or clear route | Hoist to Governor Core |
| Level05 | Master Override Hoist | Defeat Governor Warden | Win route |

### Weapon And Enemy Learning Curve

| Level | Primary Weapon/Tool Learning | Enemy Learning |
| --- | --- | --- |
| Level01 | Pressure Pistol, key/gate/lift | Scrapper melee basics |
| Level02 | Ammo discipline, cover against range | Lancer first-read and projectile avoidance |
| Level03 | Steam Scattergun, hazard shutdown | Bellows Node support priority |
| Level04 | Close-range weapon value, hazard timing | Bulwark heavy windup and mixed pressure |
| Level05 | Full weapon kit and target priority | Warden boss pattern plus mixed machines |

### Secret Language

Shared clues:

- Worker chalk marks.
- Unusual green steam leaks.
- Misaligned rivet or wall-plate patterns.
- Cooler pipe color in hot areas.
- Enamel labels that do not match the main route.
- Slightly brighter brass seams on interactable hidden panels.

Secret rule: each secret should be optional, reachable without advanced movement, and readable after discovery as fair rather than random.

### Signage Kit

Build a reusable signage library:

- Directional arrows: Intake, Pipeworks, Boilerheart, Foundry, Governor.
- Lock warnings: pressure locked, gear seal, overheat, Warden active.
- Safety stamps: keep clear, valve crew only, surge hazard, lift pressure.
- Worker marks: chalk arrows, emergency notes, hidden cache marks.
- Exit plates: green service-lift and hoist language.

### Art Kit Dependencies

Minimum modular kit useful across all levels:

- Riveted wall panels: soot brick, iron plate, copper pipe wall, furnace wall.
- Floors: oil stone, grated iron, furnace tile, governor brass inlay.
- Doors/gates: pressure gate, service hatch, lift door, hoist gate.
- Pipes: straight, elbow, junction, valve wheel, leaking vent, pressure gauge.
- Machinery: flywheel, belt drive, boiler cylinder, regulator pylon, pump stack.
- Props: workbench, crate, cartridge box, coal bin, tool rack, archive plaque.
- Decals: soot, oil spill, chalk mark, warning stamp, rivet seam, heat scorch.

### Audio And VFX Hooks

Future implementation should reserve hooks for:

- Room ambience layers by level: intake hum, pipe drip, boiler roar, furnace surge, governor clockwork.
- Objective activation: key pickup, valve turn, pressure route, regulator restore, hoist unlock.
- Hazard states: steam puff low/high, furnace pulse warning/active/safe.
- Secret discovery: subtle pressure tick, brass latch, quiet success chime.
- Enemy roles: Scrapper cutter windup, Lancer charge, Bulwark hammer, Bellows pulse, Warden phase.

### Performance Budgets For Mid/Low Windows PC

Draft targets before profiling:

- 60 FPS at 1080p on mid/low gaming PC.
- Keep each level under roughly `250k` to `500k` visible triangles in worst-case hero view for early production, then profile.
- Prefer static batching or GPU instancing for repeated pipes, rivets, wall panels, and props.
- Limit dynamic shadow-casting lights to a small number per level.
- Use material atlases for modular environment pieces.
- Quality scale steam, sparks, and heat shimmer.
- Keep collision meshes simple and separate from visual meshes.

### Android And WebGL Downshift Notes

- Replace dense pipe geometry with baked pipe-wall panels.
- Reduce transparent particles and overdraw.
- Lower texture resolution for wall/floor kits.
- Simplify Warden and Bulwark VFX.
- Reduce simultaneous enemy count before reducing level readability.
- Use static lighting or simple emissive materials where possible.
- Keep route layouts intact even when dressing is simplified.

### VR Forward Compatibility

- Keep interactions chest-height and front-facing when possible.
- Preserve no-jump route design.
- Avoid narrow mandatory catwalk combat.
- Avoid forced fast turns or behind-player ambushes.
- Design boss and heavy attacks with body tells plus ground/world indicators.
- Keep pickup and objective objects large enough to read in headset.
- Leave room near doors/lifts for comfort fade transitions and spawn orientation.

## Future Levels And Expansions

These are optional expansions beyond the current five-level route. They fit the existing lore without requiring the first release to include them.

### Expansion Level A - Gauge Hall

- Placement: between Pipeworks Annex and Boilerheart Core, or as an optional branch from Boilerheart.
- Purpose: deepen the analog bureaucracy theme and introduce more puzzle-like pressure routing.
- Target footprint: `70m x 54m`.
- Core rooms: clerk archive, gauge atrium, punch-plate sorter, calibration galleries, sealed worker refuge.
- Mechanics: multi-valve lock sequence, optional non-combat lore room, first Boiler Tick scout if that enemy is added.
- Story value: shows the Locked Shift learned to lie to the Master Governor by miscalibrating sensors.
- Independent planning now: signage, gauge props, punch-plate logic art, refuge storytelling.

### Expansion Level B - Clocktower Drive

- Placement: side route after Furnace Foundry or post-v1 expansion.
- Purpose: high-motion mechanical spectacle without breaking FPS readability.
- Target footprint: `82m x 58m`.
- Core rooms: gear observatory, pendulum maintenance bridges, escapement chamber, clocktower pressure relay.
- Mechanics: moving machinery hazards, timed safe lanes, Lancer elevated pressure.
- Story value: reveals the city above is still running on the trapped workers' machinery.
- VR caution: moving machinery must not move the player camera or force narrow timed jumps.
- Independent planning now: gear modules, animated hazard concepts, parallax machinery backdrops.

### Expansion Level C - The Locked Shift Refuge

- Placement: optional hidden level reachable from a secret in Gauge Hall or Boilerheart.
- Purpose: environmental story, resource risk/reward, slower tension between combat levels.
- Target footprint: `50m x 42m`.
- Core rooms: hidden bunk room, improvised infirmary, chalk map wall, sabotage workshop, sealed escape grate.
- Mechanics: low combat, secret chaining, lore plaques, resource cache, possible survivor audio logs later.
- Story value: humanizes the disaster without long cutscenes.
- Independent planning now: props, notes, chalk language, resource economy impact.

## Implementation Handoff Checklist

Before an implementation agent converts any section into Unity scenes or prefabs, it should verify:

- Target scene and version slice are assigned.
- Existing scenes are not overwritten without checking current diffs.
- Level bounds and room names match the current route.
- Required systems exist for any new lock, secret trigger, or enemy role.
- Validation expectations are added with the scene changes.
- Full matrix runs only after a complete versioned slice, unless the main agent chooses otherwise.

## Top Integration Opportunities

1. Use the Level03 production plan to turn the current scattergun and Bellows Node prototypes into a cohesive Boilerheart loop.
2. Add a reusable signage/decal kit before expanding maps, because it improves route readability across every level.
3. Build modular prop kits by biome: intake, pipeworks, boilerheart, foundry, governor. These can run in parallel with gameplay systems.
4. Convert Level04 into the first true hazard-combat showcase by pairing heat pulses with Bulwark staging.
5. Treat Level05 as a boss-readability map first: arena landmarks, Warden tells, cover spacing, and final hoist feedback should lead the art pass.
