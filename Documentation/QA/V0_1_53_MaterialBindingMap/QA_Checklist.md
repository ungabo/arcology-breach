# v0.1.53 Material Binding Map QA Checklist

This checklist validates the implementation of `Documentation/Planning/V0_1_53_MaterialBindingMap/material_binding_candidates_v0.1.53.json`.

## Required Acceptance Gates

- Scope gate: only material/import/validator/scene-regeneration changes are made for the material-binding slice. No gameplay stats, colliders, triggers, AI, player movement, weapon damage, pickup authority, level transitions, or build settings are changed unless separately approved by the main lane.
- P0 material role gate: every P0 `target_current_material_or_generator_role` in the JSON packet has a concrete Unity material asset or explicit generated material split.
- Texture assignment gate: every P0 binding assigns Set08 `ALB`, `NRM`, and `RMA` textures to the active Unity material. `GRM` may be imported and tracked even if no shader consumes it yet.
- Import settings gate: `ALB` is sRGB, `NRM` is imported as a normal map, `RMA` and `GRM` are linear, mipmaps are enabled, wrap mode is repeat, and max texture size is not increased above the source need.
- Shader gate: no material renders magenta, has a missing shader, or loses its base texture after scene rebuild.
- Validator gate: `V0LevelValidator` has a Set08 binding validator equivalent to the current FinalMaterialsV1 binding validator.
- Scene rebuild gate: generated scenes rebuild successfully after the P0 batch.
- Level validation gate: `Project Tools/Validate v0 Levels` passes after scene rebuild.
- Runtime smoke gate: the next Windows smoke/build matrix passes after the full P0 batch.
- No unintended gameplay authority gate: all Set08-bound route dressing and prop surfaces remain visual-only unless they already had gameplay authority before the binding.
- Windows mid/low PC budget gate: the P0 import uses shared material assets, mipmapped 512px Set08 maps, and does not duplicate texture families into a second runtime folder.
- Visual sanity gate: Level01 pressure gate, Level02 pipeworks corridor, Level03 gauges/valves, Level04 foundry hazard area, Level05 regulator route, and the pressure-pistol viewmodel all show visible steampunk material improvement without becoming too dark to navigate.

## P0 Spot Checks

- `M_Greybox_OilStoneFloor`: base texture path contains `SMD08_MAT_WetBlackStoneSlab_ALB.png`.
- `M_Steam_OilDarkStone`: base texture path contains `SMD08_MAT_BlackOilWetFloor_ALB.png`.
- `M_Greybox_SootBrickWall`: base texture path contains `SMD08_MAT_ChippedBlackIronWallPanel_ALB.png`.
- `M_Steam_RivetedIron`: base texture path contains `SMD08_MAT_RivetedBlackIronTrim_ALB.png`.
- `M_Greybox_PressureGate`: base texture path contains `SMD08_MAT_ChippedBlackIronServicePlate_ALB.png`.
- `M_Greybox_BrassTrim`: base texture path contains `SMD08_MAT_RivetedBrassTrim_ALB.png`.
- `M_Greybox_BrassGuide`: base texture path contains `SMD08_MAT_WornBrassPipe_ALB.png`.
- `M_Steam_BrassHazard`: base texture path contains `SMD08_MAT_WornBrassValveBody_ALB.png`.
- `M_Greybox_PressureWarning`: base texture path contains `SMD08_MAT_RedPressureEnamel_ALB.png`.
- `M_Steam_CreamGaugeFace`: base texture path contains `SMD08_MAT_GaugeFaceEnamel_ALB.png`.
- `M_Steam_FrostedGlassVial`: base texture path contains `SMD08_MAT_AmberGaslightGlass_ALB.png`.

## P1 Promotion Gates

- Copper coil split: only promote `SMD08_MAT_OxidizedCopperCoil` after the coil has its own material role or the shared brass role is visually acceptable on all affected objects.
- Furnace metal split: only promote `SMD08_MAT_ScorchedFurnaceMetal` or `SMD08_MAT_HeatTintedBoilerIron` after there is a furnace-specific material role.
- Smoked gauge glass: only promote after transparent/opaque shader choice is tested on pistol, valve, lift, and gate gauges.
- Soot/grime overlays: only promote after a transparent/decal path exists. Opaque broad binding fails QA.
- Rubber gasket: only promote where placeholder quality is acceptable and does not become a focal material.

## Failure Handling

- If a P0 material is too visually noisy, keep the role and switch to the listed fallback material.
- If a shader path breaks, keep Set08 textures imported but temporarily revert the affected material role to the previous FinalMaterialsV1 family.
- If route readability drops, reduce broad oily/wet materials and keep red hazard enamel on danger affordances only.
- If PC budget worsens, confirm texture duplication did not occur and reduce active P1 materials before touching P0.
