from __future__ import annotations

import json
import math
from dataclasses import dataclass, field
from datetime import datetime
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


ART_FOLDER = Path("Assets/_Project/ArtStaging/WeaponsProps")
DOC_FOLDER = Path("Documentation/AssetProduction/WeaponsProps")
SHARED_MTL_NAME = "WPN_PROP_PICKUP_WeaponsProps_BlockoutMaterials.mtl"


MATERIALS = {
    "MAT_Brass_Worn": {
        "kd": (0.72, 0.50, 0.18),
        "ks": (0.55, 0.43, 0.20),
        "ns": 84,
        "note": "Worn aged brass, intended metallic 1.0 roughness 0.42.",
    },
    "MAT_Brass_PolishedEdge": {
        "kd": (0.92, 0.70, 0.30),
        "ks": (0.80, 0.62, 0.28),
        "ns": 110,
        "note": "Bright brass edge/collar material, intended metallic 1.0 roughness 0.30.",
    },
    "MAT_BlackenedIron": {
        "kd": (0.055, 0.052, 0.046),
        "ks": (0.18, 0.17, 0.15),
        "ns": 38,
        "note": "Blackened iron and soot-dark frame, intended metallic 0.85 roughness 0.60.",
    },
    "MAT_HeatStainedSteel": {
        "kd": (0.25, 0.22, 0.19),
        "ks": (0.34, 0.31, 0.28),
        "ns": 52,
        "note": "Heat-stained steel for barrels and slug bodies.",
    },
    "MAT_CopperPipe_Aged": {
        "kd": (0.74, 0.34, 0.16),
        "ks": (0.56, 0.28, 0.13),
        "ns": 76,
        "note": "Aged copper pipe/tube material.",
    },
    "MAT_WalnutLeather_Dark": {
        "kd": (0.23, 0.12, 0.055),
        "ks": (0.13, 0.08, 0.04),
        "ns": 28,
        "note": "Dark walnut or leather grip material.",
    },
    "MAT_GaugeFace_Cream": {
        "kd": (0.92, 0.83, 0.62),
        "ks": (0.08, 0.07, 0.05),
        "ns": 18,
        "note": "Cream enamel gauge or label face.",
    },
    "MAT_Glass_PressureAmber": {
        "kd": (0.90, 0.56, 0.14),
        "ks": (0.95, 0.74, 0.35),
        "ns": 140,
        "note": "Amber pressure glass; author translucent/emissive variant in Unity if desired.",
    },
    "MAT_RedValve_Warning": {
        "kd": (0.68, 0.08, 0.035),
        "ks": (0.20, 0.07, 0.04),
        "ns": 32,
        "note": "Red valve/warning accent.",
    },
    "MAT_SocketMarker_Magenta": {
        "kd": (0.95, 0.05, 0.78),
        "ks": (0.15, 0.10, 0.12),
        "ns": 12,
        "note": "Temporary visible socket locator geometry. Hide/delete after anchors are recreated.",
    },
}


@dataclass
class Face:
    indices: tuple[int, int, int]
    material: str
    group: str


@dataclass
class MeshAsset:
    name: str
    title: str
    category: str
    dimensions_m: str
    pivot: str
    import_notes: list[str]
    animation_notes: list[str]
    collision_notes: list[str]
    sockets: list[dict[str, object]]
    lod_notes: list[str]
    vertices: list[tuple[float, float, float]] = field(default_factory=list)
    faces: list[Face] = field(default_factory=list)

    def add_vertex(self, point: tuple[float, float, float]) -> int:
        self.vertices.append(point)
        return len(self.vertices)

    def add_triangle(self, a: int, b: int, c: int, material: str, group: str) -> None:
        self.faces.append(Face((a, b, c), material, group))

    def add_quad(self, a: int, b: int, c: int, d: int, material: str, group: str) -> None:
        self.add_triangle(a, b, c, material, group)
        self.add_triangle(a, c, d, material, group)

    @property
    def bounds(self) -> dict[str, list[float]]:
        if not self.vertices:
            return {"min": [0, 0, 0], "max": [0, 0, 0], "size": [0, 0, 0]}
        mins = [min(v[i] for v in self.vertices) for i in range(3)]
        maxs = [max(v[i] for v in self.vertices) for i in range(3)]
        sizes = [maxs[i] - mins[i] for i in range(3)]
        return {
            "min": [round(v, 4) for v in mins],
            "max": [round(v, 4) for v in maxs],
            "size": [round(v, 4) for v in sizes],
        }

    @property
    def materials(self) -> list[str]:
        return sorted({face.material for face in self.faces})

    def obj_path(self) -> str:
        return f"{ART_FOLDER.as_posix()}/{self.name}.obj"

    def preview_path(self) -> str:
        return f"{DOC_FOLDER.as_posix()}/PREVIEW_{self.name}.png"


def add(a, b):
    return (a[0] + b[0], a[1] + b[1], a[2] + b[2])


def sub(a, b):
    return (a[0] - b[0], a[1] - b[1], a[2] - b[2])


def mul(a, scalar):
    return (a[0] * scalar, a[1] * scalar, a[2] * scalar)


def dot(a, b):
    return a[0] * b[0] + a[1] * b[1] + a[2] * b[2]


def cross(a, b):
    return (
        a[1] * b[2] - a[2] * b[1],
        a[2] * b[0] - a[0] * b[2],
        a[0] * b[1] - a[1] * b[0],
    )


def length(a):
    return math.sqrt(dot(a, a))


def normalize(a):
    mag = length(a)
    if mag <= 1e-8:
        return (0.0, 1.0, 0.0)
    return (a[0] / mag, a[1] / mag, a[2] / mag)


def rotate_point(point, rot):
    x, y, z = point
    rx, ry, rz = rot
    cx, sx = math.cos(rx), math.sin(rx)
    cy, sy = math.cos(ry), math.sin(ry)
    cz, sz = math.cos(rz), math.sin(rz)

    y, z = y * cx - z * sx, y * sx + z * cx
    x, z = x * cy + z * sy, -x * sy + z * cy
    x, y = x * cz - y * sz, x * sz + y * cz
    return (x, y, z)


def basis_from_axis(axis):
    w = normalize(axis)
    helper = (0.0, 1.0, 0.0)
    if abs(dot(w, helper)) > 0.94:
        helper = (1.0, 0.0, 0.0)
    u = normalize(cross(helper, w))
    v = normalize(cross(w, u))
    return u, v, w


def add_box(asset, group, center, size, material, rot=(0.0, 0.0, 0.0)):
    hx, hy, hz = size[0] / 2.0, size[1] / 2.0, size[2] / 2.0
    local = [
        (-hx, -hy, -hz),
        (hx, -hy, -hz),
        (hx, hy, -hz),
        (-hx, hy, -hz),
        (-hx, -hy, hz),
        (hx, -hy, hz),
        (hx, hy, hz),
        (-hx, hy, hz),
    ]
    idx = [asset.add_vertex(add(center, rotate_point(p, rot))) for p in local]
    quads = [
        (idx[0], idx[4], idx[5], idx[1]),
        (idx[3], idx[2], idx[6], idx[7]),
        (idx[4], idx[7], idx[6], idx[5]),
        (idx[0], idx[1], idx[2], idx[3]),
        (idx[1], idx[5], idx[6], idx[2]),
        (idx[0], idx[3], idx[7], idx[4]),
    ]
    for quad in quads:
        asset.add_quad(*quad, material, group)


