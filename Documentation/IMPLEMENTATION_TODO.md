# Brassworks Breach - Implementation To-Do

This checklist tracks implementation work. Detailed production tracking lives in `WORK_LEDGER.md`.

## Operating Directive

- [x] Maintain `CONTINUATION_DIRECTIVE.md`.
- [x] At the end of each completed step, update records, verify, commit/push when appropriate, and immediately begin the next highest-impact unfinished task.
- [x] Default to ambitious milestone batching after `v0.1.32`; each compile should be a visible leap.
- [x] Run independent asset, level, weapon/prop, enemy/readability, and QA work in parallel when write scopes can be separated.

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

## v0.1.35 Gameplay Feedback Systems Batch

- [x] Add shared non-authoritative feedback event taxonomy.
- [x] Add `GameplayFeedbackController` and primitive fallback pulse VFX.
- [x] Hook feedback events into weapon fire, impact, empty, and switch paths.
- [x] Hook feedback events into health/ammo/key/weapon pickup collection.
- [x] Hook feedback events into Scrapper, Lancer, Bellows Node, Bulwark, and Warden hit/death paths.
- [x] Hook feedback events into route-blocked, objective-complete, secret-found, pause/resume, settings, and Warden phase-change paths.
- [x] Add runtime feedback smoke coverage and include it in the Windows matrix.
- [x] Rebuild scenes, run editor validation/smoke, route audit, full Windows matrix, package, QA packet, issue-triage packet, and candidate-readiness generation.

## v0.1.36 Sidecar Package Gate And Visual Import

- [x] Create Unity sidecar asset-pack pipeline documentation.
- [x] Generate weapon, mechanical enemy, level-kit, and integration-gate sidecar bundles in parallel.
- [x] Run first sidecar validator scan.
- [x] Remediate package-local `Documentation~` manifests and import-smoke metadata.
- [x] Rerun sidecar validator for completed weapon, enemy, and level-kit packages with zero errors and zero warnings.
- [x] Import weapon, mechanical enemy, feedback FX/audio, and Steamworks level-kit sidecars as local Unity packages.
- [x] Add sidecar quarantine import validator and pass `SIDECAR_QUARANTINE_IMPORT_PASS`.
- [x] Add visual-only sidecar showcase placements across all five levels.
- [x] Validate sidecar showcases add no colliders, rigidbodies, autonomous audio, or gameplay authority.
- [x] Rebuild scenes, run route audit, run full Windows matrix, package, QA packet, issue-triage packet, and candidate-readiness generation.
- [x] Commit and push the v0.1.36 sidecar visual-import milestone.

## v0.1.37 World Label Readability Batch

- [x] Add billboarded world-label readability component.
- [x] Add generated dark backplates behind floating route, cache, pickup, and sidecar showcase labels.
- [x] Add high-contrast world-label response for readable first-person navigation.
- [x] Add packaged `RuntimeWorldLabelReadabilityTest` coverage.
- [x] Wire `V0_WORLD_LABEL_READABILITY_PASS` into the Windows matrix and candidate-readiness gate.
- [x] Rebuild scenes, run route audit, run full Windows matrix, package, QA packet, issue-triage packet, and candidate-readiness generation.
- [x] Commit and push the v0.1.37 world-label readability milestone.

## Parallel Priority: v0.1.40 Sidecar Asset Bundles

- [ ] Complete broad steampunk material family sidecar package.
- [ ] Generate package-local manifest and preview swatches/renders.
- [ ] Run package-specific sidecar validator.
- [ ] Review import risk and decide whether to import package or bind selected materials into gameplay surfaces.
- [ ] Complete level-dressing, mechanical-enemy-visual, and weapon-props sidecar packages in parallel.
- [ ] Validate completed sidecars package-by-package before any main-project import.

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

## v0.0.91 Lancer Firing Tell Pass

- [x] Add `LancerFireTellVfx` with muzzle charge ring, pressure needle, coil bloom, furnace-lens flare, pressure puff, steam puffs, and brass sparks.
- [x] Add `SteamworksAudioCue.LancerFireTell` at the end of the enum without shifting existing serialized cue values.
- [x] Generate a dedicated procedural valve-tick/coil-charge fire-tell audio clip.
- [x] Attach configured fire-tell components to generated Lancers.
- [x] Expand level validation to require configured Lancer fire-tell components.
- [x] Expand runtime smoke to verify the new Lancer fire-tell audio cue.
- [x] Expand ranged-combat smoke to verify Lancer fire-tell VFX/audio before pressure-bolt VFX and player damage.
- [x] Integrate completed high-fidelity lookdev staging outputs for corridor/door, pressure pistol, and Scrapper-like monster targets.
- [x] Build `v0.0.91` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.92 Pressure-Bolt Impact Feedback Pass

