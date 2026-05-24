Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
[System.Threading.Thread]::CurrentThread.CurrentCulture = [System.Globalization.CultureInfo]::InvariantCulture
[System.Threading.Thread]::CurrentThread.CurrentUICulture = [System.Globalization.CultureInfo]::InvariantCulture

$DocRoot = "D:\__MY APPS\Unity Doom\Documentation\AssetProduction\EnemyReadabilityBatch"
$AssetRoot = "D:\__MY APPS\Unity Doom\Assets\_Project\ArtStaging\EnemyReadabilityBatch"
$MeshRoot = Join-Path $AssetRoot "Meshes"
$MaterialRoot = Join-Path $AssetRoot "Materials"
$PreviewRoot = Join-Path $AssetRoot "Previews"
$MetadataRoot = Join-Path $AssetRoot "Metadata"
$DocPreviewRoot = Join-Path $DocRoot "Previews"

foreach ($path in @($DocRoot, $DocPreviewRoot, $AssetRoot, $MeshRoot, $MaterialRoot, $PreviewRoot, $MetadataRoot)) {
    New-Item -ItemType Directory -Force $path | Out-Null
}

function Write-TextFile {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [Parameter(Mandatory = $true)][string]$Content
    )
    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllText($Path, $Content, $utf8NoBom)
}

function Get-GuidHex {
    param([Parameter(Mandatory = $true)][string]$Identity)
    $md5 = [System.Security.Cryptography.MD5]::Create()
    try {
        $bytes = [System.Text.Encoding]::UTF8.GetBytes("EnemyReadabilityBatch|" + $Identity.ToLowerInvariant())
        return (($md5.ComputeHash($bytes) | ForEach-Object { $_.ToString("x2") }) -join "")
    }
    finally {
        $md5.Dispose()
    }
}

