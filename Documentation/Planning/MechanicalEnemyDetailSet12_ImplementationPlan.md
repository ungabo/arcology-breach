# Mechanical Enemy Detail Set 12 Implementation Plan

Scope: isolated sidecar package only. No playable scenes, shared status docs, main package manifest, gameplay code, or worker ledgers are touched.

## Asset Strategy

- Use Unity-generated meshes and procedural materials only, with no Blender or external DCC dependency.
- Prioritize brass/iron/copper material separation, layered riveted armor, glowing amber optics, pressure gauges, flywheels, hoses, pistons, and sharp tool silhouettes.
- Keep all prefabs visual-only with named SOCKET transforms for future rigging and integration.

## Deliverables

- `Runtime/Prefabs/MED12_RivetedTorsoPlateCluster.prefab`: Layered brass and dark iron torso armor plate cluster with rivet fields, soot streaks, asymmetric repairs, and readable FPS-distance panel breaks.
- `Runtime/Prefabs/MED12_GlowingEyeHeadModule.prefab`: Goggled furnace-eye head module with brass lens collars, amber glass, vertical jaw grille, cheek rivets, and hose ports.
- `Runtime/Prefabs/MED12_FlywheelShoulderAssembly.prefab`: Offset flywheel shoulder rig with iron carrier bracket, brass gear teeth, heavy hub, and piston anchor sockets.
- `Runtime/Prefabs/MED12_PistonForearm.prefab`: Black iron forearm with dual exposed pistons, brass cuffs, copper oiler tubes, rubber return hose, and tool wrist socket.
- `Runtime/Prefabs/MED12_SawClawToolArmAttachment.prefab`: Threat-forward wrist attachment with heat-scarred saw blade, three hooked claw tines, brass guard cage, and oiled bearing hub.
- `Runtime/Prefabs/MED12_PressureGaugeChestModule.prefab`: Oversized chest pressure gauge with brass bezel, ivory face, red needle, glass cap, tick geometry, and copper feeder lines.
- `Runtime/Prefabs/MED12_CableHoseBundle.prefab`: Braided black rubber hose and copper cable bundle with brass clamps, ribbed sleeves, oil stains, and modular endpoints.
- `Runtime/Prefabs/MED12_LegJointArmorPlates.prefab`: Knee/hip armor stack with angled brass plates, iron pivot drums, piston protectors, rivets, and mud-dark oil buildup.
- `Runtime/Prefabs/MED12_AssembledBustUpperBodyConcept.prefab`: Assembled upper-body concept showing how Set 12 detail modules upgrade the mechanical sentinel with a thicker threat silhouette and richer material breakup.
