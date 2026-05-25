# V0.1.53 Acceptance Checklist

Use this checklist when the main-lane v0.1.53 batch binds playable materials and polishes Level02-Level04 route modules.

## Shared Gates

| Gate | Pass Condition |
| --- | --- |
| Scope | Changes under review are limited to intended v0.1.53 material binding, Level02-Level04 route polish, and directly required QA/test updates. |
| Route roots | `ROUTE_L02_PressureBypass_v0_1_50`, `ROUTE_L03_FoundryGantry_v0_1_50`, and `ROUTE_L04_ObservatoryPumpworks_v0_1_50` remain present with `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, and `VISUALONLY_*` containers. |
| Naming stability | Required v0.1.50/v0.1.51 route object names are preserved unless a reviewed migration note maps old names to new names. |
| Collision ownership | Player-blocking collision remains only on `GEO_`, `COL_`, `TRG_`, or `AUTH_` objects. `VIS_` and `VISUALONLY_` objects remain non-blocking. |
| Traversal | Each Level02-Level04 route can be completed from module entry to rejoin without crouch, jump, physics exploit, console command, or secret-only path. |
| Material binding | Final-candidate SMD08 materials bind without missing shader, pink material, missing texture, or console import errors. |
| Readability | Dark/wet/oily materials do not hide doors, pickups, hazards, enemies, objective prompts, or route exits in normal lighting. |
| Performance | Added material polish and route dressing stay inside the existing route budgets unless a profiler note explains the exception. |

## Material Binding Gates

| Area | Required Result |
| --- | --- |
| Core surfaces | Wet black stone, chipped black iron, worn brass, oxidized copper, scorched furnace metal, gauge enamel, amber glass, and riveted trim appear on representative surfaces. |
| Hazard language | `SMD08_MAT_RedPressureEnamel` and related red enamel accents are reserved for pressure warnings, gate strips, hazard caps, valve warning rims, or weapon details. |
| Wet/oily surfaces | `SMD08_MAT_BlackOilWetFloor` and puddle-like candidates are used in patches, not broad navigation-critical floors. |
| Overlay candidates | `SMD08_MAT_SootDepositOverlay` and `SMD08_MAT_VerticalGrimeStreakOverlay` are held unless the batch includes an approved decal/transparent shader path. |
| Glass candidates | Amber and smoked gauge glass are checked in-scene for transparency/smoothness expectations; lack of true glow is acceptable only if lighting/VFX are explicitly deferred. |
| Texture channels | Each bound material keeps `ALB`, `NRM`, `RMA`, and `GRM` references where present in SMD08. |

## Per-Level Route Gates

| Level | Pass Condition |
| --- | --- |
| Level02 Pressure Bypass | Both bypass valves gate `AUTH_L02_Door_PumpRoomExit`; shortcut pipe gate cannot strand the player; steam jet and pump vent are readable before damage; peak active enemies stay at or below `7`. |
| Level03 Foundry Gantry | Coolant valve visibly changes furnace strip state; lift or lift override cannot trap the player; catwalk collision is readable; peak active enemies stay at or below `9`. |
| Level04 Observatory Pumpworks | Pressure key gates keyed maintenance door; pump reroute visibly changes arena pressure before the arena peak; vertical arena has two readable routes between lower floor and upper deck; peak active enemies stay at or below `10`. |

## Signoff Hold Conditions

- Any required route root or required route object is missing.
- A visual-only object blocks player, projectile, enemy, pickup, or interaction traces.
- Material binding causes pink/missing materials, missing textures, or sustained console errors.
- Dark/wet material binding makes a required route, pickup, hazard read, enemy, or objective prompt materially less readable.
- Any route can soft-lock, strand, or require the optional secret path to complete.
