# Brassworks Breach - v0.1.34 Level Density Polish Placement Plan

Created: `2026-05-24 12:56 -04:00`

## Purpose

This packet gives the main Unity lane a batch-ready placement plan for `v0.1.34`. The goal is to integrate several weapon/prop and enemy-readability upgrades across `Level01` through `Level05` in one visible polish leap, while preserving the stable route proved by `v0.1.33`.

Write scope for this side packet is only:

`D:\__MY APPS\Unity Doom\Documentation\Planning\V0_1_34_LevelDensityPolish\`

The main lane owns Unity scene generation, code, validators, shared status docs, release docs, builds, commits, and pushes. This plan is intentionally docs-only and does not require Blender or any external DCC workflow.

## Inputs Reviewed

- `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.33.md`
- `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.33.md`
- `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.33.md`
- `Documentation/Planning/V0_1_33_LevelDensityBatch/LEVEL_DENSITY_BATCH_PLAN_v0.1.33.md`
- `Documentation/Planning/V0_1_33_LevelDensityBatch/LEVEL_DENSITY_BATCH_STATUS.json`
- `Documentation/LEVEL_DESIGN_AND_MAPS.md`
- `Documentation/PARALLEL_LEVEL_PRODUCTION_MAPS.md`
- `Documentation/ProductionManagement/BATCH_PRODUCTION_MODE.md`
- `Documentation/VERSION_MICRO_ROADMAP.md`
- `Documentation/AssetProduction/WeaponPropBatch/README_WEAPON_PROP_BATCH.md`
- `Documentation/AssetProduction/WeaponPropBatch/WEAPON_PROP_BATCH_INTEGRATION_CHECKLIST.md`
- `Documentation/AssetProduction/WeaponPropBatch/weapon_prop_batch_manifest.json`
- `Documentation/AssetProduction/EnemyReadabilityBatch/ENEMY_READABILITY_BATCH_MANIFEST.md`
- `Documentation/AssetProduction/EnemyReadabilityBatch/ENEMY_READABILITY_BATCH_INTEGRATION_CHECKLIST.md`
- `Documentation/AssetProduction/EnemyReadabilityBatch/ENEMY_READABILITY_BATCH_ART_NOTES.md`
- `Documentation/AssetProduction/ThresholdRouteDressingBatch/THRESHOLD_ROUTE_DRESSING_BATCH_BRIEF_v0.1.33.md`

## Baseline Route Facts

`v0.1.33` route audit found no route-blocking scene composition issues. The automated route still needs human review for movement comfort, encounter pacing, audio mix, and final art readability. The route matrix currently says:

| Level | Core route | Enemies | Pickups | Hazards | Secrets | Transition |
| --- | --- | --- | --- | --- | --- | --- |
| Level01 | Gate and lift | `S:4 L:0 B:0 N:0 W:0` | `H:2 A:2 K:1 W:0` | None | 1 | Level02 open |
| Level02 | Valve and lift | `S:1 L:1 B:0 N:0 W:0` | `H:2 A:2 K:0 W:0` | None | 1 | Level03 locked until valve |
| Level03 | Valve and lift | `S:2 L:0 B:0 N:1 W:0` | `H:1 A:1 K:0 W:1` | Steam:2 | 0 | Level04 locked until valve |
| Level04 | Emergency lift | `S:2 L:1 B:1 N:0 W:0` | `H:2 A:2 K:0 W:0` | Steam:2, Furnace:2 | 1 | Level05 open |
| Level05 | Warden, guardian lock, exit | `S:1 L:1 B:1 N:0 W:1` | `H:1 A:1 K:0 W:0` | Steam:1, Furnace:1 | 0 | Final exit locked until Warden |

## Placement Strategy

This should be integrated as one density/readability batch, not as one isolated asset per release.

- Use the staged weapon/prop package as shape and material language for pressure-pistol details, ammo identity, gauge/dial support, fastener repair language, and the Level03 scattergun display.
- Use the staged enemy-readability package as a silhouette/tell upgrade source for existing Scrapper, Lancer, Bulwark, and Warden roles.
- Keep all level-density additions visual-only unless attached to existing gameplay collision by the main lane on purpose.
- Prefer wall, ceiling, lintel, edge, and rear-perimeter mounts. Avoid new floor blockers.
- Place density where it clarifies an objective, enemy role, hazard boundary, pickup, or exit. Do not fill empty space evenly.
- Preserve `v0.1.33` route dressing clearances and color language: amber for attention/objective, red for locked/hostile/unsafe, green for restored/exit.
- Do not place roots inside transition, pickup, prompt, hazard, enemy spawn, boss, final exit, or secret ownership volumes.

## Level-By-Level Safe And No-Go Zones

Coordinate convention follows the existing planning docs: `+Z` moves deeper/north, `+X` is east/right, and `1 Unity unit = 1 meter`.

| Level | Safe placement zones | No-go zones | Minimum clearances |
| --- | --- | --- | --- |
| Level01 | Gate Overlook side walls; Pressure Gate jambs outside the center aperture; Gear-Key Workshop rear/east wall; Repair Bay perimeter benches; Service Lift side/lintel mounts. | Pressure Gate center at `x -4..8, z 3..7`; Service Lift trigger/boarding at `x -3..7, z 18..24`; gear-key pickup prompt radius; first Scrapper retreat lanes; secret cache entrance. | Keep `3.5m` main lane, `2.2m` tight lane, and about `4m` lift turn space. Floor detail must be flush and non-colliding. |
| Level02 | Baffle Corridor side walls above shoulder height; Condensate Spine wall pipe panels; Lancer Bridge backplate and side supports; Routing Valve Gallery side/back wall; Locked Lift Lobby lintel. | Baffle gaps below `2.2m`; routing valve prompt radius; Lancer projectile line across the spine; lift trigger; cartridge cache standing-height access. | Keep bridge/sightline cues readable; do not put blue tell materials behind non-Lancer props. |
| Level03 | Steam-Baffle warning frames outside damage volumes; Boilerheart Ring outer perimeter; Scattergun Display back wall and plinth edge; Bellows Node Chamber perimeter; Pressure Valve Catwalk side rails/lintel. | Active steam hazard volumes; scattergun pickup/acquisition VFX space; Bellows Node chamber center and `16m x 16m` dodge footprint; pressure valve prompt; foundry lift trigger. | Keep enemy silhouettes clear against boiler glow; preserve pickup burst and Bellows pulse circle visibility. |
| Level04 | Arrival/Quench side walls; Furnace Row wall-side warning mounts; Coal Cache alcove walls; Bulwark Hammer Bay rear/perimeter; Emergency Hoist side/lintel frame. | Furnace heat lanes and warning/active VFX; steam hazard volumes; Bulwark circle space; coal cache entrance; emergency hoist trigger/boarding. | Keep broad lanes for heavy enemy movement; do not let furnace glow wash out Lancer/Bulwark tells. |
| Level05 | Pressure Chapel walls; Core Ring outer regulator panels; West/East Regulator Arm side walls; Warden Arena perimeter and high-wall feeds; Master Override Hoist side/lintel frame. | Warden center at about `x 0, z 17`; boss projectile/stomp lanes; cover pylon spacing under `8m`; Warden-to-exit baseline of `4.5m`; final hoist trigger; boss HUD readability. | Keep Warden visible on reveal; red hostile feeds and green exit release must not conflict. |

## Recommended Visible Placements

Use `LEVEL_DENSITY_POLISH_PLACEMENTS_v0.1.34.csv` as the machine-friendly grid. The batch recommends 18 visible placements: 14 core placements and 4 optional stretch placements. If the main lane needs to cut scope, ship the `P0` and `P1` rows first.

| ID | Priority | Level | Placement | Visible upgrade | Route-safety intent |
| --- | --- | --- | --- | --- | --- |
| LDP-01 | P0 | Level01 | Pressure Gate gauge-and-fastener crown | `WPB_003` gauge language plus `WPB_005` repair plates around the existing gate read. | Clarifies locked/restored pressure without narrowing the gate. |
| LDP-02 | P1 | Level01 | Repair Bay first-Scrapper tell frame | Scrapper cutter/hammer contrast reference near first combat read, placed on perimeter dressing rather than behind the enemy. | Helps first enemy read while preserving retreat lanes. |
| LDP-03 | P2 | Level01 | Gear-Key Workshop ammo/tool prop cluster | `WPB_006` ammo case and fastener-plate vocabulary on side bench and wall shelf. | Adds prop identity without hiding the key pickup. |
| LDP-04 | P0 | Level02 | Lancer Bridge blue-tell silhouette lane | Lancer long-lance and blue bolt-ring readability support behind or around the existing Lancer position. | Makes ranged threat readable without blocking projectile lanes. |
| LDP-05 | P1 | Level02 | Routing Valve gauge bank refresh | `WPB_003` gauge dial and fastener/coupler language around the valve gallery wall. | Explains valve-to-lift pressure routing without adding interactables. |
| LDP-06 | P2 | Level02 | Locked Lift ammo/pressure return shelf | Ammo cartridge case and green return pressure markers on lobby edge. | Makes post-valve return state richer without crowding the lift. |
| LDP-07 | P0 | Level03 | Scattergun Display replacement detail pass | `WPB_007` scattergun display pieces, shell row, and nameplate support at the existing pickup stand. | Highest weapon payoff; pickup silhouette and acquisition burst stay clear. |
| LDP-08 | P0 | Level03 | Bellows Node pulse-read perimeter | Support-machine hazard/tell frame, edge warning ring, and perimeter machinery mass. | Clarifies pulse danger while keeping chamber dodge space open. |
| LDP-09 | P1 | Level03 | Pressure Valve Catwalk gauge-and-vent stack | `WPB_003` gauges, `WPB_005` fasteners, vent collars, amber-to-green state plates. | Makes valve completion visibly release pressure without hiding the prompt. |
| LDP-10 | P2 | Level03 | Steam-Baffle safe-lane collars | Low-profile warning collars and safe-lane floor strips outside steam volumes. | Improves hazard timing read without masking puffs. |
| LDP-11 | P0 | Level04 | Bulwark Hammer Bay silhouette stage | Bulwark shield-door mass, side lamp, and hammer-tell contrast references on rear/perimeter stage props. | Makes heavy role readable before attack while preserving circle space. |
| LDP-12 | P1 | Level04 | Furnace Row hazard tell band | Heat-safe edge bands, warning collars, and high-wall furnace plates. | Clarifies furnace damage timing without covering enemy silhouettes. |
| LDP-13 | P2 | Level04 | Coal Cache ammo/prop reward frame | `WPB_006` ammo case language and coal-dust secret motif around the cache reward area. | Makes secret reward readable without pulling the main route through heat. |
| LDP-14 | P1 | Level04 | Emergency Hoist pressure-ready frame | Hoist lintel/side frame with green exit language and no trigger overlap. | Keeps Level05 transition unmistakable after foundry pressure. |
| LDP-15 | P0 | Level05 | Warden crown/blue-bolt arena contrast | Warden cage/crown/overhead blue-coil readability support in high-wall feeds and reveal framing. | Strengthens boss read without hiding attacks or HUD. |
| LDP-16 | P1 | Level05 | Core Ring regulator identity panels | Gauge, pressure-tank, and punch-plate panels on outer ring walls. | Helps orientation and enemy target priority without adding blockers. |
| LDP-17 | P2 | Level05 | Regulator Arm enemy-read side markers | East/West arm marker differences that keep Lancer/Scrapper/Bulwark roles distinct. | Reduces symmetry confusion while preserving side-arm loops. |
| LDP-18 | P1 | Level05 | Master Override Hoist final stack | Final hoist green unlock frame, red locked label support, side/lintel only. | Makes final exit state clear while protecting the short Warden-to-exit distance. |

## Batch Acceptance Checklist

### Import And Asset Boundaries

- [ ] Unity imports `WeaponPropBatch` staged meshes, materials, draft textures, and previews without magenta materials or missing-file errors.
- [ ] Unity imports `EnemyReadabilityBatch` staged meshes, shutdown fragments, materials, preview boards, and metadata without missing-file errors.
- [ ] Mesh colliders remain disabled unless a separate main-lane integration task explicitly owns collision.
- [ ] Staged meshes remain visual/readability references until human art review accepts them for prefab or component use.
- [ ] No Blender, external DCC files, scene files, gameplay scripts, animation controllers, hitboxes, AI rules, NavMesh obstacles, or trigger ownership are introduced by this placement pass.

### Route Safety

- [ ] Every new placement root is visual-only or attached to existing route-safe dressing authority.
- [ ] No new root sits inside a transition, pickup, prompt, hazard, enemy spawn, boss, secret, or final-exit ownership volume.
- [ ] Tight lanes remain at least `2.2m`; normal main lanes remain at least `3.5m`; lift/hoist boarding spaces preserve about `4m` turning room where planned.
- [ ] Floor strips and kick plates are non-colliding and about `0.03m` high or lower.
- [ ] Overhead or lintel detail does not reduce readable door/lift aperture height below `2.4m`.
- [ ] Route-state colors remain consistent: amber objective/attention, red locked/hostile/unsafe, green restored/exit.

### Weapon/Prop Readability

- [ ] Pressure-pistol style details keep copper coils, brass rails, blackened backing, red pressure accents, and gauge faces visually separated.
- [ ] Ammo cartridge props read as ammo/resource support, not interact prompts unless the existing pickup owns interaction.
- [ ] Scattergun display detail does not hide the world pickup, acquisition VFX/audio, route strips, chevrons, or label.
- [ ] Gauge/fastener additions do not imply extra buttons around valves, gates, or hoists.

### Enemy Readability

- [ ] Scrapper still reads as compact melee with one cutter tell and one hammer tell.
- [ ] Lancer still reads as tall ranged with a clean long lance line and blue bolt tell.
- [ ] Bulwark still reads as broad shield/heavy with visible side lamps and hammer tell.
- [ ] Warden still reads as tall cage/command unit with crown tanks and overhead bolt language.
- [ ] Enemy weak-point/tell colors do not get confused with pickups, route lamps, or generic decoration.
- [ ] No dressing sits directly behind an enemy in the same value/color range from the intended first-read angle.

### Level Smokes Before Full Matrix

- [ ] Level01: spawn, first Scrapper, key pickup, pressure gate locked/open, secret clue, service lift.
- [ ] Level02: locked lift, first Lancer read, routing valve, pressure return, cartridge cache, Boilerheart lift.
- [ ] Level03: steam baffles, scattergun pickup, Bellows Node chamber, pressure valve vent, foundry lift.
- [ ] Level04: furnace warning/active/safe lanes, coal cache, Bulwark Hammer Bay, emergency hoist.
- [ ] Level05: Core Ring orientation, side arms, Warden reveal/fight/shutdown, locked/unlocked final hoist.
- [ ] First-person screenshots exist for each changed zone from approach distance and from the objective/enemy read angle.
- [ ] Route audit for `v0.1.34` reports no blockers.
- [ ] Full V0 matrix runs only after targeted checks are clean and the batch is coherent.

## Handoff Recommendation

The highest-impact implementation shape is:

1. Integrate Level03 scattergun display detail and Level04 Bulwark staging first, because they combine weapon/prop and enemy readability in obvious player-facing beats.
2. Integrate Level05 Warden arena contrast and final hoist frame next, because the finale has the tightest route-to-exit spacing and highest clarity risk.
3. Integrate Level02 Lancer bridge and valve/lift pressure return as the midgame readability pair.
4. Integrate Level01 pressure gate/key/workshop polish as the tutorial-language reinforcement.
5. Add the optional reward/secret prop frames only after route and combat readability checks remain clean.

This keeps `v0.1.34` as a visible playable leap: weapon identity, enemy tells, objective machinery, hazards, secrets, and exits all improve together without changing the core route.
