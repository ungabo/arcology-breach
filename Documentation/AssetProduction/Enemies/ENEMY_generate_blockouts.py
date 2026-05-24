from __future__ import annotations

from dataclasses import dataclass, field
from datetime import datetime, timezone
from pathlib import Path
import json
import math
from typing import Iterable

from PIL import Image, ImageDraw, ImageFont


ROOT = Path(__file__).resolve().parents[2].parent
ASSET_DIR = ROOT / "Assets" / "_Project" / "ArtStaging" / "Enemies"
DOC_DIR = ROOT / "Documentation" / "AssetProduction" / "Enemies"
MTL_NAME = "ENEMY_BlockoutMaterials.mtl"


MATERIALS = {
    "MAT_ENEMY_AgedBrass": {
        "kd": (0.78, 0.52, 0.20),
        "ka": (0.08, 0.055, 0.025),
        "ks": (0.42, 0.34, 0.20),
        "ns": 48,
        "label": "aged brass / warm worn metal",
    },
    "MAT_ENEMY_BlackenedIron": {
        "kd": (0.10, 0.105, 0.105),
        "ka": (0.015, 0.015, 0.015),
        "ks": (0.18, 0.17, 0.16),
        "ns": 30,
        "label": "blackened iron chassis",
    },
    "MAT_ENEMY_CopperPipe": {
        "kd": (0.60, 0.26, 0.12),
        "ka": (0.06, 0.025, 0.015),
        "ks": (0.36, 0.22, 0.14),
        "ns": 36,
        "label": "aged copper pipe",
    },
    "MAT_ENEMY_DarkRubber": {
        "kd": (0.035, 0.030, 0.028),
        "ka": (0.005, 0.005, 0.005),
        "ks": (0.05, 0.045, 0.040),
        "ns": 12,
        "label": "dark hose/gasket",
    },
    "MAT_ENEMY_CreamEnamel": {
        "kd": (0.72, 0.66, 0.50),
        "ka": (0.06, 0.055, 0.045),
        "ks": (0.18, 0.16, 0.12),
        "ns": 18,
        "label": "chipped cream enamel mask plates",
    },
    "MAT_ENEMY_FurnaceGlowAmber": {
        "kd": (1.00, 0.42, 0.08),
        "ka": (0.20, 0.06, 0.01),
        "ks": (0.25, 0.14, 0.05),
        "ke": (1.00, 0.30, 0.03),
        "ns": 20,
        "label": "amber furnace emission proxy",
    },
    "MAT_ENEMY_SootShadow": {
        "kd": (0.018, 0.017, 0.015),
        "ka": (0.002, 0.002, 0.002),
        "ks": (0.04, 0.035, 0.030),
        "ns": 8,
        "label": "deep visor shadow",
    },
}


PREVIEW_COLORS = {
    name: tuple(int(c * 255) for c in data["kd"])
    for name, data in MATERIALS.items()
}


def v_add(a, b):
    return (a[0] + b[0], a[1] + b[1], a[2] + b[2])


def v_sub(a, b):
    return (a[0] - b[0], a[1] - b[1], a[2] - b[2])


def v_mul(a, s):
    return (a[0] * s, a[1] * s, a[2] * s)


def v_dot(a, b):
    return a[0] * b[0] + a[1] * b[1] + a[2] * b[2]


def v_cross(a, b):
    return (
        a[1] * b[2] - a[2] * b[1],
        a[2] * b[0] - a[0] * b[2],
        a[0] * b[1] - a[1] * b[0],
    )


def v_len(a):
    return math.sqrt(v_dot(a, a))


def v_norm(a):
    length = v_len(a)
    if length < 1e-8:
        return (0.0, 1.0, 0.0)
    return (a[0] / length, a[1] / length, a[2] / length)


def axis_vector(axis: str):
    if axis == "x":
        return (1.0, 0.0, 0.0)
    if axis == "z":
        return (0.0, 0.0, 1.0)
    return (0.0, 1.0, 0.0)


def basis_from_axis(axis):
    w = v_norm(axis)
    helper = (0.0, 1.0, 0.0)
    if abs(v_dot(w, helper)) > 0.92:
        helper = (1.0, 0.0, 0.0)
    u = v_norm(v_cross(helper, w))
    v = v_norm(v_cross(w, u))
    return u, v, w


@dataclass
class Face:
    indices: tuple[int, ...]
    material: str
    group: str


@dataclass
class Mesh:
    name: str
    vertices: list[tuple[float, float, float]] = field(default_factory=list)
    faces: list[Face] = field(default_factory=list)

    def add_vertex(self, point):
        self.vertices.append(tuple(round(float(c), 5) for c in point))
        return len(self.vertices)

    def add_face(self, indices: Iterable[int], material: str, group: str):
        self.faces.append(Face(tuple(indices), material, group))

    def triangle_count(self):
        return sum(max(0, len(face.indices) - 2) for face in self.faces)

    def bounds(self):
        xs = [v[0] for v in self.vertices]
        ys = [v[1] for v in self.vertices]
        zs = [v[2] for v in self.vertices]
        return (min(xs), min(ys), min(zs)), (max(xs), max(ys), max(zs))

    def dimensions(self):
        lo, hi = self.bounds()
        return tuple(round(hi[i] - lo[i], 3) for i in range(3))


def add_box(mesh, center, size, material, group):
    cx, cy, cz = center
    sx, sy, sz = (size[0] / 2, size[1] / 2, size[2] / 2)
    x0, x1 = cx - sx, cx + sx
    y0, y1 = cy - sy, cy + sy
    z0, z1 = cz - sz, cz + sz
    verts = [
        (x0, y0, z0),
        (x1, y0, z0),
        (x1, y1, z0),
        (x0, y1, z0),
        (x0, y0, z1),
        (x1, y0, z1),
        (x1, y1, z1),
        (x0, y1, z1),
    ]
    idx = [mesh.add_vertex(v) for v in verts]
    faces = [
        (idx[0], idx[3], idx[2], idx[1]),
        (idx[4], idx[5], idx[6], idx[7]),
        (idx[0], idx[4], idx[7], idx[3]),
        (idx[1], idx[2], idx[6], idx[5]),
        (idx[0], idx[1], idx[5], idx[4]),
        (idx[3], idx[7], idx[6], idx[2]),
    ]
    for face in faces:
        mesh.add_face(face, material, group)


