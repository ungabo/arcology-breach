# Pressure Pistol Component Build Plan

Status: active Unity lookdev recovery plan  
Subject: pressure pistol only  
Source: `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`, lower-right pistol panel  
Primary brief: `Documentation/AssetProduction/HighFidelityLookdevRecovery/UNITY_PRESSURE_PISTOL_RECOVERY04_BRIEF.md`  
Current proof note: `Documentation/AssetProduction/HighFidelityLookdevRecovery/UnityPressurePistolProof/HFLD_RECOVERY04_UNITY_PRESSURE_PISTOL_PROOF_REPORT.md`

## Purpose

Recovery04 proved that Unity can generate the required pistol nouns and counts, but the whole-gun pass still mixes too many unsolved problems at once: material response, lighting, primitive shape credibility, gauge readability, coil readability, grip scale, smoke placement, and final composition. The next lane should stop trying to rescue the entire weapon in one render. Each visible asset component must pass a focused Unity proof before it is allowed into the full pressure pistol assembly.

This is a docs-only production plan. It does not authorize gameplay integration, saved gameplay scene edits, build setting changes, Blender work, or external rendering. Unity remains the only development, testing, and lookdev render environment.

## Recommended Order Of Work

1. Prove material swatches.
2. Prove gauge module and coil module as separate high-readability hero details.
3. Prove barrel/tank module and muzzle module as the structural silhouette.
4. Prove grip/hand module with trigger and guard.
5. Integrate accepted modules into the full composition, then add fasteners/plates and steam/smoke only where they support the accepted forms.

If a later composition fails, return to the responsible component proof instead of adding whole-gun decoration to hide the issue.

## Shared Unity Assembly Contract

Use one common weapon coordinate convention across all component proofs:

- `PressurePistolRoot`: receiver/body center.
- Local X: weapon length; muzzle side is negative X, grip side is positive X.
- Local Y: vertical up.
- Local Z: side depth; visible hero-side details should face the review camera.
- Hero camera: first-person 3/4 view with the muzzle pointing left or left-forward and the grip/hand anchoring the lower-right frame.

Each component proof should export or document:

- Unity-only source path, render entrypoint, or temporary-scene method.
- One 1920x1080 review render.
- Count ledger for that component's tick marks, coil turns, rings, fasteners, plates, smoke puffs, or other measurable details.
- Material roles used.
- Anchor points used for full weapon assembly.
- Known gaps that block promotion into the full pistol.

Do not assemble with a component that has no proof render and no passed gate.

## Component 1: Material Calibration

### Target Visual Description

The material language must match the north-star pressure pistol: aged brass with polished worn edges, blackened iron for the main barrel, darker pipe metal for the lower tank and mechanical sockets, hot copper for the coil, cream enamel for the gauge face, glass highlights on the gauge lens, dark walnut/leather for the grip and glove mass, plus soot/grime and smoke.

The material read must come from Unity lighting and PBR-style response, not flat color labels.

### Unity Implementation Strategy

Create a Unity material swatch board before any component hero work. Show each role on at least a cylinder, a small bevel-like block, and a sphere or rounded proxy so reviewers can see edge highlights, curved reflections, and shadow behavior. Use the same warm key, low cool fill, rim/specular light, fog exposure, and render pipeline settings intended for the pistol proof.

Use procedural textures or authored Unity-compatible texture channels as needed, but keep the proof inside Unity. A material calibration scene may use one swatch material per role; later gun modules should consolidate material slots where practical.

### Measurable Acceptance Gates

- At least 9 visible material roles: aged brass, blackened iron, dark pipe metal, hot copper, cream gauge face, glass, walnut or leather, soot/grime, smoke.
- Metal roles show different roughness/smoothness response under the same lights.
- Brass and copper are not interchangeable at normal review size.
- Blackened iron and dark pipe metal are distinguishable by value, roughness, and edge wear.
- Glass uses transparency or specular highlight behavior and does not read as opaque gray plastic.
- Grip material shows grain, crease, seam, or worn-edge breakup.
- Swatch render is at least 1920x1080 and includes the final lookdev lighting setup.

### Failure Signs

- Materials read as flat brown, flat black, or single-hue brass.
- Copper coil color cannot be separated from aged brass.
- Gauge face looks like unlit paper or pure white plastic.
- Glass disappears completely or blocks the gauge.
- Soot/grime is painted uniformly instead of collecting in seams, recesses, and lower surfaces.

### Full Weapon Assembly Use

