# Brassworks Breach - Parallel Asset Pack Production Plan

Created: 2026-05-23 21:31 -04:00

Side-agent lane: asset-pack planning only.

Allowed output for this lane:

- `Documentation/PARALLEL_ASSET_PACK_PRODUCTION_PLAN.md`
- `Documentation/PARALLEL_ASSET_ACCEPTANCE_CHECKLIST.md`

This lane must not edit Unity scenes, scripts, README, roadmap/status docs, generated assets, or existing catalog rows unless the main development agent explicitly asks for a merge pass.

## Purpose

This document defines a practical AAA-grade asset-pack production plan that can run in parallel with current `v0` gameplay integration. The goal is to prepare a coherent steampunk asset library for `Brassworks Breach` without blocking the main code/gameplay path.

The game should not become cyberpunk or generic photorealism. The correct target is:

- Stylized steampunk silhouettes, proportions, and readability.
- High-fidelity PBR material detail on surfaces.
- Strong brassworks identity: rivets, pressure systems, gauges, valves, walnut grips, soot, oil, gaslight, wet stone, furnace glow, worn iron, and handmade machine logic.
- Assets that scale down cleanly for Android, WebGL, and VR variants.

## Visual Quality Resolution

The user asked for a heavily stylized steampunk game and later referenced photorealistic AAA assets. Resolve that as:

- Shape language: stylized, chunky, readable, iconic.
- Material language: high-fidelity PBR with believable roughness, grime, dents, soot, oxidized copper, scratched brass, hot iron, polished valve edges, enamel chips, and oily fingerprints.
- Scene readability: gameplay-first, not clutter-first.
- Performance: mid-to-low gaming PC first, with mobile/browser/VR budgets planned from the start.

Do not pursue clean sci-fi, black chrome, neon cyberpunk city language, digital holograms, smooth robots, or fragile micro-detail that disappears at gameplay distance.

## North-Star Sources

Primary references already in the project:

- `Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png`
- `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`
- `Documentation/STEAMPUNK_NORTH_STAR.md`
- `Documentation/CREATIVE_DIRECTION.md`
- `Documentation/AAA_ASSET_CATALOG.md`

All asset briefs in this lane should cite which north-star element they serve: corridor, prop, weapon, enemy, HUD/instrument, machinery, or hazard.

## Parallel Work Model

This asset-pack lane can run independently because it produces briefs, source assets, test imports, and acceptance data before the main game integrates them.

Recommended isolation:

- Work in a separate folder outside the active Unity scene path until assets are accepted.
- Use a staging namespace such as `Assets/_Project/ArtStaging` only when the main agent asks for import.
- Keep generated source files, references, and model exports organized outside scene files.
- Never replace existing materials/prefabs by filename; version them and let the main agent integrate intentionally.

Suggested status values:

- `briefed`: written brief exists.
- `generated`: texture/model/audio/VFX source exists.
- `staged`: imported to a clean Unity staging folder or test project.
- `accepted`: passes checklist and is ready for main integration.
- `integrated`: main development agent placed it into gameplay scenes/prefabs.
- `rework`: failed checklist or style review.
- `deferred`: useful later, not needed for current Windows path.

## Asset Source Strategy

Use three source paths, in this order:

1. Procedural generation now for fast, consistent, low-risk pieces.
2. Local or Unity Asset Store packs for selected baseline meshes/materials that can be reskinned into the Brassworks style.
3. External modeling and animation for hero assets where silhouette, rigging, and first-person presentation matter.

Procedural generation is best for:

- PBR material textures and trim sheets.
- Riveted panels, pipes, gauges, decals, grate floors, hazard strips, signs, plaques, and simple mechanical props.
- Low-poly and mid-poly modular kits that can be iterated quickly.
- Simple VFX textures: smoke puffs, sparks, scorch decals, pressure rings, heat masks.
- Temporary audio layers: valve clanks, steam vents, gear ticks, pressure snaps.

Asset packs are best for:

- Generic industrial props after style filtering: chains, barrels, crates, bolts, brackets, old tools, pipes, boilers, metal panels.
- Particle references or reusable particle textures.
- VR interaction reference kits, kept out of the main project until VR work begins.

External modeling is best for:

- Final first-person weapons.
- Rigged mechanical enemies.
- Hero room set pieces.
- Authored animation sets.
- Complex modular architecture with baked trims and clean LODs.

## Folder and Naming Convention

Future asset imports should use a consistent structure. This plan does not create these folders yet; it defines the target.