- [x] Add swept pressure-bolt collision checks so fast bolts reliably resolve player/world hits.
- [x] Add `PressureBoltImpactVfx` with pressure flash, brass/pressure rings, steam pop, and shard burst.
- [x] Route player and world pressure-bolt impacts through the new impact effect before bolt destruction.
- [x] Expand ranged-combat smoke so Lancer damage requires visible pressure-bolt impact VFX.
- [x] Integrate high-fidelity lookdev recovery docs after Batch01 visual rejection.
- [x] Narrow the active parallel lookdev lane to pressure-pistol-only proof work.
- [x] Build `v0.0.92` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.0.93 Bulwark Attack Readability Pass

- [x] Add `BulwarkAttackTellVfx` with slam-warning ground ring, hammer plates, furnace overpressure, steam surge, timing ring, and brass sparks.
- [x] Add `SteamworksAudioCue.BulwarkAttackTell` at the end of the enum without shifting existing serialized cue values.
- [x] Generate a dedicated procedural hammer/boiler/chain windup audio clip.
- [x] Attach configured attack-tell components to generated Bulwarks.
- [x] Expand level validation to require configured Bulwark attack-tell components.
- [x] Expand runtime smoke to verify the new Bulwark attack-tell audio cue.
- [x] Expand Bulwark combat smoke to verify tell VFX/audio before slam damage.
- [x] Build `v0.0.93` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.0 Settings Accessibility Slice

- [x] Add persisted flash-intensity setting to `GameSettings`.
- [x] Add flash sliders to the generated main menu and pause menu.
- [x] Apply flash intensity to HUD damage flash and first-person player damage VFX.
- [x] Expand scene validation for flash slider references and min/max range.
- [x] Expand runtime pause-flow smoke to verify the flash setting and label.
- [x] Build `v0.1.0` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.1 Bulwark Shutdown Polish

- [x] Add a Bulwark-specific `MachineDeathVfx` shutdown style.
- [x] Add heavy Bulwark death fragments: furnace core, boiler shells, hammer fragment, piston rods, shoulder plates, rear tank, gauge burst, chimney cap, and foot shards.
- [x] Route `BulwarkEnemyController` death through the dedicated shutdown style.
- [x] Expand Bulwark combat smoke to require Bulwark-specific shutdown detail.
- [x] Build `v0.1.1` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.2 Route Audit / Playtest Capture Slice

- [x] Add editor-side `V0RouteAudit` scene inspection.
- [x] Add `Tools/RunV0RouteAudit.ps1` helper.
- [x] Generate `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.2.md`.
- [x] Confirm no route-blocking composition issues were found by the deterministic audit.
- [x] Build `v0.1.2` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.3 Core Movement And Camera Feel

- [x] Move player acceleration, deceleration, gravity, ground-stick velocity, and pitch limit into `GameBalance`.
- [x] Update `PlayerController` to use accelerated horizontal velocity instead of instant full-speed starts/stops.
- [x] Keep mouse sensitivity clamped through `GameSettings` and menu defaults.
- [x] Add `RuntimeMovementFeelTest` and `-v0MovementSmoke` packaged launch coverage.
- [x] Expand scene generation, level validation, runtime smoke, and build matrix coverage for the movement-feel test.
- [x] Refresh `V0RouteAudit` output for `v0.1.3`.
- [x] Build `v0.1.3` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.4 Weapon, Ammo, And Enemy Pressure Balance

- [x] Retune `GameBalance` for starting ammo, ammo pickup amount, Pressure Pistol, Pressure Burst, Steam Scattergun, and Steam Scattergun slug values.
- [x] Retune Scrapper, Lancer, Bellows Node, Bulwark, and Governor Warden health/damage/cooldown/pressure values.
- [x] Centralize Pressure Pistol range plus Scrapper attack range/cooldown in `GameBalance`.
- [x] Update generated data assets and level validation to enforce the new values.
- [x] Add `RuntimeBalanceEnvelopeTest` and `-v0BalanceSmoke` packaged launch coverage.
- [x] Expand runtime smoke and the full build matrix for `V0_BALANCE_ENVELOPE_PASS`.
- [x] Refresh `V0RouteAudit` output for `v0.1.4`.
- [x] Build `v0.1.4` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.5 Level01 Brassworks Intake Flow Polish

