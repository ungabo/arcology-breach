from __future__ import annotations

import json
import math
from pathlib import Path

import numpy as np
from PIL import Image, ImageDraw, ImageFilter, ImageFont

ROOT = Path(r"D:\__MY APPS\Unity Doom")
ASSET_ROOT = ROOT / "Assets" / "_Project" / "ArtStaging" / "HighFidelityLookdev"
MODEL_DIR = ASSET_ROOT / "Models"
MAT_DIR = ASSET_ROOT / "Materials"
PREVIEW_DIR = ASSET_ROOT / "Previews"
RENDER_DIR = ROOT / "Documentation" / "ConceptRenders"
DOC_DIR = ROOT / "Documentation" / "AssetProduction" / "HighFidelityLookdev"

for folder in (MODEL_DIR, MAT_DIR, PREVIEW_DIR, RENDER_DIR, DOC_DIR):
    folder.mkdir(parents=True, exist_ok=True)


def rel(path: Path) -> str:
    return path.relative_to(ROOT).as_posix()


MATERIALS = {
    "MAT_HFLD_AgedBrassHero": {
        "kd": (0.74, 0.48, 0.18),
        "rough": 0.38,
        "metal": 1.0,
        "note": "Aged brass for hero rings, gauges, rails, pipe collars, and polished wear edges.",
    },
    "MAT_HFLD_BlackenedRivetedIron": {
        "kd": (0.08, 0.075, 0.065),
        "rough": 0.68,
        "metal": 1.0,
        "note": "Blackened iron plates with grime-dark rivets and chipped edges.",
    },
    "MAT_HFLD_OilWetStone": {
        "kd": (0.055, 0.052, 0.045),
        "rough": 0.22,
        "metal": 0.0,
        "note": "Wet oil-dark masonry and floor slabs with reflective puddled low points.",
    },
    "MAT_HFLD_GreenOxidizedCopper": {
        "kd": (0.09, 0.38, 0.32),
        "rough": 0.62,
        "metal": 0.65,
        "note": "Verdigris over copper for older service lines and oxidized machinery.",
    },
    "MAT_HFLD_AmberGlassLit": {
        "kd": (1.0, 0.48, 0.09),
        "rough": 0.18,
        "metal": 0.0,
        "note": "Warm amber glass for lamps, enemy eyes, furnace cores, and sight glows.",
    },
    "MAT_HFLD_LeatherGripDark": {
        "kd": (0.22, 0.095, 0.035),
        "rough": 0.72,
        "metal": 0.0,
        "note": "Dark worn leather for pistol grips, straps, bellows, and wrapped handles.",
    },
    "MAT_HFLD_CreamGaugeFace": {
        "kd": (0.82, 0.76, 0.61),
        "rough": 0.55,
        "metal": 0.0,
        "note": "Aged enamel pressure gauge faces with dark printed ticks and red needles.",
    },
    "MAT_HFLD_CopperCoilHot": {
        "kd": (0.80, 0.31, 0.12),
        "rough": 0.34,
        "metal": 1.0,
        "note": "Warm copper coils and exposed pressure conduits.",
    },
}


