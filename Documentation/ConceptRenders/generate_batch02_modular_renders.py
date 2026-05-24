from pathlib import Path
from PIL import Image, ImageDraw, ImageEnhance, ImageFilter
import sys
import textwrap

SCRIPT_DIR = Path(__file__).resolve().parent
sys.path.insert(0, str(SCRIPT_DIR))

import generate_concept_renders as base
import generate_staged_asset_renders as staged

ROOT = Path(r"D:\__MY APPS\Unity Doom")
OUT = ROOT / "Documentation" / "ConceptRenders"
STAGE = ROOT / "Assets" / "_Project" / "ArtStaging"
KIT = STAGE / "ModularKit"
ENEMIES = STAGE / "Enemies"
WEAPONS = STAGE / "WeaponsProps"


def load_rgba(path, fallback=(40, 34, 25)):
    if path.exists():
        return Image.open(path).convert("RGBA")
    return Image.new("RGBA", (256, 256), (*fallback, 255))


KIT_TEX = {
    "brass": load_rgba(KIT / "KIT_MAT_AgedBrass_BaseColor.png", (150, 98, 38)),
    "iron": load_rgba(KIT / "KIT_MAT_RivetedIron_BaseColor.png", (40, 42, 40)),
    "masonry": load_rgba(KIT / "KIT_MAT_OilDarkMasonry_BaseColor.png", (38, 34, 30)),
    "grate": load_rgba(KIT / "KIT_MAT_DarkGrate_BaseColor.png", (16, 17, 17)),
    "pipe": load_rgba(KIT / "KIT_MAT_OxidizedPipe_BaseColor.png", (58, 78, 64)),
    "glow": load_rgba(KIT / "KIT_MAT_WarmLampGlow_BaseColor.png", (230, 138, 47)),
}


def save(image, name):
    image = base.add_noise(image, 4)
    image = base.add_vignette(image, 82)
    image.convert("RGB").save(OUT / name, quality=93, optimize=True, progressive=True)


def tile_texture(key, size, scale=256, brightness=1.0, contrast=1.1):
    source = KIT_TEX[key].resize((scale, scale), Image.Resampling.BICUBIC)
    source = ImageEnhance.Brightness(source).enhance(brightness)
    source = ImageEnhance.Contrast(source).enhance(contrast)
    tiled = Image.new("RGBA", size, (0, 0, 0, 0))
    for y in range(0, size[1], scale):
        for x in range(0, size[0], scale):
            tiled.alpha_composite(source, (x, y))
    return tiled


def paste_poly_texture(target, key, poly, scale=256, brightness=1.0, contrast=1.1, alpha=255):
    texture = tile_texture(key, target.size, scale, brightness, contrast)
    if alpha < 255:
        texture.putalpha(alpha)
    mask = Image.new("L", target.size, 0)
    ImageDraw.Draw(mask).polygon(poly, fill=255)
    target.alpha_composite(Image.composite(texture, Image.new("RGBA", target.size, (0, 0, 0, 0)), mask))


def brighten_rgba(image, brightness=1.18, contrast=1.08):
    alpha = image.getchannel("A")
    rgb = image.convert("RGB")
    rgb = ImageEnhance.Brightness(rgb).enhance(brightness)
    rgb = ImageEnhance.Contrast(rgb).enhance(contrast)
    result = rgb.convert("RGBA")
    result.putalpha(alpha)
    return result


def mesh_image(path, size, yaw=0.0, pitch=-0.22, brightness=1.2):
    mesh = staged.parse_obj(path)
    image = staged.render_mesh(mesh, size, yaw=yaw, pitch=pitch)
    return brighten_rgba(image, brightness=brightness)


def place_mesh(target, path, size, xy, yaw=0.0, pitch=-0.22, brightness=1.2, glow=None):
    if glow:
        base.radial_glow(target, glow[0], glow[1], glow[2], glow[3])
    image = mesh_image(path, size, yaw, pitch, brightness)
    target.alpha_composite(image, xy)


