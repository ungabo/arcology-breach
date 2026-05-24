# Weapons Props Manifest

Generated: 2026-05-23 22:00:01 -04:00

Scope: staged production output for `Assets/_Project/ArtStaging/WeaponsProps/` and `Documentation/AssetProduction/WeaponsProps/` only. No scenes, gameplay scripts, README, or root project docs are modified by this batch.

Unity import notes:

- Import OBJ files at scale factor 1.0. Unity unit scale is meters.
- The shared `.mtl` file provides intentional material slots for brass, iron, walnut/leather, copper, gauge enamel, pressure glass, warning red, and socket markers.
- Magenta `SOCKET_*` geometry is visible locator geometry. During prefab integration, recreate those as empty child transforms or sockets, then hide/delete the marker mesh.
- Viewmodel meshes are proportion and silhouette blockouts, not final collision meshes.

## Shared Material Slots

- `MAT_Brass_Worn`: Worn aged brass, intended metallic 1.0 roughness 0.42.
- `MAT_Brass_PolishedEdge`: Bright brass edge/collar material, intended metallic 1.0 roughness 0.30.
- `MAT_BlackenedIron`: Blackened iron and soot-dark frame, intended metallic 0.85 roughness 0.60.
- `MAT_HeatStainedSteel`: Heat-stained steel for barrels and slug bodies.
- `MAT_CopperPipe_Aged`: Aged copper pipe/tube material.
- `MAT_WalnutLeather_Dark`: Dark walnut or leather grip material.
- `MAT_GaugeFace_Cream`: Cream enamel gauge or label face.
- `MAT_Glass_PressureAmber`: Amber pressure glass; author translucent/emissive variant in Unity if desired.
- `MAT_RedValve_Warning`: Red valve/warning accent.
- `MAT_SocketMarker_Magenta`: Temporary visible socket locator geometry. Hide/delete after anchors are recreated.

## WPN_PressurePistol_Viewmodel_Blockout

- Title: Pressure Pistol Hero Viewmodel Blockout
- Category: Weapon / first-person viewmodel
- Mesh: `Assets/_Project/ArtStaging/WeaponsProps/WPN_PressurePistol_Viewmodel_Blockout.obj`
- Preview: `Documentation/AssetProduction/WeaponsProps/PREVIEW_WPN_PressurePistol_Viewmodel_Blockout.png`
- Approx dimensions: Approx 0.22 m W x 0.33 m H x 0.82 m L including socket markers; readable compact sidearm silhouette.
- Computed bounds: min [-0.1429, -0.1485, 0.0938] max [0.1507, 0.2575, 0.7956] size [0.2935, 0.406, 0.7019] meters
- Pivot/orientation: Origin near dominant hand grip center; +Z points down barrel, +Y up.
- Mesh budget now: 1404 vertices, 2596 triangles
- Material slots: `MAT_BlackenedIron`, `MAT_Brass_PolishedEdge`, `MAT_Brass_Worn`, `MAT_CopperPipe_Aged`, `MAT_GaugeFace_Cream`, `MAT_Glass_PressureAmber`, `MAT_RedValve_Warning`, `MAT_SocketMarker_Magenta`, `MAT_WalnutLeather_Dark`

Import notes:
- Import OBJ at scale factor 1.0; 1 Unity unit equals 1 meter.
- Use mesh as a viewmodel LOD0 blockout/proportion target, not gameplay collision.
- Material slots intentionally separate brass, blackened iron, walnut/leather, copper, gauge face, amber glass, and socket markers.

Socket notes:

| Socket | Local position, meters | Purpose |
| --- | --- | --- |
| `SOCKET_Muzzle` | `[0.0, 0.205, 0.77]` | Muzzle flash, projectile origin, primary and burst VFX. |
| `SOCKET_Grip_Dominant` | `[0.0, -0.03, 0.16]` | Right-hand grip pose; one-hand VR dominant grip. |
| `SOCKET_PressureChamber` | `[0.0, 0.095, 0.43]` | Underbarrel chamber highlight and alternate fire pressure dump. |
| `SOCKET_Valve` | `[0.125, 0.155, 0.42]` | Side valve twist for reload/check and empty click. |
| `SOCKET_Vent_Right` | `[0.095, 0.205, 0.31]` | Steam vent puff during alternate pressure burst. |
| `SOCKET_Trigger` | `[0.0, 0.07, 0.25]` | Trigger motion anchor. |

Animation/moving-part notes:
- Receiver and barrel recoil as one assembly: kick back -Z 0.035 m and pitch up 4-6 degrees.
- Underbarrel pressure chamber pulses/glows for alternate fire; vent from right side socket.
- Side valve rotates around local X for check/reload and empty click.
- Gauge needle can tremble during idle and snap upward during pressure burst.
- Trigger rotates slightly backward; grip remains stable for VR one-hand pose.