def add_cylinder_between(mesh, start, end, radius, material, group, segments=12, cap=True):
    axis = v_sub(end, start)
    if v_len(axis) < 1e-6:
        return
    u, v, _ = basis_from_axis(axis)
    ring_a = []
    ring_b = []
    for i in range(segments):
        angle = 2 * math.pi * i / segments
        offset = v_add(v_mul(u, math.cos(angle) * radius), v_mul(v, math.sin(angle) * radius))
        ring_a.append(mesh.add_vertex(v_add(start, offset)))
        ring_b.append(mesh.add_vertex(v_add(end, offset)))
    for i in range(segments):
        j = (i + 1) % segments
        mesh.add_face((ring_a[i], ring_a[j], ring_b[j], ring_b[i]), material, group)
    if cap:
        mesh.add_face(tuple(reversed(ring_a)), material, group)
        mesh.add_face(tuple(ring_b), material, group)


def add_cylinder(mesh, center, radius, depth, axis, material, group, segments=12, cap=True):
    w = axis_vector(axis)
    start = v_add(center, v_mul(w, -depth / 2))
    end = v_add(center, v_mul(w, depth / 2))
    add_cylinder_between(mesh, start, end, radius, material, group, segments, cap)


def add_cone_between(mesh, base, tip, radius, material, group, segments=12):
    axis = v_sub(tip, base)
    if v_len(axis) < 1e-6:
        return
    u, v, _ = basis_from_axis(axis)
    ring = []
    for i in range(segments):
        angle = 2 * math.pi * i / segments
        offset = v_add(v_mul(u, math.cos(angle) * radius), v_mul(v, math.sin(angle) * radius))
        ring.append(mesh.add_vertex(v_add(base, offset)))
    tip_idx = mesh.add_vertex(tip)
    for i in range(segments):
        j = (i + 1) % segments
        mesh.add_face((ring[i], ring[j], tip_idx), material, group)
    mesh.add_face(tuple(reversed(ring)), material, group)


def add_sphere(mesh, center, radius, material, group, segments=12, rings=6):
    top = mesh.add_vertex((center[0], center[1] + radius, center[2]))
    bottom = mesh.add_vertex((center[0], center[1] - radius, center[2]))
    ring_indices = []
    for r in range(1, rings):
        phi = math.pi * r / rings
        y = math.cos(phi) * radius
        rr = math.sin(phi) * radius
        row = []
        for i in range(segments):
            angle = 2 * math.pi * i / segments
            row.append(
                mesh.add_vertex(
                    (
                        center[0] + math.cos(angle) * rr,
                        center[1] + y,
                        center[2] + math.sin(angle) * rr,
                    )
                )
            )
        ring_indices.append(row)
    first = ring_indices[0]
    for i in range(segments):
        mesh.add_face((top, first[i], first[(i + 1) % segments]), material, group)
    for a, b in zip(ring_indices, ring_indices[1:]):
        for i in range(segments):
            mesh.add_face((a[i], b[i], b[(i + 1) % segments], a[(i + 1) % segments]), material, group)
    last = ring_indices[-1]
    for i in range(segments):
        mesh.add_face((last[(i + 1) % segments], last[i], bottom), material, group)


def add_gear(mesh, center, inner_radius, root_radius, tooth_radius, thickness, axis, teeth, material, group):
    u, v, w = basis_from_axis(axis_vector(axis))
    count = teeth * 2
    fo, fi, bo, bi = [], [], [], []
    for i in range(count):
        angle = 2 * math.pi * i / count
        radius = tooth_radius if i % 2 == 0 else root_radius
        radial = v_add(v_mul(u, math.cos(angle)), v_mul(v, math.sin(angle)))
        front = v_add(center, v_mul(w, thickness / 2))
        back = v_add(center, v_mul(w, -thickness / 2))
        fo.append(mesh.add_vertex(v_add(front, v_mul(radial, radius))))
        fi.append(mesh.add_vertex(v_add(front, v_mul(radial, inner_radius))))
        bo.append(mesh.add_vertex(v_add(back, v_mul(radial, radius))))
        bi.append(mesh.add_vertex(v_add(back, v_mul(radial, inner_radius))))
    for i in range(count):
        j = (i + 1) % count
        mesh.add_face((fo[i], fo[j], fi[j], fi[i]), material, group)
        mesh.add_face((bo[i], bi[i], bi[j], bo[j]), material, group)
        mesh.add_face((fo[i], bo[i], bo[j], fo[j]), material, group)
        mesh.add_face((fi[j], bi[j], bi[i], fi[i]), material, group)
    rivet_axis = axis_vector(axis)
    for i in range(teeth):
        angle = 2 * math.pi * i / teeth
        radial = v_add(v_mul(u, math.cos(angle)), v_mul(v, math.sin(angle)))
        pos = v_add(center, v_mul(radial, (inner_radius + root_radius) * 0.5))
        add_cylinder_between(
            mesh,
            v_add(pos, v_mul(rivet_axis, thickness * 0.50)),
            v_add(pos, v_mul(rivet_axis, thickness * 0.68)),
            thickness * 0.10,
            "MAT_ENEMY_BlackenedIron",
            f"{group}_rivet",
            6,
            True,
        )