```text
Assets/_Project/Art/
  Materials/
    Mat_BBW_<Surface>_<Variant>.mat
  Textures/
    T_BBW_<Surface>_<Map>_<Size>.<ext>
  Meshes/
    Env/
    Props/
    Weapons/
    Enemies/
  Prefabs/
    Env/
    Props/
    Weapons/
    Enemies/
    VFX/
  Animations/
  Audio/
  LODGroups/
  Collision/
```

Prefix rules:

- `BBW` means Brassworks Breach World.
- Materials: `Mat_BBW_Brass_Aged_A`, `Mat_BBW_Iron_Riveted_B`.
- Textures: `T_BBW_Brass_Aged_BaseColor_2048`, `T_BBW_Brass_Aged_Normal_2048`, `T_BBW_Brass_Aged_Mask_2048`.
- Meshes: `SM_BBW_Corridor_Wall_4m_A`, `SM_BBW_ValveWheel_01`, `SK_BBW_Scrapper_A`.
- Prefabs: `PF_BBW_Corridor_Wall_4m_A`, `PF_BBW_SteamScattergun_Pickup_A`.
- Animations: `AN_BBW_Scrapper_AttackTell_A`, `AN_BBW_PressurePistol_Fire_A`.
- VFX prefabs: `VFX_BBW_SteamVent_Burst_A`, `VFX_BBW_MachineDeath_Scrapper_A`.
- Audio clips: `AUD_BBW_Valve_Clank_A`, `AUD_BBW_Scattergun_Fire_A`.
- Collision meshes: `COL_BBW_Corridor_Wall_4m_A`.
- LOD meshes: append `_LOD0`, `_LOD1`, `_LOD2`, `_LOD3`.

Map suffixes:

- `_BaseColor`
- `_Normal`
- `_Mask` for metallic/roughness/ambient occlusion packing when used.
- `_Emission`
- `_Height` only where parallax/displacement is explicitly approved.

Avoid spaces, vendor names in final asset names, and ambiguous terms like `final`, `new`, or `copy`.

## Unity Import Settings

Use these as baseline import rules for accepted staged assets.

Textures:

- Windows hero: 2048 maximum for weapons, enemies, hero props, and major tileables.
- Windows common: 1024 maximum for common props and repeated modular pieces.
- Android/WebGL: 512 or 1024 maximum unless the asset is a rare hero item.
- VR PC: 1024 or 2048 only where close inspection requires it.
- Quest VR: 512 or 1024, favor atlases and trim sheets.
- Compression: platform override required for Android/WebGL/Quest.
- Mipmaps: enabled for world textures; disabled only for UI or sharp masks.
- Normal maps: imported as normal maps, not default textures.
- sRGB: enabled for base color and emission color; disabled for mask/roughness/metallic/AO/normal data.

Materials:

- Prefer URP-compatible Lit materials or the active project standard.
- Keep shader variants minimal.
- Prefer trim sheets and atlases over many unique materials.
- Use emission sparingly for furnace eyes, gauges, warning lamps, and service indicators.
- Avoid expensive transparency except for glass, steam VFX, and selected gauges.

Meshes:

- Scale: 1 Unity unit = 1 meter.
- Apply transforms before export.
- Pivot should support placement: floor modules pivot at origin/base, wall modules along grid edge, props at bottom center, weapons at grip/hand reference.
- Generate lightmap UVs for static environment meshes.
- Read/Write disabled after import unless a runtime system needs it.
- Optimize mesh data enabled.
- Use consistent forward/up axes from source tool.

Animation:

- Rigged enemies should use Humanoid only if a real humanoid skeleton is useful; most machines should use Generic rigs.
- Root motion disabled unless the gameplay controller is designed for it.
- Clip names must describe gameplay state: idle, patrol, chase, attack tell, attack release, hit, shutdown.
- Loop flags set only for true loops.

Audio:

- Mono for spatial one-shot sounds.
- Stereo for ambience beds and menu music only.
- Vorbis compression for most clips.
- Keep combat one-shots short and layered.
- Provide dry source plus Unity-ready mixed clip when possible.

VFX:

- Use pooled-friendly prefabs.
- Keep particle count budgets per platform.
- Avoid large full-screen flashes.
- Keep steam translucent but short-lived.
- Provide low, medium, and high variants when feasible.

## LOD and Collision Rules

Environment:

- LOD0: full silhouette and important readable detail.
- LOD1: 50 to 60 percent triangle reduction.
- LOD2: 20 to 30 percent of LOD0, major silhouette only.
- LOD3 or impostor: optional for large distant set pieces.
- Collision: simple box/capsule/convex hulls; never use high-detail render mesh as collision for common pieces.

Props:

- Small props may use no LOD if under budget and not repeated heavily.
- Repeated props need at least LOD0/LOD1.
- Collision should match gameplay use: pickup trigger, blocker, or no collision.

Weapons:

- First-person weapon uses separate high-readability presentation mesh.
- World pickup mesh uses lower-detail readable silhouette.
- Collision is trigger or simple bounding shape only.

Enemies:

- Rigged enemy LODs must preserve targetable silhouette and attack-tell readability.
- Hitboxes should be separate simple colliders.
- Avoid tiny moving parts that imply weak points unless gameplay supports them.

VR:

- LOD transitions must be conservative because popping is more noticeable.
- Collision scale must feel physically believable near the player.
- No sharp visual flicker or tiny unreadable labels.

## Platform Variant Budgets

These are planning budgets, not hard engine limits.

| Asset Type | Windows Mid/Low PC | Android Phone | WebGL Browser | SteamVR PC | Meta Quest |
| --- | --- | --- | --- | --- | --- |
| Hero weapon texture | 2048 | 1024 | 1024 | 2048 | 1024 |
| Enemy texture | 2048 or 1024 | 1024 | 1024 | 2048 or 1024 | 1024 |
| Common prop texture | 1024 | 512 | 512 | 1024 | 512 |
| Tileable material | 2048 or 1024 | 1024 or 512 | 1024 or 512 | 2048 or 1024 | 1024 or 512 |
| Materials per modular kit | Low, trim-based | Very low | Very low | Low | Very low |
| Enemy LODs | 3 | 3 | 3 | 3 to 4 | 3 to 4 |
| VFX density | Medium | Low | Low | Medium-low | Low |
| Audio quality | Medium/high | Compressed | Compressed | Medium/high | Compressed |
| Lighting | Baked plus limited realtime | Baked/vertex | Baked/simple | Baked plus careful realtime | Baked/mobile |

## Asset Categories

### 1. Material and Texture Foundation

Goal: build the shared PBR language before producing many models.

Core material set:

- Aged brass: scratched, oily, worn bright at edges.
- Oxidized copper: green-blue patina only in creases, not cyberpunk glow.
- Riveted black iron: chipped, soot-dark, worn exposed edges.
- Wet oil-dark stone: rough slab floor with puddle variation.
- Soot brick: compact dungeon corridor wall.
- Walnut: dark polished grip wood with scratches.
- Cream enamel: gauges, plaques, signs, chipped label plates.
- Furnace iron: heat-browned metal with amber emission accents.
- Rubberized hose/leather gasket: dark flexible pressure seals.
- Glass: slightly warped gauge glass.
- Hazard paint: red/orange pressure markings and worn stripes.
- Service green paint: lift/exit affordance.

Deliverables:

- Tileable textures.
- Trim sheets.
- Decal atlas.
- Material presets.
- Platform downscale variants.

Procedural now:

- Tileables, trim sheets, decals, mask maps, basic normal maps.

Later imported/modeled:

- Photogrammetry-like grunge sources or sculpted surface detail if needed.

### 2. Modular Environment Kit

Goal: replace primitive corridor/room blockout with reusable steampunk dungeon pieces.

Core modules:

- 2m, 4m, and 8m corridor wall sections.
- Floor slabs, grates, hazard floor plates.
- Ceiling pipe panels and support beams.
- Corner pieces, arch frames, door surrounds.
- Pressure gate frame variants.
- Service lift cage and shaft modules.
- Pipe bundles: straight, elbow, valve split, wall clamp, floor riser.
- Boiler wall stack modules.
- Furnace foundry wall and floor variants.
- Secret hatch and maintenance crawl entry.
- Catwalk/rail pieces for later vertical spaces.

Procedural now:

- Grid-aligned blockout-compatible models, rivets, pipe bundles, panels, trim-sheet UVs.

Later imported/modeled:

- Hero room architecture, ornate gate machinery, complex lift mechanism, large boiler assemblies.

### 3. Props and Objective Objects

Goal: create reusable props that make gameplay rules readable.

High-priority props:

- Gear key final art family.
- Pressure gate socket, gauge, warning lamps.
- Wall pressure gauge set, readable at gameplay distance.
- Valve wheel set: small, medium, large, interactable.
- Work order boards and archive plaques.
- Health vial, pressure cartridge pack, rivet bundle, boiler cap ammo.
- Secret cache container variants.
- Tool benches, oil cans, chain spools, brass lanterns.
- Coal bins, furnace shovels, maintenance carts.
- Bellows Node support machinery dressing.
- Master override hoist and Governor Core control hardware.

Procedural now:

- Gauges, valves, crates, boards, pickup shells, simple cache containers.

Later imported/modeled:

- Dense hero props, ornate instruments, complex workbench clusters.

### 4. Weapons

Goal: create final first-person and world-pickup assets for a compact arsenal.

Weapon set:

- Pressure Pistol: starter pneumatic sidearm.
- Steam Scattergun: close-range breaching weapon.
- Rivet Launcher: precision/mechanical armor weapon.
- Optional late-game pressure tool: Arc Valve, Boiler Lance, or Gearcaster, pending mechanics.

Each weapon needs:

- First-person mesh.
- World pickup mesh.
- Low-detail dropped/display mesh.
- Material set.
- Fire, secondary fire, idle, equip, unequip, inspect/check, dry fire, pickup animation plan.
- Muzzle VFX anchors.
- Shell/pressure exhaust anchors.
- Audio layer list.
- VR hand grip notes.

Procedural now:

- PBR materials, pickup silhouettes, gauges, muzzle rings, simple first-person mesh upgrades.

Later imported/modeled:

- Final authored weapon meshes, rigging, first-person animations, VR grip variants.

### 5. Mechanical Enemies

Goal: create mechanical enemies with distinct silhouette, attack tell, weak visual language, and scalable LODs.

Enemy set:

- Scrapper: maintenance cutter machine, baseline melee.
- Boiler Tick: small pressure scout.
- Lancer: ranged valve-rifle security frame.
- Bulwark: heavy furnace-plated riot machine.
- Bellows Node: stationary amplifier/support machine.
- Governor Warden: boss guardian.

Each enemy needs:

- Design sheet: front/side/back silhouette.
- Rig plan: Generic skeleton, moving pistons/gears, colliders.
- LOD0/LOD1/LOD2 meshes.
- Hitbox and navigation footprint.
- Attack tell shapes and color cues.
- Hit, stagger, shutdown, overpressure states.
- VFX sockets: eye, boiler, joints, weapon, exhaust.
- Audio sockets: locomotion, tell, attack, hurt, shutdown.
- Platform variant notes.

Procedural now:

- Silhouette mockups, material swatches, static model references, hit/death VFX concepts.

Later imported/modeled:

- Rigged models, authored animation clips, final texture bakes.

### 6. Animations

Goal: replace procedural motion with authored motion where it improves feel and readability.

Required animation groups:

- Player weapon idle/fire/alternate/equip/dry-fire/check.
- Scrapper idle/chase/attack tell/attack release/hit/shutdown.
- Lancer idle/aim/charge/fire/recover/hit/shutdown.
- Boiler Tick idle/scuttle/leap or burst/rupture/shutdown.
- Bulwark idle/windup/slam/stagger/shutdown.
- Bellows Node idle/pulse/overheat/shutdown.
- Governor Warden idle/stomp/pressure bolt/enrage/shutdown.
- Doors, gates, lifts, hoists, large valves, machinery loops.

Procedural now:

- Timing charts, pose thumbnails, animation placeholder curves.

Later imported/modeled:

- Rigged animation clips from DCC tools or selected animation packs adapted to mechanical rigs.

### 7. VFX

Goal: make pressure, heat, damage, pickup, and machine state clear.

VFX set:

- Pressure Pistol muzzle snap.
- Scattergun blast and slug pressure spear.
- Rivet impact, metal sparks, oil flecks.
- Machine hit, shutdown, boss shutdown.
- Steam vent hazard, furnace heat shimmer.
- Gear key pickup, ammo/health pickup, weapon acquisition.
- Gate unlock/open, lift activation, valve routing.
- Bellows Node pulse and boosted enemy state.
- Objective guidance glints and service-light indicators.

Procedural now:

- Particle texture atlases, color ramps, flipbook masks, simple Unity particle prefabs in staging.

Later imported/modeled:

- Houdini/EmberGen-style flipbooks or bespoke VFX textures if needed.

### 8. Audio

Goal: support tactile steampunk identity with layered mechanical sound.

Audio families:

- Weapons: pressure snap, brass latch, steam vent, dry click, scattergun blast, slug crack, rivet punch.
- Enemies: servo tick, piston step, furnace groan, cutter scrape, valve charge, shutdown.
- Interactables: gear key chime, valve turn, gate unlock, lift start, hoist win.
- Ambience: boiler room bed, distant gears, pipe knocks, furnace rumble, steam leaks.
- UI: brass gauge tick, warning buzzer, objective confirm, menu clockwork.

Procedural now:

- Layered synthetic/mechanical placeholders with naming and routing consistency.

Later imported/recorded:

- Foley metal impacts, real steam/air releases, room tone, bespoke mechanical layers.

### 9. UI and Diegetic Instruments

Goal: keep interface compact and brass-instrument flavored.

Asset set:

- HUD brass gauge frame.
- Ammo/pressure dial.
- Health pressure/boiler gauge.
- Objective strip and indicator lamps.
- Boss pressure gauge.
- Pause/settings panels.
- World-space gauge faces.
- VR-compatible wrist/weapon display concept pieces.

Procedural now:

- Gauge faces, icon masks, enamel panels, lamp sprites.

Later imported/modeled:

- High-polish HUD graphics and VR display meshes.

## Independent Production Order

This order can run without current gameplay code being complete.

### Phase A - Shared Art Foundation

1. Create final material bible for brass, copper, iron, stone, brick, walnut, enamel, glass, furnace glow, oil, hazard paint, and service green.
2. Generate tileable material set and trim sheets.
3. Generate decal atlas: oil, soot, scorch, worn labels, rivets, scratches, leaks.
4. Create platform texture size variants and import-setting table.
5. Run acceptance checklist on all foundation materials.

Output: accepted material pack ready for modular kit and props.

### Phase B - Modular Kit Prototype

1. Build grid-aligned corridor wall/floor/ceiling modules.
2. Build pipe bundle and support beam modules.
3. Build pressure gate, lift, and secret hatch art modules.
4. Create LOD and collision variants.
5. Stage in a clean test scene or isolated prefab folder only when requested.

Output: environment kit candidate that can replace greybox sections incrementally.

### Phase C - Gameplay Props

1. Build final pickup family: gear key, health vial, pressure cartridge, weapon pickup display shells.
2. Build interactable family: valve wheels, gauges, switch boxes, pressure locks.
3. Build storytelling props: plaques, work boards, warning signs, maintenance stamps.
4. Build repeated set dressing: benches, carts, barrels, chains, lanterns, tool racks.

Output: prop library that makes existing mechanics more readable.

### Phase D - Weapon Art

1. Finalize Pressure Pistol design sheet and first-person model.
2. Finalize Steam Scattergun design sheet and first-person model.
3. Build world pickups for both.
4. Build Rivet Launcher concept and blockout only if gameplay direction is settled.
5. Prepare animation and VFX anchor maps.

Output: weapon pack ready for integration and animation.

### Phase E - Enemy Art

1. Produce Scrapper final model first because it is the most common enemy.
2. Produce Lancer final model second because ranged readability affects combat.
3. Produce Bellows Node third because support cues need strong readable state.
4. Produce Bulwark fourth for foundry combat.
5. Produce Governor Warden last because boss presentation depends on final arena needs.
6. Produce Boiler Tick when its mechanics are confirmed.

Output: enemy model and rig candidates with LOD/collider plans.

### Phase F - Animation, VFX, and Audio Polish

1. Author weapon animation pass.
2. Author Scrapper and Lancer motion pass.
3. Author enemy hit/shutdown animation pass.
4. Produce final VFX prefabs for pressure, steam, heat, pickup, and machine death.
5. Produce final audio layers and mixes.

Output: tactile feel upgrade ready for main game integration.

### Phase G - Platform Reduction Pass

1. Downscale textures and atlases.
2. Simplify materials and shaders.
3. Generate lower LOD meshes.
4. Create VFX density tiers.
5. Create compressed audio variants.
6. Mark Windows, Android, WebGL, SteamVR, and Meta Quest readiness.

Output: platform-port-ready asset pack.

## Handoff Units for Other Chat Instances

These units are safe to hand off because they have low dependency on live gameplay code.

