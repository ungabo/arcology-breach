Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
[System.Threading.Thread]::CurrentThread.CurrentCulture = [System.Globalization.CultureInfo]::InvariantCulture
[System.Threading.Thread]::CurrentThread.CurrentUICulture = [System.Globalization.CultureInfo]::InvariantCulture

$DocRoot = "D:\__MY APPS\Unity Doom\Documentation\Planning\V0_1_34_EnemyReadabilityPolish"
$AssetRoot = "D:\__MY APPS\Unity Doom\Assets\_Project\ArtStaging\V0_1_34_EnemyReadabilityPolish"
$MeshRoot = Join-Path $AssetRoot "Meshes"
$MaterialRoot = Join-Path $AssetRoot "Materials"
$PreviewRoot = Join-Path $AssetRoot "Previews"
$MetadataRoot = Join-Path $AssetRoot "Metadata"

$OwnedRoots = @(
    [System.IO.Path]::GetFullPath($DocRoot).TrimEnd('\'),
    [System.IO.Path]::GetFullPath($AssetRoot).TrimEnd('\')
)

function Assert-OwnedPath {
    param([Parameter(Mandatory = $true)][string]$Path)
    $full = [System.IO.Path]::GetFullPath($Path)
    foreach ($root in $OwnedRoots) {
        if ($full.Equals($root, [System.StringComparison]::OrdinalIgnoreCase) -or
            $full.StartsWith($root + "\", [System.StringComparison]::OrdinalIgnoreCase)) {
            return
        }
    }
    throw "Refusing to write outside owned v0.1.34 enemy readability polish scopes: $full"
}

function Ensure-Directory {
    param([Parameter(Mandatory = $true)][string]$Path)
    Assert-OwnedPath $Path
    if (-not (Test-Path -Path $Path -PathType Container)) {
        New-Item -ItemType Directory -Force -Path $Path | Out-Null
    }
}

function Write-TextFile {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [Parameter(Mandatory = $true)][string]$Content
    )
    Assert-OwnedPath $Path
    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllText($Path, $Content, $utf8NoBom)
}

function Get-GuidHex {
    param([Parameter(Mandatory = $true)][string]$Identity)
    $md5 = [System.Security.Cryptography.MD5]::Create()
    try {
        $bytes = [System.Text.Encoding]::UTF8.GetBytes("V0_1_34_EnemyReadabilityPolish|" + $Identity.ToLowerInvariant())
        return (($md5.ComputeHash($bytes) | ForEach-Object { $_.ToString("x2") }) -join "")
    }
    finally {
        $md5.Dispose()
    }
}

function Write-FolderMeta {
    param([Parameter(Mandatory = $true)][string]$FolderPath)
    $metaPath = $FolderPath + ".meta"
    $guid = Get-GuidHex $FolderPath
    $content = @"
fileFormatVersion: 2
guid: $guid
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData:
  assetBundleName:
  assetBundleVariant:
"@
    Write-TextFile -Path $metaPath -Content $content
}

function Write-DefaultMeta {
    param([Parameter(Mandatory = $true)][string]$AssetPath)
    $guid = Get-GuidHex $AssetPath
    $content = @"
fileFormatVersion: 2
guid: $guid
DefaultImporter:
  externalObjects: {}
  userData:
  assetBundleName:
  assetBundleVariant:
"@
    Write-TextFile -Path ($AssetPath + ".meta") -Content $content
}

function Write-NativeMeta {
    param([Parameter(Mandatory = $true)][string]$AssetPath)
    $guid = Get-GuidHex $AssetPath
    $content = @"
fileFormatVersion: 2
guid: $guid
NativeFormatImporter:
  externalObjects: {}
  mainObjectFileID: 2100000
  userData:
  assetBundleName:
  assetBundleVariant:
"@
    Write-TextFile -Path ($AssetPath + ".meta") -Content $content
}

$Materials = @(
    [ordered]@{ Name = "MAT_V0134_SilhouetteInk"; Role = "dark silhouette mass and armor read"; Kd = @(0.060, 0.058, 0.052); Ks = @(0.140, 0.135, 0.120); Color = @(0.060, 0.058, 0.052, 1); Metallic = 0.65; Gloss = 0.32; Emission = @(0, 0, 0, 1) },
    [ordered]@{ Name = "MAT_V0134_AgedBrassEdge"; Role = "brass edge trims, bands, cage ribs, and retainers"; Kd = @(0.760, 0.510, 0.190); Ks = @(0.410, 0.330, 0.190); Color = @(0.760, 0.510, 0.190, 1); Metallic = 0.70; Gloss = 0.46; Emission = @(0, 0, 0, 1) },
    [ordered]@{ Name = "MAT_V0134_CopperPressure"; Role = "copper pressure pipes, rods, and barrels"; Kd = @(0.560, 0.260, 0.125); Ks = @(0.310, 0.215, 0.135); Color = @(0.560, 0.260, 0.125, 1); Metallic = 0.64; Gloss = 0.40; Emission = @(0, 0, 0, 1) },
    [ordered]@{ Name = "MAT_V0134_CreamFacePlate"; Role = "cream enamel identity face plates"; Kd = @(0.735, 0.665, 0.490); Ks = @(0.180, 0.160, 0.120); Color = @(0.735, 0.665, 0.490, 1); Metallic = 0.00; Gloss = 0.30; Emission = @(0, 0, 0, 1) },
    [ordered]@{ Name = "MAT_V0134_FurnaceEyeAmber"; Role = "hot amber identity eyes"; Kd = @(1.000, 0.380, 0.055); Ks = @(0.250, 0.135, 0.050); Color = @(1.000, 0.380, 0.055, 1); Metallic = 0.00; Gloss = 0.58; Emission = @(1.15, 0.32, 0.04, 1) },
    [ordered]@{ Name = "MAT_V0134_WeakPointLampGold"; Role = "larger gold weak-point lamp glass"; Kd = @(1.000, 0.770, 0.130); Ks = @(0.300, 0.180, 0.080); Color = @(1.000, 0.770, 0.130, 1); Metallic = 0.00; Gloss = 0.64; Emission = @(1.35, 0.72, 0.10, 1) },
    [ordered]@{ Name = "MAT_V0134_PressureTankRed"; Role = "red pressure vessel read, not a pickup material"; Kd = @(0.590, 0.060, 0.040); Ks = @(0.220, 0.080, 0.060); Color = @(0.590, 0.060, 0.040, 1); Metallic = 0.48; Gloss = 0.36; Emission = @(0, 0, 0, 1) },
    [ordered]@{ Name = "MAT_V0134_BoltTellCyan"; Role = "cyan bolt charge, ranged tell, and command coil language"; Kd = @(0.070, 0.570, 1.000); Ks = @(0.160, 0.340, 0.500); Color = @(0.070, 0.570, 1.000, 1); Metallic = 0.00; Gloss = 0.70; Emission = @(0.08, 0.82, 1.55, 1) },
    [ordered]@{ Name = "MAT_V0134_HazardStrikeYellow"; Role = "yellow active danger on cutter teeth, hammer faces, and shield warnings"; Kd = @(0.960, 0.670, 0.080); Ks = @(0.220, 0.180, 0.080); Color = @(0.960, 0.670, 0.080, 1); Metallic = 0.00; Gloss = 0.32; Emission = @(0, 0, 0, 1) },
    [ordered]@{ Name = "MAT_V0134_ShutdownDimMetal"; Role = "dim broken shutdown fragments"; Kd = @(0.210, 0.185, 0.150); Ks = @(0.080, 0.070, 0.060); Color = @(0.210, 0.185, 0.150, 1); Metallic = 0.32; Gloss = 0.20; Emission = @(0, 0, 0, 1) },
    [ordered]@{ Name = "MAT_V0134_SootCavity"; Role = "soot-black visor and depth cavities"; Kd = @(0.018, 0.017, 0.015); Ks = @(0.040, 0.035, 0.030); Color = @(0.018, 0.017, 0.015, 1); Metallic = 0.00; Gloss = 0.12; Emission = @(0, 0, 0, 1) }
)

function Write-MaterialAssets {
    $mtl = [System.Collections.Generic.List[string]]::new()
    $mtl.Add("# Brassworks Breach v0.1.34 enemy readability polish proxy materials")
    $mtl.Add("# OBJ/MTL colors are staging proxies only. Preserve material names for first-pass remap.")
    $mtl.Add("")

    foreach ($mat in $Materials) {
        $mtl.Add("newmtl $($mat.Name)")
        $mtl.Add("# $($mat.Role)")
        $mtl.Add(("Ka {0:0.0000} {1:0.0000} {2:0.0000}" -f ($mat.Kd[0] * 0.12), ($mat.Kd[1] * 0.12), ($mat.Kd[2] * 0.12)))
        $mtl.Add(("Kd {0:0.0000} {1:0.0000} {2:0.0000}" -f $mat.Kd[0], $mat.Kd[1], $mat.Kd[2]))
        $mtl.Add(("Ks {0:0.0000} {1:0.0000} {2:0.0000}" -f $mat.Ks[0], $mat.Ks[1], $mat.Ks[2]))
        if (($mat.Emission[0] + $mat.Emission[1] + $mat.Emission[2]) -gt 0) {
            $mtl.Add(("Ke {0:0.0000} {1:0.0000} {2:0.0000}" -f $mat.Emission[0], $mat.Emission[1], $mat.Emission[2]))
        }
        $mtl.Add("Ns 38")
        $mtl.Add("illum 2")
        $mtl.Add("")

        $keywords = "[]"
        $lightmapFlags = 4
        if (($mat.Emission[0] + $mat.Emission[1] + $mat.Emission[2]) -gt 0) {
            $keywords = "`n  - _EMISSION"
            $lightmapFlags = 1
        }
        $matPath = Join-Path $MaterialRoot ($mat.Name + ".mat")
        $content = @"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!21 &2100000
Material:
  serializedVersion: 8
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_Name: $($mat.Name)
  m_Shader: {fileID: 46, guid: 0000000000000000f000000000000000, type: 0}
  m_Parent: {fileID: 0}
  m_ModifiedSerializedProperties: 0
  m_ValidKeywords: $keywords
  m_InvalidKeywords: []
  m_LightmapFlags: $lightmapFlags
  m_EnableInstancingVariants: 0
  m_DoubleSidedGI: 0
  m_CustomRenderQueue: -1
  stringTagMap: {}
  disabledShaderPasses: []
  m_LockedProperties:
  m_SavedProperties:
    serializedVersion: 3
    m_TexEnvs:
    - _MainTex:
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _EmissionMap:
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    m_Ints: []
    m_Floats:
    - _Glossiness: $($mat.Gloss)
    - _Metallic: $($mat.Metallic)
    - _Mode: 0
    m_Colors:
    - _Color: {r: $($mat.Color[0]), g: $($mat.Color[1]), b: $($mat.Color[2]), a: $($mat.Color[3])}
    - _EmissionColor: {r: $($mat.Emission[0]), g: $($mat.Emission[1]), b: $($mat.Emission[2]), a: $($mat.Emission[3])}
  m_BuildTextureStacks: []
"@
        Write-TextFile -Path $matPath -Content $content
        Write-NativeMeta -AssetPath $matPath
    }

    $mtlPath = Join-Path $MaterialRoot "ENEMY_V0134_ReadabilityPolishMaterials.mtl"
    Write-TextFile -Path $mtlPath -Content ($mtl -join "`r`n")
    Write-DefaultMeta -AssetPath $mtlPath
}

$script:ObjLines = $null
$script:VertexCount = 0
$script:FaceCount = 0

function Start-Obj {
    param([string]$ObjectName)
    $script:ObjLines = [System.Collections.Generic.List[string]]::new()
    $script:VertexCount = 0
    $script:FaceCount = 0
    $script:ObjLines.Add("# Brassworks Breach v0.1.34 enemy readability polish staging mesh")
    $script:ObjLines.Add("# Units: meters. Axis: +Y up, +Z forward. Overlay/readability proxy only.")
    $script:ObjLines.Add("mtllib ../Materials/ENEMY_V0134_ReadabilityPolishMaterials.mtl")
    $script:ObjLines.Add("o $ObjectName")
    $script:ObjLines.Add("")
}

function Save-Obj {
    param([string]$Path)
    Write-TextFile -Path $Path -Content ($script:ObjLines -join "`r`n")
    return [ordered]@{ vertices = $script:VertexCount; faces = $script:FaceCount }
}

function Add-Vertex {
    param([double]$X, [double]$Y, [double]$Z)
    $script:ObjLines.Add(("v {0:0.00000} {1:0.00000} {2:0.00000}" -f $X, $Y, $Z))
    $script:VertexCount++
    return $script:VertexCount
}

function Add-Face {
    param([int[]]$Ids)
    $script:ObjLines.Add("f " + ($Ids -join " "))
    $script:FaceCount++
}

function Add-Group {
    param([string]$Name, [string]$Material)
    $script:ObjLines.Add("")
    $script:ObjLines.Add("g $Name")
    $script:ObjLines.Add("usemtl $Material")
}

function Add-Box {
    param(
        [string]$Name,
        [string]$Material,
        [double]$Cx, [double]$Cy, [double]$Cz,
        [double]$Sx, [double]$Sy, [double]$Sz
    )
    Add-Group $Name $Material
    $x0 = $Cx - $Sx / 2; $x1 = $Cx + $Sx / 2
    $y0 = $Cy - $Sy / 2; $y1 = $Cy + $Sy / 2
    $z0 = $Cz - $Sz / 2; $z1 = $Cz + $Sz / 2
    $v = @(
        (Add-Vertex $x0 $y0 $z0), (Add-Vertex $x1 $y0 $z0), (Add-Vertex $x1 $y1 $z0), (Add-Vertex $x0 $y1 $z0),
        (Add-Vertex $x0 $y0 $z1), (Add-Vertex $x1 $y0 $z1), (Add-Vertex $x1 $y1 $z1), (Add-Vertex $x0 $y1 $z1)
    )
    Add-Face @($v[0], $v[1], $v[2], $v[3])
    Add-Face @($v[4], $v[7], $v[6], $v[5])
    Add-Face @($v[0], $v[4], $v[5], $v[1])
    Add-Face @($v[1], $v[5], $v[6], $v[2])
    Add-Face @($v[2], $v[6], $v[7], $v[3])
    Add-Face @($v[3], $v[7], $v[4], $v[0])
}

function Add-Cylinder {
    param(
        [string]$Name,
        [string]$Material,
        [ValidateSet("X", "Y", "Z")][string]$Axis,
        [double]$Cx, [double]$Cy, [double]$Cz,
        [double]$Radius,
        [double]$Length,
        [int]$Segments = 12
    )
    Add-Group $Name $Material
    $a = [System.Collections.Generic.List[int]]::new()
    $b = [System.Collections.Generic.List[int]]::new()
    for ($i = 0; $i -lt $Segments; $i++) {
        $t = (2.0 * [Math]::PI * $i) / $Segments
        $ct = [Math]::Cos($t)
        $st = [Math]::Sin($t)
        if ($Axis -eq "Y") {
            $a.Add((Add-Vertex ($Cx + $Radius * $ct) ($Cy - $Length / 2) ($Cz + $Radius * $st)))
            $b.Add((Add-Vertex ($Cx + $Radius * $ct) ($Cy + $Length / 2) ($Cz + $Radius * $st)))
        }
        elseif ($Axis -eq "Z") {
            $a.Add((Add-Vertex ($Cx + $Radius * $ct) ($Cy + $Radius * $st) ($Cz - $Length / 2)))
            $b.Add((Add-Vertex ($Cx + $Radius * $ct) ($Cy + $Radius * $st) ($Cz + $Length / 2)))
        }
        else {
            $a.Add((Add-Vertex ($Cx - $Length / 2) ($Cy + $Radius * $ct) ($Cz + $Radius * $st)))
            $b.Add((Add-Vertex ($Cx + $Length / 2) ($Cy + $Radius * $ct) ($Cz + $Radius * $st)))
        }
    }
    for ($i = 0; $i -lt $Segments; $i++) {
        $n = ($i + 1) % $Segments
        Add-Face @($a[$i], $a[$n], $b[$n], $b[$i])
    }
    $capA = @($a.ToArray())
    [Array]::Reverse($capA)
    Add-Face $capA
    Add-Face @($b.ToArray())
}

function Add-EyePair {
    param([string]$Prefix, [double]$Y, [double]$Z, [double]$Spacing = 0.12, [double]$Radius = 0.035)
    Add-Cylinder "$($Prefix)_left_furnace_identity_eye" "MAT_V0134_FurnaceEyeAmber" "Z" (-$Spacing / 2) $Y $Z $Radius 0.030 12
    Add-Cylinder "$($Prefix)_right_furnace_identity_eye" "MAT_V0134_FurnaceEyeAmber" "Z" ($Spacing / 2) $Y $Z $Radius 0.030 12
}

function Add-WeakLamp {
    param([string]$Name, [double]$X, [double]$Y, [double]$Z, [double]$Radius = 0.075)
    Add-Cylinder "$($Name)_black_ring" "MAT_V0134_SilhouetteInk" "Z" $X $Y $Z ($Radius * 1.24) 0.035 14
    Add-Cylinder "$($Name)_gold_glass" "MAT_V0134_WeakPointLampGold" "Z" $X $Y ($Z + 0.028) $Radius 0.026 14
}

function Add-Tank {
    param([string]$Name, [double]$X, [double]$Y, [double]$Z, [ValidateSet("X", "Y", "Z")][string]$Axis, [double]$Length = 0.52, [double]$Radius = 0.075)
    Add-Cylinder "$($Name)_red_pressure_vessel" "MAT_V0134_PressureTankRed" $Axis $X $Y $Z $Radius $Length 12
    $capX = $X
    $capY = $Y
    $capZ = $Z
    if ($Axis -eq "X") {
        $capX = $X - ($Length / 2 + 0.020)
    }
    elseif ($Axis -eq "Y") {
        $capY = $Y - ($Length / 2 + 0.020)
    }
    else {
        $capZ = $Z - ($Length / 2 + 0.020)
    }
    Add-Cylinder "$($Name)_brass_cap_a" "MAT_V0134_AgedBrassEdge" $Axis $capX $capY $capZ ($Radius * 0.86) 0.040 12
}

function Build-Scrapper {
    Start-Obj "ENEMY_V0134_Scrapper_ReadabilityOverlay"
    Add-Box "scrapper_hunched_low_center_silhouette_core" "MAT_V0134_SilhouetteInk" 0 0.92 0.00 0.60 0.82 0.36
    Add-Box "scrapper_forward_cream_mask_plate" "MAT_V0134_CreamFacePlate" 0 1.42 0.25 0.42 0.22 0.10
    Add-Box "scrapper_soot_eye_slit_depth" "MAT_V0134_SootCavity" 0 1.42 0.315 0.31 0.050 0.035
    Add-EyePair "scrapper" 1.42 0.345 0.14 0.033
    Add-WeakLamp "scrapper_chest_weak_point_separate_from_eyes" 0 1.02 0.235 0.080
    Add-Tank "scrapper_left_rear" -0.26 1.08 -0.26 "Y" 0.58 0.070
    Add-Tank "scrapper_right_rear" 0.26 1.08 -0.26 "Y" 0.58 0.070
    Add-Cylinder "scrapper_right_cutter_arc_round_tell" "MAT_V0134_HazardStrikeYellow" "Z" -0.47 0.87 0.35 0.155 0.045 16
    Add-Cylinder "scrapper_right_cutter_dark_hub" "MAT_V0134_SilhouetteInk" "Z" -0.47 0.87 0.385 0.070 0.030 12
    Add-Box "scrapper_left_hammer_block_square_tell" "MAT_V0134_HazardStrikeYellow" 0.47 0.86 0.33 0.24 0.28 0.24
    Add-Box "scrapper_uneven_arm_asymmetry_keeper" "MAT_V0134_AgedBrassEdge" 0 0.88 0.21 1.04 0.08 0.08
    return Save-Obj (Join-Path $MeshRoot "ENEMY_V0134_Scrapper_ReadabilityOverlay.obj")
}

function Build-Lancer {
    Start-Obj "ENEMY_V0134_Lancer_ReadabilityOverlay"
    Add-Box "lancer_tall_thin_ranged_silhouette_core" "MAT_V0134_SilhouetteInk" 0 1.20 0.00 0.34 1.10 0.28
    Add-Box "lancer_narrow_cream_face_plate" "MAT_V0134_CreamFacePlate" 0 1.86 0.23 0.28 0.19 0.095
    Add-Box "lancer_soot_narrow_eye_slit" "MAT_V0134_SootCavity" 0 1.86 0.295 0.22 0.042 0.030
    Add-EyePair "lancer" 1.86 0.325 0.10 0.028
    Add-WeakLamp "lancer_sternum_weak_point_below_weapon" 0 1.28 0.235 0.070
    Add-Tank "lancer_back_charge" 0 1.23 -0.29 "Y" 0.72 0.070
    Add-Cylinder "lancer_uninterrupted_forward_lance_line" "MAT_V0134_CopperPressure" "Z" 0 1.40 0.86 0.045 1.55 12
    Add-Cylinder "lancer_muzzle_blue_bolt_core_not_weakpoint" "MAT_V0134_BoltTellCyan" "Z" 0 1.40 1.66 0.080 0.060 14
    foreach ($z in @(0.38, 0.58, 0.78, 0.98)) {
        Add-Cylinder ("lancer_sequence_bolt_charge_ring_z_{0:0.00}" -f $z) "MAT_V0134_BoltTellCyan" "Z" 0 1.40 $z 0.115 0.025 16
    }
    Add-Box "lancer_shoulders_kept_small_to_protect_lance_read" "MAT_V0134_AgedBrassEdge" 0 1.45 0.19 0.56 0.075 0.075
    return Save-Obj (Join-Path $MeshRoot "ENEMY_V0134_Lancer_ReadabilityOverlay.obj")
}

function Build-Bulwark {
    Start-Obj "ENEMY_V0134_Bulwark_ReadabilityOverlay"
    Add-Box "bulwark_broad_defender_silhouette_core" "MAT_V0134_SilhouetteInk" 0 1.05 -0.02 0.82 0.90 0.35
    Add-Box "bulwark_front_shield_door_first_read" "MAT_V0134_SilhouetteInk" 0 1.04 0.31 1.22 1.08 0.12
    Add-Box "bulwark_brass_shield_top_edge" "MAT_V0134_AgedBrassEdge" 0 1.61 0.39 1.30 0.055 0.07
    Add-Box "bulwark_brass_shield_bottom_edge" "MAT_V0134_AgedBrassEdge" 0 0.47 0.39 1.30 0.055 0.07
    Add-Box "bulwark_left_hazard_shield_stripe" "MAT_V0134_HazardStrikeYellow" -0.31 1.04 0.45 0.13 0.78 0.030
    Add-Box "bulwark_right_hazard_shield_stripe" "MAT_V0134_HazardStrikeYellow" 0.31 1.04 0.45 0.13 0.78 0.030
    Add-WeakLamp "bulwark_left_side_weak_point_visible_around_shield" -0.53 1.03 0.47 0.072
    Add-WeakLamp "bulwark_right_side_weak_point_visible_around_shield" 0.53 1.03 0.47 0.072
    Add-Box "bulwark_cream_brow_face_above_shield" "MAT_V0134_CreamFacePlate" 0 1.70 0.20 0.50 0.17 0.10
    Add-Box "bulwark_soot_brow_eye_slot" "MAT_V0134_SootCavity" 0 1.70 0.27 0.38 0.040 0.030
    Add-EyePair "bulwark" 1.70 0.300 0.11 0.030
    Add-Tank "bulwark_left_shoulder_pressure" -0.58 1.36 -0.23 "Y" 0.62 0.080
    Add-Tank "bulwark_right_shoulder_pressure" 0.58 1.36 -0.23 "Y" 0.62 0.080
    Add-Box "bulwark_right_hammer_slam_tell_visible_past_shield" "MAT_V0134_HazardStrikeYellow" 0.88 0.88 0.28 0.28 0.32 0.28
    return Save-Obj (Join-Path $MeshRoot "ENEMY_V0134_Bulwark_ReadabilityOverlay.obj")
}

function Build-Warden {
    Start-Obj "ENEMY_V0134_Warden_ReadabilityOverlay"
    Add-Cylinder "warden_tall_governor_tower_silhouette_core" "MAT_V0134_SilhouetteInk" "Y" 0 1.24 -0.02 0.235 1.28 18
    Add-Cylinder "warden_lower_brass_cage_ring" "MAT_V0134_AgedBrassEdge" "Y" 0 0.66 -0.02 0.290 0.045 18
    Add-Cylinder "warden_mid_brass_cage_ring" "MAT_V0134_AgedBrassEdge" "Y" 0 1.23 -0.02 0.290 0.040 18
    Add-Cylinder "warden_upper_brass_cage_ring" "MAT_V0134_AgedBrassEdge" "Y" 0 1.80 -0.02 0.290 0.045 18
    foreach ($x in @(-0.24, -0.12, 0.12, 0.24)) {
        Add-Box ("warden_vertical_cage_rib_x_{0:0.00}" -f $x) "MAT_V0134_AgedBrassEdge" $x 1.23 0.225 0.030 1.10 0.045
    }
    Add-Box "warden_command_cream_face_plate" "MAT_V0134_CreamFacePlate" 0 1.95 0.205 0.39 0.24 0.105
    Add-Box "warden_soot_command_visor" "MAT_V0134_SootCavity" 0 1.96 0.275 0.28 0.045 0.030
    Add-EyePair "warden" 1.96 0.305 0.095 0.028
    Add-WeakLamp "warden_central_command_weak_point_on_tower" 0 1.28 0.295 0.076
    Add-Tank "warden_left_crown_pressure" -0.22 2.12 -0.08 "X" 0.34 0.058
    Add-Tank "warden_right_crown_pressure" 0.22 2.12 -0.08 "X" 0.34 0.058
    Add-Cylinder "warden_overhead_black_bolt_spine" "MAT_V0134_SilhouetteInk" "Z" 0 2.13 0.16 0.035 0.54 10
    foreach ($z in @(-0.04, 0.10, 0.24, 0.38)) {
        Add-Cylinder ("warden_overhead_cyan_command_coil_z_{0:0.00}" -f $z) "MAT_V0134_BoltTellCyan" "Z" 0 2.13 $z 0.105 0.025 16
    }
    return Save-Obj (Join-Path $MeshRoot "ENEMY_V0134_Warden_ReadabilityOverlay.obj")
}

function Build-ShutdownFragments {
    Start-Obj "FRAG_V0134_EnemyShutdownCueFragments"
    Add-Box "scrapper_dim_cutter_tooth_shutdown_piece" "MAT_V0134_HazardStrikeYellow" -0.48 0.08 0.00 0.18 0.08 0.13
    Add-Box "scrapper_dim_hammer_face_shutdown_piece" "MAT_V0134_HazardStrikeYellow" -0.28 0.08 0.00 0.18 0.09 0.15
    Add-Cylinder "lancer_dim_bolt_coil_shutdown_piece" "MAT_V0134_BoltTellCyan" "Z" -0.06 0.08 0.00 0.080 0.030 12
    Add-Box "bulwark_dim_shield_hinge_shutdown_piece" "MAT_V0134_ShutdownDimMetal" 0.18 0.08 0.00 0.22 0.08 0.12
    Add-Box "warden_dim_cage_rib_shutdown_piece" "MAT_V0134_AgedBrassEdge" 0.40 0.09 0.00 0.06 0.20 0.08
    Add-Cylinder "shared_broken_weak_lamp_lens_shutdown_piece" "MAT_V0134_WeakPointLampGold" "Z" 0.58 0.08 0.02 0.055 0.025 12
    return Save-Obj (Join-Path $MeshRoot "FRAG_V0134_EnemyShutdownCueFragments.obj")
}

function Write-PreviewSheet {
    Add-Type -AssemblyName System.Drawing
    $path = Join-Path $PreviewRoot "PREVIEW_V0134_EnemyReadabilityPolish_SwatchSheet.png"
    Assert-OwnedPath $path
    $w = 1700
    $h = 1150
    $bmp = [System.Drawing.Bitmap]::new($w, $h)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $bg = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 23, 24, 23))
    $panel = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 35, 37, 36))
    $text = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 232, 226, 204))
    $muted = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 170, 166, 148))
    $line = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(255, 96, 96, 88), 2)
    $fontTitle = [System.Drawing.Font]::new("Segoe UI", 28, [System.Drawing.FontStyle]::Bold)
    $font = [System.Drawing.Font]::new("Segoe UI", 15, [System.Drawing.FontStyle]::Regular)
    $fontSmall = [System.Drawing.Font]::new("Segoe UI", 11, [System.Drawing.FontStyle]::Regular)

    try {
        $g.FillRectangle($bg, 0, 0, $w, $h)
        $g.DrawString("Brassworks Breach v0.1.34 - Enemy Readability Polish", $fontTitle, $text, 42, 34)
        $g.DrawString("Overlay proxies for silhouette, tell language, weak-point separation, and material remap planning.", $font, $muted, 46, 82)

        $enemies = @(
            @{ Name = "Scrapper"; Role = "short melee"; Tell = "cutter arc + hammer block"; Weak = "single chest lamp"; X = 55; Y = 145 },
            @{ Name = "Lancer"; Role = "tall ranged"; Tell = "long lance + cyan coils"; Weak = "sternum lamp"; X = 875; Y = 145 },
            @{ Name = "Bulwark"; Role = "broad defender"; Tell = "shield door + hammer"; Weak = "side lamps"; X = 55; Y = 560 },
            @{ Name = "Warden"; Role = "command tower"; Tell = "cage + overhead coils"; Weak = "tower lamp"; X = 875; Y = 560 }
        )
        foreach ($enemy in $enemies) {
            $x = [int]$enemy.X
            $y = [int]$enemy.Y
            $g.FillRectangle($panel, $x, $y, 760, 340)
            $g.DrawRectangle($line, $x, $y, 760, 340)
            $g.DrawString($enemy.Name, $fontTitle, $text, $x + 24, $y + 20)
            $g.DrawString($enemy.Role, $font, $muted, $x + 28, $y + 68)
            $g.DrawString("tell: " + $enemy.Tell, $fontSmall, $muted, $x + 28, $y + 288)
            $g.DrawString("weak: " + $enemy.Weak, $fontSmall, $muted, $x + 330, $y + 288)

            $cx = $x + 390
            $base = $y + 260
            $ink = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 44, 43, 39))
            $brass = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 194, 130, 48))
            $cream = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 188, 170, 125))
            $amber = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 255, 153, 26))
            $gold = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 255, 196, 34))
            $red = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 151, 20, 17))
            $cyan = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 24, 150, 255))
            $hazard = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 245, 171, 20))

            if ($enemy.Name -eq "Scrapper") {
                $g.FillRectangle($ink, $cx - 80, $base - 150, 160, 150)
                $g.FillRectangle($cream, $cx - 58, $base - 200, 116, 42)
                $g.FillEllipse($gold, $cx - 28, $base - 94, 56, 56)
                $g.FillEllipse($hazard, $cx - 190, $base - 120, 86, 86)
                $g.FillRectangle($hazard, $cx + 110, $base - 126, 76, 86)
                $g.FillRectangle($red, $cx - 130, $base - 170, 34, 100)
                $g.FillRectangle($red, $cx + 96, $base - 170, 34, 100)
            }
            elseif ($enemy.Name -eq "Lancer") {
                $g.FillRectangle($ink, $cx - 44, $base - 220, 88, 220)
                $g.FillRectangle($cream, $cx - 34, $base - 270, 68, 38)
                $g.FillEllipse($gold, $cx - 20, $base - 140, 40, 40)
                $g.FillRectangle($brass, $cx + 25, $base - 155, 260, 20)
                foreach ($ringX in @(($cx + 70), ($cx + 115), ($cx + 160), ($cx + 205))) {
                    $g.DrawEllipse([System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(255, 24, 150, 255), 5), $ringX, $base - 178, 30, 68)
                }
                $g.FillEllipse($cyan, $cx + 274, $base - 174, 48, 48)
            }
            elseif ($enemy.Name -eq "Bulwark") {
                $g.FillRectangle($ink, $cx - 150, $base - 220, 300, 220)
                $g.FillRectangle($brass, $cx - 164, $base - 236, 328, 18)
                $g.FillRectangle($hazard, $cx - 82, $base - 198, 34, 155)
                $g.FillRectangle($hazard, $cx + 48, $base - 198, 34, 155)
                $g.FillEllipse($gold, $cx - 180, $base - 135, 48, 48)
                $g.FillEllipse($gold, $cx + 132, $base - 135, 48, 48)
                $g.FillRectangle($cream, $cx - 65, $base - 282, 130, 38)
                $g.FillRectangle($hazard, $cx + 185, $base - 120, 78, 84)
            }
            else {
                $towerTop = $y + 116
                $towerBottom = $y + 262
                $faceY = $y + 74
                $crownY = $y + 46
                $coilY = $y + 68
                $g.FillRectangle($ink, $cx - 56, $towerTop, 112, ($towerBottom - $towerTop))
                foreach ($ribX in @(($cx - 74), ($cx - 35), ($cx + 35), ($cx + 74))) {
                    $g.FillRectangle($brass, $ribX, ($towerTop + 6), 10, ($towerBottom - $towerTop - 12))
                }
                $g.FillRectangle($cream, $cx - 62, $faceY, 124, 46)
                $g.FillEllipse($gold, $cx - 26, ($towerTop + 66), 52, 52)
                $g.FillRectangle($red, $cx - 92, $crownY, 62, 24)
                $g.FillRectangle($red, $cx + 30, $crownY, 62, 24)
                foreach ($ringY in @($coilY, ($coilY + 24), ($coilY + 48), ($coilY + 72))) {
                    $g.DrawEllipse([System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(255, 24, 150, 255), 4), $cx - 42, $ringY, 84, 18)
                }
            }
            $eyeY = $base - 224
            if ($enemy.Name -eq "Warden") {
                $eyeY = $y + 96
            }
            $g.FillEllipse($amber, $cx - 34, $eyeY, 16, 16)
            $g.FillEllipse($amber, $cx + 18, $eyeY, 16, 16)
            foreach ($brush in @($ink, $brass, $cream, $amber, $gold, $red, $cyan, $hazard)) { $brush.Dispose() }
        }

        $swatchY = 1000
        $swatchX = 58
        $g.DrawString("Material swatches", $font, $text, $swatchX, $swatchY - 38)
        foreach ($mat in $Materials) {
            $color = [System.Drawing.Color]::FromArgb(255, [int]($mat.Kd[0] * 255), [int]($mat.Kd[1] * 255), [int]($mat.Kd[2] * 255))
            $brush = [System.Drawing.SolidBrush]::new($color)
            $g.FillRectangle($brush, $swatchX, $swatchY, 72, 42)
            $g.DrawRectangle($line, $swatchX, $swatchY, 72, 42)
            $g.DrawString(($mat.Name -replace "MAT_V0134_", ""), $fontSmall, $muted, $swatchX, $swatchY + 50)
            $brush.Dispose()
            $swatchX += 145
        }

        $bmp.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    }
    finally {
        foreach ($resource in @($fontTitle, $font, $fontSmall, $bg, $panel, $text, $muted, $line, $g, $bmp)) {
            if ($null -ne $resource) { $resource.Dispose() }
        }
    }
}

