# SignageDecalsV1 Manifest

Generated: 2026-05-23T23:45:59-04:00

Scope: staged cross-level signage/decal text package only. No Unity scenes, gameplay scripts, README, BUILD_STATUS, WORK_LEDGER, shared status docs, or FinalMaterialsV1 files were modified.

## Visual Target

- Brass/cream enamel plates for readable objectives.
- Red-orange danger strips for locked, hot, steam, and active combat states.
- Green arrows and plates only for exits, restored routes, and success states.
- Dark iron stencil labels for machinery and short work-order/lore scraps.
- Worker chalk secret marks that read as optional service notes, not objective markers.
- References: `Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png`, `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`, and `Documentation/STEAMPUNK_NORTH_STAR.md`.

## Unity Import Settings

- Texture Type: `Default` for world decal materials; `Sprite (2D and UI)` with `Multiple` slicing is acceptable if an integration pass wants atlas sprites.
- sRGB: on.
- Alpha Is Transparency: on.
- Mip Maps: on for in-world signage/decal quads; disable only for UI-only preview use.
- Filter Mode: `Bilinear`.
- Wrap Mode: `Clamp`.
- Compression: `None` for source review; test high-quality compression after in-game readability checks.
- Max Size: `2048`.
- Use each entry's `source_rect_px` from `SignageDecalsV1_AssetManifest.json` if slicing individual decals from a sheet.

## Generated Sheets

- `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` - 2048x2048 RGBA sheet, documentation copy at `Documentation/AssetProduction/SignageDecalsV1/T_SignageDecalsV1_ObjectivePlates_2048.png`.
- `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` - 2048x2048 RGBA sheet, documentation copy at `Documentation/AssetProduction/SignageDecalsV1/T_SignageDecalsV1_WarningHazardStrips_2048.png`.
- `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` - 2048x2048 RGBA sheet, documentation copy at `Documentation/AssetProduction/SignageDecalsV1/T_SignageDecalsV1_RouteArrowsChevrons_2048.png`.
- `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` - 2048x2048 RGBA sheet, documentation copy at `Documentation/AssetProduction/SignageDecalsV1/T_SignageDecalsV1_StencilMachineryLore_2048.png`.
- `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` - 2048x2048 RGBA sheet, documentation copy at `Documentation/AssetProduction/SignageDecalsV1/T_SignageDecalsV1_SecretServiceMarks_2048.png`.
- `Assets/_Project/ArtStaging/SignageDecalsV1/Previews/PREVIEW_SignageDecalsV1_ContactSheet.png` - contact sheet preview, documentation copy at `Documentation/AssetProduction/SignageDecalsV1/PREVIEW_SignageDecalsV1_ContactSheet.png`.

## Counts

- Total text/decal entries: 85.
- Ready entries: 79.
- Rough entries: 6.
- Rough entries are limited to optional/deferred Level03 gauge-cache and Level05 clerk-void secret marks.

## Category Counts

| Category | Count |
| --- | ---: |
| hazard label | 11 |
| lore decal | 5 |
| machinery label | 15 |
| objective sign | 15 |
| route arrow | 15 |
| secret hint | 15 |
| warning label | 4 |
| work-order decal | 5 |

## Level Counts

| Level | Count |
| --- | ---: |
| Level01 Brassworks Intake | 17 |
| Level02 Pipeworks Annex | 17 |
| Level03 Boilerheart Core | 17 |
| Level04 Furnace Foundry | 17 |
| Level05 Governor Core | 17 |

## Machine-Readable Files

- `Assets/_Project/ArtStaging/SignageDecalsV1/SignageDecalsV1_AssetManifest.json` - full manifest with hashes, import settings, sheet paths, source rectangles, and exact strings.
- `Documentation/AssetProduction/SignageDecalsV1/SignageDecalsV1_AssetManifest.json` - documentation copy of the same JSON.
- `Documentation/AssetProduction/SignageDecalsV1/SIGNAGE_DECALS_V1_COPY_DECK.csv` - copy deck CSV for design review and integration planning.