def draw_nonshipping_tag(draw, x, y):
    draw.rounded_rectangle((x, y, x + 330, y + 34), radius=7, fill=(18, 13, 9, 215), outline=(185, 122, 55, 210), width=1)
    draw.text((x + 14, y + 8), "NON-SHIPPING CONCEPT / REFERENCE", font=base.F["tiny"], fill=(235, 190, 120))


def draw_floor_grid(draw):
    for x in range(-260, 2220, 145):
        draw.line((x, 1015, 960 + (x - 960) * 0.18, 575), fill=(103, 75, 43, 64), width=2)
    for y in range(615, 1060, 50):
        t = (y - 570) / 500
        draw.line((210 + 330 * t, y, 1710 - 330 * t, y), fill=(95, 67, 42, 58), width=2)


def draw_room_shell(image, title, subtitle):
    paste_poly_texture(image, "masonry", [(0, 116), (455, 255), (560, 812), (0, 1080)], 224, 0.55, 1.2, 238)
    paste_poly_texture(image, "masonry", [(1920, 116), (1465, 255), (1360, 812), (1920, 1080)], 224, 0.55, 1.2, 238)
    paste_poly_texture(image, "iron", [(0, 116), (1920, 116), (1465, 255), (455, 255)], 224, 0.48, 1.25, 222)
    paste_poly_texture(image, "grate", [(560, 812), (1360, 812), (1920, 1080), (0, 1080)], 224, 0.68, 1.22, 242)
    paste_poly_texture(image, "masonry", [(455, 255), (1465, 255), (1360, 812), (560, 812)], 224, 0.56, 1.16, 220)
    base.header(image, title, subtitle, status="STAGED")
    draw = ImageDraw.Draw(image, "RGBA")
    draw_floor_grid(draw)
    draw_nonshipping_tag(draw, 1510, 82)
    return draw


def draw_room_notes(image, draw, notes, box=(92, 820, 675, 998), heading="Reference Notes"):
    base.shadowed_panel(image, box, fill=(17, 14, 11, 220), outline=(142, 96, 47, 220), radius=8)
    draw.text((box[0] + 32, box[1] + 30), heading, font=base.F["h3"], fill=base.COLORS["ink"])
    y = box[1] + 76
    for note in notes:
        draw.ellipse((box[0] + 36, y + 7, box[0] + 48, y + 19), fill=base.COLORS["brass_hi"])
        draw.text((box[0] + 68, y), note, font=base.F["body"], fill=base.COLORS["muted"])
        y += 31


