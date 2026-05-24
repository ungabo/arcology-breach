# Combat Feel Route Sheet

Build:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.93\BrassworksBreach_v0.0.93.exe`

Controls: mouse look, `WASD` move, left mouse fire, right mouse alternate fire, `E` interact, `1` Pressure Pistol, `2` Steam Scattergun after pickup, `Esc` pause, `R` restart after death or win. No jump or crouch required.

Purpose: evaluate player weapon feel, enemy role readability, attack tells, damage fairness, audio/VFX clarity, and resource pressure across the current five-level route.

Recommended tester behavior:

- Fight normally, not as a speedrun.
- Use Pressure Pistol primary fire and right-mouse Pressure Burst in close Scrapper pressure.
- After Level03 pickup, use both Pressure Pistol and Steam Scattergun.
- Try Steam Scattergun primary at close range and right-mouse slug at a safer distance.
- Do not farm or intentionally break AI; log natural combat issues first.

Timing target: `20-35 minutes` for a full combat-focused route.

## Global Pass / Fail

- [ ] PASS: each enemy role is understood, damage feels avoidable after one or two reads, and both weapons have a clear use case.
- [ ] FAIL: any enemy repeatedly damages the tester without readable warning, weapon behavior is unclear, or combat completion depends mostly on luck.

Notes:

```text
Overall combat result:
Most readable enemy:
Least readable enemy:
Weapon that felt best:
Weapon or enemy needing first tuning:
```

## Pressure Pistol

- [ ] PASS / [ ] FAIL: left-mouse fire feels responsive and understandable.
- [ ] PASS / [ ] FAIL: ammo cost and empty-ammo feedback are understood.
- [ ] PASS / [ ] FAIL: right-mouse Pressure Burst reads as a close-range alternate fire.
- [ ] PASS / [ ] FAIL: Pressure Burst audio/VFX/viewmodel motion make the action distinct from primary fire.
- [ ] PASS / [ ] FAIL: impact decals/sparks help confirm hits without cluttering combat.

Notes prompt:

```text
Did the tester know when shots hit?
Did Pressure Burst feel useful or confusing?
Was ammo pressure too strict, too generous, or about right?
```

## Steam Scattergun

Expected first pickup: Level03 Boilerheart Core.

- [ ] PASS / [ ] FAIL: pickup display, route strips, lamps, and `BREACH TOOL` label draw attention.
- [ ] PASS / [ ] FAIL: `2` equips the scattergun after pickup and `1` returns to pistol.
- [ ] PASS / [ ] FAIL: close-range primary blast feels distinct from pistol fire.
- [ ] PASS / [ ] FAIL: right-mouse slug reads as a precision alternate fire.
- [ ] PASS / [ ] FAIL: pickup audio/VFX make acquisition clear.

Notes prompt:

```text
Did the tester switch weapons without prompting?
Which scattergun mode did they prefer?
Was the pickup noticed before, during, or after combat pressure?
```

## Scrapper

Primary read: Level01 onward.

- [ ] PASS / [ ] FAIL: melee role is understood quickly.
- [ ] PASS / [ ] FAIL: windup warning VFX/audio appears before damage.
- [ ] PASS / [ ] FAIL: tester can backpedal, sidestep, or burst-fire to recover.
- [ ] PASS / [ ] FAIL: Scrapper death VFX is satisfying without hiding other threats.

Notes prompt:

```text
Was the melee tell visible in normal lighting?
How often did damage feel deserved?
Did simple pathing or side-steering look confusing?
```

## Lancer

Primary read: Level02 onward.

- [ ] PASS / [ ] FAIL: ranged role is understood before repeated hits.
- [ ] PASS / [ ] FAIL: pressure-bolt windup VFX/audio is readable.
- [ ] PASS / [ ] FAIL: projectile trail/impact VFX identify shot direction.
- [ ] PASS / [ ] FAIL: available cover and spacing allow recovery.

Notes prompt:

```text
Did the tester know where the shot came from?
Was there enough time to react to the fire tell?
Did line-of-sight breaking feel intentional or accidental?
```

## Bellows Node

Primary read: Level03 Boilerheart Core.

- [ ] PASS / [ ] FAIL: stationary support-machine silhouette is noticed.
- [ ] PASS / [ ] FAIL: pulse damage source is understood.
- [ ] PASS / [ ] FAIL: boosted Scrapper state is visible and meaningful.
- [ ] PASS / [ ] FAIL: Bellows durability/destruction feedback feels clear.

Notes prompt:

```text
Did the tester attack the Bellows Node or ignore it?
Could they identify the pulse radius/source?
Was boosted enemy pressure readable?
```

## Bulwark

Primary read: Level04 Furnace Foundry, repeated in Level05.

- [ ] PASS / [ ] FAIL: heavy enemy role is understood from silhouette and durability.
- [ ] PASS / [ ] FAIL: hammer windup warning VFX/audio reads before slam damage.
- [ ] PASS / [ ] FAIL: arena space gives enough room to dodge and reposition.
- [ ] PASS / [ ] FAIL: mixed Scrapper/Lancer/Bulwark pressure remains readable.

Notes prompt:

```text
Was the Bulwark attack tell visible while moving?
Did it feel too durable, too fragile, or right?
Was damage confused with furnace heat or Warden attacks?
```

## Governor Warden

Primary read: Level05 Governor Core.

- [ ] PASS / [ ] FAIL: boss identity and health HUD are obvious.
- [ ] PASS / [ ] FAIL: stomp, pressure bolts, and enraged behavior are distinguishable.
- [ ] PASS / [ ] FAIL: shutdown VFX confirms defeat.
- [ ] PASS / [ ] FAIL: final hoist unlock feels earned and understandable.

Notes prompt:

```text
Did boss pressure feel climactic or chaotic?
Was the boss health HUD useful?
Which attack caused the most unclear damage?
```

## Tuning Questions

- Which attack tell needs more lead time?
- Which enemy needs stronger silhouette language?
- Are close-range tools strong enough against Scrappers and Bulwarks?
- Does ammo scarcity create tension or interrupt learning?
- Does audio help separate enemies, weapons, hazards, and objective devices?

## Expected Current Limitations

- Weapon and enemy behavior are smoke-tested for regressions, not tuned for final feel.
- Current models are primitive silhouettes.
- Procedural audio is not final mastered content.
- Enemy navigation can show simple steering rather than polished tactical movement.
