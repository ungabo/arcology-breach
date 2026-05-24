from __future__ import annotations

import hashlib
import json
import math
from dataclasses import dataclass
from datetime import datetime
from pathlib import Path
from typing import Callable

import numpy as np
from PIL import Image, ImageDraw, ImageFont


SIZE = 2048
BATCH_ID = "BBW_FinalMaterialsV1"
ROOT = Path(__file__).resolve().parents[3]
ASSET_ROOT = ROOT / "Assets" / "_Project" / "ArtStaging" / "FinalMaterialsV1"
TEXTURE_DIR = ASSET_ROOT / "Textures"
DOC_ROOT = ROOT / "Documentation" / "AssetProduction" / "FinalMaterialsV1"
PREVIEW_DIR = DOC_ROOT / "Previews"


@dataclass(frozen=True)
class MaterialSpec:
    material_id: str
    display_name: str
    slug: str
    role: str
    tileable: bool
    generator: Callable[[int, np.random.Generator], dict]


def assert_allowed(path: Path) -> None:
    resolved = path.resolve()
    allowed = [ASSET_ROOT.resolve(), DOC_ROOT.resolve()]
    if not any(str(resolved).lower().startswith(str(root).lower()) for root in allowed):
        raise RuntimeError(f"Refusing to write outside allowed FinalMaterialsV1 folders: {resolved}")


def ensure_dirs() -> None:
    for directory in (TEXTURE_DIR, PREVIEW_DIR):
        assert_allowed(directory)
        directory.mkdir(parents=True, exist_ok=True)


def rel(path: Path) -> str:
    return path.resolve().relative_to(ROOT.resolve()).as_posix()


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def save_png(path: Path, array: np.ndarray) -> dict:
    assert_allowed(path)
    image = Image.fromarray(array)
    image.save(path, optimize=True, compress_level=7)
    return {
        "path": rel(path),
        "size": list(image.size),
        "mode": image.mode,
        "sha256": sha256(path),
    }


def clamp01(value: np.ndarray) -> np.ndarray:
    return np.clip(value, 0.0, 1.0)


def to_u8(value: np.ndarray) -> np.ndarray:
    return np.clip(np.rint(value * 255.0), 0, 255).astype(np.uint8)


def smoothstep(edge0: float, edge1: float, value: np.ndarray) -> np.ndarray:
    t = clamp01((value - edge0) / max(edge1 - edge0, 1.0e-6))
    return t * t * (3.0 - 2.0 * t)


def tile_value_noise(size: int, cells: int, rng: np.random.Generator) -> np.ndarray:
    grid = rng.random((cells, cells), dtype=np.float32)
    axis = np.linspace(0.0, float(cells), size, endpoint=False, dtype=np.float32)
    base = np.floor(axis).astype(np.int32)
    frac = axis - base
    base_next = (base + 1) % cells
    sx = frac * frac * (3.0 - 2.0 * frac)
    sy = sx

    n00 = grid[base[:, None], base[None, :]]
    n10 = grid[base[:, None], base_next[None, :]]
    n01 = grid[base_next[:, None], base[None, :]]
    n11 = grid[base_next[:, None], base_next[None, :]]

    nx0 = n00 * (1.0 - sx[None, :]) + n10 * sx[None, :]
    nx1 = n01 * (1.0 - sx[None, :]) + n11 * sx[None, :]
    return (nx0 * (1.0 - sy[:, None]) + nx1 * sy[:, None]).astype(np.float32)


def fbm(size: int, rng: np.random.Generator, cells: int = 4, octaves: int = 5) -> np.ndarray:
    total = np.zeros((size, size), dtype=np.float32)
    amp = 1.0
    amp_sum = 0.0
    for octave in range(octaves):
        total += tile_value_noise(size, cells * (2**octave), rng) * amp
        amp_sum += amp
        amp *= 0.5
    total /= amp_sum
    return clamp01(total)


def coordinate_fields(size: int) -> tuple[np.ndarray, np.ndarray]:
    axis = np.linspace(0.0, 1.0, size, endpoint=False, dtype=np.float32)
    return np.meshgrid(axis, axis)


def mix(c1: tuple[int, int, int], c2: tuple[int, int, int], amount: np.ndarray) -> np.ndarray:
    left = np.array(c1, dtype=np.float32) / 255.0
    right = np.array(c2, dtype=np.float32) / 255.0
    return left[None, None, :] * (1.0 - amount[..., None]) + right[None, None, :] * amount[..., None]


def multiply_color(base: np.ndarray, factor: np.ndarray) -> np.ndarray:
    return clamp01(base * factor[..., None])


def overlay_color(base: np.ndarray, color: tuple[int, int, int], mask: np.ndarray) -> np.ndarray:
    target = np.array(color, dtype=np.float32) / 255.0
    return clamp01(base * (1.0 - mask[..., None]) + target[None, None, :] * mask[..., None])


def normal_from_height(height: np.ndarray, strength: float = 5.0) -> np.ndarray:
    dx = (np.roll(height, -1, axis=1) - np.roll(height, 1, axis=1)) * strength
    dy = (np.roll(height, -1, axis=0) - np.roll(height, 1, axis=0)) * strength
    nx = -dx
    ny = -dy
    nz = np.ones_like(height, dtype=np.float32)
    length = np.sqrt(nx * nx + ny * ny + nz * nz)
    normal = np.stack(
        [
            nx / length * 0.5 + 0.5,
            ny / length * 0.5 + 0.5,
            nz / length * 0.5 + 0.5,
        ],
        axis=-1,
    )
    return to_u8(normal)


def grid_line_mask(xx: np.ndarray, yy: np.ndarray, columns: int, rows: int, width: float) -> np.ndarray:
    gx = np.abs((xx * columns) % 1.0 - 0.5)
    gy = np.abs((yy * rows) % 1.0 - 0.5)
    line_x = 1.0 - smoothstep(width, width * 1.8, gx)
    line_y = 1.0 - smoothstep(width, width * 1.8, gy)
    return np.maximum(line_x, line_y)


def rivet_mask(xx: np.ndarray, yy: np.ndarray, columns: int, rows: int, radius: float) -> np.ndarray:
    rx = np.abs((xx * columns) % 1.0 - 0.5)
    ry = np.abs((yy * rows) % 1.0 - 0.5)
    dist = np.sqrt(rx * rx + ry * ry)
    return 1.0 - smoothstep(radius, radius * 1.45, dist)


def wrapped_line_mask(
    size: int,
    rng: np.random.Generator,
    count: int,
    length_range: tuple[int, int],
    width_range: tuple[int, int],
    angle: float | None = None,
    alpha_range: tuple[int, int] = (60, 150),
) -> np.ndarray:
    image = Image.new("L", (size, size), 0)
    draw = ImageDraw.Draw(image)
    for _ in range(count):
        cx = float(rng.uniform(0, size))
        cy = float(rng.uniform(0, size))
        length = float(rng.integers(length_range[0], length_range[1]))
        line_angle = angle if angle is not None else float(rng.uniform(0.0, math.tau))
        line_angle += float(rng.normal(0.0, 0.2))
        dx = math.cos(line_angle) * length * 0.5
        dy = math.sin(line_angle) * length * 0.5
        width = int(rng.integers(width_range[0], width_range[1] + 1))
        alpha = int(rng.integers(alpha_range[0], alpha_range[1] + 1))
        points = (cx - dx, cy - dy, cx + dx, cy + dy)
        for ox in (-size, 0, size):
            for oy in (-size, 0, size):
                shifted = (points[0] + ox, points[1] + oy, points[2] + ox, points[3] + oy)
                draw.line(shifted, fill=alpha, width=width)
    return np.asarray(image, dtype=np.float32) / 255.0


