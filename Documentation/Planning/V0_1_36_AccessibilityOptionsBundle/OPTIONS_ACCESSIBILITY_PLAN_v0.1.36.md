# v0.1.36 Options And Accessibility Plan

Created: `2026-05-24`

## Purpose

Define a bundled options/accessibility target for later implementation. The goal is a practical v1-ready options layer that improves usability now and avoids expensive platform rewrites later.

## Design Principles

- Windows keyboard/mouse remains the primary v1 path.
- Every option needs a clear default, min/max where relevant, persistence behavior, reset behavior, and pause-menu access.
- Options must be safe to apply during gameplay unless explicitly marked as menu-only.
- Accessibility features must preserve challenge while reducing avoidable discomfort, unreadability, or device friction.
- Color, motion, audio, and text should reinforce each other; no critical state should depend on only one channel.

## Menu Structure

Recommended top-level groups:

| Group | Purpose | Required For v0.1.36 Planning |
| --- | --- | --- |
| Controls | Remapping, sensitivity, invert, device hints | Yes |
| Video | resolution, fullscreen, low-spec presets, brightness/readability | Yes |
| Audio | mix sliders, mute behavior, caption placeholders | Yes |
| Accessibility | motion, flash, shake, color/readability, captions | Yes |
| Reset | defaults per group and all settings | Yes |

Pause-menu settings should expose the same gameplay-critical options as the main menu. Resolution/fullscreen may remain main-menu-first if runtime switching is unstable, but sensitivity, volume, flash, motion, shake, captions, and readability should be adjustable while paused.

## Remappable Controls

### Core Intent List

Use gameplay intents as the public remapping surface:

| Intent | Default Windows Binding | Notes |
| --- | --- | --- |
| MoveForward | `W` | Part of Move2D axis. |
| MoveBackward | `S` | Part of Move2D axis. |
| StrafeLeft | `A` | Part of Move2D axis. |
| StrafeRight | `D` | Part of Move2D axis. |
| Look | Mouse delta | Not key-remapped; configurable by sensitivity/invert. |
| FirePrimary | Left Mouse | Future gamepad/VR trigger equivalent. |
| FireAlternate | Right Mouse | Existing pressure-burst style action should remain separate. |
| Interact | `E` | Route-critical; conflict protection required. |
| ReloadOrVent | `R` if implemented | Reserve even if not active. |
| WeaponSlot1 | `1` | Keep optional if weapon count changes. |
| WeaponSlot2 | `2` | Keep optional if weapon count changes. |
| WeaponNext | Mouse Wheel Down | Optional alternate mapping. |
| WeaponPrevious | Mouse Wheel Up | Optional alternate mapping. |
| JumpOrDodge | `Space` if implemented | Reserve for future movement. |
| CrouchOrComfort | `Left Ctrl` if implemented | Reserve for future movement/VR comfort variant. |
| Pause | `Escape` | Always available; do not allow unbound. |
| Confirm | `Enter` / Left Mouse | Menu navigation. |
| Cancel | `Escape` / Right Mouse | Menu navigation; platform-specific behavior for WebGL pointer lock. |

### Remap Rules

- Do not allow `Pause` to be unbound.
- Warn on conflicts and let players choose whether to replace the old binding.
- Keep a "Reset Controls" action separate from "Reset All Settings".
- Support keyboard, mouse buttons, and mouse wheel for Windows.
- Store remaps by intent id, not by UI label, so text can change without breaking saves.
- Keep display names platform-aware: `Left Mouse`, `LMB`, and controller glyphs should be presentation, not data.
- Reserve a second binding slot per intent if feasible; this helps accessibility, gamepad fallback, and QA.

## Mouse Sensitivity Axes

Recommended settings:

| Setting | Default | Range | Notes |
| --- | --- | --- | --- |
| Mouse Sensitivity X | `1.00` | `0.10` to `5.00` | Horizontal look multiplier. |
| Mouse Sensitivity Y | `1.00` | `0.10` to `5.00` | Vertical look multiplier. |
| Invert Look Y | `Off` | `On/Off` | Common accessibility preference. |
| Invert Look X | `Off` | `On/Off` | Useful for some devices and VR/gamepad experiments. |
| Aim Sensitivity Multiplier | `1.00` | `0.25` to `1.50` | Reserve for future zoom/aim states. |
| Menu Cursor Sensitivity | platform default | Optional | Avoid tying UI navigation to gameplay look. |

X/Y split is important for players with asymmetric desk space, trackballs, accessibility devices, touch-look tuning, and future VR/gamepad turn mapping.

## Motion, Flash, And Shake Controls

Recommended settings:

| Setting | Default | Low/Reduced Behavior |
| --- | --- | --- |
| Reduced Motion | `Off` | Disables nonessential bobbing, gauge tremble, looping chase animations, menu parallax, and strong camera presentation effects. |
| Camera Shake | `100%` | Slider from `0%` to `100%`; `0%` disables weapon/hazard/damage shake. |
| Screen Flash Intensity | existing default | Slider should affect damage flash, muzzle flash bloom, hazard flash, and white/red screen overlays. |
| Flicker Lights | `On` | Off mode replaces flicker with steady brightness or slower warning pulses. |
| Weapon Bob | `100%` | Slider or Reduced Motion controlled. |
| Recoil Camera Kick | `100%` | Presentation only; should not affect weapon simulation. |
| Full-Screen Effects | `100%` | Optional umbrella slider for edge overlays, vignette pulses, and pressure distortion. |

