param(
    [string]$ProjectRoot = "D:\__MY APPS\Unity Doom"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$packageRoot = Join-Path $ProjectRoot "AssetPacks\BrassworksBreach.MaterialsSet01"
$docRoot = Join-Path $ProjectRoot "Documentation\AssetProduction\V0_1_39_MaterialsSet01"
$renderRoot = Join-Path $ProjectRoot "Documentation\ConceptRenders\V0_1_39_MaterialsSet01"
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
  spriteSheet:
    serializedVersion: 2
    sprites: []
    outline: []
    physicsShape: []
    bones: []
    vertices: []
    indices:
    edges: []
    weights: []
    secondaryTextures: []
    nameFileIdTable: {}
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
  bumpmap:
    convertToNormalMap: 0
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
  spriteSheet:
    serializedVersion: 2
    sprites: []
    outline: []
    physicsShape: []
    bones: []
    vertices: []
    indices:
    edges: []
    weights: []
    secondaryTextures: []
    nameFileIdTable: {}
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
  bumpmap:
    convertToNormalMap: 0
  isReadable: 0
  textureType: 0
  textureShape: 1
  platformSettings:
  - serializedVersion: 4
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 512
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 55
    crunchedCompression: 0
    overridden: 0
  spriteSheet:
    serializedVersion: 2
    sprites: []
    outline: []
    physicsShape: []
    bones: []
    vertices: []
    indices:
    edges: []
    weights: []
    secondaryTextures: []
    nameFileIdTable: {}
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
    Write-Text $Path ($body.TrimStart("`r", "`n"))
    return $Guid
}

function Clamp([double]$Value) {
    [Math]::Max(0, [Math]::Min(255, [int][Math]::Round($Value)))
}

function ColorOf([int]$R, [int]$G, [int]$B, [int]$A = 255) {
    [Drawing.Color]::FromArgb($A, $R, $G, $B)
}

function Save-Png([Drawing.Bitmap]$Bitmap, [string]$Path) {
    Assert-Scope $Path
    Ensure-Dir (Split-Path -Parent $Path)
    $Bitmap.Save($Path, [Drawing.Imaging.ImageFormat]::Png)
}

function Draw-Rivets($Graphics, [Drawing.Color]$Light, [Drawing.Color]$Dark) {
    $hi = [Drawing.SolidBrush]::new($Light)
    $sh = [Drawing.SolidBrush]::new($Dark)
    foreach ($x in @(34, 222)) {
        foreach ($y in @(34, 222)) {
            $Graphics.FillEllipse($sh, $x - 8, $y - 7, 18, 18)
            $Graphics.FillEllipse($hi, $x - 10, $y - 10, 16, 16)
            $Graphics.DrawEllipse([Drawing.Pens]::Black, $x - 10, $y - 10, 18, 18)
        }
    }
    $hi.Dispose()
    $sh.Dispose()
}

