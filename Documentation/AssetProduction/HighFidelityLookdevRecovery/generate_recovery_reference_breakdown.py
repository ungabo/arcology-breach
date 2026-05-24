from pathlib import Path
from textwrap import wrap

from PIL import Image, ImageDraw, ImageFont, ImageOps


ROOT = Path("D:/__MY APPS/Unity Doom")
SOURCE = ROOT / "Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png"
BATCH01 = ROOT / "Documentation/ConceptRenders/CONTACTSHEET_LOOKDEV_HFLD_Batch01_nonshipping.jpg"
OUT = ROOT / "Documentation/ConceptRenders/CONTACTSHEET_HFLD_Recovery01_reference_breakdown_planning.jpg"


def font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont:
    candidates = [
        "C:/Windows/Fonts/segoeuib.ttf" if bold else "C:/Windows/Fonts/segoeui.ttf",
        "C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf",
    ]
    for path in candidates:
        try:
            return ImageFont.truetype(path, size=size)
        except OSError:
            pass
    return ImageFont.load_default()


FONT_TITLE = font(42, True)
FONT_H2 = font(28, True)
FONT_BODY = font(22)
FONT_SMALL = font(18)
FONT_TINY = font(15)


def draw_wrapped(draw: ImageDraw.ImageDraw, xy, text, fnt, fill, width_px, line_gap=5):
    x, y = xy
    approx_chars = max(12, int(width_px / max(8, fnt.size * 0.54)))
    for paragraph in text.split("\n"):
        lines = wrap(paragraph, approx_chars) if paragraph else [""]
        for line in lines:
            draw.text((x, y), line, font=fnt, fill=fill)
            y += fnt.size + line_gap
    return y


def fit(img: Image.Image, size):
    return ImageOps.fit(img, size, method=Image.Resampling.LANCZOS, centering=(0.5, 0.5))


def framed(canvas, img, box, title, subtitle=None):
    draw = ImageDraw.Draw(canvas)
    x, y, w, h = box
    draw.rectangle((x - 2, y - 2, x + w + 2, y + h + 44), outline=(179, 122, 50), width=2)
    canvas.paste(fit(img, (w, h)), (x, y))
    draw.rectangle((x, y + h, x + w, y + h + 44), fill=(20, 17, 14))
    draw.text((x + 12, y + h + 8), title, font=FONT_SMALL, fill=(238, 206, 150))
    if subtitle:
        draw.text((x + w - 12 - draw.textlength(subtitle, font=FONT_TINY), y + h + 12), subtitle, font=FONT_TINY, fill=(185, 174, 154))


def callout(draw, box, label, color=(255, 190, 86)):
    x1, y1, x2, y2 = box
    draw.rectangle(box, outline=color, width=4)
    draw.rectangle((x1, y1 - 30, x1 + min(520, 18 + len(label) * 10), y1), fill=(22, 17, 12))
    draw.text((x1 + 8, y1 - 27), label, font=FONT_TINY, fill=color)