def add_mask(mesh, center, scale, group):
    sx, sy, sz = scale
    add_box(mesh, center, (0.55 * sx, 0.30 * sy, 0.18 * sz), "MAT_ENEMY_CreamEnamel", f"{group}_mask_plate")
    add_box(
        mesh,
        (center[0], center[1] + 0.035 * sy, center[2] + 0.098 * sz),
        (0.42 * sx, 0.055 * sy, 0.022 * sz),
        "MAT_ENEMY_SootShadow",
        f"{group}_visor_slit",
    )
    add_box(
        mesh,
        (center[0] - 0.315 * sx, center[1] - 0.015 * sy, center[2]),
        (0.08 * sx, 0.22 * sy, 0.16 * sz),
        "MAT_ENEMY_AgedBrass",
        f"{group}_left_cheek_hinge",
    )
    add_box(
        mesh,
        (center[0] + 0.315 * sx, center[1] - 0.015 * sy, center[2]),
        (0.08 * sx, 0.22 * sy, 0.16 * sz),
        "MAT_ENEMY_AgedBrass",
        f"{group}_right_cheek_hinge",
    )
    add_cylinder(
        mesh,
        (center[0], center[1] - 0.19 * sy, center[2] + 0.02 * sz),
        0.085 * sx,
        0.12 * sz,
        "z",
        "MAT_ENEMY_CopperPipe",
        f"{group}_snout_filter",
        10,
        True,
    )


def add_furnace_core(mesh, center, radius, group):
    add_cylinder(mesh, center, radius * 1.18, radius * 0.18, "z", "MAT_ENEMY_BlackenedIron", f"{group}_iron_ring", 16, True)
    add_sphere(mesh, (center[0], center[1], center[2] + radius * 0.08), radius * 0.78, "MAT_ENEMY_FurnaceGlowAmber", f"{group}_amber_glass", 12, 6)
    add_cylinder(mesh, (center[0], center[1], center[2] + radius * 0.12), radius * 1.30, radius * 0.045, "z", "MAT_ENEMY_AgedBrass", f"{group}_brass_lip", 16, True)


def add_pipe_backpack(mesh, center, scale, group):
    sx, sy, sz = scale
    add_cylinder(mesh, center, 0.16 * sx, 0.52 * sy, "y", "MAT_ENEMY_BlackenedIron", f"{group}_pressure_tank", 12, True)
    add_cylinder(mesh, (center[0] - 0.19 * sx, center[1] - 0.02 * sy, center[2]), 0.055 * sx, 0.48 * sy, "y", "MAT_ENEMY_CopperPipe", f"{group}_left_pipe", 10, True)
    add_cylinder(mesh, (center[0] + 0.19 * sx, center[1] - 0.02 * sy, center[2]), 0.055 * sx, 0.48 * sy, "y", "MAT_ENEMY_CopperPipe", f"{group}_right_pipe", 10, True)
    add_cylinder_between(
        mesh,
        (center[0] - 0.19 * sx, center[1] + 0.20 * sy, center[2]),
        (center[0] - 0.34 * sx, center[1] + 0.34 * sy, center[2] + 0.10 * sz),
        0.035 * sx,
        "MAT_ENEMY_CopperPipe",
        f"{group}_left_exhaust_bend",
        8,
        True,
    )
    add_cylinder_between(
        mesh,
        (center[0] + 0.19 * sx, center[1] + 0.20 * sy, center[2]),
        (center[0] + 0.34 * sx, center[1] + 0.34 * sy, center[2] + 0.10 * sz),
        0.035 * sx,
        "MAT_ENEMY_CopperPipe",
        f"{group}_right_exhaust_bend",
        8,
        True,
    )
    add_cylinder(
        mesh,
        (center[0], center[1] - 0.31 * sy, center[2] + 0.01 * sz),
        0.20 * sx,
        0.08 * sy,
        "y",
        "MAT_ENEMY_AgedBrass",
        f"{group}_mount_collar",
        12,
        True,
    )


def add_piston_leg(mesh, hip, foot, side_name, scale=1.0):
    knee = ((hip[0] + foot[0]) * 0.5, (hip[1] + foot[1]) * 0.5 + 0.05 * scale, (hip[2] + foot[2]) * 0.5 + 0.02)
    add_cylinder_between(mesh, hip, knee, 0.055 * scale, "MAT_ENEMY_BlackenedIron", f"{side_name}_upper_piston", 8, True)
    add_cylinder_between(mesh, knee, foot, 0.045 * scale, "MAT_ENEMY_CopperPipe", f"{side_name}_lower_piston", 8, True)
    add_sphere(mesh, knee, 0.095 * scale, "MAT_ENEMY_AgedBrass", f"{side_name}_knee_socket", 10, 5)
    add_box(mesh, (foot[0], foot[1] - 0.035 * scale, foot[2] + 0.03 * scale), (0.28 * scale, 0.07 * scale, 0.36 * scale), "MAT_ENEMY_BlackenedIron", f"{side_name}_broad_foot")


