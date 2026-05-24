# Asset Promotion Review v0.1.11

Status: documentation-only promotion guidance for the main v0.1.11 agent  
Date: 2026-05-24  
Scope: review of current pressure-pistol component lookdev, brassworks corridor/vault-door lookdev, concept render index, and AAA asset catalog

## Review Basis

This review is based on the current files on disk:

- `Documentation/AssetProduction/PressurePistolLookDev/PRESSURE_PISTOL_COMPONENT_LOOKDEV_RENDER_REPORT.md`
- `Documentation/AssetProduction/BrassworksCorridorLookDev/BRASSWORKS_CORRIDOR_LOOKDEV_REPORT.md`
- `Documentation/ConceptRenders/INDEX.md`
- `Documentation/AAA_ASSET_CATALOG.md`

The new pressure-pistol and corridor/vault-door renders are useful Unity-only visual direction. They passed their render-lane technical checks, including nonblank output, no sampled magenta pixels, material separation, and framing. They should not be described as final AAA-quality weapon or environment art. Their geometry remains primitive/procedural lookdev, with no authored mesh, UV, collision, LOD, prefab, in-scene placement, or gameplay validation proof.

## Safe To Promote Into Gameplay Now

Nothing from the new lookdev render lanes is safe to promote directly as final gameplay art in v0.1.11.

The active catalog already lists several gameplay-verified procedural assets and material bindings, including FinalMaterialsV1 material families, pressure gate/service lift visuals, pressure-pistol gameplay functionality, route signage, objective UI, and multiple VFX/audio systems. Those existing verified assets can continue to be used, but the newly reviewed pressure-pistol component PNGs and corridor/vault-door proof renders are outside the gameplay asset lane and should not be imported into active scenes as production assets.

Allowed promotion level for v0.1.11:

- Use the new render reports as implementation guidance.
- Copy design decisions, naming, proportions, material vocabulary, and validation gates into production tasks.
- Keep gameplay-facing assets procedural or existing unless a scoped implementation creates owned prefabs/materials and passes the gates below.

## Useful As Reference Only

These assets are useful production references and should stay outside active gameplay scenes for now:

| Asset/reference | Usefulness | Promotion limit |
| --- | --- | --- |
| `PPCOMP_CONTACTSHEET_002_six_component_refinement.png` | Six-component pressure-pistol vocabulary: coil pack, gauge/dial, boiler chamber, barrel/muzzle, valve/manifold, grip/trigger guard. Useful for part breakdown and material intent. | Reference only. Do not reassemble into viewmodel art until at least one component is rebuilt as authored production geometry/materials. |
| `PPCOMP_001_copper_brass_coil_pack.png` | Strongest direction for copper/brass coil density, dark backing, patina, leads, and pressure-machine read. | Reference only unless rebuilt with real mesh bevels, pivots, material assignments, and validation. |
| `PPCOMP_002_pressure_gauge_dial.png` | Useful gauge readability target: cream dial, red needle, layered brass/blackened metal, side fittings, glass glints. | Reference only. May guide an in-game gauge prop or pistol gauge pass. |
| `PPCOMP_003_boiler_pressure_chamber.png` | Good pressure-vessel noun for the pistol and world props. | Reference only. Needs authored silhouette, UVs, grime masks, and prefab ownership. |
| `PPCOMP_004_barrel_muzzle_assembly.png` | Useful soot-dark bore, brass collar, lower pipe, and sight language. | Reference only. Do not treat as finished first-person barrel art. |
| `PPCOMP_005_valve_manifold_block.png` | Good functional pipe/valve layout reference. | Reference only. Valve wheels and segmented primitives need replacement. |
| `PPCOMP_006_leather_grip_trigger_guard.png` | Useful material-role reference for walnut/leather/brass, but visually weakest versus the north-star target. | Reference only. Needs sculpted grip silhouette before promotion. |
| `BBW_CORRIDOR_CONTACTSHEET_unity_lookdev.png` | Useful mood reference for wet floors, blackened iron, brass, amber practicals, pipes, steam, and vault-door pressure hardware. | Reference only. Do not import as art or presentation proof. |
| `BBW_CORRIDOR_001_modular_corridor_wet_pipe_lamps.png` | Useful corridor route mood and environmental vocabulary. | Reference only until rebuilt as modular scene/prefab assets. |
| `BBW_CORRIDOR_002_hero_round_vault_door.png` | Useful pressure-door silhouette and brass/blackened-steel layering. | Reference only until authored pressure-door mesh/prefab work exists. |
| `BBW_CORRIDOR_003_wall_kit_component_sheet.png` | Useful kit noun list: pipe cluster, riveted panel, lamp, valve wheel, pressure tank, wet tile. | Internal reference only. The report notes overlapping labels, so it is not presentation/final-art evidence. |
| Recovery08 pressure-pistol component renders | Current stronger older pressure-pistol direction for coil heat, gauge readability, soot-dark muzzle bore, and material swatches. | Reference only; still non-shipping proof geometry. |
| Environment Recovery01 corridor/material proof | Useful older mood/readability target for corridor material response. | Reference only; production fails until authored modular meshes, collision, LODs, and lighting workflow replace the primitive proof. |

## Blocked Until Authored Mesh/Material Work

These should be considered blocked for direct promotion until production assets exist, not just renders:

| Candidate | Blocker |
| --- | --- |
| Pressure Pistol final viewmodel art | Current component renders are isolated primitive proofs. Promotion requires authored viewmodel component meshes or an approved Unity procedural mesh builder, proper pivots/hierarchy, first-person scale checks, material assignments, muzzle alignment, recoil/flash compatibility, and gameplay smoke coverage. |
| Pressure Pistol grip/hand/glove area | The grip silhouette remains blockout-like and does not match the handmade Victorian north-star quality. A sculpted grip/trigger/hand treatment is required before full-gun promotion. |
| Pressure gauge production prop or pistol subcomponent | The lookdev dial is readable, but it needs production mesh layering, usable material slots, prefab ownership, and scene validation before use. |
| Barrel/muzzle/coil/manifold production subcomponents | The visual nouns are promising, but still need bevel continuity, collision relevance where applicable, UVs, trim/atlas choices, grime masks, LOD/platform decisions, and validation. |
| Brassworks corridor modular kit | The corridor proof is made from temporary primitives. Promotion requires actual modular wall/floor/ceiling/trim/pipe/lamp pieces with pivots, snap rules, colliders, lightmap/LOD proof, prefab variants, and route readability checks in playable scenes. |
| Hero round vault door | The rendered door is a good target, but production needs an owned door prefab or scene object with animation/interaction ownership if used, collision, scale proof, material slots, and route/objective readability validation. |
| Wet floor material/decal treatment | The render demonstrates mood, but gameplay scenes need approved material usage, performance checks, no readability loss, and platform-tier handling for Windows/Android/Browser. |

## Rejected Or Superseded

| Asset/reference | Decision |
| --- | --- |
| Environment Recovery02 render set | Rejected. Hot magenta shader-error output exceeded acceptance gates; retained only for diagnosis. |
| Environment Recovery03 render set | Rejected. Hot magenta shader-error output exceeded acceptance gates; renderer was quarantined outside `Assets`. |
| Recovery04 full-gun pressure-pistol proof | Rejected. Smoke slabs, cropped/side-on framing, orange material skew, and boxy components made it unsuitable for promotion. |
| Recovery03 pressure-pistol Python fallback proof | Failed proof. Not accepted as high-fidelity direction. |
| Batch01 high-fidelity lookdev as final art | Superseded for promotion purposes. Useful historically, but visually rejected as not close enough to the north-star sheet. |
| External renderer/DCC fallback lanes | Superseded unless explicitly reopened. v0.1.11 should preserve the Unity-only lookdev constraint unless a separate approved art task changes that policy. |

## Required Promotion Gates

Any promotion from reference/lookdev into active gameplay scenes must pass all of these gates:

1. Unity compile clean: project opens and compiles with no `error CS`, no new exceptions during load/play, and no render-lane compile blockers.
2. No magenta: promoted materials render in scene and in validation screenshots with no shader-error magenta or missing-material fallback.
3. No gameplay scene breakage: active scenes still load, player spawn/objectives/weapons/enemies/pickups/level transitions still work, and existing smoke/playthrough checks remain green.
4. Prefab/scene ownership: every promoted object has a clear owner, path, prefab or scene placement contract, material references, pivots, scale, hierarchy, and cleanup responsibility.
5. Validation coverage: add or update automated validation for expected objects/material references/routes/interactions, plus targeted smoke coverage when the asset affects gameplay.
6. Route readability: promoted art must preserve objective readability, combat silhouettes, pickup visibility, hazards, exits, and navigation cues. Wet floors, brass trim, steam, lamps, and dense pipes must not obscure routes or enemies.
7. No unapproved Blender/external DCC use: v0.1.11 promotion work must remain Unity-only unless the main agent has an explicit approved task to import authored external DCC assets. Any external mesh/material source must be documented, owned, and accepted before scene use.
8. AAA honesty: primitive lookdev, temporary procedural blocks, screenshots, and contact sheets must not be relabeled as final art. Promotion notes should say exactly what is final, what is placeholder, and what remains blocked.
9. Platform awareness: any new production asset needs a Windows target and a reduction plan or constraint note for Android and Browser/WebGL.

## Recommended First v0.1.11 Implementation Slice

Do not attempt a full pressure-pistol reassembly or corridor art swap in v0.1.11. The safest useful slice is a narrow reference-driven production planning pass:

1. Choose one hero component only: the pressure-pistol gauge/dial is the best candidate because it has strong readability, limited scope, and can improve both weapon and environment language.
2. Create or update a production brief for that component, including mesh hierarchy, pivots, material slots, validation expectations, and platform constraints.
3. If implementation is allowed by the main agent, build a Unity-only prefab/procedural mesh prototype in an owned gameplay-safe location, not by importing the render PNG or reusing temporary lookdev scene objects.
4. Validate it in an isolated scene or existing prop slot without replacing the full weapon viewmodel.
5. Keep the result labeled `prototype` or `review` until it passes compile, no-magenta, gameplay smoke, prefab ownership, validation coverage, and route/readability gates.

This gives v0.1.11 a concrete art-direction win without overpromoting weak primitive art or destabilizing the active scenes.

## Main-Lane Result

Status: implemented and verified in `v0.1.11`.

The main lane followed this recommendation by promoting only the pressure-gauge component language into gameplay. The Pressure Pistol viewmodel and Level01 pressure gate now use a generated `PressureGaugePrototype` with named brass bezel, blackened-iron backplate, cream enamel face, amber glass lens, red warning band, tick marks, rim rivets, lower pipe nipple, and pivoted needle. The full pressure-pistol and corridor/vault-door lookdev renders remain reference-only.

Verification:

- `V0_ROUTE_AUDIT_PASS`
- `V0_LEVEL_VALIDATION_PASS`
- `V0_BUILD_MATRIX_PASS`
- Build: `Builds/Windows/v0.1.11/BrassworksBreach_v0.1.11.exe`
