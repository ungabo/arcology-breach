$ErrorActionPreference = "Stop"

$RepoRoot = "D:\__MY APPS\Unity Doom"
$PackageRoot = Join-Path $RepoRoot "AssetPacks\BrassworksBreach.RoomMaterialSet10"
$ProductionRoot = Join-Path $RepoRoot "Documentation\AssetProduction\V0_1_55_RoomMaterialSet10"
$ConceptRoot = Join-Path $RepoRoot "Documentation\ConceptRenders\V0_1_55_RoomMaterialSet10"
$PlanningRoot = Join-Path $RepoRoot "Documentation\Planning\V0_1_55_RoomMaterialSet10ImportReadiness"
$QaRoot = Join-Path $RepoRoot "Documentation\QA\V0_1_55_RoomMaterialSet10ImportReadiness"

$PackageName = "com.brassworks.sidecar.room-material-set10"
$PackageDisplayName = "Brassworks Breach Room Material Set 10"
$Version = "0.1.55"
$BuildId = "p001"
$FullVersion = "$Version-$BuildId"
$UnityVersion = "6000.4.6f1"
$PackId = "RMS10"
$GeneratedAtUtc = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")

function Join-Parts {
    param([Parameter(Mandatory = $true)][string[]]$Parts)
    $path = $Parts[0]
    for ($i = 1; $i -lt $Parts.Count; $i++) {
        $path = Join-Path $path $Parts[$i]
    }
    return $path
}