def add_cylinder(asset, group, p1, p2, radius, material, segments=18, cap=True, radius2=None):
    r2 = radius if radius2 is None else radius2
    u, v, _w = basis_from_axis(sub(p2, p1))
    ring1 = []
    ring2 = []
    for i in range(segments):
        t = 2.0 * math.pi * i / segments
        radial = add(mul(u, math.cos(t)), mul(v, math.sin(t)))
        ring1.append(asset.add_vertex(add(p1, mul(radial, radius))))
        ring2.append(asset.add_vertex(add(p2, mul(radial, r2))))
    for i in range(segments):
        j = (i + 1) % segments
        asset.add_quad(ring1[i], ring1[j], ring2[j], ring2[i], material, group)
    if cap:
        c1 = asset.add_vertex(p1)
        c2 = asset.add_vertex(p2)
        for i in range(segments):
            j = (i + 1) % segments
            asset.add_triangle(c1, ring1[j], ring1[i], material, group)
            asset.add_triangle(c2, ring2[i], ring2[j], material, group)


def add_torus(asset, group, center, axis, major_radius, minor_radius, material, major_segments=28, minor_segments=8):
    u, v, w = basis_from_axis(axis)
    rows = []
    for i in range(major_segments):
        theta = 2.0 * math.pi * i / major_segments
        radial = add(mul(u, math.cos(theta)), mul(v, math.sin(theta)))
        tube_center = add(center, mul(radial, major_radius))
        row = []
        for j in range(minor_segments):
            phi = 2.0 * math.pi * j / minor_segments
            point = add(tube_center, add(mul(radial, minor_radius * math.cos(phi)), mul(w, minor_radius * math.sin(phi))))
            row.append(asset.add_vertex(point))
        rows.append(row)
    for i in range(major_segments):
        ni = (i + 1) % major_segments
        for j in range(minor_segments):
            nj = (j + 1) % minor_segments
            asset.add_quad(rows[i][j], rows[ni][j], rows[ni][nj], rows[i][nj], material, group)


def add_spoked_wheel(asset, group, center, axis, radius, tube_radius, material, spoke_count=6):
    add_torus(asset, f"{group}_rim", center, axis, radius, tube_radius, material)
    add_cylinder(asset, f"{group}_hub", add(center, mul(normalize(axis), -tube_radius * 2.2)), add(center, mul(normalize(axis), tube_radius * 2.2)), radius * 0.23, material, 16)
    u, v, _w = basis_from_axis(axis)
    for i in range(spoke_count):
        theta = 2.0 * math.pi * i / spoke_count
        target = add(center, mul(add(mul(u, math.cos(theta)), mul(v, math.sin(theta))), radius * 0.82))
        add_cylinder(asset, f"{group}_spoke_{i + 1:02d}", center, target, tube_radius * 0.55, material, 8)


def add_socket(asset, name, pos, scale=0.038):
    add_box(asset, name, pos, (scale * 1.35, scale * 0.26, scale * 0.26), "MAT_SocketMarker_Magenta")
    add_box(asset, name, pos, (scale * 0.26, scale * 1.35, scale * 0.26), "MAT_SocketMarker_Magenta")
    add_box(asset, name, pos, (scale * 0.26, scale * 0.26, scale * 1.35), "MAT_SocketMarker_Magenta")


def add_rivets(asset, positions, axis, radius=0.012):
    axis_vec = {"x": (1, 0, 0), "y": (0, 1, 0), "z": (0, 0, 1)}[axis]
    for i, pos in enumerate(positions):
        add_cylinder(
            asset,
            f"RIVET_{i + 1:02d}",
            add(pos, mul(axis_vec, -0.006)),
            add(pos, mul(axis_vec, 0.006)),
            radius,
            "MAT_Brass_PolishedEdge",
            10,
        )


