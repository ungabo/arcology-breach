# Brassworks Breach v0.1.36 Accessibility + Options Bundle

Created: `2026-05-24`

Owned scope:

- `Documentation/Planning/V0_1_36_AccessibilityOptionsBundle/`
- `Documentation/QA/V0_1_36_AccessibilityOptionsBundle/`

## Purpose

Prepare a docs-only v0.1.36 plan for a stronger options and accessibility layer. The bundle is meant for later implementation and does not change scripts, scenes, validators, build settings, packages, assets, or shared status files.

## Bundle Files

- `OPTIONS_ACCESSIBILITY_PLAN_v0.1.36.md` - feature plan for controls, sensitivity, motion/flash/shake, captions, readability, audio, and low-spec display presets.
- `INPUT_PLATFORM_COMPATIBILITY_RECOMMENDATIONS_v0.1.36.md` - implementation recommendations that keep Windows keyboard/mouse first while preserving Android, WebGL, gamepad, and VR paths.
- `PRIORITIZATION_ROADMAP_v0.1.36.md` - staged order across v0.1.35, v0.1.36, and v1 stabilization.
- `Documentation/QA/V0_1_36_AccessibilityOptionsBundle/VALIDATION_GATES_v0.1.36.md` - hold/fail gates and required evidence.
- `Documentation/QA/V0_1_36_AccessibilityOptionsBundle/SMOKE_TEST_IDEAS_v0.1.36.md` - manual and automation-friendly smoke-test ideas.

## Current Context

Existing project docs indicate Windows is the source-of-truth platform, keyboard/mouse is primary, and settings already include sensitivity, master volume, flash intensity, resolution, fullscreen, and high contrast. This bundle extends that baseline into a more complete accessibility/options target without requiring immediate implementation.

## Top Recommendations

1. Treat options as a gameplay contract, not just menu widgets: every setting should have a persisted data field, default value, reset behavior, runtime apply path, and QA evidence.
2. Keep input expressed as gameplay intents so remapping does not lock the project to raw keyboard/mouse checks.
3. Split visual comfort controls into independent toggles or sliders for motion, flash, shake, and screen effects.
4. Add captions/subtitles as placeholder-ready infrastructure even before final dialogue or audio barks exist.
5. Make low-spec display presets preserve route, enemy, pickup, hazard, boss, and exit readability before chasing cosmetic savings.