def build_scrapper():
    mesh = Mesh("ENEMY_ScrapperAutomaton_Blockout_LOD0")
    add_cylinder(mesh, (0.0, 0.90, 0.0), 0.34, 0.72, "y", "MAT_ENEMY_BlackenedIron", "boiler_torso", 14, True)
    add_cylinder(mesh, (0.0, 1.24, 0.0), 0.37, 0.09, "y", "MAT_ENEMY_AgedBrass", "upper_boiler_band", 14, True)
    add_cylinder(mesh, (0.0, 0.56, 0.0), 0.36, 0.09, "y", "MAT_ENEMY_AgedBrass", "lower_boiler_band", 14, True)
    add_furnace_core(mesh, (0.0, 0.92, 0.33), 0.145, "chest_core")
    add_mask(mesh, (0.0, 1.40, 0.12), (0.86, 0.82, 0.92), "head")
    add_pipe_backpack(mesh, (0.0, 0.95, -0.36), (0.95, 0.95, 0.95), "backpack")
    for side, sign in (("left", -1), ("right", 1)):
        shoulder = (sign * 0.42, 1.10, 0.02)
        elbow = (sign * 0.64, 0.82, 0.10)
        wrist = (sign * 0.58, 0.56, 0.28)
        add_gear(mesh, shoulder, 0.10, 0.16, 0.21, 0.10, "x", 10, "MAT_ENEMY_AgedBrass", f"{side}_cog_shoulder")
        add_cylinder_between(mesh, shoulder, elbow, 0.055, "MAT_ENEMY_BlackenedIron", f"{side}_upper_claw_arm", 8, True)
        add_cylinder_between(mesh, elbow, wrist, 0.048, "MAT_ENEMY_CopperPipe", f"{side}_forearm_piston", 8, True)
        add_sphere(mesh, elbow, 0.085, "MAT_ENEMY_AgedBrass", f"{side}_elbow_joint", 10, 5)
        add_box(mesh, (wrist[0], wrist[1], wrist[2] + 0.08), (0.18, 0.13, 0.16), "MAT_ENEMY_BlackenedIron", f"{side}_claw_palm")
        add_box(mesh, (wrist[0] + sign * 0.06, wrist[1] - 0.03, wrist[2] + 0.25), (0.06, 0.18, 0.28), "MAT_ENEMY_AgedBrass", f"{side}_outer_claw")
        add_box(mesh, (wrist[0] - sign * 0.05, wrist[1] + 0.03, wrist[2] + 0.24), (0.055, 0.16, 0.24), "MAT_ENEMY_AgedBrass", f"{side}_inner_claw")
    add_piston_leg(mesh, (-0.17, 0.54, 0.00), (-0.25, 0.10, 0.10), "left_leg", 0.95)
    add_piston_leg(mesh, (0.17, 0.54, 0.00), (0.25, 0.10, 0.10), "right_leg", 0.95)
    add_box(mesh, (0.0, 0.35, -0.03), (0.42, 0.16, 0.30), "MAT_ENEMY_BlackenedIron", "pelvis_block")
    return mesh


def build_lancer():
    mesh = Mesh("ENEMY_LancerAutomaton_Blockout_LOD0")
    add_cylinder(mesh, (0.0, 1.18, 0.0), 0.25, 0.88, "y", "MAT_ENEMY_BlackenedIron", "tall_pressure_torso", 14, True)
    add_cylinder(mesh, (0.0, 1.62, 0.0), 0.29, 0.07, "y", "MAT_ENEMY_AgedBrass", "upper_brass_band", 14, True)
    add_cylinder(mesh, (0.0, 0.75, 0.0), 0.28, 0.07, "y", "MAT_ENEMY_AgedBrass", "lower_brass_band", 14, True)
    add_furnace_core(mesh, (0.0, 1.23, 0.27), 0.105, "narrow_chest_core")
    add_mask(mesh, (0.0, 1.86, 0.09), (0.78, 0.74, 0.82), "needle_head")
    add_pipe_backpack(mesh, (0.0, 1.20, -0.33), (0.82, 1.18, 0.82), "tall_pipe_backpack")
    add_cylinder_between(mesh, (-0.20, 1.38, 0.13), (-0.17, 1.25, 0.62), 0.045, "MAT_ENEMY_BlackenedIron", "left_lance_support", 8, True)
    add_cylinder_between(mesh, (0.22, 1.36, 0.10), (0.18, 1.23, 0.62), 0.045, "MAT_ENEMY_BlackenedIron", "right_lance_support", 8, True)
    add_cylinder_between(mesh, (0.0, 1.28, 0.30), (0.0, 1.32, 1.35), 0.075, "MAT_ENEMY_CopperPipe", "pressure_lance_barrel", 12, True)
    add_cylinder_between(mesh, (0.0, 1.32, 1.08), (0.0, 1.32, 1.42), 0.12, "MAT_ENEMY_BlackenedIron", "muzzle_collar", 12, True)
    add_cone_between(mesh, (0.0, 1.32, 1.42), (0.0, 1.32, 1.68), 0.105, "MAT_ENEMY_AgedBrass", "lance_pressure_nozzle", 12)
    add_cylinder_between(mesh, (0.0, 1.19, -0.25), (0.0, 1.25, 0.50), 0.035, "MAT_ENEMY_DarkRubber", "feed_hose_under_lance", 8, True)
    for side, sign in (("left", -1), ("right", 1)):
        shoulder = (sign * 0.32, 1.45, 0.02)
        hand = (sign * 0.18, 1.22, 0.58)
        add_gear(mesh, shoulder, 0.075, 0.12, 0.155, 0.08, "x", 9, "MAT_ENEMY_AgedBrass", f"{side}_small_shoulder_gear")
        add_cylinder_between(mesh, shoulder, hand, 0.040, "MAT_ENEMY_BlackenedIron", f"{side}_thin_arm_brace", 8, True)
        add_box(mesh, hand, (0.13, 0.10, 0.13), "MAT_ENEMY_AgedBrass", f"{side}_lance_grip")
    add_piston_leg(mesh, (-0.14, 0.72, -0.02), (-0.22, 0.10, 0.06), "left_stilt_leg", 0.82)
    add_piston_leg(mesh, (0.14, 0.72, -0.02), (0.22, 0.10, 0.06), "right_stilt_leg", 0.82)
    add_box(mesh, (0.0, 0.52, -0.02), (0.34, 0.13, 0.25), "MAT_ENEMY_BlackenedIron", "narrow_pelvis")
    return mesh


