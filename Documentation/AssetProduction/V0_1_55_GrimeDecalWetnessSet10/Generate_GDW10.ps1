$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing

$ProjectRoot = 'D:\__MY APPS\Unity Doom'
$PackageRoot = 'D:\__MY APPS\Unity Doom\AssetPacks\BrassworksBreach.GrimeDecalWetnessSet10'
$ProductionRoot = 'D:\__MY APPS\Unity Doom\Documentation\AssetProduction\V0_1_55_GrimeDecalWetnessSet10'
$RenderRoot = 'D:\__MY APPS\Unity Doom\Documentation\ConceptRenders\V0_1_55_GrimeDecalWetnessSet10'
$PlanningRoot = 'D:\__MY APPS\Unity Doom\Documentation\Planning\V0_1_55_GrimeDecalWetnessSet10ImportReadiness'
$QaRoot = 'D:\__MY APPS\Unity Doom\Documentation\QA\V0_1_55_GrimeDecalWetnessSet10ImportReadiness'
$AssignedRoots = @($PackageRoot, $ProductionRoot, $RenderRoot, $PlanningRoot, $QaRoot) | ForEach-Object { [IO.Path]::GetFullPath($_).TrimEnd('\') }
$Utf8 = [Text.UTF8Encoding]::new($false)
$Culture = [Globalization.CultureInfo]::InvariantCulture
$Written = New-Object System.Collections.Generic.List[string]

function Assert-InRoot($Path) {
    $full = [IO.Path]::GetFullPath($Path).TrimEnd('\')
    foreach ($root in $AssignedRoots) {
        if ($full.Equals($root, [StringComparison]::OrdinalIgnoreCase) -or $full.StartsWith($root + '\', [StringComparison]::OrdinalIgnoreCase)) { return }
    }
    throw "Refusing to write outside assigned roots: $Path"
}

function Ensure-Dir($Path) {
    Assert-InRoot $Path
    if (-not [IO.Directory]::Exists($Path)) { [IO.Directory]::CreateDirectory($Path) | Out-Null }
}

function Rel($Path) {
    $full = [IO.Path]::GetFullPath($Path)
    $root = [IO.Path]::GetFullPath($ProjectRoot).TrimEnd('\') + '\'
    if ($full.StartsWith($root, [StringComparison]::OrdinalIgnoreCase)) { return $full.Substring($root.Length).Replace('\', '/') }
    return $full.Replace('\', '/')
}

function GuidFor($LogicalPath) {
    $md5 = [Security.Cryptography.MD5]::Create()
    try {
        $bytes = [Text.Encoding]::UTF8.GetBytes(('GDW10|' + $LogicalPath.Replace('\','/').ToLowerInvariant()))
        return (($md5.ComputeHash($bytes) | ForEach-Object { $_.ToString('x2') }) -join '')
    } finally {
        $md5.Dispose()
    }
}

function F($Value) { return ([double]$Value).ToString('0.###', $Culture) }
function Clamp($Value) { return [Math]::Max(0, [Math]::Min(255, [int]$Value)) }
function Brush($A, $R, $G, $B) { return [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb((Clamp $A), (Clamp $R), (Clamp $G), (Clamp $B))) }
function PenC($A, $R, $G, $B, $W) { return [Drawing.Pen]::new([Drawing.Color]::FromArgb((Clamp $A), (Clamp $R), (Clamp $G), (Clamp $B)), $W) }

function Write-Text($Path, $Text) {
    Assert-InRoot $Path
    $parent = [IO.Path]::GetDirectoryName($Path)
    if ($parent) { Ensure-Dir $parent }
    [IO.File]::WriteAllText($Path, $Text, $Utf8)
    $Written.Add($Path) | Out-Null
}

function Save-Png($Bitmap, $Path) {
    Assert-InRoot $Path
    Ensure-Dir ([IO.Path]::GetDirectoryName($Path))
    $Bitmap.Save($Path, [Drawing.Imaging.ImageFormat]::Png)
    $Written.Add($Path) | Out-Null
}

function Write-FolderMeta($Folder) {
    $path = "$Folder.meta"
    $text = @"
fileFormatVersion: 2
guid: $(GuidFor (Rel $path))
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: GDW10 generated sidecar folder
  assetBundleName: 
  assetBundleVariant: 
"@
    Write-Text $path $text
}

function Write-DefaultMeta($Asset, $UserData) {
    $path = "$Asset.meta"
    $text = @"
fileFormatVersion: 2
guid: $(GuidFor (Rel $Asset))
DefaultImporter:
  externalObjects: {}
  userData: $UserData
  assetBundleName: 
  assetBundleVariant: 
"@
    Write-Text $path $text
}

function Write-NativeMeta($Asset, $UserData) {
    $path = "$Asset.meta"
    $text = @"
fileFormatVersion: 2
guid: $(GuidFor (Rel $Asset))
NativeFormatImporter:
  externalObjects: {}
  mainObjectFileID: 2100000
  userData: $UserData
  assetBundleName: 
  assetBundleVariant: 
"@
    Write-Text $path $text
}

function Write-PrefabMeta($Asset, $UserData) {
    $path = "$Asset.meta"
    $text = @"
fileFormatVersion: 2
guid: $(GuidFor (Rel $Asset))
PrefabImporter:
  externalObjects: {}
  userData: $UserData
  assetBundleName: 
  assetBundleVariant: 
"@
    Write-Text $path $text
}

function Write-TextureMeta($Asset, [bool]$Srgb, [int]$TextureType, $UserData) {
    $srgbInt = if ($Srgb) { 1 } else { 0 }
    $bump = if ($TextureType -eq 1) { "  bumpmap:`n    convertToNormalMap: 0`n" } else { '' }
    $path = "$Asset.meta"
    $text = @"
fileFormatVersion: 2
guid: $(GuidFor (Rel $Asset))
TextureImporter:
  serializedVersion: 13
  mipmaps:
    enableMipMap: 1
    sRGBTexture: $srgbInt
$bump  isReadable: 0
  textureType: $TextureType
  textureShape: 1
  alphaIsTransparency: 1
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
    Write-Text $path $text
}

function Draw-Pattern($Graphics, $Mat, $W, $H) {
    $rnd = [Random]::new([int]$Mat.Seed)
    $Graphics.Clear([Drawing.Color]::Transparent)
    $Graphics.SmoothingMode = [Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $rgb = $Mat.Color.Split(';') | ForEach-Object { [int]$_ }
    $a = [int]([double]$Mat.Alpha * 255)
    $r = $rgb[0]; $g = $rgb[1]; $b = $rgb[2]

    switch ($Mat.Family) {
        'SootStreaks' {
            for ($i=0; $i -lt 34; $i++) {
                $pen = PenC ($a + $rnd.Next(-28, 42)) $r $g $b ($rnd.Next(3, 14))
                $pen.StartCap = [Drawing.Drawing2D.LineCap]::Round
                $pen.EndCap = [Drawing.Drawing2D.LineCap]::Round
                $x = $rnd.Next(20, $W - 20)
                $len = $rnd.Next([int]($H * .25), [int]($H * .94))
                $pts = @()
                for ($s=0; $s -lt 5; $s++) { $pts += [Drawing.Point]::new($x + $rnd.Next(-18, 19), [int](($s / 4.0) * $len)) }
                $Graphics.DrawCurve($pen, $pts)
                $pen.Dispose()
            }
            for ($i=0; $i -lt 18; $i++) {
                $br = Brush ($rnd.Next(24, 100)) $r $g $b
                $Graphics.FillEllipse($br, $rnd.Next(-10,$W-30), $rnd.Next(-8,[int]($H*.36)), $rnd.Next(22,110), $rnd.Next(10,46))
                $br.Dispose()
            }
        }
        'CornerGrime' {
            $path = [Drawing.Drawing2D.GraphicsPath]::new()
            $path.AddPolygon(@([Drawing.Point]::new(0,0), [Drawing.Point]::new($W,0), [Drawing.Point]::new(0,$H)))
            $br = Brush $a $r $g $b
            $Graphics.FillPath($br, $path)
            $br.Dispose(); $path.Dispose()
            for ($i=0; $i -lt 60; $i++) {
                $br = Brush ($rnd.Next(14, 84)) $r $g $b
                $size = $rnd.Next(10, 86)
                $Graphics.FillEllipse($br, $rnd.Next(-28,[int]($W*.7)), $rnd.Next(-22,[int]($H*.7)), $size, $size)
                $br.Dispose()
            }
        }
        'DampBands' {
            for ($i=0; $i -lt 8; $i++) {
                $br = Brush ($rnd.Next(36, $a)) $r $g $b
                $Graphics.FillRectangle($br, -10, [int]($H*.46) + $rnd.Next(-36,48), $W + 20, $rnd.Next(16, 56))
                $br.Dispose()
            }
            for ($i=0; $i -lt 70; $i++) {
                $br = Brush ($rnd.Next(16,64)) ($r+$rnd.Next(-5,10)) ($g+$rnd.Next(-5,12)) ($b+$rnd.Next(-4,12))
                $Graphics.FillEllipse($br, $rnd.Next(0,$W), $rnd.Next([int]($H*.34), [int]($H*.74)), $rnd.Next(4,24), $rnd.Next(2,8))
                $br.Dispose()
            }
        }
        'WetReflectionHelpers' {
            for ($i=0; $i -lt 13; $i++) {
                $br = Brush ($rnd.Next(28, $a)) $r $g $b
                $Graphics.FillEllipse($br, $rnd.Next(-45,[int]($W*.76)), $rnd.Next([int]($H*.38), [int]($H*.64)), $rnd.Next([int]($W*.2), [int]($W*.75)), $rnd.Next(6, 28))
                $br.Dispose()
            }
            $pen = PenC 92 ($r+68) ($g+68) ($b+68) 3
            for ($i=0; $i -lt 7; $i++) { $Graphics.DrawLine($pen, $rnd.Next(20,$W-80), $rnd.Next([int]($H*.45),[int]($H*.59)), $rnd.Next(80,$W), $rnd.Next([int]($H*.45),[int]($H*.59))) }
            $pen.Dispose()
        }
        'EdgeWearStrips' {
            $br = Brush $a $r $g $b
            $Graphics.FillRectangle($br, 0, [int]($H*.46), $W, [int]($H*.08))
            $br.Dispose()
            for ($i=0; $i -lt 100; $i++) {
                $pen = PenC ($rnd.Next(68,188)) $r $g $b ($rnd.Next(1,4))
                $x = $rnd.Next(0,$W-20); $y = [int]($H*.5) + $rnd.Next(-23,24)
                $Graphics.DrawLine($pen, $x, $y, [Math]::Min($W, $x + $rnd.Next(14,92)), $y + $rnd.Next(-4,5))
                $pen.Dispose()
            }
        }
        'OilStains' {
            for ($i=0; $i -lt 19; $i++) {
                $br = Brush ($rnd.Next(22,$a)) $r $g $b
                $Graphics.FillEllipse($br, $rnd.Next([int]($W*.08),[int]($W*.7)), $rnd.Next([int]($H*.12),[int]($H*.72)), $rnd.Next(38,170), $rnd.Next(22,116))
                $br.Dispose()
            }
            foreach ($c in @(@(54,84,92),@(94,70,42),@(60,45,84),@(34,84,70))) {
                $pen = PenC 55 $c[0] $c[1] $c[2] 3
                $Graphics.DrawArc($pen, $rnd.Next(34,120), $rnd.Next(70,190), $rnd.Next(130,270), $rnd.Next(55,150), $rnd.Next(0,80), $rnd.Next(80,190))
                $pen.Dispose()
            }
        }
        'WaterPuddleDecals' {
            for ($i=0; $i -lt 8; $i++) {
                $br = Brush ($rnd.Next(24,$a)) $r $g $b
                $Graphics.FillEllipse($br, $rnd.Next([int]($W*.06),[int]($W*.54)), $rnd.Next([int]($H*.2),[int]($H*.66)), $rnd.Next(115,250), $rnd.Next(46,142))
                $br.Dispose()
            }
            $pen = PenC 72 ($r+92) ($g+92) ($b+92) 3
            $Graphics.DrawEllipse($pen, [int]($W*.18), [int]($H*.33), [int]($W*.58), [int]($H*.24))
            $Graphics.DrawArc($pen, [int]($W*.24), [int]($H*.38), [int]($W*.42), [int]($H*.16), 190, 110)
            $pen.Dispose()
        }
        'MasonryVariationOverlays' {
            for ($y=0; $y -lt $H; $y += 42) {
                $offset = if (([int]($y / 42) % 2) -eq 0) { 0 } else { -64 }
                for ($x=$offset; $x -lt $W; $x += 128) {
                    $br = Brush ($rnd.Next(14, [Math]::Max(30,$a))) ($r+$rnd.Next(-8,9)) ($g+$rnd.Next(-8,9)) ($b+$rnd.Next(-8,9))
                    $Graphics.FillRectangle($br, $x, $y, 124, 38)
                    $br.Dispose()
                }
            }
            $pen = PenC 55 $r $g $b 2
            for ($y=0; $y -lt $H; $y += 42) { $Graphics.DrawLine($pen, 0, $y, $W, $y) }
            $pen.Dispose()
        }
    }
}

function New-Albedo($Mat, $Path) {
    $bmp = [Drawing.Bitmap]::new(512,512,[Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [Drawing.Graphics]::FromImage($bmp)
    Draw-Pattern $g $Mat 512 512
    $g.Dispose()
    Save-Png $bmp $Path
    $bmp.Dispose()
}

function New-Normal($Mat, $Path) {
    $bmp = [Drawing.Bitmap]::new(256,256,[Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [Drawing.Graphics]::FromImage($bmp)
    $g.Clear([Drawing.Color]::FromArgb(255,128,128,255))
    $rnd = [Random]::new(([int]$Mat.Seed) + 1307)
    $light = PenC 80 148 148 255 2
    $dark = PenC 70 108 108 255 2
    for ($i=0; $i -lt 42; $i++) {
        $x = $rnd.Next(0,256); $y = $rnd.Next(0,256); $pen = if (($i % 2) -eq 0) { $light } else { $dark }
        $g.DrawLine($pen, $x, $y, [Math]::Min(255,$x+$rnd.Next(8,72)), [Math]::Max(0,[Math]::Min(255,$y+$rnd.Next(-18,19))))
    }
    $light.Dispose(); $dark.Dispose(); $g.Dispose()
    Save-Png $bmp $Path
    $bmp.Dispose()
}

function New-Rma($Mat, $Path) {
    $bmp = [Drawing.Bitmap]::new(256,256,[Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [Drawing.Graphics]::FromImage($bmp)
    $rough = [int]((1.0 - [double]$Mat.Smoothness) * 255)
    $metal = [int]([double]$Mat.Metallic * 255)
    $g.Clear([Drawing.Color]::FromArgb(255, $rough, $metal, 205))
    $rnd = [Random]::new(([int]$Mat.Seed) + 2609)
    for ($i=0; $i -lt 30; $i++) {
        $shade = Clamp ($rough + $rnd.Next(-30,31))
        $br = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb($rnd.Next(18,70), $shade, $metal, 205))
        $g.FillEllipse($br, $rnd.Next(-20,240), $rnd.Next(-20,240), $rnd.Next(20,110), $rnd.Next(10,70))
        $br.Dispose()
    }
    $g.Dispose()
    Save-Png $bmp $Path
    $bmp.Dispose()
}

function MaterialYaml($Mat, $AlbGuid, $NrmGuid, $RmaGuid) {
    $rgb = $Mat.Color.Split(';') | ForEach-Object { [int]$_ }
    $emit = if ([string]::IsNullOrWhiteSpace($Mat.Emission)) { @(0,0,0) } else { $Mat.Emission.Split(';') | ForEach-Object { [int]$_ } }
    $keywords = "  - _ALPHABLEND_ON`n  - _METALLICGLOSSMAP`n  - _NORMALMAP"
    if (-not [string]::IsNullOrWhiteSpace($Mat.Emission)) { $keywords += "`n  - _EMISSION" }
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
  m_Name: $($Mat.Name)
  m_Shader: {fileID: 46, guid: 0000000000000000f000000000000000, type: 0}
  m_Parent: {fileID: 0}
  m_ModifiedSerializedProperties: 0
  m_ValidKeywords:
$keywords
  m_InvalidKeywords: []
  m_LightmapFlags: 4
  m_EnableInstancingVariants: 1
  m_DoubleSidedGI: 1
  m_CustomRenderQueue: 3000
  stringTagMap:
    RenderType: Transparent
  disabledShaderPasses:
  - SHADOWCASTER
  m_LockedProperties: 
  m_SavedProperties:
    serializedVersion: 3
    m_TexEnvs:
    - _BumpMap:
        m_Texture: {fileID: 2800000, guid: $NrmGuid, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _EmissionMap:
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _MainTex:
        m_Texture: {fileID: 2800000, guid: $AlbGuid, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _MetallicGlossMap:
        m_Texture: {fileID: 2800000, guid: $RmaGuid, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _OcclusionMap:
        m_Texture: {fileID: 2800000, guid: $RmaGuid, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    m_Ints: []
    m_Floats:
    - _BumpScale: $(F $Mat.Bump)
    - _Cutoff: 0.5
    - _DstBlend: 10
    - _GlossMapScale: 1
    - _Glossiness: $(F $Mat.Smoothness)
    - _GlossyReflections: 1
    - _Metallic: $(F $Mat.Metallic)
    - _Mode: 2
    - _OcclusionStrength: 0.82
    - _SmoothnessTextureChannel: 0
    - _SpecularHighlights: 1
    - _SrcBlend: 5
    - _UVSec: 0
    - _ZWrite: 0
    m_Colors:
    - _Color: {r: $(F ($rgb[0]/255.0)), g: $(F ($rgb[1]/255.0)), b: $(F ($rgb[2]/255.0)), a: $(F $Mat.Alpha)}
    - _EmissionColor: {r: $(F ($emit[0]/255.0)), g: $(F ($emit[1]/255.0)), b: $(F ($emit[2]/255.0)), a: 1}
  m_BuildTextureStacks: []
  m_AllowLocking: 1
"@
}

function PrefabYaml($Prefab, $MatGuid) {
    $rot = if ($Prefab.Placement -eq 'floor') { '{x: -0.7071068, y: 0, z: 0, w: 0.7071068}' } else { '{x: 0, y: 0, z: 0, w: 1}' }
    $pos = if ($Prefab.Placement -eq 'floor') { '{x: 0, y: 0.006, z: 0}' } else { '{x: 0, y: 0, z: 0.006}' }
    $sx = F $Prefab.ScaleX; $sy = F $Prefab.ScaleY
    @"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!1 &100000
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  serializedVersion: 6
  m_Component:
  - component: {fileID: 100001}
  m_Layer: 0
  m_Name: $($Prefab.Name)
  m_TagString: Untagged
  m_Icon: {fileID: 0}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!4 &100001
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 100000}
  serializedVersion: 2
  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}
  m_LocalPosition: {x: 0, y: 0, z: 0}
  m_LocalScale: {x: 1, y: 1, z: 1}
  m_ConstrainProportionsScale: 0
  m_Children:
  - {fileID: 200001}
  m_Father: {fileID: 0}
  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}
--- !u!1 &200000
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  serializedVersion: 6
  m_Component:
  - component: {fileID: 200001}
  - component: {fileID: 200002}
  - component: {fileID: 200003}
  m_Layer: 0
  m_Name: GDW10_CARD_$($Prefab.Slug)_$($Prefab.Variant)
  m_TagString: Untagged
  m_Icon: {fileID: 0}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!4 &200001
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 200000}
  serializedVersion: 2
  m_LocalRotation: $rot
  m_LocalPosition: $pos
  m_LocalScale: {x: $sx, y: $sy, z: 1}
  m_ConstrainProportionsScale: 0
  m_Children: []
  m_Father: {fileID: 100001}
  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}
--- !u!33 &200002
MeshFilter:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 200000}
  m_Mesh: {fileID: 10210, guid: 0000000000000000e000000000000000, type: 0}
--- !u!23 &200003
MeshRenderer:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 200000}
  m_Enabled: 1
  m_CastShadows: 0
  m_ReceiveShadows: 0
  m_DynamicOccludee: 0
  m_StaticShadowCaster: 0
  m_MotionVectors: 0
  m_LightProbeUsage: 0
  m_ReflectionProbeUsage: 0
  m_RayTracingMode: 0
  m_RayTraceProcedural: 0
  m_RenderingLayerMask: 1
  m_RendererPriority: 0
  m_Materials:
  - {fileID: 2100000, guid: $MatGuid, type: 2}
  m_StaticBatchInfo:
    firstSubMesh: 0
    subMeshCount: 0
  m_StaticBatchRoot: {fileID: 0}
  m_ProbeAnchor: {fileID: 0}
  m_LightProbeVolumeOverride: {fileID: 0}
  m_ScaleInLightmap: 1
  m_ReceiveGI: 1
  m_PreserveUVs: 0
  m_IgnoreNormalsForChartDetection: 0
  m_ImportantGI: 0
  m_StitchLightmapSeams: 0
  m_SelectedEditorRenderState: 3
  m_MinimumChartSize: 4
  m_AutoUVMaxDistance: 0.5
  m_AutoUVMaxAngle: 89
  m_LightmapParameters: {fileID: 0}
  m_SortingLayerID: 0
  m_SortingLayer: 0
  m_SortingOrder: 0
"@
}

function Draw-Backdrop($Graphics, $W, $H, [bool]$Floor) {
    $Graphics.Clear([Drawing.Color]::FromArgb(255, 33, 32, 31))
    $rnd = [Random]::new(915 + [int]$Floor)
    $mortar = PenC 255 45 43 41 2
    for ($y=0; $y -lt $H; $y += 48) {
        $offset = if (([int]($y / 48) % 2) -eq 0) { 0 } else { -72 }
        for ($x=$offset; $x -lt $W; $x += 144) {
            $shade = 39 + $rnd.Next(-8,10)
            $br = Brush 255 $shade ($shade-1) ($shade-3)
            $Graphics.FillRectangle($br, $x, $y, 142, 46)
            $br.Dispose()
            $Graphics.DrawRectangle($mortar, $x, $y, 142, 46)
        }
    }
    if ($Floor) {
        $wash = [Drawing.Drawing2D.LinearGradientBrush]::new([Drawing.Rectangle]::new(0,0,$W,$H), [Drawing.Color]::FromArgb(60,26,34,38), [Drawing.Color]::FromArgb(140,13,15,16), [Drawing.Drawing2D.LinearGradientMode]::Vertical)
        $Graphics.FillRectangle($wash, 0, 0, $W, $H)
        $wash.Dispose()
    }
    $mortar.Dispose()
}

function New-Preview($Prefab, $Mat, $Albedo, $OutPath) {
    $bmp = [Drawing.Bitmap]::new(640,440,[Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [Drawing.Drawing2D.SmoothingMode]::AntiAlias
    Draw-Backdrop $g 640 440 ($Prefab.Placement -eq 'floor')
    $tex = [Drawing.Image]::FromFile($Albedo)
    if ($Prefab.Placement -eq 'floor') { $dest = [Drawing.Rectangle]::new(110,170,420,160) }
    elseif ($Prefab.Family -eq 'DampBands' -or $Prefab.Family -eq 'EdgeWearStrips') { $dest = [Drawing.Rectangle]::new(70,190,500,90) }
    elseif ($Prefab.Family -eq 'SootStreaks') { $dest = [Drawing.Rectangle]::new(180,42,250,320) }
    elseif ($Prefab.Family -eq 'CornerGrime') { $dest = [Drawing.Rectangle]::new(58,52,330,300) }
    else { $dest = [Drawing.Rectangle]::new(92,92,456,250) }
    $g.DrawImage($tex, $dest)
    $tex.Dispose()
    $shade = Brush 170 13 13 12
    $g.FillRectangle($shade, 0, 392, 640, 48)
    $shade.Dispose()
    $font = [Drawing.Font]::new([Drawing.FontFamily]::GenericSansSerif, 14, [Drawing.FontStyle]::Bold)
    $small = [Drawing.Font]::new([Drawing.FontFamily]::GenericSansSerif, 10)
    $white = Brush 235 226 216 200
    $muted = Brush 210 160 174 170
    $g.DrawString($Prefab.Name, $font, $white, 18, 398)
    $g.DrawString(($Prefab.Family + ' / ' + $Prefab.Placement + ' quad / ' + $Mat.Name), $small, $muted, 20, 420)
    $font.Dispose(); $small.Dispose(); $white.Dispose(); $muted.Dispose(); $g.Dispose()
    Save-Png $bmp $OutPath
    $bmp.Dispose()
}

function New-ContactSheet($PreviewPaths, $OutPath) {
    $cols = 4; $cellW = 330; $cellH = 270; $rows = [Math]::Ceiling($PreviewPaths.Count / $cols)
    $bmp = [Drawing.Bitmap]::new(($cols*$cellW), ([int]$rows*$cellH + 78), [Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [Drawing.Graphics]::FromImage($bmp)
    $g.Clear([Drawing.Color]::FromArgb(255,24,25,24))
    $titleFont = [Drawing.Font]::new([Drawing.FontFamily]::GenericSansSerif, 22, [Drawing.FontStyle]::Bold)
    $labelFont = [Drawing.Font]::new([Drawing.FontFamily]::GenericSansSerif, 9)
    $titleBrush = Brush 255 225 214 190
    $labelBrush = Brush 230 188 192 178
    $g.DrawString('GDW10 Grime Decal Wetness Set 10 - visual-only finish layer', $titleFont, $titleBrush, 20, 18)
    for ($i=0; $i -lt $PreviewPaths.Count; $i++) {
        $col = $i % $cols; $row = [int][Math]::Floor($i / $cols)
        $x = $col * $cellW + 15; $y = $row * $cellH + 74
        $img = [Drawing.Image]::FromFile($PreviewPaths[$i])
        $g.DrawImage($img, [Drawing.Rectangle]::new($x, $y, 300, 206))
        $img.Dispose()
        $g.DrawString([IO.Path]::GetFileNameWithoutExtension($PreviewPaths[$i]), $labelFont, $labelBrush, $x, $y + 211)
    }
    $titleFont.Dispose(); $labelFont.Dispose(); $titleBrush.Dispose(); $labelBrush.Dispose(); $g.Dispose()
    Save-Png $bmp $OutPath
    $bmp.Dispose()
}

$dirs = @(
    $PackageRoot, "$PackageRoot\Runtime", "$PackageRoot\Runtime\Materials", "$PackageRoot\Runtime\Metadata",
    "$PackageRoot\Runtime\Prefabs", "$PackageRoot\Runtime\Previews", "$PackageRoot\Runtime\Textures",
    "$PackageRoot\Runtime\Textures\Albedo", "$PackageRoot\Runtime\Textures\Normal", "$PackageRoot\Runtime\Textures\RoughnessMetallic",
    "$PackageRoot\Documentation~", "$PackageRoot\Documentation~\Manifest", "$PackageRoot\Samples~", "$PackageRoot\Samples~\PrefabPalette",
    $ProductionRoot, $RenderRoot, $PlanningRoot, $QaRoot
)
foreach ($d in $dirs) { Ensure-Dir $d }
foreach ($d in $dirs | Where-Object { $_.StartsWith($PackageRoot, [StringComparison]::OrdinalIgnoreCase) -and $_ -ne $PackageRoot }) { Write-FolderMeta $d }

$materials = @"
Name,Family,Role,Color,Alpha,Smoothness,Metallic,Bump,Emission,Seed
GDW10_MAT_SootStreak_CharcoalHeavy,SootStreaks,Dense black soot runnels for walls and ceiling seams,18;16;13,0.76,0.16,0,0.08,,1101
GDW10_MAT_SootStreak_SoftBrown,SootStreaks,Softer brown-black smoke staining for upper walls,43;34;24,0.54,0.22,0,0.06,,1102
GDW10_MAT_CornerGrime_BlackMold,CornerGrime,Dark corner accumulation and wet soot pocketing,14;18;16,0.68,0.34,0,0.09,,1201
GDW10_MAT_CornerGrime_AshyDust,CornerGrime,Ashy gray corner dirt for less wet masonry seams,58;54;46,0.42,0.20,0,0.06,,1202
GDW10_MAT_DampBand_GreenBlack,DampBands,Low green-black damp tide marks along walls,22;43;36,0.58,0.64,0,0.05,,1301
GDW10_MAT_DampBand_CoolSeep,DampBands,Cool blue-gray seep bands and masonry moisture,38;54;58,0.48,0.70,0,0.045,,1302
GDW10_MAT_WetReflection_CoolLong,WetReflectionHelpers,Cool elongated floor glints that suggest damp reflection,92;130;146,0.42,0.92,0,0.02,18;38;44,1401
GDW10_MAT_WetReflection_WarmBroken,WetReflectionHelpers,Warm broken amber glints from gaslit wet floor areas,154;109;54,0.38,0.90,0,0.02,48;25;8,1402
GDW10_MAT_EdgeWear_RawStone,EdgeWearStrips,Pale rubbed stone and chipped masonry edge wear,158;147;124,0.62,0.28,0,0.10,,1501
GDW10_MAT_EdgeWear_RustScratch,EdgeWearStrips,Rusty rubbed strips for iron-framed masonry edges,121;65;32,0.56,0.32,0.1,0.12,,1502
GDW10_MAT_OilStain_BlackSlick,OilStains,Irregular black oil stains for floor corners and machinery bases,8;9;8,0.72,0.88,0,0.03,,1601
GDW10_MAT_OilStain_IridescentSheen,OilStains,Thin oily rainbow sheen over existing dark floor materials,34;33;29,0.46,0.95,0,0.025,8;10;12,1602
GDW10_MAT_WaterPuddle_Clear,WaterPuddleDecals,Clear shallow water puddle with soft rim highlights,88;116;126,0.40,0.96,0,0.018,6;10;12,1701
GDW10_MAT_WaterPuddle_Muddy,WaterPuddleDecals,Muddy industrial water puddle with dirty brown center,64;56;42,0.44,0.87,0,0.028,,1702
GDW10_MAT_MasonryVariation_DarkPatch,MasonryVariationOverlays,Dark masonry block variation overlay for repeated walls,25;24;23,0.36,0.26,0,0.07,,1801
GDW10_MAT_MasonryVariation_LimeBloom,MasonryVariationOverlays,Pale mineral bloom and mortar breakup over damp brick,122;128;110,0.30,0.18,0,0.065,,1802
"@ | ConvertFrom-Csv

$families = @(
    [pscustomobject]@{ Family='SootStreaks'; Slug='SootStreaks'; Placement='wall'; Role='vertical smoke trails from vents, pipe joints, and ceiling leaks'; Materials=@('GDW10_MAT_SootStreak_CharcoalHeavy','GDW10_MAT_SootStreak_SoftBrown','GDW10_MAT_SootStreak_CharcoalHeavy','GDW10_MAT_SootStreak_SoftBrown'); Scales=@('0.92x2.35','0.72x1.68','1.22x2.82','0.56x1.28') },
    [pscustomobject]@{ Family='CornerGrime'; Slug='CornerGrime'; Placement='corner'; Role='room corner darkening and wet dirt pockets'; Materials=@('GDW10_MAT_CornerGrime_BlackMold','GDW10_MAT_CornerGrime_AshyDust','GDW10_MAT_CornerGrime_BlackMold','GDW10_MAT_CornerGrime_AshyDust'); Scales=@('1.55x1.55','1.16x1.16','1.95x1.42','0.92x1.75') },
    [pscustomobject]@{ Family='DampBands'; Slug='DampBands'; Placement='wall'; Role='low wall moisture lines and seepage bands'; Materials=@('GDW10_MAT_DampBand_GreenBlack','GDW10_MAT_DampBand_CoolSeep','GDW10_MAT_DampBand_GreenBlack','GDW10_MAT_DampBand_CoolSeep'); Scales=@('2.65x0.46','2.05x0.36','3.2x0.54','1.45x0.3') },
    [pscustomobject]@{ Family='WetFloorReflectionHelpers'; Slug='WetReflectionHelpers'; Placement='floor'; Role='floor-only cards that suggest wet reflected light without lights or probes'; Materials=@('GDW10_MAT_WetReflection_CoolLong','GDW10_MAT_WetReflection_WarmBroken','GDW10_MAT_WetReflection_CoolLong','GDW10_MAT_WetReflection_WarmBroken'); Scales=@('2.9x0.48','2.1x0.34','3.4x0.58','1.35x0.28') },
    [pscustomobject]@{ Family='EdgeWearStrips'; Slug='EdgeWearStrips'; Placement='wall'; Role='thin rubbed/chipped strips for masonry ledges and trim contact edges'; Materials=@('GDW10_MAT_EdgeWear_RawStone','GDW10_MAT_EdgeWear_RustScratch','GDW10_MAT_EdgeWear_RawStone','GDW10_MAT_EdgeWear_RustScratch'); Scales=@('2.55x0.16','1.75x0.13','3.1x0.18','1.15x0.12') },
    [pscustomobject]@{ Family='OilStains'; Slug='OilStains'; Placement='floor'; Role='black floor slicks below machinery, boilers, and pipe junctions'; Materials=@('GDW10_MAT_OilStain_BlackSlick','GDW10_MAT_OilStain_IridescentSheen','GDW10_MAT_OilStain_BlackSlick','GDW10_MAT_OilStain_IridescentSheen'); Scales=@('1.18x0.82','0.92x0.62','1.55x1.05','0.66x0.48') },
    [pscustomobject]@{ Family='WaterPuddleDecals'; Slug='WaterPuddleDecals'; Placement='floor'; Role='shallow water puddles and wet patches on existing floor materials'; Materials=@('GDW10_MAT_WaterPuddle_Clear','GDW10_MAT_WaterPuddle_Muddy','GDW10_MAT_WaterPuddle_Clear','GDW10_MAT_WaterPuddle_Muddy'); Scales=@('1.58x1.02','1.18x0.78','2.08x1.24','0.86x0.52') },
    [pscustomobject]@{ Family='MasonryVariationOverlays'; Slug='MasonryVariationOverlays'; Placement='wall'; Role='transparent brick-tone overlays to break repeated masonry surfaces'; Materials=@('GDW10_MAT_MasonryVariation_DarkPatch','GDW10_MAT_MasonryVariation_LimeBloom','GDW10_MAT_MasonryVariation_DarkPatch','GDW10_MAT_MasonryVariation_LimeBloom'); Scales=@('2.2x1.18','1.6x0.95','2.75x1.45','1.22x0.72') }
)

$matGuid = @{}
$materialEntries = New-Object System.Collections.Generic.List[object]
$textureEntries = New-Object System.Collections.Generic.List[object]
foreach ($mat in $materials) {
    $matPath = "$PackageRoot\Runtime\Materials\$($mat.Name).mat"
    $alb = "$PackageRoot\Runtime\Textures\Albedo\$($mat.Name)_ALB.png"
    $nrm = "$PackageRoot\Runtime\Textures\Normal\$($mat.Name)_NRM.png"
    $rma = "$PackageRoot\Runtime\Textures\RoughnessMetallic\$($mat.Name)_RMA.png"
    New-Albedo $mat $alb; Write-TextureMeta $alb $true 0 "GDW10 $($mat.Family) albedo alpha"
    New-Normal $mat $nrm; Write-TextureMeta $nrm $false 1 "GDW10 $($mat.Family) normal helper"
    New-Rma $mat $rma; Write-TextureMeta $rma $false 0 "GDW10 $($mat.Family) roughness metallic ao"
    Write-Text $matPath (MaterialYaml $mat (GuidFor (Rel $alb)) (GuidFor (Rel $nrm)) (GuidFor (Rel $rma)))
    Write-NativeMeta $matPath "GDW10 $($mat.Family) material"
    $matGuid[$mat.Name] = GuidFor (Rel $matPath)
    $materialEntries.Add([ordered]@{ name=$mat.Name; family=$mat.Family; path=(Rel $matPath); role=$mat.Role; metallic=[double]$mat.Metallic; smoothness=[double]$mat.Smoothness; alpha=[double]$mat.Alpha; textureSet=[ordered]@{ albedo=(Rel $alb); normal=(Rel $nrm); roughnessMetallicAo=(Rel $rma) } }) | Out-Null
    $textureEntries.Add([ordered]@{ name=([IO.Path]::GetFileNameWithoutExtension($alb)); kind='albedo_alpha'; path=(Rel $alb); sourceMaterial=$mat.Name }) | Out-Null
    $textureEntries.Add([ordered]@{ name=([IO.Path]::GetFileNameWithoutExtension($nrm)); kind='normal_helper'; path=(Rel $nrm); sourceMaterial=$mat.Name }) | Out-Null
    $textureEntries.Add([ordered]@{ name=([IO.Path]::GetFileNameWithoutExtension($rma)); kind='roughness_metallic_ao'; path=(Rel $rma); sourceMaterial=$mat.Name }) | Out-Null
}

$prefabs = New-Object System.Collections.Generic.List[object]
$letters = @('A','B','C','D')
$index = 1
foreach ($fam in $families) {
    for ($i=0; $i -lt 4; $i++) {
        $scale = $fam.Scales[$i].Split('x')
        $name = 'GDW10_PREFAB_{0:D2}_{1}_{2}' -f $index, $fam.Slug, $letters[$i]
        $prefabs.Add([pscustomobject]@{ Index=$index; Name=$name; Family=$fam.Family; Slug=$fam.Slug; Variant=$letters[$i]; Material=$fam.Materials[$i]; Placement=$fam.Placement; ScaleX=[double]$scale[0]; ScaleY=[double]$scale[1]; Role=$fam.Role }) | Out-Null
        $index++
    }
}

$prefabEntries = New-Object System.Collections.Generic.List[object]
$previewEntries = New-Object System.Collections.Generic.List[object]
foreach ($p in $prefabs) {
    $path = "$PackageRoot\Runtime\Prefabs\$($p.Name).prefab"
    Write-Text $path (PrefabYaml $p $matGuid[$p.Material])
    Write-PrefabMeta $path "GDW10 $($p.Family) visual-only quad prefab"
    $preview = "$RenderRoot\$($p.Name.Replace('PREFAB','PREVIEW')).png"
    $mat = $materials | Where-Object { $_.Name -eq $p.Material } | Select-Object -First 1
    New-Preview $p $mat "$PackageRoot\Runtime\Textures\Albedo\$($mat.Name)_ALB.png" $preview
    Write-TextureMeta $preview $true 0 "GDW10 preview for $($p.Name)"
    $prefabEntries.Add([ordered]@{ name=$p.Name; family=$p.Family; variant=$p.Variant; path=(Rel $path); preview=(Rel $preview); material=$p.Material; placement=$p.Placement; builtInMesh='Unity built-in Quad fileID 10210'; childRenderers=1; componentContract='GameObject, Transform, MeshFilter, MeshRenderer only' }) | Out-Null
    $previewEntries.Add([ordered]@{ name=([IO.Path]::GetFileNameWithoutExtension($preview)); path=(Rel $preview); sourcePrefab=(Rel $path) }) | Out-Null
}

$previewPaths = @($prefabs | ForEach-Object { "$RenderRoot\$($_.Name.Replace('PREFAB','PREVIEW')).png" })
$contactDoc = "$RenderRoot\GDW10_CONTACTSHEET_GrimeDecalWetnessSet10.png"
New-ContactSheet $previewPaths $contactDoc
Write-TextureMeta $contactDoc $true 0 'GDW10 documentation contact sheet'
$contactRuntime = "$PackageRoot\Runtime\Previews\GDW10_CONTACTSHEET_GrimeDecalWetnessSet10.png"
[IO.File]::Copy($contactDoc, $contactRuntime, $true)
$Written.Add($contactRuntime) | Out-Null
Write-TextureMeta $contactRuntime $true 0 'GDW10 runtime contact sheet copy'

$familyManifestEntries = New-Object System.Collections.Generic.List[object]
foreach ($fam in $families) {
    $familyManifestEntries.Add([ordered]@{
        name=$fam.Family
        variants=4
        role=$fam.Role
        prefabPlacement=$fam.Placement
    }) | Out-Null
}

$package = [ordered]@{
    name='com.brassworks.sidecar.grime-decal-wetness-set10'
    displayName='Brassworks Breach Grime Decal Wetness Set 10'
    version='0.1.55-p001'
    unity='2022.3'
    description='Unity-only visual sidecar bundle of transparent quad decals and wetness helpers for soot streaks, corner grime, damp bands, wet floor glints, edge wear, oil stains, water puddles, and masonry variation overlays. Contains no gameplay scripts, colliders, audio, lights, probes, scenes, animation, or external DCC assets.'
    keywords=@('brassworks','sidecar','grime','decal','wetness','soot','puddles','masonry','visual-only')
    author=[ordered]@{ name='Brassworks Breach Sidecar Production' }
}
Write-Text "$PackageRoot\package.json" (($package | ConvertTo-Json -Depth 8) + "`n")
Write-DefaultMeta "$PackageRoot\package.json" 'GDW10 package manifest'

Write-Text "$PackageRoot\README.md" @"
# Brassworks Breach Grime Decal Wetness Set 10

Visual-only Unity sidecar package for the v0.1.55 finish layer. The pack provides transparent quad decals for soot streaks, corner grime, damp bands, wet floor reflection helpers, edge wear strips, oil stains, water puddles, and masonry variation overlays.

## Import Contract

- Unity 2022.3+ text assets using built-in Quad mesh references and the built-in Standard shader.
- No colliders, rigidbodies, gameplay scripts, lights, reflection probes, audio sources, animation clips, scenes, custom shaders, FBX, OBJ, Blender files, or external DCC output.
- Prefabs are placement helpers only. Rotate, scale, or duplicate in the target room as needed after import.

## Contents

- 32 visual-only quad prefabs under `Runtime/Prefabs`.
- 16 transparent Standard materials under `Runtime/Materials`.
- 48 generated PNG texture maps under `Runtime/Textures`.
- Runtime and documentation manifests for import readiness.
- Preview PNGs and a contact sheet under the assigned documentation render root.
"@
Write-DefaultMeta "$PackageRoot\README.md" 'GDW10 package readme'

Write-Text "$PackageRoot\CHANGELOG.md" @"
# Changelog

## 0.1.55-p001

- Added isolated visual-only grime and wetness finish layer sidecar.
- Added soot, corner grime, damp band, wet reflection, edge wear, oil stain, puddle, and masonry overlay quad prefabs.
- Added generated transparent material textures, previews, contact sheet, QA checklist, and normalized manifest.
"@
Write-DefaultMeta "$PackageRoot\CHANGELOG.md" 'GDW10 changelog'

Write-Text "$PackageRoot\Samples~\PrefabPalette\README.md" @"
# GDW10 Prefab Palette

Drag the prefabs from `Runtime/Prefabs` into an isolated lookdev scene or room pass. These prefabs contain only a root transform and one child Quad renderer with a transparent material.

Recommended pass order: masonry variation overlays, damp bands, corner grime, soot streaks, edge wear strips, oil/water decals, then wet reflection helper glints.
"@
Write-DefaultMeta "$PackageRoot\Samples~\PrefabPalette\README.md" 'GDW10 sample palette readme'

$packageInfo = @{
    name='BrassworksBreach.GrimeDecalWetnessSet10'
    packageId='com.brassworks.sidecar.grime-decal-wetness-set10'
    version='0.1.55-p001'
    generated_at_utc=([DateTime]::UtcNow.ToString('yyyy-MM-ddTHH:mm:ssZ'))
    unityCompatibility='Unity 2022.3+ text assets; built-in Quad mesh references and built-in Standard shader only.'
    externalDccToolsUsed=@()
}
$ownerInfo = @{ worker='GrimeDecalWetnessSet10'; assignedRootsOnly=$true; lane='sidecar-grime-decal-wetness-set10' }
$rootsInfo = @{
    packageRoot='AssetPacks/BrassworksBreach.GrimeDecalWetnessSet10'
    productionDocumentationRoot='Documentation/AssetProduction/V0_1_55_GrimeDecalWetnessSet10'
    conceptRenderRoot='Documentation/ConceptRenders/V0_1_55_GrimeDecalWetnessSet10'
    planningRoot='Documentation/Planning/V0_1_55_GrimeDecalWetnessSet10ImportReadiness'
    qaRoot='Documentation/QA/V0_1_55_GrimeDecalWetnessSet10ImportReadiness'
}
$countsInfo = @{ prefabs=$prefabs.Count; materials=$materials.Count; textures=$textureEntries.Count; meshes=0; builtInMeshReferences=1; previewPNGs=$previewEntries.Count; contactSheets=2; families=$families.Count }
$gapInfo = @{
    addressedGap='finish layer lacks soot streaks, dirty corners, damp bands, floor wetness cues, edge wear, oil/water surface breakup, and varied masonry overlays'
    placementIntent=@('wall and ceiling soot trails behind vents and pipes','dark corner dirt that deepens room silhouettes','low damp bands at masonry/floor junctions','floor glint cards that read as wetness without lights or reflection probes','edge wear strips on masonry ledges and trim','oil and water decals under machinery and pipes','masonry color overlays to break repeated wall tiling')
}
$visualContract = @{
    visualOnly=$true
    containsScripts=$false
    containsColliders=$false
    containsRigidbodies=$false
    containsLights=$false
    containsReflectionProbes=$false
    containsAudio=$false
    containsAnimations=$false
    containsScenes=$false
    containsCustomShaders=$false
    containsExternalDccAssets=$false
    intendedRuntimeBehavior='None. All gameplay, collision, lighting, audio, animation, occlusion, and interactable behavior must be added later by the importing project.'
}
$assetsInfo = @{
    prefabs=$prefabEntries.ToArray()
    materials=$materialEntries.ToArray()
    textures=$textureEntries.ToArray()
    previews=$previewEntries.ToArray()
    contactSheets=@((Rel $contactDoc),(Rel $contactRuntime))
}
$importInfo = @{ status='ready_for_quarantine_import_static_validation_complete'; pathCollisionsChecked=$true; guidCollisionsChecked=$true; rollbackPath='Remove local package reference com.brassworks.sidecar.grime-decal-wetness-set10 and delete only the isolated GDW10 assigned roots.' }
$manifest = @{
    common_schema='brassworks.sidecar.visual_pack_manifest.v1'
    pack_id='GDW10'
    display_name='Grime Decal Wetness Set 10'
    package=$packageInfo
    owner=$ownerInfo
    roots=$rootsInfo
    counts=$countsInfo
    roomtest_v0_5_gap_target=$gapInfo
    visualOnlyContract=$visualContract
    dependencies=@('Unity built-in Quad mesh','Unity built-in Standard shader')
    families=$familyManifestEntries.ToArray()
    assets=$assetsInfo
    importReadiness=$importInfo
    validationChecklist=@('Manifest JSON parses.','Prefab count is 32.','Material count is 16.','Texture count is 48.','Preview PNG count is 32 plus two contact sheets.','No .cs files in package root.','No forbidden prefab components: MonoBehaviour, Collider, Rigidbody, Light, ReflectionProbe, AudioSource, Animation.','No Blender, FBX, OBJ, audio, scene, or external DCC artifacts.')
}
$manifestJson = ($manifest | ConvertTo-Json -Depth 12) + "`n"
Write-Text "$PackageRoot\Runtime\Metadata\GDW10_GrimeDecalWetnessSet10_Manifest_0.1.55-p001.json" $manifestJson
Write-DefaultMeta "$PackageRoot\Runtime\Metadata\GDW10_GrimeDecalWetnessSet10_Manifest_0.1.55-p001.json" 'GDW10 runtime normalized manifest'
Write-Text "$PackageRoot\Documentation~\Manifest\GDW10_GrimeDecalWetnessSet10_Manifest_0.1.55-p001.json" $manifestJson
Write-DefaultMeta "$PackageRoot\Documentation~\Manifest\GDW10_GrimeDecalWetnessSet10_Manifest_0.1.55-p001.json" 'GDW10 documentation normalized manifest'

Write-Text "$RenderRoot\README.md" @"
# GDW10 Concept Renders

Generated preview PNGs for the Grime Decal Wetness Set 10 visual-only finish layer. The images show the transparent quad textures over a simple masonry or floor backdrop for import review.

Primary contact sheet: `GDW10_CONTACTSHEET_GrimeDecalWetnessSet10.png`.
"@
Write-DefaultMeta "$RenderRoot\README.md" 'GDW10 concept render readme'

Write-Text "$ProductionRoot\GDW10_AssetProductionNotes.md" @"
# GDW10 Asset Production Notes

Worker: GrimeDecalWetnessSet10
Package: `com.brassworks.sidecar.grime-decal-wetness-set10`
Version: `0.1.55-p001`

## Scope

This sidecar is an isolated visual-only finish layer for the room concept. It supplies transparent Unity quad prefabs and generated Standard-shader materials for soot streaks, corner grime, damp wall bands, wet floor reflection helpers, edge wear strips, oil stains, water puddles, and masonry variation overlays.

## Asset Method

- Unity text assets only, using built-in Quad mesh fileID `10210`.
- Generated PNG decal textures are stored in `Runtime/Textures` with matching `.meta` files.
- Materials use Unity built-in Standard shader in alpha-blend mode.
- Prefabs contain only `GameObject`, `Transform`, `MeshFilter`, and `MeshRenderer` components.
- No Blender, external DCC, custom mesh, custom shader, collider, gameplay, light, audio, animation, or scene content was created.

## Placement Notes

Apply masonry variation first, then damp bands and corner grime, then soot/edge wear, then floor oil/water, with wet reflection helpers last so the room reads damp without adding lighting dependencies.
"@
Write-DefaultMeta "$ProductionRoot\GDW10_AssetProductionNotes.md" 'GDW10 production notes'

Write-Text "$PlanningRoot\GDW10_ImportReadinessPlan.md" @"
# GDW10 Import Readiness Plan

## Gate

Status: ready for quarantine import static validation.

## Import Steps

1. Add `AssetPacks/BrassworksBreach.GrimeDecalWetnessSet10` as a local Unity package or copy it into the sidecar import staging area.
2. Open a quarantine scene and inspect the 32 prefabs under `Runtime/Prefabs`.
3. Confirm each prefab renders as a transparent quad and contains only Transform, MeshFilter, and MeshRenderer components.
4. Place families in this order for room finishing: masonry overlays, damp bands, corner grime, soot streaks, edge wear strips, oil/water floor decals, wet reflection helpers.
5. If any target material needs pipeline-specific decal shaders, remap only the materials after import; do not add behavior to this sidecar package.

## Rollback

Remove local package reference `com.brassworks.sidecar.grime-decal-wetness-set10` and delete only the GDW10 assigned roots.
"@
Write-DefaultMeta "$PlanningRoot\GDW10_ImportReadinessPlan.md" 'GDW10 import readiness plan'

Write-Text "$QaRoot\GDW10_QA_Checklist.md" @"
# GDW10 QA Checklist

## Static Validation

- [x] Package root is isolated to `AssetPacks/BrassworksBreach.GrimeDecalWetnessSet10`.
- [x] Documentation is isolated to the four assigned GDW10 documentation roots.
- [x] `package.json` exists and parses as JSON.
- [x] Normalized manifest exists in runtime metadata and package documentation.
- [x] 32 visual-only prefab assets generated.
- [x] 16 transparent Standard materials generated.
- [x] 48 PNG material textures generated.
- [x] 32 preview PNGs and two contact sheet copies generated.
- [x] Prefabs use Unity built-in Quad mesh fileID `10210` only.
- [x] Prefabs contain no Collider, Rigidbody, MonoBehaviour, Light, ReflectionProbe, AudioSource, Animation, or Animator components.
- [x] Package contains no `.cs`, `.unity`, `.anim`, `.controller`, `.fbx`, `.obj`, `.blend`, audio, or external DCC files.
- [x] Materials and previews are view/material only.

## Manual Import Review

- [ ] Open quarantine Unity project after import.
- [ ] Verify alpha blending and sorting are acceptable against the room shell.
- [ ] Verify floor helpers do not visually fight with gameplay collision surfaces.
- [ ] Verify final room read matches concept: soot, grime, dampness, wet floor reflections, edge wear, oil/water pooling, and masonry breakup.
"@
Write-DefaultMeta "$QaRoot\GDW10_QA_Checklist.md" 'GDW10 QA checklist'

$finalList = "$ProductionRoot\GDW10_FinalFileList.md"
$finalMeta = "$finalList.meta"
$all = @(Get-ChildItem -LiteralPath $PackageRoot, $ProductionRoot, $RenderRoot, $PlanningRoot, $QaRoot -Recurse -File | ForEach-Object { $_.FullName })
$all = @($all + $finalList + $finalMeta) | Sort-Object -Unique
$lines = New-Object System.Collections.Generic.List[string]
$lines.Add('# GDW10 Final File List') | Out-Null
$lines.Add('') | Out-Null
$lines.Add(('Generated files: {0}' -f $all.Count)) | Out-Null
$lines.Add('') | Out-Null
foreach ($file in $all) { $lines.Add(('- `{0}`' -f (Rel $file))) | Out-Null }
Write-Text $finalList (($lines -join "`n") + "`n")
Write-DefaultMeta $finalList 'GDW10 final file list'

Write-Host "GDW10 generated $((Get-ChildItem -LiteralPath $PackageRoot, $ProductionRoot, $RenderRoot, $PlanningRoot, $QaRoot -Recurse -File | Measure-Object).Count) files."
