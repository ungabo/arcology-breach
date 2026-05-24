from __future__ import annotations

import argparse
import math
import os
import random
import sys
from pathlib import Path

try:
    import bpy
    from mathutils import Vector
except ImportError as exc:
    raise SystemExit(
        "This scene script must be run by Blender's Python. Example:\n"
        "blender --background --factory-startup --python blender_pressure_pistol_recovery04_scene.py"
    ) from exc


ROOT = Path(__file__).resolve().parents[4]
DEFAULT_RENDER_OUT = (
    ROOT
    / "Documentation"
    / "ConceptRenders"
    / "RENDER_HFLD_Recovery04_pressure_pistol_blender_proof.jpg"
)

RANDOM_SEED = 5404
random.seed(RANDOM_SEED)


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Build and render the HFLD Recovery04 pressure-pistol Blender proof scene."
    )
    parser.add_argument(
        "--output",
        default=os.environ.get("HFLDR_RECOVERY04_OUT", str(DEFAULT_RENDER_OUT)),
        help="JPG output path for the rendered hero proof.",
    )
    parser.add_argument(
        "--samples",
        type=int,
        default=int(os.environ.get("HFLDR_RECOVERY04_SAMPLES", "96")),
        help="Cycles sample count.",
    )
    parser.add_argument(
        "--res-x",
        type=int,
        default=int(os.environ.get("HFLDR_RECOVERY04_RES_X", "1920")),
        help="Render width.",
    )
    parser.add_argument(
        "--res-y",
        type=int,
        default=int(os.environ.get("HFLDR_RECOVERY04_RES_Y", "1080")),
        help="Render height.",
    )
    parser.add_argument(
        "--save-blend",
        default=os.environ.get("HFLDR_RECOVERY04_SAVE_BLEND", ""),
        help="Optional .blend save path. Empty means render only.",
    )

    if "--" in sys.argv:
        return parser.parse_args(sys.argv[sys.argv.index("--") + 1 :])
    return parser.parse_args([])


def set_input(node: bpy.types.Node, names: tuple[str, ...], value) -> None:
    for name in names:
        if name in node.inputs:
            node.inputs[name].default_value = value
            return


def rgba(color: tuple[float, float, float], alpha: float = 1.0) -> tuple[float, float, float, float]:
    return color[0], color[1], color[2], alpha


def make_principled_material(
    name: str,
    base: tuple[float, float, float],
    *,
    metallic: float = 0.0,
    roughness: float = 0.55,
    alpha: float = 1.0,
    emission: tuple[float, float, float] | None = None,
    emission_strength: float = 0.0,
    bump_strength: float = 0.0,
    bump_scale: float = 55.0,
) -> bpy.types.Material:
    material = bpy.data.materials.new(name)
    material.use_nodes = True
    material.diffuse_color = rgba(base, alpha)

    bsdf = material.node_tree.nodes.get("Principled BSDF")
    if bsdf is None:
        return material

    set_input(bsdf, ("Base Color",), rgba(base, alpha))
    set_input(bsdf, ("Metallic",), metallic)
    set_input(bsdf, ("Roughness",), roughness)
    set_input(bsdf, ("Alpha",), alpha)

    if emission is not None:
        set_input(bsdf, ("Emission Color", "Emission"), rgba(emission, 1.0))
        set_input(bsdf, ("Emission Strength",), emission_strength)

    if alpha < 1.0:
        material.blend_method = "BLEND"
        if hasattr(material, "use_screen_refraction"):
            material.use_screen_refraction = True
        material.show_transparent_back = True
        material.diffuse_color = rgba(base, alpha)

    if bump_strength > 0.0:
        nodes = material.node_tree.nodes
        links = material.node_tree.links
        noise = nodes.new(type="ShaderNodeTexNoise")
        noise.inputs["Scale"].default_value = bump_scale
        noise.inputs["Detail"].default_value = 10.0
        noise.inputs["Roughness"].default_value = 0.62
        bump = nodes.new(type="ShaderNodeBump")
        bump.inputs["Strength"].default_value = bump_strength
        bump.inputs["Distance"].default_value = 0.06
        links.new(noise.outputs["Fac"], bump.inputs["Height"])
        links.new(bump.outputs["Normal"], bsdf.inputs["Normal"])

    return material


