# SMD08 Import Readiness Notes

## Shader Compatibility

Materials are authored as Unity built-in Standard shader YAML with generated PNG references. The `RMA` masks preserve roughness/metallic/occlusion intent even though the Standard shader uses scalar smoothness unless a project shader later consumes the extra channels.

## v0.1.52 Import/Binding Verdict

PASS WITH LIMITATIONS. The core family is strong enough for v0.1.52 quarantine import and selective binding to corridor/weapon surfaces. Bind the final-candidate materials first: wet black stone, chipped black iron, worn brass pipe, oxidized copper coil, red pressure enamel, amber glass, black oil floor, scorched furnace metal, gauge enamel, and riveted trim. Hold overlay/decal candidates for shader review, and keep the rubber gasket as a low-priority placeholder.

## Acceptance Labels

- `SMD08_MAT_WetBlackStoneSlab`: final_candidate - Strong final candidate. This directly attacks the flat grey wall/floor gap.
- `SMD08_MAT_WetBlackStoneMortar`: final_candidate - Final candidate, but should be scale-tested in Unity before broad tiling.
- `SMD08_MAT_ChippedBlackIronWallPanel`: final_candidate - Strong final candidate. This is the set anchor for black riveted iron.
- `SMD08_MAT_ChippedBlackIronServicePlate`: final_candidate - Final candidate for dark panels; slightly busy at distance by design.
- `SMD08_MAT_WornBrassPipe`: final_candidate - Strong final candidate. Replaces clean yellow brass with worn pressure hardware.
- `SMD08_MAT_WornBrassValveBody`: final_candidate - Final candidate for props; not a wall/floor hero.
- `SMD08_MAT_OxidizedCopperCoil`: final_candidate - Strong final candidate for breaking up brass monotony.
- `SMD08_MAT_OxidizedCopperRunoff`: final_candidate - Final candidate, best used sparingly because the green can become loud.
- `SMD08_MAT_RedPressureEnamel`: final_candidate - Strong final candidate for pressure danger language.
- `SMD08_MAT_ChippedRedEnamelEdge`: final_candidate - Final candidate but high-detail; should be kept to accents.
- `SMD08_MAT_AmberGaslightGlass`: final_candidate - Final candidate for atmosphere. Needs actual light/VFX later for full glow.
- `SMD08_MAT_SmokedAmberGaugeGlass`: candidate - Candidate. Reads well in previews, but transparency needs a project shader decision.
- `SMD08_MAT_SootDepositOverlay`: candidate - Candidate overlay. It is authored as an opaque material plus masks until a decal shader is chosen.
- `SMD08_MAT_VerticalGrimeStreakOverlay`: candidate - Candidate overlay. Needs alpha/decal hookup before production placement.
- `SMD08_MAT_BlackOilWetFloor`: final_candidate - Strong final candidate. Use in patches so the level does not become uniformly glossy.
- `SMD08_MAT_OilyBlackStonePuddle`: candidate - Candidate. Final look is strong; gameplay readability must be checked because it is very dark.
- `SMD08_MAT_ScorchedFurnaceMetal`: final_candidate - Strong final candidate for furnace-room identity.
- `SMD08_MAT_HeatTintedBoilerIron`: final_candidate - Final candidate, quieter than furnace metal and useful as a bridge material.
- `SMD08_MAT_GaugeFaceEnamel`: final_candidate - Strong final candidate. This is important for the pressure-pistol/HUD language.
- `SMD08_MAT_BakedIvoryGaugeFace`: candidate - Candidate. Useful filler, but less iconic than Gauge Face Enamel.
- `SMD08_MAT_RivetedBrassTrim`: final_candidate - Strong final candidate for north-star brass framing.
- `SMD08_MAT_RivetedBlackIronTrim`: final_candidate - Final candidate. Good for giving flat kit pieces a heavy industrial silhouette.
- `SMD08_MAT_SteamCondensationBlackMetal`: candidate - Candidate. The wetness is useful, but can become noisy without careful scale.
- `SMD08_MAT_CrackedBlackRubberGasket`: placeholder - Placeholder. Included for completeness, but it does not move the corridor/gun visual target much.

## Known Limitations

- Preview PNGs are procedural documentation renders, not Unity-rendered scene captures.
- No Blender, meshes, prefabs, scenes, runtime scripts, colliders, audio, or build configuration are included.
- Overlay materials (`SootDepositOverlay`, `VerticalGrimeStreakOverlay`) need a decal or transparent material path before production placement.
- True wetness, glass refraction, and amber glow will improve with project lighting/shader work; this package supplies the surface intent and masks.