- [x] Add generated gate-preview sightline rails/header/gauge/lamp before the pressure gate.
- [x] Add generated key-branch return pipe, amber plate, chevrons, and gauge cues.
- [x] Add generated service-lift green runway, chevrons, overhead pipe, and beacon light.
- [x] Add generated secret-cache clue props near the Intake pressure cache without moving the secret trigger.
- [x] Add validator coverage for the new Level01 flow-polish props.
- [x] Add `RuntimeLevel01FlowTest` and `-v0Level01FlowSmoke` packaged launch coverage.
- [x] Expand runtime smoke and the full build matrix for `V0_LEVEL01_FLOW_PASS`.
- [x] Refresh `V0RouteAudit` output for `v0.1.5`.
- [x] Build `v0.1.5` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.6 Level02/Level03 Midgame Readability Polish

- [x] Add generated Pipeworks locked-lift stop bar, pressure line, and lock gauge.
- [x] Add generated Pipeworks routing-valve lead/return cues and amber valve light.
- [x] Add generated Pipeworks Lancer sightline cover/readability props without blocking ranged smoke.
- [x] Add generated Pipeworks secret clue props near the cartridge cache without moving the secret trigger.
- [x] Add generated Boilerheart ring guide strips and scattergun trial-lane cues.
- [x] Add generated Bellows Node pulse-radius marker, boost pipe, and amber pulse-read light.
- [x] Add generated Boilerheart valve-to-lift green return strip, foundry-lift stop bar, and hazard shutdown sight glass.
- [x] Add validator coverage for the new Level02/Level03 flow-polish props.
- [x] Add `RuntimeMidgameFlowTest` and `-v0MidgameFlowSmoke` packaged launch coverage.
- [x] Expand runtime smoke and the full build matrix for `V0_MIDGAME_FLOW_PASS`.
- [x] Refresh `V0RouteAudit` output for `v0.1.6`.
- [x] Build `v0.1.6` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.7 Level04/Level05 Climax Readability Polish

- [x] Add generated Foundry furnace timing preview strip, safe-edge rail, and heat warning gauge.
- [x] Add generated Foundry Bulwark hammer-bay floor ring, boundary, and retreat cover signals.
- [x] Add generated Foundry hoist green runway/beacon and coal-cache clue props.
- [x] Add generated Governor Core Warden reveal centerline, arena boundary ring, lock stop-bar, and boss cover pylons.
- [x] Add generated Governor Core master override runway/beacon cues.
- [x] Add validator coverage for the new Level04/Level05 climax-polish props.
- [x] Add `RuntimeClimaxFlowTest` and `-v0ClimaxFlowSmoke` packaged launch coverage.
- [x] Expand runtime smoke and the full build matrix for `V0_CLIMAX_FLOW_PASS`.
- [x] Refresh `V0RouteAudit` output for `v0.1.7`.
- [x] Build `v0.1.7` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.8 AudioV1 Mix And Import Tuning

- [x] Add serialized `SteamworksAudioMixBinding` entries for every `SteamworksAudioCue`.
- [x] Tune cue-volume multipliers for weapon fire, pickups, enemy tells, hazards, gate feedback, player hurt, and win feedback.
- [x] Mark priority enemy/hazard/gate cues with spatial-intent flags for future emitter placement.
- [x] Raise active brassworks ambience mix volume for the first Windows mix pass.
- [x] Apply AudioV1 importer tuning: ambience/loop clips use Vorbis compressed-in-memory background loading, while short one-shots use ADPCM decompressed/preloaded settings.
- [x] Add AudioV1 import-shape and mix-profile validator coverage.
- [x] Add `RuntimeAudioMixTest` and `-v0AudioMixSmoke` packaged launch coverage.
- [x] Expand runtime smoke and the full build matrix for `V0_AUDIO_MIX_PASS`.
- [x] Refresh `V0RouteAudit` output for `v0.1.8`.
- [x] Build `v0.1.8` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.9 Settings, Readability, And Windows Options Polish

- [x] Add persisted resolution preset settings to `GameSettings`.
- [x] Add persisted fullscreen setting to `GameSettings`.
- [x] Wire resolution cycling and fullscreen toggle controls into the main menu.
- [x] Wire resolution cycling and fullscreen toggle controls into the pause menu.
- [x] Add HUD objective, interaction prompt, boss label, and message best-fit readability constraints.
- [x] Add editor validation for settings control wiring and HUD text readability settings.
- [x] Add `RuntimeDisplaySettingsTest` and `-v0DisplaySettingsSmoke` packaged launch coverage.
- [x] Expand runtime smoke and the full build matrix for `V0_DISPLAY_SETTINGS_PASS`.
- [x] Harden local Unity runners against stale Library lock files between batchmode invocations.
- [x] Refresh `V0RouteAudit` output for `v0.1.9`.
- [x] Build `v0.1.9` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.10 High-Contrast Readability Controls

