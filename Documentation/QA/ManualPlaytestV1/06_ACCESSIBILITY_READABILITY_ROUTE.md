# Accessibility And Readability Route Sheet

Build:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.1.0\BrassworksBreach_v0.1.0.exe`

Controls: mouse look, `WASD` move, left mouse fire, right mouse alternate fire, `E` interact, `1` Pressure Pistol, `2` Steam Scattergun after pickup, `Esc` pause, `R` restart after death or win. No jump or crouch required.

Purpose: evaluate whether a Windows tester can read objectives, prompts, HUD, color language, hazards, combat tells, and navigation with the current sensitivity, master volume, and flash-intensity options.

Recommended tester behavior:

- Move slowly and inspect signage, prompts, objective text, enemy tells, hazards, pickups, and exits.
- Try the pause menu and settings before or during the route.
- Note any text that disappears too quickly, appears too small, blends into the scene, or requires color alone.

Timing target: `25-45 minutes`.

## Global Pass / Fail

- [ ] PASS: route, objectives, prompts, color states, and combat/hazard tells are readable without outside explanation.
- [ ] FAIL: progress depends on tiny text, color-only state, hidden prompts, fast camera turns, or visual/audio overload.

Notes:

```text
Overall readability result:
Worst text/UI issue:
Worst color/contrast issue:
Worst motion/comfort issue:
Most successful readability cue:
```

## Controls And Menus

- [ ] PASS / [ ] FAIL: mouse sensitivity can be adjusted enough for the tester.
- [ ] PASS / [ ] FAIL: master volume can be adjusted enough for the tester.
- [ ] PASS / [ ] FAIL: flash intensity can be reduced enough for damage feedback comfort while remaining readable.
- [ ] PASS / [ ] FAIL: `Esc` pause is discoverable and responsive.
- [ ] PASS / [ ] FAIL: pause resume/restart/quit options are readable.
- [ ] PASS / [ ] FAIL: `E` interaction prompts are visible before the tester needs them.
- [ ] PASS / [ ] FAIL: `R` restart after death/win is readable.

Notes prompt:

```text
Sensitivity setting used:
Volume setting used:
Flash intensity setting used:
Any missing setting the tester expected:
Prompt or menu text that was hard to read:
```

## HUD And Text

- [ ] PASS / [ ] FAIL: health and ammo are readable during combat.
- [ ] PASS / [ ] FAIL: persistent objective text is readable while moving.
- [ ] PASS / [ ] FAIL: boss health HUD is readable during Warden combat.
- [ ] PASS / [ ] FAIL: temporary messages last long enough to understand.
- [ ] PASS / [ ] FAIL: archive plaque text can be read without combat pressure.

Notes prompt:

```text
Smallest readable text:
Text that vanished too quickly:
Text hidden by combat, VFX, or camera motion:
```

## Color And State Language

Expected color language: amber/brass for objectives and useful machinery, red-orange for danger/locks/enemy tells, green for exits/restored systems/success.

- [ ] PASS / [ ] FAIL: amber/brass objective cues are distinguishable from neutral machinery.
- [ ] PASS / [ ] FAIL: red-orange danger/locked states are clear without relying only on color.
- [ ] PASS / [ ] FAIL: green exit/success states stand out.
- [ ] PASS / [ ] FAIL: enemy tells are distinguishable from hazard warnings.
- [ ] PASS / [ ] FAIL: color states remain readable in Level03, Level04, and Level05 lighting.

Notes prompt:

```text
Any color-only cue:
Any cue with poor contrast:
Did shape, motion, or text reinforce color?
```

## Navigation And Comfort

- [ ] PASS / [ ] FAIL: no objective requires jump or crouch.
- [ ] PASS / [ ] FAIL: no required route feels narrower than comfortable FPS movement.
- [ ] PASS / [ ] FAIL: no required route forces abrupt 180-degree turns under pressure.
- [ ] PASS / [ ] FAIL: level exits are readable from local composition, not HUD text alone.
- [ ] PASS / [ ] FAIL: spawn orientation in each level gives a stable first read.
- [ ] PASS / [ ] FAIL: secret paths do not look like mandatory progression.

Notes prompt:

```text
Any snag point:
Any forced fast turn:
Any dead-end that looked like the main path:
Any exit that needed stronger composition:
```

## Audio Readability

- [ ] PASS / [ ] FAIL: weapon sounds are distinguishable.
- [ ] PASS / [ ] FAIL: pickup, gate, valve, and lift feedback are distinguishable.
- [ ] PASS / [ ] FAIL: Scrapper, Lancer, Bulwark, Bellows, Warden, and hazard cues do not mask each other.
- [ ] PASS / [ ] FAIL: ambience supports place identity without hiding tells.

Notes prompt:

```text
Cue that was clearest:
Cue that was masked:
Any sound too loud, quiet, sharp, or muddy:
```

## Readability Tuning Questions

- Which objective would fail if HUD text were removed?
- Which prompt needs larger text, better placement, or longer display?
- Which lock/unlock needs stronger world-state feedback?
- Which combat or hazard tell should gain shape/icon support beyond color?
- Which settings should be prioritized next for accessibility?

## Expected Current Limitations

- Settings currently cover sensitivity, master volume, and flash intensity only.
- Resolution, subtitle/caption support, and color-readability modes are not implemented.
- Current art is primitive and may not represent final contrast/material quality.
- VR comfort rules are future-facing; this Windows build still uses standard mouse-look FPS controls.
