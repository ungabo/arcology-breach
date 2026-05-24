from pathlib import Path
from PIL import Image, ImageDraw, ImageEnhance, ImageFilter
import math
import sys

SCRIPT_DIR = Path(__file__).resolve().parent
sys.path.insert(0, str(SCRIPT_DIR))

import generate_concept_renders as base

ROOT = Path(r"D:\__MY APPS\Unity Doom")
STAGE = ROOT / "Assets" / "_Project" / "ArtStaging"
OUT = ROOT / "Documentation" / "ConceptRenders"
MAT_DIR = STAGE / "MaterialsPBR"
TEX_DIR = MAT_DIR / "Textures"
PREVIEW_DIR = MAT_DIR / "Previews"
ENEMY_DIR = STAGE / "Enemies"
WEAPON_DIR = STAGE / "WeaponsProps"


def load_texture(name):
    path = TEX_DIR / name
    if path.exists():
        return Image.open(path).convert("RGB")
    return Image.new("RGB", (256, 256), (45, 39, 30))


STAGED_TEX = {
    "brass": load_texture("T_AgedBrass_BaseColor.png"),
    "iron": load_texture("T_RivetedBlackenedIron_BaseColor.png"),
    "brick": load_texture("T_SootStainedBrick_BaseColor.png"),
    "stone": load_texture("T_WetOilDarkStone_BaseColor.png"),
    "copper": load_texture("T_GreenOxidizedCopper_BaseColor.png"),
    "glass": load_texture("T_GrimyAmberGlass_BaseColor.png"),
    "hazard": load_texture("T_HazardEnamel_BaseColor.png"),
}


def tile(image, size, scale=192, brightness=1.0, contrast=1.08):
    source = image.resize((scale, scale), Image.Resampling.BICUBIC)
    source = ImageEnhance.Brightness(source).enhance(brightness)
    source = ImageEnhance.Contrast(source).enhance(contrast)
    tiled = Image.new("RGB", size)
    for y in range(0, size[1], scale):
        for x in range(0, size[0], scale):
            tiled.paste(source, (x, y))
    return tiled.convert("RGBA")


def paste_poly_texture(target, key, poly, scale=192, brightness=1.0, contrast=1.08, alpha=255):
    texture = tile(STAGED_TEX[key], target.size, scale, brightness, contrast)
    if alpha < 255:
        texture.putalpha(alpha)
    mask = Image.new("L", target.size, 0)
    ImageDraw.Draw(mask).polygon(poly, fill=255)
    target.alpha_composite(Image.composite(texture, Image.new("RGBA", target.size, (0, 0, 0, 0)), mask))


def parse_mtl(path):
    materials = {}
    current = None
    if not path.exists():
        return materials
    for raw in path.read_text(encoding="utf-8", errors="ignore").splitlines():
        line = raw.strip()
        if not line or line.startswith("#"):
            continue
        parts = line.split()
        if parts[0] == "newmtl" and len(parts) > 1:
            current = parts[1]
            materials[current] = (0.55, 0.45, 0.32)
        elif parts[0] == "Kd" and current and len(parts) >= 4:
            materials[current] = tuple(max(0.0, min(1.0, float(v))) for v in parts[1:4])
    return materials


def parse_obj(path):
    vertices = []
    faces = []
    material = "default"
    mtllibs = []
    for raw in path.read_text(encoding="utf-8", errors="ignore").splitlines():
        line = raw.strip()
        if not line or line.startswith("#"):
            continue
        parts = line.split()
        if parts[0] == "mtllib" and len(parts) > 1:
            mtllibs.append(path.parent / " ".join(parts[1:]))
        elif parts[0] == "usemtl" and len(parts) > 1:
            material = parts[1]
        elif parts[0] == "v" and len(parts) >= 4:
            vertices.append(tuple(float(v) for v in parts[1:4]))
        elif parts[0] == "f" and len(parts) >= 4:
            idxs = []
            for token in parts[1:]:
                value = token.split("/")[0]
                if not value:
                    continue
                index = int(value)
                idxs.append(index - 1 if index > 0 else len(vertices) + index)
            if len(idxs) >= 3:
                faces.append((idxs, material))
    materials = {}
    for mtl in mtllibs:
        materials.update(parse_mtl(mtl))
    return {"path": path, "vertices": vertices, "faces": faces, "materials": materials}


