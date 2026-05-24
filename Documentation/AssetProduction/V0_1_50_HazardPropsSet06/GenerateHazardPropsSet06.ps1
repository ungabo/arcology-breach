param(
    [string]$ProjectRoot = "D:\__MY APPS\Unity Doom"
)

$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Drawing

$PackageName = "BrassworksBreach.HazardPropsSet06"
$PackageId = "com.brassworks.sidecar.hazard-props-set06"
$Version = "0.1.50-p001"
$Code = "HP06"
$PackageRoot = Join-Path $ProjectRoot "AssetPacks\$PackageName"
$ProductionRoot = Join-Path $ProjectRoot "Documentation\AssetProduction\V0_1_50_HazardPropsSet06"
$RenderRoot = Join-Path $ProjectRoot "Documentation\ConceptRenders\V0_1_50_HazardPropsSet06"
$PlanningRoot = Join-Path $ProjectRoot "Documentation\Planning\V0_1_50_HazardPropsSet06ImportReadiness"
$QaRoot = Join-Path $ProjectRoot "Documentation\QA\V0_1_50_HazardPropsSet06ImportReadiness"

function New-HexGuid { -join ((1..32) | ForEach-Object { "{0:x}" -f (Get-Random -Min 0 -Max 16) }) }
function Ensure-Dir([string]$Path) { New-Item -ItemType Directory -Force -Path $Path | Out-Null }
function Write-Utf8([string]$Path, [string]$Text) {
    Ensure-Dir (Split-Path -Parent $Path)
    [System.IO.File]::WriteAllText($Path, $Text, [System.Text.UTF8Encoding]::new($false))
}
function Write-Meta([string]$Path, [string]$Guid = (New-HexGuid), [bool]$Folder = $false) {
    if ($Folder) {
        Write-Utf8 "$Path.meta" "fileFormatVersion: 2`nguid: $Guid`nfolderAsset: yes`nDefaultImporter:`n  externalObjects: {}`n  userData: `n  assetBundleName: `n  assetBundleVariant: `n"
    } else {
        Write-Utf8 "$Path.meta" "fileFormatVersion: 2`nguid: $Guid`nDefaultImporter:`n  externalObjects: {}`n  userData: `n  assetBundleName: `n  assetBundleVariant: `n"
    }
    return $Guid
}
function Write-TextureMeta([string]$Path, [string]$Guid = (New-HexGuid)) {
    Write-Utf8 "$Path.meta" "fileFormatVersion: 2`nguid: $Guid`nTextureImporter:`n  internalIDToNameTable: []`n  externalObjects: {}`n  serializedVersion: 13`n  mipmaps:`n    mipMapMode: 0`n    enableMipMap: 0`n    sRGBTexture: 1`n  textureSettings:`n    serializedVersion: 2`n    filterMode: 1`n    aniso: 1`n    mipBias: 0`n    wrapU: 1`n    wrapV: 1`n    wrapW: 1`n  maxTextureSize: 1024`n  textureFormat: 1`n  platformSettings: []`n  spriteMode: 0`n  userData: `n  assetBundleName: `n  assetBundleVariant: `n"
    return $Guid
}
function Write-ObjMeta([string]$Path, [string]$Guid = (New-HexGuid)) {
    Write-Utf8 "$Path.meta" "fileFormatVersion: 2`nguid: $Guid`nModelImporter:`n  serializedVersion: 22200`n  internalIDToNameTable: []`n  externalObjects: {}`n  materials:`n    materialImportMode: 0`n  meshes:`n    lODScreenPercentages: []`n    globalScale: 1`n    meshCompression: 0`n    addColliders: 0`n    useSRGBMaterialColor: 1`n  importAnimation: 0`n  userData: `n  assetBundleName: `n  assetBundleVariant: `n"
    return $Guid
}
function Write-Material([string]$Path, [hashtable]$Color, [float]$Metallic, [float]$Smoothness, [string]$Guid = (New-HexGuid)) {
    $yaml = @"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!21 &2100000
Material:
  serializedVersion: 8
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_Name: $([System.IO.Path]::GetFileNameWithoutExtension($Path))
  m_Shader: {fileID: 46, guid: 0000000000000000f000000000000000, type: 0}
  m_Parent: {fileID: 0}
  m_ModifiedSerializedProperties: 0
  m_ValidKeywords: []
  m_InvalidKeywords: []
  m_LightmapFlags: 4
  m_EnableInstancingVariants: 1
  m_DoubleSidedGI: 0
  m_CustomRenderQueue: -1
  stringTagMap: {}
  disabledShaderPasses: []
  m_SavedProperties:
    serializedVersion: 3
    m_TexEnvs: []
    m_Ints: []
    m_Floats:
    - _Metallic: $Metallic
    - _Glossiness: $Smoothness
    m_Colors:
    - _Color: {r: $($Color.r), g: $($Color.g), b: $($Color.b), a: $($Color.a)}
  m_BuildTextureStacks: []
"@
    Write-Utf8 $Path $yaml
    Write-Meta $Path $Guid | Out-Null
    return $Guid
}

