# Brassworks Breach Steam VFX Set 02

Self-contained sidecar package for visual-only steampunk FPS effects.

## Scope

- Steam vents, floor bursts, pressure leaks, pressure rings.
- Sparks, ricochets, muzzle flashes, furnace belches, embers.
- Valve release/lock visuals and boss-phase pressure escalation visuals.
- No gameplay authority, no autonomous audio, no colliders, no rigidbodies, and no scene edits.

## Integration Notes

All prefabs are authored under `Runtime/Prefabs` and use only package-local materials, procedural mesh assets, Unity `ParticleSystem` components, renderers, transforms, and a passive metadata component. Primary gameplay integration should instantiate and own lifetime externally.
