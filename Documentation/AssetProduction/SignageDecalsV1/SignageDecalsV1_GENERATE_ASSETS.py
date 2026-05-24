from __future__ import annotations

import csv
import hashlib
import json
import math
import random
import shutil
from datetime import datetime
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


ROOT = Path(__file__).resolve().parents[3]
DOC_ROOT = ROOT / "Documentation" / "AssetProduction" / "SignageDecalsV1"
ASSET_ROOT = ROOT / "Assets" / "_Project" / "ArtStaging" / "SignageDecalsV1"
TEXTURE_ROOT = ASSET_ROOT / "Textures"
PREVIEW_ROOT = ASSET_ROOT / "Previews"
SHEET_SIZE = (2048, 2048)
RNG = random.Random(14101)


PALETTE = {
    "transparent": "#00000000",
    "oil_black": "#0b0907",
    "oil_panel": "#18120d",
    "iron_dark": "#241f1a",
    "iron_mid": "#40352b",
    "brass_dark": "#6e4318",
    "brass": "#b87928",
    "brass_bright": "#e3a84b",
    "brass_hot": "#ffd17a",
    "cream": "#f2dfad",
    "cream_shadow": "#bfa06b",
    "amber": "#f0a33a",
    "danger_red": "#b92119",
    "danger_orange": "#ff612e",
    "green": "#4ddf72",
    "green_dark": "#17662f",
    "chalk": "#d9d0b0",
    "chalk_green": "#a8d29b",
    "soot": "#070504",
}


SHEET_INFO = {
    "objective_plates": {
        "file": "T_SignageDecalsV1_ObjectivePlates_2048.png",
        "label": "Brass/Cream Objective Plates",
        "cols": 3,
        "rows": 5,
        "style": "objective",
        "import_group": "World decal atlas, sRGB color, alpha transparency",
        "max_size": 2048,
    },
    "warning_strips": {
        "file": "T_SignageDecalsV1_WarningHazardStrips_2048.png",
        "label": "Warning And Hazard Strips",
        "cols": 3,
        "rows": 5,
        "style": "warning",
        "import_group": "World decal atlas, sRGB color, alpha transparency",
        "max_size": 2048,
    },
    "route_arrows": {
        "file": "T_SignageDecalsV1_RouteArrowsChevrons_2048.png",
        "label": "Route Arrows And Chevrons",
        "cols": 3,
        "rows": 5,
        "style": "arrow",
        "import_group": "World decal atlas, sRGB color, alpha transparency",
        "max_size": 2048,
    },
    "stencil_labels": {
        "file": "T_SignageDecalsV1_StencilMachineryLore_2048.png",
        "label": "Stencil Machinery And Work Orders",
        "cols": 5,
        "rows": 5,
        "style": "stencil",
        "import_group": "World decal atlas, sRGB color, alpha transparency",
        "max_size": 2048,
    },
    "secret_marks": {
        "file": "T_SignageDecalsV1_SecretServiceMarks_2048.png",
        "label": "Secret Service Marks",
        "cols": 3,
        "rows": 5,
        "style": "secret",
        "import_group": "World decal atlas, sRGB color, alpha transparency",
        "max_size": 2048,
    },
}


ENTRIES: list[dict] = []


def add(
    item_id: str,
    sheet: str,
    level: str,
    room: str,
    category: str,
    text: str,
    placement_intent: str,
    size_m: tuple[float, float],
    status: str = "ready",
    color_role: str = "amber",
    direction: str = "right",
) -> None:
    ENTRIES.append(
        {
            "id": item_id,
            "sheet": sheet,
            "level": level,
            "room": room,
            "category": category,
            "text": text,
            "placement_intent": placement_intent,
            "dimensions_m": {"width": size_m[0], "height": size_m[1]},
            "status": status,
            "color_role": color_role,
            "direction": direction,
        }
    )


