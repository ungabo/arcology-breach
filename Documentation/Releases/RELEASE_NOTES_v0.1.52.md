# Brassworks Breach v0.1.52 Acceptance Notes

Date: 2026-05-24

## Overview

`v0.1.52` is a non-playable sidecar/readiness acceptance slice. It does not create a new Windows executable; the latest playable build remains `v0.1.51`.

## What Changed

- Accepted `BrassworksBreach.SurfaceMaterialDetailSet08` as a quarantined material package.
- Normalized the Set08 package-local manifest to the shared sidecar schema.
- Added route-expansion tuning guidance for Level02 pressure bypass, Level03 foundry gantry, and Level04 observatory pumpworks.
- Accepted Meitner's Unity-only weapon assembly lookdev replacement as quarantined visual evidence for the pressure-pistol art direction.
- Kept Set08 out of the main Unity manifest and active gameplay scenes until a separate material-binding slice owns placement and validation.

## Set08 Contents

- 24 Unity `.mat` materials.
- 96 package-local texture PNGs: albedo, normal, roughness/metallic/ao intent, and grime/edgewear masks.
- 20 procedural material-board PNGs for review.
- Package manifests, catalogs, production reports, import-readiness notes, and QA validation evidence.

## Verification

- `Tools/SidecarValidation/Test-SidecarAssetPacks.ps1 -PackageNamePattern 'BrassworksBreach.SurfaceMaterialDetailSet08'` passed with 0 errors and 0 warnings after manifest normalization.
- Set08 JSON evidence parsed successfully.
- Route tuning packet JSON parsed successfully.
- Weapon lookdev QA manifest parsed successfully.
- Weapon lookdev render log contains `UNITY_WEAPON_ASSEMBLY_LOOKDEV_RENDERED`.

## Notes

Set08 is accepted with limitations. The strongest future binding candidates are wet black stone, chipped black iron, worn brass pipe, oxidized copper coil, red pressure enamel, amber gaslight glass, black oil floor, scorched furnace metal, gauge enamel, and riveted trim. Overlay/grime materials need a transparent or decal shader path, and cracked black rubber gasket remains placeholder quality.

The Set08 preview boards are procedural documentation boards, not final Unity-rendered scene captures.

The Unity weapon lookdev pass is accepted with a `Revise` decision. Carry forward its dark iron receiver, aged brass trim, exposed copper coil, amber pressure glass, gauge/dial language, and muzzle crown direction; do not import its isolated blockout geometry into gameplay.
