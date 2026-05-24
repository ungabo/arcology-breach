from __future__ import annotations

import datetime as dt
import json
import math
import random
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter, ImageFont


PROJECT_ROOT = Path(__file__).resolve().parents[3]
OUT_DIR = PROJECT_ROOT / "Assets" / "_Project" / "ArtStaging" / "ModularKit"
OUT_DIR.mkdir(parents=True, exist_ok=True)
random.seed(260523)

MATERIALS = {
    "MAT_AgedBrass": {"kd": (0.66, 0.47, 0.22), "color": (166, 119, 55), "map": "KIT_MAT_AgedBrass_BaseColor.png", "metallic": 1.0, "roughness": 0.72},
    "MAT_RivetedIron": {"kd": (0.20, 0.21, 0.20), "color": (52, 54, 52), "map": "KIT_MAT_RivetedIron_BaseColor.png", "metallic": 1.0, "roughness": 0.82},
    "MAT_OilDarkMasonry": {"kd": (0.18, 0.16, 0.13), "color": (46, 40, 34), "map": "KIT_MAT_OilDarkMasonry_BaseColor.png", "metallic": 0.0, "roughness": 0.96},
    "MAT_DarkGrate": {"kd": (0.07, 0.08, 0.08), "color": (18, 21, 21), "map": "KIT_MAT_DarkGrate_BaseColor.png", "metallic": 1.0, "roughness": 0.88},
    "MAT_OxidizedPipe": {"kd": (0.26, 0.29, 0.26), "color": (66, 74, 66), "map": "KIT_MAT_OxidizedPipe_BaseColor.png", "metallic": 1.0, "roughness": 0.78},
    "MAT_LampGlass": {"kd": (0.40, 0.70, 0.66), "color": (102, 178, 168), "map": "KIT_MAT_LampGlass_BaseColor.png", "alpha": 0.45, "metallic": 0.0, "roughness": 0.12},
    "MAT_WarmLampGlow": {"kd": (1.00, 0.63, 0.25), "color": (255, 160, 64), "map": "KIT_MAT_WarmLampGlow_BaseColor.png", "metallic": 0.0, "roughness": 0.35},
    "MAT_GaugeFace": {"kd": (0.78, 0.70, 0.54), "color": (198, 178, 137), "map": "KIT_MAT_GaugeFace_BaseColor.png", "metallic": 0.0, "roughness": 0.74},
    "MAT_GaugeNeedle": {"kd": (0.68, 0.08, 0.04), "color": (174, 20, 10), "map": None, "metallic": 0.0, "roughness": 0.55},
}


def clamp(value):
    return max(0, min(255, int(value)))