Hard rule: visual comfort settings must not reduce the underlying gameplay state. If reduced flash hides a damage event, substitute stable HUD, audio, controller, or textual feedback.

## Subtitle And Caption Placeholders

Even if final VO/dialogue is deferred, reserve a captions system now.

Recommended caption categories:

| Category | Example Placeholder | Purpose |
| --- | --- | --- |
| Dialogue | `[Foreman radio: route instruction]` | Future story/mission barks. |
| Enemy Tell | `[Scrapper winding up]` | Supports players who miss audio cues. |
| Hazard | `[Steam vent building pressure]` | Reinforces dangerous machinery. |
| Pickup | `[Gear key acquired]` | Confirmation without relying on sound. |
| Objective | `[Pressure gate unlocked]` | Route progression. |
| Ambient | `[distant machinery]` | Optional flavor; should be separately toggleable if noisy. |

Recommended settings:

- Subtitles: Off / Dialogue Only / Dialogue + Gameplay Cues.
- Closed Captions: Off / Important Cues / All Authored Cues.
- Caption Size: Small / Medium / Large.
- Caption Background: None / Dim / Solid.
- Speaker Labels: On / Off.
- Directional Hint: On / Off for offscreen hazards/enemies where appropriate.

Captions should avoid spoilers and should not replace clear visual gameplay tells.

## Color And Readability Options

Recommended settings:

| Setting | Default | Notes |
| --- | --- | --- |
| High Contrast HUD | existing default | Preserve current high-contrast work and expand to menus/prompts. |
| Text Size | `Medium` | Small / Medium / Large; target at least 18 px equivalent for settings body text. |
| Prompt Scale | `100%` | `80%` to `150%`; affects interact prompts and objective reminders. |
| Reticle Scale | `100%` | `75%` to `150%`; avoid covering weak points. |
| HUD Scale | `100%` | `80%` to `140%`; verify no overlap at 720p and 1080p. |
| Objective Panel Duration | `Normal` | Short / Normal / Long / Sticky. |
| Color Assist Mode | `Off` | Preserve color language but add alternate outlines/icons/patterns. |
| Brightness/Gamma | `1.00` | Use carefully so it does not wash out route colors. |

Route language should remain stable:

- Green: stocked, restored, safe, exit.
- Amber: attention, objective, charged, interactable.
- Red: locked, hostile, danger.
- Cyan/blue: attack tells or machine windup where already established.

Color assist must add shape, icon, outline, label, or pattern differences instead of simply replacing the palette.

## Audio Mix Sliders

Recommended sliders:

| Slider | Default | Scope |
| --- | --- | --- |
| Master Volume | existing default | Global multiplier. |
| Music Volume | `80%` | Future score/stingers. |
| SFX Volume | `100%` | Weapons, hits, pickups, UI, machinery. |
| Dialogue Volume | `100%` | Future VO/radio. |
| Ambience Volume | `80%` | Loops, room tone, distant machinery. |
| UI Volume | `80%` | Menu ticks, confirm/cancel, pause feedback. |
| Accessibility Cue Volume | `100%` | Optional separate route/critical cue mix if implemented. |

Mix rules:

- Critical enemy/hazard cues should stay audible above ambience and music.
- Pause/settings should not play constant hiss or loud looping machinery.
- Mute states should still allow visual/caption feedback for important events.
- Slider changes should provide a short preview tick or cue when safe.

## Low-Spec Display Presets

Recommended presets:

| Preset | Target | Expected Changes |
| --- | --- | --- |
| Quality | Windows mid PC | Full Windows styling within current budget. |
| Balanced | Windows low/mid default candidate | Reduced effects, stable 60 FPS target, readable lighting. |
| Low Spec | Older 1080p PCs | Lower shadows, fewer particles, lower effect intensity, reduced decals, lower texture bias if needed. |
| Web/Mobile Preview | Future-safe preview | Mobile/Web-style VFX density and simplified post-processing. |
| VR Comfort Preview | Future-safe preview | No forced camera effects, reduced flash, stable lighting, larger readable prompts. |

Preset rules:

- Presets may adjust many settings, but individual settings should remain visible after the preset is applied.
- Low Spec must not remove route-critical signs, prompt backgrounds, enemy weak/tell colors, hazard warnings, boss state, or final exit readability.
- Display presets should be data-driven and platform-overridable later.
- Presets should avoid destructive resolution changes without confirmation.

## Persistence Model

Recommended data fields:

- Schema version.
- Last modified timestamp if useful for debugging.
- Controls map by intent id.
- Sensitivity and invert fields.
- Video/display preset plus explicit overrides.
- Audio mix fields.
- Accessibility settings.
- Caption settings.

Persistence rules:

- Load settings before main menu interaction.
- Apply safe settings immediately after load.
- Validate ranges on load and clamp unknown/old values.
- Unknown future fields should not break older settings files.
- Reset defaults should be available per group.
- If the settings file is corrupt, fall back to defaults and show a non-blocking notice.

## Done Criteria For Later Implementation

- Main menu and pause menu expose the required option groups.
- Critical settings persist across quit/relaunch.
- Remapping protects required actions and displays conflicts.
- Mouse X/Y sensitivity can be tuned independently.
- Motion/flash/shake reductions visibly affect gameplay presentation without hiding state.
- Captions can display placeholder gameplay cues.
- High contrast, text scale, prompt scale, reticle scale, and HUD scale pass readability checks.
- Audio sliders affect their intended buses without muting critical visual/caption feedback.
- Low Spec preset improves performance risk while preserving route and combat readability.