def make_materials() -> dict[str, bpy.types.Material]:
    return {
        "black_iron": make_principled_material(
            "MAT_HFLDR_blackened_iron",
            (0.025, 0.023, 0.020),
            metallic=1.0,
            roughness=0.47,
            bump_strength=0.075,
            bump_scale=80.0,
        ),
        "pipe_dark": make_principled_material(
            "MAT_HFLDR_dark_pipe_metal",
            (0.055, 0.052, 0.047),
            metallic=1.0,
            roughness=0.62,
            bump_strength=0.065,
            bump_scale=62.0,
        ),
        "brass": make_principled_material(
            "MAT_HFLDR_aged_brass",
            (0.72, 0.42, 0.13),
            metallic=1.0,
            roughness=0.36,
            bump_strength=0.045,
            bump_scale=95.0,
        ),
        "copper_hot": make_principled_material(
            "MAT_HFLDR_hot_copper_coil",
            (0.86, 0.29, 0.075),
            metallic=0.85,
            roughness=0.27,
            emission=(1.0, 0.32, 0.06),
            emission_strength=0.55,
            bump_strength=0.035,
            bump_scale=120.0,
        ),
        "gauge_face": make_principled_material(
            "MAT_HFLDR_cream_gauge_face",
            (0.86, 0.78, 0.56),
            metallic=0.0,
            roughness=0.72,
            bump_strength=0.015,
            bump_scale=135.0,
        ),
        "gauge_ink": make_principled_material(
            "MAT_HFLDR_gauge_ink",
            (0.055, 0.038, 0.025),
            roughness=0.7,
        ),
        "needle": make_principled_material(
            "MAT_HFLDR_red_gauge_needle",
            (0.72, 0.035, 0.025),
            roughness=0.48,
        ),
        "glass": make_principled_material(
            "MAT_HFLDR_smoked_glass",
            (0.72, 0.88, 1.0),
            metallic=0.0,
            roughness=0.02,
            alpha=0.28,
        ),
        "leather": make_principled_material(
            "MAT_HFLDR_dark_worn_leather",
            (0.22, 0.085, 0.028),
            metallic=0.0,
            roughness=0.78,
            bump_strength=0.12,
            bump_scale=48.0,
        ),
        "soot": make_principled_material(
            "MAT_HFLDR_soot_dark",
            (0.006, 0.005, 0.004),
            metallic=0.15,
            roughness=0.9,
        ),
        "smoke": make_principled_material(
            "MAT_HFLDR_translucent_smoke",
            (0.46, 0.43, 0.37),
            roughness=1.0,
            alpha=0.16,
        ),
        "warm_emission": make_principled_material(
            "MAT_HFLDR_warm_practical_emission",
            (1.0, 0.58, 0.16),
            roughness=0.2,
            emission=(1.0, 0.44, 0.10),
            emission_strength=3.2,
        ),
    }


def add_bevel(obj: bpy.types.Object, width: float, segments: int = 3) -> bpy.types.Object:
    bevel = obj.modifiers.new(name="HFLDR soft bevels", type="BEVEL")
    bevel.width = width
    bevel.segments = segments
    bevel.profile = 0.5
    normal = obj.modifiers.new(name="HFLDR weighted normals", type="WEIGHTED_NORMAL")
    normal.keep_sharp = True
    return obj


def assign_material(obj: bpy.types.Object, material: bpy.types.Material) -> bpy.types.Object:
    obj.data.materials.append(material)
    return obj


def shade_smooth(obj: bpy.types.Object) -> bpy.types.Object:
    bpy.context.view_layer.objects.active = obj
    obj.select_set(True)
    bpy.ops.object.shade_smooth()
    obj.select_set(False)
    return obj


def add_cylinder(
    name: str,
    radius: float,
    depth: float,
    location: tuple[float, float, float],
    rotation: tuple[float, float, float],
    material: bpy.types.Material,
    *,
    vertices: int = 72,
    bevel: float = 0.025,
) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cylinder_add(
        vertices=vertices,
        radius=radius,
        depth=depth,
        end_fill_type="NGON",
        location=location,
        rotation=rotation,
    )
    obj = bpy.context.object
    obj.name = name
    assign_material(obj, material)
    shade_smooth(obj)
    add_bevel(obj, bevel, 4)
    return obj