def build_pressure_pistol():
    sockets = [
        {"name": "SOCKET_Muzzle", "position_m": [0.0, 0.205, 0.77], "purpose": "Muzzle flash, projectile origin, primary and burst VFX."},
        {"name": "SOCKET_Grip_Dominant", "position_m": [0.0, -0.03, 0.16], "purpose": "Right-hand grip pose; one-hand VR dominant grip."},
        {"name": "SOCKET_PressureChamber", "position_m": [0.0, 0.095, 0.43], "purpose": "Underbarrel chamber highlight and alternate fire pressure dump."},
        {"name": "SOCKET_Valve", "position_m": [0.125, 0.155, 0.42], "purpose": "Side valve twist for reload/check and empty click."},
        {"name": "SOCKET_Vent_Right", "position_m": [0.095, 0.205, 0.31], "purpose": "Steam vent puff during alternate pressure burst."},
        {"name": "SOCKET_Trigger", "position_m": [0.0, 0.07, 0.25], "purpose": "Trigger motion anchor."},
    ]
    asset = MeshAsset(
        "WPN_PressurePistol_Viewmodel_Blockout",
        "Pressure Pistol Hero Viewmodel Blockout",
        "Weapon / first-person viewmodel",
        "Approx 0.22 m W x 0.33 m H x 0.82 m L including socket markers; readable compact sidearm silhouette.",
        "Origin near dominant hand grip center; +Z points down barrel, +Y up.",
        [
            "Import OBJ at scale factor 1.0; 1 Unity unit equals 1 meter.",
            "Use mesh as a viewmodel LOD0 blockout/proportion target, not gameplay collision.",
            "Material slots intentionally separate brass, blackened iron, walnut/leather, copper, gauge face, amber glass, and socket markers.",
        ],
        [
            "Receiver and barrel recoil as one assembly: kick back -Z 0.035 m and pitch up 4-6 degrees.",
            "Underbarrel pressure chamber pulses/glows for alternate fire; vent from right side socket.",
            "Side valve rotates around local X for check/reload and empty click.",
            "Gauge needle can tremble during idle and snap upward during pressure burst.",
            "Trigger rotates slightly backward; grip remains stable for VR one-hand pose.",
        ],
        [
            "Use no mesh collider for viewmodel.",
            "Future world pickup should use a simple box/capsule proxy around receiver and grip.",
        ],
        sockets,
        [
            "LOD0 final target can keep distinct gauge/valve/tube elements.",
            "LOD1 should collapse rivets and socket marker geometry.",
            "LOD2/mobile can remove side valve spokes and copper pipe loops.",
        ],
    )
    add_box(asset, "BODY_BrassReceiver", (0.0, 0.155, 0.35), (0.17, 0.12, 0.25), "MAT_Brass_Worn")
    add_box(asset, "BODY_BlackenedRearBlock", (0.0, 0.16, 0.205), (0.15, 0.105, 0.08), "MAT_BlackenedIron")
    add_box(asset, "BODY_TopSightRib", (0.0, 0.232, 0.40), (0.045, 0.025, 0.32), "MAT_BlackenedIron")
    add_cylinder(asset, "BARREL_IronBore", (0.0, 0.205, 0.30), (0.0, 0.205, 0.735), 0.027, "MAT_BlackenedIron", 18)
    add_cylinder(asset, "BARREL_BrassMuzzleCrown", (0.0, 0.205, 0.705), (0.0, 0.205, 0.765), 0.046, "MAT_Brass_PolishedEdge", 20)
    add_cylinder(asset, "BARREL_DarkMuzzleHole", (0.0, 0.205, 0.762), (0.0, 0.205, 0.782), 0.021, "MAT_BlackenedIron", 18)
    add_cylinder(asset, "CHAMBER_UnderbarrelPressureCylinder", (0.0, 0.095, 0.22), (0.0, 0.095, 0.66), 0.046, "MAT_Brass_Worn", 20)
    for idx, z in enumerate([0.245, 0.50, 0.635]):
        add_cylinder(asset, f"CHAMBER_Band_{idx + 1:02d}", (0.0, 0.095, z - 0.012), (0.0, 0.095, z + 0.012), 0.052, "MAT_BlackenedIron", 20)
    add_box(asset, "GRIP_WalnutAngledGrip", (0.0, -0.005, 0.18), (0.105, 0.245, 0.075), "MAT_WalnutLeather_Dark", (math.radians(-18), 0, 0))
    add_box(asset, "GRIP_BrassButtPlate", (0.0, -0.118, 0.142), (0.13, 0.035, 0.09), "MAT_Brass_PolishedEdge", (math.radians(-18), 0, 0))
    add_cylinder(asset, "TRIGGER_GuardFront", (-0.052, 0.07, 0.265), (-0.052, 0.035, 0.185), 0.007, "MAT_BlackenedIron", 8)
    add_cylinder(asset, "TRIGGER_GuardRear", (0.052, 0.07, 0.265), (0.052, 0.035, 0.185), 0.007, "MAT_BlackenedIron", 8)
    add_cylinder(asset, "TRIGGER_GuardBottom", (-0.052, 0.035, 0.185), (0.052, 0.035, 0.185), 0.007, "MAT_BlackenedIron", 8)
    add_box(asset, "TRIGGER_SmallLever", (0.0, 0.065, 0.245), (0.026, 0.065, 0.012), "MAT_BlackenedIron", (math.radians(-14), 0, 0))
    add_cylinder(asset, "GAUGE_LeftCreamFace", (-0.105, 0.203, 0.322), (-0.132, 0.203, 0.322), 0.048, "MAT_GaugeFace_Cream", 20)
    add_torus(asset, "GAUGE_LeftBrassRim", (-0.134, 0.203, 0.322), (1, 0, 0), 0.049, 0.0055, "MAT_Brass_PolishedEdge")
    add_box(asset, "GAUGE_Needle", (-0.139, 0.212, 0.333), (0.004, 0.008, 0.052), "MAT_RedValve_Warning", (0, math.radians(0), math.radians(-34)))
    add_spoked_wheel(asset, "VALVE_RightWheel", (0.124, 0.155, 0.42), (1, 0, 0), 0.033, 0.0045, "MAT_RedValve_Warning", 5)
    add_cylinder(asset, "VALVE_RightStem", (0.078, 0.155, 0.42), (0.136, 0.155, 0.42), 0.012, "MAT_CopperPipe_Aged", 12)
    pipe_points = [(0.052, 0.096, 0.29), (0.102, 0.118, 0.305), (0.108, 0.177, 0.36), (0.058, 0.188, 0.405)]
    for i in range(len(pipe_points) - 1):
        add_cylinder(asset, f"PIPE_CopperLoop_{i + 1:02d}", pipe_points[i], pipe_points[i + 1], 0.0085, "MAT_CopperPipe_Aged", 10)
    add_cylinder(asset, "AMBER_PressureGlassTube", (-0.064, 0.102, 0.32), (-0.064, 0.102, 0.54), 0.011, "MAT_Glass_PressureAmber", 12)
    add_rivets(
        asset,
        [
            (-0.088, 0.203, 0.265),
            (-0.088, 0.203, 0.475),
            (0.088, 0.203, 0.265),
            (0.088, 0.203, 0.475),
            (-0.087, 0.107, 0.265),
            (0.087, 0.107, 0.475),
        ],
        "x",
        0.009,
    )
    for socket in sockets:
        add_socket(asset, socket["name"], tuple(socket["position_m"]))
    return asset


