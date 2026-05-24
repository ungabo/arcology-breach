# Secret-Hunt Route Sheet

Build:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.93\BrassworksBreach_v0.0.93.exe`

Controls: mouse look, `WASD` move, left mouse fire, right mouse alternate fire, `E` interact, `1` Pressure Pistol, `2` Steam Scattergun after pickup, `Esc` pause, `R` restart after death or win. No jump or crouch required.

Purpose: judge whether the three current registered secrets feel fair, optional, discoverable, and non-blocking.

Tester instructions:

- Do a blind search first. Do not read the expected secret names below until you have searched each level naturally.
- Spend no more than `3 minutes` extra per level searching unless the route is still fun.
- After the blind pass, read the expected secret list and verify whether the hidden-space clueing was fair in hindsight.
- Log accidental discoveries separately from intentional discoveries.

Timing target:

- Blind secret route: `20-40 minutes`.
- Target search budget: `0-3 extra minutes` for Level01, Level02, and Level04.
- Level03 and Level05 currently have no registered secrets; do not spend more than `1 minute` looking there unless something strongly suggests a hidden route.

## Global Pass / Fail

- [ ] PASS: each current secret can be found or at least feels fair after the tester learns where it was.
- [ ] FAIL: a secret feels random, invisible, mandatory-looking, reachable only by awkward movement, or is discovered accidentally during normal combat with no clue read.

Notes:

```text
Total secrets found blind:
Total secrets found after reveal:
Most fair secret:
Least fair secret:
Accidental discoveries:
```

## Blind Search Checklist

Before reading expected locations:

- [ ] PASS / [ ] FAIL: Level01 secret search did not confuse the main route.
- [ ] PASS / [ ] FAIL: Level02 secret search did not confuse the routing-valve objective.
- [ ] PASS / [ ] FAIL: Level04 secret search did not push the tester through unfair active hazard damage.
- [ ] PASS / [ ] FAIL: secret rewards felt optional and useful.

Blind notes:

```text
Level01 clue seen:
Level02 clue seen:
Level04 clue seen:
Did any secret look like required progression?
```

## Expected Secrets For Verification

Use this section after the blind search.

### Level01 - Intake Pressure Cache

Expected role: first optional pressure cache in Brassworks Intake.

- [ ] PASS / [ ] FAIL: clue is visible before discovery.
- [ ] PASS / [ ] FAIL: cache access does not require jump, crouch, or a forced fast turn.
- [ ] PASS / [ ] FAIL: reward reads as optional resources, not required progression.
- [ ] PASS / [ ] FAIL: discovery message is noticed.
- [ ] PASS / [ ] FAIL: cache does not trigger accidentally during ordinary combat unless the player meaningfully explores.

Notes prompt:

```text
What clue suggested the cache?
Was the clue noticed before or after discovery?
Was the reward worth the detour?
```

### Level02 - Pipeworks Cartridge Cache

Expected role: optional cartridge-cache secret near the Pipeworks route.

- [ ] PASS / [ ] FAIL: clue does not compete with the main routing-valve objective.
- [ ] PASS / [ ] FAIL: secret can be found without fighting camera, baffles, or pipe collision.
- [ ] PASS / [ ] FAIL: ammo/reward value matches the combat pressure nearby.
- [ ] PASS / [ ] FAIL: discovery message and run secret count feel clear.

Notes prompt:

```text
Did the tester confuse secret clueing with valve-route signage?
Was the cache too visible, too hidden, or right?
Did the reward alter Level02 or Level03 resource pressure?
```

### Level04 - Foundry Coal Cache

Expected role: optional foundry coal-cache secret.

- [ ] PASS / [ ] FAIL: clue is readable without crossing active lethal heat.
- [ ] PASS / [ ] FAIL: cache remains optional during Bulwark/furnace pressure.
- [ ] PASS / [ ] FAIL: player can enter and leave without snagging.
- [ ] PASS / [ ] FAIL: reward is useful before Level05 but not required.

Notes prompt:

```text
Was the cache clue visible during normal foundry scanning?
Did heat, steam, or Bulwark pressure obscure the clue?
Was the secret safe enough to feel fair?
```

## Win-State Secret Check

At the final win state:

- [ ] PASS / [ ] FAIL: secret progress appears consistent with discoveries.
- [ ] PASS / [ ] FAIL: tester understands that secret stats persisted across level transitions.

Notes prompt:

```text
Win message secret text:
Expected discovered count:
Actual discovered count:
Any mismatch or unclear wording:
```

## Tuning Questions

- Which secret needed one more visual clue?
- Which clue looked too much like main-route signage?
- Which reward felt too strong, too weak, or misplaced?
- Did searching increase route mastery or create dead-end frustration?

## Expected Current Limitations

- Only three secrets are currently registered: Level01, Level02, and Level04.
- Level03 and Level05 optional secrets are planned/deferred, not expected in this build.
- Secret smoke verifies discovery mechanics, not whether clueing feels fair.
- Current visual clueing may still be primitive and may need later asset/signage passes.