function Write-FolderMeta {
    param([Parameter(Mandatory = $true)][string]$FolderPath)
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
    Write-TextFile -Path ($FolderPath + ".meta") -Content $content
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

function Write-ModelMeta {
    param([Parameter(Mandatory = $true)][string]$AssetPath)
    $guid = Get-GuidHex $AssetPath
    $content = @"
fileFormatVersion: 2
guid: $guid
ModelImporter:
  serializedVersion: 24200
  internalIDToNameTable: []
  externalObjects: {}
  materials:
    materialImportMode: 2
    materialName: 0
    materialSearch: 1
    materialLocation: 1
  animations:
    legacyGenerateAnimations: 4
    bakeSimulation: 0
    resampleCurves: 1
    optimizeGameObjects: 0
    removeConstantScaleCurves: 0
    motionNodeName:
    animationImportErrors:
    animationImportWarnings:
    animationRetargetingWarnings:
    animationDoRetargetingWarnings: 0
    importAnimatedCustomProperties: 0
    importConstraints: 0
    animationCompression: 1
    animationRotationError: 0.5
    animationPositionError: 0.5
    animationScaleError: 0.5
    animationWrapMode: 0
    extraExposedTransformPaths: []
    extraUserProperties: []
    clipAnimations: []
    isReadable: 0
  meshes:
    lODScreenPercentages: []
    globalScale: 1
    meshCompression: 0
    addColliders: 0
    useSRGBMaterialColor: 1
    sortHierarchyByName: 1
    importPhysicalCameras: 1
    importVisibility: 1
    importBlendShapes: 1
    importCameras: 1
    importLights: 1
    nodeNameCollisionStrategy: 1
    fileIdsGeneration: 2
    swapUVChannels: 0
    generateSecondaryUV: 0
    useFileUnits: 1
    keepQuads: 0
    weldVertices: 1
    bakeAxisConversion: 0
    preserveHierarchy: 0
    skinWeightsMode: 0
    maxBonesPerVertex: 4
    minBoneWeight: 0.001
    optimizeBones: 1
    generateMeshLods: 0
    meshLodGenerationFlags: 0
    maximumMeshLod: -1
    meshOptimizationFlags: -1
    indexFormat: 0
    secondaryUVAngleDistortion: 8
    secondaryUVAreaDistortion: 15.000001
    secondaryUVHardAngle: 88
    secondaryUVMarginMethod: 1
    secondaryUVMinLightmapResolution: 40
    secondaryUVMinObjectScale: 1
    secondaryUVPackMargin: 4
    useFileScale: 1
    strictVertexDataChecks: 0
  tangentSpace:
    normalSmoothAngle: 60
    normalImportMode: 0
    tangentImportMode: 3
    normalCalculationMode: 4
    legacyComputeAllNormalsFromSmoothingGroupsWhenMeshHasBlendShapes: 0
    blendShapeNormalImportMode: 1
    normalSmoothingSource: 0
  referencedClips: []
  importAnimation: 1
  humanDescription:
    serializedVersion: 3
    human: []
    skeleton: []
    armTwist: 0.5
    foreArmTwist: 0.5
    upperLegTwist: 0.5
    legTwist: 0.5
    armStretch: 0.05
    legStretch: 0.05
    feetSpacing: 0
    globalScale: 1
    rootMotionBoneName:
    hasTranslationDoF: 0
    hasExtraRoot: 0
    skeletonHasParents: 1
  lastHumanDescriptionAvatarSource: {instanceID: 0}
  autoGenerateAvatarMappingIfUnspecified: 1
  animationType: 2
  humanoidOversampling: 1
  avatarSetup: 0
  addHumanoidExtraRootOnlyWhenUsingAvatar: 1
  importBlendShapeDeformPercent: 1
  remapMaterialsIfMaterialImportModeIsNone: 0
  additionalBone: 0
  userData:
  assetBundleName:
  assetBundleVariant:
"@
    Write-TextFile -Path ($AssetPath + ".meta") -Content $content
}

function Write-TextureMeta {
    param([Parameter(Mandatory = $true)][string]$AssetPath)
    $guid = Get-GuidHex $AssetPath
    $content = @"
fileFormatVersion: 2
guid: $guid
TextureImporter:
  internalIDToNameTable: []
  externalObjects: {}
  serializedVersion: 13
  mipmaps:
    mipMapMode: 0
    enableMipMap: 1
    sRGBTexture: 1
    linearTexture: 0
    fadeOut: 0
    borderMipMap: 0
    mipMapsPreserveCoverage: 0
    alphaTestReferenceValue: 0.5
    mipMapFadeDistanceStart: 1
    mipMapFadeDistanceEnd: 3
  bumpmap:
    convertToNormalMap: 0
    externalNormalMap: 0
    heightScale: 0.25
    normalMapFilter: 0
    flipGreenChannel: 0
  isReadable: 0
  streamingMipmaps: 0
  streamingMipmapsPriority: 0
  vTOnly: 0
  ignoreMipmapLimit: 0
  grayScaleToAlpha: 0
  generateCubemap: 6
  cubemapConvolution: 0
  seamlessCubemap: 0
  textureFormat: 1
  maxTextureSize: 2048
  textureSettings:
    serializedVersion: 2
    filterMode: 1
    aniso: 1
    mipBias: 0
    wrapU: 0
    wrapV: 0
    wrapW: 0
  nPOTScale: 1
  lightmap: 0
  compressionQuality: 50
  spriteMode: 0
  spriteExtrude: 1
  spriteMeshType: 1
  alignment: 0
  spritePivot: {x: 0.5, y: 0.5}
  spritePixelsToUnits: 100
  spriteBorder: {x: 0, y: 0, z: 0, w: 0}
  spriteGenerateFallbackPhysicsShape: 1
  alphaUsage: 1
  alphaIsTransparency: 0
  spriteTessellationDetail: -1
  textureType: 0
  textureShape: 1
  singleChannelComponent: 0
  flipbookRows: 1
  flipbookColumns: 1
  maxTextureSizeSet: 0
  compressionQualitySet: 0
  textureFormatSet: 0
  ignorePngGamma: 0
  applyGammaDecoding: 0
  swizzle: 50462976
  cookieLightType: 0
  platformSettings:
  - serializedVersion: 4
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    ignorePlatformSupport: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  - serializedVersion: 4
    buildTarget: Standalone
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    ignorePlatformSupport: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  - serializedVersion: 4
    buildTarget: Android
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    ignorePlatformSupport: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  - serializedVersion: 4
    buildTarget: WindowsStoreApps
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    ignorePlatformSupport: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  spriteSheet:
    serializedVersion: 2
    sprites: []
    outline: []
    customData:
    physicsShape: []
    bones: []
    spriteID:
    internalID: 0
    vertices: []
    indices:
    edges: []
    weights: []
    secondaryTextures: []
    spriteCustomMetadata:
      entries: []
    nameFileIdTable: {}
  mipmapLimitGroupName:
  pSDRemoveMatte: 0
  userData:
  assetBundleName:
  assetBundleVariant:
"@
    Write-TextFile -Path ($AssetPath + ".meta") -Content $content
}

$Materials = @(
    @{ Name = "MAT_ERB_BlackenedIron"; Label = "blackened iron chassis"; Kd = @(0.075, 0.071, 0.066); Ks = @(0.18, 0.17, 0.16); Color = @(0.075, 0.071, 0.066, 1); Metallic = 0.72; Gloss = 0.34; Emission = @(0, 0, 0, 1) },
    @{ Name = "MAT_ERB_AgedBrass"; Label = "aged brass trim"; Kd = @(0.74, 0.49, 0.18); Ks = @(0.42, 0.34, 0.20); Color = @(0.74, 0.49, 0.18, 1); Metallic = 0.70; Gloss = 0.48; Emission = @(0, 0, 0, 1) },
    @{ Name = "MAT_ERB_CopperPipe"; Label = "oxidized copper pressure pipe"; Kd = @(0.55, 0.25, 0.12); Ks = @(0.32, 0.22, 0.14); Color = @(0.55, 0.25, 0.12, 1); Metallic = 0.64; Gloss = 0.40; Emission = @(0, 0, 0, 1) },
    @{ Name = "MAT_ERB_DarkRubber"; Label = "dark rubber hoses and gaskets"; Kd = @(0.035, 0.030, 0.028); Ks = @(0.05, 0.045, 0.04); Color = @(0.035, 0.030, 0.028, 1); Metallic = 0.00; Gloss = 0.18; Emission = @(0, 0, 0, 1) },
    @{ Name = "MAT_ERB_CreamEnamel"; Label = "cream enamel face plates"; Kd = @(0.72, 0.66, 0.50); Ks = @(0.18, 0.16, 0.12); Color = @(0.72, 0.66, 0.50, 1); Metallic = 0.00; Gloss = 0.30; Emission = @(0, 0, 0, 1) },
    @{ Name = "MAT_ERB_FurnaceEyeAmber"; Label = "hot amber furnace eyes"; Kd = @(1.00, 0.38, 0.06); Ks = @(0.25, 0.14, 0.05); Color = @(1.00, 0.38, 0.06, 1); Metallic = 0.00; Gloss = 0.55; Emission = @(1.20, 0.32, 0.04, 1) },
    @{ Name = "MAT_ERB_WeakPointLamp"; Label = "readable weak-point lamp glass"; Kd = @(1.00, 0.75, 0.12); Ks = @(0.30, 0.18, 0.08); Color = @(1.00, 0.75, 0.12, 1); Metallic = 0.00; Gloss = 0.62; Emission = @(1.35, 0.72, 0.10, 1) },
    @{ Name = "MAT_ERB_PressureTankRed"; Label = "danger red pressure tanks"; Kd = @(0.58, 0.06, 0.04); Ks = @(0.22, 0.08, 0.06); Color = @(0.58, 0.06, 0.04, 1); Metallic = 0.48; Gloss = 0.36; Emission = @(0, 0, 0, 1) },
    @{ Name = "MAT_ERB_BoltTellBlue"; Label = "blue bolt/charge tell"; Kd = @(0.08, 0.56, 1.00); Ks = @(0.16, 0.34, 0.50); Color = @(0.08, 0.56, 1.00, 1); Metallic = 0.00; Gloss = 0.70; Emission = @(0.08, 0.82, 1.55, 1) },
    @{ Name = "MAT_ERB_HazardEnamel"; Label = "yellow hazard enamel for attack tells"; Kd = @(0.95, 0.66, 0.08); Ks = @(0.22, 0.18, 0.08); Color = @(0.95, 0.66, 0.08, 1); Metallic = 0.00; Gloss = 0.32; Emission = @(0, 0, 0, 1) },
    @{ Name = "MAT_ERB_ShutdownFragmentDim"; Label = "dim shutdown fragments"; Kd = @(0.20, 0.18, 0.15); Ks = @(0.08, 0.07, 0.06); Color = @(0.20, 0.18, 0.15, 1); Metallic = 0.32; Gloss = 0.20; Emission = @(0, 0, 0, 1) },
    @{ Name = "MAT_ERB_SootShadow"; Label = "soot-black visor cavities"; Kd = @(0.018, 0.017, 0.015); Ks = @(0.04, 0.035, 0.03); Color = @(0.018, 0.017, 0.015, 1); Metallic = 0.00; Gloss = 0.12; Emission = @(0, 0, 0, 1) }
)

function Write-MaterialAssets {
    $mtl = [System.Collections.Generic.List[string]]::new()
    $mtl.Add("# Brassworks Breach EnemyReadabilityBatch proxy materials")
    $mtl.Add("# Unity import note: OBJ/MTL colors are proxy materials for staging only.")
    $mtl.Add("")

    foreach ($mat in $Materials) {
        $mtl.Add("newmtl $($mat.Name)")
        $mtl.Add("# $($mat.Label)")
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
        if (($mat.Emission[0] + $mat.Emission[1] + $mat.Emission[2]) -gt 0) {
            $keywords = "`n  - _EMISSION"
        }
        $lightmapFlags = 4
        if (($mat.Emission[0] + $mat.Emission[1] + $mat.Emission[2]) -gt 0) {
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
    - _BumpMap:
        m_Texture: {fileID: 0}
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
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _MetallicGlossMap:
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _OcclusionMap:
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    - _ParallaxMap:
        m_Texture: {fileID: 0}
        m_Scale: {x: 1, y: 1}
        m_Offset: {x: 0, y: 0}
    m_Ints: []
    m_Floats:
    - _BumpScale: 1
    - _Cutoff: 0.5
    - _DetailNormalMapScale: 1
    - _DstBlend: 0
    - _GlossMapScale: 1
    - _Glossiness: $($mat.Gloss)
    - _GlossyReflections: 1
    - _Metallic: $($mat.Metallic)
    - _Mode: 0
    - _OcclusionStrength: 1
    - _Parallax: 0.02
    - _SmoothnessTextureChannel: 0
    - _SpecularHighlights: 1
    - _SrcBlend: 1
    - _UVSec: 0
    - _ZWrite: 1
    m_Colors:
    - _Color: {r: $($mat.Color[0]), g: $($mat.Color[1]), b: $($mat.Color[2]), a: $($mat.Color[3])}
    - _EmissionColor: {r: $($mat.Emission[0]), g: $($mat.Emission[1]), b: $($mat.Emission[2]), a: $($mat.Emission[3])}
  m_BuildTextureStacks: []
  m_AllowLocking: 1
"@
        Write-TextFile -Path $matPath -Content $content
        Write-NativeMeta -AssetPath $matPath
    }

    $mtlPath = Join-Path $MaterialRoot "ENEMY_ERB_ReadabilityProxyMaterials.mtl"
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
    $script:ObjLines.Add("# Brassworks Breach EnemyReadabilityBatch generated staging mesh")
    $script:ObjLines.Add("# Units: meters. Axis: +Y up, +Z forward. No colliders, no gameplay code.")
    $script:ObjLines.Add("mtllib ../Materials/ENEMY_ERB_ReadabilityProxyMaterials.mtl")
    $script:ObjLines.Add("o $ObjectName")
    $script:ObjLines.Add("")
}

function Save-Obj {
    param([string]$Path)
    Write-TextFile -Path $Path -Content ($script:ObjLines -join "`r`n")
    Write-ModelMeta -AssetPath $Path
    return @{ vertices = $script:VertexCount; faces = $script:FaceCount }
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

function Add-Lamp {
    param([string]$Prefix, [double]$Cx, [double]$Cy, [double]$Cz, [double]$Radius = 0.085)
    Add-Cylinder "$($Prefix)_blackened_lamp_ring" "MAT_ERB_BlackenedIron" "Z" $Cx $Cy $Cz ($Radius * 1.22) 0.040 16
    Add-Cylinder "$($Prefix)_readable_amber_glass" "MAT_ERB_WeakPointLamp" "Z" $Cx $Cy ($Cz + 0.030) $Radius 0.025 16
    Add-Cylinder "$($Prefix)_aged_brass_retainer" "MAT_ERB_AgedBrass" "Z" $Cx $Cy ($Cz + 0.055) ($Radius * 0.72) 0.015 16
}

function Add-FurnaceEyePair {
    param([string]$Prefix, [double]$Cy, [double]$Cz, [double]$Spread = 0.11, [double]$Radius = 0.040)
    Add-Cylinder "$($Prefix)_left_furnace_eye_socket" "MAT_ERB_SootShadow" "Z" (-1 * $Spread) $Cy $Cz ($Radius * 1.35) 0.030 12
    Add-Cylinder "$($Prefix)_left_furnace_eye_amber" "MAT_ERB_FurnaceEyeAmber" "Z" (-1 * $Spread) $Cy ($Cz + 0.025) $Radius 0.020 12
    Add-Cylinder "$($Prefix)_right_furnace_eye_socket" "MAT_ERB_SootShadow" "Z" $Spread $Cy $Cz ($Radius * 1.35) 0.030 12
    Add-Cylinder "$($Prefix)_right_furnace_eye_amber" "MAT_ERB_FurnaceEyeAmber" "Z" $Spread $Cy ($Cz + 0.025) $Radius 0.020 12
}

function Add-CutterWheel {
    param([string]$Prefix, [double]$Cx, [double]$Cy, [double]$Cz)
    Add-Cylinder "$($Prefix)_cutter_wheel_blackened_disc" "MAT_ERB_BlackenedIron" "X" $Cx $Cy $Cz 0.205 0.080 18
    Add-Cylinder "$($Prefix)_cutter_wheel_amber_warning_hub" "MAT_ERB_WeakPointLamp" "X" ($Cx + 0.050) $Cy $Cz 0.070 0.028 14
    for ($i = 0; $i -lt 10; $i++) {
        $t = (2.0 * [Math]::PI * $i) / 10
        $y = $Cy + [Math]::Sin($t) * 0.245
        $z = $Cz + [Math]::Cos($t) * 0.245
        Add-Box "$($Prefix)_cutter_tooth_$('{0:00}' -f $i)_hazard_silhouette" "MAT_ERB_HazardEnamel" $Cx $y $z 0.105 0.040 0.085
    }
}

function Add-BoltCoils {
    param([string]$Prefix, [double]$Cx, [double]$Cy, [double]$StartZ, [double]$Spacing, [int]$Count)
    for ($i = 0; $i -lt $Count; $i++) {
        $z = $StartZ + $Spacing * $i
        Add-Cylinder "$($Prefix)_bolt_charge_ring_$('{0:00}' -f $i)" "MAT_ERB_BoltTellBlue" "Z" $Cx $Cy $z 0.125 0.030 16
    }
}

function Add-PressureTank {
    param([string]$Prefix, [double]$Cx, [double]$Cy, [double]$Cz, [string]$Axis = "Y", [double]$Length = 0.62, [double]$Radius = 0.105)
    Add-Cylinder "$($Prefix)_danger_red_pressure_tank_shell" "MAT_ERB_PressureTankRed" $Axis $Cx $Cy $Cz $Radius $Length 14
    if ($Axis -eq "Y") {
        Add-Cylinder "$($Prefix)_lower_aged_brass_tank_band" "MAT_ERB_AgedBrass" "Y" $Cx ($Cy - $Length * 0.28) $Cz ($Radius * 1.06) 0.030 14
        Add-Cylinder "$($Prefix)_upper_aged_brass_tank_band" "MAT_ERB_AgedBrass" "Y" $Cx ($Cy + $Length * 0.28) $Cz ($Radius * 1.06) 0.030 14
    }
    elseif ($Axis -eq "X") {
        Add-Cylinder "$($Prefix)_left_aged_brass_tank_band" "MAT_ERB_AgedBrass" "X" ($Cx - $Length * 0.28) $Cy $Cz ($Radius * 1.06) 0.030 14
        Add-Cylinder "$($Prefix)_right_aged_brass_tank_band" "MAT_ERB_AgedBrass" "X" ($Cx + $Length * 0.28) $Cy $Cz ($Radius * 1.06) 0.030 14
    }
    else {
        Add-Cylinder "$($Prefix)_rear_aged_brass_tank_band" "MAT_ERB_AgedBrass" "Z" $Cx $Cy ($Cz - $Length * 0.28) ($Radius * 1.06) 0.030 14
        Add-Cylinder "$($Prefix)_front_aged_brass_tank_band" "MAT_ERB_AgedBrass" "Z" $Cx $Cy ($Cz + $Length * 0.28) ($Radius * 1.06) 0.030 14
    }
}

function Add-PistonLeg {
    param([string]$Side, [double]$X, [double]$HipY = 0.78, [double]$FootZ = 0.02)
    Add-Cylinder "$($Side)_upper_blackened_piston_leg" "MAT_ERB_BlackenedIron" "Y" $X ($HipY - 0.20) $FootZ 0.055 0.45 10
    Add-Cylinder "$($Side)_lower_copper_piston_leg" "MAT_ERB_CopperPipe" "Y" $X 0.28 $FootZ 0.040 0.45 10
    Add-Cylinder "$($Side)_aged_brass_knee_socket" "MAT_ERB_AgedBrass" "Y" $X 0.50 $FootZ 0.085 0.070 12
    Add-Box "$($Side)_broad_blackened_iron_foot" "MAT_ERB_BlackenedIron" $X 0.055 ($FootZ + 0.06) 0.30 0.11 0.42
}

function Build-Scrapper {
    Start-Obj "ENEMY_ERB_Scrapper_ReadabilityUpgrade_LOD0"
    Add-Cylinder "scrapper_hunched_blackened_boiler_torso" "MAT_ERB_BlackenedIron" "Y" 0 0.88 0.02 0.335 0.74 18
    Add-Cylinder "scrapper_lower_aged_brass_boiler_band" "MAT_ERB_AgedBrass" "Y" 0 0.52 0.02 0.360 0.060 18
    Add-Cylinder "scrapper_upper_aged_brass_boiler_band" "MAT_ERB_AgedBrass" "Y" 0 1.21 0.02 0.365 0.070 18
    Add-Lamp "scrapper_chest_weak_point_lamp" 0 0.91 0.345 0.095
    Add-Box "scrapper_cream_enamel_mask_plate" "MAT_ERB_CreamEnamel" 0 1.36 0.275 0.46 0.25 0.15
    Add-Box "scrapper_soot_black_visor_slit" "MAT_ERB_SootShadow" 0 1.385 0.360 0.34 0.055 0.035
    Add-FurnaceEyePair "scrapper" 1.385 0.382 0.105 0.032
    Add-PressureTank "scrapper_left_backpack" -0.19 0.91 -0.375 "Y" 0.66 0.095
    Add-PressureTank "scrapper_right_backpack" 0.19 0.91 -0.375 "Y" 0.66 0.095
    Add-Cylinder "scrapper_left_upper_hammer_arm" "MAT_ERB_BlackenedIron" "X" -0.48 0.92 0.075 0.060 0.45 10
    Add-Box "scrapper_left_hammer_head_attack_tell" "MAT_ERB_HazardEnamel" -0.78 0.81 0.215 0.30 0.22 0.30
    Add-Box "scrapper_left_hammer_blackened_striker_face" "MAT_ERB_BlackenedIron" -0.79 0.73 0.405 0.22 0.10 0.08
    Add-Cylinder "scrapper_right_upper_cutter_arm" "MAT_ERB_BlackenedIron" "X" 0.49 0.96 0.095 0.055 0.45 10
    Add-CutterWheel "scrapper_right_arm" 0.79 0.93 0.205
    Add-Cylinder "scrapper_left_forearm_copper_pressure_rod" "MAT_ERB_CopperPipe" "X" -0.62 0.71 0.13 0.035 0.42 8
    Add-Cylinder "scrapper_right_forearm_copper_pressure_rod" "MAT_ERB_CopperPipe" "X" 0.62 0.72 0.13 0.035 0.42 8
    Add-PistonLeg "scrapper_left" -0.22 0.78 0.00
    Add-PistonLeg "scrapper_right" 0.22 0.78 0.00
    Add-Box "scrapper_shutdown_ready_fragment_socket_labels" "MAT_ERB_ShutdownFragmentDim" 0 0.38 -0.185 0.32 0.08 0.08
    return Save-Obj (Join-Path $MeshRoot "ENEMY_ERB_Scrapper_ReadabilityUpgrade_LOD0.obj")
}

function Build-Lancer {
    Start-Obj "ENEMY_ERB_Lancer_ReadabilityUpgrade_LOD0"
    Add-Cylinder "lancer_narrow_blackened_spine_torso" "MAT_ERB_BlackenedIron" "Y" 0 1.19 -0.02 0.220 0.92 16
    Add-Cylinder "lancer_upper_aged_brass_spine_band" "MAT_ERB_AgedBrass" "Y" 0 1.61 -0.02 0.238 0.050 16
    Add-Cylinder "lancer_lower_aged_brass_spine_band" "MAT_ERB_AgedBrass" "Y" 0 0.77 -0.02 0.238 0.050 16
    Add-Box "lancer_cream_enamel_long_mask" "MAT_ERB_CreamEnamel" 0 1.80 0.215 0.34 0.31 0.14
    Add-Box "lancer_dark_visor_needle_slit" "MAT_ERB_SootShadow" 0 1.84 0.300 0.25 0.048 0.030
    Add-FurnaceEyePair "lancer" 1.84 0.322 0.080 0.028
    Add-Lamp "lancer_sternum_weak_point_lamp" 0 1.22 0.255 0.075
    Add-PressureTank "lancer_back_vertical_pressure_tank" 0 1.18 -0.365 "Y" 0.90 0.115
    Add-Cylinder "lancer_main_copper_lance_barrel" "MAT_ERB_CopperPipe" "Z" 0 1.34 1.005 0.060 1.70 14
    Add-Cylinder "lancer_blackened_outer_lance_sleeve" "MAT_ERB_BlackenedIron" "Z" 0 1.34 0.45 0.100 0.36 14
    Add-BoltCoils "lancer_forward_lance" 0 1.34 0.68 0.22 4
    Add-Cylinder "lancer_muzzle_blue_bolt_tell_core" "MAT_ERB_BoltTellBlue" "Z" 0 1.34 1.88 0.095 0.070 16
    Add-Box "lancer_left_shoulder_brace" "MAT_ERB_AgedBrass" -0.27 1.40 0.23 0.32 0.07 0.08
    Add-Box "lancer_right_shoulder_brace" "MAT_ERB_AgedBrass" 0.27 1.40 0.23 0.32 0.07 0.08
    Add-Cylinder "lancer_left_aiming_hose" "MAT_ERB_DarkRubber" "Z" -0.17 1.17 0.38 0.025 0.78 8
    Add-Cylinder "lancer_right_aiming_hose" "MAT_ERB_DarkRubber" "Z" 0.17 1.17 0.38 0.025 0.78 8
    Add-PistonLeg "lancer_left_stilt" -0.20 0.88 -0.03
    Add-PistonLeg "lancer_right_stilt" 0.20 0.88 -0.03
    Add-Box "lancer_recoil_shutdown_fragment_mount" "MAT_ERB_ShutdownFragmentDim" 0 0.63 0.52 0.36 0.09 0.09
    return Save-Obj (Join-Path $MeshRoot "ENEMY_ERB_Lancer_ReadabilityUpgrade_LOD0.obj")
}

function Build-Bulwark {
    Start-Obj "ENEMY_ERB_Bulwark_ReadabilityUpgrade_LOD0"
    Add-Box "bulwark_broad_blackened_iron_core_block" "MAT_ERB_BlackenedIron" 0 1.02 -0.03 0.82 0.98 0.46
    Add-Box "bulwark_front_readable_door_shield_plate" "MAT_ERB_BlackenedIron" 0 1.04 0.335 1.30 1.18 0.135
    Add-Box "bulwark_aged_brass_shield_top_rim" "MAT_ERB_AgedBrass" 0 1.65 0.422 1.36 0.065 0.080
    Add-Box "bulwark_aged_brass_shield_bottom_rim" "MAT_ERB_AgedBrass" 0 0.43 0.422 1.36 0.065 0.080
    Add-Box "bulwark_left_hazard_shield_chevron" "MAT_ERB_HazardEnamel" -0.32 1.07 0.512 0.17 0.86 0.035
    Add-Box "bulwark_right_hazard_shield_chevron" "MAT_ERB_HazardEnamel" 0.32 1.07 0.512 0.17 0.86 0.035
    Add-Lamp "bulwark_left_side_weak_point_lamp" -0.58 1.05 0.505 0.080
    Add-Lamp "bulwark_right_side_weak_point_lamp" 0.58 1.05 0.505 0.080
    Add-Box "bulwark_cream_enamel_brow_mask" "MAT_ERB_CreamEnamel" 0 1.74 0.240 0.58 0.20 0.14
    Add-Box "bulwark_soot_visor_under_brow" "MAT_ERB_SootShadow" 0 1.735 0.326 0.45 0.048 0.030
    Add-FurnaceEyePair "bulwark" 1.735 0.348 0.125 0.032
    Add-PressureTank "bulwark_left_shoulder_pressure_tank" -0.62 1.36 -0.285 "Y" 0.68 0.120
    Add-PressureTank "bulwark_right_shoulder_pressure_tank" 0.62 1.36 -0.285 "Y" 0.68 0.120
    Add-Cylinder "bulwark_right_hammer_arm_blackened_pivot" "MAT_ERB_BlackenedIron" "X" 0.78 1.08 0.08 0.075 0.48 10
    Add-Box "bulwark_right_hammer_head_big_slam_tell" "MAT_ERB_HazardEnamel" 1.04 0.88 0.30 0.34 0.34 0.36
    Add-Cylinder "bulwark_left_shield_piston_brace" "MAT_ERB_CopperPipe" "X" -0.78 0.84 0.22 0.045 0.46 10
    Add-Cylinder "bulwark_right_shield_piston_brace" "MAT_ERB_CopperPipe" "X" 0.78 0.84 0.22 0.045 0.46 10
    Add-PistonLeg "bulwark_left" -0.34 0.75 -0.03
    Add-PistonLeg "bulwark_right" 0.34 0.75 -0.03
    Add-Box "bulwark_breakaway_shield_hinge_fragment_socket" "MAT_ERB_ShutdownFragmentDim" 0 0.36 0.38 0.50 0.09 0.10
    return Save-Obj (Join-Path $MeshRoot "ENEMY_ERB_Bulwark_ReadabilityUpgrade_LOD0.obj")
}

function Build-Warden {
    Start-Obj "ENEMY_ERB_Warden_ReadabilityUpgrade_LOD0"
    Add-Cylinder "warden_tall_blackened_governor_tower_core" "MAT_ERB_BlackenedIron" "Y" 0 1.22 -0.02 0.260 1.36 18
    Add-Cylinder "warden_lower_aged_brass_cage_ring" "MAT_ERB_AgedBrass" "Y" 0 0.62 -0.02 0.315 0.055 18
    Add-Cylinder "warden_mid_aged_brass_cage_ring" "MAT_ERB_AgedBrass" "Y" 0 1.22 -0.02 0.315 0.045 18
    Add-Cylinder "warden_upper_aged_brass_cage_ring" "MAT_ERB_AgedBrass" "Y" 0 1.84 -0.02 0.315 0.055 18
    $ribXs = @(-0.26, -0.13, 0.13, 0.26)
    for ($ribIndex = 0; $ribIndex -lt $ribXs.Count; $ribIndex++) {
        $x = $ribXs[$ribIndex]
        Add-Box "warden_vertical_cage_rib_$('{0:00}' -f $ribIndex)" "MAT_ERB_AgedBrass" $x 1.22 0.255 0.035 1.20 0.050
    }
    Add-Box "warden_cream_command_face_plate" "MAT_ERB_CreamEnamel" 0 1.98 0.225 0.43 0.28 0.13
    Add-Box "warden_soot_command_visor" "MAT_ERB_SootShadow" 0 2.00 0.305 0.30 0.055 0.030
    Add-FurnaceEyePair "warden" 2.00 0.326 0.095 0.030
    Add-Lamp "warden_central_command_weak_point_lamp" 0 1.28 0.325 0.085
    Add-PressureTank "warden_left_crown_pressure_tank" -0.24 2.16 -0.09 "X" 0.34 0.070
    Add-PressureTank "warden_right_crown_pressure_tank" 0.24 2.16 -0.09 "X" 0.34 0.070
    Add-Cylinder "warden_overhead_blackened_bolt_spine" "MAT_ERB_BlackenedIron" "Z" 0 2.18 0.18 0.040 0.62 10
    Add-BoltCoils "warden_overhead" 0 2.18 -0.04 0.16 4
    Add-Cylinder "warden_left_command_hose" "MAT_ERB_DarkRubber" "Y" -0.38 1.20 -0.08 0.035 0.92 8
    Add-Cylinder "warden_right_command_hose" "MAT_ERB_DarkRubber" "Y" 0.38 1.20 -0.08 0.035 0.92 8
    Add-Cylinder "warden_left_rod_arm" "MAT_ERB_CopperPipe" "X" -0.50 1.30 0.09 0.038 0.42 8
    Add-Cylinder "warden_right_rod_arm" "MAT_ERB_CopperPipe" "X" 0.50 1.30 0.09 0.038 0.42 8
    Add-PistonLeg "warden_left_tripod" -0.26 0.68 -0.05
    Add-PistonLeg "warden_right_tripod" 0.26 0.68 -0.05
    Add-Box "warden_rear_shutdown_cage_fragment_socket" "MAT_ERB_ShutdownFragmentDim" 0 0.44 -0.285 0.40 0.08 0.08
    return Save-Obj (Join-Path $MeshRoot "ENEMY_ERB_Warden_ReadabilityUpgrade_LOD0.obj")
}

function Build-Fragments {
    param([string]$Enemy, [string[]]$Specials)
    $objectName = "FRAG_ERB_${Enemy}_ShutdownFragments"
    Start-Obj $objectName
    Add-Box "$($Enemy.ToLowerInvariant())_dimmed_mask_plate_fragment" "MAT_ERB_ShutdownFragmentDim" -0.32 0.08 0.12 0.28 0.06 0.18
    Add-Cylinder "$($Enemy.ToLowerInvariant())_loose_furnace_eye_glass" "MAT_ERB_FurnaceEyeAmber" "Z" -0.03 0.09 0.20 0.045 0.025 10
    Add-Cylinder "$($Enemy.ToLowerInvariant())_broken_weak_point_lamp_lens" "MAT_ERB_WeakPointLamp" "Z" 0.18 0.07 0.16 0.060 0.022 12
    Add-Cylinder "$($Enemy.ToLowerInvariant())_pressure_tank_cap_fragment" "MAT_ERB_AgedBrass" "Y" 0.37 0.08 0.04 0.075 0.055 12
    Add-Box "$($Enemy.ToLowerInvariant())_blackened_chassis_shard" "MAT_ERB_BlackenedIron" 0.04 0.045 -0.14 0.34 0.08 0.14
    Add-Box "$($Enemy.ToLowerInvariant())_red_pressure_tank_label_shard" "MAT_ERB_PressureTankRed" 0.43 0.065 -0.18 0.23 0.08 0.13
    $offset = -0.44
    foreach ($special in $Specials) {
        $mat = "MAT_ERB_ShutdownFragmentDim"
        if ($special -match "bolt") { $mat = "MAT_ERB_BoltTellBlue" }
        elseif ($special -match "cutter|hammer|shield") { $mat = "MAT_ERB_HazardEnamel" }
        Add-Box "$($Enemy.ToLowerInvariant())_$($special)_shutdown_piece" $mat $offset 0.055 -0.22 0.18 0.075 0.12
        $offset += 0.18
    }
    return Save-Obj (Join-Path $MeshRoot "$objectName.obj")
}

function Write-PreviewBoard {
    param(
        [string]$Enemy,
        [string]$Role,
        [string]$Tell,
        [string]$PrimaryWeakPoint,
        [string]$FragmentNote,
        [string]$AssetFileName
    )

    Add-Type -AssemblyName System.Drawing
    $w = 1500
    $h = 950
    $bmp = [System.Drawing.Bitmap]::new($w, $h)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $bg = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 22, 24, 24))
    $panel = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 35, 38, 38))
    $line = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(255, 95, 98, 94), 2)
    $iron = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 42, 43, 41))
    $brass = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 189, 129, 46))
    $cream = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 184, 168, 128))
    $amber = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 255, 156, 30))
    $red = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 148, 24, 18))
    $blue = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 33, 155, 255))
    $text = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 230, 225, 204))
    $muted = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 172, 169, 153))
    $fontTitle = [System.Drawing.Font]::new("Segoe UI", 26, [System.Drawing.FontStyle]::Bold)
    $font = [System.Drawing.Font]::new("Segoe UI", 15, [System.Drawing.FontStyle]::Regular)
    $fontSmall = [System.Drawing.Font]::new("Segoe UI", 12, [System.Drawing.FontStyle]::Regular)

    try {
        $g.FillRectangle($bg, 0, 0, $w, $h)
        $g.DrawString("Enemy Readability Batch - $Enemy", $fontTitle, $text, 42, 32)
        $g.DrawString($Role, $font, $muted, 46, 75)
        $g.FillRectangle($panel, 44, 130, 430, 690)
        $g.FillRectangle($panel, 530, 130, 430, 690)
        $g.FillRectangle($panel, 1016, 130, 430, 690)
        $g.DrawRectangle($line, 44, 130, 430, 690)
        $g.DrawRectangle($line, 530, 130, 430, 690)
        $g.DrawRectangle($line, 1016, 130, 430, 690)
        $g.DrawString("Front read", $font, $text, 66, 148)
        $g.DrawString("Side/weapon read", $font, $text, 552, 148)
        $g.DrawString("Damage read", $font, $text, 1038, 148)

        # Front silhouette proxy.
        $g.FillRectangle($iron, 182, 315, 156, 260)
        $g.FillRectangle($brass, 168, 292, 184, 32)
        $g.FillRectangle($brass, 168, 570, 184, 32)
        $g.FillRectangle($cream, 190, 238, 140, 62)
        $g.FillRectangle($iron, 218, 260, 84, 14)
        $g.FillEllipse($amber, 228, 253, 18, 18)
        $g.FillEllipse($amber, 274, 253, 18, 18)
        $g.FillEllipse($amber, 225, 410, 70, 70)
        $g.FillRectangle($red, 106, 355, 44, 190)
        $g.FillRectangle($red, 370, 355, 44, 190)
        $g.FillRectangle($iron, 128, 610, 84, 44)
        $g.FillRectangle($iron, 308, 610, 84, 44)
        $g.DrawString("furnace eyes", $fontSmall, $muted, 72, 238)
        $g.DrawString("weak lamp", $fontSmall, $muted, 296, 426)
        $g.DrawString("pressure tanks", $fontSmall, $muted, 66, 520)

        # Side/weapon proxy, varied by tell.
        $g.FillRectangle($iron, 650, 315, 125, 250)
        $g.FillRectangle($cream, 674, 248, 92, 56)
        $g.FillEllipse($amber, 690, 262, 18, 18)
        $g.FillEllipse($amber, 732, 262, 18, 18)
        if ($Tell -match "bolt") {
            $g.FillRectangle($brass, 758, 405, 230, 34)
            foreach ($x in @(795, 845, 895, 945)) {
                $g.DrawEllipse([System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(255, 33, 155, 255), 5), $x, 382, 36, 80)
            }
            $g.FillEllipse($blue, 984, 390, 52, 52)
        }
        elseif ($Tell -match "shield") {
            $g.FillRectangle($iron, 768, 270, 150, 320)
            $g.FillRectangle($brass, 752, 255, 182, 18)
            $g.FillRectangle($amber, 806, 398, 48, 48)
        }
        elseif ($Tell -match "hammer") {
            $g.FillRectangle($brass, 762, 416, 165, 26)
            $g.FillRectangle($amber, 918, 358, 88, 120)
        }
        else {
            $g.FillRectangle($brass, 762, 412, 122, 30)
            $g.FillEllipse($amber, 880, 360, 118, 118)
            for ($i = 0; $i -lt 8; $i++) {
                $angle = 2 * [Math]::PI * $i / 8
                $tx = 936 + [Math]::Cos($angle) * 75
                $ty = 419 + [Math]::Sin($angle) * 75
                $g.FillRectangle($amber, [int]$tx, [int]$ty, 24, 12)
            }
        }
        $g.DrawString($Tell, $fontSmall, $muted, 566, 612)

        # Fragment board.
        $g.FillRectangle($iron, 1092, 600, 255, 30)
        $g.FillRectangle($brass, 1112, 540, 96, 26)
        $g.FillEllipse($amber, 1224, 516, 54, 54)
        $g.FillRectangle($red, 1294, 555, 82, 30)
        $g.FillRectangle($blue, 1136, 485, 72, 16)
        $g.DrawString($PrimaryWeakPoint, $fontSmall, $muted, 1040, 235)
        $g.DrawString($FragmentNote, $fontSmall, $muted, 1040, 642)
        $g.DrawString("Material split: iron / brass / copper / enamel / lamps / tanks / tells", $fontSmall, $muted, 48, 858)
        $g.DrawString("Mesh: Assets/_Project/ArtStaging/EnemyReadabilityBatch/Meshes/$AssetFileName", $fontSmall, $muted, 48, 884)

        $assetPath = Join-Path $PreviewRoot "PREVIEW_ERB_${Enemy}_ReadabilityBoard.png"
        $docPath = Join-Path $DocPreviewRoot "PREVIEW_ERB_${Enemy}_ReadabilityBoard.png"
        $bmp.Save($assetPath, [System.Drawing.Imaging.ImageFormat]::Png)
        $bmp.Save($docPath, [System.Drawing.Imaging.ImageFormat]::Png)
        Write-TextureMeta -AssetPath $assetPath
    }
    finally {
        foreach ($resource in @($fontTitle, $font, $fontSmall, $bg, $panel, $line, $iron, $brass, $cream, $amber, $red, $blue, $text, $muted, $g, $bmp)) {
            if ($null -ne $resource) { $resource.Dispose() }
        }
    }
}

