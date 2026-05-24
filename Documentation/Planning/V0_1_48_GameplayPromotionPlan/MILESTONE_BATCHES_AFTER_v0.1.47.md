# Recommended Milestone Batches After v0.1.47

Purpose: sequence five high-visibility gameplay-promotion batches after v0.1.47. Each batch promotes groups of related visuals into owned systems, avoiding serial single-object work.

## Batch 1: Route Shell And Collision Pilot

Candidate version: v0.1.48

Promote selected `corridor-kit-set02` and `steamworks-level-kit` pieces into one controlled collision-authoritative route segment. Include corridor straight, corner, door frame, floor grate, cover crate, pipe railing, and one boiler alcove family.

Why first: players immediately feel a real place instead of isolated showcase objects, and collision lessons inform every later level-art batch.

Required QA:

- Proxy collider review.
- Movement and stuck-point route pass.
- Auto-playthrough pass.
- Combat sightline check.
- Before/after screenshots for readability.

## Batch 2: Objective Language Pass

Candidate version: v0.1.49

Promote `objective-props-set02` with selected `weapon-props-set02` support props into the existing objective route: keyed lock, pressure gate socket, valve panel, lift call station, secret cache, route-blocked feedback, and boss override switch.

Why second: objective props convert the existing progression into visible steampunk verbs without needing new mission logic.

Required QA:

- Interaction radius checks.
- Objective prompt and completion event checks.
- Route-blocked event check.
- Secret cache readability pass.
- Save/no-softlock review if persistence exists in the future slice.

## Batch 3: Feedback And VFX Socket Map

Candidate version: v0.1.50

Promote `steam-vfx-set02` and `feedback-fx-audio` through an event socket map. Cover weapon fired, weapon empty, weapon impact, enemy hit, enemy death, pickup collected, objective completed, route blocked, steam hazard, furnace hazard, and boss phase.

Why third: it makes every existing mechanic feel more finished while keeping authority in current controllers.

Required QA:

- Event trigger matrix.
- Audio mixer/settings pass.
- Particle lifetime and overdraw review.
- Runtime audio mix test.
- Combat readability pass with effects enabled.

## Batch 4: Enemy Visual Archetype Swap

Candidate version: v0.1.51

Promote enemy visuals in archetype batches using `mechanical-enemies`, `mechanical-enemy-visual-set01`, `encounter-enemy-set02`, and `enemy-animation-proxy-set01`. Map Scrapper, Lancer, Bulwark, Bellows support, and Warden visuals to existing controllers and hit volumes.

Why fourth: enemy silhouettes benefit from the earlier feedback/VFX work, and combat readability can be tested against a more complete environment.

Required QA:

- Visual shell to controller mapping.
- Hit volume overlay review.
- Attack tell readability screenshots.
- Combat route tests across Levels 01-05.
- Performance check for enemy-heavy rooms.

## Batch 5: Weapon/Viewmodel Cohesion Pass

Candidate version: v0.1.52

Promote `weapon-viewmodel-set03`, `weapon-props-set02`, and `steampunk-weapons` into a cohesive first-person and world-prop pass for pressure pistol and scattergun. Include muzzle sockets, grip alignment, ammo cells, pickup display, wall rack, weapon switch feedback, fired/empty/impact feedback, and material palette consolidation.

Why fifth: this gives a high-value player-facing upgrade after environment, objective, feedback, and enemy context are stable enough to judge weapon readability.

Required QA:

- Muzzle origin and projectile alignment.
- Viewmodel clipping at target FOVs.
- Recoil/reload timing review if implemented.
- Pickup/inventory balance pass.
- Weapon feedback event test.

## Deferred But Ready

The v0.1.46 planning packets for `RoomSetpieceKit04` and `WeaponMechanismsSet04` should remain future-lane imports until the active manifest actually references them and the quarantine validator covers them. Once imported, they fit naturally into Batch 1 and Batch 5 respectively.