- [x] Add persisted high-contrast/readability setting to `GameSettings`.
- [x] Wire high-contrast toggle controls into the main menu.
- [x] Wire high-contrast toggle controls into the pause menu.
- [x] Apply high-contrast HUD text and backplate styling at runtime.
- [x] Add editor validation for high-contrast control wiring.
- [x] Add `RuntimeReadabilitySettingsTest` and `-v0ReadabilitySmoke` packaged launch coverage.
- [x] Expand runtime smoke and the full build matrix for `V0_READABILITY_SETTINGS_PASS`.
- [x] Refresh `V0RouteAudit` output for `v0.1.10`.
- [x] Build `v0.1.10` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.11 Pressure Gauge Asset Promotion

- [x] Review Unity-only lookdev outputs and keep non-production renders in the reference lane.
- [x] Add `PressureGaugePrototype` runtime metadata for promoted gauge components.
- [x] Replace the Pressure Pistol viewmodel gauge with a named multi-part gauge prototype.
- [x] Replace the Level01 pressure-gate panel gauge with the same component language.
- [x] Add editor validation for gauge metadata, named parts, tick/rivet counts, and material roles.
- [x] Add asset-promotion review and pressure-gauge prototype production docs.
- [x] Refresh `V0RouteAudit` output for `v0.1.11`.
- [x] Build `v0.1.11` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.12 Windows Distribution Polish

- [x] Add `Tools/PackageWindowsBuild.ps1`.
- [x] Verify required Windows build artifacts before packaging.
- [x] Generate `README_WINDOWS.txt` in the staged package folder.
- [x] Generate a Windows package manifest and SHA-256 hash.
- [x] Create `Builds/WindowsPackages/v0.1.12/BrassworksBreach_v0.1.12_Windows.zip`.
- [x] Wire the package step into `Tools/RunV0BuildMatrix.ps1`.
- [x] Add `Documentation/Releases/RELEASE_NOTES_v0.1.12.md`.
- [x] Refresh `V0RouteAudit` output for `v0.1.12`.
- [x] Build and package `v0.1.12` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.13 Windows Route QA Packet Automation

- [x] Add `Tools/GenerateWindowsQAPacket.ps1`.
- [x] Generate a route-QA Markdown packet and JSON manifest from the current build, route audit, package manifest, and manual route sheets.
- [x] Refresh `Documentation/QA/ManualPlaytestV1/README.md` with the exact current Windows executable and generated QA packet path.
- [x] Wire the QA packet step into `Tools/RunV0BuildMatrix.ps1`.
- [x] Generate `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.13.md`.
- [x] Generate `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.13.json`.
- [x] Refresh `V0RouteAudit` output for `v0.1.13`.
- [x] Build, package, and generate route-QA evidence for `v0.1.13` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.14 Pressure Coil Asset Promotion

- [x] Add `PressureCoilPrototype` runtime metadata for promoted coil-pack components.
- [x] Add a named Pressure Pistol copper coil pack to the generated viewmodel.
- [x] Include blackened iron backing, aged brass rails, copper manifolds, red heat core, oxidized coil turns, rivets, pressure leads, and patina marks.
- [x] Add validator coverage for coil metadata, named parts, material roles, and coil/rivet/lead detail counts.
- [x] Add pressure-coil prototype production docs.
- [x] Refresh `V0RouteAudit` output for `v0.1.14`.
- [x] Build, package, and generate route-QA evidence for `v0.1.14` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.15 Windows Candidate Readiness Automation

- [x] Add `Tools/GenerateWindowsCandidateReadiness.ps1`.
- [x] Verify the current Windows executable, package ZIP, package manifest, route audit, QA packet, QA manifest, and release notes.
- [x] Verify required smoke-log markers for the complete Windows matrix.
- [x] Generate `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.15.md`.
- [x] Generate `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.15.json`.
- [x] Wire candidate-readiness generation into `Tools/RunV0BuildMatrix.ps1`.
- [x] Add `V0_WINDOWS_CANDIDATE_PASS` to the full matrix.
- [x] Refresh `V0RouteAudit` next-action guidance for the post-candidate-readiness sequence.
- [x] Build, package, generate route-QA evidence, and generate candidate-readiness evidence for `v0.1.15` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.16 Windows Issue Triage Packet Automation