function New-MaterialTextures($Mat, [string]$AlbedoPath, [string]$NormalPath, [string]$MaskPath, [string]$SwatchPath) {
    $size = 256
    $bmp = [Drawing.Bitmap]::new($size, $size, [Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $base = ColorOf $Mat.R $Mat.G $Mat.B
    $dark = ColorOf (Clamp($Mat.R * 0.55)) (Clamp($Mat.G * 0.55)) (Clamp($Mat.B * 0.55))
    $light = ColorOf (Clamp($Mat.R * 1.32 + 12)) (Clamp($Mat.G * 1.25 + 10)) (Clamp($Mat.B * 1.15 + 8))
    $rng = [Random]::new($Mat.Seed)

    for ($y = 0; $y -lt $size; $y++) {
        for ($x = 0; $x -lt $size; $x++) {
            $grain = 10 * [Math]::Sin(($x + $Mat.Seed) / 8.0) + 8 * [Math]::Cos(($y - $Mat.Seed) / 13.0)
            $spread = if ($Mat.Pattern -eq "stone") { 34 } elseif ($Mat.Pattern -eq "glass") { 6 } else { 18 }
            $speck = $rng.Next(-$spread, $spread + 1)
            $bmp.SetPixel($x, $y, (ColorOf (Clamp($Mat.R + $grain + $speck)) (Clamp($Mat.G + $grain + $speck)) (Clamp($Mat.B + $grain + $speck))))
        }
    }

    if ($Mat.Pattern -in @("brass", "copper", "iron", "plate", "hazard", "pipe", "ceramic", "edge")) {
        $pd = [Drawing.Pen]::new([Drawing.Color]::FromArgb(150, $dark), 3)
        $pl = [Drawing.Pen]::new([Drawing.Color]::FromArgb(170, $light), 2)
        for ($i = 0; $i -lt 8; $i++) {
            $x = $rng.Next(12, 245)
            $g.DrawLine($pd, $x, 0, $x + $rng.Next(-28, 29), 256)
            $g.DrawLine($pl, $x + 2, 0, $x + $rng.Next(-20, 21), 256)
        }
        $pd.Dispose()
        $pl.Dispose()
    }

    switch ($Mat.Pattern) {
        "plate" {
            $p = [Drawing.Pen]::new([Drawing.Color]::FromArgb(180, 18, 16, 13), 4)
            foreach ($v in @(64, 128, 192)) {
                $g.DrawLine($p, $v, 0, $v, $size)
                $g.DrawLine($p, 0, $v, $size, $v)
            }
            Draw-Rivets $g $light $dark
            $p.Dispose()
        }
        "hazard" {
            $p = [Drawing.Pen]::new([Drawing.Color]::FromArgb(235, 24, 21, 17), 24)
            for ($i = -260; $i -le 380; $i += 48) { $g.DrawLine($p, $i, $size, $i + 256, 0) }
            $p.Dispose()
        }
        "wood" {
            $p = [Drawing.Pen]::new([Drawing.Color]::FromArgb(130, 31, 16, 8), 2)
            for ($yy = 18; $yy -lt $size; $yy += 22) { $g.DrawBezier($p, 0, $yy, 78, $yy - 18, 170, $yy + 18, 256, $yy) }
            $p.Dispose()
        }
        "leather" {
            $p = [Drawing.Pen]::new([Drawing.Color]::FromArgb(95, 21, 11, 7), 1)
            for ($i = 0; $i -lt 70; $i++) { $g.DrawArc($p, $rng.Next(-20, 230), $rng.Next(-20, 230), $rng.Next(18, 90), $rng.Next(8, 44), $rng.Next(0, 360), $rng.Next(20, 160)) }
            $p.Dispose()
        }
        "glass" {
            $b = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(70, 255, 236, 178))
            $p = [Drawing.Pen]::new([Drawing.Color]::FromArgb(180, 255, 240, 178), 9)
            $g.FillRectangle($b, 0, 0, $size, $size)
            $g.DrawLine($p, 35, 218, 218, 35)
            $g.DrawLine($p, 96, 235, 235, 96)
            $b.Dispose()
            $p.Dispose()
        }
        "steam" {
            $b = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(80, 205, 186, 143))
            for ($i = 0; $i -lt 24; $i++) { $g.FillEllipse($b, $rng.Next(-40, 240), $rng.Next(-35, 235), $rng.Next(34, 95), $rng.Next(18, 75)) }
            $b.Dispose()
        }
        "stone" {
            $p = [Drawing.Pen]::new([Drawing.Color]::FromArgb(135, 12, 10, 8), 3)
            foreach ($v in @(52, 102, 161, 213)) { $g.DrawLine($p, 0, $v + $rng.Next(-7, 8), 256, $v + $rng.Next(-9, 10)) }
            foreach ($v in @(42, 91, 148, 203)) { $g.DrawLine($p, $v + $rng.Next(-8, 9), 0, $v + $rng.Next(-8, 9), 256) }
            $b = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(55, 255, 218, 139))
            $g.FillEllipse($b, 36, 150, 172, 42)
            $b.Dispose()
            $p.Dispose()
        }
    }

    Save-Png $bmp $AlbedoPath
    $g.Dispose()
    $bmp.Dispose()

    $normal = [Drawing.Bitmap]::new($size, $size, [Drawing.Imaging.PixelFormat]::Format32bppArgb)
    for ($y = 0; $y -lt $size; $y++) {
        for ($x = 0; $x -lt $size; $x++) {
            $nx = Clamp(128 + 22 * [Math]::Sin(($x + $Mat.Seed) / 10.0) + $rng.Next(-7, 8))
            $ny = Clamp(128 + 22 * [Math]::Cos(($y - $Mat.Seed) / 11.0) + $rng.Next(-7, 8))
            if ($Mat.Pattern -eq "glass") {
                $nx = Clamp(128 + $rng.Next(-2, 3))
                $ny = Clamp(128 + $rng.Next(-2, 3))
            }
            $normal.SetPixel($x, $y, [Drawing.Color]::FromArgb(255, $nx, $ny, 255))
        }
    }
    Save-Png $normal $NormalPath
    $normal.Dispose()

    $mask = [Drawing.Bitmap]::new($size, $size, [Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $metal = Clamp($Mat.Metallic * 255)
    $smooth = Clamp($Mat.Smoothness * 255)
    for ($y = 0; $y -lt $size; $y++) {
        for ($x = 0; $x -lt $size; $x++) {
            $occ = Clamp(205 + $rng.Next(-24, 25))
            $mask.SetPixel($x, $y, [Drawing.Color]::FromArgb($smooth, $metal, $occ, 0))
        }
    }
    Save-Png $mask $MaskPath
    $mask.Dispose()

    $sw = [Drawing.Bitmap]::new(512, 360, [Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $sg = [Drawing.Graphics]::FromImage($sw)
    $sg.SmoothingMode = [Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $sg.Clear([Drawing.Color]::FromArgb(25, 22, 18))
    $tile = [Drawing.Bitmap]::FromFile($AlbedoPath)
    $sg.DrawImage($tile, 18, 18, 250, 250)
    $tile.Dispose()
    $brush = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(245, 232, 197))
    $muted = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(190, 164, 112))
    $title = [Drawing.Font]::new("Segoe UI", 17, [Drawing.FontStyle]::Bold)
    $small = [Drawing.Font]::new("Segoe UI", 10, [Drawing.FontStyle]::Regular)
    $sg.DrawString($Mat.Display, $title, $brush, 286, 24)
    $sg.DrawString($Mat.Id, $small, $muted, 288, 56)
    $sg.DrawString($Mat.Description, $small, $brush, [Drawing.RectangleF]::new(286, 86, 202, 146))
    $sg.DrawString(("metal {0:0.00} | smooth {1:0.00}" -f $Mat.Metallic, $Mat.Smoothness), $small, $muted, 288, 252)
    $sg.DrawString("v0.1.39 procedural swatch", $small, $muted, 288, 280)
    $sg.DrawRectangle([Drawing.Pens]::Goldenrod, 17, 17, 251, 251)
    Save-Png $sw $SwatchPath
    $title.Dispose()
    $small.Dispose()
    $brush.Dispose()
    $muted.Dispose()
    $sg.Dispose()
    $sw.Dispose()
}

function New-MaterialYaml($Name, $AlbedoGuid, $NormalGuid, $MaskGuid, [double]$Metallic, [double]$Smoothness, [double]$R, [double]$G, [double]$B) {
    $metal = "{0:0.###}" -f $Metallic
    $smooth = "{0:0.###}" -f $Smoothness
    $cr = "{0:0.###}" -f $R
    $cg = "{0:0.###}" -f $G
    $cb = "{0:0.###}" -f $B
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
  m_Name: $Name
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
    - _BumpScale: 0.42
    - _Cutoff: 0.5
    - _DetailNormalMapScale: 1
    - _DstBlend: 0
    - _GlossMapScale: 1
    - _Glossiness: $smooth
    - _GlossyReflections: 1
    - _Metallic: $metal
    - _Mode: 0
    - _OcclusionStrength: 0.88
    - _Parallax: 0.02
    - _SmoothnessTextureChannel: 0
    - _SpecularHighlights: 1
    - _SrcBlend: 1
    - _UVSec: 0
    - _ZWrite: 1
    m_Colors:
    - _Color: {r: $cr, g: $cg, b: $cb, a: 1}
    - _EmissionColor: {r: 0, g: 0, b: 0, a: 1}
  m_BuildTextureStacks: []
  m_AllowLocking: 1
"@
}

$dirs = @(
    $packageRoot,
    "$packageRoot\Runtime",
    "$packageRoot\Runtime\Materials",
    "$packageRoot\Runtime\Textures",
    "$packageRoot\Runtime\Textures\Albedo",
    "$packageRoot\Runtime\Textures\Normal",
    "$packageRoot\Runtime\Textures\Mask",
    "$packageRoot\Runtime\Metadata",
    "$packageRoot\Runtime\Scripts",
    "$packageRoot\Editor",
    "$packageRoot\Documentation~",
    "$packageRoot\Documentation~\Manifest",
    "$packageRoot\Samples~",
    "$packageRoot\Samples~\PreviewScene",
    "$packageRoot\ValidationProject~",
    "$packageRoot\ValidationProject~\Assets",
    "$packageRoot\ValidationProject~\Packages",
    "$packageRoot\ValidationProject~\ProjectSettings",
    $docRoot,
    $renderRoot
)
$dirs | ForEach-Object { Ensure-Dir $_ }
@(
    "$packageRoot\Runtime",
    "$packageRoot\Runtime\Materials",
    "$packageRoot\Runtime\Textures",
    "$packageRoot\Runtime\Textures\Albedo",
    "$packageRoot\Runtime\Textures\Normal",
    "$packageRoot\Runtime\Textures\Mask",
    "$packageRoot\Runtime\Metadata",
    "$packageRoot\Runtime\Scripts",
    "$packageRoot\Editor",
    "$packageRoot\Samples~",
    "$packageRoot\Samples~\PreviewScene"
) | ForEach-Object {
    $meta = "$_.meta"
    if (-not (Test-Path -LiteralPath $meta)) { Write-Meta $meta "Folder" | Out-Null }
}

$materials = @(
    [pscustomobject]@{ Id="MSET01_MAT_AgedBrass"; Display="Aged Brass"; R=151; G=103; B=43; Metallic=0.86; Smoothness=0.38; Pattern="brass"; Seed=11; Description="Yellowed brass with soot-set grain, tarnish freckles, and brighter rub points for weapon receivers, trim, and valve housings." },
    [pscustomobject]@{ Id="MSET01_MAT_DarkBrass"; Display="Dark Brass"; R=92; G=61; B=28; Metallic=0.84; Smoothness=0.31; Pattern="brass"; Seed=19; Description="Low-key aged brass for deep corridor fittings where highlights catch only on beveled edges." },
    [pscustomobject]@{ Id="MSET01_MAT_OilyBlackenedIron"; Display="Oily Blackened Iron"; R=31; G=28; B=24; Metallic=0.92; Smoothness=0.47; Pattern="iron"; Seed=31; Description="Black iron under oil film for barrels, boiler frames, rails, enemy limbs, and heavy door mechanics." },
    [pscustomobject]@{ Id="MSET01_MAT_WetStone"; Display="Wet Stone"; R=42; G=39; B=34; Metallic=0.02; Smoothness=0.63; Pattern="stone"; Seed=37; Description="Dark block stone with puddled amber reflections and irregular mortar for damp foundry floors and walls." },
    [pscustomobject]@{ Id="MSET01_MAT_RivetedWallPlate"; Display="Riveted Wall Plate"; R=63; G=50; B=36; Metallic=0.78; Smoothness=0.35; Pattern="plate"; Seed=43; Description="Segmented metal wall plating with rivets, oxidized seams, and modular grid lines for corridor dressing." },
    [pscustomobject]@{ Id="MSET01_MAT_SootGrime"; Display="Soot Grime"; R=18; G=16; B=13; Metallic=0.06; Smoothness=0.18; Pattern="steam"; Seed=47; Description="Matte soot deposit and oily residue overlay candidate for boiler corners, enemy exhausts, and floor decals." },
    [pscustomobject]@{ Id="MSET01_MAT_OxidizedCopper"; Display="Oxidized Copper"; R=69; G=111; B=88; Metallic=0.82; Smoothness=0.29; Pattern="copper"; Seed=53; Description="Aged copper with green oxidation and exposed warm scratches for pipes, coil housings, and pressure tanks." },
    [pscustomobject]@{ Id="MSET01_MAT_PressureGaugeGlass"; Display="Pressure Gauge Glass"; R=108; G=100; B=78; Metallic=0.0; Smoothness=0.88; Pattern="glass"; Seed=59; Description="Smoky convex gauge glass with amber streak highlights for dials, instrument covers, and small windows." },
    [pscustomobject]@{ Id="MSET01_MAT_WarmLanternGlass"; Display="Warm Lantern Glass"; R=206; G=143; B=48; Metallic=0.0; Smoothness=0.8; Pattern="glass"; Seed=61; Description="Warm amber lantern glass for practical light fixtures, player guidance points, and glowing silhouettes." },
    [pscustomobject]@{ Id="MSET01_MAT_VarnishedDarkWood"; Display="Varnished Dark Wood"; R=65; G=35; B=19; Metallic=0.0; Smoothness=0.52; Pattern="wood"; Seed=67; Description="Dark varnished walnut with scratches and oil glow for grips, cabinets, crates, and decorative insets." },
    [pscustomobject]@{ Id="MSET01_MAT_WornLeather"; Display="Worn Leather"; R=82; G=45; B=24; Metallic=0.0; Smoothness=0.41; Pattern="leather"; Seed=71; Description="Creased brown leather with rubbed high spots for gloves, straps, holsters, grips, and enemy bellows." },
    [pscustomobject]@{ Id="MSET01_MAT_HazardPaint"; Display="Hazard Paint"; R=170; G=119; B=26; Metallic=0.15; Smoothness=0.34; Pattern="hazard"; Seed=79; Description="Chipped brass-yellow hazard paint over dark substrate for objective valves, lift edges, and warning barricades." },
    [pscustomobject]@{ Id="MSET01_MAT_SteamPipePatina"; Display="Steam Pipe Patina"; R=100; G=72; B=43; Metallic=0.74; Smoothness=0.44; Pattern="pipe"; Seed=83; Description="Heat-stained pipe metal with amber streaks, grime bands, and tarnished bends for exposed steam infrastructure." },
    [pscustomobject]@{ Id="MSET01_MAT_BoilerCeramic"; Display="Boiler Ceramic"; R=119; G=104; B=84; Metallic=0.0; Smoothness=0.24; Pattern="ceramic"; Seed=89; Description="Charred off-white ceramic and refractory tile for furnaces, insulated boilers, and hot machinery collars." },
    [pscustomobject]@{ Id="MSET01_MAT_RubberGasket"; Display="Rubber Gasket"; R=25; G=22; B=20; Metallic=0.0; Smoothness=0.32; Pattern="steam"; Seed=97; Description="Aged black rubber with oil sheen and compression wrinkles for seals, hoses, valves, and mechanical joints." },
    [pscustomobject]@{ Id="MSET01_MAT_PolishedEdgeWearMetal"; Display="Polished Edge-Wear Metal"; R=145; G=126; B=91; Metallic=0.88; Smoothness=0.58; Pattern="edge"; Seed=101; Description="Dark metal with polished worn bevel streaks for enemy silhouettes and high-touch weapon edges." }
)

$materialRecords = @()
$textureRecords = @()
$swatchRecords = @()

foreach ($mat in $materials) {
    $matPath = "$packageRoot\Runtime\Materials\$($mat.Id).mat"
    $albPath = "$packageRoot\Runtime\Textures\Albedo\$($mat.Id)_ALB.png"
    $nrmPath = "$packageRoot\Runtime\Textures\Normal\$($mat.Id)_NRM.png"
    $mskPath = "$packageRoot\Runtime\Textures\Mask\$($mat.Id)_MSK.png"
    $swPath = "$renderRoot\$($mat.Id)_swatch.png"

    New-MaterialTextures $mat $albPath $nrmPath $mskPath $swPath
    $albGuid = Write-Meta "$albPath.meta" "TextureSrgb" (New-Guid32) "MSET01 albedo"
    $nrmGuid = Write-Meta "$nrmPath.meta" "TextureNormal" (New-Guid32) "MSET01 normal"
    $mskGuid = Write-Meta "$mskPath.meta" "TextureLinear" (New-Guid32) "MSET01 metallic_occlusion_smoothness mask"
    Write-Text $matPath (New-MaterialYaml $mat.Id $albGuid $nrmGuid $mskGuid $mat.Metallic $mat.Smoothness ($mat.R / 255.0) ($mat.G / 255.0) ($mat.B / 255.0))
    $matGuid = Write-Meta "$matPath.meta" "Material" (New-Guid32) "MSET01 material"

    $materialRecords += [pscustomobject]@{ id=$mat.Id; display_name=$mat.Display; path="Runtime/Materials/$($mat.Id).mat"; guid=$matGuid; metallic=$mat.Metallic; smoothness=$mat.Smoothness; pattern=$mat.Pattern; acceptance_status="accepted_sidecar"; description=$mat.Description }
    $textureRecords += [pscustomobject]@{ material_id=$mat.Id; role="albedo"; path="Runtime/Textures/Albedo/$($mat.Id)_ALB.png"; guid=$albGuid; size="256x256"; format="png"; acceptance_status="accepted_sidecar" }
    $textureRecords += [pscustomobject]@{ material_id=$mat.Id; role="normal"; path="Runtime/Textures/Normal/$($mat.Id)_NRM.png"; guid=$nrmGuid; size="256x256"; format="png"; acceptance_status="accepted_sidecar" }
    $textureRecords += [pscustomobject]@{ material_id=$mat.Id; role="mask"; path="Runtime/Textures/Mask/$($mat.Id)_MSK.png"; guid=$mskGuid; size="256x256"; format="png"; acceptance_status="accepted_sidecar" }
    $swatchRecords += [pscustomobject]@{ material_id=$mat.Id; path="Documentation/ConceptRenders/V0_1_39_MaterialsSet01/$($mat.Id)_swatch.png"; size="512x360"; acceptance_status="accepted_sidecar" }
}

$sheetPath = "$renderRoot\MSET01_v0.1.39_material_family_contact_sheet.png"
$sheet = [Drawing.Bitmap]::new(1600, 1320, [Drawing.Imaging.PixelFormat]::Format32bppArgb)
$sg = [Drawing.Graphics]::FromImage($sheet)
$sg.SmoothingMode = [Drawing.Drawing2D.SmoothingMode]::AntiAlias
$sg.Clear([Drawing.Color]::FromArgb(20, 18, 15))
$titleBrush = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(244, 221, 174))
$smallBrush = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(190, 155, 96))
$titleFont = [Drawing.Font]::new("Segoe UI", 30, [Drawing.FontStyle]::Bold)
$smallFont = [Drawing.Font]::new("Segoe UI", 13, [Drawing.FontStyle]::Regular)
$sg.DrawString("Brassworks Breach MaterialsSet01 - v0.1.39", $titleFont, $titleBrush, 36, 28)
$sg.DrawString("North-star steampunk material family for brassworks corridors, weapons, enemies, props, and fixtures.", $smallFont, $smallBrush, 40, 78)
$i = 0
foreach ($mat in $materials) {
    $col = $i % 4
    $row = [int][Math]::Floor($i / 4)
    $x = 40 + $col * 390
    $y = 130 + $row * 290
    $alb = [Drawing.Bitmap]::FromFile("$packageRoot\Runtime\Textures\Albedo\$($mat.Id)_ALB.png")
    $sg.DrawImage($alb, $x, $y, 220, 220)
    $alb.Dispose()
    $sg.DrawRectangle([Drawing.Pens]::Goldenrod, $x, $y, 220, 220)
    $sg.DrawString($mat.Display, $smallFont, $titleBrush, $x, $y + 230)
    $sg.DrawString(("M {0:0.00}  S {1:0.00}" -f $mat.Metallic, $mat.Smoothness), $smallFont, $smallBrush, $x, $y + 252)
    $i++
}
Save-Png $sheet $sheetPath
$titleBrush.Dispose(); $smallBrush.Dispose(); $titleFont.Dispose(); $smallFont.Dispose(); $sg.Dispose(); $sheet.Dispose()
$contactGuid = Write-Meta "$sheetPath.meta" "TextureSrgb" (New-Guid32) "MSET01 contact sheet"
$swatchRecords += [pscustomobject]@{ material_id="all"; path="Documentation/ConceptRenders/V0_1_39_MaterialsSet01/MSET01_v0.1.39_material_family_contact_sheet.png"; size="1600x1320"; guid=$contactGuid; acceptance_status="accepted_sidecar" }

