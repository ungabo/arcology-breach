# Brassworks Breach - Steampunk Asset Catalog

This catalog tracks the assets needed to grow `Brassworks Breach` from a greybox proof of concept into a stylized steampunk FPS. Assets should be created in milestone-sized sets, verified in-game, and downgraded as needed for Android and browser/WebGL ports.

Status values: `planned`, `prototype`, `verified`, `in-progress`, `review`, `approved`, `deferred`.

Priority values: `P0` current milestone blocker, `P1` vertical-slice need, `P2` production quality, `P3` polish/expansion.

## Platform Tier Rule

Every major asset should eventually have three quality targets:

- `Windows`: primary target, mid-to-low gaming PC, highest asset fidelity for this project.
- `Android`: reduced texture size, fewer lights, simpler shaders, lower poly counts, compressed audio.
- `Browser`: WebGL-friendly, small downloads, minimal memory, simple materials, short audio.

## North-Star References

- `Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png`
- `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`

## 1. Environment Materials

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| MAT-001 | Greybox wall | P0 | prototype | Existing flat wall material for blockouts. |
| MAT-002 | Greybox floor | P0 | prototype | Existing dark floor material for blockouts. |
| MAT-003 | Oil-dark stone | P1 | verified | Generated procedural texture assigned to Brassworks service floors; final art polish can still improve wear/decal variation. |
| MAT-004 | Riveted iron panel | P1 | verified | Generated procedural riveted-iron texture assigned to boiler/gate pieces; final modular panel geometry still needed. |
| MAT-005 | Brass/copper pipe material | P1 | verified | Generated procedural pipe texture assigned to brass/copper pipe strips; final pipe geometry still needed. |
| MAT-006 | Soot-brick wall | P1 | planned | Primary corridor wall style. |
| MAT-007 | Walnut grip/wood trim | P2 | planned | Weapon grips, tool handles, occasional prop trim. |
| MAT-008 | Amber furnace glow | P1 | prototype | Gauge, furnace, and hazard accents. |
| MAT-009 | Red pressure warning surface | P1 | planned | Gate denial and pressure danger material. |
| MAT-010 | Green service-lift surface | P1 | planned | Exit/lift material with restored-pressure cue. |
| MAT-011 | Gauge glass and cream enamel | P2 | planned | HUD, gauges, and readable labels. |
| MAT-012 | Oil/scorch decal set | P2 | planned | Combat and machinery wear. |

## 2. Modular Geometry

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| GEO-001 | Greybox blockout kit | P0 | prototype | Existing cube-based walls/floor. |
| GEO-002 | Brassworks corridor module | P1 | planned | Soot brick, riveted panels, pipe bundles. |
| GEO-003 | Repair bay room kit | P1 | planned | Combat room modules with benches, gantries, and machine bays. |
| GEO-004 | Pressure gate frame | P1 | verified | Procedural heavy frame with header gear, lamps, brass floor track, and static supports. |
| GEO-005 | Gear-key plinth | P1 | planned | Objective station that visually explains the key. |
| GEO-006 | Service lift frame | P1 | verified | Procedural brass lift cage with platform, rails, chains, pulley gear, call box, lamps, and pressure gauge. |
| GEO-007 | Furnace control console | P2 | planned | Final-room landmark prop/geometry hybrid. |
| GEO-008 | Boiler stack wall | P2 | prototype | Primitive boiler stacks with light strips. |
| GEO-009 | Catwalk/rail kit | P2 | planned | Simple vertical-looking geometry, used carefully for readability. |
| GEO-010 | Secret service hatch | P2 | planned | Hidden route/secret entry module. |

