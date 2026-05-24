# Hazard Route Sheet

Build:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.93\BrassworksBreach_v0.0.93.exe`

Controls: mouse look, `WASD` move, left mouse fire, right mouse alternate fire, `E` interact, `1` Pressure Pistol, `2` Steam Scattergun after pickup, `Esc` pause, `R` restart after death or win. No jump or crouch required.

Purpose: evaluate steam hazards, furnace heat hazards, warning timing, damage-source clarity, avoidability, and whether hazards support combat rather than hiding it.

Tester instructions:

- Play normally until each hazard section.
- At each hazard, intentionally observe one full warning/active/safe cycle if possible.
- Do not intentionally die just to test damage unless health is high enough and the route can continue.
- If damage occurs, immediately note whether the source was clear.

Timing target: `15-25 minutes` if focusing on hazard observations during a normal run.

## Global Pass / Fail

- [ ] PASS: hazards warn before damage, are avoidable through readable movement, and are distinguishable from enemy damage.
- [ ] FAIL: hazards feel like invisible damage, unavoidable punishment, or visual noise that hides enemy tells/objectives.

Notes:

```text
Overall hazard result:
Most readable hazard:
Least readable hazard:
Unclear damage moments:
Health/ammo state after hazard-heavy sections:
```

## Level03 - Boilerheart Steam Hazards

Expected route context: reach Boilerheart Core, learn steam hazard zones, vent the Boilerheart pressure valve, observe linked hazard shutdown, use the foundry lift.

Timing target within Level03: `4-7 minutes`.

- [ ] PASS / [ ] FAIL: steam puffs or other warning state are noticed before damage.
- [ ] PASS / [ ] FAIL: low/high steam states read as active machinery, not background-only dressing.
- [ ] PASS / [ ] FAIL: player can avoid damage with lateral movement and route choice.
- [ ] PASS / [ ] FAIL: steam damage is not confused with Scrapper, Lancer, or Bellows damage.
- [ ] PASS / [ ] FAIL: pressure valve completion visibly changes the hazard/lock state.
- [ ] PASS / [ ] FAIL: post-valve route back to the foundry lift is clear.

Notes prompt:

```text
Could the tester predict unsafe steam timing?
Did they notice hazard shutdown after the valve?
Was damage source clear when combat overlapped?
```

## Level04 - Foundry Steam And Furnace Heat

Expected route context: foundry arrival, steam hazards, pulsing furnace heat-surge lanes, mixed Scrapper/Lancer/Bulwark combat, emergency hoist.

Timing target within Level04: `4-7 minutes`.

- [ ] PASS / [ ] FAIL: furnace heat warning state is visible before active damage.
- [ ] PASS / [ ] FAIL: active heat ripple or signal is noticed.
- [ ] PASS / [ ] FAIL: safe timing window can be learned within two cycles.
- [ ] PASS / [ ] FAIL: steam and furnace heat feel visually and audibly different.
- [ ] PASS / [ ] FAIL: hazard lanes do not force the player into unfair Bulwark damage.
- [ ] PASS / [ ] FAIL: hoist path is readable after hazard/combat pressure.

Notes prompt:

```text
How many heat cycles before the tester crossed safely?
Did the tester know when a lane became safe?
Did enemy pressure make hazard learning unfair?
```

## Level05 - Governor Core Steam And Furnace Heat

Expected route context: regulator lane with steam and furnace heat before or around final mixed pressure and Warden lock.

Timing target within Level05 before boss completion: `5-8 minutes`.

- [ ] PASS / [ ] FAIL: regulator steam/heat sources remain readable in darker boss-level composition.
- [ ] PASS / [ ] FAIL: hazards do not obscure the Warden boss HUD or main objective prompts.
- [ ] PASS / [ ] FAIL: damage source remains clear when Warden/Lancer pressure bolts are active.
- [ ] PASS / [ ] FAIL: hazard placement does not make the master override hoist look unsafe after unlock.

Notes prompt:

```text
Did hazards add tension or visual clutter?
Were Warden attacks confused with heat/steam damage?
Was the final hoist approach comfortable after boss defeat?
```

## Hazard Tuning Questions

- Should warning duration be longer, shorter, or unchanged?
- Does active damage need a louder cue, stronger color, or cleaner shape language?
- Are hazard safe windows readable while moving backward or strafing?
- Does any hazard need more space around it for future VR comfort?
- Are red-orange danger signals distinct from enemy attack tells?

## Expected Current Limitations

- Hazard smoke verifies damage and VFX presence, not human timing comprehension.
- Audio cues are procedural placeholders and may not yet separate all hazard states.
- Flash intensity and color-readability options are not implemented.
- Current hazard art is primitive; evaluate fairness and timing first.
