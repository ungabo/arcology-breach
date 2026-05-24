# Brassworks Breach Weapon Viewmodel Set 03 v0.1.41

This is a self-contained Unity Package Manager sidecar package for high-detail steampunk FPS weapon/viewmodel lookdev. It is visual-only and does not integrate gameplay, input, damage, inventory, pickup logic, autonomous audio, or runtime authority.

## Visual Target

Set 03 focuses on first-person weapon readability and modular construction: pressure pistol full assemblies, scattergun and bolt thrower assemblies, coil/barrel/tank modules, triple gauges, muzzle brakes, triggers, walnut and leather grips, glove silhouette tests, shell and pressure-cell ammo props, and fastener/plate sample boards.

The material pass includes aged brass, smoked brass, oxidized copper, blackened iron, edge-worn gunmetal, oily steel, blued spring steel, varnished walnut, worn leather, dark glove leather, linen wrap, red wax, amber pressure glass, and green gauge glass.

## Included Generator

After importing this package in an isolated Unity project, use:

- `Brassworks Breach/Sidecar Packs/Generate Weapon Viewmodel Set 03 v0.1.41`
- `Brassworks Breach/Sidecar Packs/Render Weapon Viewmodel Set 03 Previews v0.1.41`
- `Brassworks Breach/Sidecar Packs/Validate Weapon Viewmodel Set 03 v0.1.41`

The generator creates real Unity `.mat`, `.asset`, and `.prefab` files under this package's `Runtime` folders. The renderer creates PNG previews under `Documentation/ConceptRenders/V0_1_41_WeaponViewmodelSet03` in the repository root when the package is kept under `AssetPacks`.

## Runtime Outputs

Generated prefabs: 20 visual-only prefabs under `Runtime/Prefabs`.

Generated materials: 14 package-local material assets under `Runtime/Materials`.

Generated reusable meshes: 7 package-local mesh assets under `Runtime/Meshes`.

## Runtime Safety

Generated prefabs intentionally omit colliders, audio sources, particle systems, gameplay scripts, rigidbodies, and autonomous behavior. The only component script is `WeaponViewmodelSet03Identity`, a passive metadata marker used by package validation and intake review.

## Import Rule

Do not add this package directly to the main game until it passes isolated import, generator run, preview render pass, package validation, and art intake review. This package is a candidate library, not a gameplay integration.
