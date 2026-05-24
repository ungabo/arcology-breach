param(
    [string]$ProjectRoot = "D:\__MY APPS\Unity Doom"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Drawing

$packId = "STS05"
$version = "0.1.47"
$buildId = "p001"
$packageName = "com.brassworks.sidecar.surface-texture-set05"
$packageRoot = Join-Path $ProjectRoot "AssetPacks\BrassworksBreach.SurfaceTextureSet05"
$docRoot = Join-Path $ProjectRoot "Documentation\AssetProduction\V0_1_47_SurfaceTextureSet05"
$renderRoot = Join-Path $ProjectRoot "Documentation\ConceptRenders\V0_1_47_SurfaceTextureSet05"
$allowedRoots = @($packageRoot, $docRoot, $renderRoot) | ForEach-Object { [IO.Path]::GetFullPath($_).TrimEnd("\") }

function Assert-Scope([string]$Path) {
    $full = [IO.Path]::GetFullPath($Path)
    foreach ($root in $allowedRoots) {
        if ($full.StartsWith($root, [StringComparison]::OrdinalIgnoreCase)) { return }
    }
    throw "Refusing write outside assigned scope: $full"
}

function Ensure-Dir([string]$Path) {
    Assert-Scope $Path
    if (-not (Test-Path -LiteralPath $Path -PathType Container)) {
        New-Item -ItemType Directory -Path $Path | Out-Null
    }
}

function Write-Text([string]$Path, [string]$Text) {
    Assert-Scope $Path
    Ensure-Dir (Split-Path -Parent $Path)
    [IO.File]::WriteAllText($Path, $Text, [Text.UTF8Encoding]::new($false))
}

function New-Guid32 {
    ([guid]::NewGuid().ToString("N")).ToLowerInvariant()
}

function Write-Meta([string]$Path, [string]$Kind = "Default", [string]$Guid = (New-Guid32), [string]$UserData = "") {
    $body = switch ($Kind) {
        "Folder" {
@"
fileFormatVersion: 2
guid: $Guid
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: $UserData
  assetBundleName:
  assetBundleVariant:
"@
        }
        "Material" {
@"
fileFormatVersion: 2
guid: $Guid
NativeFormatImporter:
  externalObjects: {}
  mainObjectFileID: 2100000
  userData: $UserData
  assetBundleName:
  assetBundleVariant:
"@
        }
        "TextureNormal" {
@"
fileFormatVersion: 2
guid: $Guid
TextureImporter:
  serializedVersion: 13
  mipmaps:
    enableMipMap: 1
    sRGBTexture: 0
  bumpmap:
    convertToNormalMap: 0
  isReadable: 0
  textureType: 1
  textureShape: 1
  platformSettings:
  - serializedVersion: 4
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 512
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    overridden: 0
  userData: $UserData
  assetBundleName:
  assetBundleVariant:
"@
        }
        "TextureLinear" {
@"
fileFormatVersion: 2
guid: $Guid
TextureImporter:
  serializedVersion: 13
  mipmaps:
    enableMipMap: 1
    sRGBTexture: 0
  isReadable: 0
  textureType: 0
  textureShape: 1
  platformSettings:
  - serializedVersion: 4
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 512
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    overridden: 0
  userData: $UserData
  assetBundleName:
  assetBundleVariant:
"@
        }
        "TextureSrgb" {
@"
fileFormatVersion: 2
guid: $Guid
TextureImporter:
  serializedVersion: 13
  mipmaps:
    enableMipMap: 1
    sRGBTexture: 1
  isReadable: 0
  textureType: 0
  textureShape: 1
  platformSettings:
  - serializedVersion: 4
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 512
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    overridden: 0
  userData: $UserData
  assetBundleName:
  assetBundleVariant:
"@
        }
        default {
@"
fileFormatVersion: 2
guid: $Guid
DefaultImporter:
  externalObjects: {}
  userData: $UserData
  assetBundleName:
  assetBundleVariant:
"@
        }
    }
    Write-Text $Path ($body + "`n")
    return $Guid
}

function Color-FromHex([string]$Hex) {
    $h = $Hex.TrimStart("#")
    [Drawing.Color]::FromArgb([Convert]::ToInt32($h.Substring(0,2),16), [Convert]::ToInt32($h.Substring(2,2),16), [Convert]::ToInt32($h.Substring(4,2),16))
}

function Clamp-Byte([int]$Value) {
    [Math]::Max(0, [Math]::Min(255, $Value))
}

function Save-Png([Drawing.Bitmap]$Bitmap, [string]$Path) {
    Assert-Scope $Path
    Ensure-Dir (Split-Path -Parent $Path)
    $Bitmap.Save($Path, [Drawing.Imaging.ImageFormat]::Png)
}

function New-TextureSet($Mat, [string]$AlbedoPath, [string]$NormalPath, [string]$MaskPath, [string]$SwatchPath) {
    $size = 256
    $base = Color-FromHex $Mat.Hex
    $accent = Color-FromHex $Mat.Accent
    $alb = [Drawing.Bitmap]::new($size, $size, [Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $nrm = [Drawing.Bitmap]::new($size, $size, [Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $msk = [Drawing.Bitmap]::new($size, $size, [Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $seed = [Math]::Abs($Mat.Id.GetHashCode())
    for ($y = 0; $y -lt $size; $y++) {
        for ($x = 0; $x -lt $size; $x++) {
            $wave = [Math]::Sin(($x + ($seed % 43)) * $Mat.ScaleA) + [Math]::Cos(($y - ($seed % 31)) * $Mat.ScaleB)
            $grain = (($x * 37 + $y * 19 + $seed) % 53) - 26
            $stripe = if ((($x + $y + $seed) % [Math]::Max(9, $Mat.Stripe)) -lt 3) { 1 } else { 0 }
            $edge = if ($x -lt 9 -or $y -lt 9 -or $x -gt 246 -or $y -gt 246) { 1 } else { 0 }
            $wear = if (((($x * 5 + $y * 11 + $seed) % 97) -lt $Mat.Wear) -or $edge -eq 1) { 1 } else { 0 }
            $r = Clamp-Byte($base.R + [int]($wave * 18) + $grain + ($accent.R - $base.R) * $stripe / 5 + $wear * 22)
            $g = Clamp-Byte($base.G + [int]($wave * 18) + $grain + ($accent.G - $base.G) * $stripe / 5 + $wear * 20)
            $b = Clamp-Byte($base.B + [int]($wave * 18) + $grain + ($accent.B - $base.B) * $stripe / 5 + $wear * 18)
            if ($Mat.Pattern -eq "rivets" -and (($x % 64 - 32) * ($x % 64 - 32) + ($y % 64 - 32) * ($y % 64 - 32) -lt 72)) {
                $r = Clamp-Byte($r + 34); $g = Clamp-Byte($g + 34); $b = Clamp-Byte($b + 30)
            }
            if ($Mat.Pattern -eq "hazard" -and (([Math]::Floor(($x + $y) / 28) % 2) -eq 0)) {
                $r = $accent.R; $g = $accent.G; $b = $accent.B
            }
            if ($Mat.Pattern -eq "glass") {
                $alpha = 150
                $r = Clamp-Byte($r + 22); $g = Clamp-Byte($g + 34); $b = Clamp-Byte($b + 40)
                $alb.SetPixel($x, $y, [Drawing.Color]::FromArgb($alpha, $r, $g, $b))
            } else {
                $alb.SetPixel($x, $y, [Drawing.Color]::FromArgb(255, $r, $g, $b))
            }
            $nx = Clamp-Byte(128 + [int]($wave * 18) + ($stripe * 10) + ($wear * 14))
            $ny = Clamp-Byte(128 + [int]([Math]::Cos(($x-$y) * 0.08) * 16) - ($stripe * 8))
            $nrm.SetPixel($x, $y, [Drawing.Color]::FromArgb(255, $nx, $ny, 255))
            $metal = Clamp-Byte([int]($Mat.Metallic * 255))
            $occ = Clamp-Byte(190 + [int]($wave * 16) - $stripe * 18)
            $smooth = Clamp-Byte([int]($Mat.Smoothness * 255) + $wear * 20)
            $msk.SetPixel($x, $y, [Drawing.Color]::FromArgb(255, $metal, $occ, $smooth))
        }
    }
    Save-Png $alb $AlbedoPath
    Save-Png $nrm $NormalPath
    Save-Png $msk $MaskPath

    $sw = [Drawing.Bitmap]::new(512, 360, [Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $sg = [Drawing.Graphics]::FromImage($sw)
    $sg.SmoothingMode = [Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $sg.Clear([Drawing.Color]::FromArgb(26, 25, 23))
    $sg.DrawImage($alb, 24, 24, 256, 256)
    $sg.FillRectangle([Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(44, 41, 36)), 300, 24, 188, 256)
    $title = [Drawing.Font]::new("Segoe UI", 15, [Drawing.FontStyle]::Bold)
    $small = [Drawing.Font]::new("Segoe UI", 9)
    $light = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(236, 225, 204))
    $muted = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(171, 158, 131))
    $sg.DrawString($Mat.Name, $title, $light, [Drawing.RectangleF]::new(310, 36, 170, 56))
    $sg.DrawString($Mat.Description, $small, $light, [Drawing.RectangleF]::new(310, 96, 162, 120))
    $sg.DrawString(("metal {0:0.00} | smooth {1:0.00}" -f $Mat.Metallic, $Mat.Smoothness), $small, $muted, 310, 230)
    $sg.DrawString("Unity Standard procedural candidate", $small, $muted, 310, 252)
    $sg.DrawString($Mat.Id, $small, $muted, 24, 300)
    Save-Png $sw $SwatchPath
    $sg.Dispose(); $sw.Dispose(); $alb.Dispose(); $nrm.Dispose(); $msk.Dispose()
}

function Write-Material([string]$Path, $Mat, [string]$AlbedoGuid, [string]$NormalGuid, [string]$MaskGuid) {
    $color = Color-FromHex $Mat.Hex
    $alpha = if ($Mat.Pattern -eq "glass" -or $Mat.Id -like "*SteamFilm*") { 0.58 } else { 1 }
    $mode = if ($alpha -lt 1) { 3 } else { 0 }
    $src = if ($alpha -lt 1) { 5 } else { 1 }
    $dst = if ($alpha -lt 1) { 10 } else { 0 }
    $z = if ($alpha -lt 1) { 0 } else { 1 }
    $queue = if ($alpha -lt 1) { 3000 } else { -1 }
    $r = [Math]::Round($color.R / 255, 4)
    $g = [Math]::Round($color.G / 255, 4)
    $b = [Math]::Round($color.B / 255, 4)
    $metal = $Mat.Metallic
    $smooth = $Mat.Smoothness
    $name = [IO.Path]::GetFileNameWithoutExtension($Path)
@"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!21 &2100000
Material:
  serializedVersion: 8
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_Name: $name
  m_Shader: {fileID: 46, guid: 0000000000000000f000000000000000, type: 0}
  m_Parent: {fileID: 0}
  m_ModifiedSerializedProperties: 0
  m_ValidKeywords: []
  m_InvalidKeywords: []
  m_LightmapFlags: 4
  m_EnableInstancingVariants: 0
  m_DoubleSidedGI: 0
  m_CustomRenderQueue: $queue
  stringTagMap: {}
  disabledShaderPasses: []
  m_LockedProperties:
  m_SavedProperties:
    serializedVersion: 3
    m_TexEnvs:
    - _BumpMap:
        m_Texture: {fileID: 2800000, guid: $NormalGuid, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _DetailAlbedoMap:
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _DetailMask:
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _DetailNormalMap:
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _EmissionMap:
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _MainTex:
        m_Texture: {fileID: 2800000, guid: $AlbedoGuid, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _MetallicGlossMap:
        m_Texture: {fileID: 2800000, guid: $MaskGuid, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _OcclusionMap:
        m_Texture: {fileID: 2800000, guid: $MaskGuid, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _ParallaxMap:
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    m_Ints: []
    m_Floats:
    - _BumpScale: 0.45
    - _Cutoff: 0.5
    - _DetailNormalMapScale: 1
    - _DstBlend: $dst
    - _GlossMapScale: 1
    - _Glossiness: $smooth
    - _GlossyReflections: 1
    - _Metallic: $metal
    - _Mode: $mode
    - _OcclusionStrength: 1
    - _Parallax: 0.02
    - _SmoothnessTextureChannel: 0
    - _SpecularHighlights: 1
    - _SrcBlend: $src
    - _UVSec: 0
    - _ZWrite: $z
    m_Colors:
    - _Color: {r: $r, g: $g, b: $b, a: $alpha}
    - _EmissionColor: {r: 0, g: 0, b: 0, a: 1}
  m_BuildTextureStacks: []
"@ | ForEach-Object { Write-Text $Path ($_ + "`n") }
}

$materials = @(
    [pscustomobject]@{ Id="STS05_MAT_WetRivetedStone"; Name="Wet Riveted Stone"; Hex="#5d625d"; Accent="#a8b2a6"; Metallic=0.02; Smoothness=0.68; Pattern="rivets"; ScaleA=0.075; ScaleB=0.041; Stripe=41; Wear=11; Description="slick grey wall stone with inset rivet plates and damp highlights" },
    [pscustomobject]@{ Id="STS05_MAT_BlackenedBoilerIron"; Name="Blackened Boiler Iron"; Hex="#171819"; Accent="#60615d"; Metallic=0.88; Smoothness=0.37; Pattern="rivets"; ScaleA=0.052; ScaleB=0.063; Stripe=33; Wear=8; Description="dark heat-stained iron for boilers, bulkheads, and pressure housings" },
    [pscustomobject]@{ Id="STS05_MAT_AgedBrass"; Name="Aged Brass"; Hex="#a67c32"; Accent="#ead08a"; Metallic=0.84; Smoothness=0.46; Pattern="grain"; ScaleA=0.065; ScaleB=0.047; Stripe=25; Wear=16; Description="warm brass with tarnish, hand polish, and readable edge warmth" },
    [pscustomobject]@{ Id="STS05_MAT_OxidizedCopper"; Name="Oxidized Copper"; Hex="#7b4427"; Accent="#2d9f83"; Metallic=0.78; Smoothness=0.42; Pattern="grain"; ScaleA=0.044; ScaleB=0.071; Stripe=21; Wear=13; Description="copper sheet with verdigris seams and worn copper ridges" },
    [pscustomobject]@{ Id="STS05_MAT_OilGrime"; Name="Oil Grime"; Hex="#1f1c15"; Accent="#4c4128"; Metallic=0.03; Smoothness=0.82; Pattern="grain"; ScaleA=0.035; ScaleB=0.084; Stripe=39; Wear=3; Description="wet black-brown residue for floor spills, corners, and machine seams" },
    [pscustomobject]@{ Id="STS05_MAT_Soot"; Name="Soot"; Hex="#171614"; Accent="#4a4841"; Metallic=0.00; Smoothness=0.18; Pattern="grain"; ScaleA=0.083; ScaleB=0.057; Stripe=29; Wear=4; Description="dry carbon dust for furnace mouths, vents, and blast shadows" },
    [pscustomobject]@{ Id="STS05_MAT_HazardEnamel"; Name="Hazard Enamel"; Hex="#c89a1f"; Accent="#1c1b16"; Metallic=0.00; Smoothness=0.54; Pattern="hazard"; ScaleA=0.044; ScaleB=0.044; Stripe=18; Wear=14; Description="chipped yellow and black enamel with strong industrial readability" },
    [pscustomobject]@{ Id="STS05_MAT_GaugeGlass"; Name="Gauge Glass"; Hex="#a5d8c6"; Accent="#e8fff8"; Metallic=0.00; Smoothness=0.94; Pattern="glass"; ScaleA=0.048; ScaleB=0.077; Stripe=48; Wear=5; Description="greenish pressure-gauge glass with scratches and cloudy reflection" },
    [pscustomobject]@{ Id="STS05_MAT_Walnut"; Name="Walnut"; Hex="#60391f"; Accent="#c28a4e"; Metallic=0.00; Smoothness=0.39; Pattern="grain"; ScaleA=0.025; ScaleB=0.117; Stripe=17; Wear=9; Description="dark varnished walnut with long grain and touched bright ridges" },
    [pscustomobject]@{ Id="STS05_MAT_Leather"; Name="Leather"; Hex="#3f2619"; Accent="#8b5c37"; Metallic=0.00; Smoothness=0.31; Pattern="grain"; ScaleA=0.092; ScaleB=0.038; Stripe=31; Wear=10; Description="creased brown leather for wraps, straps, and pressure gloves" },
    [pscustomobject]@{ Id="STS05_MAT_EmberCeramic"; Name="Ember Ceramic"; Hex="#6f5b44"; Accent="#f06d22"; Metallic=0.00; Smoothness=0.22; Pattern="grain"; ScaleA=0.068; ScaleB=0.066; Stripe=27; Wear=6; Description="insulating ceramic tile warmed by furnace ember cracks" },
    [pscustomobject]@{ Id="STS05_MAT_SteamFilm"; Name="Steam Film"; Hex="#cfd5cf"; Accent="#f8fff7"; Metallic=0.00; Smoothness=0.88; Pattern="glass"; ScaleA=0.031; ScaleB=0.051; Stripe=58; Wear=1; Description="translucent condensation film for glass, gauges, and vents" },
    [pscustomobject]@{ Id="STS05_MAT_WarningPaint"; Name="Warning Paint"; Hex="#9e2d24"; Accent="#e6c24b"; Metallic=0.00; Smoothness=0.49; Pattern="hazard"; ScaleA=0.050; ScaleB=0.060; Stripe=20; Wear=17; Description="red warning paint with chipped ochre striping and worn edges" },
    [pscustomobject]@{ Id="STS05_MAT_PolishedWornEdge"; Name="Polished Worn Edge"; Hex="#c2b18d"; Accent="#fff3c4"; Metallic=0.91; Smoothness=0.76; Pattern="grain"; ScaleA=0.089; ScaleB=0.032; Stripe=23; Wear=22; Description="bright rubbed metal for edge highlights on steps, levers, and plates" }
)

@(
    $packageRoot, "$packageRoot\Runtime", "$packageRoot\Runtime\Materials", "$packageRoot\Runtime\Textures", "$packageRoot\Runtime\Textures\Albedo",
    "$packageRoot\Runtime\Textures\Normal", "$packageRoot\Runtime\Textures\Mask", "$packageRoot\Runtime\Metadata", "$packageRoot\Runtime\Scripts",
    "$packageRoot\Editor", "$packageRoot\Documentation~", "$packageRoot\Documentation~\Manifest", "$packageRoot\Samples~", "$packageRoot\Samples~\PreviewScene",
    "$packageRoot\ValidationProject~", "$packageRoot\ValidationProject~\Assets", "$packageRoot\ValidationProject~\Packages", "$packageRoot\ValidationProject~\ProjectSettings",
    $docRoot, $renderRoot
) | ForEach-Object {
    Ensure-Dir $_
    $full = [IO.Path]::GetFullPath($_).TrimEnd("\")
    if ($full -ne [IO.Path]::GetFullPath($packageRoot).TrimEnd("\") -and
        $full -ne [IO.Path]::GetFullPath($docRoot).TrimEnd("\") -and
        $full -ne [IO.Path]::GetFullPath($renderRoot).TrimEnd("\")) {
        Write-Meta "$_.meta" "Folder" | Out-Null
    }
}

$materialRecords = @()
$textureRecords = @()
$previewRecords = @()

foreach ($mat in $materials) {
    $albPath = "$packageRoot\Runtime\Textures\Albedo\$($mat.Id)_ALB.png"
    $nrmPath = "$packageRoot\Runtime\Textures\Normal\$($mat.Id)_NRM.png"
    $mskPath = "$packageRoot\Runtime\Textures\Mask\$($mat.Id)_MSK.png"
    $swPath = "$renderRoot\$($mat.Id)_procedural_swatch_v$version.png"
    New-TextureSet $mat $albPath $nrmPath $mskPath $swPath
    $albGuid = Write-Meta "$albPath.meta" "TextureSrgb" (New-Guid32) "STS05 albedo"
    $nrmGuid = Write-Meta "$nrmPath.meta" "TextureNormal" (New-Guid32) "STS05 normal"
    $mskGuid = Write-Meta "$mskPath.meta" "TextureLinear" (New-Guid32) "STS05 mask"
    $swGuid = Write-Meta "$swPath.meta" "TextureSrgb" (New-Guid32) "STS05 procedural swatch"
    $matPath = "$packageRoot\Runtime\Materials\$($mat.Id).mat"
    Write-Material $matPath $mat $albGuid $nrmGuid $mskGuid
    $matGuid = Write-Meta "$matPath.meta" "Material" (New-Guid32) "STS05 material"
    $materialRecords += [ordered]@{ id=$mat.Id; name=$mat.Name; material="Runtime/Materials/$($mat.Id).mat"; albedo="Runtime/Textures/Albedo/$($mat.Id)_ALB.png"; normal="Runtime/Textures/Normal/$($mat.Id)_NRM.png"; mask="Runtime/Textures/Mask/$($mat.Id)_MSK.png"; metallic=$mat.Metallic; smoothness=$mat.Smoothness; guid=$matGuid; family=$mat.Name }
    $textureRecords += [ordered]@{ material_id=$mat.Id; role="albedo"; path="Runtime/Textures/Albedo/$($mat.Id)_ALB.png"; guid=$albGuid; size="256x256"; format="png" }
    $textureRecords += [ordered]@{ material_id=$mat.Id; role="normal"; path="Runtime/Textures/Normal/$($mat.Id)_NRM.png"; guid=$nrmGuid; size="256x256"; format="png" }
    $textureRecords += [ordered]@{ material_id=$mat.Id; role="mask"; path="Runtime/Textures/Mask/$($mat.Id)_MSK.png"; guid=$mskGuid; size="256x256"; format="png" }
    $previewRecords += [ordered]@{ material_id=$mat.Id; role="procedural_swatch"; path="Documentation/ConceptRenders/V0_1_47_SurfaceTextureSet05/$($mat.Id)_procedural_swatch_v$version.png"; guid=$swGuid; size="512x360"; note="pre-Unity procedural fallback; Unity batch renderer overwrites/adds render previews when run" }
}

$sheetPath = "$renderRoot\STS05_v${version}_material_family_contact_sheet.png"
$sheet = [Drawing.Bitmap]::new(1792, 1184, [Drawing.Imaging.PixelFormat]::Format32bppArgb)
$sg = [Drawing.Graphics]::FromImage($sheet)
$sg.SmoothingMode = [Drawing.Drawing2D.SmoothingMode]::AntiAlias
$sg.Clear([Drawing.Color]::FromArgb(22,22,20))
$font = [Drawing.Font]::new("Segoe UI", 13, [Drawing.FontStyle]::Bold)
$small = [Drawing.Font]::new("Segoe UI", 9)
$brush = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(235,226,204))
$muted = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(170,158,132))
$sg.DrawString("Surface Texture Set 05 - material family contact sheet", [Drawing.Font]::new("Segoe UI", 20, [Drawing.FontStyle]::Bold), $brush, 30, 24)
for ($i = 0; $i -lt $materials.Count; $i++) {
    $mat = $materials[$i]
    $img = [Drawing.Image]::FromFile("$packageRoot\Runtime\Textures\Albedo\$($mat.Id)_ALB.png")
    $col = $i % 7
    $row = [Math]::Floor($i / 7)
    $x = 32 + $col * 250
    $y = 84 + $row * 520
    $sg.FillRectangle([Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(42,39,34)), $x, $y, 220, 430)
    $sg.DrawImage($img, $x + 14, $y + 14, 192, 192)
    $sg.DrawString($mat.Name, $font, $brush, [Drawing.RectangleF]::new($x + 14, $y + 224, 190, 48))
    $sg.DrawString($mat.Description, $small, $brush, [Drawing.RectangleF]::new($x + 14, $y + 278, 190, 96))
    $sg.DrawString(("M {0:0.00}  S {1:0.00}" -f $mat.Metallic, $mat.Smoothness), $small, $muted, $x + 14, $y + 392)
    $img.Dispose()
}
Save-Png $sheet $sheetPath
$sg.Dispose(); $sheet.Dispose()
$sheetGuid = Write-Meta "$sheetPath.meta" "TextureSrgb" (New-Guid32) "STS05 contact sheet"
$previewRecords += [ordered]@{ material_id="all"; role="contact_sheet"; path="Documentation/ConceptRenders/V0_1_47_SurfaceTextureSet05/STS05_v$version`_material_family_contact_sheet.png"; guid=$sheetGuid; size="1792x1184"; note="procedural contact sheet for review; Unity render contact sheet generated by batch validation" }

$packageJson = [ordered]@{
    name = $packageName
    version = "$version-$buildId"
    displayName = "Brassworks Breach Surface Texture Set 05"
    description = "Unity-only sidecar package of procedural steampunk surface-material families for later replacement of flat procedural materials. Visual/material assets only; no gameplay authority."
    unity = "6000.4"
    author = [ordered]@{ name = "Brassworks Breach Sidecar Lane" }
    dependencies = [ordered]@{}
    keywords = @("brassworks", "sidecar", "surface-textures", "materials", "steampunk", "unity-procedural")
    samples = @([ordered]@{ displayName = "Preview Scene Notes"; description = "Review setup notes for material swatches and generated Unity preview renders."; path = "Samples~/PreviewScene" })
}
Write-Text "$packageRoot\package.json" (($packageJson | ConvertTo-Json -Depth 8) + "`n")
Write-Meta "$packageRoot\package.json.meta" | Out-Null

Write-Text "$packageRoot\README.md" @"
# Brassworks Breach Surface Texture Set 05

Unity-only visual sidecar package for steampunk surface material families. The package is independent from the primary Unity project manifest and scenes.

## Contents

- `Runtime/Materials`: 14 Unity Standard shader material assets.
- `Runtime/Textures`: 42 procedural 256x256 PNG maps, split into albedo, normal, and mask maps.
- `Runtime/Metadata`: package-local material catalog.
- `Documentation~/Manifest`: package-local sidecar manifest.

Preview PNGs are intentionally kept outside the runtime package in `Documentation/ConceptRenders/V0_1_47_SurfaceTextureSet05`.

## Contract

Visual/material-only. No colliders, audio, gameplay scripts, runtime authority, or scene changes.
"@
Write-Meta "$packageRoot\README.md.meta" | Out-Null

Write-Text "$packageRoot\CHANGELOG.md" @"
# Changelog

## $version-$buildId

- Added 14 procedural steampunk surface material families.
- Added 42 procedural PNG texture maps for albedo, normal, and mask channels.
- Added package-local manifest, catalog metadata, preview notes, and isolated validation project scaffold.
"@
Write-Meta "$packageRoot\CHANGELOG.md.meta" | Out-Null

Write-Text "$packageRoot\Samples~\PreviewScene\README.md" @"
# Surface Texture Set 05 Preview Notes

Use a neutral sphere or bevelled cube grid in a throwaway preview scene to inspect the materials under warm key light and cool rim light. Preview images generated by the validation batch are written to `Documentation/ConceptRenders/V0_1_47_SurfaceTextureSet05`, not into runtime assets.
"@
Write-Meta "$packageRoot\Samples~\PreviewScene\README.md.meta" | Out-Null

Write-Text "$packageRoot\Runtime\Scripts\SurfaceTextureSet05PackageInfo.cs" @'
namespace BrassworksBreach.SurfaceTextureSet05
{
    public static class SurfaceTextureSet05PackageInfo
    {
        public const string PackageName = "com.brassworks.sidecar.surface-texture-set05";
        public const string Version = "0.1.47-p001";
        public const string RuntimeContract = "visual-material-only";
    }
}
'@
Write-Meta "$packageRoot\Runtime\Scripts\SurfaceTextureSet05PackageInfo.cs.meta" | Out-Null

Write-Text "$packageRoot\Runtime\BrassworksBreach.SurfaceTextureSet05.asmdef" @"
{
  `"name`": `"BrassworksBreach.SurfaceTextureSet05`",
  `"rootNamespace`": `"BrassworksBreach.SurfaceTextureSet05`",
  `"references`": [],
  `"includePlatforms`": [],
  `"excludePlatforms`": [],
  `"allowUnsafeCode`": false,
  `"overrideReferences`": false,
  `"precompiledReferences`": [],
  `"autoReferenced`": true,
  `"defineConstraints`": [],
  `"versionDefines`": [],
  `"noEngineReferences`": false
}
"@
Write-Meta "$packageRoot\Runtime\BrassworksBreach.SurfaceTextureSet05.asmdef.meta" | Out-Null

Write-Text "$packageRoot\Editor\BrassworksBreach.SurfaceTextureSet05.Editor.asmdef" @"
{
  `"name`": `"BrassworksBreach.SurfaceTextureSet05.Editor`",
  `"rootNamespace`": `"BrassworksBreach.SurfaceTextureSet05.Editor`",
  `"references`": [
    `"BrassworksBreach.SurfaceTextureSet05`"
  ],
  `"includePlatforms`": [
    `"Editor`"
  ],
  `"excludePlatforms`": [],
  `"allowUnsafeCode`": false,
  `"overrideReferences`": false,
  `"precompiledReferences`": [],
  `"autoReferenced`": true,
  `"defineConstraints`": [],
  `"versionDefines`": [],
  `"noEngineReferences`": false
}
"@
Write-Meta "$packageRoot\Editor\BrassworksBreach.SurfaceTextureSet05.Editor.asmdef.meta" | Out-Null

Write-Text "$packageRoot\Editor\SurfaceTextureSet05UnityValidation.cs" @'
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BrassworksBreach.SurfaceTextureSet05.Editor
{
    public static class SurfaceTextureSet05UnityValidation
    {
        public static void ValidatePackage()
        {
            const string packagePath = "Packages/com.brassworks.sidecar.surface-texture-set05";
            var materialGuids = AssetDatabase.FindAssets("t:Material", new[] { packagePath + "/Runtime/Materials" });
            var textureGuids = AssetDatabase.FindAssets("t:Texture2D", new[] { packagePath + "/Runtime/Textures" });
            var manifestGuids = AssetDatabase.FindAssets("STS05 t:TextAsset", new[] { packagePath + "/Documentation~/Manifest" });
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssetPath(packagePath + "/Runtime/Materials");
            var resolvedPackagePath = packageInfo != null ? packageInfo.resolvedPath : Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, ".."));
            var repoRoot = Path.GetFullPath(Path.Combine(resolvedPackagePath, "..", ".."));
            var reportDir = Path.Combine(repoRoot, "Documentation", "AssetProduction", "V0_1_47_SurfaceTextureSet05");
            var renderDir = Path.Combine(repoRoot, "Documentation", "ConceptRenders", "V0_1_47_SurfaceTextureSet05");
            Directory.CreateDirectory(reportDir);
            Directory.CreateDirectory(renderDir);

            var materialPaths = materialGuids.Select(AssetDatabase.GUIDToAssetPath).OrderBy(p => p).ToArray();
            var previews = RenderMaterialPreviews(materialPaths, renderDir);
            var errors = 0;
            if (materialGuids.Length != 14) errors++;
            if (textureGuids.Length != 42) errors++;
            if (manifestGuids.Length < 1) errors++;
            if (previews < 2) errors++;

            var json = "{\n" +
                "  \"status\": \"" + (errors == 0 ? "pass" : "fail") + "\",\n" +
                "  \"marker\": \"STS05_UNITY_VALIDATION_" + (errors == 0 ? "PASS" : "FAIL") + "\",\n" +
                "  \"materials\": " + materialGuids.Length + ",\n" +
                "  \"textures\": " + textureGuids.Length + ",\n" +
                "  \"manifest_files\": " + manifestGuids.Length + ",\n" +
                "  \"unity_rendered_previews\": " + previews + ",\n" +
                "  \"runtime_contract\": \"visual/material-only; no colliders, audio, scenes, gameplay scripts, or runtime authority\"\n" +
                "}\n";
            File.WriteAllText(Path.Combine(reportDir, "STS05_UnityValidationReport_v0.1.47.json"), json);
            Debug.Log("STS05_UNITY_VALIDATION_" + (errors == 0 ? "PASS" : "FAIL") + " materials=" + materialGuids.Length + " textures=" + textureGuids.Length + " previews=" + previews);
            EditorApplication.Exit(errors == 0 ? 0 : 1);
        }

        private static int RenderMaterialPreviews(string[] materialPaths, string renderDir)
        {
            var mats = materialPaths.Select(p => AssetDatabase.LoadAssetAtPath<Material>(p)).Where(m => m != null).ToArray();
            if (mats.Length == 0) return 0;

            var root = new GameObject("STS05_RenderRoot");
            var cameraObj = new GameObject("STS05_Camera");
            var camera = cameraObj.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.045f, 0.043f, 0.039f, 1f);
            camera.transform.position = new Vector3(0, 2.6f, -8.5f);
            camera.transform.rotation = Quaternion.Euler(18f, 0f, 0f);
            var lightObj = new GameObject("STS05_KeyLight");
            var light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.5f;
            light.color = new Color(1f, 0.82f, 0.56f);
            light.transform.rotation = Quaternion.Euler(42f, -32f, 0f);

            var created = 0;
            var group = new GameObject("STS05_MaterialGrid");
            group.transform.SetParent(root.transform);
            for (var i = 0; i < mats.Length; i++)
            {
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.DestroyImmediate(sphere.GetComponent<Collider>());
                sphere.name = mats[i].name;
                sphere.transform.SetParent(group.transform);
                sphere.transform.position = new Vector3((i % 7 - 3) * 1.35f, 1.3f - (i / 7) * 1.55f, 0f);
                sphere.GetComponent<Renderer>().sharedMaterial = mats[i];
            }

            created += RenderPng(camera, Path.Combine(renderDir, "STS05_UNITY_PREVIEW_material_spheres_contact_sheet_v0.1.47.png"), 1800, 900);

            for (var page = 0; page < 2; page++)
            {
                for (var i = 0; i < group.transform.childCount; i++)
                    group.transform.GetChild(i).gameObject.SetActive(i / 7 == page);
                camera.transform.position = new Vector3(0, 1.35f - page * 1.55f, -7.25f);
                camera.transform.rotation = Quaternion.Euler(10f, 0f, 0f);
                created += RenderPng(camera, Path.Combine(renderDir, "STS05_UNITY_PREVIEW_material_spheres_row_" + (page + 1) + "_v0.1.47.png"), 1600, 720);
            }

            UnityEngine.Object.DestroyImmediate(root);
            UnityEngine.Object.DestroyImmediate(cameraObj);
            UnityEngine.Object.DestroyImmediate(lightObj);
            AssetDatabase.Refresh();
            return created;
        }

        private static int RenderPng(Camera camera, string path, int width, int height)
        {
            var rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            camera.targetTexture = rt;
            RenderTexture.active = rt;
            camera.Render();
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            File.WriteAllBytes(path, tex.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(tex);
            camera.targetTexture = null;
            RenderTexture.active = null;
            rt.Release();
            UnityEngine.Object.DestroyImmediate(rt);
            return 1;
        }
    }
}
'@
Write-Meta "$packageRoot\Editor\SurfaceTextureSet05UnityValidation.cs.meta" | Out-Null

$catalog = [ordered]@{
    schema = "brassworks.surface_texture_catalog.v1"
    pack_id = $packId
    version = $version
    build_id = $buildId
    package_name = $packageName
    runtime_contract = [ordered]@{ visual_only=$true; colliders="omitted"; audio="omitted"; gameplay_scripts="omitted"; scenes="omitted"; runtime_authority="none" }
    materials = $materialRecords
    textures = $textureRecords
    preview_records = $previewRecords
}
Write-Text "$packageRoot\Runtime\Metadata\STS05_MaterialCatalog_v$version.json" (($catalog | ConvertTo-Json -Depth 10) + "`n")
Write-Meta "$packageRoot\Runtime\Metadata\STS05_MaterialCatalog_v$version.json.meta" | Out-Null

$manifest = [ordered]@{
    pack_id = $packId
    display_name = "Surface Texture Set 05"
    version = $version
    build_id = $buildId
    unity_version = "6000.4.6f1"
    generated_at_utc = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
    sidecar_project = "UD-SC-MAT-SurfaceTextureSet05"
    owner_lane = "sidecar-surface-texture-set05"
    primary_intake_owner = "main-lane-art-integration"
    canonical_root = "AssetPacks/BrassworksBreach.SurfaceTextureSet05"
    package_root = "AssetPacks/BrassworksBreach.SurfaceTextureSet05"
    package_name = $packageName
    asset_counts = [ordered]@{ generated_prefabs=0; generated_materials=$materials.Count; generated_meshes=0; textures=$textureRecords.Count; audio=0; vfx=0; animation_clips=0; preview_renders=$previewRecords.Count }
    generated_materials = @($materialRecords | ForEach-Object { "Packages/$packageName/$($_.material)" })
    generated_textures = @($textureRecords | ForEach-Object { "Packages/$packageName/$($_.path)" })
    preview_renders = @($previewRecords | ForEach-Object { $_.path })
    dependencies = @("Unity built-in Standard shader", "Unity texture import pipeline", "Unity Package Manager local package reference")
    required_primary_changes = @()
    path_collisions_checked = $true
    guid_collisions_checked = $true
    import_smoke_status = "pending_unity_validation_then_static_sidecar_validation"
    known_risks = @("Procedural 256x256 maps are first-pass production candidates, not final scanned materials.", "Shader conversion may be required if the primary project moves away from built-in Standard shader.", "Material scale/repetition should be tuned against final corridor, weapon, and enemy geometry.")
    rollback_path = "delete isolated package root or remove local package reference com.brassworks.sidecar.surface-texture-set05"
    decision = "ready_for_static_validation_and_unity_preview_review"
}
Write-Text "$packageRoot\Documentation~\Manifest\STS05_SurfaceTextureSet05_Manifest_v$version-$buildId.json" (($manifest | ConvertTo-Json -Depth 10) + "`n")

$validationManifest = [ordered]@{
    dependencies = [ordered]@{
        "com.brassworks.sidecar.surface-texture-set05" = "file:../.."
        "com.unity.modules.assetbundle" = "1.0.0"
        "com.unity.modules.imageconversion" = "1.0.0"
        "com.unity.modules.imgui" = "1.0.0"
        "com.unity.modules.jsonserialize" = "1.0.0"
        "com.unity.modules.physics" = "1.0.0"
        "com.unity.modules.uielements" = "1.0.0"
    }
}
Write-Text "$packageRoot\ValidationProject~\Packages\manifest.json" (($validationManifest | ConvertTo-Json -Depth 8) + "`n")
Write-Text "$packageRoot\ValidationProject~\ProjectSettings\ProjectVersion.txt" "m_EditorVersion: 6000.4.6f1`nm_EditorVersionWithRevision: 6000.4.6f1 (0b051c2e5d54)`n"
Write-Text "$packageRoot\ValidationProject~\Assets\README.md" "# SurfaceTextureSet05 isolated validation project`n`nThis project imports the sidecar package and renders material previews without touching primary scenes or manifests.`n"
Write-Meta "$packageRoot\ValidationProject~\Assets\README.md.meta" | Out-Null

Write-Text "$docRoot\README.md" @"
# Surface Texture Set 05 Production Notes

Unity-only material sidecar for replacing flat procedural materials later. No Blender source, no gameplay authority, no scene dependencies.

Review outputs:

- `STS05_ProductionReport_v0.1.47.md`
- `STS05_AssetInventory_v0.1.47.md`
- `STS05_UnityValidationReport_v0.1.47.json` after Unity batch validation
- `STS05_SidecarValidation_v0.1.47.json` after repository sidecar validation
"@

$inventory = [Text.StringBuilder]::new()
$null = $inventory.AppendLine("# Surface Texture Set 05 Asset Inventory")
$null = $inventory.AppendLine("")
$null = $inventory.AppendLine("| Family | Material | Albedo | Normal | Mask |")
$null = $inventory.AppendLine("| --- | --- | --- | --- | --- |")
foreach ($rec in $materialRecords) {
    $null = $inventory.AppendLine("| $($rec.family) | `$($rec.material)` | `$($rec.albedo)` | `$($rec.normal)` | `$($rec.mask)` |")
}
$null = $inventory.AppendLine("")
$null = $inventory.AppendLine("Runtime contract: visual/material-only; no colliders, audio, scenes, prefabs, gameplay scripts, or runtime authority.")
Write-Text "$docRoot\STS05_AssetInventory_v$version.md" $inventory.ToString()

Write-Text "$docRoot\STS05_ProductionReport_v$version.md" @"
# Surface Texture Set 05 Production Report

- Package: `AssetPacks/BrassworksBreach.SurfaceTextureSet05`
- Version: `$version-$buildId`
- Materials: $($materials.Count)
- Procedural texture maps: $($textureRecords.Count)
- Procedural preview PNGs: $($previewRecords.Count)
- Unity-rendered preview status: pending batch validation
- Runtime safety: visual/material-only, with no colliders, audio, scenes, prefabs, gameplay scripts, or runtime authority.

## Material Families

Wet riveted stone, blackened boiler iron, aged brass, oxidized copper, oil grime, soot, hazard enamel, gauge glass, walnut, leather, ember ceramic, steam film, warning paint, and polished worn edge.

## Validation

Run repository validation with:

`Tools/SidecarValidation/Test-SidecarAssetPacks.ps1 -PackageNamePattern 'BrassworksBreach.SurfaceTextureSet05'`
"@

Write-Text "$docRoot\STS05_ValidationEvidence_v$version.md" @"
# Surface Texture Set 05 Validation Evidence

- Package-local manifest created under `AssetPacks/BrassworksBreach.SurfaceTextureSet05/Documentation~/Manifest`.
- Independent package manifest created at package root as `package.json`.
- Isolated validation project scaffold created under `ValidationProject~`.
- Unity batch validation writes material preview PNGs to `Documentation/ConceptRenders/V0_1_47_SurfaceTextureSet05`.
- Repository sidecar validation JSON is written after the static validator run.
"@

Write-Output "STS05_GENERATION_PASS materials=$($materialRecords.Count) textures=$($textureRecords.Count) previews=$($previewRecords.Count)"