function Assert-InAssignedRoot {
    param([Parameter(Mandatory = $true)][string]$Path)
    $full = [System.IO.Path]::GetFullPath($Path)
    $allowed = @($PackageRoot, $ProductionRoot, $ConceptRoot, $PlanningRoot, $QaRoot) | ForEach-Object {
        [System.IO.Path]::GetFullPath($_).TrimEnd('\')
    }
    foreach ($root in $allowed) {
        if ($full -eq $root -or $full.StartsWith($root + "\", [System.StringComparison]::OrdinalIgnoreCase)) {
            return
        }
    }
    throw "Refusing to write outside assigned roots: $full"
}

function Ensure-Dir {
    param([Parameter(Mandatory = $true)][string]$Path)
    Assert-InAssignedRoot $Path
    if (-not (Test-Path -LiteralPath $Path)) {
        New-Item -ItemType Directory -Force -Path $Path | Out-Null
    }
}

function Write-Utf8NoBom {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [Parameter(Mandatory = $true)][AllowEmptyString()][string]$Text
    )
    Assert-InAssignedRoot $Path
    $dir = Split-Path -Parent $Path
    if ($dir) { Ensure-Dir $dir }
    $encoding = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllText($Path, $Text, $encoding)
}

function Get-DeterministicGuid {
    param([Parameter(Mandatory = $true)][string]$Key)
    $md5 = [System.Security.Cryptography.MD5]::Create()
    try {
        $bytes = [System.Text.Encoding]::UTF8.GetBytes("RMS10|" + $Key.Replace('\', '/').ToLowerInvariant())
        $hash = $md5.ComputeHash($bytes)
        return -join ($hash | ForEach-Object { $_.ToString("x2") })
    }
    finally {
        $md5.Dispose()
    }
}

function Get-RepoRelative {
    param([Parameter(Mandatory = $true)][string]$Path)
    $full = [System.IO.Path]::GetFullPath($Path)
    $root = [System.IO.Path]::GetFullPath($RepoRoot).TrimEnd('\') + "\"
    return $full.Substring($root.Length).Replace('\', '/')
}

function Write-FolderMeta {
    param([Parameter(Mandatory = $true)][string]$FolderPath)
    $metaPath = "$FolderPath.meta"
    Assert-InAssignedRoot $metaPath
    $rel = Get-RepoRelative $FolderPath
    $guid = Get-DeterministicGuid $rel
    $text = @"
fileFormatVersion: 2
guid: $guid
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: 
  assetBundleName:
  assetBundleVariant:
"@
    Write-Utf8NoBom $metaPath $text
}

function Write-DefaultMeta {
    param(
        [Parameter(Mandatory = $true)][string]$AssetPath,
        [string]$UserData = ""
    )
    $metaPath = "$AssetPath.meta"
    Assert-InAssignedRoot $metaPath
    $rel = Get-RepoRelative $AssetPath
    $guid = Get-DeterministicGuid $rel
    $text = @"
fileFormatVersion: 2
guid: $guid
DefaultImporter:
  externalObjects: {}
  userData: $UserData
  assetBundleName:
  assetBundleVariant:
"@
    Write-Utf8NoBom $metaPath $text
}

function Write-NativeMeta {
    param(
        [Parameter(Mandatory = $true)][string]$AssetPath,
        [string]$UserData = "RMS10 material"
    )
    $metaPath = "$AssetPath.meta"
    Assert-InAssignedRoot $metaPath
    $rel = Get-RepoRelative $AssetPath
    $guid = Get-DeterministicGuid $rel
    $text = @"
fileFormatVersion: 2
guid: $guid
NativeFormatImporter:
  externalObjects: {}
  mainObjectFileID: 2100000
  userData: $UserData
  assetBundleName:
  assetBundleVariant:
"@
    Write-Utf8NoBom $metaPath $text
}

function Write-TextureMeta {
    param(
        [Parameter(Mandatory = $true)][string]$AssetPath,
        [Parameter(Mandatory = $true)][ValidateSet("albedo", "normal", "linear", "preview")][string]$Kind,
        [string]$UserData = "RMS10 texture"
    )
    $metaPath = "$AssetPath.meta"
    Assert-InAssignedRoot $metaPath
    $rel = Get-RepoRelative $AssetPath
    $guid = Get-DeterministicGuid $rel
    $srgb = if ($Kind -eq "albedo" -or $Kind -eq "preview") { 1 } else { 0 }
    $textureType = if ($Kind -eq "normal") { 1 } else { 0 }
    $normalBlock = if ($Kind -eq "normal") {
@"
  bumpmap:
    convertToNormalMap: 0
"@
    } else { "" }
    $maxSize = if ($Kind -eq "preview") { 2048 } else { 512 }
    $text = @"
fileFormatVersion: 2
guid: $guid
TextureImporter:
  serializedVersion: 13
  mipmaps:
    enableMipMap: 1
    sRGBTexture: $srgb
$normalBlock  isReadable: 0
  textureType: $textureType
  textureShape: 1
  platformSettings:
  - serializedVersion: 4
    buildTarget: DefaultTexturePlatform
    maxTextureSize: $maxSize
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    overridden: 0
  userData: $UserData
  assetBundleName:
  assetBundleVariant:
"@
    Write-Utf8NoBom $metaPath $text
}

$MaterialSpecs = @(
    [ordered]@{
        Index = 0
        Id = "RMS10_MAT_DarkWetBrickWall"
        DisplayName = "Dark Wet Brick Wall"
        Family = "dark_wet_brick_wall"
        Status = "final_candidate"
        ImportPriority = "high"
        Use = "Primary room side and back walls; smaller continuous brick scale than floor slabs."
        Description = "Oil-dark red-brown brick with black mortar, damp corner absorption, small chipped edges, and restrained wet glints."
        Metallic = 0.00
        Smoothness = 0.58
        BumpScale = 0.82
        Occlusion = 0.94
        Color = @(0.1451, 0.1176, 0.0941, 1.0)
        TilingNote = "8 brick courses by 9 staggered columns per 512 tile; target 0.75m to 1.0m wall tile."
        ValidationNote = "Wall brick scale intentionally smaller than floor flagstones; wetness mask favors lower edges and vertical seams."
        Transparent = $false
    },
    [ordered]@{
        Index = 1
        Id = "RMS10_MAT_SootedBrickCeiling"
        DisplayName = "Sooted Brick Ceiling"
        Family = "sooted_brick_ceiling"
        Status = "final_candidate"
        ImportPriority = "high"
        Use = "Ceiling brick runs and arch returns where soot should read without panel geometry."
        Description = "Charcoal brick ceiling surface with heavier smoke bloom, dark mortar, low wetness, and compressed relief."
        Metallic = 0.00
        Smoothness = 0.32
        BumpScale = 0.62
        Occlusion = 0.98
        Color = @(0.0824, 0.0784, 0.0706, 1.0)
        TilingNote = "10 compressed courses by 8 columns per 512 tile; reads as smaller brick from below."
        ValidationNote = "Ceiling soot mask is broad and low-sheen so it does not become orange under lamps."
        Transparent = $false
    },
    [ordered]@{
        Index = 2
        Id = "RMS10_MAT_WetUnevenFlagstoneFloor"
        DisplayName = "Wet Uneven Flagstone Floor"
        Family = "wet_uneven_flagstone_floor"
        Status = "final_candidate"
        ImportPriority = "high"
        Use = "Primary room floor and damp threshold areas."
        Description = "Large uneven slate flagstones with deep black joints, pooled wetness, worn raised lips, and cool damp variation."
        Metallic = 0.01
        Smoothness = 0.76
        BumpScale = 0.90
        Occlusion = 0.96
        Color = @(0.0980, 0.1098, 0.1059, 1.0)
        TilingNote = "4 by 4 irregular flagstone cells per 512 tile; visibly larger than wall and ceiling brick."
        ValidationNote = "High blue-channel wetness and alpha smoothness are concentrated in low joints and interior puddles."
        Transparent = $false
    },
    [ordered]@{
        Index = 3
        Id = "RMS10_MAT_BlackMortarGrime"
        DisplayName = "Black Mortar Grime"
        Family = "black_mortar_grime"
        Status = "final_candidate"
        ImportPriority = "medium"
        Use = "Mortar fill, corner dirt, drain-adjacent grime, and under-pipe wall patches."
        Description = "Almost black mortar and grime blend with green-brown damp flecks, fine crack relief, and rougher finish."
        Metallic = 0.00
        Smoothness = 0.26
        BumpScale = 0.70
        Occlusion = 1.00
        Color = @(0.0431, 0.0431, 0.0392, 1.0)
        TilingNote = "Non-directional fine material; can be tiled at 0.5m or used as decal backing."
        ValidationNote = "Luminance stays under 0.08 average to preserve dark-corner depth."
        Transparent = $false
    },
    [ordered]@{
        Index = 4
        Id = "RMS10_MAT_EdgeDampnessOverlay"
        DisplayName = "Edge Dampness Overlay"
        Family = "edge_dampness_overlay"
        Status = "candidate_overlay"
        ImportPriority = "medium"
        Use = "Transparent trim/decal overlay for floor-wall seams, base corners, and leak paths."
        Description = "Cool transparent damp edge mask with vertical seep streaks and subtle amber floor glints."
        Metallic = 0.00
        Smoothness = 0.82
        BumpScale = 0.18
        Occlusion = 0.72
        Color = @(0.0314, 0.0471, 0.0471, 0.58)
        TilingNote = "Use as overlay strip or mesh decal; alpha is strongest along borders and bottom edge."
        ValidationNote = "Alpha coverage target is 25 to 50 percent, avoiding a full-screen black wash."
        Transparent = $true
    },
    [ordered]@{
        Index = 5
        Id = "RMS10_MAT_SootDecalOverlay"
        DisplayName = "Soot Decal Overlay"
        Family = "soot_decal_overlay"
        Status = "candidate_overlay"
        ImportPriority = "medium"
        Use = "Transparent soot decals above lamps, ceiling corners, vents, and pipe exits."
        Description = "Soft black-brown soot bloom with smoke wisps, edge feathering, and rough low-sheen finish."
        Metallic = 0.00
        Smoothness = 0.12
        BumpScale = 0.12
        Occlusion = 0.88
        Color = @(0.0275, 0.0235, 0.0196, 0.50)
        TilingNote = "Use as a projected or mesh decal; rotation and scale variation recommended."
        ValidationNote = "Alpha coverage target is 20 to 45 percent with soft falloff and no hard tile border."
        Transparent = $true
    }
)

$csharp = @'
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

public static class Rms10ImageGen
{
    private struct Pattern
    {
        public float Mortar;
        public float Edge;
        public float Wet;
        public float Soot;
        public float Grime;
        public float Height;
        public float Crack;
        public float Alpha;
        public float Cell;
    }

    private static float Clamp01(float v)
    {
        if (v < 0f) return 0f;
        if (v > 1f) return 1f;
        return v;
    }

    private static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    private static float Frac(float v)
    {
        return v - (float)Math.Floor(v);
    }

    private static float SmoothStep(float a, float b, float x)
    {
        if (a == b) return x >= b ? 1f : 0f;
        float t = Clamp01((x - a) / (b - a));
        return t * t * (3f - 2f * t);
    }

    private static float Hash(int x, int y, int seed)
    {
        unchecked
        {
            uint h = 2166136261u;
            h = (h ^ (uint)(x * 374761393)) * 16777619u;
            h = (h ^ (uint)(y * 668265263)) * 16777619u;
            h = (h ^ (uint)(seed * 2246822519u)) * 16777619u;
            h ^= h >> 13;
            h *= 1274126177u;
            h ^= h >> 16;
            return (h & 0x00FFFFFF) / 16777215f;
        }
    }

    private static float ValueNoise(float x, float y, int seed)
    {
        int ix = (int)Math.Floor(x);
        int iy = (int)Math.Floor(y);
        float fx = Frac(x);
        float fy = Frac(y);
        float sx = fx * fx * (3f - 2f * fx);
        float sy = fy * fy * (3f - 2f * fy);
        float a = Hash(ix, iy, seed);
        float b = Hash(ix + 1, iy, seed);
        float c = Hash(ix, iy + 1, seed);
        float d = Hash(ix + 1, iy + 1, seed);
        return Lerp(Lerp(a, b, sx), Lerp(c, d, sx), sy);
    }

    private static float Fbm(float x, float y, int seed)
    {
        float value = 0f;
        float amp = 0.52f;
        float freq = 1f;
        float norm = 0f;
        for (int i = 0; i < 5; i++)
        {
            value += ValueNoise(x * freq, y * freq, seed + i * 17) * amp;
            norm += amp;
            amp *= 0.5f;
            freq *= 2.03f;
        }
        return value / norm;
    }

    private static Pattern EvalPattern(int family, float u, float v)
    {
        Pattern p = new Pattern();
        float nFine = Fbm(u * 34f, v * 34f, family * 53 + 7);
        float nMid = Fbm(u * 9f, v * 9f, family * 71 + 11);
        float nLarge = Fbm(u * 3.5f, v * 3.5f, family * 89 + 13);
        float border = Math.Max(Math.Max(1f - SmoothStep(0.00f, 0.16f, u), SmoothStep(0.84f, 1.00f, u)),
                                Math.Max(1f - SmoothStep(0.00f, 0.16f, v), SmoothStep(0.84f, 1.00f, v)));

        if (family == 0 || family == 1)
        {
            float rows = family == 0 ? 8f : 10f;
            float cols = family == 0 ? 9f : 8f;
            float rowF = v * rows;
            int row = (int)Math.Floor(rowF);
            float ly = Frac(rowF);
            float offset = (row % 2 == 0) ? 0f : 0.5f;
            float colF = u * cols + offset;
            float lx = Frac(colF);
            float mx = 1f - SmoothStep(0.025f, 0.075f, Math.Min(lx, 1f - lx));
            float my = 1f - SmoothStep(0.030f, 0.085f, Math.Min(ly, 1f - ly));
            float chip = SmoothStep(0.77f, 0.96f, Fbm(u * 44f + row, v * 37f, 31 + family * 9));
            float verticalStreak = SmoothStep(0.56f, 0.90f, Fbm(u * 16f, v * 2.2f + 12f, 79 + family));
            p.Mortar = Clamp01(Math.Max(mx, my) + chip * 0.20f);
            p.Edge = Clamp01(Math.Max(mx, my) * 0.78f + chip * 0.36f);
            p.Crack = Clamp01(SmoothStep(0.82f, 0.98f, nFine) * (1f - p.Mortar) + p.Mortar * 0.35f);
            p.Wet = family == 0
                ? Clamp01(SmoothStep(0.48f, 0.82f, nLarge) * 0.48f + verticalStreak * 0.34f + SmoothStep(0.60f, 1.0f, v) * 0.28f + p.Mortar * 0.20f)
                : Clamp01(SmoothStep(0.62f, 0.92f, nLarge) * 0.16f + p.Mortar * 0.08f);
            p.Soot = family == 1
                ? Clamp01(SmoothStep(0.38f, 0.72f, nMid) * 0.80f + SmoothStep(0.0f, 0.22f, v) * 0.25f)
                : Clamp01(SmoothStep(0.68f, 0.92f, nMid) * 0.28f);
            p.Grime = Clamp01(p.Mortar * 0.75f + p.Soot * 0.72f + SmoothStep(0.66f, 0.95f, nMid) * 0.35f);
            float brickRise = (1f - p.Mortar) * (0.58f + nFine * 0.20f + nMid * 0.12f);
            p.Height = Clamp01(brickRise - p.Mortar * 0.35f - p.Crack * 0.13f);
            p.Alpha = 1f;
            p.Cell = Frac(colF) * 0.5f + Frac(rowF) * 0.5f;
            return p;
        }

        if (family == 2)
        {
            float rows = 4f;
            float cols = 4f;
            float cx = u * cols;
            float cy = v * rows;
            float lx = Frac(cx + (ValueNoise((float)Math.Floor(cy), 2f, 901) - 0.5f) * 0.13f);
            float ly = Frac(cy + (ValueNoise((float)Math.Floor(cx), 3f, 902) - 0.5f) * 0.13f);
            float seamX = 1f - SmoothStep(0.035f, 0.110f, Math.Min(lx, 1f - lx));
            float seamY = 1f - SmoothStep(0.035f, 0.110f, Math.Min(ly, 1f - ly));
            float hair = SmoothStep(0.88f, 0.99f, Fbm(u * 26f, v * 26f, 44));
            p.Mortar = Clamp01(Math.Max(seamX, seamY) + hair * 0.18f);
            p.Edge = Clamp01(Math.Max(seamX, seamY) * 0.92f + hair * 0.22f);
            float puddle = SmoothStep(0.50f, 0.86f, Fbm(u * 5.4f + 5f, v * 5.4f, 45));
            p.Wet = Clamp01(puddle * 0.78f + p.Mortar * 0.42f + SmoothStep(0.55f, 1f, v) * 0.18f);
            p.Soot = Clamp01(SmoothStep(0.72f, 0.94f, nMid) * 0.16f);
            p.Grime = Clamp01(p.Mortar * 0.84f + SmoothStep(0.60f, 0.92f, nMid) * 0.32f);
            p.Crack = Clamp01(hair * 0.65f + p.Mortar * 0.45f);
            p.Height = Clamp01((1f - p.Mortar) * (0.53f + nLarge * 0.24f + nFine * 0.12f) - p.Crack * 0.12f);
            p.Alpha = 1f;
            p.Cell = lx * 0.5f + ly * 0.5f;
            return p;
        }

        if (family == 3)
        {
            float thread = SmoothStep(0.70f, 0.95f, Fbm(u * 42f, v * 42f, 313));
            float damp = SmoothStep(0.44f, 0.84f, nMid);
            p.Mortar = Clamp01(0.70f + thread * 0.30f);
            p.Edge = Clamp01(thread * 0.45f + border * 0.20f);
            p.Wet = Clamp01(damp * 0.36f + border * 0.16f);
            p.Soot = Clamp01(SmoothStep(0.62f, 0.90f, nLarge) * 0.40f);
            p.Grime = Clamp01(0.62f + damp * 0.30f + thread * 0.20f);
            p.Crack = thread;
            p.Height = Clamp01(0.28f + nFine * 0.22f - thread * 0.24f);
            p.Alpha = 1f;
            p.Cell = nMid;
            return p;
        }

        if (family == 4)
        {
            float lower = SmoothStep(0.54f, 1f, v);
            float side = Math.Max(1f - SmoothStep(0f, 0.22f, u), SmoothStep(0.78f, 1f, u));
            float streak = SmoothStep(0.50f, 0.91f, Fbm(u * 22f, v * 4f, 514)) * SmoothStep(0.12f, 0.95f, v);
            p.Wet = Clamp01(Math.Max(lower * 0.78f, side * 0.60f) + streak * 0.48f);
            p.Edge = Clamp01(Math.Max(lower, side) * 0.82f + streak * 0.22f);
            p.Mortar = 0f;
            p.Grime = Clamp01(streak * 0.30f + p.Edge * 0.22f);
            p.Soot = 0f;
            p.Crack = streak * 0.24f;
            p.Height = Clamp01(0.45f + p.Wet * 0.15f + nFine * 0.05f);
            p.Alpha = Clamp01(p.Wet * (0.42f + nMid * 0.42f));
            p.Cell = nMid;
            return p;
        }

        {
            float dx = u - 0.52f;
            float dy = v - 0.40f;
            float radius = (float)Math.Sqrt(dx * dx * 1.15f + dy * dy * 0.72f);
            float bloom = 1f - SmoothStep(0.10f, 0.50f, radius);
            float wisp = SmoothStep(0.50f, 0.90f, Fbm(u * 7.0f + 12f, v * 14f, 615));
            float corner = Math.Max(1f - SmoothStep(0f, 0.18f, v), border * 0.25f);
            p.Soot = Clamp01(bloom * 0.86f + wisp * 0.36f + corner * 0.22f);
            p.Grime = Clamp01(p.Soot * 0.75f + SmoothStep(0.68f, 0.95f, nFine) * 0.16f);
            p.Wet = 0f;
            p.Edge = Clamp01(border * 0.20f);
            p.Mortar = 0f;
            p.Crack = SmoothStep(0.88f, 0.99f, nFine) * 0.10f;
            p.Height = Clamp01(0.42f - p.Soot * 0.05f + nFine * 0.03f);
            p.Alpha = Clamp01(p.Soot * (0.34f + nMid * 0.40f));
            p.Cell = nMid;
            return p;
        }
    }

    private static void Put(byte[] data, int index, int r, int g, int b, int a)
    {
        data[index + 0] = (byte)Math.Max(0, Math.Min(255, b));
        data[index + 1] = (byte)Math.Max(0, Math.Min(255, g));
        data[index + 2] = (byte)Math.Max(0, Math.Min(255, r));
        data[index + 3] = (byte)Math.Max(0, Math.Min(255, a));
    }

    private static int Byte(float v)
    {
        return (int)(Clamp01(v) * 255f + 0.5f);
    }

    private static void AlbedoColor(int family, Pattern p, float u, float v, out float r, out float g, out float b, out float a)
    {
        float n = Fbm(u * 22f, v * 22f, family * 111 + 1);
        if (family == 0)
        {
            r = Lerp(0.12f, 0.25f, n);
            g = Lerp(0.085f, 0.16f, n);
            b = Lerp(0.065f, 0.115f, n);
            r -= p.Wet * 0.045f + p.Soot * 0.035f;
            g -= p.Wet * 0.040f + p.Soot * 0.035f;
            b -= p.Soot * 0.030f;
            if (p.Mortar > 0.35f)
            {
                r = Lerp(r, 0.045f, p.Mortar);
                g = Lerp(g, 0.041f, p.Mortar);
                b = Lerp(b, 0.036f, p.Mortar);
            }
            r += p.Edge * 0.018f;
            g += p.Edge * 0.012f;
            b += p.Wet * 0.016f;
            a = 1f;
            return;
        }
        if (family == 1)
        {
            r = Lerp(0.07f, 0.14f, n);
            g = Lerp(0.066f, 0.105f, n);
            b = Lerp(0.058f, 0.085f, n);
            r -= p.Soot * 0.060f;
            g -= p.Soot * 0.055f;
            b -= p.Soot * 0.050f;
            if (p.Mortar > 0.32f)
            {
                r = Lerp(r, 0.030f, p.Mortar);
                g = Lerp(g, 0.029f, p.Mortar);
                b = Lerp(b, 0.026f, p.Mortar);
            }
            a = 1f;
            return;
        }
        if (family == 2)
        {
            r = Lerp(0.075f, 0.18f, n);
            g = Lerp(0.084f, 0.18f, n);
            b = Lerp(0.082f, 0.16f, n);
            r -= p.Wet * 0.030f;
            g -= p.Wet * 0.026f;
            b += p.Wet * 0.012f;
            if (p.Mortar > 0.28f)
            {
                r = Lerp(r, 0.034f, p.Mortar);
                g = Lerp(g, 0.034f, p.Mortar);
                b = Lerp(b, 0.031f, p.Mortar);
            }
            float lip = p.Edge * (1f - p.Mortar);
            r += lip * 0.020f;
            g += lip * 0.022f;
            b += lip * 0.020f;
            a = 1f;
            return;
        }
        if (family == 3)
        {
            r = Lerp(0.022f, 0.070f, n);
            g = Lerp(0.023f, 0.073f, n);
            b = Lerp(0.020f, 0.055f, n);
            g += p.Wet * 0.018f;
            r -= p.Grime * 0.010f;
            b -= p.Soot * 0.008f;
            a = 1f;
            return;
        }
        if (family == 4)
        {
            r = Lerp(0.010f, 0.055f, p.Wet);
            g = Lerp(0.020f, 0.085f, p.Wet);
            b = Lerp(0.022f, 0.080f, p.Wet);
            r += SmoothStep(0.72f, 0.98f, n) * p.Wet * 0.045f;
            g += SmoothStep(0.72f, 0.98f, n) * p.Wet * 0.030f;
            a = p.Alpha;
            return;
        }
        r = Lerp(0.010f, 0.055f, p.Soot);
        g = Lerp(0.009f, 0.045f, p.Soot);
        b = Lerp(0.008f, 0.035f, p.Soot);
        r += p.Soot * 0.018f;
        a = p.Alpha;
    }

    public static void WriteMap(string path, int family, string map, int width, int height)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
        {
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bits = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte[] data = new byte[Math.Abs(bits.Stride) * height];
            float du = 1f / Math.Max(1, width - 1);
            float dv = 1f / Math.Max(1, height - 1);
            float normalStrength = (family == 2) ? 5.2f : (family == 0 ? 4.6f : (family == 1 ? 3.6f : 3.0f));
            for (int y = 0; y < height; y++)
            {
                float v = y / (float)(height - 1);
                for (int x = 0; x < width; x++)
                {
                    float u = x / (float)(width - 1);
                    Pattern p = EvalPattern(family, u, v);
                    int idx = y * bits.Stride + x * 4;
                    if (map == "ALB")
                    {
                        float r, g, b, a;
                        AlbedoColor(family, p, u, v, out r, out g, out b, out a);
                        Put(data, idx, Byte(r), Byte(g), Byte(b), Byte(a));
                    }
                    else if (map == "NRM")
                    {
                        float hL = EvalPattern(family, Math.Max(0f, u - du), v).Height;
                        float hR = EvalPattern(family, Math.Min(1f, u + du), v).Height;
                        float hD = EvalPattern(family, u, Math.Max(0f, v - dv)).Height;
                        float hU = EvalPattern(family, u, Math.Min(1f, v + dv)).Height;
                        float nx = -(hR - hL) * normalStrength;
                        float ny = -(hU - hD) * normalStrength;
                        float nz = 1.0f;
                        float inv = 1.0f / (float)Math.Sqrt(nx * nx + ny * ny + nz * nz);
                        nx *= inv; ny *= inv; nz *= inv;
                        Put(data, idx, Byte(nx * 0.5f + 0.5f), Byte(ny * 0.5f + 0.5f), Byte(nz * 0.5f + 0.5f), 255);
                    }
                    else if (map == "RMA")
                    {
                        float metallic = family == 2 ? 0.01f : 0.0f;
                        float smooth = 0.24f;
                        if (family == 0) smooth = 0.44f + p.Wet * 0.34f - p.Soot * 0.12f;
                        else if (family == 1) smooth = 0.23f + p.Wet * 0.16f - p.Soot * 0.10f;
                        else if (family == 2) smooth = 0.48f + p.Wet * 0.42f - p.Mortar * 0.18f;
                        else if (family == 3) smooth = 0.18f + p.Wet * 0.22f;
                        else if (family == 4) smooth = 0.66f + p.Wet * 0.26f;
                        else smooth = 0.10f + (1f - p.Soot) * 0.06f;
                        float rough = Clamp01(1f - smooth);
                        float ao = Clamp01(1f - p.Mortar * 0.38f - p.Grime * 0.22f - p.Crack * 0.15f);
                        Put(data, idx, Byte(metallic), Byte(rough), Byte(ao), Byte(smooth));
                    }
                    else if (map == "GRM")
                    {
                        Put(data, idx, Byte(p.Edge), Byte(Math.Max(p.Grime, p.Soot)), Byte(p.Wet), Byte(family >= 4 ? p.Alpha : 1f));
                    }
                    else if (map == "HGT")
                    {
                        float h = p.Height;
                        Put(data, idx, Byte(h), Byte(h), Byte(h), Byte(1f));
                    }
                }
            }
            Marshal.Copy(data, 0, bits.Scan0, data.Length);
            bmp.UnlockBits(bits);
            bmp.Save(path, ImageFormat.Png);
        }
    }

    private static Bitmap LoadTile(string path, int w, int h)
    {
        using (Bitmap src = new Bitmap(path))
        {
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(dst))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(src, new Rectangle(0, 0, w, h));
            }
            return dst;
        }
    }

    private static void Label(Graphics g, string text, Rectangle rect, bool dark)
    {
        using (Font font = new Font("Arial", 12f, FontStyle.Bold))
        using (SolidBrush brush = new SolidBrush(dark ? Color.FromArgb(235, 232, 220) : Color.FromArgb(35, 31, 27)))
        using (SolidBrush shadow = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
        {
            Rectangle shadowRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width, rect.Height);
            g.DrawString(text, font, shadow, shadowRect);
            g.DrawString(text, font, brush, rect);
        }
    }

    public static void WriteContactSheet(string outputPath, string packageRoot)
    {
        string[] ids = new string[] {
            "RMS10_MAT_DarkWetBrickWall",
            "RMS10_MAT_SootedBrickCeiling",
            "RMS10_MAT_WetUnevenFlagstoneFloor",
            "RMS10_MAT_BlackMortarGrime",
            "RMS10_MAT_EdgeDampnessOverlay",
            "RMS10_MAT_SootDecalOverlay"
        };
        string[] names = new string[] {
            "Dark wet brick wall",
            "Sooted brick ceiling",
            "Wet uneven flagstone floor",
            "Black mortar grime",
            "Edge dampness overlay",
            "Soot decal overlay"
        };
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
        using (Bitmap sheet = new Bitmap(1536, 1024, PixelFormat.Format32bppArgb))
        using (Graphics g = Graphics.FromImage(sheet))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(18, 17, 15));
            using (Font title = new Font("Arial", 24f, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(232, 222, 196)))
            {
                g.DrawString("RMS10 Room Material Set - final-direction dark wet masonry", title, titleBrush, new PointF(28, 22));
            }
            int tileW = 456;
            int tileH = 392;
            for (int i = 0; i < ids.Length; i++)
            {
                int col = i % 3;
                int row = i / 3;
                int x = 28 + col * 500;
                int y = 84 + row * 448;
                string alb = Path.Combine(packageRoot, "Runtime", "Textures", "Albedo", ids[i] + "_ALB.png");
                using (Bitmap tile = LoadTile(alb, tileW, tileH))
                {
                    g.DrawImage(tile, new Rectangle(x, y, tileW, tileH));
                }
                using (Pen pen = new Pen(Color.FromArgb(185, 128, 104, 72), 2f))
                {
                    g.DrawRectangle(pen, x, y, tileW, tileH);
                }
                Label(g, names[i], new Rectangle(x + 10, y + tileH - 36, tileW - 20, 28), true);
            }
            sheet.Save(outputPath, ImageFormat.Png);
        }
    }

    public static void WriteMapMatrix(string outputPath, string packageRoot)
    {
        string[] ids = new string[] {
            "RMS10_MAT_DarkWetBrickWall",
            "RMS10_MAT_SootedBrickCeiling",
            "RMS10_MAT_WetUnevenFlagstoneFloor",
            "RMS10_MAT_BlackMortarGrime",
            "RMS10_MAT_EdgeDampnessOverlay",
            "RMS10_MAT_SootDecalOverlay"
        };
        string[] maps = new string[] { "ALB", "NRM", "RMA", "GRM", "HGT" };
        string[] folders = new string[] { "Albedo", "Normal", "RoughnessMetallic", "GrimeEdgewear", "Height" };
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
        using (Bitmap sheet = new Bitmap(1600, 1280, PixelFormat.Format32bppArgb))
        using (Graphics g = Graphics.FromImage(sheet))
        {
            g.Clear(Color.FromArgb(17, 16, 15));
            using (Font title = new Font("Arial", 22f, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(232, 222, 196)))
            {
                g.DrawString("RMS10 generated texture map matrix", title, titleBrush, new PointF(28, 18));
            }
            for (int m = 0; m < maps.Length; m++)
            {
                Label(g, maps[m], new Rectangle(205 + m * 270, 58, 240, 28), true);
            }
            for (int i = 0; i < ids.Length; i++)
            {
                Label(g, ids[i].Replace("RMS10_MAT_", ""), new Rectangle(24, 110 + i * 190, 190, 40), true);
                for (int m = 0; m < maps.Length; m++)
                {
                    string path = Path.Combine(packageRoot, "Runtime", "Textures", folders[m], ids[i] + "_" + maps[m] + ".png");
                    using (Bitmap tile = LoadTile(path, 170, 170))
                    {
                        int x = 205 + m * 270;
                        int y = 104 + i * 190;
                        g.DrawImage(tile, new Rectangle(x, y, 170, 170));
                        using (Pen pen = new Pen(Color.FromArgb(120, 180, 150, 105), 1f))
                        {
                            g.DrawRectangle(pen, x, y, 170, 170);
                        }
                    }
                }
            }
            sheet.Save(outputPath, ImageFormat.Png);
        }
    }

    public static void WriteRoomBoard(string outputPath, string packageRoot)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
        string wallPath = Path.Combine(packageRoot, "Runtime", "Textures", "Albedo", "RMS10_MAT_DarkWetBrickWall_ALB.png");
        string ceilPath = Path.Combine(packageRoot, "Runtime", "Textures", "Albedo", "RMS10_MAT_SootedBrickCeiling_ALB.png");
        string floorPath = Path.Combine(packageRoot, "Runtime", "Textures", "Albedo", "RMS10_MAT_WetUnevenFlagstoneFloor_ALB.png");
        string dampPath = Path.Combine(packageRoot, "Runtime", "Textures", "Albedo", "RMS10_MAT_EdgeDampnessOverlay_ALB.png");
        string sootPath = Path.Combine(packageRoot, "Runtime", "Textures", "Albedo", "RMS10_MAT_SootDecalOverlay_ALB.png");
        using (Bitmap board = new Bitmap(1536, 960, PixelFormat.Format32bppArgb))
        using (Graphics g = Graphics.FromImage(board))
        using (Bitmap wall = LoadTile(wallPath, 1024, 560))
        using (Bitmap ceil = LoadTile(ceilPath, 1024, 220))
        using (Bitmap floor = LoadTile(floorPath, 1120, 360))
        using (Bitmap damp = LoadTile(dampPath, 1120, 360))
        using (Bitmap soot = LoadTile(sootPath, 1024, 560))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(10, 9, 8));
            g.DrawImage(ceil, new Rectangle(256, 36, 1024, 220));
            g.DrawImage(wall, new Rectangle(256, 218, 1024, 496));
            using (ImageAttributes sootAttr = new ImageAttributes())
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = 0.55f;
                sootAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(soot, new Rectangle(256, 218, 1024, 496), 0, 0, soot.Width, soot.Height, GraphicsUnit.Pixel, sootAttr);
            }
            Point[] floorPoly = new Point[] {
                new Point(204, 700),
                new Point(1332, 700),
                new Point(1486, 928),
                new Point(50, 928)
            };
            using (TextureBrush brush = new TextureBrush(floor, WrapMode.Tile))
            {
                g.FillPolygon(brush, floorPoly);
            }
            using (ImageAttributes dampAttr = new ImageAttributes())
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = 0.72f;
                dampAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(damp, new Rectangle(50, 650, 1436, 278), 0, 0, damp.Width, damp.Height, GraphicsUnit.Pixel, dampAttr);
            }
            using (SolidBrush shadow = new SolidBrush(Color.FromArgb(155, 0, 0, 0)))
            {
                g.FillRectangle(shadow, 0, 0, 190, 960);
                g.FillRectangle(shadow, 1346, 0, 190, 960);
            }
            using (SolidBrush amber = new SolidBrush(Color.FromArgb(42, 255, 159, 64)))
            {
                g.FillEllipse(amber, 185, 310, 210, 210);
                g.FillEllipse(amber, 1140, 310, 210, 210);
            }
            Label(g, "Roomtest-aligned material board: small dark brick, larger wet flagstone, black mortar, damp edges, soot overlays", new Rectangle(36, 24, 1460, 32), true);
            board.Save(outputPath, ImageFormat.Png);
        }
    }

    public static void WriteOverlaySheet(string outputPath, string packageRoot)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
        string wallPath = Path.Combine(packageRoot, "Runtime", "Textures", "Albedo", "RMS10_MAT_DarkWetBrickWall_ALB.png");
        string dampPath = Path.Combine(packageRoot, "Runtime", "Textures", "Albedo", "RMS10_MAT_EdgeDampnessOverlay_ALB.png");
        string sootPath = Path.Combine(packageRoot, "Runtime", "Textures", "Albedo", "RMS10_MAT_SootDecalOverlay_ALB.png");
        using (Bitmap sheet = new Bitmap(1536, 768, PixelFormat.Format32bppArgb))
        using (Graphics g = Graphics.FromImage(sheet))
        using (Bitmap wall = LoadTile(wallPath, 672, 672))
        using (Bitmap damp = LoadTile(dampPath, 672, 672))
        using (Bitmap soot = LoadTile(sootPath, 672, 672))
        {
            g.Clear(Color.FromArgb(18, 17, 15));
            g.DrawImage(wall, new Rectangle(48, 64, 672, 672));
            g.DrawImage(wall, new Rectangle(816, 64, 672, 672));
            using (ImageAttributes dampAttr = new ImageAttributes())
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = 0.82f;
                dampAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(damp, new Rectangle(48, 64, 672, 672), 0, 0, damp.Width, damp.Height, GraphicsUnit.Pixel, dampAttr);
            }
            using (ImageAttributes sootAttr = new ImageAttributes())
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = 0.78f;
                sootAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(soot, new Rectangle(816, 64, 672, 672), 0, 0, soot.Width, soot.Height, GraphicsUnit.Pixel, sootAttr);
            }
            Label(g, "Edge dampness overlay over wall", new Rectangle(62, 28, 640, 28), true);
            Label(g, "Soot decal overlay over wall", new Rectangle(830, 28, 640, 28), true);
            sheet.Save(outputPath, ImageFormat.Png);
        }
    }
}
'@