The accepted swatches become the material library for all later modules. Do not allow a module into assembly if it invents a new unrelated brass, iron, copper, glass, or leather response without updating the swatch board first.

## Component 2: Gauge Module

### Target Visual Description

The gauge is a top-mounted pressure dial with a cream face, brass bezel, red needle, black tick marks, grime ring, glass lens, and physical bracket attachment. It should break the top silhouette and remain readable even in the full first-person composition.

### Unity Implementation Strategy

Build the gauge as its own Unity module with:

- Layered circular face, bezel, rear casing, glass lens, and mount bracket.
- Tick marks as geometry, small meshes, decals, or texture features that remain visible at 1080p.
- Red needle slightly raised above the face.
- Brass rim with bevel-like highlight geometry.
- Fastened bracket that connects to the receiver or barrel frame.

Render it twice: a close component view and a reduced-size preview matching its expected full-weapon scale.

### Measurable Acceptance Gates

- One clearly round gauge face.
- At least 24 visible tick marks; target 28 if matching Recovery04 density.
- One red needle visible over the cream face.
- Brass bezel, rear casing, and mount bracket are distinct shapes.
- Glass lens produces at least one highlight without hiding the tick marks or needle.
- At least 4 fasteners attach the gauge bracket or bezel.
- Gauge remains readable when scaled to its planned full-weapon size in a 1920x1080 render.

### Failure Signs

- Gauge reads as a flat sticker, coin, or clock icon pasted onto the gun.
- Face is too dark, too bright, or covered by reflection.
- Needle or tick marks vanish in the full-scale preview.
- Bracket floats or intersects without construction logic.
- Gauge is blocked by steam, muzzle parts, or the coil housing in a composition test.

### Full Weapon Assembly Use

Mount the gauge on the upper receiver/barrel frame, slightly aft of the muzzle stack and above the coil-side body. It must stay on the visible near side of the hero camera and must not be treated as optional decoration; it is one of the main identity marks of the pressure pistol.

## Component 3: Coil Module

### Target Visual Description

The coil module is an exposed hot-copper pressure or induction coil inside a recessed side window. It should read warmer and more saturated than the surrounding brass, with at least 6 distinct turns, dark backing, frame screws, and believable containment.

### Unity Implementation Strategy

Build a separate coil-window assembly:

- Copper coil as tube segments, a generated helix, or repeated torus/curve proxies.
- Recessed dark backplate to silhouette the coil turns.
- Brass or iron window frame with bevel-like edge geometry.
- Side screws or rivets to sell the access panel.
- Optional glass/heat shield only if it does not reduce readability.
- Subtle emission or warm practical light that does not bloom out the coil shape.

Test it under the final material calibration lights and in a side-on/3/4 preview matching the full pistol.

### Measurable Acceptance Gates

- At least 6 visible coil turns; target 8 when space allows.
- Coil is visibly copper/hot copper, not generic brass.
- Coil sits inside a recessed housing, not on top as a loose spring.
- Housing has at least 4 frame fasteners.
- Dark backing separates the coil from the barrel/body.
- Warm glow or highlight is present but individual turns remain readable.
- Coil preview remains readable at planned full-weapon scale.

### Failure Signs

- Coil becomes a single orange blur.
- Coil reads as a flat stripe, painted line, or random cable bundle.
- Frame is missing, so the coil looks unprotected and unmounted.
- Brass frame and copper coil merge into one material.
- Coil is placed on the far side of the gun and disappears in the hero camera.

### Full Weapon Assembly Use

Place the coil on the visible hero side of the receiver, behind or below the gauge and forward of the grip. It should act as the warm focal accent after the gauge. The coil housing must leave clearance for plates, rivets, and smoke so the turns remain visible in final composition.

## Component 4: Barrel And Tank Module

### Target Visual Description

The weapon body needs two separate cylindrical masses: a chunky upper blackened barrel and a lower pressure tank. The barrel carries the main firearm silhouette; the lower tank proves the pressure-pistol identity. Both need caps, collars, straps, seams, grime, and directional specular highlights.

### Unity Implementation Strategy

Build the upper barrel and lower tank together as the structural spine:

- Upper barrel with blackened iron material, collar rings, end caps, seam bands, and dark wear.
- Lower pressure tank with darker pipe metal or blackened metal, capped ends, support brackets, and straps.
- Brass frame rails or plates tying the cylinders together.
- Anchor points for gauge, coil, muzzle, grip, top valves, side sockets, and steam ports.
- 3/4 proof render that checks separation between the two cylinders.

