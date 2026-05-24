# Main Lane Validation Checklist v0.1.49

## Cheap Validation

- JSON parse: package manifest, validation-project manifest, and package-local manifest.
- File scope check: changed files must stay inside the five assigned roots.
- Forbidden authority check: no `Animator`, `AnimatorController`, `Rigidbody`, `Collider`, `NavMesh`, gameplay AI, damage, or health authority in sidecar assets.
- Asset count check: at least 20 prefabs, at least 16 materials, and reusable mesh assets present.
- Existing sidecar validator check: run only if the validator already knows this package or after a future validator-extension task.

## Manual Unity Review

- Place one Scrapper, Lancer, Bulwark, Warden, and BossPhase prefab in a quarantine scene.
- Confirm furnace eyes and weak-point markers are visible from the player-facing side.
- Confirm saw arms, lance rails, boiler shields, and command halos read as distinct silhouettes.
- Confirm pose variants are static pose proxies and do not contain animation-controller authority.