- [x] Add `Tools/GenerateWindowsIssueTriagePacket.ps1`.
- [x] Generate a Markdown issue-triage packet with severity rules, issue buckets, an intake template, and candidate-gate rules.
- [x] Generate `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.16.md`.
- [x] Generate `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.16.json`.
- [x] Refresh `Documentation/QA/ManualPlaytestV1/README.md` with the exact current issue-triage packet path.
- [x] Wire issue-triage packet generation into `Tools/RunV0BuildMatrix.ps1`.
- [x] Add `V0_WINDOWS_ISSUE_TRIAGE_PASS` to the full matrix.
- [x] Update candidate-readiness automation so candidate evidence requires and links the issue-triage packet.
- [x] Refresh `V0RouteAudit` output for `v0.1.16`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.16` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.17 Wall Pipe Gauge Cluster Asset Promotion

- [x] Add `WallPipeGaugeClusterPrototype` runtime metadata for promoted wall-mounted environment components.
- [x] Add a named Pipeworks Annex wall pipe/gauge/valve cluster placement.
- [x] Add a named Boilerheart Core wall pipe/gauge/valve cluster placement.
- [x] Build the component from Unity-owned geometry: iron backplate, aged brass rails, copper/iron pipe runs, two cream enamel gauges, a red valve wheel, and rivets.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, and pipe/gauge/valve/rivet counts.
- [x] Add production brief and status files under `Documentation/AssetProduction/WallPipeGaugeClusterPrototype/`.
- [x] Refresh `V0RouteAudit` output for `v0.1.17`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.17` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.18 Windows Distribution Polish

- [x] Add `LAUNCH_BRASSWORKS_BREACH.bat` to the staged Windows package.
- [x] Add `QUICKSTART_WINDOWS.txt` with extract, launch, controls, quit, and first-run notes.
- [x] Add `SUPPORT_INFO_WINDOWS.txt` with issue-reporting fields and candidate scope notes.
- [x] Expand `README_WINDOWS.txt` with launcher, direct EXE launch, folder integrity, and quit/close guidance.
- [x] Add launcher, README, quickstart, and support-info paths to the package manifest.
- [x] Expand candidate-readiness automation to require those files and verify them inside the ZIP.
- [x] Refresh `V0RouteAudit` output for `v0.1.18`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.18` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.19 Boiler Control Console Asset Promotion

- [x] Confirm no real manual route-triage notes exist yet; use the roadmap fallback and proceed to the next modular asset promotion.
- [x] Add `BoilerControlConsolePrototype` runtime metadata for promoted console components.
- [x] Add a named Pipeworks Annex boiler-control console placement.
- [x] Add a named Boilerheart Core boiler-control console placement.
- [x] Build the component from Unity-owned geometry: iron pedestal base, angled control panel, brass rails/cheeks, lever bank, two cream enamel gauges, three indicator lamps, side valve wheel, pressure pipes, and rivets.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, detail counts, and non-blocking collider footprint.
- [x] Add production brief and status files under `Documentation/AssetProduction/BoilerControlConsolePrototype/`.
- [x] Refresh `V0RouteAudit` output for `v0.1.19`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.19` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.20 Riveted Pressure Door Frame Asset Promotion

- [x] Confirm no real manual route-triage notes exist yet; use the roadmap fallback and proceed to the next modular asset promotion.
- [x] Add `RivetedPressureDoorFramePrototype` runtime metadata for promoted route-threshold components.
- [x] Add a named Pipeworks Annex riveted pressure door frame placement near the Boilerheart service lift route.
- [x] Add a named Boilerheart Core riveted pressure door frame placement near the Foundry service lift route.
- [x] Build the component from Unity-owned geometry: blackened iron columns and lintel, aged brass ribs, pressure cylinders, animated central gear hub, cross braces, amber warning lamps, cream pressure gauge, and twenty visible rivets.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, detail counts, and non-blocking collider footprint.
- [x] Add production brief and status files under `Documentation/AssetProduction/RivetedPressureDoorFramePrototype/`.
- [x] Refresh `V0RouteAudit` output for `v0.1.20`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.20` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.21 Caged Gaslight Asset Promotion

- [x] Confirm no real manual route-triage notes exist yet; use the roadmap fallback and proceed to the next modular asset promotion.
- [x] Add `CagedGaslightPrototype` runtime metadata for promoted route-lighting components.
- [x] Add named Pipeworks Annex, Boilerheart Core, Furnace Foundry, and Governor Core caged gaslight placements.
- [x] Build the component from Unity-owned geometry: soot-dark backplate, blackened iron bracket, pipe feed, aged brass valve detail, brass caps, amber glass globe, cage ribs, rivets, warm light core, and warm point light.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, detail counts, no-collider route safety, and warm point-light settings.
- [x] Add production brief and status files under `Documentation/AssetProduction/CagedGaslightPrototype/`.
- [x] Refresh `V0RouteAudit` output for `v0.1.21`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.21` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.22 Pipe Canopy Asset Promotion