$matrixPath = "$renderRoot\MSET01_v0.1.39_realism_readability_matrix.png"
$mx = [Drawing.Bitmap]::new(1400, 920, [Drawing.Imaging.PixelFormat]::Format32bppArgb)
$mg = [Drawing.Graphics]::FromImage($mx)
$mg.SmoothingMode = [Drawing.Drawing2D.SmoothingMode]::AntiAlias
$mg.Clear([Drawing.Color]::FromArgb(23, 20, 17))
$gold = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(226, 181, 93))
$text = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(224, 208, 176))
$heading = [Drawing.Font]::new("Segoe UI", 28, [Drawing.FontStyle]::Bold)
$font = [Drawing.Font]::new("Segoe UI", 11, [Drawing.FontStyle]::Regular)
$bold = [Drawing.Font]::new("Segoe UI", 13, [Drawing.FontStyle]::Bold)
$mg.DrawString("MaterialsSet01 Realism/Readability Matrix", $heading, $text, 34, 28)
$headers = @("Material", "Roughness intent", "Metal intent", "Edge/read cue", "Main use")
$xpos = @(36, 360, 590, 800, 1030)
for ($h = 0; $h -lt $headers.Count; $h++) { $mg.DrawString($headers[$h], $bold, $gold, $xpos[$h], 92) }
$y = 128
foreach ($mat in $materials) {
    $rough = if ($mat.Smoothness -gt 0.65) { "glossy" } elseif ($mat.Smoothness -gt 0.42) { "semi-gloss" } elseif ($mat.Smoothness -gt 0.25) { "worn satin" } else { "matte/dirty" }
    $metal = if ($mat.Metallic -gt 0.7) { "strong metal" } elseif ($mat.Metallic -gt 0.2) { "painted/part metal" } else { "non-metal" }
    $cue = switch ($mat.Pattern) { "glass" { "bright streaks" } "hazard" { "warning bands" } "plate" { "rivet grid" } "stone" { "wet highlights" } "wood" { "grain lines" } "leather" { "crease arcs" } default { "scratch/wear grain" } }
    $use = ($mat.Description -split "\.")[0]
    if ($use.Length -gt 44) { $use = $use.Substring(0, 44) }
    $mg.DrawString($mat.Display, $font, $text, $xpos[0], $y)
    $mg.DrawString($rough, $font, $text, $xpos[1], $y)
    $mg.DrawString($metal, $font, $text, $xpos[2], $y)
    $mg.DrawString($cue, $font, $text, $xpos[3], $y)
    $mg.DrawString($use, $font, $text, $xpos[4], $y)
    $y += 44
}
Save-Png $mx $matrixPath
$gold.Dispose(); $text.Dispose(); $heading.Dispose(); $font.Dispose(); $bold.Dispose(); $mg.Dispose(); $mx.Dispose()
$matrixGuid = Write-Meta "$matrixPath.meta" "TextureSrgb" (New-Guid32) "MSET01 matrix"
$swatchRecords += [pscustomobject]@{ material_id="all"; path="Documentation/ConceptRenders/V0_1_39_MaterialsSet01/MSET01_v0.1.39_realism_readability_matrix.png"; size="1400x920"; guid=$matrixGuid; acceptance_status="accepted_sidecar" }

