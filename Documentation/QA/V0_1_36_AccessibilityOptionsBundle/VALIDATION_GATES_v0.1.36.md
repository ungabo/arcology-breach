# v0.1.36 Accessibility + Options Validation Gates

Created: `2026-05-24`

Owned scope: `Documentation/QA/V0_1_36_AccessibilityOptionsBundle/`

## Purpose

Define hold/fail gates for later implementation of the v0.1.36 accessibility and options bundle.

## Required Evidence

Before promotion, collect:

- Settings defaults table.
- Settings persistence evidence from fresh launch, modified settings, quit, relaunch, and reset.
- Main menu and pause menu screenshots or logs showing option access.
- Remap conflict behavior evidence.
- Mouse X/Y sensitivity evidence.
- Motion/flash/shake before/after evidence.
- Caption placeholder evidence for at least one gameplay cue.
- Readability screenshots at 720p and 1080p.
- Audio slider routing evidence.
- Low Spec preset evidence in a packaged Windows build.

## Persistence Gate

Hold if:

- Any player-facing setting resets unexpectedly after relaunch.
- Corrupt or old settings data prevents main menu load.
- Numeric values load outside intended ranges without clamping.
- Reset Controls, Reset Video, Reset Audio, Reset Accessibility, or Reset All are missing after their groups are implemented.
- A setting appears in UI but has no persisted backing field unless explicitly marked session-only.

Pass expectation:

- Defaults apply on first launch.
- Changed values persist across relaunch.
- Reset restores expected defaults.
- Unknown future fields are ignored safely.

## Pause/Menu Access Gate

Hold if:

- Gameplay-critical settings are available only from the main menu.
- Pause cannot be reached after remapping.
- Settings changes trap pointer/cursor state.
- Menu navigation depends only on mouse hover with no future gamepad/touch path.
- Resolution/fullscreen changes cannot be canceled or recovered from.

Pass expectation:

- Pause menu exposes sensitivity, volume, flash, motion/shake, high contrast/readability, and captions once implemented.
- Main menu and pause menu use the same setting ids.
- Escape/cancel behavior is clear and reversible.

## Remapping Gate

Hold if:

- `Pause` can be unbound.
- `Interact`, `FirePrimary`, `Move`, or `Look` can be left unusable without warning.
- Binding labels are saved as data instead of stable intent ids.
- Duplicate bindings silently break one action.
- Mouse buttons or wheel cannot be represented for Windows defaults.

Pass expectation:

- Core Windows controls can be rebound, conflicts are visible, and reset restores defaults.

## Visual Feedback Scaling Gate

Hold if:

- Reduced flash hides damage, hazard, pickup, objective, enemy tell, boss, or exit state.
- Camera shake `0%` still produces strong forced shake.
- Reduced Motion leaves nonessential bob, tremble, chase lights, or menu motion active.
- Low-health or damage overlays obscure reticle, prompts, or central enemies.
- Low Spec preset removes readability-critical feedback.

Pass expectation:

- Reduced effects substitute stable UI, captions, icons, audio, or longer hold times where needed.

## Readability Gate

Hold if:

- Critical state is communicated by color only.
- Settings body text is below the project target of roughly 18 px equivalent where practical.
- Prompt/HUD/text scaling causes overlap at 720p or 1080p.
- High contrast makes route colors ambiguous.
- Caption backgrounds fail against bright furnace, steam, or dark iron scenes.
- Reticle scale hides weak points or enemy tells.

Pass expectation:

- Route, pickup, hazard, boss, exit, and pause/settings state remain readable in normal play.

## Audio Gate

Hold if:

- Master volume does not affect all expected audio.
- SFX, music, ambience, dialogue, UI, or caption-related cues are mislabeled or routed incorrectly after slider implementation.
- Critical enemy/hazard cues are masked by ambience or music at default mix.
- Pause/settings contains loud loops or constant hiss.
- Muted audio leaves no visual/caption alternative for critical gameplay events.

Pass expectation:

- Audio sliders are predictable and critical cues remain perceivable through at least one non-audio channel.

## Platform Compatibility Gate

Hold if:

- New settings require raw keyboard/mouse checks in gameplay systems.
- Pointer-lock behavior is made harder to adapt for WebGL.
- Touch/gamepad/VR future mappings are blocked by setting ids or intent names.
- Camera effects are implemented as unavoidable camera motion.
- Caption/readability systems assume a desktop-only canvas.

Pass expectation:

- Windows remains the best-supported target, but the architecture can still accept future Android, WebGL, gamepad, and VR providers.