- [x] Confirm no real manual route-triage notes exist yet; use the roadmap fallback and proceed to the next modular asset promotion.
- [x] Add `PipeCanopyPrototype` runtime metadata for promoted overhead route-dressing components.
- [x] Add named Intake, Pipeworks Annex, Boilerheart Core, Furnace Foundry, and Governor Core pipe canopy placements.
- [x] Build the component from Unity-owned geometry: four aged brass pipes, blackened iron collars, visible collar rivets, aged brass couplers, and valve/pressure detail.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, detail counts, and no-collider route safety.
- [x] Add production brief and status files under `Documentation/AssetProduction/PipeCanopyPrototype/`.
- [x] Refresh `V0RouteAudit` output for `v0.1.22`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.22` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.23 Rivet Band Asset Promotion

- [x] Confirm no real manual route-triage notes exist yet; use the roadmap fallback and proceed to the next modular asset promotion.
- [x] Add `RivetBandPrototype` runtime metadata for promoted flush route-dressing components.
- [x] Add named Brassworks Intake, Pipeworks Annex, and Boilerheart Core rivet band placements.
- [x] Build the component from Unity-owned geometry: blackened iron backing rail, aged brass face rib, aged brass end caps, repeated brass rivets, pressure tag plate, and scribe mark detail.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, detail counts, and no-collider route safety.
- [x] Add production brief and status files under `Documentation/AssetProduction/RivetBandPrototype/`.
- [x] Refresh `V0RouteAudit` output for `v0.1.23`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.23` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.24 Wall Valve Wheel Asset Promotion

- [x] Confirm no real manual route-triage notes exist yet; use the roadmap fallback and proceed to the next modular asset promotion.
- [x] Add `WallValveWheelPrototype` runtime metadata for promoted route-safe wall valve wheel dressing.
- [x] Add named Brassworks Intake, Pipeworks Annex, and Boilerheart Core wall valve wheel placements.
- [x] Build the component from Unity-owned geometry: blackened iron wall backplate, aged brass lower rail, aged brass valve wheel, blackened iron spokes, aged brass spindle hub, eight brass mount rivets, cream pressure label, and amber pointer mark.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, detail counts, and no-collider route safety.
- [x] Add production brief and status files under `Documentation/AssetProduction/WallValveWheelPrototype/`.
- [x] Refresh `V0RouteAudit` output for `v0.1.24`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.24` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.25 Pressure Relief Vent Asset Promotion

- [x] Confirm no real manual route-triage notes exist yet; use the roadmap fallback and proceed to the next modular asset promotion.
- [x] Add `PressureReliefVentPrototype` runtime metadata for promoted route-safe pressure-relief vent dressing.
- [x] Add named Brassworks Intake, Pipeworks Annex, and Furnace Foundry pressure-relief vent placements.
- [x] Build the component from Unity-owned geometry: blackened iron mount plate, aged brass saddle, aged brass relief vent stack, blackened iron louvers, small brass relief pipe/cap, eight brass vent bolts, amber pressure tag/pointer, and pale ambient steam puffs.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, detail counts, and no-collider route safety.
- [x] Add production brief and status files under `Documentation/AssetProduction/PressureReliefVentPrototype/`.
- [x] Refresh `V0RouteAudit` output for `v0.1.25`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.25` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.26 Catwalk Rail Asset Promotion

- [x] Confirm no real manual route-triage notes exist yet; use the roadmap fallback and proceed to the next modular asset promotion.
- [x] Add `CatwalkRailPrototype` runtime metadata for promoted route-safe catwalk/service rail dressing.
- [x] Add named Pipeworks Annex, Boilerheart Core, and Furnace Foundry catwalk/service rail placements.
- [x] Build the component from Unity-owned geometry: aged brass upper rail, blackened iron lower rail, blackened iron uprights, aged brass post caps, blackened iron bolted feet, and brass foot rivets.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, detail counts, and no-collider route safety.
- [x] Add production brief and status files under `Documentation/AssetProduction/CatwalkRailPrototype/`.
- [x] Refresh `V0RouteAudit` output for `v0.1.26`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.26` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.27 Floor Drain Grate Asset Promotion

- [x] Confirm no real manual route-triage notes exist yet; use the roadmap fallback and proceed to the next modular asset promotion.
- [x] Add `FloorDrainGratePrototype` runtime metadata for promoted route-safe floor drain grate dressing.
- [x] Add named Brassworks Intake, Pipeworks Annex, and Furnace Foundry floor drain grate placements.
- [x] Build the component from Unity-owned geometry: blackened iron drain frame, aged brass drain trim, slotted blackened iron grate bars, brass bolts, oil-dark stone stain plates, and pale ambient steam seep puffs.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, detail counts, and no-collider route safety.
- [x] Add production brief and status files under `Documentation/AssetProduction/FloorDrainGratePrototype/`.
- [x] Refresh `V0RouteAudit` output for `v0.1.27`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.27` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.28 Pressure Tank Rack Asset Promotion