Use bevel-like rings and small inset gaps to catch highlights because primitive cylinders alone are not enough.

### Measurable Acceptance Gates

- One main upper barrel and one visibly separate lower pressure tank.
- Barrel and tank do not merge into a single dark blob at 1080p.
- Barrel has at least 3 collar/cap/seam bands.
- Tank has at least 2 support straps or brackets plus capped ends.
- At least 8 fasteners appear on straps, collars, or frame plates.
- Blackened iron and lower tank material have visible roughness and edge-wear variation.
- Module includes clear anchor space for gauge, coil, muzzle, and grip.

### Failure Signs

- Lower tank reads as a shadow under the barrel instead of a separate pressure vessel.
- Barrel is a smooth unbroken tube with no construction detail.
- Brass collars sit randomly instead of wrapping seams or endpoints.
- The module only works from side view and collapses in 3/4.
- Later modules have no believable place to attach.

### Full Weapon Assembly Use

This module is the skeleton of the whole pistol. Assemble all major modules to its anchors. Do not proceed to full composition until the barrel/tank proof reads clearly without gauge, coil, muzzle, or smoke helping it.

## Component 5: Muzzle Module

### Target Visual Description

The muzzle is a short nested pressure/nozzle stack on the left-forward end of the weapon. It should have multiple rings, a central aperture, dark interior, brass/iron alternation, and foreshortened depth. It must not look like a flat cap.

### Unity Implementation Strategy

Create a modular muzzle stack that attaches to the upper barrel:

- Nested cylinders or ring meshes with decreasing diameters.
- Dark central nozzle opening.
- Outer collar, inner ring, pressure socket, and small bracket/brace.
- Brass and blackened iron material alternation.
- Optional small side port if it supports steam placement.

Render it both alone and attached to the barrel/tank module to test scale.

### Measurable Acceptance Gates

- At least 3 distinct muzzle rings or stepped forms; target 5 to 6 if the silhouette allows.
- Central aperture or dark nozzle is visible.
- Muzzle has clear depth in 3/4 perspective.
- At least 2 small fasteners, braces, or clamps tie it to the barrel.
- Materials catch edge highlights without turning the muzzle into a bright gold plug.
- Does not block the gauge or coil when the hero camera is applied.

### Failure Signs

- Muzzle reads as a single flat disk.
- Ring stack is too long and turns the pistol into a rifle.
- Brass highlights overpower the rest of the weapon.
- Muzzle silhouette points sideways instead of left-forward.
- It hides the barrel/tank separation.

### Full Weapon Assembly Use

Attach the muzzle to the negative-X end of the upper barrel. Use it to establish left-forward direction and depth, but keep it compact so the gauge, coil, and lower tank remain readable.

## Component 6: Grip And Hand Module

### Target Visual Description

The grip/hand module anchors the lower-right frame and sells first-person scale. It should read as dark walnut and/or leather with creases, seams, worn highlights, a trigger, trigger guard, rear plate, and believable connection to the receiver.

### Unity Implementation Strategy

Build the grip as an angled lower-rear module:

- Walnut or leather grip body with carved/creased surface breakup.
- Gloved hand or hand-mass proxy if needed for first-person scale.
- Trigger and trigger guard as readable separate geometry.
- Rear/side mounting plate with fasteners.
- Lower-right hero preview matching the final camera framing.

The hand mass should support the weapon read, not become the subject.

### Measurable Acceptance Gates

- Grip or hand mass anchors the lower-right of the 1920x1080 review frame.
- Trigger and trigger guard are both visible.
- Grip attaches to the receiver with a plate or bracket, not an invisible intersection.
- Walnut/leather material shows grain, crease, seam, or worn-edge detail.
- Hand/glove scale supports first-person read without hiding the lower tank.
- At least 4 fasteners or stitch/seam details are visible on the grip plate or glove cuff.

### Failure Signs

- Grip looks like a flat brown block.
- Trigger area disappears under the hand or tank.
- Hand/glove becomes oversized and covers the weapon identity details.
- Grip angle contradicts the barrel direction or breaks first-person perspective.
- No physical connection to the receiver is visible.

### Full Weapon Assembly Use

Attach the grip to the positive-X/lower receiver area so it sits in the lower-right of the hero camera. Treat it as the composition anchor; the rest of the pistol should project left-forward from this mass.

## Component 7: Fasteners And Plates

### Target Visual Description