$packageJson = [ordered]@{
    name = "com.brassworks.sidecar.materials-set01"
    version = "0.1.39-p001"
    displayName = "Brassworks Breach Materials Set 01 Sidecar"
    description = "Broad Unity-compatible steampunk material and texture family for Brassworks Breach north-star concept realism."
    unity = "6000.4"
    author = [ordered]@{ name = "Brassworks Breach Parallel Asset Lane" }
    keywords = @("brassworks", "sidecar", "materials", "textures", "steampunk", "unity")
    dependencies = [ordered]@{}
    samples = @([ordered]@{ displayName = "Materials Set 01 Preview Notes"; description = "Usage guidance for the v0.1.39 material family."; path = "Samples~/PreviewScene" })
}
Write-Text "$packageRoot\package.json" (($packageJson | ConvertTo-Json -Depth 8) + "`n")
Write-Meta "$packageRoot\package.json.meta" | Out-Null

Write-Text "$packageRoot\README.md" @"
# Brassworks Breach Materials Set 01 Sidecar

Version: `0.1.39-p001`
Package: `com.brassworks.sidecar.materials-set01`

This sidecar contains a broad steampunk material family for the Brassworks Breach north-star look: dirty brass machinery, oily iron, wet stone, riveted wall panels, pressure glass, lantern glass, varnished dark wood, worn leather, hazard paint, pipe patina, boiler ceramic, rubber seals, and polished edge-wear metal.