## Full Copy Deck

| ID | Status | Level | Room | Category | Exact text | Placement intent | Size m | Sheet | Source rect px |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| `OBJ-L01-01` | ready | Level01 Brassworks Intake | Clockwork repair bay approach | objective sign | `GEAR KEY AHEAD` | Amber route plate before the gear-key plinth. | 1.20 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `70,86,581,284` |
| `OBJ-L01-02` | ready | Level01 Brassworks Intake | Pressure gate face | objective sign | `PRESSURE GATE` | Gate identifier centered on the locked pressure gate. | 1.35 x 0.32 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `733,86,581,284` |
| `OBJ-L01-03` | ready | Level01 Brassworks Intake | Service lift alcove | objective sign | `SERVICE LIFT` | Green-side destination plate above the exit lift. | 1.25 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `1396,86,581,284` |
| `OBJ-L02-01` | ready | Level02 Pipeworks Annex | Pipeworks entry | objective sign | `ROUTE PIPE PRESSURE` | Amber plate visible from the Level02 spawn lane. | 1.45 x 0.32 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `70,484,581,284` |
| `OBJ-L02-02` | ready | Level02 Pipeworks Annex | North valve run | objective sign | `ROUTING VALVE` | Label above the required pressure-routing valve. | 1.20 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `733,484,581,284` |
| `OBJ-L02-03` | ready | Level02 Pipeworks Annex | Boilerheart service lift | objective sign | `BOILERHEART LIFT` | Green destination plate for the lift once pressure is routed. | 1.35 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `1396,484,581,284` |
| `OBJ-L03-01` | ready | Level03 Boilerheart Core | Arrival floor | objective sign | `VENT CORE PRESSURE` | Initial amber objective plate pointing toward the core valve. | 1.45 x 0.32 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `70,882,581,284` |
| `OBJ-L03-02` | ready | Level03 Boilerheart Core | Furnace-core chamber | objective sign | `PRESSURE VALVE` | Readable label at the required Boilerheart valve. | 1.25 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `733,882,581,284` |
| `OBJ-L03-03` | ready | Level03 Boilerheart Core | Foundry service lift | objective sign | `FOUNDRY LIFT` | Green lift identifier after pressure is vented. | 1.20 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `1396,882,581,284` |
| `OBJ-L04-01` | ready | Level04 Furnace Foundry | Furnace baffle lane | objective sign | `FURNACE LANE` | Amber plate at the main foundry lane entrance. | 1.20 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `70,1280,581,284` |
| `OBJ-L04-02` | ready | Level04 Furnace Foundry | Quench side bay | objective sign | `QUENCH BAY` | Readable side-route label near coolant/quench props. | 1.10 x 0.28 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `733,1280,581,284` |
| `OBJ-L04-03` | ready | Level04 Furnace Foundry | Emergency hoist end | objective sign | `EMERGENCY HOIST` | Green destination plate above the transition hoist. | 1.35 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `1396,1280,581,284` |
| `OBJ-L05-01` | ready | Level05 Governor Core | Regulator ring entry | objective sign | `REGULATOR RING` | Amber plate naming the central final encounter ring. | 1.25 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `70,1678,581,284` |
| `OBJ-L05-02` | ready | Level05 Governor Core | Warden lock signal | objective sign | `WARDEN LOCK` | Red-orange lock plate beside the denied master hoist state. | 1.15 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `733,1678,581,284` |
| `OBJ-L05-03` | ready | Level05 Governor Core | Master override hoist | objective sign | `MASTER OVERRIDE` | Green final objective plate after Warden defeat. | 1.40 x 0.32 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png` | `1396,1678,581,284` |
| `HAZ-L01-01` | ready | Level01 Brassworks Intake | Pressure lock | warning label | `PRESSURE LOCKED` | Red-orange denial strip beside the first locked gate. | 1.20 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `70,86,581,284` |
| `HAZ-L01-02` | ready | Level01 Brassworks Intake | Steam vent room | hazard label | `STEAM VENT` | Small warning strip near animated vent puffs. | 1.00 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `733,86,581,284` |
| `HAZ-L01-03` | ready | Level01 Brassworks Intake | Pressure gate threshold | hazard label | `GATE CRUSH` | Floor/door-edge warning decal at the gate travel path. | 1.00 x 0.20 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `1396,86,581,284` |
| `HAZ-L02-01` | ready | Level02 Pipeworks Annex | Pipeworks main | hazard label | `HIGH PRESSURE MAIN` | Horizontal strip on the main pipe bundle. | 1.35 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `70,484,581,284` |
| `HAZ-L02-02` | ready | Level02 Pipeworks Annex | Baffle corridor | hazard label | `PIPE BURST RISK` | Warning strip along the baffle corridor wall. | 1.20 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `733,484,581,284` |
| `HAZ-L02-03` | ready | Level02 Pipeworks Annex | North valve run | warning label | `VALVE RUN LIVE` | Active-system warning beside the required valve. | 1.15 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `1396,484,581,284` |
| `HAZ-L03-01` | ready | Level03 Boilerheart Core | Boilerheart furnace leak | hazard label | `FURNACE LEAK` | Red-orange strip next to Boilerheart steam hazard. | 1.10 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `70,882,581,284` |
| `HAZ-L03-02` | ready | Level03 Boilerheart Core | Boilerheart core bleed | hazard label | `CORE BLEED` | Hazard label beside the second linked steam hazard. | 1.00 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `733,882,581,284` |
| `HAZ-L03-03` | ready | Level03 Boilerheart Core | Bellows Node floor | warning label | `PRESSURE PULSE` | Warning strip close to Bellows Node pulse range. | 1.20 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `1396,882,581,284` |
| `HAZ-L04-01` | ready | Level04 Furnace Foundry | Furnace heat lane | hazard label | `HEAT SURGE` | Warning strip at heat-surge lane entry. | 1.00 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `70,1280,581,284` |
| `HAZ-L04-02` | ready | Level04 Furnace Foundry | Pour lane | hazard label | `POUR LANE HOT` | Floor or low wall strip along the active pour lane. | 1.15 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `733,1280,581,284` |
| `HAZ-L04-03` | ready | Level04 Furnace Foundry | Crucible steam bleed | hazard label | `CRUCIBLE BLEED` | Warning strip near foundry steam leak. | 1.20 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `1396,1280,581,284` |
| `HAZ-L05-01` | ready | Level05 Governor Core | Regulator leak | hazard label | `REGULATOR LEAK` | Steam hazard label in the regulator lane. | 1.20 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `70,1678,581,284` |
| `HAZ-L05-02` | ready | Level05 Governor Core | Regulator surge floor | hazard label | `SURGE FLOOR` | Floor strip for pulsing furnace-heat hazard. | 1.00 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `733,1678,581,284` |
| `HAZ-L05-03` | ready | Level05 Governor Core | Warden arena edge | warning label | `WARDEN ACTIVE` | Red-orange encounter-state strip near the final lock. | 1.15 x 0.22 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png` | `1396,1678,581,284` |
| `ARR-L01-01` | ready | Level01 Brassworks Intake | Service entry | route arrow | `TO KEY` | Amber floor/wall arrow toward gear-key plinth. | 0.85 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `70,86,581,284` |
| `ARR-L01-02` | ready | Level01 Brassworks Intake | Repair bay return | route arrow | `TO GATE` | Amber return arrow back to the pressure gate. | 0.85 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `733,86,581,284` |
| `ARR-L01-03` | ready | Level01 Brassworks Intake | Furnace control exit | route arrow | `TO LIFT` | Green arrow pointing to service lift. | 0.85 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `1396,86,581,284` |
| `ARR-L02-01` | ready | Level02 Pipeworks Annex | Pipeworks entry | route arrow | `TO VALVE` | Amber arrow toward north routing valve. | 0.90 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `70,484,581,284` |
| `ARR-L02-02` | ready | Level02 Pipeworks Annex | Valve return | route arrow | `TO LIFT` | Green arrow returning to Boilerheart lift. | 0.85 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `733,484,581,284` |
| `ARR-L02-03` | ready | Level02 Pipeworks Annex | Post-valve route | route arrow | `GREEN SIGNAL` | Small green success arrow near restored lift signal. | 1.00 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `1396,484,581,284` |
| `ARR-L03-01` | ready | Level03 Boilerheart Core | Arrival floor | route arrow | `TO VALVE` | Amber route arrow toward the pressure valve. | 0.90 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `70,882,581,284` |
| `ARR-L03-02` | ready | Level03 Boilerheart Core | Scattergun display | route arrow | `TO TOOL` | Amber local cue for Steam Scattergun pickup route. | 0.85 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `733,882,581,284` |
| `ARR-L03-03` | ready | Level03 Boilerheart Core | Post-valve lift route | route arrow | `TO FOUNDRY` | Green destination arrow after venting. | 1.00 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `1396,882,581,284` |
| `ARR-L04-01` | ready | Level04 Furnace Foundry | Arrival floor | route arrow | `TO FURNACE` | Amber arrow toward the main foundry lane. | 1.00 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `70,1280,581,284` |
| `ARR-L04-02` | ready | Level04 Furnace Foundry | Quench side bay | route arrow | `TO QUENCH` | Amber side-route arrow for quench bay dressing. | 1.00 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `733,1280,581,284` |
| `ARR-L04-03` | ready | Level04 Furnace Foundry | Final foundry lane | route arrow | `TO HOIST` | Green arrow toward emergency hoist. | 0.90 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `1396,1280,581,284` |
| `ARR-L05-01` | ready | Level05 Governor Core | Arrival lane | route arrow | `TO CORE` | Amber arrow into regulator ring. | 0.85 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `70,1678,581,284` |
| `ARR-L05-02` | ready | Level05 Governor Core | Core return | route arrow | `TO OVERRIDE` | Green arrow toward final hoist after Warden defeat. | 1.00 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `733,1678,581,284` |
| `ARR-L05-03` | ready | Level05 Governor Core | Master hoist signal | route arrow | `GREEN HOIST` | Success-state arrow near final lift signal. | 1.00 x 0.30 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png` | `1396,1678,581,284` |
| `MAC-L01-01` | ready | Level01 Brassworks Intake | Copper-pipe maintenance throat | machinery label | `INTAKE PUMP 1` | Stencil on pump housing or nearby wall plate. | 0.90 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `70,86,316,284` |
| `MAC-L01-02` | ready | Level01 Brassworks Intake | Clockwork repair bay | machinery label | `REPAIR BAY` | Small cream label over repair-bay entry. | 0.80 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `468,86,316,284` |
| `MAC-L01-03` | ready | Level01 Brassworks Intake | Pressure gate | machinery label | `GATE DRIVE` | Stencil on pressure-gate gear housing. | 0.80 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `866,86,316,284` |
| `LOR-L01-01` | ready | Level01 Brassworks Intake | Pressure gate maintenance board | work-order decal | `WO-01 CHECK GATE TEETH` | Short work order on a clipboard or enamel board. | 1.25 x 0.20 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `1264,86,316,284` |
| `LOR-L01-02` | ready | Level01 Brassworks Intake | Service entry wall | lore decal | `LOG: INTAKE CREW MISSING` | Small archive-flavor decal near entry benches. | 1.30 x 0.20 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `1662,86,316,284` |
| `MAC-L02-01` | ready | Level02 Pipeworks Annex | Pipeworks main | machinery label | `ANNEX MAIN FEED` | Stencil on the largest pipe run. | 1.05 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `70,484,316,284` |
| `MAC-L02-02` | ready | Level02 Pipeworks Annex | Baffle corridor | machinery label | `BAFFLE CONTROL` | Label on baffle control housing. | 0.95 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `468,484,316,284` |
| `MAC-L02-03` | ready | Level02 Pipeworks Annex | North valve run | machinery label | `VALVE RUN 02` | Stencil next to pressure-routing valve. | 0.90 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `866,484,316,284` |
| `LOR-L02-01` | ready | Level02 Pipeworks Annex | Cold pipe run | work-order decal | `WO-02 PATCH COLD PIPE` | Short work order near the south-west pipe run. | 1.20 x 0.20 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `1264,484,316,284` |
| `LOR-L02-02` | ready | Level02 Pipeworks Annex | Annex checkpoint | lore decal | `LOG: LANCERS SENT BELOW` | Small lore decal in first ranged-machine lane. | 1.25 x 0.20 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `1662,484,316,284` |
| `MAC-L03-01` | ready | Level03 Boilerheart Core | Furnace-core chamber | machinery label | `BOILERHEART CORE` | Stencil around the furnace-core landmark. | 1.05 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `70,882,316,284` |
| `MAC-L03-02` | ready | Level03 Boilerheart Core | Bellows Node alcove | machinery label | `BELLOWS NODE A` | Label on or near the support-machine prototype. | 1.00 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `468,882,316,284` |
| `MAC-L03-03` | ready | Level03 Boilerheart Core | Gauge wall | machinery label | `GAUGE BANK C` | Stencil over the gauge-bank dressing. | 0.90 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `866,882,316,284` |
| `LOR-L03-01` | ready | Level03 Boilerheart Core | Foundry lift lock panel | work-order decal | `WO-03 VENT BEFORE LIFT` | Maintenance note justifying the pressure-locked lift. | 1.25 x 0.20 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `1264,882,316,284` |
| `LOR-L03-02` | ready | Level03 Boilerheart Core | Core wall plaque cluster | lore decal | `LOG: CORE BLEEDS FAST` | Short archive-flavor decal near the core. | 1.20 x 0.20 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `1662,882,316,284` |
| `MAC-L04-01` | ready | Level04 Furnace Foundry | Pour lane | machinery label | `POUR LANE 4` | Stencil beside active furnace lane. | 0.85 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `70,1280,316,284` |
| `MAC-L04-02` | ready | Level04 Furnace Foundry | Quench side bay | machinery label | `SLAG GUTTER` | Label on low gutter dressing. | 0.85 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `468,1280,316,284` |
| `MAC-L04-03` | ready | Level04 Furnace Foundry | Bulwark pressure area | machinery label | `HAMMER BAY` | Stencil near hammer/foundry machinery. | 0.85 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `866,1280,316,284` |
| `LOR-L04-01` | ready | Level04 Furnace Foundry | Foundry maintenance board | work-order decal | `WO-04 CLEAR SLAG GUTTER` | Short work order near slag and quench props. | 1.25 x 0.20 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `1264,1280,316,284` |
| `LOR-L04-02` | ready | Level04 Furnace Foundry | Coal cache vicinity | lore decal | `LOG: COAL DOOR STICKS` | Flavor decal near coal-bin service area. | 1.20 x 0.20 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `1662,1280,316,284` |
| `MAC-L05-01` | ready | Level05 Governor Core | Regulator ring | machinery label | `GOVERNOR CORE` | Stencil on regulator ring wall panels. | 1.00 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `70,1678,316,284` |
| `MAC-L05-02` | ready | Level05 Governor Core | Regulator machinery | machinery label | `REGULATOR DRUM` | Label on drum/pylon dressing. | 1.00 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `468,1678,316,284` |
| `MAC-L05-03` | ready | Level05 Governor Core | Warden lock panel | machinery label | `WARDEN SEAL` | Small lock-state machinery label. | 0.90 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `866,1678,316,284` |
| `LOR-L05-01` | ready | Level05 Governor Core | Master hoist maintenance board | work-order decal | `WO-05 HOLD MASTER HOIST` | Short work order beside final hoist controls. | 1.25 x 0.20 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `1264,1678,316,284` |
| `LOR-L05-02` | ready | Level05 Governor Core | Clerk alcove or archive plate | lore decal | `LOG: WARDEN HEARS KEYS` | Short lore decal for the final lock mythology. | 1.25 x 0.20 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png` | `1662,1678,316,284` |
| `SEC-L01-01` | ready | Level01 Brassworks Intake | Intake pressure cache approach | secret hint | `WARM SEAM` | Worker chalk close to the warm service-panel edge. | 0.45 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `70,74,581,308` |
| `SEC-L01-02` | ready | Level01 Brassworks Intake | Intake pressure cache panel | secret hint | `THREE RIVETS OUT` | Chalk phrase below missing rivets clue. | 0.55 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `733,74,581,308` |
| `SEC-L01-03` | ready | Level01 Brassworks Intake | Intake pressure cache recess | secret hint | `SPARES BEHIND` | Low chalk note near optional resource cache. | 0.55 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `1396,74,581,308` |
| `SEC-L02-01` | ready | Level02 Pipeworks Annex | Pipeworks cartridge cache approach | secret hint | `COLD PIPE` | Worker chalk beside cooler pipe color clue. | 0.45 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `70,472,581,308` |
| `SEC-L02-02` | ready | Level02 Pipeworks Annex | Pipeworks cartridge cache recess | secret hint | `BOLT SHADOWS` | Subtle chalk note near spare bolt marks. | 0.50 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `733,472,581,308` |
| `SEC-L02-03` | ready | Level02 Pipeworks Annex | Pipeworks cartridge cache rack | secret hint | `LOW RACK` | Chalk on floor trim near low pipe-rack cache. | 0.40 x 0.16 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `1396,472,581,308` |
| `SEC-L03-01` | rough | Level03 Boilerheart Core | Optional gauge cache | secret hint | `GAUGE LIES` | Rough optional clue for deferred gauge-cache route. | 0.45 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `70,870,581,308` |
| `SEC-L03-02` | rough | Level03 Boilerheart Core | Optional gauge cache | secret hint | `SHUTTER DRAGS` | Rough optional clue for deferred gauge-shutter cache. | 0.50 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `733,870,581,308` |
| `SEC-L03-03` | rough | Level03 Boilerheart Core | Optional gauge cache | secret hint | `NEEDLE STICKS` | Rough optional clue for deferred gauge-needle cache. | 0.50 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `1396,870,581,308` |
| `SEC-L04-01` | ready | Level04 Furnace Foundry | Foundry coal cache approach | secret hint | `COAL DOOR COOL` | Worker chalk beside cooler quench-pipe clue. | 0.55 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `70,1268,581,308` |
| `SEC-L04-02` | ready | Level04 Furnace Foundry | Foundry coal cache floor | secret hint | `BLACK BOOT MARK` | Chalk phrase near coal footprints. | 0.55 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `733,1268,581,308` |
| `SEC-L04-03` | ready | Level04 Furnace Foundry | Foundry coal cache panel | secret hint | `QUENCH HUSH` | Small service mark beside quiet quench pipe. | 0.50 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `1396,1268,581,308` |
| `SEC-L05-01` | rough | Level05 Governor Core | Optional clerk void | secret hint | `WRONG CLERK TAG` | Rough optional clue for deferred clerk-label secret. | 0.55 x 0.18 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `70,1666,581,308` |
| `SEC-L05-02` | rough | Level05 Governor Core | Optional clerk void | secret hint | `INDEX 5B` | Rough optional clue for deferred archive-index secret. | 0.40 x 0.16 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `733,1666,581,308` |
| `SEC-L05-03` | rough | Level05 Governor Core | Optional clerk void | secret hint | `NO STAMP` | Rough optional clue for deferred unapproved service panel. | 0.38 x 0.16 | `Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png` | `1396,1666,581,308` |
