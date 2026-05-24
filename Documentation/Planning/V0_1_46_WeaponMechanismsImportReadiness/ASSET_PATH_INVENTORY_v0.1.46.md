# V0.1.46 Asset Path Inventory

Purpose: list representative `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/...` paths for main-lane validator coverage after `BrassworksBreach.WeaponMechanismsSet04` is imported.

## Package Counts From Evidence

| Package | Prefabs | Materials | Meshes | Textures | Audio | VFX | Animation clips | Preview renders | Notes |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| WeaponMechanismsSet04 | 29 | 20 | 11 | 0 | 0 | 0 | 0 | 11 | Visual-only weapon components: coils, gauges, grips, receiver plates, muzzle crowns, tanks, valves, cylinders, chambers, rails, gloved-hand silhouettes, and material swatches. |

## Representative Assets

| Category | Path | Validation reason |
| --- | --- | --- |
| Manifest | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Documentation~/Manifest/WMS04_WeaponMechanismsSet04_Manifest_v0.1.45-p001.json` | Package-local evidence exists and records counts, package identity, preview renders, and visual-only constraints. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_PressurePistolCoil_TripleAmber_A.prefab` | Baseline pistol coil assembly and first-person silhouette coverage. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_GaugeCluster_TripleIvory_A.prefab` | Gauge readability and red needle material coverage. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_GripAssembly_WalnutLeather_A.prefab` | Wood, leather, grip scale, and future hand-alignment risk coverage. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_ReceiverPlate_BrassLattice_A.prefab` | Receiver plate detail and brass material coverage. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_MuzzleCrown_CogBrake_B.prefab` | Muzzle silhouette coverage without firing or damage authority. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_PressureTank_TwinUnderbarrel_B.prefab` | Underbarrel tank scale and glass material coverage. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_ValveLever_RedSafety_A.prefab` | Red safety enamel and lever readability coverage. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_AmmoCylinder_EightCell_B.prefab` | Ammo cylinder visual coverage without ammo, inventory, or reload authority. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_ScattergunPressureChamber_Quad_B.prefab` | Bulky scattergun chamber density and bounds coverage. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_BoltThrowerRail_ChargedSlide_B.prefab` | Long rail profile and future projectile alignment risk coverage. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_GlovedHandSilhouette_RightGrip_A.prefab` | Hand silhouette reference; must remain visual-only and non-rigged. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_MaterialSwatch_MetalsAndGlass_A.prefab` | Consolidated material realism board coverage. |
| Material | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Materials/WMS04_MAT_AgedBrassBrushed.mat` | Core brass material loadability and fallback coverage. |
| Material | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Materials/WMS04_MAT_TealPressureGlow.mat` | Emissive/glow material loadability coverage. |
| Mesh | `Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Meshes/WMS04_MESH_Gear24Ring.asset` | Custom ring/gear mesh coverage for mechanical silhouettes. |

## Validator Asset Count Recommendation

Use the 15 non-manifest representative assets above as `RequiredAssetPaths`. The manifest should be validated through the existing `PackageCheck` manifest path and should not count toward `checkedAssetCount`.

- WeaponMechanismsSet04: 15 representative asset checks.
- Expected total after v0.1.46 if current v0.1.45 inventory is active: `packages=16 assets=138`.

This deliberately does not require every generated asset. It samples each major visual family while keeping the import gate fast enough for repeated main-lane checks.
