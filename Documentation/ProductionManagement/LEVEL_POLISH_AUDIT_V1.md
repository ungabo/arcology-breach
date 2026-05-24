# Brassworks Breach - V1 Level Polish Audit

Scope: production planning and documentation only.

Source snapshot:
- `Documentation/LEVEL_DESIGN_AND_MAPS.md`
- `Documentation/PARALLEL_LEVEL_PRODUCTION_MAPS.md`
- `Documentation/PROJECT_SPEC.md`
- `Documentation/VERSION_MICRO_ROADMAP.md`
- Read-only reference from `Assets/_Project/Editor/V0SceneBuilder.cs` and `Assets/_Project/Editor/V0LevelValidator.cs`

Collaboration guardrail: this audit does not authorize edits to active Unity scenes, generated scene builders, gameplay scripts, asset staging, concept renders, status docs, or roadmap files. Anscombe's Bulwark gameplay lane and Darwin's pressure-pistol proof render lane should remain isolated from this planning lane.

## Executive Read

The five-level route is functionally strong for a generated prototype: each level loads in order, objective locks are validated, core combat roles exist, secrets persist, hazards are covered by smoke tests, and the finale is guardian-gated. The V1 risk is not basic functionality. The risk is that Levels02-05 are still narrow generated corridors carrying production-scale mechanics, so several beats read as "systems in a line" rather than readable FPS spaces with landmark loops, fair secrets, and staged escalation.

Primary V1 polish themes:

- Expand the spaces around mechanics before adding more mechanics.
- Preserve the existing objective chain: Level01 gear key, Level02 routing valve, Level03 Boilerheart valve, Level04 hoist route, Level05 Warden lock.
- Make each level's central landmark visible from at least two rooms.
- Keep secrets optional and fair, with clue language that does not look like mandatory objective signage.
- Separate first-read enemy teaching from hazard pressure when possible.
- Treat Level04 and Level05 as main integration work because they touch active heavy-enemy and boss pacing.

## Cross-Level Audit

### Navigation

Current generated scenes validate the route but compress later levels:

- Level01 already has a start room, first combat room, side key branch, pressure gate, and final lift room.
- Level02 is about `12m x 26m`, while the production target is about `70m x 45m`.
- Level03 is about `13m x 28m`, while the production target is about `65m x 55m`.
- Level04 is about `14m x 32m`, while the production target is about `80m x 60m`.
- Level05 is about `15m x 32m`, while the production target is about `75m x 75m`.

V1 need: expand Levels02-05 into looped spaces before adding optional objectives or more enemy count. The current linear footprint makes combat, hazards, and objective devices compete for the same sightline.

### Combat Pacing

The campaign learning curve is sound:

1. Level01: Scrapper and key/gate basics.
2. Level02: first Lancer read.
3. Level03: scattergun, steam hazard, Bellows Node support role.
4. Level04: Bulwark plus mixed hazard pressure.
5. Level05: mixed enemies plus Governor Warden.

V1 need: make each new combat role get one clean teaching space before it is combined with other pressures. Level03 and Level04 are the most compressed; the scattergun, Bellows Node, steam, heat, Lancer, and Bulwark beats need room to breathe.

### Landmarking

Strong landmark candidates already exist:

- Level01: pressure gate, gear-key plinth, service lift.
- Level02: routing valve, pipe baffles, Boilerheart lift.
- Level03: Boilerheart furnace core, scattergun display, pressure valve, foundry lift.
- Level04: furnace row, Bulwark bay, emergency hoist.
- Level05: master governor regulator, Warden arena, master override hoist.

V1 need: turn candidates into route anchors. Each level should let the player regain orientation by seeing a large machine, not by reading only HUD text or a floating label.

### Objective Clarity

The persistent objective HUD, world labels, lock messages, and auto-playthrough coverage are valuable. The main issue is spatial cause and effect: several locks are functionally clear but not yet spatially framed.

V1 need: show locks before unlocks when possible:

- Level01: see pressure gate before committing to key branch.
- Level02: see locked Boilerheart lift before or while learning where the routing valve is.
- Level03: see foundry lift and steam pressure relationship before venting the valve.
- Level05: see master override hoist locked by Warden before or during the Warden reveal.

