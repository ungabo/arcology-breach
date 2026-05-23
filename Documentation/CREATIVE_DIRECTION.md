# Unity Doom Experiment - Creative Direction

## 0. v0.0 Note

This document describes the intended creative direction after the first proof of concept works.

For `v0.0`, keep the project plain: greybox dungeon, simple primitive enemies, flat colors, text UI, no custom skins, no generated textures, no sprite sheets, and no audio requirement. The names and art direction below are guide rails for later versions, not blockers for the first build.

## 1. Default Creative Model

When a design choice is uncertain, use the original Doom as the north star:

- Fast movement over tactical realism.
- Flat, readable spaces over vertical complexity.
- Low-resolution texture attitude over polished realism.
- Clear pickups, doors, monsters, and exits.
- Immediate combat feedback.
- A short level that can be completed in minutes.

This project should feel like a small original prototype from the same family of ideas, not a recreation of Doom's exact levels, names, enemies, sounds, or art.

## 2. Working Title

Working title: `Iron Chapel`

The title gives the level a useful blend of industrial machinery and corrupted gothic space. It is short, readable, and easy to replace later.

## 3. Core Pitch

The player wakes inside a sealed industrial shrine built from rusted metal, old stone, warning lights, and infernal machinery. The only way out is through a locked final chamber. A red key is hidden down a side passage. Hostile creatures guard the route. The player has a chunky hand cannon, limited ammunition, and enough speed to survive by moving aggressively.

v0.0 version of this pitch: the player starts in a plain blockout dungeon, fights primitive enemies, collects a key marker, opens a colored door, and reaches a colored exit marker.

The first playable version should be a complete five-minute loop:

1. Start in a quiet room.
2. Learn the controls by moving through a corridor.
3. Fight one enemy.
4. Find health and ammo.
5. Reach a locked red door.
6. Detour through a key path.
7. Return to unlock the door.
8. Survive one final fight.
9. Trigger the exit and win.

## 4. Resolved Design Decisions

### 4.1 Enemy Type

Decision: Start with a melee creature. In v0.0, represent it with a capsule or cube. After v0.0, replace that placeholder with the `Ash Imp`.

Reasoning: The first Doom's early enemies are simple to read, and a melee enemy is the cleanest prototype target. It creates pressure without needing projectile systems, projectile art, dodging rules, or ranged line-of-fire tuning.

Enemy description:

- Name: `Ash Imp`
- Role: Basic pressure enemy.
- Shape: Tall, hunched, broad shoulders, thin legs, horned or jagged head silhouette.
- Color: Dark red body, pale bone highlights, ember-orange eyes.
- Behavior: Idle until the player is close or visible, then closes distance quickly and swipes.
- Combat purpose: Forces the player to backpedal, circle, and shoot while moving.
- Death: Collapses into a dark red heap or fades after leaving a corpse sprite.

Future extension:

- Add a ranged cultist or fireball enemy after the MVP loop is working.

### 4.2 Weapon Visual

Decision: Use no required weapon art in v0.0. After the greybox loop works, use a centered 2D weapon sprite.

Reasoning: Classic Doom's centered weapon read is iconic, fast to implement, and avoids first-person 3D animation complexity. A sprite also matches the enemy billboard plan.

Weapon description:

- Name: `Iron Pistol`
- Role: Starter weapon.
- Shape: Bulky black/gray hand cannon with a short barrel and red-lit rear sight.
- Screen position: Center-bottom of the view.
- Animation: Subtle idle bob, quick upward kick on fire, brief muzzle flash.
- Mechanics: Hitscan, medium fire rate, reliable accuracy.
- Feel: Loud, simple, chunky.

### 4.3 Level Theme

Decision: Use a plain greybox dungeon for v0.0. After v0.0, use a mixed industrial/gothic theme, with industrial as the base and gothic as the corruption layer.

Reasoning: The original Doom often feels like tech bases being swallowed by hellish spaces. This gives us visual contrast while keeping assets simple.

Theme description:

- Base: Rusted metal panels, concrete floors, hazard trims, grated walkways.
- Corruption: Stone blocks, red-lit alcoves, skull-like key markers, dark chapel chamber.
- Color hierarchy:
  - Walls: Charcoal, rust red, dull gray.
  - Floors: Concrete gray and dark metal.
  - Accents: Warning yellow, red key lights, sickly green exit glow.
  - Enemies: Red/orange so they stand apart from the environment.

### 4.4 Main Menu

Decision: Skip the main menu for MVP.

Reasoning: The first goal is a playable level. The build can launch directly into `Level01`. Pause, death, and win screens are enough for the first prototype. A menu can be added after the game loop works.

MVP flow:

- Launch executable.
- Begin in `Level01`.
- Press Escape to pause.
- Die or win to get restart/quit options.

### 4.5 Jumping

Decision: Omit jumping for MVP.

Reasoning: Classic Doom movement is fast but mostly flat. Removing jump keeps level design, enemy navigation, collision, and testing much simpler.

Movement description:

- Fast walking speed.
- No jump.
- No crouch.
- Optional sprint is deferred unless the base speed feels too slow.
- Level geometry stays readable and mostly flat.

### 4.6 Render Pipeline

Decision: Prefer the built-in render pipeline unless the installed Unity version strongly favors URP during project creation.

Reasoning: The desired style does not need advanced rendering. Built-in materials and simple lighting are enough, and setup should stay lean.

