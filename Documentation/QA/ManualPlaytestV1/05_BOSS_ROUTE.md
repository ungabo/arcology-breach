# Boss Route Sheet

Build:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.93\BrassworksBreach_v0.0.93.exe`

Controls: mouse look, `WASD` move, left mouse fire, right mouse alternate fire, `E` interact, `1` Pressure Pistol, `2` Steam Scattergun after pickup, `Esc` pause, `R` restart after death or win. No jump or crouch required.

Purpose: evaluate the Level05 Governor Warden finale, including locked-hoist setup, boss reveal, boss HUD, attack readability, mixed-threat clarity, defeat feedback, and final unlock.

Route requirement: there is no save system in `v0.0.93`, so reach Level05 through the normal route unless a developer later provides a boss-start build.

Timing target:

- Full route to boss: use first-run or repeat-run timing from the main route.
- Level05 boss-focused target: `5-10 minutes`.
- If the tester dies twice before understanding a specific attack, log the attack as a high-priority readability issue.

## Global Pass / Fail

- [ ] PASS: tester understands the Warden is blocking the master override hoist, can read major attacks, defeats the Warden, sees the unlock, and wins.
- [ ] FAIL: boss objective, damage sources, boss HUD, or post-fight unlock are unclear enough that the tester needs outside direction.

Notes:

```text
Boss route result:
Attempts needed:
Death count:
Most unclear attack:
Most useful feedback element:
```

## Pre-Boss Setup

Expected route context: Level05 arrival -> regulator/core read -> master override hoist is locked until Warden defeat.

- [ ] PASS / [ ] FAIL: Governor Core identity is clear on arrival.
- [ ] PASS / [ ] FAIL: tester notices or tests the master override hoist lock before Warden defeat.
- [ ] PASS / [ ] FAIL: lock signal and objective text explain why the hoist is unavailable.
- [ ] PASS / [ ] FAIL: Warden reveal is front-facing enough to identify the boss.
- [ ] PASS / [ ] FAIL: boss health HUD appears promptly when Warden combat starts.

Notes prompt:

```text
Did the tester try the hoist early?
Did the Warden read as the required objective?
Was the boss HUD location noticed without prompting?
```

## Warden Fight

- [ ] PASS / [ ] FAIL: stomp attack is telegraphed before damage.
- [ ] PASS / [ ] FAIL: pressure-bolt attack direction is readable.
- [ ] PASS / [ ] FAIL: enraged half-health behavior is noticed and understood.
- [ ] PASS / [ ] FAIL: boss health bar updates clearly on damage.
- [ ] PASS / [ ] FAIL: mixed Scrapper/Lancer/Bulwark pressure does not hide Warden tells.
- [ ] PASS / [ ] FAIL: hazards do not make Warden damage feel random.
- [ ] PASS / [ ] FAIL: cover and movement space support recovery.
- [ ] PASS / [ ] FAIL: both pistol and scattergun remain usable options.

Notes prompt:

```text
Which attack caused the most damage?
Could the tester tell when the Warden was hit?
Did the tester use cover intentionally?
Was any damage source misread?
```

## Defeat And Final Hoist

- [ ] PASS / [ ] FAIL: Warden shutdown VFX clearly confirms defeat.
- [ ] PASS / [ ] FAIL: objective text or world feedback makes the unlock understandable.
- [ ] PASS / [ ] FAIL: master override hoist reads as usable after Warden death.
- [ ] PASS / [ ] FAIL: final `E` interaction triggers win state.
- [ ] PASS / [ ] FAIL: win message, including secret progress when applicable, is readable.

Notes prompt:

```text
Did the tester know where to go after Warden death?
What told them the hoist unlocked?
Was the final interaction prompt visible and readable?
```

## Boss Tuning Questions

- Does the Warden need more anticipation before stomp damage?
- Are pressure bolts too quiet, too fast, or too visually subtle?
- Does the boss have enough health to feel climactic without dragging?
- Does mixed enemy pressure enhance the fight or obscure the boss?
- Is the boss HUD clear enough while moving and taking damage?

## Expected Current Limitations

- Warden art and animation are still prototype primitive machinery.
- Warden combat smoke verifies boss HUD, damage feedback, death, and shutdown VFX, but not encounter feel.
- Enemy navigation and boss arena flow are not final.
- No save or level select exists, so late-route retests are costly.