## 3. Props and Objective Objects

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| PROP-001 | Gear key placeholder | P0 | prototype | Existing primitive gear-key pickup with teeth, hub, shaft, and bit. |
| PROP-002 | Pressure gate placeholder | P0 | prototype | Existing sliding gate with primitive rails, gear wheel, and gauge. |
| PROP-003 | Service lift placeholder | P0 | prototype | Existing green exit object with primitive cage rails and gauge. |
| PROP-004 | Health vial | P1 | verified | Primitive brass-and-glass healing pickup with red medicinal fluid, cross mark, and data-driven pickup definition. |
| PROP-005 | Pressure cartridge pack | P1 | verified | Primitive brass cartridge bundle with iron nozzles, straps, pressure gauge, and data-driven pickup definition. |
| PROP-006 | Gear key final | P1 | verified | Upright procedural brass clockwork key with gear face, teeth, spokes, stem, bit, hub, and pins. |
| PROP-007 | Pressure gate final | P1 | verified | Heavy procedural pressure gate with keyed socket, riveted slabs, gear wheel, gauge, pressure cylinders, warning lamps, and brass frame. |
| PROP-008 | Service lift final | P1 | verified | Green-lit brass service elevator visual shared by level transitions and final exits. |
| PROP-009 | Wall pressure gauge | P1 | prototype | Primitive wall gauge prop for environment readability. |
| PROP-010 | Valve wheel | P1 | prototype | Primitive valve wheel prop; future interactable/switch candidate. |
| PROP-011 | Coal furnace | P2 | prototype | Primitive environmental landmark and light source. |
| PROP-012 | Copper pipe bundle | P1 | verified | Procedural three-pipe bundle with brackets used in intake, gate, final, and pipeworks spaces. |
| PROP-013 | Work order board | P2 | verified | Environmental storytelling board with iron backboard, cream sheet, brass header clip, and compact work-order text. |
| PROP-014 | Secret cache props | P2 | prototype | Intake pressure cache and Foundry coal cache with reward pickups and validation. |
| PROP-015 | Warden lock signal | P2 | prototype | Red/green guardian-lock signal props driven by `GuardianDefeatObjective` near the Governor Core master override hoist. |
| PROP-016 | Animated machinery spinner set | P1 | prototype | Pressure-gate gears, service-lift pulleys, valve wheels, and menu gear now use `SteamworksSpinner`; final art should upgrade these into richer gear trains, belts, flywheels, and linked piston assemblies. |

## 4. Weapons

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| WPN-001 | Pressure Pistol prototype | P0 | prototype | Primitive first-person brass-and-walnut pneumatic sidearm with receiver, pressure tube, gauge, valve, trigger, and side pipes. |
| WPN-002 | Pressure Pistol final | P1 | verified | Procedural brass-and-walnut pneumatic sidearm with pressure tank, muzzle crown, sights, vent, valve, gauge, pipes, rivets, recoil/flash feedback, and data-driven definition asset. |
| WPN-003 | Steam Scattergun | P1 | planned | Close-range breaching weapon with chunky pressure release. |
| WPN-004 | Rivet Launcher | P2 | planned | Mechanical precision weapon for stronger machines. |
| WPN-005 | Weapon pickup shells | P2 | planned | World pickup visuals for weapons. |
| WPN-006 | Ammo family | P1 | prototype | Pressure cartridge pack exists with data-driven pickup definition; rivet bundles and boiler caps remain planned. |

## 5. Mechanical Enemies

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| ENEMY-001 | Primitive Scrapper | P0 | verified | Existing primitive clockwork silhouette with boiler torso, brass plate, furnace eye, pressure tank, piston arms, cutters, blocky feet, and data-driven definition asset. |
| ENEMY-002 | Scrapper final | P1 | planned | Maintenance frame with cutter arms, brass plating, pistons, and furnace eyes. |
| ENEMY-003 | Boiler Tick | P1 | planned | Squat scout with clockwork legs and pressure tank body. |
| ENEMY-004 | Lancer | P1 | verified | Thin ranged valve-rifle automaton with primitive tripod silhouette, pressure-bolt attack, and data-driven definition asset. |
| ENEMY-005 | Bulwark | P2 | prototype | Primitive furnace-plated machine with boiler body, hammer arms, data-driven definition, Level04/Level05 placement, and combat smoke coverage. |
| ENEMY-006 | Bellows Node | P2 | planned | Stationary pressure amplifier that buffs nearby machines. |
| ENEMY-007 | Governor Warden | P1 | prototype | Final guardian prototype with data-driven definition, Level05 placement, stomp attack, pressure-bolt attack, enraged half-health behavior, primitive core body/furnace heart/pressure crown/pressure cannon silhouette, boss health HUD integration, shutdown VFX, validation, and combat smoke coverage. |

