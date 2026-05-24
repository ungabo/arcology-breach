# Brassworks Breach - v0.1.36 Narrative + Encounter Beat Sheet

Created: `2026-05-24`

Owned scope:

- `Documentation/Planning/V0_1_36_NarrativeEncounterBundle/`
- `Documentation/AssetProduction/V0_1_36_NarrativeEncounterBundle/`

## Purpose

This bundle translates the existing steampunk lore into practical in-level beats for later scene integration. It is intentionally docs-only and non-authoritative: it does not change current route locks, transition triggers, enemy ownership, hazards, validators, build settings, or `v0.1.35` work.

Design target:

- Make the five-level route feel like a coherent breach into a failing municipal machine.
- Use short objective signage, physical props, and ambient storytelling instead of long exposition.
- Give enemy introductions and setpieces a clear narrative reason without adding cutscenes.
- Preserve the current proven route authority: Level01 key/gate/lift, Level02 routing valve/lift, Level03 Boilerheart valve/lift, Level04 emergency hoist, Level05 Warden/final hoist.

Color and objective language:

- Amber: attention, objective, inspect, pressure still rerouting.
- Red: locked, hostile, unsafe, pressure fault.
- Green: restored, safe, exit, lift/hoist ready.
- Route nouns stay consistent: `Gear Key`, `Pressure Gate`, `Service Lift`, `Valve Wheel`, `Boiler Seal`, `Governor Plate`, `Master Override Hoist`.

## Five-Level Arc

| Level | Story question | Gameplay promise | New narrative affordance |
| --- | --- | --- | --- |
| Level01 - Brassworks Intake | Why did the works seal? | Learn key/gate/lift while meeting the first machine threat. | The Locked Shift tried to mark a rescue path before being driven deeper. |
| Level02 - Pipeworks Annex | Why are machines firing? | Add ranged pressure and route-state valve logic. | Message pipes and bolt feeds were repurposed into enforcement lanes. |
| Level03 - Boilerheart Core | Can the pressure lock be broken? | Add linked hazard shutdown, Bellows Node, and Scattergun pickup. | Workers survived by sabotaging sensors and hiding near heat/steam voids. |
| Level04 - Furnace Foundry | What did the Governor build next? | Escalate heat lanes, Bulwark pressure, and emergency escape. | Rescue frames were reissued as containment machines. |
| Level05 - Governor Core | What protects the bad order? | Resolve with Warden defeat and final override hoist. | The Master Governor is not evil; it is enforcing a broken classification forever. |

## Level01 - Brassworks Intake

Narrative intent:

- The player breaches a sealed service intake and sees the first evidence that humans were trapped below while maintenance machines turned hostile.
- The level should teach that every route rule has a physical object: key, gate, archive, lift.

Room-by-room beats:

