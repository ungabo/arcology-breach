# Brassworks Breach - Unity Asset Acceptance Gates

Status: production planning checklist  
Scope: measurable Unity acceptance gates for concept-match assets  
Companion standard: `Documentation/ArtDirection/UNITY_CONCEPT_MATCH_PRODUCTION_STANDARD.md`

## Required Result Labels

Use one of these labels for every reviewed asset or package:

- `accepted`: ready for main integration or replacement candidate.
- `accepted-with-notes`: usable now; listed notes must be scheduled.
- `revision-required`: concept match or Unity readiness is incomplete.
- `rejected`: wrong style, unsafe for project, or outside allowed pipeline.

Acceptance proof must be generated inside Unity. Do not use Blender or external render tools for acceptance images.

## Common Gate For All Assets

| Gate ID | Check | Pass measure |
| --- | --- | --- |
| COM-01 | North-star role named | Asset declares which concept element it serves: corridor, pressure door, pistol, machine enemy, signage, UI/HUD, VFX, audio, or material. |
| COM-02 | Unity proof | Package includes Unity Game view, Scene view, prefab preview, or Play Mode screenshot proving in-engine read. |
| COM-03 | Style match | Asset uses brass/copper/iron/soot/oil/wet stone/gaslight/steam/analog machinery language and avoids clean sci-fi. |
| COM-04 | Material vocabulary | Materials map to approved families or document a new family request. |
| COM-05 | Texture import notes | Texture max sizes, sRGB/normal/ORM settings, mipmaps, and platform overrides are specified. |
| COM-06 | Geometry budget | Triangle counts are reported and inside the class target or explicitly justified. |
| COM-07 | LOD plan | Repeated, hero, and enemy assets include required LODs or written exception. |
| COM-08 | Collision plan | Collision is simple and gameplay-correct; render mesh is not used for common collision. |
| COM-09 | Lighting behavior | Asset reads under warm amber, red warning, and green service lighting where relevant. |
| COM-10 | Performance note | Windows mid/low PC and Android/WebGL/VR reduction notes are present. |
| COM-11 | Naming/versioning | New files use project-style names and do not overwrite existing active prefabs/materials by filename. |
| COM-12 | Dependencies listed | Required shaders, materials, audio cues, VFX sockets, scripts, or prefabs are named. |

Failing `COM-01`, `COM-02`, `COM-03`, `COM-06`, or `COM-08` blocks acceptance.

## Corridor Module Gate

Applies to wall, floor, ceiling, corner, doorway, pipe-run, railing, lamp, gauge, and reference corridor assemblies.

| Gate ID | Check | Pass measure |
| --- | --- | --- |
| COR-01 | Scale compatibility | Modules support `3.5m` to `5m` main corridors, `2.2m` minimum tight corridors, `3m x 2.8m` standard doors, and `4m+` VR-safe turning space near lifts. |
| COR-02 | Grid alignment | Pieces snap to whole-meter or documented modular increments without visible seams in a test assembly. |
| COR-03 | North-star density | Each 8m test bay includes base architecture, pipe/valve/gauge or machinery layer, wear decals, and route readability cue. |
| COR-04 | Material assignment | Uses soot brick, wet oil-dark stone, blackened riveted iron, aged brass, copper pipe, hazard enamel, and oil/scorch decals where appropriate. |
| COR-05 | Wet floor read | Wet/oily highlights are visible from player height without mirror-clean reflection. |
| COR-06 | Tiling break-up | No unbroken repeated wall/floor texture stretches beyond `5m` without decal, trim, pipe, panel, or lighting interruption. |
| COR-07 | Geometry budget | 4m architecture module is typically `500` to `1,800` tris; 4m pipe run is typically `800` to `2,500` tris. |
| COR-08 | LODs | Repeated modules provide at least LOD0/LOD1; dense pipe/gauge groups provide LOD0/LOD1/LOD2. |
| COR-09 | Collision | Player-facing collision is simple, snag-free, and does not block on rivets, small pipes, brackets, or decorative rails. |
| COR-10 | Lightmap/static readiness | Static environment pieces have generated or authored lightmap UV readiness notes. |
| COR-11 | Route language | Test bay includes amber attention, red locked/hazard, or green service cue where it serves progression. |
| COR-12 | Platform reduction | Android/WebGL/VR notes identify texture downscales, material merges, and particle reductions. |