class Mesh:
    def __init__(self, name: str):
        self.name = name
        self.verts: list[tuple[float, float, float]] = []
        self.faces: list[tuple[str, str, tuple[int, ...]]] = []
        self.components: list[dict] = []
        self.materials_used: set[str] = set()

    def v(self, point):
        self.verts.append(tuple(float(x) for x in point))
        return len(self.verts)

    def face(self, inds, mat, group):
        self.faces.append((mat, group, tuple(inds)))
        self.materials_used.add(mat)

    def box(self, center, size, mat, group, rot_z=0.0):
        cx, cy, cz = center
        sx, sy, sz = [s / 2.0 for s in size]
        pts = [
            (-sx, -sy, -sz),
            (sx, -sy, -sz),
            (sx, sy, -sz),
            (-sx, sy, -sz),
            (-sx, -sy, sz),
            (sx, -sy, sz),
            (sx, sy, sz),
            (-sx, sy, sz),
        ]
        rz = math.radians(rot_z)
        co, si = math.cos(rz), math.sin(rz)
        ids = []
        for x, y, z in pts:
            rx = x * co - y * si
            ry = x * si + y * co
            ids.append(self.v((cx + rx, cy + ry, cz + z)))
        for f in ((0, 1, 2, 3), (4, 7, 6, 5), (0, 4, 5, 1), (1, 5, 6, 2), (2, 6, 7, 3), (3, 7, 4, 0)):
            self.face([ids[i] for i in f], mat, group)
        self.components.append({"name": group, "shape": "box", "mat": mat, "center": center, "size": size})

    def cyl(self, center, radius, depth, axis, mat, group, seg=24):
        cx, cy, cz = center
        a_ids = []
        b_ids = []
        for i in range(seg):
            a = 2.0 * math.pi * i / seg
            u, v = math.cos(a) * radius, math.sin(a) * radius
            if axis == "x":
                p1, p2 = (cx - depth / 2, cy + u, cz + v), (cx + depth / 2, cy + u, cz + v)
            elif axis == "y":
                p1, p2 = (cx + u, cy - depth / 2, cz + v), (cx + u, cy + depth / 2, cz + v)
            else:
                p1, p2 = (cx + u, cy + v, cz - depth / 2), (cx + u, cy + v, cz + depth / 2)
            a_ids.append(self.v(p1))
            b_ids.append(self.v(p2))
        c1 = self.v((cx - depth / 2, cy, cz) if axis == "x" else (cx, cy - depth / 2, cz) if axis == "y" else (cx, cy, cz - depth / 2))
        c2 = self.v((cx + depth / 2, cy, cz) if axis == "x" else (cx, cy + depth / 2, cz) if axis == "y" else (cx, cy, cz + depth / 2))
        for i in range(seg):
            j = (i + 1) % seg
            self.face([a_ids[i], a_ids[j], b_ids[j], b_ids[i]], mat, group)
            self.face([c1, a_ids[i], a_ids[j]], mat, group)
            self.face([c2, b_ids[j], b_ids[i]], mat, group)
        self.components.append({"name": group, "shape": f"cylinder_{axis}", "mat": mat, "center": center, "radius": radius, "depth": depth})

    def gear(self, center, radius_a, radius_b, depth, axis, teeth, mat, group):
        cx, cy, cz = center
        seg = teeth * 2
        a_ids = []
        b_ids = []
        for i in range(seg):
            ang = 2.0 * math.pi * i / seg
            r = radius_a if i % 2 == 0 else radius_b
            u, v = math.cos(ang) * r, math.sin(ang) * r
            if axis == "x":
                p1, p2 = (cx - depth / 2, cy + u, cz + v), (cx + depth / 2, cy + u, cz + v)
            else:
                p1, p2 = (cx + u, cy + v, cz - depth / 2), (cx + u, cy + v, cz + depth / 2)
            a_ids.append(self.v(p1))
            b_ids.append(self.v(p2))
        c1 = self.v((cx - depth / 2, cy, cz) if axis == "x" else (cx, cy, cz - depth / 2))
        c2 = self.v((cx + depth / 2, cy, cz) if axis == "x" else (cx, cy, cz + depth / 2))
        for i in range(seg):
            j = (i + 1) % seg
            self.face([a_ids[i], a_ids[j], b_ids[j], b_ids[i]], mat, group)
            self.face([c1, a_ids[i], a_ids[j]], mat, group)
            self.face([c2, b_ids[j], b_ids[i]], mat, group)
        self.components.append({"name": group, "shape": "gear_disk", "mat": mat, "center": center, "teeth": teeth})

    def write(self, path: Path, mtl_rel="../Materials/MAT_HFLD_Batch01_LookdevMaterials.mtl"):
        with path.open("w", encoding="utf-8", newline="\n") as f:
            f.write(f"# {self.name} static lookdev blockout, non-shipping\n")
            f.write(f"mtllib {mtl_rel}\n")
            for vert in self.verts:
                f.write(f"v {vert[0]:.5f} {vert[1]:.5f} {vert[2]:.5f}\n")
            last_g = None
            last_m = None
            for mat, group, inds in self.faces:
                if group != last_g:
                    f.write(f"g {group}\n")
                    last_g = group
                if mat != last_m:
                    f.write(f"usemtl {mat}\n")
                    last_m = mat
                f.write("f " + " ".join(str(i) for i in inds) + "\n")
        return {
            "path": rel(path),
            "vertexCount": len(self.verts),
            "faceCount": len(self.faces),
            "componentCount": len(self.components),
            "materials": sorted(self.materials_used),
            "components": self.components,
        }


def write_mtl():
    path = MAT_DIR / "MAT_HFLD_Batch01_LookdevMaterials.mtl"
    with path.open("w", encoding="utf-8", newline="\n") as f:
        f.write("# HighFidelityLookdev Batch01 placeholder MTL\n")
        for name, m in MATERIALS.items():
            kd = m["kd"]
            ks = tuple(min(1.0, c + 0.18) for c in kd)
            ns = max(2.0, (1.0 - m["rough"]) * 180.0)
            f.write(f"newmtl {name}\n")
            f.write(f"Kd {kd[0]:.4f} {kd[1]:.4f} {kd[2]:.4f}\n")
            f.write(f"Ks {ks[0]:.4f} {ks[1]:.4f} {ks[2]:.4f}\n")
            f.write(f"Ns {ns:.2f}\nillum 2\n\n")
    return path