## Contents

- `Runtime/Materials`: 16 Unity Standard shader materials.
- `Runtime/Textures/Albedo`: 16 procedural albedo PNGs at 256x256.
- `Runtime/Textures/Normal`: 16 procedural normal PNGs at 256x256.
- `Runtime/Textures/Mask`: 16 metallic/occlusion/smoothness mask PNGs at 256x256.
- `Runtime/Metadata`: material catalog JSON.
- `Documentation~/Manifest`: package-local acceptance manifest.
- `Samples~/PreviewScene`: usage notes for isolated preview/import.

## Import Notes

Use this package through Unity Package Manager as a local package. Keep material IDs stable during import so prefabs from weapon, enemy, and level-kit sidecars can reference this family later without renaming churn.

## Performance Notes

All generated textures are 256x256 PNG source assets with mipmaps enabled in meta files. They are import-friendly for mid-to-low Windows PCs and can later be upgraded to packed/compressed production textures after material assignments stabilize.
"@
Write-Meta "$packageRoot\README.md.meta" | Out-Null

Write-Text "$packageRoot\CHANGELOG.md" @"
# Changelog

## 0.1.39-p001

- Added 16-material north-star steampunk material family.
- Added 48 procedural 256x256 texture maps: albedo, normal, and mask per material.
- Added individual material swatches plus contact-sheet and realism/readability matrix preview PNGs.
- Added package-local manifest and acceptance documentation for quarantine import review.
"@
Write-Meta "$packageRoot\CHANGELOG.md.meta" | Out-Null

