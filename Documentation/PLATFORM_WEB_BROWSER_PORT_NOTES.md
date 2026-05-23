# Platform Notes - Browser / WebGL

## Role

Browser/WebGL is a future port target. Do not build it until the Windows game is much farther along, but keep download size, memory, and shader complexity in mind.

## Browser Goals

The browser build should be a simplified, quick-access version:

- Smaller asset payload.
- Shorter levels or a curated demo slice.
- Simple graphics settings.
- Keyboard/mouse support.
- Potential gamepad support.

## Performance Targets

Planning targets:

- 30 FPS minimum.
- 60 FPS preferred on desktop browsers.
- Fast initial load.
- Avoid memory spikes.

## Download and Memory Budgets

Initial planning targets:

- Keep compressed build size modest.
- Avoid loading the entire full game at startup.
- Favor shared atlases and repeated modular assets.
- Short compressed audio.
- Limit unique high-resolution textures.

## Rendering Constraints

Use:

- Simple materials.
- Baked lighting.
- Minimal post-processing.
- Limited transparent VFX.
- Few realtime lights.

Avoid:

- Large shader variant explosion.
- Heavy reflection/lighting features.
- Massive texture sets.
- Large uncompressed audio.

## Input

Primary:

- Keyboard/mouse.

Needed:

- Browser pointer-lock handling.
- Clear click-to-focus flow.
- Escape/pause behavior that works with browser pointer lock.
- Possible gamepad fallback.

## Gameplay Adjustments

Likely changes:

- Curated level slice rather than full campaign.
- Fewer enemies at once.
- Lower VFX density.
- Simplified audio mix.
- Optional lower-resolution render scale.

## Development Rule

Windows remains primary until the full Windows game is built. WebGL planning should influence asset budgets and shader choices, but browser-specific implementation is deferred.

Current future WebGL quality profile asset:

`Assets/_Project/Data/WebGLBrowserQualityProfile.asset`

## Future Checklist

- [ ] Install/configure WebGL build support.
- [x] Create WebGL quality profile.
- [ ] Test pointer lock and input.
- [ ] Create reduced asset bundles or curated demo content.
- [ ] Build first WebGL smoke.
- [ ] Host locally and test in browser.
- [ ] Profile memory and load time.