def build_turret():
    mesh = Mesh("ENEMY_SentinelTurret_Blockout_LOD0")
    add_cylinder(mesh, (0.0, 0.055, 0.0), 0.46, 0.11, "y", "MAT_ENEMY_BlackenedIron", "floor_mount_disc", 18, True)
    add_gear(mesh, (0.0, 0.21, 0.0), 0.22, 0.32, 0.39, 0.10, "y", 14, "MAT_ENEMY_AgedBrass", "yaw_gear_ring")
    add_cylinder(mesh, (0.0, 0.42, 0.0), 0.22, 0.34, "y", "MAT_ENEMY_CopperPipe", "central_pressure_column", 14, True)
    add_box(mesh, (0.0, 0.70, 0.10), (0.58, 0.34, 0.44), "MAT_ENEMY_BlackenedIron", "turret_iron_housing")
    add_furnace_core(mesh, (0.0, 0.72, 0.335), 0.090, "turret_sight_core")
    add_cylinder_between(mesh, (0.0, 0.72, 0.28), (0.0, 0.72, 1.03), 0.090, "MAT_ENEMY_CopperPipe", "short_pressure_cannon", 14, True)
    add_cylinder_between(mesh, (0.0, 0.72, 0.86), (0.0, 0.72, 1.12), 0.145, "MAT_ENEMY_BlackenedIron", "heavy_muzzle_brake", 14, True)
    add_cylinder(mesh, (-0.42, 0.70, 0.07), 0.17, 0.18, "x", "MAT_ENEMY_AgedBrass", "left_ammo_drum", 12, True)
    add_cylinder(mesh, (0.42, 0.70, 0.07), 0.17, 0.18, "x", "MAT_ENEMY_AgedBrass", "right_ammo_drum", 12, True)
    add_box(mesh, (0.0, 0.70, -0.30), (0.62, 0.42, 0.09), "MAT_ENEMY_AgedBrass", "rear_wall_mount_plate")
    add_box(mesh, (0.0, 1.03, -0.08), (0.52, 0.08, 0.42), "MAT_ENEMY_BlackenedIron", "ceiling_clamp_plate")
    add_cylinder_between(mesh, (-0.28, 0.50, -0.18), (-0.28, 0.98, -0.10), 0.045, "MAT_ENEMY_CopperPipe", "left_vertical_mount_pipe", 8, True)
    add_cylinder_between(mesh, (0.28, 0.50, -0.18), (0.28, 0.98, -0.10), 0.045, "MAT_ENEMY_CopperPipe", "right_vertical_mount_pipe", 8, True)
    return mesh


def build_part_cog():
    mesh = Mesh("PART_CogShoulderJoint_Blockout")
    add_gear(mesh, (0.0, 0.0, 0.0), 0.18, 0.32, 0.40, 0.16, "x", 12, "MAT_ENEMY_AgedBrass", "cog_shoulder_joint")
    add_cylinder(mesh, (0.0, 0.0, 0.0), 0.16, 0.24, "x", "MAT_ENEMY_BlackenedIron", "iron_axle_socket", 14, True)
    return mesh


def build_part_piston_leg():
    mesh = Mesh("PART_PistonLeg_Blockout")
    add_piston_leg(mesh, (0.0, 0.78, 0.0), (0.0, 0.10, 0.05), "sample_piston_leg", 1.0)
    add_box(mesh, (0.0, 0.86, -0.02), (0.22, 0.14, 0.18), "MAT_ENEMY_AgedBrass", "hip_mount_block")
    return mesh


def build_part_mask():
    mesh = Mesh("PART_BrassMaskVisor_Blockout")
    add_mask(mesh, (0.0, 0.35, 0.0), (1.0, 1.0, 1.0), "standalone_mask")
    add_box(mesh, (0.0, 0.15, -0.04), (0.34, 0.12, 0.12), "MAT_ENEMY_AgedBrass", "lower_jaw_plate")
    return mesh


def build_part_core():
    mesh = Mesh("PART_FurnaceCore_Blockout")
    add_furnace_core(mesh, (0.0, 0.35, 0.0), 0.22, "standalone_furnace_core")
    add_cylinder(mesh, (0.0, 0.35, -0.08), 0.30, 0.08, "z", "MAT_ENEMY_AgedBrass", "rear_mount_flange", 18, True)
    return mesh


def build_part_backpack():
    mesh = Mesh("PART_PipeBackpack_Blockout")
    add_pipe_backpack(mesh, (0.0, 0.55, 0.0), (1.15, 1.15, 1.15), "standalone_pipe_backpack")
    add_box(mesh, (0.0, 0.55, -0.13), (0.42, 0.70, 0.08), "MAT_ENEMY_BlackenedIron", "spine_mount_plate")
    return mesh


ASSET_BUILDERS = {
    "ENEMY_ScrapperAutomaton_Blockout_LOD0": build_scrapper,
    "ENEMY_LancerAutomaton_Blockout_LOD0": build_lancer,
    "ENEMY_SentinelTurret_Blockout_LOD0": build_turret,
    "PART_CogShoulderJoint_Blockout": build_part_cog,
    "PART_PistonLeg_Blockout": build_part_piston_leg,
    "PART_BrassMaskVisor_Blockout": build_part_mask,
    "PART_FurnaceCore_Blockout": build_part_core,
    "PART_PipeBackpack_Blockout": build_part_backpack,
}


