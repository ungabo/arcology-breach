# Arcology Breach - Cyberpunk Asset Catalog

This catalog tracks the assets needed to grow `Arcology Breach` from a greybox proof of concept into a stylized cyberpunk FPS. Assets should be created in milestone-sized sets, verified in-game, and downgraded as needed for Android and browser/WebGL ports.

Status values: `planned`, `prototype`, `in-progress`, `review`, `approved`, `deferred`.

Priority values: `P0` current milestone blocker, `P1` vertical-slice need, `P2` production quality, `P3` polish/expansion.

## Platform Tier Rule

Every major asset should eventually have three quality targets:

- `Windows`: primary target, mid-to-low gaming PC, highest asset fidelity for this project.
- `Android`: reduced texture size, fewer lights, simpler shaders, lower poly counts, compressed audio.
- `Browser`: WebGL-friendly, small downloads, minimal memory, simple materials, short audio.

## 1. Environment Materials

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| MAT-001 | Greybox wall | P0 | prototype | Existing flat wall material for blockouts. |
| MAT-002 | Greybox floor | P0 | prototype | Existing dark floor material for blockouts. |
| MAT-003 | Wet black concrete | P1 | planned | Dark reflective concrete for Aster Gate service floors. |
| MAT-004 | Black chrome wall panel | P1 | planned | Modular corporate wall with subtle bevels, seams, and fingerprints. |
| MAT-005 | Neon cable trunk | P1 | planned | Dense cable strips with cyan/magenta emissive accents. |
| MAT-006 | Holographic warning glass | P1 | planned | Transparent or translucent signage panels. |
| MAT-007 | Corporate white composite | P2 | planned | Sable Meridian clean-room panels, scratched and vandalized. |
| MAT-008 | Amber hazard strip | P1 | planned | Pickup/objective warning trims and maintenance edges. |
| MAT-009 | Red lockdown surface | P1 | planned | Gate material with red denied-access lighting. |
| MAT-010 | Green emergency exit surface | P1 | planned | Exit/elevator/data-gate material with green glow. |
| MAT-011 | Graffiti decal set | P2 | planned | Lowline tags, resistance marks, access hints, and warnings. |
| MAT-012 | Signal glitch decal set | P2 | planned | Magenta hostile-signal noise and corrupted display patterns. |
| MAT-013 | Scorch and impact decals | P2 | planned | Combat feedback decals for walls, floor, and enemies. |

## 2. Modular Geometry

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| GEO-001 | Greybox blockout kit | P0 | prototype | Existing cube-based walls/floor. |
| GEO-002 | Service corridor module | P1 | planned | Narrow cyberpunk maintenance corridor with cable walls. |
| GEO-003 | Repair bay room kit | P1 | planned | Combat room modules with work gantries and machine bays. |
| GEO-004 | Lockdown gate frame | P1 | planned | Red corporate security gate frame. |
| GEO-005 | Access kiosk | P1 | planned | Objective station that visually explains the access shard. |
| GEO-006 | Emergency lift frame | P1 | planned | Green exit destination object. |
| GEO-007 | Transit control console | P2 | planned | Final-room landmark prop/geometry hybrid. |
| GEO-008 | Server stack wall | P2 | planned | Data-center wall modules with emissive status lights. |
| GEO-009 | Catwalk/rail kit | P2 | planned | Simple vertical-looking geometry, used carefully for readability. |
| GEO-010 | Secret service hatch | P2 | planned | Hidden route/secret entry module. |