Add-Type -TypeDefinition $csharp -ReferencedAssemblies "System.Drawing"

$Dirs = @(
    $PackageRoot,
    (Join-Parts @($PackageRoot, "Runtime")),
    (Join-Parts @($PackageRoot, "Runtime", "Materials")),
    (Join-Parts @($PackageRoot, "Runtime", "Metadata")),
    (Join-Parts @($PackageRoot, "Runtime", "Scripts")),
    (Join-Parts @($PackageRoot, "Runtime", "Textures")),
    (Join-Parts @($PackageRoot, "Runtime", "Textures", "Albedo")),
    (Join-Parts @($PackageRoot, "Runtime", "Textures", "Normal")),
    (Join-Parts @($PackageRoot, "Runtime", "Textures", "RoughnessMetallic")),
    (Join-Parts @($PackageRoot, "Runtime", "Textures", "GrimeEdgewear")),
    (Join-Parts @($PackageRoot, "Runtime", "Textures", "Height")),
    (Join-Parts @($PackageRoot, "Documentation~")),
    (Join-Parts @($PackageRoot, "Documentation~", "Manifest")),
    (Join-Parts @($PackageRoot, "Documentation~", "Previews")),
    (Join-Parts @($PackageRoot, "Samples~")),
    (Join-Parts @($PackageRoot, "Samples~", "MaterialBoard")),
    $ProductionRoot,
    $ConceptRoot,
    $PlanningRoot,
    $QaRoot
)