def build_entries() -> None:
    level01 = "Level01 Brassworks Intake"
    level02 = "Level02 Pipeworks Annex"
    level03 = "Level03 Boilerheart Core"
    level04 = "Level04 Furnace Foundry"
    level05 = "Level05 Governor Core"

    add("OBJ-L01-01", "objective_plates", level01, "Clockwork repair bay approach", "objective sign", "GEAR KEY AHEAD", "Amber route plate before the gear-key plinth.", (1.20, 0.30))
    add("OBJ-L01-02", "objective_plates", level01, "Pressure gate face", "objective sign", "PRESSURE GATE", "Gate identifier centered on the locked pressure gate.", (1.35, 0.32))
    add("OBJ-L01-03", "objective_plates", level01, "Service lift alcove", "objective sign", "SERVICE LIFT", "Green-side destination plate above the exit lift.", (1.25, 0.30), color_role="green")

    add("OBJ-L02-01", "objective_plates", level02, "Pipeworks entry", "objective sign", "ROUTE PIPE PRESSURE", "Amber plate visible from the Level02 spawn lane.", (1.45, 0.32))
    add("OBJ-L02-02", "objective_plates", level02, "North valve run", "objective sign", "ROUTING VALVE", "Label above the required pressure-routing valve.", (1.20, 0.30))
    add("OBJ-L02-03", "objective_plates", level02, "Boilerheart service lift", "objective sign", "BOILERHEART LIFT", "Green destination plate for the lift once pressure is routed.", (1.35, 0.30), color_role="green")

    add("OBJ-L03-01", "objective_plates", level03, "Arrival floor", "objective sign", "VENT CORE PRESSURE", "Initial amber objective plate pointing toward the core valve.", (1.45, 0.32))
    add("OBJ-L03-02", "objective_plates", level03, "Furnace-core chamber", "objective sign", "PRESSURE VALVE", "Readable label at the required Boilerheart valve.", (1.25, 0.30))
    add("OBJ-L03-03", "objective_plates", level03, "Foundry service lift", "objective sign", "FOUNDRY LIFT", "Green lift identifier after pressure is vented.", (1.20, 0.30), color_role="green")

    add("OBJ-L04-01", "objective_plates", level04, "Furnace baffle lane", "objective sign", "FURNACE LANE", "Amber plate at the main foundry lane entrance.", (1.20, 0.30))
    add("OBJ-L04-02", "objective_plates", level04, "Quench side bay", "objective sign", "QUENCH BAY", "Readable side-route label near coolant/quench props.", (1.10, 0.28))
    add("OBJ-L04-03", "objective_plates", level04, "Emergency hoist end", "objective sign", "EMERGENCY HOIST", "Green destination plate above the transition hoist.", (1.35, 0.30), color_role="green")

    add("OBJ-L05-01", "objective_plates", level05, "Regulator ring entry", "objective sign", "REGULATOR RING", "Amber plate naming the central final encounter ring.", (1.25, 0.30))
    add("OBJ-L05-02", "objective_plates", level05, "Warden lock signal", "objective sign", "WARDEN LOCK", "Red-orange lock plate beside the denied master hoist state.", (1.15, 0.30), color_role="danger")
    add("OBJ-L05-03", "objective_plates", level05, "Master override hoist", "objective sign", "MASTER OVERRIDE", "Green final objective plate after Warden defeat.", (1.40, 0.32), color_role="green")

    add("HAZ-L01-01", "warning_strips", level01, "Pressure lock", "warning label", "PRESSURE LOCKED", "Red-orange denial strip beside the first locked gate.", (1.20, 0.22), color_role="danger")
    add("HAZ-L01-02", "warning_strips", level01, "Steam vent room", "hazard label", "STEAM VENT", "Small warning strip near animated vent puffs.", (1.00, 0.22), color_role="danger")
    add("HAZ-L01-03", "warning_strips", level01, "Pressure gate threshold", "hazard label", "GATE CRUSH", "Floor/door-edge warning decal at the gate travel path.", (1.00, 0.20), color_role="danger")

    add("HAZ-L02-01", "warning_strips", level02, "Pipeworks main", "hazard label", "HIGH PRESSURE MAIN", "Horizontal strip on the main pipe bundle.", (1.35, 0.22), color_role="danger")
    add("HAZ-L02-02", "warning_strips", level02, "Baffle corridor", "hazard label", "PIPE BURST RISK", "Warning strip along the baffle corridor wall.", (1.20, 0.22), color_role="danger")
    add("HAZ-L02-03", "warning_strips", level02, "North valve run", "warning label", "VALVE RUN LIVE", "Active-system warning beside the required valve.", (1.15, 0.22), color_role="danger")

    add("HAZ-L03-01", "warning_strips", level03, "Boilerheart furnace leak", "hazard label", "FURNACE LEAK", "Red-orange strip next to Boilerheart steam hazard.", (1.10, 0.22), color_role="danger")
    add("HAZ-L03-02", "warning_strips", level03, "Boilerheart core bleed", "hazard label", "CORE BLEED", "Hazard label beside the second linked steam hazard.", (1.00, 0.22), color_role="danger")
    add("HAZ-L03-03", "warning_strips", level03, "Bellows Node floor", "warning label", "PRESSURE PULSE", "Warning strip close to Bellows Node pulse range.", (1.20, 0.22), color_role="danger")

    add("HAZ-L04-01", "warning_strips", level04, "Furnace heat lane", "hazard label", "HEAT SURGE", "Warning strip at heat-surge lane entry.", (1.00, 0.22), color_role="danger")
    add("HAZ-L04-02", "warning_strips", level04, "Pour lane", "hazard label", "POUR LANE HOT", "Floor or low wall strip along the active pour lane.", (1.15, 0.22), color_role="danger")
    add("HAZ-L04-03", "warning_strips", level04, "Crucible steam bleed", "hazard label", "CRUCIBLE BLEED", "Warning strip near foundry steam leak.", (1.20, 0.22), color_role="danger")

    add("HAZ-L05-01", "warning_strips", level05, "Regulator leak", "hazard label", "REGULATOR LEAK", "Steam hazard label in the regulator lane.", (1.20, 0.22), color_role="danger")
    add("HAZ-L05-02", "warning_strips", level05, "Regulator surge floor", "hazard label", "SURGE FLOOR", "Floor strip for pulsing furnace-heat hazard.", (1.00, 0.22), color_role="danger")
    add("HAZ-L05-03", "warning_strips", level05, "Warden arena edge", "warning label", "WARDEN ACTIVE", "Red-orange encounter-state strip near the final lock.", (1.15, 0.22), color_role="danger")

    add("ARR-L01-01", "route_arrows", level01, "Service entry", "route arrow", "TO KEY", "Amber floor/wall arrow toward gear-key plinth.", (0.85, 0.30), direction="right")
    add("ARR-L01-02", "route_arrows", level01, "Repair bay return", "route arrow", "TO GATE", "Amber return arrow back to the pressure gate.", (0.85, 0.30), direction="left")
    add("ARR-L01-03", "route_arrows", level01, "Furnace control exit", "route arrow", "TO LIFT", "Green arrow pointing to service lift.", (0.85, 0.30), color_role="green", direction="right")

    add("ARR-L02-01", "route_arrows", level02, "Pipeworks entry", "route arrow", "TO VALVE", "Amber arrow toward north routing valve.", (0.90, 0.30), direction="up")
    add("ARR-L02-02", "route_arrows", level02, "Valve return", "route arrow", "TO LIFT", "Green arrow returning to Boilerheart lift.", (0.85, 0.30), color_role="green", direction="down")
    add("ARR-L02-03", "route_arrows", level02, "Post-valve route", "route arrow", "GREEN SIGNAL", "Small green success arrow near restored lift signal.", (1.00, 0.30), color_role="green", direction="right")

    add("ARR-L03-01", "route_arrows", level03, "Arrival floor", "route arrow", "TO VALVE", "Amber route arrow toward the pressure valve.", (0.90, 0.30), direction="up")
    add("ARR-L03-02", "route_arrows", level03, "Scattergun display", "route arrow", "TO TOOL", "Amber local cue for Steam Scattergun pickup route.", (0.85, 0.30), direction="right")
    add("ARR-L03-03", "route_arrows", level03, "Post-valve lift route", "route arrow", "TO FOUNDRY", "Green destination arrow after venting.", (1.00, 0.30), color_role="green", direction="up")

    add("ARR-L04-01", "route_arrows", level04, "Arrival floor", "route arrow", "TO FURNACE", "Amber arrow toward the main foundry lane.", (1.00, 0.30), direction="up")
    add("ARR-L04-02", "route_arrows", level04, "Quench side bay", "route arrow", "TO QUENCH", "Amber side-route arrow for quench bay dressing.", (1.00, 0.30), direction="right")
    add("ARR-L04-03", "route_arrows", level04, "Final foundry lane", "route arrow", "TO HOIST", "Green arrow toward emergency hoist.", (0.90, 0.30), color_role="green", direction="up")

    add("ARR-L05-01", "route_arrows", level05, "Arrival lane", "route arrow", "TO CORE", "Amber arrow into regulator ring.", (0.85, 0.30), direction="up")
    add("ARR-L05-02", "route_arrows", level05, "Core return", "route arrow", "TO OVERRIDE", "Green arrow toward final hoist after Warden defeat.", (1.00, 0.30), color_role="green", direction="down")
    add("ARR-L05-03", "route_arrows", level05, "Master hoist signal", "route arrow", "GREEN HOIST", "Success-state arrow near final lift signal.", (1.00, 0.30), color_role="green", direction="right")

    add("MAC-L01-01", "stencil_labels", level01, "Copper-pipe maintenance throat", "machinery label", "INTAKE PUMP 1", "Stencil on pump housing or nearby wall plate.", (0.90, 0.18))
    add("MAC-L01-02", "stencil_labels", level01, "Clockwork repair bay", "machinery label", "REPAIR BAY", "Small cream label over repair-bay entry.", (0.80, 0.18))
    add("MAC-L01-03", "stencil_labels", level01, "Pressure gate", "machinery label", "GATE DRIVE", "Stencil on pressure-gate gear housing.", (0.80, 0.18))
    add("LOR-L01-01", "stencil_labels", level01, "Pressure gate maintenance board", "work-order decal", "WO-01 CHECK GATE TEETH", "Short work order on a clipboard or enamel board.", (1.25, 0.20))
    add("LOR-L01-02", "stencil_labels", level01, "Service entry wall", "lore decal", "LOG: INTAKE CREW MISSING", "Small archive-flavor decal near entry benches.", (1.30, 0.20))

    add("MAC-L02-01", "stencil_labels", level02, "Pipeworks main", "machinery label", "ANNEX MAIN FEED", "Stencil on the largest pipe run.", (1.05, 0.18))
    add("MAC-L02-02", "stencil_labels", level02, "Baffle corridor", "machinery label", "BAFFLE CONTROL", "Label on baffle control housing.", (0.95, 0.18))
    add("MAC-L02-03", "stencil_labels", level02, "North valve run", "machinery label", "VALVE RUN 02", "Stencil next to pressure-routing valve.", (0.90, 0.18))
    add("LOR-L02-01", "stencil_labels", level02, "Cold pipe run", "work-order decal", "WO-02 PATCH COLD PIPE", "Short work order near the south-west pipe run.", (1.20, 0.20))
    add("LOR-L02-02", "stencil_labels", level02, "Annex checkpoint", "lore decal", "LOG: LANCERS SENT BELOW", "Small lore decal in first ranged-machine lane.", (1.25, 0.20))

    add("MAC-L03-01", "stencil_labels", level03, "Furnace-core chamber", "machinery label", "BOILERHEART CORE", "Stencil around the furnace-core landmark.", (1.05, 0.18))
    add("MAC-L03-02", "stencil_labels", level03, "Bellows Node alcove", "machinery label", "BELLOWS NODE A", "Label on or near the support-machine prototype.", (1.00, 0.18))
    add("MAC-L03-03", "stencil_labels", level03, "Gauge wall", "machinery label", "GAUGE BANK C", "Stencil over the gauge-bank dressing.", (0.90, 0.18))
    add("LOR-L03-01", "stencil_labels", level03, "Foundry lift lock panel", "work-order decal", "WO-03 VENT BEFORE LIFT", "Maintenance note justifying the pressure-locked lift.", (1.25, 0.20))
    add("LOR-L03-02", "stencil_labels", level03, "Core wall plaque cluster", "lore decal", "LOG: CORE BLEEDS FAST", "Short archive-flavor decal near the core.", (1.20, 0.20))

    add("MAC-L04-01", "stencil_labels", level04, "Pour lane", "machinery label", "POUR LANE 4", "Stencil beside active furnace lane.", (0.85, 0.18))
    add("MAC-L04-02", "stencil_labels", level04, "Quench side bay", "machinery label", "SLAG GUTTER", "Label on low gutter dressing.", (0.85, 0.18))
    add("MAC-L04-03", "stencil_labels", level04, "Bulwark pressure area", "machinery label", "HAMMER BAY", "Stencil near hammer/foundry machinery.", (0.85, 0.18))
    add("LOR-L04-01", "stencil_labels", level04, "Foundry maintenance board", "work-order decal", "WO-04 CLEAR SLAG GUTTER", "Short work order near slag and quench props.", (1.25, 0.20))
    add("LOR-L04-02", "stencil_labels", level04, "Coal cache vicinity", "lore decal", "LOG: COAL DOOR STICKS", "Flavor decal near coal-bin service area.", (1.20, 0.20))

    add("MAC-L05-01", "stencil_labels", level05, "Regulator ring", "machinery label", "GOVERNOR CORE", "Stencil on regulator ring wall panels.", (1.00, 0.18))
    add("MAC-L05-02", "stencil_labels", level05, "Regulator machinery", "machinery label", "REGULATOR DRUM", "Label on drum/pylon dressing.", (1.00, 0.18))
    add("MAC-L05-03", "stencil_labels", level05, "Warden lock panel", "machinery label", "WARDEN SEAL", "Small lock-state machinery label.", (0.90, 0.18), color_role="danger")
    add("LOR-L05-01", "stencil_labels", level05, "Master hoist maintenance board", "work-order decal", "WO-05 HOLD MASTER HOIST", "Short work order beside final hoist controls.", (1.25, 0.20))
    add("LOR-L05-02", "stencil_labels", level05, "Clerk alcove or archive plate", "lore decal", "LOG: WARDEN HEARS KEYS", "Short lore decal for the final lock mythology.", (1.25, 0.20))

    add("SEC-L01-01", "secret_marks", level01, "Intake pressure cache approach", "secret hint", "WARM SEAM", "Worker chalk close to the warm service-panel edge.", (0.45, 0.18), color_role="chalk")
    add("SEC-L01-02", "secret_marks", level01, "Intake pressure cache panel", "secret hint", "THREE RIVETS OUT", "Chalk phrase below missing rivets clue.", (0.55, 0.18), color_role="chalk")
    add("SEC-L01-03", "secret_marks", level01, "Intake pressure cache recess", "secret hint", "SPARES BEHIND", "Low chalk note near optional resource cache.", (0.55, 0.18), color_role="chalk")

    add("SEC-L02-01", "secret_marks", level02, "Pipeworks cartridge cache approach", "secret hint", "COLD PIPE", "Worker chalk beside cooler pipe color clue.", (0.45, 0.18), color_role="chalk")
    add("SEC-L02-02", "secret_marks", level02, "Pipeworks cartridge cache recess", "secret hint", "BOLT SHADOWS", "Subtle chalk note near spare bolt marks.", (0.50, 0.18), color_role="chalk")
    add("SEC-L02-03", "secret_marks", level02, "Pipeworks cartridge cache rack", "secret hint", "LOW RACK", "Chalk on floor trim near low pipe-rack cache.", (0.40, 0.16), color_role="chalk")

    add("SEC-L03-01", "secret_marks", level03, "Optional gauge cache", "secret hint", "GAUGE LIES", "Rough optional clue for deferred gauge-cache route.", (0.45, 0.18), status="rough", color_role="chalk")
    add("SEC-L03-02", "secret_marks", level03, "Optional gauge cache", "secret hint", "SHUTTER DRAGS", "Rough optional clue for deferred gauge-shutter cache.", (0.50, 0.18), status="rough", color_role="chalk")
    add("SEC-L03-03", "secret_marks", level03, "Optional gauge cache", "secret hint", "NEEDLE STICKS", "Rough optional clue for deferred gauge-needle cache.", (0.50, 0.18), status="rough", color_role="chalk")

    add("SEC-L04-01", "secret_marks", level04, "Foundry coal cache approach", "secret hint", "COAL DOOR COOL", "Worker chalk beside cooler quench-pipe clue.", (0.55, 0.18), color_role="chalk")
    add("SEC-L04-02", "secret_marks", level04, "Foundry coal cache floor", "secret hint", "BLACK BOOT MARK", "Chalk phrase near coal footprints.", (0.55, 0.18), color_role="chalk")
    add("SEC-L04-03", "secret_marks", level04, "Foundry coal cache panel", "secret hint", "QUENCH HUSH", "Small service mark beside quiet quench pipe.", (0.50, 0.18), color_role="chalk")

    add("SEC-L05-01", "secret_marks", level05, "Optional clerk void", "secret hint", "WRONG CLERK TAG", "Rough optional clue for deferred clerk-label secret.", (0.55, 0.18), status="rough", color_role="chalk")
    add("SEC-L05-02", "secret_marks", level05, "Optional clerk void", "secret hint", "INDEX 5B", "Rough optional clue for deferred archive-index secret.", (0.40, 0.16), status="rough", color_role="chalk")
    add("SEC-L05-03", "secret_marks", level05, "Optional clerk void", "secret hint", "NO STAMP", "Rough optional clue for deferred unapproved service panel.", (0.38, 0.16), status="rough", color_role="chalk")


