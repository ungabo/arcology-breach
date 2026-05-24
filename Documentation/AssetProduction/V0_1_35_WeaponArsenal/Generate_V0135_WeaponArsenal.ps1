$ErrorActionPreference = "Stop"

$root = "D:\__MY APPS\Unity Doom"
$assetRoot = Join-Path $root "Assets/_Project/ArtStaging/V0_1_35_WeaponArsenal"
$meshRoot = Join-Path $assetRoot "Meshes"
$matRoot = Join-Path $assetRoot "Materials"
$manifestRoot = Join-Path $assetRoot "Manifests"
$docRoot = Join-Path $root "Documentation/AssetProduction/V0_1_35_WeaponArsenal"
$renderRoot = Join-Path $root "Documentation/ConceptRenders/V0_1_35_WeaponArsenal"
$unityProofRoot = Join-Path $assetRoot "UnityProof"

foreach ($path in @($assetRoot,$meshRoot,$matRoot,$manifestRoot,$docRoot,$renderRoot,$unityProofRoot)) {
    New-Item -ItemType Directory -Force -Path $path | Out-Null
}

$materials = @(
    @{ key="aged_brass"; kd="0.78 0.55 0.22"; ks="0.72 0.60 0.36"; ns=72; note="Dominant brass frame, softened with grime in cavities." },
    @{ key="blackened_iron"; kd="0.06 0.065 0.06"; ks="0.22 0.22 0.20"; ns=38; note="Iron plates, barrels, brackets, and collision-readable silhouettes." },
    @{ key="heat_stained_steel"; kd="0.38 0.33 0.30"; ks="0.62 0.52 0.44"; ns=85; note="Hot muzzle sleeves, breech collars, and pressure chamber edges." },
    @{ key="copper_pressure_line"; kd="0.80 0.34 0.16"; ks="0.55 0.31 0.18"; ns=64; note="Copper pipes, valves, bypass coils, and cartridges." },
    @{ key="walnut_leather"; kd="0.24 0.12 0.055"; ks="0.17 0.10 0.06"; ns=24; note="Grip panels, shoulder stock, straps, and cabinet pulls." },
    @{ key="cream_gauge_face"; kd="0.86 0.78 0.57"; ks="0.18 0.16 0.12"; ns=18; note="Readable gauge discs with black needle overlays." },
    @{ key="amber_glass"; kd="1.00 0.55 0.11"; ks="0.95 0.74 0.35"; ns=92; note="Lamps, vial windows, hot pressure indicators." },
    @{ key="route_green"; kd="0.07 0.42 0.27"; ks="0.16 0.35 0.25"; ns=36; note="Gameplay affordance green for stocked/usable panels." },
    @{ key="warning_red"; kd="0.73 0.08 0.045"; ks="0.32 0.11 0.08"; ns=30; note="Danger tabs, high pressure bands, empty slot callouts." },
    @{ key="oil_soot"; kd="0.015 0.013 0.010"; ks="0.04 0.035 0.025"; ns=6; note="Soot cards and grime masks near vents, barrels, lower cabinets." },
    @{ key="polished_wear"; kd="0.96 0.83 0.46"; ks="0.92 0.86 0.56"; ns=110; note="Edge wear decals and screw heads for close first-person read." }
)

$mtl = @("# V0.1.35 brassworks weapon arsenal staging material proxy", "# Unity import: keep material names; replace with URP/HDRP equivalents during integration.")
foreach ($m in $materials) {
    $mtl += "newmtl $($m.key)"
    $mtl += "Kd $($m.kd)"
    $mtl += "Ks $($m.ks)"
    $mtl += "Ns $($m.ns)"
    $mtl += "d 1.0"
    $mtl += "illum 2"
    $mtl += ""
}
Set-Content -Path (Join-Path $meshRoot "V0135WA_Brassworks_ArsenalPalette.mtl") -Value $mtl -Encoding ASCII

function New-MatYaml([string]$name, [string]$rgb, [float]$metallic, [float]$smoothness) {
    $parts = $rgb.Split(" ") | ForEach-Object { [double]$_ }
    $guid = -join ((1..32) | ForEach-Object { "{0:x}" -f (Get-Random -Minimum 0 -Maximum 16) })
    @"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!21 &2100000
Material:
  serializedVersion: 8
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_Name: M_V0135WA_$name
  m_Shader: {fileID: 4800000, guid: 0000000000000000f000000000000000, type: 0}
  m_ValidKeywords: []
  m_InvalidKeywords: []
  m_LightmapFlags: 4
  m_EnableInstancingVariants: 0
  m_DoubleSidedGI: 0
  m_CustomRenderQueue: -1
  stringTagMap: {}
  disabledShaderPasses: []
  m_SavedProperties:
    serializedVersion: 3
    m_TexEnvs: []
    m_Ints: []
    m_Floats:
    - _Metallic: $metallic
    - _Glossiness: $smoothness
    m_Colors:
    - _Color: {r: $($parts[0]), g: $($parts[1]), b: $($parts[2]), a: 1}
"@ | Set-Content -Path (Join-Path $matRoot "M_V0135WA_$name.mat") -Encoding ASCII
}
foreach ($m in $materials) {
    $metal = if ($m.key -match "brass|iron|steel|copper|wear") { 0.75 } else { 0.05 }
    $smooth = if ($m.key -match "glass|wear|steel") { 0.68 } elseif ($m.key -match "soot|leather") { 0.18 } else { 0.42 }
    New-MatYaml $m.key $m.kd $metal $smooth
}