def render_modular_corridor_room():
    image = base.gradient((1920, 1080), (10, 9, 8), (48, 34, 22))
    draw = draw_room_shell(
        image,
        "Batch 02 Brassworks Modular Corridor",
        "Staged ModularKit direction, composited from OBJ pieces and kit base-color materials",
    )
    base.radial_glow(image, (960, 300), 530, base.COLORS["amber"], 112)
    # Hero corridor module and supporting kit parts.
    place_mesh(image, KIT / "KIT_ModularKit_ReferenceAssembly.obj", (760, 520), (580, 250), yaw=0.1, pitch=-0.30, brightness=1.36)
    place_mesh(image, KIT / "SM_CorridorShell_Straight_4m.obj", (430, 310), (148, 300), yaw=-0.55, pitch=-0.25, brightness=1.22)
    place_mesh(image, KIT / "SM_WallPanel_RivetedTrim_4m.obj", (430, 310), (1342, 305), yaw=0.55, pitch=-0.25, brightness=1.22)
    place_mesh(image, KIT / "SM_PipeRun_Straight_4m.obj", (500, 165), (210, 238), yaw=-0.08, pitch=-0.18, brightness=1.35)
    place_mesh(image, KIT / "SM_PipeRun_Straight_4m.obj", (500, 165), (1215, 238), yaw=0.08, pitch=-0.18, brightness=1.35)
    for x in [745, 1138]:
        place_mesh(image, KIT / "SM_LampHousing_Prop.obj", (120, 180), (x, 245), yaw=0.0, pitch=-0.15, brightness=1.45, glow=((x + 60, 340), 160, base.COLORS["amber"], 78))
    place_mesh(image, KIT / "SM_PressureGauge_Prop.obj", (150, 190), (615, 494), yaw=-0.05, pitch=-0.18, brightness=1.34)
    place_mesh(image, KIT / "SM_ValveWheel_Prop.obj", (140, 140), (1234, 540), yaw=0.2, pitch=-0.08, brightness=1.35)
    # Painted-over pipes emphasize the direction while real pipe meshes sit in the scene.
    for i, y in enumerate([390, 475, 565]):
        color = base.COLORS["copper"] if i % 2 else base.COLORS["iron"]
        hi = base.COLORS["brass_hi"] if i % 2 else base.COLORS["iron_hi"]
        base.draw_pipe(draw, (100, y), (675, y + 80), 24, color, hi, shadow=False)
        base.draw_pipe(draw, (1820, y), (1245, y + 80), 24, color, hi, shadow=False)
    haze = Image.new("RGBA", image.size, (0, 0, 0, 0))
    haze_draw = ImageDraw.Draw(haze, "RGBA")
    for cx, cy, rx, ry in [(780, 690, 210, 80), (1060, 675, 245, 92), (960, 805, 320, 115)]:
        haze_draw.ellipse((cx - rx, cy - ry, cx + rx, cy + ry), fill=(218, 210, 186, 34))
    image.alpha_composite(haze.filter(ImageFilter.GaussianBlur(24)))
    draw_room_notes(image, draw, ["4m corridor bay direction", "Masonry, grate, brass, and pipe kit materials", "OBJ reference assembly used as center read"], heading="Modular Read")
    base.footer(image, "ArtStaging/ModularKit reference assembly, corridor shell, wall panel, pipe run, lamp, gauge, valve, and KIT_MAT_* base-color textures")
    save(image, "RENDER_ROOM_modularkit_brassworks_corridor_batch02.jpg")


def render_pressure_gate_control_alcove():
    image = base.gradient((1920, 1080), (9, 8, 7), (48, 32, 21))
    draw = draw_room_shell(
        image,
        "Batch 02 Pressure Gate Control Alcove",
        "Staged doorway, valve, gauge, lamp, and weapon-prop control meshes composited for review",
    )
    base.radial_glow(image, (1000, 330), 470, base.COLORS["amber"], 110)
    place_mesh(image, KIT / "SM_DoorwayThreshold_Frame_4m.obj", (860, 610), (532, 220), yaw=0.02, pitch=-0.27, brightness=1.38)
    place_mesh(image, KIT / "SM_WallPanel_RivetedTrim_4m.obj", (380, 320), (240, 340), yaw=-0.55, pitch=-0.22, brightness=1.16)
    place_mesh(image, KIT / "SM_WallPanel_RivetedTrim_4m.obj", (380, 320), (1315, 340), yaw=0.55, pitch=-0.22, brightness=1.16)
    # Gate bars and pressure-state pipes.
    for x in range(760, 1170, 78):
        draw.rounded_rectangle((x, 345, x + 26, 790), radius=10, fill=(35, 36, 34, 225), outline=(176, 112, 45, 190), width=2)
        draw.line((x + 7, 355, x + 7, 780), fill=(105, 105, 96, 105), width=2)
    for y, color, hi in [(370, base.COLORS["green"], (146, 210, 144)), (462, base.COLORS["red"], (232, 98, 68)), (555, base.COLORS["copper"], base.COLORS["brass_hi"])]:
        base.draw_pipe(draw, (112, y), (614, y + 70), 30, color, hi, shadow=False)
        base.draw_pipe(draw, (1810, y), (1306, y + 70), 30, color, hi, shadow=False)
    # Controls use staged ModularKit + WeaponsProps meshes.
    place_mesh(image, KIT / "SM_PressureGauge_Prop.obj", (190, 240), (245, 405), yaw=-0.05, pitch=-0.15, brightness=1.45, glow=((340, 510), 135, base.COLORS["amber"], 50))
    place_mesh(image, KIT / "SM_ValveWheel_Prop.obj", (170, 170), (262, 630), yaw=0.16, pitch=-0.08, brightness=1.44)
    place_mesh(image, KIT / "SM_LampHousing_Prop.obj", (120, 180), (1440, 285), yaw=0.1, pitch=-0.15, brightness=1.45, glow=((1500, 378), 175, base.COLORS["amber"], 82))
    place_mesh(image, WEAPONS / "PROP_WallPressureStation_Blockout.obj", (360, 360), (1390, 488), yaw=0.25, pitch=-0.21, brightness=1.32)
    place_mesh(image, WEAPONS / "PROP_CrankLeverSwitch_Blockout.obj", (300, 300), (136, 595), yaw=-0.2, pitch=-0.17, brightness=1.24)
    draw.rounded_rectangle((1324, 760, 1745, 850), radius=8, fill=(22, 14, 10, 218), outline=(186, 118, 50, 190), width=1)
    draw.text((1352, 786), "pressure gate reference: locked, venting, restored", font=base.F["small"], fill=base.COLORS["muted"])
    draw_room_notes(image, draw, ["Doorway OBJ used as gate frame", "Valve/gauge/pressure-station are staged assets", "Red/green pipe states are concept direction"], box=(92, 815, 725, 998), heading="Alcove Intent")
    base.footer(image, "ArtStaging/ModularKit doorway, wall, gauge, valve, lamp; WeaponsProps wall pressure station and crank lever switch")
    save(image, "RENDER_ROOM_pressure_gate_control_alcove_batch02.jpg")


