# Unity Doom Experiment - Detailed Implementation To-Do

This document breaks the project into small sequential chunks. Each chunk should leave the project in a testable state before moving on.

Status legend:

- `[ ]` Not started
- `[/]` In progress
- `[x]` Complete
- `[!]` Blocked or needs review

## Phase 0 - Review and Setup Decisions

Goal: Confirm the intended shape of the project before creating or modifying Unity assets.

- [x] Create `PROJECT_SPEC.md`.
- [x] Create `CREATIVE_DIRECTION.md`.
- [x] Create `V0_SCOPE.md`.
- [x] Answer or defer the open questions:
  - [x] v0.0 scope: plain greybox proof of concept.
  - [x] Enemy type: melee chaser; v0.0 visual is a capsule/cube, later visual is `Ash Imp`.
  - [x] Weapon visual: no required weapon art for v0.0, later centered 2D sprite `Iron Pistol`.
  - [x] Level theme: plain dungeon crawler blockout for v0.0, later mixed industrial/gothic.
  - [x] Main menu: skipped for MVP; launch straight into `Level01`.
  - [x] Jumping: omitted for MVP; movement stays flat and fast.
- [ ] Detect installed Unity version and available templates.
- [ ] Decide render pipeline:
  - [x] Default decision: built-in render pipeline if easiest/available.
  - [ ] Fallback: URP if the installed Unity setup makes that smoother.
- [ ] Decide input approach:
  - [x] Default decision: classic input for fastest MVP.
  - [ ] Fallback: New Input System if project template already supports it.
- [ ] Confirm final project root:
  - [ ] `D:\__MY APPS\Unity Doom`

Review checkpoint:

- [ ] User reviews spec, creative direction, and to-do list before Unity implementation begins.

## v0.0 Implementation Track - Proof of Concept

Goal: Build the smallest playable greybox loop before doing any skins, custom graphics, audio, or polish.

This is the active first-iteration checklist. The longer phase list below remains useful as the future `v0.1+` backlog.

### v0.0-A Project Bootstrap

- [ ] Detect installed Unity editor path/version.
- [ ] Create or initialize Unity project at `D:\__MY APPS\Unity Doom`.
- [ ] Confirm `Assets`, `ProjectSettings`, and `Packages` exist.
- [ ] Create minimal `_Project` folders:
  - [ ] `Assets/_Project/Scenes`
  - [ ] `Assets/_Project/Scripts`
  - [ ] `Assets/_Project/Prefabs`
  - [ ] `Assets/_Project/Materials`
- [ ] Create `Level01.unity`.
- [ ] Add `Level01` to build settings.

Checkpoint:

- [ ] Project opens without compile errors.
- [ ] Empty `Level01` enters play mode.

### v0.0-B Greybox Dungeon

- [ ] Create floor from primitive cube/plane.
- [ ] Create walls from primitive cubes.
- [ ] Create a simple layout:
  - [ ] Start room.
  - [ ] Corridor.
  - [ ] First enemy room.
  - [ ] Key room or alcove.
  - [ ] Locked door.
  - [ ] Exit room.
- [ ] Use simple flat colors:
  - [ ] Gray walls.
  - [ ] Dark floor.
  - [ ] Red locked door.
  - [ ] Yellow or red key marker.
  - [ ] Green exit marker.
  - [ ] Red/orange enemy marker.

Checkpoint:

- [ ] Level is navigable as a plain blockout.
- [ ] Player path from start to exit is understandable.

### v0.0-C Player Movement

- [ ] Create player prefab with CharacterController and camera.
- [ ] Add WASD movement.
- [ ] Add mouse look.
- [ ] Lock cursor during gameplay.
- [ ] Omit jump/crouch.
- [ ] Tune speed for fast dungeon movement.

Checkpoint:

- [ ] Player can move and look around.
- [ ] Player collides with walls.
- [ ] Player cannot escape the blockout.

### v0.0-D Minimal Game State and HUD

- [ ] Add player health.
- [ ] Add ammo count.
- [ ] Add key possession flag.
- [ ] Add plain text HUD:
  - [ ] Health.
  - [ ] Ammo.
  - [ ] Key yes/no.
  - [ ] Crosshair.
- [ ] Add death state.
- [ ] Add win state.
- [ ] Add restart key after death/win.

Checkpoint:

- [ ] HUD reflects player state.
- [ ] Death and win messages can appear.
- [ ] Restart reloads the scene.

### v0.0-E Simple Hitscan Weapon

- [ ] Left mouse fires.
- [ ] Raycast from center of camera.
- [ ] Consume ammo.
- [ ] Enforce fire cooldown.
- [ ] Damage objects that can be damaged.
- [ ] Use minimal feedback:
  - [ ] Crosshair remains visible.
  - [ ] Optional tiny hit marker primitive.
  - [ ] Optional debug log during development.

Checkpoint:

- [ ] Shooting decreases ammo.
- [ ] Shooting hits test damageable object.
- [ ] Cannot fire forever if ammo reaches zero.

### v0.0-F Primitive Enemy

- [ ] Create primitive enemy prefab using capsule or cube.
- [ ] Add health.
- [ ] Add simple detection by distance.
- [ ] Move toward player.
- [ ] Stop near player.
- [ ] Damage player on cooldown.
- [ ] Die/disappear when health reaches zero.

Checkpoint:

- [ ] Enemy chases the player.
- [ ] Enemy can hurt the player.
- [ ] Player can kill the enemy.

### v0.0-G Key, Door, Exit

- [ ] Create key pickup using colored cube/marker.
- [ ] Key sets inventory flag and disappears.
- [ ] Create red locked door.
- [ ] Door blocks progress before key.
- [ ] Door opens or disappears after key.
- [ ] Create green exit trigger.
- [ ] Exit trigger shows win state.

Checkpoint:

- [ ] Player cannot complete level without key.
- [ ] Player can collect key, open door, and win.

### v0.0-H End-to-End Playtest

- [ ] Play from start to win.
- [ ] Play until death, then restart.
- [ ] Verify enemy, key, door, exit, and HUD all work together.
- [ ] Tune obvious pain points only:
  - [ ] Movement speed.
  - [ ] Enemy speed.
  - [ ] Enemy damage.
  - [ ] Player health.
  - [ ] Ammo amount.

Checkpoint:

- [ ] v0.0 is a complete playable proof of concept in editor.

### v0.0-I Windows Build Smoke Test

- [ ] Configure Windows standalone target.
- [ ] Build to `D:\__MY APPS\Unity Doom\Builds\Windows`.
- [ ] Launch executable.
- [ ] Confirm input, cursor lock, shooting, restart, and win state.

Checkpoint:

- [ ] v0.0 can run outside the Unity editor, if build tooling is available.

## v0.1+ Backlog Note

The phase list below was written for the fuller Doom-like prototype. During v0.0, skip asset generation, sprite work, custom audio, advanced UI, menu work, and visual polish unless they are needed to debug the proof of concept.

## Phase 1 - Unity Project Creation and Baseline Structure

Goal: Create or initialize the Unity project and establish clean folders/settings.

### 1.1 Create/Open Project

- [ ] Check whether `D:\__MY APPS\Unity Doom` is already a Unity project.
- [ ] If not, create a new Unity project in that folder.
- [ ] Open project once in Unity or verify project files exist.
- [ ] Confirm `Assets`, `ProjectSettings`, and `Packages` are present.
- [ ] Make sure no unrelated files are overwritten.

### 1.2 Configure Project Basics

- [ ] Set target platform to Windows standalone.
- [ ] Set default orientation/display settings for desktop.
- [ ] Configure resolution/window mode defaults if available.
- [ ] Set company/product name.
- [ ] Confirm active render pipeline.
- [ ] Confirm physics layer defaults are usable.

### 1.3 Create Folder Structure

- [ ] Create `Assets/_Project`.
- [ ] Create `Assets/_Project/Scenes`.
- [ ] Create `Assets/_Project/Scripts`.
- [ ] Create `Assets/_Project/Scripts/Player`.
- [ ] Create `Assets/_Project/Scripts/Weapons`.
- [ ] Create `Assets/_Project/Scripts/Enemies`.
- [ ] Create `Assets/_Project/Scripts/World`.
- [ ] Create `Assets/_Project/Scripts/UI`.
- [ ] Create `Assets/_Project/Prefabs`.
- [ ] Create `Assets/_Project/Prefabs/Player`.
- [ ] Create `Assets/_Project/Prefabs/Weapons`.
- [ ] Create `Assets/_Project/Prefabs/Enemies`.
- [ ] Create `Assets/_Project/Prefabs/Pickups`.
- [ ] Create `Assets/_Project/Prefabs/World`.
- [ ] Create `Assets/_Project/Materials`.
- [ ] Create `Assets/_Project/Textures`.
- [ ] Create `Assets/_Project/Sprites`.
- [ ] Create `Assets/_Project/Audio`.
- [ ] Create `Assets/_Project/ScriptableObjects`.
- [ ] Create `Assets/_Project/Editor` if generation/editor tooling is needed.

