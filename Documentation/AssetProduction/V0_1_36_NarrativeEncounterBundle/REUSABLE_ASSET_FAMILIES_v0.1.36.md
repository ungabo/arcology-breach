# Brassworks Breach - v0.1.36 Reusable Narrative + Encounter Asset Families

Created: `2026-05-24`

## Scope

This document lists production-ready reusable asset families needed to support the v0.1.36 narrative and encounter beat sheet. It is a planning/asset request package only. It does not generate, stage, or integrate assets.

Production principle:

- Build families, not one-off props.
- Use shared material and decal atlases.
- Keep collision optional and conservative.
- Keep mobile/WebGL/VR reductions possible.
- Preserve the current steampunk north-star: brass, copper, soot brick, riveted iron, oil-dark stone, gauges, valve wheels, caged gaslight, furnace glow, worker chalk, and municipal pressure signage.

## Family Index

| Family ID | Family | Main levels | Production priority | Role |
| --- | --- | --- | --- | --- |
| NEA-01 | Municipal signage and route-state plates | All | P0 | Objective, hazard, lift, valve, gate, and room identity. |
| NEA-02 | Worker survival marks and secret clues | All, especially Level02/04 | P0 | Optional secret language and ambient human presence. |
| NEA-03 | Archive plaque and lore plate kit | All | P0 | Short lore beats using existing archive style. |
| NEA-04 | Objective plinth/valve/gauge dressing | Level01/02/03/05 | P0 | Physical cause/effect for key, gate, valve, final override. |
| NEA-05 | Enemy introduction staging props | Level01/02/03/04/05 | P0 | Mechanical context for Scrapper, Lancer, Bellows Node, Bulwark, Warden. |
| NEA-06 | Ambient machinery motion set | All | P1 | Non-authoritative spinners, pistons, pressure drums, capsule tubes. |
| NEA-07 | Worker trace dressing | Level01/03/04/05 | P1 | Human stakes through tools, ration crates, gloves, petitions, stamped forms. |
| NEA-08 | Secret cache containers | Level01/02/04 future-compatible | P1 | Reusable reward dressing without new route authority. |
| NEA-09 | State-change VFX/audio cue pack | All | P1 | Red/amber/green pressure changes, discovery, locks, vents, shutdowns. |
| NEA-10 | Finale classification-engine kit | Level05 | P1 | Governor Core identity and Warden arena story context. |

## NEA-01 - Municipal Signage And Route-State Plates

Purpose:

- Make objective paths, room identity, hazards, and exits readable through diegetic signage.
- Provide short labels that can appear as decals, plates, hanging tags, or enamel signs.

Needed variants:

- Room name plates: Intake, Pipeworks, Boilerheart, Foundry, Governor Core.
- Objective plates: Gear Authority Required, Route Boilerheart Pressure, Manual Vent Breaks Boiler Seal, Master Override.
- Hazard plates: Bolt Feed Live, Steam Leak, Furnace Surge, Pulse Radius.
- Exit plates: Service Lift, Emergency Hoist, Master Override Hoist.
- State lenses: red locked, amber attention, green restored.

Material and texture guidance:

- Shared sign atlas at `1024` or `2048`, with `512` fallback.
- Aged cream enamel, blackened iron backs, brass screws/rivets, red/amber/green glass.
- Text must remain readable from normal first-person approach distance.

Collision guidance:

- Non-colliding by default.
- If raised physical plates collide, thickness should be shallow and wall-mounted only.

## NEA-02 - Worker Survival Marks And Secret Clues

Purpose:

- Communicate the Locked Shift through low-cost environmental storytelling.
- Establish consistent optional secret language.

Needed variants:

- Chalk arrows, short tally marks, three-knock symbol, crossed-out gear symbol.
- Grease pencil pipe labels.
- Small cloth tags tied to pipes.
- Hidden cache markers for pressure cache, cartridge cache, coal cache.
- Warning scrawls that do not read like mandatory objectives.

Material and texture guidance:

- Decal atlas with chalk white, coal black, rust red, oil smear.
- Must have high-contrast and reduced-opacity variants.
- Avoid tiny handwriting as the only clue.

Platform notes:

- Android/WebGL fallback can use larger symbols and fewer small marks.
- VR scale should avoid requiring players to lean close to walls.

## NEA-03 - Archive Plaque And Lore Plate Kit

Purpose:

- Support existing optional archive plaques with final art language.
- Keep lore brief and readable without blocking combat.

Needed variants:

- Brass wall plaque.
- Cast-iron municipal plaque.
- Hinged maintenance clipboard plate.
- Broken plaque with missing corner.
- Small plaque light or caged lamp pairing.

Text capacity:

- One title line plus one or two short body lines.
- Use existing archive texts as first content source.

Interaction notes:

- Plaques are optional and should not be confused with switches.
- Prompt space must remain clear if made interactable by a later owner.

## NEA-04 - Objective Plinth / Valve / Gauge Dressing

Purpose:

- Strengthen cause-and-effect around route-critical objectives using visual-only dressing.

Needed variants:

- Gear-key plinth yoke with amber/green gauge.
- Pressure Gate lock diagram plate.
- Routing valve gauge bank: red-to-amber-to-green sequence.
- Boilerheart valve linked-pressure diagram.
- Final Master Override plate and lever housing.

Animation hooks:

- Gauge needle twitch.
- Lens flip red to green.
- Valve wheel spinner.
- Pressure vent puff.
- Mechanical latch release.

Integration caution:

- These assets must not introduce new interactable roots. Later integration should attach them to existing objective objects or place them as visual children.

## NEA-05 - Enemy Introduction Staging Props