- [x] Confirm no real manual route-triage notes exist yet; use the roadmap fallback and proceed to the next modular asset promotion.
- [x] Add `PressureTankRackPrototype` runtime metadata for promoted route-safe pressure tank rack dressing.
- [x] Add named Brassworks Intake, Pipeworks Annex, and Governor Core pressure tank rack placements.
- [x] Build the component from Unity-owned geometry: blackened iron rack rails/uprights, three dark pressure tanks, aged brass tank bands, feeder pipes, valve caps, twelve brass rack bolts, an amber pressure tag, cream pressure gauge, gauge needle, and pale ambient steam seep puffs.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, detail counts, and no-collider route safety.
- [x] Add production brief and status files under `Documentation/AssetProduction/PressureTankRackPrototype/`.
- [x] Refresh `V0RouteAudit` output for `v0.1.28`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.28` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.29 Windows Distribution Hardening

- [x] Scan manual QA sheets and issue-triage packets for accepted P0/P1 route notes; no accepted route issue was present.
- [x] Use the no-issue fallback from `Documentation/Planning/V0_1_29_RouteTriage/`.
- [x] Add `RELEASE_INDEX_WINDOWS.txt` to the Windows package folder and ZIP.
- [x] Add `VERIFY_SHA256_WINDOWS.txt` to the Windows package folder and ZIP.
- [x] Add package-manifest fields for release index, checksum instructions, and SHA-256 sidecar path.
- [x] Expand candidate-readiness automation to require the release index and checksum instructions on disk and inside the ZIP.
- [x] Refresh `V0RouteAudit` output for `v0.1.29`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.29` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.30 Service Lift Call Box Asset Promotion

- [x] Add `ServiceLiftCallBoxPrototype` runtime metadata for promoted route-safe service-lift/hoist call box dressing.
- [x] Add named call box placements beside all current level transitions.
- [x] Build the component from Unity-owned geometry: blackened iron backplate, aged brass pull lever, lever guard, cream lift-pressure gauge, amber/red/green lamp language, brass pressure pipes, stamped label plate, rivets, oil streaks, and scorch grime.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, detail counts, zero colliders, zero `NavMeshObstacle` components, and no gameplay-authority components.
- [x] Add production brief and status files under `Documentation/AssetProduction/ServiceLiftCallBoxPrototype/`.
- [x] Refresh `V0RouteAudit` output for `v0.1.30`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.30` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.31 Gear Key Plinth Asset Promotion

- [x] Add `GearKeyPlinthPrototype` runtime metadata for promoted route-safe gear-key presentation dressing.
- [x] Add the named `intake_gear_key_plinth` placement around the existing Level01 gear-key pickup.
- [x] Build the component from Unity-owned geometry: blackened iron pedestal, aged brass gear cradle, brass gear teeth, cream enamel gauge/label, amber ready lamp, brass trim, rivets, oil streaks, soot, and worn-edge highlights.
- [x] Preserve the existing GearKey pickup as the only gameplay authority and keep it reachable.
- [x] Add validator coverage for promotion version, placement role, required named hierarchy, material roles, detail counts, zero colliders, zero `NavMeshObstacle` components, no gameplay-authority components, and pickup reachability.
- [x] Add production brief and status files under `Documentation/AssetProduction/GearKeyPlinthPrototype/`.
- [x] Add a docs-only `ValveWheelConsolePrototype` side-agent brief for the next likely route-safe prop slice.
- [x] Refresh `V0RouteAudit` output for `v0.1.31`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.31` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.32 Valve Wheel Console Asset Promotion

- [x] Add `ValveWheelConsolePrototype` runtime metadata for promoted route-safe pressure-console dressing.
- [x] Add named `pipeworks_pressure_console` and `boilerheart_pressure_console` placements.
- [x] Build the component from Unity-owned geometry: blackened iron backplates, raised panels, aged brass valve wheel rings, blackened iron spokes, brass grips/hubs, cream pressure gauges, dark needles, amber pilot lamps, pressure pipes, brass rivets, oil grime, soot, and worn highlights.
- [x] Add validator coverage for promotion version, placement roles, required named hierarchy, material roles, detail counts, zero colliders, zero `NavMeshObstacle` components, and no gameplay-authority components.
- [x] Mark `Documentation/AssetProduction/ValveWheelConsolePrototype/status.json` verified.
- [x] Add `Documentation/ProductionManagement/BATCH_PRODUCTION_MODE.md`.
- [x] Update the continuation automation to run a 30-minute PM review and enforce ambitious batching.
- [x] Spawn parallel crews for route-dressing assets, level-density placement, weapon/prop staging, and enemy/readability staging.
- [x] Refresh `V0RouteAudit` output for `v0.1.32`.
- [x] Build, package, generate route-QA evidence, generate issue-triage evidence, and generate candidate-readiness evidence for `v0.1.32` through the full V0 matrix.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.33 Visible Threshold And Route Dressing Milestone

