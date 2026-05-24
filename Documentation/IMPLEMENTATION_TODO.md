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

## v0.0.55 Persistent Objective HUD Pass

- [x] Add persistent objective text and backplate to `HUDController`.
- [x] Generate objective HUD UI in `V0SceneBuilder`.
- [x] Set the starting objective from each scene's `GameStateController.startMessage`.
- [x] Update objectives after gear key pickup, pressure gate opening, Boilerheart valve venting, Warden defeat, player death, and win.
- [x] Validate objective HUD wiring in editor and packaged runtime smoke.
- [x] Expand auto-playthrough to verify objective text updates across the route.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.55`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.56 Standard Machine Death VFX Pass

- [x] Add `MachineDeathVfx` runtime effect.
- [x] Spawn compact death VFX for Scrappers.
- [x] Spawn compact death VFX for Lancers.
- [x] Spawn scaled death VFX for Bulwarks.
- [x] Expand combat smoke to verify Scrapper death VFX.
- [x] Expand Bulwark combat smoke to verify heavy-machine death VFX.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.56`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.57 Steampunk Machinery Motion Pass

- [x] Add reusable `SteamworksSpinner` component.
- [x] Attach spinner motion to pressure-gate header and gear wheels.
- [x] Attach spinner motion to service-lift pulley gears.
- [x] Attach spinner motion to environment valve wheels and the Boilerheart pressure valve wheel.
- [x] Attach spinner motion to the main-menu gear.
- [x] Add level validation for spinner presence and nonzero motion settings.
- [x] Add runtime smoke coverage for spinner component presence.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.57`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.58 Machine Hit VFX Pass

- [x] Add reusable `MachineHitVfx` runtime effect.
- [x] Spawn hit VFX for non-lethal Scrapper damage.
- [x] Spawn hit VFX for non-lethal Lancer damage.
- [x] Spawn scaled hit VFX for non-lethal Bulwark damage.
- [x] Spawn scaled hit VFX for non-lethal Governor Warden damage.
- [x] Expand combat-scenario smoke to verify Scrapper hit VFX.
- [x] Expand Bulwark combat smoke to verify heavy-machine hit VFX.
- [x] Expand Warden combat smoke to verify boss hit VFX.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.58`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.59 Pressure Gate Open VFX Pass

- [x] Add reusable `GateOpenVfx` runtime effect.
- [x] Spawn gate-open VFX from `LockedDoor.Open`.
- [x] Add green pressure wash, steam jets, and brass/green sparks.
- [x] Expand auto-playthrough to verify gate-open VFX during the real key route.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.59`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.60 Service Lift Activation VFX Pass

- [x] Add reusable `LiftActivationVfx` runtime effect.
- [x] Spawn lift activation VFX from `LevelTransitionTrigger`.
- [x] Add a short pressure-engage delay before scene loads.
- [x] Expand auto-playthrough to verify service-lift activation VFX before Level01 transitions to Level02.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.60`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.61 Gear Key Pickup VFX Pass

- [x] Add reusable `GearKeyPickupVfx` runtime effect.
- [x] Spawn key pickup VFX from `Pickup.Collect` for key pickups.
- [x] Add brass ring, center glow, and tooth-spark pieces.
- [x] Expand auto-playthrough to verify key pickup VFX after inventory/objective update.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.61`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.62 Resource Pickup VFX Pass

- [x] Add reusable `ResourcePickupVfx` runtime effect.
- [x] Spawn red medicinal VFX for health pickups.
- [x] Spawn brass/cyan pressure VFX for ammo pickups.
- [x] Expand auto-playthrough to verify health pickup VFX.
- [x] Expand auto-playthrough to verify ammo pickup VFX.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.62`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.63 Steam Hazard VFX Pass

- [x] Add reusable `SteamHazardVfx`.
- [x] Wire low/high steam puffs in generated steam hazards.
- [x] Add level validation for animated steam puffs.
- [x] Expand hazard smoke to verify steam puffs.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.63`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.64 Furnace Heat VFX Pass

- [x] Add reusable `FurnaceHeatHazardVfx`.
- [x] Wire warning, active, and safe phase signal animation.
- [x] Add active heat-ripple pieces to generated furnace heat hazards.
- [x] Add level validation for furnace heat VFX wiring.
- [x] Expand hazard smoke to verify active heat ripples.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.64`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.65 Machine Motion Pass

