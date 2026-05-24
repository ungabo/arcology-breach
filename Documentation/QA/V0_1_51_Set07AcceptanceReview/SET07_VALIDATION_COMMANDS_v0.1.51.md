# Set07 Validation Commands

Run these from PowerShell before staging Set07 packages into the main project.

```powershell
$Project = "D:\__MY APPS\Unity Doom"
$Unity = "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe"
Set-Location $Project
New-Item -ItemType Directory -Force -Path "$Project\Logs" | Out-Null
git status --short
```

## Static Manifest Parse

```powershell
$manifests = @(
  "AssetPacks\BrassworksBreach.WeaponComponentSet07\Documentation~\Manifest\WCS07_WeaponComponentSet07_Manifest_v0.1.51-p001.json",
  "AssetPacks\BrassworksBreach.RoomShellSet07\Documentation~\Manifest\RSS07_RoomShellSet07_Manifest_v0.1.51-p001.json",
  "AssetPacks\BrassworksBreach.MechanicalEnemyPartsSet07\Documentation~\Manifest\MEPS07_MechanicalEnemyPartsSet07_Manifest_v0.1.51-p001.json",
  "AssetPacks\BrassworksBreach.InteriorDressingSet07\Documentation~\Manifest\ID07_InteriorDressingSet07_Manifest_0.1.51-p001.json",
  "AssetPacks\BrassworksBreach.HeroRoomRenderSet07\Documentation~\Manifest\HRS07_HeroRoomRenderSet07_Manifest_v0.1.51-p001.json"
)
$manifests | ForEach-Object {
  Get-Content -Raw $_ | ConvertFrom-Json | Out-Null
  "OK: $_"
}
```

## Runtime Inventory Counts

```powershell
$packages = @(
  @{Id='WCS07'; Root='AssetPacks\BrassworksBreach.WeaponComponentSet07'},
  @{Id='RSS07'; Root='AssetPacks\BrassworksBreach.RoomShellSet07'},
  @{Id='MEPS07'; Root='AssetPacks\BrassworksBreach.MechanicalEnemyPartsSet07'},
  @{Id='ID07'; Root='AssetPacks\BrassworksBreach.InteriorDressingSet07'},
  @{Id='HRS07'; Root='AssetPacks\BrassworksBreach.HeroRoomRenderSet07'}
)
$packages | ForEach-Object {
  $runtime = Get-ChildItem -LiteralPath (Join-Path $_.Root 'Runtime') -Recurse -File
  [pscustomobject]@{
    Id = $_.Id
    Prefabs = ($runtime | Where-Object Extension -eq '.prefab').Count
    Materials = ($runtime | Where-Object Extension -eq '.mat').Count
    Meshes = ($runtime | Where-Object { $_.FullName -match '\\Runtime\\Meshes\\' -and $_.Extension -in @('.asset','.obj') }).Count
    RuntimeTextures = ($runtime | Where-Object { $_.FullName -match '\\Runtime\\Textures\\' -and $_.Extension -in @('.png','.jpg','.jpeg','.tga') }).Count
    RuntimeScripts = ($runtime | Where-Object Extension -eq '.cs').Count
    RuntimeAsmdefs = ($runtime | Where-Object Extension -eq '.asmdef').Count
  }
} | Format-Table -AutoSize
```

## Runtime Meta Coverage

```powershell
$packages | ForEach-Object {
  $missing = @()
  Get-ChildItem -LiteralPath (Join-Path $_.Root 'Runtime') -Recurse -File |
    Where-Object { -not $_.Name.EndsWith('.meta') } |
    ForEach-Object {
      if (-not (Test-Path -LiteralPath ($_.FullName + '.meta'))) { $missing += $_.FullName }
    }
  [pscustomobject]@{ Id = $_.Id; MissingRuntimeMeta = $missing.Count; Sample = ($missing | Select-Object -First 3) -join '; ' }
} | Format-Table -AutoSize
```

## Prefab Forbidden Component Scan