function Write-ObjMesh([string]$Path, [string]$Kind, [double]$W, [double]$H) {
    $name = [System.IO.Path]::GetFileNameWithoutExtension($Path)
    $verts = @()
    $faces = @()
    switch ($Kind) {
        "octplate" {
            for ($i=0; $i -lt 8; $i++) { $a = [Math]::PI*2*$i/8 + [Math]::PI/8; $verts += "v $([Math]::Round([Math]::Cos($a)*$W,4)) $([Math]::Round([Math]::Sin($a)*$W,4)) 0" }
            $faces += "f 1 2 3 4 5 6 7 8"
        }
        "gauge" {
            $verts += "v 0 0 0"
            for ($i=0; $i -lt 24; $i++) { $a = [Math]::PI*2*$i/24; $verts += "v $([Math]::Round([Math]::Cos($a)*$W,4)) $([Math]::Round([Math]::Sin($a)*$W,4)) 0" }
            for ($i=2; $i -le 24; $i++) { $faces += "f 1 $i $($i+1)" }
            $faces += "f 1 25 2"
        }
        default {
            $hw = $W/2; $hh = $H/2
            $verts += "v -$hw -$hh 0"; $verts += "v $hw -$hh 0"; $verts += "v $hw $hh 0"; $verts += "v -$hw $hh 0"
            $faces += "f 1 2 3 4"
        }
    }
    $obj = "o $name`n" + (($verts + @("vn 0 0 1") + $faces) -join "`n") + "`n"
    Write-Utf8 $Path $obj
    return (Write-ObjMeta $Path)
}