def draw_lineup_card(image, box, title, mesh_paths, status, notes=None):
    draw = ImageDraw.Draw(image, "RGBA")
    base.shadowed_panel(image, box, fill=(22, 19, 16, 226), outline=(120, 82, 45, 224), radius=8)
    x1, y1, x2, y2 = box
    draw.text((x1 + 26, y1 + 24), title, font=base.F["h3"], fill=base.COLORS["ink"])
    count = len(mesh_paths)
    view_w = (x2 - x1 - 70) // max(1, count)
    for i, (path, yaw, label, brightness) in enumerate(mesh_paths):
        render = mesh_image(path, (view_w, y2 - y1 - 140), yaw=yaw, pitch=-0.24, brightness=brightness)
        tx = x1 + 35 + i * view_w
        ty = y1 + 76
        shadow = Image.new("RGBA", render.size, (0, 0, 0, 0))
        sd = ImageDraw.Draw(shadow, "RGBA")
        sd.ellipse((render.width * 0.15, render.height * 0.82, render.width * 0.85, render.height * 0.98), fill=(0, 0, 0, 78))
        image.alpha_composite(shadow.filter(ImageFilter.GaussianBlur(8)), (tx, ty))
        image.alpha_composite(render, (tx, ty))
        draw.text((tx + 10, y2 - 66), label, font=base.F["tiny"], fill=base.COLORS["muted"])
    draw.text((x1 + 26, y2 - 38), status, font=base.F["small"], fill=base.COLORS["muted"])
    if notes:
        draw.text((x2 - 310, y2 - 38), notes, font=base.F["small"], fill=(210, 160, 88))


