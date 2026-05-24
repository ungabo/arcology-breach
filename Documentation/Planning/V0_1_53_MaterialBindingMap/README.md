# v0.1.53 Material Binding Map

Prepared for the main implementation lane. This is a docs-only binding plan; it does not import assets, generate images, edit scenes, or change gameplay authority.

## Read Inputs

- `AssetPacks/BrassworksBreach.SurfaceMaterialDetailSet08/Runtime/Metadata/SMD08_SurfaceMaterialDetailCatalog_0.1.52-p001.json`
- `Documentation/AssetProduction/V0_1_52_SurfaceMaterialDetailSet08/SMD08_AssetInventory_0.1.52-p001.md`
- `Documentation/Planning/V0_1_52_SurfaceMaterialDetailSet08ImportReadiness/`
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Editor/UnityCorridorMaterialProofRenderer.cs`

## Implementation Shape

The fastest safe v0.1.53 path is to add a Set08 material application helper beside the existing `ApplyFinalMaterialTextureSet(...)` path in `V0SceneBuilder`, then update validator coverage beside `ValidateFinalMaterialsV1()`.

Recommended helper contract:

- Source root: `AssetPacks/BrassworksBreach.SurfaceMaterialDetailSet08/Runtime/Textures`
- Texture naming: `<set08_material_id>_ALB.png`, `_NRM.png`, `_RMA.png`, `_GRM.png`
- Import settings: `ALB` sRGB default texture, `NRM` normal map, `RMA` and `GRM` linear default textures, mipmaps on, wrap repeat.
- Material wiring: base map to `_MainTex` and `_BaseMap`; normal to `_BumpMap`; `RMA` to `_OcclusionMap` for v0.1.53; scalar metallic/smoothness/bump from catalog. Do not rely on `RMA` as a true metallic/smoothness map until a channel conversion or shader path exists.
- Preserve gameplay authority: material binding must not add colliders, triggers, behaviours, nav blockers, weapon stats, pickup definitions, or scene flow changes.

## P0 Binding Map

These should produce the biggest visible leap with the lowest code risk.

| Surface role | Current material/generator role | Set08 material | Fallback | Notes |
| --- | --- | --- | --- | --- |
| Primary corridor floors | `M_Greybox_OilStoneFloor` / `floorMaterial` / broad generated floors | `SMD08_MAT_WetBlackStoneSlab` | `SMD08_MAT_WetBlackStoneMortar` | Use for Level01-Level05 floor slabs. Keep tiling around `2.2-3.0` and verify it does not shimmer. |
| Oil-dark floor patches and grime plates | `M_Steam_OilDarkStone` / `oilStoneMaterial` / floor patches and grime route dressing | `SMD08_MAT_BlackOilWetFloor` | `SMD08_MAT_WetBlackStoneSlab` | Use in patches, not whole levels, so navigation remains readable. |
| Primary corridor walls | `M_Greybox_SootBrickWall` / `wallMaterial` / generated blockout walls | `SMD08_MAT_ChippedBlackIronWallPanel` | `SMD08_MAT_WetBlackStoneMortar` | Stronger north-star read than the current soot brick. If all-iron feels too heavy, split lower walls to stone mortar later. |
| Iron ribs, frames, grates, backplates | `M_Steam_RivetedIron` / `rivetedIronMaterial` and `ironMaterial` | `SMD08_MAT_RivetedBlackIronTrim` | `SMD08_MAT_ChippedBlackIronServicePlate` | This hits pressure gates, lifts, route prototypes, weapon iron parts, and utility props. |
| Pressure gate and service-door slabs | `M_Greybox_PressureGate` / `doorMaterial` | `SMD08_MAT_ChippedBlackIronServicePlate` | `SMD08_MAT_ChippedBlackIronWallPanel` | Dense service plating should make the pressure gate read like an engineered object instead of a flat block. |
| Brass trim, door frames, weapon bands | `M_Greybox_BrassTrim` / `gunTrimMaterial` | `SMD08_MAT_RivetedBrassTrim` | `SMD08_MAT_WornBrassValveBody` | Immediate pressure-pistol, gate, lift, and trim lift. |
| Pipes, guide rails, route strips | `M_Greybox_BrassGuide` / `brassGuideMaterial` | `SMD08_MAT_WornBrassPipe` | `SMD08_MAT_RivetedBrassTrim` | This replaces clean yellow guide brass with tarnished pressure hardware. |
| Brass hazard fittings and valve bodies | `M_Steam_BrassHazard` / `brassHazardMaterial` | `SMD08_MAT_WornBrassValveBody` | `SMD08_MAT_WornBrassPipe` | Good for valves, caps, call boxes, and pressure fittings. |
| Red hazard language | `M_Greybox_PressureWarning` / `pressureWarningMaterial` | `SMD08_MAT_RedPressureEnamel` | `SMD08_MAT_ChippedRedEnamelEdge` | Use for readable pressure affordances. Avoid broad walls. |
| Gauge faces | `M_Steam_CreamGaugeFace` / `gaugeFaceMaterial` | `SMD08_MAT_GaugeFaceEnamel` | `SMD08_MAT_BakedIvoryGaugeFace` | Critical for the pressure pistol, door gauges, valve gauges, lift gauges, and route dressing. |
| Amber glass and small status windows | `M_Steam_FrostedGlassVial` / `glassVialMaterial` | `SMD08_MAT_AmberGaslightGlass` | Current FinalMaterialsV1 `AmberGlass` | If transparency gets unstable, keep it opaque/tinted for v0.1.53 and defer refraction. |

## P1 Binding Map

These are useful, but they either need a new generated role or a shader/decal decision.

| Surface role | Current material/generator role | Set08 material | Fallback | Notes |
| --- | --- | --- | --- | --- |
| Pressure pistol copper coil detail | `CreatePressureCoilPrototype(...)` currently shares `M_Greybox_BrassTrim` | `SMD08_MAT_OxidizedCopperCoil` | `SMD08_MAT_WornBrassPipe` | Add a separate generated material role if possible so the coil stops reading as generic brass. |
| Copper runoff on old pipe backs | future split from `M_Greybox_BrassGuide` or route dressing brass | `SMD08_MAT_OxidizedCopperRunoff` | `SMD08_MAT_OxidizedCopperCoil` | Use sparingly because the green can become loud. |
| Furnace doors, hot boiler plates, muzzle shields | future furnace-specific split from `M_Steam_RivetedIron` | `SMD08_MAT_ScorchedFurnaceMetal` | `SMD08_MAT_HeatTintedBoilerIron` | Valuable for Level04 and weapon muzzle identity. Avoid applying to all iron. |
| Quieter hot metal brackets | future hot-bracket split from `M_Steam_RivetedIron` | `SMD08_MAT_HeatTintedBoilerIron` | `SMD08_MAT_RivetedBlackIronTrim` | Good bridge material where scorched furnace metal is too loud. |
| Gauge cover glass | future split from `M_Steam_FrostedGlassVial` | `SMD08_MAT_SmokedAmberGaugeGlass` | `SMD08_MAT_AmberGaslightGlass` | Candidate only until transparency choice is stable. |
| Wall soot and vent residue overlays | future decal/overlay generated role | `SMD08_MAT_SootDepositOverlay` | `SMD08_MAT_BlackOilWetFloor` as opaque grime patch | Do not bind as a broad opaque wall material. Needs alpha/decal path. |
| Vertical grime under pipes | future decal/overlay generated role | `SMD08_MAT_VerticalGrimeStreakOverlay` | `SMD08_MAT_BlackOilWetFloor` as opaque grime patch | Use after transparent/decal workflow exists. |
| Rubber gaskets and pressure seals | future gasket split from route dressing or valves | `SMD08_MAT_CrackedBlackRubberGasket` | current dark iron | Placeholder quality; only use where a dark gasket is better than no gasket. |

## Risks And Fallback Choices

- Shared material blast radius: `M_Steam_RivetedIron` and `M_Greybox_BrassTrim` touch many objects. If a binding looks wrong in the weapon viewmodel, split a new material for that surface instead of reverting the whole map.
- `RMA` is not a ready-made Unity metallic/smoothness texture for the current helper. Use the catalog metallic/smoothness scalars first; add channel conversion later.
- Overlay materials are intentionally P1. Binding them as opaque panels will look like failed patches.
- Glass can go wrong if Standard/URP transparency settings diverge. For v0.1.53, a readable opaque amber surface is acceptable if transparent glass causes shader or sorting trouble.
- The pressure pistol grip has no Set08 wood/leather replacement. Keep `M_Greybox_WalnutGrip` on existing FinalMaterialsV1 `GreasyWalnut`.
- Set08 textures are 512px, which is good for mid/low PC budget. Avoid duplicating texture imports into another folder unless there is a package import blocker.

## Validation Hooks

- Extend `V0LevelValidator` with `ValidateSet08MaterialBindings()` and call it after `ValidateFinalMaterialsV1()`.
- For every P0 role, validate the target material exists, has `ALB`, `NRM`, and `RMA` assigned from Set08, and has no missing shader.
- Validate import settings: `ALB` sRGB on, `NRM` as normal map, `RMA` and `GRM` sRGB off, mipmaps on, repeat wrap.
- Run scene rebuild and level validation after implementation.
- Run one Windows build matrix after the full P0 batch, not after each individual binding.
- Smoke check Level01 pressure gate, Level02 pipeworks, Level03 gauges/valves, Level04 foundry hazards, Level05 regulator route, and the first-person pressure pistol.

## Implementation Order

1. Add Set08 texture constants and helper methods.
2. Bind all P0 existing material assets in `BuildV0()`.
3. Add validator coverage for all P0 bindings.
4. Rebuild generated scenes.
5. Run `Validate v0 Levels`.
6. Run one full Windows build matrix for v0.1.53.
7. Promote P1 only after the P0 result is visually sane.