| Unit | Scope | Dependencies | Output |
| --- | --- | --- | --- |
| Material Bible | Surface style, PBR values, texture briefs | North-star docs only | Material bible and generation prompts |
| Texture Generator | Tileables, trims, decals | Material Bible | Source textures and Unity import notes |
| Modular Kit Designer | Corridor, floor, wall, pipe kit briefs | Current level scale docs | Mesh briefs, dimensions, LOD/collision specs |
| Prop Pack Designer | Gauges, valves, pickups, signs, benches | Asset catalog and story docs | Prop briefs and acceptance sheets |
| Weapon Art Designer | Pressure Pistol and Scattergun final art | Existing weapon mechanics | Orthographic design sheets, mesh specs |
| Enemy Art Designer | Scrapper/Lancer/Bulwark/Bellows/Warden | Enemy roles in catalog | Silhouettes, rig notes, attack tell specs |
| VFX Designer | Steam, sparks, pressure, heat, pickup effects | Current VFX names and cues | VFX atlas/prefab specs |
| Audio Designer | Mechanical audio families | Current audio cue list | Clip list, layer descriptions, mix notes |
| Platform Optimizer | Variant budgets and reduction plan | Accepted asset list | Android/WebGL/VR variant checklist |

Each unit should update only its assigned doc or a new staging doc, then the main agent can merge results into `AAA_ASSET_CATALOG.md` later.

## Production Tracking Table

Use this table as the starting asset-pack tracker. The main ledger can absorb accepted items later.

| ID | Work Item | Status | Owner Lane | Can Start Now | Notes |
| --- | --- | --- | --- | --- | --- |
| AP-001 | Material bible | briefed | asset-pack | yes | Highest leverage; all later assets inherit this. |
| AP-002 | PBR tileable texture set | briefed | asset-pack | yes | Procedural generation candidate. |
| AP-003 | Trim/decal atlas | briefed | asset-pack | yes | Enables modular environment polish. |
| AP-004 | Corridor module kit | briefed | asset-pack | yes | Must respect existing dungeon scale. |
| AP-005 | Pipe/valve/gauge prop family | briefed | asset-pack | yes | Useful across all levels and VR. |
| AP-006 | Pickup/objective final props | briefed | asset-pack | yes | Gear key, cartridges, health, weapon pickup displays. |
| AP-007 | Pressure Pistol final model brief | briefed | asset-pack | yes | Mechanics already exist. |
| AP-008 | Steam Scattergun final model brief | briefed | asset-pack | yes | Mechanics already exist. |
| AP-009 | Scrapper final model brief | briefed | asset-pack | yes | Common enemy, stable role. |
| AP-010 | Lancer final model brief | briefed | asset-pack | yes | Ranged role already exists. |
| AP-011 | Bellows Node final model brief | briefed | asset-pack | yes | Stable support role, stationary. |
| AP-012 | Bulwark final model brief | briefed | asset-pack | yes | Existing heavy role. |
| AP-013 | Governor Warden final model brief | briefed | asset-pack | limited | Better after arena readability is final. |
| AP-014 | Boiler Tick final model brief | deferred | asset-pack | no | Wait for mechanics. |
| AP-015 | Weapon animation plan | briefed | asset-pack | yes | Clip plan can start; final clips need rig. |
| AP-016 | Enemy animation plan | briefed | asset-pack | yes | Timing/tell plan can start. |
| AP-017 | VFX atlas plan | briefed | asset-pack | yes | Can use current VFX list. |
| AP-018 | Audio asset list | briefed | asset-pack | yes | Can use current audio cue list. |
| AP-019 | Platform variant plan | briefed | asset-pack | yes | Must be applied to every asset. |

## First Five Asset-Pack Priorities

1. Material bible plus PBR tileables and trim sheets.
2. Modular corridor/pipe/gate/lift kit.
3. Gameplay prop family: gauges, valves, pickups, signs, cache objects.
4. Pressure Pistol and Steam Scattergun final art packages.
5. Scrapper and Lancer final enemy model packages.

## Acceptance Gate

An asset is not ready for main integration until it passes the checklist in:

`Documentation/PARALLEL_ASSET_ACCEPTANCE_CHECKLIST.md`

The main game agent should be able to inspect a staged asset and know:

- What it is.
- Which mechanic or level it supports.
- Which platform variants exist.
- How expensive it is.
- Whether it matches the steampunk north star.
- Whether it has LODs, collision, import settings, and naming ready.

## Current Side-Agent Status

As of 2026-05-23 21:31 -04:00:

- This lane has produced the production plan.
- No Unity scenes, scripts, README, existing roadmap/status docs, or generated assets were edited.
- Next safe side-agent task: create the material bible and generation prompts for AP-001 through AP-003 in a new staging document or source-art folder approved by the main agent.