- [x] Integrate the 10-family threshold/route dressing plan from `Documentation/Planning/V0_1_33_BatchPlan/`.
- [x] Implement `PistonDoorBracePrototype` across major gate/lift/hoist thresholds where clearance allows.
- [x] Implement `PipeClampCouplerSetPrototype` grouped placements across existing route pipework.
- [x] Implement `OilSootGrimePanelSetPrototype` grouped machinery/grime placements across all levels.
- [x] Implement `AmberIndicatorPlatePrototype` grouped route-attention plates across all levels.
- [x] Implement `BrassThresholdKickPlatePrototype` at repeated gate/lift/hoist aprons.
- [x] Implement `RivetedPatchRepairPlatePrototype` on route-adjacent walls and service panels.
- [x] Implement at least two optional families from pressure gasket rings, route return pipe markers, steam vent residue collars, and hoist chain anchor plates.
- [x] Touch at least four levels, with Level01 through Level05 preferred.
- [x] Use targeted compile, scene rebuild, level validation, and route smoke during development.
- [x] Run route audit and full V0 matrix only when the whole milestone is coherent.

Completed in `v0.1.33`: 10 route-dressing families, 50 placements, all five levels, route audit, package, QA packet, issue triage, and candidate readiness.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.34 Playable Polish Leap After Route Dressing

- [x] Integrate the `v0.1.34` weapon/prop polish staging packet into generated weapon and pickup visuals where safe.
- [x] Integrate the `v0.1.34` enemy readability staging packet into generated Scrapper, Lancer, Bulwark, and Warden visuals where safe.
- [x] Add `V0134BatchPolishPrototype` runtime metadata for batch-level visual-only polish.
- [x] Add validator coverage for version, batch ID, category, target ID, placement role, required visual parts, zero colliders, zero `NavMeshObstacle` components, and no gameplay-authority components.
- [x] Use targeted scene rebuild, level validation, editor smoke, Windows build, and packaged runtime smoke during development.
- [x] Run route audit, package, QA packet, issue triage, and candidate readiness after the whole milestone is coherent.
- [x] Keep the next side-agent wave assigned as parallel bundles rather than sequential single assets.

Completed in `v0.1.34`: visual-only polish for the Pressure Pistol, Steam Scattergun, pressure cartridges, scattergun pickup display, and four enemy readability silhouettes; side-agent packets for weapon/prop polish, enemy readability, level-density planning, and batch validation; route audit, package, QA packet, issue triage, and candidate readiness.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.1.35 Gameplay Feedback And Asset Bundle Prep

- [x] Stage a weapon/gameplay prop arsenal bundle with pressure pistol, scattergun, ammo, display, cabinet, and future weapon silhouettes.
- [x] Stage a mechanical enemy pack bundle with Scrapper, Lancer, Bulwark, Warden, and Foundry Overseer visual candidates.
- [x] Stage a level module/setpiece bundle with corridor, door/vault, pipe gallery, furnace/catwalk, trim, lighting, and all-level placement plans.
- [x] Stage a UI/audio/VFX feedback bundle with cue IDs, icons, placeholder audio, material recipes, and preview sheets.
- [x] Add performance/import/LOD readiness rules for staged bundle promotion.
- [x] Add gameplay systems and batch-validation planning for the next main-lane implementation leap.
- [ ] Implement the main-lane gameplay feedback systems batch.
- [ ] Run targeted `v045` development smokes, then route audit and full V0 matrix at milestone completion.

Next-step directive: continue immediately with the next highest-impact unfinished task.

## v0.2 Combat Feel Slice

- [ ] Run manual Windows playthrough.
- [ ] Record tuning notes in `WORK_LEDGER.md`.
- [x] Add first collision-cover pass to `Brassworks Intake`.
- [x] Add initial automated movement/combat balance profile.
- [x] Tune player movement speed and camera feel.
- [x] Tune `Pressure Pistol` damage, fire rate, ammo, and feedback.
- [x] Tune `Scrapper` speed, detection range, damage, and attack cooldown.
- [x] Confirm `Brassworks Intake` route object placement and room flow against `LEVEL_DESIGN_AND_MAPS.md` through deterministic smoke coverage.
- [x] Confirm Pipeworks and Boilerheart route object placement through deterministic midgame smoke coverage.
- [x] Confirm Foundry and Governor Core route object placement through deterministic climax smoke coverage.
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
- [x] Promote service-lift call box visual dressing across all current lifts/hoists.
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
