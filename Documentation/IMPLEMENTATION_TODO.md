# Arcology Breach - Implementation To-Do

This checklist tracks implementation work. Detailed production tracking lives in `WORK_LEDGER.md`.

## Completed

- [x] Create Unity project.
- [x] Create generated `Level01`.
- [x] Implement FPS movement.
- [x] Implement mouse look.
- [x] Implement hitscan weapon.
- [x] Implement health, ammo, death, restart.
- [x] Implement primitive mechanical melee enemy.
- [x] Implement access/key pickup.
- [x] Implement locked gate.
- [x] Implement exit trigger.
- [x] Implement text HUD.
- [x] Implement v0.1 presentation feedback.
- [x] Create public GitHub repo.
- [x] Add cyberpunk story bible.
- [x] Add AAA-style roadmap, asset catalog, tracking, and handoff docs.
- [x] Add level map/progression planning.
- [x] Review locally cached Unity Asset Store packs.

## Current Priority: v0.2 Combat Feel Slice

- [ ] Run manual Windows playthrough.
- [ ] Record tuning notes in `WORK_LEDGER.md`.
- [ ] Tune player movement speed and camera feel.
- [ ] Tune `Pulse Pistol` damage, fire rate, ammo, and feedback.
- [ ] Tune `Scrapper` speed, detection range, damage, and attack cooldown.
- [ ] Make lockdown gate and access shard feedback clearer.
- [ ] Confirm `Aster Gate Intake` scale and room flow against `LEVEL_DESIGN_AND_MAPS.md`.
- [ ] Add first cyberpunk audio set.
- [ ] Improve enemy navigation or obstacle handling.
- [ ] Add first cyberpunk material pass if combat feel is stable.
- [ ] Rebuild Windows player.
- [ ] Update `BUILD_STATUS.md`.
- [ ] Update `HANDOFF.md`.

## v0.3 Art Direction Slice

- [ ] Generate wet concrete material.
- [ ] Generate black chrome wall material.
- [ ] Generate neon cable trunk material.
- [ ] Generate access shard visual.
- [ ] Generate lockdown gate visual.
- [ ] Generate emergency lift/data gate visual.
- [ ] Generate `Scrapper` visual.
- [ ] Generate `Pulse Pistol` visual.
- [ ] Replace placeholder hit marker with cyberpunk impact VFX.
- [ ] Add signage, graffiti, and holographic warning props.
- [ ] Verify scene readability after art pass.

## v0.4 Systems Foundation

- [ ] Data-driven weapon definitions.
- [ ] Data-driven enemy definitions.
- [ ] Interaction system.
- [ ] Pickup/inventory cleanup.
- [ ] Level transition controller.
- [ ] Level validation tool.
- [ ] Build automation cleanup.
- [ ] Platform asset-quality settings.

## Platform Planning

Windows is the primary development target. Android, browser/WebGL, and VR are deferred until after the full Windows game is built, but their constraints must influence architecture and assets as they are created.

- [x] Add Windows target profile.
- [x] Add Android port notes.
- [x] Add browser/WebGL port notes.
- [x] Add Steam/Meta VR port notes.
- [ ] Add platform quality presets in Unity.
- [ ] Add platform-specific asset import rules.
- [ ] Add Android input/touch control design later.
- [ ] Add WebGL loading/download budget later.
- [ ] Add future XR/OpenXR input and comfort design later.

## Decision Log

- 2026-05-22: Unity FPS proof of concept implemented.
- 2026-05-22: v0.1 presentation feedback added.
- 2026-05-22: Public repo created.
- 2026-05-22: Project pivoted to original cyberpunk identity: `Arcology Breach`.
- 2026-05-22: Android, browser/WebGL, SteamVR/OpenXR, and Meta Quest planning added as deferred port tracks.
- 2026-05-22: Level map planning and locally cached Asset Store pack review added.