## 3. Props and Objective Objects

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| PROP-001 | Access shard placeholder | P0 | prototype | Existing yellow pickup cube. |
| PROP-002 | Lockdown gate placeholder | P0 | prototype | Existing sliding red gate. |
| PROP-003 | Emergency exit placeholder | P0 | prototype | Existing green exit object. |
| PROP-004 | Health injector | P1 | planned | Cyberpunk health pickup, bright red/white medical tech. |
| PROP-005 | Ammo capacitor pack | P1 | planned | Blue/cyan energy-ammo pickup. |
| PROP-006 | Access shard final | P1 | planned | Floating amber data shard or key chip. |
| PROP-007 | Corporate lockdown gate final | P1 | planned | Heavy sliding security gate with red panels and status strips. |
| PROP-008 | Emergency lift/data gate final | P1 | planned | Green-lit exit slab, lift door, or data corridor. |
| PROP-009 | Security camera | P2 | planned | Surveillance prop and possible detection mechanic later. |
| PROP-010 | Drone dock | P2 | planned | Enemy spawn/ambience prop. |
| PROP-011 | Broken vending chassis | P2 | planned | Environmental storytelling prop. |
| PROP-012 | Corpse/abandoned recovery rig | P2 | planned | Non-graphic evidence of failed recovery teams. |
| PROP-013 | Hologram ad pillar | P2 | planned | Cyberpunk scene dressing and light source. |

## 4. Weapons

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| WPN-001 | Pulse Pistol placeholder | P0 | prototype | Existing blocky camera-mounted weapon. |
| WPN-002 | Pulse Pistol final | P1 | planned | Compact black-market sidearm with cyan/magenta energy discharge. |
| WPN-003 | Rail Shotgun | P1 | planned | Close-range breaching rail tool firing magnetic flechettes. |
| WPN-004 | Arc Ripper | P2 | planned | Energy weapon that chains through mechanical targets. |
| WPN-005 | Weapon pickup shells | P2 | planned | World pickup visuals for weapons. |
| WPN-006 | Ammo family | P1 | planned | Capacitor packs, flechette bundles, charge cells. |

## 5. Mechanical Enemies

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| ENEMY-001 | Primitive Scrapper | P0 | prototype | Existing capsule/cube mechanical melee placeholder. |
| ENEMY-002 | Scrapper final | P1 | planned | Maintenance frame with cutter arms, orange hazard plating, lens cluster. |
| ENEMY-003 | Lancer | P1 | planned | Thin ranged security chassis with cyan targeting beam. |
| ENEMY-004 | Bulwark | P2 | planned | Heavy riot-control frame with shield plating and weak rear components. |
| ENEMY-005 | Needle Swarm | P2 | planned | Small surgical drones converted into fast melee/cutting threats. |
| ENEMY-006 | Choir Node | P2 | planned | Stationary signal amplifier that buffs nearby machines. |
| ENEMY-007 | Interdict Core Guardian | P3 | deferred | Later boss or mini-boss encounter. |

## 6. Animations

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| ANIM-001 | Pulse Pistol idle/fire | P1 | prototype | Existing recoil/flash can be expanded. |
| ANIM-002 | Pulse Pistol reload/check | P2 | planned | Optional if reloads become part of gameplay. |
| ANIM-003 | Scrapper idle/chase | P1 | planned | Mechanical servo movement, clear forward pressure. |
| ANIM-004 | Scrapper attack tell | P1 | planned | Cutter arm windup before damage. |
| ANIM-005 | Scrapper hit/death | P1 | planned | Sparks, stagger, collapse, or shutdown. |
| ANIM-006 | Lancer aim/fire | P1 | planned | Beam tell and shot release. |
| ANIM-007 | Gate open/close | P1 | prototype | Existing sliding motion, needs styled animation. |
| ANIM-008 | Pickup bob/glitch | P1 | prototype | Existing bobbing, can become holographic jitter. |

## 7. VFX

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| VFX-001 | Muzzle flash placeholder | P0 | prototype | Existing primitive flash. |
| VFX-002 | Pulse muzzle flash | P1 | planned | Cyan/magenta hard-light burst. |
| VFX-003 | Impact sparks | P1 | planned | Short metal spark or energy flicker. |
| VFX-004 | Machine hit effect | P1 | planned | Sparks, oil flecks, small light burst. |
| VFX-005 | Machine death effect | P1 | planned | Shutdown flash, capacitor pop, smoke. |
| VFX-006 | Access shard pickup effect | P1 | planned | Amber data-glitch pickup effect. |
| VFX-007 | Lockdown gate open effect | P2 | planned | Red strips change to green/white, sparks optional. |
| VFX-008 | Emergency lift activation | P1 | planned | Green scan-line or data gate effect. |
| VFX-009 | Hostile signal field | P2 | planned | Magenta glitch field for hazards/enemy buffs. |