## 6. Animations

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| ANIM-001 | Pressure Pistol idle/fire | P1 | prototype | Existing recoil/flash can be expanded. |
| ANIM-002 | Pressure Pistol reload/check | P2 | planned | Optional if reloads become part of gameplay. |
| ANIM-003 | Scrapper idle/chase | P1 | planned | Piston-driven movement, clear forward pressure. |
| ANIM-004 | Scrapper attack tell | P1 | planned | Cutter arm windup before damage. |
| ANIM-005 | Scrapper hit/death | P1 | planned | Sparks, stagger, collapse, or shutdown. |
| ANIM-006 | Lancer aim/fire | P1 | planned | Valve charge tell and shot release. |
| ANIM-007 | Gate open/close | P1 | prototype | Existing sliding motion, needs gear-driven animation. |
| ANIM-008 | Pickup bob/spin | P1 | prototype | Existing bobbing/spin can become clockwork hover or plinth animation. |
| ANIM-009 | Machinery spinner loop | P1 | prototype | Reusable local-axis spinner loop for pressure gates, service lifts, valve wheels, and menu steamworks motion; final pass should vary speed, timing, linkage, and sound. |

## 7. VFX

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| VFX-001 | Muzzle flash placeholder | P0 | prototype | Existing primitive flash. |
| VFX-002 | Pressure muzzle flash | P1 | planned | Hot spark and steam puff. |
| VFX-003 | Impact sparks | P1 | prototype | Short primitive metal spark burst at weapon impact point. |
| VFX-004 | Machine hit effect | P1 | prototype | `MachineHitVfx` now spawns short brass sparks and steam pops for non-lethal Scrapper, Lancer, Bulwark, and Governor Warden damage; final pass should add per-enemy oil flecks, impact decals, and stronger directional bursts. |
| VFX-005 | Machine death effect | P1 | prototype | Standard machine shutdown now spawns steam puffs, brass sparks, and a pressure ring for Scrappers/Lancers/Bulwarks; final per-enemy variants remain planned. |
| VFX-005B | Warden shutdown burst | P1 | prototype | Boss-specific shutdown effect spawned by `GovernorWardenController`, verified by Warden combat smoke. |
| VFX-005C | Standard shutdown burst | P1 | prototype | Compact `MachineDeathVfx` used by Scrappers and Lancers, scaled up for Bulwarks, verified by combat smoke. |
| VFX-006 | Gear-key pickup effect | P1 | prototype | `GearKeyPickupVfx` spawns a brass ring, center glow, and tooth sparks when the gear key is collected; final pass should add clockwork glyphs and stronger pickup audio sync. |
| VFX-006B | Resource pickup effects | P1 | prototype | `ResourcePickupVfx` spawns red medicinal bursts for health vials and brass/cyan pressure bursts for ammo pickups, verified by auto-playthrough. |
| VFX-007 | Pressure gate open effect | P2 | prototype | `GateOpenVfx` spawns green pressure wash, steam jets, and brass/green sparks when the Level01 pressure gate opens; final pass should link lamp states, gear sound, and denser steam. |
| VFX-008 | Service lift activation | P1 | prototype | `LiftActivationVfx` spawns a green floor pressure ring, center steam pulse, and brass/green contact sparks before level-transition scene loads. |
| VFX-009 | Steam hazard field | P2 | prototype | `SteamHazardVfx` animates low/high steam puffs on generated steam hazards; final pass should add denser particles, sound sync, and wind direction. |

## 8. Audio

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| AUD-001 | Pressure Pistol fire | P1 | prototype | Procedural pneumatic snap with noisy pressure tail. |
| AUD-002 | Empty weapon click | P1 | prototype | Procedural dry mechanical click. |
| AUD-003 | Scrapper hurt | P1 | prototype | Procedural metal strain and spark crackle. |
| AUD-004 | Scrapper death | P1 | prototype | Procedural shutdown whine and metal drop impression. |
| AUD-005 | Player hurt | P1 | prototype | Procedural impact thud and pressure pulse. |
| AUD-006 | Health pickup | P1 | prototype | Procedural glass/medical tick. |
| AUD-007 | Ammo pickup | P1 | prototype | Procedural pressure-cartridge tick. |
| AUD-008 | Gear-key pickup | P1 | prototype | Procedural gear-chime unlock cue. |
| AUD-009 | Gate open | P1 | prototype | Procedural valve release and motor slide. |
| AUD-010 | Gate denied | P1 | prototype | Procedural pressure denial buzzer. |
| AUD-011 | Exit/win cue | P1 | prototype | Procedural service-lift chime. |
| AUD-012 | Brassworks ambience | P2 | planned | Steam, furnace rumble, distant machinery, pipe knocks. |