def add_box(
    name: str,
    location: tuple[float, float, float],
    dimensions: tuple[float, float, float],
    material: bpy.types.Material,
    *,
    rotation: tuple[float, float, float] = (0.0, 0.0, 0.0),
    bevel: float = 0.02,
) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cube_add(size=1.0, location=location, rotation=rotation)
    obj = bpy.context.object
    obj.name = name
    obj.dimensions = dimensions
    bpy.context.view_layer.objects.active = obj
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    assign_material(obj, material)
    add_bevel(obj, bevel, 3)
    return obj


def add_uv_ellipsoid(
    name: str,
    location: tuple[float, float, float],
    scale: tuple[float, float, float],
    material: bpy.types.Material,
    *,
    rotation: tuple[float, float, float] = (0.0, 0.0, 0.0),
    segments: int = 32,
    rings: int = 16,
) -> bpy.types.Object:
    bpy.ops.mesh.primitive_uv_sphere_add(
        segments=segments,
        ring_count=rings,
        radius=1.0,
        location=location,
        rotation=rotation,
    )
    obj = bpy.context.object
    obj.name = name
    obj.scale = scale
    assign_material(obj, material)
    shade_smooth(obj)
    return obj


def add_rivet(
    name: str,
    location: tuple[float, float, float],
    material: bpy.types.Material,
    *,
    radius: float = 0.032,
) -> bpy.types.Object:
    bpy.ops.mesh.primitive_uv_sphere_add(
        segments=14,
        ring_count=8,
        radius=radius,
        location=location,
    )
    obj = bpy.context.object
    obj.name = name
    assign_material(obj, material)
    shade_smooth(obj)
    return obj


def add_curve_polyline(
    name: str,
    points: list[tuple[float, float, float]],
    material: bpy.types.Material,
    *,
    bevel_depth: float = 0.025,
    bevel_resolution: int = 4,
) -> bpy.types.Object:
    curve = bpy.data.curves.new(name=name, type="CURVE")
    curve.dimensions = "3D"
    curve.resolution_u = 2
    curve.bevel_depth = bevel_depth
    curve.bevel_resolution = bevel_resolution
    spline = curve.splines.new("POLY")
    spline.points.add(len(points) - 1)
    for point, coord in zip(spline.points, points):
        point.co = (coord[0], coord[1], coord[2], 1.0)
    obj = bpy.data.objects.new(name, curve)
    bpy.context.collection.objects.link(obj)
    assign_material(obj, material)
    return obj


def look_at(obj: bpy.types.Object, target: tuple[float, float, float]) -> None:
    direction = Vector(target) - obj.location
    obj.rotation_euler = direction.to_track_quat("-Z", "Y").to_euler()


def create_plate_with_rivets(
    name: str,
    location: tuple[float, float, float],
    dimensions: tuple[float, float, float],
    mats: dict[str, bpy.types.Material],
    *,
    y_face: float | None = None,
    rivet_radius: float = 0.027,
) -> int:
    add_box(name, location, dimensions, mats["brass"], bevel=0.018)
    y = y_face if y_face is not None else location[1] - dimensions[1] * 0.58
    count = 0
    for sx in (-0.36, 0.36):
        for sz in (-0.34, 0.34):
            add_rivet(
                f"{name}_rivet_{count:02d}",
                (
                    location[0] + dimensions[0] * sx,
                    y,
                    location[2] + dimensions[2] * sz,
                ),
                mats["brass"],
                radius=rivet_radius,
            )
            count += 1
    return count


def create_rivet_arc(
    prefix: str,
    center_x: float,
    center_y: float,
    center_z: float,
    radius: float,
    x_offset: float,
    mats: dict[str, bpy.types.Material],
    *,
    start_deg: float = 205.0,
    end_deg: float = 335.0,
    count: int = 12,
) -> int:
    for index in range(count):
        t = math.radians(start_deg + (end_deg - start_deg) * index / max(1, count - 1))
        add_rivet(
            f"{prefix}_rivet_arc_{index:02d}",
            (
                center_x + x_offset,
                center_y + math.cos(t) * radius,
                center_z + math.sin(t) * radius,
            ),
            mats["brass"],
            radius=0.024,
        )
    return count