Collision/placement notes:
- Use no mesh collider for viewmodel.
- Future world pickup should use a simple box/capsule proxy around receiver and grip.

LOD/platform notes:
- LOD0 final target can keep distinct gauge/valve/tube elements.
- LOD1 should collapse rivets and socket marker geometry.
- LOD2/mobile can remove side valve spokes and copper pipe loops.

## WPN_SteamScattergun_Viewmodel_Blockout

- Title: Steam Scattergun Hero Viewmodel Blockout
- Category: Weapon / first-person viewmodel
- Mesh: `Assets/_Project/ArtStaging/WeaponsProps/WPN_SteamScattergun_Viewmodel_Blockout.obj`
- Preview: `Documentation/AssetProduction/WeaponsProps/PREVIEW_WPN_SteamScattergun_Viewmodel_Blockout.png`
- Approx dimensions: Approx 0.38 m W x 0.42 m H x 1.12 m L including socket markers; broad triple-barrel breacher silhouette.
- Computed bounds: min [-0.19, -0.1527, 0.1443] max [0.1984, 0.3185, 1.0834] size [0.3884, 0.4712, 0.9391] meters
- Pivot/orientation: Origin near rear dominant grip; +Z points down barrels, +Y up.
- Mesh budget now: 2420 vertices, 4560 triangles
- Material slots: `MAT_BlackenedIron`, `MAT_Brass_PolishedEdge`, `MAT_Brass_Worn`, `MAT_CopperPipe_Aged`, `MAT_HeatStainedSteel`, `MAT_RedValve_Warning`, `MAT_SocketMarker_Magenta`, `MAT_WalnutLeather_Dark`

Import notes:
- Import OBJ at scale factor 1.0.
- Viewmodel is intentionally oversized and asymmetric for first-person readability.
- Triple barrels, pump, shell rack, pressure chamber, valve, and socket markers are distinct OBJ groups.

Socket notes:

| Socket | Local position, meters | Purpose |
| --- | --- | --- |
| `SOCKET_Muzzle` | `[0.0, 0.235, 1.055]` | Center of triple-barrel muzzle flash; cone/slug VFX should branch per barrel. |
| `SOCKET_Grip_Dominant` | `[0.0, 0.02, 0.26]` | Right-hand grip/trigger pose. |
| `SOCKET_Grip_Support` | `[0.0, 0.055, 0.66]` | Support hand/pump pose; optional VR second hand. |
| `SOCKET_Pump` | `[0.0, 0.06, 0.67]` | Pump group slides along Z after primary fire. |
| `SOCKET_PressureChamber` | `[0.0, 0.105, 0.38]` | Rear pressure chamber lock/slug charge. |
| `SOCKET_Valve` | `[0.17, 0.19, 0.31]` | Rotating cap/valve for slug chamber lock. |
| `SOCKET_Vent_Left` | `[-0.15, 0.22, 0.46]` | Broad steam vent after blast. |

Animation/moving-part notes:
- Whole weapon bucks back -Z 0.055 m with 7-9 degree pitch on primary.
- MOVING_PumpGrip slides -Z 0.08 m then returns after primary blast.
- Right valve wheel rotates around local X before alternate slug fire.
- Rear pressure chamber coils can vibrate at idle and brighten during slug charge.
- Side shell rack can index or shake as a non-critical secondary motion.

Collision/placement notes:
- No mesh collider for viewmodel.
- Future pickup/world version can use a compound box collider: receiver, barrel cluster, pump grip.

LOD/platform notes:
- LOD0 final target should preserve the triple muzzle cluster and side shell rack.
- LOD1 can merge barrel bands and simplify valve spokes.
- LOD2/mobile can remove coil rings and shell rack cylinders while preserving broad silhouette.

## PICKUP_PressureCell_Ammo_Blockout

- Title: Pressure Pistol Ammo / Pressure Cell Pickup
- Category: Pickup / ammo
- Mesh: `Assets/_Project/ArtStaging/WeaponsProps/PICKUP_PressureCell_Ammo_Blockout.obj`
- Preview: `Documentation/AssetProduction/WeaponsProps/PREVIEW_PICKUP_PressureCell_Ammo_Blockout.png`
- Approx dimensions: Approx 0.24 m W x 0.43 m H x 0.20 m D including socket markers; small upright pressure cell.
- Computed bounds: min [-0.09, 0.02, -0.085] max [0.085, 0.4116, 0.0955] size [0.175, 0.3916, 0.1805] meters
- Pivot/orientation: Origin at base center for stable world placement.
- Mesh budget now: 938 vertices, 1772 triangles
- Material slots: `MAT_BlackenedIron`, `MAT_Brass_PolishedEdge`, `MAT_Brass_Worn`, `MAT_GaugeFace_Cream`, `MAT_Glass_PressureAmber`, `MAT_RedValve_Warning`, `MAT_SocketMarker_Magenta`