function New-ObjBuilder([string]$fileName) {
    return [ordered]@{
        fileName = $fileName
        lines = New-Object System.Collections.Generic.List[string]
        vertices = New-Object System.Collections.Generic.List[string]
        faces = New-Object System.Collections.Generic.List[string]
        index = 1
    }
}

function Add-Cube($b, [string]$name, [string]$mat, [double]$cx, [double]$cy, [double]$cz, [double]$sx, [double]$sy, [double]$sz) {
    $x0=$cx-$sx/2; $x1=$cx+$sx/2; $y0=$cy-$sy/2; $y1=$cy+$sy/2; $z0=$cz-$sz/2; $z1=$cz+$sz/2
    $verts = @(
        @($x0,$y0,$z0), @($x1,$y0,$z0), @($x1,$y1,$z0), @($x0,$y1,$z0),
        @($x0,$y0,$z1), @($x1,$y0,$z1), @($x1,$y1,$z1), @($x0,$y1,$z1)
    )
    $start = $b.index
    $b.lines.Add("o $name") | Out-Null
    $b.lines.Add("usemtl $mat") | Out-Null
    foreach ($v in $verts) { $b.lines.Add(("v {0:N4} {1:N4} {2:N4}" -f $v[0],$v[1],$v[2])) | Out-Null }
    $f = @(
        @($start,($start+1),($start+2),($start+3)), @(($start+4),($start+7),($start+6),($start+5)),
        @($start,($start+4),($start+5),($start+1)), @(($start+1),($start+5),($start+6),($start+2)),
        @(($start+2),($start+6),($start+7),($start+3)), @(($start+3),($start+7),($start+4),$start)
    )
    foreach ($face in $f) { $b.lines.Add(("f {0} {1} {2} {3}" -f $face[0],$face[1],$face[2],$face[3])) | Out-Null }
    $b.index += 8
}

function Add-Cylinder($b, [string]$name, [string]$mat, [double]$cx, [double]$cy, [double]$cz, [double]$radius, [double]$length, [string]$axis="X", [int]$segments=16) {
    $b.lines.Add("o $name") | Out-Null
    $b.lines.Add("usemtl $mat") | Out-Null
    $start = $b.index
    for ($i=0; $i -lt $segments; $i++) {
        $a = 2 * [Math]::PI * $i / $segments
        $u = [Math]::Cos($a) * $radius
        $v = [Math]::Sin($a) * $radius
        if ($axis -eq "X") {
            $b.lines.Add(("v {0:N4} {1:N4} {2:N4}" -f ($cx-$length/2),($cy+$u),($cz+$v))) | Out-Null
            $b.lines.Add(("v {0:N4} {1:N4} {2:N4}" -f ($cx+$length/2),($cy+$u),($cz+$v))) | Out-Null
        } elseif ($axis -eq "Y") {
            $b.lines.Add(("v {0:N4} {1:N4} {2:N4}" -f ($cx+$u),($cy-$length/2),($cz+$v))) | Out-Null
            $b.lines.Add(("v {0:N4} {1:N4} {2:N4}" -f ($cx+$u),($cy+$length/2),($cz+$v))) | Out-Null
        } else {
            $b.lines.Add(("v {0:N4} {1:N4} {2:N4}" -f ($cx+$u),($cy+$v),($cz-$length/2))) | Out-Null
            $b.lines.Add(("v {0:N4} {1:N4} {2:N4}" -f ($cx+$u),($cy+$v),($cz+$length/2))) | Out-Null
        }
    }
    for ($i=0; $i -lt $segments; $i++) {
        $a = $start + ($i*2)
        $b2 = $start + ((($i+1) % $segments)*2)
        $b.lines.Add(("f {0} {1} {2} {3}" -f $a,($a+1),($b2+1),$b2)) | Out-Null
    }
    $b.lines.Add(("f " + (($start..($start+($segments-1)*2)) | Where-Object { ($_ - $start) % 2 -eq 0 }))) | Out-Null
    $b.lines.Add(("f " + (($start..($start+($segments-1)*2+1)) | Where-Object { ($_ - $start) % 2 -eq 1 }))) | Out-Null
    $b.index += $segments * 2
}

function Save-Obj($b) {
    $out = @("# V0.1.35 Weapon Arsenal staging mesh", "mtllib V0135WA_Brassworks_ArsenalPalette.mtl", "s off") + $b.lines
    Set-Content -Path (Join-Path $meshRoot $b.fileName) -Value $out -Encoding ASCII
}

