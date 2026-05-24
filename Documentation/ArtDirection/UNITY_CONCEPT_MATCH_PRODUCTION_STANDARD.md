# Brassworks Breach - Unity Concept Match Production Standard

Status: production planning standard  
Scope: Unity asset requirements for matching the north-star steampunk concept art across the full game  
Primary source art: `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`  
Related direction: `Documentation/STEAMPUNK_NORTH_STAR.md`, `Documentation/AAA_ASSET_CATALOG.md`, `Documentation/PARALLEL_ASSET_PACK_PRODUCTION_PLAN.md`, `Documentation/PARALLEL_LEVEL_PRODUCTION_MAPS.md`, `Documentation/AssetProduction/FinalMaterialsV1/`

## Non-Negotiables

- Development, testing, lookdev renders, and acceptance proof images are Unity-only.
- Do not use Blender or external render tools for lookdev, preview, or acceptance output.
- Windows mid/low PC is the source fidelity tier. Android, WebGL, and VR are reduction targets, not the visual source of truth.
- Every accepted asset must preserve the handmade analog pressure-machine language: rivets, gauges, valve wheels, pipe logic, soot, oil, brass, copper, iron, walnut, amber glass, furnace glow, steam, and wet stone.
- No clean sci-fi corridors, black chrome, floating digital UI, corporate signage, smooth futuristic robots, or purely electronic weapons.

## Target Read

The concept sheet resolves into three Unity production pillars:

1. Corridor: compact industrial dungeon space with dense overhead and wall pipework, wet reflective floor slabs, soot brick, riveted iron, brass rails, gaslight, pressure tanks, gauges, and a large gear-driven pressure door.
2. Machine enemy: clockwork maintenance or containment machinery with boiler torso, exposed pistons, furnace eyes, cutter or hammer work arms, pressure tanks, flywheels, readable attack tells, and shutdown steam.
3. Pressure pistol: brass, blackened iron, copper coil, pressure cylinder, walnut grip, cream gauge, muzzle crown, vents, rivets, and first-person readability.

Assets should look slightly exaggerated and readable in motion, not photogrammetry-clean. Use high-fidelity PBR detail for grime, scratches, dents, soot, oxidation, wetness, and worn bright edges, but keep silhouettes chunky and game-readable.

## Unity Render Target

Use the active Unity render pipeline and URP-compatible Lit materials unless the main project changes standard. The production look depends on the following in-engine treatment:

- Warm amber key lights from gas lamps and furnace sources, usually around `2200K` to `3000K`.
- Red/orange pressure warning lights for locked, hostile, overheat, or hazard states.
- Green service/relief lights for safe route, lift ready, pressure restored, and objective complete.
- Baked or static lighting for dense environment work. Keep dynamic shadow casters limited to gameplay-critical lights and hero moments.
- Reflection probes for wet floor and metal response. Do not rely on realtime planar reflections for normal gameplay scenes.
- Volumetric feeling is created with short-lived steam/smoke particles, layered fog, and lighting contrast, not by filling the screen with transparent overdraw.
- Lookdev proof should include at least one Unity Game view or Scene view render with the asset in project lighting and one neutral turntable or contact view when practical.

## Material Vocabulary

Use FinalMaterialsV1 as the current material vocabulary when available. Material assignment should be functional, not decorative.

| Material family | Unity role | Concept-match requirement |
| --- | --- | --- |
| Aged Brass | trims, valve wheels, rail caps, gauge bezels, weapon frames, fasteners | must show worn bright edges and oily recesses |
| Blackened Riveted Iron | door slabs, wall plates, structural frames, enemy armor plates | must carry mass, soot, chips, and rivet/seam language |
| Wet Oil-Dark Stone | floors, service slabs, damp lower walls | must provide low-angle highlights without mirror-clean reflections |
| Soot Brick | corridor walls, utility rooms, service tunnels | must repeat cleanly and be broken by decals, pipes, braces, or stains every few meters |
| Copper Pipe | pipe bundles, heat lines, pistol coil, pressure-routing language | patina only in creases; avoid bright sci-fi glow |
| Greasy Walnut | weapon grips, tool handles, selected old machinery | directional UVs must support grain direction |
| Cream Enamel Gauge | gauge faces, small labels, instrument panels | must be readable at gameplay distance where it communicates state |
| Amber Glass | gas lamps, gauge glass, furnace lenses, enemy eyes | use sparingly because transparency is expensive |
| Leather Bellows | bellows, flexible pressure joints, old seals | folds must align to mesh shape |
| Hazard Enamel | warning plates, red pressure markings, worn stripe decals | must read immediately as danger or lock feedback |
| Scorch/Oil Decal Atlas | bullet hits, leaks, soot halos, grime breaks | must break tiling and reinforce usage/wear |

Material slot goals:

- Corridor modules: `1` to `3` material slots, preferably atlas/trim based.
- Hero pressure gate: up to `5` material slots.
- Pressure pistol viewmodel: up to `4` material slots.
- Scrapper/Bulwark: up to `4` material slots.
- Warden boss: up to `6` material slots.
- UI/HUD sprites: atlas-first; no unnecessary per-widget materials.

## Geometry Density

These are production budgets for Windows source assets. They are acceptance targets, not engine hard limits.

| Asset class | LOD0 target | Required LODs | Notes |
| --- | ---: | --- | --- |
| 4m wall/floor/ceiling module | `500` to `1,800` tris | LOD0/LOD1 for repeated modules | bake small rivets and seams into normals unless hero-close |
| 4m pipe run with brackets | `800` to `2,500` tris | LOD0/LOD1/LOD2 when repeated heavily | pipe silhouettes matter at corners and valves |
| Valve wheel or wall gauge prop | `400` to `1,500` tris | LOD0/LOD1 when repeated | gauge face can be atlas detail |
| Gas lamp or amber glass fixture | `500` to `2,000` tris | LOD0/LOD1 | transparency area must stay small |
| Pressure door/gate hero prefab | `12k` to `22k` tris | LOD0/LOD1/LOD2 | preserve gear, wheel, gauge, lamp, and socket silhouette |
| Service lift/hoist hero prefab | `10k` to `20k` tris | LOD0/LOD1/LOD2 | simple collision, visible pulley/chain language |
| Pressure pistol first-person viewmodel | `12k` to `24k` tris | viewmodel LOD optional; world pickup LODs required | gauge, coil, tank, muzzle crown, and grip must read at 1080p |
| Pressure pistol world pickup | `3k` to `7k` tris | LOD0/LOD1/LOD2 | must share silhouette with viewmodel |
| Scrapper | `12k` to `20k` tris | LOD0/LOD1/LOD2 | cutter arms, boiler torso, furnace eye, tank, feet readable |
| Bulwark | `18k` to `32k` tris | LOD0/LOD1/LOD2 | heavy mass, hammer arms, furnace plates readable |
| Governor Warden | `35k` to `65k` tris | LOD0/LOD1/LOD2 and optional LOD3 | boss can spend more but must keep VFX quality tiers |

LOD reduction targets:

- LOD1: `50%` to `60%` of LOD0 triangles.
- LOD2: `20%` to `30%` of LOD0 triangles.
- LOD3/impostor: optional for large distant set pieces.
- LOD transitions must not remove gameplay tells, targetable silhouettes, glowing eyes, warning lamps, or objective state cues.

Collision rules:

- Use simple boxes, capsules, convex hulls, or hand-authored low-poly collision.
- Do not use high-detail render meshes as collision for common repeated pieces.
- Corridor collision must preserve player movement first. Pipes, rivets, rail details, and small brackets should usually be non-blocking.
- Door, lift, cover, and combat props need collision that matches gameplay function, not ornamental shape.

## Corridor and Scene Density

Every first-person view should have clear foreground, midground, and destination information.

Minimum density for a production corridor bay:

- Base architecture: floor, wall, ceiling, and trim all arted; no exposed greybox.
- Functional layer: at least `2` distinct steampunk systems visible per 8m of route, such as pipe bundle, gauge bank, rail, boiler tank, valve, pressure lamp, or signage.
- Wear layer: oil/scorch/soot/leak decals or vertex variation breaking repeated surfaces every `3m` to `5m`.
- Readability layer: one clear route cue through light, arrows, pipe direction, floor guide strip, or objective lamp.
- Sound/VFX hook: at least one ambient machinery, vent, lamp buzz, drip, or distant rumble source per major room, with density scaled by platform.

Avoid clutter that blocks combat. In Scrapper lanes, maintain at least `3.5m` usable width. In Bulwark rooms, preserve broad dodge lanes and simple collision. Near doors/lifts, keep at least `4m` turning space for future VR compatibility.

## Wet Floors and Metal Highlights

The north-star corridor depends on low-angle wet highlights and worn metal edges. In Unity:

- Use wetness through smoothness/roughness response, oil-dark color separation, decals, and reflection probes.
- Keep puddle shapes shallow and broken. Avoid mirror-like full-floor reflections.
- Put strongest wet highlights along the golden path, door threshold, and machinery silhouettes.
- Brass/copper highlights should catch edges, rivets, knobs, gauge bezels, rail caps, and pistol contours.
- Blackened iron should stay darker and heavier than brass; it frames the brass detail rather than competing with it.
- If a scene looks brown-on-brown, add material separation through blackened iron, cream enamel labels, red/green lamp states, pale steam, and wet floor highlights.

## Smoke, Steam, Sparks, and Heat

Steam is both identity and gameplay language. It must be controlled.

Ambient steam:

- Short puffs from vents, pipe seams, floor cracks, and pressure tanks.
- Typical vent budget: `12` to `35` visible particles per burst on Windows, `6` to `15` on Android/WebGL/Quest.
- Lifetime target: `0.8s` to `2.5s`; longer only for large room mood with low opacity.
- Do not place dense steam directly over required enemy tells, key UI labels, or interact prompts.