### 1.4 Create Baseline Scene

- [ ] Create `Level01.unity`.
- [ ] Add simple ground plane.
- [ ] Add directional or area lighting.
- [ ] Add temporary camera.
- [ ] Save scene to `Assets/_Project/Scenes/Level01.unity`.
- [ ] Add scene to build settings.

Test checkpoint:

- [ ] Project opens without compiler errors.
- [ ] `Level01` opens and plays in editor.
- [ ] Empty scene renders correctly.

## Phase 2 - Player Controller Foundation

Goal: Make the player move and look around in a simple test scene.

### 2.1 Player Prefab

- [ ] Create player root GameObject.
- [ ] Add CharacterController.
- [ ] Add child Camera.
- [ ] Set player height, radius, camera height, and starting position.
- [ ] Create `Player` prefab.

### 2.2 Movement Script

- [ ] Create `PlayerController.cs`.
- [ ] Implement WASD movement.
- [ ] Implement gravity.
- [ ] Implement optional sprint only if approved.
- [ ] Implement optional jump only if approved.
- [ ] Expose movement speed and mouse sensitivity in inspector.
- [ ] Lock cursor during gameplay.
- [ ] Unlock cursor when app focus is lost or paused later.

### 2.3 Mouse Look

- [ ] Implement horizontal body rotation.
- [ ] Implement vertical camera pitch.
- [ ] Clamp vertical look angle.
- [ ] Smooth only if needed; prefer immediate retro feel.

### 2.4 Basic Collision Test Area

- [ ] Add temporary walls.
- [ ] Verify player cannot pass through walls.
- [ ] Verify player can move through corridors.
- [ ] Verify camera height feels correct.

Test checkpoint:

- [ ] Play mode starts at player camera.
- [ ] WASD movement works.
- [ ] Mouse look works.
- [ ] Cursor locks during play.
- [ ] Player collides with simple walls.

## Phase 3 - Core Game State and UI Shell

Goal: Add the minimum UI and game-state plumbing before combat systems depend on it.

### 3.1 Game State Controller

- [ ] Create `GameStateController.cs`.
- [ ] Track gameplay, paused, dead, and won states.
- [ ] Handle Escape pause toggle.
- [ ] Handle restart input during death/win.
- [ ] Handle cursor lock/unlock based on state.
- [ ] Add scene reload behavior.

### 3.2 HUD Canvas

- [ ] Create screen-space canvas.
- [ ] Add health text/indicator.
- [ ] Add ammo text/indicator.
- [ ] Add key indicator.
- [ ] Add crosshair.
- [ ] Add damage flash image.
- [ ] Add pause panel.
- [ ] Add death panel.
- [ ] Add win panel.

### 3.3 HUD Controller

- [ ] Create `HUDController.cs`.
- [ ] Expose methods for health, ammo, key, damage flash, pause, death, and win updates.
- [ ] Connect HUD to GameStateController.
- [ ] Ensure UI scales cleanly at 16:9 resolutions.

Test checkpoint:

- [ ] HUD visible during play.
- [ ] Pause panel appears and disappears.
- [ ] Cursor behavior changes correctly on pause.
- [ ] Restart can reload scene from a test key path.

## Phase 4 - Player Health and Inventory

Goal: Give the player gameplay state that pickups, enemies, and weapons can modify.

### 4.1 Player Health

- [ ] Create `PlayerHealth.cs`.
- [ ] Add max health and current health.
- [ ] Implement damage method.
- [ ] Implement heal method.
- [ ] Trigger death state at zero health.
- [ ] Trigger HUD updates.
- [ ] Trigger damage flash when hurt.

### 4.2 Player Inventory

- [ ] Create `PlayerInventory.cs`.
- [ ] Track ammo.
- [ ] Track key possession.
- [ ] Add methods:
  - [ ] AddAmmo.
  - [ ] TryUseAmmo.
  - [ ] AddKey.
  - [ ] HasKey.
