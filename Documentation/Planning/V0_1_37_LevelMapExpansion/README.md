# v0.1.37 Level Map Expansion

Created: `2026-05-24`

This planning lane expands the level-map and scale rules needed for the Steamworks Level Kit sidecar. The goal is to let parallel level-kit workers build useful modules now without waiting for final map art or final encounter scripting.

## Package Link

Primary sidecar output: `AssetPacks/BrassworksBreach.SteamworksLevelKit`

## Design Intent

Brassworks Breach levels should feel like a pressure-locked underground steamworks: compact dungeon-crawler readability, industrial vertical mass, oily stone floors, soot brick walls, brass/copper machinery, vault doors, gauges, valves, catwalks, and dangerous boiler heat. Modules should snap fast enough for batch level assembly while preserving a first-person combat footprint.

## Immediate Use

- Use `4m` corridor modules for main flow.
- Use wall-bay modules for landmarks and route-state reads.
- Use vault/pressure frames at transitions and blocked routes.
- Use catwalks and railings for perimeter identity, not player snag hazards.
- Use smoke anchors only where steam will not occlude enemy attack tells.
