# Steam Corridor Dressing Set 09 QA Checklist

## Static File Checks

- `package.json` parses as JSON and declares `com.brassworks.sidecar.steam-corridor-dressing-set09`.
- Runtime metadata catalog parses as JSON.
- Package manifest mirror parses as JSON.
- Runtime assembly definition and editor assembly definition parse as JSON.
- The catalog contains at least 12 piece definitions.
- The catalog groups pieces into wall, floor, ceiling, and doorway families.
- No files are written outside the approved Set09 package/documentation roots.
- No main `Packages/manifest.json`, `Assets/_Project`, scene, or existing documentation files are modified by this sidecar slice.

## Unity Import Checks

1. Import the package in a quarantine project.
2. Confirm no compile errors in `BrassworksBreach.SteamCorridorDressingSet09` or `.Editor`.
3. Run the generator menu item.
4. Confirm `Runtime/Generated` is created inside the package only.
5. Confirm 12 material assets, 6 mesh assets, 20 piece prefabs, and 1 palette prefab are generated.
6. Inspect generated prefabs for forbidden components.
7. Confirm generated prefabs contain only `Transform`, `MeshFilter`, and `MeshRenderer` components.

## Visual Acceptance

- Wall pieces clearly read as brass/copper pipework, pressure gauges, tanks, valves, gaslight, and cable brackets.
- Floor pieces clearly read as wet drains, grates, steam vents, handrails, and inspection hatches.
- Ceiling pieces clearly read as pipe clusters, condensate trays, lamp cages, cable looms, and steam nozzles.
- Doorway pieces clearly read as riveted headers, pressure locks, threshold drains, and side tanks.
- Wet black surfaces remain dark and glossy without becoming flat gray.
- Brass, copper, and iron materials stay visually distinct under warm corridor light.
- Amber gaslight and red valve/gauge accents remain readable but do not imply gameplay signaling by default.

## Promotion Gate

Only promote generated assets after quarantine import, prefab component inspection, and one temporary review scene pass.