| Room | Story affordance | Enemy / pacing beat | Objective beat | Secrets and rewards | Signage / lore plate | Setpiece moment |
| --- | --- | --- | --- | --- | --- | --- |
| Soot-brick service entry | Fresh breach marks, blown soot, city-side work order crate, torn pressure-runner contract stub. | Safe arrival, no enemy for first read. | Spawn briefing: `Enter the intake and find a way through the sealed works.` | None. | Wall stencil: `CAIRNWICK MUNICIPAL HEAT - SERVICE INTAKE 7`. | Steam hisses shut behind player to imply one-way breach without adding a route lock. |
| Copper-pipe maintenance throat | Chalk arrows from the Locked Shift point toward a side repair bay, then stop under claw marks. | First safe silhouette of Scrapper beyond pipe ribs before active chase. | HUD can remain movement/combat focused. | Small ammo/health readable from main path if existing placement supports it. | Warning tag: `Contaminant Protocol Active - Keep Hands Clear`. | Pipe shadow reveals Scrapper cutter-arm motion before it rounds the corner. |
| Clockwork repair bay | Benches, hanging tools, inert automaton shells, stamped repair cards. | First Scrapper fight. Keep it short and readable. | Objective after combat: `Locate the Gear Key.` | Optional bench drawer or side alcove can foreshadow first secret language. | Lore plate candidate: `Intake Archive` existing plaque text. | Dead machine on bench sparks when Scrapper dies, reinforcing machine shutdown language. |
| Gear-key plinth | Gear key sits in a brass yoke with a pressure-gauge lock diagram. | One light ambush or post-pickup Scrapper pressure only if current route budget allows. | Key pickup: `Gear Key acquired. Return to the Pressure Gate.` | None required. | Plinth label: `Zone Gear - Intake Gate Only`. | Plinth vents amber, then green, making lock/key causality obvious. |
| Pressure gate return | Gate was visible before key route; workers tried to wedge it open with tools. | Optional final Scrapper from side throat after unlock. | Gate feedback: `Pressure Gate open. Reach the Service Lift.` | Secret pressure cache clue can sit off the return route, not in the gate footprint. | Door plate: `PRESSURE GATE A1 - GEAR AUTHORITY REQUIRED`. | Gate gears rotate, red lens flips green, worker wedge falls loose. |
| Furnace control room | First hint the works power the city above: heat schedule board, pressure quotas, council seal. | Low pressure cleanup fight or no fight depending current pacing. | Reinforce exit direction with green lift cues. | `Secret - Intake Pressure Cache` uses maintenance ration boxes and hidden chalk mark. | Chalk note: `If you can still read this, do not trust the green lamps unless the gauges agree.` | Furnace gauge spikes, then stabilizes when lift powers. |
| Service lift | Lift cage, pulley, green restored-pressure lamp, emergency capacity tag. | No new combat inside lift boarding footprint. | Exit beat: `Ride the Service Lift to the Pipeworks.` | None. | Lift sign: `DOWN: PIPEWORKS ANNEX / BOILERHEART FEED`. | Lift call lever pulls with heavy pneumatic thump before transition cue. |

Enemy introduction rule:

- Scrapper should read as a maintenance machine first: cutter arm, tool clamp, piston legs, service number, then hostile behavior.

## Level02 - Pipeworks Annex

Narrative intent:

- The Annex explains how civil infrastructure became weaponized. Message tubes, bolt feeds, and valve-rifle service frames now enforce the Governor's shutdown order.
- Lancers should feel like repurposed pipe inspectors or line-clearance machines, not generic soldiers.

Room-by-room beats:

| Room | Story affordance | Enemy / pacing beat | Objective beat | Secrets and rewards | Signage / lore plate | Setpiece moment |
| --- | --- | --- | --- | --- | --- | --- |
| Narrow pipeworks entry | Dense pipe bundles, condensate drip, service maps with crossed-out rescue routes. | One Scrapper or distant machine audio only. | Spawn briefing: `Reroute pipe pressure before the Boilerheart lift will answer.` | None. | Wall band: `PIPEWORKS ANNEX - MESSAGE / BOLT / CONDENSATE`. | A message capsule rattles through a tube, then jams under red light. |
| Baffle corridor | Alternating pipe baffles create controlled sightlines. | First Lancer silhouette at distance before direct fire. | Teach ranged pressure by sightline, not text. | Ammo pickup visible after crossing the lane. | Floor stencil: `KEEP CLEAR OF RIVET FEED`. | First pressure bolt punches a pipe clamp, venting harmless steam near cover. |
| Scrapper/Lancer lane | Bolt-feed racks, valve-rifle maintenance cradle, pressure target board. | Mixed melee/ranged pressure: Scrapper pushes, Lancer anchors. | Objective still points toward routing valve. | None required. | Lore plate candidate: `Pipeworks Archive` existing plaque text. | Lancer fires through a narrow pipe gap; player learns to break line of sight. |
| Routing valve gallery | Large Valve Wheel, pressure diagram with red-to-green route path. | Defensible objective pause after fight; avoid enemy inside prompt radius. | Valve interaction: `Pipe pressure routed. Boilerheart lift restored.` | Optional micro-cache behind pipe rack, subordinate to existing cartridge-cache secret. | Valve label: `BOILERHEART FEED - MANUAL ROUTING`. | Gauge banks snap from red to amber to green in sequence. |
| Cartridge-cache secret | Hidden maintenance rack behind misaligned pipe panel, worker ration sign. | No required combat in secret space. | Discovery feedback should be distinct from pickup feedback. | `Secret - Pipeworks Cartridge Cache`: cartridges, health, note. | Chalk mark: `Three short knocks. Then pull the cold pipe.` | Secret opens as a tiny service panel, not a full route branch. |
| Locked lift lobby | Lift was initially red/locked, now green after valve. | One short return pressure beat if route budget allows; no spawn blocking lift. | Exit beat: `Ride the Service Lift to the Boilerheart.` | None. | Lift placard: `BOILERHEART CORE - AUTHORIZED PRESSURE ONLY`. | Lift chains tense after valve completion, giving cause/effect across rooms. |