Purpose:

- Give each enemy family a practical machine context before or during its introduction.

Needed variants:

- Scrapper repair bench: tool clamps, cutter-arm rack, inert small automaton shell.
- Lancer bolt-feed rack: pressure cartridges, valve-rifle cradle, perforated target board.
- Bellows Node hazard base: floor ring, pressure crown, warning posts, pulse gauge.
- Bulwark rescue-frame cradle: clamp arms, containment stamp, hammer rack, furnace plate pile.
- Warden feed-pipe crown: service-frame sockets, pressure hoses, core clamps.

Readability notes:

- Staging props should frame enemy silhouettes, not hide them.
- Avoid placing high-contrast signage directly behind ranged tells or boss attacks.
- Collision should be disabled or perimeter-only until route review.

## NEA-06 - Ambient Machinery Motion Set

Purpose:

- Add life to rooms through small mechanical actions that do not own gameplay.

Needed variants:

- Wall gear spinner.
- Belt pulley loop.
- Piston pump.
- Message capsule tube with jam state.
- Pressure drum rocker.
- Gauge cluster twitch.
- Hanging chain/counterweight loop.

Performance notes:

- Sparse placement. Use one or two meaningful moving clusters per room.
- Support static fallback with same silhouette.
- Avoid per-prop unique materials.

VR notes:

- No large fast motion close to player eye height.
- Avoid forced attention snaps or intense flashes.

## NEA-07 - Worker Trace Dressing

Purpose:

- Add human stakes without cutscenes or long lore dumps.

Needed variants:

- Worn tool roll.
- Abandoned gloves.
- Ration crate.
- Stamped work order bundle.
- City Council sealed memo.
- Locked Shift petition sheet.
- Broken pressure mask.
- Maintenance lunch tin.
- Boot scuff decal cluster.

Placement notes:

- Use in corners, benches, shelves, near safe walls, and secret spaces.
- Keep out of primary movement lanes and enemy navigation.
- Do not overuse in every room; human traces should feel discovered.

## NEA-08 - Secret Cache Containers

Purpose:

- Standardize existing and future secret reward presentation.

Needed variants:

- Pressure cache: brass service locker, pressure cartridges, small health tin.
- Cartridge cache: pipe-rack panel, bolt-feed crate, ammo sleeves.
- Coal cache: coal bin, hidden ration pocket, dust panel.
- Generic cache open/closed states.
- Discovery dust/steam puff socket.

Rules:

- Cache props must not imply a new secret if placed on the main route.
- Open state should clearly show the secret has been claimed.
- Discovery cue should differ from normal pickup collection.

## NEA-09 - State-Change VFX / Audio Cue Pack

Purpose:

- Provide reusable presentation hooks for objective state changes and ambient scripting.

Needed VFX variants:

- Amber attention puff.
- Red lock rejection hiss.
- Green restored pressure vent.
- Gauge needle spark.
- Secret discovery dust puff.
- Valve vent pressure drop.
- Machine shutdown brass-spark burst.
- Warden lock lamp red-to-green pulse.

Needed audio variants:

- Short brass objective ping.
- Dry lock rejection clank.
- Green route restore thump.
- Valve wheel groan.
- Pipe pressure reroute knocks.
- Secret discovery chime.
- Machine shutdown hiss.
- Governor classification stamp.

Performance and accessibility:

- Keep cues short, non-looping unless explicitly ambient.
- Avoid harsh high-frequency repetition.
- Provide volume/mix category compatibility for later settings.

## NEA-10 - Finale Classification-Engine Kit

Purpose:

- Make Level05 feel like the Master Governor's analog decision engine.

Needed variants:

- Punch-plate wall.
- Classification stamp arm.
- Mercury switch bank, stylized/nonliteral if needed.
- Pressure drum stack.
- Petition board under glass.
- Governor order plate: `CONTAINMENT SUPERSEDES RESCUE`.
- Warden feed socket cluster.
- Final override lever housing.

Readability notes:

- Use these as high-wall and perimeter identity pieces.
- Keep Warden center, projectile lanes, boss HUD, and final hoist path visually clear.
- Red/green state lamps should be tied to existing Warden lock authority if later integrated.

## Cross-Family Naming Recommendations

Suggested asset prefixes for later production:

- `NES_SIGN_` for narrative/encounter signage.
- `NES_MARK_` for worker chalk/secret marks.
- `NES_PLAQUE_` for archive plaques.
- `NES_OBJ_` for objective dressing.
- `NES_STAGE_` for enemy staging props.
- `NES_MACH_` for ambient machinery.
- `NES_TRACE_` for worker trace props.
- `NES_CACHE_` for secret cache props.
- `NES_VFX_` and `NES_AUD_` for state-change cues.
- `NES_CORE_` for Governor Core finale kit.

Suggested material families:

- `M_NES_AgedBrass`
- `M_NES_BlackenedIron`
- `M_NES_CreamEnamel`
- `M_NES_RouteGlass_RedAmberGreen`
- `M_NES_ChalkMarks`
- `M_NES_SootDust`
- `M_NES_FurnaceGlow`

## Production Acceptance Checklist

- [ ] Each family uses shared materials or atlases wherever practical.
- [ ] Critical text remains readable at reduced texture sizes.
- [ ] Props have non-colliding or conservative collider guidance.
- [ ] Optional animation can be disabled without changing route state.
- [ ] Windows LOD and mobile/WebGL/VR fallback strategy is noted for major props.
- [ ] Signage does not invent new mechanics or objective nouns beyond current route truth.
- [ ] Enemy staging props improve silhouette context without hiding combat tells.
