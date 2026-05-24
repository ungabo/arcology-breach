# Brassworks Breach - v0.1.35 Level Module + Setpiece Staging Package

Created: `2026-05-24`

## Scope

This package stages a large Unity-only environment module batch for a `v0.1.35` or `v0.1.36` integration leap. It covers corridor, pressure door, vault door, pipe gallery, furnace alcove, catwalk rail, floor/wall/ceiling trim, lighting fixtures, and grouped setpiece dressing across all five current levels.

Owned write scope:

- `Assets/_Project/ArtStaging/V0_1_35_LevelModuleSetpieces/`
- `Documentation/AssetProduction/V0_1_35_LevelModuleSetpieces/`
- `Documentation/ConceptRenders/V0_1_35_LevelModuleSetpieces/`
- `Documentation/Planning/V0_1_35_LevelSetpiecePlacements/`

No Blender or external DCC was used. Prefabs and preview sheets were generated in Unity batchmode via:

`Assets/_Project/ArtStaging/V0_1_35_LevelModuleSetpieces/Editor/V0135LevelModuleSetpieceGenerator.cs`

## Generated Unity Assets

| Family | Staged prefab | Role |
| --- | --- | --- |
| Corridor kit | `Prefabs/SM_V0135_CorridorBay_4x6m.prefab` | 4m x 6m corridor shell with soot brick, riveted panels, ceiling pipe channels, and floor slab. |
| Door/vault kit | `Prefabs/SM_V0135_PressureDoor_Frame_4m.prefab` | Pressure-door jamb/lintel frame with pistons, gauge, gasket sill, amber state lens. |
| Door/vault kit | `Prefabs/SM_V0135_VaultDoor_Round_5m.prefab` | Hero round vault face with brass ring, spoke lock, red locked tab, floor anchors. |
| Pipe/valve kit | `Prefabs/SM_V0135_PipeGallery_WallRun_6m.prefab` | Wall-mounted pipe gallery with clamps, valve wheel, gauge, green restored tab. |
| Furnace/boiler alcove | `Prefabs/SM_V0135_FurnaceBoiler_Alcove_5m.prefab` | Soot-brick furnace bay with boiler tanks, glow mouth, warning band, coal lip. |
| Railing/catwalk kit | `Prefabs/SM_V0135_CatwalkRail_4m.prefab` | Brass/copper rail with posts, kick plate, foot plates, amber route cap. |
| Trim kit | `Prefabs/SM_V0135_TrimPack_FloorWallCeiling_4m.prefab` | Floor strip, waist rail, gasket strip, ceiling trim, rivet rows. |
| Lighting fixtures | `Prefabs/SM_V0135_CagedGaslight_WallFixture.prefab` | Caged amber wall lamp with feed pipe and preview point light. |
| Setpiece dressing | `Prefabs/SM_V0135_SetpieceDressing_Group_AllLevels.prefab` | Grouped sampler for Level01 through Level05 placement language. |

## Materials

The batch includes 12 Unity materials:

- `M_V0135_BlackenedRivetedIron`
- `M_V0135_AgedBrass`
- `M_V0135_CopperPipe`
- `M_V0135_OilDarkStone`
- `M_V0135_SootBrick`
- `M_V0135_WarmAmberGlass`
- `M_V0135_FurnaceGlow`
- `M_V0135_RouteGreenGlass`
- `M_V0135_WarningRedEnamel`
- `M_V0135_RubberGasket`
- `M_V0135_CreamGaugeFace`
- `M_V0135_HeatBlueSteel`

These track the approved steampunk north-star: brass, copper, soot, riveted iron, pressure gauges, valve wheels, steam/heat cues, furnace glow, oil-dark stone, and amber gaslight.

## Preview Outputs

Unity-rendered proof sheets live outside game-build asset scope:

- `Documentation/ConceptRenders/V0_1_35_LevelModuleSetpieces/CONTACTSHEET_V0135_LevelModuleSetpieces_UnityProof.png`
- `Documentation/ConceptRenders/V0_1_35_LevelModuleSetpieces/RENDER_V0135_CorridorDoorPipeGallery_UnityProof.png`
- `Documentation/ConceptRenders/V0_1_35_LevelModuleSetpieces/RENDER_V0135_FurnaceCatwalkTrim_UnityProof.png`

Copies also exist under the staging package `Previews/` folder for quick Unity browsing.

## Integration Intent

Use this batch as a visual and modular proxy kit. It is not a scene placement pass and does not own gameplay authority. Recommended integration order:

1. Level03 Bellows/Scattergun and Level04 Furnace/Bulwark beats first, because they show the biggest visual leap.
2. Level05 Warden/Core Ring second, because the finale benefits from high-wall machinery and clearer final hoist state.
3. Level02 Valve/Lancer bridge and Level01 Pressure Gate/tutorial language after the midgame and finale are stable.

Keep all pieces visual-only until each placement passes first-person route smoke checks.