def make_texture(file_name, base, style):
    image = Image.new("RGB", (512, 512), base)
    pixels = image.load()
    for y in range(512):
        for x in range(512):
            grain = random.randint(-16, 16)
            wave = int(10 * math.sin(x * 0.07 + y * 0.021))
            pixels[x, y] = tuple(clamp(base[i] + grain + wave // 2) for i in range(3))

    draw = ImageDraw.Draw(image, "RGBA")
    if style == "brass":
        for _ in range(180):
            x, y = random.randrange(512), random.randrange(512)
            draw.line((x, y, x + random.randint(-44, 44), y + random.randint(-5, 5)), fill=(224, 168, 70, 42), width=random.choice([1, 1, 2]))
        for _ in range(28):
            x, y, r = random.randrange(512), random.randrange(512), random.randint(9, 30)
            draw.ellipse((x - r, y - r, x + r, y + r), fill=(28, 118, 92, random.randint(24, 58)))
    elif style == "iron":
        for _ in range(220):
            x, y = random.randrange(512), random.randrange(512)
            draw.line((x, y, x + random.randint(-72, 72), y + random.randint(-4, 4)), fill=(145, 146, 132, 38), width=1)
        for _ in range(38):
            x, y, r = random.randrange(512), random.randrange(512), random.randint(5, 16)
            draw.ellipse((x - r, y - r, x + r, y + r), fill=(9, 12, 13, 68))
    elif style == "masonry":
        for row_y in range(48, 512, 86):
            draw.line((0, row_y, 512, row_y + random.randint(-3, 3)), fill=(10, 9, 8, 120), width=5)
        for row, row_y in enumerate(range(0, 512, 86)):
            offset = 0 if row % 2 == 0 else 90
            for x in range(offset, 512, 180):
                draw.line((x, row_y, x + random.randint(-4, 4), row_y + 84), fill=(10, 9, 8, 105), width=4)
        for _ in range(34):
            x, y, r = random.randrange(512), random.randrange(512), random.randint(14, 55)
            draw.ellipse((x - r, y - r, x + r, y + r), fill=(5, 7, 6, random.randint(28, 82)))
    elif style == "grate":
        for x in range(0, 512, 48):
            draw.rectangle((x, 0, x + 14, 512), fill=(8, 10, 10, 130))
            draw.line((x + 15, 0, x + 15, 512), fill=(120, 125, 112, 45), width=2)
        for y in range(0, 512, 70):
            draw.rectangle((0, y, 512, y + 10), fill=(4, 6, 6, 120))
    elif style == "pipe":
        for _ in range(92):
            x, y = random.randrange(512), random.randrange(512)
            draw.line((x, y, x + random.randint(-20, 20), y + random.randint(20, 90)), fill=(60, 130, 95, 56), width=random.choice([1, 2, 3]))
    elif style == "glass":
        for _ in range(70):
            x, y = random.randrange(512), random.randrange(512)
            draw.line((x, y, x + random.randint(-24, 24), y + random.randint(-100, 100)), fill=(210, 250, 230, 60), width=random.choice([1, 2]))
        draw.rectangle((0, 0, 511, 511), outline=(210, 250, 230, 90), width=16)
    elif style == "glow":
        for radius in range(250, 8, -8):
            alpha = max(8, int(120 * (1 - radius / 260)))
            draw.ellipse((256 - radius, 256 - radius, 256 + radius, 256 + radius), fill=(255, 180, 65, alpha))
    elif style == "gauge":
        image = Image.new("RGB", (512, 512), (194, 177, 138))
        draw = ImageDraw.Draw(image, "RGBA")
        draw.ellipse((38, 38, 474, 474), fill=(205, 190, 154, 255), outline=(42, 33, 24, 255), width=8)
        for i in range(41):
            angle = math.radians(220 - i * (260 / 40))
            r1 = 175 if i % 5 else 158
            r2 = 202
            draw.line((256 + math.cos(angle) * r1, 256 - math.sin(angle) * r1, 256 + math.cos(angle) * r2, 256 - math.sin(angle) * r2), fill=(32, 28, 22, 220), width=4 if i % 5 == 0 else 2)
        font = ImageFont.load_default()
        for label, degree in [("0", 220), ("50", 155), ("100", 90), ("150", 25), ("200", -40)]:
            angle = math.radians(degree)
            draw.text((256 + math.cos(angle) * 122 - 12, 256 - math.sin(angle) * 122 - 7), label, fill=(34, 28, 22), font=font)
        draw.text((214, 332), "STEAM", fill=(58, 46, 35), font=font)
        draw.line((256, 256, 353, 166), fill=(166, 26, 12, 255), width=8)
        draw.ellipse((238, 238, 274, 274), fill=(58, 46, 35, 255))

    image.filter(ImageFilter.UnsharpMask(radius=1.0, percent=85, threshold=4)).save(OUT_DIR / file_name)


def write_materials():
    texture_styles = {
        "KIT_MAT_AgedBrass_BaseColor.png": ((151, 109, 51), "brass"),
        "KIT_MAT_RivetedIron_BaseColor.png": ((50, 52, 50), "iron"),
        "KIT_MAT_OilDarkMasonry_BaseColor.png": ((42, 36, 31), "masonry"),
        "KIT_MAT_DarkGrate_BaseColor.png": ((20, 23, 23), "grate"),
        "KIT_MAT_OxidizedPipe_BaseColor.png": ((62, 70, 62), "pipe"),
        "KIT_MAT_LampGlass_BaseColor.png": ((92, 155, 150), "glass"),
        "KIT_MAT_WarmLampGlow_BaseColor.png": ((241, 139, 44), "glow"),
        "KIT_MAT_GaugeFace_BaseColor.png": ((195, 178, 140), "gauge"),
    }
    for file_name, (base, style) in texture_styles.items():
        make_texture(file_name, base, style)

    lines = ["# Brassworks Breach modular kit material library", "# Units/materials intended for Unity OBJ import", ""]
    for name, material in MATERIALS.items():
        kd = material["kd"]
        lines.extend([f"newmtl {name}", "Ka 0.020 0.020 0.020", f"Kd {kd[0]:.3f} {kd[1]:.3f} {kd[2]:.3f}", "Ks 0.180 0.160 0.120", "Ns 48.000", "illum 2"])
        if "alpha" in material:
            lines.append(f"d {material['alpha']:.3f}")
        if material["map"]:
            lines.append(f"map_Kd {material['map']}")
        lines.append("")
    (OUT_DIR / "KIT_ModularKit_Materials.mtl").write_text("\n".join(lines), encoding="utf-8")


def sub(a, b):
    return (a[0] - b[0], a[1] - b[1], a[2] - b[2])


def cross(a, b):
    return (a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0])


def dot(a, b):
    return a[0] * b[0] + a[1] * b[1] + a[2] * b[2]


def normalized(value):
    value_length = math.sqrt(dot(value, value))
    if value_length <= 1e-8:
        return (0.0, 1.0, 0.0)
    return (value[0] / value_length, value[1] / value_length, value[2] / value_length)


def average(points):
    return tuple(sum(point[i] for point in points) / len(points) for i in range(3))


class Mesh:
    def __init__(self, name):
        self.name = name
        self.vertices = []
        self.uvs = []
        self.normals = []
        self.faces = []
        self.preview_faces = []

    def face(self, points, material, group, uvs=None, orient=None):
        points = [tuple(point) for point in points]
        if orient is not None:
            normal = normalized(cross(sub(points[1], points[0]), sub(points[2], points[0])))
            if dot(normal, orient) < 0:
                points.reverse()
                if uvs:
                    uvs = list(reversed(uvs))
        normal = normalized(cross(sub(points[1], points[0]), sub(points[2], points[0])))
        self.normals.append(normal)
        normal_index = len(self.normals)
        if uvs is None:
            uvs = [(0, 0), (1, 0), (1, 1), (0, 1)] if len(points) == 4 else [(0, 0), (1, 0), (0.5, 1)]
        refs = []
        for point, uv in zip(points, uvs):
            self.vertices.append(point)
            self.uvs.append(uv)
            refs.append((len(self.vertices), len(self.uvs), normal_index))
        self.faces.append((group, material, refs))
        self.preview_faces.append((points, material))

    def write(self):
        lines = [
            f"# {self.name}",
            "# Units: meters. 1 OBJ unit = 1 Unity meter.",
            "# Coordinate system: +Y up, +Z forward.",
            "mtllib KIT_ModularKit_Materials.mtl",
            f"o {self.name}",
        ]
        lines.extend(f"v {x:.5f} {y:.5f} {z:.5f}" for x, y, z in self.vertices)
        lines.extend(f"vt {u:.5f} {v:.5f}" for u, v in self.uvs)
        lines.extend(f"vn {x:.5f} {y:.5f} {z:.5f}" for x, y, z in self.normals)
        lines.append("s off")
        last_group = None
        last_material = None
        for group, material, refs in self.faces:
            if group != last_group:
                lines.append(f"g {group}")
                last_group = group
            if material != last_material:
                lines.append(f"usemtl {material}")
                last_material = material
            lines.append("f " + " ".join(f"{v}/{uv}/{normal}" for v, uv, normal in refs))
        (OUT_DIR / f"{self.name}.obj").write_text("\n".join(lines) + "\n", encoding="utf-8")

    def bbox(self):
        xs = [vertex[0] for vertex in self.vertices]
        ys = [vertex[1] for vertex in self.vertices]
        zs = [vertex[2] for vertex in self.vertices]
        return (min(xs), min(ys), min(zs)), (max(xs), max(ys), max(zs))

    def dimensions(self):
        b0, b1 = self.bbox()
        return [round(b1[i] - b0[i], 3) for i in range(3)]


def box(mesh, x0, x1, y0, y1, z0, z1, material, group):
    mesh.face([(x0, y0, z1), (x1, y0, z1), (x1, y1, z1), (x0, y1, z1)], material, group, orient=(0, 0, 1))
    mesh.face([(x1, y0, z0), (x0, y0, z0), (x0, y1, z0), (x1, y1, z0)], material, group, orient=(0, 0, -1))
    mesh.face([(x1, y0, z1), (x1, y0, z0), (x1, y1, z0), (x1, y1, z1)], material, group, orient=(1, 0, 0))
    mesh.face([(x0, y0, z0), (x0, y0, z1), (x0, y1, z1), (x0, y1, z0)], material, group, orient=(-1, 0, 0))
    mesh.face([(x0, y1, z1), (x1, y1, z1), (x1, y1, z0), (x0, y1, z0)], material, group, orient=(0, 1, 0))
    mesh.face([(x0, y0, z0), (x1, y0, z0), (x1, y0, z1), (x0, y0, z1)], material, group, orient=(0, -1, 0))


def cylinder(mesh, axis, center, length_value, radius, sides, material, group, caps=True):
    cx, cy, cz = center
    if axis == "X":
        axis0, axis1 = cx - length_value / 2, cx + length_value / 2
        def point(axis_value, angle):
            return (axis_value, cy + radius * math.sin(angle), cz + radius * math.cos(angle))
        cap_axes = [(axis0, (-1, 0, 0)), (axis1, (1, 0, 0))]
    elif axis == "Y":
        axis0, axis1 = cy - length_value / 2, cy + length_value / 2
        def point(axis_value, angle):
            return (cx + radius * math.cos(angle), axis_value, cz + radius * math.sin(angle))
        cap_axes = [(axis0, (0, -1, 0)), (axis1, (0, 1, 0))]
    else:
        axis0, axis1 = cz - length_value / 2, cz + length_value / 2
        def point(axis_value, angle):
            return (cx + radius * math.cos(angle), cy + radius * math.sin(angle), axis_value)
        cap_axes = [(axis0, (0, 0, -1)), (axis1, (0, 0, 1))]

    for i in range(sides):
        theta0 = 2 * math.pi * i / sides
        theta1 = 2 * math.pi * (i + 1) / sides
        p0, p1, p2, p3 = point(axis0, theta0), point(axis0, theta1), point(axis1, theta1), point(axis1, theta0)
        mid = average([p0, p1, p2, p3])
        orient = (0 if axis == "X" else mid[0] - cx, 0 if axis == "Y" else mid[1] - cy, 0 if axis == "Z" else mid[2] - cz)
        mesh.face([p0, p1, p2, p3], material, group, uvs=[(i / sides, 0), ((i + 1) / sides, 0), ((i + 1) / sides, 1), (i / sides, 1)], orient=orient)
    if caps:
        for axis_value, orient in cap_axes:
            center_point = {"X": (axis_value, cy, cz), "Y": (cx, axis_value, cz), "Z": (cx, cy, axis_value)}[axis]
            for i in range(sides):
                theta0 = 2 * math.pi * i / sides
                theta1 = 2 * math.pi * (i + 1) / sides
                mesh.face([center_point, point(axis_value, theta0), point(axis_value, theta1)], material, group, orient=orient)


def torus_xy(mesh, center, major, minor, seg_major, seg_minor, material, group):
    cx, cy, cz = center
    for i in range(seg_major):
        theta0 = 2 * math.pi * i / seg_major
        theta1 = 2 * math.pi * (i + 1) / seg_major
        for j in range(seg_minor):
            phi0 = 2 * math.pi * j / seg_minor
            phi1 = 2 * math.pi * (j + 1) / seg_minor
            points = []
            for theta, phi in [(theta0, phi0), (theta1, phi0), (theta1, phi1), (theta0, phi1)]:
                radial = (math.cos(theta), math.sin(theta), 0)
                points.append((cx + (major + minor * math.cos(phi)) * radial[0], cy + (major + minor * math.cos(phi)) * radial[1], cz + minor * math.sin(phi)))
            mid_theta = (theta0 + theta1) / 2
            centerline = (cx + major * math.cos(mid_theta), cy + major * math.sin(mid_theta), cz)
            mid = average(points)
            mesh.face(points, material, group, orient=(mid[0] - centerline[0], mid[1] - centerline[1], mid[2] - centerline[2]))


def elbow_torus(mesh, center, major, minor, seg_major, seg_minor, material, group):
    cx, cy, cz = center
    for i in range(seg_major):
        theta0 = (math.pi / 2) * i / seg_major
        theta1 = (math.pi / 2) * (i + 1) / seg_major
        for j in range(seg_minor):
            phi0 = 2 * math.pi * j / seg_minor
            phi1 = 2 * math.pi * (j + 1) / seg_minor
            points = []
            for theta, phi in [(theta0, phi0), (theta1, phi0), (theta1, phi1), (theta0, phi1)]:
                normal = (math.cos(theta), 0, math.sin(theta))
                centerline = (cx + major * math.cos(theta), cy, cz + major * math.sin(theta))
                points.append((centerline[0] + minor * math.cos(phi) * normal[0], centerline[1] + minor * math.sin(phi), centerline[2] + minor * math.cos(phi) * normal[2]))
            mid_theta = (theta0 + theta1) / 2
            centerline = (cx + major * math.cos(mid_theta), cy, cz + major * math.sin(mid_theta))
            mid = average(points)
            mesh.face(points, material, group, orient=(mid[0] - centerline[0], mid[1] - centerline[1], mid[2] - centerline[2]))


def wall_panel():
    mesh = Mesh("SM_WallPanel_RivetedTrim_4m")
    box(mesh, -2, 2, 0, 3, -0.16, 0, "MAT_OilDarkMasonry", "Masonry_Backplate")
    for x0, x1 in [(-2, -1.84), (1.84, 2), (-0.035, 0.035)]:
        box(mesh, x0, x1, 0.16, 2.84, 0, 0.08, "MAT_AgedBrass", "Brass_VerticalTrim")
    for y0, y1 in [(0, 0.18), (1.47, 1.53), (2.82, 3)]:
        box(mesh, -2, 2, y0, y1, 0, 0.08, "MAT_AgedBrass", "Brass_HorizontalTrim")
    for x in [-1.92, 1.92]:
        for y in [0.35, 0.72, 1.09, 1.46, 1.83, 2.20, 2.57]:
            cylinder(mesh, "Z", (x, y, 0.105), 0.05, 0.05, 12, "MAT_RivetedIron", "Iron_Rivets")
    for x in [-1.52, -0.76, 0, 0.76, 1.52]:
        for y in [0.09, 1.50, 2.91]:
            cylinder(mesh, "Z", (x, y, 0.105), 0.05, 0.044, 12, "MAT_RivetedIron", "Iron_Rivets")
    return mesh


def floor_tile():
    mesh = Mesh("SM_FloorTile_GrateSeams_4m")
    box(mesh, -2, 2, -0.14, 0, -2, 2, "MAT_RivetedIron", "Iron_BaseSlab")
    box(mesh, -0.74, 0.74, -0.135, -0.018, -1.72, 1.72, "MAT_DarkGrate", "Recessed_GrateVoid")
    for x0, x1 in [(-1.88, -0.84), (0.84, 1.88)]:
        for z0, z1 in [(-1.88, -0.10), (0.10, 1.88)]:
            box(mesh, x0, x1, 0, 0.035, z0, z1, "MAT_RivetedIron", "Raised_FloorPlates")
    for z in [-1.55 + i * 0.31 for i in range(11)]:
        box(mesh, -0.74, 0.74, 0.006, 0.055, z - 0.025, z + 0.025, "MAT_AgedBrass", "Brass_GrateCrossBars")
    for x in [-0.56, -0.28, 0, 0.28, 0.56]:
        box(mesh, x - 0.022, x + 0.022, 0.006, 0.07, -1.72, 1.72, "MAT_RivetedIron", "Iron_GrateLongBars")
    box(mesh, -2, 2, 0.003, 0.025, -0.035, 0.035, "MAT_DarkGrate", "OilDark_PlateSeams")
    box(mesh, -0.035, 0.035, 0.003, 0.025, -2, 2, "MAT_DarkGrate", "OilDark_PlateSeams")
    for x0, x1, z0, z1 in [(-2, 2, -2, -1.88), (-2, 2, 1.88, 2), (-2, -1.88, -2, 2), (1.88, 2, -2, 2)]:
        box(mesh, x0, x1, 0.01, 0.06, z0, z1, "MAT_AgedBrass", "Brass_EdgeTrim")
    for x in [-1.72, -1.08, 1.08, 1.72]:
        for z in [-1.72, -1.08, 1.08, 1.72]:
            cylinder(mesh, "Y", (x, 0.045, z), 0.05, 0.045, 12, "MAT_AgedBrass", "Brass_FloorRivets")
    return mesh


def ceiling_panel():
    mesh = Mesh("SM_CeilingPanel_PipeChannels_4m")
    box(mesh, -2, 2, 0, 0.16, -2, 2, "MAT_RivetedIron", "Iron_CeilingSlab")
    for x in [-0.86, 0.86]:
        box(mesh, x - 0.08, x + 0.08, -0.13, 0, -2, 2, "MAT_AgedBrass", "Brass_PipeChannelRails")
    box(mesh, -0.14, 0.14, -0.08, 0, -2, 2, "MAT_DarkGrate", "Central_ChannelShadow")
    cylinder(mesh, "Z", (0, -0.16, 0), 3.78, 0.075, 16, "MAT_OxidizedPipe", "Oxidized_CeilingPipe")
    for z in [-1.55, -0.55, 0.55, 1.55]:
        box(mesh, -1.2, 1.2, -0.115, -0.055, z - 0.04, z + 0.04, "MAT_AgedBrass", "Brass_ChannelStraps")
    return mesh


def corridor_shell():
    mesh = Mesh("SM_CorridorShell_Straight_4m")
    box(mesh, -2, 2, -0.14, 0, -2, 2, "MAT_RivetedIron", "Floor_Slab")
    box(mesh, -2, -1.82, 0, 3.0, -2, 2, "MAT_OilDarkMasonry", "Left_MasonryWall")
    box(mesh, 1.82, 2, 0, 3.0, -2, 2, "MAT_OilDarkMasonry", "Right_MasonryWall")
    box(mesh, -2, 2, 3.0, 3.16, -2, 2, "MAT_RivetedIron", "Ceiling_Slab")
    for x0, x1 in [(-1.81, -1.73), (1.73, 1.81)]:
        box(mesh, x0, x1, 0, 0.18, -2, 2, "MAT_AgedBrass", "Wall_BaseTrim")
        box(mesh, x0, x1, 2.82, 3, -2, 2, "MAT_AgedBrass", "Wall_CrownTrim")
        for z in [-1.5, -0.5, 0.5, 1.5]:
            cylinder(mesh, "X", ((x0 + x1) / 2, 0.42, z), 0.09, 0.045, 12, "MAT_RivetedIron", "Wall_Rivets")
            cylinder(mesh, "X", ((x0 + x1) / 2, 2.58, z), 0.09, 0.045, 12, "MAT_RivetedIron", "Wall_Rivets")
    cylinder(mesh, "Z", (1.62, 2.18, 0), 3.8, 0.075, 16, "MAT_OxidizedPipe", "Right_WallPipeRun")
    for z in [-1.4, 0, 1.4]:
        box(mesh, 1.67, 1.86, 2.08, 2.28, z - 0.035, z + 0.035, "MAT_AgedBrass", "Pipe_Brackets")
    return mesh


def doorway_frame():
    mesh = Mesh("SM_DoorwayThreshold_Frame_4m")
    box(mesh, -2, -0.88, 0, 3, -0.18, 0.18, "MAT_OilDarkMasonry", "Left_DoorPier")
    box(mesh, 0.88, 2, 0, 3, -0.18, 0.18, "MAT_OilDarkMasonry", "Right_DoorPier")
    box(mesh, -0.88, 0.88, 2.32, 3, -0.18, 0.18, "MAT_OilDarkMasonry", "Top_LintelFill")
    box(mesh, -1.04, -0.88, 0, 2.45, 0.18, 0.34, "MAT_AgedBrass", "Left_BrassJamb")
    box(mesh, 0.88, 1.04, 0, 2.45, 0.18, 0.34, "MAT_AgedBrass", "Right_BrassJamb")
    box(mesh, -1.04, 1.04, 2.28, 2.45, 0.18, 0.34, "MAT_AgedBrass", "Brass_Header")
    box(mesh, -1.12, 1.12, -0.10, 0.08, -0.26, 0.36, "MAT_RivetedIron", "Iron_ThresholdPlate")
    box(mesh, -0.78, 0.78, -0.07, 0.10, 0.05, 0.31, "MAT_DarkGrate", "Threshold_GrateInset")
    for x in [-0.96, 0.96]:
        for y in [0.35, 0.75, 1.15, 1.55, 1.95, 2.35]:
            cylinder(mesh, "Z", (x, y, 0.365), 0.05, 0.045, 12, "MAT_RivetedIron", "Doorframe_Rivets")
    return mesh


def corner_marker():
    mesh = Mesh("SM_CornerIntersection_Marker_4m")
    box(mesh, -2, 2, -0.07, 0, -2, 2, "MAT_RivetedIron", "Low_Profile_BasePlate")
    box(mesh, -0.09, 0.09, 0, 0.055, -2, 2, "MAT_AgedBrass", "Brass_NorthSouthMarker")
    box(mesh, -2, 2, 0, 0.055, -0.09, 0.09, "MAT_AgedBrass", "Brass_EastWestMarker")
    box(mesh, -0.32, 0.32, 0.012, 0.072, -0.32, 0.32, "MAT_DarkGrate", "Center_ServicePlate")
    for x in [-1.78, 1.78]:
        for z in [-1.78, 1.78]:
            cylinder(mesh, "Y", (x, 0.225, z), 0.45, 0.085, 16, "MAT_AgedBrass", "Brass_CornerBollards")
            cylinder(mesh, "Y", (x, 0.485, z), 0.08, 0.12, 16, "MAT_RivetedIron", "Iron_BollardCaps")
    return mesh


def pipe_run():
    mesh = Mesh("SM_PipeRun_Straight_4m")
    cylinder(mesh, "X", (0, 0, 0), 4.0, 0.14, 24, "MAT_OxidizedPipe", "Main_PipeCylinder")
    for x in [-1.92, 1.92]:
        cylinder(mesh, "X", (x, 0, 0), 0.18, 0.22, 24, "MAT_AgedBrass", "Brass_EndFlanges")
    for x in [-1.2, 0, 1.2]:
        box(mesh, x - 0.055, x + 0.055, -0.28, -0.13, -0.18, 0.18, "MAT_AgedBrass", "Pipe_HangerStraps")
        box(mesh, x - 0.18, x + 0.18, -0.34, -0.28, -0.08, 0.08, "MAT_RivetedIron", "Pipe_MountingFeet")
    return mesh


def pipe_elbow():
    mesh = Mesh("SM_PipeElbow_90deg")
    elbow_torus(mesh, (0, 0, 0), 0.62, 0.14, 18, 16, "MAT_OxidizedPipe", "Quarter_Bend_Pipe")
    cylinder(mesh, "Z", (0.62, 0, -0.08), 0.22, 0.20, 20, "MAT_AgedBrass", "Start_BrassFlange")
    cylinder(mesh, "X", (-0.08, 0, 0.62), 0.22, 0.20, 20, "MAT_AgedBrass", "End_BrassFlange")
    return mesh


def valve_wheel():
    mesh = Mesh("SM_ValveWheel_Prop")
    torus_xy(mesh, (0, 0, 0), 0.31, 0.035, 32, 10, "MAT_AgedBrass", "Brass_OuterWheelRing")
    cylinder(mesh, "Z", (0, 0, 0), 0.12, 0.09, 18, "MAT_RivetedIron", "Iron_CenterHub")
    cylinder(mesh, "Z", (0, 0, -0.10), 0.22, 0.055, 16, "MAT_OxidizedPipe", "Valve_Stem")
    box(mesh, -0.29, 0.29, -0.025, 0.025, -0.03, 0.03, "MAT_AgedBrass", "Horizontal_Spoke")
    box(mesh, -0.025, 0.025, -0.29, 0.29, -0.03, 0.03, "MAT_AgedBrass", "Vertical_Spoke")
    for point in [(0.31, 0), (0, 0.31), (-0.31, 0), (0, -0.31)]:
        cylinder(mesh, "Z", (point[0], point[1], 0), 0.08, 0.038, 12, "MAT_RivetedIron", "Wheel_HandleKnobs")
    return mesh


def pressure_gauge():
    mesh = Mesh("SM_PressureGauge_Prop")
    cylinder(mesh, "Z", (0, 0, 0), 0.14, 0.245, 32, "MAT_AgedBrass", "Brass_GaugeHousing")
    cylinder(mesh, "Z", (0, 0, 0.081), 0.018, 0.188, 32, "MAT_GaugeFace", "Gauge_DialFace")
    torus_xy(mesh, (0, 0, 0.095), 0.205, 0.022, 32, 8, "MAT_RivetedIron", "Iron_FrontBezel")
    mesh.face([(-0.014, -0.020, 0.107), (0.014, -0.020, 0.107), (0.115, 0.110, 0.107)], "MAT_GaugeNeedle", "Gauge_RedNeedle", orient=(0, 0, 1))
    cylinder(mesh, "Z", (0, 0, 0.113), 0.012, 0.026, 12, "MAT_RivetedIron", "Gauge_CenterPin")
    cylinder(mesh, "Y", (0, -0.285, -0.03), 0.26, 0.045, 14, "MAT_OxidizedPipe", "Lower_PipeSocket")
    return mesh


def lamp_housing():
    mesh = Mesh("SM_LampHousing_Prop")
    cylinder(mesh, "Y", (0, 0.30, 0), 0.48, 0.155, 24, "MAT_LampGlass", "Cylindrical_GlassGlobe")
    cylinder(mesh, "Y", (0, 0.06, 0), 0.10, 0.22, 24, "MAT_AgedBrass", "Bottom_BrassCap")
    cylinder(mesh, "Y", (0, 0.55, 0), 0.12, 0.23, 24, "MAT_AgedBrass", "Top_BrassCap")
    cylinder(mesh, "Y", (0, 0.30, 0), 0.34, 0.075, 16, "MAT_WarmLampGlow", "Warm_InnerGlow")
    for i in range(8):
        angle = 2 * math.pi * i / 8
        cylinder(mesh, "Y", (math.cos(angle) * 0.195, 0.30, math.sin(angle) * 0.195), 0.54, 0.014, 8, "MAT_RivetedIron", "Iron_CageRods")
    box(mesh, -0.08, 0.08, 0.61, 0.76, -0.08, 0.08, "MAT_AgedBrass", "Lamp_TopYoke")
    box(mesh, -0.28, 0.28, 0.74, 0.80, -0.04, 0.04, "MAT_RivetedIron", "Lamp_HangingCrossbar")
    return mesh


def reference_assembly():
    mesh = Mesh("KIT_ModularKit_ReferenceAssembly")
    box(mesh, -2, 2, -0.14, 0, -2, 2, "MAT_RivetedIron", "Reference_Floor")
    box(mesh, -2, -1.84, 0, 3, -2, 2, "MAT_OilDarkMasonry", "Reference_LeftWall")
    box(mesh, 1.84, 2, 0, 3, -2, 2, "MAT_OilDarkMasonry", "Reference_RightWall")
    box(mesh, -2, 2, 3, 3.16, -2, 2, "MAT_RivetedIron", "Reference_Ceiling")
    box(mesh, -1.84, -1.70, 0, 0.20, -2, 2, "MAT_AgedBrass", "Reference_LeftBaseTrim")
    box(mesh, 1.70, 1.84, 0, 0.20, -2, 2, "MAT_AgedBrass", "Reference_RightBaseTrim")
    cylinder(mesh, "Z", (1.62, 2.15, 0), 3.7, 0.075, 16, "MAT_OxidizedPipe", "Reference_WallPipe")
    cylinder(mesh, "Y", (-1.80, 1.55, -1.55), 0.45, 0.16, 18, "MAT_LampGlass", "Reference_LeftLampGlass")
    cylinder(mesh, "Y", (-1.80, 1.55, -1.55), 0.34, 0.075, 16, "MAT_WarmLampGlow", "Reference_LeftLampGlow")
    cylinder(mesh, "Z", (0, 1.4, 1.92), 0.10, 0.22, 24, "MAT_AgedBrass", "Reference_GaugeHousing")
    return mesh


BUILDERS = [
    (wall_panel, "4m wall panel with riveted brass trim and oil-dark masonry slabs", "Origin at lower center of front module; face detail points toward +Z"),
    (floor_tile, "4m floor tile with recessed grate, raised plate seams, brass trim", "Origin at tile center, walking surface near Y=0"),
    (ceiling_panel, "4m ceiling panel with underside pipe channels and straps", "Origin at panel center; place underside around ceiling height"),
    (corridor_shell, "4m straight corridor shell segment, open at both ends", "Origin centered on corridor bay; floor top at Y=0"),
    (doorway_frame, "4m wall module doorway frame and threshold", "Origin at doorway centerline; clear opening approx. 1.76m W x 2.32m H"),
    (corner_marker, "4m intersection marker plate with brass cross markers and corner bollards", "Origin at center of intersection marker"),
    (pipe_run, "4m straight pipe run with flanges and brackets", "Origin centered on pipe centerline"),
    (pipe_elbow, "90 degree oxidized pipe elbow with brass flanges", "Origin at elbow bend corner center"),
    (valve_wheel, "Valve wheel prop with rim, spokes, hub, and stem", "Origin at valve hub center"),
    (pressure_gauge, "Pressure gauge prop with dial face, rim, needle, and pipe socket", "Origin at gauge dial center"),
    (lamp_housing, "Caged steampunk lamp housing with glass and warm glow slot", "Origin at lamp base center"),
    (reference_assembly, "Combined reference corridor bay for import sanity checks", "Origin centered in 4m corridor bay"),
]


def project(point):
    x, y, z = point
    return (x - z) * 0.94, y * 1.10 + (x + z) * 0.38, x + z - y * 0.45


def render_preview(mesh, path):
    width, height = 760, 560
    projected_faces = []
    coords = []
    for points, material in mesh.preview_faces:
        projected = [project(point) for point in points]
        coords.extend((x, y) for x, y, _ in projected)
        projected_faces.append((sum(depth for _, _, depth in projected) / len(projected), projected, material))
    min_x, max_x = min(x for x, _ in coords), max(x for x, _ in coords)
    min_y, max_y = min(y for _, y in coords), max(y for _, y in coords)
    margin = 58
    scale = min((width - margin * 2) / max(0.01, max_x - min_x), (height - margin * 2 - 28) / max(0.01, max_y - min_y))
    image = Image.new("RGB", (width, height), (21, 22, 22))
    draw = ImageDraw.Draw(image, "RGBA")
    for depth, projected, material in sorted(projected_faces, key=lambda item: item[0]):
        base = MATERIALS[material]["color"]
        shade = 0.80 + 0.18 * math.sin(depth * 0.7)
        fill = tuple(clamp(channel * shade) for channel in base) + (236,)
        points = [((x - min_x) * scale + margin, height - ((y - min_y) * scale + margin)) for x, y, _ in projected]
        draw.polygon(points, fill=fill, outline=(31, 30, 27, 130))
    draw.rectangle((0, 0, width, 32), fill=(9, 9, 9, 190))
    draw.text((14, 10), mesh.name, fill=(232, 224, 204), font=ImageFont.load_default())
    image.save(path)


def make_previews(meshes):
    preview_files = []
    for mesh in meshes:
        file_name = f"PREVIEW_{mesh.name}.png"
        render_preview(mesh, OUT_DIR / file_name)
        preview_files.append(file_name)

    thumb_w, thumb_h = 360, 274
    cols = 3
    rows = math.ceil(len(preview_files) / cols)
    sheet = Image.new("RGB", (cols * thumb_w, rows * thumb_h), (18, 19, 19))
    draw = ImageDraw.Draw(sheet)
    for index, file_name in enumerate(preview_files):
        image = Image.open(OUT_DIR / file_name).resize((thumb_w, thumb_h - 26), Image.Resampling.LANCZOS)
        x, y = (index % cols) * thumb_w, (index // cols) * thumb_h
        sheet.paste(image, (x, y))
        draw.rectangle((x, y + thumb_h - 26, x + thumb_w, y + thumb_h), fill=(11, 11, 11))
        draw.text((x + 10, y + thumb_h - 18), file_name.replace("PREVIEW_", "").replace(".png", ""), fill=(228, 219, 198), font=ImageFont.load_default())
    sheet.save(OUT_DIR / "PREVIEW_ModularKit_ContactSheet.png")

    texture_files = [material["map"] for material in MATERIALS.values() if material["map"]]
    swatches = Image.new("RGB", (4 * 260, 2 * 252), (18, 19, 19))
    draw = ImageDraw.Draw(swatches)
    for index, file_name in enumerate(texture_files):
        image = Image.open(OUT_DIR / file_name).resize((228, 188), Image.Resampling.LANCZOS)
        x, y = (index % 4) * 260 + 16, (index // 4) * 252 + 16
        swatches.paste(image, (x, y))
        draw.text((x, y + 196), file_name.replace("KIT_MAT_", "").replace("_BaseColor.png", ""), fill=(230, 220, 198), font=ImageFont.load_default())
    swatches.save(OUT_DIR / "PREVIEW_TextureSwatches.png")
    return preview_files + ["PREVIEW_ModularKit_ContactSheet.png", "PREVIEW_TextureSwatches.png"]


def write_manifest(mesh_notes, preview_files):
    mesh_entries = []
    for mesh, description, pivot_note in mesh_notes:
        b0, b1 = mesh.bbox()
        mesh_entries.append(
            {
                "name": mesh.name,
                "file": f"{mesh.name}.obj",
                "description": description,
                "dimensions_m_xyz": mesh.dimensions(),
                "bbox_min_xyz_m": [round(value, 3) for value in b0],
                "bbox_max_xyz_m": [round(value, 3) for value in b1],
                "pivot_note": pivot_note,
                "material_slots": sorted({face[1] for face in mesh.faces}),
                "face_count": len(mesh.faces),
                "vertex_count_authored": len(mesh.vertices),
                "source_units": "meters",
            }
        )
    manifest = {
        "kit_name": "Brassworks Breach ModularKit 01 - Steampunk Corridor/Room Starter",
        "generated_at_local": dt.datetime.now(dt.timezone.utc).astimezone().isoformat(timespec="seconds"),
        "unit_scale": "1 OBJ unit = 1 Unity meter",
        "coordinate_system": "+Y up, +Z forward; authored for Unity OBJ import",
        "material_library": "KIT_ModularKit_Materials.mtl",
        "materials": [
            {"name": name, "base_color_texture": material["map"], "metallic_note": material["metallic"], "roughness_note": material["roughness"], "alpha": material.get("alpha", 1.0)}
            for name, material in MATERIALS.items()
        ],
        "meshes": mesh_entries,
        "previews": preview_files,
        "collision_notes": "Use simple box colliders for wall/floor/ceiling/shell/doorway modules; capsule colliders for pipe runs; convex mesh or primitive colliders for valve/gauge/lamp props. Avoid visual rivets/grate bars as gameplay collision.",
        "lod_notes": "LOD0 only in this staging pass. Suggested LOD1 removes rivets, individual grate bars, and cage rods; suggested LOD2 uses proxy boxes/cylinders only.",
        "import_notes": "Import OBJ files into Unity at scale factor 1.0. Keep the MTL and KIT_MAT_* textures beside the OBJ files so material slots resolve. If Unity does not auto-wire the MTL maps, assign the listed textures manually.",
    }
    (OUT_DIR / "KIT_ModularKit_Manifest.json").write_text(json.dumps(manifest, indent=2), encoding="utf-8")


def main():
    write_materials()
    meshes = []
    mesh_notes = []
    for builder, description, pivot_note in BUILDERS:
        mesh = builder()
        mesh.write()
        meshes.append(mesh)
        mesh_notes.append((mesh, description, pivot_note))
    preview_files = make_previews(meshes)
    write_manifest(mesh_notes, preview_files)
    print(f"Generated {len(meshes)} OBJ meshes, {sum(1 for material in MATERIALS.values() if material['map'])} textures, and {len(preview_files)} previews in {OUT_DIR}")


if __name__ == "__main__":
    main()
