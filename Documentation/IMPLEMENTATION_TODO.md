# Brassworks Breach - Implementation To-Do

This checklist tracks implementation work. Detailed production tracking lives in `WORK_LEDGER.md`.

## Operating Directive

- [x] Maintain `CONTINUATION_DIRECTIVE.md`.
- [x] At the end of each completed step, update records, verify, commit/push when appropriate, and immediately begin the next highest-impact unfinished task.

## Completed

- [x] Create Unity project.
- [x] Create generated `Level01`.
- [x] Implement FPS movement.
- [x] Implement mouse look.
- [x] Implement hitscan weapon.
- [x] Implement health, ammo, death, restart.
- [x] Implement primitive mechanical melee enemy.
- [x] Implement key pickup.
- [x] Implement locked gate.
- [x] Implement exit trigger.
- [x] Implement text HUD.
- [x] Implement v0.1 presentation feedback.
- [x] Create public GitHub repo.
- [x] Add story/lore bible.
- [x] Add AAA-style roadmap, asset catalog, and tracking docs.
- [x] Add level map/progression planning.
- [x] Review locally cached Unity Asset Store packs.
- [x] Generate and import steampunk north-star concept art.
- [x] Retheme working title/product metadata to `Brassworks Breach`.

## v0.0.7 Steampunk Retheme and Pause Flow

- [x] Remove old build/version wording from active progress/to-do docs.
- [x] Add pause menu with resume, restart, and quit.
- [x] Add packaged pause-flow automation.
- [x] Retheme code vocabulary to pressure pistol, gear key, pressure gate, service lift, and steamworks audio.
- [x] Retheme generated scene dressing toward brass, copper, iron, oil stone, pipes, and boiler stacks.
- [x] Rebuild generated scene.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.7`.

## Current Priority: v0.0.8 Brassworks Prop Pass

- [x] Add gear-shaped key pickup.
- [x] Add pressure-gauge details to weapon, gate, and service lift.
- [x] Add valve wheels, steam vents, and coal furnace prop.
- [x] Rebuild generated scene.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.8`.

## v0.0.9 Brass Gauge HUD Pass

- [x] Add HUD backplates.
- [x] Add health gauge fill.
- [x] Add ammo gauge fill.
- [x] Add gear-key status lamp.
- [x] Rebuild generated scene.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.9`.

## Current Priority: v0.0.10 Primitive Scrapper Visual

- [x] Replace capsule enemy visual with clockwork Scrapper silhouette.
- [x] Add boiler torso, brass plate, furnace eye, pressure tank, piston arms, cutter blades, and feet.
- [x] Rebuild generated scene.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.10`.

## v0.0.11 Impact Spark Pass

- [x] Replace yellow hit-marker sphere with spark-burst impact feedback.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.11`.

## v0.0.12 Pickup Visual Pass

- [x] Replace cube health pickup with brass-and-glass medicinal vial.
- [x] Replace cube ammo pickup with brass pressure-cartridge pack.
- [x] Rebuild generated scene.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.12`.

## v0.0.13 Pressure Pistol Viewmodel Pass

- [x] Replace blocky pressure pistol with primitive brass-and-walnut pneumatic sidearm.
- [x] Add barrel cylinders, pressure tube, brass receiver, iron backplate, trigger guard, trigger, gauge, valve wheel, and side pipes.
- [x] Rebuild generated scene.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.13`.

## v0.0.14 Main Menu Flow

- [x] Add generated `MainMenu` scene.
- [x] Add `MainMenuController` with start and quit actions.
- [x] Put `MainMenu` first and `Level01` second in build settings.
- [x] Add automated test-flag routing from menu into gameplay.
- [x] Rebuild generated scenes.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.14`.

## v0.0.15 Settings Foundation

- [x] Add persistent `GameSettings`.
- [x] Add mouse sensitivity and master volume sliders to the main menu.
- [x] Add mouse sensitivity and master volume sliders to the pause overlay.
- [x] Apply mouse sensitivity through player look.
- [x] Apply master volume through steamworks audio.
- [x] Rebuild generated scenes.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.15`.

## v0.0.16 Level Transition and Level02 Foundation

- [x] Add generated `Level02` Pipeworks Annex scene.
- [x] Add `LevelTransitionTrigger`.
- [x] Convert Level01 service lift into a transition to `Level02`.
- [x] Add final service lift win state to `Level02`.
- [x] Expand editor smoke to validate all three build scenes.
- [x] Expand packaged auto-playthrough through Level01 transition and Level02 exit.
- [x] Rebuild generated scenes.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.16`.

## v0.0.17 Durable Run State