- [x] Add reusable `MachineMotionVfx`.
- [x] Wire procedural motion for Scrappers.
- [x] Wire procedural motion for Lancers.
- [x] Wire procedural motion for Bulwarks.
- [x] Wire procedural motion for the Governor Warden.
- [x] Add level validation for enemy machine motion.
- [x] Expand runtime smoke to require configured machine motion.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.65`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.66 Pressure Bolt VFX Pass

- [x] Add reusable `PressureBoltVfx`.
- [x] Orient pressure bolts along travel direction.
- [x] Add pressure-bolt glow/trail/spark pieces.
- [x] Wire projectile VFX for Lancers.
- [x] Wire projectile VFX for the Governor Warden.
- [x] Expand ranged-combat smoke to verify visible pressure-bolt VFX.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.66`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.67 Pressure Pistol Impact Decal Pass

- [x] Add reusable `ImpactDecalVfx`.
- [x] Replace spark-only hit marker with scorch/brass/spark impact decal VFX.
- [x] Expand combat-scenario smoke to verify impact decal VFX on a non-lethal hit.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.67`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.68 Player Damage VFX Pass

- [x] Add reusable `PlayerDamageVfx`.
- [x] Spawn first-person pressure/heat/brass hurt burst from `PlayerHealth.TakeDamage`.
- [x] Expand combat-edge smoke to verify player damage VFX after melee damage.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.68`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.69 Lore Plaque Pass

- [x] Add reusable `LorePlaque` interactable.
- [x] Place compact archive plaques in Level01 through Level05.
- [x] Add short Brassworks lore text to each plaque.
- [x] Add level validation for plaque triggers, prompts, and narrative text.
- [x] Expand interaction smoke to read a plaque and verify HUD text.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.69`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.70 Pipeworks Routing Valve Pass

- [x] Make `SteamValveObjective` support configurable follow-up objective text.
- [x] Add `Pipeworks Routing Valve Objective` to Level02.
- [x] Lock the Level02 Boilerheart lift until the routing valve is complete.
- [x] Add level validation for the routing valve, lock wiring, and visual pieces.
- [x] Expand auto-playthrough to verify locked lift rejection, valve completion, objective update, and Level02 transition.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.70`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.71 Pipeworks Secret Cache Pass

- [x] Add `Secret - Pipeworks Cartridge Cache` to Level02.
- [x] Add health and pressure-cartridge rewards to the cache.
- [x] Add level validation for the Pipeworks secret cache and ammo reward.
- [x] Expand auto-playthrough final run-stat check to require at least three registered secrets.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.71`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.72 Brassworks Ambience Pass

- [x] Add procedural looping brassworks ambience to `SteamworksAudio`.
- [x] Apply ambience volume through the existing master-volume setting.
- [x] Add level validation for ambience configuration.
- [x] Expand runtime smoke to verify the packaged ambience loop is active.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.72`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.73 Pressure Burst Alternate Fire Pass

- [x] Add secondary Pressure Burst tuning to `GameBalance` and `WeaponDefinition`.
- [x] Add right-mouse secondary fire to `WeaponController`.
- [x] Make Pressure Burst consume three cartridges and fire a short-range deterministic pellet pattern.
- [x] Add level validation for controller and definition secondary-fire values.
- [x] Expand combat-scenario smoke to verify secondary burst ammo use before the primary-shot kill flow.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.73`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.74 Steam Scattergun Prototype Pass

- [x] Add Steam Scattergun balance values and data-driven weapon definition.
- [x] Add generic primary pellet/ammo/spread fields to weapon definitions.
- [x] Add weapon unlock state and transition persistence.
- [x] Add `1`/`2` weapon switching.
- [x] Add Boilerheart Steam Scattergun pickup definition and primitive pickup visual.
- [x] Add packaged weapon-switch smoke test and matrix step.
- [x] Add level validation for scattergun definition, pickup, and weapon-switch test wiring.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.74`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.75 Steam Scattergun Viewmodel Pass

