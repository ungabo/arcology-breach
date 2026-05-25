# Concept Replica Pipe Bundle Set14 Implementation Plan

Checkpoint: 2026-05-25 shutdown partial.

## Scope

Pipe Bundle pass B is set up as a Unity-only sidecar package at `AssetPacks/BrassworksBreach.ConceptReplicaPipeBundleSet14/`.

## Current Work Completed

- Cloned the Set13 sidecar package into the owned Set14 package path.
- Renamed package/code identifiers to Set14 / `CRPB14` / `0.1.58-p014`.
- Patched the Unity generator toward pass B priorities:
  - tighter crop composition with heavier bottom horizontal run, left elbow/riser, right paired verticals, and middle wall branch;
  - more collars, flange lips, bolt rings, soot bands, and contact grime;
  - deeper irregular wet masonry backing with black mortar recesses, inset chips, wet reflection slivers, and wall/floor contact shadows;
  - extra subtle steam planes around the crop.

## Not Yet Done

- Unity batch generation has not been run for Set14.
- Root concept renders have not been produced.
- Manifest/catalog JSON has not been regenerated from Set14 code.
- Prefab visual-only scan has not been rerun.

## Next Fix

Create a package-local validation project that references `com.brassworks.sidecar.concept-replica-pipe-bundle-set14`, run `BrassworksBreach.ConceptReplicaPipeBundleSet14.Editor.ConceptReplicaPipeBundleSet14Generator.GenerateAll`, then review the three Unity PNGs before any further geometry iteration.