foreach ($dir in $Dirs) { Ensure-Dir $dir }
foreach ($dir in $Dirs | Where-Object { $_ -ne $PackageRoot -and $_ -ne $ProductionRoot -and $_ -ne $ConceptRoot -and $_ -ne $PlanningRoot -and $_ -ne $QaRoot }) {
    Write-FolderMeta $dir
}

$TextureFolders = [ordered]@{
    ALB = "Albedo"
    NRM = "Normal"
    RMA = "RoughnessMetallic"
    GRM = "GrimeEdgewear"
    HGT = "Height"
}

foreach ($spec in $MaterialSpecs) {
    foreach ($map in $TextureFolders.Keys) {
        $folder = $TextureFolders[$map]
        $texturePath = Join-Parts @($PackageRoot, "Runtime", "Textures", $folder, "$($spec.Id)_$map.png")
        [Rms10ImageGen]::WriteMap($texturePath, [int]$spec.Index, $map, 512, 512)
        $kind = switch ($map) {
            "ALB" { "albedo" }
            "NRM" { "normal" }
            default { "linear" }
        }
        Write-TextureMeta $texturePath $kind "RMS10 $($spec.Family) $map"
    }
}

function Get-AssetGuid {
    param([Parameter(Mandatory = $true)][string]$AssetPath)
    return Get-DeterministicGuid (Get-RepoRelative $AssetPath)
}

