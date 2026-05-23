# Unity Doom Experiment - Project Specification

## 1. Project Intent

Build a very simple retro first-person shooter in Unity inspired by the feel of early 1990s shooters like the original Doom: fast movement, simple maze-like rooms, sprite-style enemies, chunky textures, health/ammo pickups, a basic weapon, and a clear level exit.

The first version should be a small greybox proof of concept for Windows using mouse and keyboard. The goal is not to clone Doom exactly, and v0.0 should not spend time on custom skins. The broader goal is to create a compact, understandable Unity project that can later grow into a more Doom-like prototype with scenes, scripts, prefabs, materials, textures, UI, and generated assets.

Working title: `Iron Chapel`

Default model: when in doubt, choose the simpler original-Doom-like answer: fast flat movement, readable rooms, centered weapon sprite, sprite/billboard enemies, key door progression, obvious pickups, and direct combat feedback.

## 1.1 Version 0.0 Proof-of-Concept Scope

The first implementation target is `v0.0`, a plain greybox proof of concept.

For v0.0, the project should avoid custom skins, generated art, sprite sheets, audio polish, and detailed environment dressing. The purpose is to prove that the core Unity workflow and gameplay loop work on this computer.

v0.0 should use:

- Primitive walls, floors, doors, pickups, enemies, and markers.
- Flat colors instead of textures.
- Plain text HUD instead of custom UI art.
- A minimal or invisible weapon representation.
- A small dungeon layout with simple rooms and corridors.
- One enemy type represented by a capsule or cube.
- One key, one locked door, and one exit.

The `Iron Chapel`, `Ash Imp`, `Iron Pistol`, textures, sprites, audio, and visual effects remain the intended creative direction for a later pass after v0.0 proves the loop.

## 2. Target Platform

- Platform: Windows desktop
- Engine: Unity installed on this computer, exact version to be detected when project creation begins
- Render pipeline: Prefer Unity built-in render pipeline or URP depending on installed Unity template availability
- Input: Mouse look plus keyboard movement
- Build type: Standalone Windows x86_64
- Performance target: Stable 60 FPS on a normal desktop machine
- Display target: 16:9 desktop, scalable to common resolutions

## 3. Design Pillars

1. Fast and readable
   - Player movement should feel immediate.
   - Enemy silhouettes, pickups, doors, and exits should be obvious at a glance.

2. Retro, not complex
   - v0.0 uses flat colors and primitive geometry.
   - Later passes can add low-resolution textures.
   - Flat/simple lighting.
   - Limited color palette with strong contrast.
   - Billboarding sprites for enemies and pickups where useful after the mechanics work.

3. Small complete loop
   - Start level.
   - Explore.
   - Fight enemies.
   - Collect health/ammo/key.
   - Open locked door.
   - Reach exit.
   - See win screen.

4. Easy to inspect and extend
   - Clear folder organization.
   - Simple scripts with focused responsibilities.
   - Prefabs for reusable gameplay objects.
   - One small level built from modular pieces.

## 4. Core Gameplay Summary

For v0.0, the player starts in a plain greybox dungeon with a simple hitscan attack. The level contains a few rooms connected by corridors. Primitive enemies idle until the player is close, then move toward the player and attack at close range. The player can shoot enemies, collect a key, unlock a door, and reach an exit trigger.

After v0.0 works, the same loop can be dressed as a small industrial/hellish maze with a pistol, pickups, sprites, textures, sounds, and stronger Doom-like presentation.

## 5. Feature Set

### 5.1 v0.0 Required Feature Set

- First-person movement and mouse look.
- Wall collision.
- Basic health and death.
- Basic hitscan shooting.
- Basic ammo tracking.
- One primitive enemy that chases, attacks, takes damage, and dies.
- Plain HUD showing health, ammo, and key state.
- Key pickup.
- Locked door.
- Exit trigger.
- Restart after death or win.
- One small greybox level.

### 5.2 Post-v0 MVP Feature Set

After v0.0 works, the following features can turn the proof of concept into a more Doom-like first prototype.

### 5.3 Player

- First-person controller
- Mouse look with configurable sensitivity
- WASD movement
- Sprint optional, but not required
- Collision against walls and props
- Health value with damage feedback
- Death state and restart option
- Simple interaction key for doors, switches, and pickups if needed

### 5.4 Weapon

- One starter weapon: pistol or hand cannon
- Hitscan firing
- Fire rate cooldown
- Ammo count
- Muzzle flash visual
- Simple shooting animation or weapon bob
- Hit impact visual on walls/enemies
- Damage application to enemies

For v0.0, the weapon can be invisible or represented by a simple UI/camera child object. The centered `Iron Pistol` sprite is deferred until after the loop is playable.

### 5.5 Enemies

