# RMS10 Import Readiness Notes 0.1.55-p001

Generated: 2026-05-25T00:40:38Z

## Intake Recommendation

Verdict: STATIC READY WITH LIMITATIONS.

Import through a quarantine Unity project before adding a local package reference to the primary project. The package is isolated and visual/material-only, but the two transparent overlay materials need render-path verification before production use.

## Required Quarantine Checks

- Add local package reference to AssetPacks/BrassworksBreach.RoomMaterialSet10.
- Confirm all 6 .mat assets import with built-in Standard shader references.
- Confirm all 30 PNG maps import at 512x512 with expected sRGB/linear/normal settings.
- Apply wall, ceiling, and floor materials to a roomtest-derived board. Wall and ceiling brick should tile smaller than floor flagstones.
- Test RMS10_MAT_EdgeDampnessOverlay and RMS10_MAT_SootDecalOverlay with the primary transparent/decal strategy.

## Promotion Order

1. RMS10_MAT_DarkWetBrickWall
2. RMS10_MAT_SootedBrickCeiling
3. RMS10_MAT_WetUnevenFlagstoneFloor
4. RMS10_MAT_BlackMortarGrime
5. Overlay candidates after transparency review.

## Rollback

Remove the local package reference com.brassworks.sidecar.room-material-set10, then delete AssetPacks/BrassworksBreach.RoomMaterialSet10.