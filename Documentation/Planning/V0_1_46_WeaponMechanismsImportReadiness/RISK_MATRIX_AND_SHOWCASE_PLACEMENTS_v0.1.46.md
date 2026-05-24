# V0.1.46 Risk Matrix And Showcase Placements

Purpose: identify import risks and propose presentation-only placements for WeaponMechanismsSet04 across all five v0 levels. These placements are suggestions for main-lane scene builder work after package resolution and import validation pass.

## Risk Matrix

| Risk | Likelihood | Impact | Mitigation | Validation owner |
| --- | --- | --- | --- | --- |
| Weapon components are mistaken for gameplay-ready weapons | Medium | High | Keep every instance under quarantine showcase roots and block firing, damage, inventory, pickup, muzzle authority, recoil, reload, projectile, and weapon selection scripts. | Gameplay owner |
| Visual muzzle crowns imply firing or damage authority | Medium | High | Validate as static meshes only; do not attach projectile origins, hitboxes, damage volumes, muzzle flash systems, or weapon configs. | Gameplay owner |
| Ammo cylinders imply ammo count or reload state | Medium | Medium | Treat cylinders as visual reference only; do not connect to ammo, reload, pickup, UI, or inventory systems. | Design owner |
| Gloved hand silhouettes are mistaken for rigged viewmodel hands | Medium | High | Keep silhouettes unrigged and unanimated; future promotion requires animation, rigging, clipping, FOV, and player-body ownership review. | Animation owner |
| Viewmodel promotion happens before scale/socket review | Medium | High | Require a later weapon-viewmodel integration packet before using these pieces in the live first-person camera stack. | Art integration |
| Modular parts produce cluttered or unreadable quarantine showcases | Medium | Medium | Place one or two compact objects per level and inspect from player-height camera before raising renderer thresholds. | Level owner |
| Emissive/glass materials imply live lighting or gameplay signals | Medium | Medium | Treat glow and glass as materials only; do not add Light components, warning state logic, or interactable feedback without a later systems pass. | Art integration |
| Prefabs gain colliders, rigidbodies, audio, or gameplay scripts during placement | Low | High | Use existing presentation cleanup and validator scans to require zero colliders, zero rigidbodies, zero autonomous audio, and no gameplay authority scripts. | Main lane |
| Rollback leaves missing package references in generated scenes | Medium | High | Keep placements named and grouped under `Sidecar Quarantine Showcase - <LevelXX>`; remove package reference, validator entry, placements, and required names together. | Main lane |

## Showcase Rules

- Put every imported visual under `Sidecar Quarantine Showcase - <LevelXX>`.
- Keep all WeaponMechanismsSet04 instances visual-only.
- Do not attach or preserve colliders, rigidbodies, triggers, autonomous audio sources, AI controllers, damage scripts, hitboxes, pickups, inventory scripts, weapon configs, muzzle flash authority, projectile spawners, recoil scripts, reload scripts, or viewmodel animation controllers.
- Do not place weapon parts across doors, route chokepoints, pickups, enemy telegraphs, objective reads, exits, or boss mechanics.
- Use labels and object names that clearly identify the quarantine package and avoid suggesting live weapon availability.
- Treat gloved-hand silhouette pieces as composition references only, not player hands.

## Proposed Per-Level Placements

| Level | Placement name | Asset path | Position | Rotation | Scale | Reason |
| --- | --- | --- | --- | --- | ---: | --- |
| Level01 | `WMS04PressurePistolCoilTripleAmberA` | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_PressurePistolCoil_TripleAmber_A.prefab` | `(-5.10, 0.55, 10.75)` | `(0, 92, 0)` | 0.38 | Intro-level pistol coil read without live firing, pickup, or damage authority. |
| Level01 | `WMS04GaugeClusterTripleIvoryA` | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_GaugeCluster_TripleIvory_A.prefab` | `(-4.72, 1.22, 10.25)` | `(0, 90, 0)` | 0.42 | Early gauge-language review beside weapon component silhouette. |
| Level02 | `WMS04GripAssemblyWalnutLeatherA` | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_GripAssembly_WalnutLeather_A.prefab` | `(5.05, 0.42, 13.20)` | `(0, -58, 0)` | 0.42 | Grip scale, wood/leather material, and future hand-alignment review. |
| Level02 | `WMS04ReceiverPlateBrassLatticeA` | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_ReceiverPlate_BrassLattice_A.prefab` | `(5.32, 1.06, 13.05)` | `(0, -78, 0)` | 0.44 | Receiver-side detail read in pipeworks lighting. |
| Level03 | `WMS04AmmoCylinderEightCellB` | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_AmmoCylinder_EightCell_B.prefab` | `(-5.18, 0.54, 16.25)` | `(0, 88, 0)` | 0.42 | Ammo mechanism visual check with no ammo, reload, pickup, or UI authority. |
| Level03 | `WMS04ScattergunPressureChamberQuadB` | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_ScattergunPressureChamber_Quad_B.prefab` | `(-4.95, 0.62, 17.20)` | `(0, 102, 0)` | 0.34 | Bulky scattergun component density and player-height readability review. |
| Level04 | `WMS04BoltThrowerRailChargedSlideB` | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_BoltThrowerRail_ChargedSlide_B.prefab` | `(5.22, 0.64, 20.40)` | `(0, -66, 0)` | 0.34 | Long rail profile review without projectile, aim, or hitbox authority. |
| Level04 | `WMS04MuzzleCrownCogBrakeB` | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_MuzzleCrown_CogBrake_B.prefab` | `(5.10, 0.85, 21.05)` | `(0, -42, 0)` | 0.44 | Muzzle form review while explicitly quarantining firing and damage systems. |
| Level05 | `WMS04PressureTankTwinUnderbarrelB` | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_PressureTank_TwinUnderbarrel_B.prefab` | `(-5.08, 0.58, 23.30)` | `(0, 86, 0)` | 0.38 | Pressure-tank silhouette and glass material review in late-level lighting. |
| Level05 | `WMS04GlovedHandRightGripA` | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_GlovedHandSilhouette_RightGrip_A.prefab` | `(-4.62, 0.68, 24.15)` | `(0, 118, 0)` | 0.36 | Future viewmodel hand-composition risk review; must remain unrigged and visual-only. |

## Suggested `V0LevelValidator` Required Names

Add these only after placements are finalized:

- Level01: `WMS04PressurePistolCoilTripleAmberA`, `WMS04GaugeClusterTripleIvoryA`.
- Level02: `WMS04GripAssemblyWalnutLeatherA`, `WMS04ReceiverPlateBrassLatticeA`.
- Level03: `WMS04AmmoCylinderEightCellB`, `WMS04ScattergunPressureChamberQuadB`.
- Level04: `WMS04BoltThrowerRailChargedSlideB`, `WMS04MuzzleCrownCogBrakeB`.
- Level05: `WMS04PressureTankTwinUnderbarrelB`, `WMS04GlovedHandRightGripA`.

Raise renderer thresholds only after Unity confirms the showcase roots render correctly and remain readable from player-height camera views.