function Make-PressurePistol {
    $b = New-ObjBuilder "V0135WA_001_PressurePistol_FinalComponentPack.obj"
    Add-Cube $b "brass_frame_split_receiver" "aged_brass" 0 0 0 1.15 0.34 0.38
    Add-Cylinder $b "barrel_heat_stained_main_tube" "heat_stained_steel" 0.78 0 0 0.115 1.25 "X" 20
    Add-Cylinder $b "barrel_blackened_inner_bore" "blackened_iron" 1.43 0 0 0.075 0.18 "X" 20
    Add-Cylinder $b "underbarrel_copper_pressure_chamber" "copper_pressure_line" 0.55 -0.20 0 0.09 0.92 "X" 16
    Add-Cylinder $b "top_recoil_coil_01" "polished_wear" 0.10 0.26 0 0.05 0.92 "X" 12
    Add-Cylinder $b "top_recoil_coil_02" "aged_brass" -0.14 0.26 0 0.055 0.16 "X" 12
    Add-Cylinder $b "top_recoil_coil_03" "aged_brass" 0.12 0.26 0 0.055 0.16 "X" 12
    Add-Cylinder $b "cream_pressure_gauge_disc_left" "cream_gauge_face" -0.30 0.10 -0.23 0.10 0.035 "Z" 20
    Add-Cylinder $b "black_gauge_rim_left" "blackened_iron" -0.30 0.10 -0.255 0.115 0.022 "Z" 20
    Add-Cube $b "gauge_needle_left" "warning_red" -0.30 0.135 -0.278 0.018 0.070 0.010
    Add-Cube $b "walnut_leather_wrapped_grip" "walnut_leather" -0.52 -0.45 0 0.25 0.78 0.27
    Add-Cube $b "blackened_trigger_guard" "blackened_iron" -0.18 -0.34 0 0.42 0.09 0.09
    Add-Cube $b "trigger_warning_red_tab" "warning_red" -0.08 -0.46 0 0.07 0.18 0.045
    Add-Cylinder $b "amber_pressure_lamp_right" "amber_glass" 0.08 0.10 0.245 0.075 0.08 "Z" 16
    foreach ($x in @(-0.48,-0.20,0.10,0.38,0.66)) { Add-Cylinder $b "polished_rivet_$x" "polished_wear" $x 0.18 -0.205 0.026 0.018 "Z" 10 }
    Add-Cube $b "oil_soot_muzzle_wear_card" "oil_soot" 1.36 -0.045 0 0.12 0.055 0.25
    Save-Obj $b
}

function Make-Scattergun {
    $b = New-ObjBuilder "V0135WA_002_SteamScattergun_FinalComponentPack.obj"
    Add-Cube $b "blackened_iron_receiver_box" "blackened_iron" 0 0 0 1.25 0.42 0.52
    Add-Cube $b "aged_brass_receiver_side_plates" "aged_brass" 0 0.02 -0.285 1.36 0.34 0.05
    Add-Cube $b "aged_brass_receiver_side_plates_R" "aged_brass" 0 0.02 0.285 1.36 0.34 0.05
    foreach ($z in @(-0.18,0,0.18)) { Add-Cylinder $b "triple_heat_barrel_z$z" "heat_stained_steel" 1.02 0.05 $z 0.095 1.50 "X" 20 }
    Add-Cylinder $b "lower_steam_expansion_tank" "copper_pressure_line" 0.55 -0.30 0 0.145 1.18 "X" 18
    Add-Cylinder $b "rear_pressure_dial_large" "cream_gauge_face" -0.55 0.19 -0.32 0.13 0.035 "Z" 24
    Add-Cylinder $b "rear_pressure_dial_rim" "aged_brass" -0.55 0.19 -0.345 0.15 0.025 "Z" 24
    Add-Cube $b "pump_grip_walnut_rib_01" "walnut_leather" 0.45 -0.12 0 0.50 0.10 0.62
    Add-Cube $b "pump_grip_walnut_rib_02" "walnut_leather" 0.45 -0.23 0 0.45 0.08 0.58
    Add-Cube $b "shoulder_stock_walnut_core" "walnut_leather" -0.95 -0.10 0 0.78 0.30 0.40
    Add-Cube $b "stock_brass_butt_plate" "aged_brass" -1.36 -0.10 0 0.08 0.40 0.48
    Add-Cylinder $b "left_copper_bypass_coil" "copper_pressure_line" -0.10 0.31 -0.34 0.045 1.05 "X" 14
    Add-Cylinder $b "right_copper_bypass_coil" "copper_pressure_line" -0.10 0.31 0.34 0.045 1.05 "X" 14
    Add-Cube $b "route_green_loaded_indicator" "route_green" -0.12 0.275 0 0.28 0.06 0.16
    Add-Cube $b "oil_soot_muzzle_smear" "oil_soot" 1.74 0.05 0 0.10 0.29 0.48
    foreach ($x in @(-0.58,-0.32,-0.06,0.20,0.46,0.72)) { Add-Cylinder $b "side_rivet_row_$x" "polished_wear" $x 0.20 -0.326 0.022 0.018 "Z" 10 }
    Save-Obj $b
}

function Make-Cartridges {
    $b = New-ObjBuilder "V0135WA_003_PressureCartridgeFamily.obj"
    $offsets = @(-0.78,-0.39,0,0.39,0.78)
    $names = @("pistol_short_cell","scattergun_slug_canister","ruptured_empty_cell","high_pressure_redband_cell","display_cutaway_cell")
    for ($i=0; $i -lt $offsets.Count; $i++) {
        $x=$offsets[$i]
        Add-Cylinder $b "$($names[$i])_copper_body" "copper_pressure_line" $x 0 0 0.09 0.34 "Y" 18
        Add-Cylinder $b "$($names[$i])_brass_cap_top" "aged_brass" $x 0.20 0 0.095 0.055 "Y" 18
        Add-Cylinder $b "$($names[$i])_blackened_base" "blackened_iron" $x -0.20 0 0.096 0.055 "Y" 18
        $labelMat = "cream_gauge_face"
        if ($i -eq 3) { $labelMat = "warning_red" }
        Add-Cube $b "$($names[$i])_label_band" $labelMat $x 0.02 -0.095 0.16 0.10 0.018
    }
    Add-Cube $b "small_rack_brass_rail" "aged_brass" 0 -0.28 0 1.95 0.045 0.20
    Save-Obj $b
}