- [x] Add a distinct first-person Steam Scattergun viewmodel.
- [x] Add weapon-view references to `WeaponController`.
- [x] Toggle Pressure Pistol and Steam Scattergun viewmodels when switching weapons.
- [x] Expand weapon-switch smoke to verify active viewmodel swapping.
- [x] Expand level validation to require Steam Scattergun viewmodel pieces.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.75`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.76 Steam Scattergun Blast VFX Pass

- [x] Add reusable `ScattergunBlastVfx`.
- [x] Spawn Steam Scattergun pressure-ring, steam-core, and brass-spark blast feedback when firing.
- [x] Expand weapon-switch smoke to verify the scattergun blast VFX spawns with visible pieces.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.76`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.77 Steam Scattergun Audio Cue Pass

- [x] Add `SteamworksAudioCue.SteamScattergunFire`.
- [x] Generate a dedicated low-pressure procedural scattergun blast clip.
- [x] Route Steam Scattergun fire through its dedicated cue while preserving Pressure Pistol audio.
- [x] Expand runtime smoke to verify scattergun cue configuration.
- [x] Expand weapon-switch smoke to verify scattergun audio routing.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.77`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.78 Bellows Node Support Enemy Pass

- [x] Add `BellowsNodeController`.
- [x] Add `BellowsNodePulseVfx`.
- [x] Add Bellows Node balance values and `BellowsNodeDefinition.asset`.
- [x] Add primitive Bellows Node silhouette to Level03.
- [x] Add packaged `RuntimeBellowsNodeTest`.
- [x] Add `V0_BELLOWS_NODE_PASS` to the one-command matrix.
- [x] Expand validation for Bellows Node definition, balance, visual pieces, and machine motion.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.78`.

## v0.0.79 Bellows Node Pressure Boost Pass

- [x] Add reusable pressure-boost state to standard `EnemyController` machines.
- [x] Let Bellows Node pulses boost nearby Scrappers for a short duration.
- [x] Add Bellows Node boost duration and multiplier balance constants.
- [x] Wire boost values into generated Bellows Node scene placement.
- [x] Expand validation for Bellows Node boost tuning.
- [x] Expand packaged `RuntimeBellowsNodeTest` to verify nearby Scrapper boost.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.79`.

## v0.0.80 Bellows Node Boost Readability Pass

- [x] Add `PressureBoostVfx` procedural brass/steam overdrive effect.
- [x] Trigger `PressureBoostVfx` from Scrapper pressure boost state.
- [x] Disable boost VFX primitive colliders immediately so procedural art cannot block weapon tests.
- [x] Expand packaged `RuntimeBellowsNodeTest` to verify boost VFX visibility.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.80`.

## v0.0.81 Bellows Node Pulse Audio Pass

- [x] Add `SteamworksAudioCue.BellowsNodePulse` without shifting existing serialized cue values.
- [x] Add dedicated procedural bellows/steam pulse clip.
- [x] Track last spatial audio cue in `SteamworksAudio` for packaged routing verification.
- [x] Route Bellows Node pulses through the dedicated support cue.
- [x] Expand runtime smoke for support cue configuration.
- [x] Expand Bellows Node smoke for dedicated spatial pulse-audio routing.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.81`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.82 Steam Scattergun Pickup Feedback Pass

- [x] Add reusable `WeaponPickupVfx` brass/steam acquisition effect.
- [x] Spawn `WeaponPickupVfx` for weapon pickups instead of the generic resource pickup burst.
- [x] Route weapon-switch smoke to Level03 so Steam Scattergun acquisition uses the real world pickup.
- [x] Expand weapon-switch smoke to verify pickup unlock, immediate equip, active weapon identity, and acquisition VFX visibility.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.82`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.83 Weapon Pickup Audio Pass

- [x] Add `SteamworksAudioCue.WeaponPickup` at the end of the enum without shifting existing serialized cue values.
- [x] Generate a dedicated procedural brass latch, pressure rise, gear chime, and steam bloom pickup clip.
- [x] Route the Steam Scattergun pickup definition through the dedicated weapon-pickup cue.
- [x] Expand runtime smoke to verify weapon-pickup cue configuration.
- [x] Expand weapon-switch smoke to verify real Level03 pickup audio routing before fire tests.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.83`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.84 Steam Scattergun Slug Identity Pass

- [x] Add `SteamworksAudioCue.SteamScattergunSlug` at the end of the enum without shifting existing serialized cue values.
- [x] Generate a dedicated procedural pressure crack, bolt clang, pipe whistle, and steam jet slug clip.
- [x] Add `ScattergunSlugVfx` pressure-spear effect.
- [x] Route Steam Scattergun secondary fire through slug-specific audio/VFX while keeping primary blast feedback unchanged.
- [x] Expand runtime smoke to verify slug cue configuration.
- [x] Expand weapon-switch smoke to verify slug ammo cost, cue routing, VFX spawn, non-lethal slug damage, primary kill, and pistol re-equip.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.84`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.85 Steam Scattergun World Pickup Art Pass