ASSET_NOTES = {
    "ENEMY_ScrapperAutomaton_Blockout_LOD0": {
        "role": "Compact melee worker-machine with hunched boiler torso, oversized claw arms, and broad piston feet.",
        "pivot": "Origin at ground footprint center. Keep +Y up, +Z forward; place melee hit sockets at claw palms.",
        "collider": "Primary capsule 0.85m diameter x 1.55m height, centered near Y 0.78. Add optional trigger boxes on claw arcs during attack frames.",
        "lod": "LOD1 can remove rivets, shoulder gear teeth, and backpack bends. LOD2 should collapse claws to simple mitts and replace cylinders with six-sided rods.",
        "rig": "Root, pelvis, boiler/spine, head visor, left/right cog shoulder, elbow, wrist/claw, hip, knee, ankle, backpack exhaust controls.",
        "animation": "Idle pressure bob, scuttle walk, claw windup, double swipe, stagger, shutdown with furnace core dim.",
    },
    "ENEMY_LancerAutomaton_Blockout_LOD0": {
        "role": "Tall ranged automaton with narrow body, stilt piston legs, and a pressure lance/cannon extending forward.",
        "pivot": "Origin at ground footprint center. Barrel points +Z; muzzle socket should be placed at lance tip.",
        "collider": "Primary capsule 0.70m diameter x 2.05m height. Use separate narrow raycast origin/muzzle transform for projectile or hitscan.",
        "lod": "LOD1 can reduce lance supports and backpack pipes. LOD2 keeps only torso, head, legs, and one barrel cylinder for combat readability.",
        "rig": "Root, hips, torso, neck/head, shoulder braces, lance aim/recoil bone, hip/knee/ankle pistons, backpack hose controls.",
        "animation": "Idle aiming sway, brace legs, charge pressure pulse, fire recoil, vent steam, stagger, collapse.",
    },
    "ENEMY_SentinelTurret_Blockout_LOD0": {
        "role": "Static sentinel turret with floor, wall, and ceiling mounting cues in one universal blockout.",
        "pivot": "Origin at floor mount center for default placement. For wall/ceiling variants, duplicate mesh later and move pivot to mount plate center.",
        "collider": "Use a cylinder or box for base/housing and a short non-blocking barrel trigger. Avoid mesh collision.",
        "lod": "LOD1 removes gear teeth and side drums. LOD2 is a box housing plus single barrel; static batching is recommended.",
        "rig": "No full character rig required. Use separate yaw base and pitch barrel transforms; optional recoil child for muzzle.",
        "animation": "Search sweep, target acquire snap, fire recoil, overheat vent, destroyed droop.",
    },
    "PART_CogShoulderJoint_Blockout": {
        "role": "Reusable cog shoulder or hip joint with readable gear teeth and rivet rhythm.",
        "pivot": "Origin centered on axle. X axis runs through socket for shoulder placement.",
        "collider": "No gameplay collider; visual only unless used as a weak-point hit target.",
        "lod": "Bake gear teeth to normal map or collapse to 10-sided cylinder for LOD2.",
        "rig": "Parent under upper-arm or shoulder bone; rotate around local X.",
        "animation": "Can counter-rotate subtly during idle/walk to sell clockwork motion.",
    },
    "PART_PistonLeg_Blockout": {
        "role": "Reusable piston leg assembly with hip mount, brass knee socket, telescoping lower rod, and broad metal foot.",
        "pivot": "Origin remains at floor center for preview. For production rigging, pivot hip block at upper socket.",
        "collider": "Character capsule handles collision; optional footstep contact box only for VFX/audio timing.",
        "lod": "Remove knee sphere rivets and simplify lower rod to box at distance.",
        "rig": "Hip, knee, ankle, and piston stretch helper bones.",
        "animation": "Use IK for planted foot and piston compression on step impact.",
    },
    "PART_BrassMaskVisor_Blockout": {
        "role": "Reusable enemy face plate with cream enamel mask, brass cheek hinges, snout filter, and dark visor slit.",
        "pivot": "Origin is under the mask for preview. Production pivot should sit at neck/head joint center.",
        "collider": "Head hitbox can be simple box matching mask silhouette.",
        "lod": "At LOD2, keep the visor slit as dark/emissive material strip and merge hinges into the mask body.",
        "rig": "Parent to head bone; optional jaw/filter bob during venting.",
        "animation": "Small visor pulse or filter shake during alert/firing states.",
    },
    "PART_FurnaceCore_Blockout": {
        "role": "Reusable glowing amber furnace core/weak-point insert with brass retaining ring.",
        "pivot": "Origin at core center. Forward-facing ring is +Z.",
        "collider": "Optional sphere trigger 0.45m diameter for weak-point tests.",
        "lod": "LOD1 can become flat emissive disc; LOD2 can bake glow into torso material.",
        "rig": "Usually parented to torso; scale pulse helper optional.",
        "animation": "Low-amplitude glow pulse, damage flare, shutdown dim.",
    },
    "PART_PipeBackpack_Blockout": {
        "role": "Reusable boiler backpack with tank, paired vertical pipes, exhaust bends, and mount collar.",
        "pivot": "Origin centered on tank. For production, pivot at spine mount plate.",
        "collider": "Visual only; use character capsule unless backpack is intended as targetable silhouette.",
        "lod": "LOD1 removes pipe bends; LOD2 merges tank and pipes into a single low-sided hull.",
        "rig": "Parent to spine/torso. Exhaust outlets need sockets for steam VFX.",
        "animation": "Idle vent puffs, overheat shake, burst steam on death.",
    },
}


def write_mtl(path):
    lines = [
        "# Worker D generated Brassworks Breach enemy blockout materials",
        "# Unity import note: OBJ/MTL colors are proxy materials for staging only.",
        "",
    ]
    for name, data in MATERIALS.items():
        lines.append(f"newmtl {name}")
        lines.append(f"# {data['label']}")
        lines.append("Ka {:.4f} {:.4f} {:.4f}".format(*data["ka"]))
        lines.append("Kd {:.4f} {:.4f} {:.4f}".format(*data["kd"]))
        lines.append("Ks {:.4f} {:.4f} {:.4f}".format(*data["ks"]))
        if "ke" in data:
            lines.append("Ke {:.4f} {:.4f} {:.4f}".format(*data["ke"]))
        lines.append(f"Ns {data['ns']}")
        lines.append("illum 2")
        lines.append("")
    path.write_text("\n".join(lines), encoding="utf-8")


def write_obj(mesh, path):
    lines = [
        f"# Worker D generated {mesh.name}",
        "# Brassworks Breach enemy production-staging blockout",
        "# Units: meters. Axis: +Y up, +Z forward. Apply transforms before production export.",
        f"mtllib {MTL_NAME}",
        f"o {mesh.name}",
        "",
    ]
    for vertex in mesh.vertices:
        lines.append("v {:.5f} {:.5f} {:.5f}".format(*vertex))
    lines.append("")
    current_group = None
    current_material = None
    for face in mesh.faces:
        if face.group != current_group:
            lines.append(f"g {face.group}")
            current_group = face.group
        if face.material != current_material:
            lines.append(f"usemtl {face.material}")
            current_material = face.material
        lines.append("f " + " ".join(str(i) for i in face.indices))
    lines.append("")
    path.write_text("\n".join(lines), encoding="utf-8")


