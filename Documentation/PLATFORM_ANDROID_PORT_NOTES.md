# Platform Notes - Android Phone

## Role

Android is a future port target. Do not build it until the Windows game is much farther along, but make asset and architecture choices now that can scale down later.

Target: modern Android phones, including mid-range devices.

## Expected Changes From Windows

Android will likely need:

- Touch controls.
- Simplified UI.
- Lower enemy counts.
- Smaller levels or segmented loading.
- Reduced texture sizes.
- Simpler lighting.
- Reduced VFX.
- More aggressive audio compression.
- Shorter sessions and faster resume.

## Performance Targets

Planning targets:

- 30 FPS minimum.
- 60 FPS preferred on stronger devices.
- Keep thermal throttling in mind.
- Avoid long sustained GPU-heavy scenes.

## Memory and Storage

Initial planning budgets:

- Download size: keep as small as practical.
- Texture sizes: 512/1024 for most assets.
- Avoid many large unique textures.
- Favor atlases, trim sheets, and repeated modular assets.
- Audio should be compressed and short.

## Rendering Constraints

Use:

- Baked lighting where possible.
- Simple emissive materials.
- Limited realtime shadows.
- Low overdraw UI.
- Simple particle counts.
- Mobile-friendly shaders.

Avoid:

- Heavy transparency stacks.
- Large screen-space effects.
- Expensive post-processing.
- Many realtime lights.

## Input Design

Needed later:

- Virtual movement stick.
- Swipe/look region.
- Fire button.
- Interact button.
- Weapon switch if multiple weapons exist.
- Pause/settings.
- Aim sensitivity and touch customization.

Consider optional controller support.

## Gameplay Adjustments

Likely changes:

- Slight aim assist.
- Slower enemy pressure.
- Larger interact volumes.
- More generous pickups.
- Simpler encounter layouts.
- Fewer simultaneous drones/enemies.

## Development Rule

Windows remains primary until the full Windows game is built. Android planning should influence asset scalability, but Android-specific implementation is deferred.

## Future Checklist

- [ ] Install/configure Android build support.
- [ ] Create Android quality profile.
- [ ] Add touch input layer.
- [ ] Add mobile HUD layout.
- [ ] Create reduced asset import presets.
- [ ] Build first Android smoke APK.
- [ ] Test on physical mid-range phone.
- [ ] Profile CPU/GPU/thermal behavior.
