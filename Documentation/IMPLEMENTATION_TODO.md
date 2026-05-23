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

## v0.0.35 Level Transition Controller Pass

- [x] Add scene-local `LevelTransitionController`.
- [x] Route service-lift scene loads through `LevelTransitionController`.
- [x] Route gameplay restart through `LevelTransitionController` when present.
- [x] Preserve fallback direct scene loading for safety.
- [x] Add controller to generated gameplay scenes.
- [x] Expand runtime smoke and level validation to require the controller.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.35`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.36 Platform Quality Profile Pass

- [x] Add `PlatformQualityProfile` ScriptableObject type.
- [x] Add generated `WindowsMidLowQualityProfile.asset`.
- [x] Add generated `AndroidPhoneQualityProfile.asset`.
- [x] Add generated `WebGLBrowserQualityProfile.asset`.
- [x] Add generated `PcVrQualityProfile.asset`.
- [x] Add generated `MetaQuestQualityProfile.asset`.
- [x] Assign Windows profile asset to menu and gameplay runtime performance profiles.
- [x] Expand level validation to require the active Windows quality profile.
- [x] Expand runtime smoke to require the applied Windows profile asset.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.36`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.37 Expanded Combat Automation Pass

- [x] Add `RuntimeCombatScenarioTest`.
- [x] Verify pressure-pistol cooldown rejection preserves ammo.
- [x] Verify ammo decreases once per valid shot.
- [x] Verify Scrapper survives until the final expected shot.
- [x] Verify final expected shot destroys the Scrapper.
- [x] Add `V0_COMBAT_SCENARIO_PASS` to the build matrix runner.
- [x] Add combat scenario automation routing from the main menu.
- [x] Expand runtime smoke and level validation to require the scenario test component.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.37`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.38 Level03 Boilerheart Foundation Pass

- [x] Add generated `Assets/_Project/Scenes/Level03.unity`.
- [x] Add Boilerheart Core blockout.
- [x] Add Level03 enemies, pickups, dressing, work-order board, pipe bundle, and final lift.
- [x] Change Level02 service lift from win exit to transition into Level03.
- [x] Expand build scene order to MainMenu, Level01, Level02, Level03.
- [x] Expand level validation for Level03 and four-scene order.
- [x] Expand editor smoke for Level03.
- [x] Expand auto-playthrough to validate Level01 -> Level02 -> Level03 -> win.
- [x] Update `LEVEL_DESIGN_AND_MAPS.md`.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.38`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.39 Boilerheart Pressure-Valve Objective Pass

- [x] Add reusable `SteamValveObjective`.
- [x] Lock Level03 final lift behind Boilerheart pressure venting.
- [x] Generate Boilerheart pressure-valve objective visuals.
- [x] Validate final lift/valve linkage and valve visuals.
- [x] Expand auto-playthrough to verify locked-lift rejection, valve venting, and final win.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.39`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.40 Steam Hazard Foundation Pass

- [x] Add reusable `SteamHazard`.
- [x] Add steam hazard balance values.
- [x] Place two Boilerheart steam hazard volumes.
- [x] Add primitive vent/puff/floor warning visuals.
- [x] Add validation for hazard triggers and balance values.
- [x] Add packaged hazard smoke test.
- [x] Add hazard smoke to the one-command build matrix.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.40`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.41 Scene Objective Briefing Pass

- [x] Add `GameStateController.startMessage`.
- [x] Generate Level01 objective briefing.
- [x] Generate Level02 objective briefing.
- [x] Generate Level03 objective briefing.
- [x] Validate scene-specific briefing messages.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.41`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.42 Boilerheart Hazard Shutdown Pass

- [x] Link Boilerheart pressure valve to steam hazards.
- [x] Disable linked hazards when the valve is vented.
- [x] Validate valve-to-hazard linkage.
- [x] Expand auto-playthrough to verify hazard shutdown.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.42`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.43 Secret Area Foundation Pass

- [x] Add reusable `SecretArea`.
- [x] Add Intake pressure cache secret.
- [x] Add health/ammo rewards to the secret cache.
- [x] Add validation for secret trigger and visuals.
- [x] Add packaged secret smoke test.
- [x] Add secret smoke to the one-command build matrix.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.43`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.44 Run Secret Stats Pass

- [x] Add persistent `RunStats`.
- [x] Register secrets when scenes load.
- [x] Mark secret discoveries in run stats.
- [x] Reset run stats from main menu start.
- [x] Display secret progress on win message when secrets exist.
- [x] Expand secret smoke to verify run-stat discovery.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.44`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.45 Secret Stats Auto-Playthrough Pass

- [x] Expand auto-playthrough to verify secret totals persist to win.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.45`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.46 Level04 Furnace Foundry Foundation Pass

- [x] Add generated `Assets/_Project/Scenes/Level04.unity`.
- [x] Convert Level03 final win lift into a valve-gated transition to `Level04`.
- [x] Add lock support to `LevelTransitionTrigger`.
- [x] Add Furnace Foundry blockout, enemies, pickups, steam hazards, furnace-row dressing, work-order board, and first emergency-hoist route.
- [x] Expand build scene order to MainMenu, Level01, Level02, Level03, Level04.
- [x] Expand level validation and editor smoke for Level04.
- [x] Expand auto-playthrough to validate Level01 -> Level02 -> Level03 -> Level04 -> win.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.46`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.47 Furnace Heat Hazard Pass

