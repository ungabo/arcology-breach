# Brassworks Breach - Route Audit v0.1.33

Build target: `v0.1.33`

Purpose: deterministic scene inspection for the current five-level Windows route. This supplements, but does not replace, a human feel/playability pass.

## Scene Route Matrix

| Scene | Core Route Objects | Enemies | Pickups | Hazards | Secrets | Transition / Exit | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Level01 | Gate:yes Lift:yes | S:4 L:0 B:0 N:0 W:0 | H:2 A:2 K:1 W:0 | Steam:0 Furnace:0 | 1 | to Level02 open | Plaques:1 Spawn->gate 22.5m Spawn->lift 34.6m |
| Level02 | Valve:yes Lift:yes | S:1 L:1 B:0 N:0 W:0 | H:2 A:2 K:0 W:0 | Steam:0 Furnace:0 | 1 | to Level03 locked | Plaques:1 Spawn->valve 20.6m Spawn->lift 23.2m |
| Level03 | Valve:yes Lift:yes | S:2 L:0 B:0 N:1 W:0 | H:1 A:1 K:0 W:1 | Steam:2 Furnace:0 | 0 | to Level04 locked | Plaques:1 Spawn->valve 18.3m Spawn->lift 24.3m |
| Level04 | Emergency lift:yes | S:2 L:1 B:1 N:0 W:0 | H:2 A:2 K:0 W:0 | Steam:2 Furnace:2 | 1 | to Level05 open | Plaques:1 Spawn->lift 28.3m |
| Level05 | Warden:yes Guardian lock:yes Exit:yes | S:1 L:1 B:1 N:0 W:1 | H:1 A:1 K:0 W:0 | Steam:1 Furnace:1 | 0 | final exit locked | Plaques:1 Warden->exit 4.5m Spawn->Warden 24.1m |

## Findings

- No route-blocking scene composition issues were found by the deterministic audit.
- The full automated matrix still remains the source of truth for objective completion, combat, hazards, secrets, settings, and build health.
- Human feel review is still required for movement comfort, encounter pacing, audio mix, and final art readability.

## Next Actionable Slices

- `v0.1.31`: promote the next authored gameplay prop component from the asset queue if no accepted manual route note appears.
- `v0.1.32`: continue release-distribution or manual QA hardening based on the next route packet.
- `v0.1.33`: reserve a route-polish slice for any accepted human feel issue from the Windows QA packet.
