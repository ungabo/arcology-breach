# CAML10 Corridor Assembly Lookdev Plan

## Scope

Create a render-only Unity lookdev package under `Documentation/AssetProduction/V0_1_55_CorridorAssemblyLookdev10` and write final concept renders under `Documentation/ConceptRenders/V0_1_55_CorridorAssemblyLookdev10`.

## Input Families

- `com.brassworks.sidecar.room-shell-set07`: accepted shell/corridor modules, read-only package reference.
- `com.brassworks.sidecar.hero-room-render-set07`: prior pressure-door/corridor beauty reference, read-only package reference.
- `com.brassworks.sidecar.steam-corridor-dressing-set09`: completed corridor dressing palette, read-only package reference.
- `com.brassworks.sidecar.gaslight-pipe-dressing-set10`: validated gaslight/pipe fixture family, read-only package reference.
- `com.brassworks.sidecar.room-material-set10`: static-ready dark wet masonry material family, read-only package reference.
- `Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png`: view-only north-star comparison target.

## Isolation Rules

- No main project scene edits.
- No edits to the main `Packages/manifest.json`.
- No edits to `Assets/_Project`, shared scripts, or roomtest files.
- Unity only. No Blender or external DCC.
- The nested Unity project may generate temporary `Library`, `Logs`, and `Temp` folders inside the assigned `AssetProduction` root only.

## Planned Outputs

- Three 1920x1080 PNG concept renders.
- One PNG contact sheet.
- One render manifest and asset assembly note.
- One blunt QA comparison report naming passes, failures, and the next package to revise.