def rotate_vertex(v, yaw, pitch):
    x, y, z = v
    cy, sy = math.cos(yaw), math.sin(yaw)
    x, z = x * cy + z * sy, -x * sy + z * cy
    cp, sp = math.cos(pitch), math.sin(pitch)
    y, z = y * cp - z * sp, y * sp + z * cp
    return x, y, z


def face_normal(points):
    if len(points) < 3:
        return (0, 0, 1)
    ax, ay, az = points[0]
    bx, by, bz = points[1]
    cx, cy, cz = points[2]
    ux, uy, uz = bx - ax, by - ay, bz - az
    vx, vy, vz = cx - ax, cy - ay, cz - az
    nx = uy * vz - uz * vy
    ny = uz * vx - ux * vz
    nz = ux * vy - uy * vx
    length = math.sqrt(nx * nx + ny * ny + nz * nz) or 1.0
    return nx / length, ny / length, nz / length


def mat_color(mesh, material):
    kd = mesh["materials"].get(material, (0.55, 0.45, 0.32))
    rgb = tuple(int(max(0, min(255, value * 255))) for value in kd)
    if "FurnaceGlow" in material or "Amber" in material or "Glass" in material:
        rgb = tuple(min(255, int(c * 1.18 + 20)) for c in rgb)
    return rgb


def render_mesh(mesh, size=(420, 260), yaw=0.0, pitch=-0.2):
    width, height = size
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    if not mesh["vertices"]:
        return image
    rotated = [rotate_vertex(v, yaw, pitch) for v in mesh["vertices"]]
    xs = [p[0] for p in rotated]
    ys = [p[1] for p in rotated]
    zs = [p[2] for p in rotated]
    span_x = max(xs) - min(xs) or 1
    span_y = max(ys) - min(ys) or 1
    scale = min((width - 42) / span_x, (height - 42) / span_y)
    cx = (max(xs) + min(xs)) / 2
    cy = (max(ys) + min(ys)) / 2
    draw = ImageDraw.Draw(image, "RGBA")
    faces = []
    for idxs, material in mesh["faces"]:
        points = [rotated[i] for i in idxs if 0 <= i < len(rotated)]
        if len(points) < 3:
            continue
        depth = sum(p[2] for p in points) / len(points)
        faces.append((depth, points, material))
    light = (-0.35, 0.72, -0.58)
    faces.sort(key=lambda item: item[0])
    for _, points, material in faces:
        normal = face_normal(points)
        dot = max(0.0, normal[0] * light[0] + normal[1] * light[1] + normal[2] * light[2])
        shade = 0.42 + dot * 0.58
        color = tuple(int(c * shade) for c in mat_color(mesh, material))
        outline = tuple(max(0, int(c * 0.45)) for c in color)
        projected = [((p[0] - cx) * scale + width / 2, height / 2 - (p[1] - cy) * scale + (p[2] - sum(zs) / len(zs)) * scale * 0.04) for p in points]
        draw.polygon(projected, fill=(*color, 245), outline=(*outline, 160))
    glow = Image.new("RGBA", size, (0, 0, 0, 0))
    gd = ImageDraw.Draw(glow, "RGBA")
    for _, points, material in faces:
        if "FurnaceGlow" not in material and "Amber" not in material and "Glass" not in material:
            continue
        projected = [((p[0] - cx) * scale + width / 2, height / 2 - (p[1] - cy) * scale + (p[2] - sum(zs) / len(zs)) * scale * 0.04) for p in points]
        if projected:
            mx = sum(p[0] for p in projected) / len(projected)
            my = sum(p[1] for p in projected) / len(projected)
            gd.ellipse((mx - 42, my - 42, mx + 42, my + 42), fill=(255, 112, 35, 38))
    image.alpha_composite(glow.filter(ImageFilter.GaussianBlur(12)))
    return image