Required proof: one Unity screenshot of a player-height 8m corridor bay and one neutral module/contact view.

## Pressure Door And Gate Acceptance

Applies to pressure gate, standard pressure door, lift gate, hoist gate, lock socket, and gate frame variants.

| Gate ID | Check | Pass measure |
| --- | --- | --- |
| GATE-01 | Scale | Standard gate supports `3m x 2.8m`; large pressure gate supports `4m` to `6m` width and about `4m` height. |
| GATE-02 | Silhouette | Gear wheel, keyed socket, pressure gauge, riveted slabs, heavy frame, warning lamps, and pipe/cylinder logic read from `12m`. |
| GATE-03 | State colors | Locked/denied is red or red-orange; ready/open/restored is green; key objective attention can be amber. |
| GATE-04 | Material roles | Door mass is blackened riveted iron; mechanical trim is aged brass/copper; warning plates use hazard enamel; gauges use cream enamel and glass. |
| GATE-05 | Animation readiness | Opening parts, wheel rotation, lamp states, steam vents, and sound events have named sockets or animation tracks. |
| GATE-06 | VFX presence | Denied, unlocking, opening, and fully-open states each have VFX notes or prefab references. |
| GATE-07 | Audio presence | Denied, unlock, gear movement, pressure release, and open-stop cues are named or stubbed. |
| GATE-08 | Collision | Closed state blocks correctly; open state clears the player path; decorative gears/rails do not snag. |
| GATE-09 | Geometry budget | Hero pressure gate is typically `12k` to `22k` tris with LOD0/LOD1/LOD2. |
| GATE-10 | Gameplay readability | Player can tell whether the gate needs a key, pressure routing, Warden defeat, or another objective without reading long text. |

Required proof: Unity screenshot of locked state, open/green state, and at least one close view of gauge/socket detail.

## Pressure Pistol Gate

Applies to first-person pressure pistol, world pickup, display stand, ammo feedback, muzzle VFX anchors, and alternate-fire visual pieces.

| Gate ID | Check | Pass measure |
| --- | --- | --- |
| PISTOL-01 | Silhouette | Pressure tank, muzzle crown, barrel, copper coil or side pipe, cream gauge, valve/dump lever, trigger, and walnut grip read in first person. |
| PISTOL-02 | First-person framing | Viewmodel sits lower right, does not cover crosshair/objectives, and gauge/pressure indicator is readable at `1920x1080`. |
| PISTOL-03 | Material roles | Brass frame, blackened iron barrel/receiver, copper pipe/coil, greasy walnut grip, cream gauge, glass, and oily wear are separated. |
| PISTOL-04 | Hero budget | Viewmodel target is `12k` to `24k` tris and up to `4` material slots unless justified. |
| PISTOL-05 | World pickup | Pickup version preserves silhouette at `3k` to `7k` tris and includes LOD0/LOD1/LOD2. |
| PISTOL-06 | Animation sockets | Muzzle, vent, pressure tank, gauge needle, valve/dump lever, chamber, hand grip, and shell/pressure exhaust anchors are named. |
| PISTOL-07 | VFX presence | Primary fire, impact, dry/empty if applicable, alternate pressure burst, and pickup/acquire VFX are named or stubbed. |
| PISTOL-08 | Audio presence | Primary snap, pressure tail, brass latch, alternate valve dump, and pickup/acquire cues are named or stubbed. |
| PISTOL-09 | Readability under lighting | Weapon reads under corridor amber, darker combat, red hazard, and green service lighting. |
| PISTOL-10 | Platform variants | Android/WebGL world pickup and VR grip/display notes are present. |

Required proof: Unity first-person screenshot, neutral side/profile view, and world pickup/display screenshot.

## Scrapper Machinery Gate

Applies to final Scrapper model, rig, VFX sockets, combat state props, and shutdown pieces.

