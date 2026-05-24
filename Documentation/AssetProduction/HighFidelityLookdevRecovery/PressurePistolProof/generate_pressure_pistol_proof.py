from __future__ import annotations

import json
import math
import random
from pathlib import Path

import numpy as np
from PIL import Image, ImageDraw, ImageEnhance, ImageFilter, ImageFont


ROOT = Path(__file__).resolve().parents[4]
CONCEPT = ROOT / "Documentation" / "ConceptArt" / "north-star-steampunk-brassworks-pressure-pistol.png"
BATCH01 = ROOT / "Documentation" / "ConceptRenders" / "RENDER_LOOKDEV_HFLD_Batch01_pressure_pistol_nonshipping.jpg"
OUT_DIR = ROOT / "Documentation" / "ConceptRenders"
PROOF_DIR = Path(__file__).resolve().parent

RENDER_OUT = OUT_DIR / "RENDER_HFLD_Recovery03_pressure_pistol_proof_python_fallback.jpg"
SHEET_OUT = OUT_DIR / "CONTACTSHEET_HFLD_Recovery03_pressure_pistol_proof_python_fallback.jpg"
METRICS_OUT = PROOF_DIR / "pressure_pistol_proof_metrics.json"

W, H = 1920, 1080
SS = 2
RW, RH = W * SS, H * SS
RANDOM_SEED = 5303

random.seed(RANDOM_SEED)
np.random.seed(RANDOM_SEED)


def s(value: float) -> int:
    return int(round(value * SS))


def sb(box: tuple[float, float, float, float]) -> tuple[int, int, int, int]:
    return tuple(s(v) for v in box)


def sp(points: list[tuple[float, float]]) -> list[tuple[int, int]]:
    return [(s(x), s(y)) for x, y in points]


def load_font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont | ImageFont.ImageFont:
    names = [
        "C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf",
        "C:/Windows/Fonts/segoeuib.ttf" if bold else "C:/Windows/Fonts/segoeui.ttf",
    ]
    for name in names:
        try:
            return ImageFont.truetype(name, s(size))
        except OSError:
            pass
    return ImageFont.load_default()


FONT_SMALL = load_font(18)
FONT_MED = load_font(24)
FONT_MED_BOLD = load_font(24, bold=True)
FONT_BIG = load_font(42, bold=True)


metrics: dict[str, object] = {
    "tool": "Python 3.13 + Pillow 12.2 + NumPy procedural raster fallback",
    "unity_render_required": True,
    "random_seed": RANDOM_SEED,
    "hero_render": str(RENDER_OUT.relative_to(ROOT)).replace("\\", "/"),
    "contact_sheet": str(SHEET_OUT.relative_to(ROOT)).replace("\\", "/"),
    "dimensions": {"hero_render": [W, H]},
    "component_counts": {
        "main_blackened_barrel": 1,
        "lower_pressure_tank": 1,
        "top_pressure_gauge": 1,
        "coil_windows": 1,
        "muzzle_ring_stack_rings": 5,
        "trigger": 1,
        "trigger_guard": 1,
        "leather_glove_grip_mass": 1,
        "steam_pressure_ports": 0,
        "top_valves_caps": 0,
        "plates_straps_brackets": 0,
        "visible_fasteners": 0,
        "visible_coil_turns": 0,
        "gauge_tick_marks": 0,
    },
    "material_roles": [
        "blackened iron",
        "aged brass",
        "darker pipe metal",
        "hot copper coil",
        "cream gauge face",
        "glass highlight",
        "dark leather",
        "soot/grime",
    ],
    "logical_material_slots": [
        "blackened_iron",
        "aged_brass",
        "dark_pipe_metal",
        "hot_copper",
        "gauge_glass_face",
        "leather_soot_grime",
    ],
}


def add_count(name: str, amount: int = 1) -> None:
    counts = metrics["component_counts"]  # type: ignore[index]
    counts[name] = int(counts.get(name, 0)) + amount  # type: ignore[union-attr]


def rgba(color: tuple[int, int, int], alpha: int = 255) -> tuple[int, int, int, int]:
    return color[0], color[1], color[2], alpha


BLACK_IRON = (34, 34, 32)
BLACK_IRON_HI = (122, 97, 66)
BLACK_IRON_DARK = (5, 7, 8)
PIPE_DARK = (28, 27, 25)
PIPE_HI = (104, 83, 60)
BRASS = (151, 94, 36)
BRASS_HI = (238, 174, 82)
BRASS_DARK = (55, 37, 20)
COPPER = (198, 78, 28)
COPPER_HI = (255, 168, 66)
COPPER_DARK = (83, 31, 18)
LEATHER = (88, 43, 22)
LEATHER_HI = (172, 96, 53)
LEATHER_DARK = (30, 14, 9)
CREAM = (226, 210, 162)
RED = (164, 33, 27)
SOOT = (10, 9, 8)


