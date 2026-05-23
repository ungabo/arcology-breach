# Arcology Breach - v0 Scope

## Purpose

The original `v0.0` goal was to prove that the Unity FPS loop works on this machine. That goal is complete.

Current `v0.1` keeps the game simple but now points toward the original cyberpunk identity:

- Greybox `Aster Gate Intake`.
- Primitive mechanical `Scrapper` enemies.
- Placeholder `Pulse Pistol`.
- Access shard objective.
- Red corporate lockdown gate.
- Green emergency lift/data gate.

## v0.0 Complete

- First-person movement and mouse look.
- Wall collision.
- Basic health and death.
- Basic hitscan shooting.
- Ammo tracking.
- Primitive melee enemy.
- Text HUD.
- Key/access pickup.
- Locked route.
- Exit trigger.
- Restart after death/win.
- Windows build and smoke tests.

## v0.1 Complete

- Camera-mounted weapon placeholder.
- Muzzle flash.
- Damage flash.
- Bobbing pickups.
- Sliding gate.
- Colored point lights.
- Enemy lens markers.

## Active v0.2 Direction

`v0.2` should make the prototype feel like `Arcology Breach`, even before final art:

- Rename visible gameplay language to access shard, lockdown gate, emergency lift.
- Tune combat through manual play.
- Tune the first procedural cyberpunk audio pass.
- Tune the new Scrapper attack tell and improve enemy navigation.
- Confirm `Aster Gate Intake` room scale and objective flow.
- Add first neon material pass only after combat tuning.
- Keep packaged runtime smoke and auto-playthrough tests passing.

## Deferred

- Final enemy models.
- Final weapon models.
- Large art batches.
- Full UI.
- Multiple levels.
- Android build.
- Browser/WebGL build.
- VR build.

Android, browser, and VR planning are tracked in:

- `PLATFORM_ANDROID_PORT_NOTES.md`
- `PLATFORM_WEB_BROWSER_PORT_NOTES.md`
- `PLATFORM_VR_PORT_NOTES.md`

Level map and future transition planning is tracked in:

- `LEVEL_DESIGN_AND_MAPS.md`
