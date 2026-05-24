from pathlib import Path
from textwrap import wrap

from PIL import Image, ImageDraw, ImageFont, ImageOps


ROOT = Path("D:/__MY APPS/Unity Doom")
SOURCE = ROOT / "Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png"
BATCH01 = ROOT / "Documentation/ConceptRenders/RENDER_LOOKDEV_HFLD_Batch01_pressure_pistol_nonshipping.jpg"
OUT = ROOT / "Documentation/ConceptRenders/CONTACTSHEET_HFLD_Recovery02_pressure_pistol_target_breakdown_planning.jpg"


def font(size: int, bold: bool = False):
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


TITLE = font(40, True)
H2 = font(26, True)
BODY = font(21)
SMALL = font(17)
TINY = font(14)


def fit(img, size, center=(0.5, 0.5)):
    return ImageOps.fit(img, size, method=Image.Resampling.LANCZOS, centering=center)


def wrap_text(draw, xy, text, fnt, fill, width_px, gap=5):
    x, y = xy
    approx = max(12, int(width_px / max(8, fnt.size * 0.54)))
    for line in wrap(text, approx):
        draw.text((x, y), line, font=fnt, fill=fill)
        y += fnt.size + gap
    return y


def callout(draw, box, label, color=(255, 187, 78)):
    x1, y1, x2, y2 = box
    draw.rectangle(box, outline=color, width=4)
    label_w = int(draw.textlength(label, font=TINY)) + 18
    draw.rectangle((x1, y1 - 30, x1 + label_w, y1), fill=(20, 16, 11))
    draw.text((x1 + 8, y1 - 27), label, font=TINY, fill=color)


def panel(draw, xywh, title):
    x, y, w, h = xywh
    draw.rectangle((x, y, x + w, y + h), fill=(25, 22, 18), outline=(118, 82, 43), width=2)
    draw.text((x + 22, y + 18), title, font=H2, fill=(246, 211, 151))


def main():
    source = Image.open(SOURCE).convert("RGB")
    failed = Image.open(BATCH01).convert("RGB")

    # Lower-right source panel, adjusted slightly to preserve smoke and hand framing.
    pistol = source.crop((768, 512, 1536, 1024))

    W, H = 2000, 1420
    canvas = Image.new("RGB", (W, H), (13, 12, 10))
    draw = ImageDraw.Draw(canvas)

    draw.rectangle((0, 0, W, 112), fill=(36, 27, 20))
    draw.text((44, 26), "HFLD Recovery 02 - Pressure Pistol Target Breakdown", font=TITLE, fill=(246, 218, 166))
    draw.text((46, 78), "Planning/reference only. This is not a successful render or asset proof.", font=SMALL, fill=(216, 197, 164))

    # Main source crop.
    main_x, main_y, main_w, main_h = 44, 145, 1120, 745
    draw.rectangle((main_x - 2, main_y - 2, main_x + main_w + 2, main_y + main_h + 42), outline=(179, 122, 50), width=2)
    canvas.paste(fit(pistol, (main_w, main_h)), (main_x, main_y))
    draw.rectangle((main_x, main_y + main_h, main_x + main_w, main_y + main_h + 42), fill=(20, 17, 14))
    draw.text((main_x + 12, main_y + main_h + 9), "Source concept gun: 3/4 first-person, layered metal, gauge, coil, leather hand", font=SMALL, fill=(238, 206, 150))

    # Callout boxes are positioned in the fitted main crop.
    callout(draw, (145, 182, 435, 300), "blackened barrel + brass caps")
    callout(draw, (360, 170, 600, 340), "top gauge: cream face, red needle")
    callout(draw, (595, 285, 920, 420), "hot copper coil window")
    callout(draw, (760, 360, 1085, 480), "nested muzzle / nozzle stack")
    callout(draw, (190, 460, 565, 620), "lower pressure tank")
    callout(draw, (575, 420, 765, 595), "side plates, brackets, fasteners")
    callout(draw, (770, 560, 1095, 790), "leather glove/grip scale")
    callout(draw, (655, 182, 900, 285), "top valves / steam ports")
    callout(draw, (485, 610, 730, 735), "trigger guard area")

    # Failed comparison.
    comp_x, comp_y, comp_w, comp_h = 1220, 145, 700, 445
    draw.rectangle((comp_x - 2, comp_y - 2, comp_x + comp_w + 2, comp_y + comp_h + 42), outline=(135, 75, 45), width=2)
    canvas.paste(fit(failed, (comp_w, comp_h)), (comp_x, comp_y))
    draw.rectangle((comp_x, comp_y + comp_h, comp_x + comp_w, comp_y + comp_h + 42), fill=(20, 17, 14))
    draw.text((comp_x + 12, comp_y + comp_h + 9), "Batch01 failed: side-view graphic, not enough depth/material/detail", font=SMALL, fill=(238, 206, 150))
    callout(draw, (1265, 235, 1775, 455), "flat schematic: lacks 3/4 camera, bevels, PBR")
    callout(draw, (1465, 420, 1750, 515), "coil/gauge nouns present, material proof absent")

    # Fastest path panel.
    panel(draw, (1220, 660, 700, 610), "Fastest Credible Next Step")
    y = 720
    path_items = [
        "Use Unity editor proof first: bevel-like cylinders, brass plates, coil turns, rivets, leather/glove mass, steam ports, smoky lighting.",
        "Do not use the current Batch01 pistol as proof; it is useful only as a component reminder.",
        "Only move to Unity after the offline proof passes silhouette, component count, material, lighting, and human review gates.",
        "Unity validation later uses only an isolated HFLD_Recovery_PressurePistol scene under HighFidelityLookdevRecovery; never gameplay scenes.",
        "A 2D paintover/composite is acceptable for planning, but not as proof of PBR or geometry quality.",
    ]
    for item in path_items:
        draw.ellipse((1244, y + 8, 1254, y + 18), fill=(224, 151, 54))
        y = wrap_text(draw, (1270, y), item, BODY, (226, 216, 196), 610, 6) + 10

    # Acceptance gates panel.
    panel(draw, (44, 980, 1120, 300), "Gun-Only Acceptance Gates")
    gate_text = (
        "Proof render: 1920x1080 minimum, 3/4 first-person camera, gun occupies 55-80% width. "
        "Required components: main blackened barrel, lower tank, top gauge, coil window with 6+ turns, nested muzzle, trigger/guard, leather glove/grip, 2+ ports, 2+ top caps, 8+ brackets/plates, 60+ fasteners. "
        "Materials: no more than 6 production material slots, but 8 visible roles via masks/regions: blackened iron, aged brass, dark pipe metal, hot copper, cream gauge face, glass highlight, leather, soot/grime. "
        "Lighting: warm amber key, low fill, readable gauge/coil, rim highlights, smoky dark background."
    )
    wrap_text(draw, (70, 1035), gate_text, BODY, (226, 216, 196), 1065, 7)

    draw.rectangle((0, H - 56, W, H), fill=(36, 27, 20))
    draw.text((44, H - 38), "Output: Documentation/ConceptRenders/CONTACTSHEET_HFLD_Recovery02_pressure_pistol_target_breakdown_planning.jpg", font=SMALL, fill=(210, 193, 164))
    draw.text((1558, H - 38), "Label: planning/reference, not success", font=SMALL, fill=(246, 189, 97))

    OUT.parent.mkdir(parents=True, exist_ok=True)
    canvas.save(OUT, quality=92, subsampling=1)
    print(OUT)


if __name__ == "__main__":
    main()