def build_corridor():
    mesh = Mesh("HF_CorridorDoor_LookdevBlockout")
    for ix in range(-3, 3):
        for iz in range(10):
            mesh.box((ix + 0.5, -0.05, iz + 0.5), (0.96, 0.10, 0.96), "MAT_HFLD_OilWetStone", f"floor_oil_slab_{ix+3}_{iz}")
    mesh.box((-3.25, 1.65, 5.0), (0.25, 3.4, 10.0), "MAT_HFLD_OilWetStone", "left_oil_masonry_wall")
    mesh.box((3.25, 1.65, 5.0), (0.25, 3.4, 10.0), "MAT_HFLD_OilWetStone", "right_oil_masonry_wall")
    mesh.box((0, 3.45, 5.0), (6.5, 0.22, 10.0), "MAT_HFLD_BlackenedRivetedIron", "ceiling_blackened_plate")
    for z in (1.3, 3.1, 4.9, 6.7, 8.5):
        mesh.box((-3.05, 1.7, z), (0.12, 2.8, 0.08), "MAT_HFLD_BlackenedRivetedIron", f"left_riveted_panel_strip_{z}")
        mesh.box((3.05, 1.7, z), (0.12, 2.8, 0.08), "MAT_HFLD_BlackenedRivetedIron", f"right_riveted_panel_strip_{z}")
    mesh.box((0, 1.9, 10.10), (5.3, 3.8, 0.28), "MAT_HFLD_BlackenedRivetedIron", "pressure_gate_backplate")
    mesh.cyl((0, 2.0, 9.94), 1.78, 0.24, "z", "MAT_HFLD_BlackenedRivetedIron", "round_vault_door", 36)
    mesh.cyl((0, 2.0, 9.76), 1.32, 0.12, "z", "MAT_HFLD_AgedBrassHero", "brass_vault_ring", 36)
    mesh.cyl((0, 2.0, 9.58), 0.52, 0.18, "z", "MAT_HFLD_AgedBrassHero", "central_gear_hub", 30)
    for angle in range(0, 360, 45):
        mesh.box((0, 2.0, 9.52), (2.7, 0.12, 0.10), "MAT_HFLD_AgedBrassHero", f"vault_spoke_{angle}", rot_z=angle)
    for x in (-2.45, -1.65, 1.65, 2.45):
        mesh.cyl((x, 3.58, 5.0), 0.10, 10.0, "z", "MAT_HFLD_AgedBrassHero", f"overhead_brass_pipe_{x}", 20)
    for x in (-2.72, 2.72):
        for z in (1.1, 3.4, 5.7, 8.0):
            mesh.cyl((x, 0.80, z), 0.06, 1.45, "y", "MAT_HFLD_AgedBrassHero", f"guardrail_post_{x}_{z}", 14)
            mesh.cyl((x, 1.30, z + 0.55), 0.04, 1.9, "z", "MAT_HFLD_AgedBrassHero", f"guardrail_run_{x}_{z}", 14)
    for x in (-1.8, 1.8):
        mesh.cyl((x, 2.35, 9.45), 0.16, 0.18, "z", "MAT_HFLD_AmberGlassLit", f"amber_lamp_glass_{x}", 20)
        mesh.cyl((x, 2.35, 9.57), 0.21, 0.06, "z", "MAT_HFLD_AgedBrassHero", f"lamp_brass_cage_{x}", 20)
    for angle in range(0, 360, 15):
        x = math.cos(math.radians(angle)) * 1.62
        y = 2.0 + math.sin(math.radians(angle)) * 1.62
        mesh.cyl((x, y, 9.45), 0.045, 0.07, "z", "MAT_HFLD_AgedBrassHero", f"vault_rivet_{angle}", 10)
    return mesh