| Gate ID | Check | Pass measure |
| --- | --- | --- |
| SCRAP-01 | Role silhouette | Baseline melee maintenance machine reads as fast, cutter-armed, boiler-bodied, and lower than Bulwark/Warden. |
| SCRAP-02 | Required features | Boiler torso, furnace eye, piston limbs, cutter or claw arms, pressure tank, flywheel/gear detail, blocky feet, and exhaust points are present. |
| SCRAP-03 | Attack tell | Cutter glow, furnace flare, pressure surge, floor warning, steam puff, or equivalent tell reads before damage. |
| SCRAP-04 | Hit/death hooks | Hit sparks/steam and shutdown burst sockets are named for eye, torso, cutters, tank, and joints. |
| SCRAP-05 | Budget | Target `12k` to `20k` tris, up to `4` material slots, LOD0/LOD1/LOD2. |
| SCRAP-06 | Collision | Capsule or simple body/limb hit volumes support movement and targeting without snagging. |
| SCRAP-07 | Distance read | Furnace eye and cutter silhouette read at the expected first-combat distance in Level01. |
| SCRAP-08 | Audio presence | Idle/servo, attack tell, hit, and shutdown cue names are present. |

Required proof: Unity front three-quarter, side silhouette, and in-game Level01 distance screenshot.

## Bulwark Machinery Gate

Applies to final Bulwark model, rig, attack tell props, and foundry presentation.

| Gate ID | Check | Pass measure |
| --- | --- | --- |
| BUL-01 | Role silhouette | Heavy furnace-plated riot/rescue-frame machine reads as wide, slow, and dangerous before it attacks. |
| BUL-02 | Required features | Boiler or furnace core, heavy chest plates, hammer/slam arms, reinforced feet, pressure tank, heat vents, and warning lamps are present. |
| BUL-03 | Attack tell | Hammer lift, pressure timing ring, furnace overpressure, tank steam surge, and ground slam warning or equivalents are readable. |
| BUL-04 | Arena compatibility | Silhouette remains clear in wide foundry rooms with cover around edges. |
| BUL-05 | Budget | Target `18k` to `32k` tris, up to `4` material slots, LOD0/LOD1/LOD2. |
| BUL-06 | Collision | Simple body and attack volumes match gameplay while avoiding snag-heavy decorative geometry. |
| BUL-07 | VFX/audio presence | Slam tell, impact, overpressure, hit, and shutdown cues are named or stubbed. |
| BUL-08 | Platform tiers | VFX quality tiers are documented for Windows, Android/WebGL, and VR. |

Required proof: Unity in-arena screenshot from player height and neutral silhouette/contact view.

## Governor Warden Machinery Gate

Applies to Warden boss, Governor Core arena machinery, shutdown sequence, and boss presentation.

| Gate ID | Check | Pass measure |
| --- | --- | --- |
| WARDEN-01 | Boss silhouette | Reads as guardian of the Master Governor: taller, more complex, and more ceremonial than Scrapper/Bulwark. |
| WARDEN-02 | Required features | Core body, furnace heart, pressure crown or regulator head, pressure cannon/bolt source, heavy legs, exhaust stacks, and readable weak/state lights are present. |
| WARDEN-03 | Phase readability | Normal, enraged half-health, pressure bolt, stomp, and shutdown states have visible differences. |
| WARDEN-04 | Arena landmarks | Boss reads against regulator pylons, master hoist, red hostile pressure lines, and green shutdown relief lines. |
| WARDEN-05 | Budget | Target `35k` to `65k` tris, up to `6` material slots, LOD0/LOD1/LOD2, optional LOD3. |
| WARDEN-06 | VFX tiers | Bolt trail/impact, stomp warning, enrage, hit, and shutdown burst have quality tiers and particle limits. |
| WARDEN-07 | Audio presence | Wake, bolt charge/fire, stomp tell/impact, enrage, hit, shutdown, and hoist unlock cues are named or stubbed. |
| WARDEN-08 | VR orientation | Initial reveal does not require an immediate 180-degree turn and keeps orientation landmarks visible. |

Required proof: Unity arena screenshot from approach, combat-distance boss screenshot, and shutdown/green relief proof when available.

## Signage And Decal Gate

Applies to objective plates, warning signs, route arrows, chevrons, lore plaques, work-order labels, floor strips, leak/scorch/oil decals, and secret clues.

