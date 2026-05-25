# GPD10 Import Readiness Plan

## Package

- Package root: $PackageRel
- Version: $Version
- Intent: visual-only fixture dressing for roomtest v0.5 lamp/fixture gap.

## Quarantine Import Steps

1. Import or locally reference $PackageName in a quarantine Unity project.
2. Open Runtime/Prefabs and instantiate representative prefabs from each family.
3. Confirm all render with package materials and built-in primitive meshes.
4. Confirm prefabs have no colliders, rigidbodies, lights, reflection probes, scripts, audio, animation, timeline, or scene dependencies.
5. Test placement on v0.5 wall surfaces: gaslight centered on dark brick, pipe brackets tied to wall plaques, reflection helpers placed near damp floor/wall glints.
6. Promote only selected visual prefabs into main project content after art and QA signoff.

## Boundaries

No changes are required in main Assets, Packages, ProjectSettings, roomtest scenes, or build settings for this sidecar to exist.