def build_pressure_pistol(mats: dict[str, bpy.types.Material]) -> dict[str, int]:
    counts = {
        "visible_fasteners": 0,
        "brass_plates": 0,
        "coil_turns": 8,
        "gauge_ticks": 44,
        "pressure_ports": 0,
        "top_valves": 0,
    }

    barrel_axis = (0.0, math.pi / 2.0, 0.0)
    side_port_axis = (math.pi / 2.0, 0.0, 0.0)

    add_cylinder(
        "HFLDR_main_blackened_barrel_bevelled",
        0.34,
        4.55,
        (-0.38, 0.0, 0.78),
        barrel_axis,
        mats["black_iron"],
        bevel=0.035,
    )
    add_cylinder(
        "HFLDR_barrel_inner_shadow_bore",
        0.225,
        0.055,
        (-2.72, -0.005, 0.78),
        barrel_axis,
        mats["soot"],
        bevel=0.008,
    )

    for index, x in enumerate((-2.42, -1.66, -0.72, 0.28, 1.08)):
        add_cylinder(
            f"HFLDR_aged_brass_barrel_collar_{index:02d}",
            0.395,
            0.16,
            (x, 0.0, 0.78),
            barrel_axis,
            mats["brass"],
            bevel=0.025,
        )
        counts["visible_fasteners"] += create_rivet_arc(
            f"HFLDR_collar_{index:02d}",
            x,
            0.0,
            0.78,
            0.405,
            -0.085,
            mats,
            count=10,
        )

    for index, (x, radius, depth) in enumerate(
        (
            (-2.90, 0.31, 0.34),
            (-3.12, 0.26, 0.28),
            (-3.29, 0.19, 0.18),
            (-3.40, 0.135, 0.09),
        )
    ):
        mat = mats["brass"] if index % 2 == 0 else mats["pipe_dark"]
        add_cylinder(
            f"HFLDR_nested_muzzle_ring_{index:02d}",
            radius,
            depth,
            (x, 0.0, 0.78),
            barrel_axis,
            mat,
            bevel=0.022,
        )

    add_cylinder(
        "HFLDR_lower_pressure_tank_bevelled",
        0.245,
        3.42,
        (-0.48, 0.16, 0.22),
        barrel_axis,
        mats["pipe_dark"],
        bevel=0.035,
    )
    for index, x in enumerate((-1.95, -1.18, -0.38, 0.42, 1.02)):
        add_cylinder(
            f"HFLDR_lower_tank_brass_strap_{index:02d}",
            0.272,
            0.085,
            (x, 0.16, 0.22),
            barrel_axis,
            mats["brass"],
            bevel=0.016,
        )
        counts["visible_fasteners"] += create_rivet_arc(
            f"HFLDR_lower_strap_{index:02d}",
            x,
            0.16,
            0.22,
            0.285,
            -0.045,
            mats,
            start_deg=210.0,
            end_deg=323.0,
            count=6,
        )

    bracket_specs = (
        ("HFLDR_vertical_tank_bracket_left", (-1.52, -0.08, 0.48), (0.13, 0.10, 0.55)),
        ("HFLDR_vertical_tank_bracket_mid", (-0.46, -0.08, 0.48), (0.13, 0.10, 0.55)),
        ("HFLDR_vertical_tank_bracket_right", (0.68, -0.08, 0.48), (0.13, 0.10, 0.55)),
    )
    for name, loc, dims in bracket_specs:
        counts["visible_fasteners"] += create_plate_with_rivets(name, loc, dims, mats, y_face=-0.16)
        counts["brass_plates"] += 1

    side_plates = [
        (-2.02, -0.39, 0.78),
        (-1.46, -0.39, 0.78),
        (-0.90, -0.39, 0.78),
        (-0.34, -0.39, 0.78),
        (0.22, -0.39, 0.78),
        (0.78, -0.39, 0.78),
        (1.24, -0.39, 0.72),
    ]
    for index, loc in enumerate(side_plates):
        counts["visible_fasteners"] += create_plate_with_rivets(
            f"HFLDR_side_brass_service_plate_{index:02d}",
            loc,
            (0.43, 0.055, 0.19),
            mats,
            y_face=-0.43,
        )
        counts["brass_plates"] += 1

    for index, loc in enumerate(((-1.16, -0.04, 1.10), (-0.34, -0.04, 1.11), (0.54, -0.04, 1.11))):
        counts["visible_fasteners"] += create_plate_with_rivets(
            f"HFLDR_top_brass_access_plate_{index:02d}",
            loc,
            (0.55, 0.18, 0.065),
            mats,
            y_face=-0.15,
            rivet_radius=0.023,
        )
        counts["brass_plates"] += 1

    tray = add_box(
        "HFLDR_dark_recessed_coil_window_backing",
        (0.38, -0.455, 0.58),
        (1.38, 0.045, 0.42),
        mats["soot"],
        bevel=0.015,
    )
    tray.hide_select = True
    counts["brass_plates"] += 1
    for suffix, z in (("top", 0.82), ("bottom", 0.34)):
        add_box(
            f"HFLDR_coil_window_brass_rail_{suffix}",
            (0.38, -0.485, z),
            (1.52, 0.07, 0.055),
            mats["brass"],
            bevel=0.018,
        )
        counts["brass_plates"] += 1
    for suffix, x in (("left", -0.44), ("right", 1.20)):
        add_box(
            f"HFLDR_coil_window_brass_endcap_{suffix}",
            (x, -0.485, 0.58),
            (0.07, 0.07, 0.46),
            mats["brass"],
            bevel=0.018,
        )
        counts["brass_plates"] += 1

    coil_points: list[tuple[float, float, float]] = []
    turns = counts["coil_turns"]
    start_x = -0.34
    end_x = 1.12
    point_count = 260
    for i in range(point_count):
        t = i / (point_count - 1)
        angle = t * math.tau * turns
        x = start_x + (end_x - start_x) * t
        y = -0.505 + math.cos(angle) * 0.055
        z = 0.58 + math.sin(angle) * 0.155
        coil_points.append((x, y, z))
    add_curve_polyline(
        "HFLDR_exposed_hot_copper_coil_8_turns",
        coil_points,
        mats["copper_hot"],
        bevel_depth=0.022,
        bevel_resolution=5,
    )

    for index, x in enumerate((-1.28, 0.00, 0.98)):
        add_cylinder(
            f"HFLDR_front_pressure_port_socket_{index:02d}",
            0.083,
            0.12,
            (x, -0.435, 0.86),
            side_port_axis,
            mats["pipe_dark"],
            vertices=28,
            bevel=0.012,
        )
        add_cylinder(
            f"HFLDR_front_pressure_port_dark_hole_{index:02d}",
            0.050,
            0.016,
            (x, -0.505, 0.86),
            side_port_axis,
            mats["soot"],
            vertices=24,
            bevel=0.004,
        )
        counts["pressure_ports"] += 1

    for index, x in enumerate((-1.70, -0.06, 0.86)):
        add_cylinder(
            f"HFLDR_top_valve_stem_{index:02d}",
            0.045,
            0.24,
            (x, -0.02, 1.22),
            (0.0, 0.0, 0.0),
            mats["pipe_dark"],
            vertices=24,
            bevel=0.008,
        )
        add_cylinder(
            f"HFLDR_top_valve_brass_cap_{index:02d}",
            0.105,
            0.055,
            (x, -0.02, 1.36),
            (0.0, 0.0, 0.0),
            mats["brass"],
            vertices=32,
            bevel=0.012,
        )
        counts["top_valves"] += 1

    create_gauge(mats, counts)
    create_trigger_guard_and_grip(mats, counts)
    create_smoke_and_practicals(mats)
    return counts