def build_scattergun():
    sockets = [
        {"name": "SOCKET_Muzzle", "position_m": [0.0, 0.235, 1.055], "purpose": "Center of triple-barrel muzzle flash; cone/slug VFX should branch per barrel."},
        {"name": "SOCKET_Grip_Dominant", "position_m": [0.0, 0.02, 0.26], "purpose": "Right-hand grip/trigger pose."},
        {"name": "SOCKET_Grip_Support", "position_m": [0.0, 0.055, 0.66], "purpose": "Support hand/pump pose; optional VR second hand."},
        {"name": "SOCKET_Pump", "position_m": [0.0, 0.06, 0.67], "purpose": "Pump group slides along Z after primary fire."},
        {"name": "SOCKET_PressureChamber", "position_m": [0.0, 0.105, 0.38], "purpose": "Rear pressure chamber lock/slug charge."},
        {"name": "SOCKET_Valve", "position_m": [0.17, 0.19, 0.31], "purpose": "Rotating cap/valve for slug chamber lock."},
        {"name": "SOCKET_Vent_Left", "position_m": [-0.15, 0.22, 0.46], "purpose": "Broad steam vent after blast."},
    ]
    asset = MeshAsset(
        "WPN_SteamScattergun_Viewmodel_Blockout",
        "Steam Scattergun Hero Viewmodel Blockout",
        "Weapon / first-person viewmodel",
        "Approx 0.38 m W x 0.42 m H x 1.12 m L including socket markers; broad triple-barrel breacher silhouette.",
        "Origin near rear dominant grip; +Z points down barrels, +Y up.",
        [
            "Import OBJ at scale factor 1.0.",
            "Viewmodel is intentionally oversized and asymmetric for first-person readability.",
            "Triple barrels, pump, shell rack, pressure chamber, valve, and socket markers are distinct OBJ groups.",
        ],
        [
            "Whole weapon bucks back -Z 0.055 m with 7-9 degree pitch on primary.",
            "MOVING_PumpGrip slides -Z 0.08 m then returns after primary blast.",
            "Right valve wheel rotates around local X before alternate slug fire.",
            "Rear pressure chamber coils can vibrate at idle and brighten during slug charge.",
            "Side shell rack can index or shake as a non-critical secondary motion.",
        ],
        [
            "No mesh collider for viewmodel.",
            "Future pickup/world version can use a compound box collider: receiver, barrel cluster, pump grip.",
        ],
        sockets,
        [
            "LOD0 final target should preserve the triple muzzle cluster and side shell rack.",
            "LOD1 can merge barrel bands and simplify valve spokes.",
            "LOD2/mobile can remove coil rings and shell rack cylinders while preserving broad silhouette.",
        ],
    )
    add_box(asset, "BODY_BlackenedRearReceiver", (0.0, 0.19, 0.33), (0.25, 0.17, 0.25), "MAT_BlackenedIron")
    add_box(asset, "BODY_BrassSidePlates", (0.0, 0.19, 0.36), (0.29, 0.12, 0.18), "MAT_Brass_Worn")
    add_box(asset, "BODY_BrassTopRib", (0.0, 0.305, 0.72), (0.052, 0.027, 0.62), "MAT_Brass_PolishedEdge")
    barrel_positions = [(-0.055, 0.218), (0.055, 0.218), (0.0, 0.268)]
    for idx, (x, y) in enumerate(barrel_positions):
        add_cylinder(asset, f"BARREL_TripleCluster_{idx + 1:02d}", (x, y, 0.40), (x, y, 1.025), 0.032, "MAT_HeatStainedSteel", 18)
        add_cylinder(asset, f"BARREL_BrassSawedMuzzle_{idx + 1:02d}", (x, y, 0.955), (x, y, 1.045), 0.042, "MAT_Brass_PolishedEdge", 18)
        add_cylinder(asset, f"BARREL_DarkMuzzleHole_{idx + 1:02d}", (x, y, 1.035), (x, y, 1.06), 0.022, "MAT_BlackenedIron", 18)
        for bidx, z in enumerate([0.50, 0.74]):
            add_cylinder(asset, f"BARREL_BlackBand_{idx + 1:02d}_{bidx + 1:02d}", (x, y, z - 0.012), (x, y, z + 0.012), 0.037, "MAT_BlackenedIron", 18)
    add_cylinder(asset, "CHAMBER_RearPressureTank", (0.0, 0.105, 0.19), (0.0, 0.105, 0.56), 0.073, "MAT_Brass_Worn", 22)
    for i, z in enumerate([0.235, 0.29, 0.345, 0.40, 0.455, 0.51]):
        add_torus(asset, f"CHAMBER_CoilRing_{i + 1:02d}", (0.0, 0.105, z), (0, 0, 1), 0.078, 0.005, "MAT_CopperPipe_Aged", 24, 6)
    add_box(asset, "MOVING_PumpGrip_Walnut", (0.0, 0.055, 0.665), (0.19, 0.085, 0.245), "MAT_WalnutLeather_Dark")
    for z in [0.57, 0.64, 0.71, 0.78]:
        add_box(asset, f"MOVING_PumpGrip_Rib_{int(z * 100):03d}", (0.0, 0.095, z), (0.205, 0.018, 0.018), "MAT_Brass_Worn")
    add_box(asset, "GRIP_DominantWalnutGrip", (0.0, 0.01, 0.245), (0.125, 0.265, 0.09), "MAT_WalnutLeather_Dark", (math.radians(-16), 0, 0))
    add_box(asset, "GRIP_BrassButtPlate", (0.0, -0.12, 0.20), (0.155, 0.038, 0.105), "MAT_Brass_PolishedEdge", (math.radians(-16), 0, 0))
    add_box(asset, "TRIGGER_GuardLarge", (0.0, 0.077, 0.285), (0.135, 0.022, 0.105), "MAT_BlackenedIron")
    add_box(asset, "TRIGGER_Lever", (0.0, 0.058, 0.285), (0.030, 0.075, 0.014), "MAT_BlackenedIron", (math.radians(-12), 0, 0))
    add_spoked_wheel(asset, "VALVE_RightSlugLockWheel", (0.168, 0.19, 0.31), (1, 0, 0), 0.045, 0.006, "MAT_RedValve_Warning", 6)
    add_cylinder(asset, "VALVE_RightStem", (0.105, 0.19, 0.31), (0.182, 0.19, 0.31), 0.014, "MAT_CopperPipe_Aged", 12)
    add_box(asset, "RACK_LeftSlugBracket", (-0.153, 0.18, 0.51), (0.025, 0.09, 0.27), "MAT_BlackenedIron")
    for i, z in enumerate([0.425, 0.51, 0.595]):
        add_cylinder(asset, f"RACK_BrassSlug_{i + 1:02d}", (-0.17, 0.18, z - 0.033), (-0.17, 0.18, z + 0.033), 0.020, "MAT_Brass_PolishedEdge", 12)
    tube_points = [(-0.08, 0.112, 0.39), (-0.135, 0.145, 0.46), (-0.118, 0.205, 0.59), (-0.055, 0.218, 0.68)]
    for i in range(len(tube_points) - 1):
        add_cylinder(asset, f"PIPE_LeftCopperRoute_{i + 1:02d}", tube_points[i], tube_points[i + 1], 0.010, "MAT_CopperPipe_Aged", 10)
    add_rivets(
        asset,
        [
            (-0.135, 0.245, 0.28),
            (0.135, 0.245, 0.28),
            (-0.135, 0.245, 0.43),
            (0.135, 0.245, 0.43),
            (-0.135, 0.135, 0.28),
            (0.135, 0.135, 0.43),
        ],
        "x",
        0.010,
    )
    for socket in sockets:
        add_socket(asset, socket["name"], tuple(socket["position_m"]), 0.042)
    return asset


def build_pressure_cell_pickup():
    sockets = [
        {"name": "SOCKET_PickupVFX", "position_m": [0.0, 0.39, 0.0], "purpose": "Amber pickup glint/steam puff origin."},
        {"name": "SOCKET_Valve", "position_m": [0.0, 0.35, 0.0], "purpose": "Tiny top valve spin or idle hiss."},
        {"name": "SOCKET_Collision", "position_m": [0.0, 0.19, 0.0], "purpose": "Suggested trigger capsule center, radius 0.16 m, height 0.42 m."},
    ]
    asset = MeshAsset(
        "PICKUP_PressureCell_Ammo_Blockout",
        "Pressure Pistol Ammo / Pressure Cell Pickup",
        "Pickup / ammo",
        "Approx 0.24 m W x 0.43 m H x 0.20 m D including socket markers; small upright pressure cell.",
        "Origin at base center for stable world placement.",
        [
            "Import OBJ at scale factor 1.0.",
            "Readable as pistol ammo at floor height; top valve and amber glass identify pressure contents.",
        ],
        [
            "Idle pickup can bob 0.03 m and rotate slowly around Y.",
            "Top valve can spin briefly on pickup.",
            "Amber tube can pulse with ammo highlight material.",
        ],
        [
            "Use a trigger capsule: radius 0.16 m, height 0.42 m.",
            "Optional small cylinder/box visual mesh only; do not use detailed mesh as collision.",
        ],
        sockets,
        [
            "No LOD needed for v0 if used sparingly.",
            "Mobile variant can remove rivets and top wheel spokes.",
        ],
    )
    add_cylinder(asset, "CELL_MainBrassCylinder", (0.0, 0.065, 0.0), (0.0, 0.305, 0.0), 0.068, "MAT_Brass_Worn", 20)
    add_cylinder(asset, "CELL_BlackBaseCap", (0.0, 0.02, 0.0), (0.0, 0.065, 0.0), 0.085, "MAT_BlackenedIron", 20)
    add_cylinder(asset, "CELL_BrassTopCap", (0.0, 0.305, 0.0), (0.0, 0.345, 0.0), 0.083, "MAT_Brass_PolishedEdge", 20)
    add_spoked_wheel(asset, "VALVE_TopHandWheel", (0.0, 0.37, 0.0), (0, 1, 0), 0.055, 0.005, "MAT_RedValve_Warning", 5)
    add_cylinder(asset, "GLASS_AmberPressureTube", (-0.076, 0.105, 0.0), (-0.076, 0.285, 0.0), 0.014, "MAT_Glass_PressureAmber", 12)
    add_cylinder(asset, "GAUGE_FrontCreamFace", (0.0, 0.19, 0.067), (0.0, 0.19, 0.089), 0.038, "MAT_GaugeFace_Cream", 18)
    add_torus(asset, "GAUGE_FrontBrassRim", (0.0, 0.19, 0.091), (0, 0, 1), 0.039, 0.0045, "MAT_Brass_PolishedEdge")
    add_box(asset, "LABEL_CreamAmmoStripe", (0.0, 0.115, 0.071), (0.09, 0.027, 0.014), "MAT_GaugeFace_Cream")
    add_box(asset, "LABEL_RedPressureStripe", (0.0, 0.145, 0.071), (0.09, 0.017, 0.015), "MAT_RedValve_Warning")
    add_rivets(asset, [(0.066, 0.07, 0.0), (-0.066, 0.07, 0.0), (0.066, 0.30, 0.0), (-0.066, 0.30, 0.0)], "y", 0.008)
    for socket in sockets:
        add_socket(asset, socket["name"], tuple(socket["position_m"]), 0.032)
    return asset