## 9. UI and HUD

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| UI-001 | Text HUD | P0 | prototype | Existing plain HUD. |
| UI-002 | Brass gauge HUD frame | P1 | prototype | Primitive instrument-panel HUD backplates and fill gauges. |
| UI-003 | Gear-key indicator | P1 | prototype | Primitive amber/green gear-key status lamp. |
| UI-004 | Damage pressure overlay | P1 | prototype | Existing red damage flash can become pressure warning overlay. |
| UI-005 | Pressure warning prompt | P1 | prototype | Clear denied/key-required feedback through HUD text, gate audio, and world label. |
| UI-006 | Main menu | P2 | prototype | Generated start/quit menu with brassworks backdrop and automated test routing into gameplay. |
| UI-007 | Settings and accessibility | P2 | prototype | Mouse sensitivity and master volume sliders exist on main and pause menus; resolution, flash intensity, and color readability remain planned. |
| UI-008 | Boss health gauge | P1 | prototype | Top-center brass backplate, red pressure fill, and boss label for the Governor Warden; validated in scene/runtime smoke and damage-checked by Warden combat smoke. |
| UI-009 | Persistent objective HUD | P1 | prototype | Brass objective backplate and route text that updates for key, gate, valve, Warden defeat, death, and win states; validated by runtime smoke and auto-playthrough. |

## 10. Level Assets

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| LVL-001 | Brassworks Intake greybox | P0 | prototype | Current generated `Level01`. |
| LVL-002 | Brassworks Intake combat slice | P1 | verified | Current layout has objective guide strips, world labels, Scrapper attack tells, and first collision-cover pass. |
| LVL-003 | Brassworks Intake art pass | P1 | prototype | First procedural dressing pass: oil-stone patches, pipe runs, boiler stacks, hazard strips. |
| LVL-004 | Pipeworks Annex prototype | P2 | prototype | Generated second level with pipeworks blockout, Scrapper/Lancer pressure, pickups, dressing, and service-lift transition. |
| LVL-005 | Gauge Hall concept | P2 | planned | Valve/gauge lock sequence level concept. |
| LVL-006 | Furnace Foundry prototype | P2 | prototype | Generated fourth level with foundry blockout, mixed Scrapper/Lancer/Bulwark pressure, steam hazards, pulsing furnace heat lanes, secret coal cache, furnace-row dressing, and emergency-hoist transition to Level05. |
| LVL-007 | Governor Core prototype | P2 | prototype | Generated fifth level with core-ring blockout, mixed Scrapper/Lancer/Bulwark/Warden pressure, steam hazard, furnace-heat surge, regulator dressing, Warden-locked master override hoist, and win state. |

## 11. Platform Variants

For each P1/P2 visual asset, define:

- Windows source asset.
- Android reduced asset.
- Browser reduced asset.

Reduction checklist:

- Texture size downscale.
- Mesh simplification.
- Reduced material count.
- Reduced transparency.
- Baked or vertex lighting where possible.
- Shorter compressed audio.
- Fewer unique variants loaded at once.

## 12. Generation Method

1. Add or update catalog row.
2. Write a short generation brief.
3. Generate/build only the assets needed for the current milestone.
4. Import into Unity with platform-friendly settings.
5. Test in the scene.
6. Update status and notes.
7. Add follow-up work to `WORK_LEDGER.md`.

## 13. Local Asset Pack Candidates

Local installed/downloaded Unity Asset Store packs are tracked in `ASSET_PACK_REVIEW.md`.

Near-term candidates:

- BlockOut Prototype Kit: fast layout and encounter scale testing.
- Unity Particle Pack / Particle Pack 5x: sparks, smoke, pressure vents, machine shutdown VFX.
- Medieval or industrial prop packs if present: possible source for stone, wood, metal, chains, barrels, or workshop props after art-direction review.
- FPS Microgame weapon add-ons: weapon-scale reference only, not final style.

Do not import large unrelated demo kits into the main project until a scoped task requires them.