def create_gauge(mats: dict[str, bpy.types.Material], counts: dict[str, int]) -> None:
    gauge_axis = (math.pi / 2.0, 0.0, 0.0)
    center = (0.18, -0.53, 1.30)

    add_cylinder(
        "HFLDR_top_pressure_gauge_brass_bezel",
        0.365,
        0.085,
        center,
        gauge_axis,
        mats["brass"],
        vertices=80,
        bevel=0.018,
    )
    add_cylinder(
        "HFLDR_top_pressure_gauge_cream_face",
        0.305,
        0.035,
        (center[0], center[1] - 0.045, center[2]),
        gauge_axis,
        mats["gauge_face"],
        vertices=80,
        bevel=0.006,
    )
    add_cylinder(
        "HFLDR_top_pressure_gauge_glass_lens",
        0.322,
        0.018,
        (center[0], center[1] - 0.074, center[2]),
        gauge_axis,
        mats["glass"],
        vertices=80,
        bevel=0.004,
    )

    tick_count = counts["gauge_ticks"]
    for index in range(tick_count):
        angle = math.radians(224.0 - index * (268.0 / (tick_count - 1)))
        radius = 0.236
        tick_len = 0.060 if index % 4 == 0 else 0.035
        loc = (
            center[0] + math.sin(angle) * radius,
            center[1] - 0.096,
            center[2] + math.cos(angle) * radius,
        )
        add_box(
            f"HFLDR_gauge_tick_mark_{index:02d}",
            loc,
            (0.010, 0.010, tick_len),
            mats["gauge_ink"],
            rotation=(0.0, angle, 0.0),
            bevel=0.002,
        )

    needle_angle = math.radians(-42.0)
    add_box(
        "HFLDR_gauge_red_pressure_needle",
        (
            center[0] + math.sin(needle_angle) * 0.085,
            center[1] - 0.112,
            center[2] + math.cos(needle_angle) * 0.085,
        ),
        (0.028, 0.012, 0.225),
        mats["needle"],
        rotation=(0.0, needle_angle, 0.0),
        bevel=0.004,
    )
    add_cylinder(
        "HFLDR_gauge_center_pin",
        0.030,
        0.018,
        (center[0], center[1] - 0.124, center[2]),
        gauge_axis,
        mats["brass"],
        vertices=32,
        bevel=0.004,
    )

    highlight_points = []
    for i in range(42):
        t = math.radians(116.0 + i * 1.75)
        highlight_points.append(
            (
                center[0] + math.sin(t) * 0.238,
                center[1] - 0.135,
                center[2] + math.cos(t) * 0.238,
            )
        )
    add_curve_polyline(
        "HFLDR_gauge_curved_glass_highlight",
        highlight_points,
        mats["glass"],
        bevel_depth=0.007,
        bevel_resolution=3,
    )

    add_cylinder(
        "HFLDR_gauge_brass_mount_stem",
        0.075,
        0.28,
        (center[0], -0.11, 1.03),
        (0.0, 0.0, 0.0),
        mats["brass"],
        vertices=32,
        bevel=0.012,
    )