function Write-DocsAndMetadata {
    param([hashtable]$Stats)

    $manifest = [ordered]@{
        package = "V0_1_34_EnemyReadabilityPolish"
        version = "0.1.34"
        date = "2026-05-24"
        purpose = "Enemy readability staging package for Scrapper, Lancer, Bulwark, and Warden tell language, silhouettes, and material remap planning."
        sourceReviewed = @(
            "Assets/_Project/ArtStaging/EnemyReadabilityBatch",
            "Documentation/AssetProduction/EnemyReadabilityBatch"
        )
        unityContract = [ordered]@{
            units = "1 Unity unit = 1 meter"
            axis = "+Y up, +Z forward"
            importPath = "Assets/_Project/ArtStaging/V0_1_34_EnemyReadabilityPolish"
            generatedGeometry = "OBJ/MTL overlay proxies, Unity Standard material proxies, PNG swatch sheet"
            excluded = @("gameplay code", "scene builder", "colliders", "rigging", "Blender dependency", "generated scenes", "shared integration docs")
        }
        files = [ordered]@{
            integrationBrief = "Documentation/Planning/V0_1_34_EnemyReadabilityPolish/V0_1_34_EnemyReadability_IntegrationBrief.md"
            acceptanceGates = "Documentation/Planning/V0_1_34_EnemyReadabilityPolish/V0_1_34_EnemyReadability_AcceptanceGates.md"
            docManifest = "Documentation/Planning/V0_1_34_EnemyReadabilityPolish/V0_1_34_EnemyReadability_Manifest.json"
            assetManifest = "Assets/_Project/ArtStaging/V0_1_34_EnemyReadabilityPolish/Metadata/V0_1_34_EnemyReadability_Manifest.json"
            materialSet = "Assets/_Project/ArtStaging/V0_1_34_EnemyReadabilityPolish/Materials/ENEMY_V0134_ReadabilityPolishMaterials.mtl"
            preview = "Assets/_Project/ArtStaging/V0_1_34_EnemyReadabilityPolish/Previews/PREVIEW_V0134_EnemyReadabilityPolish_SwatchSheet.png"
        }
        enemies = @(
            [ordered]@{ id = "Scrapper"; overlayMesh = "Meshes/ENEMY_V0134_Scrapper_ReadabilityOverlay.obj"; intent = "compact melee worker-machine"; criticalReads = @("hunched low mass", "cutter arc right side", "hammer block left side", "separate chest weak lamp", "paired amber furnace eyes"); stats = $Stats.Scrapper },
            [ordered]@{ id = "Lancer"; overlayMesh = "Meshes/ENEMY_V0134_Lancer_ReadabilityOverlay.obj"; intent = "tall ranged automaton"; criticalReads = @("thin upright core", "uninterrupted +Z lance line", "cyan bolt rings", "sternum weak lamp below weapon", "muzzle tell distinct from weak point"); stats = $Stats.Lancer },
            [ordered]@{ id = "Bulwark"; overlayMesh = "Meshes/ENEMY_V0134_Bulwark_ReadabilityOverlay.obj"; intent = "broad shield defender"; criticalReads = @("shield-door first read", "side weak lamps around shield", "hammer visible past shield", "brow eyes above shield", "shoulder pressure tanks"); stats = $Stats.Bulwark },
            [ordered]@{ id = "Warden"; overlayMesh = "Meshes/ENEMY_V0134_Warden_ReadabilityOverlay.obj"; intent = "tall command/cage unit"; criticalReads = @("cage tower silhouette", "crown pressure tanks", "overhead cyan command coils", "central tower weak lamp", "command face above body"); stats = $Stats.Warden }
        )
        shutdownCueFragments = [ordered]@{
            mesh = "Meshes/FRAG_V0134_EnemyShutdownCueFragments.obj"
            intent = "shared small breakaway tell kit for shutdown/death readability planning"
            stats = $Stats.ShutdownFragments
        }
        materials = $Materials | ForEach-Object { [ordered]@{ name = $_.Name; role = $_.Role } }
        acceptanceSummary = @(
            "Enemy identity must pass silhouette-only review before material review.",
            "Furnace eyes, weak lamps, pressure tanks, and attack tells must remain visually separate.",
            "Blue/cyan is reserved for Lancer and Warden bolt/command charge language.",
            "No gameplay behavior, hitboxes, rigging, colliders, or scene edits are implied by this package."
        )
    }
    $manifestJson = $manifest | ConvertTo-Json -Depth 8
    Write-TextFile -Path (Join-Path $DocRoot "V0_1_34_EnemyReadability_Manifest.json") -Content $manifestJson
    Write-TextFile -Path (Join-Path $MetadataRoot "V0_1_34_EnemyReadability_Manifest.json") -Content $manifestJson
    Write-DefaultMeta -AssetPath (Join-Path $MetadataRoot "V0_1_34_EnemyReadability_Manifest.json")

    $readme = @'
# V0.1.34 Enemy Readability Polish

Date: 2026-05-24
Package state: staging package for main-lane integration
Scope: v0.1.34 enemy readability polish only

This package turns the earlier `EnemyReadabilityBatch` vocabulary into a smaller integration-facing overlay set. It is meant to help the main lane make one large playable-art leap without waiting on final rigs.

Matching Unity staging payload:

`Assets/_Project/ArtStaging/V0_1_34_EnemyReadabilityPolish/`

## Contents

- `V0_1_34_EnemyReadability_IntegrationBrief.md` - concise handoff and integration order.
- `V0_1_34_EnemyReadability_AcceptanceGates.md` - visual gates and stop conditions.
- `V0_1_34_EnemyReadability_Manifest.json` - machine-readable package index.
- `Tools/Generate-V0_1_34-EnemyReadabilityPolish.ps1` - reproducible local generator.
- Unity staging folder with OBJ overlay meshes, MTL/material proxies, metadata, and a PNG swatch sheet.

## Guardrails

- No gameplay code.
- No scene builder edits.
- No generated scene edits.
- No colliders, rigging, or final animation.
- No Blender dependency.
- No shared integration or status files touched.
'@
    Write-TextFile -Path (Join-Path $DocRoot "README.md") -Content $readme

    $brief = @'
# V0.1.34 Enemy Readability Polish Integration Brief

Date: 2026-05-24
Package: `V0_1_34_EnemyReadabilityPolish`

## Goal

Make Scrapper, Lancer, Bulwark, and Warden readable in combat before final rigging by staging a compact overlay package for silhouette, tell language, weak-point separation, and material remap planning.

## Reviewed Source

- `Assets/_Project/ArtStaging/EnemyReadabilityBatch/`
- `Documentation/AssetProduction/EnemyReadabilityBatch/`

## Integration Shape

Use this package as a visual overlay/reference layer, not as a final enemy replacement. The OBJ meshes are intentionally lightweight and should be placed beside or temporarily parented under current enemy prefabs for lookdev review.

Recommended child organization for a prefab trial:

- `readability_silhouette_proxy`
- `readability_attack_tells`
- `readability_weak_points_visual_only`
- `readability_pressure_language`
- `readability_shutdown_fragments_visual_only`

## Enemy Direction

Scrapper should read as the short melee unit: hunched, low, asymmetric, with a round cutter tell on one side and a block hammer tell on the other. Keep the chest weak lamp separate from the paired furnace eyes.

Lancer should read as the tall ranged unit: thin body, uninterrupted +Z lance line, cyan charge rings, and a muzzle cue that cannot be confused with the sternum weak lamp.

Bulwark should read as the broad defender: shield-door mass first, brow eyes above the shield, side weak lamps visible around the shield, and a hammer tell visible past the shield edge.

Warden should read as the command unit: tall cage/tower profile, brass ribs, crown pressure tanks, central tower weak lamp, and overhead cyan command coils. It must not collapse into a taller Lancer.

## Import Notes

- OBJ units are meters.
- Axis is `+Y` up and `+Z` forward.
- Keep mesh colliders disabled.
- Preserve material names for first-pass remapping.
- Treat all weak-point lamps as visual affordances only until gameplay owns the rule.

## Top Integration Recommendations

1. Do a silhouette-only graybox pass first, then enable material colors.
2. Keep weak-point lamps physically separate from furnace eyes and muzzle/bolt effects.
3. Reserve cyan/blue only for Lancer and Warden charge language.
4. Preserve Scrapper asymmetry and Bulwark side-lamp visibility during rig or prefab cleanup.
5. Use shutdown fragments as small dim breakaway cues, not pickups or interactables.
'@
    Write-TextFile -Path (Join-Path $DocRoot "V0_1_34_EnemyReadability_IntegrationBrief.md") -Content $brief

    $gates = @'
# V0.1.34 Enemy Readability Acceptance Gates

Date: 2026-05-24

## Global Gates

- [ ] Unity imports all five OBJ files without missing material errors.
- [ ] `ENEMY_V0134_ReadabilityPolishMaterials.mtl` is present and material names remain readable.
- [ ] No mesh colliders are generated during import.
- [ ] The package can be reviewed without rigging, animation, gameplay code, or scene builder changes.
- [ ] The PNG swatch sheet is visible in the Project window for quick reference.
- [ ] The manifest JSON in `Metadata/` matches the planning manifest.

## Silhouette Gates

- [ ] Scrapper reads as short, compact, hunched melee before color/material review.
- [ ] Lancer reads as tall, thin, ranged, and forward-facing before color/material review.
- [ ] Bulwark reads as broad, shielded, and defensive before color/material review.
- [ ] Warden reads as tall, caged, and command-oriented before color/material review.

## Tell Gates

- [ ] Scrapper keeps one circular cutter tell and one block hammer tell.
- [ ] Lancer keeps a clean uninterrupted +Z lance direction line.
- [ ] Lancer cyan rings read as pre-fire charge, not weak-point targets.
- [ ] Bulwark side weak lamps remain visible around the shield mass.
- [ ] Bulwark hammer tell remains visible past the shield edge.
- [ ] Warden overhead cyan coils read as command/charge language, not a second lance.

## Material Gates

- [ ] Dark silhouette material remains the dominant armor/silhouette value.
- [ ] Brass is used for readable edges, cage ribs, trims, and retainers.
- [ ] Copper is used for pressure transfer and weapon-barrel language.
- [ ] Cream enamel is limited to face/identity plates.
- [ ] Amber furnace eyes are distinct from gold weak-point lamps.
- [ ] Red tanks read as pressure vessels, not guaranteed pickups or explosive loot.
- [ ] Cyan/blue is reserved for Lancer and Warden bolt/command charge cues.
- [ ] Dim shutdown pieces read as inactive debris.

## Stop Conditions

Stop integration and return to staging if any of these happen:

- Any enemy loses identity in flat lighting.
- Weak-point lamps merge visually with furnace eyes, muzzle fire, or general decoration.
- The Lancer becomes bulky enough to read as melee.
- The Warden reads as only a taller Lancer.
- The Bulwark shield hides every readable damage affordance.
- Cyan/blue appears on Scrapper or Bulwark attack tells.
- Shutdown fragments read as pickups, ammo, or interactables.
'@
    Write-TextFile -Path (Join-Path $DocRoot "V0_1_34_EnemyReadability_AcceptanceGates.md") -Content $gates

    $importNotes = @'
# V0.1.34 Enemy Readability Polish Import Notes

Unity path: `Assets/_Project/ArtStaging/V0_1_34_EnemyReadabilityPolish/`

## Contract

- Overlay/readability proxies only.
- OBJ units are meters.
- Axis is `+Y` up and `+Z` forward.
- Keep mesh colliders disabled.
- Preserve material names for lookdev remap.
- Do not infer gameplay hitboxes, damage rules, AI behavior, or rig requirements from these files.

## Primary Files

- `Meshes/ENEMY_V0134_Scrapper_ReadabilityOverlay.obj`
- `Meshes/ENEMY_V0134_Lancer_ReadabilityOverlay.obj`
- `Meshes/ENEMY_V0134_Bulwark_ReadabilityOverlay.obj`
- `Meshes/ENEMY_V0134_Warden_ReadabilityOverlay.obj`
- `Meshes/FRAG_V0134_EnemyShutdownCueFragments.obj`
- `Materials/ENEMY_V0134_ReadabilityPolishMaterials.mtl`
- `Materials/MAT_V0134_*.mat`
- `Previews/PREVIEW_V0134_EnemyReadabilityPolish_SwatchSheet.png`
- `Metadata/V0_1_34_EnemyReadability_Manifest.json`

## Main-Lane Use

Parent the overlay OBJ under current enemy prefabs only long enough to review silhouette and tell placement. Replace or merge the geometry by hand during the final art pass.
'@
    Write-TextFile -Path (Join-Path $MetadataRoot "V0_1_34_ImportNotes.md") -Content $importNotes
    Write-DefaultMeta -AssetPath (Join-Path $MetadataRoot "V0_1_34_ImportNotes.md")
}

foreach ($path in @($DocRoot, (Join-Path $DocRoot "Tools"), $AssetRoot, $MeshRoot, $MaterialRoot, $PreviewRoot, $MetadataRoot)) {
    Ensure-Directory $path
}
foreach ($folder in @($MeshRoot, $MaterialRoot, $PreviewRoot, $MetadataRoot)) {
    Write-FolderMeta $folder
}

Write-MaterialAssets

$stats = @{}
$stats.Scrapper = Build-Scrapper
$stats.Lancer = Build-Lancer
$stats.Bulwark = Build-Bulwark
$stats.Warden = Build-Warden
$stats.ShutdownFragments = Build-ShutdownFragments

Write-PreviewSheet
Write-DocsAndMetadata $stats

Write-Host "Generated v0.1.34 enemy readability polish package."
Write-Host "Docs: $DocRoot"
Write-Host "Assets: $AssetRoot"