function Make-DisplayFrame {
    $b = New-ObjBuilder "V0135WA_004_WallWeaponDisplayFrame.obj"
    Add-Cube $b "wall_mount_blackened_backplate" "blackened_iron" 0 0 0 2.40 1.25 0.10
    Add-Cube $b "aged_brass_top_rail" "aged_brass" 0 0.67 0.07 2.55 0.10 0.12
    Add-Cube $b "aged_brass_bottom_rail" "aged_brass" 0 -0.67 0.07 2.55 0.10 0.12
    Add-Cube $b "left_vertical_brass_frame" "aged_brass" -1.27 0 0.07 0.10 1.36 0.12
    Add-Cube $b "right_vertical_brass_frame" "aged_brass" 1.27 0 0.07 0.10 1.36 0.12
    foreach ($x in @(-0.70,0.70)) { Add-Cube $b "weapon_hook_pair_$x" "blackened_iron" $x -0.12 0.22 0.14 0.48 0.16 }
    Add-Cube $b "cream_label_plate_pressure_pistol" "cream_gauge_face" -0.58 -0.50 0.14 0.68 0.16 0.035
    Add-Cube $b "cream_label_plate_scattergun" "cream_gauge_face" 0.62 -0.50 0.14 0.78 0.16 0.035
    Add-Cylinder $b "amber_display_lamp_left" "amber_glass" -1.05 0.48 0.18 0.075 0.055 "Z" 16
    Add-Cylinder $b "amber_display_lamp_right" "amber_glass" 1.05 0.48 0.18 0.075 0.055 "Z" 16
    foreach ($x in @(-1.10,-0.55,0,0.55,1.10)) { Add-Cylinder $b "frame_screw_$x" "polished_wear" $x 0.59 0.145 0.025 0.020 "Z" 10 }
    Save-Obj $b
}

function Make-AmmoCabinet {
    $b = New-ObjBuilder "V0135WA_005_AmmoCabinetVendingProp.obj"
    Add-Cube $b "cabinet_blackened_iron_shell" "blackened_iron" 0 0 0 1.10 1.70 0.55
    Add-Cube $b "aged_brass_front_frame" "aged_brass" 0 0 0.31 1.20 1.80 0.08
    Add-Cube $b "route_green_stock_window" "route_green" -0.22 0.34 0.37 0.46 0.44 0.04
    Add-Cube $b "amber_glass_low_pressure_window" "amber_glass" 0.34 0.34 0.37 0.30 0.44 0.04
    Add-Cube $b "dispense_slot_blackened" "blackened_iron" 0 -0.48 0.39 0.76 0.18 0.06
    Add-Cylinder $b "coin_valve_copper_wheel" "copper_pressure_line" 0.42 -0.13 0.40 0.13 0.055 "Z" 20
    Add-Cylinder $b "pressure_gauge_cabinet" "cream_gauge_face" -0.40 -0.12 0.40 0.13 0.035 "Z" 20
    Add-Cube $b "warning_red_empty_flag" "warning_red" 0 -0.78 0.40 0.46 0.10 0.04
    Add-Cube $b "walnut_pull_handle" "walnut_leather" 0 -0.62 0.45 0.44 0.08 0.09
    Add-Cylinder $b "left_pressure_pipe" "copper_pressure_line" -0.66 0.08 0 0.045 1.50 "Y" 12
    Add-Cylinder $b "right_pressure_pipe" "copper_pressure_line" 0.66 0.08 0 0.045 1.50 "Y" 12
    Add-Cube $b "oil_soot_floor_smudge_panel" "oil_soot" 0 -0.86 0.02 0.92 0.06 0.45
    Save-Obj $b
}

function Make-FutureAlt {
    $b = New-ObjBuilder "V0135WA_006_FutureAltLightningLance_Silhouette.obj"
    Add-Cube $b "long_brass_spine" "aged_brass" 0 0 0 2.05 0.16 0.18
    Add-Cylinder $b "forward_arc_prong_left" "heat_stained_steel" 1.22 0.11 -0.12 0.045 0.84 "X" 14
    Add-Cylinder $b "forward_arc_prong_right" "heat_stained_steel" 1.22 0.11 0.12 0.045 0.84 "X" 14
    Add-Cylinder $b "central_glass_accumulator" "amber_glass" 0.18 0.20 0 0.13 0.72 "X" 18
    Add-Cylinder $b "rear_copper_induction_coil" "copper_pressure_line" -0.52 0.20 0 0.07 0.72 "X" 14
    Add-Cube $b "walnut_forward_grip" "walnut_leather" 0.28 -0.30 0 0.26 0.48 0.18
    Add-Cube $b "stock_iron_folding_strut" "blackened_iron" -1.10 -0.16 0 0.56 0.08 0.16
    Add-Cube $b "route_green_charge_window" "route_green" -0.16 0.05 0.12 0.28 0.06 0.05
    Add-Cube $b "warning_red_overload_switch" "warning_red" -0.78 0.00 -0.13 0.16 0.06 0.04
    Add-Cube $b "oil_soot_arc_scarring" "oil_soot" 1.56 0.12 0 0.16 0.18 0.34
    Save-Obj $b
}

