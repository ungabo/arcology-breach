# v0.1.36 Input And Platform Compatibility Recommendations

Created: `2026-05-24`

## Purpose

Preserve the current Windows-first keyboard/mouse experience while preventing options and accessibility implementation from blocking future Android, WebGL, gamepad, SteamVR, or Meta Quest work.

## Windows-First Contract

Windows v1 should remain:

- Keyboard/mouse primary.
- Mouse-look and hitscan-style aiming friendly.
- Pause available through `Escape`.
- Settings available from main menu and pause.
- 1080p mid/low PC as the main readability and performance target.

Windows-first does not mean keyboard/mouse-only internals. The implementation should expose gameplay intents and presentation settings that other platforms can bind differently later.

## Input Intent Boundary

Recommended architecture:

| Layer | Owns | Avoid |
| --- | --- | --- |
| Device Input | Keyboard, mouse, gamepad, touch, VR controllers | Gameplay consequences. |
| Intent Mapping | FirePrimary, Interact, Pause, Move2D, Look2D | Platform-specific UI labels. |
| Gameplay Systems | Consumes intents and aim/move providers | Direct raw key checks. |
| Presentation | Binding labels, glyphs, captions, prompts | Saving gameplay state. |

Minimum later implementation rule: new controls/options work should not increase direct raw input checks in gameplay systems.

## Aim And Look Providers

Separate look input from aim source:

- Windows/WebGL flat screen: mouse delta drives camera; aim source is camera center.
- Android: touch delta drives camera; aim source may remain camera center with optional assist.
- Gamepad: stick delta drives camera; aim source camera center with acceleration/deadzone tuning.
- VR: headset owns view; controller pose owns weapon aim.

Recommended interfaces or conceptual contracts:

- `LookDeltaProvider`: returns horizontal/vertical look deltas after sensitivity, invert, smoothing, and platform scaling.
- `AimRayProvider`: returns origin and direction for weapon/fire/interact checks.
- `MoveVectorProvider`: returns normalized movement vector by gameplay intent.
- `PauseRequestProvider`: handles platform-specific pause/focus behavior.

## Remapping Compatibility

Windows remapping should save by intent id:

- Good: `FirePrimary -> Mouse0`
- Risky: `Left Mouse Button Text -> Mouse0`

Support at least one primary binding and preferably one alternate binding per intent. Alternate bindings make gamepad, accessibility devices, and QA automation easier without changing core gameplay.

Conflict behavior should be deterministic:

- Required intents cannot be cleared.
- Duplicate bindings should require confirmation.
- Menu navigation should always retain a safe fallback.
- Reset Controls should restore Windows defaults even if a platform profile exists.

## Android Touch Path

Do now:

- Keep intent names touch-friendly.
- Keep look sensitivity independent per axis.
- Ensure interact/fire/alt-fire are discrete intents, not mouse-only assumptions.
- Avoid UI layouts that require hover.

Reserve later:

- Virtual stick.
- Swipe look zone.
- Touch-safe buttons.
- Aim assist.
- Safe-area aware pause/settings menu.

Risk to avoid: remapping UI that only understands physical keyboard keys.

## WebGL Path

Do now:

- Keep pause behavior abstract enough to handle browser pointer unlock.
- Avoid assuming `Escape` always means in-game pause; in browsers it may release pointer lock first.
- Keep settings serializable in a form that can later use browser storage.
- Keep captions and readability settings independent of OS-level file paths.

Reserve later:

- Click-to-focus prompt.
- Pointer-lock state messaging.
- Browser local storage or indexed persistence.
- Chrome/Edge smoke tests.

Risk to avoid: treating pointer lock, pause, and cancel as a single unqualified action.

## Gamepad Path

Do now:

- Keep all player actions expressible as button/axis intents.
- Keep sensitivity and invert settings generic enough for sticks.
- Plan deadzone and acceleration settings even if deferred.
- Keep menu focus navigation independent of mouse cursor.

Reserve later:

- Button glyphs.
- Per-device presets.
- Stick deadzone sliders.
- Aim assist and target friction.

Risk to avoid: menus that require precise mouse-only interaction.

## VR Path

Do now:

- Make camera shake, head bob, recoil camera kick, FOV pulses, and full-screen flashes optional.
- Keep weapon firing able to accept an aim origin/direction that is not the camera center.
- Keep prompt scale and text size adjustable.
- Do not rely on tiny screen-space HUD text as the only source of gameplay state.

Reserve later:

- Dominant hand.
- Snap/smooth turn.
- Comfort vignette.
- Height recenter.
- Wrist/object/weapon-mounted UI.
- Controller pose aim.

Risk to avoid: implementation where weapon recoil or damage forcibly moves the camera.

## Settings Persistence Compatibility

Recommended data rules:

- Use a schema version.
- Use stable setting ids.
- Clamp loaded numeric values.
- Ignore unknown future fields.
- Keep platform profile separate from user overrides.
- Avoid platform-specific path assumptions.

Suggested conceptual shape:

```text
SettingsProfile
  schemaVersion
  platformProfileId
  controlsByIntent
  look
  video
  audio
  accessibility
  captions
```

## Integration Recommendations

1. Implement settings through a central settings service or data object before expanding menu controls.
2. Apply settings through subscribers so HUD, camera, audio, VFX, and input can respond without circular dependencies.
3. Keep defaults in one canonical place and let platform profiles override those defaults.
4. Treat QA automation as an input provider so smoke tests can exercise remaps and settings without physical devices.
5. Keep every setting reversible from the pause menu unless it is technically unsafe at runtime.
6. Prefer additive settings ids over renaming old ids once players may have saved settings.

## Hold Conditions For Future Implementation

- A new option only changes the visible menu but not persisted runtime behavior.
- A new setting cannot be reset.
- A control can be unbound in a way that traps the player.
- A visual comfort toggle hides route, enemy, hazard, boss, or exit state.
- A direct keyboard/mouse check is added to a gameplay path that should consume intents.
- A low-spec preset breaks readability or conflicts with established route colors.

