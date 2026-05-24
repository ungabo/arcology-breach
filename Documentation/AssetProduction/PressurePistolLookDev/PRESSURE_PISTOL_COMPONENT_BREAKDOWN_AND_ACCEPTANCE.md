# Pressure Pistol Component Breakdown And Acceptance

Status: component-first Unity lookdev lane  
Scope: `Assets/_Project/LookDev/PressurePistolComponents/**`, `Assets/_Project/Editor/PressurePistolLookDevRenderer.cs`, `Documentation/ConceptRenders/PressurePistolComponents/**`, `Documentation/AssetProduction/PressurePistolLookDev/**`  
North-star source: `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`  
Rule: Unity-only. Do not use Blender or external DCC for this lane.

## Lane Goal

Break the pressure pistol into inspectable, reusable component language before another full-gun proof. A component passes only when it reads as a believable steampunk machine part on its own: layered silhouette, aged material separation, grime in recesses, polished worn edges, visible fasteners, and warm gaslight/copper highlights.

This first pass builds only:

- Copper/brass coil pack.
- Pressure gauge/dial.

The remaining components are acceptance targets for later component renders.

## Component Breakdown

| Component | Role In Weapon | Required Shapes | Required Material Read | First-Pass Status |
| --- | --- | --- | --- | --- |
| Copper/brass coil pack | Heat/pressure conversion window and main steampunk identity beat | Separate copper loop turns, dark recessed cavity, upper/lower brass rails, side cheeks, manifolds, lead pipes, screw rows, heat core | Copper must differ from brass; core may glow warm, but metal still needs soot, tarnish, patina, and edge wear | Built in `PPCOMP_001_copper_brass_coil_pack.png` |
| Gauge/dial | Readable pressure state and focal detail | Rear cup, stacked bezel rings, cream face, radial tick marks, numerals, red needle, glass layer, side ports, top valve cap, raised screws | Cream enamel face must stay readable; brass rim needs polished bites; glass highlight cannot wash out ticks | Built in `PPCOMP_002_pressure_gauge_dial.png` |
| Boiler chamber | Chunky pressure reservoir behind/under barrel | Main cylindrical tank, domed caps, band straps, pressure seams, fill plug, small soot vents, bracket feet | Blackened iron or aged brass body with darker oily seams and heat staining around vents | Not built |
| Barrel rings | Muzzle/barrel depth and first-person silhouette | Nested collars, stepped muzzle crown, dark bore, clamp bands, small sight bead or pressure port | Dark bore must be blackened iron; collars must catch brass edge highlights without becoming flat orange | Not built |
| Grip | First-person hand anchor and material contrast | Walnut grip slabs or leather wrap, brass tang, trigger guard mount, butt cap, crease/groove detail | Walnut/leather must be visually distinct from brass/iron; directional grain or creases required | Not built |
| Valves | Functional pressure-routing language | Small valve wheels, knurled caps, pipe sockets, relief stems, capped vents | Brass/copper valves need oily recesses; red/green accents only if tied to pressure state | Not built |
| Screws/rivets | Scale cue and handmade construction | Raised screw heads, slotted cuts, rivet rows, bracket bolts, asymmetric repair fasteners | Bright edge on heads, dark slot line, soot or oil at some bases | Partially built on coil/gauge |
| Material set | Production vocabulary for the viewmodel | Aged brass, blackened riveted iron, copper pipe/coil, cream enamel gauge, amber glass, greasy walnut, leather, soot/oil grime | Use FinalMaterialsV1 texture families where possible; temporary color materials must preserve the same roles | Partially built on coil/gauge |
| Lighting/camera | Review consistency and concept match | Warm amber key, low cool fill, brass rim, dark riveted backing, mild oily ledge reflection, 3/4 component framing | Deep shadows with readable midtones; no full-screen steam/smoke; components must not disappear into black | Built for first two renders |

## Acceptance Checklist

