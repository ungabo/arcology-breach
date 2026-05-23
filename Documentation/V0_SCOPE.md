# Unity Doom Experiment - v0.0 Proof-of-Concept Scope

## 1. Purpose

Version `0.0` is a proof of concept, not a polished game slice.

The only question v0.0 needs to answer is:

Can we use Unity on this machine to quickly build a simple first-person dungeon crawler/shooter loop that runs on Windows?

If the answer is yes, later versions can add Doom-like skins, sprites, textures, sounds, visual effects, and stronger level identity. For v0.0, the project should stay intentionally plain.

## 2. v0.0 Design Rule

Prefer the simplest thing that proves the mechanic.

- Use cubes for walls.
- Use flat colors instead of textures.
- Use capsules or cubes for enemies.
- Use simple trigger volumes for pickups and exits.
- Use text UI instead of custom HUD art.
- Use debug-style feedback where polished feedback would slow the build down.
- Avoid asset generation unless a tiny placeholder is faster than a primitive.

## 3. v0.0 Player Experience

The player launches directly into a small greybox dungeon. They can:

1. Move with WASD.
2. Look around with the mouse.
3. Shoot a simple hitscan weapon.
4. Kill one or more primitive enemies.
5. Collect a key.
6. Open a locked door.
7. Reach an exit.
8. See a win state.

The v0.0 level can look like a blockout. It only needs to be readable and playable.

## 4. Included in v0.0

### 4.1 Project and Build

- Unity project created in `D:\__MY APPS\Unity Doom`.
- One playable scene: `Level01`.
- Windows standalone build target configured.
- Project can be opened and played in Unity.
- Ideally, a Windows build can be produced and launched.

### 4.2 Player

- First-person camera.
- Mouse look.
- WASD movement.
- Collision with walls.
- Cursor lock during gameplay.
- No jump.
- No crouch.
- Sprint omitted unless movement feels painfully slow.

### 4.3 Weapon

- One invisible or ultra-minimal weapon.
- Left click fires.
- Hitscan raycast from screen center.
- Ammo count.
- Fire cooldown.
- Simple impact feedback:
  - Console/log optional during development.
  - Tiny primitive marker or particle optional.

No weapon sprite is required for v0.0.

### 4.4 Enemy

- One primitive enemy type.
- Capsule or cube visual.
- Detects player by distance.
- Moves toward player.
- Damages player at close range.
- Takes damage.
- Dies/disappears when health reaches zero.

No sprite, animation, or custom model is required for v0.0.

### 4.5 Health and Inventory

- Player health.
- Player death state.
- Ammo count.
- Key possession.
- Health pickup optional but useful.
- Ammo pickup optional but useful.
- Key pickup required.

### 4.6 Door and Exit

- One locked door.
- Door requires key.
- Door can open by trigger or interaction.
- One exit trigger beyond the locked door.
- Win state when exit is reached.

### 4.7 UI

- Plain text HUD:
  - Health
  - Ammo
  - Key yes/no
- Simple crosshair, which can be a text `+` or minimal UI image.
- Death text.
- Win text.
- Restart key.
- Pause optional for v0.0.

### 4.8 Level

- Small greybox layout:
  - Start room
  - Corridor
  - Enemy room
  - Key area
  - Locked door
  - Exit room
- Primitive walls/floors.
- Flat colors:
  - Gray walls
  - Dark floor
  - Red locked door
  - Yellow key marker
  - Green exit marker
  - Red or orange enemy marker

## 5. Explicitly Deferred Until After v0.0

- Generated textures.
- Sprite sheets.
- Enemy art.
- Weapon art.
- Pickup art.
- Custom audio.
- Music.
- Muzzle flash.
- Impact decals.
- Main menu.
- Multiple enemy types.
- Ranged enemies.
- Additional weapons.
- Advanced level dressing.
- Polished lighting.
- Pixelated render effect.
- Save/load.
- Settings menu.

## 6. v0.0 Acceptance Criteria

v0.0 is complete when:

- `Level01` runs in the Unity editor.
- The player can move, look, shoot, and collide with walls.
- At least one enemy can chase, attack, take damage, and die.
- The HUD shows health, ammo, and key state.
- The player can collect a key.
- The locked door blocks progress until the key is collected.
- The player can reach the exit and trigger a win message.
- The player can die and restart.
- The project has a clear path toward a Windows build.

Optional but desirable:

- A Windows standalone build launches and can be completed.
- Health and ammo pickups exist.
- Pause works.

## 7. v0.0 Non-Goal Reminder

Do not judge v0.0 by how it looks.

Judge it by whether the core loop works and whether the Unity workflow is smooth enough to continue.

## 8. After v0.0

If v0.0 works, the next version can become `v0.1`, focused on presentation:

- Replace primitive enemy with `Ash Imp` sprite.
- Add centered `Iron Pistol` sprite.
- Add simple textures.
- Add pickup visuals.
- Add muzzle flash and impact effects.
- Add sounds.
- Improve lighting and level mood.