- [ ] Trigger HUD updates.
- [ ] Set initial ammo.

Test checkpoint:

- [ ] Inspector debug buttons or temporary test input can damage/heal player.
- [ ] Health UI updates.
- [ ] Ammo UI updates.
- [ ] Key indicator updates.
- [ ] Death state triggers correctly.

## Phase 5 - Weapon System

Goal: Let the player fire a basic hitscan weapon with ammo and impact feedback.

### 5.1 Weapon Controller

- [ ] Create `WeaponController.cs`.
- [ ] Bind left mouse button to fire.
- [ ] Enforce fire rate.
- [ ] Check ammo before firing.
- [ ] Notify inventory when ammo is consumed.
- [ ] Block firing while paused/dead/won.

### 5.2 Hitscan Damage

- [ ] Create `HitscanWeapon.cs` or keep logic in `WeaponController` if still simple.
- [ ] Raycast from camera center.
- [ ] Define weapon range.
- [ ] Define weapon damage.
- [ ] Detect objects implementing damage receiver.
- [ ] Spawn impact visual at hit point.

### 5.3 Weapon Visual

- [ ] Create temporary weapon object or sprite in front of camera.
- [ ] Add simple idle bob.
- [ ] Add brief recoil or fire animation.
- [ ] Add muzzle flash sprite/quad.
- [ ] Hide muzzle flash when not firing.

### 5.4 Impact Feedback

- [ ] Create impact prefab:
  - [ ] Small spark sprite/particle.
  - [ ] Short lifetime.
- [ ] Spawn impact on walls.
- [ ] Spawn enemy hurt feedback when hitting enemies later.

Test checkpoint:

- [ ] Left click fires.
- [ ] Ammo decreases.
- [ ] Cannot fire with zero ammo.
- [ ] Raycast hits test cubes.
- [ ] Impact effect appears.
- [ ] Weapon visual gives clear feedback.

## Phase 6 - Asset Generation Pass 1 (v0.1+ Deferred)

Goal: Create enough original prototype art assets to replace raw Unity primitives.

v0.0 note: skip this entire phase unless a tiny placeholder asset is faster than a primitive. Use flat colors and cubes/capsules first.

### 6.1 Texture Generation

- [ ] Create or generate `metal_panel.png`.
- [ ] Create or generate `stone_brick.png`.
- [ ] Create or generate `warning_trim.png`.
- [ ] Create or generate `concrete_floor.png`.
- [ ] Create or generate `metal_grate_floor.png`.
- [ ] Create or generate `dark_ceiling_panel.png`.
- [ ] Import textures into Unity.
- [ ] Set filter mode to Point or low-res-friendly settings.
- [ ] Set wrap mode to Repeat for tileable textures.

### 6.2 Materials

- [ ] Create wall material using metal panel texture.
- [ ] Create stone material.
- [ ] Create warning trim material.
- [ ] Create floor material.
- [ ] Create ceiling material.
- [ ] Tune color/brightness for readability.

### 6.3 Sprite/Effect Assets

- [ ] Create crosshair sprite.
- [ ] Create muzzle flash sprite.
- [ ] Create impact spark sprite.
- [ ] Create simple pickup sprites or texture atlases.
- [ ] Import sprites with correct texture settings.

Test checkpoint:

- [ ] Textures display in scene.
- [ ] Materials tile correctly.
- [ ] Sprites have transparent backgrounds where expected.
- [ ] No missing material/pink shader errors.

## Phase 7 - Damage Interfaces and Enemy Foundation

Goal: Add a test enemy that can take damage, chase, attack, and die.

### 7.1 Damage Contract

- [ ] Create `IDamageable.cs` or `Damageable.cs`.
- [ ] Standardize method for applying damage.
- [ ] Update weapon raycast to use the damage contract.

### 7.2 Enemy Prefab

- [ ] Create enemy root GameObject.
- [ ] Add collider.
- [ ] Add visual placeholder.
- [ ] Add NavMeshAgent or simple movement component.
- [ ] Create `EnemyBasic` prefab.

### 7.3 Enemy Health

- [ ] Create `EnemyHealth.cs`.
- [ ] Implement max health/current health.
- [ ] Implement damage reaction.
- [ ] Implement death behavior.
- [ ] Disable collider/AI on death.
- [ ] Destroy after delay or leave corpse visual.

