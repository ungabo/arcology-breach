# Pressure Pistol Lookdev Recovery Checklist

Status: active recovery target  
Supersedes broad three-subject recovery for the next visual pass  
Source concept: `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`, lower-right gun panel  
Rejected comparison: `Documentation/ConceptRenders/RENDER_LOOKDEV_HFLD_Batch01_pressure_pistol_nonshipping.jpg`

## Recovery Focus

Only the pressure pistol is active until its visual direction is accepted. Corridor and Scrapper recovery remain paused so density, material realism, and camera can be solved on one hero prop first.

The next acceptable artifact is either:

- a credible pressure-pistol proof render that is visibly closer to the concept, or
- an honestly labeled planning/reference breakdown if a credible render cannot be produced yet.

## 1. Source Gun Read

- [x] Identify the source gun crop as a first-person 3/4 hero prop, not a side-view diagram.
- [x] Record the major silhouette: chunky horizontal barrel group, lower pressure tank, raised top gauge, exposed coil window, blocky brass frame, short muzzle stack, leather glove/grip mass.
- [x] Create pressure-pistol-only annotated target breakdown JPG.
- [ ] Build a measured silhouette overlay: source crop vs next proof render at the same approximate scale and angle.
- [ ] Build a detail ledger from the source crop with counted fasteners, coil turns, gauge markings, ports, rings, plates, and grip seams.

## 2. Exact Visual Targets

### Silhouette

- [ ] 3/4 first-person angle with the muzzle pointing left and slightly away from camera.
- [ ] Top gauge breaks the upper silhouette clearly.
- [ ] Lower pressure tank creates a second dark cylinder below the barrel.
- [ ] Coil window creates an orange/hot copper detail band on the upper-right side.
- [ ] Grip/glove mass anchors the bottom-right of the frame and provides human scale.
- [ ] Muzzle stack has nested rings/nozzle detail instead of a single flat cap.

### Gauge

- [ ] One large top-mounted pressure gauge with brass bezel, cream face, red needle, glass highlight, grime ring, and tick marks.
- [ ] Gauge face must remain readable at 1080p review scale.
- [ ] Gauge is physically attached by bracket/plate, not floating above the weapon.

### Coil Window

- [ ] Exposed copper coil visible through a rectangular brass/iron housing.
- [ ] At least 6 visible coil turns.
- [ ] Coil glow/hot copper reads warmer and more saturated than surrounding brass.
- [ ] Window frame has bevels, screws, grime, and glass/cover variation if applicable.

### Barrel And Frame

- [ ] Main upper barrel reads as blackened iron, not flat brown or solid black.
- [ ] Aged brass frame wraps and brackets the barrel, gauge, coil window, and side plates.
- [ ] Barrel includes caps, collars, seams, screws, grime bands, and worn edge highlights.
- [ ] Muzzle includes small nested rings and a pressure/nozzle detail.

### Lower Pressure Tank

- [ ] Lower tank is clearly separate from the main barrel.
- [ ] Ends are capped, beveled, and held by straps/brackets.
- [ ] Tank has grime bands, rivets, and directional specular highlights.

### Leather Grip And Glove Area

- [ ] Bottom-right glove/hand area is visible enough to establish first-person scale.
- [ ] Leather has warm dark-brown color, creases, seams, worn highlights, and roughness contrast.
- [ ] Grip/trigger area includes a readable trigger guard, trigger, and attachment plate.

### Rivets And Fasteners

- [ ] At least 60 visible screws/rivets/bolts on the proof render.
- [ ] Fasteners vary in size and placement instead of repeating a single dot style.
- [ ] Key rings, plates, brackets, and the gauge mount use fasteners to communicate construction.

### Steam Ports And Pressure Detail

- [ ] At least 2 visible steam/pressure ports or vent sockets.
- [ ] At least 2 small top valves/caps/stacks.
- [ ] Optional steam plume must come from a visible port and not obscure the gauge or coil.

### Grime And Wear

- [ ] Soot and oil collect around seams, plate edges, gauge mount, muzzle, and lower tank brackets.
- [ ] Brass shows edge polishing, tarnish, and darker recesses.
- [ ] Iron shows scratches, chipped edges, and roughness breakup.
- [ ] Leather shows creases and oily worn highlights.

### Lighting And Camera

- [ ] Camera is close 3/4 first-person, not orthographic side view.
- [ ] Key light is warm amber from upper-left/front-left.
- [ ] Low cool/dark fill preserves the gun silhouette against smoky background.
- [ ] Rim/specular highlights catch gauge bezel, barrel caps, coil housing, muzzle, and lower tank.
- [ ] Background is dark smoky neutral and does not compete with the gun.

## 3. Next Best Production Route

Fastest credible route: Blender offline proof first.

Why:

- The current Batch01 pistol asset is too flat and diagrammatic for a credible high-fidelity proof.
- Blender can quickly build or kitbash bevelled cylinders, plates, rivets, coil turns, ports, steam, leather/glove forms, and dramatic lighting without touching Unity gameplay content.
- A Blender proof can validate silhouette, material language, and lighting before Unity integration.

Use Unity only after the Blender proof is accepted:

- Scene path if needed later: `Assets/_Project/ArtStaging/HighFidelityLookdevRecovery/Scenes/HFLD_Recovery_PressurePistol.unity`
- Do not add it to Build Settings.
- Do not open/save gameplay scenes.
- Keep all assets under `Assets/_Project/ArtStaging/HighFidelityLookdevRecovery/`.

Use 2D paintover/composite only for target clarification:

- Good for annotations and review language.
- Not enough by itself to prove geometry, PBR, lighting, or game-art feasibility.
- Must be labeled planning/reference unless it is clearly a paintover target, not an asset render.

## 4. Deliverable Checklist

- [x] Gun-only recovery checklist created.
- [x] Gun-only acceptance gates created.
- [x] Pressure-pistol target breakdown image created and labeled planning/reference.
- [ ] Measured silhouette overlay created.
- [ ] Density greybox model/proof created.
- [ ] Material swatch test created.
- [ ] First credible Blender proof render created.
- [ ] Human review confirms it is closer to the source concept than Batch01.
- [ ] Unity isolated validation scene created only after offline proof acceptance.

