from __future__ import annotations

import hashlib
import json
import math
import random
from datetime import datetime
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont, ImageOps


ROOT = Path(__file__).resolve().parents[3]
ASSET_ROOT = ROOT / "Assets" / "_Project" / "ArtStaging" / "UIHudV1"
DOC_ROOT = ROOT / "Documentation" / "AssetProduction" / "UIHudV1"
SCALE = 4
RNG = random.Random(9321)


PALETTE = {
    "oil_black": "#0c0a08",
    "oil_panel": "#17100b",
    "iron_dark": "#211d19",
    "iron_edge": "#3b332b",
    "brass_dark": "#6e4318",
    "brass": "#b87928",
    "brass_bright": "#f0b95a",
    "brass_hot": "#ffd17a",
    "cream": "#f2dfad",
    "cream_shadow": "#bfa06b",
    "health_red": "#d03424",
    "health_red_dark": "#6d1510",
    "pressure_amber": "#f0a33a",
    "pressure_amber_dark": "#835014",
    "danger_orange": "#ff612e",
    "success_green": "#4ddf72",
    "success_green_dark": "#17662f",
    "steam_cyan": "#a6e7e3",
    "shadow": "#030201",
    "transparent": "#00000000",
}


ASSETS: list[dict] = []


def hx(value: str, alpha: int | None = None) -> tuple[int, int, int, int]:
    value = value.strip("#")
    if len(value) == 8:
        r, g, b, a = (int(value[i : i + 2], 16) for i in range(0, 8, 2))
    else:
        r, g, b = (int(value[i : i + 2], 16) for i in range(0, 6, 2))
        a = 255
    if alpha is not None:
        a = alpha
    return r, g, b, a


def pal(name: str, alpha: int | None = None) -> tuple[int, int, int, int]:
    return hx(PALETTE[name], alpha)


def mix(a: tuple[int, int, int, int], b: tuple[int, int, int, int], t: float) -> tuple[int, int, int, int]:
    return tuple(int(a[i] + (b[i] - a[i]) * t) for i in range(4))


def luminance(rgb: tuple[int, int, int]) -> float:
    values = []
    for c in rgb:
        x = c / 255.0
        values.append(x / 12.92 if x <= 0.03928 else ((x + 0.055) / 1.055) ** 2.4)
    return values[0] * 0.2126 + values[1] * 0.7152 + values[2] * 0.0722


def contrast(fg: str, bg: str) -> float:
    fg_rgb = hx(fg)[:3]
    bg_rgb = hx(bg)[:3]
    l1 = luminance(fg_rgb)
    l2 = luminance(bg_rgb)
    high = max(l1, l2)
    low = min(l1, l2)
    return (high + 0.05) / (low + 0.05)


def sc(v):
    if isinstance(v, (tuple, list)):
        if v and isinstance(v[0], (tuple, list)):
            return [sc(item) for item in v]
        return tuple(int(round(x * SCALE)) for x in v)
    return int(round(v * SCALE))


def new_canvas(size: tuple[int, int], fill: tuple[int, int, int, int] | None = None) -> Image.Image:
    if fill is None:
        fill = (0, 0, 0, 0)
    return Image.new("RGBA", sc(size), fill)


