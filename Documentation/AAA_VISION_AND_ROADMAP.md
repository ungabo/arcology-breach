# Brassworks Breach - AAA-Style Vision and Roadmap

## 1. Intent

Grow the current Unity proof of concept into an original, heavily stylized steampunk first-person action game. The target is a polished, atmospheric, readable shooter with strong mechanical enemy identity and a tactile brassworks world.

## 2. Target Experience

The player enters sealed industrial works, pipe spines, gauge halls, foundries, lift shafts, and governor-core machine chambers. Combat is fast and legible. Exploration is compact but layered. The world tells a story about broken infrastructure, hidden workers, pressure logic, and tools turned violent.

## 3. Design Pillars

1. Steampunk identity
   - Brass, copper, riveted iron, gaslight, soot, steam, gauges, valves, gears, and clockwork machines.

2. Mechanical threat language
   - Enemies are corrupted work machines and pressure systems.

3. Movement-first combat
   - Combat arenas reward strafing, repositioning, and target priority.

4. Environmental storytelling
   - Lore is embedded in spaces, signs, work orders, maintenance marks, and machine behavior.

5. Production discipline
   - Every feature, asset, and milestone is tracked, verified, and kept tied to buildable slices.

## 4. Roadmap

### v0.0 Complete

- Greybox FPS loop.
- Primitive enemy.
- Key/lock objective.
- Locked route and exit.
- Editor/build/runtime smoke tests.

### v0.1 Complete

- Basic presentation feedback.
- Blocky weapon.
- Muzzle flash.
- Damage flash.
- Bobbing pickups.
- Sliding gate.
- Accent lights.

### v0.2 Target: Steampunk Combat Feel Slice

- Manual Windows playthrough.
- Retheme all objective text and object names to the brassworks identity.
- Tune player speed, weapon fire, ammo, enemy speed, and damage.
- Improve mechanical enemy readability.
- Tune simple steamworks audio set.
- Add clearer gear-key/pressure-gate feedback.
- Confirm `Brassworks Intake` scale, room flow, and gate/key/lift spatial relationship.

### v0.3 Target: Steampunk Art Direction Slice

- First brass/iron/oil-stone material kit.
- Gear-key visual.
- Pressure-gate visual.
- Service-lift visual.
- Scrapper enemy visual.
- Pressure Pistol visual.
- First steam/spark VFX pass.

### v0.4 Target: Systems Foundation

- Data-driven weapons.
- Data-driven enemies.
- Interaction system.
- Pickup and inventory cleanup.
- Level transition controller.
- Level validation tool.
- Build automation cleanup.

### v0.5 Target: First Vertical Slice

- One polished steampunk level.
- Two weapons.
- Three mechanical enemy types.
- Secrets.
- Audio pass.
- Lighting pass.
- UI pass.
- Full manual playthrough validation.

### v0.6 Target: Content Expansion

- Second level.
- Additional mechanical enemy.
- Additional weapon.
- More props and environmental hazards.
- Level transition flow.

### v0.7 Target: Feel and Presentation

- Weapon animations.
- Enemy animations.
- Better attack tells.
- Camera impulse tuning.
- Stronger audio mix.
- Performance profiling.

### v0.8 Target: Beta-Style Content Lock

- Planned mechanics complete.
- Core assets complete.
- Known issues triaged.
- Replace temporary code paths.

### v0.9 Target: Release Candidate

- Optimization.
- Bug fixing.
- Settings menu.
- Accessibility options.
- Release packaging.

### v1.0 Target: Public Prototype Release

- Public source/docs are coherent.
- Build instructions are reliable.
- Release notes and known issues are written.
- Windows build is playable end to end.

## 5. Campaign and Map Ladder

Detailed map notes live in `LEVEL_DESIGN_AND_MAPS.md`. Current planned ladder:

1. `Brassworks Intake`: gear key, pressure gate, service lift.
2. `Pipeworks Spine`: pressure routing, longer sightlines, first ranged machine.
3. `Gauge Hall`: lock sequences, valve puzzles, support-node enemy.
4. `Furnace Foundry`: industrial hazards and heavy machines.
5. `Governor Core`: final breach and mixed mechanical opposition.

## 6. Current Strategic Priority

1. Retheme code, scene, and docs to the steampunk north star.
2. Complete v0.0.7 pause/quit flow and automated test coverage.
3. Build and test v0.0.7.
4. Tune combat feel.
5. Build first steampunk material/enemy/weapon visual pass.