def create_trigger_guard_and_grip(
    mats: dict[str, bpy.types.Material], counts: dict[str, int]
) -> None:
    add_box(
        "HFLDR_angled_dark_leather_grip_core",
        (1.46, -0.18, 0.08),
        (0.36, 0.30, 0.92),
        mats["leather"],
        rotation=(0.0, math.radians(-12.0), 0.0),
        bevel=0.075,
    )
    for index, z in enumerate((-0.25, -0.08, 0.09, 0.26)):
        add_box(
            f"HFLDR_leather_grip_wrinkle_band_{index:02d}",
            (1.46, -0.345, z),
            (0.40, 0.035, 0.030),
            mats["brass"] if index in (0, 3) else mats["leather"],
            rotation=(0.0, math.radians(-12.0), 0.0),
            bevel=0.010,
        )

    guard_points = [
        (0.80, -0.48, 0.47),
        (0.92, -0.51, 0.22),
        (1.12, -0.51, 0.07),
        (1.34, -0.49, 0.18),
        (1.42, -0.46, 0.45),
    ]
    add_curve_polyline(
        "HFLDR_brass_trigger_guard_loop",
        guard_points,
        mats["brass"],
        bevel_depth=0.032,
        bevel_resolution=5,
    )
    add_box(
        "HFLDR_dark_curved_trigger",
        (1.11, -0.505, 0.31),
        (0.075, 0.045, 0.34),
        mats["pipe_dark"],
        rotation=(0.0, math.radians(-20.0), 0.0),
        bevel=0.025,
    )

    glove_specs = [
        ("palm", (1.88, -0.39, -0.03), (0.42, 0.25, 0.30), (0.0, 0.0, math.radians(8.0))),
        ("thumb", (1.56, -0.53, 0.23), (0.13, 0.105, 0.30), (math.radians(11.0), 0.0, math.radians(-22.0))),
        ("finger_a", (1.72, -0.56, 0.40), (0.115, 0.075, 0.34), (math.radians(7.0), 0.0, math.radians(16.0))),
        ("finger_b", (1.90, -0.55, 0.36), (0.105, 0.072, 0.31), (math.radians(4.0), 0.0, math.radians(9.0))),
        ("finger_c", (2.05, -0.52, 0.28), (0.095, 0.068, 0.27), (math.radians(2.0), 0.0, math.radians(3.0))),
    ]
    for suffix, loc, scale, rot in glove_specs:
        add_uv_ellipsoid(
            f"HFLDR_lower_right_leather_glove_{suffix}",
            loc,
            scale,
            mats["leather"],
            rotation=rot,
            segments=32,
            rings=14,
        )

    knuckle_points = [(1.66, -0.64, 0.52), (1.83, -0.64, 0.50), (1.99, -0.62, 0.43)]
    for index, loc in enumerate(knuckle_points):
        add_rivet(
            f"HFLDR_leather_knuckle_brass_stud_{index:02d}",
            loc,
            mats["brass"],
            radius=0.035,
        )
        counts["visible_fasteners"] += 1


