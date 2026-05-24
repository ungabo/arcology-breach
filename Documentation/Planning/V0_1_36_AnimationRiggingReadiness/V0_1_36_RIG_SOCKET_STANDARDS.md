# V0.1.36 Rig And Socket Standards

Purpose: establish a single readiness vocabulary for staged mechanical enemies, staged weapons, and future weapon silhouettes before final rigs or Animator Controllers are authored.

## Global Conventions

Use stable transform names even while meshes are still proxy-quality. Final meshes may replace visual children, but they should preserve these roots and sockets.

| Prefix | Use |
| --- | --- |
| `RIG_` | Skeleton or mechanical articulation transform. |
| `SOCK_` | Attachment, VFX, weak-point, pickup, muzzle, or gameplay-facing locator. |
| `GEO_` | Render mesh child under a rig or socket. |
| `COL_HINT_` | Documentation-only collision hint transform if staged visually later. |
| `FX_HINT_` | Documentation-only VFX timing/source hint if staged visually later. |

Recommended axes:

- Enemy forward: Unity +Z.
- Enemy up: Unity +Y.
- Enemy root pivot: floor contact center under the torso mass.
- Weapon forward: muzzle points +Z in local weapon space.
- Viewmodel weapon grip: local origin near receiver center, with separate hand sockets.
- Pickup weapon pivot: stable display/pickup center, not the first-person recoil pivot.

## Shared Enemy Root Layout

All mechanical enemies should expose:

| Transform | Purpose |
| --- | --- |
| `RIG_Root` | Floor-space root for navigation placement and root motion decisions. |
| `RIG_Hips` | Main locomotion body mass; keep broad mechanical weight stable. |
| `RIG_SpineCage_01` | Boiler/cage torso sway and hit reaction target. |
| `RIG_HeadOrMask` | Face, visor, furnace-eye, or command mask target. |
| `RIG_Arm_L`, `RIG_Arm_R` | Tool-arm roots; asymmetry should survive retargeting. |
| `RIG_Leg_L`, `RIG_Leg_R` | Main leg piston chain roots. |
| `SOCK_WeakPoint_Primary` | Main amber/red damage affordance locator. |
| `SOCK_WeakPoint_Secondary_L`, `SOCK_WeakPoint_Secondary_R` | Optional flank, shield, or boss secondary weak locators. |
| `SOCK_FurnaceEyes` | Identity/alert glow source, separate from damage weak points. |
| `SOCK_ShutdownBurst_Core` | Center of shutdown pop, crumble, or pressure vent. |
| `SOCK_Audio_MechLoop` | Looping servo/boiler source. |
| `SOCK_Audio_AttackTell` | Pre-attack tell source, usually tool or coil. |

## Enemy Family Standards

### Scrapper

Scale target: 1.55 m, low fast saw-claw silhouette.

Required readiness sockets:

- `SOCK_Tool_Saw_L` or `SOCK_Tool_Saw_R`: spinning cutter wheel source; preserve one circular cutter read.
- `SOCK_Tool_Claw_L` or `SOCK_Tool_Claw_R`: claw snap, scrape, or swipe source.
- `SOCK_WeakPoint_Primary`: centered on chest boiler, not in the face.
- `SOCK_Tank_Back_L`, `SOCK_Tank_Back_R`: pressure puff, hit shake, or cap-pop points.
- `SOCK_FootSpark_L`, `SOCK_FootSpark_R`: low scraping locomotion accents.
- `SOCK_ShutdownFragment_Mask`, `SOCK_ShutdownFragment_Tool`, `SOCK_ShutdownFragment_TankCap`.

Rig notes:

- Keep a crouched hip and forward head/mask offset.
- Tool arms may use two-bone IK, but tool disks and claws should be child transforms so procedural spin/snap can layer over authored clips.
- Avoid mirrored tool reads; the player should always distinguish saw side from claw side.

### Lancer

Scale target: 1.90 m, narrow forward lance silhouette.

Required readiness sockets:

- `SOCK_Tool_Lance_Muzzle`: projectile or beam origin; must stay on uninterrupted +Z lance line.
- `SOCK_Tool_Lance_Base`: recoil and bracing hinge.
- `SOCK_Coil_Backpack`: cyan charge source.
- `SOCK_Coil_Ring_01` through `SOCK_Coil_Ring_03`: sequential charge tell points.
- `SOCK_WeakPoint_Primary`: sternum or head lamp, separate from muzzle core.
- `SOCK_ShutdownFragment_MuzzleSleeve`, `SOCK_ShutdownFragment_Coil`.

Rig notes:

- Preserve thin-body readability; do not bulk out shoulders just to fit humanoid arm proportions.
- Lance aim should support additive upper-body yaw/pitch or procedural aim offset without moving root locomotion authority.

### Bulwark

Scale target: 2.15 m, broad shield-wall silhouette.

Required readiness sockets:

- `SOCK_Shield_Face`: frontal denial surface and impact spark source.
- `SOCK_Shield_Hinge_L`, `SOCK_Shield_Hinge_R`: shield opening, stagger, or break-timing pivots.
- `SOCK_Tool_Hammer`: heavy slam source.
- `SOCK_WeakPoint_Secondary_L`, `SOCK_WeakPoint_Secondary_R`: shield/flank weak lamps.
- `SOCK_WeakPoint_Primary`: body or guard-break lamp if future combat needs a central target.
- `SOCK_SteamVent_Shoulder_L`, `SOCK_SteamVent_Shoulder_R`.
- `SOCK_ShutdownFragment_ShieldHinge`, `SOCK_ShutdownFragment_HammerFace`.