def build_slug_canister_pickup():
    sockets = [
        {"name": "SOCKET_PickupVFX", "position_m": [0.0, 0.36, 0.0], "purpose": "Pickup glint and slug ammo steam puff."},
        {"name": "SOCKET_LidHinge", "position_m": [-0.18, 0.24, -0.07], "purpose": "Optional lid flip/inspect animation."},
        {"name": "SOCKET_Collision", "position_m": [0.0, 0.16, 0.0], "purpose": "Suggested trigger box 0.46 m W x 0.38 m H x 0.32 m D."},
    ]
    asset = MeshAsset(
        "PICKUP_ScattergunSlugCanister_Blockout",
        "Scattergun Ammo / Slug Canister Pickup",
        "Pickup / ammo",
        "Approx 0.46 m W x 0.38 m H x 0.31 m D including socket markers; squat ammo canister with visible brass slugs.",
        "Origin at bottom center for floor placement.",
        [
            "Import OBJ at scale factor 1.0.",
            "Designed as a world pickup silhouette, not a viewmodel part.",
            "Slug tubes are intentionally separated as material-readable brass rounds.",
        ],
        [
            "Canister can rotate/bob as a pickup.",
            "Lid hinge can pop open for pickup feedback.",
            "Visible slug tubes can glow or flash on pickup.",
        ],
        [
            "Use a trigger box 0.46 m W x 0.38 m H x 0.32 m D.",
            "Optional simple rigidbody collider can be a box around rack base only if used as set dressing.",
        ],
        sockets,
        [
            "LOD0 keeps visible slug tubes.",
            "LOD1 can merge rack and remove handle bolts.",
            "LOD2/mobile can replace individual slugs with a single brass insert block.",
        ],
    )
    add_box(asset, "CANISTER_BlackenedRackBase", (0.0, 0.055, 0.0), (0.36, 0.11, 0.22), "MAT_BlackenedIron")
    add_cylinder(asset, "CANISTER_MainSlugDrum", (-0.155, 0.18, 0.0), (0.155, 0.18, 0.0), 0.087, "MAT_Brass_Worn", 22)
    add_cylinder(asset, "CANISTER_LeftIronCap", (-0.19, 0.18, 0.0), (-0.155, 0.18, 0.0), 0.093, "MAT_BlackenedIron", 22)
    add_cylinder(asset, "CANISTER_RightValveCap", (0.155, 0.18, 0.0), (0.205, 0.18, 0.0), 0.071, "MAT_Brass_PolishedEdge", 22)
    add_box(asset, "STRAP_DarkLeatherOverDrum", (0.0, 0.25, 0.0), (0.055, 0.045, 0.20), "MAT_WalnutLeather_Dark")
    add_cylinder(asset, "HANDLE_CopperCarryLoop_A", (-0.12, 0.27, -0.08), (-0.04, 0.325, -0.08), 0.010, "MAT_CopperPipe_Aged", 10)
    add_cylinder(asset, "HANDLE_CopperCarryLoop_B", (-0.04, 0.325, -0.08), (0.04, 0.325, -0.08), 0.010, "MAT_CopperPipe_Aged", 10)
    add_cylinder(asset, "HANDLE_CopperCarryLoop_C", (0.04, 0.325, -0.08), (0.12, 0.27, -0.08), 0.010, "MAT_CopperPipe_Aged", 10)
    for i, z in enumerate([-0.068, 0.0, 0.068]):
        add_cylinder(asset, f"SLUG_BrassVisibleRound_{i + 1:02d}", (-0.13, 0.28, z), (0.13, 0.28, z), 0.023, "MAT_Brass_PolishedEdge", 14)
        add_cylinder(asset, f"SLUG_HeatTip_{i + 1:02d}", (0.13, 0.28, z), (0.17, 0.28, z), 0.020, "MAT_HeatStainedSteel", 14, radius2=0.008)
    add_box(asset, "LABEL_CreamSlugPlate", (0.0, 0.105, 0.114), (0.22, 0.05, 0.012), "MAT_GaugeFace_Cream")
    add_box(asset, "LABEL_RedAmmoStripe", (0.0, 0.062, 0.115), (0.20, 0.025, 0.014), "MAT_RedValve_Warning")
    add_rivets(asset, [(-0.15, 0.105, 0.114), (0.15, 0.105, 0.114), (-0.15, 0.02, 0.114), (0.15, 0.02, 0.114)], "z", 0.007)
    for socket in sockets:
        add_socket(asset, socket["name"], tuple(socket["position_m"]), 0.034)
    return asset