### Secrets

Current registered secrets: Level01 Intake Pressure Cache, Level02 Pipeworks Cartridge Cache, Level04 Foundry Coal Cache. Run stats persist and final win can report totals.

V1 need: improve fairness and clue language before adding many more secrets. Level03 and Level05 can support future secrets, but the priority is making the three current secrets feel discovered, not merely placed.

### Hazard Readability

Steam and furnace hazards have VFX and validation. The risk is encounter context: in tight rooms, a readable hazard can still feel unfair if the player has no safe lateral route.

V1 need:

- Pre-hazard preview space before damage.
- At least one safe lane around every damaging hazard.
- Audio and world-space warning that works without screen shake.
- Post-objective state changes, especially Level03 hazard shutdown after venting.

### Performance Risk

The production maps want dense pipes, furnace glow, rotating machinery, steam, sparks, heat shimmer, Warden VFX, and more enemies. These are exactly the things that can turn a compact FPS map muddy or expensive.

V1 need: define per-level hero views and budgets before art dressing expands. Static batching, prop clusters, simple collision, and quality-scaled particles should be assumed in every scene slice.

## Level01 - Brassworks Intake

V1 role: tutorial map for movement, Pressure Pistol basics, Scrapper pressure, gear key, pressure gate, first secret language, and service lift.

Current baseline:
- Generated blockout is the most complete route shape in the current campaign.
- Current floor is roughly `36m x 46m`, with start, corridor, first combat room, side key room, locked gate corridor, final room, and service lift.
- Current enemies: four Scrappers across first room, key room, and final room.
- Current resources: health, pressure cartridges, plus secret health/ammo.
- Current objective: gear key opens pressure gate, service lift transitions to Level02.

### Navigation

Verdict: mostly viable for V1 with focused polish.

The current route already teaches a side branch and returns the player to the locked path. The V1 issue is not scale as much as spatial read: the player should see the pressure gate as the next goal before the key route pulls them east. The side branch should feel like a workroom loop, not a separate dead-end.

V1 actions:
- Strengthen the first view from the repair bay into the locked gate corridor.
- Make the key room return path visibly reconnect to the pressure gate.
- Keep all passages at least `3m` wide except optional service recesses.
- Keep collision simple around repair-bay cover so Scrapper movement stays clean.

### Combat Pacing

Verdict: good tutorial density, but final room should not feel like a duplicate of the first room.

The four Scrapper setup is acceptable for a first map. The first Scrapper should remain a clean face-to-face tutorial. The key-room Scrapper can be a light objective pickup response. The final room should add one environmental wrinkle, not just two enemies in another box.

V1 actions:
- Keep the first Scrapper in front of the player, with no behind-player ambush.
- Stage the key-room Scrapper as a dormant machine waking near the plinth.
- Convert the final encounter into one Scrapper plus route pressure, or two Scrappers with stronger cover spacing and a clear fallback.
- Avoid introducing lethal hazards in Level01.

### Landmarking

Verdict: strong candidates, needs stronger first-viewport identity.

The pressure gate, gear-key plinth, and green service lift are solid. The arrival space should immediately communicate "Brassworks Intake" with soot brick, copper manifolds, and service-entry signage, not only greybox walls.

V1 actions:
- Make the pressure gate the largest object visible from the repair bay.
- Give the gear-key plinth amber lighting and a distinct silhouette.
- Give the service lift a green pulley/pressure identity that cannot be confused with a normal prop.
- Keep the intake archive plaque readable but optional.

### Secrets

Verdict: functional, but currently too close to the main flow to feel authored.

The current `Secret - Intake Pressure Cache` is useful as the first secret-system proof. For V1 it should move or dress into a suspicious service-panel alcove. The clue should be subtle: chalk, missing rivets, hairline steam, or a warmer pipe glow.

V1 actions:
- Add an optional-looking clue before the player enters the cache trigger.
- Avoid placing the secret where the player can accidentally discover it during ordinary combat strafing.
- Keep rewards small: one health, one cartridge cache, optional lore scrap later.
- Ensure the secret remains reachable without jumping or crouching.

