# CAML10 Asset Assembly Notes

- Unity version: 6000.4.6f1
- North-star reference: `Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png`
- Method: in-memory Unity editor scenes, Camera.Render, PNG export.
- Input packages are local package references and remain unmodified.

## Package Families Used

- `com.brassworks.sidecar.room-shell-set07`: corridor bays, bend, vault/shortcut frames.
- `com.brassworks.sidecar.hero-room-render-set07`: prior pressure-door/corridor render reference only; the assembly script does not modify or rely on its scenes.
- `com.brassworks.sidecar.steam-corridor-dressing-set09`: pipe, gauge, tank, valve, floor/drain and doorway dressing.
- `com.brassworks.sidecar.gaslight-pipe-dressing-set10`: gaslights, reflection helpers, plaques.
- `com.brassworks.sidecar.room-material-set10`: dark wet brick, sooted ceiling, wet flagstone, black mortar staging materials.

## Render Outputs

- `Documentation/ConceptRenders/V0_1_55_CorridorAssemblyLookdev10/CAML10_RENDER_01_corridor_assembly_hero.png`: Accepted-family corridor hero; 26 package prefab instances, 193 Unity staging primitives.
- `Documentation/ConceptRenders/V0_1_55_CorridorAssemblyLookdev10/CAML10_RENDER_02_corner_service_density.png`: Corner / service density assembly; 9 package prefab instances, 171 Unity staging primitives.
- `Documentation/ConceptRenders/V0_1_55_CorridorAssemblyLookdev10/CAML10_RENDER_03_material_fixture_detail.png`: Material and fixture close review; 5 package prefab instances, 28 Unity staging primitives.
- `Documentation/ConceptRenders/V0_1_55_CorridorAssemblyLookdev10/CAML10_CONTACTSHEET_corridor_assembly.png`: contact sheet.