### 7.4 Enemy AI

- [ ] Create `EnemyAI.cs`.
- [ ] Implement states:
  - [ ] Idle.
  - [ ] Chase.
  - [ ] Attack.
  - [ ] Dead.
- [ ] Detect player by distance.
- [ ] Optionally use line of sight.
- [ ] Move toward player.
- [ ] Stop within attack range.
- [ ] Respect game paused/dead/won states.

### 7.5 Enemy Attack

- [ ] Create `EnemyAttack.cs` or integrate into AI if simple.
- [ ] Apply damage to player at cooldown interval.
- [ ] Add attack windup or flash if quick to implement.
- [ ] Tune damage and cooldown.

Test checkpoint:

- [ ] Enemy stands idle when player is far away.
- [ ] Enemy chases when player gets close.
- [ ] Enemy damages player when close enough.
- [ ] Player can kill enemy with weapon.
- [ ] Enemy stops acting after death.

## Phase 8 - Enemy Visual Asset Pass (v0.1+ Deferred)

Goal: Replace the enemy placeholder with a readable retro visual.

v0.0 note: skip this phase. The first enemy can remain a colored capsule or cube.

### 8.1 Decide Enemy Visual Technique

- [ ] Use billboard sprite if approved.
- [ ] Use low-poly mesh if sprite approach is too slow.
- [ ] Keep scale and silhouette readable.

### 8.2 Billboard System

- [ ] Create `Billboard.cs`.
- [ ] Rotate sprite to face player camera.
- [ ] Lock rotation to Y axis if needed.
- [ ] Test at different distances and angles.

### 8.3 Enemy Sprite/Material

- [ ] Create enemy idle sprite.
- [ ] Create enemy hurt sprite or color flash.
- [ ] Create enemy death sprite or corpse visual.
- [ ] Import sprites with point filtering.
- [ ] Assign to enemy prefab.

### 8.4 Enemy Feedback

- [ ] Flash enemy when hit.
- [ ] Play hurt/death sound later when audio exists.
- [ ] Spawn small blood/spark effect if desired.

Test checkpoint:

- [ ] Enemy is visually distinct from walls/floors.
- [ ] Enemy faces player correctly.
- [ ] Enemy hurt/death state is readable.
- [ ] Sprite does not clip badly into floor.

## Phase 9 - Pickups and Interactables

Goal: Add health, ammo, key, doors, and exit flow.

### 9.1 Pickup Base

- [ ] Create `Pickup.cs`.
- [ ] Detect player via trigger.
- [ ] Apply pickup effect.
- [ ] Play feedback effect/sound later.
- [ ] Destroy or disable after collection.
- [ ] Add optional bob/spin visual.

### 9.2 Health Pickup

- [ ] Create health pickup prefab.
- [ ] Add health pack visual.
- [ ] Heal player by configured amount.
- [ ] Do not exceed max health.
- [ ] Add to test scene.

### 9.3 Ammo Pickup

- [ ] Create ammo pickup prefab.
- [ ] Add ammo box visual.
- [ ] Increase player ammo by configured amount.
- [ ] Add to test scene.

### 9.4 Key Pickup

- [ ] Create key pickup prefab.
- [ ] Add key card/skull key visual.
- [ ] Set player inventory key flag.
- [ ] Update HUD key indicator.
- [ ] Add to test scene.

### 9.5 Doors

- [ ] Create `Door.cs`.
- [ ] Add sliding or rotating open behavior.
- [ ] Add open speed.
- [ ] Add closed/open states.
- [ ] Trigger by interaction key or trigger zone.

### 9.6 Locked Door

- [ ] Create `LockedDoor.cs` or extend `Door`.
- [ ] Check player key before opening.
- [ ] Show locked feedback if player lacks key.
- [ ] Open after key collected.

### 9.7 Exit

- [ ] Create `ExitTrigger.cs`.
- [ ] Trigger win state on enter or interaction.
- [ ] Connect to GameStateController.
- [ ] Add visible exit marker.

Test checkpoint:

- [ ] Health pickup heals player.
- [ ] Ammo pickup increases ammo.
- [ ] Key pickup updates HUD.
- [ ] Locked door blocks player before key.
- [ ] Locked door opens after key.
- [ ] Exit triggers win state.

