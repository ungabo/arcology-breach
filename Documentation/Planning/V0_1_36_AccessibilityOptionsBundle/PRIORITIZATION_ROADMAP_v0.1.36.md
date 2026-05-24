# v0.1.35 / v0.1.36 / v1 Accessibility And Options Prioritization

Created: `2026-05-24`

## Purpose

Define the practical order for options/accessibility work so the project can keep making big parallel progress without destabilizing Windows gameplay.

## v0.1.35 Stabilization Carry-In

Focus: protect what already exists while feedback polish lands.

Priority order:

1. Preserve existing settings: sensitivity, master volume, flash intensity, resolution, fullscreen, and high contrast.
2. Confirm pause/settings readability still passes after feedback polish assets and UI changes.
3. Keep damage flash, low health, low ammo, pickup, objective, secret, route confirmation, and denied interaction cues from relying on color alone.
4. Ensure pause/settings audio remains restrained and does not mask menu operation.
5. Record any settings regressions as blockers for v0.1.36 implementation.

Recommended output: regression notes only unless the main implementation lane explicitly assigns code changes.

## v0.1.36 Options Bundle Implementation Target

Focus: create a fuller player-facing settings layer.

Priority order:

1. Settings data model and persistence schema.
2. Main menu and pause menu option grouping.
3. Independent mouse sensitivity X/Y plus invert Y/X.
4. Motion/flash/shake controls wired to camera, VFX, and HUD presentation.
5. Audio mix sliders beyond master volume.
6. HUD/text/prompt/reticle scale and high-contrast expansion.
7. Remappable controls for core Windows intents.
8. Caption/subtitle placeholder infrastructure for gameplay cues.
9. Low-spec display presets that preserve readability.
10. QA evidence packet for persistence, pause access, readability, and platform-safe input.

Recommended implementation stance: land vertical slices. For example, sensitivity X/Y should include data, UI, apply, persistence, reset, and smoke evidence before moving to the next large option family.

## v1 Stabilization

Focus: make settings reliable enough for a public Windows package.

Priority order:

1. Settings migration and corrupt-file fallback.
2. Full reset behavior by group and all settings.
3. Complete remapping conflict UX and alternate bindings.
4. Caption coverage for all authored VO/gameplay cue events that ship.
5. Low-spec preset validation in packaged Windows builds.
6. Manual readability pass at 720p, 1080p, fullscreen, and windowed.
7. Manual audio mix pass using quiet, normal, and loud output levels.
8. Accessibility documentation in release/support notes.
9. Platform readiness notes for Android/WebGL/VR with no immediate port claims.

## P0 / P1 / P2 Classification

P0 before Windows v1:

- Pause access to settings.
- Sensitivity and master volume persistence.
- Flash reduction.
- High contrast/readability baseline.
- Low-spec display preset that does not break route/combat readability.
- Settings reset/fallback.

P1 before Windows v1:

- Remappable controls for core actions.
- Independent X/Y sensitivity.
- Camera shake and reduced motion controls.
- Audio mix sliders.
- Caption placeholders for critical gameplay cues.
- HUD/prompt/text scale.

P2 after Windows v1 or during v1 polish if time allows:

- Full subtitle styling suite.
- Multiple color-assist palettes.
- Gamepad glyphs and complete gamepad settings.
- Touch layout editor.
- VR-specific comfort presets.
- Advanced audio routing and per-cue volume categories.

## Dependency Notes

- Remapping depends on stable intent ids.
- Caption placeholders depend on event ids or authored cue keys.
- Low-spec presets depend on quality/profile apply paths.
- Readability scale depends on UI layout resiliency.
- VR comfort depends on camera effects being presentation-layer settings.

## Recommended First Three Implementation Slices

1. Settings schema plus pause/main menu apply/reset/persist smoke.
2. Look controls: sensitivity X/Y, invert X/Y, runtime apply, persistence, reset.
3. Comfort controls: flash, shake, reduced motion, full-screen effects, readability smoke.

These slices reduce immediate player friction and establish the architecture needed for the larger bundle.

