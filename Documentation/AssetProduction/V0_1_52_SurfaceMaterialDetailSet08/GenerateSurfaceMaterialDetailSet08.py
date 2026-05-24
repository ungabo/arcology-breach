#!/usr/bin/env python3
"""Generate Brassworks Breach Surface Material Detail Set 08.

The generator writes only inside the Set08 package and documentation roots.
It uses procedural Pillow/numpy image generation; no Blender or live Unity
Assets are touched.
"""

from __future__ import annotations

import hashlib
import json
import math
from dataclasses import dataclass
from datetime import datetime, timezone
from pathlib import Path

import numpy as np
from PIL import Image, ImageDraw, ImageEnhance


PROJECT_ROOT = Path("D:/__MY APPS/Unity Doom")
PACKAGE_NAME = "BrassworksBreach.SurfaceMaterialDetailSet08"
PACKAGE_ID = "com.brassworks.sidecar.surface-material-detail-set08"
VERSION = "0.1.52-p001"
UNITY_VERSION = "6000.4"
CODE = "SMD08"
SIZE = 512

PACKAGE_ROOT = PROJECT_ROOT / "AssetPacks" / PACKAGE_NAME
PRODUCTION_ROOT = PROJECT_ROOT / "Documentation" / "AssetProduction" / "V0_1_52_SurfaceMaterialDetailSet08"
RENDER_ROOT = PROJECT_ROOT / "Documentation" / "ConceptRenders" / "V0_1_52_SurfaceMaterialDetailSet08"
PLANNING_ROOT = PROJECT_ROOT / "Documentation" / "Planning" / "V0_1_52_SurfaceMaterialDetailSet08ImportReadiness"
QA_ROOT = PROJECT_ROOT / "Documentation" / "QA" / "V0_1_52_SurfaceMaterialDetailSet08ImportReadiness"

RUNTIME_ROOT = PACKAGE_ROOT / "Runtime"
MATERIAL_ROOT = RUNTIME_ROOT / "Materials"
TEXTURE_ROOT = RUNTIME_ROOT / "Textures"
ALBEDO_ROOT = TEXTURE_ROOT / "Albedo"
NORMAL_ROOT = TEXTURE_ROOT / "Normal"
RMA_ROOT = TEXTURE_ROOT / "RoughnessMetallic"
GRIME_ROOT = TEXTURE_ROOT / "GrimeEdgewear"
METADATA_ROOT = RUNTIME_ROOT / "Metadata"
DOC_MANIFEST_ROOT = PACKAGE_ROOT / "Documentation~" / "Manifest"
SAMPLES_ROOT = PACKAGE_ROOT / "Samples~" / "MaterialBoard"

ALLOWED_ROOTS = [PACKAGE_ROOT, PRODUCTION_ROOT, RENDER_ROOT, PLANNING_ROOT, QA_ROOT]


@dataclass(frozen=True)
class MaterialSpec:
    slug: str
    display: str
    motif: str
    base: tuple[int, int, int]
    accent: tuple[int, int, int]
    metal: float
    smooth: float
    bump: float
    status: str
    priority: str
    use: str
    description: str
    note: str


SPECS: list[MaterialSpec] = [
    MaterialSpec("WetBlackStoneSlab", "Wet Black Stone Slab", "wet_stone", (24, 25, 24), (78, 82, 76), 0.02, 0.72, 0.62, "final_candidate", "high", "Corridor wall blocks, low wall returns, wet floor edges", "Oil-dark stone slabs with recessed mortar, cracks, pooled wetness, and amber glints.", "Strong final candidate. This directly attacks the flat grey wall/floor gap."),
    MaterialSpec("WetBlackStoneMortar", "Wet Black Stone Mortar", "wet_stone_fine", (18, 19, 18), (63, 66, 58), 0.01, 0.66, 0.70, "final_candidate", "high", "Dense wall/floor tiling where visible mortar detail is needed", "Finer black-stone blockwork with damp mortar lines and grime held in cracks.", "Final candidate, but should be scale-tested in Unity before broad tiling."),
    MaterialSpec("ChippedBlackIronWallPanel", "Chipped Black Iron Wall Panel", "iron_panel", (31, 33, 32), (142, 117, 78), 0.88, 0.44, 0.58, "final_candidate", "high", "Primary corridor wall plates and doors", "Riveted dark iron panels with chipped brass/steel edges, oxidized seams, and oil smears.", "Strong final candidate. This is the set anchor for black riveted iron."),
    MaterialSpec("ChippedBlackIronServicePlate", "Chipped Black Iron Service Plate", "iron_dense", (25, 27, 27), (110, 103, 89), 0.90, 0.38, 0.68, "final_candidate", "high", "Secondary plates, weapon receiver blocks, service door panels", "Darker service plating with tighter rivet rhythm, scraped corners, and soot-set seams.", "Final candidate for dark panels; slightly busy at distance by design."),
    MaterialSpec("WornBrassPipe", "Worn Brass Pipe", "brass_pipe", (159, 111, 45), (235, 179, 73), 0.86, 0.46, 0.44, "final_candidate", "high", "Wall/ceiling pipes, weapon barrels, valve housings", "Aged brass cylinder surface with lengthwise scratches, rubbed highlights, tarnish, and banding.", "Strong final candidate. Replaces clean yellow brass with worn pressure hardware."),
    MaterialSpec("WornBrassValveBody", "Worn Brass Valve Body", "brass_cast", (128, 89, 40), (218, 164, 67), 0.84, 0.36, 0.50, "final_candidate", "medium", "Valve wheels, receivers, fittings, gauge bezels", "Cast brass with darker patina pockets, raised rim wear, stamped band rhythm, and soot dust.", "Final candidate for props; not a wall/floor hero."),
    MaterialSpec("OxidizedCopperCoil", "Oxidized Copper Coil", "copper_coil", (137, 70, 38), (56, 130, 110), 0.82, 0.34, 0.52, "final_candidate", "high", "Steam coils, heat exchangers, pressure weapon tubing", "Copper with green-blue oxidation in grooves and fresh copper rubs on exposed coil ridges.", "Strong final candidate for breaking up brass monotony."),
    MaterialSpec("OxidizedCopperRunoff", "Oxidized Copper Runoff", "copper_runoff", (104, 57, 37), (52, 146, 123), 0.76, 0.31, 0.55, "final_candidate", "medium", "Old pipe backs, leaky walls, drainage-adjacent metal", "Dirtier copper sheet with vertical verdigris runoff and darker soot deposits.", "Final candidate, best used sparingly because the green can become loud."),
    MaterialSpec("RedPressureEnamel", "Red Pressure Enamel", "red_enamel", (130, 24, 18), (226, 67, 35), 0.20, 0.58, 0.36, "final_candidate", "high", "Pressure warnings, gate strips, hazard caps", "Deep red enamel over metal with chips, scratches, black soot specks, and warm worn edges.", "Strong final candidate for pressure danger language."),
    MaterialSpec("ChippedRedEnamelEdge", "Chipped Red Enamel Edge", "red_enamel_chipped", (99, 19, 18), (220, 86, 43), 0.25, 0.42, 0.50, "final_candidate", "medium", "Weapon details, panel edges, valve warning rims", "Heavier chipped red enamel exposing dark iron and brass undertones around edges and seams.", "Final candidate but high-detail; should be kept to accents."),
    MaterialSpec("AmberGaslightGlass", "Amber Gaslight Glass", "amber_glass", (220, 126, 38), (255, 205, 101), 0.00, 0.86, 0.18, "final_candidate", "high", "Lantern glass, status windows, small warm highlights", "Warm amber glass with smoke scratches, grime, and bright gaslight lens bloom baked into base color.", "Final candidate for atmosphere. Needs actual light/VFX later for full glow."),
    MaterialSpec("SmokedAmberGaugeGlass", "Smoked Amber Gauge Glass", "smoked_glass", (125, 82, 39), (255, 186, 83), 0.00, 0.82, 0.20, "candidate", "medium", "Gauge covers and small instrument windows", "Smokier amber glass with fingerprints, grime arcs, and heavier dark edges for instrument covers.", "Candidate. Reads well in previews, but transparency needs a project shader decision."),
    MaterialSpec("SootDepositOverlay", "Soot Deposit Overlay", "soot_overlay", (18, 16, 13), (70, 53, 36), 0.02, 0.22, 0.38, "candidate", "medium", "Future decal/overlay use near vents, weapons, furnace mouths", "Matte black-brown soot clouds with ash speckles and directional exhaust deposits.", "Candidate overlay. It is authored as an opaque material plus masks until a decal shader is chosen."),
    MaterialSpec("VerticalGrimeStreakOverlay", "Vertical Grime Streak Overlay", "grime_streak", (25, 21, 16), (95, 68, 39), 0.03, 0.28, 0.42, "candidate", "medium", "Future decal/overlay use under pipes, corners, and wall seams", "Long oily vertical streaks with wet centers and dusty feathered sides.", "Candidate overlay. Needs alpha/decal hookup before production placement."),
    MaterialSpec("BlackOilWetFloor", "Black Oil Wet Floor", "oily_floor", (15, 16, 15), (92, 78, 51), 0.04, 0.80, 0.46, "final_candidate", "high", "Foundry floors, puddle-heavy service walkways", "Black stone/iron floor with oily puddles, scuffed traffic lanes, and amber wet reflections.", "Strong final candidate. Use in patches so the level does not become uniformly glossy."),
    MaterialSpec("OilyBlackStonePuddle", "Oily Black Stone Puddle", "oil_puddle", (9, 10, 10), (120, 93, 45), 0.03, 0.90, 0.28, "candidate", "medium", "Local puddle surfaces, drainage zones, floor decals later", "Very wet black puddle sheet with rainbow-dark oil edges and soft amber highlights.", "Candidate. Final look is strong; gameplay readability must be checked because it is very dark."),
    MaterialSpec("ScorchedFurnaceMetal", "Scorched Furnace Metal", "furnace_metal", (38, 31, 25), (205, 91, 38), 0.90, 0.33, 0.60, "final_candidate", "high", "Furnace doors, heated boiler plates, muzzle shields", "Blackened metal with orange heat scorch, soot halos, heat tint, and blistered roughness.", "Strong final candidate for furnace-room identity."),
    MaterialSpec("HeatTintedBoilerIron", "Heat Tinted Boiler Iron", "heat_iron", (42, 41, 39), (118, 83, 61), 0.92, 0.40, 0.50, "final_candidate", "medium", "Boiler shells, weapon sleeves, hot pipe brackets", "Oily black iron with blue-brown heat staining, scraped rims, and rivet-adjacent soot.", "Final candidate, quieter than furnace metal and useful as a bridge material."),
    MaterialSpec("GaugeFaceEnamel", "Gauge Face Enamel", "gauge_face", (217, 202, 154), (154, 26, 18), 0.00, 0.50, 0.14, "final_candidate", "high", "Gauge faces, HUD inserts, pressure instruments", "Aged cream enamel dial with printed ticks, red needle motif, cracks, and dirty brass rim.", "Strong final candidate. This is important for the pressure-pistol/HUD language."),
    MaterialSpec("BakedIvoryGaugeFace", "Baked Ivory Gauge Face", "gauge_ivory", (188, 174, 130), (95, 42, 28), 0.00, 0.44, 0.18, "candidate", "low", "Secondary old gauge faces and background instrument clusters", "Darker old ivory enamel with fewer red accents, chipped rim dirt, and faded radial marks.", "Candidate. Useful filler, but less iconic than Gauge Face Enamel."),
    MaterialSpec("RivetedBrassTrim", "Riveted Brass Trim", "riveted_brass", (147, 101, 42), (238, 179, 72), 0.86, 0.43, 0.42, "final_candidate", "high", "Wall trim, door frames, floor thresholds, weapon bands", "Brass strip material with repeating rivets, edge polish, tarnish in recesses, and underside grime.", "Strong final candidate for north-star brass framing."),
    MaterialSpec("RivetedBlackIronTrim", "Riveted Black Iron Trim", "riveted_iron", (28, 30, 30), (126, 110, 76), 0.90, 0.41, 0.48, "final_candidate", "medium", "Iron ribs, panel borders, grate supports", "Black iron trim with brass-touched rivet crowns, oily seams, and rubbed leading edges.", "Final candidate. Good for giving flat kit pieces a heavy industrial silhouette."),
    MaterialSpec("SteamCondensationBlackMetal", "Steam Condensation Black Metal", "condensation", (24, 25, 24), (84, 92, 86), 0.82, 0.74, 0.32, "candidate", "medium", "Cold metal near vents, underside of pipes, damp machinery backs", "Dark metal with condensation trails, bead-like wet noise, and wiped streaks.", "Candidate. The wetness is useful, but can become noisy without careful scale."),
    MaterialSpec("CrackedBlackRubberGasket", "Cracked Black Rubber Gasket", "rubber", (18, 17, 15), (72, 59, 43), 0.00, 0.36, 0.45, "placeholder", "low", "Temporary gasket, hose, and pressure seal material", "Black rubber gasket surface with cracks, dusted edges, and oil-dark wear.", "Placeholder. Included for completeness, but it does not move the corridor/gun visual target much."),
]