def rotate_for_preview(point):
    yaw = math.radians(36)
    pitch = math.radians(-24)
    x, y, z = point
    x2 = x * math.cos(yaw) + z * math.sin(yaw)
    z2 = -x * math.sin(yaw) + z * math.cos(yaw)
    y2 = y * math.cos(pitch) - z2 * math.sin(pitch)
    z3 = y * math.sin(pitch) + z2 * math.cos(pitch)
    return (x2, y2, z3)


def face_normal(points):
    if len(points) < 3:
        return (0.0, 1.0, 0.0)
    a, b, c = points[0], points[1], points[2]
    return v_norm(v_cross(v_sub(b, a), v_sub(c, a)))


def shaded_color(material, points):
    base = PREVIEW_COLORS.get(material, (180, 180, 180))
    normal = face_normal([rotate_for_preview(p) for p in points])
    light = v_norm((-0.45, 0.85, 0.55))
    intensity = 0.54 + 0.42 * max(0.0, v_dot(normal, light))
    return tuple(max(0, min(255, int(c * intensity))) for c in base)


def render_preview(mesh, path, title, size=(640, 640)):
    width, height = size
    image = Image.new("RGB", size, (236, 232, 222))
    draw = ImageDraw.Draw(image)
    rotated = [rotate_for_preview(v) for v in mesh.vertices]
    min_x, max_x = min(p[0] for p in rotated), max(p[0] for p in rotated)
    min_y, max_y = min(p[1] for p in rotated), max(p[1] for p in rotated)
    content_w = max(0.1, max_x - min_x)
    content_h = max(0.1, max_y - min_y)
    scale = min((width - 100) / content_w, (height - 145) / content_h)
    cx = width / 2 - (min_x + max_x) * scale / 2
    cy = height / 2 + 20 + (min_y + max_y) * scale / 2

    def project(rotated_point):
        return (rotated_point[0] * scale + cx, -rotated_point[1] * scale + cy)

    draw.rectangle((0, 0, width, 58), fill=(35, 36, 34))
    try:
        font_title = ImageFont.truetype("arial.ttf", 22)
        font_small = ImageFont.truetype("arial.ttf", 14)
    except OSError:
        font_title = ImageFont.load_default()
        font_small = ImageFont.load_default()
    draw.text((24, 16), title, fill=(235, 210, 154), font=font_title)

    sorted_faces = sorted(
        mesh.faces,
        key=lambda face: sum(rotated[i - 1][2] for i in face.indices) / len(face.indices),
    )
    for face in sorted_faces:
        original_points = [mesh.vertices[i - 1] for i in face.indices]
        poly = [project(rotated[i - 1]) for i in face.indices]
        draw.polygon(poly, fill=shaded_color(face.material, original_points))
        draw.line(poly + [poly[0]], fill=(34, 34, 32), width=1)

    dims = mesh.dimensions()
    footer = f"{dims[0]:.2f}m W x {dims[1]:.2f}m H x {dims[2]:.2f}m D | {mesh.triangle_count()} est. tris"
    draw.rectangle((0, height - 42, width, height), fill=(35, 36, 34))
    draw.text((24, height - 29), footer, fill=(230, 225, 208), font=font_small)
    image.save(path)