Enemy introduction rule:

- Lancer's first shot should have a visible pressure charge and projectile trail. The room geometry should give the player one obvious answer: step behind baffle/pipe cover.

## Level03 - Boilerheart Core

Narrative intent:

- The player reaches the machine heart that can still break pressure locks if vented by hand.
- This is the first level where human survival tactics should be plainly visible: hidden notes, sabotaged sensors, maintenance voids, and weapon caches.

Room-by-room beats:

| Room | Story affordance | Enemy / pacing beat | Objective beat | Secrets and rewards | Signage / lore plate | Setpiece moment |
| --- | --- | --- | --- | --- | --- | --- |
| Arrival floor | Heat shimmer, floor drains, pressure map showing Foundry route as sealed. | Safe read, then light Scrapper pressure from side. | Spawn briefing: `Vent the Boilerheart valve to unlock the Foundry lift.` | Health/ammo staging before hazard chamber. | Entry sign: `BOILERHEART CORE - DO NOT MANUALLY VENT UNDER LOAD`. | A sealed Foundry lift indicator blinks red in view early. |
| Scattergun display alcove | Locked Shift improvised weapon stand, shell rack, brass nameplate. | Pickup before heavier pressure; no enemy overlaps acquisition burst. | Capability beat: `Steam Scattergun acquired.` | Shell rack nearby reinforces ammo identity. | Nameplate: `Steam Scattergun - Breach Pattern`. | Pickup stand vents a pressure ring and leaves empty yoke after collection. |
| Furnace-core chamber | Giant boiler mouth, paired steam leaks, gauge banks, worker chalk warnings. | Scrapper/Lancer combination around steam hazards. | Objective still points to valve; hazards teach timing. | Optional small resource nook behind safe side of steam cycle. | Lore plate candidate: `Boilerheart Archive` existing plaque text. | Steam hazards visibly share pipe feeds with the valve route. |
| Bellows Node perimeter | Stationary support machine bolted to floor/wall, pulse ring marks. | First Bellows Node support setpiece: pulse damage/boost plus nearby Scrapper pressure. | Player understands it as a machine to shut down or avoid, depending current mechanics. | None required. | Warning plate: `BELLOWS AMPLIFIER - STAND OUTSIDE PULSE RING`. | Pulse overdrives nearby Scrapper with visible pressure flare. |
| Pressure valve catwalk | Valve Wheel, catwalk rail, manual override diagram, gloves abandoned on railing. | Brief calm before interaction; keep prompt clear. | Valve interaction: `Boilerheart vented. Steam hazards falling. Foundry lift restored.` | None. | Valve tag: `MANUAL VENT BREAKS BOILER SEAL`. | Turning valve should audibly lower the whole room's pressure, then linked steam hazards die back. |
| Foundry lift lobby | Red lock becomes green, hoist line warms up, Foundry warning glow visible beyond. | Optional final pressure from remaining enemy, not inside lift. | Exit beat: `Ride the Service Lift to the Furnace Foundry.` | None. | Lift placard: `FOUNDRY ROW - HEAT AUTHORITY REQUIRED`. | Red Foundry seal cracks open as green pressure vents across lintel. |

Enemy introduction rule:

- Bellows Node must read as a stationary machine with a danger radius. Its pulse ring and sound should be readable before the first damaging pulse when feasible.

## Level04 - Furnace Foundry

Narrative intent:

- The foundry reveals the Governor has not only repurposed machines; it is using rescue and construction frames as containment units.
- The player should feel industrial scale: pouring lanes, furnace surge cycles, heavier silhouettes, emergency escape.

Room-by-room beats:

| Room | Story affordance | Enemy / pacing beat | Objective beat | Secrets and rewards | Signage / lore plate | Setpiece moment |
| --- | --- | --- | --- | --- | --- | --- |
| Arrival floor | Foundry horn, heat warning, soot silhouettes of workers who fled toward coal storage. | Brief setup with distant Bulwark footfalls or hammer clangs. | Spawn briefing: `Cross the Furnace Foundry and reach the emergency hoist.` | Health/ammo before hazard lane. | Sign: `FURNACE FOUNDRY - RESCUE FRAMES STORED NORTH`. | Emergency shutters slam behind, framing forward motion without new route logic. |
| Furnace baffle lane | Heat-surge strips, casting leaks, red warning lamps. | Lancer anchors across hazard timing lane; Scrapper pressures from safe side. | Teach heat-cycle timing with route lighting. | None required. | Floor warning: `RED LANE ACTIVE ON PRESSURE SURGE`. | Furnace heat ripple sweeps the lane before enemies commit. |
| Mixed foundry floor | Overhead gantry, hammer press, broken rescue frame parts. | First Bulwark heavy encounter, supported by lighter enemies. | Objective remains exit-focused; do not add a new lock. | Ammo reward after Bulwark route pressure. | Lore plate candidate: `Foundry Archive` existing plaque text. | Bulwark powers up from a rescue-frame cradle, hammer arms unlocking under red light. |
| Coal-cache secret | Coal bin, hidden worker air pocket, ration mark. | Optional quiet reward nook outside main pressure. | Discovery feedback only; no new route requirement. | `Secret - Foundry Coal Cache`: health/ammo and worker note. | Chalk note: `Coal dust hides breath from the sensors.` | Coal chute panel shifts aside with falling dust puff. |
| Emergency hoist approach | Green hoist signal partly obscured by furnace smoke, pulley gears, escape placard. | Final mixed cleanup, keep hoist path at least current route width. | Exit beat: `Ride the Emergency Hoist to the Governor Core.` | None. | Hoist plate: `EMERGENCY ASCENT / GOVERNOR CORE SERVICE RING`. | Hoist counterweights begin moving while nearby foundry machines continue failing. |

Enemy introduction rule:

- Bulwark should be staged as a failed rescue tool: furnace-plated body, hammer arms, containment stamp, slow readable windup. Its first encounter should sell weight, not surprise speed.

## Level05 - Governor Core

Narrative intent:

- The finale makes the Master Governor feel like a cathedral-sized analog bureaucracy: gears, stamps, plates, pressure drums, and bad rules made physical.
- The Warden is the last enforcement machine protecting a stalled order, not a villain monologue.

Room-by-room beats:

| Room | Story affordance | Enemy / pacing beat | Objective beat | Secrets and rewards | Signage / lore plate | Setpiece moment |
| --- | --- | --- | --- | --- | --- | --- |
| Core arrival / pressure chapel | Tall gear banks, stamped rejection plates, worker petitions pinned under glass. | Safe read with distant Warden silhouette or core heartbeat. | Spawn briefing: `Reach the Master Override. Defeat the Warden blocking the hoist.` | Final health/ammo staging before ring. | Entry sign: `MASTER GOVERNOR - CLASSIFICATION ENGINE`. | A giant punch plate repeats `CONTAMINANT` as the player enters. |
| Regulator ring | Circular pressure route, steam leak, furnace surge, east/west regulator arms. | Mixed Scrapper/Lancer/Bulwark pressure with hazards, but sightlines must stay readable. | Objective points toward Warden/final lock, not side lore. | Optional final supplies on side arms. | Lore plate candidate: `Governor Archive` existing plaque text. | Red and green route lights fight each other around the ring, showing conflicting machine state. |
| Warden arena | Master Governor crown, feed pipes into Warden, boss lock lamps, broad dodge space. | Governor Warden reveal, stomp/bolt/enrage/shutdown. | Boss HUD appears only during fight; final hoist stays locked until defeat. | Ammo/health placement supports fight without turning into clutter. | Floor seal: `NO ORDER SUPERSEDES CONTAINMENT`. | Warden assembles pressure from multiple service frames, then detaches from feed pipes. |
| Master override hoist | Final hoist, manual reset plate, green lamps after Warden shutdown. | No new enemy after Warden unless already route-owned. | Warden defeat: `Master Override unlocked. Ride the hoist.` Win interaction remains current authority. | Secret total can display at win as already supported. | Override plate: `VENT NETWORK / RELEASE LOWER WORKS / REPORT GOVERNOR FAULT`. | Warden shutdown sends pressure back through the lock lamps, red to green, opening final hoist authority. |