Make-PressurePistol
Make-Scattergun
Make-Cartridges
Make-DisplayFrame
Make-AmmoCabinet
Make-FutureAlt

$recipes = [ordered]@{
    package = "V0_1_35_WeaponArsenal"
    visual_direction = "Heavy steampunk brassworks: layered aged brass, blackened iron, copper pressure plumbing, cream gauges, amber glass, readable red/green gameplay affordances, and localized soot/wear passes."
    unity_only_policy = "All proof assets are authored as Unity-importable OBJ/MTL/material proxies. Preview sheets are procedural documentation images; no Blender or external DCC render dependency."
    materials = $materials
    shared_import_notes = @(
        "OBJ units are authored as meters. Keep scale factor at 1.0 on import.",
        "Generate colliders from simple boxes/capsules in Unity; do not use render mesh as collision.",
        "Replace proxy Standard materials with final project shader variants after import.",
        "Preserve object names: they encode integration components for sockets, VFX anchors, and wear passes."
    )
}
$recipes | ConvertTo-Json -Depth 6 | Set-Content -Path (Join-Path $docRoot "V0_1_35_WEAPON_ARSENAL_MATERIAL_RECIPES.json") -Encoding ASCII

$manifest = [ordered]@{
    package = "V0_1_35_WeaponArsenal"
    created_for = "v0.1.35 weapon + gameplay prop arsenal staging"
    owned_scopes = @(
        "Assets/_Project/ArtStaging/V0_1_35_WeaponArsenal/",
        "Documentation/AssetProduction/V0_1_35_WeaponArsenal/",
        "Documentation/ConceptRenders/V0_1_35_WeaponArsenal/"
    )
    assets = @(
        @{ id="V0135WA_001"; name="Pressure Pistol Final Component Pack"; mesh="Meshes/V0135WA_001_PressurePistol_FinalComponentPack.obj"; role="first-person weapon/viewmodel and pickup basis"; scale_m="1.55 length"; collision="3 capsule/box primitives: barrel, receiver, grip"; lod="LOD0 component mesh, LOD1 merge rivets/coils, LOD2 silhouette receiver+barrel"; gates=@("Readable pistol silhouette at 8m pickup scale","Gauge/lamp remain visible in first-person crop","No collider generated from decorative coils") },
        @{ id="V0135WA_002"; name="Steam Scattergun Final Component Pack"; mesh="Meshes/V0135WA_002_SteamScattergun_FinalComponentPack.obj"; role="heavy shotgun/scattergun viewmodel and wall display"; scale_m="2.15 length"; collision="5 primitives: stock, receiver, barrel cluster, tank, pump"; lod="LOD0 full, LOD1 remove rivets/gauge needles, LOD2 single barrel block"; gates=@("Triple barrel reads from front","Pump/stock grip volumes fit hand sockets","Soot pass does not hide muzzle direction") },
        @{ id="V0135WA_003"; name="Pressure Cartridge Family"; mesh="Meshes/V0135WA_003_PressureCartridgeFamily.obj"; role="ammo pickups, cabinet contents, UI icon source"; scale_m="0.34 cartridge height"; collision="capsule per cartridge or one pickup trigger box"; lod="LOD0 individual cartridges, LOD1 single rack strip"; gates=@("Red high-pressure cartridge distinct from standard","Empty/ruptured cartridge readable as spent","Pickup trigger larger than visual mesh") },
        @{ id="V0135WA_004"; name="Wall Weapon Display Frame"; mesh="Meshes/V0135WA_004_WallWeaponDisplayFrame.obj"; role="armory wall prop and weapon pickup framing"; scale_m="2.55 x 1.36"; collision="single thin box for backplate plus optional hooks"; lod="LOD0 full frame, LOD1 remove labels/lamps"; gates=@("Hooks align with pistol/scattergun mounts","Green/red gameplay labels can be swapped by state","Backplate stays behind level wall collision") },
        @{ id="V0135WA_005"; name="Ammo Cabinet / Vending Prop"; mesh="Meshes/V0135WA_005_AmmoCabinetVendingProp.obj"; role="interactive ammo vendor prop"; scale_m="1.20 x 1.80 x 0.55"; collision="single cabinet box plus trigger volume at front"; lod="LOD0 windows/pipes/gauge, LOD1 no side pipes, LOD2 cabinet shell"; gates=@("Front affordance visible from 15m","Dispense slot aligns to pickup spawn socket","State colors map green=stocked amber=low red=empty") },
        @{ id="V0135WA_006"; name="Future Alt Lightning Lance Silhouette"; mesh="Meshes/V0135WA_006_FutureAltLightningLance_Silhouette.obj"; role="future weapon exploration, not final v0.1.35 integration"; scale_m="2.05 length"; collision="prototype only: box along spine"; lod="silhouette validation only"; gates=@("Different enough from pistol/scattergun","Preserves brassworks language","Can be shelved without blocking current integration") }
    )
    preview_outputs = @(
        "Documentation/ConceptRenders/V0_1_35_WeaponArsenal/V0135WA_contact_sheet.png",
        "Documentation/ConceptRenders/V0_1_35_WeaponArsenal/V0135WA_component_breakdown.png",
        "Documentation/ConceptRenders/V0_1_35_WeaponArsenal/V0135WA_material_recipe_sheet.png"
    )
}
$manifest | ConvertTo-Json -Depth 8 | Set-Content -Path (Join-Path $manifestRoot "V0_1_35_WEAPON_ARSENAL_MANIFEST.json") -Encoding ASCII
$manifest | ConvertTo-Json -Depth 8 | Set-Content -Path (Join-Path $docRoot "V0_1_35_WEAPON_ARSENAL_MANIFEST.json") -Encoding ASCII