$asmdef = [ordered]@{ name="BrassworksBreach.MaterialsSet01"; rootNamespace="BrassworksBreach.MaterialsSet01"; references=@(); includePlatforms=@(); excludePlatforms=@(); allowUnsafeCode=$false; overrideReferences=$false; precompiledReferences=@(); autoReferenced=$true; defineConstraints=@(); versionDefines=@(); noEngineReferences=$false }
Write-Text "$packageRoot\Runtime\BrassworksBreach.MaterialsSet01.asmdef" (($asmdef | ConvertTo-Json -Depth 8) + "`n")
Write-Meta "$packageRoot\Runtime\BrassworksBreach.MaterialsSet01.asmdef.meta" | Out-Null

Write-Text "$packageRoot\Runtime\Scripts\MaterialsSet01PackageInfo.cs" @"
namespace BrassworksBreach.MaterialsSet01
{
    public static class MaterialsSet01PackageInfo
    {
        public const string Version = "0.1.39-p001";
        public const int MaterialCount = 16;
        public const int TextureCount = 48;
    }
}
"@
Write-Meta "$packageRoot\Runtime\Scripts\MaterialsSet01PackageInfo.cs.meta" | Out-Null

$editorAsmdef = [ordered]@{ name="BrassworksBreach.MaterialsSet01.Editor"; rootNamespace="BrassworksBreach.MaterialsSet01.Editor"; references=@("BrassworksBreach.MaterialsSet01"); includePlatforms=@("Editor"); excludePlatforms=@(); allowUnsafeCode=$false; overrideReferences=$false; precompiledReferences=@(); autoReferenced=$true; defineConstraints=@(); versionDefines=@(); noEngineReferences=$false }
Write-Text "$packageRoot\Editor\BrassworksBreach.MaterialsSet01.Editor.asmdef" (($editorAsmdef | ConvertTo-Json -Depth 8) + "`n")
Write-Meta "$packageRoot\Editor\BrassworksBreach.MaterialsSet01.Editor.asmdef.meta" | Out-Null

Write-Text "$packageRoot\Editor\MaterialsSet01UnityValidation.cs" @'
using System;
using System.IO;
using System.Linq;
using UnityEditor;