def build_pressure_station_prop():
    sockets = [
        {"name": "SOCKET_Interact", "position_m": [0.0, 0.47, 0.20], "purpose": "Player interaction trace target."},
        {"name": "SOCKET_ValveWheel", "position_m": [0.0, 0.47, 0.13], "purpose": "Valve wheel rotation anchor."},
        {"name": "SOCKET_SteamVent", "position_m": [0.0, 1.05, 0.12], "purpose": "Steam puff/pressure release VFX."},
        {"name": "SOCKET_Hose", "position_m": [0.28, 0.28, 0.18], "purpose": "Future hose/nozzle attach point."},
        {"name": "SOCKET_WallMount", "position_m": [0.0, 0.56, 0.0], "purpose": "Backplate wall align point."},
    ]
    asset = MeshAsset(
        "PROP_WallPressureStation_Blockout",
        "Wall-Mounted Pressure Station Prop",
        "Prop / interactable station",
        "Approx 0.78 m W x 1.12 m H x 0.29 m D including socket markers; wall-mounted station with gauges and valve.",
        "Origin at bottom-center of wall contact plane; back face sits near Z 0.",
        [
            "Import OBJ at scale factor 1.0.",
            "Place against a wall with local +Z facing into the room.",
            "Use as a staged interactable prop; gameplay hook should target SOCKET_Interact later.",
        ],
        [
            "Large valve wheel rotates around local Z 100-160 degrees during use.",
            "Gauge needles can swing up/down during pressure station activation.",
            "Top vent emits short steam burst after interaction.",
            "Copper side pipes can pulse via material animation.",
        ],
        [
            "Use a simple box collider around the backplate, approx 0.74 m W x 1.08 m H x 0.10 m D.",
            "If interactive, add a trigger/interact volume protruding 0.35 m from the valve wheel.",
        ],
        sockets,
        [
            "LOD0 keeps gauges, valve spokes, cylinders, and pipe routes.",
            "LOD1 can flatten small rivets and simplify pipe elbows.",
            "LOD2/mobile can replace wheel spokes with a disk silhouette and remove needles.",
        ],
    )
    add_box(asset, "PANEL_BlackenedWallPlate", (0.0, 0.56, 0.025), (0.72, 1.06, 0.05), "MAT_BlackenedIron")
    add_box(asset, "PANEL_BrassInnerPlate", (0.0, 0.56, 0.058), (0.58, 0.88, 0.026), "MAT_Brass_Worn")
    for x in [-0.22, 0.22]:
        add_cylinder(asset, f"TANK_VerticalPressureCylinder_{'L' if x < 0 else 'R'}", (x, 0.22, 0.105), (x, 0.68, 0.105), 0.055, "MAT_Brass_Worn", 18)
        add_cylinder(asset, f"TANK_BlackBandTop_{'L' if x < 0 else 'R'}", (x, 0.61, 0.105), (x, 0.64, 0.105), 0.061, "MAT_BlackenedIron", 18)
        add_cylinder(asset, f"TANK_BlackBandBottom_{'L' if x < 0 else 'R'}", (x, 0.27, 0.105), (x, 0.30, 0.105), 0.061, "MAT_BlackenedIron", 18)
    for i, x in enumerate([-0.17, 0.17]):
        add_cylinder(asset, f"GAUGE_CreamFace_{i + 1:02d}", (x, 0.865, 0.067), (x, 0.865, 0.102), 0.087, "MAT_GaugeFace_Cream", 24)
        add_torus(asset, f"GAUGE_BrassRim_{i + 1:02d}", (x, 0.865, 0.105), (0, 0, 1), 0.088, 0.007, "MAT_Brass_PolishedEdge")
        add_box(asset, f"GAUGE_RedNeedle_{i + 1:02d}", (x + 0.018, 0.875, 0.112), (0.010, 0.074, 0.006), "MAT_RedValve_Warning", (0, 0, math.radians(-32 if i == 0 else 24)))
    add_spoked_wheel(asset, "MOVING_MainValveWheel", (0.0, 0.47, 0.128), (0, 0, 1), 0.132, 0.010, "MAT_RedValve_Warning", 6)
    add_cylinder(asset, "MOVING_MainValveStem", (0.0, 0.47, 0.058), (0.0, 0.47, 0.155), 0.026, "MAT_Brass_PolishedEdge", 18)
    pipe_runs = [
        [(-0.22, 0.68, 0.13), (-0.22, 0.78, 0.13), (-0.17, 0.80, 0.13)],
        [(0.22, 0.68, 0.13), (0.22, 0.78, 0.13), (0.17, 0.80, 0.13)],
        [(-0.22, 0.23, 0.13), (-0.10, 0.20, 0.13), (0.0, 0.34, 0.13)],
        [(0.22, 0.23, 0.13), (0.30, 0.28, 0.13), (0.30, 0.28, 0.19)],
        [(-0.04, 0.58, 0.13), (0.04, 0.58, 0.13), (0.12, 0.68, 0.13)],
    ]
    for run_idx, points in enumerate(pipe_runs):
        for seg_idx in range(len(points) - 1):
            add_cylinder(asset, f"PIPE_CopperRun_{run_idx + 1:02d}_{seg_idx + 1:02d}", points[seg_idx], points[seg_idx + 1], 0.013, "MAT_CopperPipe_Aged", 10)
    add_cylinder(asset, "VENT_TopSteamNozzle", (0.0, 0.99, 0.06), (0.0, 1.07, 0.135), 0.030, "MAT_BlackenedIron", 16)
    add_box(asset, "LABEL_CreamPressurePlate", (0.0, 0.145, 0.085), (0.38, 0.075, 0.016), "MAT_GaugeFace_Cream")
    add_box(asset, "LABEL_RedWarningStripe", (0.0, 0.102, 0.087), (0.36, 0.025, 0.018), "MAT_RedValve_Warning")
    rivet_positions = []
    for x in [-0.32, -0.10, 0.10, 0.32]:
        rivet_positions.append((x, 1.04, 0.055))
        rivet_positions.append((x, 0.08, 0.055))
    for y in [0.20, 0.44, 0.68, 0.92]:
        rivet_positions.append((-0.33, y, 0.055))
        rivet_positions.append((0.33, y, 0.055))
    add_rivets(asset, rivet_positions, "z", 0.010)
    for socket in sockets:
        add_socket(asset, socket["name"], tuple(socket["position_m"]), 0.044)
    return asset


def build_crank_lever_prop():
    sockets = [
        {"name": "SOCKET_Interact", "position_m": [0.07, 0.18, 0.16], "purpose": "Player interaction trace target near handle."},
        {"name": "SOCKET_LeverPivot", "position_m": [0.0, 0.32, 0.09], "purpose": "Lever/crank rotates around local Z from idle to pulled state."},
        {"name": "SOCKET_Handle", "position_m": [0.13, 0.07, 0.13], "purpose": "Hand/contact point for pull animation."},
        {"name": "SOCKET_WallMount", "position_m": [0.0, 0.25, 0.0], "purpose": "Wall/console align point."},
    ]
    asset = MeshAsset(
        "PROP_CrankLeverSwitch_Blockout",
        "Crank Lever / Switch Prop",
        "Prop / interactable switch",
        "Approx 0.38 m W x 0.55 m H x 0.22 m D including socket markers; compact crank switch silhouette.",
        "Origin at bottom-center of wall contact plane; back face sits near Z 0.",
        [
            "Import OBJ at scale factor 1.0.",
            "Place on wall panels, machinery, or console fronts with +Z facing the player.",
            "MOVING_LeverArm and MOVING_WoodHandle groups should become separate child meshes for animation if integrated.",
        ],
        [
            "Lever rotates around SOCKET_LeverPivot/local Z by roughly 70 degrees.",
            "Wood handle can counter-rotate around its own X axis while lever moves.",
            "Ratchet teeth can click via audio/VFX at start, midpoint, and end stops.",
        ],
        [
            "Use a small box collider on plate, approx 0.32 m W x 0.50 m H x 0.08 m D.",
            "Use a trigger/interact volume around handle, approx 0.20 m radius.",
        ],
        sockets,
        [
            "LOD0 keeps ratchet teeth, hub, lever, handle, and rivets.",
            "LOD1 can merge plate and hub details.",
            "LOD2/mobile can remove ratchet teeth and socket marker geometry.",
        ],
    )
    add_box(asset, "PLATE_BlackenedMount", (0.0, 0.25, 0.026), (0.31, 0.50, 0.052), "MAT_BlackenedIron")
    add_box(asset, "PLATE_BrassInset", (0.0, 0.25, 0.059), (0.22, 0.38, 0.025), "MAT_Brass_Worn")
    add_cylinder(asset, "MOVING_PivotHub", (0.0, 0.32, 0.055), (0.0, 0.32, 0.122), 0.070, "MAT_Brass_PolishedEdge", 20)
    add_cylinder(asset, "MOVING_LeverArm", (0.0, 0.32, 0.125), (0.115, 0.085, 0.125), 0.023, "MAT_BlackenedIron", 12)
    add_cylinder(asset, "MOVING_WoodHandle", (0.072, 0.06, 0.125), (0.165, 0.06, 0.125), 0.033, "MAT_WalnutLeather_Dark", 14)
    add_cylinder(asset, "MOVING_HandleBrassCap", (0.165, 0.06, 0.125), (0.19, 0.06, 0.125), 0.034, "MAT_Brass_PolishedEdge", 14)
    add_torus(asset, "RATCHET_BrassArcGuide", (0.0, 0.32, 0.081), (0, 0, 1), 0.126, 0.006, "MAT_Brass_Worn", 28, 6)
    for i, theta in enumerate([205, 225, 245, 265, 285, 305]):
        rad = math.radians(theta)
        x = math.cos(rad) * 0.126
        y = 0.32 + math.sin(rad) * 0.126
        add_box(asset, f"RATCHET_Tooth_{i + 1:02d}", (x, y, 0.09), (0.020, 0.034, 0.032), "MAT_BlackenedIron", (0, 0, rad))
    add_box(asset, "LABEL_CreamSwitchPlate", (0.0, 0.095, 0.082), (0.18, 0.052, 0.014), "MAT_GaugeFace_Cream")
    add_box(asset, "LABEL_RedSetMark", (0.068, 0.095, 0.091), (0.032, 0.038, 0.014), "MAT_RedValve_Warning")
    add_rivets(asset, [(-0.12, 0.46, 0.058), (0.12, 0.46, 0.058), (-0.12, 0.05, 0.058), (0.12, 0.05, 0.058)], "z", 0.009)
    for socket in sockets:
        add_socket(asset, socket["name"], tuple(socket["position_m"]), 0.037)
    return asset