Gameplay steam and pressure:

- Damage hazard steam must have warning state, active state, and relief/safe state.
- Gate open, lift activate, pressure valve, machine death, and weapon fire should each have a distinct pressure puff/spark profile.
- White/grey steam reads as pressure; orange heat shimmer reads as furnace; brass sparks read as machine impact.
- Quality tiers are required for Warden, Bulwark, steam hazards, and dense boiler rooms.

## Camera and Framing Standards

First-person gameplay framing:

- Pressure pistol sits lower right but does not cover objective labels, interact prompts, or center aim.
- The pistol gauge or pressure indicator must be readable at `1920x1080` while moving.
- Door goals and service lifts should be framed by warm/green light at the end of corridors.
- Enemy weak/tell features must face the player during default approach states when possible.

Unity lookdev framing:

- Corridor proof: one 16:9 view from player height looking toward the pressure door or service destination.
- Weapon proof: first-person view plus neutral side/profile view in Unity.
- Enemy proof: front three-quarter view, side silhouette, and in-game distance view in Unity.
- UI proof: `1920x1080`, `1280x720`, and one narrow/aspect stress view with no overlap.

## UI, HUD, and World Label Style

The interface is a compact mechanical instrument panel, not a floating digital screen.

- HUD panels use brass, blackened iron, cream enamel, amber glass, pressure needles, fill gauges, rivets, and small lamps.
- Health, ammo, pressure, objective, key, and boss status should feel like gauges, dials, plates, or lamps.
- World labels should be diegetic plates, stencils, chalk marks, enamel plaques, stamped brass signs, or work-order cards.
- Color language: amber for attention/key route, red/orange for locked/hazard/hostile pressure, green for restored/safe/exit.
- Text must be readable at gameplay distance. Prefer short industrial labels: `GEAR SEAL LOCKED`, `VALVE CREW ONLY`, `SURGE HAZARD`, `CONTAINMENT FRAME ONLINE`.
- No holograms, blue sci-fi panels, flat neon outlines, corporate wayfinding, or clean futuristic fonts.

## Mid/Low Windows PC Constraints

Use these as the first production profile before platform reductions:

- Worst-case hero view per level: roughly `250k` to `500k` visible triangles until profiling replaces this estimate.
- Use static batching or GPU instancing for repeated pipes, rivets, wall modules, valve props, and lamps.
- Keep dynamic shadow-casting lights to a small number per level; bake or fake the rest.
- Prefer `2048` textures for hero weapons, enemies, hero doors, and major tileables.
- Prefer `1024` textures for common props and repeated modules.
- Use atlases and trim sheets to reduce material count in modular environments.
- Keep transparent particles quality-scaled and short-lived.
- Audio should be layered but not excessive; looped ambience must be budgeted and spatially mixed.
- All hero prefabs need LOD, collision, import notes, and platform reduction notes before acceptance.

## Android, WebGL, and VR Downscaling

Android/WebGL:

- Downscale hero textures to `1024`; common props and repeated modules to `512` where acceptable.
- Merge materials through atlases and trim sheets.
- Reduce transparent particles, steam overdraw, heat shimmer, and spark counts.
- Replace most dynamic lights with static lighting, emissive materials, or simple unlit cues.
- Simplify Bulwark and Warden VFX first, then reduce enemy mesh LOD distances.
- Compress audio aggressively and shorten non-critical tails.
- Avoid expensive transparency except key glass/gauge moments and short steam bursts.

VR:

- Preserve comfortable scale, clear collision, stable frame pacing, and conservative LOD transitions.
- Keep route labels larger and more physical; avoid tiny gauge text required for progression.
- Do not force immediate 180-degree turns for reveals or threats.
- Steam cannot obscure the player's near-field view or create constant eye-level fog.
- Avoid high-frequency flicker in lamps, gauges, sparks, and warning lights.
- Keep interactable handles, valve wheels, levers, and weapon grips physically plausible for future hand presence.
- Quest follows mobile-like texture and VFX budgets; SteamVR PC can keep higher textures but still needs VR-safe collision and particle comfort.

## Asset Submission Requirements

Every concept-match asset package should include:

- Unity-ready asset or prefab path proposal.
- Source concept element served: corridor, pressure door, pistol, Scrapper, Bulwark, Warden, signage, UI/HUD, VFX, audio, or material.
- Material assignment table using the project vocabulary.
- Triangle/material/texture counts.
- LOD and collision plan.
- VFX and audio socket/event notes where applicable.
- Unity-only proof images or screenshots.
- Platform reduction notes for Android/WebGL/VR.
- Acceptance gate references from `Documentation/ArtDirection/UNITY_ASSET_ACCEPTANCE_GATES.md`.

Do not replace existing materials, prefabs, or shared names by accident. Version new work and let the main integration lane choose what becomes active.
