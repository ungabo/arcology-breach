# High Fidelity Lookdev Recovery Acceptance Rubric

Use this rubric before calling any recovery output "high fidelity" or "closer to the north star." A render can be useful while still failing the high-fidelity gate; label it honestly.

## Global File And Scope Checks

| Check | Pass Criteria | Evidence |
| --- | --- | --- |
| Write scope | New/changed files are only in `Documentation/AssetProduction/HighFidelityLookdevRecovery/`, `Documentation/ConceptRenders/`, or optional `Assets/_Project/ArtStaging/HighFidelityLookdevRecovery/`. | `git status --short` and path list. |
| Source concept reference | Render review includes side-by-side comparison to `north-star-steampunk-brassworks-pressure-pistol.png`. | Contact sheet or review doc. |
| Label honesty | Planning sheets are labeled `planning/reference`; non-final renders are labeled `nonshipping` or `proof`. | Filename and visible title block. |
| Render resolution | Single hero proof is at least 1920x1080 JPG/PNG. Concept-layout proof is at least 1536x1024. | Image dimensions. |
| No production scene edits | No changes to gameplay scenes, scripts, build settings, or previous HighFidelityLookdev files. | `git status --short` and file timestamps. |

## Reference Analysis Gate

| Check | Pass Criteria |
| --- | --- |
| Panel breakdown | Corridor, enemy, and pistol regions are separately annotated. |
| Density ledger | Counts exist for pipes, rivets, gauges, lamps, valves, steam plumes, coils, major door braces, and hero fasteners. |
| Palette sampling | At least 8 named material/color targets are sampled or described. |
| Failure comparison | Batch01 is compared to source with direct visual deficiencies listed. |
| Human review | Reviewer agrees the analysis describes the actual concept rather than generic steampunk. |

## Scene Setup Gate

| Check | Pass Criteria |
| --- | --- |
| Isolation path | Unity route, if used, lives at `Assets/_Project/ArtStaging/HighFidelityLookdevRecovery/Scenes/HFLD_Recovery_Lookdev.unity`. |
| Scene exclusion | Recovery scene is not added to Build Settings and does not reference gameplay-only systems. |
| Asset prefixes | Recovery assets/materials use `HFLDR_` and `MAT_HFLDR_` prefixes. |
| Output path | All review renders write to `Documentation/ConceptRenders/`. |
| Reproducibility | Render notes identify tool, source assets, camera, and lighting setup. |

## Camera And Composition Gate

| Check | Pass Criteria |
| --- | --- |
| Corridor framing | Low, wide perspective with door centered or slightly weighted; wet floor foreground covers at least 20% of image height. |
| Door focal read | Door wheel/hub and radial braces are readable at review scale without zooming. |
| Pistol framing | Gauge and coil are unobstructed; leather glove/hand anchors first-person scale. |
| Enemy framing | Head, eyes, torso, saw/claw, gear/flywheel, and feet are visible in one 3/4 pose. |
| Camera consistency | No orthographic flat-diagram look unless explicitly labeled as diagram/reference. |

## Material Realism Gate

| Check | Pass Criteria |
| --- | --- |
| Material role count | At least 8 distinct roles visible: aged brass, blackened iron, dark pipe metal, wet stone, amber glass, copper coil, cream gauge face, leather. |
| PBR maps | Hero materials include base color, normal, and roughness/metallic/ao or equivalent procedural channels. |
| Roughness variety | Wet stone, polished brass edges, soot-dark walls, dull iron, leather, and gauge glass reflect differently. |
| Edge response | Beveled or normal-mapped edges catch visible highlights on pipes, door braces, pistol barrel, and enemy shell. |
| Weathering | Oil, soot, oxidation, scratches, grime rings, and water streaks appear as layered detail, not single flat fills. |

## Lighting Gate

| Check | Pass Criteria |
| --- | --- |
| Color temperature | Dominant practicals are warm amber, approximately 1800K-2600K. |
| Contrast | The image has deep shadow zones, bright lamp/glow highlights, and readable midtone metal forms. |
| Highlight islands | At least 4 distinct warm light sources or glow clusters are visible in corridor proof. |
| Reflections | Wet floor and brass/iron surfaces show directional specular highlights. |
| Bloom control | Lamps and coils glow, but gauge faces, rivets, and wall bricks remain readable. |

## Environment Density Gate

| Check | Pass Criteria |
| --- | --- |
| Pipe count | Corridor proof shows at least 12 pipe runs or segments with 3+ diameters. |
| Rivet count | Pressure door plus nearby wall plates show at least 150 visible rivets/bolts. |
| Gauge count | Corridor proof shows at least 4 readable gauge faces. |
| Lamp count | Corridor proof shows at least 4 practical lamps with cages or housings. |
| Valve count | Corridor proof shows at least 4 valves/wheels/cranks. |
| Steam count | Corridor proof shows at least 5 localized steam plumes with varied opacity. |
| Layering | Foreground, midground, and background each have mechanical dressing. |

## Asset Detail Gate

| Check | Pass Criteria |
| --- | --- |
| Pistol components | Main barrel, lower reservoir, exposed coil, gauge, muzzle, trigger/guard, leather grip/glove, side plates, fasteners, and pressure ports are all visible. |
| Pistol fasteners | At least 60 visible screws/rivets/bolts. |
| Pistol coil | At least 6 visible coil turns with warm copper/emissive accent. |
| Enemy components | Boiler torso, brass head, amber eyes, grille, stacks, side gear, saw, claw, piston legs, heavy feet, and belly gauge are visible. |
| Enemy fasteners | At least 100 visible rivets/bolts across torso, limbs, and weapons. |
| Door components | Radial hub/wheel, braces, lock bars, grille vents, rivet ring, and side mechanical panels are visible. |

## Post And Atmosphere Gate

| Check | Pass Criteria |
| --- | --- |
| Steam quality | Steam has directional motion/shape and does not look like uniform grey circles. |
| Soot and haze | Haze separates depth without hiding the focal asset. |
| Wetness | Floor has high-contrast reflection streaks and puddle/oil breakup. |
| Grade | Overall image reads dark industrial brassworks with warm amber highlights, not beige/orange flat art. |
| Clean export | Final review includes one unannotated render plus optional annotated copy. |

## Human North-Star Review

Pass only if at least two reviewers can answer "yes" to all of these:

- Does it immediately read as the same world as the source concept?
- Does it have comparable density and layered mechanical detail?
- Do the materials feel like metal, wet stone, glass, leather, smoke, and heat rather than flat shapes?
- Is the lighting dramatic, warm, reflective, and high contrast?
- Would the image still look related to the concept if the labels were removed?