function Write-ContactSheet {
    Add-Type -AssemblyName System.Drawing
    $w = 1600
    $h = 1100
    $bmp = [System.Drawing.Bitmap]::new($w, $h)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $bg = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 24, 25, 24))
    $text = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 232, 226, 204))
    $muted = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 171, 168, 150))
    $fontTitle = [System.Drawing.Font]::new("Segoe UI", 28, [System.Drawing.FontStyle]::Bold)
    $font = [System.Drawing.Font]::new("Segoe UI", 16, [System.Drawing.FontStyle]::Regular)
    try {
        $g.FillRectangle($bg, 0, 0, $w, $h)
        $g.DrawString("Brassworks Breach - Enemy Readability Batch Contact Sheet", $fontTitle, $text, 42, 34)
        $g.DrawString("Staged silhouettes for Scrapper, Lancer, Bulwark, and Warden. No gameplay code, no colliders, no scene builder.", $font, $muted, 46, 82)
        $items = @("Scrapper", "Lancer", "Bulwark", "Warden")
        for ($i = 0; $i -lt $items.Count; $i++) {
            $enemy = $items[$i]
            $source = Join-Path $PreviewRoot "PREVIEW_ERB_${enemy}_ReadabilityBoard.png"
            $img = [System.Drawing.Image]::FromFile($source)
            try {
                $col = $i % 2
                $row = [Math]::Floor($i / 2)
                $x = 54 + $col * 760
                $y = 145 + $row * 455
                $g.DrawImage($img, $x, $y, 700, 395)
                $g.DrawString($enemy, $font, $text, $x, $y + 405)
            }
            finally {
                $img.Dispose()
            }
        }
        $assetPath = Join-Path $PreviewRoot "PREVIEW_ERB_EnemyReadabilityBatch_ContactSheet.png"
        $docPath = Join-Path $DocPreviewRoot "PREVIEW_ERB_EnemyReadabilityBatch_ContactSheet.png"
        $bmp.Save($assetPath, [System.Drawing.Imaging.ImageFormat]::Png)
        $bmp.Save($docPath, [System.Drawing.Imaging.ImageFormat]::Png)
        Write-TextureMeta -AssetPath $assetPath
    }
    finally {
        foreach ($resource in @($fontTitle, $font, $bg, $text, $muted, $g, $bmp)) {
            if ($null -ne $resource) { $resource.Dispose() }
        }
    }
}