- [x] Add `RunProgress` snapshot store.
- [x] Capture player health and ammo before service-lift scene transition.
- [x] Apply persisted health and ammo to the next scene's spawned player.
- [x] Reset run progress on main-menu start.
- [x] Expand auto-playthrough to spend ammo before transition and verify persistence in `Level02`.
- [x] Rebuild generated scenes.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.17`.

## v0.0.18 Lancer Ranged Enemy Prototype

- [x] Add `RangedEnemyController`.
- [x] Add `PressureBolt` projectile.
- [x] Add primitive Lancer visual with rifle barrel, pressure tank, furnace lens, and tripod legs.
- [x] Place a Lancer in `Level02`.
- [x] Expand editor smoke to require the Level02 ranged enemy.
- [x] Keep auto-playthrough deterministic by disabling ranged enemies during objective automation.
- [x] Rebuild generated scenes.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.18`.

## v0.0.19 Ranged Combat Smoke

- [x] Add `RuntimeRangedCombatTest`.
- [x] Add menu automation routing for `-v0RangedCombatSmoke`.
- [x] Verify a Level02 Lancer pressure bolt damages the player.
- [x] Rebuild generated scenes.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.19`.

## v0.0.20 Combat Edge Smoke

- [x] Add `RuntimeCombatEdgeTest`.
- [x] Add menu automation routing for `-v0CombatEdgeSmoke`.
- [x] Verify empty-ammo weapon behavior.
- [x] Verify Scrapper melee damage.
- [x] Verify player death state disables gameplay.
- [x] Rebuild generated scenes.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged combat-edge smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.20`.

## v0.0.21 Windows Runtime Performance Profile

- [x] Add `RuntimePerformanceProfile`.
- [x] Apply 60 FPS target, no v-sync lock, limited pixel lights, no MSAA, no dynamic resolution, shorter shadow distance, and reduced LOD bias.
- [x] Add performance profile to main menu and gameplay scenes.
- [x] Expand editor/runtime smoke to require the profile.
- [x] Rebuild generated scenes.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged combat-edge smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.21`.

## v0.2 Combat Feel Slice

- [ ] Run manual Windows playthrough.
- [ ] Record tuning notes in `WORK_LEDGER.md`.
- [ ] Tune player movement speed and camera feel.
- [ ] Tune `Pressure Pistol` damage, fire rate, ammo, and feedback.
- [ ] Tune `Scrapper` speed, detection range, damage, and attack cooldown.
- [ ] Confirm `Brassworks Intake` scale and room flow against `LEVEL_DESIGN_AND_MAPS.md`.
- [ ] Manual readability pass for Scrapper attack tells and world labels.
- [ ] Manual listen pass for procedural audio levels and tone.

## v0.3 Art Direction Slice

- [x] Add first primitive brassworks prop silhouettes.
- [ ] Generate oil-dark stone material.
- [ ] Generate riveted iron wall material.
- [ ] Generate brass/copper pipe material.
- [ ] Generate gear-key visual.
- [ ] Generate pressure-gate visual.
- [ ] Generate service-lift visual.
- [x] Generate primitive `Scrapper` visual.
- [x] Generate primitive `Pressure Pistol` visual.
- [x] Replace placeholder hit marker with primitive spark impact VFX.
- [ ] Add gauges, valve wheels, furnace props, pipe bundles, and work-order signs.
- [ ] Verify scene readability after art pass.

## v0.4 Systems Foundation

- [ ] Data-driven weapon definitions.
- [ ] Data-driven enemy definitions.
- [ ] Interaction system.
- [ ] Pickup/inventory cleanup.
- [ ] Level transition controller.
- [ ] Level validation tool.
- [ ] Expand combat automation harness.
- [ ] Build automation cleanup.
- [ ] Platform asset-quality settings.

## Platform Planning

Windows is the primary development target. Android, browser/WebGL, and VR are deferred until after the full Windows game is built, but their constraints must influence architecture and assets as they are created.

- [x] Add Windows target profile.
- [x] Add Android port notes.
- [x] Add browser/WebGL port notes.
- [x] Add Steam/Meta VR port notes.
- [x] Add Windows runtime quality profile in Unity.
- [ ] Add Android quality profile in Unity.
- [ ] Add WebGL quality profile in Unity.
- [ ] Add platform-specific asset import rules.
- [ ] Add Android input/touch control design later.
- [ ] Add WebGL loading/download budget later.
- [ ] Add future XR/OpenXR input and comfort design later.

## Decision Log

- 2026-05-22: Unity FPS proof of concept implemented.
- 2026-05-22: v0.1 presentation feedback added.
- 2026-05-22: Public repo created.
- 2026-05-22: Android, browser/WebGL, SteamVR/OpenXR, and Meta Quest planning added as deferred port tracks.
- 2026-05-22: Level map planning and locally cached Asset Store pack review added.
- 2026-05-23: Project art direction pivoted to heavy steampunk using the generated north-star concept sheets.