def draw_mesh_card(image, mesh_path, box, label, angles=(-0.65, 0.15, 0.92), status="staged OBJ blockout"):
    draw = ImageDraw.Draw(image, "RGBA")
    base.shadowed_panel(image, box, fill=(23, 20, 17, 226), outline=(118, 82, 46, 230), radius=8)
    mesh = parse_obj(mesh_path)
    x1, y1, x2, y2 = box
    inner_w = x2 - x1 - 48
    view_w = max(120, inner_w // len(angles))
    for i, angle in enumerate(angles):
        thumb = render_mesh(mesh, (view_w, y2 - y1 - 104), angle, -0.28)
        tx = x1 + 24 + i * view_w
        ty = y1 + 26
        shadow = Image.new("RGBA", thumb.size, (0, 0, 0, 0))
        sd = ImageDraw.Draw(shadow, "RGBA")
        sd.ellipse((thumb.width * 0.18, thumb.height * 0.83, thumb.width * 0.82, thumb.height * 0.98), fill=(0, 0, 0, 72))
        image.alpha_composite(shadow.filter(ImageFilter.GaussianBlur(8)), (tx, ty))
        image.alpha_composite(thumb, (tx, ty))
    draw.text((x1 + 24, y2 - 68), label, font=base.F["small"], fill=base.COLORS["ink"])
    draw.text((x1 + 24, y2 - 40), status, font=base.F["tiny"], fill=base.COLORS["muted"])


def render_staged_enemy_turntable():
    image = base.gradient((1920, 1080), (17, 14, 11), (46, 34, 22))
    base.radial_glow(image, (1380, 280), 450, base.COLORS["amber"], 88)
    base.header(image, "Staged Enemy Blockout Turntables", "OBJ/MTL renders from Assets/_Project/ArtStaging/Enemies", status="STAGED")
    enemies = [
        ("ENEMY_ScrapperAutomaton_Blockout_LOD0.obj", "Scrapper Automaton"),
        ("ENEMY_LancerAutomaton_Blockout_LOD0.obj", "Lancer Automaton"),
        ("ENEMY_SentinelTurret_Blockout_LOD0.obj", "Sentinel Turret"),
    ]
    for i, (file_name, label) in enumerate(enemies):
        draw_mesh_card(image, ENEMY_DIR / file_name, (70 + i * 605, 150, 565 + i * 605, 545), label)
    parts = [
        ("PART_BrassMaskVisor_Blockout.obj", "Mask Visor"),
        ("PART_CogShoulderJoint_Blockout.obj", "Cog Joint"),
        ("PART_FurnaceCore_Blockout.obj", "Furnace Core"),
        ("PART_PipeBackpack_Blockout.obj", "Pipe Backpack"),
        ("PART_PistonLeg_Blockout.obj", "Piston Leg"),
    ]
    for i, (file_name, label) in enumerate(parts):
        draw_mesh_card(image, ENEMY_DIR / file_name, (70 + i * 360, 610, 390 + i * 360, 958), label, angles=(0.2,), status="staged part OBJ")
    base.footer(image, "Assets/_Project/ArtStaging/Enemies/*.obj and ENEMY_BlockoutMaterials.mtl")
    base.save(image, "RENDER_OBJECT_staged_enemy_blockouts_turntable.jpg")


def render_staged_weapon_turntable():
    image = base.gradient((1920, 1080), (17, 14, 11), (48, 35, 22))
    base.radial_glow(image, (1320, 310), 460, base.COLORS["amber"], 85)
    base.header(image, "Staged Weapon And Prop Blockouts", "OBJ/MTL renders from Assets/_Project/ArtStaging/WeaponsProps", status="STAGED")
    objects = [
        ("WPN_PressurePistol_Viewmodel_Blockout.obj", "Pressure Pistol Viewmodel"),
        ("WPN_SteamScattergun_Viewmodel_Blockout.obj", "Steam Scattergun Viewmodel"),
        ("PROP_WallPressureStation_Blockout.obj", "Wall Pressure Station"),
        ("PROP_CrankLeverSwitch_Blockout.obj", "Crank Lever Switch"),
        ("PICKUP_PressureCell_Ammo_Blockout.obj", "Pressure Cell Ammo"),
        ("PICKUP_ScattergunSlugCanister_Blockout.obj", "Slug Canister Pickup"),
    ]
    for index, (file_name, label) in enumerate(objects):
        row, col = divmod(index, 3)
        draw_mesh_card(image, WEAPON_DIR / file_name, (70 + col * 605, 150 + row * 420, 565 + col * 605, 520 + row * 420), label)
    base.footer(image, "Assets/_Project/ArtStaging/WeaponsProps/*.obj and WPN_PROP_PICKUP_WeaponsProps_BlockoutMaterials.mtl")
    base.save(image, "RENDER_OBJECT_staged_weapon_props_turntable.jpg")


def render_staged_material_contact_sheet():
    image = base.gradient((1800, 1280), (18, 15, 12), (45, 32, 20))
    draw = ImageDraw.Draw(image, "RGBA")
    base.header(image, "Staged PBR Material Batch 01", "Material preview sheets and base-color tiles from ArtStaging/MaterialsPBR", status="STAGED")
    preview_files = [
        PREVIEW_DIR / "T_MaterialsPBR_Batch01_BaseColorTiling_ContactSheet.png",
        PREVIEW_DIR / "T_MaterialsPBR_Batch01_SwatchSheet.png",
        PREVIEW_DIR / "T_MaterialsPBR_Batch01_MapTriplets_ContactSheet.jpg",
    ]
    boxes = [(70, 150, 850, 520), (910, 150, 1730, 520), (70, 570, 850, 1180)]
    for path, box in zip(preview_files, boxes):
        base.shadowed_panel(image, box, fill=(23, 20, 17, 226), outline=(118, 82, 46, 230), radius=8)
        if path.exists():
            thumb = Image.open(path).convert("RGB")
            thumb.thumbnail((box[2] - box[0] - 44, box[3] - box[1] - 82), Image.Resampling.LANCZOS)
            image.paste(thumb.convert("RGBA"), (box[0] + 22 + (box[2] - box[0] - 44 - thumb.width) // 2, box[1] + 22))
        draw.text((box[0] + 24, box[3] - 48), path.name, font=base.F["tiny"], fill=base.COLORS["ink"])
    texture_names = [
        ("T_AgedBrass_BaseColor.png", "Aged Brass"),
        ("T_RivetedBlackenedIron_BaseColor.png", "Riveted Iron"),
        ("T_SootStainedBrick_BaseColor.png", "Soot Brick"),
        ("T_WetOilDarkStone_BaseColor.png", "Wet Oil Stone"),
        ("T_GreenOxidizedCopper_BaseColor.png", "Oxidized Copper"),
        ("T_GrimyAmberGlass_BaseColor.png", "Amber Glass"),
        ("T_HazardEnamel_BaseColor.png", "Hazard Enamel"),
        ("T_LeatherBellows_BaseColor.png", "Leather Bellows"),
    ]
    x0, y0 = 930, 590
    for index, (file_name, label) in enumerate(texture_names):
        row, col = divmod(index, 4)
        x = x0 + col * 195
        y = y0 + row * 240
        base.shadowed_panel(image, (x, y, x + 165, y + 205), fill=(21, 18, 15, 226), outline=(90, 66, 42, 220), radius=8)
        tex = load_texture(file_name)
        tex.thumbnail((133, 133), Image.Resampling.LANCZOS)
        image.paste(tex.convert("RGBA"), (x + 16, y + 18))
        draw.text((x + 16, y + 163), label, font=base.F["tiny"], fill=base.COLORS["ink"])
        draw.text((x + 16, y + 183), "base color", font=base.F["tiny"], fill=base.COLORS["muted"])
    base.footer(image, "Assets/_Project/ArtStaging/MaterialsPBR/Previews and Textures")
    base.save(image, "CONTACTSHEET_staged_material_pbr_batch01.jpg")


def render_staged_room():
    image = base.gradient((1920, 1080), (8, 8, 7), (37, 27, 18))
    paste_poly_texture(image, "iron", [(0, 116), (450, 245), (565, 790), (0, 1080)], 256, 0.54, 1.25, 245)
    paste_poly_texture(image, "iron", [(1920, 116), (1470, 245), (1355, 790), (1920, 1080)], 256, 0.54, 1.25, 245)
    paste_poly_texture(image, "brick", [(450, 245), (1470, 245), (1355, 790), (565, 790)], 256, 0.58, 1.18, 240)
    paste_poly_texture(image, "stone", [(565, 790), (1355, 790), (1920, 1080), (0, 1080)], 256, 0.62, 1.25, 245)
    paste_poly_texture(image, "iron", [(0, 116), (1920, 116), (1470, 245), (450, 245)], 256, 0.42, 1.22, 225)
    base.header(image, "Staged Material Corridor Mood", "Room mood render using ArtStaging PBR textures plus OBJ blockout silhouettes", status="STAGED")
    draw = ImageDraw.Draw(image, "RGBA")
    base.radial_glow(image, (960, 340), 480, base.COLORS["amber"], 120)
    # Brass arch and trim rails.
    draw.rounded_rectangle((620, 285, 1300, 820), radius=46, fill=(15, 14, 13, 185), outline=(210, 142, 48), width=8)
    for x in [665, 1255]:
        draw.rounded_rectangle((x - 24, 285, x + 24, 830), radius=9, fill=(170, 105, 34), outline=(57, 34, 14), width=2)
        base.draw_rivets(draw, [(x, 340 + i * 70) for i in range(7)], 5)
    for x, y in [(430, 350), (1490, 350), (805, 258), (1115, 258)]:
        base.radial_glow(image, (x, y), 185, base.COLORS["amber"], 82)
        draw.rounded_rectangle((x - 26, y - 60, x + 26, y + 60), radius=14, fill=(92, 62, 26, 230), outline=(235, 178, 86), width=2)
        draw.rectangle((x - 13, y - 36, x + 13, y + 36), fill=(255, 176, 62, 170))
    # Staged-texture pipes.
    for i, y in enumerate([260, 350, 445, 548]):
        color = base.COLORS["copper"] if i % 2 else base.COLORS["iron"]
        hi = base.COLORS["brass_hi"] if i % 2 else base.COLORS["iron_hi"]
        base.draw_pipe(draw, (75, y), (690, y + 92), 28, color, hi, shadow=False)
        base.draw_pipe(draw, (1845, y), (1230, y + 92), 28, color, hi, shadow=False)
    # Object silhouettes from staged OBJs.
    scrapper = parse_obj(ENEMY_DIR / "ENEMY_ScrapperAutomaton_Blockout_LOD0.obj")
    pistol = parse_obj(WEAPON_DIR / "WPN_PressurePistol_Viewmodel_Blockout.obj")
    scrapper_img = render_mesh(scrapper, (210, 245), yaw=0.2, pitch=-0.25)
    pistol_img = render_mesh(pistol, (300, 150), yaw=-0.1, pitch=-0.18)
    image.alpha_composite(scrapper_img, (840, 548))
    image.alpha_composite(pistol_img, (1130, 705))
    draw.polygon([(1068, 840), (1485, 805), (1585, 866), (1170, 925)], fill=(18, 16, 14, 215), outline=(184, 119, 46, 160))
    draw.text((1178, 846), "staged pickup silhouette", font=base.F["tiny"], fill=base.COLORS["muted"])
    haze = Image.new("RGBA", image.size, (0, 0, 0, 0))
    hdraw = ImageDraw.Draw(haze, "RGBA")
    for cx, cy, rx, ry in [(760, 650, 180, 70), (1110, 625, 210, 90), (960, 760, 260, 105)]:
        hdraw.ellipse((cx - rx, cy - ry, cx + rx, cy + ry), fill=(220, 213, 190, 38))
    image.alpha_composite(haze.filter(ImageFilter.GaussianBlur(24)))
    base.footer(image, "MaterialsPBR base-color textures; Enemies/ENEMY_ScrapperAutomaton_Blockout_LOD0.obj; WeaponsProps/WPN_PressurePistol_Viewmodel_Blockout.obj")
    base.save(image, "RENDER_ROOM_staged_material_corridor_mood.jpg")


def render_staged_assets_contact_sheet():
    files = [
        "CONTACTSHEET_staged_material_pbr_batch01.jpg",
        "RENDER_OBJECT_staged_enemy_blockouts_turntable.jpg",
        "RENDER_OBJECT_staged_weapon_props_turntable.jpg",
        "RENDER_ROOM_staged_material_corridor_mood.jpg",
    ]
    image = base.gradient((1800, 1280), (18, 15, 12), (45, 32, 20))
    draw = ImageDraw.Draw(image, "RGBA")
    base.header(image, "Current Staged Asset Render Sheet", "Review-only JPG lane from live ArtStaging assets", status="STAGED")
    boxes = [(70, 155, 850, 560), (930, 155, 1730, 560), (70, 645, 850, 1050), (930, 645, 1730, 1050)]
    for path_name, box in zip(files, boxes):
        base.shadowed_panel(image, box, fill=(23, 20, 17, 226), outline=(118, 82, 46, 230), radius=8)
        path = OUT / path_name
        if path.exists():
            thumb = Image.open(path).convert("RGB")
            thumb.thumbnail((box[2] - box[0] - 44, box[3] - box[1] - 92), Image.Resampling.LANCZOS)
            image.paste(thumb.convert("RGBA"), (box[0] + 22 + (box[2] - box[0] - 44 - thumb.width) // 2, box[1] + 22))
        draw.text((box[0] + 24, box[3] - 58), path_name, font=base.F["tiny"], fill=base.COLORS["ink"])
        draw.text((box[0] + 24, box[3] - 34), "status: staged", font=base.F["small"], fill=base.COLORS["muted"])
    draw.rounded_rectangle((70, 1122, 1730, 1204), radius=8, fill=(16, 14, 12, 210), outline=(105, 75, 45, 190), width=1)
    draw.text((100, 1147), "These JPGs are concept/review outputs only. They read from ArtStaging but remain outside Unity build assets.", font=base.F["body"], fill=base.COLORS["muted"])
    base.footer(image, "Assets/_Project/ArtStaging read-only inputs; generated JPGs in Documentation/ConceptRenders")
    base.save(image, "CONTACTSHEET_staged_assets_current.jpg")


def main():
    render_staged_material_contact_sheet()
    render_staged_enemy_turntable()
    render_staged_weapon_turntable()
    render_staged_room()
    render_staged_assets_contact_sheet()
    for path in sorted(OUT.glob("*staged*.jpg")):
        with Image.open(path) as image:
            print(f"{path.name}\t{image.size[0]}x{image.size[1]}\t{path.stat().st_size}")


if __name__ == "__main__":
    main()
