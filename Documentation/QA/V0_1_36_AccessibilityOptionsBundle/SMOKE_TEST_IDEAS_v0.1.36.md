# v0.1.36 Accessibility + Options Smoke-Test Ideas

Created: `2026-05-24`

## Purpose

Provide lightweight manual and automation-friendly smoke ideas for later implementation. These are not active tests in this docs-only bundle.

## Smoke 1: First Launch Defaults

Steps:

1. Delete or move the local settings file in a controlled test environment.
2. Launch the packaged Windows build.
3. Open settings from the main menu.
4. Verify defaults for controls, sensitivity, video, audio, accessibility, and captions.
5. Start gameplay and open pause settings.

Pass markers:

- `V0136_OPTIONS_DEFAULTS_PASS`
- `V0136_PAUSE_SETTINGS_ACCESS_PASS`

## Smoke 2: Persistence Round Trip

Steps:

1. Change sensitivity X/Y, invert Y, master volume, flash intensity, high contrast, captions, and display preset.
2. Resume gameplay for a few seconds.
3. Quit to desktop.
4. Relaunch.
5. Confirm all values match the modified settings.
6. Reset each group and confirm defaults return.

Pass markers:

- `V0136_OPTIONS_PERSISTENCE_PASS`
- `V0136_OPTIONS_RESET_PASS`

## Smoke 3: Remap Core Controls

Steps:

1. Rebind `Interact` to a non-default key.
2. Rebind `FireAlternate` to a different mouse button or key.
3. Attempt to unbind `Pause`.
4. Attempt a duplicate binding and confirm warning/replace behavior.
5. Complete a small route interaction using the changed bindings.
6. Reset controls.

Pass markers:

- `V0136_REMAP_CORE_PASS`
- `V0136_REMAP_CONFLICT_PASS`

## Smoke 4: Mouse Sensitivity Axes

Steps:

1. Set X sensitivity high and Y sensitivity low.
2. Verify horizontal turn is visibly faster than vertical look.
3. Invert Y and verify vertical direction changes.
4. Reset look controls.

Pass marker:

- `V0136_LOOK_AXIS_SETTINGS_PASS`

## Smoke 5: Visual Comfort Scaling

Steps:

1. Trigger damage, weapon fire, pickup, enemy tell, and hazard feedback at defaults.
2. Set flash to minimum, camera shake to `0%`, and Reduced Motion to `On`.
3. Repeat the same events.
4. Confirm events remain understandable without strong flash/shake/motion.

Pass markers:

- `V0136_FLASH_REDUCTION_PASS`
- `V0136_SHAKE_REDUCTION_PASS`
- `V0136_REDUCED_MOTION_PASS`

## Smoke 6: Caption Placeholder

Steps:

1. Enable Dialogue + Gameplay Cues or equivalent.
2. Trigger one pickup, one hazard, one enemy tell, and one objective/route update.
3. Verify caption placement, size, background, and timing.
4. Disable captions and verify they disappear.

Pass marker:

- `V0136_CAPTION_PLACEHOLDER_PASS`

## Smoke 7: Readability Scaling

Steps:

1. Test 1080p fullscreen with default HUD scale.
2. Test 720p windowed with large text, large prompt scale, and high contrast.
3. Visit a dark iron area, bright furnace area, steam hazard, route gate, pickup, and boss/exit state.
4. Confirm no text overlap, no clipped buttons, and no critical color-only state.

Pass markers:

- `V0136_READABILITY_1080P_PASS`
- `V0136_READABILITY_720P_PASS`

## Smoke 8: Audio Mix

Steps:

1. At defaults, trigger weapon, pickup, enemy, hazard, ambience, UI, and objective cues.
2. Reduce ambience/music while keeping SFX high.
3. Reduce SFX while captions/readability cues are enabled.
4. Mute master volume and confirm visual/caption alternatives remain for critical events.

Pass marker:

- `V0136_AUDIO_MIX_PASS`

## Smoke 9: Low Spec Preset

Steps:

1. Apply Low Spec preset.
2. Load a representative route with enemies, pickups, hazards, objective text, and exit/boss state.
3. Confirm reduced effects do not hide critical state.
4. Quit and relaunch to confirm preset persistence.

Pass markers:

- `V0136_LOW_SPEC_READABILITY_PASS`
- `V0136_LOW_SPEC_PERSISTENCE_PASS`

## Smoke 10: WebGL/VR Future Safety Review

Review-only steps:

1. List any new direct keyboard/mouse reads introduced by the implementation.
2. List any camera motion that cannot be disabled.
3. List any caption/HUD dependency that assumes desktop screen-space only.
4. List any pause behavior that cannot support pointer unlock.

Pass marker:

- `V0136_PLATFORM_COMPAT_REVIEW_PASS`

