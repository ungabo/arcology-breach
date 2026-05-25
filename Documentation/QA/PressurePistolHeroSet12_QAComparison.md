# Pressure Pistol Hero Set12 QA Comparison

North-star reference: `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`.

## Silhouette

PASS. The Set12 assembly follows the reference pistol's compressed over-under silhouette: blocky rear receiver, long dark barrel, under-slung pressure cylinder, raised top coil array, side gauge, cog muzzle, raked wooden grip, and gloved first-person hand. The weapon now reads as a chunky brassworks tool rather than a simple tube-and-grip placeholder.

## Component Density

PASS. The full hero prefab contains 283 mesh parts, with isolated modules for grip, trigger guard, pressure barrel, coil array, gauge, valve wheels, cog muzzle, glove hand, and full assembly. Detail language includes clamps, rivets, side plates, pipe runs, coil rings, handwheel spokes, gauge ticks, and grime seams.

## Material Realism

PASS. Materials are texture-backed procedural PNGs rather than flat swatches: aged brass has patina and scratches, copper has teal oxidation and heat bands, iron has blued pitting and oily edge wear, amber glass has glow and bubbles, walnut/leather include grain/cracks, and black grime/oil is used in recesses.

## FPS Readability

PASS. The FPS render keeps the raked leather glove, walnut grip, trigger guard, glowing coil array, readable gauge, and cog muzzle in the lower-right first-person composition. The large shapes remain readable while small rivets and valves add close-range inspection detail.

## Known Limitations

- Geometry is procedural Unity mesh composition, so curved pipes and trigger guard arcs are segmented rather than sculpted continuous surfaces.
- No rigging, reload mechanics, recoil animation, particles, or gameplay integration are included in this sidecar.
- Gauge numerals are represented by procedural tick geometry and a printed texture; hand-authored text labels can be added later if the main art lane wants exact markings.
- Hand proxy is a visual framing proxy, not a skinned character hand.

Validation status: PASS.
