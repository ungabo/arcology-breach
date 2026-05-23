# Platform Notes - VR: Steam and Meta

## Role

VR is a future platform family for `Brassworks Breach`. The core Windows game should be built in a way that can support VR later, especially through clean input, interaction, camera, scale, and comfort decisions.

Target stores:

- Steam, likely PC VR through OpenXR.
- Meta Store / Quest, likely standalone VR through OpenXR/Meta tooling.

Do not build VR now. Design so VR is not blocked later.

## Major VR Constraint

VR is not just another camera mode. It changes:

- Locomotion comfort.
- Weapon handling.
- UI placement.
- Interaction distance.
- Enemy attack distance/readability.
- Performance budgets.
- Scale perception.
- Animation and camera assumptions.

## Architecture Rules Starting Now

Avoid:

- Hard-coding gameplay to one desktop camera.
- Camera shake that cannot be disabled.
- HUD-only objective feedback.
- Tiny interact targets.
- Weapon code that assumes only screen-center raycasts forever.
- Enemy attacks with unreadable close-range body clipping.

Prefer:

- Input abstraction.
- Weapon logic separated from weapon presentation.
- Interactions based on reusable components.
- World-space UI support.
- Comfort toggles.
- Real-world scale discipline.
- Enemies readable at VR distances.

## VR Locomotion Planning

Potential VR locomotion modes:

- Smooth locomotion with comfort options.
- Snap turn.
- Optional smooth turn.
- Teleport or dash fallback for comfort.
- Vignette/tunneling option.

The flat fast movement of the Windows game may need a VR-specific mode.

## VR Combat Planning

Potential approaches:

- Keep `Pressure Pistol` as a hand-held VR weapon.
- Aim by controller pose, not screen center.
- Separate hit logic from desktop crosshair logic.
- Add reload/charge interaction only if it is fun and reliable.
- Enemy melee ranges must be tuned to avoid discomfort.

## VR UI Planning

Avoid relying only on flat screen HUD.

VR-compatible UI should use:

- Wrist display.
- Weapon-mounted ammo display.
- World-space objective prompts.
- Diegetic gear-key/pressure-gate indicators.

## SteamVR / PC VR

Planning target:

- OpenXR.
- Desktop PC renders both eyes.
- Higher visual budget than standalone Quest, but still stricter than flat-screen because of VR frame rate.

Performance target:

- 90 FPS preferred.
- 72/80 FPS fallback depending headset.

## Meta Quest / Standalone VR

Planning target:

- OpenXR/Meta tooling.
- Much lower asset and shader budget.
- Similar to Android constraints, but with VR frame-rate pressure.

Performance target:

- 72 FPS minimum planning target.
- Aggressive draw call, shader, and overdraw control.

Likely Quest changes:

- Simplified levels.
- Lower enemy counts.
- Reduced VFX.
- Baked lighting.
- Mobile-friendly shaders.
- Smaller textures.

## Asset Implications

All major assets should be created with future VR in mind:

- Correct scale.
- Clear silhouettes from close range.
- Reduced visual noise near the player's face.
- No tiny text required to understand objectives.
- Avoid excessive flicker.
- Avoid strong forced camera motion.

## Future Checklist

- [ ] Add Unity XR/OpenXR packages in a branch.
- [ ] Create VR input abstraction.
- [ ] Create XR rig scene variant.
- [ ] Create VR weapon presentation.
- [ ] Convert HUD info to world/wrist/weapon UI.
- [ ] Add comfort settings.
- [ ] Create PC VR smoke build.
- [ ] Create Quest smoke build.
- [ ] Profile frame rate and thermals.

## Development Rule

Build the full Windows game first, but keep gameplay systems modular enough for VR:

- weapon mechanics separate from camera presentation
- input separated from gameplay intent
- interactions reusable across mouse, touch, controller, and VR hands
- UI data separate from UI display

Current future VR quality profile assets:

- `Assets/_Project/Data/PcVrQualityProfile.asset`
- `Assets/_Project/Data/MetaQuestQualityProfile.asset`
