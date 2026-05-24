# High Fidelity Asset Breakdown And Standards

Created: 2026-05-23 22:41:03 -04:00  
Status: target standards plus Batch01 non-shipping placeholders

## Corridor And Pressure Door

Target components:

| Component | Required details | Material IDs | LOD0 target | Notes |
| --- | --- | --- | --- | --- |
| Wet floor slab module | 1 m stone slabs, beveled cracks, oil puddle roughness variation | `MAT_HFLD_OilWetStone` | 400-900 tris per unique slab | Tileable but with vertex/UV variation |
| Side masonry wall bay | Oil-dark bricks/blocks, recessed mortar, soot bands | `MAT_HFLD_OilWetStone` | 1.5k-3k tris per 4 m bay | Use trim/decal grime for variation |
| Riveted iron wall plates | Raised seams, rivets, service hatches, chipped edges | `MAT_HFLD_BlackenedRivetedIron`, `MAT_HFLD_AgedBrassHero` | 2k-5k tris per bay | Must not read as clean sci-fi paneling |
| Overhead pipe set | Brass/copper pipes, collars, elbows, pressure taps | `MAT_HFLD_AgedBrassHero`, `MAT_HFLD_GreenOxidizedCopper` | 2k-6k tris per repeatable set | Include diameter hierarchy |
| Guardrail kit | Posts, horizontal rails, brackets, rivets | `MAT_HFLD_AgedBrassHero` | 800-1.8k tris per 4 m segment | Must not block FPS movement if integrated |
| Amber lamp | Caged amber glass, soot above lamp, brass cap | `MAT_HFLD_AmberGlassLit`, `MAT_HFLD_AgedBrassHero` | 800-2.5k tris | Bake/support emissive mask |
| Pressure gauge | Cream face, brass bezel, needle, grime ring | `MAT_HFLD_CreamGaugeFace`, `MAT_HFLD_AgedBrassHero` | 500-1.5k tris | Readable from 2-4 m |
| Vault pressure door | Round iron door, brass gear hub, radial braces, rivet ring, lock bars | `MAT_HFLD_BlackenedRivetedIron`, `MAT_HFLD_AgedBrassHero` | 25k-55k tris hero | Focal asset, 2k textures minimum |

Texture and texel targets:

- Corridor tiling surfaces: 256 px/m baseline, 512 px/m for close wall/floor hero panels.
- Door hero atlas: 2048 BaseColor/Normal/ORM minimum for Windows; optional 4096 authoring source downscaled to 2048.
- Trim sheets: 2048 for shared brass/iron trim; masks packed R=AO, G=roughness, B=metallic.
- Decals: 512-1024 for soot leaks, oil footprints, gauge labels, hazard marks.

LOD targets:

- LOD0: close inspection, full rivets and pipe collars.
- LOD1: 45-60% of LOD0 triangles, keep silhouette rings, pipe diameters, and lamps.
- LOD2: 12-20% of LOD0 triangles, bake rivets/seams to normal maps.
- Collision: simple boxes/capsules only; no render mesh collision.

## Pressure Pistol

Target components:

| Component | Required details | Material IDs | LOD0 target | Notes |
| --- | --- | --- | --- | --- |
| Main barrel | Blackened iron cylinder, brass muzzle, side wear | `MAT_HFLD_BlackenedRivetedIron`, `MAT_HFLD_AgedBrassHero` | 3k-6k tris | First-person silhouette must stay chunky |
| Lower pressure reservoir | Secondary cylinder, capped ends, grime bands | `MAT_HFLD_BlackenedRivetedIron`, `MAT_HFLD_AgedBrassHero` | 2k-4k tris | Distinct from main barrel |
| Coil window | Exposed copper coil inside iron/brass frame | `MAT_HFLD_CopperCoilHot`, `MAT_HFLD_BlackenedRivetedIron` | 3k-7k tris | Key north-star detail |
| Top pressure gauge | Cream face, red needle, brass bezel, glass cover | `MAT_HFLD_CreamGaugeFace`, `MAT_HFLD_AgedBrassHero`, `MAT_HFLD_AmberGlassLit` | 1k-2.5k tris | Must read in first-person view |
| Grip and glove contact | Worn dark leather, seams, brass screws | `MAT_HFLD_LeatherGripDark`, `MAT_HFLD_AgedBrassHero` | 3k-6k tris | Needs authored normal detail |
| Trigger and guard | Brass guard, dark iron trigger | `MAT_HFLD_AgedBrassHero`, `MAT_HFLD_BlackenedRivetedIron` | 800-1.5k tris | Clear silhouette near hand |
| Rivets and plate screws | Side plate fasteners and service seams | `MAT_HFLD_AgedBrassHero` | 500-1.5k tris | Bake small rivets where possible |
| Steam vent sockets | Small ports for VFX anchors | `MAT_HFLD_AgedBrassHero`, `MAT_HFLD_BlackenedRivetedIron` | 300-900 tris | Name anchors in final prefab |

Texture and texel targets:

- First-person weapon: 1024 px/m visual density minimum.
- Windows LOD0: one 2048 weapon atlas plus optional 1024 gauge/emissive detail sheet.
- Authored source can be 4096, but imported Windows default should be 2048 unless the final weapon is uniquely hero-critical.
- Packed ORM required; emission mask optional for coil and gauge glass.