def build_pistol():
    mesh = Mesh("HF_PressurePistol_LookdevBlockout")
    mesh.cyl((0.0, 0.18, 0), 0.18, 2.45, "x", "MAT_HFLD_BlackenedRivetedIron", "blackened_pressure_barrel", 32)
    mesh.cyl((0.48, -0.05, 0), 0.26, 1.25, "x", "MAT_HFLD_BlackenedRivetedIron", "lower_pressure_reservoir", 28)
    mesh.cyl((-1.36, 0.18, 0), 0.25, 0.22, "x", "MAT_HFLD_AgedBrassHero", "muzzle_brass_ring", 28)
    mesh.cyl((1.22, 0.18, 0), 0.24, 0.26, "x", "MAT_HFLD_AgedBrassHero", "rear_brass_cap", 28)
    for i, x in enumerate(np.linspace(-0.45, 0.52, 7)):
        mesh.cyl((float(x), 0.42, 0), 0.23, 0.055, "x", "MAT_HFLD_CopperCoilHot", f"copper_pressure_coil_{i}", 24)
    mesh.box((0.05, 0.42, 0), (1.18, 0.18, 0.38), "MAT_HFLD_BlackenedRivetedIron", "coil_guard_frame")
    mesh.cyl((0.05, 0.78, -0.02), 0.29, 0.075, "y", "MAT_HFLD_CreamGaugeFace", "top_pressure_gauge_face", 32)
    mesh.cyl((0.05, 0.74, -0.02), 0.34, 0.08, "y", "MAT_HFLD_AgedBrassHero", "top_gauge_brass_bezel", 32)
    mesh.box((0.76, -0.62, 0), (0.38, 1.08, 0.32), "MAT_HFLD_LeatherGripDark", "dark_leather_grip", rot_z=-18)
    mesh.box((0.48, -0.10, 0), (0.86, 0.16, 0.22), "MAT_HFLD_AgedBrassHero", "trigger_guard_bridge", rot_z=-8)
    mesh.cyl((0.35, -0.30, 0), 0.055, 0.20, "z", "MAT_HFLD_BlackenedRivetedIron", "iron_trigger", 16)
    for x in (-0.8, -0.15, 0.5, 1.05):
        mesh.cyl((x, 0.18, 0.23), 0.035, 0.05, "z", "MAT_HFLD_AgedBrassHero", f"side_plate_rivet_{x}", 12)
        mesh.cyl((x, 0.18, -0.23), 0.035, 0.05, "z", "MAT_HFLD_AgedBrassHero", f"opposite_plate_rivet_{x}", 12)
    return mesh


def build_monster():
    mesh = Mesh("HF_ScrapperMonster_LookdevBlockout")
    mesh.cyl((0, 1.75, 0), 0.55, 1.15, "y", "MAT_HFLD_BlackenedRivetedIron", "boiler_torso_shell", 32)
    mesh.cyl((0, 2.38, 0), 0.38, 0.45, "y", "MAT_HFLD_AgedBrassHero", "round_brass_head", 28)
    for x in (-0.18, 0.18):
        mesh.cyl((x, 2.42, -0.37), 0.10, 0.08, "z", "MAT_HFLD_AmberGlassLit", f"amber_eye_{x}", 20)
    mesh.box((0, 2.20, -0.42), (0.36, 0.22, 0.08), "MAT_HFLD_BlackenedRivetedIron", "grille_mouth")
    mesh.cyl((0, 1.45, -0.62), 0.23, 0.08, "z", "MAT_HFLD_CreamGaugeFace", "belly_pressure_gauge", 24)
    mesh.gear((0.65, 1.85, 0.42), 0.52, 0.42, 0.08, "z", 16, "MAT_HFLD_AgedBrassHero", "exposed_back_gear")
    mesh.cyl((0, 2.86, 0), 0.10, 0.40, "y", "MAT_HFLD_AgedBrassHero", "steam_stack_left", 20)
    mesh.cyl((0.34, 2.78, 0.08), 0.09, 0.34, "y", "MAT_HFLD_AgedBrassHero", "steam_stack_right", 18)
    mesh.box((-0.88, 1.78, -0.05), (0.92, 0.16, 0.16), "MAT_HFLD_AgedBrassHero", "left_upper_piston_arm", rot_z=-28)
    mesh.box((-1.34, 1.32, -0.08), (0.70, 0.14, 0.14), "MAT_HFLD_BlackenedRivetedIron", "left_lower_piston_arm", rot_z=-42)
    mesh.gear((-1.78, 1.08, -0.10), 0.42, 0.30, 0.08, "x", 18, "MAT_HFLD_BlackenedRivetedIron", "left_saw_blade")
    mesh.box((0.88, 1.78, -0.05), (0.92, 0.16, 0.16), "MAT_HFLD_AgedBrassHero", "right_upper_piston_arm", rot_z=28)
    mesh.box((1.34, 1.32, -0.08), (0.70, 0.14, 0.14), "MAT_HFLD_BlackenedRivetedIron", "right_lower_piston_arm", rot_z=42)
    for i, angle in enumerate((-30, 0, 30)):
        mesh.box((1.62 + i * 0.045, 0.96 + abs(i - 1) * 0.04, -0.12), (0.42, 0.08, 0.07), "MAT_HFLD_AgedBrassHero", f"right_claw_blade_{i}", rot_z=angle)
    for side in (-1, 1):
        mesh.box((side * 0.38, 0.88, 0), (0.18, 0.78, 0.16), "MAT_HFLD_BlackenedRivetedIron", f"upper_leg_{side}", rot_z=side * 14)
        mesh.box((side * 0.52, 0.38, -0.03), (0.15, 0.64, 0.14), "MAT_HFLD_AgedBrassHero", f"lower_leg_{side}", rot_z=side * -10)
        mesh.box((side * 0.65, 0.03, -0.12), (0.48, 0.12, 0.62), "MAT_HFLD_BlackenedRivetedIron", f"heavy_plated_foot_{side}")
    for y in (1.35, 1.76, 2.14):
        mesh.cyl((0, y, 0), 0.57, 0.055, "y", "MAT_HFLD_AgedBrassHero", f"torso_brass_band_{y}", 32)
    for angle in range(0, 360, 30):
        x = math.cos(math.radians(angle)) * 0.57
        z = math.sin(math.radians(angle)) * 0.57
        mesh.cyl((x, 1.76, z), 0.035, 0.055, "y", "MAT_HFLD_AgedBrassHero", f"torso_rivet_{angle}", 10)
    return mesh