def create_smoke_and_practicals(mats: dict[str, bpy.types.Material]) -> None:
    for index in range(34):
        x = random.uniform(-3.2, 2.6)
        y = random.uniform(-0.88, 0.85)
        z = random.uniform(0.28, 1.95)
        if index < 10:
            x = random.uniform(-1.4, 1.2)
            y = random.uniform(-0.78, -0.58)
            z = random.uniform(0.72, 1.18)
        add_uv_ellipsoid(
            f"HFLDR_layered_smoke_puff_{index:02d}",
            (x, y, z),
            (
                random.uniform(0.22, 0.62),
                random.uniform(0.035, 0.10),
                random.uniform(0.12, 0.42),
            ),
            mats["smoke"],
            rotation=(
                random.uniform(-0.15, 0.15),
                random.uniform(-0.35, 0.35),
                random.uniform(-0.4, 0.4),
            ),
            segments=24,
            rings=10,
        )

    practical_specs = [
        ("left_high", (-2.45, 1.12, 1.95), 0.085, 260.0),
        ("mid_back", (-0.62, 1.36, 1.70), 0.065, 120.0),
        ("right_low", (1.45, 1.18, 1.14), 0.055, 75.0),
    ]
    for suffix, loc, radius, power in practical_specs:
        add_uv_ellipsoid(
            f"HFLDR_warm_background_practical_bulb_{suffix}",
            loc,
            (radius, radius, radius),
            mats["warm_emission"],
            segments=20,
            rings=10,
        )
        bpy.ops.object.light_add(type="POINT", location=loc)
        light = bpy.context.object
        light.name = f"HFLDR_warm_practical_light_{suffix}"
        light.data.color = (1.0, 0.58, 0.22)
        light.data.energy = power
        light.data.shadow_soft_size = 1.2