The pressure pistol needs dense mechanical construction: rivets, screws, bolts, collars, straps, brackets, side plates, access panels, and frame rails. These details must look like they hold the weapon together, not like random dots.

### Unity Implementation Strategy

Create a reusable fastener and plate kit:

- At least 3 fastener sizes or silhouettes.
- Brass and dark metal variants.
- Plate shapes for gauge mount, coil frame, barrel collars, tank straps, receiver side plates, and grip mount.
- Placement rules tied to seams, brackets, corners, and load-bearing connections.
- Optional random wear rotation/scale, but not random placement.

Fasteners should be added during component proofs and reconciled in the final full-weapon count.

### Measurable Acceptance Gates

- Full assembled weapon has at least 60 visible fasteners.
- Full assembled weapon has at least 8 distinct plates, straps, brackets, or rails; target 24+ if density allows.
- No single component relies on a repeated identical dot pattern as its only detail.
- Fasteners vary in size or material between major structural zones.
- Fasteners land on actual construction seams, collars, frames, or plates.
- Key identity areas have fasteners: gauge, coil window, barrel collars, lower tank straps, muzzle mount, grip mount.

### Failure Signs

- Details appear as a decorative dot texture instead of hardware.
- Fasteners float, intersect, or ignore perspective.
- There are high counts but low construction clarity.
- Plates cover important gauge, coil, or tank reads.
- Full weapon looks busy but not mechanically understandable.

### Full Weapon Assembly Use

Fasteners and plates are the binding layer across all modules. They should be placed only after each base component reads clearly, then used to clarify how modules connect. If they hide the silhouette or key details, remove or relocate them.

## Component 8: Steam And Smoke

### Target Visual Description

Steam and smoke should support the warm, dark brassworks mood without hiding the weapon. Plumes should emerge from visible ports, valves, or seams. Smoke should create depth and atmosphere while leaving gauge, coil, muzzle, tank, and grip readable.

### Unity Implementation Strategy

Use Unity particle systems, soft transparent quads, or procedural billboard clusters in the temporary lookdev scene. Anchor all plumes to visible ports or vents. Test with the final lighting setup because smoke can easily destroy contrast.

Use steam late in the component sequence: first prove the hard-surface parts, then test smoke as an overlay module.

### Measurable Acceptance Gates

- At least 2 visible steam/pressure port origins before smoke is enabled.
- Smoke/steam uses transparent material, soft edges, and lighting/color that fit the scene.
- Steam does not cover more than a small portion of the gauge face, coil window, muzzle opening, trigger, or grip silhouette.
- Plumes read as depth layers, not flat gray stickers.
- Full render still passes component readability with smoke on.

### Failure Signs

- Smoke hides the failures of geometry or material work.
- Plumes come from nowhere.
- Steam covers the gauge or coil, forcing reviewers to guess.
- Smoke turns the background into the subject.
- Alpha sorting or billboard edges are obvious at review size.

### Full Weapon Assembly Use

Add steam after full hard-surface assembly is readable. Use it to emphasize pressure ports, heat, and brassworks atmosphere, not to solve composition. If smoke makes a component fail readability, the smoke fails.

## Full Composition Entry Criteria

Start a full pressure pistol composition only after these are true:

- Material calibration has passed.
- Gauge and coil both pass at reduced full-weapon scale.
- Barrel/tank module reads without supporting details.
- Muzzle module reads as left-forward depth, not a flat cap.
- Grip/hand module establishes lower-right first-person scale.
- Fastener/plate kit has enough variety to support construction logic.
- Steam/smoke has a port-based test and a clear non-obscuring rule.

The full render may still fail, but the failure should point to a specific module, lighting setup, or camera choice instead of the entire gun collapsing at once.

## Full Assembly Target

The final assembled pressure pistol proof should show:

- Chunky 3/4 first-person pressure pistol.
- Muzzle pointing left or left-forward.
- Leather/walnut grip or gloved hand anchoring the lower-right.
- Separate blackened barrel and lower pressure tank.
- Brass frame, brackets, plates, collars, and dense rivets over dark metal.
- Readable cream-face pressure gauge with red needle, brass bezel, glass highlight, and tick marks.
- Exposed warm copper coil window with at least 6 visible turns.
- Nested muzzle stack with visible depth.
- Trigger and trigger guard.
- Warm practical highlights, low cool fill, dark smoky background, and non-obscuring steam.

Only after this passes component gates and human review should it inform production asset work.