Import notes:
- Import OBJ at scale factor 1.0.
- Readable as pistol ammo at floor height; top valve and amber glass identify pressure contents.

Socket notes:

| Socket | Local position, meters | Purpose |
| --- | --- | --- |
| `SOCKET_PickupVFX` | `[0.0, 0.39, 0.0]` | Amber pickup glint/steam puff origin. |
| `SOCKET_Valve` | `[0.0, 0.35, 0.0]` | Tiny top valve spin or idle hiss. |
| `SOCKET_Collision` | `[0.0, 0.19, 0.0]` | Suggested trigger capsule center, radius 0.16 m, height 0.42 m. |

Animation/moving-part notes:
- Idle pickup can bob 0.03 m and rotate slowly around Y.
- Top valve can spin briefly on pickup.
- Amber tube can pulse with ammo highlight material.

Collision/placement notes:
- Use a trigger capsule: radius 0.16 m, height 0.42 m.
- Optional small cylinder/box visual mesh only; do not use detailed mesh as collision.

LOD/platform notes:
- No LOD needed for v0 if used sparingly.
- Mobile variant can remove rivets and top wheel spokes.

## PICKUP_ScattergunSlugCanister_Blockout

- Title: Scattergun Ammo / Slug Canister Pickup
- Category: Pickup / ammo
- Mesh: `Assets/_Project/ArtStaging/WeaponsProps/PICKUP_ScattergunSlugCanister_Blockout.obj`
- Preview: `Documentation/AssetProduction/WeaponsProps/PREVIEW_PICKUP_ScattergunSlugCanister_Blockout.png`
- Approx dimensions: Approx 0.46 m W x 0.38 m H x 0.31 m D including socket markers; squat ammo canister with visible brass slugs.
- Computed bounds: min [-0.2029, 0.0, -0.11] max [0.205, 0.383, 0.122] size [0.4079, 0.383, 0.232] meters
- Pivot/orientation: Origin at bottom center for floor placement.
- Mesh budget now: 576 vertices, 1036 triangles
- Material slots: `MAT_BlackenedIron`, `MAT_Brass_PolishedEdge`, `MAT_Brass_Worn`, `MAT_CopperPipe_Aged`, `MAT_GaugeFace_Cream`, `MAT_HeatStainedSteel`, `MAT_RedValve_Warning`, `MAT_SocketMarker_Magenta`, `MAT_WalnutLeather_Dark`

Import notes:
- Import OBJ at scale factor 1.0.
- Designed as a world pickup silhouette, not a viewmodel part.
- Slug tubes are intentionally separated as material-readable brass rounds.

Socket notes:

| Socket | Local position, meters | Purpose |
| --- | --- | --- |
| `SOCKET_PickupVFX` | `[0.0, 0.36, 0.0]` | Pickup glint and slug ammo steam puff. |
| `SOCKET_LidHinge` | `[-0.18, 0.24, -0.07]` | Optional lid flip/inspect animation. |
| `SOCKET_Collision` | `[0.0, 0.16, 0.0]` | Suggested trigger box 0.46 m W x 0.38 m H x 0.32 m D. |

Animation/moving-part notes:
- Canister can rotate/bob as a pickup.
- Lid hinge can pop open for pickup feedback.
- Visible slug tubes can glow or flash on pickup.

Collision/placement notes:
- Use a trigger box 0.46 m W x 0.38 m H x 0.32 m D.
- Optional simple rigidbody collider can be a box around rack base only if used as set dressing.

LOD/platform notes:
- LOD0 keeps visible slug tubes.
- LOD1 can merge rack and remove handle bolts.
- LOD2/mobile can replace individual slugs with a single brass insert block.

## PROP_WallPressureStation_Blockout

- Title: Wall-Mounted Pressure Station Prop
- Category: Prop / interactable station
- Mesh: `Assets/_Project/ArtStaging/WeaponsProps/PROP_WallPressureStation_Blockout.obj`
- Preview: `Documentation/AssetProduction/WeaponsProps/PREVIEW_PROP_WallPressureStation_Blockout.png`
- Approx dimensions: Approx 0.78 m W x 1.12 m H x 0.29 m D including socket markers; wall-mounted station with gauges and valve.
- Computed bounds: min [-0.36, 0.03, -0.0297] max [0.36, 1.0905, 0.2297] size [0.72, 1.0605, 0.2594] meters
- Pivot/orientation: Origin at bottom-center of wall contact plane; back face sits near Z 0.
- Mesh budget now: 1954 vertices, 3652 triangles
- Material slots: `MAT_BlackenedIron`, `MAT_Brass_PolishedEdge`, `MAT_Brass_Worn`, `MAT_CopperPipe_Aged`, `MAT_GaugeFace_Cream`, `MAT_RedValve_Warning`, `MAT_SocketMarker_Magenta`