def create_world_and_lighting(mats: dict[str, bpy.types.Material]) -> None:
    world = bpy.context.scene.world or bpy.data.worlds.new("World")
    bpy.context.scene.world = world
    world.color = (0.012, 0.010, 0.008)

    add_box(
        "HFLDR_dark_smoky_background_wall",
        (-0.20, 1.52, 0.78),
        (7.4, 0.08, 3.8),
        mats["soot"],
        bevel=0.0,
    )
    add_box(
        "HFLDR_warm_underlit_table_plane",
        (-0.28, 0.10, -0.36),
        (7.0, 4.2, 0.06),
        mats["pipe_dark"],
        bevel=0.0,
    )

    bpy.ops.object.light_add(type="AREA", location=(-2.35, -3.25, 3.45))
    key = bpy.context.object
    key.name = "HFLDR_warm_amber_large_key_light"
    key.data.energy = 680.0
    key.data.size = 4.2
    key.data.color = (1.0, 0.57, 0.24)

    bpy.ops.object.light_add(type="AREA", location=(2.70, 1.30, 2.05))
    rim = bpy.context.object
    rim.name = "HFLDR_warm_rim_specular_light"
    rim.data.energy = 220.0
    rim.data.size = 2.2
    rim.data.color = (1.0, 0.72, 0.38)

    bpy.ops.object.light_add(type="AREA", location=(2.80, -2.60, 0.92))
    fill = bpy.context.object
    fill.name = "HFLDR_low_cool_fill_light"
    fill.data.energy = 42.0
    fill.data.size = 5.5
    fill.data.color = (0.26, 0.34, 0.46)


def setup_camera() -> None:
    bpy.ops.object.camera_add(location=(4.15, -5.65, 2.10))
    camera = bpy.context.object
    camera.name = "HFLDR_camera_north_star_lower_right_crop_match"
    look_at(camera, (-0.32, -0.20, 0.66))
    camera.data.lens = 58.0
    camera.data.sensor_width = 32.0
    camera.data.dof.use_dof = True
    camera.data.dof.focus_distance = 5.9
    camera.data.dof.aperture_fstop = 5.6
    bpy.context.scene.camera = camera


def configure_render(args: argparse.Namespace) -> Path:
    scene = bpy.context.scene
    scene.render.engine = "CYCLES"
    scene.cycles.samples = max(16, args.samples)
    scene.cycles.use_denoising = True
    scene.cycles.max_bounces = 8
    scene.cycles.diffuse_bounces = 3
    scene.cycles.glossy_bounces = 4
    scene.cycles.transparent_max_bounces = 8
    scene.render.resolution_x = args.res_x
    scene.render.resolution_y = args.res_y
    scene.render.resolution_percentage = 100
    scene.render.film_transparent = False

    try:
        scene.view_settings.view_transform = "Filmic"
    except TypeError:
        pass
    try:
        scene.view_settings.look = "Medium High Contrast"
    except TypeError:
        pass
    scene.view_settings.exposure = -0.10
    scene.view_settings.gamma = 1.0

    out_path = Path(args.output).resolve()
    out_path.parent.mkdir(parents=True, exist_ok=True)
    scene.render.filepath = str(out_path)
    scene.render.image_settings.file_format = "JPEG"
    scene.render.image_settings.quality = 92
    scene.render.image_settings.color_mode = "RGB"
    return out_path


def clear_scene() -> None:
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete()
    for collection in (bpy.data.meshes, bpy.data.curves, bpy.data.materials, bpy.data.images):
        for datablock in list(collection):
            if datablock.users == 0:
                collection.remove(datablock)


def main() -> None:
    args = parse_args()
    clear_scene()
    mats = make_materials()
    create_world_and_lighting(mats)
    counts = build_pressure_pistol(mats)
    setup_camera()
    out_path = configure_render(args)

    print("HFLD Recovery04 Blender pressure-pistol proof scene")
    print(f"Random seed: {RANDOM_SEED}")
    print(f"Output: {out_path}")
    print(f"Visible fasteners generated: {counts['visible_fasteners']}")
    print(f"Brass plates/brackets/rails generated: {counts['brass_plates']}")
    print(f"Coil turns generated: {counts['coil_turns']}")
    print(f"Gauge tick marks generated: {counts['gauge_ticks']}")
    print(f"Pressure ports generated: {counts['pressure_ports']}")
    print(f"Top valves/caps generated: {counts['top_valves']}")

    if args.save_blend:
        blend_path = Path(args.save_blend).resolve()
        blend_path.parent.mkdir(parents=True, exist_ok=True)
        bpy.ops.wm.save_as_mainfile(filepath=str(blend_path))
        print(f"Saved blend: {blend_path}")

    bpy.ops.render.render(write_still=True)
    print("Recovery04 render complete.")


if __name__ == "__main__":
    main()