### 4.7 Input Approach

Decision: Use classic Unity input for MVP unless the created project template already includes and configures the new Input System.

Reasoning: The control set is tiny, and classic input is quick to inspect and debug.

## 5. Player Experience Description

The player should feel fast, lightly armed, and trapped. The correct response to danger is movement: strafe, backpedal, keep firing, grab supplies, and keep pushing forward.

The game should avoid long pauses or complicated interactions. Doors open quickly. Pickups collect instantly. Enemies make themselves known immediately. The HUD should be plain and direct.

## 6. Level Description

### 6.1 Start Room - Intake Cell

A small square chamber with concrete flooring, dark metal walls, and one lit exit. The player starts facing the doorway. A subtle green light or arrow-like trim can pull attention toward the corridor.

Purpose:

- Establish scale.
- Let the player move and look around safely.
- Frame the first corridor.

### 6.2 First Corridor - Rust Throat

A narrow metal corridor with repeating wall panels and warning trim. One bend or partial wall prevents the entire level from being visible immediately.

Purpose:

- Teach forward movement.
- Build anticipation before the first enemy.

### 6.3 First Fight Room - Furnace Antechamber

A medium room with one `Ash Imp`. The room has enough space to backpedal and strafe. A small health pickup sits beyond the enemy so the player is rewarded for clearing the room.

Purpose:

- Teach shooting.
- Teach enemy threat.
- Confirm health and ammo HUD behavior.

### 6.4 Hub Room - Red Lock Junction

A larger junction with a clearly marked red locked door and a side path. The locked door should have red trim, a key symbol, or a red light. The side path should be obviously available.

Purpose:

- Introduce the goal.
- Make the player understand they need the key.
- Provide a central landmark.

### 6.5 Key Path - Broken Reliquary

A side route with stone wall sections interrupting the metal base. It contains one or two enemies, an ammo pickup, and the red key at the end.

Purpose:

- Create a short objective detour.
- Add contrast through gothic stone textures.
- Give the player enough ammo for the final room.

### 6.6 Final Room - Iron Chapel

A wider chamber behind the red locked door. It has a darker ceiling, stone and metal walls, and a sickly green exit portal or switch at the far end. Two or three enemies guard the exit.

Purpose:

- Deliver the final combat beat.
- Give the level a memorable endpoint.
- Trigger the win state.

## 7. Asset Descriptions

### 7.1 Wall Textures

`metal_panel.png`

- Dark gray base.
- Repeating rectangular panels.
- Rust seams and bolt dots.
- Tileable at 64x64 or 128x128.

`stone_brick.png`

- Uneven gray stone blocks.
- Dark mortar lines.
- Occasional red-brown staining.
- Used in corrupted/gothic areas.

`warning_trim.png`

- Yellow and black diagonal hazard stripe.
- Used around doors, key areas, and dangerous edges.
- Should be readable even at a distance.

### 7.2 Floor and Ceiling Textures

`concrete_floor.png`

- Flat gray floor with cracks and scuffs.
- Low contrast so enemies and pickups stand out.

`metal_grate_floor.png`

- Dark metal with simple grid openings or stripes.
- Used sparingly for visual variety.

`dark_ceiling_panel.png`

- Nearly black metal tile.
- Subtle panel lines.
- Keeps the player's focus on walls, enemies, and pickups.

### 7.3 Pickups

Health pickup:

- Red and white med box or vial.
- Bright enough to spot on the floor.
- Slight bob/spin animation.

Ammo pickup:

- Small dark metal ammo box.
- Yellow bullet marks or hazard stripe.
- Slight bob/spin animation.

Red key:

- Floating red keycard or skull-key token.
- Red light or glow.
- Larger visual priority than normal pickups.

### 7.4 Effects

Muzzle flash:

- Brief yellow/orange starburst at the weapon barrel.
- Very short duration.

Impact spark:

- Small yellow/white hit burst.
- Can vanish quickly.

Damage flash:

- Red transparent overlay around the screen.
- Short and punchy.

Exit:

- Sickly green portal, switch, or glowing slab.
- Should be the brightest non-enemy element in the final room.

## 8. UI Description

The HUD should be low and compact:

- Health on the lower left.
- Ammo on the lower right.
- Key indicator near the center or lower middle.
- Crosshair at screen center.

Visual style:

- Blocky text.
- Dark translucent backing only if needed for readability.
- Red damage flash.
- Pause/death/win panels use plain text and simple buttons.

## 9. Audio Description

The sound direction should be sharp and synthetic, with short clips:

- Pistol: loud midrange blast, no long tail.
- Empty click: dry mechanical tap.
- Enemy hurt: short distorted grunt.
- Enemy death: lower distorted collapse sound.
- Pickup: bright blip.
- Door: short metal slide or clank.
- Player hurt: low thud or breath hit.
- Win: brief green/arcane synth chord.

Music is deferred. If added later, use a short dark synth loop with minimal melody.

## 10. Implementation Notes From Creative Direction

- Prioritize the first vertical slice over polish.
- Use sprites/billboards for enemies, weapon, pickups, and effects where practical.
- Use simple geometry for level walls, floors, and ceilings.
- Keep the level mostly flat.
- Make the red key and red locked door visually linked.
- Make the final exit green so it is distinct from the red key path.
- Keep enemy behavior simple and readable.
- Add polish only after the complete loop works.