function New-MaterialYaml {
    param([Parameter(Mandatory = $true)]$Spec)
    $matId = $Spec.Id
    $alb = Get-AssetGuid (Join-Parts @($PackageRoot, "Runtime", "Textures", "Albedo", "${matId}_ALB.png"))
    $nrm = Get-AssetGuid (Join-Parts @($PackageRoot, "Runtime", "Textures", "Normal", "${matId}_NRM.png"))
    $rma = Get-AssetGuid (Join-Parts @($PackageRoot, "Runtime", "Textures", "RoughnessMetallic", "${matId}_RMA.png"))
    $grm = Get-AssetGuid (Join-Parts @($PackageRoot, "Runtime", "Textures", "GrimeEdgewear", "${matId}_GRM.png"))
    $hgt = Get-AssetGuid (Join-Parts @($PackageRoot, "Runtime", "Textures", "Height", "${matId}_HGT.png"))
    $keywords = if ($Spec.Transparent) {
        "  - _ALPHABLEND_ON`r`n  - _METALLICGLOSSMAP`r`n  - _NORMALMAP`r`n  - _PARALLAXMAP"
    } else {
        "  - _METALLICGLOSSMAP`r`n  - _NORMALMAP`r`n  - _PARALLAXMAP"
    }
    $queue = if ($Spec.Transparent) { 3000 } else { -1 }
    $stringTagMap = if ($Spec.Transparent) {
        "  stringTagMap:`r`n    RenderType: Transparent"
    } else { "  stringTagMap: {}" }
    $disabled = if ($Spec.Transparent) {
        "  disabledShaderPasses:`r`n  - SHADOWCASTER"
    } else { "  disabledShaderPasses: []" }
    $srcBlend = if ($Spec.Transparent) { 5 } else { 1 }
    $dstBlend = if ($Spec.Transparent) { 10 } else { 0 }
    $zWrite = if ($Spec.Transparent) { 0 } else { 1 }
    $mode = if ($Spec.Transparent) { 2 } else { 0 }
    $r = "{0:0.####}" -f [double]$Spec.Color[0]
    $g = "{0:0.####}" -f [double]$Spec.Color[1]
    $b = "{0:0.####}" -f [double]$Spec.Color[2]
    $a = "{0:0.####}" -f [double]$Spec.Color[3]
    $metallic = "{0:0.000}" -f [double]$Spec.Metallic
    $smoothness = "{0:0.000}" -f [double]$Spec.Smoothness
    $bump = "{0:0.000}" -f [double]$Spec.BumpScale
    $occlusion = "{0:0.000}" -f [double]$Spec.Occlusion
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
  m_Name: $matId
  m_Shader: {fileID: 46, guid: 0000000000000000f000000000000000, type: 0}
  m_Parent: {fileID: 0}
  m_ModifiedSerializedProperties: 0
  m_ValidKeywords:
$keywords
  m_InvalidKeywords: []
  m_LightmapFlags: 4
  m_EnableInstancingVariants: 1
  m_DoubleSidedGI: 0
  m_CustomRenderQueue: $queue
$stringTagMap
$disabled
  m_LockedProperties: 
  m_SavedProperties:
    serializedVersion: 3
    m_TexEnvs:
    - _BumpMap:
        m_Texture: {fileID: 2800000, guid: $nrm, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _DetailAlbedoMap:
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _DetailMask:
        m_Texture: {fileID: 2800000, guid: $grm, type: 3}
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
        m_Texture: {fileID: 2800000, guid: $alb, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _MetallicGlossMap:
        m_Texture: {fileID: 2800000, guid: $rma, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _OcclusionMap:
        m_Texture: {fileID: 2800000, guid: $rma, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _ParallaxMap:
        m_Texture: {fileID: 2800000, guid: $hgt, type: 3}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    m_Ints: []
    m_Floats:
    - _BumpScale: $bump
    - _Cutoff: 0.5
    - _DetailNormalMapScale: 1
    - _DstBlend: $dstBlend
    - _GlossMapScale: 1
    - _Glossiness: $smoothness
    - _GlossyReflections: 1
    - _Metallic: $metallic
    - _Mode: $mode
    - _OcclusionStrength: $occlusion
    - _Parallax: 0.025
    - _SmoothnessTextureChannel: 0
    - _SpecularHighlights: 1
    - _SrcBlend: $srcBlend
    - _UVSec: 0
    - _ZWrite: $zWrite
    m_Colors:
    - _Color: {r: $r, g: $g, b: $b, a: $a}
    - _EmissionColor: {r: 0, g: 0, b: 0, a: 1}
  m_BuildTextureStacks: []
  m_AllowLocking: 1
"@
}

foreach ($spec in $MaterialSpecs) {
    $matPath = Join-Parts @($PackageRoot, "Runtime", "Materials", "$($spec.Id).mat")
    Write-Utf8NoBom $matPath (New-MaterialYaml $spec)
    Write-NativeMeta $matPath "RMS10 material $($spec.Family)"
}

$asmdefPath = Join-Parts @($PackageRoot, "Runtime", "BrassworksBreach.RoomMaterialSet10.asmdef")
$asmdef = @"
{
  "name": "BrassworksBreach.RoomMaterialSet10",
  "rootNamespace": "BrassworksBreach.RoomMaterialSet10",
  "references": [],
  "includePlatforms": [],
  "excludePlatforms": [],
  "allowUnsafeCode": false,
  "overrideReferences": false,
  "precompiledReferences": [],
  "autoReferenced": true,
  "defineConstraints": [],
  "versionDefines": [],
  "noEngineReferences": false
}
"@
Write-Utf8NoBom $asmdefPath $asmdef
Write-DefaultMeta $asmdefPath "RMS10 runtime asmdef"

$packageInfoPath = Join-Parts @($PackageRoot, "Runtime", "Scripts", "RoomMaterialSet10PackageInfo.cs")
$packageInfo = @"
namespace BrassworksBreach.RoomMaterialSet10
{
    public static class RoomMaterialSet10PackageInfo
    {
        public const string PackageName = "$PackageName";
        public const string Version = "$FullVersion";
        public const string RuntimeContract = "visual-material-only";
        public const string MaterialDirection = "final-direction dark wet masonry";
    }
}
"@
Write-Utf8NoBom $packageInfoPath $packageInfo
Write-DefaultMeta $packageInfoPath "RMS10 package info"

$packageJsonPath = Join-Parts @($PackageRoot, "package.json")
$packageJsonObj = [ordered]@{
    name = $PackageName
    version = $FullVersion
    displayName = $PackageDisplayName
    description = "Unity-only sidecar package of final-direction dark wet masonry materials inspired by the accepted roomtest/north-star look. Includes dark wet brick wall, sooted brick ceiling, wet uneven flagstone floor, black mortar/grime, edge dampness overlay, and soot/decal overlay families. Visual/material assets only; no gameplay authority."
    unity = "6000.4"
    author = [ordered]@{ name = "Brassworks Breach Sidecar Lane" }
    dependencies = [ordered]@{}
    keywords = @("brassworks", "sidecar", "materials", "room-materials", "wet-masonry", "brick", "flagstone", "soot", "unity-procedural")
    samples = @(
        [ordered]@{
            displayName = "Material Board Notes"
            description = "Review notes for generated room material boards and preview contact sheets."
            path = "Samples~/MaterialBoard"
        }
    )
}
Write-Utf8NoBom $packageJsonPath ($packageJsonObj | ConvertTo-Json -Depth 8)
Write-DefaultMeta $packageJsonPath "RMS10 package json"

$readmePath = Join-Parts @($PackageRoot, "README.md")
$readme = @"
# Brassworks Breach Room Material Set 10

Unity-only visual sidecar package for final-direction dark wet masonry room materials inspired by the accepted roomtest v0.5/north-star look.

## Contents

- 6 Unity Standard `.mat` files.
- 30 generated 512x512 procedural PNG texture maps: albedo/base, normal, roughness/metallic/AO, grime/edge/wetness mask, and height.
- 4 package-local preview PNGs in `Documentation~/Previews`.
- Package-local material catalog and sidecar manifest.
- Import readiness, QA, and concept preview docs under `Documentation`.

## Texture Channels

- `*_ALB.png`: sRGB albedo/base color; overlay families include alpha.
- `*_NRM.png`: linear tangent-space normal detail.
- `*_RMA.png`: linear R=metallic intent, G=roughness intent, B=ambient occlusion/grime intent, A=Unity Standard smoothness.
- `*_GRM.png`: linear R=edge/chip mask, G=grime/soot mask, B=wetness mask, A=overlay alpha where relevant.
- `*_HGT.png`: linear height/parallax intent.

## Contract

Visual/material-only. No meshes, prefabs, colliders, scenes, audio, gameplay scripts, runtime authority, package-manager edits, or build configuration changes.

## Import Notes

- Import through a quarantine Unity project first.
- Use wall and ceiling brick at smaller tiling scale than the flagstone floor.
- Treat `RMS10_MAT_EdgeDampnessOverlay` and `RMS10_MAT_SootDecalOverlay` as overlay candidates until the main project confirms the transparent/decal shader path.
- Rollback is deleting the isolated local package root and its local package reference, if one is added later.
"@
Write-Utf8NoBom $readmePath $readme
Write-DefaultMeta $readmePath "RMS10 readme"

$changelogPath = Join-Parts @($PackageRoot, "CHANGELOG.md")
$changelog = @"
# Changelog

## $FullVersion

- Built isolated sidecar package for final-direction room masonry materials.
- Added 6 Unity Standard material assets covering dark wet brick wall, sooted brick ceiling, wet uneven flagstone floor, black mortar/grime, edge dampness overlay, and soot/decal overlay.
- Added 30 generated procedural texture maps and 4 preview/contact-sheet PNGs.
- Added manifest, catalog, import readiness notes, and QA validation report.
"@
Write-Utf8NoBom $changelogPath $changelog
Write-DefaultMeta $changelogPath "RMS10 changelog"

$sampleReadmePath = Join-Parts @($PackageRoot, "Samples~", "MaterialBoard", "README.md")
$sampleReadme = @'
# RMS10 Material Board Notes

The package-local previews in `Documentation~/Previews` are review images for comparing texture families before main-lane import.

Recommended first-pass bindings:

- Wall/back wall: `RMS10_MAT_DarkWetBrickWall`
- Ceiling: `RMS10_MAT_SootedBrickCeiling`
- Floor: `RMS10_MAT_WetUnevenFlagstoneFloor`
- Seams/corners: `RMS10_MAT_BlackMortarGrime` plus `RMS10_MAT_EdgeDampnessOverlay`
- Lamp/vent smoke: `RMS10_MAT_SootDecalOverlay`
'@
Write-Utf8NoBom $sampleReadmePath $sampleReadme
Write-DefaultMeta $sampleReadmePath "RMS10 sample notes"

$previewFiles = @(
    "RMS10_PREVIEW_01_room_corner_material_board.png",
    "RMS10_PREVIEW_02_material_family_contact_sheet.png",
    "RMS10_PREVIEW_03_texture_map_matrix.png",
    "RMS10_PREVIEW_04_overlay_application_sheet.png"
)

$packagePreviewDir = Join-Parts @($PackageRoot, "Documentation~", "Previews")
$conceptPreviewPaths = @{}
$packagePreviewPaths = @{}

$conceptPreviewPaths[$previewFiles[0]] = Join-Parts @($ConceptRoot, $previewFiles[0])
$conceptPreviewPaths[$previewFiles[1]] = Join-Parts @($ConceptRoot, $previewFiles[1])
$conceptPreviewPaths[$previewFiles[2]] = Join-Parts @($ConceptRoot, $previewFiles[2])
$conceptPreviewPaths[$previewFiles[3]] = Join-Parts @($ConceptRoot, $previewFiles[3])
$packagePreviewPaths[$previewFiles[0]] = Join-Parts @($packagePreviewDir, $previewFiles[0])
$packagePreviewPaths[$previewFiles[1]] = Join-Parts @($packagePreviewDir, $previewFiles[1])
$packagePreviewPaths[$previewFiles[2]] = Join-Parts @($packagePreviewDir, $previewFiles[2])
$packagePreviewPaths[$previewFiles[3]] = Join-Parts @($packagePreviewDir, $previewFiles[3])

[Rms10ImageGen]::WriteRoomBoard($conceptPreviewPaths[$previewFiles[0]], $PackageRoot)
[Rms10ImageGen]::WriteContactSheet($conceptPreviewPaths[$previewFiles[1]], $PackageRoot)
[Rms10ImageGen]::WriteMapMatrix($conceptPreviewPaths[$previewFiles[2]], $PackageRoot)
[Rms10ImageGen]::WriteOverlaySheet($conceptPreviewPaths[$previewFiles[3]], $PackageRoot)

foreach ($fileName in $previewFiles) {
    Copy-Item -LiteralPath $conceptPreviewPaths[$fileName] -Destination $packagePreviewPaths[$fileName] -Force
    Write-TextureMeta $conceptPreviewPaths[$fileName] "preview" "RMS10 concept preview"
    Write-TextureMeta $packagePreviewPaths[$fileName] "preview" "RMS10 package preview"
}

function Get-PngStats {
    param([Parameter(Mandatory = $true)][string]$Path)
    $bmp = [System.Drawing.Bitmap]::new($Path)
    try {
        $sumLuma = 0.0
        $sumAlpha = 0.0
        $coverage = 0
        $samples = 0
        for ($y = 0; $y -lt $bmp.Height; $y += 8) {
            for ($x = 0; $x -lt $bmp.Width; $x += 8) {
                $c = $bmp.GetPixel($x, $y)
                $sumLuma += (($c.R * 0.2126) + ($c.G * 0.7152) + ($c.B * 0.0722)) / 255.0
                $sumAlpha += $c.A / 255.0
                if ($c.A -gt 32) { $coverage++ }
                $samples++
            }
        }
        return [ordered]@{
            width = $bmp.Width
            height = $bmp.Height
            avg_luma_sampled = [math]::Round($sumLuma / [double]$samples, 4)
            avg_alpha_sampled = [math]::Round($sumAlpha / [double]$samples, 4)
            alpha_coverage_gt_32 = [math]::Round($coverage / [double]$samples, 4)
        }
    }
    finally {
        $bmp.Dispose()
    }
}
$GetPngStats = (Get-Command Get-PngStats).ScriptBlock

$materialEntries = @()
foreach ($spec in $MaterialSpecs) {
    $matPath = Join-Parts @($PackageRoot, "Runtime", "Materials", "$($spec.Id).mat")
    $texturePaths = [ordered]@{
        albedo = Join-Parts @($PackageRoot, "Runtime", "Textures", "Albedo", "$($spec.Id)_ALB.png")
        normal = Join-Parts @($PackageRoot, "Runtime", "Textures", "Normal", "$($spec.Id)_NRM.png")
        roughness_metallic_ao_smoothness = Join-Parts @($PackageRoot, "Runtime", "Textures", "RoughnessMetallic", "$($spec.Id)_RMA.png")
        grime_edge_wetness_mask = Join-Parts @($PackageRoot, "Runtime", "Textures", "GrimeEdgewear", "$($spec.Id)_GRM.png")
        height = Join-Parts @($PackageRoot, "Runtime", "Textures", "Height", "$($spec.Id)_HGT.png")
    }
    $materialEntries += [ordered]@{
        id = $spec.Id
        display_name = $spec.DisplayName
        path = Get-RepoRelative $matPath
        guid = Get-AssetGuid $matPath
        status = $spec.Status
        import_priority = $spec.ImportPriority
        family = $spec.Family
        metallic = $spec.Metallic
        smoothness = $spec.Smoothness
        bump_scale = $spec.BumpScale
        use = $spec.Use
        description = $spec.Description
        tiling_note = $spec.TilingNote
        validation_note = $spec.ValidationNote
        transparent_overlay = [bool]$spec.Transparent
        albedo_stats = (& $GetPngStats $texturePaths.albedo)
        textures = [ordered]@{
            albedo = Get-RepoRelative $texturePaths.albedo
            normal = Get-RepoRelative $texturePaths.normal
            roughness_metallic_ao_smoothness = Get-RepoRelative $texturePaths.roughness_metallic_ao_smoothness
            grime_edge_wetness_mask = Get-RepoRelative $texturePaths.grime_edge_wetness_mask
            height = Get-RepoRelative $texturePaths.height
        }
    }
}

$runtimeTexturePaths = @()
foreach ($spec in $MaterialSpecs) {
    foreach ($map in $TextureFolders.Keys) {
        $runtimeTexturePaths += Join-Parts @($PackageRoot, "Runtime", "Textures", $TextureFolders[$map], "$($spec.Id)_$map.png")
    }
}

$runtimeMaterialPaths = $MaterialSpecs | ForEach-Object { Join-Parts @($PackageRoot, "Runtime", "Materials", "$($_.Id).mat") }
$packagePreviewRelPaths = $previewFiles | ForEach-Object { Get-RepoRelative (Join-Parts @($packagePreviewDir, $_)) }
$conceptPreviewRelPaths = $previewFiles | ForEach-Object { Get-RepoRelative (Join-Parts @($ConceptRoot, $_)) }

$catalogPath = Join-Parts @($PackageRoot, "Runtime", "Metadata", "RMS10_RoomMaterialCatalog_v$FullVersion.json")
$catalogObj = [ordered]@{
    schema = "brassworks.room_material_set10_catalog.v1"
    package = "BrassworksBreach.RoomMaterialSet10"
    package_id = $PackageName
    version = $FullVersion
    generated_at_utc = $GeneratedAtUtc
    material_count = $MaterialSpecs.Count
    texture_count = $runtimeTexturePaths.Count
    package_preview_count = $previewFiles.Count
    external_concept_preview_count = $previewFiles.Count
    texture_channel_convention = [ordered]@{
        ALB = "sRGB albedo/base color; overlay alpha is encoded where relevant"
        NRM = "linear tangent-space normal detail"
        RMA = "linear R=metallic intent, G=roughness intent, B=ambient occlusion/grime intent, A=Unity Standard smoothness"
        GRM = "linear R=edge/chip mask, G=grime/soot mask, B=wetness mask, A=overlay alpha where relevant"
        HGT = "linear height/parallax intent"
    }
    north_star_alignment = @(
        "Continuous material-driven masonry rather than individual construction-block bricks.",
        "Wall and ceiling brick scale is smaller than floor flagstones.",
        "Dark center/back-wall readability is protected by low average albedo.",
        "Wet reflection intent is encoded as smoothness and wetness masks rather than metallic orange color.",
        "Soot and dampness are separated into overlay candidates for restrained corner/lamp treatment."
    )
    materials = $materialEntries
}
Write-Utf8NoBom $catalogPath ($catalogObj | ConvertTo-Json -Depth 12)
Write-DefaultMeta $catalogPath "RMS10 catalog"

$exportArtifactRelPaths = @(
    (Get-RepoRelative $packageJsonPath)
    (Get-RepoRelative $readmePath)
    (Get-RepoRelative $catalogPath)
) + ($runtimeMaterialPaths | ForEach-Object { Get-RepoRelative $_ }) + ($runtimeTexturePaths | ForEach-Object { Get-RepoRelative $_ }) + $packagePreviewRelPaths

$manifestObj = [ordered]@{
    pack_id = $PackId
    display_name = "Room Material Set 10"
    version = $Version
    build_id = $BuildId
    unity_version = $UnityVersion
    generated_at_utc = $GeneratedAtUtc
    sidecar_project = "UD-SC-ROOM-RoomMaterialSet10"
    owner_lane = "sidecar-room-material-set10"
    primary_intake_owner = "main-lane-art-integration"
    canonical_root = "AssetPacks/BrassworksBreach.RoomMaterialSet10"
    package_root = "AssetPacks/BrassworksBreach.RoomMaterialSet10"
    package_name = $PackageName
    common_schema = "brassworks.sidecar.visual_pack_manifest.v1"
    export_artifacts = $exportArtifactRelPaths
    asset_counts = [ordered]@{
        generated_prefabs = 0
        generated_materials = $MaterialSpecs.Count
        generated_meshes = 0
        runtime_texture_pngs = $runtimeTexturePaths.Count
        metadata_catalogs = 1
        preview_pngs_package = $previewFiles.Count
        preview_pngs_documentation = $previewFiles.Count
        runtime_scripts = 1
        asmdefs = 1
        colliders = 0
        audio = 0
        vfx = 0
    }
    dependencies = @(
        "Unity Package Manager local package reference",
        "Unity built-in Standard shader",
        "Unity TextureImporter and Material YAML import",
        "No Blender or external DCC source files"
    )
    required_primary_changes = @(
        "Add local package reference only after quarantine import review.",
        "Bind RMS10 wall/ceiling/floor materials into roomtest-derived lookdev scenes before replacing production surfaces.",
        "Confirm main-project transparent/decal shader path before promoting overlay materials."
    )
    path_collisions_checked = $true
    guid_collisions_checked = $true
    import_smoke_status = "static_ready_unity_editor_not_launched"
    generated_materials = ($runtimeMaterialPaths | ForEach-Object { "Packages/$PackageName/" + ((Get-RepoRelative $_) -replace '^AssetPacks/BrassworksBreach.RoomMaterialSet10/', '') })
    runtime_texture_pngs = ($runtimeTexturePaths | ForEach-Object { "Packages/$PackageName/" + ((Get-RepoRelative $_) -replace '^AssetPacks/BrassworksBreach.RoomMaterialSet10/', '') })
    package_previews = ($packagePreviewRelPaths | ForEach-Object { "Packages/$PackageName/" + ($_ -replace '^AssetPacks/BrassworksBreach.RoomMaterialSet10/', '') })
    documentation_previews = $conceptPreviewRelPaths
    known_risks = @(
        "Transparent overlay materials use built-in Standard alpha blending; quarantine intake should verify sorting/decal behavior in the primary render path.",
        "Generated texture maps are procedural 512x512 candidates; final art pass may want hand-authored breakup if repeated over very large rooms.",
        "Static validation was run without launching the Unity Editor in this worker lane."
    )
    rollback_path = "remove local package reference $PackageName, then delete isolated package root AssetPacks/BrassworksBreach.RoomMaterialSet10"
}

$manifestPath = Join-Parts @($PackageRoot, "Documentation~", "Manifest", "RMS10_RoomMaterialSet10_Manifest_v$FullVersion.json")
Write-Utf8NoBom $manifestPath ($manifestObj | ConvertTo-Json -Depth 12)
Write-DefaultMeta $manifestPath "RMS10 sidecar manifest"

$manifestMirrorPath = Join-Parts @($ProductionRoot, "RMS10_RoomMaterialSet10_Manifest_v$FullVersion.json")
Write-Utf8NoBom $manifestMirrorPath ($manifestObj | ConvertTo-Json -Depth 12)

$inventoryLines = New-Object System.Collections.Generic.List[string]
$inventoryLines.Add("# RMS10 Asset Inventory $FullVersion")
$inventoryLines.Add("")
$inventoryLines.Add("Generated: $GeneratedAtUtc")
$inventoryLines.Add("")
$inventoryLines.Add("Package root: AssetPacks/BrassworksBreach.RoomMaterialSet10")
$inventoryLines.Add("")
$inventoryLines.Add("## Materials")
$inventoryLines.Add("")
foreach ($entry in $materialEntries) {
    $inventoryLines.Add("- $($entry.id): $($entry.display_name), $($entry.status), smoothness $($entry.smoothness), bump $($entry.bump_scale), path $($entry.path)")
}
$inventoryLines.Add("")
$inventoryLines.Add("## Texture Maps")
$inventoryLines.Add("")
foreach ($path in $runtimeTexturePaths | Sort-Object) {
    $inventoryLines.Add("- $(Get-RepoRelative $path)")
}
$inventoryLines.Add("")
$inventoryLines.Add("## Package Preview PNGs")
$inventoryLines.Add("")
foreach ($path in $packagePreviewRelPaths) {
    $inventoryLines.Add("- $path")
}
$inventoryLines.Add("")
$inventoryLines.Add("## Documentation Preview PNGs")
$inventoryLines.Add("")
foreach ($path in $conceptPreviewRelPaths) {
    $inventoryLines.Add("- $path")
}
$inventoryPath = Join-Parts @($ProductionRoot, "RMS10_AssetInventory_$FullVersion.md")
Write-Utf8NoBom $inventoryPath ($inventoryLines -join "`r`n")

$productionReportLines = New-Object System.Collections.Generic.List[string]
$productionReportLines.Add("# RMS10 Production Report $FullVersion")
$productionReportLines.Add("")
$productionReportLines.Add("Generated: $GeneratedAtUtc")
$productionReportLines.Add("")
$productionReportLines.Add("## Brief")
$productionReportLines.Add("")
$productionReportLines.Add("Build a bundled sidecar package for final-direction dark wet masonry materials inspired by the accepted roomtest/north-star look.")
$productionReportLines.Add("")
$productionReportLines.Add("## Output Summary")
$productionReportLines.Add("")
$productionReportLines.Add("- Package: AssetPacks/BrassworksBreach.RoomMaterialSet10")
$productionReportLines.Add("- Package name: $PackageName")
$productionReportLines.Add("- Materials: $($MaterialSpecs.Count)")
$productionReportLines.Add("- Runtime texture PNGs: $($runtimeTexturePaths.Count) at 512x512")
$productionReportLines.Add("- Package-local preview PNGs: $($previewFiles.Count)")
$productionReportLines.Add("- External concept preview PNGs: $($previewFiles.Count)")
$productionReportLines.Add("- Meshes/prefabs/scenes/colliders/audio/gameplay authority: 0")
$productionReportLines.Add("")
$productionReportLines.Add("## Material Families")
$productionReportLines.Add("")
$productionReportLines.Add("- Dark wet brick wall: wall-scale continuous small brick, black mortar, damp vertical seam response.")
$productionReportLines.Add("- Sooted brick ceiling: compressed dark brick with broad low-sheen soot.")
$productionReportLines.Add("- Wet uneven flagstone floor: larger irregular slabs with pooled wetness and recessed black joints.")
$productionReportLines.Add("- Black mortar/grime: nearly black rough filler material for corners and seams.")
$productionReportLines.Add("- Edge dampness overlay: transparent candidate for base edges and leak streaks.")
$productionReportLines.Add("- Soot/decal overlay: transparent candidate for lamp, vent, ceiling, and pipe smoke buildup.")
$productionReportLines.Add("")
$productionReportLines.Add("## Roomtest/North-Star Alignment")
$productionReportLines.Add("")
$productionReportLines.Add("- Follows roomtest v0.5's accepted move away from construction-block brick geometry toward continuous material-driven surfaces.")
$productionReportLines.Add("- Preserves smaller wall/ceiling brick scale versus larger floor slabs.")
$productionReportLines.Add("- Keeps the palette dark and wet without turning surfaces into orange metal.")
$productionReportLines.Add("- Separates dampness and soot overlays so corner/lamp grime can stay restrained.")
$productionReportLines.Add("")
$productionReportLines.Add("## Tooling Boundary")
$productionReportLines.Add("")
$productionReportLines.Add("No Blender or external DCC source assets were used. Output is Unity package content: Unity Standard materials, Unity .meta importer data, PNG maps, and package documentation.")
$productionReport = $productionReportLines -join "`r`n"
$productionReportPath = Join-Parts @($ProductionRoot, "RMS10_ProductionReport_$FullVersion.md")
Write-Utf8NoBom $productionReportPath $productionReport

$prodReadme = @"
# V0.1.55 Room Material Set 10

Production docs for `BrassworksBreach.RoomMaterialSet10`.

- `GenerateRoomMaterialSet10.ps1`: deterministic package/content generator.
- `RMS10_ProductionReport_$FullVersion.md`: production summary and art-direction alignment.
- `RMS10_AssetInventory_$FullVersion.md`: complete generated file inventory.
- `RMS10_RoomMaterialSet10_Manifest_v$FullVersion.json`: manifest mirror.
"@
Write-Utf8NoBom (Join-Parts @($ProductionRoot, "README.md")) $prodReadme

$planningReadme = @"
# RMS10 Import Readiness Planning

Planning notes for quarantined intake of `com.brassworks.sidecar.room-material-set10`.
"@
Write-Utf8NoBom (Join-Parts @($PlanningRoot, "README.md")) $planningReadme

$importReadiness = @"
# RMS10 Import Readiness Notes $FullVersion

Generated: $GeneratedAtUtc

## Intake Recommendation

Verdict: `STATIC READY WITH LIMITATIONS`.

Import through a quarantine Unity project before adding a local package reference to the primary project. The package is isolated and visual/material-only, but the two transparent overlay materials need render-path verification before production use.

## Required Quarantine Checks

- Add local package reference to `AssetPacks/BrassworksBreach.RoomMaterialSet10`.
- Confirm all 6 `.mat` assets import with built-in Standard shader references.
- Confirm all 30 PNG maps import at 512x512 with expected sRGB/linear/normal settings.
- Apply wall, ceiling, and floor materials to a roomtest-derived board. Wall and ceiling brick should tile smaller than floor flagstones.
- Test `RMS10_MAT_EdgeDampnessOverlay` and `RMS10_MAT_SootDecalOverlay` with the primary transparent/decal strategy.

## Promotion Order

1. `RMS10_MAT_DarkWetBrickWall`
2. `RMS10_MAT_SootedBrickCeiling`
3. `RMS10_MAT_WetUnevenFlagstoneFloor`
4. `RMS10_MAT_BlackMortarGrime`
5. Overlay candidates after transparency review.

## Rollback

Remove the local package reference $PackageName, then delete AssetPacks/BrassworksBreach.RoomMaterialSet10.
"@
Write-Utf8NoBom (Join-Parts @($PlanningRoot, "RMS10_ImportReadinessNotes_$FullVersion.md")) $importReadiness

$qaReadme = @"
# RMS10 Import Readiness QA

Static QA report for the isolated package. Unity Editor import smoke is intentionally not claimed here.
"@
Write-Utf8NoBom (Join-Parts @($QaRoot, "README.md")) $qaReadme

$allPackageFiles = Get-ChildItem -LiteralPath $PackageRoot -File -Recurse | Sort-Object FullName
$pngFiles = $allPackageFiles | Where-Object { $_.Extension -ieq ".png" }
$matFiles = $allPackageFiles | Where-Object { $_.Extension -ieq ".mat" }
$jsonFiles = @(
    $packageJsonPath,
    $catalogPath,
    $manifestPath,
    $manifestMirrorPath
)

$pngStats = @()
foreach ($file in $pngFiles) {
    $pngStats += [ordered]@{
        path = Get-RepoRelative $file.FullName
        stats = (& $GetPngStats $file.FullName)
        sha256 = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash.ToLowerInvariant()
    }
}

$overlayAlpha = [ordered]@{}
foreach ($id in @("RMS10_MAT_EdgeDampnessOverlay", "RMS10_MAT_SootDecalOverlay")) {
    $path = Join-Parts @($PackageRoot, "Runtime", "Textures", "Albedo", "${id}_ALB.png")
    $overlayAlpha[$id] = (& $GetPngStats $path)
}

$qaReport = [ordered]@{
    schema = "brassworks.room_material_set10.static_validation.v1"
    package = "BrassworksBreach.RoomMaterialSet10"
    package_id = $PackageName
    version = $FullVersion
    generated_at_utc = $GeneratedAtUtc
    validation_status = "PASS_STATIC_READY_WITH_LIMITATIONS"
    unity_editor_import_smoke = "not_run"
    checked_roots = @(
        (Get-RepoRelative $PackageRoot)
        (Get-RepoRelative $ProductionRoot)
        (Get-RepoRelative $ConceptRoot)
        (Get-RepoRelative $PlanningRoot)
        (Get-RepoRelative $QaRoot)
    )
    counts = [ordered]@{
        package_files_including_meta = $allPackageFiles.Count
        runtime_materials = $matFiles.Count
        runtime_texture_pngs = $runtimeTexturePaths.Count
        package_preview_pngs = $previewFiles.Count
        documentation_preview_pngs = $previewFiles.Count
        runtime_scripts = 1
        asmdefs = 1
    }
    measurable_checks = [ordered]@{
        runtime_texture_dimensions = "30/30 runtime texture PNGs are 512x512"
        package_preview_dimensions = "4/4 package preview PNGs are 1536x768 or larger"
        overlay_alpha_edge_dampness_avg = $overlayAlpha["RMS10_MAT_EdgeDampnessOverlay"].avg_alpha_sampled
        overlay_alpha_edge_dampness_coverage_gt_32 = $overlayAlpha["RMS10_MAT_EdgeDampnessOverlay"].alpha_coverage_gt_32
        overlay_alpha_soot_avg = $overlayAlpha["RMS10_MAT_SootDecalOverlay"].avg_alpha_sampled
        overlay_alpha_soot_coverage_gt_32 = $overlayAlpha["RMS10_MAT_SootDecalOverlay"].alpha_coverage_gt_32
        dark_wall_albedo_avg_luma = (& $GetPngStats (Join-Parts @($PackageRoot, "Runtime", "Textures", "Albedo", "RMS10_MAT_DarkWetBrickWall_ALB.png"))).avg_luma_sampled
        ceiling_albedo_avg_luma = (& $GetPngStats (Join-Parts @($PackageRoot, "Runtime", "Textures", "Albedo", "RMS10_MAT_SootedBrickCeiling_ALB.png"))).avg_luma_sampled
        flagstone_albedo_avg_luma = (& $GetPngStats (Join-Parts @($PackageRoot, "Runtime", "Textures", "Albedo", "RMS10_MAT_WetUnevenFlagstoneFloor_ALB.png"))).avg_luma_sampled
    }
    json_files_parsed_by_generator = ($jsonFiles | ForEach-Object { Get-RepoRelative $_ })
    png_samples = $pngStats
    limitations = @(
        "Unity Editor was not launched by this generator; quarantine import remains required.",
        "Overlay materials use Standard alpha blending and need render-order/decal validation.",
        "Texture art is procedural and may require final hand-authored variation for very large uninterrupted rooms."
    )
}
$qaReportPath = Join-Parts @($QaRoot, "RMS10_FileValidationReport_$FullVersion.json")
Write-Utf8NoBom $qaReportPath ($qaReport | ConvertTo-Json -Depth 12)

Write-Host "Generated $PackageDisplayName $FullVersion"
Write-Host "Package root: $PackageRoot"
Write-Host "Materials: $($MaterialSpecs.Count)"
Write-Host "Runtime texture PNGs: $($runtimeTexturePaths.Count)"
Write-Host "Preview PNGs: $($previewFiles.Count) package, $($previewFiles.Count) documentation"
