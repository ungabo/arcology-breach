# Brassworks Breach Weapon Mechanisms Set 04

Isolated Unity sidecar package for steampunk first-person weapon mechanism lookdev.

The package is intentionally visual-only. It contains generated prefab candidates, materials, reusable meshes, a runtime catalog, and a package-local manifest. It does not include gameplay scripts, colliders, rigidbodies, autonomous audio, rigging, inventory logic, damage logic, or main-project integration.

## Generation

Use the isolated validation project:

```text
Unity.exe -batchmode -quit -projectPath AssetPacks/BrassworksBreach.WeaponMechanismsSet04/ValidationProject~ -executeMethod BrassworksBreach.Sidecars.WeaponMechanismsSet04.WeaponMechanismsSet04Generator.GenerateValidateAndQuit
```

Preview renders are written outside the package under:

```text
Documentation/ConceptRenders/V0_1_45_WeaponMechanismsSet04
```

Production evidence is written under:

```text
Documentation/AssetProduction/V0_1_45_WeaponMechanismsSet04
```
