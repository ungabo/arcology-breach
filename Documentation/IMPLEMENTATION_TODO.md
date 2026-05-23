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

## v0.0.22 Level Validation Tool

- [x] Add `V0LevelValidator`.
- [x] Validate build scene order.
- [x] Validate main menu wiring.
- [x] Validate gameplay scene player, HUD, game state, enemies, pickups, doors, transitions, final exit, and performance profile.
- [x] Validate pickup trigger colliders.
- [x] Validate enemy character controllers and Lancer muzzle wiring.
- [x] Integrate validator into editor smoke.
- [x] Run standalone level validation.
- [x] Rebuild generated scenes.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged combat-edge smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.22`.

## v0.0.23 Procedural Material Texture Pass

- [x] Add generated texture asset folder.
- [x] Generate procedural oil-dark stone texture.
- [x] Generate procedural riveted-iron texture.
- [x] Generate procedural brass/copper pipe texture.
- [x] Assign generated textures to active steampunk materials.
- [x] Rebuild generated scenes and material assets.
- [x] Run standalone level validation.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged combat-edge smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.23`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.24 Gear-Key and Pressure-Gate Visual Pass

- [x] Replace the horizontal gear-key pickup with an upright clockwork key silhouette.
- [x] Add gear teeth, spokes, stem, bit, hub, and pins to the gear-key visual.
- [x] Add a static heavy pressure-gate frame with header gear, lamps, and floor track.
- [x] Add pressure-gate keyed socket, riveted slabs, brass rails, gauge, pressure cylinders, rivets, and warning pipe.
- [x] Expand level validation to require the new gate/key visual objects.
- [x] Rebuild generated scenes.
- [x] Run standalone level validation.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged combat-edge smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.24`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.25 Service-Lift Visual Pass

- [x] Add brass platform deck, iron grate, and threshold to the service-lift shell.
- [x] Add cage rails, cross braces, lift chains, and overhead pulley gear.
- [x] Add call box, gauge, lever, and green signal lamps.
- [x] Apply the lift visual to both Level01 transition and Level02 final exit.
- [x] Expand level validation to require service-lift visual objects.
- [x] Rebuild generated scenes.
- [x] Run standalone level validation.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged combat-edge smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.25`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.26 Pressure-Pistol Visual Pass

- [x] Add pressure tank, tank bands, and red pressure line to the viewmodel.
- [x] Add muzzle crown, rear sight, and front sight.
- [x] Add steam vent chimney, vent cap, bolt handle, and bolt knob.
- [x] Add walnut grip plates and receiver rivets.
- [x] Expand level validation to require pressure-pistol visual objects.
- [x] Rebuild generated scenes.
- [x] Run standalone level validation.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged combat-edge smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.26`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.27 Environment Prop and Signage Pass

- [x] Add reusable procedural pipe-bundle helper.
- [x] Add reusable procedural work-order board helper.
- [x] Add intake and pressure-gate work-order boards to Level01.
- [x] Add gate/final pipe bundles to Level01.
- [x] Add pipeworks work-order board and triple pipe bundle to Level02.
- [x] Expand level validation to require the new environment prop visuals.
- [x] Rebuild generated scenes.
- [x] Run standalone level validation.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged combat-edge smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.27`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.28 Level01 Combat-Space Cover Pass

- [x] Add repair-bay boiler/crate/low-pipe cover.
- [x] Add key-room workbench cover.
- [x] Add final-room west/east stack cover and low center barrier.
- [x] Expand level validation to require the new Level01 cover objects.
- [x] Rebuild generated scenes.
- [x] Run standalone level validation.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged combat-edge smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.28`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.29 Movement and Combat Balance Pass

- [x] Add centralized `GameBalance` profile.
- [x] Apply tuned player movement speed and starting ammo from `GameBalance`.
- [x] Apply tuned Pressure Pistol damage and fire cadence from `GameBalance`.
- [x] Apply tuned Scrapper speed, damage, windup, and obstacle probing from `GameBalance`.
- [x] Apply tuned Lancer fire cadence, projectile speed, and projectile damage from `GameBalance`.
- [x] Expand level validation to require active balance values.
- [x] Rebuild generated scenes.
- [x] Run standalone level validation.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged combat-edge smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.29`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.30 Data-Driven Weapon Definition Pass