def main():
    src = Image.open(SOURCE).convert("RGB")
    batch = Image.open(BATCH01).convert("RGB")

    corridor = src.crop((0, 0, 1536, 512))
    enemy = src.crop((0, 512, 768, 1024))
    pistol = src.crop((768, 512, 1536, 1024))

    W, H = 2200, 1650
    bg = Image.new("RGB", (W, H), (13, 12, 10))
    draw = ImageDraw.Draw(bg)

    draw.rectangle((0, 0, W, 118), fill=(36, 27, 20))
    draw.text((48, 28), "HFLD Recovery 01 - Reference Breakdown / Planning Only", font=FONT_TITLE, fill=(246, 218, 166))
    draw.text((50, 82), "This is not a success render. It defines measurable targets for the next proof pass.", font=FONT_SMALL, fill=(210, 193, 164))

    framed(bg, corridor, (48, 150, 1320, 440), "North-star corridor: dense, wet, warm practical lighting", "source crop")
    framed(bg, pistol, (1410, 150, 720, 440), "North-star pressure pistol: layered first-person hero prop", "source crop")
    framed(bg, enemy, (48, 680, 620, 420), "North-star Scrapper: boiler mass, tools, readable threat", "source crop")
    framed(bg, batch, (750, 680, 650, 420), "Batch01 rejected: flat schematic, low material/detail proof", "failed attempt")

    # Source crop callouts after paste.
    callout(draw, (180, 178, 385, 262), "boilers + readable gauges")
    callout(draw, (535, 160, 1035, 250), "12+ overhead pipe runs, collars, elbows")
    callout(draw, (1025, 215, 1245, 465), "pressure door: radial hub, braces, rivets")
    callout(draw, (430, 350, 1040, 560), "wet floor reflections and slab breakup")
    callout(draw, (710, 260, 820, 450), "warm amber practical lamps")
    callout(draw, (1535, 176, 1688, 302), "top gauge")
    callout(draw, (1664, 292, 1908, 365), "hot copper coil")
    callout(draw, (1802, 452, 2058, 572), "leather glove + scale")
    callout(draw, (142, 755, 292, 875), "saw silhouette")
    callout(draw, (392, 720, 535, 835), "side gear")
    callout(draw, (245, 730, 390, 835), "amber eyes + grille")
    callout(draw, (820, 760, 1350, 1025), "too few forms, no PBR, no camera depth")

    panel_x, panel_y = 1455, 675
    draw.rectangle((panel_x, panel_y, 2130, 1405), fill=(25, 22, 18), outline=(118, 82, 43), width=2)
    draw.text((panel_x + 24, panel_y + 22), "Next Proof Must Hit These Gates", font=FONT_H2, fill=(245, 207, 145))
    y = panel_y + 72
    gates = [
        "1920x1080 minimum for a single hero render; keep planning sheets at 1536x1024 or larger.",
        "Corridor: 12+ pipe runs, 4+ lamps, 4+ gauges, 4+ valves, 5+ steam plumes, 150+ door/wall rivets.",
        "Materials: aged brass, blackened iron, pipe metal, wet stone, amber glass, copper coil, cream gauge face, leather.",
        "Lighting: amber practicals near 1800K-2600K, deep shadows, controlled bloom, wet reflection streaks.",
        "Pistol: 3/4 first-person view, visible gauge, coil with 6+ turns, glove/grip, 60+ fasteners.",
        "Enemy: boiler torso, amber eyes, grille, stacks, side gear, saw, claw, piston legs, 100+ fasteners.",
        "Human review: image must read as the same world as the source with labels removed.",
    ]
    for item in gates:
        draw.ellipse((panel_x + 25, y + 8, panel_x + 35, y + 18), fill=(224, 151, 54))
        y = draw_wrapped(draw, (panel_x + 48, y), item, FONT_BODY, (226, 216, 196), 590, 6) + 8

    draw.rectangle((48, 1185, 1400, 1520), fill=(25, 22, 18), outline=(118, 82, 43), width=2)
    draw.text((72, 1210), "Candid Batch01 Diagnosis", font=FONT_H2, fill=(245, 207, 145))
    diagnosis = (
        "Batch01 captured a few nouns from the concept, but it did not capture the look. "
        "It lacks dimensional geometry, practical lighting, wet reflections, material roughness separation, dense pipe/rivet dressing, "
        "first-person weapon scale, and atmospheric depth. The next pass starts with counted density and material/lighting validation before any 'high fidelity' label is allowed."
    )
    draw_wrapped(draw, (72, 1262), diagnosis, FONT_BODY, (226, 216, 196), 1270, 8)

    draw.rectangle((0, H - 58, W, H), fill=(36, 27, 20))
    draw.text((50, H - 40), "Output: Documentation/ConceptRenders/CONTACTSHEET_HFLD_Recovery01_reference_breakdown_planning.jpg", font=FONT_SMALL, fill=(210, 193, 164))
    draw.text((1710, H - 40), "Label: planning/reference, not success", font=FONT_SMALL, fill=(246, 189, 97))

    OUT.parent.mkdir(parents=True, exist_ok=True)
    bg.save(OUT, quality=92, subsampling=1)
    print(OUT)


if __name__ == "__main__":
    main()