def font(size):
    for candidate in (r"C:\Windows\Fonts\segoeui.ttf", r"C:\Windows\Fonts\arial.ttf"):
        if Path(candidate).exists():
            return ImageFont.truetype(candidate, size)
    return ImageFont.load_default()


TITLE = font(30)
LABEL = font(22)
SMALL = font(15)


def noise_image(img, amount=14, blend=0.22):
    arr = np.asarray(img).astype(np.int16)
    noise = np.random.default_rng(2201).normal(0, amount, arr.shape[:2])[:, :, None]
    noisy = Image.fromarray(np.clip(arr + noise, 0, 255).astype(np.uint8), "RGB")
    return Image.blend(img, noisy, blend)


def draw_rivets(draw, pts, radius=5):
    for x, y in pts:
        draw.ellipse((x - radius, y - radius, x + radius, y + radius), fill=(205, 139, 49, 230), outline=(40, 24, 12, 230), width=1)


def render_corridor(path: Path):
    img = Image.new("RGB", (1400, 820), (11, 10, 9))
    draw = ImageDraw.Draw(img, "RGBA")
    draw.polygon([(0, 0), (1400, 0), (1050, 420), (350, 420)], fill=(21, 20, 18))
    draw.polygon([(0, 820), (1400, 820), (1050, 420), (350, 420)], fill=(30, 28, 24))
    draw.polygon([(0, 0), (350, 420), (0, 820)], fill=(18, 18, 16))
    draw.polygon([(1400, 0), (1050, 420), (1400, 820)], fill=(16, 15, 13))
    for i in range(13):
        t = i / 12
        y = int(420 + (820 - 420) * (t**1.7))
        draw.line((0, y, 1400, y), fill=(80, 60, 35, 125), width=2)
    for x in range(-400, 1800, 150):
        draw.line((x, 820, 700 + (x - 700) * 0.2, 420), fill=(65, 51, 34, 115), width=2)
    for i in range(30):
        rng = np.random.default_rng(i + 20)
        x, y = int(rng.integers(150, 1250)), int(rng.integers(500, 790))
        draw.ellipse((x - 80, y - 9, x + 110, y + 15), fill=(179, 116, 42, 30))
    cx, cy = 700, 315
    draw.rectangle((432, 118, 968, 520), fill=(26, 24, 21), outline=(114, 75, 32), width=4)
    draw.ellipse((490, 98, 910, 518), fill=(35, 29, 22), outline=(181, 111, 38), width=8)
    draw.ellipse((560, 168, 840, 448), outline=(150, 97, 38), width=18)
    for a in range(0, 360, 30):
        rad = math.radians(a)
        draw.line((cx, cy, cx + math.cos(rad) * 176, cy + math.sin(rad) * 176), fill=(176, 107, 39, 210), width=10)
    draw.ellipse((644, 252, 756, 364), fill=(82, 53, 24), outline=(220, 151, 55), width=8)
    draw_rivets(draw, [(cx + math.cos(math.radians(a)) * 198, cy + math.sin(math.radians(a)) * 198) for a in range(0, 360, 15)], 5)
    for y, width, color in ((68, 18, (123, 76, 32)), (96, 10, (183, 116, 42)), (146, 14, (88, 58, 32)), (184, 9, (196, 130, 48))):
        draw.line((0, y, 1400, y + 30), fill=color + (220,), width=width)
    for x in (420, 980):
        draw.ellipse((x - 34, 240, x + 34, 346), fill=(255, 146, 34, 120), outline=(228, 158, 68), width=5)
    for x in (185, 1215):
        draw.ellipse((x - 55, 224, x + 55, 334), fill=(28, 25, 22), outline=(175, 118, 48), width=8)
        draw.ellipse((x - 38, 242, x + 38, 318), fill=(197, 180, 134), outline=(40, 28, 18), width=2)
        draw.line((x, 280, x + 21, 255), fill=(35, 19, 10), width=4)
    for i, (x, y) in enumerate(((160, 310), (460, 180), (1050, 170), (1220, 360))):
        cloud = Image.new("RGBA", img.size, (0, 0, 0, 0))
        cd = ImageDraw.Draw(cloud)
        rng = np.random.default_rng(100 + i)
        for _ in range(28):
            ox, oy, r = int(rng.normal(0, 45)), int(rng.normal(0, 42)), int(rng.integers(16, 54))
            cd.ellipse((x + ox - r, y + oy - r, x + ox + r, y + oy + r), fill=(190, 185, 170, 34))
        img = Image.alpha_composite(img.convert("RGBA"), cloud.filter(ImageFilter.GaussianBlur(12))).convert("RGB")
    draw = ImageDraw.Draw(img, "RGBA")
    draw.rectangle((16, 16, 560, 66), fill=(0, 0, 0, 145))
    draw.text((30, 27), "LOOKDEV / NON-SHIPPING - Corridor + pressure door target", fill=(242, 213, 143), font=LABEL)
    noise_image(img, 12, 0.25).save(path, quality=93)


