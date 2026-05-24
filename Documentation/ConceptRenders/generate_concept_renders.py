from pathlib import Path
from PIL import Image, ImageDraw, ImageFont, ImageFilter, ImageEnhance, ImageOps
import math
import random
import textwrap

ROOT = Path(r"D:\__MY APPS\Unity Doom")
SRC = ROOT / "Documentation" / "AssetPreviews"
OUT = ROOT / "Documentation" / "ConceptRenders"
OUT.mkdir(parents=True, exist_ok=True)

random.seed(87)


def load_font(size, bold=False):
    names = ["segoeuib.ttf", "arialbd.ttf"] if bold else ["segoeui.ttf", "arial.ttf"]
    for name in names:
        path = Path(r"C:\Windows\Fonts") / name
        if path.exists():
            return ImageFont.truetype(str(path), size)
    return ImageFont.load_default()


F = {
    "h1": load_font(50, True),
    "h2": load_font(34, True),
    "h3": load_font(25, True),
    "body": load_font(21),
    "small": load_font(16),
    "tiny": load_font(13),
    "label": load_font(18, True),
}

COLORS = {
    "bg0": (14, 12, 10),
    "bg1": (35, 28, 20),
    "ink": (238, 224, 190),
    "muted": (168, 150, 120),
    "brass": (173, 118, 45),
    "brass_hi": (235, 178, 86),
    "copper": (145, 76, 37),
    "iron": (39, 42, 41),
    "iron_hi": (95, 96, 87),
    "amber": (255, 171, 60),
    "red": (157, 42, 30),
    "green": (72, 137, 92),
    "cream": (216, 197, 154),
    "wood": (92, 52, 30),
}

TEXTURES = {}
for key, filename in {
    "brass_pipe": "T_Steam_BrassPipe_preview.png",
    "hazard_pipe": "T_Steam_BrassHazardPipe_preview.png",
    "stone": "T_Steam_OilDarkStone_preview.png",
    "iron": "T_Steam_RivetedIron_preview.png",
}.items():
    path = SRC / filename
    if path.exists():
        TEXTURES[key] = Image.open(path).convert("RGB")
    else:
        TEXTURES[key] = Image.new("RGB", (128, 128), (36, 35, 33))

PREVIEW_FILES = [
    "concept_art_contact_sheet.jpg",
    "material_color_swatches.jpg",
    "procedural_texture_contact_sheet.png",
    "T_Steam_BrassHazardPipe_preview.png",
    "T_Steam_BrassPipe_preview.png",
    "T_Steam_OilDarkStone_preview.png",
    "T_Steam_RivetedIron_preview.png",
]


def gradient(size, top, bottom):
    width, height = size
    image = Image.new("RGB", size, top)
    px = image.load()
    for y in range(height):
        t = y / max(1, height - 1)
        color = tuple(int(top[i] * (1 - t) + bottom[i] * t) for i in range(3))
        for x in range(width):
            px[x, y] = color
    return image.convert("RGBA")


def add_noise(image, amount=12):
    image = image.convert("RGBA")
    noise = Image.new("RGBA", image.size, (0, 0, 0, 0))
    px = noise.load()
    for y in range(0, image.height, 2):
        for x in range(0, image.width, 2):
            value = random.randint(-amount, amount)
            alpha = random.randint(8, 25)
            if value >= 0:
                px[x, y] = (value, value, value, alpha)
            else:
                px[x, y] = (0, 0, 0, min(30, alpha + abs(value)))
    return Image.alpha_composite(image, noise)


def add_vignette(image, strength=145):
    image = image.convert("RGBA")
    width, height = image.size
    mask = Image.new("L", (width, height), 0)
    draw = ImageDraw.Draw(mask)
    max_radius = int(math.hypot(width, height) / 2)
    cx, cy = width // 2, height // 2
    for radius in range(max_radius, 0, -12):
        alpha = int(strength * (1 - radius / max_radius) ** 1.6)
        draw.ellipse((cx - radius, cy - radius, cx + radius, cy + radius), fill=alpha)
    dark = Image.new("RGBA", image.size, (0, 0, 0, strength))
    inverse = ImageOps.invert(mask)
    image.alpha_composite(Image.composite(dark, Image.new("RGBA", image.size, (0, 0, 0, 0)), inverse))
    return image


