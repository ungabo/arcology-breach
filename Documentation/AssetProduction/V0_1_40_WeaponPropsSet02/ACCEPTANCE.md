# Weapon Props Set 02 Acceptance Notes

## Package Manifest Notes

- Package root: `AssetPacks/BrassworksBreach.WeaponPropsSet02`
- UPM package name: `com.brassworks.sidecar.weapon-props-set02`
- Package version: `0.1.40`
- Package-local manifest: `AssetPacks/BrassworksBreach.WeaponPropsSet02/Documentation~/Manifest/WPS02_WeaponPropsSet02_Manifest_v0.1.40-p001.json`
- Validation project: `AssetPacks/BrassworksBreach.WeaponPropsSet02/ValidationProject~`

## Acceptance Gates

- Package metadata exists: `package.json`, `README.md`, `CHANGELOG.md`, runtime/editor asmdefs, and `Documentation~/Manifest`.
- Unity generator creates at least 14 prefabs, 12 materials, and 4 reusable meshes.
- Preview renderer creates individual PNGs and a contact sheet in `Documentation/ConceptRenders/V0_1_40_WeaponPropsSet02`.
- Sidecar validation reports zero errors and zero warnings when run package-specifically.
- No gameplay integration or edits outside the approved write scopes.

## Integration Risks

- Prefabs are built from procedural primitive/custom mesh lookdev parts and need authored mesh replacement before final art lock.
- Materials are color/emission lookdev materials and do not include final texture maps, decals, dirt masks, or normal maps.
- First-person candidates still need hand scale, muzzle alignment, recoil clearance, and animation socket review.
- Cabinet, rack, gear-key housing, and pressure-cell canister include no colliders, interaction scripts, prompts, or gameplay binding.
- Dense named subparts are useful for promotion but may need batching or LOD simplification for runtime performance.