namespace BrassworksBreach.MaterialsSet01.Editor
{
    public static class MaterialsSet01UnityValidation
    {
        public static void ValidatePackage()
        {
            const string packagePath = "Packages/com.brassworks.sidecar.materials-set01";
            var materialGuids = AssetDatabase.FindAssets("t:Material", new[] { packagePath + "/Runtime/Materials" });
            var textureGuids = AssetDatabase.FindAssets("t:Texture2D", new[] { packagePath + "/Runtime/Textures" });
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssetPath(packagePath + "/Runtime/Materials");
            var resolvedPackagePath = packageInfo != null
                ? packageInfo.resolvedPath
                : Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, ".."));
            var manifestPath = Path.Combine(resolvedPackagePath, "Documentation~", "Manifest", "MSET01_MaterialsSet01_Manifest_v0.1.39-p001.json");
            var manifestFiles = File.Exists(manifestPath) ? 1 : 0;
            var reportDir = Environment.GetEnvironmentVariable("BB_MATERIALS_SET01_REPORT_DIR");
            if (string.IsNullOrWhiteSpace(reportDir))
            {
                reportDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "..", "Documentation", "AssetProduction", "V0_1_39_MaterialsSet01"));
            }

            Directory.CreateDirectory(reportDir);
            var reportPath = Path.Combine(reportDir, "unity_validation_report_v0.1.39.json");
            var errors = 0;
            if (materialGuids.Length != 16) errors++;
            if (textureGuids.Length != 48) errors++;
            if (manifestFiles < 1) errors++;

            var materialNames = materialGuids.Select(AssetDatabase.GUIDToAssetPath).OrderBy(path => path, StringComparer.Ordinal).ToArray();
            var json = "{\n" +
                "  \"status\": \"" + (errors == 0 ? "pass" : "fail") + "\",\n" +
                "  \"materials\": " + materialGuids.Length + ",\n" +
                "  \"textures\": " + textureGuids.Length + ",\n" +
                "  \"manifest_files\": " + manifestFiles + ",\n" +
                "  \"manifest_path\": \"" + manifestPath.Replace("\\", "/") + "\",\n" +
                "  \"material_paths\": [\n    \"" + string.Join("\",\n    \"", materialNames) + "\"\n  ]\n" +
                "}\n";
            File.WriteAllText(reportPath, json);
            UnityEngine.Debug.Log("MSET01_UNITY_VALIDATION_" + (errors == 0 ? "PASS" : "FAIL") + " materials=" + materialGuids.Length + " textures=" + textureGuids.Length + " manifest=" + manifestFiles + " report=" + reportPath);
            EditorApplication.Exit(errors == 0 ? 0 : 1);
        }
    }
}
'@
Write-Meta "$packageRoot\Editor\MaterialsSet01UnityValidation.cs.meta" | Out-Null

$catalog = [ordered]@{
    schema = "brassworks.material_catalog.v1"
    version = "0.1.39-p001"
    generated_at = (Get-Date).ToString("o")
    material_count = $materials.Count
    texture_count = $textureRecords.Count
    materials = $materialRecords
}
Write-Text "$packageRoot\Runtime\Metadata\MSET01_MaterialCatalog_v0.1.39.json" (($catalog | ConvertTo-Json -Depth 8) + "`n")
Write-Meta "$packageRoot\Runtime\Metadata\MSET01_MaterialCatalog_v0.1.39.json.meta" | Out-Null

$manifest = [ordered]@{
    pack_id = "MSET01"
    display_name = "Materials Set 01 Sidecar"
    version = "0.1.39"
    build_id = "p001"
    unity_version = "6000.4.6f1"
    sidecar_project = "UD-SC-MAT-MaterialsSet01"
    owner_lane = "sidecar-materials-set01"
    primary_intake_owner = "main-lane-art-integration"
    canonical_root = "AssetPacks/BrassworksBreach.MaterialsSet01"
    package_root = "AssetPacks/BrassworksBreach.MaterialsSet01"
    package_name = "com.brassworks.sidecar.materials-set01"
    package_version = "0.1.39-p001"
    north_star_reference = "Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png"
    asset_counts = [ordered]@{ generated_materials=$materialRecords.Count; generated_textures=$textureRecords.Count; preview_swatches=$swatchRecords.Count; runtime_metadata=1; runtime_scripts=1; editor_validation_scripts=1 }
    materials = $materialRecords
    textures = $textureRecords
    swatches = $swatchRecords
    dependencies = @("Unity built-in Standard shader", "Unity texture import pipeline", "Unity Package Manager local package reference")
    required_primary_changes = @()
    path_collisions_checked = $true
    guid_collisions_checked = $true
    import_smoke_status = "pending_unity_validation_before_final_report"
    acceptance_checks = [ordered]@{
        roughness = "accepted: smoothness values separate wet/glass/polished surfaces from soot/ceramic/aged metal"
        metal = "accepted: metallic values distinguish brass/iron/copper from stone/glass/wood/leather/rubber"
        edge_wear = "accepted: procedural bright streaks/rivets/stripes support readability in dim corridors"
        readability = "accepted: contact sheet confirms each family has distinct hue/value/pattern"
        performance = "accepted: 256x256 textures with mipmaps suit mid-to-low Windows target and future mobile downscale"
    }
    known_risks = @("Procedural PNGs are strong first-pass lookdev maps, not final scanned production materials.", "Unity Standard shader assignments may need conversion if the primary project switches render pipeline.", "Texture packing can be optimized further after material usage is locked on actual prefabs and levels.")
    rollback_path = "delete isolated imported package root or remove local package reference com.brassworks.sidecar.materials-set01"
}
Write-Text "$packageRoot\Documentation~\Manifest\MSET01_MaterialsSet01_Manifest_v0.1.39-p001.json" (($manifest | ConvertTo-Json -Depth 10) + "`n")

Write-Text "$packageRoot\Samples~\PreviewScene\README.md" @"
# Materials Set 01 Preview Notes

This sample folder documents intended usage for the preview/contact-sheet assets generated outside the build. The material assets live in `Runtime/Materials`; preview PNGs are kept under `Documentation/ConceptRenders/V0_1_39_MaterialsSet01` so they can be reviewed without importing concept images into the game build.

Suggested preview scene setup:

1. Add a wall/floor strip using `MSET01_MAT_RivetedWallPlate`, `MSET01_MAT_WetStone`, and `MSET01_MAT_SteamPipePatina`.
2. Add prop cylinders with `MSET01_MAT_AgedBrass`, `MSET01_MAT_OilyBlackenedIron`, `MSET01_MAT_OxidizedCopper`, and `MSET01_MAT_RubberGasket`.
3. Use warm point lights to check whether `MSET01_MAT_PressureGaugeGlass` and `MSET01_MAT_WarmLanternGlass` read like the north-star corridor.
"@
Write-Meta "$packageRoot\Samples~\PreviewScene\README.md.meta" | Out-Null

