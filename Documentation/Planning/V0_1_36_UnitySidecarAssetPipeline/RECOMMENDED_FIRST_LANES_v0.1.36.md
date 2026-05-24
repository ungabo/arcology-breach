# V0.1.36 Recommended First Sidecar Lanes

Purpose: choose the first sidecar asset-pack lanes that provide the most development speed without taking authority away from the primary Unity project.

## Recommended Start Order

1. Weapon/prop lookdev pack.
2. Mechanical enemy visual pack.
3. Level module/prefab pack after the first two prove the import gates.

The feedback UI/audio/VFX lane is valuable, but it should be fourth unless the main lane needs it urgently, because UI and audio integration can quickly touch runtime settings, mixer assumptions, input hints, or gameplay feedback timing.

## Lane 1: Weapon/Prop Lookdev Pack

Proposed project: `UD-SC-WPN-WeaponPropLookdev`

Proposed package: `com.brassworks.sidecar.weapon-prop-lookdev`

Why first:

- Strong fit for visual-only sidecar work.
- Existing project already has pressure pistol, scattergun, pickups, ammo, wall station, and prop language to extend.
- Scale, pivot, muzzle, socket, pickup, and wall-display checks can be validated without changing gameplay code.
- Failures are easy to quarantine and roll back.

Candidate contents:

- Pressure Pistol visual polish candidates.
- Steam Scattergun silhouette/material candidates.
- Ammo pressure cell and slug canister variants.
- Wall pressure station and crank lever prop variants.
- Viewmodel scale proxies and display/pickup pivots.
- Material recipe updates for brass, copper, soot, gauge glass, amber charge, and red danger states.

Sidecar outputs:

- UPM-shaped package folder.
- Optional `.unitypackage` snapshot.
- Contact sheet with first-person crop, pickup crop, and wall-display crop.
- Manifest with prefab/material/mesh counts.
- Import report with missing-reference and scale/pivot findings.

Primary-lane dependencies:

- Existing weapon runtime components remain authoritative.
- No new ammo, damage, reload, unlock, or input logic.
- Any socket names must align with v0.1.36 rig/socket readiness docs.

## Lane 2: Mechanical Enemy Visual Pack

Proposed project: `UD-SC-ENM-MechanicalEnemyVisuals`

Proposed package: `com.brassworks.sidecar.mechanical-enemy-visuals`

Why second:

- Enemy visuals benefit from parallel silhouette and material work while gameplay continues.
- Current staged families give clear targets: Scrapper, Lancer, Bulwark, Warden, and Foundry Overseer.
- Visual identity can be tested in isolation through lineup previews, weak-point visibility, LODs, and material states.

Candidate contents:

- Visual-only prefab candidates for Scrapper, Lancer, Bulwark, Warden, and Foundry Overseer.
- Weak-point material variants and furnace-eye/charge-state material variants.
- Socket marker empties for future animation and VFX.
- LOD0/LOD1/LOD2 visual candidates.
- Enemy lineup preview scene under `Samples~`, not for primary-game import as a scene.

Sidecar outputs:

- UPM-shaped package folder.
- Contact sheet with neutral gray silhouette, lit material, weak-point, and LOD comparisons.
- Manifest listing required sockets and material state names.
- Import report focused on missing materials, transform naming, LOD readability, and prefab references.

Primary-lane dependencies:

- No spawn logic.
- No combat timing.
- No route authority.
- No new enemy behavior scripts.
- No scene placement.

## Lane 3: Level Module/Prefab Pack

Proposed project: `UD-SC-LVL-LevelModulesPrefabs`

Proposed package: `com.brassworks.sidecar.level-modules-prefabs`

Why third:

- It can provide a large visual lift, but it has higher collision risk with level layout, colliders, lighting, occlusion, and route smoke checks.
- It should start after lane 1 proves package/import mechanics and lane 2 proves larger prefab-family review.

Candidate contents:

- Corridor bay modules.
- Door and vault frame modules.
- Pipe gallery wall runs.
- Furnace/boiler alcoves.
- Catwalk rail and trim modules.
- Caged gaslight, gauge clusters, valve wheels, and pressure signage variants.

Sidecar outputs:

- Grid/scale validation sheet.
- Collision guidance document.
- LOD and material budget table.
- Contact sheet showing modules at player-height camera.
- Manifest listing module dimensions and required pivot rules.

Primary-lane dependencies:

- Main lane owns actual level placement.
- Main lane owns route blockers, nav constraints, objective markers, encounter pacing, lighting bake policy, and performance validation.
- Sidecar exports should remain modular visual candidates until placement smoke passes.

## Deferred Lane: Feedback UI/Audio/VFX Pack

Proposed project: `UD-SC-FBK-FeedbackUIAudioVFX`

Proposed package: `com.brassworks.sidecar.feedback-ui-audio-vfx`

Why defer:

- Feedback assets are useful but easy to over-couple with runtime timing, settings, mixers, input prompts, and accessibility rules.
- Best used after the acceptance gates are proven and after the primary lane defines exactly which feedback events need content.

Candidate contents:

- Hit sparks, steam puffs, pressure bursts, furnace embers, pickup glints.
- HUD icon sprites and warning decals.
- One-shot audio candidates for dry-fire, charge-ready, valve turn, unlock, impact, and weak-point break.
- Visual-only feedback prefab candidates with no gameplay event wiring.

Primary-lane dependencies:

- Main lane owns event timing, mixer routing, volume settings, accessibility toggles, and UI binding.

## Recommended v0.1.36 Pilot

Pilot only two sidecars:

1. `UD-SC-WPN-WeaponPropLookdev`
2. `UD-SC-ENM-MechanicalEnemyVisuals`

Success criteria before a third sidecar:

- Both packs export with unique package roots and manifests.
- Both pass clean throwaway import smoke.
- Primary lane can review without touching packages, build settings, scenes, or scripts.
- Rollback is a single root-folder deletion for each pack.
- Reviewers are not waiting on missing docs, screenshots, or dependency lists.