- [x] Add a Level03 pickup display stand, iron yoke, and enamel nameplate.
- [x] Add brass top rib, walnut pump grip, pressure coil, rear valve wheel, and shell-rack rounds to the Steam Scattergun world pickup.
- [x] Expand Level03 validation to require named pickup-polish pieces.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.85`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.86 Steam Scattergun Pickup Readability Pass

- [x] Add brass route strips and floor chevrons leading into the Level03 Steam Scattergun pickup.
- [x] Add an overhead sign, `BREACH TOOL` world label, lamps, and pressure piping around the pickup.
- [x] Expand Level03 validation to require named pickup readability cues.
- [x] Create `Documentation/PARALLEL_WORKSTREAM_STATUS.md` for side-agent coordination.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.86`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.87 Pressure Burst Feedback Pass

- [x] Add `SteamworksAudioCue.PressureBurst` at the end of the enum without shifting existing serialized cue values.
- [x] Generate a dedicated procedural pressure-dump audio clip for the Pressure Pistol secondary fire.
- [x] Add `PressureBurstVfx` pressure-ring, steam-core, brass-valve flash, and shard burst effect.
- [x] Route Pressure Pistol secondary fire through dedicated Pressure Burst audio/VFX while keeping primary fire unchanged.
- [x] Expand runtime smoke to verify Pressure Burst cue configuration.
- [x] Expand combat-scenario smoke to verify Pressure Burst audio routing and VFX spawn.
- [x] Rebuild generated scenes through the runner.
- [x] Build Windows player through the runner.
- [x] Run full packaged smoke matrix through the runner.
- [x] Commit and push `v0.0.87`.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.88 Pressure Pistol Viewmodel Polish Pass

- [x] Add secondary-fire animation support to `WeaponView`.
- [x] Add Pressure Pistol pressure-dump viewmodel cue references: gauge needle, valve wheel, dump lever, pressure chamber, and vent flash.
- [x] Route secondary fire through `WeaponView.PlayFire(secondaryShot: true)` while keeping primary recoil behavior intact.
- [x] Add Pressure Pistol relief nozzle and dump lever geometry to generated scenes.
- [x] Expand level validation to require the new pressure-dump cue objects.
- [x] Expand combat-scenario smoke to verify secondary pressure viewmodel cues animate.
- [x] Build `v0.0.88` through the full V0 matrix.
- [x] Integrate completed side-agent staging outputs for PBR material textures, enemy OBJ blockouts, and weapon/prop OBJ blockouts.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.89 Scrapper Attack Readability Pass

- [x] Add `ScrapperAttackTellVfx` with cutter-edge warning glows, furnace flare, pressure surge, steam puffs, brass sparks, and ground warning ring.
- [x] Add `SteamworksAudioCue.EnemyAttackTell` at the end of the enum without shifting existing serialized cue values.
- [x] Generate a dedicated procedural ratchet/pressure-rise attack-tell audio clip.
- [x] Attach configured attack-tell components to generated Scrappers.
- [x] Expand level validation to require configured Scrapper attack-tell components.
- [x] Expand runtime smoke to verify the new attack-tell audio cue.
- [x] Expand combat-edge smoke to verify Scrapper tell VFX/audio before melee damage.
- [x] Build `v0.0.89` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.90 Scrapper Shutdown Polish Pass

- [x] Add a Scrapper-specific `MachineDeathVfx` shutdown style without changing Lancer/Bulwark/support-machine shutdown hooks.
- [x] Add boiler cap, chest plate, cutter shards, flywheel gears, valve wheel, pressure-tank burst, and furnace-flash pieces to the Scrapper shutdown variant.
- [x] Route `EnemyController.Die()` through the Scrapper shutdown variant.
- [x] Expand combat smoke to require Scrapper-specific shutdown detail.
- [x] Integrate side-agent batch 02 room/object concept renders under `Documentation/ConceptRenders/`.
- [x] Build `v0.0.90` through the full V0 matrix.

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