def render_pistol(path: Path):
    img = Image.new("RGB", (1200, 760), (32, 29, 25))
    draw = ImageDraw.Draw(img, "RGBA")
    for i in range(60):
        rng = np.random.default_rng(i + 200)
        x, y, r = int(rng.integers(0, 1200)), int(rng.integers(0, 760)), int(rng.integers(40, 180))
        draw.ellipse((x - r, y - r, x + r, y + r), fill=(85, 78, 65, 18))
    draw.polygon([(640, 500), (930, 615), (1040, 720), (725, 720), (570, 575)], fill=(75, 34, 15), outline=(172, 105, 49))
    draw.rounded_rectangle((210, 285, 880, 405), radius=48, fill=(28, 26, 24), outline=(189, 117, 43), width=8)
    draw.rounded_rectangle((310, 428, 760, 515), radius=35, fill=(21, 20, 18), outline=(145, 91, 37), width=7)
    draw.ellipse((150, 280, 300, 410), fill=(111, 68, 30), outline=(224, 151, 61), width=8)
    draw.ellipse((820, 276, 965, 412), fill=(95, 59, 26), outline=(219, 150, 62), width=8)
    draw.rounded_rectangle((455, 210, 850, 350), radius=24, fill=(38, 29, 21), outline=(182, 111, 42), width=7)
    for x in range(500, 820, 44):
        draw.ellipse((x - 25, 226, x + 25, 336), outline=(230, 105, 39), width=16)
        draw.line((x, 233, x, 329), fill=(255, 171, 72, 100), width=4)
    draw.ellipse((450, 98, 645, 293), fill=(111, 69, 29), outline=(228, 156, 64), width=10)
    draw.ellipse((474, 122, 621, 269), fill=(213, 198, 157), outline=(45, 26, 13), width=4)
    for a in range(220, 500, 28):
        rad = math.radians(a)
        draw.line((548 + math.cos(rad) * 54, 196 + math.sin(rad) * 54, 548 + math.cos(rad) * 66, 196 + math.sin(rad) * 66), fill=(39, 25, 13), width=2)
    draw.line((548, 196, 590, 168), fill=(104, 24, 17), width=5)
    draw.arc((540, 450, 690, 630), 190, 345, fill=(190, 121, 43), width=11)
    draw.arc((575, 478, 650, 603), 200, 330, fill=(25, 20, 14), width=8)
    draw_rivets(draw, [(285 + i * 90, 415) for i in range(7)] + [(340 + i * 110, 275) for i in range(5)], 6)
    cloud = Image.new("RGBA", img.size, (0, 0, 0, 0))
    cd = ImageDraw.Draw(cloud)
    rng = np.random.default_rng(505)
    for _ in range(35):
        x, y, r = int(rng.normal(825, 75)), int(rng.normal(150, 58)), int(rng.integers(18, 64))
        cd.ellipse((x - r, y - r, x + r, y + r), fill=(186, 181, 169, 34))
    img = Image.alpha_composite(img.convert("RGBA"), cloud.filter(ImageFilter.GaussianBlur(10))).convert("RGB")
    draw = ImageDraw.Draw(img, "RGBA")
    draw.rectangle((16, 16, 505, 66), fill=(0, 0, 0, 145))
    draw.text((30, 27), "LOOKDEV / NON-SHIPPING - Pressure pistol target", fill=(242, 213, 143), font=LABEL)
    noise_image(img, 10, 0.22).save(path, quality=93)