def rgba(hex_value: str, alpha: int | None = None) -> tuple[int, int, int, int]:
    value = hex_value.strip("#")
    if len(value) == 8:
        r, g, b, a = (int(value[i : i + 2], 16) for i in range(0, 8, 2))
    else:
        r, g, b = (int(value[i : i + 2], 16) for i in range(0, 6, 2))
        a = 255
    if alpha is not None:
        a = alpha
    return r, g, b, a


def pal(name: str, alpha: int | None = None) -> tuple[int, int, int, int]:
    return rgba(PALETTE[name], alpha)


def font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont | ImageFont.ImageFont:
    candidates = [
        "C:/Windows/Fonts/bahnschrift.ttf",
        "C:/Windows/Fonts/Bahnschrift.ttf",
        "C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf",
        "C:/Windows/Fonts/consolab.ttf" if bold else "C:/Windows/Fonts/consola.ttf",
    ]
    for path in candidates:
        try:
            return ImageFont.truetype(path, size)
        except OSError:
            pass
    return ImageFont.load_default()


def text_bbox(draw: ImageDraw.ImageDraw, text: str, fnt: ImageFont.ImageFont) -> tuple[int, int, int, int]:
    return draw.textbbox((0, 0), text, font=fnt)


def text_size(draw: ImageDraw.ImageDraw, text: str, fnt: ImageFont.ImageFont) -> tuple[int, int]:
    x0, y0, x1, y1 = text_bbox(draw, text, fnt)
    return x1 - x0, y1 - y0