- One base enemy type for MVP
- Sprite-like/billboard enemy visual
- First enemy: `Ash Imp`, a melee pressure creature
- Idle state
- Aggro when player is visible or nearby
- Move toward player using NavMesh or simple steering
- Attack state with close-range swipe damage
- Take damage and die
- Death visual or sprite swap
- Optional drop pickup after death

For v0.0, the enemy should be a primitive capsule or cube. The `Ash Imp` sprite is deferred until after the primitive enemy behavior works.

### 5.6 Level

- One playable level
- Modular wall/floor/ceiling pieces or simple geometry
- Start room
- Combat room
- Key pickup area
- Locked door
- Exit room
- Basic environmental hazards optional
- Lighting tuned for retro readability

For v0.0, the level should be a plain greybox dungeon crawler layout with flat colors and minimal geometry.

### 5.7 Doors and Interactables

- Basic door that opens when triggered or interacted with
- Locked door requiring key
- Exit trigger or switch
- Clear feedback when door is locked

### 5.8 Pickups

- Health pickup
- Ammo pickup
- Key pickup
- Pickup visual using generated sprites or simple meshes
- Pickup sound or UI feedback

For v0.0, pickups can be colored cubes or simple trigger volumes. The key should be a bright marker, likely yellow or red.

### 5.9 UI

- HUD showing health
- HUD showing ammo
- Key possession indicator
- Crosshair
- Damage feedback flash
- Pause menu
- Death screen with restart
- Win screen with restart or quit

For v0.0, use plain text and a simple crosshair. Custom HUD panels/icons are deferred.

### 5.10 Audio

- Deferred for v0.0 unless trivial.
- Later simple generated or placeholder sounds:
  - Weapon fire
  - Enemy hurt
  - Enemy death
  - Pickup
  - Door open
  - Player hurt
  - Win/exit cue
- Music is optional for MVP. If included, use a short looping retro-style synth ambience.

### 5.11 Asset Creation

For v0.0, do not create a full custom asset set. Use Unity primitives, flat colors, simple materials, and text UI.

After v0.0, required prototype assets should be created inside the Unity project. The assets can be simple, generated, procedural, or hand-authored using basic tools/scripts. The first styled prototype does not require polished art, but it should have a coherent visual direction.

Post-v0 asset targets:

- Wall texture set:
  - Metal panel
  - Stone/brick
  - Warning/tech trim
- Floor texture set:
  - Concrete
  - Grate or metal
- Ceiling texture:
  - Dark panel
- Enemy visual:
  - Billboard sprite sheet for the `Ash Imp`
- Weapon visual:
  - Centered pistol/hand cannon sprite
- Pickup visuals:
  - Health pack
  - Ammo box
  - Key card/skull key
- UI visuals:
  - Crosshair
  - Damage flash
  - Simple HUD panels/icons
- Effects:
  - Muzzle flash sprite
  - Hit spark or impact decal

## 6. Visual Direction

### 6.1 Style

- Retro industrial/gothic dungeon
- Chunky, high-contrast texture work
- Limited palette: charcoal, rust red, sickly green, hazard yellow, pale gray
- Simple shapes and sharp silhouettes
- Low texture resolution, likely 64x64 or 128x128
- Avoid realistic PBR detail for MVP
- Environment base: rusted metal tech structure
- Corruption layer: stone chapel/reliquary details and red infernal accents

### 6.1.1 Creative Descriptions

- The level is a sealed industrial shrine called `Iron Chapel`.
- The player starts in a cold intake cell and pushes through metal corridors into a corrupted chapel chamber.
- The key route should feel like the tech base has cracked open into an older stone reliquary.
- The red key and red locked door should be visually linked with matching red trims/lights.
- The final exit should glow sickly green so it reads differently from enemies, damage, and the key objective.
- The first enemy, the `Ash Imp`, is a red/orange melee creature with a jagged silhouette and ember-like eyes.
- The starter weapon, the `Iron Pistol`, is a chunky centered hand cannon with a short muzzle flash and quick recoil.

### 6.2 Camera and Presentation

- First-person camera
- Mild weapon bob
- Optional pixelated render effect if practical and not disruptive
- Field of view around 75 to 90 degrees
- No complex post-processing required

### 6.3 Lighting

- Mostly baked or static lights
- Strong room readability
- Optional colored accent lights near doors, key areas, and exit
- Avoid darkness that makes navigation frustrating

## 7. Controls

Default keyboard and mouse controls:

- W: Move forward
- A: Strafe left
- S: Move backward
- D: Strafe right
- Mouse: Look
- Left mouse button: Fire
- E: Interact
- R: Restart after death/win, optional
- Escape: Pause/unpause

Optional controls:

- Shift: Deferred; only add sprint if base movement tuning feels too slow
- Space: No action for MVP; jumping is intentionally omitted

## 8. Technical Architecture