function New-PreviewPng([string]$Path, [string]$Title, [System.Drawing.Color]$A, [System.Drawing.Color]$B, [string]$Motif) {
    $bmp = [System.Drawing.Bitmap]::new(512, 512)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $brush = [System.Drawing.Drawing2D.LinearGradientBrush]::new([System.Drawing.Rectangle]::new(0,0,512,512), $A, $B, 45)
    $g.FillRectangle($brush, 0, 0, 512, 512)
    $penDark = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(230, 38, 29, 22), 10)
    $penHot = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(240, 255, 116, 37), 8)
    $brass = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(230, 172, 124, 48))
    $iron = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(230, 42, 45, 42))
    if ($Motif -eq "gauge") {
        $g.FillEllipse($iron, 96, 76, 320, 320); $g.FillEllipse($brass, 118, 98, 276, 276); $g.DrawLine($penHot, 256, 238, 346, 166)
    } elseif ($Motif -eq "lamp") {
        $g.FillRectangle($iron, 182, 76, 148, 344); $g.FillEllipse([System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(230,255,79,32)), 166, 160, 180, 180)
    } elseif ($Motif -eq "scorch") {
        for ($i=0; $i -lt 9; $i++) { $g.FillEllipse([System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(50+$i*18,18,14,11)), 70+$i*18, 122+$i*9, 370-$i*22, 210-$i*12) }
    } else {
        $g.FillRectangle($iron, 92, 126, 328, 220); $g.DrawRectangle($penHot, 118, 152, 276, 168); $g.FillEllipse($brass, 224, 200, 64, 64)
    }
    $font = [System.Drawing.Font]::new("Arial", 22, [System.Drawing.FontStyle]::Bold)
    $sf = [System.Drawing.StringFormat]::new(); $sf.Alignment = "Center"
    $g.DrawString($Title, $font, [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(240, 248, 226, 182)), [System.Drawing.RectangleF]::new(28, 424, 456, 58), $sf)
    Ensure-Dir (Split-Path -Parent $Path)
    $bmp.Save($Path, [System.Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose(); $bmp.Dispose()
    Write-TextureMeta $Path | Out-Null
}

function New-Part([string]$Name, [string]$Mesh, [string]$MatGuid, [float[]]$Pos, [float[]]$Scale, [float[]]$Rot = @(0,0,0)) {
    return [pscustomobject]@{ Name=$Name; Mesh=$Mesh; MatGuid=$MatGuid; Pos=$Pos; Scale=$Scale; Rot=$Rot }
}
function Write-Prefab([string]$Path, [array]$Parts) {
    $rootGo = Get-Random -Min 100000 -Max 999999
    $rootTr = Get-Random -Min 100000 -Max 999999
    $prefabName = [System.IO.Path]::GetFileNameWithoutExtension($Path)
    $yaml = "%YAML 1.1`n%TAG !u! tag:unity3d.com,2011:`n--- !u!1 &$rootGo`nGameObject:`n  m_ObjectHideFlags: 0`n  m_CorrespondingSourceObject: {fileID: 0}`n  m_PrefabInstance: {fileID: 0}`n  m_PrefabAsset: {fileID: 0}`n  serializedVersion: 6`n  m_Component:`n  - component: {fileID: $rootTr}`n  m_Layer: 0`n  m_Name: $prefabName`n  m_TagString: Untagged`n  m_Icon: {fileID: 0}`n  m_NavMeshLayer: 0`n  m_StaticEditorFlags: 0`n  m_IsActive: 1`n--- !u!4 &$rootTr`nTransform:`n  m_ObjectHideFlags: 0`n  m_CorrespondingSourceObject: {fileID: 0}`n  m_PrefabInstance: {fileID: 0}`n  m_PrefabAsset: {fileID: 0}`n  m_GameObject: {fileID: $rootGo}`n  serializedVersion: 2`n  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}`n  m_LocalPosition: {x: 0, y: 0, z: 0}`n  m_LocalScale: {x: 1, y: 1, z: 1}`n  m_ConstrainProportionsScale: 0`n  m_Children:`n"
    $childBlocks = ""
    foreach ($p in $Parts) {
        $go=Get-Random -Min 1000000 -Max 9999999; $tr=Get-Random -Min 1000000 -Max 9999999; $mf=Get-Random -Min 1000000 -Max 9999999; $mr=Get-Random -Min 1000000 -Max 9999999
        $yaml += "  - {fileID: $tr}`n"
        $meshId = @{Cube=10202; Cylinder=10206; Sphere=10207; Capsule=10208; Plane=10209; Quad=10210}[$p.Mesh]
        if (-not $meshId) { $meshId = 10202 }
        $childBlocks += "--- !u!1 &$go`nGameObject:`n  m_ObjectHideFlags: 0`n  m_CorrespondingSourceObject: {fileID: 0}`n  m_PrefabInstance: {fileID: 0}`n  m_PrefabAsset: {fileID: 0}`n  serializedVersion: 6`n  m_Component:`n  - component: {fileID: $tr}`n  - component: {fileID: $mf}`n  - component: {fileID: $mr}`n  m_Layer: 0`n  m_Name: $($p.Name)`n  m_TagString: Untagged`n  m_Icon: {fileID: 0}`n  m_NavMeshLayer: 0`n  m_StaticEditorFlags: 0`n  m_IsActive: 1`n--- !u!4 &$tr`nTransform:`n  m_ObjectHideFlags: 0`n  m_CorrespondingSourceObject: {fileID: 0}`n  m_PrefabInstance: {fileID: 0}`n  m_PrefabAsset: {fileID: 0}`n  m_GameObject: {fileID: $go}`n  serializedVersion: 2`n  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}`n  m_LocalPosition: {x: $($p.Pos[0]), y: $($p.Pos[1]), z: $($p.Pos[2])}`n  m_LocalScale: {x: $($p.Scale[0]), y: $($p.Scale[1]), z: $($p.Scale[2])}`n  m_ConstrainProportionsScale: 0`n  m_Children: []`n  m_Father: {fileID: $rootTr}`n  m_LocalEulerAnglesHint: {x: $($p.Rot[0]), y: $($p.Rot[1]), z: $($p.Rot[2])}`n--- !u!33 &$mf`nMeshFilter:`n  m_ObjectHideFlags: 0`n  m_CorrespondingSourceObject: {fileID: 0}`n  m_PrefabInstance: {fileID: 0}`n  m_PrefabAsset: {fileID: 0}`n  m_GameObject: {fileID: $go}`n  m_Mesh: {fileID: $meshId, guid: 0000000000000000e000000000000000, type: 0}`n--- !u!23 &$mr`nMeshRenderer:`n  m_ObjectHideFlags: 0`n  m_CorrespondingSourceObject: {fileID: 0}`n  m_PrefabInstance: {fileID: 0}`n  m_PrefabAsset: {fileID: 0}`n  m_GameObject: {fileID: $go}`n  m_Enabled: 1`n  m_CastShadows: 1`n  m_ReceiveShadows: 1`n  m_DynamicOccludee: 1`n  m_StaticShadowCaster: 0`n  m_MotionVectors: 1`n  m_LightProbeUsage: 1`n  m_ReflectionProbeUsage: 1`n  m_RayTracingMode: 2`n  m_RayTraceProcedural: 0`n  m_RenderingLayerMask: 1`n  m_RendererPriority: 0`n  m_Materials:`n  - {fileID: 2100000, guid: $($p.MatGuid), type: 2}`n  m_StaticBatchInfo:`n    firstSubMesh: 0`n    subMeshCount: 0`n  m_StaticBatchRoot: {fileID: 0}`n  m_ProbeAnchor: {fileID: 0}`n  m_LightProbeVolumeOverride: {fileID: 0}`n  m_ScaleInLightmap: 1`n  m_ReceiveGI: 1`n  m_PreserveUVs: 0`n  m_IgnoreNormalsForChartDetection: 0`n  m_ImportantGI: 0`n  m_StitchLightmapSeams: 1`n  m_SelectedEditorRenderState: 3`n  m_MinimumChartSize: 4`n  m_AutoUVMaxDistance: 0.5`n  m_AutoUVMaxAngle: 89`n  m_LightmapParameters: {fileID: 0}`n  m_SortingLayerID: 0`n  m_SortingLayer: 0`n  m_SortingOrder: 0`n"
    }
    $yaml += "  m_Father: {fileID: 0}`n  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}`n" + $childBlocks
    Write-Utf8 $Path $yaml
    Write-Meta $Path | Out-Null
}

Remove-Item -LiteralPath $PackageRoot -Recurse -Force -ErrorAction SilentlyContinue
foreach ($dir in @($PackageRoot,$ProductionRoot,$RenderRoot,$PlanningRoot,$QaRoot)) { Ensure-Dir $dir }
foreach ($dir in @("Runtime","Runtime\Materials","Runtime\Meshes","Runtime\Prefabs","Runtime\Previews","Runtime\Metadata","Documentation~","Documentation~\Manifest","Samples~","Samples~\PrefabPalette")) {
    $full = Join-Path $PackageRoot $dir
    Ensure-Dir $full
    if ($dir -notlike "*~*") { Write-Meta $full (New-HexGuid) $true | Out-Null }
}

$materials = @(
    @{n="AgedBrassHeatStained"; c=@{r=.77;g=.48;b=.18;a=1}; m=.82; s=.48},
    @{n="BlackenedBoilerIron"; c=@{r=.05;g=.055;b=.05;a=1}; m=.7; s=.24},
    @{n="WarningEnamelRed"; c=@{r=.83;g=.08;b=.035;a=1}; m=.1; s=.36},
    @{n="WarningEnamelCream"; c=@{r=.92;g=.76;b=.43;a=1}; m=.08; s=.32},
    @{n="GlowValveAmber"; c=@{r=1;g=.38;b=.06;a=1}; m=.0; s=.58},
    @{n="GlowPressureCyan"; c=@{r=.06;g=.77;b=1;a=1}; m=.0; s=.52},
    @{n="CharredSoot"; c=@{r=.015;g=.012;b=.01;a=1}; m=.0; s=.08},
    @{n="OilWetEdge"; c=@{r=.025;g=.02;b=.015;a=1}; m=.2; s=.75},
    @{n="OxidizedCopperGreen"; c=@{r=.12;g=.43;b=.36;a=1}; m=.66; s=.3},
    @{n="CrackedCeramicInsulator"; c=@{r=.71;g=.64;b=.52;a=1}; m=.0; s=.18},
    @{n="GaugeGlassSmoked"; c=@{r=.35;g=.48;b=.5;a=.42}; m=.0; s=.86},
    @{n="RivetDarkSteel"; c=@{r=.18;g=.18;b=.16;a=1}; m=.75; s=.34},
    @{n="HazardStripeBlack"; c=@{r=.02;g=.018;b=.014;a=1}; m=.1; s=.2},
    @{n="HazardStripeAmber"; c=@{r=1;g=.58;b=.06;a=1}; m=.05; s=.38},
    @{n="SteamBoundaryWhite"; c=@{r=.83;g=.82;b=.74;a=1}; m=.02; s=.22},
    @{n="ScorchedCopperEdge"; c=@{r=.48;g=.16;b=.055;a=1}; m=.55; s=.26}
)
$matGuids = @{}
foreach ($mat in $materials) {
    $matGuids[$mat.n] = Write-Material (Join-Path $PackageRoot "Runtime\Materials\$Code`_MAT_$($mat.n).mat") $mat.c $mat.m $mat.s
}

$meshDefs = @(
    @("OverheatedBoilerFacePlate","octplate",1.0,1.0), @("PressureWarningPlate","quad",1.6,.8),
    @("ValveLockDial","gauge",.6,.6), @("CrackedPipeWrapBand","quad",1.2,.35),
    @("FloorScorchCard","quad",1.7,1.1), @("RailLampBackPlate","quad",.55,1.2),
    @("PistonPinchMarker","quad",.85,.85), @("GaugeClusterDisc","gauge",.45,.45),
    @("SteamBoundaryMarkerPanel","quad",.48,1.65), @("ChevronWarningTab","quad",.42,.16),
    @("RivetWasherDisc","gauge",.13,.13), @("LongHeatCrackShard","quad",.12,.9)
)
$meshGuids = @{}
foreach ($m in $meshDefs) {
    $meshGuids[$m[0]] = Write-ObjMesh (Join-Path $PackageRoot "Runtime\Meshes\$Code`_MESH_$($m[0]).obj") $m[1] $m[2] $m[3]
}

$prefabSpecs = @(
    @("OverheatedBoilerFace_A", @("AgedBrassHeatStained","BlackenedBoilerIron","GlowValveAmber")),
    @("OverheatedBoilerFace_B", @("BlackenedBoilerIron","ScorchedCopperEdge","WarningEnamelRed")),
    @("OverheatedBoilerFace_C", @("AgedBrassHeatStained","CharredSoot","GlowPressureCyan")),
    @("PressureWarningPlate_RectA", @("WarningEnamelRed","WarningEnamelCream","RivetDarkSteel")),
    @("PressureWarningPlate_RectB", @("WarningEnamelCream","HazardStripeBlack","AgedBrassHeatStained")),
    @("PressureWarningPlate_Octagon", @("WarningEnamelRed","BlackenedBoilerIron","HazardStripeAmber")),
    @("GlowingValveLock_Amber", @("AgedBrassHeatStained","GlowValveAmber","RivetDarkSteel")),
    @("GlowingValveLock_Cyan", @("OxidizedCopperGreen","GlowPressureCyan","GaugeGlassSmoked")),
    @("GlowingValveLock_RedSeal", @("BlackenedBoilerIron","WarningEnamelRed","GlowValveAmber")),
    @("CrackedPipeWrap_Short", @("CrackedCeramicInsulator","RivetDarkSteel","CharredSoot")),
    @("CrackedPipeWrap_Long", @("CrackedCeramicInsulator","OilWetEdge","ScorchedCopperEdge")),
    @("CrackedPipeWrap_Clamped", @("AgedBrassHeatStained","CrackedCeramicInsulator","HazardStripeBlack")),
    @("FloorScorchCard_Round", @("CharredSoot","ScorchedCopperEdge","OilWetEdge")),
    @("FloorScorchCard_Arc", @("CharredSoot","WarningEnamelAmber","OilWetEdge")),
    @("FloorScorchCard_Directional", @("ScorchedCopperEdge","HazardStripeAmber","CharredSoot")),
    @("RailWarningLamp_Single", @("BlackenedBoilerIron","GlowValveAmber","AgedBrassHeatStained")),
    @("RailWarningLamp_Caged", @("RivetDarkSteel","GlowValveAmber","WarningEnamelRed")),
    @("RailWarningLamp_Double", @("BlackenedBoilerIron","GlowPressureCyan","AgedBrassHeatStained")),
    @("PistonPinchMarker_Chevron", @("HazardStripeAmber","HazardStripeBlack","RivetDarkSteel")),
    @("PistonPinchMarker_TwinPlate", @("WarningEnamelRed","WarningEnamelCream","RivetDarkSteel")),
    @("PistonPinchMarker_Corner", @("HazardStripeBlack","HazardStripeAmber","AgedBrassHeatStained")),
    @("PressureGaugeCluster_Triple", @("AgedBrassHeatStained","GaugeGlassSmoked","WarningEnamelRed")),
    @("PressureGaugeCluster_Quad", @("BlackenedBoilerIron","GaugeGlassSmoked","GlowPressureCyan")),
    @("PressureGaugeCluster_Alarm", @("WarningEnamelRed","GaugeGlassSmoked","GlowValveAmber")),
    @("SteamBurnBoundary_PostA", @("SteamBoundaryWhite","WarningEnamelRed","RivetDarkSteel")),
    @("SteamBurnBoundary_PostB", @("AgedBrassHeatStained","SteamBoundaryWhite","GlowValveAmber")),
    @("SteamBurnBoundary_ChainMarker", @("HazardStripeAmber","HazardStripeBlack","SteamBoundaryWhite")),
    @("HazardReadability_KitbashSampler", @("AgedBrassHeatStained","WarningEnamelRed","GlowValveAmber"))
)

foreach ($spec in $prefabSpecs) {
    $a=$matGuids[$spec[1][0]]; if (-not $a) { $a=$matGuids["HazardStripeAmber"] }
    $b=$matGuids[$spec[1][1]]; if (-not $b) { $b=$matGuids["HazardStripeAmber"] }
    $c=$matGuids[$spec[1][2]]; if (-not $c) { $c=$matGuids["HazardStripeAmber"] }
    $parts = @(
        (New-Part "BackPlate" "Cube" $a @(0,0,0) @(1.35,.08,.75)),
        (New-Part "ReadableFace" "Cube" $b @(0,.055,0) @(1.06,.035,.52)),
        (New-Part "SignalLens" "Sphere" $c @(0,.11,0) @(.23,.23,.23)),
        (New-Part "LeftRivet" "Sphere" $matGuids["RivetDarkSteel"] @(-.52,.12,-.24) @(.08,.08,.08)),
        (New-Part "RightRivet" "Sphere" $matGuids["RivetDarkSteel"] @(.52,.12,.24) @(.08,.08,.08))
    )
    if ($spec[0] -like "*GaugeCluster*") {
        $parts += (New-Part "GaugeLeft" "Cylinder" $matGuids["GaugeGlassSmoked"] @(-.32,.16,0) @(.28,.035,.28))
        $parts += (New-Part "GaugeRight" "Cylinder" $matGuids["GaugeGlassSmoked"] @(.32,.16,0) @(.28,.035,.28))
    }
    if ($spec[0] -like "*Lamp*") {
        $parts += (New-Part "LampCageVerticalA" "Cube" $matGuids["RivetDarkSteel"] @(-.24,.18,0) @(.035,.42,.035))
        $parts += (New-Part "LampCageVerticalB" "Cube" $matGuids["RivetDarkSteel"] @(.24,.18,0) @(.035,.42,.035))
    }
    if ($spec[0] -like "*Boundary*") {
        $parts += (New-Part "TallMarkerPost" "Cube" $matGuids["SteamBoundaryWhite"] @(0,.34,0) @(.18,.8,.18))
    }
    Write-Prefab (Join-Path $PackageRoot "Runtime\Prefabs\$Code`_PREFAB_$($spec[0]).prefab") $parts
}

$previewSpecs = $prefabSpecs | Select-Object -First 24
$i=0
foreach ($spec in $previewSpecs) {
    $i++
    $motif = if ($spec[0] -like "*Gauge*") {"gauge"} elseif ($spec[0] -like "*Lamp*") {"lamp"} elseif ($spec[0] -like "*Scorch*") {"scorch"} else {"plate"}
    $pngName = "{0}_PREVIEW_{1:00}_{2}.png" -f $Code,$i,$spec[0]
    New-PreviewPng (Join-Path $PackageRoot "Runtime\Previews\$pngName") $spec[0] ([System.Drawing.Color]::FromArgb(64,53,43)) ([System.Drawing.Color]::FromArgb(135,79,31)) $motif
    Copy-Item -LiteralPath (Join-Path $PackageRoot "Runtime\Previews\$pngName") -Destination (Join-Path $RenderRoot $pngName) -Force
    Copy-Item -LiteralPath (Join-Path $PackageRoot "Runtime\Previews\$pngName.meta") -Destination (Join-Path $RenderRoot "$pngName.meta") -Force
}

$packageJson = @{
    name=$PackageId; version=$Version; displayName="Brassworks Breach Hazard Props Set 06";
    description="Unity-only sidecar package of visual steampunk environmental hazard/readability props: boiler heat faces, warning plates, glowing valve locks, cracked pipe wraps, scorch cards, rail lamps, pinch markers, pressure gauges, and steam-burn boundary markers. Visual assets only; no gameplay authority.";
    unity="6000.4"; author=@{name="Brassworks Breach Sidecar Lane"}; dependencies=@{};
    keywords=@("brassworks","sidecar","hazard-props","readability","steampunk","visual-only");
    samples=@(@{displayName="Prefab Palette Notes"; description="Review notes for the visual-only hazard prop prefab palette."; path="Samples~/PrefabPalette"})
} | ConvertTo-Json -Depth 8
Write-Utf8 (Join-Path $PackageRoot "package.json") $packageJson
Write-Meta (Join-Path $PackageRoot "package.json") | Out-Null

$readme = @"
# Brassworks Breach Hazard Props Set 06

Package: `$PackageId`

Visual-only Unity sidecar for steampunk hazard/readability props. The pack is import-safe under `VisualOnly_HazardPropsSet06` and deliberately contains no gameplay scripts, colliders, rigidbodies, cameras, scene files, or audio. Gameplay authority remains in main-scene AUTH objects during later integration.

Contents:

- 28 visual-only prefabs across overheated boiler faces, pressure warning plates, glowing valve locks, cracked pipe wraps, floor scorch cards, rail warning lamps, piston pinch markers, pressure gauge clusters, and steam-burn boundary markers.
- 16 reusable Standard materials tuned for brass, iron, warning enamel, glow accents, soot, oil, oxidized copper, glass, and scorched edges.
- 12 reusable low-poly OBJ mesh/card assets for future replacement or prefab-family extension.
- 24 generated preview PNGs mirrored to `Documentation/ConceptRenders/V0_1_50_HazardPropsSet06`.

Performance notes:

- Primitive-composed prefabs use small child counts and shared materials.
- Preview textures are 512 px, no mipmaps, and not required at runtime.
- No animation, physics, lighting, cameras, or audio are included.
"@
Write-Utf8 (Join-Path $PackageRoot "README.md") $readme
Write-Meta (Join-Path $PackageRoot "README.md") | Out-Null
Write-Utf8 (Join-Path $PackageRoot "CHANGELOG.md") "# Changelog`n`n## $Version`n`n- Initial isolated visual-only hazard/readability prop sidecar.`n"
Write-Meta (Join-Path $PackageRoot "CHANGELOG.md") | Out-Null
Write-Utf8 (Join-Path $PackageRoot "Samples~\PrefabPalette\README.md") "# Prefab Palette Notes`n`nImport the package into an empty validation project and inspect `Runtime/Prefabs`. Do not drag these into production scenes until integration creates scene-local AUTH gameplay objects.`n"

$assetFiles = Get-ChildItem -LiteralPath $PackageRoot -Recurse -File | Where-Object { $_.Extension -notin ".meta" }
$manifest = [ordered]@{
    packageName=$PackageName; packageId=$PackageId; version=$Version; unity="6000.4";
    importRootRecommendation="Assets/VisualOnly_HazardPropsSet06";
    visualOnly=$true; gameplayAuthority="main-scene AUTH objects later";
    counts=[ordered]@{
        prefabs=(Get-ChildItem -LiteralPath (Join-Path $PackageRoot "Runtime\Prefabs") -Filter *.prefab).Count;
        materials=(Get-ChildItem -LiteralPath (Join-Path $PackageRoot "Runtime\Materials") -Filter *.mat).Count;
        meshes=(Get-ChildItem -LiteralPath (Join-Path $PackageRoot "Runtime\Meshes") -Filter *.obj).Count;
        previews=(Get-ChildItem -LiteralPath (Join-Path $PackageRoot "Runtime\Previews") -Filter *.png).Count
    };
    forbiddenContent=@("gameplay scripts","colliders","rigidbodies","cameras","scene files","audio");
    assets=($assetFiles | ForEach-Object { $_.FullName.Replace($PackageRoot + "\","").Replace("\","/") })
}
$manifestJson = $manifest | ConvertTo-Json -Depth 8
Write-Utf8 (Join-Path $PackageRoot "Runtime\Metadata\$Code`_HazardPropsSet06Manifest_v0.1.50.json") $manifestJson
Write-Meta (Join-Path $PackageRoot "Runtime\Metadata\$Code`_HazardPropsSet06Manifest_v0.1.50.json") | Out-Null
Write-Utf8 (Join-Path $PackageRoot "Documentation~\Manifest\$Code`_HazardPropsSet06Manifest_v0.1.50.json") $manifestJson

$inventoryLines = @("# HP06 Asset Inventory", "", "| Type | Count |", "| --- | ---: |")
$inventoryLines += "| Prefabs | $($manifest.counts.prefabs) |"
$inventoryLines += "| Materials | $($manifest.counts.materials) |"
$inventoryLines += "| Reusable OBJ Meshes | $($manifest.counts.meshes) |"
$inventoryLines += "| Preview PNGs | $($manifest.counts.previews) |"
$inventoryLines += ""
$inventoryLines += "## Prefabs"
$inventoryLines += (Get-ChildItem -LiteralPath (Join-Path $PackageRoot "Runtime\Prefabs") -Filter *.prefab | Sort-Object Name | ForEach-Object { "- $($_.Name)" })
$inventoryLines += ""
$inventoryLines += "## Materials"
$inventoryLines += (Get-ChildItem -LiteralPath (Join-Path $PackageRoot "Runtime\Materials") -Filter *.mat | Sort-Object Name | ForEach-Object { "- $($_.Name)" })
$inventoryLines += ""
$inventoryLines += "## Meshes"
$inventoryLines += (Get-ChildItem -LiteralPath (Join-Path $PackageRoot "Runtime\Meshes") -Filter *.obj | Sort-Object Name | ForEach-Object { "- $($_.Name)" })
Write-Utf8 (Join-Path $ProductionRoot "$Code`_AssetInventory_v0.1.50.md") ($inventoryLines -join "`n")

$production = @"
# Hazard Props Set 06 Production Report

Created isolated Unity package `$PackageName` at `$PackageRoot`.

Art direction: stylized steampunk brass/iron/oil hazard readability. The set favors bold silhouettes, hot amber/red/cyan signal accents, readable warning enamel, scorched floor cards, and compact primitive-composed meshes for mid/low Windows PCs.

Import contract:

- Suggested import location: `Assets/VisualOnly_HazardPropsSet06`.
- Visual only. No gameplay scripts, colliders, rigidbodies, cameras, scene files, or audio.
- Main-scene AUTH objects retain gameplay authority later.
- Reusable materials and mesh cards are isolated in package `Runtime`.
"@
Write-Utf8 (Join-Path $ProductionRoot "$Code`_ProductionReport_v0.1.50.md") $production
Write-Utf8 (Join-Path $ProductionRoot "README.md") "# Hazard Props Set 06 Production Notes`n`nReview `HP06_ProductionReport_v0.1.50.md`, `HP06_AssetInventory_v0.1.50.md`, and validation files in the QA import-readiness folder.`n"

$planning = @"
# Hazard Props Set 06 Import Readiness Plan

- Import package under `Assets/VisualOnly_HazardPropsSet06` only after sidecar review.
- Keep prefabs visual-only; pair with main-scene AUTH hazard gameplay objects during integration.
- Use rail lamps and valve locks as readable affordance markers, not as logic holders.
- Use floor scorch cards and steam-burn boundary markers for no-go readability near later trigger volumes.
"@
Write-Utf8 (Join-Path $PlanningRoot "$Code`_ImportReadinessPlan_v0.1.50.md") $planning

$forbidden = @("Collider","Rigidbody","Camera:","AudioSource","MonoBehaviour")
$prefabText = Get-ChildItem -LiteralPath (Join-Path $PackageRoot "Runtime\Prefabs") -Filter *.prefab | ForEach-Object { Get-Content -LiteralPath $_.FullName -Raw }
$forbiddenHits = foreach ($term in $forbidden) { if (($prefabText -join "`n") -match [regex]::Escape($term)) { $term } }
$forbiddenExts = @(".unity",".wav",".mp3",".ogg",".cs")
$sceneFiles = Get-ChildItem -LiteralPath $PackageRoot -Recurse -File | Where-Object { $forbiddenExts -contains $_.Extension.ToLowerInvariant() }
$validation = [ordered]@{
    packageName=$PackageName; packageId=$PackageId; version=$Version;
    checkedAt=(Get-Date).ToString("o");
    results=[ordered]@{
        rootIsAssigned=$PackageRoot.StartsWith((Join-Path $ProjectRoot "AssetPacks\$PackageName"));
        min24Prefabs=$manifest.counts.prefabs -ge 24;
        min14Materials=$manifest.counts.materials -ge 14;
        min10Meshes=$manifest.counts.meshes -ge 10;
        min24Previews=$manifest.counts.previews -ge 24;
        noForbiddenPrefabComponents=($forbiddenHits.Count -eq 0);
        noSceneScriptAudioFiles=($sceneFiles.Count -eq 0);
        packageJsonPresent=(Test-Path (Join-Path $PackageRoot "package.json"));
        manifestJsonPresent=(Test-Path (Join-Path $PackageRoot "Documentation~\Manifest\$Code`_HazardPropsSet06Manifest_v0.1.50.json"))
    };
    counts=$manifest.counts;
    forbiddenHits=@($forbiddenHits);
    forbiddenFiles=@($sceneFiles | ForEach-Object { $_.FullName })
}
$validationJson = $validation | ConvertTo-Json -Depth 8
Write-Utf8 (Join-Path $ProductionRoot "$Code`_SidecarValidation_v0.1.50.json") $validationJson
Write-Utf8 (Join-Path $QaRoot "$Code`_ImportReadinessValidation_v0.1.50.json") $validationJson
$evidence = @"
# HP06 Import Readiness Validation Evidence

Command:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File "$ProductionRoot\GenerateHazardPropsSet06.ps1"
```

Results:

- Prefabs: $($manifest.counts.prefabs)
- Materials: $($manifest.counts.materials)
- Reusable OBJ meshes: $($manifest.counts.meshes)
- Preview PNGs: $($manifest.counts.previews)
- Forbidden prefab components found: $($forbiddenHits.Count)
- Scene/script/audio files found: $($sceneFiles.Count)

Status: $(if (($validation.results.Values | Where-Object { $_ -ne $true }).Count -eq 0) { "PASS" } else { "REVIEW REQUIRED" })
"@
Write-Utf8 (Join-Path $QaRoot "$Code`_ValidationEvidence_v0.1.50.md") $evidence

Write-Output $validationJson
