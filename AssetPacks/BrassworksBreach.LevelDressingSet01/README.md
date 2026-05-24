# Brassworks Breach Level Dressing Set 01

Unity-only sidecar package for broad steampunk corridor and room density dressing. It is intentionally isolated from gameplay scenes and can be imported later by the primary integration lane.

## Contents

- Runtime prefabs under `Runtime/Prefabs`
- Procedural materials under `Runtime/Materials`
- Reusable generated meshes under `Runtime/Meshes`
- Generated catalog metadata under `Runtime/Metadata`
- Package-local manifest under `Documentation~/Manifest`
- Editor generator under `Editor`

## Unity Menu

- `Brassworks/Sidecars/Level Dressing Set 01 v0.1.40/Generate Package Assets`
- `Brassworks/Sidecars/Level Dressing Set 01 v0.1.40/Render Preview PNGs`
- `Brassworks/Sidecars/Level Dressing Set 01 v0.1.40/Generate and Render Preview`

Preview output defaults to `Documentation/ConceptRenders/V0_1_40_LevelDressingSet01` at the repository root. Set `BRASSWORKS_SCLD_PREVIEW_ROOT` to override that path.

## Integration Notes

This package is a sidecar only. It does not modify primary gameplay scenes, shared scripts, build settings, or the main project package manifest.
