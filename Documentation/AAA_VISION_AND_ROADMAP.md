# AAA-Style Vision and Roadmap

## 1. Intent

The project has proven that a simple Unity FPS loop can be built and packaged on this machine. The next long-term direction is to grow the proof of concept into a modern, high-quality first-person action game while keeping the original design DNA: fast movement, clear combat, maze-like spaces, readable enemies, key/door progression, secrets, and strong atmosphere.

This document does not pretend that a one-person prototype becomes a full AAA game overnight. It defines an AAA-style target so every later pass can move in a coherent direction.

## 2. Target Experience

Working title remains `Iron Chapel`.

The player explores a hostile industrial cathedral complex where military machinery, gothic stonework, and occult energy have fused together. Combat should be immediate and aggressive. The game should reward movement, target prioritization, resource awareness, and exploration.

The final target should feel:

- Fast and physical.
- Dark but readable.
- Brutal without visual noise.
- Heavy on atmosphere but light on downtime.
- Built around tight combat arenas connected by exploration spaces.
- Understandable on first play, deeper on repeated play.

## 3. Design Pillars

1. Combat clarity
   - Every enemy has a readable silhouette, attack tell, role, and counterplay.
   - Projectiles, melee ranges, pickups, hazards, and exits must be legible under pressure.

2. Movement-first survival
   - Standing still is dangerous.
   - Combat arenas give room for strafing, flanking, and repositioning.
   - Weapons and enemies are tuned around motion.

3. Industrial gothic identity
   - Rusted machine spaces and stone ritual architecture share the same world.
   - Red, green, amber, steel, black, and bone tones are used with discipline.

4. Layered progression
   - Core path is obvious enough to avoid frustration.
   - Optional secrets reward close inspection.
   - Keys, switches, shortcuts, and locked areas create structure.

5. Production discipline
   - Every feature has an owner status, acceptance criteria, and verification path.
   - New ideas go into the backlog before implementation.
   - Handoff docs stay current enough for a new chat to resume work.

## 4. Game Scope Ladder

### v0.0 Complete

Proof of concept:

- Greybox level.
- FPS movement.
- Hitscan weapon.
- Primitive enemies.
- Key, door, exit.
- HUD, death, win, restart.
- Windows build and smoke tests.

### v0.1 Complete

Light presentation pass:

- Blocky weapon placeholder.
- Muzzle flash.
- Damage flash.
- Bobbing pickups.
- Sliding door.
- Accent lights.
- Enemy eye markers.

### v0.2 Target: Playable Combat Slice

Goal: make the game loop feel good before adding large content volume.

- Replace direct enemy movement with more robust navigation.
- Add one polished basic enemy with placeholder animation states.
- Add weapon spread, impact reactions, and recoil tuning.
- Add simple sound effects.
- Add combat arena with cover, loops, and resource placement.
- Add manual playtest checklist and tuning notes.

### v0.3 Target: Art Direction Slice

Goal: replace greybox visuals with a cohesive prototype look.

- Generate first tileable wall/floor/trim texture set.
- Create first weapon sprite/model pass.
- Create first enemy skin/sprite/model pass.
- Add atmospheric lighting pass.
- Add basic VFX for muzzle, impact, blood/sparks, pickups, door, exit.
- Establish naming conventions and import settings for generated assets.

### v0.4 Target: Systems Foundation

Goal: make the project scalable.

- Data-driven weapon definitions.
- Data-driven enemy definitions.
- Health, armor, ammo, and pickup system cleanup.
- Damage type model.
- Interaction system.
- Save-free level state reset.
- Better game state flow.
- Build automation scripts.

### v0.5 Target: First Vertical Slice

Goal: one level that represents the future game.

- One complete level with beginning, middle, finale, and secrets.
- Two weapons.
- Three enemy types.
- Health/ammo/armor economy.
- Locked door/key/switch loop.
- Audio pass.
- Lighting pass.
- Start/win/death/pause flow.
- Windows build validated through full manual playthrough.

### v0.6 Target: Content Expansion

Goal: prove the pipeline can make more than one level.

- Second level.
- Additional texture themes.
- Additional enemy variant.
- Additional weapon or alternate fire.
- More props and environmental hazards.
- Level transition flow.

### v0.7 Target: Feel and Presentation

Goal: improve moment-to-moment quality.

- Weapon animations.
- Enemy animation and hit reactions.
- Better enemy attack tells.
- Camera shake and impulse tuning.
- Footstep, weapon, monster, UI, and ambient audio.
- More sophisticated HUD.
- Performance profiling.

### v0.8 Target: Beta-Style Content Lock

Goal: stabilize a small complete game.

- Finalize core mechanics.
- Complete planned levels.
- Complete core asset set.
- Replace temporary code paths.
- Validate all win/death/restart flows.
- Full bug backlog triage.

### v0.9 Target: Release Candidate

Goal: polish, optimize, and package.

- Performance budget pass.
- Bug fixing.
- Accessibility options.
- Settings menu.
- Save/settings persistence.
- Installer or packaged release folder.
- Public release notes.

### v1.0 Target: Public Prototype Release

Goal: a complete, playable, public prototype.

- Public repo has source and docs.
- Release build is attached through GitHub Releases or documented build steps.
- Known issues are documented.
- Future roadmap is clear.

## 5. AAA-Style Feature Domains

### Gameplay

- Fast first-person movement.
- Multiple weapons.
- Multiple enemy roles.
- Key/door/switch progression.
- Secrets.
- Resource economy.
- Arena combat.
- Environmental hazards.
- Difficulty tuning.

### Level Design

- Modular room and corridor kit.
- Combat arenas.
- Exploration spaces.
- Landmark composition.
- Shortcuts.
- Secrets.
- Level scripting.
- Checkpoint/restart design.

### AI

- Enemy perception.
- Navigation.
- Attack selection.
- Group pressure.
- Stagger/hit reactions.
- Spawn triggers.
- Difficulty scaling.

### Combat

- Weapon handling.
- Damage model.
- Hit feedback.
- Enemy reactions.
- Player damage feedback.
- Ammo economy.
- Armor or mitigation model.

### Presentation

- Environment materials.
- Enemy skins.
- Weapon models/sprites.
- Props.
- VFX.
- Lighting.
- Post-processing.
- UI/HUD.
- Animation.
- Audio.

### Technical

- Unity project structure.
- Editor generation tools.
- Data-driven content.
- Build automation.
- Smoke tests.
- Manual test plans.
- Profiling.
- Version control hygiene.

## 6. Quality Bars

### Prototype Bar

- Feature works.
- Clear enough to test.
- Can be rebuilt.
- Does not block other work.

### Vertical Slice Bar

- Feature works in context.
- Has readable visuals and audio.
- Tuned enough for manual playtesting.
- Has acceptance criteria.
- Has basic regression coverage or a repeatable test.

### AAA-Style Bar

- Feature has final-ish art/audio direction.
- Works across levels and edge cases.
- Has debug tooling.
- Has performance budget.
- Has accessibility consideration.
- Has documented owner/status/history.

## 7. Current Strategic Priority

Next priority after the current v0.1 state:

1. Manual playthrough and tuning.
2. Add simple audio.
3. Improve enemy movement and attack readability.
4. Create first material/texture set.
5. Replace primitive weapon/enemy visuals with first stylized placeholders.

Do not jump straight to large asset generation before the combat loop feels good.