| Gate ID | Check | Pass measure |
| --- | --- | --- |
| SIGN-01 | Style | Looks like enamel, brass, stamped iron, chalk, paper work order, or industrial stencil. |
| SIGN-02 | Copy length | Gameplay signs use short labels readable while moving; lore/work orders can be denser but optional. |
| SIGN-03 | Color language | Amber route/key, red/orange locked/hazard, green service/relief, cream/black for neutral labels. |
| SIGN-04 | Legibility | Primary gameplay sign is readable from `6m` to `10m` at `1920x1080`; critical HUD/world prompts also pass `1280x720`. |
| SIGN-05 | Atlas readiness | Decals/sprites are atlas-friendly and include alpha/import notes. |
| SIGN-06 | Placement rules | Signs point before the decision, confirm at the objective, and do not clutter combat silhouettes. |
| SIGN-07 | Secret language | Secret decals use suspicious wear, missing rivets, chalk marks, green steam, or brighter brass seams without looking mandatory. |
| SIGN-08 | Platform variants | Android/WebGL/VR readability notes are present, including larger VR text where needed. |

Required proof: Unity screenshot from gameplay distance and texture/contact sheet.

## UI And HUD Gate

Applies to health/ammo/pressure, objective HUD, gear key indicator, damage overlay, boss health, menus, prompts, and diegetic instrument panels.

| Gate ID | Check | Pass measure |
| --- | --- | --- |
| HUD-01 | Instrument style | HUD uses brass, blackened iron, cream enamel, glass, needles, gauges, rivets, lamps, and compact plates. |
| HUD-02 | No digital drift | No clean holograms, neon sci-fi outlines, corporate UI panels, or floating digital display language. |
| HUD-03 | Required states | Health, ammo/pressure, objective, key/lock feedback, interact prompt, damage, and boss health have clear states. |
| HUD-04 | Readability | Passes `1920x1080`, `1280x720`, and one narrow/aspect stress view with no text overlap. |
| HUD-05 | Contrast | Text and icon states remain readable over dark corridor, bright wet floor, steam, red hazard, and green lift backgrounds. |
| HUD-06 | Sprite readiness | Nine-slice/panel sprites, icon atlases, import settings, and scale notes are documented. |
| HUD-07 | Motion restraint | Needle bounces, lamp blinks, and damage overlays do not obscure targeting or flicker aggressively. |
| HUD-08 | Controller/VR future | VR notes identify what becomes wrist, weapon, or diegetic display later. |

Required proof: Unity UI screenshots at required resolutions and one gameplay screenshot with pistol visible.

## VFX And Audio Presence Gate

Applies to weapons, enemies, pressure systems, hazards, pickups, objective feedback, ambience, UI, and level transitions.

| Gate ID | Check | Pass measure |
| --- | --- | --- |
| VFXAUD-01 | Event coverage | Every gameplay-critical interaction has both visual and audio feedback or a documented exception. |
| VFXAUD-02 | Pressure vocabulary | Steam puffs, brass sparks, pressure rings, furnace heat, oil/scorch impact, and lamp states are used consistently. |
| VFXAUD-03 | Particle budget | Ambient vents stay short and low density; hero bursts are time-limited and quality-scaled. |
| VFXAUD-04 | Gameplay clarity | VFX do not obscure enemy attack tells, interact prompts, route signs, or player aim at the critical moment. |
| VFXAUD-05 | Audio identity | Cues include pressure snap, brass latch, valve dump, gear movement, pipe knock, furnace rumble, steam vent, and machine strain where relevant. |
| VFXAUD-06 | Spatial mix | World cues are spatialized or otherwise positioned; UI cues remain clean and not overbearing. |
| VFXAUD-07 | Platform tiers | Android/WebGL/VR particle density, overdraw, shimmer, and audio compression notes are present. |
| VFXAUD-08 | Runtime hook names | Prefab/event/socket names are documented for integration. |

Required proof: Unity Play Mode screenshot or short in-engine capture note for VFX, plus cue list for audio presence.

## Review Form Template

Use this template at the bottom of any package review.

```text
Asset/package:
Reviewer:
Date:
Result:
Concept element served:
Unity proof path(s):
Triangle count:
Material slots:
Texture sizes:
LOD status:
Collision status:
VFX/audio status:
Windows status:
Android/WebGL status:
VR status:
Blocking failures:
Accepted notes:
Next integration owner:
```
