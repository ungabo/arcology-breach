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
