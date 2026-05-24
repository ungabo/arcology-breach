# Room Setpiece Kit 04 Validation Evidence

Package root: `AssetPacks/BrassworksBreach.RoomSetpieceKit04`

Generation:

- Unity editor: `6000.4.6f1`
- Batch method: `BrassworksBreach.Sidecars.RoomSetpieceKit04.RoomSetpieceKit04Generator.GenerateValidateAndQuit`
- Unity log: `AssetPacks/BrassworksBreach.RoomSetpieceKit04/ValidationProject~/unity-generation.log`
- Result markers: `RSK04_GENERATE_PASS v0.1.45`, `RSK04_PREVIEW_PASS v0.1.45`, `RSK04_UNITY_VALIDATION_PASS v0.1.45`

Counts:

- Prefabs: 30
- Materials: 18
- Reusable meshes: 10
- Preview PNGs: 12

Preview PNGs:

- `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04/RSK04_PREVIEW_all_setpieces_contact_sheet_v0.1.45.png`
- `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04/RSK04_PREVIEW_boiler_chamber_wall_bay_v0.1.45.png`
- `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04/RSK04_PREVIEW_brass_floor_trim_threshold_v0.1.45.png`
- `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04/RSK04_PREVIEW_catwalk_balcony_module_v0.1.45.png`
- `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04/RSK04_PREVIEW_furnace_control_wall_v0.1.45.png`
- `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04/RSK04_PREVIEW_large_warning_gauge_wall_v0.1.45.png`
- `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04/RSK04_PREVIEW_material_swatch_v0.1.45.png`
- `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04/RSK04_PREVIEW_pipe_gallery_ceiling_cluster_v0.1.45.png`
- `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04/RSK04_PREVIEW_pressure_vault_door_alcove_v0.1.45.png`
- `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04/RSK04_PREVIEW_regulator_core_machinery_v0.1.45.png`
- `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04/RSK04_PREVIEW_room_corner_clutter_cluster_v0.1.45.png`
- `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04/RSK04_PREVIEW_service_stair_silhouette_v0.1.45.png`

Visual-only authority check:

- Unity validation report: `Documentation/AssetProduction/V0_1_45_RoomSetpieceKit04/RSK04_RoomSetpieceKit04_UnityValidationReport_v0.1.45.json`
- Prefab text scan for `Collider`, `Rigidbody`, `AudioSource`, and `m_Script`: no matches.
- Generated prefabs contain mesh filters/renderers only, with no gameplay scripts, colliders, rigidbodies, or autonomous audio.

Sidecar validator:

```text
Sidecar asset-pack validation
Project: D:\__MY APPS\Unity Doom
Asset packs: D:\__MY APPS\Unity Doom\AssetPacks
Pattern: BrassworksBreach.RoomSetpieceKit04
Packages checked: 1
Errors: 0
Warnings: 0
```

Caveats:

- These are procedural Unity lookdev assets, not final hand-authored AAA meshes.
- They are intentionally visual-only; primary-lane integration must add collision, gameplay scale decisions, lighting balance, occlusion, and level placement.
- Generated materials use Standard shader lookdev settings and may need render-pipeline conversion during promotion.