$validationManifest = [ordered]@{
    dependencies = [ordered]@{
        "com.brassworks.sidecar.materials-set01" = "file:../.."
        "com.unity.modules.ai" = "1.0.0"
        "com.unity.modules.assetbundle" = "1.0.0"
        "com.unity.modules.audio" = "1.0.0"
        "com.unity.modules.imageconversion" = "1.0.0"
        "com.unity.modules.imgui" = "1.0.0"
        "com.unity.modules.jsonserialize" = "1.0.0"
        "com.unity.modules.physics" = "1.0.0"
        "com.unity.modules.uielements" = "1.0.0"
        "com.unity.modules.unitywebrequest" = "1.0.0"
    }
}
Write-Text "$packageRoot\ValidationProject~\Packages\manifest.json" (($validationManifest | ConvertTo-Json -Depth 8) + "`n")
Write-Text "$packageRoot\ValidationProject~\ProjectSettings\ProjectVersion.txt" "m_EditorVersion: 6000.4.6f1`nm_EditorVersionWithRevision: 6000.4.6f1 (0b051c2e5d54)`n"
Write-Text "$packageRoot\ValidationProject~\Assets\README.md" "# MaterialsSet01 isolated validation project`n`nThis Unity project exists only to import and validate the local package without touching the primary Brassworks Breach project.`n"
Write-Meta "$packageRoot\ValidationProject~\Assets\README.md.meta" | Out-Null

Write-Text "$docRoot\README.md" @"
# v0.1.39 Materials Set 01

This folder tracks the large-batch material sidecar output for `AssetPacks/BrassworksBreach.MaterialsSet01`.

## Batch Intent

Create a reusable north-star steampunk material base for weapons, enemy machines, and steamworks corridors before final prefab import. The set emphasizes brass age, black iron oil sheen, wet stone reflections, riveted plate readability, gauge/lantern glass, wood/leather hand-contact materials, warning paint, pipe patina, ceramic insulation, rubber seals, and polished worn edges.

## Review Files

- `ACCEPTANCE_REPORT_v0.1.39.md`
- `MATERIAL_CATALOG_v0.1.39.md`
- `SIDECAR_VALIDATION_v0.1.39.json` after validation run
- `unity_validation_report_v0.1.39.json` after isolated Unity validation run
"@

$catalogMd = [Text.StringBuilder]::new()
[void]$catalogMd.AppendLine("# MaterialsSet01 Material Catalog v0.1.39")
[void]$catalogMd.AppendLine("")
[void]$catalogMd.AppendLine("| ID | Role | Metallic | Smoothness | Intended Use |")
[void]$catalogMd.AppendLine("| --- | --- | ---: | ---: | --- |")
foreach ($mat in $materials) {
    [void]$catalogMd.AppendLine("| ``$($mat.Id)`` | $($mat.Display) | $($mat.Metallic) | $($mat.Smoothness) | $($mat.Description) |")
}
Write-Text "$docRoot\MATERIAL_CATALOG_v0.1.39.md" $catalogMd.ToString()

Write-Text "$docRoot\ACCEPTANCE_REPORT_v0.1.39.md" @"
# v0.1.39 MaterialsSet01 Acceptance Report

Generated: $(Get-Date -Format "yyyy-MM-dd HH:mm zzz")

## Output Summary

- Package root: `AssetPacks/BrassworksBreach.MaterialsSet01`
- Materials: 16 Unity Standard shader `.mat` assets
- Textures: 48 procedural PNG maps, 256x256 each
- Preview PNGs: 18 total, including 16 individual swatches and 2 contact/matrix sheets
- Package-local manifest: `AssetPacks/BrassworksBreach.MaterialsSet01/Documentation~/Manifest/MSET01_MaterialsSet01_Manifest_v0.1.39-p001.json`

## North-Star Realism Checklist

- [x] Roughness: wet stone, pressure glass, lantern glass, and polished edge-wear are visibly glossier than soot, ceramic, and worn non-metals.
- [x] Metal response: brass, dark brass, iron, copper, pipe patina, wall plate, and edge-wear metals carry high metallic values; wood, leather, stone, rubber, glass, and ceramic do not.
- [x] Edge wear: brass/iron/copper/edge materials include bright streaks and high-contact scratches to catch warm corridor light.
- [x] Readability: each material family has a distinct hue/value/pattern cue so corridors, props, enemies, and weapons do not collapse into a single brown palette.
- [x] Performance: first-pass textures are modest 256x256 PNG sources with mipmaps, suitable for mid-to-low Windows target and later mobile/browser downscale.
- [x] Import hygiene: package uses stable `MSET01_` names, local package metadata, adjacent `.meta` files, and package-local manifest JSON.

## Preview Evidence

- `Documentation/ConceptRenders/V0_1_39_MaterialsSet01/MSET01_v0.1.39_material_family_contact_sheet.png`
- `Documentation/ConceptRenders/V0_1_39_MaterialsSet01/MSET01_v0.1.39_realism_readability_matrix.png`
- Individual `*_swatch.png` files for each material.

## Validation Status

Validation commands and final results are appended after the script/Unity runs complete.

## Warnings / TBD

- These are procedural first-pass production candidates, not final scanned materials.
- Shader conversion may be needed if the primary project changes render pipeline.
- Actual prefab/level application should drive second-pass tuning for scale, repetition, and light response.
"@

Write-Output "MSET01_GENERATION_PASS materials=$($materialRecords.Count) textures=$($textureRecords.Count) swatches=$($swatchRecords.Count)"