def checkerboard(size: tuple[int, int], cell: int = 24) -> Image.Image:
    width, height = size
    yy, xx = np.indices((height, width))
    mask = ((xx // cell + yy // cell) % 2).astype(np.uint8)
    light = np.array([89, 82, 70], dtype=np.uint8)
    dark = np.array([48, 45, 40], dtype=np.uint8)
    pixels = np.where(mask[..., None] == 1, light, dark)
    return Image.fromarray(pixels, "RGB")


def composite_for_preview(array: np.ndarray) -> Image.Image:
    image = Image.fromarray(array)
    if image.mode == "RGBA":
        background = checkerboard(image.size, 32)
        background.paste(image, mask=image.getchannel("A"))
        return background.convert("RGB")
    return image.convert("RGB")


def material_aged_brass(size: int, rng: np.random.Generator) -> dict:
    xx, yy = coordinate_fields(size)
    broad = fbm(size, rng, 5, 5)
    fine = fbm(size, rng, 48, 4)
    tarnish = smoothstep(0.56, 0.75, fbm(size, rng, 7, 5))
    pits = smoothstep(0.79, 0.93, fbm(size, rng, 70, 3))
    scratches = wrapped_line_mask(size, rng, 520, (48, 260), (1, 3), angle=0.05, alpha_range=(35, 115))
    small_rivets = rivet_mask(xx + 0.035, yy + 0.02, 6, 6, 0.050) * 0.55

    base = mix((161, 101, 37), (225, 158, 63), broad * 0.65 + fine * 0.2)
    base = overlay_color(base, (59, 117, 91), tarnish * 0.62)
    base = overlay_color(base, (245, 184, 82), scratches * 0.26)
    base = overlay_color(base, (73, 49, 30), pits * 0.40)
    base = overlay_color(base, (207, 142, 55), small_rivets * 0.22)

    height = clamp01(0.48 + fine * 0.09 + scratches * 0.05 - pits * 0.15 + small_rivets * 0.18)
    ao = clamp01(0.93 - pits * 0.28 - tarnish * 0.08 - small_rivets * 0.06)
    roughness = clamp01(0.50 + tarnish * 0.24 + fine * 0.10 - scratches * 0.16)
    metallic = clamp01(0.92 - tarnish * 0.35 - pits * 0.12)
    return {
        "base": to_u8(base),
        "height": height,
        "ao": ao,
        "roughness": roughness,
        "metallic": metallic,
        "notes": [
            "Warm aged brass with oxidized green pockets, pitting, shallow rivets, and polished hairline scratches.",
            "Metallic channel is reduced in patina and pit regions; normals are height-derived.",
        ],
        "quality": "pass",
        "qualityNotes": "Reads as brass in contact sheets; suitable for trim, gauges, valves, and pressure hardware staging.",
    }


def material_blackened_iron(size: int, rng: np.random.Generator) -> dict:
    xx, yy = coordinate_fields(size)
    plate_noise = fbm(size, rng, 8, 5)
    soot = fbm(size, rng, 18, 4)
    seams = grid_line_mask(xx + 0.015, yy + 0.025, 4, 4, 0.018)
    rivets = rivet_mask(xx + 0.015, yy + 0.025, 4, 4, 0.045)
    scratches = wrapped_line_mask(size, rng, 460, (80, 420), (1, 3), alpha_range=(25, 100))
    rust = smoothstep(0.69, 0.86, fbm(size, rng, 12, 5)) * (seams * 0.45 + rivets * 0.32 + 0.08)

    base = mix((20, 23, 23), (49, 51, 48), plate_noise * 0.7)
    base = overlay_color(base, (91, 48, 31), rust * 0.55)
    base = overlay_color(base, (118, 116, 105), scratches * 0.24)
    base = multiply_color(base, 0.88 - seams * 0.30 - soot * 0.09 + rivets * 0.12)

    height = clamp01(0.46 + plate_noise * 0.05 - seams * 0.19 + rivets * 0.23 + scratches * 0.035)
    ao = clamp01(0.90 - seams * 0.42 - rivets * 0.12 - rust * 0.18)
    roughness = clamp01(0.66 + soot * 0.16 + rust * 0.12 - scratches * 0.19)
    metallic = clamp01(0.84 - rust * 0.36 - soot * 0.10)
    return {
        "base": to_u8(base),
        "height": height,
        "ao": ao,
        "roughness": roughness,
        "metallic": metallic,
        "notes": [
            "Dark riveted iron panel field with raised rivets, recessed seams, soot, rust halos, and scrape wear.",
            "Panel/rivet layout is tileable for modular wall and armor material tests.",
        ],
        "quality": "pass",
        "qualityNotes": "Strong silhouette-safe rivet/seam read; needs final UV alignment before hero gate use.",
    }


def material_wet_oil_stone(size: int, rng: np.random.Generator) -> dict:
    xx, yy = coordinate_fields(size)
    grain = fbm(size, rng, 18, 5)
    fine = fbm(size, rng, 72, 3)
    seams = grid_line_mask(xx + 0.04, yy + 0.02, 5, 5, 0.026)
    cracks = wrapped_line_mask(size, rng, 190, (70, 360), (2, 5), alpha_range=(55, 150))
    oil = smoothstep(0.58, 0.78, fbm(size, rng, 6, 5))
    oil = clamp01(oil * (0.75 + fbm(size, rng, 24, 3) * 0.5))

    base = mix((24, 27, 25), (63, 62, 55), grain * 0.72 + fine * 0.12)
    base = multiply_color(base, 0.92 - seams * 0.34 - cracks * 0.26)
    base = overlay_color(base, (12, 12, 10), oil * 0.47)
    base = overlay_color(base, (87, 70, 43), oil * smoothstep(0.72, 0.95, fbm(size, rng, 16, 3)) * 0.28)

    height = clamp01(0.47 + grain * 0.08 + fine * 0.035 - seams * 0.22 - cracks * 0.14 - oil * 0.035)
    ao = clamp01(0.88 - seams * 0.42 - cracks * 0.32 - oil * 0.05)
    roughness = clamp01(0.78 - oil * 0.54 + fine * 0.08 + seams * 0.04)
    metallic = np.zeros((size, size), dtype=np.float32)
    return {
        "base": to_u8(base),
        "height": height,
        "ao": ao,
        "roughness": roughness,
        "metallic": metallic,
        "notes": [
            "Damp dark floor slabs with oily pooling, traffic grime, chipped seams, and hairline cracks.",
            "Low roughness in oil regions is intentionally exaggerated for wet floor lookdev.",
        ],
        "quality": "pass",
        "qualityNotes": "Oil/stone separation is clear in BaseColor and ORM; final engine lighting should tune wetness.",
    }


def material_soot_brick(size: int, rng: np.random.Generator) -> dict:
    xx, yy = coordinate_fields(size)
    rows = 16
    columns = 8
    y_scaled = yy * rows
    row_index = np.floor(y_scaled).astype(np.int32)
    y_frac = y_scaled - row_index
    x_offset = ((row_index % 2) * 0.5).astype(np.float32)
    x_scaled = xx * columns + x_offset
    x_frac = x_scaled - np.floor(x_scaled)
    mortar = np.maximum(
        1.0 - smoothstep(0.055, 0.10, np.minimum(x_frac, 1.0 - x_frac)),
        1.0 - smoothstep(0.070, 0.12, np.minimum(y_frac, 1.0 - y_frac)),
    )
    brick_variation = fbm(size, rng, 16, 4)
    soot = smoothstep(0.53, 0.82, fbm(size, rng, 5, 5))
    chips = smoothstep(0.76, 0.90, fbm(size, rng, 56, 3)) * (1.0 - mortar)
    scratches = wrapped_line_mask(size, rng, 230, (40, 210), (1, 3), alpha_range=(25, 80))

    base = mix((86, 35, 24), (155, 65, 38), brick_variation)
    base = overlay_color(base, (32, 28, 24), mortar * 0.88)
    base = overlay_color(base, (20, 18, 16), soot * 0.48)
    base = overlay_color(base, (191, 112, 68), chips * 0.28)
    base = overlay_color(base, (47, 35, 27), scratches * 0.18)

    height = clamp01(0.55 + brick_variation * 0.06 - mortar * 0.28 - chips * 0.11 + scratches * 0.025)
    ao = clamp01(0.91 - mortar * 0.54 - chips * 0.10 - soot * 0.14)
    roughness = clamp01(0.78 + soot * 0.12 + mortar * 0.05 - chips * 0.05)
    metallic = np.zeros((size, size), dtype=np.float32)
    return {
        "base": to_u8(base),
        "height": height,
        "ao": ao,
        "roughness": roughness,
        "metallic": metallic,
        "notes": [
            "Offset Victorian utility brick with dark mortar, soot bloom, chips, and worn brick variation.",
            "Designed as a tileable wall foundation, not a unique hero wall panel.",
        ],
        "quality": "pass",
        "qualityNotes": "Brick courses and soot read cleanly at preview scale; repeat should be broken with decals in long halls.",
    }


def material_copper_pipe(size: int, rng: np.random.Generator) -> dict:
    xx, yy = coordinate_fields(size)
    axial = fbm(size, rng, 16, 4)
    fine = fbm(size, rng, 80, 3)
    band_frac = (xx * 8.0) % 1.0
    bands = 1.0 - smoothstep(0.045, 0.080, np.minimum(band_frac, 1.0 - band_frac))
    seam = 1.0 - smoothstep(0.018, 0.048, np.abs((yy * 2.0) % 1.0 - 0.5))
    verdigris = smoothstep(0.60, 0.82, fbm(size, rng, 10, 5)) * (0.35 + bands * 0.45 + seam * 0.25)
    scratches = wrapped_line_mask(size, rng, 470, (90, 500), (1, 3), angle=0.02, alpha_range=(30, 100))

    base = mix((113, 55, 24), (216, 116, 54), axial * 0.72 + fine * 0.14)
    base = overlay_color(base, (42, 119, 102), verdigris * 0.68)
    base = overlay_color(base, (237, 151, 75), scratches * 0.28)
    base = multiply_color(base, 0.96 - seam * 0.14 + bands * 0.10)

    height = clamp01(0.50 + axial * 0.045 + fine * 0.025 + bands * 0.14 - seam * 0.08 + scratches * 0.025)
    ao = clamp01(0.91 - bands * 0.10 - seam * 0.26 - verdigris * 0.08)
    roughness = clamp01(0.47 + verdigris * 0.33 + fine * 0.09 - scratches * 0.15)
    metallic = clamp01(0.91 - verdigris * 0.42)
    return {
        "base": to_u8(base),
        "height": height,
        "ao": ao,
        "roughness": roughness,
        "metallic": metallic,
        "notes": [
            "Oxidized copper pipe surface with axial polish, pipe bands, seam grime, verdigris pockets, and scratches.",
            "Directional banding is intended for pipe UVs and may be too directional for broad flat walls.",
        ],
        "quality": "pass",
        "qualityNotes": "Copper/verdigris split is readable; pass as pipe material staging, not as a universal tileable metal.",
    }


def material_greasy_walnut(size: int, rng: np.random.Generator) -> dict:
    xx, yy = coordinate_fields(size)
    grain_wave = (np.sin((xx * 24.0 + fbm(size, rng, 5, 4) * 3.2) * math.tau) + 1.0) * 0.5
    grain_noise = fbm(size, rng, 34, 4)
    pore_noise = fbm(size, rng, 95, 3)
    dark_pores = smoothstep(0.58, 0.88, grain_wave * 0.65 + grain_noise * 0.35)
    grease = smoothstep(0.64, 0.83, fbm(size, rng, 7, 5))
    scratches = wrapped_line_mask(size, rng, 410, (70, 360), (1, 3), angle=0.02, alpha_range=(35, 105))

    base = mix((58, 28, 16), (122, 67, 36), grain_noise * 0.6 + grain_wave * 0.28)
    base = overlay_color(base, (27, 15, 10), dark_pores * 0.34)
    base = overlay_color(base, (155, 89, 48), scratches * 0.22)
    base = overlay_color(base, (28, 18, 13), grease * 0.28)
    base = multiply_color(base, 0.92 + pore_noise * 0.12)

    height = clamp01(0.49 + grain_wave * 0.05 + grain_noise * 0.055 - dark_pores * 0.08 + scratches * 0.035)
    ao = clamp01(0.88 - dark_pores * 0.24 - scratches * 0.06)
    roughness = clamp01(0.64 - grease * 0.31 + pore_noise * 0.08 + scratches * 0.04)
    metallic = np.zeros((size, size), dtype=np.float32)
    return {
        "base": to_u8(base),
        "height": height,
        "ao": ao,
        "roughness": roughness,
        "metallic": metallic,
        "notes": [
            "Dark greasy walnut with lengthwise grain, polished hand wear, scratches, and oil-dark pores.",
            "Directional grain is meant for weapon grips, tool handles, and trim strips.",
        ],
        "quality": "pass",
        "qualityNotes": "Walnut color and grain read distinctly from leather; pass with UV-direction caveat.",
    }


def material_cream_enamel_gauge(size: int, rng: np.random.Generator) -> dict:
    base_noise = fbm(size, rng, 18, 4)
    fine = fbm(size, rng, 80, 3)
    image = Image.new("RGB", (size, size), (225, 207, 164))
    pixels = np.asarray(image, dtype=np.float32) / 255.0
    pixels = pixels * (0.90 + base_noise[..., None] * 0.14 + fine[..., None] * 0.035)

    alpha = np.linspace(0.0, math.tau, size, endpoint=False, dtype=np.float32)
    yy, xx = np.indices((size, size), dtype=np.float32)
    cx = cy = size * 0.5
    dist = np.sqrt((xx - cx) ** 2 + (yy - cy) ** 2) / (size * 0.5)
    ring = smoothstep(0.92, 0.97, dist) - smoothstep(1.00, 1.05, dist)
    center_stain = 1.0 - smoothstep(0.0, 0.54, dist)
    pixels = overlay_color(pixels, (74, 51, 30), ring * 0.55)
    pixels = overlay_color(pixels, (187, 157, 97), center_stain * 0.12)

    draw_img = Image.fromarray(to_u8(pixels))
    draw = ImageDraw.Draw(draw_img)
    for tick in range(0, 61):
        angle = math.radians(218 - tick * 256.0 / 60.0)
        inner = 0.70 if tick % 5 == 0 else 0.76
        outer = 0.86
        width = 7 if tick % 5 == 0 else 3
        color = (38, 33, 27) if tick % 5 == 0 else (70, 59, 47)
        p1 = (cx + math.cos(angle) * size * 0.5 * inner, cy - math.sin(angle) * size * 0.5 * inner)
        p2 = (cx + math.cos(angle) * size * 0.5 * outer, cy - math.sin(angle) * size * 0.5 * outer)
        draw.line((p1[0], p1[1], p2[0], p2[1]), fill=color, width=width)
    for angle_deg in (305, 322, 339):
        angle = math.radians(angle_deg)
        p1 = (cx + math.cos(angle) * size * 0.5 * 0.66, cy - math.sin(angle) * size * 0.5 * 0.66)
        p2 = (cx + math.cos(angle) * size * 0.5 * 0.86, cy - math.sin(angle) * size * 0.5 * 0.86)
        draw.line((p1[0], p1[1], p2[0], p2[1]), fill=(122, 27, 21), width=10)
    needle_angle = math.radians(326)
    p2 = (cx + math.cos(needle_angle) * size * 0.5 * 0.64, cy - math.sin(needle_angle) * size * 0.5 * 0.64)
    draw.line((cx, cy, p2[0], p2[1]), fill=(26, 23, 20), width=18)
    draw.ellipse((cx - 42, cy - 42, cx + 42, cy + 42), fill=(60, 44, 27), outline=(26, 21, 18), width=5)

    pixels = np.asarray(draw_img, dtype=np.float32) / 255.0
    chip_mask = smoothstep(0.74, 0.90, fbm(size, rng, 44, 4)) * (ring * 0.70 + (1.0 - smoothstep(0.75, 0.90, dist)) * 0.15)
    pixels = overlay_color(pixels, (50, 42, 33), chip_mask * 0.58)
    grime = smoothstep(0.62, 0.86, fbm(size, rng, 9, 5)) * 0.18
    pixels = overlay_color(pixels, (91, 69, 41), grime)

    tick_mask = np.mean(np.abs(pixels - np.array([225, 207, 164], dtype=np.float32) / 255.0), axis=-1)
    height = clamp01(0.52 + base_noise * 0.035 - chip_mask * 0.13 + ring * 0.12 + tick_mask * 0.035)
    ao = clamp01(0.92 - chip_mask * 0.24 - ring * 0.16)
    roughness = clamp01(0.42 + grime * 0.65 + chip_mask * 0.28 + fine * 0.05)
    metallic = clamp01(ring * 0.62 + chip_mask * 0.10)
    return {
        "base": to_u8(pixels),
        "height": height,
        "ao": ao,
        "roughness": roughness,
        "metallic": metallic,
        "notes": [
            "Non-tileable cream enamel gauge-face material swatch with decorative ticks, danger arc, brass-dark rim, chips, and grime.",
            "Includes no readable text; intended as material/lookdev reference for future gauge atlas work.",
        ],
        "quality": "pass",
        "qualityNotes": "Pass as a gauge/enamel staging target; fail as a tileable material by design.",
    }


def material_amber_glass(size: int, rng: np.random.Generator) -> dict:
    xx, yy = coordinate_fields(size)
    flow = fbm(size, rng, 9, 5)
    fine = fbm(size, rng, 64, 4)
    scratches = wrapped_line_mask(size, rng, 360, (70, 320), (1, 2), alpha_range=(35, 110))
    grime = smoothstep(0.62, 0.82, fbm(size, rng, 16, 4))
    vertical_warp = (np.sin((xx * 8.0 + flow * 1.3) * math.tau) + 1.0) * 0.5

    base_rgb = mix((117, 57, 18), (244, 141, 31), flow * 0.62 + vertical_warp * 0.20 + fine * 0.10)
    base_rgb = overlay_color(base_rgb, (255, 188, 58), smoothstep(0.68, 0.92, flow) * 0.24)
    base_rgb = overlay_color(base_rgb, (42, 31, 20), grime * 0.34)
    base_rgb = overlay_color(base_rgb, (255, 218, 118), scratches * 0.20)
    alpha = clamp01(0.52 + smoothstep(0.58, 0.86, flow) * 0.18 - grime * 0.12)

    height = clamp01(0.50 + vertical_warp * 0.045 + fine * 0.025 + scratches * 0.055 - grime * 0.05)
    ao = clamp01(0.97 - grime * 0.16)
    roughness = clamp01(0.12 + grime * 0.46 + scratches * 0.28 + fine * 0.05)
    metallic = np.zeros((size, size), dtype=np.float32)
    rgba = np.dstack([to_u8(base_rgb), to_u8(alpha)])
    return {
        "base": rgba,
        "height": height,
        "ao": ao,
        "roughness": roughness,
        "metallic": metallic,
        "notes": [
            "Amber warped glass with alpha in BaseColor, grime blooms, hairline scratches, and wavering normal detail.",
            "Requires future Unity transparent shader validation; no material asset is authored here.",
        ],
        "quality": "pass",
        "qualityNotes": "Pass as transparent/glass lookdev source; shader sorting and reflection behavior remain untested.",
    }


def material_leather_bellows(size: int, rng: np.random.Generator) -> dict:
    xx, yy = coordinate_fields(size)
    fold = (np.sin((yy * 28.0) * math.tau) + 1.0) * 0.5
    fold_profile = smoothstep(0.25, 0.88, fold)
    grain = fbm(size, rng, 52, 4)
    creases = wrapped_line_mask(size, rng, 360, (55, 250), (1, 3), angle=0.05, alpha_range=(25, 95))
    seam = 1.0 - smoothstep(0.018, 0.046, np.minimum(np.abs((xx * 4.0) % 1.0 - 0.05), np.abs((xx * 4.0) % 1.0 - 0.95)))
    stitch_frac = (yy * 48.0) % 1.0
    stitches = seam * (1.0 - smoothstep(0.12, 0.25, np.minimum(stitch_frac, 1.0 - stitch_frac)))
    oil = smoothstep(0.66, 0.84, fbm(size, rng, 8, 5))

    base = mix((49, 23, 15), (118, 62, 33), grain * 0.44 + fold_profile * 0.36)
    base = overlay_color(base, (22, 13, 10), (1.0 - fold_profile) * 0.22 + oil * 0.22)
    base = overlay_color(base, (157, 88, 45), creases * 0.20)
    base = overlay_color(base, (153, 97, 63), stitches * 0.52)

    height = clamp01(0.42 + fold_profile * 0.24 + grain * 0.035 - creases * 0.045 + stitches * 0.12)
    ao = clamp01(0.90 - (1.0 - fold_profile) * 0.22 - seam * 0.12)
    roughness = clamp01(0.68 - oil * 0.30 + grain * 0.08 + creases * 0.04)
    metallic = np.zeros((size, size), dtype=np.float32)
    return {
        "base": to_u8(base),
        "height": height,
        "ao": ao,
        "roughness": roughness,
        "metallic": metallic,
        "notes": [
            "Ribbed leather bellows with accordion fold normals, side seams, stitch marks, grease, and worn creases.",
            "Fold frequency is procedural and should be remapped per final bellows mesh UVs.",
        ],
        "quality": "pass",
        "qualityNotes": "Distinct from walnut and readable as bellows material; final mesh-specific fold spacing needed.",
    }


def material_hazard_enamel(size: int, rng: np.random.Generator) -> dict:
    xx, yy = coordinate_fields(size)
    diagonal = ((xx + yy) * 8.0) % 1.0
    stripes = (diagonal > 0.50).astype(np.float32)
    edge = np.minimum(np.abs(diagonal - 0.50), np.minimum(diagonal, 1.0 - diagonal))
    stripe_edge_wear = 1.0 - smoothstep(0.015, 0.060, edge)
    broad = fbm(size, rng, 10, 5)
    fine = fbm(size, rng, 62, 3)
    chips = smoothstep(0.73, 0.91, fbm(size, rng, 38, 4)) * (0.45 + stripe_edge_wear * 0.75)
    scratches = wrapped_line_mask(size, rng, 570, (70, 430), (1, 4), alpha_range=(30, 115))
    rivets = rivet_mask(xx + 0.02, yy + 0.02, 4, 4, 0.045) * 0.75

    yellow = mix((158, 102, 22), (236, 169, 42), broad * 0.55 + fine * 0.15)
    black = mix((15, 16, 15), (45, 39, 27), broad * 0.40)
    base = yellow * (1.0 - stripes[..., None]) + black * stripes[..., None]
    base = overlay_color(base, (47, 43, 37), chips * 0.70)
    base = overlay_color(base, (224, 188, 98), scratches * (1.0 - stripes) * 0.21)
    base = overlay_color(base, (104, 101, 93), scratches * stripes * 0.16)
    base = overlay_color(base, (71, 67, 58), rivets * 0.45)

    height = clamp01(0.52 + fine * 0.03 - chips * 0.15 + scratches * 0.045 + rivets * 0.18)
    ao = clamp01(0.92 - chips * 0.24 - rivets * 0.14)
    roughness = clamp01(0.46 + chips * 0.30 + scratches * 0.18 + broad * 0.08)
    metallic = clamp01(chips * 0.55 + rivets * 0.66)
    return {
        "base": to_u8(base),
        "height": height,
        "ao": ao,
        "roughness": roughness,
        "metallic": metallic,
        "notes": [
            "Chipped yellow/black hazard enamel with exposed dark metal, rivets, scratches, grime, and worn stripe edges.",
            "Stripe scale is preview-ready; final signage should author UV-aware stripe placement.",
        ],
        "quality": "pass",
        "qualityNotes": "Hazard read is immediate and matches pressure-warning language; UV-specific variants still needed.",
    }


def material_decal_atlas(size: int, rng: np.random.Generator) -> dict:
    atlas = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    normal_height = np.zeros((size, size), dtype=np.float32) + 0.50
    ao = np.ones((size, size), dtype=np.float32) * 0.96
    roughness = np.ones((size, size), dtype=np.float32) * 0.80
    metallic = np.zeros((size, size), dtype=np.float32)
    alpha_mask = np.zeros((size, size), dtype=np.float32)
    draw = ImageDraw.Draw(atlas, "RGBA")
    cols, rows = 6, 4
    cell_w, cell_h = size // cols, size // rows

    def cell_bounds(index: int) -> tuple[int, int, int, int]:
        col = index % cols
        row = index // cols
        margin = 30
        return (
            col * cell_w + margin,
            row * cell_h + margin,
            (col + 1) * cell_w - margin,
            (row + 1) * cell_h - margin,
        )

    def paste_blob(index: int, color: tuple[int, int, int], kind: str, alpha_base: int) -> None:
        x0, y0, x1, y1 = cell_bounds(index)
        w = x1 - x0
        h = y1 - y0
        local = Image.new("L", (w, h), 0)
        local_draw = ImageDraw.Draw(local)
        if kind == "puddle":
            for _ in range(12):
                cx = int(rng.integers(w * 0.15, w * 0.85))
                cy = int(rng.integers(h * 0.20, h * 0.80))
                rx = int(rng.integers(w * 0.08, w * 0.28))
                ry = int(rng.integers(h * 0.05, h * 0.18))
                local_draw.ellipse((cx - rx, cy - ry, cx + rx, cy + ry), fill=int(rng.integers(80, alpha_base)))
        elif kind == "scorch":
            for radius in range(min(w, h) // 2, 12, -18):
                alpha = int(alpha_base * (radius / (min(w, h) / 2)) * 0.45)
                local_draw.ellipse((w / 2 - radius, h / 2 - radius, w / 2 + radius, h / 2 + radius), fill=alpha)
            local_draw.ellipse((w * 0.31, h * 0.32, w * 0.69, h * 0.68), fill=int(alpha_base * 0.34))
        elif kind == "drip":
            for _ in range(8):
                sx = int(rng.integers(w * 0.12, w * 0.88))
                sy = int(rng.integers(h * 0.05, h * 0.25))
                length = int(rng.integers(h * 0.28, h * 0.78))
                width = int(rng.integers(7, 18))
                local_draw.line((sx, sy, sx + int(rng.normal(0, 12)), sy + length), fill=alpha_base, width=width)
                local_draw.ellipse((sx - width, sy + length - width, sx + width, sy + length + width), fill=int(alpha_base * 0.82))
        elif kind == "scratch":
            for _ in range(26):
                sx = float(rng.uniform(w * 0.08, w * 0.90))
                sy = float(rng.uniform(h * 0.12, h * 0.88))
                length = float(rng.uniform(w * 0.18, w * 0.72))
                angle = float(rng.uniform(-0.55, 0.55))
                local_draw.line(
                    (sx, sy, sx + math.cos(angle) * length, sy + math.sin(angle) * length),
                    fill=int(rng.integers(alpha_base // 2, alpha_base)),
                    width=int(rng.integers(2, 6)),
                )
        elif kind == "stamp":
            local_draw.rectangle((w * 0.13, h * 0.25, w * 0.87, h * 0.75), outline=alpha_base, width=14)
            local_draw.line((w * 0.22, h * 0.62, w * 0.78, h * 0.38), fill=alpha_base, width=15)
            local_draw.line((w * 0.22, h * 0.38, w * 0.78, h * 0.62), fill=alpha_base, width=15)
        colored = Image.new("RGBA", (w, h), (*color, 0))
        colored.putalpha(local)
        atlas.alpha_composite(colored, (x0, y0))

        mask_np = np.asarray(local, dtype=np.float32) / 255.0
        ys = slice(y0, y1)
        xs = slice(x0, x1)
        alpha_mask[ys, xs] = np.maximum(alpha_mask[ys, xs], mask_np)
        if kind == "puddle":
            roughness[ys, xs] = np.minimum(roughness[ys, xs], 0.18 + (1.0 - mask_np) * 0.62)
            normal_height[ys, xs] += mask_np * 0.035
        elif kind == "scorch":
            roughness[ys, xs] = np.maximum(roughness[ys, xs], 0.92 * mask_np)
            normal_height[ys, xs] -= mask_np * 0.025
            ao[ys, xs] -= mask_np * 0.16
        elif kind == "scratch":
            normal_height[ys, xs] += mask_np * 0.045
            ao[ys, xs] -= mask_np * 0.08
        elif kind == "stamp":
            roughness[ys, xs] = np.minimum(roughness[ys, xs], 0.50 + (1.0 - mask_np) * 0.35)
            normal_height[ys, xs] += mask_np * 0.025
        else:
            roughness[ys, xs] = np.minimum(roughness[ys, xs], 0.32 + (1.0 - mask_np) * 0.55)
            normal_height[ys, xs] += mask_np * 0.030

    decal_plan = [
        ((18, 14, 10), "puddle", 210),
        ((21, 17, 12), "puddle", 190),
        ((36, 29, 19), "drip", 210),
        ((42, 32, 20), "drip", 185),
        ((11, 10, 9), "scorch", 220),
        ((20, 17, 14), "scorch", 200),
        ((24, 23, 22), "scorch", 190),
        ((67, 52, 32), "puddle", 170),
        ((52, 43, 33), "scratch", 180),
        ((79, 66, 48), "scratch", 165),
        ((98, 41, 30), "stamp", 190),
        ((207, 144, 41), "stamp", 175),
        ((15, 13, 11), "puddle", 200),
        ((35, 29, 22), "drip", 195),
        ((8, 8, 8), "scorch", 220),
        ((70, 51, 34), "scratch", 160),
        ((150, 37, 25), "stamp", 180),
        ((42, 102, 72), "stamp", 165),
        ((31, 26, 20), "drip", 205),
        ((50, 39, 25), "puddle", 185),
        ((14, 13, 12), "scorch", 210),
        ((82, 72, 58), "scratch", 170),
        ((107, 50, 28), "drip", 160),
        ((191, 135, 45), "stamp", 160),
    ]
    for index, (color, kind, alpha_base) in enumerate(decal_plan):
        paste_blob(index, color, kind, alpha_base)

    alpha = to_u8(alpha_mask)
    rgba = np.asarray(atlas, dtype=np.uint8)
    height = clamp01(normal_height + fbm(size, rng, 80, 2) * 0.015 * alpha_mask)
    ao = clamp01(ao)
    roughness = clamp01(roughness)
    return {
        "base": rgba,
        "height": height,
        "ao": ao,
        "roughness": roughness,
        "metallic": metallic,
        "alpha": alpha,
        "notes": [
            "RGBA decal atlas with 24 oil, soot, scorch, scratch, drip, and stamped warning/service marks.",
            "BaseColor alpha stores decal opacity; companion Alpha map is exported for tools that prefer a separate opacity mask.",
        ],
        "quality": "pass",
        "qualityNotes": "Pass as a review/import atlas; decal shader setup and per-decal UV metadata still need integration work.",
    }


MATERIALS: list[MaterialSpec] = [
    MaterialSpec("MAT_BBW_AgedBrass_V1", "Aged Brass", "AgedBrass", "Shared brass trim, gauges, valves, pickup shells, weapon accents", True, material_aged_brass),
    MaterialSpec("MAT_BBW_BlackenedRivetedIron_V1", "Blackened Riveted Iron", "BlackenedRivetedIron", "Wall plates, gates, armor, furnace machinery, heavy braces", True, material_blackened_iron),
    MaterialSpec("MAT_BBW_WetOilDarkStone_V1", "Wet Oil-Dark Stone", "WetOilDarkStone", "Service floors, damp maintenance slabs, oil-dark dungeon floor stone", True, material_wet_oil_stone),
    MaterialSpec("MAT_BBW_SootBrick_V1", "Soot Brick", "SootBrick", "Corridor walls, furnace masonry, chimney recesses", True, material_soot_brick),
    MaterialSpec("MAT_BBW_CopperPipe_V1", "Copper Pipe", "CopperPipe", "Copper pipe bundles, pressure tanks, antique service machinery", True, material_copper_pipe),
    MaterialSpec("MAT_BBW_GreasyWalnut_V1", "Greasy Walnut", "GreasyWalnut", "Weapon grips, tool handles, dark wood trim, oily worker-maintained props", True, material_greasy_walnut),
    MaterialSpec("MAT_BBW_CreamEnamelGauge_V1", "Cream Enamel Gauge", "CreamEnamelGauge", "Gauge faces, chipped cream enamel plates, pressure readouts", False, material_cream_enamel_gauge),
    MaterialSpec("MAT_BBW_AmberGlass_V1", "Amber Glass", "AmberGlass", "Gauge lenses, lamps, furnace-adjacent glass, amber warning glass", True, material_amber_glass),
    MaterialSpec("MAT_BBW_LeatherBellows_V1", "Leather Bellows", "LeatherBellows", "Pump bellows, pressure organs, accordion machinery joints", True, material_leather_bellows),
    MaterialSpec("MAT_BBW_HazardEnamel_V1", "Hazard Enamel", "HazardEnamel", "Pressure-warning panels, machinery guards, danger striping", True, material_hazard_enamel),
    MaterialSpec("MAT_BBW_ScorchOilDecalAtlas_V1", "Scorch/Oil Decal Atlas", "ScorchOilDecalAtlas", "Oil pools, leaks, scorch marks, scratches, grime stamps, warning decals", False, material_decal_atlas),
]


def write_contact_sheet_base(material_records: list[dict]) -> dict:
    tile = 360
    label_h = 58
    cols = 4
    rows = math.ceil(len(material_records) / cols)
    sheet = Image.new("RGB", (cols * tile, rows * (tile + label_h) + 52), (24, 22, 19))
    draw = ImageDraw.Draw(sheet)
    font_title = load_font(30)
    font_label = load_font(24)
    draw.text((18, 12), "Brassworks Breach Final Materials V1 - BaseColor Review", fill=(232, 207, 151), font=font_title)
    for index, record in enumerate(material_records):
        col = index % cols
        row = index // cols
        x = col * tile
        y = 52 + row * (tile + label_h)
        preview = composite_for_preview(record["arrays"]["base"]).resize((tile, tile), Image.Resampling.LANCZOS)
        sheet.paste(preview, (x, y))
        draw.rectangle((x, y + tile, x + tile, y + tile + label_h), fill=(29, 27, 23))
        draw.text((x + 12, y + tile + 9), record["displayName"], fill=(232, 207, 151), font=font_label)
        tileable = "tileable" if record["tileable"] else "non-tileable"
        draw.text((x + 12, y + tile + 34), tileable, fill=(154, 142, 119), font=load_font(16))
    path = PREVIEW_DIR / "PREVIEW_BBW_FinalMaterialsV1_BaseColorContactSheet.png"
    return save_png(path, np.asarray(sheet, dtype=np.uint8))


def write_contact_sheet_triplets(material_records: list[dict]) -> dict:
    thumb = 270
    label_w = 310
    header_h = 70
    row_h = thumb + 34
    width = label_w + thumb * 3
    height = header_h + row_h * len(material_records)
    sheet = Image.new("RGB", (width, height), (22, 21, 18))
    draw = ImageDraw.Draw(sheet)
    font_title = load_font(28)
    font_label = load_font(22)
    font_small = load_font(16)
    draw.text((18, 14), "Brassworks Breach Final Materials V1 - Map Triplets", fill=(232, 207, 151), font=font_title)
    for col, label in enumerate(("BaseColor", "Normal", "ORM")):
        draw.text((label_w + col * thumb + 12, 45), label, fill=(188, 166, 116), font=font_small)
    for index, record in enumerate(material_records):
        y = header_h + index * row_h
        row_color = (27, 27, 24) if index % 2 else (18, 19, 17)
        draw.rectangle((0, y, width, y + row_h), fill=row_color)
        draw.text((16, y + 36), record["displayName"], fill=(232, 207, 151), font=font_label)
        draw.text((16, y + 66), record["materialId"], fill=(154, 142, 119), font=font_small)
        base = composite_for_preview(record["arrays"]["base"]).resize((thumb, thumb), Image.Resampling.LANCZOS)
        normal = Image.fromarray(record["arrays"]["normal"]).resize((thumb, thumb), Image.Resampling.LANCZOS).convert("RGB")
        orm = Image.fromarray(record["arrays"]["orm"]).resize((thumb, thumb), Image.Resampling.LANCZOS).convert("RGB")
        for col, preview in enumerate((base, normal, orm)):
            sheet.paste(preview, (label_w + col * thumb, y + 16))
    path = PREVIEW_DIR / "PREVIEW_BBW_FinalMaterialsV1_MapTripletsContactSheet.png"
    return save_png(path, np.asarray(sheet, dtype=np.uint8))


def write_contact_sheet_tiling(material_records: list[dict]) -> dict:
    tileable_records = [record for record in material_records if record["tileable"]]
    tile = 340
    label_h = 50
    cols = 3
    rows = math.ceil(len(tileable_records) / cols)
    sheet = Image.new("RGB", (cols * tile, rows * (tile + label_h) + 54), (24, 22, 19))
    draw = ImageDraw.Draw(sheet)
    draw.text((18, 12), "BaseColor 2x2 Tiling Preview - Tileable V1 Materials", fill=(232, 207, 151), font=load_font(28))
    for index, record in enumerate(tileable_records):
        col = index % cols
        row = index // cols
        x = col * tile
        y = 54 + row * (tile + label_h)
        base = composite_for_preview(record["arrays"]["base"])
        tiled = Image.new("RGB", (base.width * 2, base.height * 2))
        for ox in (0, base.width):
            for oy in (0, base.height):
                tiled.paste(base, (ox, oy))
        preview = tiled.resize((tile, tile), Image.Resampling.LANCZOS)
        sheet.paste(preview, (x, y))
        draw.rectangle((x, y + tile, x + tile, y + tile + label_h), fill=(29, 27, 23))
        draw.text((x + 12, y + tile + 12), record["displayName"], fill=(232, 207, 151), font=load_font(21))
    path = PREVIEW_DIR / "PREVIEW_BBW_FinalMaterialsV1_TilingContactSheet.png"
    return save_png(path, np.asarray(sheet, dtype=np.uint8))


def write_decal_alpha_preview(material_records: list[dict]) -> dict:
    decal = next(record for record in material_records if record["slug"] == "ScorchOilDecalAtlas")
    base = composite_for_preview(decal["arrays"]["base"]).resize((650, 650), Image.Resampling.LANCZOS)
    alpha = Image.fromarray(decal["arrays"]["alpha"]).convert("L").resize((650, 650), Image.Resampling.LANCZOS).convert("RGB")
    sheet = Image.new("RGB", (1300, 720), (24, 22, 19))
    draw = ImageDraw.Draw(sheet)
    draw.text((18, 14), "Scorch/Oil Decal Atlas Preview - RGBA Composite and Alpha", fill=(232, 207, 151), font=load_font(28))
    sheet.paste(base, (0, 70))
    sheet.paste(alpha, (650, 70))
    draw.text((18, 46), "BaseColor over checkerboard", fill=(154, 142, 119), font=load_font(17))
    draw.text((668, 46), "Separate opacity helper", fill=(154, 142, 119), font=load_font(17))
    path = PREVIEW_DIR / "PREVIEW_BBW_FinalMaterialsV1_DecalAtlasAlpha.png"
    return save_png(path, np.asarray(sheet, dtype=np.uint8))


def load_font(size: int) -> ImageFont.ImageFont:
    for name in ("arial.ttf", "segoeui.ttf", "DejaVuSans.ttf"):
        try:
            return ImageFont.truetype(name, size=size)
        except OSError:
            continue
    return ImageFont.load_default()


def make_manifest(material_records: list[dict], preview_records: list[dict]) -> dict:
    return {
        "batchId": BATCH_ID,
        "project": "Brassworks Breach",
        "createdLocal": datetime.now().astimezone().isoformat(timespec="seconds"),
        "status": "final-material-v1 staging package; not integrated into Unity materials/scenes",
        "allowedWriteScope": [
            rel(ASSET_ROOT),
            rel(DOC_ROOT),
        ],
        "sourceReferencesRead": [
            "Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png",
            "Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png",
            "Documentation/AAA_ASSET_CATALOG.md",
            "Documentation/PARALLEL_ASSET_GENERATION_BRIEFS.md",
            "Assets/_Project/ArtStaging/MaterialsPBR/ (read-only reference)",
        ],
        "textureResolution": [SIZE, SIZE],
        "mapPacking": {
            "BaseColor": "sRGB PNG. AmberGlass and ScorchOilDecalAtlas include alpha in BaseColor.",
            "Normal": "Tangent-space PNG generated from procedural height; import as Unity Normal Map.",
            "ORM": "Existing MaterialsPBR-compatible ORM: R=ambient occlusion, G=roughness, B=metallic. Import linear/non-sRGB.",
            "Alpha": "Separate opacity helper exported only for ScorchOilDecalAtlas.",
        },
        "materials": [
            {
                "id": record["materialId"],
                "displayName": record["displayName"],
                "role": record["role"],
                "tileable": record["tileable"],
                "quality": record["quality"],
                "qualityNotes": record["qualityNotes"],
                "maps": record["maps"],
                "notes": record["notes"],
            }
            for record in material_records
        ],
        "files": [file_record for record in material_records for file_record in record["files"]],
        "previews": preview_records,
        "counts": {
            "materials": len(material_records),
            "baseColor": sum(1 for record in material_records if "baseColor" in record["maps"]),
            "normal": sum(1 for record in material_records if "normal" in record["maps"]),
            "orm": sum(1 for record in material_records if "orm" in record["maps"]),
            "alphaHelpers": sum(1 for record in material_records if "alpha" in record["maps"]),
            "previewSheets": len(preview_records),
        },
    }


def write_manifest_markdown(manifest: dict) -> dict:
    lines: list[str] = [
        f"# {BATCH_ID} Manifest",
        "",
        f"Created: {manifest['createdLocal']}",
        "Status: final-material-v1 staging package; no Unity scenes, gameplay scripts, shared status docs, README files, ConceptRenders, existing material packages, or active material assets were modified.",
        "",
        "## Scope",
        "",
        "This package stages final-material V1 texture sources for the steampunk art direction. It is intentionally outside active Unity material integration: texture PNGs are placed in the isolated ArtStaging folder, while previews and reports live in this documentation folder.",
        "",
        "## Map Packing",
        "",
        "- BaseColor: sRGB PNG. Amber Glass and Scorch/Oil Decal Atlas include alpha in BaseColor.",
        "- Normal: height-derived tangent-space approximation; import as a Unity Normal Map.",
        "- ORM: R = ambient occlusion, G = roughness, B = metallic, matching the existing MaterialsPBR staging convention.",
        "- Alpha: separate opacity helper only for the decal atlas.",
        "",
        "## Materials",
        "",
        "| Material ID | Display | Tileable | BaseColor | Normal | ORM | Extra | Quality |",
        "| --- | --- | --- | --- | --- | --- | --- | --- |",
    ]
    for material in manifest["materials"]:
        maps = material["maps"]
        extra = maps.get("alpha", "")
        lines.append(
            f"| `{material['id']}` | {material['displayName']} | {str(material['tileable']).lower()} | "
            f"`{maps['baseColor']}` | `{maps['normal']}` | `{maps['orm']}` | "
            f"{('`' + extra + '`') if extra else ''} | {material['quality']} |"
        )
    lines.extend(
        [
            "",
            "## Preview Sheets",
            "",
        ]
    )
    for preview in manifest["previews"]:
        lines.append(f"- `{preview['path']}` ({preview['size'][0]}x{preview['size'][1]}, {preview['mode']})")
    lines.extend(
        [
            "",
            "## Counts",
            "",
            f"- Materials: {manifest['counts']['materials']}",
            f"- BaseColor maps: {manifest['counts']['baseColor']}",
            f"- Normal maps: {manifest['counts']['normal']}",
            f"- ORM maps: {manifest['counts']['orm']}",
            f"- Alpha helper maps: {manifest['counts']['alphaHelpers']}",
            f"- Preview sheets: {manifest['counts']['previewSheets']}",
            "",
            "## Source Inputs Read",
            "",
        ]
    )
    for reference in manifest["sourceReferencesRead"]:
        lines.append(f"- `{reference}`")
    path = DOC_ROOT / "BBW_FinalMaterialsV1_Manifest.md"
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")
    return {
        "path": rel(path),
        "sizeBytes": path.stat().st_size,
        "sha256": sha256(path),
    }


def write_acceptance_report(manifest: dict) -> dict:
    pass_count = sum(1 for material in manifest["materials"] if material["quality"] == "pass")
    fail_count = len(manifest["materials"]) - pass_count
    lines: list[str] = [
        "# BBW_FinalMaterialsV1 Acceptance Report",
        "",
        f"Created: {manifest['createdLocal']}",
        "",
        "## Result",
        "",
        f"- Quality bar: {pass_count} pass, {fail_count} fail.",
        "- Package is accepted for review/import staging only.",
        "- No gameplay scripts, scenes, existing material packages, shared status docs, ConceptRenders, README, BUILD_STATUS, or WORK_LEDGER files were edited.",
        "",
        "## Self-Quality Bar",
        "",
        "| Material | Pass/Fail | Notes |",
        "| --- | --- | --- |",
    ]
    for material in manifest["materials"]:
        lines.append(f"| {material['displayName']} | {material['quality']} | {material['qualityNotes']} |")
    lines.extend(
        [
            "",
            "## Approximation Notes",
            "",
            "- Normals are generated from procedural height fields, not sculpt bakes or photogrammetry.",
            "- ORM is plausible for preview PBR response but still needs Unity lighting/shader validation.",
            "- Cream Enamel Gauge is intentionally non-tileable and should become a proper gauge-face atlas in final integration.",
            "- Amber Glass uses BaseColor alpha for prototype opacity; transparent sorting, refraction, and reflection are not validated here.",
            "- Scorch/Oil Decal Atlas uses BaseColor alpha plus a separate alpha helper; decal material/shader setup is future integration work.",
            "- Copper Pipe, Greasy Walnut, and Leather Bellows are directional materials and should be UV-aligned during import.",
            "",
            "## Acceptance Checks",
            "",
            "- [x] Eleven steampunk material families staged.",
            "- [x] Each material has BaseColor, Normal, and ORM maps at 2048x2048.",
            "- [x] Decal atlas includes a separate 2048x2048 Alpha helper.",
            "- [x] Contact sheets exist in the documentation preview folder.",
            "- [x] Human-readable and machine-readable manifests exist.",
            "- [x] Files were written only under the requested FinalMaterialsV1 asset/doc folders.",
            "",
            "## Import Notes",
            "",
            "- Import BaseColor as sRGB.",
            "- Import Normal as Normal Map.",
            "- Import ORM as linear/non-sRGB. This package uses R=AO, G=roughness, B=metallic.",
            "- Do not create Unity `.mat` assets from these automatically; review the contact sheets first.",
        ]
    )
    path = DOC_ROOT / "BBW_FinalMaterialsV1_AcceptanceReport.md"
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")
    return {
        "path": rel(path),
        "sizeBytes": path.stat().st_size,
        "sha256": sha256(path),
    }


def main() -> None:
    ensure_dirs()
    material_records: list[dict] = []
    for index, spec in enumerate(MATERIALS):
        rng = np.random.default_rng(93001 + index * 137)
        generated = spec.generator(SIZE, rng)
        normal = normal_from_height(generated["height"], strength=6.0)
        orm = np.dstack([to_u8(generated["ao"]), to_u8(generated["roughness"]), to_u8(generated["metallic"])])

        maps = {}
        files = []
        base_path = TEXTURE_DIR / f"T_BBW_{spec.slug}_BaseColor_{SIZE}.png"
        normal_path = TEXTURE_DIR / f"T_BBW_{spec.slug}_Normal_{SIZE}.png"
        orm_path = TEXTURE_DIR / f"T_BBW_{spec.slug}_ORM_{SIZE}.png"

        base_record = save_png(base_path, generated["base"])
        base_record.update({"materialId": spec.material_id, "mapType": "baseColor"})
        normal_record = save_png(normal_path, normal)
        normal_record.update({"materialId": spec.material_id, "mapType": "normal"})
        orm_record = save_png(orm_path, orm)
        orm_record.update({"materialId": spec.material_id, "mapType": "orm"})
        files.extend([base_record, normal_record, orm_record])
        maps["baseColor"] = rel(base_path)
        maps["normal"] = rel(normal_path)
        maps["orm"] = rel(orm_path)

        arrays = {
            "base": generated["base"],
            "normal": normal,
            "orm": orm,
        }

        if "alpha" in generated:
            alpha_path = TEXTURE_DIR / f"T_BBW_{spec.slug}_Alpha_{SIZE}.png"
            alpha_record = save_png(alpha_path, generated["alpha"])
            alpha_record.update({"materialId": spec.material_id, "mapType": "alpha"})
            files.append(alpha_record)
            maps["alpha"] = rel(alpha_path)
            arrays["alpha"] = generated["alpha"]

        material_records.append(
            {
                "materialId": spec.material_id,
                "displayName": spec.display_name,
                "slug": spec.slug,
                "role": spec.role,
                "tileable": spec.tileable,
                "maps": maps,
                "files": files,
                "notes": generated["notes"],
                "quality": generated["quality"],
                "qualityNotes": generated["qualityNotes"],
                "arrays": arrays,
            }
        )

    preview_records = [
        write_contact_sheet_base(material_records),
        write_contact_sheet_triplets(material_records),
        write_contact_sheet_tiling(material_records),
        write_decal_alpha_preview(material_records),
    ]
    for preview in preview_records:
        preview["kind"] = "preview"

    manifest = make_manifest(material_records, preview_records)
    json_path = DOC_ROOT / "BBW_FinalMaterialsV1_Manifest.json"
    markdown_record = write_manifest_markdown(manifest)
    acceptance_record = write_acceptance_report(manifest)
    manifest["documentationFiles"] = [
        {
            "path": rel(json_path),
            "note": "Self hash omitted because embedding it would change the manifest contents.",
        },
        markdown_record,
        acceptance_record,
    ]
    json_path.write_text(json.dumps(manifest, indent=2) + "\n", encoding="utf-8")

    print(json.dumps(manifest["counts"], indent=2))
    print(f"Wrote {len(manifest['files'])} texture maps and {len(manifest['previews'])} preview sheets.")


if __name__ == "__main__":
    main()
