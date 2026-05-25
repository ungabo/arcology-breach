# HCLL11 Blunt North-Star Comparison And Prioritized Fixes

Reference: `Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png`.
Status: useful visual evidence, still lookdev, not final environment art.

## Verdict

This is closer to the north-star corridor than the prior assembly pass because Set11 finally gives the corridor real object depth: pipe offsets, lamp cages, gauge needles, boiler columns, valve banks, grates, threshold trim, and wet soot cards now read as layered machinery instead of flat wall dressing. The lighting pass also lands the requested mood: mostly black masonry, warm lamp pockets, wet floor hits, and door fog.

It is not final-ish in the production-art sense. The room shell is still primitive Unity staging, the wet floor uses helper glints rather than physically coherent reflections, the fog is card-based rather than volumetric, and there is no authored damage/sculpt pass tying every object into the masonry. It should be accepted as composition/lookdev evidence and rejected as shipping environment art.

## What Works

- The wide shot has a readable pressure-door destination and a corridor silhouette with foreground, midground, and background layers.
- Set11 dressing fixes the biggest CAML10 failure: the sidewall pieces now have visible depth, flanges, spokes, gauges, and bracket overlap.
- RoomMaterialSet10 plus wetness cards gives the floor and masonry the right dark, damp value range for the north-star target.
- Warm gaslights create useful pools of amber, and their reflections help sell the wet flagstone read.
- DoorVaultSet10 plus BDM10 makes the far mechanism feel like a locked pressure system instead of a generic door.

## What Still Fails

- The room envelope is still visibly made from boxes and tiled primitives. It lacks the north-star's hand-authored bevels, chipped corners, recessed mortar, and believable masonry thickness.
- Reflections are staged cards and smooth materials. The main integration pass needs a real wet material response and lighting/reflection setup.
- Fog cards help composition but do not have volumetric depth, self-shadowing, or proper sorting guarantees in gameplay camera movement.
- Object dressing is dense but still too clean in placement. The north-star needs more asymmetric grime history, dents, cable sag, bracket variation, and occluded service clutter.
- No gameplay readability pass has been run. A playable integration still needs collision, scale, path clearance, player weapon/HUD overlap checks, and performance budgets.

## Prioritized Fixes For Main Integration

1. Promote the best Set11 families first: caged gaslights, layered wall pipe runs, gauge clusters, boiler columns, valve banks, floor drains, and threshold trims.
2. Build a real modular dark-masonry shell with bevels, mortar recesses, corner damage, UV-controlled grime, and snap pivots before importing this composition into a playable lane.
3. Replace card-only wet highlights with a validated wet flagstone material setup: roughness variation, puddle masks, reflection probes or screen-space reflections, and lamp reflection breakup.
4. Re-stage the pressure door as the composition anchor: keep it readable from player height, reserve a clean silhouette around the gear/lock center, and let pipes frame rather than cover it.
5. Convert steam/fog into gameplay-safe VFX prefabs with sorting tests, lifetime controls, overdraw budgets, and camera-path readability checks.
6. Add hand-authored grime/damage passes after layout lock: soot halos over lamps, water tracks from pipe leaks, green/brass oxidation at seams, dark contact shadows under every bracket.
7. Run a player-camera validation pass with weapon/HUD present to ensure the beautiful foreground density does not hide enemies, pickups, doors, or traversal cues.

## Shot Metrics Summary

- `Documentation/ConceptRenders/V0_1_56_HeroCorridorLightingLookdevSet11/HCLL11_RENDER_01_wide_corridor.png`: avg luma 0.164, content 99.9%, near-black 0.8%, warm 18.3%, status pass.
- `Documentation/ConceptRenders/V0_1_56_HeroCorridorLightingLookdevSet11/HCLL11_RENDER_02_material_lighting_closeup.png`: avg luma 0.172, content 100.0%, near-black 0.6%, warm 24.0%, status pass.
- `Documentation/ConceptRenders/V0_1_56_HeroCorridorLightingLookdevSet11/HCLL11_RENDER_03_object_dressing_detail.png`: avg luma 0.126, content 82.7%, near-black 23.5%, warm 39.0%, status pass.
- `Documentation/ConceptRenders/V0_1_56_HeroCorridorLightingLookdevSet11/HCLL11_CONTACTSHEET.png`: avg luma 0.147, content 88.0%, near-black 13.6%, warm 21.4%, status pass.