$breakdown = @"
# V0.1.35 Weapon Arsenal Component Breakdown

Visual target: heavy brassworks/steampunk, designed as integration-ready staging geometry rather than concept-only sketches. All assets are proxy meshes with named OBJ objects for Unity selection, material replacement, socketing, and LOD cleanup.

## Pressure Pistol Final Component Pack
- Coils: `top_recoil_coil_01..03` define the spring/pressure silhouette above the receiver.
- Dials/gauges: `cream_pressure_gauge_disc_left`, `black_gauge_rim_left`, `gauge_needle_left`.
- Barrels: `barrel_heat_stained_main_tube`, `barrel_blackened_inner_bore`, soot card at muzzle.
- Grip/frame: walnut grip, blackened trigger guard, split brass receiver frame.
- Valves/rivets/lamps: copper chamber, amber pressure lamp, polished rivet row.
- Wear/soot: `oil_soot_muzzle_wear_card`, polished rivets as edge-wear proxies.

## Steam Scattergun Final Component Pack
- Coils/lines: left/right copper bypass coils and lower steam expansion tank.
- Dials/gauges: rear cream pressure dial with brass rim.
- Barrels: triple heat-stained barrel cluster with soot smear at muzzle.
- Grip/stock: ribbed pump grip, walnut shoulder stock, brass butt plate.
- Frames/plates: blackened receiver box with brass side plates.
- Wear/soot: muzzle smear and polished side rivets.

## Pressure Cartridge Family
- Five variants: pistol short cell, scattergun slug canister, ruptured empty, redband high-pressure, display cutaway.
- Shared components: copper body, brass cap, blackened base, front label band.
- Gameplay readability: red band for dangerous/high value, cream label for standard, rack rail for cabinet dressing.

## Wall Weapon Display Frame
- Brass frame rails, iron backplate, paired weapon hooks, cream labels, amber lamps, polished screws.
- Intended to stage mounted weapons without requiring scene edits in this batch.

## Ammo Cabinet / Vending Prop
- Iron shell, brass front frame, green stock window, amber low-pressure window, red empty flag, coin valve wheel, gauge, side pressure pipes, soot floor panel.
- Recommended sockets: `pickup_spawn_front`, `coin_valve_interact`, `status_window_state`.

## Future Alt Lightning Lance Silhouette
- Alternate future weapon direction only: long brass spine, twin arc prongs, amber accumulator, copper induction coil, route green charge window, red overload switch.
- Purpose: widen v0.1.35 arsenal silhouette range without changing current weapon definitions.
"@
Set-Content -Path (Join-Path $docRoot "V0_1_35_WEAPON_ARSENAL_COMPONENT_BREAKDOWN.md") -Value $breakdown -Encoding ASCII

$acceptance = @"
# V0.1.35 Weapon Arsenal Acceptance Gates

## Import
- Import OBJ meshes from `Assets/_Project/ArtStaging/V0_1_35_WeaponArsenal/Meshes/` with scale factor 1.0.
- Keep hierarchy/object names during import for component selection.
- Resolve materials through `V0135WA_Brassworks_ArsenalPalette.mtl` or replace with final Unity shader materials from the recipe JSON.

## Integration
- Pressure pistol and steam scattergun must fit first-person camera crops before final prefab conversion.
- Pickups require trigger volumes larger than visible geometry.
- Wall display and ammo cabinet must use primitive collision only.
- Red/green/amber status materials must map consistently: green stocked/usable, amber low/charged, red empty/danger.

## LOD And Collision
- LOD0: current staged mesh with decorative rivets/coils/gauges.
- LOD1: remove rivets, gauge needles, small coils, and soot cards.
- LOD2: merge to silhouette boxes/cylinders preserving weapon class readability.
- Collision: box/capsule primitives; never mesh collider for coils, pipes, lamps, rivets, or labels.

## Visual Proof
- Preview sheets live under `Documentation/ConceptRenders/V0_1_35_WeaponArsenal/`.
- Unity-only lookdev policy is preserved: the staging assets are Unity-readable and no Blender/DCC files are included.
"@
Set-Content -Path (Join-Path $docRoot "V0_1_35_WEAPON_ARSENAL_ACCEPTANCE_GATES.md") -Value $acceptance -Encoding ASCII

$scale = @"
# V0.1.35 Weapon Arsenal Scale And Collision Notes