### 8.1 Project Structure

Proposed Unity asset folders:

- `Assets/_Project/Scenes`
- `Assets/_Project/Scripts`
- `Assets/_Project/Scripts/Player`
- `Assets/_Project/Scripts/Weapons`
- `Assets/_Project/Scripts/Enemies`
- `Assets/_Project/Scripts/World`
- `Assets/_Project/Scripts/UI`
- `Assets/_Project/Prefabs`
- `Assets/_Project/Prefabs/Player`
- `Assets/_Project/Prefabs/Weapons`
- `Assets/_Project/Prefabs/Enemies`
- `Assets/_Project/Prefabs/Pickups`
- `Assets/_Project/Prefabs/World`
- `Assets/_Project/Materials`
- `Assets/_Project/Textures`
- `Assets/_Project/Sprites`
- `Assets/_Project/Audio`
- `Assets/_Project/ScriptableObjects`
- `Assets/_Project/Editor`

### 8.2 Scenes

- `Boot.unity`
  - Optional scene used to initialize game state and load the main level.
  - Can be skipped if MVP stays simple.

- `Level01.unity`
  - Main playable level.
  - Contains player start, enemies, pickups, locked door, and exit.

- `MainMenu.unity`
  - Optional for MVP.
  - If included, offers Play and Quit.

Minimum required scene for first playable prototype:

- `Level01.unity`

### 8.3 Main Script Responsibilities

Player scripts:

- `PlayerController`
  - Handles movement, mouse look, grounding/collision integration.

- `PlayerHealth`
  - Tracks health, damage, death, and health UI events.

- `PlayerInventory`
  - Tracks key possession and ammo.

- `PlayerInteractor`
  - Raycasts for doors/switches or handles trigger interactions.

Weapon scripts:

- `WeaponController`
  - Handles input, fire cooldown, ammo checks, weapon animation triggers.

- `HitscanWeapon`
  - Performs raycast, applies damage, spawns impact effect.

Enemy scripts:

- `EnemyHealth`
  - Tracks health and death.

- `EnemyAI`
  - Controls idle, chase, attack, and death transitions.

- `EnemyAttack`
  - Applies damage to player through melee or simple ranged behavior.

World scripts:

- `Door`
  - Opens/closes or slides when activated.

- `LockedDoor`
  - Requires a key before opening.

- `Pickup`
  - Base behavior for pickup collection.

- `ExitTrigger`
  - Ends level and shows win state.

UI scripts:

- `HUDController`
  - Displays health, ammo, and key status.

- `GameStateController`
  - Handles pause, restart, death, and win states.

Utility scripts:

- `Billboard`
  - Rotates sprites to face camera.

- `Damageable`
  - Interface or base class for objects that can take damage.

### 8.4 Data Model

Use simple serialized fields for MVP. ScriptableObjects may be introduced for:

- Weapon data
- Enemy data
- Pickup data

For the first version, ScriptableObjects are useful but not mandatory. If implementation speed matters, serialized MonoBehaviour fields are acceptable.

### 8.5 Physics and Navigation

Recommended MVP approach:

- Player uses CharacterController for simple FPS movement.
- Weapon uses Physics.Raycast for hitscan.
- Enemies use Unity NavMeshAgent if available and quick to set up.
- If NavMesh introduces setup friction, use simple direct movement with obstacle-aware level layout.
- Player movement stays mostly flat with no jump/crouch for MVP.

### 8.6 Input System

Preferred approach:

- Use classic `Input.GetAxis`, `Input.GetKey`, and `Input.GetMouseButton` for MVP simplicity.
- If the chosen Unity template already includes and configures the new Input System, it can be used instead, but only if it does not slow down the first playable slice.

### 8.7 Save/Progression

No save system required for MVP.

### 8.8 Build and Configuration

- Configure project for Windows standalone.
- Hide/lock cursor during gameplay.
- Unlock cursor during pause/death/win screens.
- Include `Level01` in build settings.
- Add simple quality settings appropriate for low-spec desktop playback.

## 9. Level Design Specification

### 9.1 Level Flow

1. Start room
   - Player learns movement and shooting safely.
   - Contains pistol and/or initial ammo if weapon is not automatic.

2. First corridor
   - One enemy visible at medium distance.
   - Health pickup after first encounter.

3. Hub room
   - Two exits:
     - Locked door to final room.
     - Side path to key.

4. Side path
   - One or two enemies.
   - Ammo pickup.
   - Key pickup at the end.

5. Return to hub
   - Locked door can now open.

6. Final room
   - Slightly larger combat space.
   - Two or three enemies.
   - Exit switch or portal.

7. Win state
   - Triggered by interacting with exit or entering portal.

### 9.1.1 Room Descriptions

1. Intake Cell
   - Small start room with concrete floor, dark metal walls, and one obvious exit.