def downsample(img: Image.Image) -> Image.Image:
    return img.resize((img.size[0] // SCALE, img.size[1] // SCALE), Image.Resampling.LANCZOS)


def draw_rr(draw: ImageDraw.ImageDraw, box, radius, fill=None, outline=None, width=1):
    draw.rounded_rectangle(sc(box), radius=sc(radius), fill=fill, outline=outline, width=sc(width))


def draw_ellipse(draw: ImageDraw.ImageDraw, box, fill=None, outline=None, width=1):
    draw.ellipse(sc(box), fill=fill, outline=outline, width=sc(width))


def draw_line(draw: ImageDraw.ImageDraw, points, fill, width=1, joint="curve"):
    draw.line(sc(points), fill=fill, width=sc(width), joint=joint)


def draw_poly(draw: ImageDraw.ImageDraw, points, fill=None, outline=None):
    draw.polygon(sc(points), fill=fill, outline=outline)


def gradient_rect(img: Image.Image, box, c1, c2, vertical=False):
    draw = ImageDraw.Draw(img)
    x0, y0, x1, y1 = sc(box)
    length = max(1, (y1 - y0) if vertical else (x1 - x0))
    for i in range(length):
        t = i / max(1, length - 1)
        c = mix(c1, c2, t)
        if vertical:
            draw.line((x0, y0 + i, x1, y0 + i), fill=c)
        else:
            draw.line((x0 + i, y0, x0 + i, y1), fill=c)


def alpha_paste(base: Image.Image, overlay: Image.Image, xy: tuple[int, int]):
    base.alpha_composite(overlay, dest=sc(xy))


def load_font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont | ImageFont.ImageFont:
    candidates = [
        "C:/Windows/Fonts/bahnschrift.ttf",
        "C:/Windows/Fonts/Bahnschrift.ttf",
        "C:/Windows/Fonts/consolab.ttf" if bold else "C:/Windows/Fonts/consola.ttf",
        "C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf",
    ]
    for path in candidates:
        try:
            return ImageFont.truetype(path, sc(size))
        except OSError:
            continue
    return ImageFont.load_default()


def load_display_font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont | ImageFont.ImageFont:
    candidates = [
        "C:/Windows/Fonts/bahnschrift.ttf",
        "C:/Windows/Fonts/Bahnschrift.ttf",
        "C:/Windows/Fonts/consolab.ttf" if bold else "C:/Windows/Fonts/consola.ttf",
        "C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf",
    ]
    for path in candidates:
        try:
            return ImageFont.truetype(path, size)
        except OSError:
            continue
    return ImageFont.load_default()


def draw_centered_text(
    draw: ImageDraw.ImageDraw,
    box,
    text: str,
    size: int,
    fill,
    bold: bool = False,
    stroke_fill=None,
    stroke_width: int = 0,
):
    font = load_font(size, bold)
    scaled_box = sc(box)
    bbox = draw.textbbox((0, 0), text, font=font, stroke_width=sc(stroke_width))
    tx = scaled_box[0] + (scaled_box[2] - scaled_box[0] - (bbox[2] - bbox[0])) / 2
    ty = scaled_box[1] + (scaled_box[3] - scaled_box[1] - (bbox[3] - bbox[1])) / 2 - sc(1)
    draw.text((tx, ty), text, font=font, fill=fill, stroke_fill=stroke_fill, stroke_width=sc(stroke_width))


def draw_label(draw: ImageDraw.ImageDraw, xy, text: str, size: int, fill, bold: bool = False):
    draw.text(sc(xy), text, font=load_font(size, bold), fill=fill)


def draw_rivet(draw: ImageDraw.ImageDraw, x: float, y: float, r: float = 4.0):
    draw_ellipse(draw, (x - r, y - r, x + r, y + r), fill=pal("brass_bright"), outline=pal("brass_dark"), width=1)
    draw_ellipse(draw, (x - r * 0.45, y - r * 0.45, x + r * 0.45, y + r * 0.45), fill=pal("brass"))
    draw_line(draw, (x - r * 0.5, y, x + r * 0.5, y), fill=pal("brass_dark"), width=1)


def draw_corner_screw(draw: ImageDraw.ImageDraw, x: float, y: float, r: float = 8.0):
    draw_ellipse(draw, (x - r, y - r, x + r, y + r), fill=pal("iron_dark"), outline=pal("brass_bright"), width=2)
    draw_line(draw, (x - r * 0.55, y - r * 0.2, x + r * 0.55, y + r * 0.2), fill=pal("cream_shadow"), width=2)


def draw_panel_shell(
    draw: ImageDraw.ImageDraw,
    size: tuple[int, int],
    outer_radius: int = 12,
    border: int = 14,
    face_alpha: int = 228,
    bright: bool = False,
):
    w, h = size
    draw_rr(draw, (2, 2, w - 2, h - 2), outer_radius, fill=pal("shadow", 190), outline=pal("brass_dark"), width=3)
    draw_rr(draw, (5, 5, w - 5, h - 5), outer_radius - 2, fill=pal("brass" if bright else "brass_dark"), outline=pal("brass_bright"), width=2)
    draw_rr(draw, (border, border, w - border, h - border), max(4, outer_radius - 5), fill=pal("oil_panel", face_alpha), outline=pal("iron_edge"), width=2)
    draw_rr(draw, (border + 5, border + 5, w - border - 5, h - border - 5), max(3, outer_radius - 7), fill=None, outline=pal("brass", 170), width=1)
    for x, y in ((18, 18), (w - 18, 18), (18, h - 18), (w - 18, h - 18)):
        draw_corner_screw(draw, x, y, 6)


def draw_segmented_fill(size, c_left, c_right, segment_count: int, border_color, glass=True) -> Image.Image:
    w, h = size
    img = new_canvas(size)
    grad = new_canvas(size)
    gradient_rect(grad, (0, 0, w, h), c_left, c_right)
    mask = Image.new("L", sc(size), 0)
    mdraw = ImageDraw.Draw(mask)
    mdraw.rounded_rectangle(sc((1, 1, w - 1, h - 1)), radius=sc(h / 2), fill=255)
    img.alpha_composite(Image.composite(grad, Image.new("RGBA", grad.size, (0, 0, 0, 0)), mask))
    draw = ImageDraw.Draw(img)
    for i in range(1, segment_count):
        x = w * i / segment_count
        draw_line(draw, (x, 3, x, h - 3), fill=border_color, width=2)
    if glass:
        draw_rr(draw, (1, 1, w - 1, h * 0.46), h / 2, fill=(255, 255, 255, 45))
    draw_rr(draw, (1, 1, w - 1, h - 1), h / 2, outline=pal("cream_shadow", 170), width=1)
    return downsample(img)


def save_asset(
    rel_path: str,
    img: Image.Image,
    asset_type: str,
    status: str,
    ready: bool,
    import_settings: str,
    nine_slice: str,
    notes: str,
):
    path = ASSET_ROOT / rel_path
    path.parent.mkdir(parents=True, exist_ok=True)
    img.save(path)
    ASSETS.append(
        {
            "file": str(path.relative_to(ROOT)).replace("\\", "/"),
            "name": path.stem,
            "asset_type": asset_type,
            "status": status,
            "ready_for_integration": ready,
            "size_px": list(img.size),
            "import_settings": import_settings,
            "nine_slice": nine_slice,
            "notes": notes,
            "sha256": sha256(path),
        }
    )


def save_doc_image(rel_path: str, img: Image.Image):
    path = DOC_ROOT / rel_path
    path.parent.mkdir(parents=True, exist_ok=True)
    img.save(path)


def sha256(path: Path) -> str:
    h = hashlib.sha256()
    with path.open("rb") as f:
        for chunk in iter(lambda: f.read(65536), b""):
            h.update(chunk)
    return h.hexdigest()


def render_health_frame() -> Image.Image:
    size = (512, 96)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    draw_panel_shell(draw, size, outer_radius=14, border=13)
    draw_rr(draw, (86, 32, 444, 64), 16, fill=pal("shadow", 220), outline=pal("brass_bright"), width=2)
    draw_rr(draw, (96, 39, 434, 57), 8, fill=pal("oil_black", 235), outline=pal("iron_edge"), width=1)
    draw_ellipse(draw, (20, 20, 78, 78), fill=pal("iron_dark"), outline=pal("brass_bright"), width=3)
    draw_ellipse(draw, (31, 30, 67, 70), fill=pal("health_red_dark"), outline=pal("cream_shadow"), width=2)
    draw_rr(draw, (44, 22, 54, 29), 2, fill=pal("brass_bright"))
    draw_line(draw, (40, 50, 58, 50), fill=pal("cream"), width=4)
    draw_line(draw, (49, 41, 49, 59), fill=pal("cream"), width=4)
    for x in range(108, 425, 34):
        draw_line(draw, (x, 34, x, 62), fill=pal("brass_dark", 150), width=1)
    for x in (92, 450):
        draw_ellipse(draw, (x - 13, 35, x + 13, 61), fill=pal("brass_dark"), outline=pal("brass_bright"), width=2)
    return downsample(img)


def render_ammo_frame() -> Image.Image:
    size = (512, 96)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    draw_panel_shell(draw, size, outer_radius=14, border=13)
    draw_rr(draw, (68, 32, 388, 64), 16, fill=pal("shadow", 220), outline=pal("brass_bright"), width=2)
    draw_rr(draw, (78, 39, 378, 57), 8, fill=pal("oil_black", 235), outline=pal("iron_edge"), width=1)
    for i in range(4):
        x = 410 + i * 18
        draw_rr(draw, (x, 30, x + 12, 66), 5, fill=pal("pressure_amber_dark"), outline=pal("brass_bright"), width=2)
        draw_ellipse(draw, (x + 1, 25, x + 11, 35), fill=pal("brass_bright"), outline=pal("brass_dark"), width=1)
    draw_ellipse(draw, (20, 24, 60, 64), fill=pal("cream"), outline=pal("brass_bright"), width=3)
    for angle in range(-120, 121, 30):
        cx, cy, r = 40, 44, 17
        rad = math.radians(angle - 90)
        x0 = cx + math.cos(rad) * (r - 4)
        y0 = cy + math.sin(rad) * (r - 4)
        x1 = cx + math.cos(rad) * r
        y1 = cy + math.sin(rad) * r
        draw_line(draw, (x0, y0, x1, y1), fill=pal("iron_dark"), width=1)
    draw_line(draw, (40, 44, 51, 36), fill=pal("danger_orange"), width=2)
    draw_ellipse(draw, (36, 40, 44, 48), fill=pal("brass_dark"))
    return downsample(img)


def render_boss_frame() -> Image.Image:
    size = (768, 96)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    draw_panel_shell(draw, size, outer_radius=14, border=13)
    draw_rr(draw, (56, 45, 712, 72), 14, fill=pal("shadow", 225), outline=pal("brass_bright"), width=2)
    draw_rr(draw, (70, 52, 698, 65), 6, fill=pal("oil_black", 235), outline=pal("iron_edge"), width=1)
    for x in range(92, 690, 48):
        draw_line(draw, (x, 47, x, 70), fill=pal("brass_dark", 150), width=1)
    for x in (36, 732):
        draw_ellipse(draw, (x - 16, 32, x + 16, 64), fill=pal("iron_dark"), outline=pal("danger_orange"), width=2)
        draw_line(draw, (x - 10, 48, x + 10, 48), fill=pal("danger_orange"), width=2)
        draw_line(draw, (x, 38, x, 58), fill=pal("danger_orange"), width=2)
    return downsample(img)


def render_objective_backplate() -> Image.Image:
    size = (640, 72)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    draw_panel_shell(draw, size, outer_radius=10, border=10)
    draw_ellipse(draw, (18, 18, 54, 54), fill=pal("pressure_amber_dark"), outline=pal("brass_bright"), width=3)
    draw_ellipse(draw, (28, 28, 44, 44), fill=pal("pressure_amber", 230))
    draw_rr(draw, (64, 22, 616, 52), 6, fill=pal("oil_black", 210), outline=pal("iron_edge"), width=1)
    return downsample(img)


def render_prompt_backplate() -> Image.Image:
    size = (640, 80)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    draw_panel_shell(draw, size, outer_radius=12, border=11)
    draw_rr(draw, (22, 18, 80, 62), 9, fill=pal("cream", 235), outline=pal("brass_bright"), width=2)
    draw_centered_text(draw, (22, 18, 80, 62), "E", 27, pal("iron_dark"), True)
    draw_rr(draw, (92, 24, 614, 56), 6, fill=pal("oil_black", 215), outline=pal("iron_edge"), width=1)
    return downsample(img)


def render_lamp(state: str) -> Image.Image:
    size = (96, 96)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    draw_ellipse(draw, (8, 8, 88, 88), fill=pal("iron_dark"), outline=pal("brass_bright"), width=4)
    for i in range(10):
        ang = math.radians(i * 36)
        x = 48 + math.cos(ang) * 32
        y = 48 + math.sin(ang) * 32
        draw_rivet(draw, x, y, 3)
    if state == "on":
        glass = pal("success_green", 240)
        glow = pal("success_green", 60)
    elif state == "denied":
        glass = pal("danger_orange", 240)
        glow = pal("danger_orange", 65)
    else:
        glass = pal("pressure_amber", 150)
        glow = pal("pressure_amber", 35)
    draw_ellipse(draw, (18, 18, 78, 78), fill=glow)
    draw_ellipse(draw, (26, 24, 70, 72), fill=glass, outline=pal("cream_shadow"), width=2)
    draw_rr(draw, (43, 18, 53, 78), 3, fill=pal("cream", 55))
    if state == "denied":
        draw_line(draw, (34, 34, 62, 62), fill=pal("cream"), width=5)
        draw_line(draw, (62, 34, 34, 62), fill=pal("cream"), width=5)
    elif state == "on":
        draw_line(draw, (32, 50, 43, 62, 66, 35), fill=pal("cream"), width=5)
    return downsample(img)


def render_dial_frame() -> Image.Image:
    size = (256, 256)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    draw_ellipse(draw, (8, 8, 248, 248), fill=pal("shadow", 180), outline=pal("brass_dark"), width=3)
    draw_ellipse(draw, (16, 16, 240, 240), fill=pal("brass_dark"), outline=pal("brass_bright"), width=5)
    draw_ellipse(draw, (34, 34, 222, 222), fill=pal("cream"), outline=pal("iron_dark"), width=3)
    draw_ellipse(draw, (45, 45, 211, 211), fill=pal("cream", 245), outline=pal("cream_shadow"), width=2)
    cx, cy = 128, 134
    for i in range(31):
        angle = math.radians(-135 + i * 9)
        long_tick = i % 5 == 0
        r0 = 72 if long_tick else 78
        r1 = 84
        x0 = cx + math.cos(angle) * r0
        y0 = cy + math.sin(angle) * r0
        x1 = cx + math.cos(angle) * r1
        y1 = cy + math.sin(angle) * r1
        draw_line(draw, (x0, y0, x1, y1), fill=pal("iron_dark"), width=2 if long_tick else 1)
    draw_centered_text(draw, (86, 166, 170, 192), "AMMO", 17, pal("iron_dark"), True)
    draw_centered_text(draw, (89, 88, 167, 116), "PSI", 16, pal("brass_dark"), True)
    draw_ellipse(draw, (116, 122, 140, 146), fill=pal("brass_dark"), outline=pal("brass_bright"), width=2)
    for x, y in ((30, 30), (226, 30), (30, 226), (226, 226)):
        draw_corner_screw(draw, x, y, 7)
    return downsample(img)


def render_dial_arc() -> Image.Image:
    size = (256, 256)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    # Full-pressure source: Unity can radial-fill or tint this as needed.
    for width, alpha, offset in ((18, 190, 0), (12, 235, 3), (5, 255, 6)):
        draw.arc(sc((47 + offset, 47 + offset, 209 - offset, 209 - offset)), start=225, end=495, fill=pal("pressure_amber", alpha), width=sc(width))
    draw.arc(sc((47, 47, 209, 209)), start=225, end=495, fill=pal("danger_orange", 240), width=sc(4))
    return downsample(img)


def render_dial_needle() -> Image.Image:
    size = (256, 256)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    cx, cy = 128, 134
    angle = math.radians(-35)
    tip = (cx + math.cos(angle) * 74, cy + math.sin(angle) * 74)
    base_l = (cx + math.cos(angle + math.pi / 2) * 5, cy + math.sin(angle + math.pi / 2) * 5)
    base_r = (cx + math.cos(angle - math.pi / 2) * 5, cy + math.sin(angle - math.pi / 2) * 5)
    draw_poly(draw, (base_l, tip, base_r), fill=pal("danger_orange"), outline=pal("health_red_dark"))
    draw_ellipse(draw, (116, 122, 140, 146), fill=pal("iron_dark"), outline=pal("brass_bright"), width=2)
    return downsample(img)


def render_panel(size=(768, 384), bright=False) -> Image.Image:
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    draw_panel_shell(draw, size, outer_radius=18, border=38, face_alpha=235, bright=bright)
    w, h = size
    for x in range(74, w - 72, 70):
        draw_line(draw, (x, 31, x + 22, 31), fill=pal("brass_bright", 120), width=2)
        draw_line(draw, (x, h - 31, x + 22, h - 31), fill=pal("brass_bright", 120), width=2)
    for y in range(82, h - 78, 64):
        draw_line(draw, (31, y, 31, y + 18), fill=pal("brass_bright", 100), width=2)
        draw_line(draw, (w - 31, y, w - 31, y + 18), fill=pal("brass_bright", 100), width=2)
    return downsample(img)


def render_button(state: str) -> Image.Image:
    size = (320, 64)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    face_alpha = {"normal": 235, "hover": 245, "pressed": 255}[state]
    bright = state != "normal"
    draw_panel_shell(draw, size, outer_radius=9, border=11, face_alpha=face_alpha, bright=bright)
    if state == "pressed":
        draw_rr(draw, (18, 18, 302, 46), 6, fill=pal("pressure_amber_dark", 80), outline=pal("danger_orange", 170), width=1)
    elif state == "hover":
        draw_rr(draw, (18, 18, 302, 46), 6, fill=pal("brass_bright", 34), outline=pal("brass_hot", 160), width=1)
    return downsample(img)


def render_slider_track() -> Image.Image:
    size = (360, 40)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    draw_rr(draw, (3, 8, 357, 32), 12, fill=pal("brass_dark"), outline=pal("brass_bright"), width=2)
    draw_rr(draw, (14, 15, 346, 25), 5, fill=pal("oil_black", 240), outline=pal("iron_edge"), width=1)
    for x in range(38, 331, 32):
        draw_line(draw, (x, 12, x, 29), fill=pal("cream_shadow", 110), width=1)
    return downsample(img)


def render_slider_handle() -> Image.Image:
    size = (48, 48)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    draw_ellipse(draw, (3, 3, 45, 45), fill=pal("brass_dark"), outline=pal("brass_bright"), width=3)
    draw_ellipse(draw, (13, 13, 35, 35), fill=pal("iron_dark"), outline=pal("brass"), width=2)
    draw_line(draw, (24, 8, 24, 40), fill=pal("brass_hot"), width=3)
    draw_line(draw, (8, 24, 40, 24), fill=pal("brass_hot"), width=3)
    draw_ellipse(draw, (19, 19, 29, 29), fill=pal("cream"))
    return downsample(img)


def render_corner_cap() -> Image.Image:
    size = (64, 64)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    draw_rr(draw, (3, 3, 61, 61), 10, fill=pal("brass_dark"), outline=pal("brass_bright"), width=3)
    draw_ellipse(draw, (17, 17, 47, 47), fill=pal("iron_dark"), outline=pal("brass"), width=2)
    draw_corner_screw(draw, 32, 32, 8)
    return downsample(img)


def render_edge_piece(horizontal=True) -> Image.Image:
    size = (256, 32) if horizontal else (32, 256)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    if horizontal:
        draw_rr(draw, (0, 4, 256, 28), 8, fill=pal("brass_dark"), outline=pal("brass_bright"), width=2)
        draw_line(draw, (0, 16, 256, 16), fill=pal("brass", 180), width=3)
        for x in range(28, 236, 40):
            draw_rivet(draw, x, 16, 3)
    else:
        draw_rr(draw, (4, 0, 28, 256), 8, fill=pal("brass_dark"), outline=pal("brass_bright"), width=2)
        draw_line(draw, (16, 0, 16, 256), fill=pal("brass", 180), width=3)
        for y in range(28, 236, 40):
            draw_rivet(draw, 16, y, 3)
    return downsample(img)


def render_reticle(style: str) -> Image.Image:
    size = (64, 64)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    if style == "brass_crosshair":
        col = pal("brass_hot", 230)
        outline = pal("shadow", 180)
        for pts in [
            (32, 8, 32, 22),
            (32, 42, 32, 56),
            (8, 32, 22, 32),
            (42, 32, 56, 32),
        ]:
            draw_line(draw, pts, fill=outline, width=5)
            draw_line(draw, pts, fill=col, width=2)
        draw_ellipse(draw, (29, 29, 35, 35), fill=pal("cream"), outline=pal("shadow"), width=1)
    else:
        draw_ellipse(draw, (22, 22, 42, 42), outline=pal("steam_cyan", 170), width=2)
        draw_ellipse(draw, (30, 30, 34, 34), fill=pal("cream"))
        for angle in range(0, 360, 90):
            rad = math.radians(angle)
            x0 = 32 + math.cos(rad) * 15
            y0 = 32 + math.sin(rad) * 15
            x1 = 32 + math.cos(rad) * 24
            y1 = 32 + math.sin(rad) * 24
            draw_line(draw, (x0, y0, x1, y1), fill=pal("brass_hot", 210), width=2)
    return downsample(img)


def draw_icon_base(draw: ImageDraw.ImageDraw):
    draw_ellipse(draw, (7, 7, 89, 89), fill=pal("shadow", 170), outline=pal("brass_dark"), width=2)
    draw_ellipse(draw, (12, 12, 84, 84), fill=pal("iron_dark", 235), outline=pal("brass_bright"), width=3)
    draw_ellipse(draw, (22, 22, 74, 74), fill=pal("oil_black", 210), outline=pal("brass", 150), width=1)


def render_icon(kind: str) -> Image.Image:
    size = (96, 96)
    img = new_canvas(size)
    draw = ImageDraw.Draw(img)
    draw_icon_base(draw)
    if kind == "interact_e":
        draw_rr(draw, (28, 28, 68, 68), 7, fill=pal("cream"), outline=pal("brass_bright"), width=2)
        draw_centered_text(draw, (28, 28, 68, 68), "E", 28, pal("iron_dark"), True)
    elif kind == "gear_key":
        cx, cy = 45, 36
        for i in range(10):
            a = math.radians(i * 36)
            x = cx + math.cos(a) * 22
            y = cy + math.sin(a) * 22
            draw_rr(draw, (x - 4, y - 4, x + 4, y + 4), 1, fill=pal("brass_bright"))
        draw_ellipse(draw, (26, 17, 64, 55), fill=pal("brass"), outline=pal("brass_hot"), width=2)
        draw_ellipse(draw, (36, 27, 54, 45), fill=pal("iron_dark"))
        draw_line(draw, (45, 55, 45, 76), fill=pal("brass_hot"), width=7)
        draw_line(draw, (45, 72, 61, 72), fill=pal("brass_hot"), width=7)
        draw_line(draw, (45, 64, 56, 64), fill=pal("brass_hot"), width=5)
    elif kind == "valve":
        draw_ellipse(draw, (24, 24, 72, 72), outline=pal("brass_hot"), width=5)
        for angle in range(0, 360, 45):
            rad = math.radians(angle)
            draw_line(draw, (48, 48, 48 + math.cos(rad) * 24, 48 + math.sin(rad) * 24), fill=pal("brass_bright"), width=4)
        draw_ellipse(draw, (39, 39, 57, 57), fill=pal("iron_dark"), outline=pal("cream_shadow"), width=2)
    elif kind == "lift":
        draw_rr(draw, (29, 23, 67, 70), 2, fill=pal("iron_dark"), outline=pal("brass_hot"), width=3)
        for x in (39, 48, 57):
            draw_line(draw, (x, 26, x, 67), fill=pal("brass", 160), width=2)
        draw_poly(draw, ((48, 16), (36, 29), (43, 29), (43, 40), (53, 40), (53, 29), (60, 29)), fill=pal("success_green"))
    elif kind == "ammo":
        for i in range(3):
            x = 34 + i * 11
            draw_rr(draw, (x, 27, x + 8, 66), 4, fill=pal("pressure_amber_dark"), outline=pal("brass_hot"), width=2)
            draw_ellipse(draw, (x, 22, x + 8, 31), fill=pal("brass_hot"))
    elif kind == "health":
        draw_rr(draw, (34, 20, 62, 74), 8, fill=pal("health_red_dark"), outline=pal("cream_shadow"), width=3)
        draw_rr(draw, (40, 14, 56, 24), 3, fill=pal("brass_bright"))
        draw_line(draw, (40, 48, 56, 48), fill=pal("cream"), width=5)
        draw_line(draw, (48, 40, 48, 56), fill=pal("cream"), width=5)
    elif kind == "warning":
        draw_poly(draw, ((48, 18), (74, 72), (22, 72)), fill=pal("danger_orange"), outline=pal("cream"))
        draw_line(draw, (48, 36, 48, 55), fill=pal("oil_black"), width=5)
        draw_ellipse(draw, (45, 60, 51, 66), fill=pal("oil_black"))
    elif kind == "secret":
        draw_rr(draw, (28, 34, 68, 72), 4, fill=pal("iron_edge"), outline=pal("brass_hot"), width=3)
        draw_line(draw, (32, 42, 64, 42), fill=pal("brass_dark"), width=2)
        draw_ellipse(draw, (42, 49, 54, 61), fill=pal("shadow"), outline=pal("cream_shadow"), width=1)
        draw_line(draw, (48, 58, 48, 66), fill=pal("shadow"), width=4)
        draw_poly(draw, ((48, 20), (58, 32), (52, 32), (52, 39), (44, 39), (44, 32), (38, 32)), fill=pal("pressure_amber"))
    elif kind == "pause":
        draw_rr(draw, (35, 25, 44, 71), 2, fill=pal("cream"), outline=pal("brass_dark"), width=1)
        draw_rr(draw, (52, 25, 61, 71), 2, fill=pal("cream"), outline=pal("brass_dark"), width=1)
    elif kind == "mouse_right":
        draw_rr(draw, (31, 19, 65, 76), 16, fill=pal("cream"), outline=pal("brass_bright"), width=3)
        draw_line(draw, (48, 20, 48, 42), fill=pal("iron_dark"), width=2)
        draw_rr(draw, (49, 23, 62, 43), 6, fill=pal("danger_orange"))
        draw_ellipse(draw, (45, 35, 51, 45), fill=pal("iron_dark"))
    return downsample(img)


def checker(size, cell=12):
    img = Image.new("RGBA", size, pal("oil_black", 255))
    draw = ImageDraw.Draw(img)
    for y in range(0, size[1], cell):
        for x in range(0, size[0], cell):
            if ((x // cell) + (y // cell)) % 2 == 0:
                draw.rectangle((x, y, x + cell - 1, y + cell - 1), fill=pal("oil_panel", 255))
    return img


def make_contact_sheet(asset_entries: list[dict]) -> Image.Image:
    items = [entry for entry in asset_entries if not entry["file"].endswith(".json")]
    cell_w, cell_h = 280, 200
    cols = 4
    rows = math.ceil(len(items) / cols)
    img = Image.new("RGBA", (cols * cell_w, rows * cell_h + 80), pal("oil_black"))
    draw = ImageDraw.Draw(img)
    draw.rectangle((0, 0, img.size[0], 80), fill=pal("iron_dark"))
    draw.text((24, 16), "UI HUD V1 CONTACT SHEET", font=load_display_font(24, True), fill=pal("brass_hot"))
    draw.text((24, 48), "Staged transparent PNG kit - not wired into scenes/code", font=load_display_font(13), fill=pal("cream"))
    for idx, entry in enumerate(items):
        col = idx % cols
        row = idx // cols
        x = col * cell_w
        y = 80 + row * cell_h
        draw.rectangle((x, y, x + cell_w, y + cell_h), fill=pal("oil_panel"), outline=pal("iron_edge"))
        path = ROOT / entry["file"]
        asset = Image.open(path).convert("RGBA")
        thumb_bg = checker((cell_w - 36, cell_h - 58), 10)
        thumb = ImageOps.contain(asset, (cell_w - 46, cell_h - 76), Image.Resampling.LANCZOS)
        tx = x + 18 + (thumb_bg.size[0] - thumb.size[0]) // 2
        ty = y + 14 + (thumb_bg.size[1] - thumb.size[1]) // 2
        img.alpha_composite(thumb_bg, (x + 18, y + 12))
        img.alpha_composite(thumb, (tx, ty))
        label = entry["name"].replace("_", " ")
        if len(label) > 31:
            label = label[:28] + "..."
        draw.text((x + 18, y + cell_h - 40), label, font=load_display_font(11), fill=pal("cream"))
        status = "READY" if entry["ready_for_integration"] else "ROUGH"
        draw.text((x + 18, y + cell_h - 22), status, font=load_display_font(10, True), fill=pal("success_green") if status == "READY" else pal("pressure_amber"))
    return img


def make_hud_mockup() -> Image.Image:
    size = (1920, 1080)
    img = Image.new("RGBA", size, pal("oil_black"))
    draw = ImageDraw.Draw(img)
    # Non-shipping neutral backdrop, just enough to judge scale and contrast.
    for y in range(0, 1080, 80):
        shade = 16 + (y // 80) % 2 * 7
        draw.rectangle((0, y, 1920, y + 80), fill=(shade, shade - 3, shade - 7, 255))
    for x in range(0, 1920, 160):
        draw.line((x, 0, x + 480, 1080), fill=(56, 39, 22, 80), width=4)
    draw.rectangle((0, 0, 1920, 1080), outline=pal("brass_dark"), width=8)

    def place(rel, xy, scale=1.0):
        asset = Image.open(ASSET_ROOT / rel).convert("RGBA")
        if scale != 1.0:
            asset = asset.resize((int(asset.size[0] * scale), int(asset.size[1] * scale)), Image.Resampling.LANCZOS)
        img.alpha_composite(asset, xy)

    place("Gauges/HUD_HealthGauge_Frame_512x96.png", (28, 956))
    place("Gauges/HUD_HealthGauge_Fill_Red_384x32.png", (126, 990))
    draw.text((132, 966), "HEALTH 100/100", font=load_display_font(18, True), fill=pal("cream"), stroke_width=2, stroke_fill=pal("shadow"))

    place("Gauges/HUD_PressureAmmoGauge_Frame_512x96.png", (1380, 956))
    place("Gauges/HUD_PressureAmmoGauge_Fill_Amber_384x32.png", (1464, 990))
    draw.text((1510, 966), "AMMO 30", font=load_display_font(18, True), fill=pal("cream"), stroke_width=2, stroke_fill=pal("shadow"))

    place("Icons/HUD_KeyLamp_On_96x96.png", (912, 952), 0.8)
    draw.text((840, 1010), "GEAR KEY YES", font=load_display_font(19, True), fill=pal("cream"), stroke_width=2, stroke_fill=pal("shadow"))

    place("Gauges/HUD_BossPressureGauge_Frame_768x96.png", (576, 24))
    place("Gauges/HUD_BossPressureGauge_Fill_Red_704x24.png", (608, 74))
    draw.text((776, 42), "GOVERNOR WARDEN", font=load_display_font(22, True), fill=pal("cream"), stroke_width=2, stroke_fill=pal("shadow"))

    place("Panels/HUD_ObjectiveBackplate_640x72.png", (24, 112))
    draw.text((112, 136), "OBJECTIVE: ROUTE PIPE PRESSURE", font=load_display_font(20, True), fill=pal("cream"), stroke_width=2, stroke_fill=pal("shadow"))

    place("Reticles/RETICLE_BrassCrosshair_64x64.png", (928, 508))
    place("Panels/HUD_PromptBackplate_640x80.png", (640, 650))
    draw.text((748, 675), "TURN VALVE", font=load_display_font(24, True), fill=pal("cream"), stroke_width=2, stroke_fill=pal("shadow"))
    return img


def generate_assets():
    for directory in [
        ASSET_ROOT / "Gauges",
        ASSET_ROOT / "Icons",
        ASSET_ROOT / "Panels",
        ASSET_ROOT / "Reticles",
        ASSET_ROOT / "Previews",
        DOC_ROOT,
    ]:
        directory.mkdir(parents=True, exist_ok=True)

    sprite_ui = "Texture Type: Sprite (2D and UI); Sprite Mode: Single; sRGB on; Alpha Is Transparency on; Mesh Type Full Rect; Mip Maps off; Filter Bilinear; Compression None for source."
    sprite_icon = "Texture Type: Sprite (2D and UI); Sprite Mode: Single; sRGB on; Alpha Is Transparency on; Mesh Type Tight or Full Rect; Mip Maps off; Filter Bilinear; Compression None for source."
    fill_ui = "Texture Type: Sprite (2D and UI); Sprite Mode: Single; Image Type Filled compatible; sRGB on; Alpha Is Transparency on; Mesh Type Full Rect; Mip Maps off; Filter Bilinear."

    save_asset("Gauges/HUD_HealthGauge_Frame_512x96.png", render_health_frame(), "HUD gauge frame", "ready", True, sprite_ui, "Suggested border: L/R 34 px, T/B 22 px. Use Sliced Image for flexible width.", "Drop-in health backplate/frame for lower-left HUD.")
    save_asset("Gauges/HUD_HealthGauge_Fill_Red_384x32.png", draw_segmented_fill((384, 32), pal("health_red"), pal("danger_orange"), 18, pal("health_red_dark", 200)), "HUD gauge fill", "ready", True, fill_ui, "Do not nine-slice; use Image.Type.Filled Horizontal, origin Left.", "Segmented health/pressure-fluid fill source.")
    save_asset("Gauges/HUD_PressureAmmoGauge_Frame_512x96.png", render_ammo_frame(), "HUD gauge frame", "ready", True, sprite_ui, "Suggested border: L/R 34 px, T/B 22 px. Use Sliced Image for flexible width.", "Horizontal ammo gauge compatible with current HUD fill behavior.")
    save_asset("Gauges/HUD_PressureAmmoGauge_Fill_Amber_384x32.png", draw_segmented_fill((384, 32), pal("pressure_amber"), pal("brass_hot"), 15, pal("pressure_amber_dark", 210)), "HUD gauge fill", "ready", True, fill_ui, "Do not nine-slice; use Image.Type.Filled Horizontal, origin Left.", "Pressure-ammo fill source.")
    save_asset("Gauges/HUD_PressureAmmoDial_Frame_256x256.png", render_dial_frame(), "HUD dial gauge", "rough", False, sprite_ui, "No nine-slice. Use as fixed-size dial face.", "Radial ammo pressure dial concept. Requires UI script/layout work before integration.")
    save_asset("Gauges/HUD_PressureAmmoDial_FillArc_256x256.png", render_dial_arc(), "HUD dial gauge fill", "rough", False, fill_ui, "No nine-slice. Use radial filled Image or mask.", "Radial arc source for future pressure dial.")
    save_asset("Gauges/HUD_PressureAmmoDial_Needle_256x256.png", render_dial_needle(), "HUD dial gauge needle", "rough", False, sprite_ui, "No nine-slice. Rotate RectTransform around center.", "Separate needle sprite for future pressure dial.")
    save_asset("Gauges/HUD_BossPressureGauge_Frame_768x96.png", render_boss_frame(), "Boss HUD gauge frame", "ready", True, sprite_ui, "Suggested border: L/R 42 px, T/B 22 px. Use Sliced Image if boss names vary.", "Top-center boss pressure gauge frame.")
    save_asset("Gauges/HUD_BossPressureGauge_Fill_Red_704x24.png", draw_segmented_fill((704, 24), pal("health_red_dark"), pal("danger_orange"), 22, pal("shadow", 190)), "Boss HUD gauge fill", "ready", True, fill_ui, "Do not nine-slice; use Image.Type.Filled Horizontal, origin Left.", "Boss pressure fill compatible with current bossFillImage.")

    save_asset("Panels/HUD_ObjectiveBackplate_640x72.png", render_objective_backplate(), "HUD objective panel", "ready", True, sprite_ui, "Suggested border: L/R 32 px, T/B 20 px. Use Sliced Image.", "Objective HUD backplate with lamp socket.")
    save_asset("Panels/HUD_PromptBackplate_640x80.png", render_prompt_backplate(), "HUD prompt panel", "ready", True, sprite_ui, "Suggested border: L/R 34 px, T/B 22 px. Use Sliced Image.", "Interaction prompt panel with built-in E keycap slot.")
    save_asset("Panels/PANEL_Menu_BrassPanel_768x384.png", render_panel((768, 384)), "Menu panel", "ready", True, sprite_ui, "Suggested border: 48 px on all sides.", "Large main/pause/settings brass panel.")
    save_asset("Panels/PANEL_Menu_Header_768x96.png", render_panel((768, 96), bright=True), "Menu header panel", "ready", True, sprite_ui, "Suggested border: L/R 48 px, T/B 22 px.", "Header or title strip for menu UI.")
    for state in ("normal", "hover", "pressed"):
        save_asset(f"Panels/PANEL_Menu_Button_{state.title()}_320x64.png", render_button(state), "Menu button", "ready", True, sprite_ui, "Suggested border: L/R 20 px, T/B 16 px.", f"Button state sprite: {state}.")
    save_asset("Panels/PANEL_Menu_SliderTrack_360x40.png", render_slider_track(), "Menu slider", "ready", True, sprite_ui, "Suggested border: L/R 18 px, T/B 10 px.", "Slider track/backplate.")
    save_asset("Panels/PANEL_Menu_SliderHandle_48x48.png", render_slider_handle(), "Menu slider", "ready", True, sprite_icon, "No nine-slice.", "Valve-wheel slider handle.")
    save_asset("Panels/PANEL_Menu_CornerCap_64x64.png", render_corner_cap(), "Menu panel piece", "rough", False, sprite_icon, "No nine-slice; use as decorative overlay.", "Standalone corner cap for custom assembled panels.")
    save_asset("Panels/PANEL_Menu_EdgeHorizontal_256x32.png", render_edge_piece(True), "Menu panel piece", "rough", False, sprite_ui, "Tile or stretch horizontally; border optional L/R 16 px.", "Optional horizontal panel rail.")
    save_asset("Panels/PANEL_Menu_EdgeVertical_32x256.png", render_edge_piece(False), "Menu panel piece", "rough", False, sprite_ui, "Tile or stretch vertically; border optional T/B 16 px.", "Optional vertical panel rail.")

    for state in ("off", "on", "denied"):
        save_asset(f"Icons/HUD_KeyLamp_{state.title()}_96x96.png", render_lamp(state), "HUD key/objective lamp", "ready", True, sprite_icon, "No nine-slice. Fixed-size icon.", f"Gear/objective lamp state: {state}.")

    for style in ("brass_crosshair", "pressure_pinpoint"):
        save_asset(f"Reticles/RETICLE_{style.title().replace('_', '')}_64x64.png", render_reticle(style), "Reticle", "ready", True, sprite_icon, "No nine-slice. Fixed 64 px reticle.", f"Transparent reticle variant: {style}.")

    icon_kinds = [
        "interact_e",
        "gear_key",
        "valve",
        "lift",
        "ammo",
        "health",
        "warning",
        "secret",
        "pause",
        "mouse_right",
    ]
    for kind in icon_kinds:
        save_asset(f"Icons/ICON_Prompt_{kind.title().replace('_', '')}_96x96.png", render_icon(kind), "Prompt icon", "ready", True, sprite_icon, "No nine-slice. Fixed 96 px icon.", f"Prompt icon for {kind.replace('_', ' ')}.")

    contact = make_contact_sheet(ASSETS)
    contact_asset = ASSET_ROOT / "Previews" / "PREVIEW_UIHudV1_ContactSheet.png"
    contact.save(contact_asset)
    save_doc_image("PREVIEW_UIHudV1_ContactSheet.png", contact)
    ASSETS.append(
        {
            "file": str(contact_asset.relative_to(ROOT)).replace("\\", "/"),
            "name": contact_asset.stem,
            "asset_type": "Preview sheet",
            "status": "review",
            "ready_for_integration": False,
            "size_px": list(contact.size),
            "import_settings": "Review-only preview. Do not import into runtime UI.",
            "nine_slice": "None.",
            "notes": "Contact sheet for art review.",
            "sha256": sha256(contact_asset),
        }
    )

    mockup = make_hud_mockup()
    mock_asset = ASSET_ROOT / "Previews" / "PREVIEW_UIHudV1_HUDMockup_1920x1080.png"
    mockup.save(mock_asset)
    save_doc_image("PREVIEW_UIHudV1_HUDMockup_1920x1080.png", mockup)
    ASSETS.append(
        {
            "file": str(mock_asset.relative_to(ROOT)).replace("\\", "/"),
            "name": mock_asset.stem,
            "asset_type": "Preview mockup",
            "status": "review",
            "ready_for_integration": False,
            "size_px": list(mockup.size),
            "import_settings": "Review-only preview. Do not import into runtime UI.",
            "nine_slice": "None.",
            "notes": "Composited HUD scale mockup.",
            "sha256": sha256(mock_asset),
        }
    )

    write_manifest()


def write_manifest():
    generated = datetime.now().astimezone().isoformat(timespec="seconds")
    ready = [a for a in ASSETS if a["ready_for_integration"]]
    rough = [a for a in ASSETS if not a["ready_for_integration"] and not a["asset_type"].lower().startswith("preview")]
    previews = [a for a in ASSETS if a["asset_type"].lower().startswith("preview")]
    manifest = {
        "generated": generated,
        "scope": {
            "asset_root": str(ASSET_ROOT.relative_to(ROOT)).replace("\\", "/"),
            "documentation_root": str(DOC_ROOT.relative_to(ROOT)).replace("\\", "/"),
            "staging_only": True,
            "no_scene_or_code_integration": True,
        },
        "palette": PALETTE,
        "accessibility_contrast": {
            "cream_on_oil_panel": round(contrast(PALETTE["cream"], PALETTE["oil_panel"]), 2),
            "brass_hot_on_oil_panel": round(contrast(PALETTE["brass_hot"], PALETTE["oil_panel"]), 2),
            "pressure_amber_on_oil_black": round(contrast(PALETTE["pressure_amber"], PALETTE["oil_black"]), 2),
            "danger_orange_on_oil_black": round(contrast(PALETTE["danger_orange"], PALETTE["oil_black"]), 2),
            "health_red_on_oil_black": round(contrast(PALETTE["health_red"], PALETTE["oil_black"]), 2),
            "success_green_on_oil_black": round(contrast(PALETTE["success_green"], PALETTE["oil_black"]), 2),
        },
        "assets": ASSETS,
        "ready_for_integration": [a["file"] for a in ready],
        "rough_or_requires_layout_work": [a["file"] for a in rough],
        "previews": [a["file"] for a in previews],
        "later_integration_tasks": [
            "Import runtime sprites as Sprite (2D and UI), disable mip maps, preserve alpha, and keep source compression lossless until readability is checked in-game.",
            "Replace current solid-color HUD Image backplates/fills with these sprites in a dedicated integration branch; do not alter gameplay logic.",
            "Set horizontal fills to Image.Type Filled/Horizontal with Left origin; use frames as Sliced where manifest borders are supplied.",
            "If adopting the circular ammo dial, add layout and needle/arc driving separately because current HUDController drives horizontal fillAmount.",
            "Tune CanvasScaler reference resolution and anchored offsets against 16:9, 16:10, and ultrawide before committing final placements.",
            "Verify red/orange pressure warnings with color-blind filters; supplement danger states with icon shape and text, not color alone.",
        ],
    }

    asset_json = ASSET_ROOT / "UIHudV1_AssetManifest.json"
    doc_json = DOC_ROOT / "UIHudV1_MANIFEST.json"
    asset_json.write_text(json.dumps(manifest, indent=2), encoding="utf-8")
    doc_json.write_text(json.dumps(manifest, indent=2), encoding="utf-8")

    lines = [
        "# UIHudV1 Manifest",
        "",
        f"Generated: {generated}",
        "",
        "Scope: staged V1 HUD/UI art assets only. No gameplay scripts, Unity scenes, existing UI code, README, BUILD_STATUS, WORK_LEDGER, ConceptRenders, or active Bulwark/pressure-pistol files were modified.",
        "",
        "## Visual Target",
        "",
        "- Dark oil/iron instrument-panel base with worn brass trim.",
        "- Cream enamel for readable gauge faces and labels.",
        "- Red/orange for health, pressure danger, boss pressure, and denied states.",
        "- Amber/brass for ammo, pressure hardware, interactable prompts, and useful machinery.",
        "- Green reserved for success/restored/key-acquired states.",
        "",
        "## Palette",
        "",
        "| Token | Hex | Use |",
        "| --- | --- | --- |",
    ]
    palette_uses = {
        "oil_black": "Deep transparent-edge shadow and UI backdrops",
        "oil_panel": "Main panel fill",
        "iron_dark": "Dark metal plates and icon interiors",
        "brass": "Primary brass trim",
        "brass_bright": "Raised brass highlights",
        "brass_hot": "High-readability brass highlights",
        "cream": "Text and gauge enamel",
        "health_red": "Health/boss pressure fill",
        "pressure_amber": "Ammo/pressure fill",
        "danger_orange": "Warning/denied accents",
        "success_green": "Acquired/unlocked/success state",
        "steam_cyan": "Secondary precision/steam reticle cue",
    }
    for key, use in palette_uses.items():
        lines.append(f"| `{key}` | `{PALETTE[key]}` | {use} |")

    acc = manifest["accessibility_contrast"]
    lines.extend(
        [
            "",
            "## Accessibility Contrast Notes",
            "",
            f"- Cream text on oil panel: {acc['cream_on_oil_panel']}:1, suitable for HUD labels.",
            f"- Hot brass on oil panel: {acc['brass_hot_on_oil_panel']}:1, suitable for highlights and selected states.",
            f"- Pressure amber on oil black: {acc['pressure_amber_on_oil_black']}:1, strong enough for fills/icons.",
            f"- Danger orange on oil black: {acc['danger_orange_on_oil_black']}:1, strong enough for warning accents.",
            f"- Health red on oil black: {acc['health_red_on_oil_black']}:1, acceptable for large fills but should not be the only critical signal.",
            f"- Success green on oil black: {acc['success_green_on_oil_black']}:1, strong enough for acquired/unlocked lamps.",
            "- Danger, key, and prompt states include shape changes or iconography so the UI is not color-only.",
            "",
            "## Unity Import Settings",
            "",
            "- Texture Type: `Sprite (2D and UI)`.",
            "- Sprite Mode: `Single`.",
            "- Mesh Type: `Full Rect` for gauge frames, fills, panels, and sliced sprites; `Tight` is acceptable for standalone icons/reticles.",
            "- sRGB: on for all color PNGs.",
            "- Alpha Is Transparency: on.",
            "- Mip Maps: off for UI.",
            "- Filter Mode: `Bilinear`.",
            "- Compression: `None` for source review; later platform variants can use high-quality compression after readability checks.",
            "- Max Size: 1024 for HUD/menu panels and 256 or 512 for icons/reticles, matching source dimensions.",
            "",
            "## Nine-Slice Notes",
            "",
            "| Family | Suggested Border | Notes |",
            "| --- | --- | --- |",
            "| Health/ammo frames | L/R 34 px, T/B 22 px | Sliced, keep fill window center flexible. |",
            "| Boss pressure frame | L/R 42 px, T/B 22 px | Sliced if width changes; current source supports 768 px top-center use. |",
            "| Objective/prompt backplates | L/R 32-34 px, T/B 20-22 px | Sliced, leave icon/lamp socket fixed by layout. |",
            "| Large menu panel | 48 px all sides | Sliced for main/pause/settings containers. |",
            "| Menu buttons | L/R 20 px, T/B 16 px | Sliced state sprites: normal, hover, pressed. |",
            "| Slider track | L/R 18 px, T/B 10 px | Sliced or fixed width. |",
            "| Icons, lamps, reticles, dial | None | Fixed-size sprites. |",
            "",
            "## Ready For Integration",
            "",
        ]
    )
    for item in ready:
        lines.append(f"- `{item['file']}` - {item['notes']}")

    lines.extend(["", "## Rough Or Requires Layout Work", ""])
    for item in rough:
        lines.append(f"- `{item['file']}` - {item['notes']}")

    lines.extend(["", "## Preview Files", ""])
    for item in previews:
        lines.append(f"- `{item['file']}` - {item['notes']}")
    lines.extend(
        [
            "- `Documentation/AssetProduction/UIHudV1/PREVIEW_UIHudV1_ContactSheet.png` - documentation copy.",
            "- `Documentation/AssetProduction/UIHudV1/PREVIEW_UIHudV1_HUDMockup_1920x1080.png` - documentation copy.",
            "",
            "## Full Asset Inventory",
            "",
            "| File | Size | Status | Integration | Nine-slice |",
            "| --- | ---: | --- | --- | --- |",
        ]
    )
    for item in ASSETS:
        integration = "ready" if item["ready_for_integration"] else "review/rough"
        lines.append(f"| `{item['file']}` | {item['size_px'][0]}x{item['size_px'][1]} | {item['status']} | {integration} | {item['nine_slice']} |")

    lines.extend(["", "## Later Integration Tasks", ""])
    for task in manifest["later_integration_tasks"]:
        lines.append(f"- {task}")

    (DOC_ROOT / "UIHudV1_MANIFEST.md").write_text("\n".join(lines) + "\n", encoding="utf-8")
    (DOC_ROOT / "UIHudV1_WORK_LOG.md").write_text(
        "\n".join(
            [
                "# UIHudV1 Work Log",
                "",
                f"Generated: {generated}",
                "",
                "- Read `Documentation/PROJECT_SPEC.md` and `Documentation/AAA_ASSET_CATALOG.md` for current UI direction and color language.",
                "- Reviewed `Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png` for instrument-panel HUD treatment.",
                "- Read current HUD/menu code paths as reference only: `Assets/_Project/Scripts/UI/HUDController.cs`, `MainMenuController.cs`, `PauseMenuController.cs`, and HUD/menu construction portions of `Assets/_Project/Editor/V0SceneBuilder.cs`.",
                "- Generated deterministic transparent PNG art kit under `Assets/_Project/ArtStaging/UIHudV1/`.",
                "- Generated documentation manifest, JSON manifest, contact sheet, and HUD mockup under `Documentation/AssetProduction/UIHudV1/`.",
                "- No Unity scenes, gameplay scripts, existing UI code, active Bulwark files, pressure-pistol proof files, README, BUILD_STATUS, WORK_LEDGER, or ConceptRenders were modified.",
            ]
        )
        + "\n",
        encoding="utf-8",
    )


if __name__ == "__main__":
    generate_assets()