### Objective Clarity

Verdict: good system clarity, needs spatial cause-and-effect polish.

The HUD and labels cover the basics. V1 should rely more on room composition: locked gate first, amber key route second, green exit third.

V1 actions:
- Make the locked gate red state visible before key pickup.
- Turn the gate state green after unlock/open feedback.
- Place floor strips so amber guides to the key and green guides to the lift do not compete.
- Keep objective text short and direct.

### Hazard Readability

Verdict: no lethal hazard needed.

Visual steam can establish language for later maps, but Level01 should not teach damage hazards. Any steam puffs should sit outside required traversal.

V1 actions:
- Use visual-only leaks near pipes.
- Reserve red-orange danger language for Scrapper tells and the locked gate.
- Do not add furnace heat or steam damage to Level01.

### Enemy Roles

Verdict: Scrapper-only is correct.

V1 actions:
- Keep Scrapper silhouettes well lit in first contact.
- Make first attack tell readable against the room background.
- Avoid mixed enemy roles until Level02.

### Art Dressing

Verdict: high value and low risk.

V1 actions:
- Prioritize soot-brick service entry, repair benches, oil-dark stone, copper manifolds, pressure gauges, and pulley lift frame.
- Use repeated small props in clusters rather than loose clutter.
- Keep cover silhouettes readable at combat height.

### Performance Risk

Verdict: low.

Risks are mostly over-dressing the tutorial space with small props and transparent steam.

V1 actions:
- Static-batch wall/floor/bench clusters.
- Keep active particles minimal.
- Use the repair-bay dogleg as an occlusion opportunity.

## Level02 - Pipeworks Annex

V1 role: first ranged-combat level, pipe-routing objective, cartridge-cache secret, and larger industrial scale.

Current baseline:
- Generated blockout is a narrow `12m x 26m` baffle corridor.
- Current enemies: one Scrapper and one Lancer.
- Current objective: `Pipeworks Routing Valve Objective` unlocks `Pipeworks Service Lift To Boilerheart`.
- Current secret: `Secret - Pipeworks Cartridge Cache`.

### Navigation

Verdict: functionally valid but below V1 map scale.

The current corridor proves the valve lock and Lancer systems, but it does not yet deliver the planned pipe-spine, branch, valve gallery, bridge, and locked lift lobby. V1 needs a loop so the player can see the Boilerheart lift, branch to the routing valve, then return through changed pressure state.

V1 actions:
- Expand into arrival, baffle corridor, condensate spine, valve gallery, Lancer bridge, locked lift lobby, and Boilerheart lift.
- Let the lift lobby be visible before the valve is solved.
- Add at least one side resource room that is not the secret.
- Keep pipe baffles wide enough that backing away from Lancer fire does not snag the player.

### Combat Pacing

Verdict: the Lancer is introduced too close to the player.

The Lancer needs a long read with cover breaks. In the current scale, the player meets ranged pressure in a narrow lane. That works for automation but not for a first human read.

V1 actions:
- Give the first Lancer a bridge or raised platform with `12m` to `18m` of readable sightline.
- Place cover every `5m` to `6m`, not as a continuous wall.
- Use one Scrapper to teach moving through baffles before the Lancer shot lane.
- After routing the valve, add a small return-route response instead of increasing initial pressure.

### Landmarking

Verdict: needs a stronger pipeworks hero shape.

The routing valve and lift are readable objects, but the level needs a memorable pipe spine or bridge visible from several points.

V1 actions:
- Build a central pipe rack or condensate spine that runs visually from arrival to lift.
- Make the routing valve gallery a side landmark with gauge bank and amber lamps.
- Make the Boilerheart lift green only after pressure is routed.

### Secrets

Verdict: current secret is correct in concept, weak in authored discovery.

The cartridge cache should sit near a cold pipe, pipe-shadow recess, or maintenance store clue, not read as another objective label.

V1 actions:
- Use clue language such as cold-blue pipe, worker chalk, missing wall plate, or spare bolt marks.
- Keep the cache entrance standing-height; no crouch requirement.
- Keep the reward biased toward ammo because this level introduces ammo pressure from ranged combat.
- Ensure auto-playthrough secret totals still reach expected campaign total if the secret is moved.

