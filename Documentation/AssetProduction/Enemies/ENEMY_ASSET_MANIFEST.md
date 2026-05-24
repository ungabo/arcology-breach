# Enemy Asset Manifest

Timestamp: 2026-05-23 21:57 -04:00

Worker: D

Scope: first production-staged enemy blockout OBJs for Brassworks Breach. These are Unity-friendly mesh files for silhouette, scale, rig planning, collider planning, and material direction. They are not final skinned meshes.

## Unity Import Contract

- Units: `1 Unity unit = 1 meter`.
- Axis: `+Y` up, `+Z` forward.
- OBJ material source: `Assets/_Project/ArtStaging/Enemies/ENEMY_BlockoutMaterials.mtl`.
- Recommended import settings: import materials from MTL for preview, calculate normals if needed, disable mesh colliders by default, and keep transforms at identity after import.
- Performance intent: readable low/mid PC silhouettes with low material count, simple cylinders, and detachable shared parts for later atlas/material consolidation.

## Asset Index

| Asset | Mesh | Preview | Size (m) | Est. Tris | Role |
| --- | --- | --- | --- | ---: | --- |
| `ENEMY_ScrapperAutomaton_Blockout_LOD0` | `Assets/_Project/ArtStaging/Enemies/ENEMY_ScrapperAutomaton_Blockout_LOD0.obj` | `Documentation/AssetProduction/Enemies/PREVIEW_ENEMY_ScrapperAutomaton_Blockout_LOD0.png` | 1.44 x 1.49 x 1.21 | 2068 | Compact melee worker-machine with hunched boiler torso, oversized claw arms, and broad piston feet. |
| `ENEMY_LancerAutomaton_Blockout_LOD0` | `Assets/_Project/ArtStaging/Enemies/ENEMY_LancerAutomaton_Blockout_LOD0.obj` | `Documentation/AssetProduction/Enemies/PREVIEW_ENEMY_LancerAutomaton_Blockout_LOD0.png` | 0.73 x 1.93 x 2.17 | 1926 | Tall ranged automaton with narrow body, stilt piston legs, and a pressure lance/cannon extending forward. |
| `ENEMY_SentinelTurret_Blockout_LOD0` | `Assets/_Project/ArtStaging/Enemies/ENEMY_SentinelTurret_Blockout_LOD0.obj` | `Documentation/AssetProduction/Enemies/PREVIEW_ENEMY_SentinelTurret_Blockout_LOD0.png` | 1.02 x 1.07 x 1.58 | 1148 | Static sentinel turret with floor, wall, and ceiling mounting cues in one universal blockout. |
| `PART_CogShoulderJoint_Blockout` | `Assets/_Project/ArtStaging/Enemies/PART_CogShoulderJoint_Blockout.obj` | `Documentation/AssetProduction/Enemies/PREVIEW_PART_CogShoulderJoint_Blockout.png` | 0.24 x 0.80 x 0.80 | 484 | Reusable cog shoulder or hip joint with readable gear teeth and rivet rhythm. |
| `PART_PistonLeg_Blockout` | `Assets/_Project/ArtStaging/Enemies/PART_PistonLeg_Blockout.obj` | `Documentation/AssetProduction/Enemies/PREVIEW_PART_PistonLeg_Blockout.png` | 0.28 x 0.90 x 0.37 | 160 | Reusable piston leg assembly with hip mount, brass knee socket, telescoping lower rod, and broad metal foot. |
| `PART_BrassMaskVisor_Blockout` | `Assets/_Project/ArtStaging/Enemies/PART_BrassMaskVisor_Blockout.obj` | `Documentation/AssetProduction/Enemies/PREVIEW_PART_BrassMaskVisor_Blockout.png` | 0.71 x 0.42 x 0.21 | 96 | Reusable enemy face plate with cream enamel mask, brass cheek hinges, snout filter, and dark visor slit. |
| `PART_FurnaceCore_Blockout` | `Assets/_Project/ArtStaging/Enemies/PART_FurnaceCore_Blockout.obj` | `Documentation/AssetProduction/Enemies/PREVIEW_PART_FurnaceCore_Blockout.png` | 0.60 x 0.59 x 0.34 | 308 | Reusable glowing amber furnace core/weak-point insert with brass retaining ring. |
| `PART_PipeBackpack_Blockout` | `Assets/_Project/ArtStaging/Enemies/PART_PipeBackpack_Blockout.obj` | `Documentation/AssetProduction/Enemies/PREVIEW_PART_PipeBackpack_Blockout.png` | 0.84 x 0.82 x 0.46 | 228 | Reusable boiler backpack with tank, paired vertical pipes, exhaust bends, and mount collar. |

## Scale, Pivot, Collider, LOD, Rig, And Animation Notes

### ENEMY_ScrapperAutomaton_Blockout_LOD0

- Role: Compact melee worker-machine with hunched boiler torso, oversized claw arms, and broad piston feet.
- Pivot/origin: Origin at ground footprint center. Keep +Y up, +Z forward; place melee hit sockets at claw palms.
- Collider suggestion: Primary capsule 0.85m diameter x 1.55m height, centered near Y 0.78. Add optional trigger boxes on claw arcs during attack frames.
- LOD notes: LOD1 can remove rivets, shoulder gear teeth, and backpack bends. LOD2 should collapse claws to simple mitts and replace cylinders with six-sided rods.
- Rig/skeleton requirements: Root, pelvis, boiler/spine, head visor, left/right cog shoulder, elbow, wrist/claw, hip, knee, ankle, backpack exhaust controls.
- Animation requirements: Idle pressure bob, scuttle walk, claw windup, double swipe, stagger, shutdown with furnace core dim.

### ENEMY_LancerAutomaton_Blockout_LOD0