function Write-ManifestJson {
    param([hashtable]$Stats)
    $manifest = [ordered]@{
        package = "EnemyReadabilityBatch"
        date = "2026-05-24"
        unityContract = [ordered]@{
            units = "1 Unity unit = 1 meter"
            axis = "+Y up, +Z forward"
            importPath = "Assets/_Project/ArtStaging/EnemyReadabilityBatch"
            generatedGeometry = "OBJ/MTL proxy meshes, Unity Standard .mat proxies, PNG readability boards"
            excluded = @("gameplay code", "scene builder", "colliders", "Blender dependency", "shared docs/material edits")
        }
        materialSet = "Assets/_Project/ArtStaging/EnemyReadabilityBatch/Materials/ENEMY_ERB_ReadabilityProxyMaterials.mtl"
        enemies = @(
            [ordered]@{ id = "Scrapper"; mesh = "Meshes/ENEMY_ERB_Scrapper_ReadabilityUpgrade_LOD0.obj"; fragments = "Meshes/FRAG_ERB_Scrapper_ShutdownFragments.obj"; preview = "Previews/PREVIEW_ERB_Scrapper_ReadabilityBoard.png"; readability = @("furnace eyes", "chest weak-point lamp", "pressure tanks", "cutter tell", "hammer tell", "detached shutdown fragments"); stats = $Stats.Scrapper },
            [ordered]@{ id = "Lancer"; mesh = "Meshes/ENEMY_ERB_Lancer_ReadabilityUpgrade_LOD0.obj"; fragments = "Meshes/FRAG_ERB_Lancer_ShutdownFragments.obj"; preview = "Previews/PREVIEW_ERB_Lancer_ReadabilityBoard.png"; readability = @("furnace eyes", "sternum weak-point lamp", "back pressure tank", "forward bolt coil tell", "muzzle tell", "detached shutdown fragments"); stats = $Stats.Lancer },
            [ordered]@{ id = "Bulwark"; mesh = "Meshes/ENEMY_ERB_Bulwark_ReadabilityUpgrade_LOD0.obj"; fragments = "Meshes/FRAG_ERB_Bulwark_ShutdownFragments.obj"; preview = "Previews/PREVIEW_ERB_Bulwark_ReadabilityBoard.png"; readability = @("furnace brow eyes", "side weak-point lamps", "shoulder pressure tanks", "shield read", "hammer tell", "detached shutdown fragments"); stats = $Stats.Bulwark },
            [ordered]@{ id = "Warden"; mesh = "Meshes/ENEMY_ERB_Warden_ReadabilityUpgrade_LOD0.obj"; fragments = "Meshes/FRAG_ERB_Warden_ShutdownFragments.obj"; preview = "Previews/PREVIEW_ERB_Warden_ReadabilityBoard.png"; readability = @("furnace command eyes", "central command weak-point lamp", "crown pressure tanks", "overhead bolt tell", "cage silhouette", "detached shutdown fragments"); stats = $Stats.Warden }
        )
        materials = $Materials | ForEach-Object { [ordered]@{ name = $_.Name; role = $_.Label } }
    }
    $json = $manifest | ConvertTo-Json -Depth 8
    $assetPath = Join-Path $MetadataRoot "ERB_EnemyReadabilityBatch_Manifest.json"
    $docPath = Join-Path $DocRoot "ERB_EnemyReadabilityBatch_Manifest.json"
    Write-TextFile -Path $assetPath -Content $json
    Write-TextFile -Path $docPath -Content $json
    Write-DefaultMeta -AssetPath $assetPath
}

