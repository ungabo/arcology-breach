# v0.1.37 Sidecar Gate Remediation

Timestamp: 2026-05-24 14:17:33 -04:00

## Scope

Remediated package-gate failures reported by `Tools/SidecarValidation/Test-SidecarAssetPacks.ps1` for the v0.1.37 sidecar asset packs.

Allowed write scopes used:

- `AssetPacks/BrassworksBreach.SteampunkWeapons/**`
- `AssetPacks/BrassworksBreach.MechanicalEnemies/**`
- `AssetPacks/BrassworksBreach.SteamworksLevelKit/Documentation~/**`
- `AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Metadata/**`
- `Documentation/AssetProduction/V0_1_37_SidecarGateRemediation/**`

## Changes

- Added package-local `Documentation~/Manifest` JSON for Steampunk Weapons and Mechanical Enemies.
- Added missing Steampunk Weapons `CHANGELOG.md`.
- Corrected Steampunk Weapons package namespace to `com.brassworks.sidecar.steampunk-weapons` and updated its package-local generator constant plus validation-project package reference.
- Patched the Steamworks Level Kit package-local manifest with `import_smoke_status`, explicit generated asset lists, and a positive package-local GUID collision check.
- Added adjacent `.meta` files for Mechanical Enemies `.gitkeep` placeholders, Steamworks Level Kit runtime metadata JSON, and Steamworks Level Kit `.gitkeep` placeholders.

## Remaining Warnings

No blocking package-shape warnings remain after the primary lane added adjacent `.meta` files for the Steamworks Level Kit `.gitkeep` placeholders.

## Validator Result

Commands:

```powershell
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -PackageNamePattern 'BrassworksBreach.SteampunkWeapons'
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -PackageNamePattern 'BrassworksBreach.MechanicalEnemies'
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -PackageNamePattern 'BrassworksBreach.SteamworksLevelKit'
```

Result: each completed v0.1.37 sidecar package passes with 0 errors and 0 warnings.

Note: a later active sidecar lane may create a new package root before it is complete. During active package generation, validate completed packages by explicit `-PackageNamePattern`; run the default all-package validator once active package workers report completion.