2. Rust Throat
   - Narrow metal corridor with warning trim and a bend that hides the first fight.

3. Furnace Antechamber
   - Medium first combat room with one `Ash Imp` and a health pickup beyond it.

4. Red Lock Junction
   - Hub room with the red locked door and a visible side path toward the key.

5. Broken Reliquary
   - Stone-and-metal side route with one or two enemies, ammo, and the red key.

6. Iron Chapel
   - Final chamber with two or three enemies and a green exit portal or switch.

### 9.2 Level Metrics

Approximate starting metrics:

- Player height: 1.8 Unity units
- Corridor width: 3 to 4 Unity units
- Door width: 2 to 3 Unity units
- Room size: 8x8 to 14x14 Unity units
- Ceiling height: 3 to 4 Unity units
- Enemy attack range:
  - Melee: 1.5 to 2 Unity units
  - Ranged if used: 8 to 15 Unity units

### 9.3 Encounter Balance

Initial values to tune:

- Player health: 100
- Pistol damage: 20
- Pistol fire rate: 3 to 5 shots per second
- Starting ammo: 40
- Base enemy health: 40
- Base enemy damage: 10
- Health pickup amount: 25
- Ammo pickup amount: 15

## 10. Acceptance Criteria

v0.0 is considered complete when:

- The Unity project opens without errors.
- `Level01` can be played from the editor.
- The player can move, look around, and shoot.
- Enemies can detect, chase, attack, take damage, and die.
- Health, ammo, and key status are visible in the HUD.
- The key pickup works.
- A locked door requires a key.
- The player can reach an exit and trigger a win screen.
- The player can die and restart.
- The level is a simple greybox dungeon using primitive geometry.
- The project has a clear path to a Windows build.

Post-v0 MVP acceptance criteria:

- The project can produce a Windows build.
- Health and ammo pickups work.
- Basic pause flow works.
- Simple prototype visuals replace the raw primitives.
- Any generated assets needed for the styled prototype are stored inside the project.

## 11. Testing Strategy

### 11.1 Editor Testing

Test each component in isolation as it is built:

- Player movement in an empty test area
- Shooting and hit detection against dummy targets
- Enemy damage/death using test enemy prefab
- Enemy chase/attack behavior in a small arena
- Pickups with player inventory and UI
- Door and locked door behavior
- Exit trigger and game state transitions

### 11.2 Playthrough Testing

Run full playthroughs after level integration:

- Start to exit without dying
- Death and restart
- Attempt locked door before key
- Collect key and open locked door
- Run out of ammo behavior
- Pickup collection feedback
- Pause/unpause behavior

### 11.3 Build Testing

After editor playthrough passes:

- Create Windows standalone build.
- Launch build outside Unity.
- Verify mouse capture, keyboard input, restart, pause, and quit behavior.
- Verify audio volume is reasonable.
- Verify HUD scales correctly at common window sizes.

## 12. Non-Goals for First Prototype

These are intentionally out of scope unless added later:

- Multiplayer
- Save/load
- Multiple levels
- Advanced enemy types
- Complex weapon inventory
- Advanced procedural generation
- Full menu system
- Polished animation pipeline
- Networked scoreboards
- Real Doom WAD compatibility
- Exact Doom mechanics or asset recreation

## 13. Likely Future Extensions

After MVP, useful next steps could include:

- Shotgun or plasma weapon
- Second enemy type
- Enemy projectile attacks
- Secret rooms
- Animated doors
- More pickup types
- Better sprite animation
- Retro pixelated render pass
- Main menu
- Level select
- Settings screen for mouse sensitivity and volume
- Minimap
- More complete sound and music pass

## 14. Resolved Design Decisions

These are the default answers for implementation. They can still be changed during review, but work should proceed with these choices unless redirected.

1. v0.0 scope: plain greybox dungeon crawler/shooter proof of concept.
2. v0.0 visuals: Unity primitives, flat colors, and plain text UI.
3. First enemy behavior: melee chaser. v0.0 visual is a capsule or cube; later visual is the `Ash Imp`.
4. Weapon behavior: simple hitscan. v0.0 visual can be invisible/minimal; later visual is the centered 2D `Iron Pistol`.
5. Level theme after v0.0: mixed industrial/gothic, modeled after the classic tech-base-meets-hell feeling.
6. Main menu: skipped for v0.0/MVP; launch directly into `Level01`.
7. Jumping: omitted for v0.0/MVP; movement stays flat, fast, and classic.
8. Render pipeline: prefer built-in render pipeline unless Unity project creation strongly favors URP.
9. Input: prefer classic Unity input unless the chosen template already has the new Input System ready.
10. Audio: deferred for v0.0; short generated/prototype sounds can come later.
11. First objective loop: collect key, open locked door, clear final room, reach exit.