Rig notes:

- The shield can be a large child assembly, but the torso cage should remain the root mass.
- Hammer windup must be visible around or above shield mass.
- Shield recoil, stagger, and hit reactions should be additive-friendly.

### Warden

Scale target: 2.35 m, command tower with twin coils.

Required readiness sockets:

- `SOCK_Coil_Crown_L`, `SOCK_Coil_Crown_R`: cyan command/bolt tell sources.
- `SOCK_CommandLamp`: identity and alert state source.
- `SOCK_WeakPoint_Primary`: central tower lamp, not the face.
- `SOCK_Tool_Gavel`: command strike, slam, or callout source.
- `SOCK_Tool_Pincer`: grab or point-tell source.
- `SOCK_Aura_FloorCenter`: later non-gameplay visual ring or command pulse origin.
- `SOCK_ShutdownFragment_CageRib`, `SOCK_ShutdownFragment_CrownCoil`.

Rig notes:

- Preserve cage ribs and crown height so the Warden does not collapse into the Lancer read.
- Upper-body command gestures should be additive-friendly over slow locomotion and idle.

### Foundry Overseer

Scale target: 2.75 m, elite tri-tool furnace miniboss.

Required readiness sockets:

- `SOCK_Tool_Saw`: broad cutter source.
- `SOCK_Tool_Hammer`: heavy slam source.
- `SOCK_Tool_BackLance`: rear lance or overhead projectile source.
- `SOCK_Coil_Crown_01` through `SOCK_Coil_Crown_04`: boss charge/readability sequence.
- `SOCK_WeakPoint_Primary`: boss furnace lamp.
- `SOCK_WeakPoint_Secondary_CenterApron`: second readable break target.
- `SOCK_PhaseVent_L`, `SOCK_PhaseVent_R`, `SOCK_PhaseVent_Back`.
- `SOCK_ShutdownBurst_Core`, `SOCK_ShutdownBurst_Crown`, `SOCK_ShutdownBurst_Tools`.

Rig notes:

- Author as a mechanical boss rig, not a scaled humanoid. The crown, apron, back lance, saw, and hammer should each keep separate animation control.
- Plan for phase-ready additive layers even if v0.1 gameplay does not yet use phases.

## Weapon Socket Standards

### Pressure Pistol

Required sockets:

- `SOCK_Grip_Main`: right-hand grip or pickup grab origin.
- `SOCK_Grip_Support`: optional left/support hand or VR offhand proximity target.
- `SOCK_Trigger`: trigger finger reference.
- `SOCK_Muzzle`: projectile, muzzle flash, smoke, and trace origin.
- `SOCK_Muzzle_SootCard`: visual-only soot/smoke anchor.
- `SOCK_RecoilPivot`: receiver-centered recoil rotation point.
- `SOCK_Gauge_Needle`: animated gauge needle pivot.
- `SOCK_Coil_01` through `SOCK_Coil_03`: recoil coil compression or heat glow points.
- `SOCK_Reload_Cell`: pressure-cell insertion/removal locator.
- `SOCK_Pickup_Display`: world pickup/wall display origin.
- `SOCK_VR_Hand_R`, `SOCK_VR_Hand_L`: future controller alignment points.

### Steam Scattergun

Required sockets:

- `SOCK_Grip_Main`: dominant grip/trigger hand.
- `SOCK_Grip_Pump`: pump hand and reload/pump animation reference.
- `SOCK_Stock_Shoulder`: shoulder alignment and viewmodel anchoring.
- `SOCK_Muzzle_Barrel_01` through `SOCK_Muzzle_Barrel_03`: pellet/slug visual origins.
- `SOCK_Muzzle_Center`: aggregate trace/VFX origin if gameplay uses one source.
- `SOCK_RecoilPivot`: receiver or barrel-cluster recoil pivot.
- `SOCK_Gauge_Needle`: rear pressure dial pivot.
- `SOCK_Reload_Breech`: shell/slug insertion target.
- `SOCK_Tank_Lower`: steam tank pressure glow/vent source.
- `SOCK_Pickup_Display`: wall-display and pickup origin.
- `SOCK_VR_Hand_R`, `SOCK_VR_Hand_L`: future controller alignment points.

## Future Weapon Silhouette Standards

Any future silhouette, including lightning lance or other brassworks weapons, should expose this minimum set:

- `SOCK_Grip_Main`
- `SOCK_Grip_Support`
- `SOCK_Trigger`
- `SOCK_Muzzle`
- `SOCK_RecoilPivot`
- `SOCK_Reload_Primary`
- `SOCK_AmmoOrPressureCell`
- `SOCK_StatusGauge`
- `SOCK_Pickup_Display`
- `SOCK_WallMount_A`
- `SOCK_WallMount_B`
- `SOCK_VR_Hand_R`
- `SOCK_VR_Hand_L`

Do not encode gameplay behavior in socket names. Use neutral names that support current weapons, pickups, wall displays, inspection poses, and future VR hand placement.

