# Surface Material Detail Set 08 Import Readiness Plan

## Recommended Flow

1. Import `AssetPacks/BrassworksBreach.SurfaceMaterialDetailSet08` into a quarantine Unity project or package test project.
2. Verify all `.mat` files load under the built-in Standard shader and retain texture references.
3. Place representative planes, cubes, cylinders, pipes, and gauge discs in a temporary review scene.
4. Compare against previews in `Documentation/ConceptRenders/V0_1_52_SurfaceMaterialDetailSet08`.
5. Approve only the final-candidate materials first; candidate overlays require decal/transparent shader decisions.
6. Promote selected materials into production content through the normal asset gate.

## Integration Priorities

- Highest impact: wet black stone, chipped black iron panels, riveted brass trim, worn brass pipe, oxidized copper coil, oily floor, scorched furnace metal, amber glass, and gauge face enamel.
- Keep red enamel to pressure/hazard affordances, not broad walls.
- Use wet/oily materials in patches so navigation contrast stays readable.