Import notes:
- Import OBJ at scale factor 1.0.
- Place against a wall with local +Z facing into the room.
- Use as a staged interactable prop; gameplay hook should target SOCKET_Interact later.

Socket notes:

| Socket | Local position, meters | Purpose |
| --- | --- | --- |
| `SOCKET_Interact` | `[0.0, 0.47, 0.2]` | Player interaction trace target. |
| `SOCKET_ValveWheel` | `[0.0, 0.47, 0.13]` | Valve wheel rotation anchor. |
| `SOCKET_SteamVent` | `[0.0, 1.05, 0.12]` | Steam puff/pressure release VFX. |
| `SOCKET_Hose` | `[0.28, 0.28, 0.18]` | Future hose/nozzle attach point. |
| `SOCKET_WallMount` | `[0.0, 0.56, 0.0]` | Backplate wall align point. |

Animation/moving-part notes:
- Large valve wheel rotates around local Z 100-160 degrees during use.
- Gauge needles can swing up/down during pressure station activation.
- Top vent emits short steam burst after interaction.
- Copper side pipes can pulse via material animation.

Collision/placement notes:
- Use a simple box collider around the backplate, approx 0.74 m W x 1.08 m H x 0.10 m D.
- If interactive, add a trigger/interact volume protruding 0.35 m from the valve wheel.

LOD/platform notes:
- LOD0 keeps gauges, valve spokes, cylinders, and pipe routes.
- LOD1 can flatten small rivets and simplify pipe elbows.
- LOD2/mobile can replace wheel spokes with a disk silhouette and remove needles.

## PROP_CrankLeverSwitch_Blockout

- Title: Crank Lever / Switch Prop
- Category: Prop / interactable switch
- Mesh: `Assets/_Project/ArtStaging/WeaponsProps/PROP_CrankLeverSwitch_Blockout.obj`
- Preview: `Documentation/AssetProduction/WeaponsProps/PREVIEW_PROP_CrankLeverSwitch_Blockout.png`
- Approx dimensions: Approx 0.38 m W x 0.55 m H x 0.22 m D including socket markers; compact crank switch silhouette.
- Computed bounds: min [-0.155, 0.0, -0.025] max [0.19, 0.5, 0.185] size [0.345, 0.5, 0.2099] meters
- Pivot/orientation: Origin at bottom-center of wall contact plane; back face sits near Z 0.
- Mesh budget now: 560 vertices, 1000 triangles
- Material slots: `MAT_BlackenedIron`, `MAT_Brass_PolishedEdge`, `MAT_Brass_Worn`, `MAT_GaugeFace_Cream`, `MAT_RedValve_Warning`, `MAT_SocketMarker_Magenta`, `MAT_WalnutLeather_Dark`

Import notes:
- Import OBJ at scale factor 1.0.
- Place on wall panels, machinery, or console fronts with +Z facing the player.
- MOVING_LeverArm and MOVING_WoodHandle groups should become separate child meshes for animation if integrated.

Socket notes:

| Socket | Local position, meters | Purpose |
| --- | --- | --- |
| `SOCKET_Interact` | `[0.07, 0.18, 0.16]` | Player interaction trace target near handle. |
| `SOCKET_LeverPivot` | `[0.0, 0.32, 0.09]` | Lever/crank rotates around local Z from idle to pulled state. |
| `SOCKET_Handle` | `[0.13, 0.07, 0.13]` | Hand/contact point for pull animation. |
| `SOCKET_WallMount` | `[0.0, 0.25, 0.0]` | Wall/console align point. |

Animation/moving-part notes:
- Lever rotates around SOCKET_LeverPivot/local Z by roughly 70 degrees.
- Wood handle can counter-rotate around its own X axis while lever moves.
- Ratchet teeth can click via audio/VFX at start, midpoint, and end stops.

Collision/placement notes:
- Use a small box collider on plate, approx 0.32 m W x 0.50 m H x 0.08 m D.
- Use a trigger/interact volume around handle, approx 0.20 m radius.

LOD/platform notes:
- LOD0 keeps ratchet teeth, hub, lever, handle, and rivets.
- LOD1 can merge plate and hub details.
- LOD2/mobile can remove ratchet teeth and socket marker geometry.