def draw_centered(draw: ImageDraw.ImageDraw, box: tuple[int, int, int, int], text: str, fill, max_size: int, min_size: int = 18, bold: bool = True) -> int:
    x0, y0, x1, y1 = box
    max_w = x1 - x0
    max_h = y1 - y0
    best = font(min_size, bold)
    best_lines = [text]
    for size in range(max_size, min_size - 1, -1):
        fnt = font(size, bold)
        lines = wrap_lines(draw, text, fnt, max_w)
        line_heights = [text_size(draw, line, fnt)[1] for line in lines]
        total_h = sum(line_heights) + max(0, len(lines) - 1) * int(size * 0.22)
        widest = max(text_size(draw, line, fnt)[0] for line in lines)
        if widest <= max_w and total_h <= max_h:
            best = fnt
            best_lines = lines
            break
    line_gap = 6
    heights = [text_size(draw, line, best)[1] for line in best_lines]
    total_h = sum(heights) + max(0, len(best_lines) - 1) * line_gap
    y = y0 + (max_h - total_h) // 2
    for line, h in zip(best_lines, heights):
        w, _ = text_size(draw, line, best)
        draw.text((x0 + (max_w - w) // 2, y), line, font=best, fill=fill)
        y += h + line_gap
    return best.size if hasattr(best, "size") else max_size


def wrap_lines(draw: ImageDraw.ImageDraw, text: str, fnt: ImageFont.ImageFont, max_w: int) -> list[str]:
    if text_size(draw, text, fnt)[0] <= max_w:
        return [text]
    words = text.split()
    lines: list[str] = []
    current = ""
    for word in words:
        trial = word if not current else f"{current} {word}"
        if text_size(draw, trial, fnt)[0] <= max_w or not current:
            current = trial
        else:
            lines.append(current)
            current = word
    if current:
        lines.append(current)
    return lines


def add_noise(draw: ImageDraw.ImageDraw, box: tuple[int, int, int, int], amount: int = 90) -> None:
    x0, y0, x1, y1 = box
    for _ in range(amount):
        x = RNG.randint(x0, max(x0, x1 - 1))
        y = RNG.randint(y0, max(y0, y1 - 1))
        color = pal("soot", RNG.randint(22, 76)) if RNG.random() < 0.70 else pal("brass_hot", RNG.randint(18, 48))
        draw.point((x, y), fill=color)


def draw_rivets(draw: ImageDraw.ImageDraw, box: tuple[int, int, int, int], fill, outline) -> None:
    x0, y0, x1, y1 = box
    points = [(x0 + 18, y0 + 18), (x1 - 18, y0 + 18), (x0 + 18, y1 - 18), (x1 - 18, y1 - 18)]
    for x, y in points:
        draw.ellipse((x - 8, y - 8, x + 8, y + 8), fill=fill, outline=outline, width=2)
        draw.ellipse((x - 3, y - 3, x + 3, y + 3), fill=outline)


def color_for(entry: dict) -> tuple[int, int, int, int]:
    role = entry.get("color_role", "amber")
    if role == "green":
        return pal("green")
    if role == "danger":
        return pal("danger_orange")
    if role == "chalk":
        return pal("chalk")
    return pal("amber")


def draw_objective(draw: ImageDraw.ImageDraw, entry: dict, box: tuple[int, int, int, int]) -> None:
    x0, y0, x1, y1 = box
    accent = color_for(entry)
    draw.rounded_rectangle(box, radius=18, fill=pal("brass_dark"), outline=pal("brass_hot"), width=4)
    inset = (x0 + 16, y0 + 16, x1 - 16, y1 - 16)
    draw.rounded_rectangle(inset, radius=12, fill=pal("cream"), outline=pal("brass"), width=3)
    draw.rectangle((x0 + 34, y0 + 32, x1 - 34, y0 + 46), fill=accent)
    draw.rectangle((x0 + 34, y1 - 46, x1 - 34, y1 - 32), fill=accent)
    draw_rivets(draw, box, pal("brass_bright"), pal("brass_dark"))
    text_box = (x0 + 42, y0 + 62, x1 - 42, y1 - 62)
    draw_centered(draw, text_box, entry["text"], pal("oil_black"), 52, 28)
    add_noise(draw, inset, 180)


def draw_warning(draw: ImageDraw.ImageDraw, entry: dict, box: tuple[int, int, int, int]) -> None:
    x0, y0, x1, y1 = box
    draw.rounded_rectangle(box, radius=12, fill=pal("oil_black"), outline=pal("danger_orange"), width=4)
    inner = (x0 + 12, y0 + 12, x1 - 12, y1 - 12)
    draw.rounded_rectangle(inner, radius=8, fill=pal("iron_dark"), outline=pal("brass_dark"), width=2)
    band_h = 28
    for band_y in (y0 + 22, y1 - 22 - band_h):
        for i, x in enumerate(range(x0 + 24, x1 - 24, 44)):
            fill = pal("danger_orange") if i % 2 == 0 else pal("oil_black")
            draw.polygon(
                [(x, band_y + band_h), (min(x + 28, x1 - 24), band_y + band_h), (min(x + 42, x1 - 24), band_y), (min(x + 14, x1 - 24), band_y)],
                fill=fill,
            )
    label_box = (x0 + 30, y0 + 62, x1 - 30, y1 - 62)
    draw.rounded_rectangle(label_box, radius=6, fill=pal("cream"), outline=pal("brass"), width=2)
    draw_centered(draw, (label_box[0] + 16, label_box[1] + 12, label_box[2] - 16, label_box[3] - 12), entry["text"], pal("danger_red"), 38, 20)
    add_noise(draw, box, 130)


def arrow_points(direction: str, cx: int, cy: int, w: int, h: int) -> list[tuple[int, int]]:
    if direction == "left":
        return [(cx - w // 2, cy), (cx - w // 6, cy - h // 2), (cx - w // 6, cy - h // 5), (cx + w // 2, cy - h // 5), (cx + w // 2, cy + h // 5), (cx - w // 6, cy + h // 5), (cx - w // 6, cy + h // 2)]
    if direction == "up":
        return [(cx, cy - h // 2), (cx + w // 2, cy - h // 6), (cx + w // 5, cy - h // 6), (cx + w // 5, cy + h // 2), (cx - w // 5, cy + h // 2), (cx - w // 5, cy - h // 6), (cx - w // 2, cy - h // 6)]
    if direction == "down":
        return [(cx, cy + h // 2), (cx + w // 2, cy + h // 6), (cx + w // 5, cy + h // 6), (cx + w // 5, cy - h // 2), (cx - w // 5, cy - h // 2), (cx - w // 5, cy + h // 6), (cx - w // 2, cy + h // 6)]
    return [(cx + w // 2, cy), (cx + w // 6, cy - h // 2), (cx + w // 6, cy - h // 5), (cx - w // 2, cy - h // 5), (cx - w // 2, cy + h // 5), (cx + w // 6, cy + h // 5), (cx + w // 6, cy + h // 2)]


def draw_arrow(draw: ImageDraw.ImageDraw, entry: dict, box: tuple[int, int, int, int]) -> None:
    x0, y0, x1, y1 = box
    accent = color_for(entry)
    draw.rounded_rectangle(box, radius=12, fill=pal("oil_black", 210), outline=pal("brass"), width=3)
    cx = x0 + (x1 - x0) // 2
    cy = y0 + (y1 - y0) // 2 - 24
    pts = arrow_points(entry.get("direction", "right"), cx, cy, int((x1 - x0) * 0.66), int((y1 - y0) * 0.42))
    shadow_pts = [(x + 5, y + 5) for x, y in pts]
    draw.polygon(shadow_pts, fill=pal("soot", 170))
    draw.polygon(pts, fill=accent, outline=pal("brass_hot"))
    for i in range(3):
        off = i * 22
        draw.line((x0 + 34 + off, y1 - 52, x0 + 72 + off, y1 - 52), fill=pal("brass_bright"), width=5)
    draw_centered(draw, (x0 + 34, y1 - 82, x1 - 34, y1 - 24), entry["text"], pal("cream"), 36, 18)
    add_noise(draw, box, 100)


def draw_stencil(draw: ImageDraw.ImageDraw, entry: dict, box: tuple[int, int, int, int]) -> None:
    x0, y0, x1, y1 = box
    fill = pal("cream") if entry["category"] != "machinery label" else pal("brass_hot")
    edge = color_for(entry)
    draw.rounded_rectangle(box, radius=8, fill=pal("oil_black", 170), outline=edge, width=2)
    draw.rectangle((x0 + 10, y0 + 10, x1 - 10, y1 - 10), outline=pal("iron_mid"), width=2)
    draw_centered(draw, (x0 + 18, y0 + 20, x1 - 18, y1 - 20), entry["text"], fill, 34, 16, bold=True)
    for _ in range(10):
        yy = RNG.randint(y0 + 14, y1 - 14)
        xx = RNG.randint(x0 + 10, x1 - 50)
        draw.line((xx, yy, xx + RNG.randint(16, 55), yy + RNG.randint(-2, 2)), fill=pal("soot", 130), width=1)
    add_noise(draw, box, 80)


def draw_secret(draw: ImageDraw.ImageDraw, entry: dict, box: tuple[int, int, int, int]) -> None:
    x0, y0, x1, y1 = box
    rough = entry["status"] == "rough"
    chalk = pal("chalk_green" if not rough else "chalk")
    draw.rounded_rectangle(box, radius=8, fill=pal("transparent"), outline=chalk, width=2)
    mark_x = x0 + 34
    mark_y = y0 + 34
    draw.line((mark_x, mark_y + 22, mark_x + 26, mark_y, mark_x + 52, mark_y + 22), fill=chalk, width=5)
    draw.line((mark_x + 10, mark_y + 28, mark_x + 42, mark_y + 28), fill=chalk, width=4)
    if rough:
        draw.line((x1 - 56, y0 + 24, x1 - 24, y0 + 56), fill=pal("danger_orange", 190), width=3)
        draw.line((x1 - 24, y0 + 24, x1 - 56, y0 + 56), fill=pal("danger_orange", 190), width=3)
    draw_centered(draw, (x0 + 26, y0 + 78, x1 - 26, y1 - 28), entry["text"], chalk, 36, 16, bold=True)
    for _ in range(32):
        x = RNG.randint(x0 + 14, x1 - 14)
        y = RNG.randint(y0 + 14, y1 - 14)
        draw.ellipse((x, y, x + RNG.randint(1, 4), y + RNG.randint(1, 4)), fill=pal("chalk", RNG.randint(70, 170)))


def draw_entry(draw: ImageDraw.ImageDraw, entry: dict, box: tuple[int, int, int, int]) -> None:
    style = SHEET_INFO[entry["sheet"]]["style"]
    if style == "objective":
        draw_objective(draw, entry, box)
    elif style == "warning":
        draw_warning(draw, entry, box)
    elif style == "arrow":
        draw_arrow(draw, entry, box)
    elif style == "stencil":
        draw_stencil(draw, entry, box)
    elif style == "secret":
        draw_secret(draw, entry, box)


def make_sheet(sheet_key: str, entries: list[dict]) -> Path:
    info = SHEET_INFO[sheet_key]
    img = Image.new("RGBA", SHEET_SIZE, (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    cols = info["cols"]
    rows = info["rows"]
    margin = 42
    gutter = 26
    cell_w = (SHEET_SIZE[0] - margin * 2 - gutter * (cols - 1)) // cols
    cell_h = (SHEET_SIZE[1] - margin * 2 - gutter * (rows - 1)) // rows
    for index, entry in enumerate(entries):
        col = index % cols
        row = index // cols
        x0 = margin + col * (cell_w + gutter)
        y0 = margin + row * (cell_h + gutter)
        x1 = x0 + cell_w
        y1 = y0 + cell_h
        pad_x = 28
        pad_y = 44 if info["style"] != "secret" else 32
        decal_box = (x0 + pad_x, y0 + pad_y, x1 - pad_x, y1 - pad_y)
        draw_entry(draw, entry, decal_box)
        small = font(20)
        draw.text((x0 + 6, y0 + 8), entry["id"], font=small, fill=pal("cream", 150))
        if entry["status"] == "rough":
            draw.text((x1 - 78, y0 + 8), "ROUGH", font=small, fill=pal("danger_orange", 190))
        entry["sheet_file"] = f"Assets/_Project/ArtStaging/SignageDecalsV1/Textures/{info['file']}"
        entry["source_rect_px"] = {"x": decal_box[0], "y": decal_box[1], "width": decal_box[2] - decal_box[0], "height": decal_box[3] - decal_box[1]}
        entry["sheet_dimensions_px"] = {"width": SHEET_SIZE[0], "height": SHEET_SIZE[1]}
        entry["import_group"] = info["import_group"]
    out_path = TEXTURE_ROOT / info["file"]
    img.save(out_path)
    shutil.copy2(out_path, DOC_ROOT / info["file"])
    return out_path


def sheet_preview(sheet_path: Path) -> Image.Image:
    img = Image.open(sheet_path).convert("RGBA")
    bg = Image.new("RGBA", img.size, pal("oil_panel"))
    d = ImageDraw.Draw(bg)
    step = 128
    for y in range(0, img.size[1], step):
        for x in range(0, img.size[0], step):
            if (x // step + y // step) % 2 == 0:
                d.rectangle((x, y, x + step, y + step), fill=pal("iron_dark"))
    bg.alpha_composite(img)
    return bg


def make_contact_sheet(sheet_paths: list[Path]) -> Path:
    thumb_w, thumb_h = 640, 640
    header_h = 72
    cols = 2
    rows = math.ceil(len(sheet_paths) / cols)
    out = Image.new("RGBA", (cols * thumb_w, rows * (thumb_h + header_h)), pal("oil_black"))
    draw = ImageDraw.Draw(out)
    title_font = font(27, True)
    for i, path in enumerate(sheet_paths):
        x = (i % cols) * thumb_w
        y = (i // cols) * (thumb_h + header_h)
        draw.rectangle((x, y, x + thumb_w, y + header_h), fill=pal("oil_panel"), outline=pal("brass_dark"), width=2)
        label = path.stem.replace("T_SignageDecalsV1_", "").replace("_2048", "")
        draw.text((x + 18, y + 22), label, font=title_font, fill=pal("brass_hot"))
        preview = sheet_preview(path).resize((thumb_w, thumb_h), Image.Resampling.LANCZOS)
        out.alpha_composite(preview, (x, y + header_h))
    out_path = PREVIEW_ROOT / "PREVIEW_SignageDecalsV1_ContactSheet.png"
    out.convert("RGBA").save(out_path)
    shutil.copy2(out_path, DOC_ROOT / "PREVIEW_SignageDecalsV1_ContactSheet.png")
    return out_path


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def write_csv() -> Path:
    path = DOC_ROOT / "SIGNAGE_DECALS_V1_COPY_DECK.csv"
    fields = [
        "id",
        "status",
        "level",
        "room",
        "category",
        "text",
        "placement_intent",
        "dimensions_m.width",
        "dimensions_m.height",
        "sheet_file",
        "source_rect_px.x",
        "source_rect_px.y",
        "source_rect_px.width",
        "source_rect_px.height",
        "import_group",
    ]
    with path.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=fields)
        writer.writeheader()
        for entry in ENTRIES:
            writer.writerow(
                {
                    "id": entry["id"],
                    "status": entry["status"],
                    "level": entry["level"],
                    "room": entry["room"],
                    "category": entry["category"],
                    "text": entry["text"],
                    "placement_intent": entry["placement_intent"],
                    "dimensions_m.width": entry["dimensions_m"]["width"],
                    "dimensions_m.height": entry["dimensions_m"]["height"],
                    "sheet_file": entry["sheet_file"],
                    "source_rect_px.x": entry["source_rect_px"]["x"],
                    "source_rect_px.y": entry["source_rect_px"]["y"],
                    "source_rect_px.width": entry["source_rect_px"]["width"],
                    "source_rect_px.height": entry["source_rect_px"]["height"],
                    "import_group": entry["import_group"],
                }
            )
    return path


def write_json(sheet_paths: list[Path], preview_path: Path) -> Path:
    payload = {
        "package": "SignageDecalsV1",
        "generated_at": datetime.now().astimezone().isoformat(timespec="seconds"),
        "scope": {
            "documentation": "Documentation/AssetProduction/SignageDecalsV1/",
            "asset_staging": "Assets/_Project/ArtStaging/SignageDecalsV1/",
            "scene_placement": "none",
        },
        "visual_references": [
            "Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png",
            "Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png",
            "Documentation/STEAMPUNK_NORTH_STAR.md",
        ],
        "import_settings": {
            "texture_type": "Default for world decal materials; Sprite (2D and UI) with Multiple slicing is also acceptable for atlas cropping.",
            "srgb": True,
            "alpha_is_transparency": True,
            "mip_maps": True,
            "filter_mode": "Bilinear",
            "wrap_mode": "Clamp",
            "compression": "None for V1 source review; high-quality compression can be tested after in-game readability checks.",
            "max_size": 2048,
            "notes": "Use source_rect_px from each entry if slicing individual decals from a sheet.",
        },
        "sheets": [
            {
                "path": f"Assets/_Project/ArtStaging/SignageDecalsV1/Textures/{path.name}",
                "dimensions_px": {"width": SHEET_SIZE[0], "height": SHEET_SIZE[1]},
                "sha256": sha256(path),
            }
            for path in sheet_paths
        ],
        "preview": {
            "path": "Assets/_Project/ArtStaging/SignageDecalsV1/Previews/PREVIEW_SignageDecalsV1_ContactSheet.png",
            "sha256": sha256(preview_path),
        },
        "counts": {
            "total": len(ENTRIES),
            "ready": sum(1 for entry in ENTRIES if entry["status"] == "ready"),
            "rough": sum(1 for entry in ENTRIES if entry["status"] == "rough"),
            "by_category": category_counts(),
            "by_level": level_counts(),
        },
        "entries": ENTRIES,
    }
    path = ASSET_ROOT / "SignageDecalsV1_AssetManifest.json"
    path.write_text(json.dumps(payload, indent=2), encoding="utf-8")
    shutil.copy2(path, DOC_ROOT / "SignageDecalsV1_AssetManifest.json")
    return path


def category_counts() -> dict[str, int]:
    counts: dict[str, int] = {}
    for entry in ENTRIES:
        counts[entry["category"]] = counts.get(entry["category"], 0) + 1
    return dict(sorted(counts.items()))


def level_counts() -> dict[str, int]:
    counts: dict[str, int] = {}
    for entry in ENTRIES:
        counts[entry["level"]] = counts.get(entry["level"], 0) + 1
    return dict(sorted(counts.items()))


def write_markdown(sheet_paths: list[Path], preview_path: Path, csv_path: Path, json_path: Path) -> Path:
    ready = sum(1 for entry in ENTRIES if entry["status"] == "ready")
    rough = len(ENTRIES) - ready
    lines: list[str] = []
    lines.append("# SignageDecalsV1 Manifest")
    lines.append("")
    lines.append(f"Generated: {datetime.now().astimezone().isoformat(timespec='seconds')}")
    lines.append("")
    lines.append("Scope: staged cross-level signage/decal text package only. No Unity scenes, gameplay scripts, README, BUILD_STATUS, WORK_LEDGER, shared status docs, or FinalMaterialsV1 files were modified.")
    lines.append("")
    lines.append("## Visual Target")
    lines.append("")
    lines.append("- Brass/cream enamel plates for readable objectives.")
    lines.append("- Red-orange danger strips for locked, hot, steam, and active combat states.")
    lines.append("- Green arrows and plates only for exits, restored routes, and success states.")
    lines.append("- Dark iron stencil labels for machinery and short work-order/lore scraps.")
    lines.append("- Worker chalk secret marks that read as optional service notes, not objective markers.")
    lines.append("- References: `Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png`, `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`, and `Documentation/STEAMPUNK_NORTH_STAR.md`.")
    lines.append("")
    lines.append("## Unity Import Settings")
    lines.append("")
    lines.append("- Texture Type: `Default` for world decal materials; `Sprite (2D and UI)` with `Multiple` slicing is acceptable if an integration pass wants atlas sprites.")
    lines.append("- sRGB: on.")
    lines.append("- Alpha Is Transparency: on.")
    lines.append("- Mip Maps: on for in-world signage/decal quads; disable only for UI-only preview use.")
    lines.append("- Filter Mode: `Bilinear`.")
    lines.append("- Wrap Mode: `Clamp`.")
    lines.append("- Compression: `None` for source review; test high-quality compression after in-game readability checks.")
    lines.append("- Max Size: `2048`.")
    lines.append("- Use each entry's `source_rect_px` from `SignageDecalsV1_AssetManifest.json` if slicing individual decals from a sheet.")
    lines.append("")
    lines.append("## Generated Sheets")
    lines.append("")
    for path in sheet_paths:
        rel_asset = f"Assets/_Project/ArtStaging/SignageDecalsV1/Textures/{path.name}"
        rel_doc = f"Documentation/AssetProduction/SignageDecalsV1/{path.name}"
        lines.append(f"- `{rel_asset}` - 2048x2048 RGBA sheet, documentation copy at `{rel_doc}`.")
    lines.append(f"- `Assets/_Project/ArtStaging/SignageDecalsV1/Previews/{preview_path.name}` - contact sheet preview, documentation copy at `Documentation/AssetProduction/SignageDecalsV1/{preview_path.name}`.")
    lines.append("")
    lines.append("## Counts")
    lines.append("")
    lines.append(f"- Total text/decal entries: {len(ENTRIES)}.")
    lines.append(f"- Ready entries: {ready}.")
    lines.append(f"- Rough entries: {rough}.")
    lines.append("- Rough entries are limited to optional/deferred Level03 gauge-cache and Level05 clerk-void secret marks.")
    lines.append("")
    lines.append("## Category Counts")
    lines.append("")
    lines.append("| Category | Count |")
    lines.append("| --- | ---: |")
    for category, count in category_counts().items():
        lines.append(f"| {category} | {count} |")
    lines.append("")
    lines.append("## Level Counts")
    lines.append("")
    lines.append("| Level | Count |")
    lines.append("| --- | ---: |")
    for level, count in level_counts().items():
        lines.append(f"| {level} | {count} |")
    lines.append("")
    lines.append("## Machine-Readable Files")
    lines.append("")
    lines.append(f"- `{json_path.relative_to(ROOT).as_posix()}` - full manifest with hashes, import settings, sheet paths, source rectangles, and exact strings.")
    lines.append(f"- `{(DOC_ROOT / json_path.name).relative_to(ROOT).as_posix()}` - documentation copy of the same JSON.")
    lines.append(f"- `{csv_path.relative_to(ROOT).as_posix()}` - copy deck CSV for design review and integration planning.")
    lines.append("")
    lines.append("## Full Copy Deck")
    lines.append("")
    lines.append("| ID | Status | Level | Room | Category | Exact text | Placement intent | Size m | Sheet | Source rect px |")
    lines.append("| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |")
    for entry in ENTRIES:
        dims = entry["dimensions_m"]
        rect = entry["source_rect_px"]
        rect_txt = f"{rect['x']},{rect['y']},{rect['width']},{rect['height']}"
        size_txt = f"{dims['width']:.2f} x {dims['height']:.2f}"
        lines.append(
            f"| `{entry['id']}` | {entry['status']} | {entry['level']} | {entry['room']} | {entry['category']} | `{entry['text']}` | {entry['placement_intent']} | {size_txt} | `{entry['sheet_file']}` | `{rect_txt}` |"
        )
    path = DOC_ROOT / "SIGNAGE_DECALS_V1_MANIFEST.md"
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")
    return path


def main() -> None:
    DOC_ROOT.mkdir(parents=True, exist_ok=True)
    TEXTURE_ROOT.mkdir(parents=True, exist_ok=True)
    PREVIEW_ROOT.mkdir(parents=True, exist_ok=True)
    build_entries()
    sheet_paths: list[Path] = []
    for sheet_key in SHEET_INFO:
        entries = [entry for entry in ENTRIES if entry["sheet"] == sheet_key]
        expected = SHEET_INFO[sheet_key]["cols"] * SHEET_INFO[sheet_key]["rows"]
        if len(entries) > expected:
            raise ValueError(f"{sheet_key} has {len(entries)} entries but only {expected} sheet cells")
        sheet_paths.append(make_sheet(sheet_key, entries))
    preview_path = make_contact_sheet(sheet_paths)
    csv_path = write_csv()
    json_path = write_json(sheet_paths, preview_path)
    md_path = write_markdown(sheet_paths, preview_path, csv_path, json_path)
    print(f"Wrote {len(sheet_paths)} sheets")
    print(f"Wrote {len(ENTRIES)} manifest entries: {sum(1 for e in ENTRIES if e['status'] == 'ready')} ready, {sum(1 for e in ENTRIES if e['status'] == 'rough')} rough")
    print(md_path.relative_to(ROOT).as_posix())


if __name__ == "__main__":
    main()