def render_monster(path: Path):
    img = Image.new("RGB", (1150, 820), (27, 25, 22))
    draw = ImageDraw.Draw(img, "RGBA")
    draw.rectangle((0, 610, 1150, 820), fill=(36, 34, 30))
    for y in range(630, 820, 42):
        draw.line((0, y, 1150, y), fill=(76, 57, 36, 120), width=2)
    draw.ellipse((410, 230, 710, 530), fill=(42, 34, 26), outline=(178, 113, 42), width=8)
    draw.ellipse((455, 108, 665, 318), fill=(101, 65, 28), outline=(218, 147, 59), width=8)
    draw.rounded_rectangle((475, 310, 645, 610), radius=42, fill=(33, 30, 26), outline=(160, 99, 40), width=7)
    for x in (512, 608):
        draw.ellipse((x - 36, 180, x + 36, 252), fill=(255, 141, 24, 140), outline=(229, 155, 61), width=6)
        draw.ellipse((x - 18, 198, x + 18, 234), fill=(255, 191, 50, 220))
    for x in range(515, 615, 18):
        draw.line((x, 270, x, 320), fill=(16, 14, 12), width=7)
    for x in (500, 602):
        draw.rounded_rectangle((x - 22, 58, x + 22, 170), radius=14, fill=(83, 55, 27), outline=(206, 136, 54), width=5)
    cloud = Image.new("RGBA", img.size, (0, 0, 0, 0))
    cd = ImageDraw.Draw(cloud)
    rng = np.random.default_rng(612)
    for _ in range(45):
        x, y, r = int(rng.normal(560, 105)), int(rng.normal(55, 42)), int(rng.integers(18, 70))
        cd.ellipse((x - r, y - r, x + r, y + r), fill=(184, 179, 166, 34))
    img = Image.alpha_composite(img.convert("RGBA"), cloud.filter(ImageFilter.GaussianBlur(12))).convert("RGB")
    draw = ImageDraw.Draw(img, "RGBA")
    draw.ellipse((660, 290, 875, 505), outline=(198, 132, 50), width=22)
    for a in range(0, 360, 30):
        draw.line((768, 398, 768 + math.cos(math.radians(a)) * 102, 398 + math.sin(math.radians(a)) * 102), fill=(162, 99, 40, 180), width=5)
    draw.line((430, 365, 250, 485), fill=(160, 103, 48), width=22)
    draw.line((250, 485, 160, 575), fill=(51, 46, 39), width=20)
    pts = []
    for i in range(40):
        r = 88 if i % 2 == 0 else 64
        a = 2 * math.pi * i / 40
        pts.append((132 + math.cos(a) * r, 590 + math.sin(a) * r))
    draw.polygon(pts, fill=(43, 39, 34), outline=(205, 134, 50))
    draw.line((692, 360, 850, 490), fill=(164, 102, 41), width=22)
    draw.line((850, 490, 950, 582), fill=(54, 48, 40), width=20)
    for a in (-35, 0, 35):
        draw.polygon([(970, 594), (1075, 570 + a), (1015, 626 + a)], fill=(169, 105, 44), outline=(231, 158, 64))
    for sx in (-1, 1):
        x = 560 + sx * 80
        draw.line((x, 555, x + sx * 70, 675), fill=(58, 51, 43), width=24)
        draw.line((x + sx * 70, 675, x + sx * 110, 760), fill=(145, 91, 38), width=22)
        draw.rounded_rectangle((x + sx * 72 - 70, 742, x + sx * 72 + 86, 795), radius=12, fill=(37, 33, 29), outline=(159, 99, 40), width=5)
    draw_rivets(draw, [(428 + i * 32, 352) for i in range(9)] + [(460 + i * 28, 522) for i in range(8)], 5)
    draw.rectangle((16, 16, 548, 66), fill=(0, 0, 0, 145))
    draw.text((30, 27), "LOOKDEV / NON-SHIPPING - Scrapper monster target", fill=(242, 213, 143), font=LABEL)
    noise_image(img, 11, 0.23).save(path, quality=93)