LOD targets:

- LOD0 first-person: 25k-40k tris.
- World pickup LOD0: 8k-15k tris.
- LOD1: 40-50% of LOD0, preserve muzzle, gauge, coil frame, grip silhouette.
- LOD2: 10-15% of LOD0, simplify coils to baked cards or normal detail.

## Scrapper-Like Monster

Target components:

| Component | Required details | Material IDs | LOD0 target | Notes |
| --- | --- | --- | --- | --- |
| Boiler torso | Rounded iron shell, brass bands, rivets, belly gauge | `MAT_HFLD_BlackenedRivetedIron`, `MAT_HFLD_AgedBrassHero`, `MAT_HFLD_CreamGaugeFace` | 10k-18k tris | Primary target mass |
| Brass head | Round mask, amber eyes, grille mouth, grime | `MAT_HFLD_AgedBrassHero`, `MAT_HFLD_AmberGlassLit`, `MAT_HFLD_BlackenedRivetedIron` | 6k-12k tris | Eyes must be readable attack state |
| Steam stacks | Two short chimneys, soot rings, VFX sockets | `MAT_HFLD_AgedBrassHero`, `MAT_HFLD_GreenOxidizedCopper` | 1k-3k tris | Use sockets for steam puffs |
| Exposed gear wheel | Side/back gear, spokes, hub bolts | `MAT_HFLD_AgedBrassHero`, `MAT_HFLD_BlackenedRivetedIron` | 2k-6k tris | Must reinforce mechanical identity |
| Piston arms | Upper/lower rods, brass joints, dark iron sleeves | `MAT_HFLD_AgedBrassHero`, `MAT_HFLD_BlackenedRivetedIron` | 6k-12k tris | Final rig later, static now |
| Saw weapon | Serrated circular saw, oily dark metal, brass hub | `MAT_HFLD_BlackenedRivetedIron`, `MAT_HFLD_AgedBrassHero` | 3k-8k tris | Combat role read |
| Claw weapon | Three brass/iron blades, hinge plate | `MAT_HFLD_AgedBrassHero`, `MAT_HFLD_BlackenedRivetedIron` | 2k-5k tris | Alternate side read |
| Legs and feet | Piston shins, heavy plated feet, ankle bolts | `MAT_HFLD_BlackenedRivetedIron`, `MAT_HFLD_AgedBrassHero` | 8k-16k tris | Foot contact should feel heavy |

Texture and texel targets:

- Enemy atlas: 2048 BaseColor/Normal/ORM for Windows LOD0.
- Optional 1024 emission sheet for eyes, furnace core, and pressure lamps.
- Use shared trim materials where possible; avoid unique 4k enemy textures for common enemies.
- Texel density: 512 px/m body baseline, 768 px/m for head/eyes/weapons.

LOD targets:

- LOD0: 45k-70k tris for close combat hero view.
- LOD1: 20k-30k tris, preserve head, eyes, weapon silhouettes, torso mass.
- LOD2: 6k-10k tris, bake rivets/gears where possible.
- LOD3 or impostor: optional for large enemy counts.
- Collision: capsule body, simple limb/weapon hit volumes only.

## Windows Mid/Low PC Acceptance

- Materials per hero asset: keep under 6 visible material slots where possible.
- Texture memory: corridor modules rely on shared 2048 trims and 1024/2048 tileables; pistol and enemy each get one primary 2048 atlas.
- Draw calls: combine static corridor trim where practical; do not give every pipe bracket a unique material.
- Lights: use baked/static amber lights for corridor dressing; reserve dynamic lights for gameplay-critical flashes.
- Transparency: steam and glass must have low overdraw; use mesh/card proxies and particle budget limits.
- Readability: pistol coil/gauge, enemy eyes/weapons, and pressure door hub must read at normal play distance.
- Scale: 1 Unity unit = 1 m; doors and corridors must support FPS movement and enemy pathing.

## Later Platform Simplification

Android:

- Downscale primary atlases to 1024 or 512 depending on asset importance.
- Merge brass/copper/iron trim into a single lower-cost material where practical.
- Replace steam volume with simple flipbook or fewer particles.
- Use LOD1 as default for enemies and world pickups.

WebGL:

- Prioritize shared trim sheets and compressed texture variants.
- Avoid costly transparency stacks and large unique textures.
- Keep corridor modules small enough for streaming and quick first load.

VR:

- Preserve physical scale, hand readability, and close-up material quality on the pistol and door controls.
- Reduce small noisy normal detail that shimmers near the face.
- Avoid thick steam directly in front of player interaction points.
- Ensure enemy attack tells are readable from wider stereo angles.

## Batch01 Placeholder Acceptance

- [x] Contains component vocabulary for all three north-star subjects.
- [x] Uses stable `HF_` and `MAT_HFLD_` naming for staged assets.
- [x] Produces view-only JPG review renders clearly marked lookdev/non-shipping.
- [x] Produces material swatch PNG and OBJ/MTL source files for possible Unity import.
- [ ] Not final mesh topology.
- [ ] Not rigged.
- [ ] Not UV-unwrapped for final materials.
- [ ] Not tuned in a Unity lighting scene.
- [ ] Needs artist-authored sculpt/texture pass before production integration.