### Objective Clarity

Verdict: strong mechanics, needs stronger spatial loop.

The lock and valve messages are clear. V1 should show why the valve matters through pressure lamps and pipe routing, not only HUD text.

V1 actions:
- Place red pressure lamps along the lift route before valve completion.
- Flip repeaters to green after valve completion.
- Add a world label or enamel sign near the locked lift: `BOILERHEART FEED: PRESSURE ROUTE INCOMPLETE`.
- Keep valve prompt chest-height and front-facing.

### Hazard Readability

Verdict: visual-only hazards are enough for V1 unless the route expands substantially.

Level02 should foreshadow steam and pressure without adding damage. The player is already learning ranged combat.

V1 actions:
- Use condensate, vents, and drip pans as dressing.
- Do not place damaging steam in the first Lancer lane.
- Reserve damaging hazards for Level03.

### Enemy Roles

Verdict: strong Lancer onboarding opportunity.

V1 actions:
- Make the Lancer silhouette visible before its first shot.
- Ensure projectile VFX contrast against pipe backgrounds.
- Let the player break line of sight laterally, not only by retreating.
- Do not add Bulwarks or Bellows Nodes here.

### Art Dressing

Verdict: high impact, moderate performance risk.

V1 actions:
- Use modular pipe-wall panels instead of many individual background pipes.
- Add gauge banks, pressure lamps, valve wheels, pipe labels, and cartridge crates.
- Keep the visual hierarchy: pipes are environment, valve is objective, lift is exit.

### Performance Risk

Verdict: medium.

Dense pipework can become expensive and visually noisy.

V1 actions:
- Build pipe walls as batched modules.
- Limit active steam leaks to two or three per view.
- Use occluding baffle turns to hide deeper pipe rooms.

## Level03 - Boilerheart Core

V1 role: hazard-management level, Steam Scattergun acquisition, Bellows Node support-machine teaching, and foundry descent lock.

Current baseline:
- Generated blockout is a compact `13m x 28m` boiler room.
- Current systems: Boilerheart pressure valve, locked foundry lift, two steam hazards, Steam Scattergun pickup, Bellows Node, archive plaque.
- Current enemies: two Scrappers plus one Bellows Node.

### Navigation

Verdict: core systems are present, but the level is too linear for the number of mechanics.

The player currently receives scattergun, steam hazards, Bellows Node, valve, and lift in one compressed lane. V1 should become a boiler ring with side branches: scattergun display on one side, Bellows Node chamber on the other, pressure valve catwalk near the lift.

V1 actions:
- Build a central Boilerheart Ring around the furnace core.
- Put the scattergun display in a readable branch before the support-machine spike.
- Put the Bellows Node in a side chamber with a clear pulse radius.
- Let the pressure valve catwalk overlook or visually connect to the foundry lift.

### Combat Pacing

Verdict: good ingredients, needs sequencing.

The scattergun and Bellows Node should not compete for first-read attention. The player should get the scattergun, use it in a simple close-range fight, then encounter the Bellows Node support role.

V1 actions:
- First approach: one or two Scrappers with steam preview, low pressure.
- Scattergun trial: close Scrapper pressure after pickup.
- Bellows Node: one node plus nearby Scrapper boost target, with obvious priority read.
- Valve return: safer steam route plus small final pressure, not a large ambush.

### Landmarking

Verdict: strongest level landmark in the current route.

The furnace core should be the dominant orientation device. It must be visible from arrival, ring, scattergun route, and valve route.

V1 actions:
- Give the furnace core a vertical silhouette and readable glow.
- Use overhead pipes to point toward the valve catwalk.
- Use green/amber state changes after venting.
- Keep the scattergun display visually valuable without making it look like the main exit.

### Secrets

Verdict: no current secret; do not add one until the main loop is readable.

Level03 can support a future gauge-cache secret, but V1 priority is the scattergun/Bellows/valve loop. A secret here should be a later optional layer, not a new requirement for first pass cohesion.

