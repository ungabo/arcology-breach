# High Fidelity Lookdev Recovery Checklist

Status: recovery planning started after Batch01 visual rejection  
Source concept: `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`  
Allowed output lanes: this folder, `Documentation/ConceptRenders/`, and optional isolated staging under `Assets/_Project/ArtStaging/HighFidelityLookdevRecovery/`

## Current Active Focus

As of the follow-up direction, active recovery is narrowed to the pressure pistol only. Use `HFLD_PRESSURE_PISTOL_RECOVERY_CHECKLIST.md` and `HFLD_PRESSURE_PISTOL_ACCEPTANCE_GATES.md` for the next visual pass. Corridor and Scrapper recovery are paused until the gun reaches an acceptable visual direction.

## Recovery Principle

Batch01 is treated as a failed visual attempt, not a baseline to defend. The next pass must prove density, lighting, materials, and camera composition against the north-star concept before it is called high fidelity.

## 0. Reference Reset

- [x] Open the north-star concept and identify the three targets: brassworks corridor and pressure door, Scrapper-like automaton, first-person pressure pistol.
- [x] Open the rejected Batch01 contact sheet and compare it directly to the concept.
- [x] Record that the current recovery image output is a planning/reference breakdown, not a successful render.
- [ ] Build a one-page visual target board with final accepted crops, palette samples, material notes, and density counters.
- [ ] Freeze a short "do not ship as high fidelity" note for every blockout-only render until it passes the rubric.

## 1. Reference Analysis

- [x] Source image dimensions verified as 1536x1024 RGB.
- [x] Split source into corridor, enemy, and pistol target zones.
- [ ] Make a density ledger from the concept with counted pipes, rivets, gauges, lamps, steam plumes, cylinders, brackets, and major silhouette breaks.
- [ ] Sample the concept palette into named ranges: blackened iron, aged brass, hot copper, wet stone, amber glass, cream gauge face, leather, steam haze.
- [ ] Define target contrast from the source: deep shadow floor/walls, bright lamp cores, warm metal highlights, and wet reflection streaks.

## 2. Scene Setup

- [ ] Create an isolated lookdev workspace only if the next pass is rendered in Unity:
  `Assets/_Project/ArtStaging/HighFidelityLookdevRecovery/Scenes/HFLD_Recovery_Lookdev.unity`.
- [ ] Keep the recovery scene out of Build Settings and do not load or save `Assets/_Project/Scenes/Level*.unity` or `MainMenu.unity`.
- [ ] Duplicate or generate recovery-only materials/assets under `Assets/_Project/ArtStaging/HighFidelityLookdevRecovery/`.
- [ ] Prefix all recovery-only assets with `HFLDR_` or `MAT_HFLDR_`.
- [ ] Place all review JPG/PNG output in `Documentation/ConceptRenders/`.

## 3. Camera And Framing

- [ ] Corridor shot: wide cinematic frame, low eye height, one-point perspective toward the pressure door, visible wet floor reflections in the foreground.
- [ ] Pressure door: centered hero focal asset occupying roughly 20-35% of image width, with radial gear and rivet ring readable.
- [ ] Pistol shot: first-person 3/4 view, muzzle pointing left, top gauge and coil window unobstructed, leather glove/hand providing scale.
- [ ] Enemy shot: 3/4 front view, saw/claw/gear wheel/steam stacks visible, heavy feet grounded.
- [ ] Render proof at minimum 1920x1080 for single-shot review, or 1536x1024 for concept-match contact-sheet composition.

## 4. Material Realism

- [ ] Replace flat color fills with PBR materials: base color, normal, packed ORM, and emission where needed.
- [ ] Author visible roughness variation for wet stone, aged brass, blackened iron, leather, copper coil, and cream gauge faces.
- [ ] Add bevels or normal-map edge wear to every hero metal form so highlights catch on rims and rivets.
- [ ] Add oil, soot, oxidation, scratches, grime rings, and water streak decals.
- [ ] Use at least 8 visually distinct material roles in the recovery render: blackened iron, aged brass, darker pipe metal, wet stone, amber glass, copper coil, cream gauge face, leather.

## 5. Lighting

- [ ] Use warm practical lamps as the dominant light source: target amber range approximately 1800K-2600K.
- [ ] Keep fill light low and neutral/cool enough that the scene remains high contrast.
- [ ] Add specular reflection probes or equivalent environment reflection for wet floors and brass.
- [ ] Add controlled bloom/glow to lamp glass, hot copper coils, and enemy eyes without washing out dark wall detail.
- [ ] Verify the final shot has bright highlight islands and deep shadows; it must not read as evenly lit UI/vector art.

## 6. Environment Density

- [ ] Corridor proof must include at least 12 visible pipe runs or pipe segments with diameter variation.
- [ ] Corridor proof must include at least 4 practical lamps, 4 readable gauges, 4 valves/wheels, and 5 steam plumes.
- [ ] Pressure door must include at least 1 large radial wheel/hub, 6-8 radial braces, 2 lock bars, 2 grille vents, and 150 visible rivets/bolts.
- [ ] Wall/floor set must include masonry blocks, riveted plates, railings, brackets, floor seams, puddles, and grime decals.
- [ ] Foreground, midground, and background must each contain mechanical detail, not just a decorated back wall.

## 7. Asset Detail

- [ ] Pressure pistol must include main barrel, lower reservoir, exposed copper coil, top gauge, front muzzle assembly, trigger/guard, leather grip/glove, side plates, screws/rivets, and at least 2 steam/pressure ports.
- [ ] Pistol proof must show 60+ visible screws/rivets/fasteners and at least 6 coil turns.
- [ ] Scrapper must include boiler torso, brass head, amber eyes, mouth grille, steam stacks, side gear/flywheel, saw arm, claw arm, piston legs, heavy feet, belly gauge, and 100+ visible rivets.
- [ ] All hero cylindrical forms need bevels, caps, seams, collars, and grime bands.
- [ ] No primitive-only silhouette may pass without secondary details and material breakup.

## 8. Post And Atmosphere

- [ ] Add localized steam with directional drift and opacity variation.
- [ ] Add smoky background haze while preserving silhouettes of hero assets.
- [ ] Add wet-floor reflection streaks, not generic blur blobs.
- [ ] Grade toward warm brass highlights, dark oil-black shadows, and muted stone walls.
- [ ] Export one clean render and one annotated review copy when a render is ready.

## 9. Review Gates

- [x] Gate 0: failed Batch01 acknowledged and documented.
- [x] Gate 1: planning/reference breakdown exists in `Documentation/ConceptRenders/`.
- [ ] Gate 2: density greybox screenshot passes counts before material polish begins.
- [ ] Gate 3: material swatches pass PBR/roughness/edge-wear review before hero render.
- [ ] Gate 4: first hero proof render passes objective checklist and human north-star comparison.
- [ ] Gate 5: only after Gate 4, decide whether to promote the Unity lookdev scene into a production art task.