| Asset | Approx Scale | Pivot Recommendation | Collision Recommendation |
| --- | --- | --- | --- |
| Pressure pistol | 1.55m length | Receiver center for viewmodel, grip base for pickup | Capsule barrel, box receiver, box grip |
| Steam scattergun | 2.15m length | Receiver center, forward at barrel cluster for wall mount | Box stock, box receiver, capsule barrel cluster, capsule tank |
| Cartridge family | 0.34m cartridge height | Cartridge base or rack center | Capsule per loose cartridge, one trigger box for pickup |
| Wall display frame | 2.55m wide x 1.36m high | Backplate center | Thin box backplate, optional hook boxes |
| Ammo cabinet | 1.20m wide x 1.80m high | Floor center front | Cabinet box plus front interaction trigger |
| Lightning lance alt | 2.05m length | Spine center | Prototype box only |
"@
Set-Content -Path (Join-Path $docRoot "V0_1_35_WEAPON_ARSENAL_SCALE_COLLISION_LOD_NOTES.md") -Value $scale -Encoding ASCII

Add-Type -AssemblyName System.Drawing
function Draw-Sheet([string]$path, [string]$title, [array]$cards) {
    $bmp = New-Object System.Drawing.Bitmap 1800, 1200
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.Clear([System.Drawing.Color]::FromArgb(24,25,23))
    $fontTitle = New-Object System.Drawing.Font "Arial", 34, ([System.Drawing.FontStyle]::Bold)
    $fontHead = New-Object System.Drawing.Font "Arial", 20, ([System.Drawing.FontStyle]::Bold)
    $fontBody = New-Object System.Drawing.Font "Arial", 13
    $brushTitle = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(238,218,166))
    $brushBody = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(218,207,181))
    $g.DrawString($title, $fontTitle, $brushTitle, 50, 34)
    $penBrass = New-Object System.Drawing.Pen ([System.Drawing.Color]::FromArgb(191,139,54)), 5
    $penIron = New-Object System.Drawing.Pen ([System.Drawing.Color]::FromArgb(68,70,64)), 8
    $brushBrass = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(181,126,43))
    $brushCopper = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(191,82,38))
    $brushIron = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(45,48,44))
    $brushAmber = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(238,142,39))
    $brushGreen = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(39,138,88))
    $brushRed = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(190,38,30))
    for ($i=0; $i -lt $cards.Count; $i++) {
        $col = $i % 3; $row = [Math]::Floor($i / 3)
        $x = 50 + $col * 570; $y = 120 + $row * 500
        $g.FillRectangle((New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(35,36,33))), $x, $y, 520, 430)
        $g.DrawRectangle($penBrass, $x, $y, 520, 430)
        $g.DrawString($cards[$i].name, $fontHead, $brushTitle, $x+24, $y+18)
        $baseY = $y + 238
        switch ($cards[$i].kind) {
            "pistol" {
                $g.FillRectangle($brushBrass, $x+125, $baseY-70, 185, 58)
                $g.FillEllipse($brushCopper, $x+260, $baseY-112, 94, 38)
                $g.DrawLine($penIron, $x+305, $baseY-42, $x+450, $baseY-42)
                $g.FillRectangle($brushIron, $x+95, $baseY-20, 54, 125)
                $g.FillEllipse($brushAmber, $x+198, $baseY-105, 42, 42)
            }
            "scattergun" {
                $g.FillRectangle($brushIron, $x+80, $baseY-76, 230, 80)
                foreach ($dy in @(-34,0,34)) { $g.DrawLine($penIron, $x+280, $baseY+$dy, $x+470, $baseY+$dy) }
                $g.FillRectangle($brushBrass, $x+44, $baseY-50, 92, 70)
                $g.FillEllipse($brushCopper, $x+180, $baseY+20, 185, 48)
                $g.FillRectangle($brushGreen, $x+155, $baseY-110, 75, 22)
            }
            "cartridge" {
                for ($j=0; $j -lt 5; $j++) {
                    $cx = $x+95+$j*78
                    $g.FillEllipse($brushBrass, $cx, $baseY-110, 44, 28)
                    $g.FillRectangle($brushCopper, $cx, $baseY-96, 44, 138)
                    $g.FillEllipse($brushIron, $cx, $baseY+28, 44, 28)
                    if ($j -eq 3) { $g.FillRectangle($brushRed, $cx+3, $baseY-35, 38, 22) } else { $g.FillRectangle($brushTitle, $cx+3, $baseY-35, 38, 22) }
                }
            }
            "frame" {
                $g.DrawRectangle($penBrass, $x+80, $baseY-150, 360, 220)
                $g.FillRectangle($brushIron, $x+95, $baseY-135, 330, 190)
                $g.FillRectangle($brushBrass, $x+145, $baseY-35, 85, 24)
                $g.FillRectangle($brushBrass, $x+295, $baseY-35, 85, 24)
                $g.FillEllipse($brushAmber, $x+120, $baseY-115, 36, 36)
                $g.FillEllipse($brushAmber, $x+372, $baseY-115, 36, 36)
            }
            "cabinet" {
                $g.FillRectangle($brushIron, $x+160, $baseY-170, 200, 285)
                $g.DrawRectangle($penBrass, $x+150, $baseY-180, 220, 305)
                $g.FillRectangle($brushGreen, $x+185, $baseY-120, 70, 70)
                $g.FillRectangle($brushAmber, $x+275, $baseY-120, 55, 70)
                $g.FillEllipse($brushCopper, $x+285, $baseY-10, 58, 58)
                $g.FillRectangle($brushRed, $x+215, $baseY+78, 90, 22)
            }
            default {
                $g.DrawLine($penBrass, $x+70, $baseY-40, $x+430, $baseY-40)
                $g.DrawLine($penIron, $x+300, $baseY-82, $x+470, $baseY-108)
                $g.DrawLine($penIron, $x+300, $baseY-8, $x+470, $baseY+18)
                $g.FillEllipse($brushAmber, $x+180, $baseY-105, 150, 62)
                $g.FillRectangle($brushGreen, $x+210, $baseY-20, 82, 25)
                $g.FillRectangle($brushRed, $x+110, $baseY-20, 45, 24)
            }
        }
        $bodyRect = New-Object System.Drawing.RectangleF (($x + 24), ($y + 318), 472, 82)
        $g.DrawString($cards[$i].body, $fontBody, $brushBody, $bodyRect)
    }
    $bmp.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose(); $bmp.Dispose()
}