```powershell
$classIds = @{
  '20'='Camera'; '54'='Rigidbody'; '64'='MeshCollider'; '65'='BoxCollider';
  '82'='AudioSource'; '95'='Animator'; '108'='Light'; '114'='MonoBehaviour';
  '135'='SphereCollider'; '136'='CapsuleCollider'; '198'='ParticleSystem'
}
$forbidden = @('Camera','Rigidbody','MeshCollider','BoxCollider','SphereCollider','CapsuleCollider','AudioSource','Animator','MonoBehaviour','ParticleSystem')
$packages | ForEach-Object {
  $counts = @{}
  Get-ChildItem -LiteralPath (Join-Path $_.Root 'Runtime\Prefabs') -Recurse -File -Filter *.prefab | ForEach-Object {
    Select-String -LiteralPath $_.FullName -Pattern '^--- !u!(\d+) &' | ForEach-Object {
      $id = $_.Matches[0].Groups[1].Value
      if ($classIds.ContainsKey($id)) {
        $name = $classIds[$id]
        $counts[$name] = 1 + [int]($counts[$name])
      }
    }
  }
  $bad = 0
  foreach ($name in $forbidden) { $bad += [int]($counts[$name]) }
  [pscustomobject]@{
    Id = $_.Id
    ForbiddenGameplayComponents = $bad
    Lights = [int]($counts['Light'])
    Result = $(if ($bad -eq 0) { 'PASS' } else { 'FAIL' })
  }
} | Format-Table -AutoSize
```

Expected result: forbidden gameplay components should be `0` for all five packages. HRS07 should show `20` lights; treat that as a review warning, not an automatic pass for production.

## Preview Evidence Count

```powershell
$previewRoots = @(
  @{Id='WCS07'; Root='Documentation\ConceptRenders\V0_1_51_WeaponComponentSet07'},
  @{Id='WCS07_Assembly'; Root='Documentation\ConceptRenders\V0_1_51_WeaponComponentSet07_AssemblyLookdev'},
  @{Id='RSS07'; Root='Documentation\ConceptRenders\V0_1_51_RoomShellSet07'},
  @{Id='MEPS07'; Root='Documentation\ConceptRenders\V0_1_51_MechanicalEnemyPartsSet07'},
  @{Id='ID07'; Root='Documentation\ConceptRenders\V0_1_51_InteriorDressingSet07'},
  @{Id='HRS07'; Root='Documentation\ConceptRenders\V0_1_51_HeroRoomRenderSet07'}
)
$previewRoots | ForEach-Object {
  [pscustomobject]@{
    Id = $_.Id
    Pngs = @(Get-ChildItem -LiteralPath $_.Root -Recurse -File -Filter *.png -ErrorAction SilentlyContinue).Count
    Root = $_.Root
  }
} | Format-Table -AutoSize
```

## Unity Import Smoke After PM Adds Local Package References

Only run this in the PM's integration branch after adding local package references to `Packages/manifest.json`.

```json
"com.brassworks.sidecar.weapon-component-set07": "file:../AssetPacks/BrassworksBreach.WeaponComponentSet07",
"com.brassworks.sidecar.room-shell-set07": "file:../AssetPacks/BrassworksBreach.RoomShellSet07",
"com.brassworks.sidecar.mechanical-enemy-parts-set07": "file:../AssetPacks/BrassworksBreach.MechanicalEnemyPartsSet07",
"com.brassworks.sidecar.interior-dressing-set07": "file:../AssetPacks/BrassworksBreach.InteriorDressingSet07",
"com.brassworks.sidecar.hero-room-render-set07": "file:../AssetPacks/BrassworksBreach.HeroRoomRenderSet07"
```

```powershell
& $Unity -batchmode -projectPath $Project -quit -logFile "$Project\Logs\set07-import-smoke.log"
Select-String -Path "$Project\Logs\set07-import-smoke.log" -Pattern "error CS|Exception|Failed|Missing|could not" -CaseSensitive:$false
```

## Main-Lane Validators After Import

The current `SidecarQuarantineImportValidator` does not include these Set07 packages yet. It only proves Set07 after the PM adds Set07 package checks to that validator or runs an equivalent temporary validator in the integration branch.

```powershell
& $Unity -batchmode -projectPath $Project -executeMethod SidecarQuarantineImportValidator.RunValidation -quit -logFile "$Project\Logs\set07-sidecar-quarantine-validator.log"
& $Unity -batchmode -projectPath $Project -executeMethod V0LevelValidator.RunValidation -quit -logFile "$Project\Logs\set07-level-validation.log"
& $Unity -batchmode -projectPath $Project -executeMethod V0SceneBuilder.RunSmokeTest -quit -logFile "$Project\Logs\set07-smoke-test.log"
```

For HRS07, add an explicit human review step after the Unity import smoke: inspect the 20 package-local `Light` components and decide whether to strip, bake, replace, or keep them only in a review scene.