Write-FolderMeta (Join-Path $AssetRoot "Meshes")
Write-FolderMeta (Join-Path $AssetRoot "Materials")
Write-FolderMeta (Join-Path $AssetRoot "Previews")
Write-FolderMeta (Join-Path $AssetRoot "Metadata")

Write-MaterialAssets

$stats = @{}
$stats.Scrapper = Build-Scrapper
$stats.Lancer = Build-Lancer
$stats.Bulwark = Build-Bulwark
$stats.Warden = Build-Warden
$stats.ScrapperFragments = Build-Fragments "Scrapper" @("cutter_tooth", "hammer_face")
$stats.LancerFragments = Build-Fragments "Lancer" @("bolt_coil", "muzzle_sleeve")
$stats.BulwarkFragments = Build-Fragments "Bulwark" @("shield_hinge", "hammer_face")
$stats.WardenFragments = Build-Fragments "Warden" @("bolt_coil", "cage_rib")

Write-PreviewBoard "Scrapper" "compact melee worker-machine: readable low mass, dangerous arms" "cutter + hammer tells" "front chest weak-point lamp plus paired furnace eyes" "loose cutter teeth, tank cap, lamp lens, mask shard" "ENEMY_ERB_Scrapper_ReadabilityUpgrade_LOD0.obj"
Write-PreviewBoard "Lancer" "tall ranged automaton: thin body, long forward weapon line" "forward bolt coil tell" "sternum weak-point lamp plus muzzle charge cue" "bolt coil, muzzle sleeve, tank cap, lamp lens" "ENEMY_ERB_Lancer_ReadabilityUpgrade_LOD0.obj"
Write-PreviewBoard "Bulwark" "broad defender: shield-door mass with side lamp reads" "shield + hammer tells" "side weak-point lamps and furnace brow eyes" "shield hinge, hammer face, tank cap, mask shard" "ENEMY_ERB_Bulwark_ReadabilityUpgrade_LOD0.obj"
Write-PreviewBoard "Warden" "tall command unit: cage/tower read and overhead charge language" "overhead bolt tell" "central command weak-point lamp and crown pressure tanks" "cage rib, bolt coil, lamp lens, tank cap" "ENEMY_ERB_Warden_ReadabilityUpgrade_LOD0.obj"
Write-ContactSheet
Write-ManifestJson $stats

Write-Host "Generated EnemyReadabilityBatch staged assets."
Write-Host "Asset root: $AssetRoot"
Write-Host "Doc root: $DocRoot"