def make_background() -> Image.Image:
    yy, xx = np.mgrid[0:RH, 0:RW]
    base = np.zeros((RH, RW, 3), dtype=np.float32)
    base[:, :] = np.array([17, 16, 14], dtype=np.float32)

    warm = np.exp(-(((xx - s(660)) / s(880)) ** 2 + ((yy - s(210)) / s(420)) ** 2))
    amber_pool = np.exp(-(((xx - s(900)) / s(820)) ** 2 + ((yy - s(620)) / s(520)) ** 2))
    cool = np.exp(-(((xx - s(1680)) / s(760)) ** 2 + ((yy - s(720)) / s(620)) ** 2))
    vignette = np.sqrt(((xx - RW / 2) / (RW / 1.18)) ** 2 + ((yy - RH / 2) / (RH / 1.0)) ** 2)

    base += warm[..., None] * np.array([82, 45, 12], dtype=np.float32)
    base += amber_pool[..., None] * np.array([38, 23, 8], dtype=np.float32)
    base += cool[..., None] * np.array([8, 12, 18], dtype=np.float32)
    base -= np.clip(vignette - 0.35, 0, 1)[..., None] * 34
    base += np.random.normal(0, 4.0, base.shape)
    base = np.clip(base, 0, 255).astype(np.uint8)

    bg = Image.fromarray(base, "RGB").convert("RGBA")

    smoke = Image.new("RGBA", (RW, RH), (0, 0, 0, 0))
    d = ImageDraw.Draw(smoke, "RGBA")
    for _ in range(115):
        cx = s(random.uniform(60, 1860))
        cy = s(random.uniform(40, 1030))
        rx = s(random.uniform(80, 310))
        ry = s(random.uniform(45, 210))
        alpha = random.randint(8, 28)
        col = (145, 132, 112, alpha) if random.random() < 0.62 else (70, 75, 78, alpha)
        d.ellipse((cx - rx, cy - ry, cx + rx, cy + ry), fill=col)
    smoke = smoke.filter(ImageFilter.GaussianBlur(s(28)))
    bg.alpha_composite(smoke)

    grade = Image.new("RGBA", (RW, RH), (0, 0, 0, 0))
    gd = ImageDraw.Draw(grade, "RGBA")
    gd.rectangle((0, 0, RW, RH), fill=(6, 5, 4, 28))
    gd.ellipse(sb((-220, -160, 980, 700)), fill=(68, 38, 10, 34))
    gd.ellipse(sb((870, 40, 2140, 1180)), fill=(0, 10, 16, 28))
    grade = grade.filter(ImageFilter.GaussianBlur(s(68)))
    bg.alpha_composite(grade)
    return bg


def textured_masked_fill(
    size: tuple[int, int],
    mask: Image.Image,
    base: tuple[int, int, int],
    hi: tuple[int, int, int],
    dark: tuple[int, int, int],
    gloss_axis: str = "vertical",
    noise: float = 18,
) -> Image.Image:
    w, h = size
    yy, xx = np.mgrid[0:h, 0:w]
    if gloss_axis == "vertical":
        t = yy / max(1, h - 1)
        shine = 0.18 + 0.62 * np.exp(-((t - 0.34) / 0.18) ** 2) + 0.18 * np.exp(-((t - 0.74) / 0.18) ** 2)
    elif gloss_axis == "horizontal":
        t = xx / max(1, w - 1)
        shine = 0.16 + 0.70 * np.exp(-((t - 0.36) / 0.22) ** 2)
    else:
        t = (xx + yy) / max(1, w + h - 2)
        shine = 0.16 + 0.68 * np.exp(-((t - 0.42) / 0.24) ** 2)

    long_variation = 0.12 * np.sin((xx / max(1, w)) * math.tau * 3.1 + 0.8)
    shine = np.clip(shine + long_variation, 0, 1)
    base_arr = np.array(base, dtype=np.float32)
    hi_arr = np.array(hi, dtype=np.float32)
    dark_arr = np.array(dark, dtype=np.float32)
    arr = dark_arr + (base_arr - dark_arr) * (0.54 + 0.28 * shine[..., None])
    arr += (hi_arr - base_arr) * (shine[..., None] ** 2.2) * 0.58
    arr += np.random.normal(0, noise, arr.shape)
    arr = np.clip(arr, 0, 255).astype(np.uint8)
    image = Image.fromarray(arr, "RGB").convert("RGBA")
    image.putalpha(mask)
    return image


