using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using PackageManagerInfo = UnityEditor.PackageManager.PackageInfo;

public static class SidecarQuarantineImportValidator
{
    private static readonly PackageCheck[] Packages =
    {
        new PackageCheck(
            "Corridor Kit Set 02",
            "com.brassworks.sidecar.corridor-kit-set02",
            "Documentation~/Manifest/SCK2_CorridorKitSet02_Manifest_v0.1.41-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorStraight_4m_NorthStar.prefab",
                "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorStraight_2m_ServiceDense.prefab",
                "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorCorner_90_Bulkhead.prefab",
                "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorTJunction_PipeSpine.prefab",
                "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorCrossJunction_CompassHub.prefab",
                "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_Door_BulkheadRound_3m.prefab",
                "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_Door_PressureLock_DoubleLeaf.prefab",
                "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_RoomWallPanel_GaugeNest.prefab",
                "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_LightRun_AmberCaged_4m.prefab",
                "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Materials/SCK2_MAT_PressureGreenGlass.mat",
                "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Meshes/SCK2_MESH_NorthStar8Unit.asset"
            }),
        new PackageCheck(
            "Encounter Enemy Set 02",
            "com.brassworks.sidecar.encounter-enemy-set02",
            "Documentation~/Manifest/EE02_EncounterEnemySet02_Manifest_v0.1.41-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_AshcanReclaimer_A_IdleSawScout.prefab",
                "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_AshcanReclaimer_B_ClawWindupTell.prefab",
                "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_PressureSpindle_B_NeedleThrustTell.prefab",
                "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_GatehammerBastion_A_ShieldedIdle.prefab",
                "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_GovernorWarden_B_BellBeaconCastTell.prefab",
                "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Materials/EE02_MAT_RedOverheatTell.mat",
                "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Materials/EE02_MAT_ReadabilityGhost.mat",
                "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Meshes/EE02_MESH_36ToothSawBlade.asset",
                "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Meshes/EE02_MESH_GovernorCommandGearHalo.asset"
            }),
        new PackageCheck(
            "Feedback FX Audio",
            "com.brassworks.sidecar.feedback-fx-audio",
            "Documentation~/Manifest/SCFX_FeedbackFXAudio_Manifest_v0.1.38-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.feedback-fx-audio/Runtime/Prefabs/SCFX_EVT_WeaponFired.prefab",
                "Packages/com.brassworks.sidecar.feedback-fx-audio/Runtime/Prefabs/SCFX_EVT_EnemyDeath.prefab",
                "Packages/com.brassworks.sidecar.feedback-fx-audio/Runtime/Prefabs/SCFX_EVT_ObjectiveCompleted.prefab",
                "Packages/com.brassworks.sidecar.feedback-fx-audio/Runtime/Materials/SCFX_MAT_AgedBrass.mat",
                "Packages/com.brassworks.sidecar.feedback-fx-audio/Runtime/Audio/SCFX_AUD_WeaponFired.wav"
            }),
        new PackageCheck(
            "Materials Set 01",
            "com.brassworks.sidecar.materials-set01",
            "Documentation~/Manifest/MSET01_MaterialsSet01_Manifest_v0.1.39-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.materials-set01/Runtime/Materials/MSET01_MAT_AgedBrass.mat",
                "Packages/com.brassworks.sidecar.materials-set01/Runtime/Materials/MSET01_MAT_OilyBlackenedIron.mat",
                "Packages/com.brassworks.sidecar.materials-set01/Runtime/Materials/MSET01_MAT_RivetedWallPlate.mat",
                "Packages/com.brassworks.sidecar.materials-set01/Runtime/Materials/MSET01_MAT_PressureGaugeGlass.mat",
                "Packages/com.brassworks.sidecar.materials-set01/Runtime/Textures/Albedo/MSET01_MAT_AgedBrass_ALB.png",
                "Packages/com.brassworks.sidecar.materials-set01/Runtime/Textures/Normal/MSET01_MAT_AgedBrass_NRM.png",
                "Packages/com.brassworks.sidecar.materials-set01/Runtime/Textures/Mask/MSET01_MAT_AgedBrass_MSK.png"
            }),
        new PackageCheck(
            "Surface Material Detail Set 08",
            "com.brassworks.sidecar.surface-material-detail-set08",
            "Documentation~/Manifest/SMD08_SurfaceMaterialDetailSet08_Manifest_0.1.52-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.surface-material-detail-set08/Runtime/Materials/SMD08_MAT_WetBlackStoneSlab.mat",
                "Packages/com.brassworks.sidecar.surface-material-detail-set08/Runtime/Materials/SMD08_MAT_ChippedBlackIronWallPanel.mat",
                "Packages/com.brassworks.sidecar.surface-material-detail-set08/Runtime/Materials/SMD08_MAT_RivetedBrassTrim.mat",
                "Packages/com.brassworks.sidecar.surface-material-detail-set08/Runtime/Materials/SMD08_MAT_WornBrassPipe.mat",
                "Packages/com.brassworks.sidecar.surface-material-detail-set08/Runtime/Materials/SMD08_MAT_RedPressureEnamel.mat",
                "Packages/com.brassworks.sidecar.surface-material-detail-set08/Runtime/Materials/SMD08_MAT_GaugeFaceEnamel.mat",
                "Packages/com.brassworks.sidecar.surface-material-detail-set08/Runtime/Textures/Albedo/SMD08_MAT_WetBlackStoneSlab_ALB.png",
                "Packages/com.brassworks.sidecar.surface-material-detail-set08/Runtime/Textures/Normal/SMD08_MAT_WetBlackStoneSlab_NRM.png",
                "Packages/com.brassworks.sidecar.surface-material-detail-set08/Runtime/Textures/RoughnessMetallic/SMD08_MAT_WetBlackStoneSlab_RMA.png",
                "Packages/com.brassworks.sidecar.surface-material-detail-set08/Runtime/Textures/GrimeEdgewear/SMD08_MAT_WetBlackStoneSlab_GRM.png",
                "Packages/com.brassworks.sidecar.surface-material-detail-set08/Runtime/Metadata/SMD08_SurfaceMaterialDetailCatalog_0.1.52-p001.json"
            }),
        new PackageCheck(
            "Steam Corridor Dressing Set 09",
            "com.brassworks.sidecar.steam-corridor-dressing-set09",
            "Documentation~/Manifest/SCD09_SteamCorridorDressingSet09_Manifest_0.1.54-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-set09/Runtime/Generated/Prefabs/SCD09_PREFAB_001_WallPipeTripleRun_A.prefab",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-set09/Runtime/Generated/Prefabs/SCD09_PREFAB_005_WallGaslightSconce_E.prefab",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-set09/Runtime/Generated/Prefabs/SCD09_PREFAB_014_CeilingLampCage_C.prefab",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-set09/Runtime/Generated/Prefabs/SCD09_PREFAB_017_DoorwayRivetedHeader_A.prefab",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-set09/Runtime/Generated/Materials/SCD09_MAT_AgedBrass.mat",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-set09/Runtime/Generated/Materials/SCD09_MAT_WetBlackStone.mat",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-set09/Runtime/Generated/Meshes/SCD09_MESH_Cylinder24.asset",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-set09/Runtime/Metadata/SCD09_SteamCorridorDressingSet09_Catalog_0.1.54-p001.json"
            }),
        new PackageCheck(
            "Clockwork Enemy Parts Set 09",
            "com.brassworks.sidecar.clockwork-enemy-parts-set09",
            "Documentation~/Manifest/CEPS09_ClockworkEnemyPartsSet09_Manifest_v0.1.54-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.clockwork-enemy-parts-set09/Runtime/Prefabs/CEPS09_ArchetypePreview_BoilerBrute_HumanoidSilhouette.prefab",
                "Packages/com.brassworks.sidecar.clockwork-enemy-parts-set09/Runtime/Prefabs/CEPS09_ArchetypePreview_SkitterUnit_ReadableLowProfile.prefab",
                "Packages/com.brassworks.sidecar.clockwork-enemy-parts-set09/Runtime/Prefabs/CEPS09_ArchetypePreview_WallCeilingSentry_MountedProfile.prefab",
                "Packages/com.brassworks.sidecar.clockwork-enemy-parts-set09/Runtime/Prefabs/CEPS09_Shared_Gauge_SteamPressureDial_A.prefab",
                "Packages/com.brassworks.sidecar.clockwork-enemy-parts-set09/Runtime/Materials/CEPS09_MAT_AgedBrassBoiler.mat",
                "Packages/com.brassworks.sidecar.clockwork-enemy-parts-set09/Runtime/Materials/CEPS09_MAT_AmberGlowGlass.mat",
                "Packages/com.brassworks.sidecar.clockwork-enemy-parts-set09/Runtime/Textures/CEPS09_TEX_AgedBrassBoiler_Base.png",
                "Packages/com.brassworks.sidecar.clockwork-enemy-parts-set09/Runtime/Metadata/CEPS09_ClockworkEnemyPartsCatalog_v0.1.54-p001.json"
            }),
        new PackageCheck(
            "Steam Corridor Dressing High Fidelity Set 11",
            "com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11",
            "Documentation~/Manifest/SCDHF11_SteamCorridorDressingHighFidelitySet11_Manifest_v0.1.56-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11/Runtime/Prefabs/SCDHF11_PREFAB_WallPipeRunLayered_A.prefab",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11/Runtime/Prefabs/SCDHF11_PREFAB_CagedGaslight_Long_A.prefab",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11/Runtime/Prefabs/SCDHF11_PREFAB_GaugeCluster_Triple_A.prefab",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11/Runtime/Prefabs/SCDHF11_PREFAB_BoilerTankColumn_A.prefab",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11/Runtime/Materials/SCDHF11_MAT_AgedBrassDeepPatina.mat",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11/Runtime/Materials/SCDHF11_MAT_BlackenedRivetedIron.mat",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11/Runtime/Textures/SCDHF11_TEX_AgedBrassDeepPatina_Base.png",
                "Packages/com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11/Runtime/Metadata/SCDHF11_CorridorDressingCatalog_v0.1.56-p001.json"
            }),
        new PackageCheck(
            "Mechanical Sentinel Hero Set 10",
            "com.brassworks.sidecar.mechanical-sentinel-hero-set10",
            "Runtime/Metadata/MSH10_MechanicalSentinelHeroSet10_Manifest_v0.1.55-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.mechanical-sentinel-hero-set10/Runtime/Prefabs/MSH10_MechanicalSentinelHeroAssembly.prefab",
                "Packages/com.brassworks.sidecar.mechanical-sentinel-hero-set10/Runtime/Prefabs/MSH10_BoilerTorso_Module.prefab",
                "Packages/com.brassworks.sidecar.mechanical-sentinel-hero-set10/Runtime/Prefabs/MSH10_FurnaceHead_Module.prefab",
                "Packages/com.brassworks.sidecar.mechanical-sentinel-hero-set10/Runtime/Prefabs/MSH10_LeftSawArm_Module.prefab",
                "Packages/com.brassworks.sidecar.mechanical-sentinel-hero-set10/Runtime/Materials/MSH10_MAT_AgedBrassPatina.mat",
                "Packages/com.brassworks.sidecar.mechanical-sentinel-hero-set10/Runtime/Materials/MSH10_MAT_AmberFurnaceGlow.mat",
                "Packages/com.brassworks.sidecar.mechanical-sentinel-hero-set10/Runtime/Textures/MSH10_TEX_AgedBrassPatina_Albedo.png",
                "Packages/com.brassworks.sidecar.mechanical-sentinel-hero-set10/Runtime/Metadata/MSH10_MechanicalSentinelHeroSet10_Catalog_v0.1.55-p001.json"
            }),
        new PackageCheck(
            "Steam Atmosphere VFX Set 10",
            "com.brassworks.sidecar.steam-atmosphere-vfx-set10",
            "Documentation~/Manifest/SAV10_SteamAtmosphereVfxSet10_Manifest_v0.1.55-p020.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.steam-atmosphere-vfx-set10/Runtime/Prefabs/SAV10_PREFAB_WarmGaslightHaze.prefab",
                "Packages/com.brassworks.sidecar.steam-atmosphere-vfx-set10/Runtime/Prefabs/SAV10_PREFAB_PipeLeakSteamJet.prefab",
                "Packages/com.brassworks.sidecar.steam-atmosphere-vfx-set10/Runtime/Prefabs/SAV10_PREFAB_LowFloorMist.prefab",
                "Packages/com.brassworks.sidecar.steam-atmosphere-vfx-set10/Runtime/Prefabs/SAV10_PREFAB_BacklitDoorFog.prefab",
                "Packages/com.brassworks.sidecar.steam-atmosphere-vfx-set10/Runtime/Materials/SAV10_MAT_WarmGaslightHaze.mat",
                "Packages/com.brassworks.sidecar.steam-atmosphere-vfx-set10/Runtime/Textures/SAV10_TEX_WarmGaslightHaze.png",
                "Packages/com.brassworks.sidecar.steam-atmosphere-vfx-set10/Runtime/Metadata/SAV10_SteamAtmosphereVfxCatalog_v0.1.55-p020.json"
            }),
        new PackageCheck(
            "Brassworks Door Mechanism Set 10",
            "com.brassworks.sidecar.brassworks-door-mechanism-set10",
            "Documentation~/Manifest/BDM10_BrassworksDoorMechanismSet10_Manifest_v0.1.55-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.brassworks-door-mechanism-set10/Runtime/Prefabs/BDM10_PressureWheel_CrankedValve.prefab",
                "Packages/com.brassworks.sidecar.brassworks-door-mechanism-set10/Runtime/Prefabs/BDM10_GaugeValve_ManifoldCluster.prefab",
                "Packages/com.brassworks.sidecar.brassworks-door-mechanism-set10/Runtime/Prefabs/BDM10_PistonBrace_SteamActuated.prefab",
                "Packages/com.brassworks.sidecar.brassworks-door-mechanism-set10/Runtime/Prefabs/BDM10_GearHub_LargeCogCore.prefab",
                "Packages/com.brassworks.sidecar.brassworks-door-mechanism-set10/Runtime/Materials/BDM10_MAT_AgedBrassPatina.mat",
                "Packages/com.brassworks.sidecar.brassworks-door-mechanism-set10/Runtime/Materials/BDM10_MAT_BlackenedOilyIron.mat",
                "Packages/com.brassworks.sidecar.brassworks-door-mechanism-set10/Runtime/Textures/BDM10_TEX_AgedBrassPatina_Albedo.png",
                "Packages/com.brassworks.sidecar.brassworks-door-mechanism-set10/Runtime/Metadata/BDM10_BrassworksDoorMechanismSet10_Manifest_v0.1.55-p001.json"
            }),
        new PackageCheck(
            "Steampunk Weapons",
            "com.brassworks.sidecar.steampunk-weapons",
            "Documentation~/Manifest/SCWPN_SteampunkWeapons_Manifest_v0.1.37-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.steampunk-weapons/Runtime/Prefabs/BB_V0137_PressurePistolCore.prefab",
                "Packages/com.brassworks.sidecar.steampunk-weapons/Runtime/Prefabs/BB_V0137_CopperCoilAssembly.prefab",
                "Packages/com.brassworks.sidecar.steampunk-weapons/Runtime/Prefabs/BB_V0137_BrassDialGaugeAssembly.prefab",
                "Packages/com.brassworks.sidecar.steampunk-weapons/Runtime/Materials/BB_AgedBrass.mat",
                "Packages/com.brassworks.sidecar.steampunk-weapons/Runtime/Materials/BB_BlackenedIron.mat"
            }),
        new PackageCheck(
            "Weapon Props Set 02",
            "com.brassworks.sidecar.weapon-props-set02",
            "Documentation~/Manifest/WPS02_WeaponPropsSet02_Manifest_v0.1.40-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_PressurePistol_Frame_A.prefab",
                "Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_PressurePistol_BarrelAssembly.prefab",
                "Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_Scattergun_Body_TwinBoiler.prefab",
                "Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_AmmoCartridge_Cluster.prefab",
                "Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_WallWeaponRack_ThreeSlot.prefab",
                "Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_GearKey_Housing.prefab",
                "Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Materials/WPS02_MAT_AgedBrass.mat",
                "Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Meshes/WPS02_Mesh_BeveledBox.asset"
            }),
        new PackageCheck(
            "Mechanical Enemies",
            "com.brassworks.sidecar.mechanical-enemies",
            "Documentation~/Manifest/SCENM_MechanicalEnemies_Manifest_v0.1.37-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.mechanical-enemies/Runtime/Prefabs/SCENM_SawScrapper.prefab",
                "Packages/com.brassworks.sidecar.mechanical-enemies/Runtime/Prefabs/SCENM_RivetLancer.prefab",
                "Packages/com.brassworks.sidecar.mechanical-enemies/Runtime/Prefabs/SCENM_BulwarkFurnace.prefab",
                "Packages/com.brassworks.sidecar.mechanical-enemies/Runtime/Materials/SCENM_MAT_AgedBrass.mat",
                "Packages/com.brassworks.sidecar.mechanical-enemies/Runtime/Meshes/SCENM_MESH_24ToothSawBlade.asset"
            }),
        new PackageCheck(
            "Mechanical Enemy Visual Set 01",
            "com.brassworks.sidecar.mechanical-enemy-visual-set01",
            "Documentation~/Manifest/MEV01_MechanicalEnemyVisualSet01_Manifest_v0.1.40-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_SawScrapper_A_BoilerSaw.prefab",
                "Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_RivetLancer_B_RailLance.prefab",
                "Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_BulwarkFurnace_A_ShieldBoiler.prefab",
                "Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_BellowsSupport_B_PressureNode.prefab",
                "Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_WardenOverseer_A_TallWarden.prefab",
                "Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Materials/MEV01_MAT_FurnaceOrangeGlow.mat",
                "Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Meshes/MEV01_MESH_28ToothSawBlade.asset"
            }),
        new PackageCheck(
            "Steamworks Level Kit",
            "com.brassworks.sidecar.steamworks-level-kit",
            "Documentation~/Manifest/SCLVL_SteamworksLevelKit_Manifest_v0.1.39-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_CorridorStraight_4m.prefab",
                "Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_ArchedPressureDoor_4m.prefab",
                "Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_ValveConsole.prefab",
                "Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Materials/SCLVL_AgedBrass.mat",
                "Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Meshes/SCLVL_BoxUnit.asset"
            }),
        new PackageCheck(
            "Weapon Viewmodel Set 03",
            "com.brassworks.sidecar.weapon-viewmodel-set03",
            "Documentation~/Manifest/WVM03_WeaponViewmodelSet03_Manifest_v0.1.41-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_PressurePistol_FullAssembly_A.prefab",
                "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_PressurePistol_FullAssembly_B_DualGauge.prefab",
                "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_Scattergun_FullAssembly_A.prefab",
                "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_BoltThrower_FullAssembly_A.prefab",
                "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_GloveSilhouette_RightGrip.prefab",
                "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_GloveSilhouette_LeftSupport.prefab",
                "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_AmmoPressureCell_Single.prefab",
                "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Materials/WVM03_MAT_GreenGaugeGlass.mat",
                "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Meshes/WVM03_Mesh_GlovePalm.asset",
                "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/WeaponViewmodelSet03Identity.cs"
            }),
        new PackageCheck(
            "Level Dressing Set 01",
            "com.brassworks.sidecar.level-dressing-set01",
            "Documentation~/Manifest/SCLD_LevelDressingSet01_Manifest_v0.1.40-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_RivetedTrimPlate_2m.prefab",
                "Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_PipeJunction_T_2m.prefab",
                "Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_PressureTank_FloorLarge.prefab",
                "Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_ValveCluster_Floor_2m.prefab",
                "Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_CagedLamp_Wall.prefab",
                "Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_SootGrimeDecal_Wide.prefab",
                "Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_ServicePanel_Floor_1x2m.prefab",
                "Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Materials/SCLD_MAT_AgedBrass.mat",
                "Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Meshes/SCLD_MESH_BoxUnit.asset"
            }),
        new PackageCheck(
            "Objective Props Set 02",
            "com.brassworks.sidecar.objective-props-set02",
            "Documentation~/Manifest/OPS02_ObjectivePropsSet02_Manifest_v0.1.42-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_KeyedLock_TriGearVault.prefab",
                "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_KeyedLock_RuneCogDoorSocket.prefab",
                "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_ValvePanel_TwinPressurePuzzle.prefab",
                "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_LiftCallStation_BrassCageUpDown.prefab",
                "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_PressureRegulator_RedlineGovernor.prefab",
                "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_SecretCache_FloorGearSafe.prefab",
                "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_Actuator_BridgeThrowLever.prefab",
                "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_GovernorOverride_BossKillSwitch.prefab",
                "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Materials/OPS02_MAT_RedOverrideEnamel.mat",
                "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Meshes/OPS02_Mesh_Gear18ToothUnit.asset",
                "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/ObjectivePropsSet02Identity.cs"
            }),
        new PackageCheck(
            "Steam VFX Set 02",
            "com.brassworks.sidecar.steam-vfx-set02",
            "Documentation~/Manifest/BBSVFX02_SteamVFXSet02_Manifest_v0.1.42-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_SteamVent_FloorBurst.prefab",
                "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_SteamVent_WallJet.prefab",
                "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_PressureLeak_RuptureCone.prefab",
                "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_MuzzleFlash_PistolBoiler.prefab",
                "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_SparkRicochet_WallHit.prefab",
                "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_FurnaceBlast_DoorBelch.prefab",
                "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_BossPhase_GovernorOvercrank.prefab",
                "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Materials/BBSVFX02_MAT_SteamDense.mat",
                "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Meshes/BBSVFX02_MESH_RadialBurst_16.asset",
                "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Scripts/SteamVfxSet02Identity.cs"
            }),
        new PackageCheck(
            "Level Atmosphere Set 03",
            "com.brassworks.sidecar.level-atmosphere-set03",
            "Documentation~/Manifest/SCLA_LevelAtmosphereSet03_Manifest_v0.1.44-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_SteamPipeCluster_WallLeaker_A.prefab",
                "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_SteamPipeCluster_CornerBleed.prefab",
                "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_PressureLamp_WallCaged_A.prefab",
                "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_WallGrimePanel_OilStreaks.prefab",
                "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_HangingChains_TripleSlack.prefab",
                "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_FloorDrainCover_LongGutter.prefab",
                "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_WarningGauge_TripleRack.prefab",
                "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_OverheadPipeCanopy_ValveRun.prefab",
                "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_DenseAmbienceCombo_CorridorBite.prefab",
                "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Materials/SCLA_MAT_AmberLampGlass.mat",
                "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Meshes/SCLA_MESH_SteamWispUnit.asset"
            }),
        new PackageCheck(
            "Enemy Animation Proxy Set 01",
            "com.brassworks.sidecar.enemy-animation-proxy-set01",
            "Documentation~/Manifest/EAP01_EnemyAnimationProxySet01_Manifest_v0.1.44-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_ScrapperAshcan_01_IdleBrace.prefab",
                "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_ScrapperAshcan_03_SawLunge.prefab",
                "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_LancerPressureSpindle_01_AimLine.prefab",
                "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_LancerPressureSpindle_03_ThrustPeak.prefab",
                "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_BulwarkGatehammer_02_HammerRaise.prefab",
                "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_WardenGovernor_02_SignalRaise.prefab",
                "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Materials/EAP01_MAT_FurnaceOrangeGlow.mat",
                "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Meshes/EAP01_MESH_CommandGearHalo.asset",
                "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/AnimationClips/EAP01_CLIP_ScrapperAshcan_PoseProxyOnly.anim",
                "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/AnimationClips/EAP01_CLIP_WardenGovernor_PoseProxyOnly.anim"
            }),
        new PackageCheck(
            "Room Setpiece Kit 04",
            "com.brassworks.sidecar.room-setpiece-kit04",
            "Documentation~/Manifest/RSK04_RoomSetpieceKit04_Manifest_v0.1.45-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_BoilerChamberWallBay_A.prefab",
                "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_PressureVaultDoorAlcove_A.prefab",
                "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_CatwalkBalconyModule_A.prefab",
                "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_RegulatorCoreMachinery_A.prefab",
                "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_PipeGalleryCeilingCluster_A.prefab",
                "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_ServiceStairSilhouette_A.prefab",
                "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_FurnaceControlWall_A.prefab",
                "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_BrassFloorTrimThreshold_A.prefab",
                "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_LargeWarningGaugeWall_A.prefab",
                "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_RoomCornerClutterCluster_A.prefab",
                "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Materials/RSK04_MAT_AgedBrass.mat",
                "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Meshes/RSK04_MESH_Gear24Unit.asset"
            }),
        new PackageCheck(
            "Weapon Mechanisms Set 04",
            "com.brassworks.sidecar.weapon-mechanisms-set04",
            "Documentation~/Manifest/WMS04_WeaponMechanismsSet04_Manifest_v0.1.45-p001.json",
            new[]
            {
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_PressurePistolCoil_TripleAmber_A.prefab",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_GaugeCluster_TripleIvory_A.prefab",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_GripAssembly_WalnutLeather_A.prefab",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_ReceiverPlate_BrassLattice_A.prefab",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_MuzzleCrown_CogBrake_B.prefab",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_PressureTank_TwinUnderbarrel_B.prefab",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_ValveLever_RedSafety_A.prefab",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_AmmoCylinder_EightCell_B.prefab",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_ScattergunPressureChamber_Quad_B.prefab",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_BoltThrowerRail_ChargedSlide_B.prefab",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_GlovedHandSilhouette_RightGrip_A.prefab",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_MaterialSwatch_MetalsAndGlass_A.prefab",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Materials/WMS04_MAT_AgedBrassBrushed.mat",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Materials/WMS04_MAT_TealPressureGlow.mat",
                "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Meshes/WMS04_MESH_Gear24Ring.asset"
            })
    };

    [MenuItem("Project Tools/Validate Sidecar Quarantine Imports")]
    public static void RunValidation()
    {
        int checkedAssetCount = 0;

        foreach (PackageCheck package in Packages)
        {
            PackageManagerInfo packageInfo = RequirePackageResolved(package);
            RequirePackageJson(package, packageInfo);
            RequireProjectManifestReference(package);
            RequirePackageFile(packageInfo, package.ManifestRelativePath, package.Label + " package-local manifest");

            foreach (string assetPath in package.RequiredAssetPaths)
            {
                RequireLoadableAsset(assetPath, package.Label);
                checkedAssetCount++;
            }
        }

        Debug.Log("SIDECAR_QUARANTINE_IMPORT_PASS packages=" + Packages.Length + " assets=" + checkedAssetCount);
    }

    private static PackageManagerInfo RequirePackageResolved(PackageCheck package)
    {
        PackageManagerInfo packageInfo = PackageManagerInfo.FindForAssetPath(package.RequiredAssetPaths[0]);
        if (packageInfo == null)
        {
            throw new InvalidOperationException(package.Label + " package is not resolved through Package Manager from " + package.RequiredAssetPaths[0]);
        }

        if (!string.Equals(packageInfo.name, package.PackageName, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(package.Label + " package resolved to " + packageInfo.name + " instead of " + package.PackageName);
        }

        if (string.IsNullOrWhiteSpace(packageInfo.resolvedPath) || !Directory.Exists(packageInfo.resolvedPath))
        {
            throw new InvalidOperationException(package.Label + " package resolved path is missing: " + packageInfo.resolvedPath);
        }

        return packageInfo;
    }

    private static void RequirePackageJson(PackageCheck package, PackageManagerInfo packageInfo)
    {
        string packageJsonPath = Path.Combine(packageInfo.resolvedPath, "package.json");
        if (!File.Exists(packageJsonPath))
        {
            throw new InvalidOperationException(package.Label + " package.json was not found at " + packageJsonPath);
        }

        string packageJson = File.ReadAllText(packageJsonPath);
        if (!packageJson.Contains("\"name\"") || !packageJson.Contains(package.PackageName))
        {
            throw new InvalidOperationException(package.Label + " package.json does not identify " + package.PackageName);
        }
    }

    private static void RequireProjectManifestReference(PackageCheck package)
    {
        string manifestPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Packages", "manifest.json"));
        string manifestText = File.ReadAllText(manifestPath);
        if (!manifestText.Contains(package.PackageName))
        {
            throw new InvalidOperationException("Project manifest does not reference " + package.PackageName);
        }
    }

    private static void RequirePackageFile(PackageManagerInfo packageInfo, string relativePath, string label)
    {
        string diskPath = Path.Combine(packageInfo.resolvedPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
        if (!File.Exists(diskPath))
        {
            throw new InvalidOperationException(label + " was not found at " + diskPath);
        }

        if (string.IsNullOrWhiteSpace(File.ReadAllText(diskPath)))
        {
            throw new InvalidOperationException(label + " is empty at " + diskPath);
        }
    }

    private static void RequireLoadableAsset(string assetPath, string label)
    {
        UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
        if (asset == null)
        {
            throw new InvalidOperationException(label + " required asset is not loadable at " + assetPath);
        }
    }

    private sealed class PackageCheck
    {
        public PackageCheck(string label, string packageName, string manifestRelativePath, string[] requiredAssetPaths)
        {
            Label = label;
            PackageName = packageName;
            ManifestRelativePath = manifestRelativePath;
            RequiredAssetPaths = requiredAssetPaths;
        }

        public string Label { get; }
        public string PackageName { get; }
        public string ManifestRelativePath { get; }
        public string[] RequiredAssetPaths { get; }
    }
}
