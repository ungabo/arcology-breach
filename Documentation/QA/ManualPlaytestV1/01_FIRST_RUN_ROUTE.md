# First-Run Route Sheet

Build:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.93\BrassworksBreach_v0.0.93.exe`

Controls: mouse look, `WASD` move, left mouse fire, right mouse alternate fire, `E` interact, `1` Pressure Pistol, `2` Steam Scattergun after pickup, `Esc` pause, `R` restart after death or win. No jump or crouch required.

Purpose: observe whether a first-time Windows player can complete the current five-level path from Main Menu to win while understanding where to go, what is locked, what changed after objectives, and what each new enemy or hazard is doing.

## Session Setup

- Tester has not read the detailed level route before starting.
- Tester may read the controls above.
- Tester should think aloud or write short notes while playing.
- Facilitator should avoid giving directions unless the tester is blocked for more than `2 minutes`.

Timing target:

- Full first-run target: `18-30 minutes`.
- Repeat-player target: `10-18 minutes`.
- Log any level where confusion alone adds more than `2 minutes`.

## Global Pass / Fail

- [ ] PASS: tester reaches the final win state and can explain the main objective chain for each level.
- [ ] FAIL: tester cannot complete the route without outside direction, or completes but cannot explain key locks, valves, lifts, hazards, or the Warden lock.

Notes:

```text
Overall first-run result:
Biggest confusion point:
Biggest feel issue:
Best-read objective or encounter:
```

## Level01 - Brassworks Intake

Expected route: spawn briefing -> movement/combat read -> gear key -> pressure gate -> service lift to Level02.

Timing target: `3-5 minutes`.

- [ ] PASS / [ ] FAIL: objective briefing is noticed within `10 seconds`.
- [ ] PASS / [ ] FAIL: tester understands where to go first without circling the spawn room for more than `45 seconds`.
- [ ] PASS / [ ] FAIL: pressure gate and gear-key relationship is understandable.
- [ ] PASS / [ ] FAIL: gear-key pickup feedback is noticed.
- [ ] PASS / [ ] FAIL: pressure-gate opening feedback is noticed.
- [ ] PASS / [ ] FAIL: service lift reads as the level exit and uses green/success language clearly.
- [ ] PASS / [ ] FAIL: Scrapper melee role is understood before repeated damage.
- [ ] PASS / [ ] FAIL: no jump, crouch, or snaggy tight turn feels required.

Notes prompt:

```text
Where did the tester try to go first?
Was the gate understood before or only after finding the key?
Was the service lift obvious?
Any collision snag or forced fast turn?
```

## Level02 - Pipeworks Annex

Expected route: arrive -> see/try locked Boilerheart lift -> find routing valve -> fight Scrapper/Lancer pressure -> use unlocked lift to Level03.

Timing target: `3-5 minutes`.

- [ ] PASS / [ ] FAIL: Pipeworks identity is clear on arrival.
- [ ] PASS / [ ] FAIL: locked Boilerheart lift denial is understood before valve completion.
- [ ] PASS / [ ] FAIL: routing valve reads as the active objective.
- [ ] PASS / [ ] FAIL: tester can tell what changed after routing pipe pressure.
- [ ] PASS / [ ] FAIL: first Lancer reads as a ranged enemy before unavoidable repeated damage.
- [ ] PASS / [ ] FAIL: tester can break line of sight or use cover without snagging.
- [ ] PASS / [ ] FAIL: lift to Level03 is found after valve completion.

Notes prompt:

```text
Did the tester test the locked lift?
How long to find the valve?
Did the Lancer shot tell read before impact?
Did any pipe/baffle geometry confuse route direction?
```

## Level03 - Boilerheart Core

Expected route: arrive -> read locked foundry lift -> collect Steam Scattergun -> learn steam/Bellows pressure -> vent Boilerheart pressure valve -> observe hazard shutdown -> use foundry lift.

Timing target: `4-7 minutes`.

- [ ] PASS / [ ] FAIL: foundry lift lock is understood before pressure valve completion.
- [ ] PASS / [ ] FAIL: Steam Scattergun pickup reads as important and usable.
- [ ] PASS / [ ] FAIL: tester can switch between `1` and `2` after pickup.
- [ ] PASS / [ ] FAIL: steam hazards warn before damage.
- [ ] PASS / [ ] FAIL: Bellows Node reads as a support/damage machine and not just background dressing.
- [ ] PASS / [ ] FAIL: pressure valve reads as the route unlock objective.
- [ ] PASS / [ ] FAIL: tester understands that valve completion changed hazard/lock state.
- [ ] PASS / [ ] FAIL: foundry lift direction is clear after the valve.

Notes prompt:

```text
Did the tester notice the scattergun before combat pressure?
Was the Bellows pulse source clear?
Was steam damage confused with enemy damage?
What told the tester the foundry lift was now usable?
```

## Level04 - Furnace Foundry

Expected route: arrive -> read foundry/furnace identity -> survive steam and furnace heat -> fight mixed pressure including Bulwark -> use emergency hoist to Level05.

Timing target: `4-7 minutes`.

- [ ] PASS / [ ] FAIL: furnace heat lanes warn before damage.
- [ ] PASS / [ ] FAIL: steam and furnace heat are distinguishable from enemy damage.
- [ ] PASS / [ ] FAIL: Bulwark reads as a heavy enemy before close-range damage lands repeatedly.
- [ ] PASS / [ ] FAIL: Bulwark slam tell VFX/audio is noticed.
- [ ] PASS / [ ] FAIL: tester can keep moving and find enough room to fight.
- [ ] PASS / [ ] FAIL: emergency hoist reads as the next-level exit.

Notes prompt:

```text
Did heat timing feel learnable?
Did Bulwark damage feel fair?
Were Lancer/Scrapper/Bulwark roles distinguishable in mixed pressure?
Did the hoist stand out after the fight?
```

## Level05 - Governor Core

Expected route: arrive -> read regulator/core identity -> confirm master override hoist locked -> fight mixed machines and Warden -> Warden defeat unlocks hoist -> use hoist to win.

Timing target: `5-8 minutes`.

- [ ] PASS / [ ] FAIL: master override hoist lock is understood before Warden defeat.
- [ ] PASS / [ ] FAIL: Warden reveal and boss health HUD are noticed.
- [ ] PASS / [ ] FAIL: Warden damage sources are distinguishable from other enemies and hazards.
- [ ] PASS / [ ] FAIL: cover/movement space is readable during the fight.
- [ ] PASS / [ ] FAIL: Warden shutdown feedback is noticed.
- [ ] PASS / [ ] FAIL: tester understands the hoist is now unlocked.
- [ ] PASS / [ ] FAIL: final win state triggers successfully.

Notes prompt:

```text
Did the tester try the locked hoist before the Warden died?
Was boss health readable during combat?
What was the main source of damage taken?
Was the final unlock cause/effect clear?
```

## Tuning Questions

- Which objective was least clear without HUD text?
- Which lock or unlock needed stronger local feedback?
- Which enemy did the tester understand fastest?
- Which enemy or hazard felt unfair rather than challenging?
- Did resource pressure feel too light, too punishing, or about right?
- Did any level require a turn or corridor read that would be uncomfortable for future VR?

## Expected Current Limitations

- This run judges human readability; automated tests already verify the chain can complete.
- Current art is primitive/procedural, so rough visuals are expected.
- Enemy pathing may show simple steering artifacts.
- Audio is placeholder procedural content and may need tuning even when the route passes.
- No save exists, so late failures require replaying from the start.