Enemy introduction rule:

- Warden's parts should echo previous enemies: Scrapper cutter/service limbs, Lancer pressure cannon, Bulwark furnace body, Bellows pressure crown. This makes the campaign read as escalation rather than a new genre.

## Objective Signage Set

Use short readable labels. These are candidates for plaques, decals, prop labels, or HUD strings during later integration.

| ID | Level | Placement | Text |
| --- | --- | --- | --- |
| NES-SIGN-01 | Level01 | Service entry wall | `SERVICE INTAKE 7 - AUTHORIZED RUNNERS ONLY` |
| NES-SIGN-02 | Level01 | Gear-key plinth | `INTAKE GEAR - GATE A1` |
| NES-SIGN-03 | Level01 | Pressure gate | `GEAR AUTHORITY REQUIRED` |
| NES-SIGN-04 | Level02 | Pipeworks entry | `PIPEWORKS ANNEX - BOLT FEED LIVE` |
| NES-SIGN-05 | Level02 | Routing valve | `ROUTE BOILERHEART PRESSURE` |
| NES-SIGN-06 | Level03 | Scattergun stand | `STEAM SCATTERGUN - BREACH PATTERN` |
| NES-SIGN-07 | Level03 | Bellows Node perimeter | `BELLOWS AMPLIFIER - PULSE RADIUS` |
| NES-SIGN-08 | Level03 | Boilerheart valve | `MANUAL VENT BREAKS BOILER SEAL` |
| NES-SIGN-09 | Level04 | Furnace lane | `RED LANE ACTIVE ON PRESSURE SURGE` |
| NES-SIGN-10 | Level04 | Bulwark cradle | `RESCUE FRAME REISSUED FOR CONTAINMENT` |
| NES-SIGN-11 | Level05 | Core entry | `MASTER GOVERNOR - CLASSIFICATION ENGINE` |
| NES-SIGN-12 | Level05 | Final hoist | `MASTER OVERRIDE - VENT AND RELEASE` |

## Secret Language

Secrets should feel like worker survival craft, not arcade bonus rooms.

- Chalk marks should be small, repeatable, and lower priority than objective signage.
- Secret entrances should use service panels, coal chutes, pipe racks, loose grates, and ration caches.
- Discovery confirmation should be distinct from pickup confirmation, preferably a softer brass chime plus dust/steam release.
- Secrets should never use green exit language unless they genuinely return to the main route.

## Setpiece Scripting Hooks For Later Integration

These are naming candidates only. They are not new code requirements.

| Hook candidate | Trigger source | Presentation response |
| --- | --- | --- |
| `NES_Intake_BreachSeal` | Level01 spawn / entry volume | Rear steam hiss, breach lamp flicker, intake tone. |
| `NES_GearKey_PlinthRelease` | Gear Key pickup | Amber plinth vents, key yoke opens, short pressure ping. |
| `NES_PressureGate_StateGreen` | Existing gate unlock/open authority | Red lens flips green, gear clack, pressure mist. |
| `NES_Pipeworks_MessageJam` | Level02 entry read zone | Message capsule rattles and jams in tube. |
| `NES_RoutingValve_PressureReroute` | Existing valve completion | Gauge bank sequence, pipe knocks, lift wake cue. |
| `NES_Boilerheart_HazardLinkedRead` | Level03 hazard approach | Steam feed lights trace from hazards to valve. |
| `NES_Boilerheart_VentDrop` | Existing Boilerheart valve completion | Room pressure tone lowers, hazards visibly fall off. |
| `NES_Bellows_PulseWarn` | Bellows Node active pulse windup | Floor/ring glow, intake wheeze, pressure crown spin. |
| `NES_Foundry_BulwarkWake` | Bulwark encounter activation | Cradle clamps release, hammer arms unlock. |
| `NES_Warden_LockState` | Existing Warden lock / defeat authority | Boss lock lamps red before fight, green after shutdown. |

## Non-Goals

- No new mandatory objectives.
- No new campaign route authority.
- No new secrets beyond the currently expected secret pattern unless a later owner explicitly expands route validation.
- No scene edits, prefab edits, script edits, validators, build settings, or shared status-file changes from this bundle.
- No long lore text during combat.