V1 actions:
- Plan a future `Optional Gauge Cache` behind gauge shutters or a maintenance void.
- If added, place it away from the scattergun route so it does not confuse weapon pickup priority.
- Reward ammo/health and a lore scrap, not a required weapon or progression flag.

### Objective Clarity

Verdict: mechanically clear, spatially cramped.

The foundry lift lock and pressure valve are proven. The missing piece is readable pressure causality: vent valve, steam shuts down, lift unlocks.

V1 actions:
- Show the foundry lift locked before the valve is reachable.
- Connect valve pipes visually to hazards and lift.
- After valve completion, change nearby steam/lamps so the player sees route state change.
- Keep objective text: `Vent the Boilerheart pressure valve` then `Ride the foundry lift`.

### Hazard Readability

Verdict: functional VFX exists, but V1 needs safe preview and safe lanes.

The two current steam hazards validate damage and puffs. They should become part of a readable pressure rhythm rather than surprise damage in a tight room.

V1 actions:
- Place visual-only steam before damaging steam.
- Add warning floor plates outside the damage trigger footprint.
- Ensure each hazard has a lateral bypass.
- Make linked hazard shutdown obvious after valve venting.

### Enemy Roles

Verdict: Bellows Node is the key risk and opportunity.

V1 actions:
- Add a floor ring or pulsing pressure line showing Bellows Node range.
- Add pipes, warning plates, and boosted-Scrapper visual line so players understand support behavior.
- Keep Bellows Node stationary and readable.
- Do not add a Lancer unless the room expands enough for a separate ranged sightline.

### Art Dressing

Verdict: high priority.

V1 actions:
- Build boiler column, pressure gauges, valve cathedral, bellows pipes, scattergun stand, route strips, and steam curtains.
- Keep scattergun pickup art readable from the approach and from the combat return route.
- Avoid adding too many small gauges where the valve is the real interaction.

### Performance Risk

Verdict: medium.

Steam particles, furnace glow, scattergun display lights, and Bellows pulse VFX can overlap.

V1 actions:
- Cap visible steam puffs per view.
- Use emissive-style material cues where possible instead of many dynamic lights.
- Keep the furnace core collision simple.
- Profile the hero view from the ring toward valve, core, and lift.

## Level04 - Furnace Foundry

V1 role: first major hazard-combat showcase and Bulwark heavy introduction.

Current baseline:
- Generated blockout is a compact `14m x 32m` foundry lane.
- Current systems: two steam hazards, two furnace heat hazards, foundry secret, mixed Scrapper/Lancer/Bulwark combat, emergency hoist to Level05.
- Current enemies: Scrapper, Lancer, Bulwark, Scrapper.
- Active collaboration note: Bulwark gameplay/readability is in a concurrent lane and should not be touched by side planning work.

### Navigation

Verdict: validates the route but is too narrow for V1 heavy combat.

The Bulwark needs wide arena space, and furnace hazards need lateral safe lanes. The current lane makes heat, ranged fire, melee pressure, and heavy pressure stack too easily.

V1 actions:
- Expand to arrival hoist, side loop, furnace row, gantry lane, Bulwark hammer bay, coal cache, regulator/hoist zone.
- Provide at least two routes into the furnace row.
- Keep Bulwark arena mostly flat with wide circulation.
- Keep the emergency hoist visible but not reachable through a hazard pinch.

### Combat Pacing

Verdict: high-value escalation, currently too compressed.

V1 should teach furnace heat timing before the Bulwark fight, then stage the Bulwark in a readable bay.

V1 actions:
- Furnace Row Crossfire: Scrappers plus one Lancer with safe cover.
- Heat Pulse Pinch: one light enemy near a heat lane, not the Bulwark.
- Bulwark Reveal: one Bulwark in a wide space with clear windup and edge cover.
- Hoist Response: small pressure after objective/route completion.

### Landmarking

Verdict: furnace row and hoist can carry the map.

V1 actions:
- Make furnace row visible from arrival.
- Use a massive hammer bay or rescue-frame cradle as the Bulwark staging landmark.
- Make emergency hoist green and elevated enough to read over local clutter.
- Preserve red-orange hazard language without stealing green exit language.

### Secrets