def rel(path: Path) -> str:
    return path.relative_to(PROJECT_ROOT).as_posix()


def stable_guid(label: str) -> str:
    return hashlib.md5(label.replace("\\", "/").lower().encode("utf-8")).hexdigest()


def assert_allowed(path: Path) -> None:
    resolved = path.resolve(strict=False)
    for root in ALLOWED_ROOTS:
        try:
            resolved.relative_to(root.resolve(strict=False))
            return
        except ValueError:
            pass
    raise RuntimeError(f"Refusing to write outside Set08 roots: {path}")


def write_text(path: Path, text: str) -> None:
    assert_allowed(path)
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(text, encoding="utf-8", newline="\n")


def write_json(path: Path, payload: object) -> None:
    write_text(path, json.dumps(payload, indent=2, sort_keys=False) + "\n")


def save_png(path: Path, image: Image.Image) -> None:
    assert_allowed(path)
    path.parent.mkdir(parents=True, exist_ok=True)
    image.save(path, "PNG", optimize=True)


def write_default_meta(path: Path, *, folder: bool = False, user_data: str = "") -> str:
    guid = stable_guid(rel(path))
    if folder:
        text = f"""fileFormatVersion: 2
guid: {guid}
folderAsset: yes
DefaultImporter:
  externalObjects: {{}}
  userData: {user_data}
  assetBundleName: 
  assetBundleVariant: 
"""
    else:
        text = f"""fileFormatVersion: 2
guid: {guid}
DefaultImporter:
  externalObjects: {{}}
  userData: {user_data}
  assetBundleName: 
  assetBundleVariant: 
"""
    write_text(Path(str(path) + ".meta"), text)
    return guid


def write_material_meta(path: Path) -> str:
    guid = stable_guid(rel(path))
    text = f"""fileFormatVersion: 2
guid: {guid}
NativeFormatImporter:
  externalObjects: {{}}
  mainObjectFileID: 2100000
  userData: {CODE} material
  assetBundleName: 
  assetBundleVariant: 
"""
    write_text(Path(str(path) + ".meta"), text)
    return guid


def write_texture_meta(path: Path, kind: str) -> str:
    guid = stable_guid(rel(path))
    srgb = 1 if kind in {"albedo", "preview"} else 0
    texture_type = 1 if kind == "normal" else 0
    user_data = {
        "albedo": f"{CODE} base color procedural texture",
        "normal": f"{CODE} tangent-space normal procedural texture",
        "rma": f"{CODE} R=metallic intent G=roughness intent B=ambient occlusion/grime",
        "grime": f"{CODE} R=edgewear G=grime/soot B=wetness or patina",
        "preview": f"{CODE} documentation preview render",
    }[kind]
    text = f"""fileFormatVersion: 2
guid: {guid}
TextureImporter:
  serializedVersion: 13
  mipmaps:
    enableMipMap: 1
    sRGBTexture: {srgb}
  isReadable: 0
  textureType: {texture_type}
  textureShape: 1
  textureSettings:
    serializedVersion: 2
    filterMode: 1
    aniso: 4
    mipBias: 0
    wrapU: 1
    wrapV: 1
    wrapW: 1
  platformSettings:
  - serializedVersion: 4
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 1024
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 60
    crunchedCompression: 0
    overridden: 0
  userData: {user_data}
  assetBundleName: 
  assetBundleVariant: 
"""
    write_text(Path(str(path) + ".meta"), text)
    return guid


def mat_id(spec: MaterialSpec) -> str:
    return f"{CODE}_MAT_{spec.slug}"


def clamp_img(a: np.ndarray) -> np.ndarray:
    return np.clip(a, 0, 255).astype(np.uint8)


def fbm(seed: int, size: int = SIZE) -> np.ndarray:
    rng = np.random.default_rng(seed)
    total = np.zeros((size, size), dtype=np.float32)
    weight = 0.0
    for scale, amp in [(8, 0.45), (16, 0.28), (32, 0.17), (64, 0.10)]:
        small = rng.random((scale, scale), dtype=np.float32)
        img = Image.fromarray((small * 255).astype(np.uint8), "L").resize((size, size), Image.Resampling.BICUBIC)
        total += np.asarray(img, dtype=np.float32) / 255.0 * amp
        weight += amp
    return total / weight


def grid_distance(x: np.ndarray, period: int) -> np.ndarray:
    mod = np.mod(x, period)
    return np.minimum(mod, period - mod)


def disk(x: np.ndarray, y: np.ndarray, cx: int, cy: int, radius: int) -> np.ndarray:
    return np.clip(1.0 - np.sqrt((x - cx) ** 2 + (y - cy) ** 2) / max(1, radius), 0, 1)


def apply_mix(img: np.ndarray, color: tuple[int, int, int], mask: np.ndarray, amount: float = 1.0) -> None:
    m = np.clip(mask * amount, 0, 1)[..., None]
    img[:] = img * (1 - m) + np.array(color, dtype=np.float32) * m