def write_mtl(path):
    lines = ["# Shared blockout material palette for Brassworks Breach WeaponsProps staging.\n"]
    for name, data in MATERIALS.items():
        kd = data["kd"]
        ks = data["ks"]
        lines.extend(
            [
                f"newmtl {name}\n",
                f"Kd {kd[0]:.4f} {kd[1]:.4f} {kd[2]:.4f}\n",
                f"Ka {kd[0] * 0.18:.4f} {kd[1] * 0.18:.4f} {kd[2] * 0.18:.4f}\n",
                f"Ks {ks[0]:.4f} {ks[1]:.4f} {ks[2]:.4f}\n",
                f"Ns {data['ns']:.1f}\n",
                "illum 2\n",
                "\n",
            ]
        )
    path.write_text("".join(lines), encoding="utf-8")


def write_obj(asset, art_dir):
    path = art_dir / f"{asset.name}.obj"
    lines = [
        f"# {asset.title}\n",
        "# Generated staged blockout mesh for Brassworks Breach.\n",
        "# Unity scale: 1 unit = 1 meter. +Y up, +Z forward.\n",
        f"mtllib {SHARED_MTL_NAME}\n",
        f"o {asset.name}\n",
    ]
    for vx, vy, vz in asset.vertices:
        lines.append(f"v {vx:.6f} {vy:.6f} {vz:.6f}\n")
    current_group = None
    current_material = None
    for face in asset.faces:
        if face.group != current_group:
            current_group = face.group
            lines.append(f"g {current_group}\n")
        if face.material != current_material:
            current_material = face.material
            lines.append(f"usemtl {current_material}\n")
        a, b, c = face.indices
        lines.append(f"f {a} {b} {c}\n")
    path.write_text("".join(lines), encoding="utf-8")


def material_color(material, shade=1.0):
    kd = MATERIALS[material]["kd"]
    return tuple(max(0, min(255, int(c * 255 * shade))) for c in kd)


def render_preview(asset, path, size=(1100, 820)):
    width, height = size
    img = Image.new("RGB", size, (28, 30, 32))
    draw = ImageDraw.Draw(img)
    title_band = 78
    view_dir = normalize((0.72, -0.55, 0.86))
    screen_u = normalize(cross((0, 1, 0), view_dir))
    screen_v = normalize(cross(view_dir, screen_u))
    projected = [(dot(v, screen_u), dot(v, screen_v), dot(v, view_dir)) for v in asset.vertices]
    xs = [p[0] for p in projected]
    ys = [p[1] for p in projected]
    min_x, max_x = min(xs), max(xs)
    min_y, max_y = min(ys), max(ys)
    content_w = width - 110
    content_h = height - title_band - 110
    scale = min(content_w / max(max_x - min_x, 0.01), content_h / max(max_y - min_y, 0.01))
    ox = width / 2.0 - (min_x + max_x) * 0.5 * scale
    oy = title_band + content_h / 2.0 + 20 + (min_y + max_y) * 0.5 * scale
    light_dir = normalize((-0.4, 0.8, -0.6))
    face_items = []
    for face in asset.faces:
        pts3 = [asset.vertices[i - 1] for i in face.indices]
        centroid = mul(add(add(pts3[0], pts3[1]), pts3[2]), 1 / 3)
        depth = dot(centroid, view_dir)
        normal = normalize(cross(sub(pts3[1], pts3[0]), sub(pts3[2], pts3[0])))
        shade = 0.62 + 0.38 * max(0.0, dot(normal, light_dir))
        face_items.append((depth, face, shade))
    face_items.sort(key=lambda item: item[0])
    for _depth, face, shade in face_items:
        poly = []
        for idx in face.indices:
            px, py, _pz = projected[idx - 1]
            poly.append((ox + px * scale, oy - py * scale))
        fill = material_color(face.material, shade)
        outline = tuple(max(0, int(c * 0.62)) for c in fill)
        draw.polygon(poly, fill=fill, outline=outline)

    try:
        font_title = ImageFont.truetype("arial.ttf", 24)
        font_small = ImageFont.truetype("arial.ttf", 16)
    except OSError:
        font_title = ImageFont.load_default()
        font_small = ImageFont.load_default()
    draw.rectangle((0, 0, width, title_band), fill=(38, 40, 42))
    draw.text((26, 18), asset.name, fill=(236, 228, 208), font=font_title)
    draw.text((26, 49), asset.dimensions_m, fill=(190, 185, 170), font=font_small)
    draw.text((26, height - 34), f"{len(asset.vertices)} verts / {len(asset.faces)} tris | staged OBJ blockout", fill=(174, 172, 160), font=font_small)
    path.parent.mkdir(parents=True, exist_ok=True)
    img.save(path)


def write_contact_sheet(asset_paths, output_path):
    thumbs = []
    for asset, preview_path in asset_paths:
        img = Image.open(preview_path).convert("RGB")
        img.thumbnail((720, 470))
        thumbs.append((asset, img.copy()))
    sheet = Image.new("RGB", (1600, 1160), (24, 26, 28))
    draw = ImageDraw.Draw(sheet)
    try:
        font_title = ImageFont.truetype("arial.ttf", 30)
        font_label = ImageFont.truetype("arial.ttf", 18)
    except OSError:
        font_title = ImageFont.load_default()
        font_label = ImageFont.load_default()
    draw.text((34, 24), "Brassworks Breach - Weapons & Props Staging Contact Sheet", fill=(235, 226, 205), font=font_title)
    positions = [(35, 85), (815, 85), (35, 445), (815, 445), (35, 805), (815, 805)]
    for (asset, img), (x, y) in zip(thumbs, positions):
        draw.rectangle((x - 8, y - 8, x + 728, y + 320), fill=(42, 43, 43))
        sheet.paste(img.resize((720, 300)), (x, y))
        draw.text((x + 8, y + 305), asset.name, fill=(224, 212, 185), font=font_label)
    sheet.save(output_path)