- [x] Add reusable `FurnaceHeatHazard`.
- [x] Add furnace heat balance values.
- [x] Place two pulsing Furnace Foundry heat-surge lanes.
- [x] Add warning, active, and safe phase visuals.
- [x] Add level validation for furnace hazard setup and visuals.
- [x] Expand packaged hazard smoke to verify Level03 steam damage and Level04 furnace-heat damage.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.47`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.48 Bulwark Heavy Enemy Pass

- [x] Add `BulwarkEnemyController`.
- [x] Add Bulwark balance values and `BulwarkDefinition.asset`.
- [x] Add primitive Bulwark silhouette with boiler body, furnace belly, pressure tank, piston legs, and hammer arms.
- [x] Place the first Bulwark in `Level04`.
- [x] Add validation for Bulwark definition, balance, and visuals.
- [x] Add packaged Bulwark combat smoke.
- [x] Add Bulwark combat smoke to the one-command build matrix.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.48`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.49 Foundry Secret Cache Pass

- [x] Add Level04 `Secret - Foundry Coal Cache`.
- [x] Add foundry secret reward pickups.
- [x] Add coal-bin/floor-plate/cache visual props.
- [x] Expand level validation to require Level04 secrets and foundry cache visuals.
- [x] Expand auto-playthrough to verify multi-level secret totals at win.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.49`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.50 Governor Core Foundation Pass

- [x] Add generated `Assets/_Project/Scenes/Level05.unity`.
- [x] Convert Level04 `Foundry Emergency Hoist` from final win device into a transition to `Level05`.
- [x] Add Governor Core blockout, regulator dressing, work-order board, pipe bundle, steam hazard, and furnace-heat hazard.
- [x] Add mixed Scrapper/Lancer/Bulwark enemy pressure in Level05.
- [x] Add Level05 master override hoist as the current final win device.
- [x] Expand build scene order to MainMenu, Level01, Level02, Level03, Level04, Level05.
- [x] Expand level validation and editor smoke for Level05.
- [x] Expand auto-playthrough to validate Level01 -> Level02 -> Level03 -> Level04 -> Level05 -> win.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.50`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.51 Governor Warden Guardian Pass

- [x] Add `GovernorWardenController`.
- [x] Add Governor Warden balance values and `GovernorWardenDefinition.asset`.
- [x] Add primitive Governor Warden silhouette with core body, furnace heart, pressure crown, back boiler, piston arms, stomp plates, and pressure cannon muzzle.
- [x] Place the first Governor Warden in `Level05`.
- [x] Add Warden stomp, pressure-bolt, and enraged half-health behavior.
- [x] Add validation for Warden definition, balance, muzzle wiring, and visual pieces.
- [x] Add packaged Warden combat smoke.
- [x] Add Warden combat smoke to the one-command build matrix.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.51`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.52 Warden-Gated Finale Pass

- [x] Add `GuardianDefeatObjective`.
- [x] Extend `ExitTrigger` to support guardian-locked final exits.
- [x] Link Level05 master override hoist to the Governor Warden defeat objective.
- [x] Add Warden lock red/green signal props near the final hoist.
- [x] Expand validation for guardian objective wiring and locked final hoist state.
- [x] Expand auto-playthrough to verify locked final hoist rejection, Warden defeat, unlock, and win.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.52`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.53 Warden Boss Health HUD Pass

- [x] Add hidden top-center boss health HUD fields and generated UI elements.
- [x] Show/update/hide boss health from `GovernorWardenController`.
- [x] Validate HUD boss health wiring in `V0LevelValidator`.
- [x] Check HUD boss health wiring in packaged runtime smoke.
- [x] Expand Warden combat smoke to verify the boss bar appears and drops after damage.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.53`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.54 Warden Shutdown VFX Pass

- [x] Add `WardenShutdownVfx` runtime effect.
- [x] Spawn Warden shutdown steam, brass sparks, and pressure ring on defeat.
- [x] Update Warden defeat message to match the vented pressure-machine fantasy.
- [x] Expand Warden combat smoke to verify shutdown VFX visible pieces.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.54`.

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
- [x] Level transition controller.
- [x] Level validation tool.
- [x] Expand combat automation harness.
- [x] Build automation cleanup.
- [x] Platform asset-quality settings.

## Platform Planning

Windows is the primary development target. Android, browser/WebGL, and VR are deferred until after the full Windows game is built, but their constraints must influence architecture and assets as they are created.

- [x] Add Windows target profile.
- [x] Add Android port notes.
- [x] Add browser/WebGL port notes.
- [x] Add Steam/Meta VR port notes.
- [x] Add Windows runtime quality profile in Unity.
- [x] Add Android quality profile in Unity.
- [x] Add WebGL quality profile in Unity.
- [x] Add future VR quality profiles in Unity.
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
