# GDW10 Import Readiness Plan

## Gate

Status: ready for quarantine import static validation.

## Import Steps

1. Add AssetPacks/BrassworksBreach.GrimeDecalWetnessSet10 as a local Unity package or copy it into the sidecar import staging area.
2. Open a quarantine scene and inspect the 32 prefabs under Runtime/Prefabs.
3. Confirm each prefab renders as a transparent quad and contains only Transform, MeshFilter, and MeshRenderer components.
4. Place families in this order for room finishing: masonry overlays, damp bands, corner grime, soot streaks, edge wear strips, oil/water floor decals, wet reflection helpers.
5. If any target material needs pipeline-specific decal shaders, remap only the materials after import; do not add behavior to this sidecar package.

## Rollback

Remove local package reference com.brassworks.sidecar.grime-decal-wetness-set10 and delete only the GDW10 assigned roots.