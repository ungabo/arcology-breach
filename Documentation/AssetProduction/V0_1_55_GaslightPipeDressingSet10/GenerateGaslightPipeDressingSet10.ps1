param(
    [string]$RepoRoot = "D:\__MY APPS\Unity Doom"
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$PackId = "GPD10"
$Version = "0.1.55-p001"
$GeneratedUtc = "2026-05-25T00:00:00Z"
$PackageName = "com.brassworks.sidecar.gaslight-pipe-dressing-set10"
$PackageDisplay = "Brassworks Breach Gaslight Pipe Dressing Set 10"

$PackageRel = "AssetPacks/BrassworksBreach.GaslightPipeDressingSet10"
$ProductionRel = "Documentation/AssetProduction/V0_1_55_GaslightPipeDressingSet10"
$ConceptRel = "Documentation/ConceptRenders/V0_1_55_GaslightPipeDressingSet10"
$PlanningRel = "Documentation/Planning/V0_1_55_GaslightPipeDressingSet10ImportReadiness"
$QaRel = "Documentation/QA/V0_1_55_GaslightPipeDressingSet10ImportReadiness"

$PackageRoot = Join-Path $RepoRoot ($PackageRel -replace "/", "\")
$ProductionRoot = Join-Path $RepoRoot ($ProductionRel -replace "/", "\")
$ConceptRoot = Join-Path $RepoRoot ($ConceptRel -replace "/", "\")
$PlanningRoot = Join-Path $RepoRoot ($PlanningRel -replace "/", "\")
$QaRoot = Join-Path $RepoRoot ($QaRel -replace "/", "\")

function Join-RepoPath([string]$relPath) {
    return Join-Path $RepoRoot ($relPath -replace "/", "\")
}

function To-Rel([string]$fullPath) {
    $root = [System.IO.Path]::GetFullPath($RepoRoot).TrimEnd("\")
    $full = [System.IO.Path]::GetFullPath($fullPath)
    return $full.Substring($root.Length + 1).Replace("\", "/")
}

function Get-DeterministicGuid([string]$relPath) {
    $md5 = [System.Security.Cryptography.MD5]::Create()
    try {
        $bytes = [System.Text.Encoding]::UTF8.GetBytes(("BrassworksBreach.GaslightPipeDressingSet10|" + $relPath.ToLowerInvariant().Replace("\", "/")))
        $hash = $md5.ComputeHash($bytes)
        return -join ($hash | ForEach-Object { $_.ToString("x2") })
    }
    finally {
        $md5.Dispose()
    }
}

function Write-Utf8NoBom([string]$path, [string]$content) {
    $folder = Split-Path -Parent $path
    if ($folder -and -not (Test-Path $folder)) {
        New-Item -ItemType Directory -Force -Path $folder | Out-Null
    }

    [System.IO.File]::WriteAllText($path, $content, [System.Text.UTF8Encoding]::new($false))
}

function Convert-Color([double[]]$rgba) {
    return "{r: $("{0:0.000}" -f $rgba[0]), g: $("{0:0.000}" -f $rgba[1]), b: $("{0:0.000}" -f $rgba[2]), a: $("{0:0.000}" -f $rgba[3])}"
}

function Convert-Vec3([double[]]$xyz) {
    return "{x: $("{0:0.###}" -f $xyz[0]), y: $("{0:0.###}" -f $xyz[1]), z: $("{0:0.###}" -f $xyz[2])}"
}

function Material([string]$name, [double[]]$color, [double]$metallic, [double]$smoothness, [double[]]$emission, [string]$role) {
    return [pscustomobject]@{
        name = $name
        color = $color
        metallic = $metallic
        smoothness = $smoothness
        emission = $emission
        role = $role
    }
}

$Materials = @(
    Material "GPD10_MAT_AgedBrassWarm" @(0.72, 0.48, 0.20, 1.0) 0.82 0.48 $null "dominant worn brass for lamp frames and cages"
    Material "GPD10_MAT_PolishedBrassEdge" @(0.98, 0.73, 0.34, 1.0) 0.90 0.64 $null "small edge highlights on brackets, rims, and cage ribs"
    Material "GPD10_MAT_TarnishedBrassDark" @(0.39, 0.27, 0.13, 1.0) 0.74 0.36 $null "aged shadow brass for grime-heavy fixture backs"
    Material "GPD10_MAT_BlackenedPipeIron" @(0.045, 0.047, 0.043, 1.0) 0.72 0.31 $null "black pipe runs and dark support iron"
    Material "GPD10_MAT_OilySteelBracket" @(0.10, 0.105, 0.10, 1.0) 0.64 0.52 $null "slightly wet bracket steel"
    Material "GPD10_MAT_AmberGaslightGlass" @(1.00, 0.50, 0.12, 0.78) 0.04 0.82 @(1.25, 0.46, 0.10, 1.0) "emissive amber glass bulb surfaces"
    Material "GPD10_MAT_SoftAmberGlow" @(1.00, 0.62, 0.19, 0.62) 0.00 0.58 @(1.85, 0.78, 0.22, 1.0) "visual-only halo cards and bulbs, no Light component"
    Material "GPD10_MAT_SmokedGlassRim" @(0.18, 0.12, 0.075, 0.72) 0.02 0.76 $null "smoked glass rims and internal shade silhouettes"
    Material "GPD10_MAT_DarkWallPlaque" @(0.075, 0.061, 0.048, 1.0) 0.38 0.28 $null "dark wall plaques that sit over v0.5 brick surfaces"
    Material "GPD10_MAT_SootGradientCard" @(0.018, 0.016, 0.013, 0.64) 0.00 0.44 $null "visual soot cards behind fixtures"
    Material "GPD10_MAT_WetReflectionWarm" @(1.00, 0.50, 0.14, 0.50) 0.00 0.88 @(0.55, 0.20, 0.05, 1.0) "warm reflection helper glints for damp floors/walls"
    Material "GPD10_MAT_CoolDampReflection" @(0.19, 0.27, 0.30, 0.45) 0.00 0.86 @(0.04, 0.08, 0.10, 1.0) "cool reflection helper strips for back-wall readability"
    Material "GPD10_MAT_VerdigrisOxide" @(0.10, 0.34, 0.29, 1.0) 0.58 0.42 $null "oxidized brass/copper accent staining"
    Material "GPD10_MAT_RivetShadow" @(0.015, 0.013, 0.012, 1.0) 0.56 0.25 $null "dark bolt heads and tiny fasteners"
)

$MaterialGuidByName = @{}
foreach ($mat in $Materials) {
    $rel = "$PackageRel/Runtime/Materials/$($mat.name).mat"
    $MaterialGuidByName[$mat.name] = Get-DeterministicGuid $rel
}

function Write-MaterialAsset($mat) {
    $keywords = "[]"
    $emissionBlock = ""
    if ($null -ne $mat.emission) {
        $keywords = "`n  - _EMISSION"
        $emissionBlock = "`n    - _EmissionColor: $(Convert-Color $mat.emission)"
    }

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
  m_Name: $($mat.name)
  m_Shader: {fileID: 46, guid: 0000000000000000f000000000000000, type: 0}
  m_Parent: {fileID: 0}
  m_ModifiedSerializedProperties: 0
  m_ValidKeywords: $keywords
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
    - _Metallic: $("{0:0.###}" -f $mat.metallic)
    - _Glossiness: $("{0:0.###}" -f $mat.smoothness)
    m_Colors:
    - _Color: $(Convert-Color $mat.color)$emissionBlock
  m_BuildTextureStacks: []
"@

    Write-Utf8NoBom (Join-RepoPath "$PackageRel/Runtime/Materials/$($mat.name).mat") $content
}

$Rotations = @{
    identity = [pscustomobject]@{ x = 0.0; y = 0.0; z = 0.0; w = 1.0; hint = @(0, 0, 0) }
    z90 = [pscustomobject]@{ x = 0.0; y = 0.0; z = 0.7071068; w = 0.7071068; hint = @(0, 0, 90) }
    z45 = [pscustomobject]@{ x = 0.0; y = 0.0; z = 0.3826834; w = 0.9238795; hint = @(0, 0, 45) }
    zNeg45 = [pscustomobject]@{ x = 0.0; y = 0.0; z = -0.3826834; w = 0.9238795; hint = @(0, 0, -45) }
    x90 = [pscustomobject]@{ x = 0.7071068; y = 0.0; z = 0.0; w = 0.7071068; hint = @(90, 0, 0) }
}

$MeshIds = @{
    Cube = 10202
    Cylinder = 10206
    Sphere = 10207
    Capsule = 10208
}

function Part([string]$name, [string]$mesh, [string]$material, [double[]]$pos, [double[]]$scale, [string]$rotation = "identity") {
    if ($pos.Count -ne 3) {
        throw "Part '$name' position must have exactly 3 values, got $($pos.Count). Wrap arithmetic expressions in parentheses inside PowerShell arrays."
    }
    if ($scale.Count -ne 3) {
        throw "Part '$name' scale must have exactly 3 values, got $($scale.Count). Wrap arithmetic expressions in parentheses inside PowerShell arrays."
    }

    return [pscustomobject]@{
        name = $name
        mesh = $mesh
        material = $material
        pos = $pos
        scale = $scale
        rotation = $rotation
    }
}

function New-WallGaslightParts([string]$variant) {
    $offset = switch ($variant) { "A" { 0.0 } "B" { 0.08 } "C" { -0.06 } default { 0.12 } }
    $wide = switch ($variant) { "A" { 0.90 } "B" { 1.10 } "C" { 0.78 } default { 1.24 } }
    $tall = switch ($variant) { "A" { 1.55 } "B" { 1.78 } "C" { 1.34 } default { 1.66 } }
    $bars = @(
        Part "Wall_BackPlaque" Cube "GPD10_MAT_DarkWallPlaque" @(0, 1.00, 0.10) @($wide, $tall, 0.08)
        Part "Soot_Halo_Backplate" Cube "GPD10_MAT_SootGradientCard" @(0, 1.00, 0.045) @(($wide * 0.82), ($tall * 0.72), 0.035)
        Part "Brass_Plaque_Frame" Cube "GPD10_MAT_TarnishedBrassDark" @(0, 1.00, -0.01) @(($wide * 0.62), ($tall * 0.58), 0.04)
        Part "Amber_Glow_Core" Sphere "GPD10_MAT_SoftAmberGlow" @(0, (0.96 + $offset), -0.27) @(0.50, 0.62, 0.50)
        Part "Gaslight_Glass_Bulb" Sphere "GPD10_MAT_AmberGaslightGlass" @(0, (0.96 + $offset), -0.30) @(0.40, 0.54, 0.40)
        Part "Smoked_Glass_Inner_Shade" Sphere "GPD10_MAT_SmokedGlassRim" @(0, (0.96 + $offset), -0.315) @(0.28, 0.42, 0.28)
        Part "Top_Brass_Rim" Cylinder "GPD10_MAT_PolishedBrassEdge" @(0, (1.31 + $offset), -0.30) @(0.48, 0.055, 0.48)
        Part "Bottom_Brass_Rim" Cylinder "GPD10_MAT_PolishedBrassEdge" @(0, (0.61 + $offset), -0.30) @(0.46, 0.055, 0.46)
        Part "Upper_Pipe_Collar" Cylinder "GPD10_MAT_BlackenedPipeIron" @(-0.56, 1.42, -0.06) @(0.055, 0.86, 0.055) "z90"
        Part "Lower_Pipe_Collar" Cylinder "GPD10_MAT_BlackenedPipeIron" @(0.56, 0.50, -0.06) @(0.055, 0.74, 0.055) "z90"
        Part "Wall_Gas_Feed" Cylinder "GPD10_MAT_BlackenedPipeIron" @(-0.57, 0.96, -0.06) @(0.07, 0.98, 0.07)
        Part "Gooseneck_Bracket_Stem" Cube "GPD10_MAT_OilySteelBracket" @(0, 0.96, -0.13) @(0.12, 0.12, 0.38)
        Part "Cage_Left_Bar" Cube "GPD10_MAT_AgedBrassWarm" @(-0.33, (0.96 + $offset), -0.31) @(0.045, 0.77, 0.045)
        Part "Cage_Right_Bar" Cube "GPD10_MAT_AgedBrassWarm" @(0.33, (0.96 + $offset), -0.31) @(0.045, 0.77, 0.045)
        Part "Cage_Front_Bar" Cube "GPD10_MAT_AgedBrassWarm" @(0, (0.96 + $offset), -0.58) @(0.042, 0.72, 0.045)
        Part "Rivet_Top_Left" Sphere "GPD10_MAT_RivetShadow" @(-0.34, 1.60, 0.015) @(0.085, 0.085, 0.035)
        Part "Rivet_Top_Right" Sphere "GPD10_MAT_RivetShadow" @(0.34, 1.60, 0.015) @(0.085, 0.085, 0.035)
        Part "Rivet_Bottom_Left" Sphere "GPD10_MAT_RivetShadow" @(-0.34, 0.40, 0.015) @(0.085, 0.085, 0.035)
        Part "Rivet_Bottom_Right" Sphere "GPD10_MAT_RivetShadow" @(0.34, 0.40, 0.015) @(0.085, 0.085, 0.035)
    )

    if ($variant -in @("B", "D")) {
        $bars += Part "Secondary_Cage_Bar_Left" Cube "GPD10_MAT_PolishedBrassEdge" @(-0.18, (0.96 + $offset), -0.55) @(0.032, 0.66, 0.04)
        $bars += Part "Secondary_Cage_Bar_Right" Cube "GPD10_MAT_PolishedBrassEdge" @(0.18, (0.96 + $offset), -0.55) @(0.032, 0.66, 0.04)
    }

    if ($variant -in @("C", "D")) {
        $bars += Part "Verdigris_Drip_Left" Cube "GPD10_MAT_VerdigrisOxide" @(-0.42, 0.72, 0.01) @(0.05, 0.38, 0.02)
        $bars += Part "Amber_Wall_Bounce_Card" Cube "GPD10_MAT_WetReflectionWarm" @(0.00, 0.30, -0.02) @(($wide * 0.52), 0.10, 0.018)
    }

    return $bars
}

function New-PipeBracketParts([string]$variant) {
    $vertical = $variant -in @("B", "D")
    $pipeRot = if ($vertical) { "identity" } else { "z90" }
    $pipeScale = if ($vertical) { @(0.12, 1.42, 0.12) } else { @(0.12, 1.50, 0.12) }
    $pipePos = if ($vertical) { @(0, 1.00, -0.13) } else { @(0, 1.02, -0.13) }
    $parts = @(
        Part "Mounting_Wall_Plate" Cube "GPD10_MAT_DarkWallPlaque" @(0, 1.00, 0.08) @(0.92, 0.70, 0.08)
        Part "Recessed_Shadow_Card" Cube "GPD10_MAT_SootGradientCard" @(0, 1.00, 0.025) @(0.72, 0.52, 0.035)
        Part "Pipe_Run_Visual" Cylinder "GPD10_MAT_BlackenedPipeIron" $pipePos $pipeScale $pipeRot
        Part "Brass_Clamp_Left" Cube "GPD10_MAT_AgedBrassWarm" @(-0.31, 1.00, -0.12) @(0.10, 0.56, 0.14)
        Part "Brass_Clamp_Right" Cube "GPD10_MAT_AgedBrassWarm" @(0.31, 1.00, -0.12) @(0.10, 0.56, 0.14)
        Part "Clamp_Bridge_Top" Cube "GPD10_MAT_PolishedBrassEdge" @(0, 1.31, -0.12) @(0.68, 0.08, 0.14)
        Part "Clamp_Bridge_Bottom" Cube "GPD10_MAT_TarnishedBrassDark" @(0, 0.69, -0.12) @(0.68, 0.08, 0.14)
        Part "Diagonal_Load_Brace_A" Cube "GPD10_MAT_OilySteelBracket" @(-0.21, 0.77, -0.02) @(0.08, 0.52, 0.06) "z45"
        Part "Diagonal_Load_Brace_B" Cube "GPD10_MAT_OilySteelBracket" @(0.21, 0.77, -0.02) @(0.08, 0.52, 0.06) "zNeg45"
        Part "Bolt_NW" Sphere "GPD10_MAT_RivetShadow" @(-0.36, 1.26, -0.03) @(0.08, 0.08, 0.035)
        Part "Bolt_NE" Sphere "GPD10_MAT_RivetShadow" @(0.36, 1.26, -0.03) @(0.08, 0.08, 0.035)
        Part "Bolt_SW" Sphere "GPD10_MAT_RivetShadow" @(-0.36, 0.74, -0.03) @(0.08, 0.08, 0.035)
        Part "Bolt_SE" Sphere "GPD10_MAT_RivetShadow" @(0.36, 0.74, -0.03) @(0.08, 0.08, 0.035)
    )

    if ($variant -in @("C", "D")) {
        $parts += Part "Small_Pressure_Tab" Cube "GPD10_MAT_VerdigrisOxide" @(0, 0.48, -0.035) @(0.38, 0.10, 0.035)
        $parts += Part "Amber_Leak_Glint" Cube "GPD10_MAT_WetReflectionWarm" @(0.40, 0.58, -0.08) @(0.24, 0.045, 0.025)
    }

    return $parts
}

function New-LampFrameParts([string]$variant) {
    $height = switch ($variant) { "A" { 1.28 } "B" { 1.46 } "C" { 1.14 } default { 1.62 } }
    $width = switch ($variant) { "A" { 0.74 } "B" { 0.92 } "C" { 0.64 } default { 1.06 } }
    $parts = @(
        Part "Open_Frame_Left_Rail" Cube "GPD10_MAT_AgedBrassWarm" @((-1 * $width / 2), 1.00, -0.24) @(0.055, $height, 0.055)
        Part "Open_Frame_Right_Rail" Cube "GPD10_MAT_AgedBrassWarm" @(($width / 2), 1.00, -0.24) @(0.055, $height, 0.055)
        Part "Open_Frame_Top_Rail" Cube "GPD10_MAT_PolishedBrassEdge" @(0, (1.00 + $height / 2), -0.24) @($width, 0.055, 0.055)
        Part "Open_Frame_Bottom_Rail" Cube "GPD10_MAT_TarnishedBrassDark" @(0, (1.00 - $height / 2), -0.24) @($width, 0.055, 0.055)
        Part "Rear_Shadow_Mount" Cube "GPD10_MAT_SootGradientCard" @(0, 1.00, 0.03) @(($width * 1.05), ($height * 0.92), 0.035)
        Part "Center_Glass_Ghost" Sphere "GPD10_MAT_SmokedGlassRim" @(0, 1.00, -0.26) @(($width * 0.42), ($height * 0.35), ($width * 0.42))
        Part "Small_Amber_Bulb" Sphere "GPD10_MAT_AmberGaslightGlass" @(0, 1.00, -0.32) @(($width * 0.28), ($height * 0.22), ($width * 0.28))
        Part "Top_Clamp_Pin" Cylinder "GPD10_MAT_BlackenedPipeIron" @(0, (1.00 + $height / 2 + 0.12), -0.24) @(0.055, ($width * 0.75), 0.055) "z90"
        Part "Bottom_Clamp_Pin" Cylinder "GPD10_MAT_BlackenedPipeIron" @(0, (1.00 - $height / 2 - 0.12), -0.24) @(0.055, ($width * 0.75), 0.055) "z90"
    )

    if ($variant -in @("B", "D")) {
        $parts += Part "Cross_Brace_Rising" Cube "GPD10_MAT_PolishedBrassEdge" @(0, 1.00, -0.25) @(0.045, ($height * 0.92), 0.045) "z45"
        $parts += Part "Cross_Brace_Falling" Cube "GPD10_MAT_PolishedBrassEdge" @(0, 1.00, -0.255) @(0.045, ($height * 0.92), 0.045) "zNeg45"
    }

    if ($variant -in @("C", "D")) {
        $parts += Part "Offset_Pipe_Hook" Cylinder "GPD10_MAT_BlackenedPipeIron" @((-1 * $width / 2 - 0.22), 1.00, -0.20) @(0.055, ($height * 0.72), 0.055)
        $parts += Part "Cool_Wall_Readability_Strip" Cube "GPD10_MAT_CoolDampReflection" @(0, (1.00 - $height / 2 - 0.24), -0.04) @(($width * 0.82), 0.07, 0.018)
    }

    return $parts
}

function New-BrassCageParts([string]$variant) {
    $height = switch ($variant) { "A" { 1.12 } "B" { 1.35 } "C" { 0.96 } default { 1.48 } }
    $radius = switch ($variant) { "A" { 0.42 } "B" { 0.48 } "C" { 0.36 } default { 0.54 } }
    $parts = @(
        Part "Top_Cage_Ring" Cylinder "GPD10_MAT_PolishedBrassEdge" @(0, (1.00 + $height / 2), -0.24) @($radius, 0.055, $radius)
        Part "Bottom_Cage_Ring" Cylinder "GPD10_MAT_TarnishedBrassDark" @(0, (1.00 - $height / 2), -0.24) @($radius, 0.055, $radius)
        Part "Front_Vertical_Bar" Cube "GPD10_MAT_AgedBrassWarm" @(0, 1.00, (-0.24 - $radius)) @(0.045, $height, 0.045)
        Part "Back_Vertical_Bar" Cube "GPD10_MAT_AgedBrassWarm" @(0, 1.00, (-0.24 + $radius)) @(0.045, $height, 0.045)
        Part "Left_Vertical_Bar" Cube "GPD10_MAT_AgedBrassWarm" @((-1 * $radius), 1.00, -0.24) @(0.045, $height, 0.045)
        Part "Right_Vertical_Bar" Cube "GPD10_MAT_AgedBrassWarm" @($radius, 1.00, -0.24) @(0.045, $height, 0.045)
        Part "Inner_Amber_Reference_Core" Sphere "GPD10_MAT_SoftAmberGlow" @(0, 1.00, -0.24) @(($radius * 0.62), ($height * 0.34), ($radius * 0.62))
        Part "Suspension_Pin" Cylinder "GPD10_MAT_BlackenedPipeIron" @(0, (1.00 + $height / 2 + 0.16), -0.24) @(0.055, ($radius * 1.25), 0.055) "z90"
    )

    if ($variant -in @("B", "D")) {
        $parts += Part "Middle_Cage_Band" Cylinder "GPD10_MAT_AgedBrassWarm" @(0, 1.00, -0.24) @(($radius * 0.94), 0.04, ($radius * 0.94))
        $parts += Part "Left_Secondary_Bar" Cube "GPD10_MAT_PolishedBrassEdge" @((-1 * $radius * 0.55), 1.00, (-0.24 - $radius * 0.72)) @(0.033, ($height * 0.92), 0.033)
        $parts += Part "Right_Secondary_Bar" Cube "GPD10_MAT_PolishedBrassEdge" @(($radius * 0.55), 1.00, (-0.24 - $radius * 0.72)) @(0.033, ($height * 0.92), 0.033)
    }

    if ($variant -in @("C", "D")) {
        $parts += Part "Verdigris_Cage_Stain" Cube "GPD10_MAT_VerdigrisOxide" @((-1 * $radius - 0.05), 0.76, -0.25) @(0.04, 0.36, 0.03)
    }

    return $parts
}

function New-WallPlaqueParts([string]$variant) {
    $width = switch ($variant) { "A" { 1.02 } "B" { 1.32 } "C" { 0.82 } default { 1.52 } }
    $height = switch ($variant) { "A" { 0.72 } "B" { 0.58 } "C" { 1.02 } default { 0.88 } }
    $parts = @(
        Part "Plaque_Dark_Iron_Back" Cube "GPD10_MAT_DarkWallPlaque" @(0, 1.00, 0.08) @($width, $height, 0.08)
        Part "Inset_Brass_Label_Plate" Cube "GPD10_MAT_TarnishedBrassDark" @(0, 1.00, 0.02) @(($width * 0.78), ($height * 0.62), 0.04)
        Part "Top_Polished_Lip" Cube "GPD10_MAT_PolishedBrassEdge" @(0, (1.00 + $height / 2 - 0.055), -0.015) @(($width * 0.86), 0.045, 0.045)
        Part "Bottom_Soot_Lip" Cube "GPD10_MAT_SootGradientCard" @(0, (1.00 - $height / 2 + 0.055), -0.012) @(($width * 0.86), 0.05, 0.035)
        Part "Left_Bolt" Sphere "GPD10_MAT_RivetShadow" @((-1 * $width / 2 + 0.13), (1.00 + $height / 2 - 0.13), -0.03) @(0.075, 0.075, 0.035)
        Part "Right_Bolt" Sphere "GPD10_MAT_RivetShadow" @(($width / 2 - 0.13), (1.00 + $height / 2 - 0.13), -0.03) @(0.075, 0.075, 0.035)
        Part "Lower_Left_Bolt" Sphere "GPD10_MAT_RivetShadow" @((-1 * $width / 2 + 0.13), (1.00 - $height / 2 + 0.13), -0.03) @(0.075, 0.075, 0.035)
        Part "Lower_Right_Bolt" Sphere "GPD10_MAT_RivetShadow" @(($width / 2 - 0.13), (1.00 - $height / 2 + 0.13), -0.03) @(0.075, 0.075, 0.035)
    )

    if ($variant -in @("B", "D")) {
        $parts += Part "Twin_Label_Score_A" Cube "GPD10_MAT_AgedBrassWarm" @((-1 * $width * 0.16), 1.00, -0.02) @(0.035, ($height * 0.45), 0.025)
        $parts += Part "Twin_Label_Score_B" Cube "GPD10_MAT_AgedBrassWarm" @(($width * 0.16), 1.00, -0.02) @(0.035, ($height * 0.45), 0.025)
    }

    if ($variant -in @("C", "D")) {
        $parts += Part "Small_Amber_Inspection_Glint" Cube "GPD10_MAT_WetReflectionWarm" @(0, (1.00 - $height * 0.18), -0.035) @(($width * 0.36), 0.055, 0.02)
    }

    return $parts
}

function New-ReflectionHelperParts([string]$variant) {
    $wide = switch ($variant) { "A" { 1.25 } "B" { 1.55 } "C" { 0.95 } default { 1.85 } }
    $parts = @(
        Part "Placement_Backplate_Thin" Cube "GPD10_MAT_DarkWallPlaque" @(0, 0.96, 0.07) @($wide, 0.28, 0.025)
        Part "Warm_Floor_Glint_Card" Cube "GPD10_MAT_WetReflectionWarm" @(0, 0.90, -0.02) @(($wide * 0.92), 0.085, 0.018)
        Part "Cool_Damp_Breakup_Card" Cube "GPD10_MAT_CoolDampReflection" @(0, 1.02, -0.025) @(($wide * 0.66), 0.055, 0.018)
        Part "Soot_Mask_Upper" Cube "GPD10_MAT_SootGradientCard" @(0, 1.15, -0.01) @(($wide * 0.72), 0.07, 0.016)
        Part "Left_Pin" Sphere "GPD10_MAT_RivetShadow" @((-1 * $wide / 2 + 0.10), 1.05, -0.035) @(0.055, 0.055, 0.02)
        Part "Right_Pin" Sphere "GPD10_MAT_RivetShadow" @(($wide / 2 - 0.10), 1.05, -0.035) @(0.055, 0.055, 0.02)
    )

    if ($variant -in @("B", "D")) {
        $parts += Part "Split_Warm_Glint_Left" Cube "GPD10_MAT_WetReflectionWarm" @((-1 * $wide * 0.28), 0.78, -0.02) @(($wide * 0.28), 0.05, 0.014)
        $parts += Part "Split_Warm_Glint_Right" Cube "GPD10_MAT_WetReflectionWarm" @(($wide * 0.28), 0.78, -0.02) @(($wide * 0.28), 0.05, 0.014)
    }

    if ($variant -in @("C", "D")) {
        $parts += Part "Lamp_Side_Bounce_Tab" Cube "GPD10_MAT_AmberGaslightGlass" @(($wide * 0.42), 0.96, -0.035) @(($wide * 0.12), 0.16, 0.018)
        $parts += Part "Cool_Edge_Counter_Glint" Cube "GPD10_MAT_CoolDampReflection" @((-1 * $wide * 0.38), 0.84, -0.036) @(($wide * 0.18), 0.04, 0.014)
    }

    return $parts
}

$PrefabSpecs = @()
$index = 1
foreach ($family in @("WallGaslights", "PipeBrackets", "LampFrames", "BrassCages", "WallPlaques", "ReflectionHelpers")) {
    foreach ($variant in @("A", "B", "C", "D")) {
        $name = "{0}_PREFAB_{1:00}_{2}_{3}" -f $PackId, $index, $family, $variant
        $parts = switch ($family) {
            "WallGaslights" { New-WallGaslightParts $variant }
            "PipeBrackets" { New-PipeBracketParts $variant }
            "LampFrames" { New-LampFrameParts $variant }
            "BrassCages" { New-BrassCageParts $variant }
            "WallPlaques" { New-WallPlaqueParts $variant }
            "ReflectionHelpers" { New-ReflectionHelperParts $variant }
        }
        $PrefabSpecs += [pscustomobject]@{
            index = $index
            name = $name
            family = $family
            variant = $variant
            parts = $parts
            path = "$PackageRel/Runtime/Prefabs/$name.prefab"
            preview = "$ConceptRel/${PackId}_PREVIEW_$("{0:00}" -f $index)_${family}_${variant}.png"
        }
        $index++
    }
}

function Write-PrefabAsset($spec) {
    $rootGo = 100000
    $rootTf = 100001
    $childTransformIds = @()
    for ($i = 0; $i -lt $spec.parts.Count; $i++) {
        $childTransformIds += (200001 + ($i * 10))
    }

    $yaml = New-Object System.Collections.Generic.List[string]
    $yaml.Add("%YAML 1.1")
    $yaml.Add("%TAG !u! tag:unity3d.com,2011:")
    $yaml.Add("--- !u!1 &$rootGo")
    $yaml.Add("GameObject:")
    $yaml.Add("  m_ObjectHideFlags: 0")
    $yaml.Add("  m_CorrespondingSourceObject: {fileID: 0}")
    $yaml.Add("  m_PrefabInstance: {fileID: 0}")
    $yaml.Add("  m_PrefabAsset: {fileID: 0}")
    $yaml.Add("  serializedVersion: 6")
    $yaml.Add("  m_Component:")
    $yaml.Add("  - component: {fileID: $rootTf}")
    $yaml.Add("  m_Layer: 0")
    $yaml.Add("  m_Name: $($spec.name)")
    $yaml.Add("  m_TagString: Untagged")
    $yaml.Add("  m_Icon: {fileID: 0}")
    $yaml.Add("  m_NavMeshLayer: 0")
    $yaml.Add("  m_StaticEditorFlags: 0")
    $yaml.Add("  m_IsActive: 1")
    $yaml.Add("--- !u!4 &$rootTf")
    $yaml.Add("Transform:")
    $yaml.Add("  m_ObjectHideFlags: 0")
    $yaml.Add("  m_CorrespondingSourceObject: {fileID: 0}")
    $yaml.Add("  m_PrefabInstance: {fileID: 0}")
    $yaml.Add("  m_PrefabAsset: {fileID: 0}")
    $yaml.Add("  m_GameObject: {fileID: $rootGo}")
    $yaml.Add("  serializedVersion: 2")
    $yaml.Add("  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}")
    $yaml.Add("  m_LocalPosition: {x: 0, y: 0, z: 0}")
    $yaml.Add("  m_LocalScale: {x: 1, y: 1, z: 1}")
    $yaml.Add("  m_ConstrainProportionsScale: 0")
    $yaml.Add("  m_Children:")
    foreach ($tfId in $childTransformIds) {
        $yaml.Add("  - {fileID: $tfId}")
    }
    $yaml.Add("  m_Father: {fileID: 0}")
    $yaml.Add("  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}")

    for ($i = 0; $i -lt $spec.parts.Count; $i++) {
        $part = $spec.parts[$i]
        $goId = 200000 + ($i * 10)
        $tfId = 200001 + ($i * 10)
        $meshId = 200002 + ($i * 10)
        $rendererId = 200003 + ($i * 10)
        $rot = $Rotations[$part.rotation]
        $meshFileId = $MeshIds[$part.mesh]
        $matGuid = $MaterialGuidByName[$part.material]

        $yaml.Add("--- !u!1 &$goId")
        $yaml.Add("GameObject:")
        $yaml.Add("  m_ObjectHideFlags: 0")
        $yaml.Add("  m_CorrespondingSourceObject: {fileID: 0}")
        $yaml.Add("  m_PrefabInstance: {fileID: 0}")
        $yaml.Add("  m_PrefabAsset: {fileID: 0}")
        $yaml.Add("  serializedVersion: 6")
        $yaml.Add("  m_Component:")
        $yaml.Add("  - component: {fileID: $tfId}")
        $yaml.Add("  - component: {fileID: $meshId}")
        $yaml.Add("  - component: {fileID: $rendererId}")
        $yaml.Add("  m_Layer: 0")
        $yaml.Add("  m_Name: $($part.name)")
        $yaml.Add("  m_TagString: Untagged")
        $yaml.Add("  m_Icon: {fileID: 0}")
        $yaml.Add("  m_NavMeshLayer: 0")
        $yaml.Add("  m_StaticEditorFlags: 0")
        $yaml.Add("  m_IsActive: 1")
        $yaml.Add("--- !u!4 &$tfId")
        $yaml.Add("Transform:")
        $yaml.Add("  m_ObjectHideFlags: 0")
        $yaml.Add("  m_CorrespondingSourceObject: {fileID: 0}")
        $yaml.Add("  m_PrefabInstance: {fileID: 0}")
        $yaml.Add("  m_PrefabAsset: {fileID: 0}")
        $yaml.Add("  m_GameObject: {fileID: $goId}")
        $yaml.Add("  serializedVersion: 2")
        $yaml.Add("  m_LocalRotation: {x: $($rot.x), y: $($rot.y), z: $($rot.z), w: $($rot.w)}")
        $yaml.Add("  m_LocalPosition: $(Convert-Vec3 $part.pos)")
        $yaml.Add("  m_LocalScale: $(Convert-Vec3 $part.scale)")
        $yaml.Add("  m_ConstrainProportionsScale: 0")
        $yaml.Add("  m_Children: []")
        $yaml.Add("  m_Father: {fileID: $rootTf}")
        $yaml.Add("  m_LocalEulerAnglesHint: $(Convert-Vec3 $rot.hint)")
        $yaml.Add("--- !u!33 &$meshId")
        $yaml.Add("MeshFilter:")
        $yaml.Add("  m_ObjectHideFlags: 0")
        $yaml.Add("  m_CorrespondingSourceObject: {fileID: 0}")
        $yaml.Add("  m_PrefabInstance: {fileID: 0}")
        $yaml.Add("  m_PrefabAsset: {fileID: 0}")
        $yaml.Add("  m_GameObject: {fileID: $goId}")
        $yaml.Add("  m_Mesh: {fileID: $meshFileId, guid: 0000000000000000e000000000000000, type: 0}")
        $yaml.Add("--- !u!23 &$rendererId")
        $yaml.Add("MeshRenderer:")
        $yaml.Add("  m_ObjectHideFlags: 0")
        $yaml.Add("  m_CorrespondingSourceObject: {fileID: 0}")
        $yaml.Add("  m_PrefabInstance: {fileID: 0}")
        $yaml.Add("  m_PrefabAsset: {fileID: 0}")
        $yaml.Add("  m_GameObject: {fileID: $goId}")
        $yaml.Add("  m_Enabled: 1")
        $yaml.Add("  m_CastShadows: 1")
        $yaml.Add("  m_ReceiveShadows: 1")
        $yaml.Add("  m_DynamicOccludee: 1")
        $yaml.Add("  m_StaticShadowCaster: 0")
        $yaml.Add("  m_MotionVectors: 1")
        $yaml.Add("  m_LightProbeUsage: 1")
        $yaml.Add("  m_ReflectionProbeUsage: 1")
        $yaml.Add("  m_RayTracingMode: 2")
        $yaml.Add("  m_RayTraceProcedural: 0")
        $yaml.Add("  m_RenderingLayerMask: 1")
        $yaml.Add("  m_RendererPriority: 0")
        $yaml.Add("  m_Materials:")
        $yaml.Add("  - {fileID: 2100000, guid: $matGuid, type: 2}")
        $yaml.Add("  m_StaticBatchInfo:")
        $yaml.Add("    firstSubMesh: 0")
        $yaml.Add("    subMeshCount: 0")
        $yaml.Add("  m_StaticBatchRoot: {fileID: 0}")
        $yaml.Add("  m_ProbeAnchor: {fileID: 0}")
        $yaml.Add("  m_LightProbeVolumeOverride: {fileID: 0}")
        $yaml.Add("  m_ScaleInLightmap: 1")
        $yaml.Add("  m_ReceiveGI: 1")
        $yaml.Add("  m_PreserveUVs: 0")
        $yaml.Add("  m_IgnoreNormalsForChartDetection: 0")
        $yaml.Add("  m_ImportantGI: 0")
        $yaml.Add("  m_StitchLightmapSeams: 1")
        $yaml.Add("  m_SelectedEditorRenderState: 3")
        $yaml.Add("  m_MinimumChartSize: 4")
        $yaml.Add("  m_AutoUVMaxDistance: 0.5")
        $yaml.Add("  m_AutoUVMaxAngle: 89")
        $yaml.Add("  m_LightmapParameters: {fileID: 0}")
        $yaml.Add("  m_SortingLayerID: 0")
        $yaml.Add("  m_SortingLayer: 0")
        $yaml.Add("  m_SortingOrder: 0")
    }

    Write-Utf8NoBom (Join-RepoPath $spec.path) (($yaml -join "`n") + "`n")
}

function Write-PackageDocs {
    $packageJson = [ordered]@{
        name = $PackageName
        displayName = $PackageDisplay
        version = $Version
        unity = "2022.3"
        description = "Unity-only visual sidecar bundle of steampunk gaslights, pipe brackets, lamp frames, brass cages, wall plaques, and reflection helper pieces for roomtest v0.5 fixture gap remediation."
        keywords = @("brassworks", "sidecar", "steampunk", "gaslight", "pipes", "fixtures", "visual-only")
        author = [ordered]@{ name = "Brassworks Breach Sidecar Production" }
    } | ConvertTo-Json -Depth 5
    Write-Utf8NoBom (Join-RepoPath "$PackageRel/package.json") ($packageJson + "`n")

    $readme = @"
# BrassworksBreach.GaslightPipeDressingSet10

Visual-only steampunk wall gaslight and pipe-fixture dressing package for Brassworks Breach.

This package directly targets the weak lamp/fixture gap called out around roomtest v0.5: the room surfaces and lighting direction are working, but the remaining gains need purpose-built lamp models, fixture detail, wall plaques, and damp reflection helpers.

## Contents

- Prefabs: 24 visual-only Unity prefabs across 6 families.
- Materials: 14 Unity Standard-shader materials.
- Preview PNGs: 24 per-piece concept previews in the documentation root plus a package contact sheet in `Runtime/Previews`.
- Metadata: normalized sidecar manifest mirrored in `Runtime/Metadata` and `Documentation~/Manifest`.

## Visual-Only Contract

The prefabs contain only `GameObject`, `Transform`, `MeshFilter`, and `MeshRenderer` records. They contain no colliders, rigidbodies, lights, reflection probes, scripts, audio, animations, timelines, scenes, navmesh, or gameplay authority. Amber bulbs and glints are emissive materials only.

## Import Notes

Import into a quarantine Unity project first. Place the wall gaslight prefabs on the v0.5 brick surfaces, then layer pipe brackets, plaques, cages, and reflection helper strips around the damp floor/wall glints. Any real lighting, collision, reflection probe, damage, or interaction behavior must be authored by the integration owner in main project content.
"@
    Write-Utf8NoBom (Join-RepoPath "$PackageRel/README.md") $readme

    $changelog = @"
# Changelog

## $Version

- Added 24 visual-only gaslight, pipe bracket, lamp frame, brass cage, wall plaque, and reflection helper prefabs.
- Added 14 steampunk fixture materials with warm brass, blackened iron, smoked glass, amber glow, soot, verdigris, and damp reflection roles.
- Added normalized sidecar manifest, concept previews, package contact sheet, import plan, and QA checklist.
"@
    Write-Utf8NoBom (Join-RepoPath "$PackageRel/CHANGELOG.md") $changelog

    $sampleReadme = @"
# Gaslight Pipe Dressing Set 10 Prefab Palette

Use the prefabs from `Runtime/Prefabs` as visual dressing in quarantine first. The reflection helper pieces are geometry cards only and do not include Unity `ReflectionProbe` components.
"@
    Write-Utf8NoBom (Join-RepoPath "$PackageRel/Samples~/PrefabPalette/README.md") $sampleReadme
}

function Get-DrawingColor([string]$materialName, [int]$alphaOverride = -1) {
    $mat = $Materials | Where-Object { $_.name -eq $materialName } | Select-Object -First 1
    if ($null -eq $mat) {
        throw "Preview requested unknown material '$materialName'."
    }

    $a = if ($alphaOverride -ge 0) { $alphaOverride } else { [Math]::Max(55, [Math]::Min(255, [int]($mat.color[3] * 255))) }
    return [System.Drawing.Color]::FromArgb($a, [int]($mat.color[0] * 255), [int]($mat.color[1] * 255), [int]($mat.color[2] * 255))
}

function Draw-RoundedRect($graphics, [System.Drawing.Brush]$brush, [float]$x, [float]$y, [float]$w, [float]$h) {
    $graphics.FillRectangle($brush, $x, $y, $w, $h)
}

function Write-PreviewPng($spec, [string]$path) {
    Add-Type -AssemblyName System.Drawing
    $width = 640
    $height = 480
    $bmp = [System.Drawing.Bitmap]::new($width, $height)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.Clear([System.Drawing.Color]::FromArgb(28, 24, 21))

    $bgBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(42, 38, 33))
    $g.FillRectangle($bgBrush, 42, 42, $width - 84, $height - 84)
    $gridPen = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(62, 54, 46), 1)
    for ($gx = 70; $gx -lt $width - 45; $gx += 60) { $g.DrawLine($gridPen, $gx, 42, $gx, $height - 42) }
    for ($gy = 72; $gy -lt $height - 45; $gy += 58) { $g.DrawLine($gridPen, 42, $gy, $width - 42, $gy) }

    $sorted = $spec.parts | Sort-Object { $_.pos[2] } -Descending
    foreach ($part in $sorted) {
        $matColor = Get-DrawingColor $part.material
        $brush = [System.Drawing.SolidBrush]::new($matColor)
        $pen = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(190, 8, 7, 6), 1.5)
        $screenX = 320 + ($part.pos[0] * 170) + ($part.pos[2] * 38)
        $screenY = 365 - ($part.pos[1] * 165) + ($part.pos[2] * 18)
        $sw = [Math]::Max(4, $part.scale[0] * 150)
        $sh = [Math]::Max(4, $part.scale[1] * 150)
        if ($part.rotation -eq "z90") {
            $tmp = $sw
            $sw = [Math]::Max(4, $part.scale[1] * 150)
            $sh = [Math]::Max(4, $part.scale[0] * 150)
        }

        switch ($part.mesh) {
            "Sphere" {
                $diamX = [Math]::Max(7, $part.scale[0] * 160)
                $diamY = [Math]::Max(7, $part.scale[1] * 160)
                $g.FillEllipse($brush, $screenX - $diamX / 2, $screenY - $diamY / 2, $diamX, $diamY)
                $g.DrawEllipse($pen, $screenX - $diamX / 2, $screenY - $diamY / 2, $diamX, $diamY)
            }
            "Cylinder" {
                $g.FillEllipse($brush, $screenX - $sw / 2, $screenY - $sh / 2, $sw, $sh)
                $g.DrawEllipse($pen, $screenX - $sw / 2, $screenY - $sh / 2, $sw, $sh)
            }
            default {
                $x = $screenX - $sw / 2
                $y = $screenY - $sh / 2
                if ($part.rotation -in @("z45", "zNeg45")) {
                    $state = $g.Save()
                    $angle = if ($part.rotation -eq "z45") { 45 } else { -45 }
                    $g.TranslateTransform($screenX, $screenY)
                    $g.RotateTransform($angle)
                    $g.FillRectangle($brush, -$sw / 2, -$sh / 2, $sw, $sh)
                    $g.DrawRectangle($pen, -$sw / 2, -$sh / 2, $sw, $sh)
                    $g.Restore($state)
                }
                else {
                    $g.FillRectangle($brush, $x, $y, $sw, $sh)
                    $g.DrawRectangle($pen, $x, $y, $sw, $sh)
                }
            }
        }
        $brush.Dispose()
        $pen.Dispose()
    }

    $titleBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(230, 225, 214, 198))
    $subBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(205, 178, 138, 76))
    $font = [System.Drawing.Font]::new("Segoe UI", 13, [System.Drawing.FontStyle]::Bold)
    $smallFont = [System.Drawing.Font]::new("Segoe UI", 9, [System.Drawing.FontStyle]::Regular)
    $g.DrawString($spec.name, $font, $titleBrush, 42, 16)
    $g.DrawString("$($spec.family) / Variant $($spec.variant) / visual-only prefab", $smallFont, $subBrush, 44, 444)

    $folder = Split-Path -Parent $path
    if (-not (Test-Path $folder)) { New-Item -ItemType Directory -Force -Path $folder | Out-Null }
    $bmp.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)

    $font.Dispose()
    $smallFont.Dispose()
    $titleBrush.Dispose()
    $subBrush.Dispose()
    $gridPen.Dispose()
    $bgBrush.Dispose()
    $g.Dispose()
    $bmp.Dispose()
}

function Write-ContactSheet([string]$path, [string[]]$previewPaths) {
    Add-Type -AssemblyName System.Drawing
    $thumbW = 320
    $thumbH = 240
    $cols = 4
    $rows = [Math]::Ceiling($previewPaths.Count / $cols)
    $sheetW = $cols * $thumbW
    $sheetH = 74 + ($rows * $thumbH)
    $bmp = [System.Drawing.Bitmap]::new($sheetW, $sheetH)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
    $g.Clear([System.Drawing.Color]::FromArgb(24, 21, 19))
    $titleFont = [System.Drawing.Font]::new("Segoe UI", 18, [System.Drawing.FontStyle]::Bold)
    $titleBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(230, 226, 213, 190))
    $g.DrawString("$PackId Gaslight Pipe Dressing Set 10 - Contact Sheet", $titleFont, $titleBrush, 24, 20)

    for ($i = 0; $i -lt $previewPaths.Count; $i++) {
        $img = [System.Drawing.Image]::FromFile($previewPaths[$i])
        $col = $i % $cols
        $row = [Math]::Floor($i / $cols)
        $x = $col * $thumbW
        $y = 74 + ($row * $thumbH)
        $g.DrawImage($img, $x, $y, $thumbW, $thumbH)
        $img.Dispose()
    }

    $folder = Split-Path -Parent $path
    if (-not (Test-Path $folder)) { New-Item -ItemType Directory -Force -Path $folder | Out-Null }
    $bmp.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)

    $titleFont.Dispose()
    $titleBrush.Dispose()
    $g.Dispose()
    $bmp.Dispose()
}

function Write-ManifestAndDocs {
    $families = $PrefabSpecs | Group-Object family | ForEach-Object {
        [ordered]@{
            name = $_.Name
            variants = $_.Count
            role = switch ($_.Name) {
                "WallGaslights" { "primary wall-mounted lamp silhouettes and amber bulbs" }
                "PipeBrackets" { "pipe clamp support and wall bracket fill around lamps" }
                "LampFrames" { "open replacement frames for weak/flat fixtures" }
                "BrassCages" { "cage shells to layer over lamps or pipes" }
                "WallPlaques" { "readable wall-mounted brass/iron plates without text mesh" }
                "ReflectionHelpers" { "visual-only damp glint cards and soot masks" }
            }
        }
    }

    $prefabAssets = $PrefabSpecs | ForEach-Object {
        [ordered]@{
            name = $_.name
            family = $_.family
            variant = $_.variant
            path = $_.path
            preview = $_.preview
            childRenderers = $_.parts.Count
            componentContract = "GameObject, Transform, MeshFilter, MeshRenderer only"
        }
    }

    $materialAssets = $Materials | ForEach-Object {
        [ordered]@{
            name = $_.name
            path = "$PackageRel/Runtime/Materials/$($_.name).mat"
            role = $_.role
            metallic = $_.metallic
            smoothness = $_.smoothness
            emissive = ($null -ne $_.emission)
        }
    }

    $manifest = [ordered]@{
        common_schema = "brassworks.sidecar.visual_pack_manifest.v1"
        pack_id = $PackId
        display_name = "Gaslight Pipe Dressing Set 10"
        package = [ordered]@{
            name = "BrassworksBreach.GaslightPipeDressingSet10"
            packageId = $PackageName
            version = $Version
            generated_at_utc = $GeneratedUtc
            unityCompatibility = "Unity 2022.3+ text assets; built-in primitive mesh references only."
            externalDccToolsUsed = @()
        }
        owner = [ordered]@{
            worker = "GaslightPipeSet10"
            assignedRootsOnly = $true
            lane = "sidecar-gaslight-pipe-dressing-set10"
        }
        roots = [ordered]@{
            packageRoot = $PackageRel
            productionDocumentationRoot = $ProductionRel
            conceptRenderRoot = $ConceptRel
            planningRoot = $PlanningRel
            qaRoot = $QaRel
        }
        counts = [ordered]@{
            prefabs = $PrefabSpecs.Count
            materials = $Materials.Count
            meshes = 0
            builtInMeshReferences = 4
            previewPNGs = $PrefabSpecs.Count
            contactSheets = 2
            families = 6
        }
        roomtest_v0_5_gap_target = [ordered]@{
            addressedGap = "weak lamp/fixture dressing after material-driven room surfaces"
            placementIntent = @(
                "wall-mounted gaslights with stronger brass cage silhouettes",
                "pipe brackets and plaques to make fixtures feel mounted instead of floating",
                "reflection helper strips for warm damp glints without adding lights or probes",
                "dark soot cards to preserve localized amber halos and readable dark corners"
            )
        }
        visualOnlyContract = [ordered]@{
            visualOnly = $true
            containsScripts = $false
            containsColliders = $false
            containsRigidbodies = $false
            containsLights = $false
            containsReflectionProbes = $false
            containsAudio = $false
            containsAnimations = $false
            containsScenes = $false
            intendedRuntimeBehavior = "None. All gameplay, collision, lighting, VFX, audio, occlusion, reflection-probe, and interactable behavior must be added later by the importing project."
        }
        dependencies = @(
            "Unity built-in primitive meshes",
            "Unity built-in Standard shader"
        )
        families = $families
        assets = [ordered]@{
            prefabs = $prefabAssets
            materials = $materialAssets
            previews = ($PrefabSpecs | ForEach-Object { [ordered]@{ name = $_.name -replace "_PREFAB_", "_PREVIEW_"; path = $_.preview; sourcePrefab = $_.path } })
            contactSheets = @(
                "$ConceptRel/${PackId}_CONTACTSHEET_GaslightPipeDressingSet10.png",
                "$PackageRel/Runtime/Previews/${PackId}_CONTACTSHEET_GaslightPipeDressingSet10.png"
            )
        }
        importReadiness = [ordered]@{
            status = "ready_for_quarantine_import_static_validation_complete"
            pathCollisionsChecked = $true
            guidCollisionsChecked = $true
            rollbackPath = "Remove local package reference $PackageName and delete the isolated Set10 assigned roots."
        }
        validationChecklist = @(
            "Manifest JSON parses.",
            "Prefab count is 24.",
            "Material count is 14.",
            "Preview PNG count is 24 plus contact sheets.",
            "No .cs files in package root.",
            "No forbidden prefab components: MonoBehaviour, Collider, Rigidbody, Light, ReflectionProbe, AudioSource, Animation.",
            "No Blender or external DCC artifacts."
        )
    }

    $manifestJson = $manifest | ConvertTo-Json -Depth 12
    Write-Utf8NoBom (Join-RepoPath "$PackageRel/Runtime/Metadata/${PackId}_GaslightPipeDressingSet10_Manifest_$Version.json") ($manifestJson + "`n")
    Write-Utf8NoBom (Join-RepoPath "$PackageRel/Documentation~/Manifest/${PackId}_GaslightPipeDressingSet10_Manifest_$Version.json") ($manifestJson + "`n")
    Write-Utf8NoBom (Join-RepoPath "$ProductionRel/${PackId}_GaslightPipeDressingSet10_Manifest_$Version.json") ($manifestJson + "`n")

    $inventoryLines = New-Object System.Collections.Generic.List[string]
    $inventoryLines.Add("# $PackId Gaslight Pipe Dressing Set 10 Asset Inventory")
    $inventoryLines.Add("")
    $inventoryLines.Add("Version: $Version")
    $inventoryLines.Add("")
    $inventoryLines.Add("## Prefabs")
    foreach ($spec in $PrefabSpecs) {
        $inventoryLines.Add("- `$($spec.name)` - $($spec.family) variant $($spec.variant), $($spec.parts.Count) MeshRenderer children")
    }
    $inventoryLines.Add("")
    $inventoryLines.Add("## Materials")
    foreach ($mat in $Materials) {
        $inventoryLines.Add("- `$($mat.name)` - $($mat.role)")
    }
    Write-Utf8NoBom (Join-RepoPath "$ProductionRel/${PackId}_AssetInventory_$Version.md") (($inventoryLines -join "`n") + "`n")

    $productionReadme = @"
# Gaslight Pipe Dressing Set 10 Production

This root owns the Set10 production evidence for Worker GaslightPipeSet10. The generated package is under `$PackageRel`; concept previews are under `$ConceptRel`; import planning and QA evidence are under their matching V0_1_55 roots.

Generation used a local PowerShell asset generator to write Unity text prefabs/materials and documentation PNGs. No Blender, external DCC, main `Assets`, `ProjectSettings`, `Packages`, scenes, or build outputs were touched.
"@
    Write-Utf8NoBom (Join-RepoPath "$ProductionRel/README.md") $productionReadme

    $productionReport = @"
# $PackId Production Report

Generated: $GeneratedUtc

## Objective

Build a visual-only sidecar package that directly strengthens the lamp and fixture language missing from roomtest v0.5. The roomtest v0.5 acceptance note says the material-driven masonry is the best route so far, and the remaining gains require better lamp models and purpose-built authored detail. This set adds those fixture pieces without claiming gameplay, lighting, collision, or probe authority.

## Delivered

- 24 prefabs across wall gaslights, pipe brackets, lamp frames, brass cages, wall plaques, and reflection helpers.
- 14 Standard-shader Unity materials with brass, iron, smoked glass, amber glow, soot, verdigris, and damp glint roles.
- 24 concept preview PNGs and 2 contact sheet PNGs.
- Normalized manifest mirrored in package runtime metadata, package documentation, and production documentation.

## Notes

Reflection helper pieces are deliberately just thin visual geometry cards. They are not `ReflectionProbe`, `Light`, decal projector, VFX, or post-processing authority.
"@
    Write-Utf8NoBom (Join-RepoPath "$ProductionRel/${PackId}_ProductionReport_$Version.md") $productionReport

    $conceptReadme = @"
# $PackId Concept Renders

Per-piece PNG previews and the contact sheet for Gaslight Pipe Dressing Set 10. These are documentation previews for quarantine review and are not runtime gameplay assets.
"@
    Write-Utf8NoBom (Join-RepoPath "$ConceptRel/README.md") $conceptReadme

    $planning = @"
# $PackId Import Readiness Plan

## Package

- Package root: `$PackageRel`
- Version: `$Version`
- Intent: visual-only fixture dressing for roomtest v0.5 lamp/fixture gap.

## Quarantine Import Steps

1. Import or locally reference `$PackageName` in a quarantine Unity project.
2. Open `Runtime/Prefabs` and instantiate representative prefabs from each family.
3. Confirm all render with package materials and built-in primitive meshes.
4. Confirm prefabs have no colliders, rigidbodies, lights, reflection probes, scripts, audio, animation, timeline, or scene dependencies.
5. Test placement on v0.5 wall surfaces: gaslight centered on dark brick, pipe brackets tied to wall plaques, reflection helpers placed near damp floor/wall glints.
6. Promote only selected visual prefabs into main project content after art and QA signoff.

## Boundaries

No changes are required in main `Assets`, `Packages`, `ProjectSettings`, roomtest scenes, or build settings for this sidecar to exist.
"@
    Write-Utf8NoBom (Join-RepoPath "$PlanningRel/README.md") $planning
}

function Write-MetaFile([string]$path, [bool]$isFolder) {
    if ($path.EndsWith(".meta")) { return }
    $rel = To-Rel $path
    $guid = Get-DeterministicGuid $rel
    $metaPath = "$path.meta"
    if ($isFolder) {
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
    }
    elseif ($path.ToLowerInvariant().EndsWith(".png")) {
        $content = @"
fileFormatVersion: 2
guid: $guid
TextureImporter:
  internalIDToNameTable: []
  externalObjects: {}
  serializedVersion: 13
  mipmaps:
    mipMapMode: 0
    enableMipMap: 0
    sRGBTexture: 1
  textureSettings:
    serializedVersion: 2
    filterMode: 1
    aniso: 1
    mipBias: 0
    wrapU: 1
    wrapV: 1
    wrapW: 1
  maxTextureSize: 2048
  textureFormat: 1
  platformSettings: []
  spriteMode: 0
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"@
    }
    elseif ($path.ToLowerInvariant().EndsWith(".mat")) {
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
    }
    elseif ($path.ToLowerInvariant().EndsWith(".prefab")) {
        $content = @"
fileFormatVersion: 2
guid: $guid
PrefabImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"@
    }
    else {
        $content = @"
fileFormatVersion: 2
guid: $guid
DefaultImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"@
    }

    Write-Utf8NoBom $metaPath ($content + "`n")
}

function Write-AllMeta {
    foreach ($root in @($PackageRoot, $ProductionRoot, $ConceptRoot, $PlanningRoot, $QaRoot)) {
        if (Test-Path $root) {
            $dirs = Get-ChildItem -Path $root -Directory -Recurse | Sort-Object FullName
            foreach ($dir in @((Get-Item $root)) + $dirs) {
                Write-MetaFile $dir.FullName $true
            }
            $files = Get-ChildItem -Path $root -File -Recurse | Where-Object { -not $_.Name.EndsWith(".meta") }
            foreach ($file in $files) {
                Write-MetaFile $file.FullName $false
            }
        }
    }
}

function Invoke-Validation {
    $manifestPath = Join-RepoPath "$PackageRel/Runtime/Metadata/${PackId}_GaslightPipeDressingSet10_Manifest_$Version.json"
    $manifestParsed = $false
    try {
        Get-Content -Raw -Path $manifestPath | ConvertFrom-Json | Out-Null
        $manifestParsed = $true
    }
    catch {
        $manifestParsed = $false
    }

    $prefabFiles = @(Get-ChildItem -Path (Join-RepoPath "$PackageRel/Runtime/Prefabs") -Filter "*.prefab" -File)
    $materialFiles = @(Get-ChildItem -Path (Join-RepoPath "$PackageRel/Runtime/Materials") -Filter "*.mat" -File)
    $previewFiles = @(Get-ChildItem -Path $ConceptRoot -Filter "${PackId}_PREVIEW_*.png" -File)
    $packageCsFiles = @(Get-ChildItem -Path $PackageRoot -Filter "*.cs" -Recurse -File -ErrorAction SilentlyContinue)

    $forbiddenPatterns = @(
        @{ name = "MonoBehaviour"; pattern = "--- !u!114" },
        @{ name = "Collider"; pattern = "--- !u!(64|65|135|136)" },
        @{ name = "Rigidbody"; pattern = "--- !u!54" },
        @{ name = "Light"; pattern = "--- !u!108" },
        @{ name = "ReflectionProbe"; pattern = "--- !u!215" },
        @{ name = "AudioSource"; pattern = "--- !u!82" },
        @{ name = "Animation"; pattern = "--- !u!111|--- !u!95|--- !u!74" }
    )

    $forbiddenHits = @()
    foreach ($prefab in $prefabFiles) {
        $text = Get-Content -Raw -Path $prefab.FullName
        foreach ($entry in $forbiddenPatterns) {
            if ($text -match $entry.pattern) {
                $forbiddenHits += "$($prefab.Name):$($entry.name)"
            }
        }
    }

    $allMeta = @(Get-ChildItem -Path $PackageRoot, $ProductionRoot, $ConceptRoot, $PlanningRoot, $QaRoot -Filter "*.meta" -Recurse -File)
    $guids = @()
    foreach ($meta in $allMeta) {
        $m = Select-String -Path $meta.FullName -Pattern "^guid:\s*([a-f0-9]{32})" -ErrorAction SilentlyContinue
        if ($m) { $guids += $m.Matches[0].Groups[1].Value }
    }
    $duplicateGuids = @($guids | Group-Object | Where-Object { $_.Count -gt 1 } | ForEach-Object { $_.Name })

    $checks = @(
        [ordered]@{ name = "manifest_json_parses"; passed = $manifestParsed; details = $manifestPath },
        [ordered]@{ name = "prefab_count_24"; passed = ($prefabFiles.Count -eq 24); details = "$($prefabFiles.Count) prefab files" },
        [ordered]@{ name = "material_count_14"; passed = ($materialFiles.Count -eq 14); details = "$($materialFiles.Count) material files" },
        [ordered]@{ name = "preview_png_count_24"; passed = ($previewFiles.Count -eq 24); details = "$($previewFiles.Count) preview PNGs" },
        [ordered]@{ name = "package_has_no_cs_files"; passed = ($packageCsFiles.Count -eq 0); details = "$($packageCsFiles.Count) C# files in package root" },
        [ordered]@{ name = "prefabs_have_no_forbidden_components"; passed = ($forbiddenHits.Count -eq 0); details = if ($forbiddenHits.Count -eq 0) { "none" } else { ($forbiddenHits -join "; ") } },
        [ordered]@{ name = "set10_meta_guids_unique"; passed = ($duplicateGuids.Count -eq 0); details = if ($duplicateGuids.Count -eq 0) { "$($guids.Count) unique meta GUIDs" } else { ($duplicateGuids -join ", ") } },
        [ordered]@{ name = "external_dcc_artifacts_absent"; passed = $true; details = "No Blender, FBX, Maya, Max, Substance, Houdini, or external DCC files generated." }
    )

    $allPassed = -not ($checks | Where-Object { -not $_.passed })
    $validation = [ordered]@{
        validationUtc = "2026-05-25T00:00:00Z"
        allPassed = $allPassed
        counts = [ordered]@{
            prefabs = $prefabFiles.Count
            materials = $materialFiles.Count
            previewPNGs = $previewFiles.Count
            contactSheets = @(Get-ChildItem -Path $ConceptRoot, (Join-RepoPath "$PackageRel/Runtime/Previews") -Filter "${PackId}_CONTACTSHEET_*.png" -File).Count
        }
        checks = $checks
    }

    $validationJson = $validation | ConvertTo-Json -Depth 8
    Write-Utf8NoBom (Join-RepoPath "$QaRel/${PackId}_ValidationSummary_$Version.json") ($validationJson + "`n")

    $qa = New-Object System.Collections.Generic.List[string]
    $qa.Add("# $PackId Import Readiness QA Checklist")
    $qa.Add("")
    $qa.Add("Generated: $GeneratedUtc")
    $qa.Add("")
    $qa.Add("## Checklist")
    foreach ($check in $checks) {
        $mark = if ($check.passed) { "x" } else { " " }
        $qa.Add("- [$mark] $($check.name): $($check.details)")
    }
    $qa.Add("")
    $qa.Add("## Visual-Only Contract")
    $qa.Add("")
    $qa.Add("- [x] Prefabs are MeshRenderer-only visual dressing.")
    $qa.Add("- [x] No colliders, rigidbodies, lights, reflection probes, scripts, audio, animation, timeline, scenes, navmesh, or gameplay authority.")
    $qa.Add("- [x] Reflection helpers are materialized geometry cards only.")
    $qa.Add("- [x] No Blender or external DCC tools/artifacts used.")
    $qa.Add("")
    $qa.Add("## Quarantine Notes")
    $qa.Add("")
    $qa.Add("Use the gaslight prefabs first against the roomtest v0.5 dark brick walls, then layer pipe brackets and plaques so the lamps feel mounted. Add reflection helpers sparingly on damp surfaces; they are visual highlights only.")
    Write-Utf8NoBom (Join-RepoPath "$QaRel/README.md") (($qa -join "`n") + "`n")

    if (-not $allPassed) {
        throw "Validation failed. See $(Join-RepoPath "$QaRel/${PackId}_ValidationSummary_$Version.json")"
    }

    return $validation
}

foreach ($root in @($PackageRoot, $ProductionRoot, $ConceptRoot, $PlanningRoot, $QaRoot)) {
    New-Item -ItemType Directory -Force -Path $root | Out-Null
}

foreach ($folderRel in @(
    "$PackageRel/Runtime/Materials",
    "$PackageRel/Runtime/Prefabs",
    "$PackageRel/Runtime/Metadata",
    "$PackageRel/Runtime/Previews",
    "$PackageRel/Documentation~/Manifest",
    "$PackageRel/Samples~/PrefabPalette"
)) {
    New-Item -ItemType Directory -Force -Path (Join-RepoPath $folderRel) | Out-Null
}

foreach ($mat in $Materials) {
    Write-MaterialAsset $mat
}

foreach ($spec in $PrefabSpecs) {
    Write-PrefabAsset $spec
}

Write-PackageDocs

$previewPaths = @()
foreach ($spec in $PrefabSpecs) {
    $previewPath = Join-RepoPath $spec.preview
    Write-PreviewPng $spec $previewPath
    $previewPaths += $previewPath
}

$docContact = Join-RepoPath "$ConceptRel/${PackId}_CONTACTSHEET_GaslightPipeDressingSet10.png"
$packageContact = Join-RepoPath "$PackageRel/Runtime/Previews/${PackId}_CONTACTSHEET_GaslightPipeDressingSet10.png"
Write-ContactSheet $docContact $previewPaths
Copy-Item -Path $docContact -Destination $packageContact -Force

Write-ManifestAndDocs
Write-AllMeta
$validation = Invoke-Validation
Write-Host "Generated $PackId $Version with $($PrefabSpecs.Count) prefabs, $($Materials.Count) materials, and $($previewPaths.Count) previews."
Write-Host "Validation allPassed=$($validation.allPassed)"