## 8. Audio

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| AUD-001 | Pulse Pistol fire | P1 | prototype | Procedural synthetic snap with capacitor tail. |
| AUD-002 | Empty weapon click | P1 | prototype | Procedural dry failed discharge sound. |
| AUD-003 | Scrapper hurt | P1 | prototype | Procedural servo strain and spark crackle. |
| AUD-004 | Scrapper death | P1 | prototype | Procedural shutdown whine and metal drop impression. |
| AUD-005 | Player hurt | P1 | prototype | Procedural suit impact thud and glitch pulse. |
| AUD-006 | Health pickup | P1 | prototype | Procedural medical injector chirp. |
| AUD-007 | Ammo pickup | P1 | prototype | Procedural capacitor charge tick. |
| AUD-008 | Access shard pickup | P1 | prototype | Procedural amber data unlock cue. |
| AUD-009 | Gate open | P1 | prototype | Procedural mag-lock release and motor slide. |
| AUD-010 | Gate denied | P1 | prototype | Procedural corporate denial buzzer. |
| AUD-011 | Exit/win cue | P1 | prototype | Procedural emergency lift chime/data unlock. |
| AUD-012 | Arcology ambience | P2 | planned | Rain, hum, distant servos, ads, vents. |

## 9. UI and HUD

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| UI-001 | Text HUD | P0 | prototype | Existing plain HUD. |
| UI-002 | Runner HUD frame | P1 | planned | Compact cyan/magenta cyberpunk HUD. |
| UI-003 | Access shard indicator | P1 | planned | Amber shard icon/status. |
| UI-004 | Damage glitch overlay | P1 | prototype | Existing red damage flash can become glitch overlay. |
| UI-005 | Lockdown warning prompt | P1 | planned | Clear denied/access-required feedback. |
| UI-006 | Main menu | P2 | planned | Stylized start/settings/quit. |
| UI-007 | Settings and accessibility | P2 | planned | Sensitivity, volume, resolution, flash intensity, color readability. |

## 10. Level Assets

| ID | Asset | Priority | Status | Description |
| --- | --- | --- | --- | --- |
| LVL-001 | Aster Gate Intake greybox | P0 | prototype | Current generated `Level01`. |
| LVL-002 | Aster Gate combat slice | P1 | planned | Retuned version of current layout. |
| LVL-003 | Aster Gate art pass | P1 | planned | First cyberpunk material, prop, lighting, and signage pass. |
| LVL-004 | Data Stack concept | P2 | planned | Server-heavy second level concept. |
| LVL-005 | Transit Spine concept | P2 | planned | Rail/transit themed level concept. |
| LVL-006 | Lowline market approach | P3 | deferred | Later exterior/lower-city sequence. |

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

- Snaps Prototype Sci-Fi Industrial: modular blockout/reference kit for cyberpunk service corridors.
- Snaps Prototype Sci-Fi Military Base: additional sci-fi modules for scale and room composition.
- BlockOut Prototype Kit: fast layout and encounter scale testing.
- Volumetric Lines: lasers, scanlines, targeting beams, holographic accents.
- Unity Particle Pack / Particle Pack 5x: impact sparks, smoke, machine shutdown VFX.
- Space Robot Kyle: robot placeholder/reference for mechanical enemy shape language.
- FPS Microgame weapon add-ons: weapon-scale and material reference for the Pulse Pistol family.

Do not import large city, terrain, demo, or VR kits into the main project until a scoped task requires them.