def render_enemy_lineup():
    image = base.gradient((1920, 1080), (17, 14, 11), (45, 32, 21))
    base.header(image, "Batch 02 Scrapper / Sentinel Enemy Lineup", "Staged enemy OBJ blockouts rendered as non-shipping reference art", status="STAGED")
    draw = ImageDraw.Draw(image, "RGBA")
    draw_nonshipping_tag(draw, 1510, 82)
    base.radial_glow(image, (1000, 300), 520, base.COLORS["amber"], 88)
    draw_lineup_card(
        image,
        (70, 155, 900, 790),
        "Scrapper Automaton",
        [
            (ENEMIES / "ENEMY_ScrapperAutomaton_Blockout_LOD0.obj", -0.7, "left three-quarter", 1.35),
            (ENEMIES / "ENEMY_ScrapperAutomaton_Blockout_LOD0.obj", 0.08, "front read", 1.42),
            (ENEMIES / "ENEMY_ScrapperAutomaton_Blockout_LOD0.obj", 0.78, "right three-quarter", 1.35),
        ],
        "staged OBJ blockout - melee silhouette check",
        "cutter arms / furnace eye",
    )
    draw_lineup_card(
        image,
        (1020, 155, 1850, 790),
        "Sentinel Turret",
        [
            (ENEMIES / "ENEMY_SentinelTurret_Blockout_LOD0.obj", -0.8, "left three-quarter", 1.35),
            (ENEMIES / "ENEMY_SentinelTurret_Blockout_LOD0.obj", 0.0, "front read", 1.42),
            (ENEMIES / "ENEMY_SentinelTurret_Blockout_LOD0.obj", 0.82, "right three-quarter", 1.35),
        ],
        "staged OBJ blockout - stationary threat read",
        "barrel / chassis mass",
    )
    # Shared part vocabulary strip.
    parts = [
        ("PART_BrassMaskVisor_Blockout.obj", "visor"),
        ("PART_FurnaceCore_Blockout.obj", "furnace core"),
        ("PART_CogShoulderJoint_Blockout.obj", "cog joint"),
        ("PART_PipeBackpack_Blockout.obj", "pipe pack"),
        ("PART_PistonLeg_Blockout.obj", "piston leg"),
    ]
    base.shadowed_panel(image, (170, 840, 1750, 1000), fill=(18, 15, 12, 215), outline=(112, 78, 45, 210), radius=8)
    draw.text((205, 870), "Shared staged enemy part language", font=base.F["h3"], fill=base.COLORS["ink"])
    for i, (file_name, label) in enumerate(parts):
        x = 570 + i * 220
        place_mesh(image, ENEMIES / file_name, (115, 100), (x, 870), yaw=0.15, pitch=-0.18, brightness=1.28)
        draw.text((x - 4, 960), label, font=base.F["tiny"], fill=base.COLORS["muted"])
    base.footer(image, "ArtStaging/Enemies Scrapper and Sentinel OBJ blockouts plus shared enemy part OBJs and ENEMY_BlockoutMaterials.mtl")
    save(image, "RENDER_OBJECT_scrapper_sentinel_lineup_batch02.jpg")


def render_weapon_prop_lineup():
    image = base.gradient((1920, 1080), (17, 14, 11), (48, 34, 22))
    base.header(image, "Batch 02 Weapon / Prop Lineup", "Staged weapon and gameplay prop OBJ blockouts rendered for user review", status="STAGED")
    draw = ImageDraw.Draw(image, "RGBA")
    draw_nonshipping_tag(draw, 1510, 82)
    base.radial_glow(image, (1010, 325), 530, base.COLORS["amber"], 86)
    cards = [
        ((70, 155, 670, 515), "Pressure Pistol", WEAPONS / "WPN_PressurePistol_Viewmodel_Blockout.obj", [-0.75, 0.1, 0.82], "starter weapon viewmodel"),
        ((710, 155, 1310, 515), "Steam Scattergun", WEAPONS / "WPN_SteamScattergun_Viewmodel_Blockout.obj", [-0.75, 0.1, 0.82], "close-range weapon viewmodel"),
        ((1350, 155, 1850, 515), "Control Props", WEAPONS / "PROP_WallPressureStation_Blockout.obj", [-0.55, 0.15, 0.7], "wall pressure station"),
        ((70, 585, 570, 945), "Crank Lever", WEAPONS / "PROP_CrankLeverSwitch_Blockout.obj", [-0.55, 0.15, 0.7], "switch silhouette"),
        ((610, 585, 1190, 945), "Pressure Cell Ammo", WEAPONS / "PICKUP_PressureCell_Ammo_Blockout.obj", [-0.65, 0.05, 0.75], "pickup scale/read"),
        ((1230, 585, 1850, 945), "Slug Canister Pickup", WEAPONS / "PICKUP_ScattergunSlugCanister_Blockout.obj", [-0.65, 0.05, 0.75], "scattergun ammo read"),
    ]
    for box, title, path, yaws, note in cards:
        draw_lineup_card(
            image,
            box,
            title,
            [(path, yaw, label, 1.34) for yaw, label in zip(yaws, ["left", "front", "right"])],
            f"staged OBJ blockout - {note}",
        )
    base.footer(image, "ArtStaging/WeaponsProps pressure pistol, steam scattergun, wall pressure station, crank lever, pressure cell, and slug canister OBJs")
    save(image, "RENDER_OBJECT_weapon_prop_lineup_batch02.jpg")