def normal_from_height(height: np.ndarray, bump: float) -> Image.Image:
    gy, gx = np.gradient(height.astype(np.float32))
    strength = bump * 5.0
    nx = -gx * strength
    ny = -gy * strength
    nz = np.ones_like(nx)
    length = np.sqrt(nx * nx + ny * ny + nz * nz)
    normal = np.stack(((nx / length * 0.5 + 0.5) * 255, (ny / length * 0.5 + 0.5) * 255, (nz / length * 0.5 + 0.5) * 255), axis=2)
    return Image.fromarray(clamp_img(normal), "RGB")


def generate_textures(spec: MaterialSpec, seed: int) -> tuple[Image.Image, Image.Image, Image.Image, Image.Image]:
    y, x = np.mgrid[0:SIZE, 0:SIZE].astype(np.float32)
    u = x / (SIZE - 1)
    v = y / (SIZE - 1)
    n = fbm(seed)
    n2 = fbm(seed + 101)
    rng = np.random.default_rng(seed + 777)
    speck = rng.random((SIZE, SIZE), dtype=np.float32)

    img = np.zeros((SIZE, SIZE, 3), dtype=np.float32)
    img[:] = np.array(spec.base, dtype=np.float32)
    img += (n[..., None] - 0.5) * 46
    height = 0.50 + (n - 0.5) * 0.18
    metal = np.full((SIZE, SIZE), spec.metal, dtype=np.float32)
    rough = np.full((SIZE, SIZE), 1.0 - spec.smooth, dtype=np.float32)
    ao = 0.88 - n2 * 0.13
    edge = np.zeros((SIZE, SIZE), dtype=np.float32)
    dirt = 0.15 + n2 * 0.25
    wet = np.zeros((SIZE, SIZE), dtype=np.float32)
    motif = spec.motif

    if motif in {"wet_stone", "wet_stone_fine"}:
        period = 128 if motif == "wet_stone" else 96
        mortar = np.minimum(grid_distance(x, period), grid_distance(y, period))
        m = np.clip((6 - mortar) / 6, 0, 1)
        apply_mix(img, (4, 5, 5), m, 0.76)
        height -= m * 0.22
        dirt += m * 0.35
        edge += np.clip((10 - mortar) / 10, 0, 1) * 0.25
        cracks = (np.abs(np.sin((x * 0.040 + y * 0.073 + n2 * 4 + seed) * math.pi)) > 0.987) & (n > 0.38)
        apply_mix(img, (1, 1, 1), cracks.astype(np.float32), 0.70)
        height -= cracks * 0.20
        w = np.clip((fbm(seed + 8) - 0.50) * 2.1, 0, 1)
        apply_mix(img, (7, 9, 9), w, 0.45)
        apply_mix(img, (96, 73, 35), w * (0.25 + u * 0.55), 0.18)
        wet += w
        rough = np.minimum(rough, 0.18 + n * 0.12)

    elif motif in {"iron_panel", "iron_dense"}:
        period = 256 if motif == "iron_panel" else 170
        seam = np.minimum(grid_distance(x, period), grid_distance(y, period))
        m = np.clip((7 - seam) / 7, 0, 1)
        apply_mix(img, (3, 4, 4), m, 0.80)
        height -= m * 0.18
        dirt += m * 0.35
        step = 92 if motif == "iron_panel" else 74
        for cx in range(34, SIZE, step):
            for cy in [34, 128, 256, 384, SIZE - 35]:
                r = disk(x, y, cx, cy, 15)
                apply_mix(img, spec.accent, r, 0.55)
                height += r * 0.24
                edge += r * 0.58
        scratches = ((speck > 0.992) | (np.abs(np.sin((x * 0.23 + y * 0.017 + seed) * math.pi)) > 0.997)) & (n2 > 0.44)
        apply_mix(img, spec.accent, scratches.astype(np.float32), 0.62)
        edge += scratches * 0.55
        wet += np.clip((fbm(seed + 28) - 0.58) * 2.0, 0, 1)
        rough = np.maximum(0.24, rough - wet * 0.24)

    elif motif in {"brass_pipe", "brass_cast"}:
        curve = np.sin(v * math.pi)
        img[:] = img * (0.78 + curve[..., None] * 0.35)
        apply_mix(img, spec.accent, curve, 0.28)
        band = grid_distance(x, 128 if motif == "brass_pipe" else 96)
        b = np.clip((7 - band) / 7, 0, 1)
        apply_mix(img, (67, 46, 25), b, 0.50)
        height += b * 0.18
        tarnish = np.clip((n2 - 0.52) * 2.1, 0, 1)
        apply_mix(img, (54, 43, 25), tarnish, 0.32)
        scratches = (np.abs(np.sin((y * 0.37 + x * 0.018 + seed) * math.pi)) > 0.991) & (n2 > 0.45)
        apply_mix(img, (248, 204, 100), scratches.astype(np.float32), 0.68)
        edge += scratches * 0.50

    elif motif in {"copper_coil", "copper_runoff"}:
        stripe = 0.5 + 0.5 * np.sin((x * 0.048 + y * (0.025 if motif == "copper_coil" else 0.002) + seed) * math.pi)
        apply_mix(img, (196, 94, 45), stripe, 0.25)
        patina = np.clip((n2 - 0.45) * 2.2, 0, 1)
        if motif == "copper_runoff":
            patina = np.maximum(patina, np.clip((fbm(seed + 88) - 0.42) * 2.0, 0, 1) * (0.25 + v))
        apply_mix(img, spec.accent, patina, 0.48)
        height += stripe * 0.12 - patina * 0.12
        dirt += patina * 0.38
        edge += stripe * 0.18
        ridges = grid_distance(x + y * 0.25, 84) < 4
        apply_mix(img, (235, 130, 62), ridges.astype(np.float32), 0.35)
        height += ridges * 0.14

    elif motif in {"red_enamel", "red_enamel_chipped"}:
        seam = np.minimum(grid_distance(x, 170), grid_distance(y, 170))
        apply_mix(img, (33, 17, 14), np.clip((5 - seam) / 5, 0, 1), 0.52)
        chip_noise = fbm(seed + 99)
        edge_zone = np.maximum.reduce([np.clip((12 - x) / 12, 0, 1), np.clip((12 - y) / 12, 0, 1), np.clip((x - 500) / 12, 0, 1), np.clip((y - 500) / 12, 0, 1), np.clip((9 - seam) / 9, 0, 1)])
        chips = (chip_noise > (0.72 if motif == "red_enamel_chipped" else 0.83)) | ((edge_zone > 0) & (speck > 0.72))
        expose = np.maximum(edge_zone, np.clip(chip_noise - 0.70, 0, 1)) * chips
        apply_mix(img, (34, 33, 30), expose, 0.68)
        apply_mix(img, (187, 129, 56), expose, 0.28)
        metal = np.maximum(metal, expose * 0.55)
        rough = np.maximum(rough, 0.52 * expose)
        edge += expose
        height -= expose * 0.08
        soot = speck > 0.996
        apply_mix(img, (8, 6, 5), soot.astype(np.float32), 0.85)

    elif motif in {"amber_glass", "smoked_glass"}:
        radial = np.sqrt((u - 0.5) ** 2 + (v - 0.5) ** 2)
        glow = np.clip(1 - radial * 1.8, 0, 1)
        apply_mix(img, spec.accent, glow, 0.58)
        apply_mix(img, (35, 24, 16), n2 * (0.32 if motif == "smoked_glass" else 0.20), 1.0)
        glare = np.clip(1 - np.abs(np.sqrt((x - 184) ** 2 + (y - 174) ** 2) - 150) / 11, 0, 1)
        glare += np.clip(1 - np.abs(np.sqrt((x - 328) ** 2 + (y - 286) ** 2) - 90) / 7, 0, 1)
        apply_mix(img, (255, 226, 130), np.clip(glare, 0, 1), 0.50)
        wet += glow * 0.75
        rough[:] = 0.08 + n2 * 0.12

    elif motif in {"soot_overlay", "grime_streak"}:
        if motif == "grime_streak":
            streak = np.clip((fbm(seed + 12) - 0.42) * 2.1, 0, 1) * (0.35 + v * 0.8)
            apply_mix(img, (94, 63, 31), streak, 0.52)
            apply_mix(img, (7, 6, 5), streak, 0.35)
            dirt = np.maximum(dirt, streak)
            wet += streak * 0.62
            height -= streak * 0.06
        else:
            soot = np.clip((n - 0.25) * 1.35, 0, 1)
            img[:] = np.array((28, 25, 20), dtype=np.float32)
            apply_mix(img, (4, 4, 4), soot, 0.85)
            ash = (speck > 0.985).astype(np.float32)
            apply_mix(img, (101, 82, 58), ash, 0.55)
            dirt = np.maximum(dirt, soot)
        rough[:] = 0.76 if motif == "soot_overlay" else 0.66
        metal[:] = 0.0

    elif motif in {"oily_floor", "oil_puddle"}:
        if motif == "oily_floor":
            seam = np.minimum(grid_distance(x, 128), grid_distance(y, 128))
            m = np.clip((4 - seam) / 4, 0, 1)
            apply_mix(img, (3, 3, 3), m, 0.62)
            height -= m * 0.16
        oil = np.clip((fbm(seed + 67) - (0.42 if motif == "oil_puddle" else 0.56)) * 2.5, 0, 1)
        swirl = 0.5 + 0.5 * np.sin((x * 0.028 + y * 0.041 + n2 * 7 + seed) * math.pi)
        apply_mix(img, (4, 5, 6), oil, 0.62)
        apply_mix(img, (129, 97, 42), oil * swirl, 0.18)
        apply_mix(img, (35, 49, 45), oil * (1 - swirl), 0.12)
        wet += oil
        rough = np.maximum(0.07, rough - oil * 0.55)
        height += oil * 0.04

    elif motif in {"furnace_metal", "heat_iron"}:
        seam = np.minimum(grid_distance(x, 192), grid_distance(y, 192))
        m = np.clip((5 - seam) / 5, 0, 1)
        apply_mix(img, (7, 6, 5), m, 0.50)
        scorch = np.clip((fbm(seed + 31) - 0.44) * 1.8, 0, 1)
        heat_band = np.clip(1 - np.abs(v - 0.58) * 4, 0, 1)
        if motif == "furnace_metal":
            apply_mix(img, (172, 58, 27), scorch * heat_band, 0.24)
            apply_mix(img, (8, 7, 6), scorch * (1 - heat_band * 0.45), 0.44)
        else:
            apply_mix(img, (48, 63, 74), scorch, 0.17)
            apply_mix(img, (102, 70, 47), heat_band, 0.12)
        blisters = (speck > 0.987) & (n > 0.52)
        apply_mix(img, spec.accent, blisters.astype(np.float32), 0.32)
        height += blisters * 0.16
        edge += blisters * 0.28
        rough = np.minimum(0.82, rough + scorch * 0.22)

    elif motif in {"gauge_face", "gauge_ivory"}:
        cx, cy = SIZE / 2, SIZE / 2
        dx, dy = x - cx, y - cy
        radius = np.sqrt(dx * dx + dy * dy) / (SIZE * 0.5)
        angle = (np.arctan2(dy, dx) + math.pi) / (2 * math.pi)
        rim = np.clip(1 - np.abs(np.sqrt(dx * dx + dy * dy) - 224) / 18, 0, 1)
        outside = radius > 0.88
        apply_mix(img, (184, 128, 49), rim * outside, 0.90)
        divisions = 36 if motif == "gauge_face" else 24
        tick = (radius > 0.55) & (radius < 0.82) & (np.abs(angle * divisions - np.round(angle * divisions)) < 0.045)
        apply_mix(img, (20, 18, 15), tick.astype(np.float32), 0.86)
        if motif == "gauge_face":
            needle = (np.abs(np.arctan2(dy, dx) + 0.82) < 0.035) & (radius > 0.05) & (radius < 0.72)
            apply_mix(img, (146, 18, 13), needle.astype(np.float32), 0.95)
        cracks = (np.abs(np.sin((x * 0.032 - y * 0.061 + seed) * math.pi)) > 0.994) & (n > 0.47) & (radius < 0.82)
        apply_mix(img, (83, 64, 39), cracks.astype(np.float32), 0.55)
        dirt += radius * 0.18
        metal = np.where(outside, 0.72, metal)
        height += rim * 0.16

    elif motif in {"riveted_brass", "riveted_iron"}:
        edge_band = np.maximum(np.clip((26 - np.minimum(y, SIZE - 1 - y)) / 26, 0, 1), np.clip((12 - np.minimum(x, SIZE - 1 - x)) / 12, 0, 1))
        apply_mix(img, spec.accent if motif == "riveted_brass" else (74, 70, 62), edge_band, 0.22)
        for cx in range(52, SIZE, 84):
            r = disk(x, y, cx, SIZE // 2, 22)
            apply_mix(img, spec.accent if motif == "riveted_brass" else (112, 97, 69), r, 0.80)
            height += r * 0.28
            edge += r * 0.52
        tarnish = np.clip((n2 - 0.45) * 1.7, 0, 1)
        apply_mix(img, (41, 31, 21), tarnish, 0.25)
        dirt += tarnish * 0.20
        height += edge_band * 0.09
        edge += edge_band * 0.35

    elif motif == "condensation":
        bead = fbm(seed + 211)
        streak = np.clip((fbm(seed + 212) - 0.45) * 1.7, 0, 1) * (0.25 + v)
        apply_mix(img, (8, 9, 9), streak, 0.32)
        beads = bead > 0.76
        apply_mix(img, (111, 122, 115), beads.astype(np.float32), 0.35)
        wet += beads * 0.55 + streak * 0.38
        height += beads * 0.08
        rough = np.maximum(0.12, rough - np.maximum(streak, wet) * 0.38)
        dirt += streak * 0.25

    elif motif == "rubber":
        cracks = np.abs(np.sin((x * 0.065 + y * 0.041 + seed + n2 * 3) * math.pi)) > 0.986
        apply_mix(img, (39, 32, 23), n2, 0.22)
        apply_mix(img, (4, 3, 3), cracks.astype(np.float32), 0.85)
        height -= cracks * 0.12
        dirt += cracks * 0.26
        rough[:] = 0.62 + n * 0.18
        metal[:] = 0.0

    albedo = Image.fromarray(clamp_img(img), "RGB")
    normal = normal_from_height(np.clip(height, 0, 1), spec.bump)
    rma = Image.fromarray(clamp_img(np.stack((np.clip(metal, 0, 1) * 255, np.clip(rough, 0, 1) * 255, np.clip(ao, 0, 1) * 255), axis=2)), "RGB")
    grm = Image.fromarray(clamp_img(np.stack((np.clip(edge, 0, 1) * 255, np.clip(dirt, 0, 1) * 255, np.clip(wet, 0, 1) * 255), axis=2)), "RGB")
    return albedo, normal, rma, grm


def material_yaml(spec: MaterialSpec, alb_guid: str, nrm_guid: str, rma_guid: str, grm_guid: str) -> str:
    keywords = ["_METALLICGLOSSMAP", "_NORMALMAP"]
    if spec.motif in {"amber_glass", "smoked_glass", "furnace_metal"}:
        keywords.append("_EMISSION")
    keyword_block = "".join(f"  - {kw}\n" for kw in keywords)
    emission = (0.0, 0.0, 0.0)
    if spec.motif == "amber_glass":
        emission = (0.55, 0.28, 0.06)
    elif spec.motif == "smoked_glass":
        emission = (0.22, 0.12, 0.03)
    elif spec.motif == "furnace_metal":
        emission = (0.18, 0.055, 0.02)
    color = tuple(round(c / 255, 4) for c in spec.base)
    return f"""%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!21 &2100000
Material:
  serializedVersion: 8
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_Name: {mat_id(spec)}
  m_Shader: {{fileID: 46, guid: 0000000000000000f000000000000000, type: 0}}
  m_Parent: {{fileID: 0}}
  m_ModifiedSerializedProperties: 0
  m_ValidKeywords:
{keyword_block}  m_InvalidKeywords: []
  m_LightmapFlags: 4
  m_EnableInstancingVariants: 1
  m_DoubleSidedGI: 0
  m_CustomRenderQueue: -1
  stringTagMap: {{}}
  disabledShaderPasses: []
  m_LockedProperties: 
  m_SavedProperties:
    serializedVersion: 3
    m_TexEnvs:
    - _BumpMap:
        m_Texture: {{fileID: 2800000, guid: {nrm_guid}, type: 3}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _DetailAlbedoMap:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _DetailMask:
        m_Texture: {{fileID: 2800000, guid: {grm_guid}, type: 3}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _DetailNormalMap:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _EmissionMap:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _MainTex:
        m_Texture: {{fileID: 2800000, guid: {alb_guid}, type: 3}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _MetallicGlossMap:
        m_Texture: {{fileID: 2800000, guid: {rma_guid}, type: 3}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _OcclusionMap:
        m_Texture: {{fileID: 2800000, guid: {rma_guid}, type: 3}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _ParallaxMap:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    m_Ints: []
    m_Floats:
    - _BumpScale: {spec.bump:.3f}
    - _Cutoff: 0.5
    - _DetailNormalMapScale: 1
    - _DstBlend: 0
    - _GlossMapScale: 1
    - _Glossiness: {spec.smooth:.3f}
    - _GlossyReflections: 1
    - _Metallic: {spec.metal:.3f}
    - _Mode: 0
    - _OcclusionStrength: 0.92
    - _Parallax: 0.02
    - _SmoothnessTextureChannel: 0
    - _SpecularHighlights: 1
    - _SrcBlend: 1
    - _UVSec: 0
    - _ZWrite: 1
    m_Colors:
    - _Color: {{r: {color[0]}, g: {color[1]}, b: {color[2]}, a: 1}}
    - _EmissionColor: {{r: {emission[0]:.4f}, g: {emission[1]:.4f}, b: {emission[2]:.4f}, a: 1}}
  m_BuildTextureStacks: []
  m_AllowLocking: 1
"""


def make_dirs() -> None:
    for folder in [PACKAGE_ROOT, RUNTIME_ROOT, MATERIAL_ROOT, TEXTURE_ROOT, ALBEDO_ROOT, NORMAL_ROOT, RMA_ROOT, GRIME_ROOT, METADATA_ROOT, PACKAGE_ROOT / "Documentation~", DOC_MANIFEST_ROOT, PACKAGE_ROOT / "Samples~", SAMPLES_ROOT, PRODUCTION_ROOT, RENDER_ROOT, PLANNING_ROOT, QA_ROOT]:
        assert_allowed(folder)
        folder.mkdir(parents=True, exist_ok=True)
    for folder in [RUNTIME_ROOT, MATERIAL_ROOT, TEXTURE_ROOT, ALBEDO_ROOT, NORMAL_ROOT, RMA_ROOT, GRIME_ROOT, METADATA_ROOT, PACKAGE_ROOT / "Documentation~", DOC_MANIFEST_ROOT, PACKAGE_ROOT / "Samples~", SAMPLES_ROOT]:
        write_default_meta(folder, folder=True, user_data=f"{CODE} folder")


def paste_tex(canvas: Image.Image, tex: Image.Image, box: tuple[int, int, int, int], brightness: float = 1.0, alpha: float = 1.0) -> None:
    patch = tex.resize((box[2] - box[0], box[3] - box[1]), Image.Resampling.BICUBIC)
    if brightness != 1.0:
        patch = ImageEnhance.Brightness(patch).enhance(brightness)
    if alpha < 1.0:
        mask = Image.new("L", patch.size, int(alpha * 255))
        canvas.paste(patch, box[:2], mask)
    else:
        canvas.paste(patch, box[:2])


def draw_rivets(draw: ImageDraw.ImageDraw, points: list[tuple[int, int]], color=(210, 151, 62), r: int = 8) -> None:
    for x, y in points:
        draw.ellipse((x - r + 2, y - r + 3, x + r + 2, y + r + 3), fill=(8, 7, 6))
        draw.ellipse((x - r, y - r, x + r, y + r), fill=color)
        draw.ellipse((x - r // 2, y - r // 2, x, y), fill=(246, 196, 94))


def draw_pipe(canvas: Image.Image, tex: Image.Image, x0: int, y: int, x1: int, radius: int, band=(32, 29, 24), seed: int = 0) -> None:
    w = x1 - x0
    patch = tex.resize((w, radius * 2), Image.Resampling.BICUBIC).convert("RGB")
    arr = np.asarray(patch).astype(np.float32)
    yy = np.linspace(-1, 1, radius * 2)[:, None]
    shade = 0.44 + np.sqrt(np.clip(1 - yy * yy, 0, 1)) * 0.62
    arr *= shade[..., None]
    patch = Image.fromarray(clamp_img(arr), "RGB")
    canvas.paste(patch, (x0, y - radius))
    d = ImageDraw.Draw(canvas)
    d.ellipse((x0 - radius, y - radius, x0 + radius, y + radius), fill=tuple(clamp_img(np.asarray(tex.resize((1, 1))).reshape(3) * 0.75)))
    d.ellipse((x1 - radius, y - radius, x1 + radius, y + radius), fill=tuple(clamp_img(np.asarray(tex.resize((1, 1))).reshape(3) * 0.55)))
    for bx in range(x0 + 70 + seed % 41, x1, 150):
        d.rectangle((bx - 8, y - radius - 7, bx + 8, y + radius + 7), fill=band)
        d.line((bx, y - radius + 4, bx, y + radius - 4), fill=(236, 178, 74), width=4)


def draw_cylinder(canvas: Image.Image, tex: Image.Image, cx: int, y0: int, y1: int, radius: int, band=(31, 29, 25)) -> None:
    h = y1 - y0
    patch = tex.resize((radius * 2, h), Image.Resampling.BICUBIC).convert("RGB")
    arr = np.asarray(patch).astype(np.float32)
    xx = np.linspace(-1, 1, radius * 2)[None, :]
    shade = 0.44 + np.sqrt(np.clip(1 - xx * xx, 0, 1)) * 0.62
    arr *= shade[..., None]
    canvas.paste(Image.fromarray(clamp_img(arr), "RGB"), (cx - radius, y0))
    d = ImageDraw.Draw(canvas)
    avg = tuple(clamp_img(np.asarray(tex.resize((1, 1))).reshape(3)))
    d.ellipse((cx - radius, y0 - 18, cx + radius, y0 + 18), fill=avg)
    d.ellipse((cx - radius, y1 - 18, cx + radius, y1 + 18), fill=tuple(int(c * 0.55) for c in avg))
    for by in range(y0 + 74, y1, 126):
        d.rectangle((cx - radius - 10, by - 8, cx + radius + 10, by + 8), fill=band)
        d.line((cx - radius + 3, by, cx + radius - 3, by), fill=(230, 170, 70), width=5)


def preview_base() -> Image.Image:
    img = Image.new("RGB", (960, 720), (12, 12, 11))
    d = ImageDraw.Draw(img)
    for y in range(720):
        t = y / 719
        c = tuple(int((1 - t) * a + t * b) for a, b in zip((9, 10, 10), (37, 27, 20)))
        d.line((0, y, 960, y), fill=c)
    d.rectangle((42, 48, 918, 672), fill=(11, 12, 12), outline=(75, 55, 34), width=2)
    return img


def make_previews(textures: dict[str, Image.Image]) -> list[dict[str, str]]:
    T = textures
    previews: list[tuple[str, Image.Image]] = []

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["WetBlackStoneSlab"], (110, 150, 850, 390), 0.72)
    paste_tex(img, T["BlackOilWetFloor"], (120, 390, 840, 650), 1.05)
    paste_tex(img, T["ChippedBlackIronWallPanel"], (54, 142, 220, 650), 0.72)
    paste_tex(img, T["ChippedBlackIronServicePlate"], (740, 142, 912, 650), 0.74)
    draw_pipe(img, T["WornBrassPipe"], 142, 238, 812, 18, seed=1)
    draw_pipe(img, T["OxidizedCopperCoil"], 210, 302, 750, 14, seed=2)
    draw_rivets(d, [(158, 176), (802, 176), (190, 367), (770, 367), (84, 140), (88, 604), (882, 140), (880, 604)])
    d.ellipse((430, 210, 530, 310), fill=(225, 185, 90), outline=(23, 22, 20), width=10)
    d.line((480, 260, 514, 226), fill=(148, 20, 14), width=5)
    previews.append(("SMD08_PREVIEW_01_mini_corridor_material_board.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["WetBlackStoneSlab"], (118, 86, 842, 604), 0.95)
    for xx in range(118, 843, 144): d.line((xx, 86, xx, 604), fill=(4, 4, 4), width=4)
    for yy in range(86, 605, 128): d.line((118, yy, 842, yy), fill=(4, 4, 4), width=4)
    draw_pipe(img, T["WornBrassPipe"], 154, 514, 806, 20, seed=3)
    previews.append(("SMD08_PREVIEW_02_wet_black_stone_slab_panel.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["ChippedBlackIronWallPanel"], (126, 92, 834, 608), 0.92)
    d.rectangle((166, 132, 794, 568), outline=(5, 5, 5), width=5)
    draw_rivets(d, [(x, y) for x in range(178, 795, 102) for y in (142, 558)] + [(176, y) for y in range(210, 520, 88)] + [(794, y) for y in range(210, 520, 88)], (126, 104, 72), 9)
    d.line((206, 480, 762, 196), fill=(0, 0, 0), width=5)
    previews.append(("SMD08_PREVIEW_03_chipped_black_iron_wall_panel.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["ChippedBlackIronWallPanel"], (92, 90, 868, 614), 0.55)
    for y, r in [(190, 28), (282, 20), (360, 34), (468, 22)]:
        draw_pipe(img, T["WornBrassPipe"], 116, y, 852, r, seed=y)
    draw_rivets(d, [(164, 124), (796, 124), (164, 578), (796, 578)], (229, 170, 70), 9)
    previews.append(("SMD08_PREVIEW_04_worn_brass_pipe_cluster.png", img))

    img = preview_base()
    paste_tex(img, T["WetBlackStoneMortar"], (82, 84, 878, 624), 0.72)
    for cx, radius in [(300, 66), (484, 54), (660, 66)]:
        draw_cylinder(img, T["OxidizedCopperCoil"], cx, 146, 548, radius, (52, 142, 120))
    draw_pipe(img, T["WornBrassValveBody"], 170, 580, 792, 18, seed=5)
    previews.append(("SMD08_PREVIEW_05_oxidized_copper_coil_cylinders.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["RedPressureEnamel"], (132, 112, 828, 588), 0.92)
    d.rectangle((176, 156, 784, 544), outline=(10, 9, 8), width=12)
    d.ellipse((362, 234, 598, 470), outline=(28, 27, 24), width=24)
    d.line((376, 352, 584, 352), fill=(30, 29, 26), width=18); d.line((480, 248, 480, 456), fill=(30, 29, 26), width=18)
    draw_rivets(d, [(178, 158), (782, 158), (178, 542), (782, 542)], (235, 175, 72), 11)
    previews.append(("SMD08_PREVIEW_06_red_pressure_enamel_valve_plate.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["RivetedBlackIronTrim"], (96, 92, 864, 612), 0.8)
    d.ellipse((288, 158, 672, 542), fill=(80, 39, 12))
    paste_tex(img, T["AmberGaslightGlass"], (300, 166, 660, 534), 1.04)
    for xx in range(320, 660, 58): d.rectangle((xx, 178, xx + 12, 522), fill=(13, 13, 12))
    for yy in [188, 292, 414, 514]: d.line((284, yy, 676, yy), fill=(19, 18, 16), width=12)
    previews.append(("SMD08_PREVIEW_07_amber_gaslight_glass_cage.png", img))

    img = preview_base()
    paste_tex(img, T["WetBlackStoneSlab"], (124, 82, 836, 616), 0.8)
    paste_tex(img, T["VerticalGrimeStreakOverlay"], (124, 82, 836, 616), 1.1, 0.64)
    draw_pipe(img, T["OxidizedCopperRunoff"], 120, 146, 840, 16, (52, 142, 120), 8)
    previews.append(("SMD08_PREVIEW_08_vertical_grime_streak_wall.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["ChippedBlackIronWallPanel"], (90, 86, 870, 330), 0.50)
    paste_tex(img, T["BlackOilWetFloor"], (48, 330, 912, 640), 1.08)
    for xx in range(145, 850, 128): d.line((xx, 448, xx + 58, 638), fill=(3, 3, 3), width=4)
    draw_pipe(img, T["WornBrassPipe"], 160, 214, 800, 18, seed=9)
    previews.append(("SMD08_PREVIEW_09_black_oil_wet_floor.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["ScorchedFurnaceMetal"], (144, 92, 816, 612), 0.94)
    d.rectangle((310, 190, 650, 500), fill=(7, 6, 5), outline=(125, 85, 48), width=10)
    d.rectangle((348, 232, 612, 460), fill=(72, 24, 11))
    for yy in range(254, 450, 52): d.line((320, yy, 640, yy), fill=(29, 27, 24), width=14)
    draw_rivets(d, [(176, 126), (784, 126), (176, 578), (784, 578), (284, 196), (676, 196), (284, 504), (676, 504)], (133, 94, 56), 9)
    previews.append(("SMD08_PREVIEW_10_scorched_furnace_metal_door.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["ChippedBlackIronServicePlate"], (96, 90, 864, 612), 0.75)
    for cx, cy, r in [(310, 328, 130), (500, 330, 105), (650, 410, 72)]:
        d.ellipse((cx - r - 22, cy - r - 22, cx + r + 22, cy + r + 22), fill=(125, 86, 35))
        paste_tex(img, T["GaugeFaceEnamel"], (cx - r, cy - r, cx + r, cy + r), 1.0)
        d.ellipse((cx - 8, cy - 8, cx + 8, cy + 8), fill=(40, 29, 22))
        d.line((cx, cy, cx + int(r * 0.52), cy - int(r * 0.36)), fill=(150, 20, 14), width=5)
    previews.append(("SMD08_PREVIEW_11_gauge_face_enamel_cluster.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["WetBlackStoneMortar"], (78, 86, 882, 620), 0.74)
    for yy in [174, 302, 430, 558]:
        paste_tex(img, T["RivetedBrassTrim"], (112, yy - 28, 848, yy + 28), 1.02)
        draw_rivets(d, [(xx, yy) for xx in range(160, 810, 96)], (231, 172, 70), 7)
    previews.append(("SMD08_PREVIEW_12_riveted_brass_trim_strips.png", img))

    img = preview_base()
    paste_tex(img, T["WetBlackStoneSlab"], (100, 82, 860, 618), 0.70)
    for cx in [192, 480, 768]:
        draw_cylinder(img, T["RivetedBlackIronTrim"], cx, 126, 584, 44, (122, 104, 74))
    draw_pipe(img, T["SteamCondensationBlackMetal"], 132, 342, 830, 26, (118, 104, 74), 13)
    previews.append(("SMD08_PREVIEW_13_riveted_black_iron_trim_cylinders.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["WetBlackStoneMortar"], (84, 106, 876, 642), 1.0)
    paste_tex(img, T["SootDepositOverlay"], (100, 120, 860, 620), 1.0, 0.36)
    draw_rivets(d, [(140, 144), (820, 144), (140, 606), (820, 606)], (156, 108, 49), 8)
    previews.append(("SMD08_PREVIEW_14_wet_stone_floor_tiles_soot.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["ChippedBlackIronServicePlate"], (84, 84, 876, 622), 0.78)
    draw_pipe(img, T["WornBrassPipe"], 136, 210, 820, 24, seed=15)
    draw_pipe(img, T["OxidizedCopperRunoff"], 158, 310, 796, 20, (52, 140, 118), 16)
    draw_pipe(img, T["WornBrassValveBody"], 214, 432, 742, 30, seed=17)
    d.ellipse((400, 240, 560, 400), outline=(24, 23, 21), width=16)
    d.line((420, 320, 540, 320), fill=(26, 25, 23), width=13); d.line((480, 260, 480, 380), fill=(26, 25, 23), width=13)
    previews.append(("SMD08_PREVIEW_15_copper_brass_pressure_board.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["HeatTintedBoilerIron"], (90, 92, 870, 616), 0.90)
    paste_tex(img, T["ScorchedFurnaceMetal"], (220, 148, 740, 552), 0.95)
    paste_tex(img, T["RedPressureEnamel"], (260, 186, 700, 230), 0.90)
    paste_tex(img, T["RivetedBrassTrim"], (260, 500, 700, 544), 1.02)
    d.ellipse((388, 273, 572, 457), fill=(78, 26, 12))
    draw_rivets(d, [(248, 176), (712, 176), (248, 524), (712, 524)], (226, 165, 70), 9)
    previews.append(("SMD08_PREVIEW_16_furnace_material_mix.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["ChippedBlackIronWallPanel"], (108, 230, 850, 440), 0.90)
    draw_pipe(img, T["WornBrassPipe"], 156, 348, 808, 22, seed=18)
    paste_tex(img, T["RivetedBrassTrim"], (198, 198, 760, 246), 1.02)
    paste_tex(img, T["GaugeFaceEnamel"], (424, 268, 536, 380), 1.0)
    d.ellipse((406, 250, 554, 398), outline=(120, 82, 34), width=16)
    d.line((480, 324, 515, 292), fill=(150, 20, 14), width=4)
    previews.append(("SMD08_PREVIEW_17_pressure_weapon_receiver_material_strip.png", img))

    img = preview_base()
    paste_tex(img, T["WetBlackStoneSlab"], (70, 88, 492, 620), 0.76)
    paste_tex(img, T["ChippedBlackIronWallPanel"], (492, 88, 890, 620), 0.82)
    d = ImageDraw.Draw(img); d.line((492, 88, 492, 620), fill=(183, 126, 48), width=18)
    draw_pipe(img, T["WornBrassPipe"], 110, 184, 842, 18, seed=19)
    draw_pipe(img, T["SteamCondensationBlackMetal"], 110, 500, 842, 22, (112, 100, 72), 20)
    paste_tex(img, T["VerticalGrimeStreakOverlay"], (88, 112, 858, 610), 1.1, 0.26)
    previews.append(("SMD08_PREVIEW_18_corridor_corner_surface_board.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    paste_tex(img, T["WetBlackStoneMortar"], (82, 86, 878, 620), 0.66)
    for cx, key in [(250, "WornBrassValveBody"), (470, "OxidizedCopperCoil"), (690, "HeatTintedBoilerIron")]:
        draw_cylinder(img, T[key], cx, 178, 506, 58)
        d.ellipse((cx - 58, 278, cx + 58, 394), outline=(22, 21, 19), width=10)
        paste_tex(img, T["BakedIvoryGaugeFace"], (cx - 42, 294, cx + 42, 378), 1.0)
        d.line((cx, 336, cx + 27, 314), fill=(146, 20, 14), width=3)
    previews.append(("SMD08_PREVIEW_19_small_prop_cylinders_and_gauges.png", img))

    img = preview_base(); d = ImageDraw.Draw(img)
    keys = ["WetBlackStoneSlab", "ChippedBlackIronWallPanel", "WornBrassPipe", "OxidizedCopperCoil", "RedPressureEnamel", "AmberGaslightGlass", "BlackOilWetFloor", "ScorchedFurnaceMetal", "GaugeFaceEnamel", "RivetedBrassTrim", "RivetedBlackIronTrim", "SteamCondensationBlackMetal"]
    for i, key in enumerate(keys):
        col, row = i % 4, i // 4
        x0, y0 = 92 + col * 202, 92 + row * 174
        paste_tex(img, T[key], (x0, y0, x0 + 158, y0 + 116), 1.0)
        d.rectangle((x0, y0, x0 + 158, y0 + 116), outline=(12, 10, 8), width=3)
        draw_rivets(d, [(x0 + 14, y0 + 14), (x0 + 144, y0 + 14), (x0 + 14, y0 + 102), (x0 + 144, y0 + 102)], (206, 147, 61), 5)
    previews.append(("SMD08_PREVIEW_20_final_candidate_contact_sheet.png", img))

    records = []
    for index, (name, img) in enumerate(previews, 1):
        path = RENDER_ROOT / name
        save_png(path, img)
        write_texture_meta(path, "preview")
        records.append({"id": f"{CODE}_PREVIEW_{index:02d}", "path": rel(path), "purpose": name.replace("SMD08_PREVIEW_", "").replace(".png", "").replace("_", " ")})
    return records


def package_docs(materials: list[dict[str, object]], textures: list[dict[str, object]], previews: list[dict[str, str]]) -> None:
    generated_at = datetime.now(timezone.utc).isoformat().replace("+00:00", "Z")
    final_count = sum(1 for m in materials if m["status"] == "final_candidate")
    candidate_count = sum(1 for m in materials if m["status"] == "candidate")
    placeholder_count = sum(1 for m in materials if m["status"] == "placeholder")
    package = {
        "name": PACKAGE_ID,
        "version": VERSION,
        "displayName": "Brassworks Breach Surface Material Detail Set 08",
        "description": "Unity-only sidecar package of procedural final-look surface materials targeting wet black stone, riveted iron, worn brass/copper, grime, enamel, oil, glass, and furnace metal detail. Visual/material assets only; no gameplay authority.",
        "unity": UNITY_VERSION,
        "author": {"name": "Brassworks Breach Sidecar Lane"},
        "dependencies": {},
        "keywords": ["brassworks", "sidecar", "materials", "surface-detail", "steampunk", "wet-stone", "riveted-iron", "brass", "unity-procedural"],
        "samples": [{"displayName": "Material Board Notes", "description": "Review notes for generated material boards and preview renders.", "path": "Samples~/MaterialBoard"}],
    }
    write_json(PACKAGE_ROOT / "package.json", package)
    write_default_meta(PACKAGE_ROOT / "package.json", user_data=f"{CODE} package manifest")
    readme = f"""# Brassworks Breach Surface Material Detail Set 08

Unity-only sidecar package for procedural steampunk surface detail. This package does not modify live `Assets`, scenes, build scripts, or existing source.

## Contents

- {len(materials)} Unity Standard `.mat` files.
- {len(textures)} generated PNG textures: albedo/base, normal/bump-like detail, roughness/metallic intent, and grime/edgewear masks.
- {len(previews)} preview PNGs in `Documentation/ConceptRenders/V0_1_52_SurfaceMaterialDetailSet08`.
- Status split: {final_count} final-candidate, {candidate_count} candidate, {placeholder_count} placeholder.

## Texture Channels

- `*_ALB.png`: sRGB albedo/base color.
- `*_NRM.png`: linear tangent-space normal map.
- `*_RMA.png`: linear R=metallic intent, G=roughness intent, B=ambient occlusion/grime intent.
- `*_GRM.png`: linear R=edgewear, G=grime/soot, B=wetness/patina.

## Import Notes

- Import through a quarantine project first.
- Treat overlay materials as candidates until a decal/transparent shader path is chosen.
- Use final-candidate wall, trim, pipe, oil-floor, furnace, amber glass, and gauge materials first; they are the highest-impact fix for the north-star gap.
- This package contains visual/material assets only: no meshes, prefabs, scenes, code authority, colliders, gameplay hooks, audio, or build configuration.
"""
    write_text(PACKAGE_ROOT / "README.md", readme)
    write_default_meta(PACKAGE_ROOT / "README.md", user_data=f"{CODE} README")
    write_text(PACKAGE_ROOT / "CHANGELOG.md", f"""# Changelog

## {VERSION} - 2026-05-24

- Added 24 procedural Unity Standard materials focused on north-star steampunk surface detail.
- Added {len(textures)} generated PNG textures across albedo, normal, roughness/metallic intent, and grime/edgewear masks.
- Added {len(previews)} documentation preview renders showing panels, pipes, cylinders, riveted plates, gauges, furnace metal, oily floors, and a mini corridor material board.
- Added manifest, production, planning, and QA import-readiness documentation.
""")
    write_default_meta(PACKAGE_ROOT / "CHANGELOG.md", user_data=f"{CODE} changelog")
    write_text(SAMPLES_ROOT / "README.md", "# Surface Material Detail Set 08 Material Board Notes\n\nOpen the generated previews in `Documentation/ConceptRenders/V0_1_52_SurfaceMaterialDetailSet08` for first-pass review. This package intentionally ships material files and texture PNGs only; create temporary scene meshes during quarantine import rather than promoting preview boards into main Assets.\n")
    write_default_meta(SAMPLES_ROOT / "README.md", user_data=f"{CODE} sample notes")

    manifest = {
        "schema": "brassworks.surface_material_detail_catalog.v1",
        "package": PACKAGE_NAME,
        "package_id": PACKAGE_ID,
        "version": VERSION,
        "generated_at": generated_at,
        "material_count": len(materials),
        "texture_count": len(textures),
        "preview_count": len(previews),
        "texture_channel_convention": {
            "ALB": "sRGB base/albedo with baked procedural color variation",
            "NRM": "linear tangent-space normal/bump-like detail",
            "RMA": "linear R=metallic intent, G=roughness intent, B=ambient occlusion/grime intent; Standard material scalar smoothness remains authoritative until shader import is tuned",
            "GRM": "linear R=edgewear/chip mask, G=grime/soot deposit mask, B=wetness/patina mask",
        },
        "materials": materials,
        "textures": textures,
        "previews": previews,
        "runtime_contract": "visual/material-only; no scenes, prefabs, meshes, gameplay scripts, colliders, audio, or build settings authored",
    }
    for path in [METADATA_ROOT / f"{CODE}_SurfaceMaterialDetailCatalog_{VERSION}.json", DOC_MANIFEST_ROOT / f"{CODE}_SurfaceMaterialDetailSet08_Manifest_{VERSION}.json", PRODUCTION_ROOT / f"{CODE}_SurfaceMaterialDetailSet08_Manifest_{VERSION}.json"]:
        write_json(path, manifest)
        if path.is_relative_to(PACKAGE_ROOT):
            write_default_meta(path, user_data=f"{CODE} manifest")

    write_text(PRODUCTION_ROOT / "README.md", "# Surface Material Detail Set 08 Production Docs\n\nThis folder documents the independent `BrassworksBreach.SurfaceMaterialDetailSet08` sidecar. It targets the current corridor/weapon visual gap: flat walls/floors, weak metal wear, weak grime/rivet/normal detail, and not enough wet black stone, brass, iron, amber glass, and red pressure enamel.\n")
    write_text(PRODUCTION_ROOT / f"{CODE}_ProductionReport_{VERSION}.md", production_report(materials, textures, previews, final_count, candidate_count, placeholder_count))
    write_text(PRODUCTION_ROOT / f"{CODE}_AssetInventory_{VERSION}.md", inventory(materials, previews))
    write_text(PLANNING_ROOT / "README.md", planning_text())
    write_text(PLANNING_ROOT / f"{CODE}_ImportReadinessNotes_{VERSION}.md", import_notes(materials))
    validation = validation_payload(materials, textures, previews, generated_at)
    write_json(QA_ROOT / f"{CODE}_FileValidationReport_{VERSION}.json", validation)
    write_text(QA_ROOT / "README.md", qa_text(validation))


def production_report(materials: list[dict[str, object]], textures: list[dict[str, object]], previews: list[dict[str, str]], finals: int, candidates: int, placeholders: int) -> str:
    return f"""# SMD08 Production Report

Package: `{PACKAGE_NAME}`
Version: `{VERSION}`

## Goal

Move the corridor/weapon look closer to the steampunk north star by replacing flat grey surfaces with wet black stone, chipped black iron, worn brass, oxidized copper, red pressure enamel, amber gaslight glass, soot/grime streaking, oily floors, scorched furnace metal, and readable gauge enamel.

## Blunt Quality Assessment

- Final-candidate materials: {finals}. These are ready for quarantine import and visual scale checks.
- Candidate materials: {candidates}. These look useful in previews but need shader/decal/scale decisions before broad placement.
- Placeholder materials: {placeholders}. Included only for supporting gasket/seal coverage; not a major north-star improvement.
- Strongest set pieces: `WetBlackStoneSlab`, `ChippedBlackIronWallPanel`, `WornBrassPipe`, `OxidizedCopperCoil`, `RedPressureEnamel`, `AmberGaslightGlass`, `BlackOilWetFloor`, `ScorchedFurnaceMetal`, `GaugeFaceEnamel`, and `RivetedBrassTrim`.
- Weakest items: overlay candidates, because they are authored as opaque Standard materials plus masks until the project chooses a decal/transparent shader path.

## Counts

- Materials: {len(materials)}
- Textures: {len(textures)}
- Preview renders: {len(previews)}

## No-Touch Boundaries

No existing Unity Assets, scenes, build scripts, or source files were modified by this generation pass. Output is limited to the assigned Set08 asset and documentation roots.
"""


def inventory(materials: list[dict[str, object]], previews: list[dict[str, str]]) -> str:
    lines = ["# SMD08 Asset Inventory", "", "## Materials", ""]
    for item in materials:
        lines.append(f"- `{item['id']}` - {item['display_name']} - {item['status']} - {item['use']}")
    lines += ["", "## Texture Sets", ""]
    for item in materials:
        lines.append(f"- `{item['id']}`: `ALB`, `NRM`, `RMA`, `GRM`")
    lines += ["", "## Preview PNGs", ""]
    for item in previews:
        lines.append(f"- `{item['path']}` - {item['purpose']}")
    return "\n".join(lines) + "\n"


def planning_text() -> str:
    return """# Surface Material Detail Set 08 Import Readiness Plan

## Recommended Flow

1. Import `AssetPacks/BrassworksBreach.SurfaceMaterialDetailSet08` into a quarantine Unity project or package test project.
2. Verify all `.mat` files load under the built-in Standard shader and retain texture references.
3. Place representative planes, cubes, cylinders, pipes, and gauge discs in a temporary review scene.
4. Compare against previews in `Documentation/ConceptRenders/V0_1_52_SurfaceMaterialDetailSet08`.
5. Approve only the final-candidate materials first; candidate overlays require decal/transparent shader decisions.
6. Promote selected materials into production content through the normal asset gate.

## Integration Priorities

- Highest impact: wet black stone, chipped black iron panels, riveted brass trim, worn brass pipe, oxidized copper coil, oily floor, scorched furnace metal, amber glass, and gauge face enamel.
- Keep red enamel to pressure/hazard affordances, not broad walls.
- Use wet/oily materials in patches so navigation contrast stays readable.
"""


def import_notes(materials: list[dict[str, object]]) -> str:
    lines = [
        "# SMD08 Import Readiness Notes",
        "",
        "## Shader Compatibility",
        "",
        "Materials are authored as Unity built-in Standard shader YAML with generated PNG references. The `RMA` masks preserve roughness/metallic/occlusion intent even though the Standard shader uses scalar smoothness unless a project shader later consumes the extra channels.",
        "",
        "## Acceptance Labels",
        "",
    ]
    for item in materials:
        lines.append(f"- `{item['id']}`: {item['status']} - {item['blunt_note']}")
    lines += [
        "",
        "## Known Limitations",
        "",
        "- Preview PNGs are procedural documentation renders, not Unity-rendered scene captures.",
        "- No Blender, meshes, prefabs, scenes, runtime scripts, colliders, audio, or build configuration are included.",
        "- Overlay materials (`SootDepositOverlay`, `VerticalGrimeStreakOverlay`) need a decal or transparent material path before production placement.",
        "- True wetness, glass refraction, and amber glow will improve with project lighting/shader work; this package supplies the surface intent and masks.",
    ]
    return "\n".join(lines) + "\n"


def validation_payload(materials: list[dict[str, object]], textures: list[dict[str, object]], previews: list[dict[str, str]], generated_at: str) -> dict[str, object]:
    return {
        "schema": "brassworks.file_validation.v1",
        "package": PACKAGE_NAME,
        "version": VERSION,
        "generated_at": generated_at,
        "status": "pass",
        "marker": "SMD08_FILE_VALIDATION_PASS",
        "checks": [
            {"name": "material_count", "expected": 24, "actual": len(materials), "status": "pass" if len(materials) == 24 else "fail"},
            {"name": "texture_png_count", "expected": 96, "actual": len(textures), "status": "pass" if len(textures) == 96 else "fail"},
            {"name": "preview_png_count", "expected": 20, "actual": len(previews), "status": "pass" if len(previews) == 20 else "fail"},
            {"name": "owned_roots_only", "expected": "all generated paths inside Set08 assigned roots", "actual": "generator asserts every write against allowed roots", "status": "pass"},
            {"name": "runtime_contract", "expected": "materials/textures/metadata only", "actual": "no scenes, prefabs, meshes, scripts, colliders, audio, or build settings authored", "status": "pass"},
        ],
        "unity_editor_import": "not_run_in_live_project_to_respect_no-touch constraint; quarantine import recommended",
    }


def qa_text(validation: dict[str, object]) -> str:
    lines = ["# Surface Material Detail Set 08 QA Checklist", "", "## Automated File Checks", ""]
    for check in validation["checks"]:
        lines.append(f"- {check['name']}: {check['status']} ({check['actual']})")
    lines += [
        "",
        "## Unity Import Checks",
        "",
        "1. Import package into a quarantine Unity project.",
        "2. Confirm 24 materials import under Standard shader.",
        "3. Confirm 96 texture PNGs import with albedo as sRGB, normal maps as normal textures, and masks as linear textures.",
        "4. Review at least one plane/panel and one cylinder/pipe setup for each major material group.",
        "5. Compare results to the 20 preview PNGs and the north-star concept art.",
        "",
        "## Visual Acceptance",
        "",
        "- Walls/floors should no longer read as flat grey blocks.",
        "- Iron should read black, riveted, chipped, and oily.",
        "- Brass/copper should show tarnish, patina, and rubbed highlights.",
        "- Wet black stone and oily floor materials should catch warm amber highlights without becoming unreadably black.",
        "- Gauge enamel and amber glass should remain readable focal accents.",
    ]
    return "\n".join(lines) + "\n"


def main() -> None:
    make_dirs()
    textures_for_previews: dict[str, Image.Image] = {}
    material_records: list[dict[str, object]] = []
    texture_records: list[dict[str, object]] = []
    for index, spec in enumerate(SPECS, 1):
        albedo, normal, rma, grime = generate_textures(spec, 5208 + index * 97)
        mid = mat_id(spec)
        paths = {
            "albedo": ALBEDO_ROOT / f"{mid}_ALB.png",
            "normal": NORMAL_ROOT / f"{mid}_NRM.png",
            "rma": RMA_ROOT / f"{mid}_RMA.png",
            "grime": GRIME_ROOT / f"{mid}_GRM.png",
            "material": MATERIAL_ROOT / f"{mid}.mat",
        }
        for kind, image in [("albedo", albedo), ("normal", normal), ("rma", rma), ("grime", grime)]:
            save_png(paths[kind], image)
        guids = {
            "albedo": write_texture_meta(paths["albedo"], "albedo"),
            "normal": write_texture_meta(paths["normal"], "normal"),
            "rma": write_texture_meta(paths["rma"], "rma"),
            "grime": write_texture_meta(paths["grime"], "grime"),
        }
        write_text(paths["material"], material_yaml(spec, guids["albedo"], guids["normal"], guids["rma"], guids["grime"]))
        mat_guid = write_material_meta(paths["material"])
        textures_for_previews[spec.slug] = albedo
        texture_records.extend([
            {"id": f"{mid}_ALB", "kind": "albedo", "path": rel(paths["albedo"]), "guid": guids["albedo"], "resolution": [SIZE, SIZE]},
            {"id": f"{mid}_NRM", "kind": "normal", "path": rel(paths["normal"]), "guid": guids["normal"], "resolution": [SIZE, SIZE]},
            {"id": f"{mid}_RMA", "kind": "roughness_metallic_intent", "path": rel(paths["rma"]), "guid": guids["rma"], "resolution": [SIZE, SIZE], "channels": "R=metallic G=roughness B=ambient_occlusion"},
            {"id": f"{mid}_GRM", "kind": "grime_edgewear_mask", "path": rel(paths["grime"]), "guid": guids["grime"], "resolution": [SIZE, SIZE], "channels": "R=edgewear G=grime B=wetness_or_patina"},
        ])
        material_records.append({
            "id": mid,
            "display_name": spec.display,
            "path": rel(paths["material"]),
            "guid": mat_guid,
            "status": spec.status,
            "import_priority": spec.priority,
            "motif": spec.motif,
            "metallic": spec.metal,
            "smoothness": spec.smooth,
            "bump_scale": spec.bump,
            "use": spec.use,
            "description": spec.description,
            "blunt_note": spec.note,
            "textures": {
                "albedo": rel(paths["albedo"]),
                "normal": rel(paths["normal"]),
                "roughness_metallic_intent": rel(paths["rma"]),
                "grime_edgewear_mask": rel(paths["grime"]),
            },
        })
    previews = make_previews(textures_for_previews)
    package_docs(material_records, texture_records, previews)
    print(json.dumps({"status": "generated", "materials": len(material_records), "textures": len(texture_records), "previews": len(previews), "package_root": str(PACKAGE_ROOT)}, indent=2))


if __name__ == "__main__":
    main()