def paste_rotated(target: Image.Image, layer: Image.Image, center: tuple[float, float], angle: float) -> None:
    rotated = layer.rotate(angle, resample=Image.Resampling.BICUBIC, expand=True)
    target.alpha_composite(rotated, (s(center[0]) - rotated.width // 2, s(center[1]) - rotated.height // 2))


def scale_layer(layer: Image.Image, scale: float, center: tuple[float, float], offset: tuple[float, float] = (0, 0)) -> Image.Image:
    resized = layer.resize((int(layer.width * scale), int(layer.height * scale)), Image.Resampling.LANCZOS)
    out = Image.new("RGBA", layer.size, (0, 0, 0, 0))
    x = s(center[0] + offset[0]) - resized.width // 2
    y = s(center[1] + offset[1]) - resized.height // 2
    out.alpha_composite(resized, (x, y))
    return out


def new_layer(w: int, h: int) -> Image.Image:
    return Image.new("RGBA", (s(w), s(h)), (0, 0, 0, 0))


def draw_rivet(
    draw: ImageDraw.ImageDraw,
    cx: float,
    cy: float,
    r: float,
    fill: tuple[int, int, int] = BRASS,
    slot: bool = True,
    count: bool = True,
) -> None:
    if count:
        add_count("visible_fasteners")
    shadow = tuple(max(0, c - 55) for c in fill)
    highlight = tuple(min(255, c + 82) for c in fill)
    draw.ellipse(sb((cx - r, cy - r, cx + r, cy + r)), fill=rgba(shadow, 255))
    draw.ellipse(sb((cx - r * 0.73, cy - r * 0.79, cx + r * 0.73, cy + r * 0.70)), fill=rgba(fill, 255))
    draw.arc(sb((cx - r * 0.65, cy - r * 0.66, cx + r * 0.52, cy + r * 0.50)), 205, 315, fill=rgba(highlight, 225), width=max(1, s(1.3)))
    if slot and r >= 3.8:
        draw.line((s(cx - r * 0.43), s(cy), s(cx + r * 0.43), s(cy)), fill=rgba(shadow, 210), width=max(1, s(1.1)))


def draw_scratches(draw: ImageDraw.ImageDraw, area: tuple[float, float, float, float], amount: int, warm: bool = False) -> None:
    x0, y0, x1, y1 = area
    for _ in range(amount):
        x = random.uniform(x0, x1)
        y = random.uniform(y0, y1)
        length = random.uniform(8, 38)
        angle = random.uniform(-0.25, 0.2)
        col = (205, 143, 78, random.randint(42, 108)) if warm else (155, 132, 101, random.randint(32, 92))
        draw.line((s(x), s(y), s(x + math.cos(angle) * length), s(y + math.sin(angle) * length)), fill=col, width=max(1, s(random.uniform(0.55, 1.35))))


def draw_barrel() -> Image.Image:
    w, h = 1010, 178
    layer = new_layer(w, h)
    mask = Image.new("L", layer.size, 0)
    md = ImageDraw.Draw(mask)
    md.rounded_rectangle(sb((55, 34, w - 72, h - 34)), radius=s(54), fill=255)
    md.ellipse(sb((28, 32, 160, h - 32)), fill=255)
    md.ellipse(sb((w - 155, 26, w - 35, h - 30)), fill=255)
    metal = textured_masked_fill(layer.size, mask, BLACK_IRON, BLACK_IRON_HI, BLACK_IRON_DARK, "vertical", noise=15)
    layer.alpha_composite(metal)
    d = ImageDraw.Draw(layer, "RGBA")

    band_specs = [
        (78, 26, 38, 126),
        (186, 24, 26, 130),
        (520, 29, 24, 120),
        (868, 27, 32, 126),
        (932, 22, 42, 136),
    ]
    for x, y, bw, bh in band_specs:
        d.rounded_rectangle(sb((x, y, x + bw, y + bh)), radius=s(11), fill=rgba(BRASS_DARK, 255), outline=rgba(BRASS_HI, 130), width=s(2))
        d.rectangle(sb((x + 4, y + 5, x + bw - 5, y + bh - 6)), fill=rgba(BRASS, 214))
        d.line((s(x + 5), s(y + 12), s(x + bw - 5), s(y + 12)), fill=rgba(BRASS_HI, 180), width=s(2))
        add_count("plates_straps_brackets")
        for ry in (y + 19, y + bh - 18):
            draw_rivet(d, x + bw / 2, ry, 4.2, BRASS)

    d.line((s(88), s(53), s(885), s(47)), fill=(206, 158, 94, 92), width=s(3))
    d.line((s(130), s(126), s(846), s(132)), fill=(4, 3, 2, 118), width=s(4))
    d.arc(sb((32, 35, 158, 143)), 95, 267, fill=rgba(BRASS_HI, 170), width=s(3))
    d.arc(sb((856, 30, 982, 148)), 278, 86, fill=rgba(BRASS_HI, 125), width=s(3))
    draw_scratches(d, (115, 44, 875, 134), 84, warm=False)
    return layer


def draw_lower_tank() -> Image.Image:
    w, h = 830, 154
    layer = new_layer(w, h)
    mask = Image.new("L", layer.size, 0)
    md = ImageDraw.Draw(mask)
    md.rounded_rectangle(sb((42, 29, w - 45, h - 29)), radius=s(48), fill=255)
    md.ellipse(sb((20, 26, 132, h - 27)), fill=255)
    md.ellipse(sb((w - 129, 25, w - 22, h - 28)), fill=255)
    metal = textured_masked_fill(layer.size, mask, PIPE_DARK, PIPE_HI, BLACK_IRON_DARK, "vertical", noise=14)
    layer.alpha_composite(metal)
    d = ImageDraw.Draw(layer, "RGBA")
    for x in (84, 190, 342, 486, 642, 736):
        d.rounded_rectangle(sb((x, 18, x + 33, h - 18)), radius=s(10), fill=rgba(BRASS_DARK, 240), outline=rgba(BRASS_HI, 120), width=s(2))
        d.line((s(x + 5), s(33), s(x + 29), s(33)), fill=rgba(BRASS_HI, 150), width=s(2))
        add_count("plates_straps_brackets")
        for yy in (39, h - 39):
            draw_rivet(d, x + 16.5, yy, 4.4, BRASS)
    for x in (120, 255, 415, 575, 702):
        d.line((s(x), s(42), s(x + 45), s(123)), fill=(5, 4, 3, 95), width=s(3))
    d.line((s(60), s(48), s(770), s(49)), fill=(206, 164, 98, 88), width=s(3))
    d.line((s(80), s(116), s(760), s(120)), fill=(2, 2, 2, 130), width=s(5))
    draw_scratches(d, (66, 42, 770, 118), 72, warm=False)
    return layer


def draw_muzzle_stack() -> Image.Image:
    w, h = 390, 146
    layer = new_layer(w, h)
    d = ImageDraw.Draw(layer, "RGBA")
    # Back throat and five nested rings/nozzles.
    d.rounded_rectangle(sb((185, 44, 355, 103)), radius=s(22), fill=rgba(BLACK_IRON, 245), outline=rgba(BRASS_HI, 150), width=s(2))
    rings = [
        (135, 27, 96, BRASS_DARK),
        (108, 35, 84, BRASS),
        (82, 42, 72, PIPE_DARK),
        (58, 48, 58, BRASS),
        (34, 54, 45, BLACK_IRON),
    ]
    for x, y, hh, color in rings:
        d.rounded_rectangle(sb((x, y, x + 54, y + hh)), radius=s(18), fill=rgba(color, 255), outline=rgba(BRASS_HI, 145), width=s(2))
        d.line((s(x + 7), s(y + 12), s(x + 47), s(y + 12)), fill=rgba(BRASS_HI, 150), width=s(2))
        add_count("plates_straps_brackets")
        for yy in (y + 16, y + hh - 15):
            draw_rivet(d, x + 27, yy, 3.8, BRASS)
    d.ellipse(sb((15, 50, 67, 96)), fill=rgba(BLACK_IRON_DARK, 255), outline=rgba(BRASS_HI, 105), width=s(2))
    d.ellipse(sb((27, 60, 54, 86)), fill=(0, 0, 0, 255))
    d.line((s(22), s(55), s(60), s(55)), fill=rgba(BRASS_HI, 150), width=s(2))
    return layer


def draw_coil_housing() -> Image.Image:
    w, h = 610, 220
    layer = new_layer(w, h)
    d = ImageDraw.Draw(layer, "RGBA")
    d.rounded_rectangle(sb((22, 18, w - 24, h - 22)), radius=s(24), fill=rgba(BRASS_DARK, 255), outline=rgba(BRASS_HI, 185), width=s(3))
    d.rounded_rectangle(sb((45, 36, w - 48, h - 40)), radius=s(18), fill=rgba(BRASS, 228), outline=rgba(BRASS_HI, 180), width=s(2))
    d.rounded_rectangle(sb((83, 58, w - 82, h - 62)), radius=s(14), fill=(28, 18, 12, 242), outline=rgba(BLACK_IRON_DARK, 210), width=s(4))
    add_count("plates_straps_brackets", 4)

    # Coil turns: separated hot copper loops in a dark window.
    coil_y = h / 2 + 2
    start_x = 128
    turns = 8
    for i in range(turns):
        cx = start_x + i * 48
        add_count("visible_coil_turns")
        d.ellipse(sb((cx - 28, coil_y - 47, cx + 28, coil_y + 47)), outline=rgba(COPPER_HI, 252), width=s(12))
        d.ellipse(sb((cx - 20, coil_y - 38, cx + 20, coil_y + 38)), outline=rgba(COPPER, 230), width=s(5))
        d.arc(sb((cx - 31, coil_y - 50, cx + 31, coil_y + 50)), 210, 318, fill=(255, 220, 122, 185), width=s(3))
    glow = Image.new("RGBA", layer.size, (0, 0, 0, 0))
    gd = ImageDraw.Draw(glow, "RGBA")
    gd.rounded_rectangle(sb((90, 54, w - 86, h - 57)), radius=s(22), fill=(224, 88, 26, 78))
    glow = glow.filter(ImageFilter.GaussianBlur(s(14)))
    layer.alpha_composite(glow)

    # Repaint crisp copper over the glow.
    d = ImageDraw.Draw(layer, "RGBA")
    for i in range(turns):
        cx = start_x + i * 48
        d.ellipse(sb((cx - 28, coil_y - 47, cx + 28, coil_y + 47)), outline=rgba(COPPER_HI, 252), width=s(10))
        d.ellipse(sb((cx - 20, coil_y - 38, cx + 20, coil_y + 38)), outline=rgba(COPPER, 230), width=s(4))

    for x in (55, 105, 505, 555):
        for y in (48, 176):
            draw_rivet(d, x, y, 5.0, BRASS)
    for x in range(145, 490, 70):
        draw_rivet(d, x, 39, 4.0, BRASS)
        draw_rivet(d, x, 184, 4.0, BRASS)
    d.line((s(58), s(45), s(552), s(41)), fill=rgba(BRASS_HI, 160), width=s(3))
    d.line((s(80), s(183), s(530), s(188)), fill=rgba(SOOT, 110), width=s(5))
    return layer


def draw_side_frame() -> Image.Image:
    w, h = 840, 300
    layer = new_layer(w, h)
    d = ImageDraw.Draw(layer, "RGBA")

    rails = [
        (34, 52, 735, 88),
        (70, 172, 760, 210),
        (160, 102, 610, 142),
    ]
    for x0, y0, x1, y1 in rails:
        d.rounded_rectangle(sb((x0, y0, x1, y1)), radius=s(10), fill=rgba(BRASS_DARK, 255), outline=rgba(BRASS_HI, 132), width=s(2))
        d.rectangle(sb((x0 + 7, y0 + 8, x1 - 7, y1 - 8)), fill=rgba(BRASS, 190))
        add_count("plates_straps_brackets")
        for x in np.linspace(x0 + 35, x1 - 35, 7):
            draw_rivet(d, float(x), (y0 + y1) / 2, 4.3, BRASS)

    verticals = [
        (125, 24, 166, 226),
        (292, 38, 336, 236),
        (470, 43, 512, 238),
        (650, 34, 693, 228),
    ]
    for x0, y0, x1, y1 in verticals:
        d.rounded_rectangle(sb((x0, y0, x1, y1)), radius=s(12), fill=rgba(BRASS_DARK, 255), outline=rgba(BRASS_HI, 140), width=s(2))
        d.rectangle(sb((x0 + 7, y0 + 9, x1 - 7, y1 - 9)), fill=rgba(BRASS, 170))
        add_count("plates_straps_brackets")
        for y in np.linspace(y0 + 26, y1 - 26, 4):
            draw_rivet(d, (x0 + x1) / 2, float(y), 4.0, BRASS)

    # Round pressure side medallion.
    d.ellipse(sb((548, 112, 706, 270)), fill=rgba(BRASS_DARK, 255), outline=rgba(BRASS_HI, 175), width=s(3))
    d.ellipse(sb((575, 139, 679, 243)), fill=rgba(BRASS, 210), outline=rgba(BRASS_HI, 145), width=s(3))
    d.ellipse(sb((609, 173, 645, 209)), fill=rgba(PIPE_DARK, 255), outline=rgba(BRASS_HI, 135), width=s(2))
    add_count("plates_straps_brackets", 2)
    for a in range(0, 360, 45):
        draw_rivet(d, 627 + math.cos(math.radians(a)) * 64, 191 + math.sin(math.radians(a)) * 64, 4.1, BRASS)

    # Two pressure ports/sockets.
    for cx, cy in ((238, 233), (754, 115)):
        d.ellipse(sb((cx - 34, cy - 34, cx + 34, cy + 34)), fill=rgba(BRASS_DARK, 255), outline=rgba(BRASS_HI, 150), width=s(3))
        d.ellipse(sb((cx - 18, cy - 18, cx + 18, cy + 18)), fill=rgba(BLACK_IRON_DARK, 255), outline=rgba(PIPE_HI, 110), width=s(2))
        for a in (0, 90, 180, 270):
            draw_rivet(d, cx + math.cos(math.radians(a)) * 27, cy + math.sin(math.radians(a)) * 27, 3.6, BRASS)
        add_count("steam_pressure_ports")

    draw_scratches(d, (45, 45, 760, 230), 84, warm=True)
    return layer


def draw_trigger_and_guard(gun: Image.Image) -> None:
    d = ImageDraw.Draw(gun, "RGBA")
    # Shadowed guard loop.
    d.arc(sb((880, 610, 1205, 850)), 176, 357, fill=rgba(BRASS_DARK, 255), width=s(20))
    d.arc(sb((898, 626, 1184, 833)), 180, 354, fill=rgba(BRASS_HI, 135), width=s(4))
    d.arc(sb((915, 638, 1163, 815)), 184, 350, fill=rgba(SOOT, 135), width=s(4))
    add_count("plates_straps_brackets", 1)
    for x, y in ((915, 660), (1180, 720), (1010, 821), (1100, 812)):
        draw_rivet(d, x, y, 5.0, BRASS)

    # Trigger.
    d.pieslice(sb((1010, 660, 1118, 810)), 80, 260, fill=rgba(BLACK_IRON_DARK, 255), outline=rgba(PIPE_HI, 95), width=s(2))
    d.pieslice(sb((1038, 680, 1096, 777)), 80, 265, fill=(0, 0, 0, 0))
    d.line((s(1042), s(677), s(1068), s(790)), fill=rgba(BLACK_IRON_HI, 120), width=s(2))


def draw_leather_hand(gun: Image.Image) -> None:
    # Palm/back-of-hand base.
    hand_mask = Image.new("L", (RW, RH), 0)
    hd = ImageDraw.Draw(hand_mask)
    palm = [
        (1115, 636),
        (1378, 614),
        (1595, 706),
        (1750, 838),
        (1715, 940),
        (1442, 945),
        (1212, 876),
        (1064, 748),
    ]
    hd.polygon(sp(palm), fill=255)
    hd.rounded_rectangle(sb((1200, 748, 1720, 952)), radius=s(84), fill=255)

    texture = textured_masked_fill((RW, RH), hand_mask, LEATHER, LEATHER_HI, LEATHER_DARK, "diagonal", noise=19)
    gun.alpha_composite(texture)
    d = ImageDraw.Draw(gun, "RGBA")
    d.line(sp(palm + [palm[0]]), fill=(217, 140, 78, 110), width=s(2))
    d.line(sp([(1078, 722), (1255, 794), (1460, 854), (1705, 895)]), fill=(21, 10, 7, 142), width=s(5))
    d.line(sp([(1110, 684), (1278, 728), (1500, 815), (1718, 858)]), fill=(210, 132, 76, 82), width=s(3))

    # Fingers tucked around the grip.
    finger_specs = [
        (1238, 790, 1540, 883, 10),
        (1270, 842, 1585, 931, 7),
        (1395, 705, 1655, 796, 17),
    ]
    for x0, y0, x1, y1, angle in finger_specs:
        fw, fh = int(x1 - x0), int(y1 - y0)
        layer = new_layer(fw, fh)
        fd = ImageDraw.Draw(layer, "RGBA")
        fd.rounded_rectangle(sb((3, 5, fw - 3, fh - 6)), radius=s(35), fill=rgba(LEATHER_DARK, 235), outline=rgba(LEATHER_HI, 98), width=s(2))
        fd.rounded_rectangle(sb((11, 11, fw - 15, fh - 16)), radius=s(31), fill=rgba(LEATHER, 228))
        fd.line((s(30), s(20), s(fw - 34), s(22)), fill=rgba(LEATHER_HI, 70), width=s(2))
        fd.line((s(30), s(fh - 25), s(fw - 38), s(fh - 30)), fill=rgba(SOOT, 95), width=s(4))
        for k in range(3):
            xx = 58 + k * 78
            fd.arc(sb((xx, 14, xx + 86, fh - 10)), 200, 330, fill=rgba(LEATHER_HI, 72), width=s(2))
        rot = layer.rotate(angle, resample=Image.Resampling.BICUBIC, expand=True)
        gun.alpha_composite(rot, (s(x0) - s(8), s(y0) - s(10)))

    # Grip spine and seams.
    d.rounded_rectangle(sb((1180, 570, 1535, 735)), radius=s(22), fill=rgba(LEATHER_DARK, 238), outline=rgba(BRASS_HI, 95), width=s(2))
    for off in range(0, 290, 48):
        d.line((s(1212 + off), s(590), s(1168 + off), s(710)), fill=(209, 133, 76, 95), width=s(2))
        d.line((s(1215 + off), s(592), s(1170 + off), s(710)), fill=(20, 9, 6, 80), width=s(1))
    draw_scratches(d, (1095, 635, 1725, 936), 150, warm=True)

    # Fasteners on glove strap and grip plate.
    d.rounded_rectangle(sb((1360, 648, 1530, 704)), radius=s(12), fill=rgba(BRASS_DARK, 220), outline=rgba(BRASS_HI, 118), width=s(2))
    add_count("plates_straps_brackets")
    for x in (1385, 1425, 1465, 1505):
        draw_rivet(d, x, 676, 4.7, BRASS)


def draw_top_valve(gun: Image.Image, cx: float, base_y: float, scale: float = 1.0, steam: bool = False) -> None:
    d = ImageDraw.Draw(gun, "RGBA")
    w = 54 * scale
    h = 118 * scale
    d.rounded_rectangle(sb((cx - w / 2, base_y - h, cx + w / 2, base_y)), radius=s(12 * scale), fill=rgba(BRASS_DARK, 255), outline=rgba(BRASS_HI, 145), width=s(2))
    d.rectangle(sb((cx - w * 0.36, base_y - h + 18 * scale, cx + w * 0.36, base_y - 18 * scale)), fill=rgba(BRASS, 214))
    d.rounded_rectangle(sb((cx - w * 0.70, base_y - h - 18 * scale, cx + w * 0.70, base_y - h + 10 * scale)), radius=s(7 * scale), fill=rgba(BRASS_DARK, 255), outline=rgba(BRASS_HI, 145), width=s(2))
    d.line((s(cx - w * 0.38), s(base_y - h + 30 * scale), s(cx + w * 0.36), s(base_y - h + 30 * scale)), fill=rgba(BRASS_HI, 150), width=s(2))
    for yy in (base_y - h + 38 * scale, base_y - 20 * scale):
        draw_rivet(d, cx, yy, 4.2 * scale, BRASS)
    add_count("top_valves_caps")
    add_count("plates_straps_brackets")

    if steam:
        steam_layer = Image.new("RGBA", (RW, RH), (0, 0, 0, 0))
        sd = ImageDraw.Draw(steam_layer, "RGBA")
        for _ in range(26):
            ox = random.uniform(-45, 90)
            oy = random.uniform(-170, -18)
            rx = random.uniform(28, 86)
            ry = random.uniform(20, 78)
            sd.ellipse(sb((cx + ox - rx, base_y - h + oy - ry, cx + ox + rx, base_y - h + oy + ry)), fill=(190, 179, 155, random.randint(20, 64)))
        steam_layer = steam_layer.filter(ImageFilter.GaussianBlur(s(12)))
        gun.alpha_composite(steam_layer)


def draw_gauge(gun: Image.Image) -> None:
    layer = new_layer(250, 250)
    d = ImageDraw.Draw(layer, "RGBA")
    cx = cy = 125
    # Mount yoke.
    d.rounded_rectangle(sb((94, 182, 156, 236)), radius=s(12), fill=rgba(BRASS_DARK, 255), outline=rgba(BRASS_HI, 160), width=s(2))
    add_count("plates_straps_brackets")
    for x in (104, 146):
        draw_rivet(d, x, 211, 4.0, BRASS)

    d.ellipse(sb((24, 21, 226, 223)), fill=rgba(BRASS_DARK, 255), outline=rgba(BRASS_HI, 230), width=s(5))
    d.ellipse(sb((42, 39, 208, 205)), fill=rgba(BRASS, 225), outline=rgba(BRASS_HI, 120), width=s(2))
    d.ellipse(sb((55, 52, 195, 192)), fill=rgba(CREAM, 255), outline=(98, 74, 47, 210), width=s(2))
    d.ellipse(sb((70, 67, 180, 177)), fill=(214, 195, 146, 70))

    for i in range(44):
        a = math.radians(-218 + i * (256 / 43))
        r_outer = 66
        r_inner = 55 if i % 4 else 48
        x0 = cx + math.cos(a) * r_inner
        y0 = cy + math.sin(a) * r_inner
        x1 = cx + math.cos(a) * r_outer
        y1 = cy + math.sin(a) * r_outer
        d.line((s(x0), s(y0), s(x1), s(y1)), fill=(62, 44, 31, 230), width=max(1, s(1.2 if i % 4 else 2.2)))
        add_count("gauge_tick_marks")

    for label, a in [("0", -218), ("40", -150), ("80", -84), ("120", -18), ("160", 38)]:
        aa = math.radians(a)
        tx = cx + math.cos(aa) * 38
        ty = cy + math.sin(aa) * 38
        d.text((s(tx - 10), s(ty - 8)), label, font=FONT_SMALL, fill=(48, 34, 22, 235))

    needle_angle = math.radians(-36)
    d.line((s(cx), s(cy), s(cx + math.cos(needle_angle) * 54), s(cy + math.sin(needle_angle) * 54)), fill=rgba(RED, 245), width=s(3))
    d.ellipse(sb((cx - 10, cy - 10, cx + 10, cy + 10)), fill=rgba(BRASS_DARK, 255), outline=rgba(BRASS_HI, 150), width=s(2))
    d.arc(sb((46, 34, 199, 186)), 206, 305, fill=(255, 255, 238, 98), width=s(7))
    d.pieslice(sb((44, 37, 206, 198)), 208, 300, fill=(255, 255, 255, 38))

    for a in range(0, 360, 45):
        draw_rivet(d, cx + math.cos(math.radians(a)) * 96, cy + math.sin(math.radians(a)) * 96, 3.4, BRASS)

    paste_rotated(gun, layer, (750, 292), -2.5)


def draw_bracket_shadows(gun: Image.Image) -> None:
    shadow = Image.new("RGBA", (RW, RH), (0, 0, 0, 0))
    d = ImageDraw.Draw(shadow, "RGBA")
    d.ellipse(sb((260, 510, 1240, 735)), fill=(0, 0, 0, 125))
    d.ellipse(sb((680, 585, 1730, 1010)), fill=(0, 0, 0, 120))
    d.ellipse(sb((470, 255, 1430, 512)), fill=(0, 0, 0, 76))
    shadow = shadow.filter(ImageFilter.GaussianBlur(s(20)))
    gun.alpha_composite(shadow)


def draw_final_highlights(gun: Image.Image) -> None:
    d = ImageDraw.Draw(gun, "RGBA")
    # Warm key streaks and cool rim flecks that sit over assembled parts.
    highlight_lines = [
        ((315, 326), (1240, 292), (255, 198, 120, 72), 2.2),
        ((396, 534), (1120, 502), (240, 177, 90, 64), 2.0),
        ((982, 332), (1490, 320), (255, 188, 98, 68), 2.0),
        ((1070, 582), (1320, 552), (229, 170, 92, 62), 1.8),
        ((1490, 735), (1720, 862), (226, 152, 88, 54), 2.0),
    ]
    for p0, p1, color, width in highlight_lines:
        d.line((s(p0[0]), s(p0[1]), s(p1[0]), s(p1[1])), fill=color, width=s(width))

    # Soot pockets around seams and under the muzzle.
    soot = Image.new("RGBA", (RW, RH), (0, 0, 0, 0))
    sd = ImageDraw.Draw(soot, "RGBA")
    for box in [
        (250, 378, 440, 474),
        (745, 380, 875, 470),
        (1120, 475, 1510, 575),
        (342, 620, 1120, 720),
        (1035, 635, 1240, 770),
    ]:
        sd.ellipse(sb(box), fill=(0, 0, 0, 72))
    soot = soot.filter(ImageFilter.GaussianBlur(s(14)))
    gun.alpha_composite(soot)


def build_render() -> Image.Image:
    bg = make_background()
    gun = Image.new("RGBA", (RW, RH), (0, 0, 0, 0))

    draw_bracket_shadows(gun)
    draw_leather_hand(gun)

    paste_rotated(gun, draw_lower_tank(), (740, 596), -5.0)
    paste_rotated(gun, draw_barrel(), (760, 374), -4.7)
    paste_rotated(gun, draw_muzzle_stack(), (326, 364), -4.7)
    paste_rotated(gun, draw_side_frame(), (895, 525), -5.0)
    paste_rotated(gun, draw_coil_housing(), (1215, 415), -5.0)

    draw_trigger_and_guard(gun)
    draw_top_valve(gun, 530, 284, 0.92, steam=False)
    draw_top_valve(gun, 1202, 289, 1.05, steam=True)
    draw_top_valve(gun, 1394, 337, 0.75, steam=False)
    draw_gauge(gun)

    # Extra exposed bracket bolts and small top caps.
    d = ImageDraw.Draw(gun, "RGBA")
    extra_fasteners = [
        (445, 447), (500, 442), (570, 438), (640, 434), (702, 430),
        (788, 424), (878, 421), (945, 508), (1016, 503), (1100, 498),
        (1290, 540), (1368, 528), (1480, 494), (1510, 443), (318, 432),
        (363, 420), (410, 401), (1158, 639), (1222, 630), (1286, 620),
    ]
    for cx, cy in extra_fasteners:
        draw_rivet(d, cx, cy, 4.5, BRASS)

    # A small third pressure socket under the coil frame.
    d.ellipse(sb((1328, 568, 1402, 642)), fill=rgba(BRASS_DARK, 255), outline=rgba(BRASS_HI, 145), width=s(3))
    d.ellipse(sb((1348, 588, 1382, 622)), fill=rgba(BLACK_IRON_DARK, 255), outline=rgba(PIPE_HI, 100), width=s(2))
    add_count("steam_pressure_ports")
    add_count("plates_straps_brackets")
    for a in (0, 90, 180, 270):
        draw_rivet(d, 1365 + math.cos(math.radians(a)) * 30, 605 + math.sin(math.radians(a)) * 30, 3.7, BRASS)

    draw_final_highlights(gun)

    gun = scale_layer(gun, 0.86, (960, 540), (0, -2))

    alpha = gun.getchannel("A")
    solid = alpha.point(lambda a: 255 if a >= 190 else 0)
    bbox = solid.getbbox()
    if bbox:
        # Convert supersampled bbox to final dimensions.
        fx0, fy0, fx1, fy1 = [v / SS for v in bbox]
        metrics["silhouette_bbox_px"] = [round(fx0), round(fy0), round(fx1), round(fy1)]
        metrics["frame_occupancy"] = {
            "width_percent": round(((fx1 - fx0) / W) * 100, 1),
            "height_percent": round(((fy1 - fy0) / H) * 100, 1),
        }

    bg.alpha_composite(gun)
    # Subtle photographic bloom on the copper and key-light areas.
    bloom_mask = Image.new("L", (RW, RH), 0)
    bd = ImageDraw.Draw(bloom_mask)
    bd.rounded_rectangle(sb((925, 329, 1515, 520)), radius=s(28), fill=88)
    bd.ellipse(sb((625, 190, 870, 405)), fill=52)
    bloom = bg.filter(ImageFilter.GaussianBlur(s(6)))
    bg = Image.composite(bloom, bg, bloom_mask)
    final = bg.convert("RGB").resize((W, H), Image.Resampling.LANCZOS)
    final = ImageEnhance.Contrast(final).enhance(1.18)
    final = ImageEnhance.Brightness(final).enhance(0.92)
    final = ImageEnhance.Color(final).enhance(1.08)
    yy, xx = np.mgrid[0:H, 0:W]
    vignette = np.sqrt(((xx - W / 2) / (W * 0.66)) ** 2 + ((yy - H / 2) / (H * 0.68)) ** 2)
    v = np.clip((vignette - 0.38) / 0.7, 0, 1)
    arr = np.asarray(final).astype(np.float32)
    arr *= (1.0 - v[..., None] * 0.36)
    arr += np.random.normal(0, 2.0, arr.shape)
    final = Image.fromarray(np.clip(arr, 0, 255).astype(np.uint8), "RGB")
    return final


def fit_image(img: Image.Image, box: tuple[int, int]) -> Image.Image:
    canvas = Image.new("RGB", box, (18, 15, 12))
    tmp = img.copy()
    tmp.thumbnail(box, Image.Resampling.LANCZOS)
    canvas.paste(tmp, ((box[0] - tmp.width) // 2, (box[1] - tmp.height) // 2))
    return canvas


def draw_label(draw: ImageDraw.ImageDraw, xy: tuple[int, int], text: str, font: ImageFont.ImageFont, fill=(239, 201, 136)) -> None:
    draw.text((s(xy[0]), s(xy[1])), text, font=font, fill=fill)


def make_contact_sheet(hero: Image.Image) -> Image.Image:
    sheet_w, sheet_h = 2200, 1500
    sheet = Image.new("RGB", (sheet_w * SS, sheet_h * SS), (16, 13, 10))
    d = ImageDraw.Draw(sheet, "RGBA")

    # Header
    d.rectangle((0, 0, sheet.width, s(116)), fill=(32, 22, 17, 255))
    draw_label(d, (44, 30), "HFLD Recovery 03 - Pressure Pistol Fallback Proof", FONT_BIG)
    draw_label(d, (46, 82), "Non-shipping lookdev proof attempt. Python/Pillow fallback superseded by Unity render direction.", FONT_MED, fill=(218, 188, 143))

    hero_box = (52, 150, 1450, 938)
    hero_fit = fit_image(hero, (hero_box[2] - hero_box[0], hero_box[3] - hero_box[1]))
    sheet.paste(hero_fit.resize((s(hero_fit.width), s(hero_fit.height)), Image.Resampling.LANCZOS), (s(hero_box[0]), s(hero_box[1])))
    d.rectangle(sb(hero_box), outline=(207, 133, 52, 255), width=s(3))
    draw_label(d, (70, 904), "Hero proof render: no annotation layer, 1920x1080 source JPG", FONT_MED, fill=(236, 202, 148))

    concept = Image.open(CONCEPT).convert("RGB")
    source_crop = concept.crop((768, 512, 1536, 1024))
    ref_box = (1510, 150, 2144, 572)
    ref_fit = fit_image(source_crop, (ref_box[2] - ref_box[0], ref_box[3] - ref_box[1]))
    sheet.paste(ref_fit.resize((s(ref_fit.width), s(ref_fit.height)), Image.Resampling.LANCZOS), (s(ref_box[0]), s(ref_box[1])))
    d.rectangle(sb(ref_box), outline=(207, 133, 52, 255), width=s(3))
    draw_label(d, (1528, 536), "North-star gun crop/reference", FONT_MED, fill=(236, 202, 148))

    batch_box = (1510, 618, 2144, 1018)
    if BATCH01.exists():
        batch = Image.open(BATCH01).convert("RGB")
        batch_fit = fit_image(batch, (batch_box[2] - batch_box[0], batch_box[3] - batch_box[1]))
        sheet.paste(batch_fit.resize((s(batch_fit.width), s(batch_fit.height)), Image.Resampling.LANCZOS), (s(batch_box[0]), s(batch_box[1])))
    d.rectangle(sb(batch_box), outline=(138, 83, 43, 255), width=s(3))
    draw_label(d, (1528, 982), "Rejected Batch01 comparison", FONT_MED, fill=(236, 202, 148))

    panel = (52, 990, 1450, 1418)
    d.rectangle(sb(panel), fill=(25, 19, 14, 255), outline=(133, 86, 40, 255), width=s(2))
    draw_label(d, (80, 1022), "Measured Checks", FONT_MED_BOLD, fill=(255, 218, 153))
    counts = metrics["component_counts"]  # type: ignore[index]
    occupancy = metrics.get("frame_occupancy", {"width_percent": "n/a", "height_percent": "n/a"})
    lines = [
        f"Dimensions: {W}x{H} hero JPG; contact sheet {sheet_w}x{sheet_h}",
        f"Gauge: visible cream face, brass bezel, red needle, {counts['gauge_tick_marks']} tick marks",
        f"Coil window: {counts['visible_coil_turns']} visible hot-copper turns",
        f"Fasteners: {counts['visible_fasteners']} visible screws/rivets/bolts generated",
        f"Plates/brackets/straps: {counts['plates_straps_brackets']} distinct pieces",
        f"Ports/valves: {counts['steam_pressure_ports']} pressure ports, {counts['top_valves_caps']} top caps/valves",
        f"Material roles: {len(metrics['material_roles'])} visual roles via {len(metrics['logical_material_slots'])} logical slots",
        f"Frame occupancy: {occupancy['width_percent']}% width, {occupancy['height_percent']}% height",
    ]
    y = 1070
    for line in lines:
        d.text((s(86), s(y)), line, font=FONT_MED, fill=(224, 201, 166, 255))
        y += 39

    gate_panel = (1510, 1060, 2144, 1418)
    d.rectangle(sb(gate_panel), fill=(25, 19, 14, 255), outline=(133, 86, 40, 255), width=s(2))
    draw_label(d, (1538, 1092), "Gate Read", FONT_MED_BOLD, fill=(255, 218, 153))
    gate_lines = [
        "Pass: scope, subject focus, JPG dimensions,",
        "component counts, readable gauge/coil,",
        "dark smoky background, warm key/cool fill.",
        "",
        "Partial/fail: not Unity/true 3D, no real material response,",
        "silhouette still less chunky/layered than source.",
        "",
        "Label: proof attempt, non-shipping, not accepted final art.",
    ]
    y = 1140
    for line in gate_lines:
        d.text((s(1538), s(y)), line, font=FONT_MED, fill=(224, 201, 166, 255))
        y += 38

    d.rectangle((0, s(1440), sheet.width, sheet.height), fill=(32, 22, 17, 255))
    draw_label(d, (46, 1455), f"Output: Documentation/ConceptRenders/{RENDER_OUT.name}", FONT_SMALL, fill=(214, 173, 112))
    draw_label(d, (1514, 1455), "Pressure-pistol-only fallback proof", FONT_SMALL, fill=(214, 173, 112))

    return sheet.resize((sheet_w, sheet_h), Image.Resampling.LANCZOS)


def write_outputs() -> None:
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    PROOF_DIR.mkdir(parents=True, exist_ok=True)

    hero = build_render()
    hero.save(RENDER_OUT, "JPEG", quality=94, subsampling=1, optimize=True)
    sheet = make_contact_sheet(hero)
    sheet.save(SHEET_OUT, "JPEG", quality=92, subsampling=1, optimize=True)

    metrics["dimensions"]["contact_sheet"] = list(sheet.size)  # type: ignore[index]
    with METRICS_OUT.open("w", encoding="utf-8") as f:
        json.dump(metrics, f, indent=2)

    print(json.dumps(metrics, indent=2))


if __name__ == "__main__":
    write_outputs()