def write_manifest(assets, doc_dir, timestamp):
    manifest_json = {
        "generated_at": timestamp,
        "worker": "Worker C",
        "project": "Brassworks Breach",
        "scope": [
            ART_FOLDER.as_posix(),
            DOC_FOLDER.as_posix(),
        ],
        "unity_import": {
            "scale": "1 Unity unit = 1 meter; import OBJ scale factor 1.0.",
            "axis": "+Y up, +Z forward/down-barrel for weapons, +Z room-facing for wall props.",
            "materials": f"Shared MTL: {ART_FOLDER.as_posix()}/{SHARED_MTL_NAME}",
            "socket_markers": "Magenta geometry groups named SOCKET_* are temporary locator meshes; recreate as empty transforms or sockets during prefab integration.",
        },
        "materials": {name: data["note"] for name, data in MATERIALS.items()},
        "assets": [],
    }
    for asset in assets:
        manifest_json["assets"].append(
            {
                "name": asset.name,
                "title": asset.title,
                "category": asset.category,
                "obj": asset.obj_path(),
                "preview": asset.preview_path(),
                "approx_dimensions_m": asset.dimensions_m,
                "computed_bounds_m": asset.bounds,
                "pivot": asset.pivot,
                "vertex_count": len(asset.vertices),
                "triangle_count": len(asset.faces),
                "materials": asset.materials,
                "sockets": asset.sockets,
                "import_notes": asset.import_notes,
                "animation_notes": asset.animation_notes,
                "collision_notes": asset.collision_notes,
                "lod_notes": asset.lod_notes,
            }
        )
    (doc_dir / "WEAPONS_PROPS_MANIFEST.json").write_text(json.dumps(manifest_json, indent=2), encoding="utf-8")

    lines = [
        "# Weapons Props Manifest\n",
        "\n",
        f"Generated: {timestamp}\n",
        "\n",
        "Scope: staged production output for `Assets/_Project/ArtStaging/WeaponsProps/` and `Documentation/AssetProduction/WeaponsProps/` only. No scenes, gameplay scripts, README, or root project docs are modified by this batch.\n",
        "\n",
        "Unity import notes:\n",
        "\n",
        "- Import OBJ files at scale factor 1.0. Unity unit scale is meters.\n",
        "- The shared `.mtl` file provides intentional material slots for brass, iron, walnut/leather, copper, gauge enamel, pressure glass, warning red, and socket markers.\n",
        "- Magenta `SOCKET_*` geometry is visible locator geometry. During prefab integration, recreate those as empty child transforms or sockets, then hide/delete the marker mesh.\n",
        "- Viewmodel meshes are proportion and silhouette blockouts, not final collision meshes.\n",
        "\n",
        "## Shared Material Slots\n",
        "\n",
    ]
    for name, data in MATERIALS.items():
        lines.append(f"- `{name}`: {data['note']}\n")
    lines.append("\n")
    for asset in assets:
        lines.extend(
            [
                f"## {asset.name}\n",
                "\n",
                f"- Title: {asset.title}\n",
                f"- Category: {asset.category}\n",
                f"- Mesh: `{asset.obj_path()}`\n",
                f"- Preview: `{asset.preview_path()}`\n",
                f"- Approx dimensions: {asset.dimensions_m}\n",
                f"- Computed bounds: min {asset.bounds['min']} max {asset.bounds['max']} size {asset.bounds['size']} meters\n",
                f"- Pivot/orientation: {asset.pivot}\n",
                f"- Mesh budget now: {len(asset.vertices)} vertices, {len(asset.faces)} triangles\n",
                f"- Material slots: {', '.join(f'`{m}`' for m in asset.materials)}\n",
                "\n",
                "Import notes:\n",
            ]
        )
        for item in asset.import_notes:
            lines.append(f"- {item}\n")
        lines.append("\nSocket notes:\n\n")
        lines.append("| Socket | Local position, meters | Purpose |\n")
        lines.append("| --- | --- | --- |\n")
        for socket in asset.sockets:
            lines.append(f"| `{socket['name']}` | `{socket['position_m']}` | {socket['purpose']} |\n")
        lines.append("\nAnimation/moving-part notes:\n")
        for item in asset.animation_notes:
            lines.append(f"- {item}\n")
        lines.append("\nCollision/placement notes:\n")
        for item in asset.collision_notes:
            lines.append(f"- {item}\n")
        lines.append("\nLOD/platform notes:\n")
        for item in asset.lod_notes:
            lines.append(f"- {item}\n")
        lines.append("\n")
    (doc_dir / "WEAPONS_PROPS_MANIFEST.md").write_text("".join(lines), encoding="utf-8")


def append_work_log(doc_dir, timestamp, assets):
    path = doc_dir / "WEAPONS_PROPS_WORK_LOG.md"
    if path.exists():
        text = path.read_text(encoding="utf-8")
    else:
        text = "# Weapons Props Work Log\n\n"
    lines = [
        f"## {timestamp}\n",
        "\n",
        "- Worker C generated the first production-staged weapons/props batch as deterministic OBJ blockouts with shared MTL material slots.\n",
        "- Created hero viewmodel blockouts for `WPN_PressurePistol_Viewmodel_Blockout` and `WPN_SteamScattergun_Viewmodel_Blockout` with muzzle, grip, pressure chamber, valve, vent, pump/trigger socket marker geometry.\n",
        "- Created world pickup blockouts for `PICKUP_PressureCell_Ammo_Blockout` and `PICKUP_ScattergunSlugCanister_Blockout` with trigger/collision guidance.\n",
        "- Created interactable prop blockouts for `PROP_WallPressureStation_Blockout` and `PROP_CrankLeverSwitch_Blockout` with valve/lever moving-part notes.\n",
        "- Exported individual preview PNGs and a contact sheet for quick art review.\n",
        "- Stayed within Worker C scope; no Unity scenes, gameplay scripts, README, or root project documentation were modified.\n",
        "\n",
        "Generated assets:\n",
    ]
    for asset in assets:
        lines.append(f"- `{asset.obj_path()}` ({len(asset.vertices)} verts / {len(asset.faces)} tris)\n")
    lines.append("\n")
    path.write_text(text.rstrip() + "\n\n" + "".join(lines), encoding="utf-8")


def main():
    script_dir = Path(__file__).resolve().parent
    project_root = script_dir.parents[2]
    art_dir = project_root / ART_FOLDER
    doc_dir = project_root / DOC_FOLDER
    art_dir.mkdir(parents=True, exist_ok=True)
    doc_dir.mkdir(parents=True, exist_ok=True)

    timestamp = datetime.now().astimezone().strftime("%Y-%m-%d %H:%M:%S %z")
    timestamp = f"{timestamp[:-2]}:{timestamp[-2:]}"

    assets = [
        build_pressure_pistol(),
        build_scattergun(),
        build_pressure_cell_pickup(),
        build_slug_canister_pickup(),
        build_pressure_station_prop(),
        build_crank_lever_prop(),
    ]

    write_mtl(art_dir / SHARED_MTL_NAME)
    for asset in assets:
        write_obj(asset, art_dir)
        render_preview(asset, doc_dir / f"PREVIEW_{asset.name}.png")
    write_contact_sheet(
        [(asset, doc_dir / f"PREVIEW_{asset.name}.png") for asset in assets],
        doc_dir / "PREVIEW_WeaponsProps_ContactSheet.png",
    )
    write_manifest(assets, doc_dir, timestamp)
    append_work_log(doc_dir, timestamp, assets)

    print(f"Generated {len(assets)} staged mesh assets in {art_dir}")
    print(f"Documentation and previews written to {doc_dir}")


if __name__ == "__main__":
    main()