Verdict: current coal cache is good conceptually, needs safer clueing.

The coal cache should be readable from slag dust, cooler pipe color, and worker marks. It should not require crossing active lethal heat.

V1 actions:
- Place clue before the cache entrance, not inside the trigger.
- Keep the cache off the main hoist line so it feels optional.
- Reward health and ammo, with possible scattergun ammo value later.
- Preserve current secret total behavior.

### Objective Clarity

Verdict: current objective is simple exit traversal; production target can support a cooling regulator, but that is main-lane work.

A cooling regulator objective would help Level04 feel less like a corridor to Level05, but it touches scene flow and auto-playthrough. It should be implemented only as a main integration slice when the route is ready.

V1 actions:
- For near-term cohesion, keep `Reach the emergency hoist` clear.
- If adding regulator later, show `heat surge unsafe` before restore and green hoist state after restore.
- Do not add a new lock until validation and auto-playthrough are updated in the same slice.

### Hazard Readability

Verdict: VFX exists, but V1 needs choreography.

Current furnace warnings and heat ripples validate the system. The V1 issue is whether a player can read and react while under fire.

V1 actions:
- Add at least `0.75s` warning readability in world space before damage.
- Place safe islands wider than the player plus dodge room.
- Avoid forcing backpedal through active heat.
- Add hazard audio/readability as its own integration slice.

### Enemy Roles

Verdict: Bulwark reveal is the major pacing beat.

V1 actions:
- Make the Bulwark visible before aggro.
- Give it enough arena width to avoid cheap cornering.
- Keep Lancer support out of the first Bulwark read unless the room is very wide.
- Coordinate any Bulwark file or scene touch with the active Bulwark lane.

### Art Dressing

Verdict: high value, high noise risk.

V1 actions:
- Dress with furnace mouths, hammer press silhouettes, chain hoists, slag gutters, quench tanks, coal bins, warning lamps.
- Keep combat cover silhouettes clean.
- Keep heat signals brighter than generic furnace decoration only when active or warning.

### Performance Risk

Verdict: medium-high.

Furnace glow, heat shimmer, steam, Bulwark VFX, dynamic lights, and dense foundry props can stack.

V1 actions:
- Quality-scale heat shimmer.
- Use repeated furnace modules.
- Static-batch large props.
- Limit dynamic shadow-casting lights.
- Profile from furnace row toward Bulwark bay and hoist.

## Level05 - Governor Core

V1 role: final prototype climax, full enemy kit reminder, Governor Warden fight, and master override win.

Current baseline:
- Generated blockout is a compact `15m x 32m` governor lane.
- Current systems: steam hazard, furnace heat hazard, mixed Scrapper/Lancer/Bulwark pressure, Governor Warden, Warden boss HUD, Warden shutdown VFX, guardian-gated master override hoist.
- Current enemies: Scrapper, Lancer, Bulwark, Warden.

### Navigation

Verdict: functional finale, not yet a boss map.

The current corridor proves Warden gating and win flow. V1 needs a boss arena with stable orientation, side regulator arms, and pre-finale resource prep.

V1 actions:
- Expand to arrival hoist, pressure chapel approach, core ring, side regulator arms, Warden arena, master override hoist.
- Keep the boss arena mostly flat.
- Use cover pylons spaced at least `8m` apart.
- Let side arms loop back into the core instead of dead-ending during combat.

### Combat Pacing

Verdict: boss works technically, but mixed pressure needs separation.

The player should get one final reminder encounter before the boss, then fight the Warden in a dedicated space. The Bulwark should not blur the first Warden read.

V1 actions:
- Core Ring Mixed Machines: one small mixed encounter before the arena.
- Regulator Arms: optional resource risk with one focused role each.
- Warden Arena: Warden first, support only if balance proves the arena can carry it.
- Preserve boss health HUD and shutdown burst.

### Landmarking

Verdict: strong theme, needs boss-orientation landmarks.

The master governor should be a physical orientation system, not just dressing.

V1 actions:
- Make regulator drums or logic cylinders visible from arrival and boss arena.
- Keep master override hoist visible and locked before Warden defeat.
- Use north/south arena landmarks so the player can recover orientation during boss movement.
- Change Warden pressure lines from red/orange to green relief after shutdown if feasible.