def make_contact_sheet(preview_paths, output_path):
    thumbs = []
    for path in preview_paths:
        img = Image.open(path).resize((320, 320), Image.Resampling.LANCZOS)
        thumbs.append((path, img))
    cols = 4
    rows = math.ceil(len(thumbs) / cols)
    sheet = Image.new("RGB", (cols * 320, rows * 320 + 56), (28, 29, 28))
    draw = ImageDraw.Draw(sheet)
    try:
        font_title = ImageFont.truetype("arial.ttf", 24)
    except OSError:
        font_title = ImageFont.load_default()
    draw.text((18, 16), "Brassworks Breach Enemy Blockouts - Worker D Contact Sheet", fill=(236, 205, 136), font=font_title)
    for index, (_, img) in enumerate(thumbs):
        x = (index % cols) * 320
        y = 56 + (index // cols) * 320
        sheet.paste(img, (x, y))
    sheet.save(output_path)


def relative(path):
    return path.relative_to(ROOT).as_posix()


def write_manifest(meshes, obj_paths, preview_paths, timestamp):
    json_data = {
        "generated_by": "Worker D",
        "timestamp": timestamp,
        "unity_units": "1 Unity unit = 1 meter",
        "axis": "+Y up, +Z forward",
        "material_file": relative(ASSET_DIR / MTL_NAME),
        "assets": [],
    }
    for mesh, obj_path, preview_path in zip(meshes, obj_paths, preview_paths):
        note = ASSET_NOTES[mesh.name]
        json_data["assets"].append(
            {
                "name": mesh.name,
                "obj": relative(obj_path),
                "preview": relative(preview_path),
                "dimensions_m": mesh.dimensions(),
                "vertices": len(mesh.vertices),
                "estimated_triangles": mesh.triangle_count(),
                **note,
            }
        )
    (DOC_DIR / "ENEMY_ASSET_MANIFEST.json").write_text(json.dumps(json_data, indent=2), encoding="utf-8")

    lines = [
        "# Enemy Asset Manifest",
        "",
        f"Timestamp: {timestamp}",
        "",
        "Worker: D",
        "",
        "Scope: first production-staged enemy blockout OBJs for Brassworks Breach. These are Unity-friendly mesh files for silhouette, scale, rig planning, collider planning, and material direction. They are not final skinned meshes.",
        "",
        "## Unity Import Contract",
        "",
        "- Units: `1 Unity unit = 1 meter`.",
        "- Axis: `+Y` up, `+Z` forward.",
        "- OBJ material source: `Assets/_Project/ArtStaging/Enemies/ENEMY_BlockoutMaterials.mtl`.",
        "- Recommended import settings: import materials from MTL for preview, calculate normals if needed, disable mesh colliders by default, and keep transforms at identity after import.",
        "- Performance intent: readable low/mid PC silhouettes with low material count, simple cylinders, and detachable shared parts for later atlas/material consolidation.",
        "",
        "## Asset Index",
        "",
        "| Asset | Mesh | Preview | Size (m) | Est. Tris | Role |",
        "| --- | --- | --- | --- | ---: | --- |",
    ]
    for mesh, obj_path, preview_path in zip(meshes, obj_paths, preview_paths):
        dims = mesh.dimensions()
        note = ASSET_NOTES[mesh.name]
        lines.append(
            f"| `{mesh.name}` | `{relative(obj_path)}` | `{relative(preview_path)}` | "
            f"{dims[0]:.2f} x {dims[1]:.2f} x {dims[2]:.2f} | {mesh.triangle_count()} | {note['role']} |"
        )
    lines.extend(
        [
            "",
            "## Scale, Pivot, Collider, LOD, Rig, And Animation Notes",
            "",
        ]
    )
    for mesh in meshes:
        note = ASSET_NOTES[mesh.name]
        lines.extend(
            [
                f"### {mesh.name}",
                "",
                f"- Role: {note['role']}",
                f"- Pivot/origin: {note['pivot']}",
                f"- Collider suggestion: {note['collider']}",
                f"- LOD notes: {note['lod']}",
                f"- Rig/skeleton requirements: {note['rig']}",
                f"- Animation requirements: {note['animation']}",
                "",
            ]
        )
    lines.extend(
        [
            "## Shared Material Proxy Notes",
            "",
            "- `MAT_ENEMY_AgedBrass`: warm brass for readable steampunk highlights and joints.",
            "- `MAT_ENEMY_BlackenedIron`: soot-dark chassis and armor mass.",
            "- `MAT_ENEMY_CopperPipe`: pipes, pressure rods, lance barrel accents.",
            "- `MAT_ENEMY_DarkRubber`: hoses/gaskets.",
            "- `MAT_ENEMY_CreamEnamel`: mask plates; later supports chipped enamel texture detail.",
            "- `MAT_ENEMY_FurnaceGlowAmber`: emissive furnace eyes/cores; keep bloom controlled so silhouettes remain readable.",
            "- `MAT_ENEMY_SootShadow`: visor slit and deep interior cavities.",
            "",
            "## Contact Sheet",
            "",
            f"- `{relative(DOC_DIR / 'PREVIEW_EnemyBlockoutContactSheet.png')}`",
            "",
            "## Production Follow-Up",
            "",
            "- Convert accepted blockouts to skinned FBX for Scrapper and Lancer after gameplay scale approval.",
            "- Split Sentinel into floor, wall, and ceiling prefab variants once placement rules are final.",
            "- Bake high-poly bevel/rivet detail into atlas materials for LOD1/LOD2 instead of carrying every small shape as geometry.",
        ]
    )
    (DOC_DIR / "ENEMY_ASSET_MANIFEST.md").write_text("\n".join(lines), encoding="utf-8")


def append_work_log(meshes, timestamp):
    log_path = DOC_DIR / "ENEMY_ASSET_WORK_LOG.md"
    if not log_path.exists():
        log_path.write_text("# Enemy Asset Work Log\n\n", encoding="utf-8")
    lines = [
        f"## {timestamp} - Worker D",
        "",
        "- Created first production-staged enemy blockout package in owned enemy staging paths.",
        "- Generated OBJ/MTL assets for Scrapper Automaton, Lancer Automaton, Sentinel Turret, and five reusable enemy parts.",
        "- Generated individual preview renders, a contact sheet, and markdown/JSON manifests with scale, pivot, collider, LOD, rig, and animation notes.",
        "- Did not modify scenes, gameplay scripts, README, root docs, prefabs, or existing materials.",
        "",
        "Generated meshes:",
    ]
    for mesh in meshes:
        dims = mesh.dimensions()
        lines.append(f"- `{mesh.name}`: {dims[0]:.2f}m W x {dims[1]:.2f}m H x {dims[2]:.2f}m D, {mesh.triangle_count()} est. tris.")
    lines.append("")
    with log_path.open("a", encoding="utf-8") as handle:
        handle.write("\n".join(lines))


def main():
    ASSET_DIR.mkdir(parents=True, exist_ok=True)
    DOC_DIR.mkdir(parents=True, exist_ok=True)
    timestamp = datetime.now().astimezone().strftime("%Y-%m-%d %H:%M %z")
    timestamp = f"{timestamp[:-2]}:{timestamp[-2:]}"

    write_mtl(ASSET_DIR / MTL_NAME)
    meshes = [builder() for builder in ASSET_BUILDERS.values()]
    obj_paths = []
    preview_paths = []
    for mesh in meshes:
        obj_path = ASSET_DIR / f"{mesh.name}.obj"
        preview_path = DOC_DIR / f"PREVIEW_{mesh.name}.png"
        write_obj(mesh, obj_path)
        render_preview(mesh, preview_path, mesh.name)
        obj_paths.append(obj_path)
        preview_paths.append(preview_path)
    make_contact_sheet(preview_paths, DOC_DIR / "PREVIEW_EnemyBlockoutContactSheet.png")
    write_manifest(meshes, obj_paths, preview_paths, timestamp)
    append_work_log(meshes, timestamp)
    print(f"Generated {len(meshes)} meshes, {len(preview_paths)} previews, manifest, and work log.")
    for path in obj_paths + preview_paths:
        print(relative(path))
    print(relative(DOC_DIR / "PREVIEW_EnemyBlockoutContactSheet.png"))
    print(relative(DOC_DIR / "ENEMY_ASSET_MANIFEST.md"))
    print(relative(DOC_DIR / "ENEMY_ASSET_MANIFEST.json"))
    print(relative(DOC_DIR / "ENEMY_ASSET_WORK_LOG.md"))


if __name__ == "__main__":
    main()