## Phase 10 - Level Blockout

Goal: Build the first complete level layout using simple geometry and modular pieces.

### 10.1 Blockout Geometry

- [ ] Create start room.
- [ ] Create first corridor.
- [ ] Create first enemy encounter.
- [ ] Create hub room.
- [ ] Create locked final door.
- [ ] Create side path to key.
- [ ] Create final combat room.
- [ ] Create exit area.

### 10.2 Navigation and Collision

- [ ] Ensure corridor widths are comfortable.
- [ ] Ensure doorways are passable.
- [ ] Ensure no wall gaps let player escape.
- [ ] Ensure enemy movement works in all combat spaces.
- [ ] Bake NavMesh if using NavMeshAgent.

### 10.3 Apply Materials

- [ ] Apply wall materials.
- [ ] Apply floor materials.
- [ ] Apply ceiling materials if ceilings are used.
- [ ] Add warning trims near doors/exit.
- [ ] Ensure texture scale is consistent.

### 10.4 Gameplay Placement

- [ ] Place player start.
- [ ] Place enemies.
- [ ] Place health pickups.
- [ ] Place ammo pickups.
- [ ] Place key pickup.
- [ ] Place locked door.
- [ ] Place exit trigger.

Test checkpoint:

- [ ] Full level can be traversed.
- [ ] No obvious stuck spots.
- [ ] Critical path is understandable.
- [ ] Enemy encounters are playable.
- [ ] Player can complete level from start to win.

## Phase 11 - Audio Pass (v0.1+ Deferred)

Goal: Add simple sound feedback for the core loop.

v0.0 note: skip this phase unless the core loop is already working and audio is trivial to add.

### 11.1 Generate or Create Audio Clips

- [ ] Weapon fire sound.
- [ ] Empty weapon click.
- [ ] Enemy hurt sound.
- [ ] Enemy death sound.
- [ ] Pickup sound.
- [ ] Door open sound.
- [ ] Player hurt sound.
- [ ] Win sound.
- [ ] Optional ambient loop.

### 11.2 Audio Integration

- [ ] Add AudioSource to weapon or central audio manager.
- [ ] Play weapon fire sound.
- [ ] Play empty click sound.
- [ ] Play enemy hurt/death sounds.
- [ ] Play pickup sounds.
- [ ] Play door sound.
- [ ] Play player hurt sound.
- [ ] Play win sound.
- [ ] Set reasonable default volumes.

Test checkpoint:

- [ ] Audio plays at expected moments.
- [ ] No sound is painfully loud.
- [ ] Repeated weapon fire does not clip badly.
- [ ] Spatial sounds are clear enough or intentionally 2D.

## Phase 12 - Game Feel and Balance Pass

Goal: Tune the prototype until it feels like a coherent small game.

### 12.1 Player Feel

- [ ] Tune movement speed.
- [ ] Tune mouse sensitivity.
- [ ] Tune FOV.
- [ ] Tune weapon bob.
- [ ] Tune recoil/muzzle flash duration.

### 12.2 Combat Feel

- [ ] Tune pistol damage.
- [ ] Tune fire rate.
- [ ] Tune enemy health.
- [ ] Tune enemy speed.
- [ ] Tune enemy attack range.
- [ ] Tune enemy damage/cooldown.
- [ ] Tune ammo placement.
- [ ] Tune health placement.

### 12.3 Readability

- [ ] Ensure pickups are visible.
- [ ] Ensure key route is understandable.
- [ ] Ensure locked door communicates requirement.
- [ ] Ensure exit is visually obvious.
- [ ] Ensure enemy silhouettes are visible against level backgrounds.

Test checkpoint:

- [ ] First full playthrough feels fair.
- [ ] Player can recover from minor mistakes.
- [ ] Ammo economy is not frustrating.
- [ ] Level has a clear beginning, middle, and end.

## Phase 13 - Menus, Restart, and Build Polish

Goal: Make the prototype usable outside the editor.

### 13.1 Pause/Death/Win UX

- [ ] Confirm pause menu text and buttons.
- [ ] Confirm death screen text and restart.
- [ ] Confirm win screen text and restart/quit.
- [ ] Add quit behavior for build.
- [ ] Ensure cursor unlocks on non-gameplay screens.

### 13.2 Optional Main Menu