- Role: Tall ranged automaton with narrow body, stilt piston legs, and a pressure lance/cannon extending forward.
- Pivot/origin: Origin at ground footprint center. Barrel points +Z; muzzle socket should be placed at lance tip.
- Collider suggestion: Primary capsule 0.70m diameter x 2.05m height. Use separate narrow raycast origin/muzzle transform for projectile or hitscan.
- LOD notes: LOD1 can reduce lance supports and backpack pipes. LOD2 keeps only torso, head, legs, and one barrel cylinder for combat readability.
- Rig/skeleton requirements: Root, hips, torso, neck/head, shoulder braces, lance aim/recoil bone, hip/knee/ankle pistons, backpack hose controls.
- Animation requirements: Idle aiming sway, brace legs, charge pressure pulse, fire recoil, vent steam, stagger, collapse.

### ENEMY_SentinelTurret_Blockout_LOD0

- Role: Static sentinel turret with floor, wall, and ceiling mounting cues in one universal blockout.
- Pivot/origin: Origin at floor mount center for default placement. For wall/ceiling variants, duplicate mesh later and move pivot to mount plate center.
- Collider suggestion: Use a cylinder or box for base/housing and a short non-blocking barrel trigger. Avoid mesh collision.
- LOD notes: LOD1 removes gear teeth and side drums. LOD2 is a box housing plus single barrel; static batching is recommended.
- Rig/skeleton requirements: No full character rig required. Use separate yaw base and pitch barrel transforms; optional recoil child for muzzle.
- Animation requirements: Search sweep, target acquire snap, fire recoil, overheat vent, destroyed droop.

### PART_CogShoulderJoint_Blockout

- Role: Reusable cog shoulder or hip joint with readable gear teeth and rivet rhythm.
- Pivot/origin: Origin centered on axle. X axis runs through socket for shoulder placement.
- Collider suggestion: No gameplay collider; visual only unless used as a weak-point hit target.
- LOD notes: Bake gear teeth to normal map or collapse to 10-sided cylinder for LOD2.
- Rig/skeleton requirements: Parent under upper-arm or shoulder bone; rotate around local X.
- Animation requirements: Can counter-rotate subtly during idle/walk to sell clockwork motion.

### PART_PistonLeg_Blockout

- Role: Reusable piston leg assembly with hip mount, brass knee socket, telescoping lower rod, and broad metal foot.
- Pivot/origin: Origin remains at floor center for preview. For production rigging, pivot hip block at upper socket.
- Collider suggestion: Character capsule handles collision; optional footstep contact box only for VFX/audio timing.
- LOD notes: Remove knee sphere rivets and simplify lower rod to box at distance.
- Rig/skeleton requirements: Hip, knee, ankle, and piston stretch helper bones.
- Animation requirements: Use IK for planted foot and piston compression on step impact.

### PART_BrassMaskVisor_Blockout

- Role: Reusable enemy face plate with cream enamel mask, brass cheek hinges, snout filter, and dark visor slit.
- Pivot/origin: Origin is under the mask for preview. Production pivot should sit at neck/head joint center.
- Collider suggestion: Head hitbox can be simple box matching mask silhouette.
- LOD notes: At LOD2, keep the visor slit as dark/emissive material strip and merge hinges into the mask body.
- Rig/skeleton requirements: Parent to head bone; optional jaw/filter bob during venting.
- Animation requirements: Small visor pulse or filter shake during alert/firing states.

### PART_FurnaceCore_Blockout

- Role: Reusable glowing amber furnace core/weak-point insert with brass retaining ring.
- Pivot/origin: Origin at core center. Forward-facing ring is +Z.
- Collider suggestion: Optional sphere trigger 0.45m diameter for weak-point tests.
- LOD notes: LOD1 can become flat emissive disc; LOD2 can bake glow into torso material.
- Rig/skeleton requirements: Usually parented to torso; scale pulse helper optional.
- Animation requirements: Low-amplitude glow pulse, damage flare, shutdown dim.

### PART_PipeBackpack_Blockout

- Role: Reusable boiler backpack with tank, paired vertical pipes, exhaust bends, and mount collar.
- Pivot/origin: Origin centered on tank. For production, pivot at spine mount plate.
- Collider suggestion: Visual only; use character capsule unless backpack is intended as targetable silhouette.
- LOD notes: LOD1 removes pipe bends; LOD2 merges tank and pipes into a single low-sided hull.
- Rig/skeleton requirements: Parent to spine/torso. Exhaust outlets need sockets for steam VFX.
- Animation requirements: Idle vent puffs, overheat shake, burst steam on death.

## Shared Material Proxy Notes

- `MAT_ENEMY_AgedBrass`: warm brass for readable steampunk highlights and joints.
- `MAT_ENEMY_BlackenedIron`: soot-dark chassis and armor mass.
- `MAT_ENEMY_CopperPipe`: pipes, pressure rods, lance barrel accents.
- `MAT_ENEMY_DarkRubber`: hoses/gaskets.
- `MAT_ENEMY_CreamEnamel`: mask plates; later supports chipped enamel texture detail.
- `MAT_ENEMY_FurnaceGlowAmber`: emissive furnace eyes/cores; keep bloom controlled so silhouettes remain readable.
- `MAT_ENEMY_SootShadow`: visor slit and deep interior cavities.

## Contact Sheet

- `Documentation/AssetProduction/Enemies/PREVIEW_EnemyBlockoutContactSheet.png`

## Production Follow-Up

- Convert accepted blockouts to skinned FBX for Scrapper and Lancer after gameplay scale approval.
- Split Sentinel into floor, wall, and ceiling prefab variants once placement rules are final.
- Bake high-poly bevel/rivet detail into atlas materials for LOD1/LOD2 instead of carrying every small shape as geometry.