Draw-Sheet (Join-Path $renderRoot "V0135WA_contact_sheet.png") "V0.1.35 Brassworks Weapon + Gameplay Prop Arsenal" @(
    @{ name="Pressure Pistol"; kind="pistol"; body="Final component pack: brass receiver, heat barrel, recoil coils, gauge, amber lamp, grip, rivets, soot." },
    @{ name="Steam Scattergun"; kind="scattergun"; body="Final component pack: triple barrel, stock, pump, expansion tank, bypass coils, pressure dial." },
    @{ name="Pressure Cartridge Family"; kind="cartridge"; body="Five ammo variants for pickups, cabinet stock, UI icons, and spent dressing." },
    @{ name="Wall Weapon Display"; kind="frame"; body="Armory frame with hooks, labels, lamps, brass rails, screws, and state-friendly panels." },
    @{ name="Ammo Cabinet / Vendor"; kind="cabinet"; body="Interactive prop candidate with status windows, dispense slot, gauge, coin valve, pipes." },
    @{ name="Future Lightning Lance"; kind="lance"; body="Alternate future silhouette preserving brassworks language while widening the arsenal." }
)

Draw-Sheet (Join-Path $renderRoot "V0135WA_component_breakdown.png") "V0.1.35 Component Breakdown: Coils, Dials, Frames, Wear" @(
    @{ name="Coils + Pressure Lines"; kind="lance"; body="Copper lines and coils are separate OBJ objects for VFX sockets and LOD removal." },
    @{ name="Dials + Gauges"; kind="frame"; body="Cream faces, brass rims, and red needles separate for UI readability and animated pressure states." },
    @{ name="Barrels + Chambers"; kind="scattergun"; body="Heat-stained barrels and copper pressure tanks define weapon class at a glance." },
    @{ name="Grips + Stocks"; kind="pistol"; body="Walnut/leather components are isolated for hand placement, pickup scale, and material swaps." },
    @{ name="Cabinet State Panels"; kind="cabinet"; body="Green/amber/red components map to stocked, low, and empty/danger states." },
    @{ name="Soot + Polished Wear"; kind="cartridge"; body="Soot cards and wear/rivets are lightweight surface passes, not collision geometry." }
)

Draw-Sheet (Join-Path $renderRoot "V0135WA_material_recipe_sheet.png") "V0.1.35 Material Recipes: Brassworks Palette" @(
    @{ name="Aged Brass"; kind="frame"; body="Primary frame material; use darker cavity grime and polished exposed edges." },
    @{ name="Blackened Iron"; kind="scattergun"; body="Receivers, hooks, backplates, and structural silhouettes for strong read in dark levels." },
    @{ name="Copper Lines"; kind="lance"; body="Pressure tubes, valves, coils, and cartridge bodies. Accent, not full-object wash." },
    @{ name="Walnut / Leather"; kind="pistol"; body="Grips, stocks, cabinet pulls, and straps with low gloss and hand-worn edge highlights." },
    @{ name="Amber Glass"; kind="cabinet"; body="Lamps and pressure windows; integrate with emissive shader variants later." },
    @{ name="Soot + Wear"; kind="cartridge"; body="Localized muzzle grime, floor smudges, and polished rivet highlights." }
)

$unityProof = @"
V0.1.35 Weapon Arsenal Unity Proof Notes

This batch contains Unity-importable OBJ/MTL assets and Unity .mat proxy files only. Preview PNGs are documentation contact sheets generated from the same package descriptions for user review. No Blender or external DCC files were authored.

Suggested Unity proof pass:
1. Drag Meshes/*.obj into a temporary import scene.
2. Apply M_V0135WA_* materials or the imported MTL materials.
3. Frame each asset with a 35mm camera, one warm key, one cool rim, ambient intensity 0.55.
4. Capture screenshots to Documentation/ConceptRenders/V0_1_35_WeaponArsenal if final lookdev proof is needed.
"@
Set-Content -Path (Join-Path $unityProofRoot "V0135WA_UnityOnlyLookdevProofNotes.txt") -Value $unityProof -Encoding ASCII

Write-Host "Generated V0.1.35 weapon arsenal staging package."