| Gate | Pass Criteria | Coil Pack | Gauge/Dial | Later Components |
| --- | --- | --- | --- | --- |
| Unity-only proof | Render comes from Unity editor/batchmode and writes outside game assets | Required | Required | Required |
| Isolated component | Component can be judged without full-gun silhouette | Required | Required | Required |
| Layered geometry | At least 5 visible sub-shapes, not one primitive | Required | Required | Required |
| Fastener language | Screws/rivets/slots are visible at review size | 20+ target | 10+ target | 8+ per component |
| Material separation | Brass, copper/iron/enamel/glass/leather/wood roles do not collapse into one brown mass | Brass/copper/iron/heat | Brass/enamel/iron/glass/red | Per component role |
| Edge wear | Worn bright accents appear on rims, rails, collars, or screw heads | Required | Required | Required |
| Recess grime | Dark occlusion exists under rails, in cavities, slots, or seams | Required | Required | Required |
| Functional logic | Pipes, valves, coils, vents, and dials feel mechanically connected | Manifolds/leads | Ports/top valve | Required |
| Concept match | Reads as Victorian industrial pressure machinery, not clean sci-fi | Required | Required | Required |
| Camera/lighting | Warm key, cool fill, rim light, dark backing, and not-empty render check | Required | Required | Required |

## Component-Specific Checks

### Coils

- At least `8` separate visible coil turns for component proof; target `10` to `12`.
- Coils sit in a recessed iron/brass window, not floating in space.
- Hot core is warm orange/red, but surrounding copper stays metallic and worn.
- Include at least one tarnish/patina accent and at least one soot/occlusion layer.
- Manifold pipes or pressure leads show where the coil connects to the larger gun.

### Gauge/Dial

- Cream face remains readable at `1600x1000` without labels explaining it.
- Minimum `24` tick marks, target `30+`.
- Red needle, brass hub, and dark counterweight are distinct.
- Glass exists as a subtle highlight layer, not an opaque plate.
- Bezel must include raised screws or rivets and at least two stacked collars/rims.

### Boiler Chamber

- Cylindrical tank body with distinct cap geometry at both ends.
- Band straps use separate brass/iron material from the tank.
- Include pressure seams, drain/fill cap, soot staining, and at least two small ports.
- Must read heavier than the coil/gauge components.

### Barrel Rings

- Muzzle has visible nested depth: dark bore plus at least `3` rings/collars.
- Rings vary in diameter and material so the silhouette is stepped.
- Add clamp screws, soot around the bore, and a polished rim catchlight.

### Grip

- Shape must support a first-person lower-right anchor.
- Walnut grain or leather crease direction follows the grip curve.
- Brass tang, butt cap, and trigger guard mount are separate shapes.
- Avoid smooth toy-plastic brown; use dark recesses, worn edge highlights, and stitch/groove detail.

### Valves

- Valve wheel/cap geometry must be legible without text labels.
- Mount valves to pipes or sockets so they feel functional.
- Include dark oil at bases and bright wear on hand-contact edges.

### Screws/Rivets

- Raised heads should vary slightly by role: brass polished screws, dark iron rivets, slotted heads for inspection plates.
- Slots must be visible as dark cuts or overlaid dark bars.
- Rows should follow rails, brackets, bezels, and straps.

### Materials

- Aged brass: worn edges, oily recesses, muted gold, not saturated orange.
- Copper: warmer/redder than brass with heat staining and patina in creases.
- Blackened iron: heavy, dark, rough, scratched, and used as structural mass.
- Cream enamel: readable gauge face with dark print/ticks.
- Amber glass: small, transparent, glinting, and not overused.
- Walnut/leather: directional grain or crease detail, darker than brass, not flat brown.
- Soot/oil grime: concentrated in cavities, bore, seams, and screw bases.

### Lighting And Camera

- Use warm amber key from upper-left/front-left.
- Add low cool fill so blackened iron remains readable.
- Add brass rim/specular light to expose bevels and raised fasteners.
- Use a dark riveted/pipe backing and low oily ledge reflection for scale.
- Component renders should be `1600x1000` or higher; contact sheet should show all current component proofs.

## Reassembly Gate

Do not attempt another whole-gun render until at least these components pass human review as separate renders:

- Coil pack.
- Gauge/dial.
- Boiler chamber.
- Barrel ring/muzzle stack.
- Grip/trigger guard.
- Valve/port cluster.
- Material swatch proof for brass, copper, iron, cream enamel, glass, walnut/leather, soot/oil.
