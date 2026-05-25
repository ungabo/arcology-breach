# DV10 Import Readiness Plan

## Package

- Package name: `com.brassworks.sidecar.door-vault-set10`
- Local package root: `AssetPacks/BrassworksBreach.DoorVaultSet10`
- Recommended intake: add as local package in a quarantine integration branch, review previews, then promote selected visual prefabs.

## Import Notes

- Treat all prefabs as visual modules only.
- Do not infer collision, door motion, interaction, lock state, lighting, audio, or gameplay authority from this package.
- Candidate assembly is a composition preview for art direction and placement discussion.
- Amber side lamps provide emissive material color only; any real Light components belong in scene or gameplay integration work.
- Preview PNGs are documentation evidence and should not be imported as runtime textures.

## Rollback

Remove the local package reference `com.brassworks.sidecar.door-vault-set10` and delete the isolated package root if quarantine review rejects the asset.
