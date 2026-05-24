# Roomtest Reference Look Project

This isolated Unity project is a deterministic look-development test for the dark wet brick room reference. It is intentionally separate from the main Brassworks Breach project.

Run from `D:\__MY APPS\Unity Doom`:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe" -batchmode -quit -projectPath "D:\__MY APPS\Unity Doom\roomtest" -executeMethod RoomTestBuilder.BuildAndRenderReferenceRoom -logFile "D:\__MY APPS\Unity Doom\roomtest\roomtest_unity.log"
```

Outputs:
- `Assets/RoomTest/Textures/` procedural albedo, normal, height, occlusion, and metallic/smoothness maps.
- `Assets/RoomTest/Materials/` wall, floor, ceiling, lamp, and trim materials.
- `Assets/RoomTest/Meshes/RT_BrickUnit_Beveled.mesh` and `Assets/RoomTest/Prefabs/RT_BrickUnitObject.prefab`.
- `Assets/RoomTest/Scenes/Roomtest_BrickLighting.unity`.
- `Renders/roomtest_brick_lighting_v0.1.png`.
- `Renders/roomtest_metrics_v0.1.json`.

The first pass uses PBR materials as the primary detail carrier, with a brick object/prefab included as a component proof. The room itself uses large surfaces with high-detail texture, normal, smoothness, and ambient-occlusion maps so the look can scale without requiring thousands of individual brick meshes.

See `Documentation/ROOMTEST_STAGE_GATES.md` for the staged review sequence: texture PNGs, mapping PNGs, material block preview, full room render, then measured revision.
