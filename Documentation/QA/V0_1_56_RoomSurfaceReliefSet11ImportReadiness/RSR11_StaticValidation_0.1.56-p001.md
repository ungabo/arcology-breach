# RSR11 Static Validation 0.1.56-p001

Status: PASS_STATIC_READY

- JSON parsed: 5/5
- Runtime prefabs: 28
- Materials: 14
- Mesh assets: 17
- Runtime texture PNGs: 56
- Preview PNGs checked nonblank: 59/59
- Visual-only prefab scans passed: 28/28
- External DCC artifacts: 0
- Package files including meta: 326

## Blunt Target Comparison

Surface relief is real geometry now, not a flat material board. Wetness is stronger through high-smoothness flagstone maps and helper cards, but it still depends on integration lighting and probes. Mortar darkness is intentionally close to the target. Brick randomness is improved with jitter and variant modules, though long playable corridors still need variant mixing and hand placement. Ceiling and floor scales are separated: small ceiling bricks, larger floor slabs. Remaining gaps are scene lighting, collision, GI/probes, runtime decals, and bespoke hero damage.