- [ ] Create `MainMenu.unity` if approved.
- [ ] Add Play button.
- [ ] Add Quit button.
- [ ] Add title and simple background.
- [ ] Add to build settings before `Level01`.

### 13.3 Project Settings

- [ ] Confirm build scenes order.
- [ ] Confirm product name.
- [ ] Confirm icon if any.
- [ ] Confirm quality settings.
- [ ] Confirm input works after build.

Test checkpoint:

- [ ] App can be played without using Unity editor.
- [ ] Menus and restart work in a build-like flow.
- [ ] Quit works in standalone build.

## Phase 14 - Windows Build

Goal: Produce a working Windows build and verify it.

### 14.1 Build

- [ ] Create build output folder, likely `D:\__MY APPS\Unity Doom\Builds\Windows`.
- [ ] Build Windows x86_64 standalone.
- [ ] Capture build warnings/errors.
- [ ] Fix blocking build issues.

### 14.2 Build Verification

- [ ] Launch executable directly.
- [ ] Verify mouse lock.
- [ ] Verify movement.
- [ ] Verify shooting.
- [ ] Verify enemy behavior.
- [ ] Verify pickups.
- [ ] Verify locked door/key.
- [ ] Verify win flow.
- [ ] Verify death/restart flow.
- [ ] Verify pause/quit behavior.

Final MVP checkpoint:

- [ ] Windows build is playable from start to finish.
- [ ] Unity editor project remains clean and understandable.
- [ ] All prototype assets are stored in the project.
- [ ] Documentation is updated with any deviations from this plan.

## Phase 15 - Documentation and Handoff Notes

Goal: Leave the project understandable after the build works.

- [ ] Update `PROJECT_SPEC.md` if implementation choices changed.
- [ ] Add a simple project README if useful.
- [ ] Document controls.
- [ ] Document how to open the Unity project.
- [ ] Document how to build for Windows.
- [ ] Document known issues.
- [ ] Document suggested next enhancements.

## Component Grouping Summary

For v0.0, implementation should proceed in this order:

1. Project bootstrap
2. Greybox dungeon
3. Player movement
4. Minimal game state and HUD
5. Simple hitscan weapon
6. Primitive enemy
7. Key, door, and exit
8. End-to-end editor playtest
9. Windows build smoke test

For v0.1+, implementation can proceed in this order:

1. Project setup
2. Player movement
3. Game state and HUD shell
4. Health and inventory
5. Weapon system
6. First asset generation pass
7. Enemy behavior
8. Enemy visuals
9. Pickups, doors, and exit
10. Level blockout
11. Audio
12. Balance and game feel
13. Menu/build polish
14. Windows build
15. Documentation updates

## First Vertical Slice Definition

For v0.0, the first useful vertical slice should include:

- [ ] One room.
- [ ] Player movement and mouse look.
- [ ] One simple hitscan weapon with no required art.
- [ ] One primitive enemy.
- [ ] Health/ammo HUD.
- [ ] One key pickup.
- [ ] One exit trigger.

This slice proves the project can support the full loop before spending time on the complete level.

## Risk List

- [ ] Unity command line project creation may depend on installed editor paths.
- [ ] Render pipeline may affect material/shader setup.
- [ ] NavMesh setup may take extra time if package/tools are not available.
- [ ] Generated art may need multiple passes for readability.
- [ ] Input behavior may differ between editor and Windows build.
- [ ] Cursor lock behavior needs explicit testing in standalone build.
- [ ] Audio generation/import may require format tweaks.

## Decision Log

Use this section as the project evolves.

- 2026-05-22: Initial spec and implementation to-do created before Unity project work begins.
- 2026-05-22: Default design answers added. The MVP uses a melee `Ash Imp`, centered 2D `Iron Pistol`, mixed industrial/gothic `Iron Chapel` level, direct-to-level startup, flat no-jump movement, built-in render pipeline preference, and classic Unity input preference.
- 2026-05-22: v0.0 scope simplified to a plain greybox proof of concept. Custom skins, generated textures, sprites, audio, and polish are deferred until after the core loop works.
- 2026-05-22: v0.0 core loop implemented and verified in editor/build/runtime smoke tests. A small v0.1 presentation pass added a blocky weapon, muzzle flash, damage flash, bobbing pickups, sliding door, accent lights, and simple enemy eye markers.