def radial_glow(image, xy, radius, color, alpha=170):
    overlay = Image.new("RGBA", image.size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(overlay)
    x, y = xy
    for r in range(radius, 0, -8):
        t = 1 - r / radius
        a = int(alpha * (t ** 1.7))
        draw.ellipse((x - r, y - r, x + r, y + r), fill=(*color, a))
    image.alpha_composite(overlay)


def header(image, title, subtitle, status="MOCKUP"):
    draw = ImageDraw.Draw(image)
    draw.rectangle((0, 0, image.width, 116), fill=(10, 9, 8, 205))
    draw.text((42, 24), title, font=F["h1"], fill=COLORS["ink"])
    draw.text((46, 78), subtitle, font=F["body"], fill=COLORS["muted"])
    badge = f"{status} - REVIEW ONLY"
    text_width = draw.textlength(badge, font=F["label"])
    box = (image.width - text_width - 72, 32, image.width - 38, 70)
    draw.rounded_rectangle(box, radius=8, fill=(84, 46, 28, 235), outline=(202, 139, 65, 255), width=1)
    draw.text((image.width - text_width - 55, 40), badge, font=F["label"], fill=(252, 218, 152))


def footer(image, sources):
    draw = ImageDraw.Draw(image)
    draw.rectangle((0, image.height - 58, image.width, image.height), fill=(8, 7, 6, 220))
    text = "Sources: " + sources
    if len(text) > 190:
        text = text[:187] + "..."
    draw.text((42, image.height - 39), text, font=F["small"], fill=COLORS["muted"])


def tile_texture(name, size, scale=128, brightness=1.0, contrast=1.05):
    base = TEXTURES[name].resize((scale, scale), Image.Resampling.BICUBIC)
    base = ImageEnhance.Brightness(base).enhance(brightness)
    base = ImageEnhance.Contrast(base).enhance(contrast)
    tiled = Image.new("RGB", size)
    for y in range(0, size[1], scale):
        for x in range(0, size[0], scale):
            tiled.paste(base, (x, y))
    return tiled.convert("RGBA")


def paste_texture_poly(image, name, poly, scale=128, brightness=1.0, contrast=1.05, alpha=255):
    texture = tile_texture(name, image.size, scale, brightness, contrast)
    if alpha < 255:
        texture.putalpha(alpha)
    mask = Image.new("L", image.size, 0)
    ImageDraw.Draw(mask).polygon(poly, fill=255)
    image.alpha_composite(Image.composite(texture, Image.new("RGBA", image.size, (0, 0, 0, 0)), mask))


def draw_rivets(draw, points, radius=5, fill=(196, 136, 57), outline=(42, 28, 15)):
    for x, y in points:
        draw.ellipse((x - radius, y - radius, x + radius, y + radius), fill=fill, outline=outline)
        draw.arc((x - radius + 1, y - radius + 1, x + radius - 1, y + radius - 1), 210, 40, fill=(245, 190, 91), width=1)


def draw_pipe(draw, p1, p2, width, body, highlight=None, shadow=True):
    x1, y1 = p1
    x2, y2 = p2
    if shadow:
        draw.line((x1 + 7, y1 + 8, x2 + 7, y2 + 8), fill=(0, 0, 0, 115), width=width + 7)
    draw.line((x1, y1, x2, y2), fill=body, width=width)
    if highlight:
        if abs(x2 - x1) >= abs(y2 - y1):
            draw.line((x1, y1 - width // 4, x2, y2 - width // 4), fill=highlight, width=max(2, width // 6))
        else:
            draw.line((x1 - width // 4, y1, x2 - width // 4, y2), fill=highlight, width=max(2, width // 6))
    radius = width // 2
    for x, y in [(x1, y1), (x2, y2)]:
        draw.ellipse((x - radius, y - radius, x + radius, y + radius), fill=body, outline=(27, 21, 16), width=2)
        draw.ellipse((x - radius + 5, y - radius + 5, x + radius - 5, y + radius - 5), outline=highlight or body, width=1)


def draw_valve(draw, cx, cy, radius, angle=0):
    draw.ellipse((cx - radius - 12, cy - radius - 12, cx + radius + 12, cy + radius + 12), fill=(0, 0, 0, 80))
    draw.ellipse((cx - radius, cy - radius, cx + radius, cy + radius), outline=COLORS["brass_hi"], width=12)
    draw.ellipse((cx - radius + 10, cy - radius + 10, cx + radius - 10, cy + radius - 10), outline=COLORS["copper"], width=5)
    for i in range(6):
        a = angle + i * math.tau / 6
        x = cx + math.cos(a) * (radius - 15)
        y = cy + math.sin(a) * (radius - 15)
        draw.line((cx, cy, x, y), fill=COLORS["brass"], width=10)
        draw.line((cx, cy - 2, x, y - 2), fill=COLORS["brass_hi"], width=3)
    draw.ellipse((cx - 22, cy - 22, cx + 22, cy + 22), fill=COLORS["iron"], outline=COLORS["brass_hi"], width=4)
    draw_rivets(draw, [(cx + math.cos(i * math.tau / 8) * radius, cy + math.sin(i * math.tau / 8) * radius) for i in range(8)], 4)


def draw_gauge(draw, cx, cy, radius, needle=0.65):
    draw.ellipse((cx - radius - 10, cy - radius - 10, cx + radius + 10, cy + radius + 10), fill=(0, 0, 0, 100))
    draw.ellipse((cx - radius, cy - radius, cx + radius, cy + radius), fill=COLORS["brass"], outline=(42, 25, 11), width=4)
    draw.ellipse((cx - radius + 12, cy - radius + 12, cx + radius - 12, cy + radius - 12), fill=COLORS["cream"], outline=(68, 48, 28), width=2)
    draw.arc((cx - radius + 24, cy - radius + 24, cx + radius - 24, cy + radius - 24), 205, 300, fill=COLORS["green"], width=8)
    draw.arc((cx - radius + 24, cy - radius + 24, cx + radius - 24, cy + radius - 24), 300, 350, fill=COLORS["red"], width=8)
    for i in range(9):
        a = math.radians(205 + i * 18)
        x1 = cx + math.cos(a) * (radius - 28)
        y1 = cy + math.sin(a) * (radius - 28)
        x2 = cx + math.cos(a) * (radius - 39)
        y2 = cy + math.sin(a) * (radius - 39)
        draw.line((x1, y1, x2, y2), fill=(44, 35, 25), width=2)
    a = math.radians(205 + needle * 145)
    draw.line((cx, cy, cx + math.cos(a) * (radius - 44), cy + math.sin(a) * (radius - 44)), fill=(20, 19, 17), width=5)
    draw.ellipse((cx - 7, cy - 7, cx + 7, cy + 7), fill=(30, 26, 20))


def draw_floor_grid(draw, y_base=840):
    for x in range(-300, 2300, 160):
        draw.line((x, y_base, 960 + (x - 960) * 0.17, 420), fill=(82, 67, 48, 80), width=2)
    for y in range(470, 940, 52):
        t = (y - 420) / 520
        draw.line((160 + 360 * t, y, 1760 - 360 * t, y), fill=(80, 64, 45, 72), width=2)


def shadowed_panel(image, box, fill=(27, 23, 19, 225), outline=(122, 84, 45, 255), radius=10):
    draw = ImageDraw.Draw(image)
    x1, y1, x2, y2 = box
    draw.rounded_rectangle((x1 + 8, y1 + 10, x2 + 8, y2 + 10), radius=radius, fill=(0, 0, 0, 85))
    draw.rounded_rectangle(box, radius=radius, fill=fill, outline=outline, width=1)


def object_stage(title, subtitle):
    image = gradient((1920, 1080), (20, 17, 13), (54, 39, 24))
    radial_glow(image, (1380, 340), 420, COLORS["amber"], 95)
    draw = ImageDraw.Draw(image, "RGBA")
    draw_floor_grid(draw, 900)
    header(image, title, subtitle)
    return image


def save(image, name):
    image = add_noise(image, 5)
    image = add_vignette(image, 110)
    image.convert("RGB").save(OUT / name, quality=92, optimize=True, progressive=True)


def render_pipe_valve():
    image = object_stage("Pipe, Valve, And Gauge Kit", "Procedural mock render using preview brass, hazard pipe, iron, and oil-stone materials")
    draw = ImageDraw.Draw(image, "RGBA")
    draw.polygon([(420, 770), (1430, 690), (1620, 805), (650, 930)], fill=(18, 18, 17, 210), outline=(120, 83, 43, 180))
    for i in range(8):
        y = 430 + i * 52
        draw_pipe(draw, (360, y), (1500, y - 70), 28 if i % 2 else 36, COLORS["copper"] if i % 2 else COLORS["iron"], COLORS["brass_hi"] if i % 2 else COLORS["iron_hi"])
    draw_pipe(draw, (310, 585), (1580, 515), 74, COLORS["brass"], COLORS["brass_hi"])
    for x in range(480, 1460, 180):
        draw.rounded_rectangle((x - 18, 515, x + 18, 618), radius=8, fill=COLORS["iron"], outline=COLORS["brass_hi"], width=2)
        draw.line((x - 10, 525, x + 10, 610), fill=COLORS["red"], width=6)
        draw_rivets(draw, [(x, 527), (x, 607)], 4)
    draw_valve(draw, 930, 540, 122, angle=0.35)
    draw_gauge(draw, 1200, 355, 78, needle=0.82)
    draw_pipe(draw, (1110, 410), (1005, 488), 28, COLORS["copper"], COLORS["brass_hi"])
    for i, x in enumerate([560, 1320]):
        draw_gauge(draw, x, 405 + i * 30, 56, needle=0.35 + i * 0.25)
    shadowed_panel(image, (84, 815, 540, 990), fill=(18, 15, 12, 220))
    draw.text((112, 845), "Read Targets", font=F["h3"], fill=COLORS["ink"])
    for j, note in enumerate(["Oversized valve silhouette", "Cream gauge faces with red/green states", "Hazard bands stay legible at corridor range"]):
        y = 890 + j * 32
        draw.ellipse((115, y + 5, 127, y + 17), fill=COLORS["brass_hi"])
        draw.text((142, y), note, font=F["body"], fill=COLORS["muted"])
    footer(image, "AssetPreviews/T_Steam_BrassPipe_preview.png, T_Steam_BrassHazardPipe_preview.png, T_Steam_RivetedIron_preview.png; PROP-PIPE-001, PROP-MECH-001 briefs")
    save(image, "RENDER_OBJECT_pipe_valve_gauge_mockup.jpg")


def render_pressure_pistol():
    image = object_stage("Pressure Pistol Viewmodel", "Procedural silhouette mockup for WPN-001 before staged mesh delivery")
    draw = ImageDraw.Draw(image, "RGBA")
    for i, label in enumerate(["front read", "side read", "pickup read"]):
        x = 112 + i * 245
        shadowed_panel(image, (x, 150, x + 205, 330), fill=(23, 21, 18, 210), outline=(94, 65, 37, 220), radius=8)
        draw.text((x + 22, 294), label, font=F["small"], fill=COLORS["muted"])
        cx, cy = x + 102, 230
        if i == 0:
            draw.ellipse((cx - 38, cy - 45, cx + 38, cy + 45), fill=COLORS["brass"], outline=COLORS["iron"], width=5)
            draw_gauge(draw, cx, cy, 25, 0.55)
        elif i == 1:
            draw_pipe(draw, (cx - 70, cy - 8), (cx + 80, cy - 8), 28, COLORS["brass"], COLORS["brass_hi"], shadow=False)
            draw.rounded_rectangle((cx - 45, cy + 10, cx + 35, cy + 55), radius=9, fill=COLORS["wood"], outline=COLORS["brass_hi"], width=2)
        else:
            draw.rounded_rectangle((cx - 70, cy - 22, cx + 68, cy + 28), radius=14, fill=COLORS["iron"], outline=COLORS["brass"], width=4)
            draw.line((cx - 26, cy + 22, cx - 45, cy + 70), fill=COLORS["wood"], width=24)
    draw.rounded_rectangle((650, 455, 1170, 585), radius=36, fill=COLORS["iron"], outline=(27, 21, 15), width=5)
    draw.rounded_rectangle((690, 430, 1045, 535), radius=26, fill=(78, 69, 56), outline=COLORS["brass"], width=5)
    draw.rounded_rectangle((735, 452, 1010, 512), radius=18, fill=COLORS["brass"], outline=(74, 45, 18), width=3)
    for x in range(765, 1000, 42):
        draw_rivets(draw, [(x, 482)], 5)
    draw_pipe(draw, (1070, 485), (1518, 475), 52, COLORS["iron"], COLORS["iron_hi"])
    draw_pipe(draw, (1060, 425), (1500, 390), 28, COLORS["brass"], COLORS["brass_hi"])
    draw_pipe(draw, (1030, 610), (1355, 682), 36, COLORS["copper"], COLORS["brass_hi"])
    draw.ellipse((1468, 428, 1588, 520), fill=COLORS["brass"], outline=(42, 25, 12), width=5)
    draw.ellipse((1502, 454, 1570, 494), fill=(22, 20, 18), outline=COLORS["brass_hi"], width=2)
    draw.polygon([(720, 565), (872, 600), (815, 820), (650, 780)], fill=COLORS["wood"], outline=(42, 23, 13))
    draw.line((702, 600, 835, 635), fill=(154, 95, 46), width=5)
    draw.line((692, 665, 823, 705), fill=(42, 24, 15), width=4)
    draw.arc((892, 555, 1010, 700), 35, 175, fill=COLORS["brass_hi"], width=7)
    draw.arc((923, 585, 980, 680), 55, 165, fill=(18, 16, 14), width=6)
    draw_gauge(draw, 1125, 365, 62, 0.73)
    draw_valve(draw, 625, 520, 44, angle=0.1)
    radial_glow(image, (1560, 475), 190, COLORS["amber"], 90)
    draw.line((1570, 474, 1690, 448), fill=(255, 187, 76, 115), width=4)
    shadowed_panel(image, (96, 790, 530, 994), fill=(18, 15, 12, 220))
    draw.text((126, 822), "Silhouette Notes", font=F["h3"], fill=COLORS["ink"])
    for j, note in enumerate(["Short crowned muzzle", "Rear pressure gauge", "Looped pressure tube", "Walnut grip block"]):
        y = 870 + j * 30
        draw.rectangle((128, y + 7, 140, y + 19), fill=COLORS["brass_hi"])
        draw.text((158, y), note, font=F["body"], fill=COLORS["muted"])
    footer(image, "AssetPreviews brass/iron previews; WPN-001 Pressure Pistol brief")
    save(image, "RENDER_OBJECT_pressure_pistol_mockup.jpg")


def render_steam_scattergun():
    image = object_stage("Steam Scattergun Display", "Procedural silhouette mockup for WPN-002 optional pickup and viewmodel")
    draw = ImageDraw.Draw(image, "RGBA")
    draw.polygon([(480, 755), (1350, 690), (1540, 792), (700, 930)], fill=(20, 18, 16, 230), outline=COLORS["brass"], width=2)
    draw.rounded_rectangle((835, 660, 1168, 747), radius=12, fill=COLORS["iron"], outline=COLORS["brass_hi"], width=3)
    for dy in [0, 46, -46]:
        draw_pipe(draw, (645, 440 + dy // 4), (1450, 377 + dy // 5), 48, COLORS["iron"], COLORS["iron_hi"])
        draw.ellipse((1410, 338 + dy // 5, 1516, 422 + dy // 5), fill=COLORS["brass"], outline=(40, 25, 13), width=5)
        draw.ellipse((1442, 360 + dy // 5, 1500, 401 + dy // 5), fill=(18, 16, 14), outline=COLORS["brass_hi"], width=2)
    draw.rounded_rectangle((560, 480, 1105, 617), radius=32, fill=COLORS["iron"], outline=(31, 25, 20), width=5)
    draw.rounded_rectangle((625, 454, 1045, 530), radius=18, fill=COLORS["brass"], outline=(68, 42, 20), width=3)
    for x in range(665, 1028, 52):
        draw_rivets(draw, [(x, 492)], 5)
    draw.rounded_rectangle((720, 612, 1160, 702), radius=25, fill=COLORS["wood"], outline=COLORS["brass_hi"], width=3)
    for x in range(760, 1135, 44):
        draw.line((x, 622, x - 18, 694), fill=(48, 27, 17), width=5)
    for i in range(8):
        x = 480 + i * 26
        draw.arc((x, 395, x + 84, 570), 100, 260, fill=COLORS["copper"], width=12)
        draw.arc((x, 396, x + 84, 570), 110, 160, fill=COLORS["brass_hi"], width=3)
    draw_gauge(draw, 1180, 536, 58, 0.68)
    draw_valve(draw, 475, 590, 58, angle=0.25)
    for i in range(6):
        x = 1065 + i * 38
        draw.rounded_rectangle((x, 615, x + 25, 690), radius=8, fill=(129, 32, 23), outline=COLORS["brass_hi"], width=2)
        draw.rectangle((x, 615, x + 25, 635), fill=COLORS["brass_hi"])
    radial_glow(image, (1475, 380), 230, COLORS["amber"], 80)
    shadowed_panel(image, (98, 770, 545, 987), fill=(18, 15, 12, 225))
    draw.text((128, 805), "Pickup Read", font=F["h3"], fill=COLORS["ink"])
    for j, note in enumerate(["Triple barrel cluster", "Large brass top rib", "Walnut pump grip", "Rear pressure coil", "Shell rack silhouette"]):
        y = 850 + j * 27
        draw.ellipse((130, y + 7, 142, y + 19), fill=COLORS["brass_hi"])
        draw.text((158, y), note, font=F["body"], fill=COLORS["muted"])
    footer(image, "AssetPreviews brass/iron previews; WPN-002 Steam Scattergun brief")
    save(image, "RENDER_OBJECT_steam_scattergun_mockup.jpg")


def render_scrapper():
    image = object_stage("Scrapper Mechanical Enemy", "Procedural front/side silhouette mockup for ENEMY-001")
    draw = ImageDraw.Draw(image, "RGBA")
    cx = 985
    radial_glow(image, (cx, 355), 300, COLORS["amber"], 70)
    for side in [-1, 1]:
        hip = (cx + side * 92, 610)
        knee = (cx + side * 150, 740)
        foot = (cx + side * 205, 844)
        draw.line((hip, knee), fill=COLORS["iron"], width=34)
        draw.line((knee, foot), fill=COLORS["iron"], width=30)
        draw.line((hip[0] - side * 7, hip[1] - 5, knee[0] - side * 7, knee[1] - 5), fill=COLORS["iron_hi"], width=5)
        draw.polygon([(foot[0] - 55 * side, foot[1] + 30), (foot[0] + 70 * side, foot[1] + 18), (foot[0] + 45 * side, foot[1] - 18), (foot[0] - 42 * side, foot[1] - 8)], fill=COLORS["iron"], outline=COLORS["brass"], width=2)
    draw.ellipse((cx - 155, 345, cx + 155, 665), fill=COLORS["iron"], outline=COLORS["brass"], width=8)
    draw.rounded_rectangle((cx - 115, 385, cx + 115, 625), radius=50, fill=(57, 54, 45), outline=COLORS["brass_hi"], width=4)
    for y in range(420, 616, 38):
        draw.line((cx - 108, y, cx + 108, y), fill=(20, 19, 17, 90), width=4)
    draw_rivets(draw, [(cx - 120, 400 + i * 38) for i in range(6)] + [(cx + 120, 400 + i * 38) for i in range(6)], 5)
    draw.rounded_rectangle((cx - 118, 238, cx + 118, 385), radius=38, fill=COLORS["brass"], outline=(49, 30, 14), width=5)
    draw.rectangle((cx - 78, 293, cx + 78, 342), fill=(27, 22, 16), outline=COLORS["brass_hi"], width=2)
    radial_glow(image, (cx, 317), 110, (255, 92, 29), 135)
    draw.ellipse((cx - 30, 298, cx + 30, 336), fill=(255, 99, 30), outline=(63, 18, 10), width=2)
    draw.line((cx - 74, 277, cx + 74, 254), fill=COLORS["iron"], width=10)
    for side in [-1, 1]:
        shoulder = (cx + side * 145, 455)
        elbow = (cx + side * 270, 540)
        wrist = (cx + side * 352, 705)
        draw.line((shoulder, elbow), fill=COLORS["copper"], width=28)
        draw.line((elbow, wrist), fill=COLORS["iron"], width=34)
        draw.ellipse((elbow[0] - 30, elbow[1] - 30, elbow[0] + 30, elbow[1] + 30), fill=COLORS["brass"], outline=COLORS["iron"], width=4)
        tip = (wrist[0] + side * 105, wrist[1] - 22)
        draw.polygon([(wrist[0], wrist[1] - 35), (tip[0], tip[1]), (wrist[0] + side * 35, wrist[1] + 10)], fill=(165, 174, 160), outline=(24, 24, 23))
        draw.polygon([(wrist[0] - side * 5, wrist[1] + 34), (tip[0] - side * 8, wrist[1] + 66), (wrist[0] + side * 38, wrist[1] + 10)], fill=(103, 111, 102), outline=(24, 24, 23))
    for i, label in enumerate(["side profile", "attack tell", "shutdown mass"]):
        x = 100 + i * 215
        shadowed_panel(image, (x, 176, x + 170, 435), fill=(22, 20, 18, 220), outline=(92, 65, 36, 230), radius=8)
        sx, sy = x + 85, 290
        draw.ellipse((sx - 46, sy - 48, sx + 46, sy + 78), fill=COLORS["iron"], outline=COLORS["brass"], width=3)
        draw.rounded_rectangle((sx - 38, sy - 100, sx + 35, sy - 40), radius=18, fill=COLORS["brass"], outline=(45, 28, 15), width=2)
        if i == 1:
            draw.line((sx + 35, sy - 5, sx + 110, sy - 88), fill=COLORS["copper"], width=17)
            draw.polygon([(sx + 98, sy - 105), (sx + 145, sy - 132), (sx + 122, sy - 76)], fill=(170, 178, 166))
        else:
            draw.line((sx + 30, sy + 10, sx + 95, sy + 70), fill=COLORS["copper"], width=15)
        draw.line((sx - 18, sy + 70, sx - 54, sy + 134), fill=COLORS["iron"], width=17)
        draw.line((sx + 20, sy + 70, sx + 55, sy + 132), fill=COLORS["iron"], width=17)
        draw.text((x + 24, 401), label, font=F["small"], fill=COLORS["muted"])
    shadowed_panel(image, (94, 755, 565, 985), fill=(18, 15, 12, 220))
    draw.text((126, 790), "Enemy Read", font=F["h3"], fill=COLORS["ink"])
    for j, note in enumerate(["Hunched boiler torso", "Single furnace eye", "Cutter arms read first", "No organic monster language", "Shutdown pose keeps target mass"]):
        y = 836 + j * 28
        draw.rectangle((128, y + 7, 142, y + 19), fill=COLORS["brass_hi"])
        draw.text((160, y), note, font=F["body"], fill=COLORS["muted"])
    footer(image, "AssetPreviews brass/iron previews; ENEMY-001 Scrapper brief")
    save(image, "RENDER_OBJECT_scrapper_enemy_mockup.jpg")


def room_base(title, subtitle):
    image = gradient((1920, 1080), (9, 9, 8), (42, 31, 20))
    paste_texture_poly(image, "iron", [(0, 116), (420, 270), (520, 790), (0, 1080)], 128, 0.58, 1.35, 235)
    paste_texture_poly(image, "iron", [(1920, 116), (1495, 270), (1390, 790), (1920, 1080)], 128, 0.58, 1.35, 235)
    paste_texture_poly(image, "stone", [(520, 790), (1390, 790), (1920, 1080), (0, 1080)], 120, 0.62, 1.3, 240)
    paste_texture_poly(image, "stone", [(420, 270), (1495, 270), (1390, 790), (520, 790)], 128, 0.46, 1.25, 220)
    paste_texture_poly(image, "iron", [(0, 116), (1920, 116), (1495, 270), (420, 270)], 128, 0.45, 1.25, 210)
    header(image, title, subtitle)
    return image


def draw_corridor_arch(draw, center_x=960, y=270, width=760, height=520):
    x1, x2 = center_x - width // 2, center_x + width // 2
    draw.rounded_rectangle((x1, y, x2, y + height), radius=48, fill=(18, 17, 15, 150), outline=COLORS["brass"], width=8)
    draw.rectangle((x1 + 60, y + 68, x2 - 60, y + height), fill=(23, 22, 20, 190), outline=(86, 61, 39), width=3)
    for x in [x1 + 90, x2 - 90]:
        draw.rounded_rectangle((x - 20, y + 12, x + 20, y + height + 8), radius=8, fill=COLORS["brass"], outline=(47, 29, 15), width=2)
        draw_rivets(draw, [(x, y + 58 + i * 60) for i in range(7)], 5)
    draw.arc((x1 + 70, y + 20, x2 - 70, y + 245), 180, 360, fill=COLORS["brass_hi"], width=8)


def render_intake_corridor():
    image = room_base("Brassworks Intake Corridor", "Level01 mood mockup using material previews and procedural corridor geometry")
    draw = ImageDraw.Draw(image, "RGBA")
    radial_glow(image, (1010, 328), 410, COLORS["amber"], 120)
    draw_corridor_arch(draw, 960, 268, 780, 520)
    for i, y in enumerate([220, 300, 385, 470, 555]):
        draw_pipe(draw, (50, y + 30), (690, y + 92), 22 + i % 2 * 7, COLORS["copper"] if i % 2 else COLORS["iron"], COLORS["brass_hi"] if i % 2 else COLORS["iron_hi"], shadow=False)
        draw_pipe(draw, (1870, y + 20), (1235, y + 88), 24 + i % 2 * 6, COLORS["copper"] if not i % 2 else COLORS["iron"], COLORS["brass_hi"] if not i % 2 else COLORS["iron_hi"], shadow=False)
    for x, y in [(365, 365), (1550, 356), (720, 255), (1230, 255)]:
        radial_glow(image, (x, y), 180, COLORS["amber"], 85)
        draw.rounded_rectangle((x - 24, y - 56, x + 24, y + 56), radius=14, fill=(72, 45, 24, 230), outline=COLORS["brass_hi"], width=2)
        draw.rectangle((x - 13, y - 35, x + 13, y + 35), fill=(255, 169, 55, 180))
    for x in range(610, 1320, 110):
        draw.line((x, 806, x + 110, 1038), fill=(28, 28, 27, 190), width=10)
        draw.line((x + 5, 806, x + 115, 1038), fill=(113, 79, 45, 95), width=2)
    draw.line((520, 830, 1455, 770), fill=COLORS["brass"], width=7)
    draw.line((408, 1015, 1560, 880), fill=(91, 67, 43), width=6)
    haze = Image.new("RGBA", image.size, (0, 0, 0, 0))
    hdraw = ImageDraw.Draw(haze, "RGBA")
    for hx, hy, rx, ry in [(690, 640, 210, 90), (1315, 600, 190, 85), (970, 705, 260, 105), (480, 520, 150, 65)]:
        hdraw.ellipse((hx - rx, hy - ry, hx + rx, hy + ry), fill=(205, 199, 179, 35))
    image.alpha_composite(haze.filter(ImageFilter.GaussianBlur(26)))
    draw_gauge(draw, 610, 392, 47, 0.78)
    draw_valve(draw, 1338, 442, 50, angle=0.5)
    shadowed_panel(image, (95, 830, 582, 990), fill=(18, 15, 12, 220))
    draw.text((126, 862), "Room Intent", font=F["h3"], fill=COLORS["ink"])
    for j, note in enumerate(["Brass arch frames mark route", "Pipes imply pressure flow", "Wet oil-stone floor catches amber light"]):
        y = 906 + j * 30
        draw.ellipse((130, y + 8, 142, y + 20), fill=COLORS["brass_hi"])
        draw.text((160, y), note, font=F["body"], fill=COLORS["muted"])
    footer(image, "AssetPreviews material previews; Level01 Brassworks Intake map; MAT-FOUND and PROP-PIPE briefs")
    save(image, "RENDER_ROOM_brassworks_intake_corridor_mockup.jpg")


def render_steam_baffle():
    image = room_base("Steam-Baffle Approach", "Level03 hazard-language mockup with readable steam puffs and pressure controls")
    draw = ImageDraw.Draw(image, "RGBA")
    radial_glow(image, (960, 250), 360, (255, 120, 48), 90)
    for x in [690, 870, 1050, 1230]:
        draw.rounded_rectangle((x - 34, 305, x + 34, 792), radius=12, fill=COLORS["iron"], outline=COLORS["brass"], width=4)
        for y in range(330, 775, 66):
            draw.rectangle((x - 28, y, x + 28, y + 20), fill=(111, 63, 34, 210), outline=(30, 22, 16))
        draw.line((x - 18, 310, x - 18, 790), fill=COLORS["iron_hi"], width=2)
    for x in [605, 780, 960, 1140, 1310]:
        draw.ellipse((x - 82, 780, x + 82, 860), fill=(17, 17, 16, 230), outline=COLORS["brass"], width=3)
        for off in [-45, -20, 5, 30, 55]:
            draw.line((x + off, 795, x + off - 24, 846), fill=(83, 66, 45), width=5)
    haze = Image.new("RGBA", image.size, (0, 0, 0, 0))
    hdraw = ImageDraw.Draw(haze, "RGBA")
    for x, height, alpha in [(610, 380, 75), (785, 270, 60), (960, 415, 82), (1135, 245, 58), (1310, 350, 72)]:
        for k in range(6):
            y = 775 - k * (height // 6)
            rx = 55 + k * 18
            ry = 32 + k * 14
            hdraw.ellipse((x - rx, y - ry, x + rx, y + ry), fill=(214, 208, 190, max(12, alpha - k * 9)))
    image.alpha_composite(haze.filter(ImageFilter.GaussianBlur(22)))
    draw_pipe(draw, (270, 380), (660, 430), 38, COLORS["green"], (143, 206, 142), shadow=False)
    draw_pipe(draw, (1670, 350), (1260, 420), 38, COLORS["red"], (231, 92, 59), shadow=False)
    draw_pipe(draw, (270, 502), (660, 545), 30, COLORS["copper"], COLORS["brass_hi"], shadow=False)
    draw_pipe(draw, (1660, 504), (1260, 545), 30, COLORS["copper"], COLORS["brass_hi"], shadow=False)
    shadowed_panel(image, (112, 260, 424, 650), fill=(17, 15, 13, 225), outline=(120, 83, 46, 230), radius=8)
    draw.text((144, 292), "Gauge Alcove", font=F["h3"], fill=COLORS["ink"])
    draw_gauge(draw, 250, 405, 72, 0.88)
    draw_valve(draw, 250, 552, 58, angle=0.1)
    draw.rounded_rectangle((1460, 250, 1752, 488), radius=12, fill=(32, 22, 16, 225), outline=COLORS["brass"], width=2)
    draw.text((1495, 285), "Danger State", font=F["h3"], fill=COLORS["ink"])
    draw.rectangle((1496, 335, 1710, 370), fill=COLORS["red"], outline=COLORS["brass_hi"])
    draw.rectangle((1496, 390, 1645, 425), fill=COLORS["green"], outline=COLORS["brass_hi"])
    shadowed_panel(image, (92, 840, 662, 988), fill=(18, 15, 12, 220))
    draw.text((124, 872), "Hazard Read", font=F["h3"], fill=COLORS["ink"])
    for j, note in enumerate(["Steam columns leave visible safe gaps", "Red/green pipes map pressure state", "Control alcove gives objective clue"]):
        y = 914 + j * 27
        draw.rectangle((126, y + 7, 140, y + 19), fill=COLORS["brass_hi"])
        draw.text((158, y), note, font=F["body"], fill=COLORS["muted"])
    footer(image, "AssetPreviews material previews; Steam-Baffle Approach in Level03 map; VFX and PROP-MECH briefs")
    save(image, "RENDER_ROOM_steam_baffle_approach_mockup.jpg")


def contact_sheet_sources():
    image = gradient((1800, 1280), (18, 15, 12), (40, 30, 21))
    draw = ImageDraw.Draw(image, "RGBA")
    header(image, "Preview Source Contact Sheet", "Approved read-only preview inputs available before staged asset delivery", status="MOCKUP SOURCE")
    x0, y0 = 70, 155
    cell_w, cell_h = 520, 300
    for index, filename in enumerate(PREVIEW_FILES):
        row, col = divmod(index, 3)
        x = x0 + col * 570
        y = y0 + row * 355
        shadowed_panel(image, (x, y, x + cell_w, y + cell_h), fill=(23, 20, 17, 230), outline=(112, 78, 43, 230), radius=8)
        path = SRC / filename
        if path.exists():
            src_image = Image.open(path).convert("RGB")
            src_image.thumbnail((cell_w - 44, cell_h - 92), Image.Resampling.LANCZOS)
            tx = x + 22 + ((cell_w - 44) - src_image.width) // 2
            ty = y + 22 + ((cell_h - 92) - src_image.height) // 2
            image.paste(src_image.convert("RGBA"), (tx, ty))
        draw.text((x + 24, y + cell_h - 52), filename, font=F["small"], fill=COLORS["ink"])
        draw.text((x + 24, y + cell_h - 28), "source preview", font=F["tiny"], fill=COLORS["muted"])
    draw.rounded_rectangle((70, 1122, 1730, 1210), radius=8, fill=(16, 14, 12, 210), outline=(105, 75, 45, 190), width=1)
    message = "Baseline preview sources for the procedural mockups. Staged ArtStaging assets are rendered separately when present."
    draw.text((100, 1144), message, font=F["body"], fill=COLORS["muted"])
    footer(image, "Documentation/AssetPreviews/*.jpg and *.png")
    save(image, "CONTACTSHEET_preview_material_sources.jpg")


def contact_sheet_mockups():
    files = [
        "RENDER_OBJECT_pipe_valve_gauge_mockup.jpg",
        "RENDER_OBJECT_pressure_pistol_mockup.jpg",
        "RENDER_OBJECT_steam_scattergun_mockup.jpg",
        "RENDER_OBJECT_scrapper_enemy_mockup.jpg",
        "RENDER_ROOM_brassworks_intake_corridor_mockup.jpg",
        "RENDER_ROOM_steam_baffle_approach_mockup.jpg",
    ]
    image = gradient((1800, 1280), (18, 15, 12), (45, 32, 20))
    draw = ImageDraw.Draw(image, "RGBA")
    header(image, "Concept Render Contact Sheet", "First-pass review JPGs generated outside Unity build assets", status="MOCKUP")
    x0, y0 = 70, 155
    cell_w, cell_h = 520, 340
    for index, filename in enumerate(files):
        row, col = divmod(index, 3)
        x = x0 + col * 570
        y = y0 + row * 410
        shadowed_panel(image, (x, y, x + cell_w, y + cell_h), fill=(23, 20, 17, 230), outline=(112, 78, 43, 230), radius=8)
        path = OUT / filename
        if path.exists():
            src_image = Image.open(path).convert("RGB")
            src_image.thumbnail((cell_w - 44, 250), Image.Resampling.LANCZOS)
            tx = x + 22 + ((cell_w - 44) - src_image.width) // 2
            ty = y + 22 + (250 - src_image.height) // 2
            image.paste(src_image.convert("RGBA"), (tx, ty))
        draw.text((x + 24, y + 286), filename, font=F["tiny"], fill=COLORS["ink"])
        draw.text((x + 24, y + 310), "status: mockup", font=F["small"], fill=COLORS["muted"])
    draw.rounded_rectangle((70, 1035, 1730, 1198), radius=8, fill=(16, 14, 12, 210), outline=(105, 75, 45, 190), width=1)
    text = "Use these for fast art-direction review only. Replace or supplement with staged turntables once MaterialsPBR, ModularKit, WeaponsProps, and Enemies workers publish usable meshes/textures under ArtStaging."
    y = 1062
    for line in textwrap.wrap(text, 130):
        draw.text((100, y), line, font=F["body"], fill=COLORS["muted"])
        y += 30
    footer(image, "Generated mockups in Documentation/ConceptRenders")
    save(image, "CONTACTSHEET_mock_concept_renders.jpg")


def main():
    contact_sheet_sources()
    render_pipe_valve()
    render_pressure_pistol()
    render_steam_scattergun()
    render_scrapper()
    render_intake_corridor()
    render_steam_baffle()
    contact_sheet_mockups()
    for path in sorted(OUT.glob("*.jpg")):
        with Image.open(path) as image:
            print(f"{path.name}\t{image.size[0]}x{image.size[1]}\t{path.stat().st_size}")


if __name__ == "__main__":
    main()