def render_contact_sheet():
    files = [
        "RENDER_ROOM_modularkit_brassworks_corridor_batch02.jpg",
        "RENDER_ROOM_pressure_gate_control_alcove_batch02.jpg",
        "RENDER_OBJECT_scrapper_sentinel_lineup_batch02.jpg",
        "RENDER_OBJECT_weapon_prop_lineup_batch02.jpg",
    ]
    image = base.gradient((1800, 1280), (18, 15, 12), (45, 32, 20))
    draw = ImageDraw.Draw(image, "RGBA")
    base.header(image, "Batch 02 Concept Render Contact Sheet", "Current staged ModularKit, enemy, weapon, and prop review outputs", status="STAGED")
    draw_nonshipping_tag(draw, 1390, 82)
    boxes = [(70, 155, 850, 555), (930, 155, 1730, 555), (70, 645, 850, 1045), (930, 645, 1730, 1045)]
    for path_name, box in zip(files, boxes):
        base.shadowed_panel(image, box, fill=(23, 20, 17, 226), outline=(118, 82, 46, 230), radius=8)
        path = OUT / path_name
        if path.exists():
            thumb = Image.open(path).convert("RGB")
            thumb.thumbnail((box[2] - box[0] - 44, box[3] - box[1] - 92), Image.Resampling.LANCZOS)
            image.paste(thumb.convert("RGBA"), (box[0] + 22 + (box[2] - box[0] - 44 - thumb.width) // 2, box[1] + 22))
        draw.text((box[0] + 24, box[3] - 58), path_name, font=base.F["tiny"], fill=base.COLORS["ink"])
        draw.text((box[0] + 24, box[3] - 34), "status: staged composite/reference", font=base.F["small"], fill=base.COLORS["muted"])
    note = "Batch 02 is view-only concept/reference art. It reads staged meshes and materials but writes only JPGs in Documentation/ConceptRenders."
    base.shadowed_panel(image, (70, 1122, 1730, 1208), fill=(16, 14, 12, 210), outline=(105, 75, 45, 190), radius=8)
    y = 1144
    for line in textwrap.wrap(note, 132):
        draw.text((100, y), line, font=base.F["body"], fill=base.COLORS["muted"])
        y += 30
    base.footer(image, "Batch 02 JPGs generated in Documentation/ConceptRenders from ArtStaging read-only inputs")
    save(image, "CONTACTSHEET_batch02_modular_room_object_renders.jpg")


def main():
    render_modular_corridor_room()
    render_pressure_gate_control_alcove()
    render_enemy_lineup()
    render_weapon_prop_lineup()
    render_contact_sheet()
    for name in [
        "RENDER_ROOM_modularkit_brassworks_corridor_batch02.jpg",
        "RENDER_ROOM_pressure_gate_control_alcove_batch02.jpg",
        "RENDER_OBJECT_scrapper_sentinel_lineup_batch02.jpg",
        "RENDER_OBJECT_weapon_prop_lineup_batch02.jpg",
        "CONTACTSHEET_batch02_modular_room_object_renders.jpg",
    ]:
        path = OUT / name
        with Image.open(path) as image:
            print(f"{path.name}\t{image.size[0]}x{image.size[1]}\t{path.stat().st_size}")


if __name__ == "__main__":
    main()