def make_swatches():
    cols, cell, header = 3, 300, 72
    rows = math.ceil(len(MATERIALS) / cols)
    sheet = Image.new("RGB", (cols * cell, header + rows * cell), (22, 20, 18))
    draw = ImageDraw.Draw(sheet)
    draw.rectangle((0, 0, sheet.width, header), fill=(42, 32, 22))
    draw.text((24, 18), "HFLD Batch01 Material Swatches - Lookdev / Non-Shipping", fill=(238, 210, 146), font=TITLE)
    yy, xx = np.mgrid[0:196, 0:244]
    rng = np.random.default_rng(401)
    for i, (name, m) in enumerate(MATERIALS.items()):
        x, y = (i % cols) * cell, header + (i // cols) * cell
        base = np.array(m["kd"]) * 255
        noise = rng.random((196, 244, 1))
        vignette = 0.72 + 0.28 * (1 - ((xx - 122) ** 2 + (yy - 98) ** 2) / (180**2))[:, :, None]
        arr = np.clip(base * (0.72 + 0.35 * noise) * vignette, 0, 255).astype(np.uint8)
        sw = Image.fromarray(arr, "RGB")
        sd = ImageDraw.Draw(sw, "RGBA")
        if "Brass" in name or "RivetedIron" in name:
            for rx in range(30, 244, 58):
                for ry in range(30, 196, 58):
                    sd.ellipse((rx - 6, ry - 6, rx + 6, ry + 6), fill=(230, 185, 92, 100), outline=(34, 29, 21, 130), width=2)
        if "Stone" in name:
            for rx in range(0, 244, 61):
                sd.line((rx, 0, rx, 196), fill=(5, 5, 4, 130), width=3)
            for ry in range(0, 196, 61):
                sd.line((0, ry, 244, ry), fill=(5, 5, 4, 130), width=3)
        if "Amber" in name:
            for _ in range(14):
                rx, ry = int(rng.integers(0, 244)), int(rng.integers(0, 196))
                sd.line((rx, ry, rx + int(rng.integers(-40, 41)), ry + int(rng.integers(35, 95))), fill=(255, 218, 110, 80), width=2)
        if "Leather" in name:
            for ry in range(20, 196, 28):
                sd.arc((10, ry - 8, 234, ry + 22), 180, 350, fill=(20, 10, 5, 90), width=2)
        sheet.paste(sw.filter(ImageFilter.UnsharpMask(radius=1, percent=80)), (x + 28, y + 26))
        draw.text((x + 28, y + 232), name.replace("MAT_HFLD_", ""), fill=(236, 220, 176), font=LABEL)
        draw.text((x + 28, y + 258), f"Rough {m['rough']:.2f}  Metal {m['metal']:.2f}", fill=(168, 154, 128), font=SMALL)
    out = MAT_DIR / "MAT_HFLD_Batch01_MaterialSwatches.png"
    prev = PREVIEW_DIR / "PREVIEW_HFLD_Batch01_MaterialSwatches.png"
    sheet.save(out)
    sheet.save(prev)
    return out, prev


def make_contact(paths, swatch):
    contact = Image.new("RGB", (1600, 1800), (18, 17, 15))
    draw = ImageDraw.Draw(contact)
    draw.rectangle((0, 0, 1600, 96), fill=(42, 32, 22))
    draw.text((30, 24), "High Fidelity Lookdev Batch 01 - Non-Shipping Review Sheet", fill=(242, 213, 143), font=TITLE)
    thumbs = [Image.open(p).convert("RGB") for p in paths] + [Image.open(swatch).convert("RGB")]
    labels = ["Corridor / pressure door", "Pressure pistol", "Scrapper-like monster", "Material target swatches"]
    positions = [(40, 130), (830, 130), (40, 900), (830, 900)]
    for thumb, label, (x, y) in zip(thumbs, labels, positions):
        thumb.thumbnail((730, 620), Image.Resampling.LANCZOS)
        draw.text((x, y), label, fill=(236, 220, 176), font=LABEL)
        contact.paste(thumb, (x, y + 46))
        draw.rectangle((x, y + 46, x + thumb.width, y + 46 + thumb.height), outline=(116, 86, 46), width=3)
    out = RENDER_DIR / "CONTACTSHEET_LOOKDEV_HFLD_Batch01_nonshipping.jpg"
    prev = PREVIEW_DIR / "PREVIEW_HFLD_Batch01_ContactSheet.jpg"
    contact.save(out, quality=92)
    contact.save(prev, quality=92)
    return out, prev


def main():
    mtl = write_mtl()
    models = [
        build_corridor().write(MODEL_DIR / "HF_CorridorDoor_LookdevBlockout.obj"),
        build_pistol().write(MODEL_DIR / "HF_PressurePistol_LookdevBlockout.obj"),
        build_monster().write(MODEL_DIR / "HF_ScrapperMonster_LookdevBlockout.obj"),
    ]
    swatch, swatch_preview = make_swatches()
    corridor_render = RENDER_DIR / "RENDER_LOOKDEV_HFLD_Batch01_corridor_pressure_door_nonshipping.jpg"
    pistol_render = RENDER_DIR / "RENDER_LOOKDEV_HFLD_Batch01_pressure_pistol_nonshipping.jpg"
    monster_render = RENDER_DIR / "RENDER_LOOKDEV_HFLD_Batch01_scrapper_monster_nonshipping.jpg"
    render_corridor(corridor_render)
    render_pistol(pistol_render)
    render_monster(monster_render)
    contact, contact_preview = make_contact([corridor_render, pistol_render, monster_render], swatch)
    manifest = {
        "batchId": "HFLD_Batch01_StaticLookdev",
        "status": "lookdev-non-shipping",
        "sourceConcept": "Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png",
        "materialLibrary": rel(mtl),
        "materials": MATERIALS,
        "models": models,
        "swatches": [rel(swatch), rel(swatch_preview)],
        "renders": [rel(corridor_render), rel(pistol_render), rel(monster_render), rel(contact), rel(contact_preview)],
    }
    manifest_path = DOC_DIR / "HFLD_Batch01_AssetManifest.json"
    manifest_path.write_text(json.dumps(manifest, indent=2) + "\n", encoding="utf-8")
    print(json.dumps({"manifest": rel(manifest_path), "models": [m["path"] for m in models], "renders": manifest["renders"]}, indent=2))


if __name__ == "__main__":
    main()