### Secrets

Verdict: no current secret; future secret is optional.

The proposed Governor Clerk Void is a good final optional cache, but it should not be added during boss readability work unless secret validation is updated at the same time.

V1 actions:
- Plan but defer a `Secret - Governor Clerk Void`.
- Keep any future final secret accessible before boss or after boss defeat, not during active boss combat.
- Reward lore and recovery resources, not progression.

### Objective Clarity

Verdict: strong lock logic, needs stronger pre-fight framing.

The hoist lock message and Warden objective are clear. V1 should ensure the player understands "Warden active equals hoist sealed" before fighting.

V1 actions:
- Place a visible locked hoist signal in the arena approach.
- Add Warden warning signage: `MASTER OVERRIDE SEALED - WARDEN ACTIVE`.
- Keep objective text: `Defeat the Governor Warden` then `Activate the master override hoist`.
- Make unlock feedback visible even if the player is looking away from the hoist when the Warden dies.

### Hazard Readability

Verdict: should be restrained in the boss space.

Steam and heat are useful finale reminders, but hazards inside a boss fight can obscure whether damage came from Warden, Lancer, heat, or steam.

V1 actions:
- Keep hazards on arena edges or pre-boss regulator lanes.
- Avoid placing active furnace heat in the Warden's main stomp lane.
- Ensure boss attack tells have visual priority over ambient hazard decoration.
- Use world-space indicators instead of camera effects.

### Enemy Roles

Verdict: Warden identity is the finale. Protect that read.

V1 actions:
- Warden first reveal should be centered and front-facing.
- Preserve clear projectile and stomp tells.
- Use cover that blocks pressure bolts but does not trivialize pathing.
- Avoid simultaneous Bulwark pressure during the first Warden teaching phase.

### Art Dressing

Verdict: high value, high composition risk.

V1 actions:
- Dress with brass logic cylinders, regulator pylons, pressure needles, punch-card belts, clerk alcoves, master reset plate, and heavy chain hoist.
- Keep boss arena cover readable against machinery.
- Use red/orange hostile pressure lines feeding Warden and green relief after defeat.

### Performance Risk

Verdict: high.

Boss VFX, pressure bolts, shutdown burst, regulator machinery, furnace/steam hazards, and dynamic lights can overlap in one hero view.

V1 actions:
- Build quality tiers for Warden arena VFX.
- Use large repeated panels for governor machinery.
- Limit transparent tubes and steam in boss view.
- Profile the boss arena during Warden attacks and shutdown.

## Highest-Risk Items For V1

1. Level05 boss arena scale and readability.
2. Level04 Bulwark plus furnace hazard choreography.
3. Level03 Bellows Node spatial readability and scattergun pacing.
4. Level02 first Lancer sightline and lock/valve loop.
5. Cross-level art dressing density versus performance.

## Safest Parallel Work

These can move ahead without touching active gameplay files if their file scopes stay in documentation or asset-staging lanes:

- Paper blockout refinements for each production map.
- Signage, warning stamp, and route-label text sheets.
- Secret clue visual language.
- Modular prop kit briefs for intake, pipeworks, boilerheart, foundry, governor.
- Performance budget notes and hero-view checklists.
- Manual playtest checklists.

## Main Integration Work

These must stay in the main integration lane because they touch generated scenes, shared scene builder/validator files, gameplay systems, runtime tests, or active combat tuning:

- Any edit to `Assets/_Project/Scenes/Level01.unity` through `Level05.unity`.
- Any edit to `Assets/_Project/Editor/V0SceneBuilder.cs`.
- Any edit to `Assets/_Project/Editor/V0LevelValidator.cs`.
- Any change to `RuntimeAutoPlaythroughTest`, `RuntimeHazardTest`, `RuntimeSecretTest`, `RuntimeBellowsNodeTest`, `RuntimeBulwarkCombatTest`, or `RuntimeWardenCombatTest`.
- Any new Level04 Bulwark placement, attack-readability, or combat tuning while the Bulwark lane is active.
- Any new Warden or final-hoist behavior.