- [x] Add `WeaponDefinition` ScriptableObject type.
- [x] Add generated `PressurePistolDefinition.asset`.
- [x] Assign the weapon definition to both gameplay-scene `WeaponController` instances.
- [x] Keep serialized weapon fallback fields backward-compatible.
- [x] Expand level validation to require the weapon definition and its active values.
- [x] Rebuild generated scenes.
- [x] Run standalone level validation.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged combat-edge smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.30`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.31 Data-Driven Enemy Definition Pass

- [x] Add `EnemyDefinition` ScriptableObject type.
- [x] Add generated `ScrapperDefinition.asset`.
- [x] Add generated `LancerDefinition.asset`.
- [x] Assign Scrapper definitions to all melee enemies.
- [x] Assign Lancer definition to the ranged enemy.
- [x] Keep serialized enemy fallback fields backward-compatible.
- [x] Expand level validation to require enemy definitions and active values.
- [x] Rebuild generated scenes.
- [x] Run standalone level validation.
- [x] Run editor smoke.
- [x] Build Windows player.
- [x] Run packaged runtime smoke.
- [x] Run packaged auto-playthrough.
- [x] Run packaged combat smoke.
- [x] Run packaged combat-edge smoke.
- [x] Run packaged ranged combat smoke.
- [x] Run packaged pause-flow smoke.
- [x] Commit and push `v0.0.31`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.32 Build Automation Cleanup Pass

- [x] Add `Tools/RunV0BuildMatrix.ps1`.
- [x] Make the runner read `GameBranding.BuildVersion`.
- [x] Make the runner execute scene rebuild, level validation, editor smoke, Windows build, and packaged smoke tests.
- [x] Make the runner fail if expected pass markers are missing.
- [x] Bump build version to `v0.0.32`.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run packaged runtime smoke through the runner.
- [x] Run packaged auto-playthrough through the runner.
- [x] Run packaged combat smoke through the runner.
- [x] Run packaged combat-edge smoke through the runner.
- [x] Run packaged ranged combat smoke through the runner.
- [x] Run packaged pause-flow smoke through the runner.
- [x] Commit and push `v0.0.32`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.33 Interaction System Foundation Pass

- [x] Add `IInteractable` contract.
- [x] Add `PlayerInteraction` camera scanner using the `E` key.
- [x] Add HUD interaction prompt text.
- [x] Make the pressure gate interactable while retaining key proximity auto-open behavior.
- [x] Make the Level01 service lift interactable while retaining trigger transition behavior.
- [x] Make the Level02 final lift interactable while retaining trigger completion behavior.
- [x] Add packaged interaction smoke test.
- [x] Add interaction smoke to the one-command V0 matrix runner.
- [x] Expand level validation for player interaction wiring and interactable prompts.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.33`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.34 Data-Driven Pickup Definition Pass

- [x] Add `PickupDefinition` ScriptableObject type.
- [x] Add generated `HealthVialDefinition.asset`.
- [x] Add generated `PressureCartridgeDefinition.asset`.
- [x] Add generated `GearKeyDefinition.asset`.
- [x] Assign pickup definitions to all active pickups.
- [x] Move pickup message/audio configuration into definitions.
- [x] Keep `PlayerInventory` direct add methods backward-compatible.
- [x] Expand level validation to require pickup definitions and active values.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.34`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.2 Combat Feel Slice

- [ ] Run manual Windows playthrough.
- [ ] Record tuning notes in `WORK_LEDGER.md`.
- [x] Add first collision-cover pass to `Brassworks Intake`.
- [x] Add initial automated movement/combat balance profile.
- [ ] Tune player movement speed and camera feel.
- [ ] Tune `Pressure Pistol` damage, fire rate, ammo, and feedback.
- [ ] Tune `Scrapper` speed, detection range, damage, and attack cooldown.
- [ ] Confirm `Brassworks Intake` scale and room flow against `LEVEL_DESIGN_AND_MAPS.md`.
- [ ] Manual readability pass for Scrapper attack tells and world labels.
- [ ] Manual listen pass for procedural audio levels and tone.

## v0.3 Art Direction Slice

- [x] Add first primitive brassworks prop silhouettes.
- [x] Generate oil-dark stone material.
- [x] Generate riveted iron wall material.
- [x] Generate brass/copper pipe material.
- [x] Generate gear-key visual.
- [x] Generate pressure-gate visual.
- [x] Generate service-lift visual.
- [x] Generate primitive `Scrapper` visual.
- [x] Generate primitive `Pressure Pistol` visual.
- [x] Replace placeholder hit marker with primitive spark impact VFX.
- [x] Add gauges, valve wheels, furnace props, pipe bundles, and work-order signs.
- [ ] Verify scene readability after art pass.

## v0.4 Systems Foundation

- [x] Data-driven weapon definitions.
- [x] Data-driven enemy definitions.
- [x] Interaction system.
- [x] Pickup/inventory cleanup.
- [ ] Level transition controller.
- [x] Level validation tool.
- [ ] Expand combat automation harness.
- [x] Build automation cleanup.
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
