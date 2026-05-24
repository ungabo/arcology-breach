$ErrorActionPreference = "Stop"

$root = "D:\__MY APPS\Unity Doom"
$assetRoot = Join-Path $root "Assets/_Project/ArtStaging/V0_1_35_FeedbackPolish"
$docRoot = Join-Path $root "Documentation/AssetProduction/V0_1_35_FeedbackPolish"
$renderRoot = Join-Path $root "Documentation/ConceptRenders/V0_1_35_FeedbackPolish"

$dirs = @(
    $assetRoot,
    (Join-Path $assetRoot "Manifests"),
    (Join-Path $assetRoot "Materials"),
    (Join-Path $assetRoot "UI"),
    (Join-Path $assetRoot "UI/Sprites"),
    (Join-Path $assetRoot "UI/Recipes"),
    (Join-Path $assetRoot "VFX"),
    (Join-Path $assetRoot "VFX/Recipes"),
    (Join-Path $assetRoot "Audio"),
    (Join-Path $assetRoot "Audio/Cues"),
    $docRoot,
    $renderRoot
)
foreach ($dir in $dirs) {
    New-Item -ItemType Directory -Force -Path $dir | Out-Null
}

$cueMatrix = @(
    [ordered]@{
        id = "weapon_impact"
        display = "Weapon Impact"
        priority = 80
        color = "amber"
        ui = "micro crosshair bloom, brass tick ring, 0.07s flash"
        vfx = "2-3 pressure sparks, tiny steam puff, decal glint; world-space only"
        audio = "short brass ping plus softened metal tick; -13 LUFS short cue target"
        mix = "high-mid transient, below weapon fire, sidechain ducked by gunshot bus"
        accessibility = "shape + timing must carry confirmation without relying on amber alone"
    },
    [ordered]@{
        id = "enemy_hit"
        display = "Enemy Hit Confirmation"
        priority = 90
        color = "amber"
        ui = "thin gauge needle nudge at reticle edge, 0.10s"
        vfx = "small amber enamel chip, weak-point lamp flicker if valid target"
        audio = "soft ratchet click with body-material layer hook"
        mix = "stackable, random pitch +/-4%, max 4 voices"
        accessibility = "do not strobe; hit state also uses enemy flinch/sparks"
    },
    [ordered]@{
        id = "enemy_death"
        display = "Enemy Death Confirmation"
        priority = 96
        color = "green"
        ui = "reticle settles to route-green check notch, 0.18s"
        vfx = "shutdown steam sigh, amber lamp collapse to dark, brass fragments sparkle once"
        audio = "valve drop, low metal clonk, steam release tail"
        mix = "one-shot emphasis, protected from crowd hit spam for 0.25s"
        accessibility = "clear silhouette drop plus non-red success color"
    },
    [ordered]@{
        id = "pickup_acquired"
        display = "Pickup Acquisition"
        priority = 75
        color = "green"
        ui = "brass inventory plate slides in/out, icon lamp fill"
        vfx = "small intake swirl toward player, green enamel glint at pickup origin"
        audio = "positive valve tick, soft gauge rise, tiny steam inhale"
        mix = "front UI bus, duck ambience by 1 dB for 0.12s"
        accessibility = "icon and text token support for colorblind readability"
    },
    [ordered]@{
        id = "secret_discovery"
        display = "Secret Discovery"
        priority = 100
        color = "green_amber"
        ui = "large brass nameplate with hidden cog iris, 1.2s minimum read"
        vfx = "green route lamps chase once, dust/steam reveal puff"
        audio = "warm three-note chime through brass resonator, no jump scare"
        mix = "music-compatible, never masks enemy threat callouts"
        accessibility = "caption-safe label, longer hold, no required rapid flashing"
    },
    [ordered]@{
        id = "objective_update"
        display = "Objective Update"
        priority = 86
        color = "amber"
        ui = "objective brass ledger tab stamps in with needle sweep"
        vfx = "none by default; optional world marker pressure lamp pulse"
        audio = "paper/plate stamp plus quiet gauge tick"
        mix = "UI bus with narrow transient; avoid competing with pickup confirm"
        accessibility = "text contrast 7:1 target on dark iron backing"
    },
    [ordered]@{
        id = "denied_interaction"
        display = "Denied Interaction"
        priority = 88
        color = "red"
        ui = "red enamel lock tab shakes once, no repeated screen shake"
        vfx = "small red pressure spark at interact point, valve refuses half-turn"
        audio = "dry clack + blocked steam chirp; short, non-punishing"
        mix = "low volume but sharp attack; throttle repeat to 0.35s"
        accessibility = "must include lock icon silhouette, not red-only language"
    },
    [ordered]@{
        id = "route_confirmation"
        display = "Route Confirmation"
        priority = 82
        color = "green"
        ui = "green enamel arrow lamp sweeps along route plate"
        vfx = "world route lamps chase away from player once"
        audio = "valve opens, soft pressure rise, two light ticks"
        mix = "spatialized if world-sourced, UI layer if compass-sourced"
        accessibility = "animated direction and arrow shape carry meaning without color"
    },
    [ordered]@{
        id = "pause_settings_readability"
        display = "Pause / Settings Readability"
        priority = 70
        color = "neutral"
        ui = "dark iron panel, brass section headers, cream text, focus lamp rail"
        vfx = "none; keep menus calm and legible"
        audio = "soft mechanical focus ticks, no looping hiss under settings"
        mix = "UI ticks -18 LUFS, respect menu SFX slider"
        accessibility = "minimum 18px body equivalent, high contrast, reduced motion path"
    },
    [ordered]@{
        id = "low_health"
        display = "Low Health Language"
        priority = 98
        color = "red"
        ui = "red pressure gasket edge vignette, heart gauge needle tremble"
        vfx = "rare steam cough from player rig, never opaque center screen"
        audio = "muffled pressure thump at restrained cadence"
        mix = "heartbeat-style cue capped and disabled in pause/settings"
        accessibility = "reduced-motion swaps pulse for static cracked gauge + audio toggle"
    },
    [ordered]@{
        id = "low_ammo"
        display = "Low Ammo Language"
        priority = 92
        color = "amber_red"
        ui = "ammo cartridge icon drains to amber, final shots show red rim"
        vfx = "weapon gauge needle dips, tiny chamber sputter on dry threshold"
        audio = "hollow cartridge rattle, last-round click accent"
        mix = "weapon-adjacent, less urgent than health, no constant loop"
        accessibility = "numeric ammo remains primary; color only reinforces state"
    }
)

$palette = [ordered]@{
    aged_brass = @{ rgb = @(0.78, 0.55, 0.22); hex = "#C78C38"; usage = "plates, trim, edge wear, fast-read frames" }
    blackened_iron = @{ rgb = @(0.055, 0.060, 0.055); hex = "#0E0F0E"; usage = "menu backers, reticle anchors, high-contrast silhouettes" }
    cream_text = @{ rgb = @(0.88, 0.80, 0.62); hex = "#E0CC9E"; usage = "readable labels, gauge faces, icon inlays" }
    amber_signal = @{ rgb = @(1.00, 0.55, 0.10); hex = "#FF8C1A"; usage = "attention, impact, objective pending" }
    route_green = @{ rgb = @(0.06, 0.48, 0.29); hex = "#0F7A4A"; usage = "success, route confirmation, acquired states" }
    warning_red = @{ rgb = @(0.78, 0.07, 0.04); hex = "#C7120A"; usage = "denied, low health, empty, danger" }
    steam_white = @{ rgb = @(0.78, 0.74, 0.66); hex = "#C7BDA8"; usage = "short puffs and readability silhouettes" }
}

function New-MatYaml([string]$name, [array]$rgb, [float]$metallic, [float]$smoothness) {
    $path = Join-Path $assetRoot "Materials/M_V0135FP_$name.mat"
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
  m_Name: M_V0135FP_$name
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
    - _Color: {r: $($rgb[0]), g: $($rgb[1]), b: $($rgb[2]), a: 1}
"@ | Set-Content -Path $path -Encoding ASCII
}

New-MatYaml "aged_brass_plate" $palette.aged_brass.rgb 0.7 0.46
New-MatYaml "blackened_iron_backer" $palette.blackened_iron.rgb 0.75 0.28
New-MatYaml "cream_gauge_face" $palette.cream_text.rgb 0.03 0.36
New-MatYaml "amber_enamel_lamp" $palette.amber_signal.rgb 0.05 0.72
New-MatYaml "green_enamel_lamp" $palette.route_green.rgb 0.05 0.62
New-MatYaml "red_enamel_lamp" $palette.warning_red.rgb 0.05 0.58
New-MatYaml "steam_soft_puff" $palette.steam_white.rgb 0.0 0.22

Add-Type -AssemblyName System.Drawing

function ColorFromHex([string]$hex) {
    return [System.Drawing.ColorTranslator]::FromHtml($hex)
}

function Save-Icon([string]$path, [string]$kind, [string]$signalHex) {
    $bmp = New-Object System.Drawing.Bitmap 256, 256
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.Clear([System.Drawing.Color]::Transparent)
    $brass = New-Object System.Drawing.SolidBrush (ColorFromHex $palette.aged_brass.hex)
    $iron = New-Object System.Drawing.SolidBrush (ColorFromHex $palette.blackened_iron.hex)
    $cream = New-Object System.Drawing.SolidBrush (ColorFromHex $palette.cream_text.hex)
    $signal = New-Object System.Drawing.SolidBrush (ColorFromHex $signalHex)
    $penIron = New-Object System.Drawing.Pen (ColorFromHex $palette.blackened_iron.hex), 10
    $penCream = New-Object System.Drawing.Pen (ColorFromHex $palette.cream_text.hex), 8
    $penSignal = New-Object System.Drawing.Pen (ColorFromHex $signalHex), 12

    $g.FillRectangle($brass, 24, 34, 208, 188)
    $g.FillRectangle($iron, 38, 48, 180, 160)
    $g.FillRectangle($cream, 52, 62, 152, 132)

    switch ($kind) {
        "impact" {
            $g.DrawEllipse($penSignal, 76, 78, 104, 104)
            $g.DrawLine($penIron, 128, 64, 128, 194)
            $g.DrawLine($penIron, 64, 128, 194, 128)
            $g.FillEllipse($signal, 108, 108, 40, 40)
        }
        "enemy" {
            $g.FillPie($signal, 58, 74, 140, 140, 200, 140)
            $g.DrawEllipse($penIron, 62, 70, 132, 132)
            $g.FillRectangle($iron, 92, 126, 72, 34)
            $g.FillEllipse($cream, 82, 104, 26, 26)
            $g.FillEllipse($cream, 148, 104, 26, 26)
        }
        "pickup" {
            $g.FillRectangle($signal, 78, 86, 100, 74)
            $g.FillRectangle($brass, 92, 68, 72, 30)
            $g.DrawLine($penCream, 128, 72, 128, 180)
            $g.DrawLine($penCream, 82, 126, 174, 126)
        }
        "secret" {
            $g.FillEllipse($signal, 62, 62, 132, 132)
            for ($i = 0; $i -lt 8; $i++) {
                $a = (2 * [Math]::PI * $i / 8)
                $x1 = 128 + [Math]::Cos($a) * 38
                $y1 = 128 + [Math]::Sin($a) * 38
                $x2 = 128 + [Math]::Cos($a) * 74
                $y2 = 128 + [Math]::Sin($a) * 74
                $g.DrawLine($penIron, [float]$x1, [float]$y1, [float]$x2, [float]$y2)
            }
            $g.FillEllipse($iron, 108, 108, 40, 40)
        }
        "objective" {
            $g.FillRectangle($signal, 72, 78, 112, 94)
            $g.DrawLine($penIron, 92, 104, 164, 104)
            $g.DrawLine($penIron, 92, 130, 150, 130)
            $g.DrawLine($penIron, 92, 156, 132, 156)
        }
        "denied" {
            $g.FillRectangle($signal, 68, 112, 120, 78)
            $g.DrawArc($penIron, 86, 62, 84, 84, 180, 180)
            $g.DrawLine($penCream, 92, 150, 164, 150)
        }
        "route" {
            $g.DrawLine($penSignal, 66, 132, 164, 132)
            $g.DrawLine($penSignal, 130, 92, 174, 132)
            $g.DrawLine($penSignal, 130, 172, 174, 132)
            $g.FillEllipse($iron, 54, 116, 32, 32)
        }
        "settings" {
            $g.DrawEllipse($penSignal, 78, 78, 100, 100)
            for ($i = 0; $i -lt 6; $i++) {
                $a = (2 * [Math]::PI * $i / 6)
                $x1 = 128 + [Math]::Cos($a) * 34
                $y1 = 128 + [Math]::Sin($a) * 34
                $x2 = 128 + [Math]::Cos($a) * 74
                $y2 = 128 + [Math]::Sin($a) * 74
                $g.DrawLine($penIron, [float]$x1, [float]$y1, [float]$x2, [float]$y2)
            }
            $g.FillEllipse($iron, 112, 112, 32, 32)
        }
        "health" {
            $points = @(
                (New-Object System.Drawing.Point 128, 184),
                (New-Object System.Drawing.Point 62, 112),
                (New-Object System.Drawing.Point 82, 74),
                (New-Object System.Drawing.Point 128, 92),
                (New-Object System.Drawing.Point 174, 74),
                (New-Object System.Drawing.Point 194, 112)
            )
            $g.FillPolygon($signal, $points)
            $g.DrawLine($penIron, 102, 128, 126, 128)
            $g.DrawLine($penIron, 126, 128, 138, 106)
            $g.DrawLine($penIron, 138, 106, 158, 150)
        }
        "ammo" {
            for ($i = 0; $i -lt 4; $i++) {
                $x = 72 + $i * 32
                $g.FillRectangle($signal, $x, 74, 22, 104)
                $g.FillEllipse($brass, $x, 62, 22, 24)
                $g.FillRectangle($iron, $x, 158, 22, 20)
            }
        }
        default {
            $g.FillEllipse($signal, 74, 74, 108, 108)
        }
    }
    $bmp.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose()
    $bmp.Dispose()
}

$iconKinds = @{
    weapon_impact = "impact"
    enemy_hit = "enemy"
    enemy_death = "enemy"
    pickup_acquired = "pickup"
    secret_discovery = "secret"
    objective_update = "objective"
    denied_interaction = "denied"
    route_confirmation = "route"
    pause_settings_readability = "settings"
    low_health = "health"
    low_ammo = "ammo"
}

$colorHexByCue = @{
    weapon_impact = $palette.amber_signal.hex
    enemy_hit = $palette.amber_signal.hex
    enemy_death = $palette.route_green.hex
    pickup_acquired = $palette.route_green.hex
    secret_discovery = $palette.route_green.hex
    objective_update = $palette.amber_signal.hex
    denied_interaction = $palette.warning_red.hex
    route_confirmation = $palette.route_green.hex
    pause_settings_readability = $palette.cream_text.hex
    low_health = $palette.warning_red.hex
    low_ammo = $palette.amber_signal.hex
}

foreach ($cue in $cueMatrix) {
    Save-Icon (Join-Path $assetRoot "UI/Sprites/V0135FP_$($cue.id)_icon.png") $iconKinds[$cue.id] $colorHexByCue[$cue.id]
}

function Write-Wav([string]$path, [double[]]$freqs, [double]$durationSeconds, [double]$gain, [string]$envelope) {
    $sampleRate = 44100
    $samples = [int]($sampleRate * $durationSeconds)
    $data = New-Object byte[] ($samples * 2)
    for ($i = 0; $i -lt $samples; $i++) {
        $t = $i / $sampleRate
        $amp = 1.0
        if ($envelope -eq "ping") {
            $amp = [Math]::Exp(-8.0 * $t)
        } elseif ($envelope -eq "sigh") {
            $amp = [Math]::Min(1.0, $t / 0.05) * [Math]::Exp(-2.6 * $t)
        } elseif ($envelope -eq "tick") {
            $amp = [Math]::Exp(-18.0 * $t)
        } else {
            $amp = [Math]::Sin([Math]::PI * [Math]::Min(1.0, $t / $durationSeconds))
        }
        $value = 0.0
        foreach ($freq in $freqs) {
            $value += [Math]::Sin(2.0 * [Math]::PI * $freq * $t)
        }
        $value = $value / [Math]::Max(1, $freqs.Count)
        $noise = ([Math]::Sin(2.0 * [Math]::PI * 37.0 * $t) * [Math]::Sin(2.0 * [Math]::PI * 911.0 * $t)) * 0.10
        $short = [int16]([Math]::Max(-1.0, [Math]::Min(1.0, ($value + $noise) * $amp * $gain)) * 32767)
        $bytes = [BitConverter]::GetBytes($short)
        $data[$i * 2] = $bytes[0]
        $data[$i * 2 + 1] = $bytes[1]
    }
    $writer = New-Object System.IO.BinaryWriter([System.IO.File]::Open($path, [System.IO.FileMode]::Create))
    $writer.Write([System.Text.Encoding]::ASCII.GetBytes("RIFF"))
    $writer.Write([int](36 + $data.Length))
    $writer.Write([System.Text.Encoding]::ASCII.GetBytes("WAVEfmt "))
    $writer.Write([int]16)
    $writer.Write([int16]1)
    $writer.Write([int16]1)
    $writer.Write([int]$sampleRate)
    $writer.Write([int]($sampleRate * 2))
    $writer.Write([int16]2)
    $writer.Write([int16]16)
    $writer.Write([System.Text.Encoding]::ASCII.GetBytes("data"))
    $writer.Write([int]$data.Length)
    $writer.Write($data)
    $writer.Dispose()
}

$wavSpecs = @{
    weapon_impact = @{ f = @(760, 1240); d = 0.18; g = 0.22; e = "ping" }
    enemy_hit = @{ f = @(510, 880); d = 0.13; g = 0.18; e = "tick" }
    enemy_death = @{ f = @(160, 250, 410); d = 0.55; g = 0.24; e = "sigh" }
    pickup_acquired = @{ f = @(620, 930, 1240); d = 0.30; g = 0.20; e = "ping" }
    secret_discovery = @{ f = @(392, 523, 659); d = 1.10; g = 0.18; e = "swell" }
    objective_update = @{ f = @(360, 720); d = 0.24; g = 0.16; e = "tick" }
    denied_interaction = @{ f = @(180, 230); d = 0.22; g = 0.24; e = "tick" }
    route_confirmation = @{ f = @(480, 640, 820); d = 0.38; g = 0.18; e = "ping" }
    pause_settings_readability = @{ f = @(440, 660); d = 0.12; g = 0.10; e = "tick" }
    low_health = @{ f = @(82, 123); d = 0.70; g = 0.20; e = "sigh" }
    low_ammo = @{ f = @(300, 380); d = 0.26; g = 0.18; e = "tick" }
}
foreach ($cue in $cueMatrix) {
    $s = $wavSpecs[$cue.id]
    Write-Wav (Join-Path $assetRoot "Audio/Cues/V0135FP_$($cue.id).wav") $s.f $s.d $s.g $s.e
}

$uiRecipe = [ordered]@{
    package = "V0_1_35_FeedbackPolish"
    intent = "Coordinated player-feedback language for brassworks UI plates, enamel lamps, gauge motion, and readable combat/system confirmation."
    sprite_import_notes = @(
        "Import icons as Sprite (2D and UI), alpha is transparency, 256 px source size.",
        "Point-filter is acceptable for icon proof, final UI should use bilinear and atlas compression matched to the HUD.",
        "Use cream icon shapes plus amber/green/red signal fills so color is supportive, not sole meaning."
    )
    hud_language = @(
        "Brass plates anchor event class; blackened iron backers carry contrast; cream labels carry text.",
        "Amber is attention/pending/impact, green is success/route/acquired, red is denied/danger/empty.",
        "Gauge needle motion should overshoot 6-10 degrees then settle within 0.12-0.22 seconds."
    )
    cues = $cueMatrix
}
$uiRecipe | ConvertTo-Json -Depth 8 | Set-Content -Path (Join-Path $assetRoot "UI/Recipes/V0135FP_UI_CuePackage.json") -Encoding ASCII

$vfxRecipe = [ordered]@{
    package = "V0_1_35_FeedbackPolish"
    unity_only_policy = "VFX are recipe-staged for Unity ParticleSystem/VFX Graph implementation only. No external DCC render or simulation dependency."
    shared_limits = @{
        sparks_per_impact = "2-6 visible particles, capped per weapon fire event"
        steam_lifetime = "0.25-0.8s depending on event importance"
        emission_textures = "use soft circular sprite or project-standard smoke; do not add large atlas dependency in this staging batch"
        overdraw = "avoid full-screen steam; keep puffs behind reticle and away from menu text"
    }
    recipes = @(
        @{ id="steam_puff_soft"; color="steam white with warm amber edge"; shape="billboard puffs, upward drag"; use="impact, pickup, secret reveal, enemy death"; notes="short-lived, no center-screen opacity above 20%" },
        @{ id="pressure_sparks_amber"; color="amber to brass"; shape="thin streak particles with gravity"; use="weapon impact, enemy hit"; notes="high contrast against blackened iron; cap bursts under rapid fire" },
        @{ id="pressure_sparks_red_denied"; color="red enamel spark"; shape="single blocked-interaction fleck"; use="denied interaction"; notes="throttle repeat and pair with lock icon" },
        @{ id="route_lamp_chase_green"; color="route green"; shape="sequential lamp pulses"; use="route confirmation, secret discovery"; notes="directional animation must be readable in grayscale by position/time" },
        @{ id="shutdown_sigh"; color="amber lamp fades to soot"; shape="steam exhale plus small brass glitter"; use="enemy death"; notes="protect this cue from hit spam; one death confirmation per enemy" }
    )
}
$vfxRecipe | ConvertTo-Json -Depth 8 | Set-Content -Path (Join-Path $assetRoot "VFX/Recipes/V0135FP_VFX_CuePackage.json") -Encoding ASCII

$audioIndex = [ordered]@{
    package = "V0_1_35_FeedbackPolish"
    intent = "Procedural placeholder WAVs express cue length, envelope, pitch register, and mix hierarchy. Replace with final authored brass/steam layers after integration."
    import_notes = @(
        "Import as mono, decompress on load for short UI/gameplay cues under 1s unless final audio policy differs.",
        "Looping disabled for all cues in this staging pack.",
        "Route, pickup, objective, and menu cues belong on UI/SFX bus; impact/death can be world or hybrid bus depending on source."
    )
    cues = $cueMatrix | ForEach-Object {
        [ordered]@{
            id = $_.id
            file = "Audio/Cues/V0135FP_$($_.id).wav"
            intent = $_.audio
            mix = $_.mix
            priority = $_.priority
        }
    }
}
$audioIndex | ConvertTo-Json -Depth 8 | Set-Content -Path (Join-Path $assetRoot "Audio/V0135FP_AUDIO_CUE_INDEX.json") -Encoding ASCII

$manifest = [ordered]@{
    package = "V0_1_35_FeedbackPolish"
    created_for = "v0.1.35 UI + Audio + VFX polish staging bundle"
    north_star = "steampunk/brassworks player-feedback system: brass plates, enamel lamps, pressure gauges, steam puffs, sparks, and restrained mechanical audio"
    owned_scopes = @(
        "Assets/_Project/ArtStaging/V0_1_35_FeedbackPolish/",
        "Documentation/AssetProduction/V0_1_35_FeedbackPolish/",
        "Documentation/ConceptRenders/V0_1_35_FeedbackPolish/"
    )
    asset_groups = @(
        @{ type="materials"; path="Materials/"; count=7; notes="proxy Standard .mat files for brass, iron, cream, amber, green, red, steam" },
        @{ type="ui_sprites"; path="UI/Sprites/"; count=$cueMatrix.Count; notes="256px transparent PNG icons for all requested cue classes" },
        @{ type="ui_recipe"; path="UI/Recipes/V0135FP_UI_CuePackage.json"; count=1; notes="cue matrix, HUD language, import notes" },
        @{ type="vfx_recipe"; path="VFX/Recipes/V0135FP_VFX_CuePackage.json"; count=1; notes="Unity ParticleSystem/VFX Graph recipe staging" },
        @{ type="audio_cues"; path="Audio/Cues/"; count=$cueMatrix.Count; notes="mono WAV placeholder cues encoding timing/mix intent" },
        @{ type="audio_index"; path="Audio/V0135FP_AUDIO_CUE_INDEX.json"; count=1; notes="audio import and mix notes" }
    )
    cue_coverage = $cueMatrix | ForEach-Object { $_.id }
    preview_outputs = @(
        "Documentation/ConceptRenders/V0_1_35_FeedbackPolish/V0135FP_feedback_system_contact_sheet.png",
        "Documentation/ConceptRenders/V0_1_35_FeedbackPolish/V0135FP_ui_audio_vfx_matrix.png",
        "Documentation/ConceptRenders/V0_1_35_FeedbackPolish/V0135FP_accessibility_readability_sheet.png"
    )
}
$manifest | ConvertTo-Json -Depth 8 | Set-Content -Path (Join-Path $assetRoot "Manifests/V0_1_35_FEEDBACK_POLISH_MANIFEST.json") -Encoding ASCII
$manifest | ConvertTo-Json -Depth 8 | Set-Content -Path (Join-Path $docRoot "V0_1_35_FEEDBACK_POLISH_MANIFEST.json") -Encoding ASCII

$recipesDoc = @"
# V0.1.35 Feedback Polish Material, Sprite, Sound Recipes

## Brass UI Plates
- Base: aged brass with darker grime in seams and a subtle polished rim.
- Function: frames event class and separates UI from world noise.
- Unity notes: use one shared material variant for plates; reserve emissive only for lamps, not plate fill.

## Enamel Lamps
- Amber: impact, pending, objective, low-ammo early warning.
- Green: acquired, route confirmed, enemy death/clear state, secret positive reveal.
- Red: denied, low-health, empty, danger.
- Readability: every lamp state must also have icon shape, motion, or text support.

## Steam Puffs
- Use short puffs as punctuation, not atmosphere.
- Impact/pickup puffs should stay under 0.35s; death/secret puffs can last 0.6-0.8s.
- Keep center-screen alpha low and never obscure reticle or objective text.

## Pressure Sparks
- Amber sparks: weapon impact and enemy hit.
- Red sparks: denied interaction only, throttled.
- Keep bursts small: 2-6 particles, with larger events using longer audio/UI instead of more particles.

## Gauge Needle Motion
- Hit: tiny 3-5 degree nudge, settle in 0.10s.
- Objective: 20-30 degree sweep, settle in 0.22s.
- Low health: tremble, reduced-motion fallback is static cracked gauge.
- Low ammo: downward dip at threshold and final shot rim pulse.

## Audio Cue Intent
- Procedural WAVs in the staging folder are timing/mix placeholders, not final foley.
- Impacts use short brass ping/metal ticks under weapon fire.
- Death uses valve drop, clonk, steam release, protected from hit spam.
- Pickup and route cues are positive but brief.
- Denied cue is sharp and throttled so it reads without irritating repetition.
- Pause/settings uses only restrained focus ticks and no constant hiss.
"@
Set-Content -Path (Join-Path $docRoot "V0_1_35_FEEDBACK_POLISH_RECIPES.md") -Value $recipesDoc -Encoding ASCII

$importDoc = @"
# V0.1.35 Feedback Polish Import Notes

## Unity Asset Import
- PNGs: import as Sprite (2D and UI), alpha enabled, 256 px source art.
- WAVs: import as mono SFX, no loop, normalize disabled if project audio policy allows manual gain staging.
- Materials: Standard shader proxy materials only; replace with project UI/VFX shader variants during final integration.
- JSON files: treat as staging data/reference, not runtime gameplay config unless an integrator intentionally maps them.

## Integration Boundaries
- This bundle intentionally contains no scripts, scenes, prefabs, validators, build settings, or package changes.
- Main gameplay systems should map cue IDs to existing event hooks rather than adding new event names.
- Suggested event IDs: `weapon_impact`, `enemy_hit`, `enemy_death`, `pickup_acquired`, `secret_discovery`, `objective_update`, `denied_interaction`, `route_confirmation`, `pause_settings_readability`, `low_health`, `low_ammo`.

## Performance Notes
- UI icons are small transparent PNGs; pack into the project HUD atlas during integration.
- WAV placeholders are short mono files; final audio should keep similar durations to avoid UI sluggishness.
- VFX recipes favor low particle counts and short lifetimes to keep combat readable.
- Throttle denied/low-health/low-ammo repeat cues so feedback does not become a constant loop.
"@
Set-Content -Path (Join-Path $docRoot "V0_1_35_FEEDBACK_POLISH_IMPORT_PERFORMANCE_NOTES.md") -Value $importDoc -Encoding ASCII

$accessDoc = @"
# V0.1.35 Feedback Polish Accessibility And Acceptance Gates

## Accessibility / Readability
- Never communicate state with color alone: pair amber/green/red with icon shape, motion, text token, or audio cadence.
- Pause/settings body text should target at least 18 px equivalent and 7:1 contrast on dark iron panels.
- Reduced-motion mode should disable gauge tremble, screen-edge pulsing, and lamp chase while preserving static icons and audio.
- Low-health feedback must not obscure the reticle or central enemies.
- Secret/objective panels need longer holds than combat pips so players can read them.

## Acceptance Gates
- All eleven cue IDs are present in manifest, UI recipe, VFX recipe coverage, and audio index.
- Each requested category has a PNG icon and WAV placeholder.
- Amber, green, and red meanings are consistent across UI, VFX, and audio documents.
- Denied interaction has repeat throttling guidance.
- Low-health and low-ammo are distinguishable by urgency, shape, and audio register.
- No files are required outside owned staging scopes for review.
"@
Set-Content -Path (Join-Path $docRoot "V0_1_35_FEEDBACK_POLISH_ACCESSIBILITY_ACCEPTANCE_GATES.md") -Value $accessDoc -Encoding ASCII

function Draw-PreviewSheet([string]$path, [string]$title, [array]$items, [string]$mode) {
    $bmp = New-Object System.Drawing.Bitmap 1920, 1240
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.Clear([System.Drawing.Color]::FromArgb(23, 24, 22))
    $fontTitle = New-Object System.Drawing.Font "Arial", 34, ([System.Drawing.FontStyle]::Bold)
    $fontHead = New-Object System.Drawing.Font "Arial", 18, ([System.Drawing.FontStyle]::Bold)
    $fontBody = New-Object System.Drawing.Font "Arial", 12
    $brushTitle = New-Object System.Drawing.SolidBrush (ColorFromHex $palette.cream_text.hex)
    $brushBody = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(222, 211, 184))
    $brassPen = New-Object System.Drawing.Pen (ColorFromHex $palette.aged_brass.hex), 4
    $g.DrawString($title, $fontTitle, $brushTitle, 54, 34)
    for ($i = 0; $i -lt $items.Count; $i++) {
        $col = $i % 4
        $row = [Math]::Floor($i / 4)
        $x = 54 + $col * 455
        $y = 118 + $row * 360
        $g.FillRectangle((New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(35, 36, 33))), $x, $y, 405, 310)
        $g.DrawRectangle($brassPen, $x, $y, 405, 310)
        $signal = ColorFromHex $colorHexByCue[$items[$i].id]
        $signalBrush = New-Object System.Drawing.SolidBrush $signal
        $g.FillEllipse($signalBrush, $x + 24, $y + 24, 52, 52)
        $g.DrawString($items[$i].display, $fontHead, $brushTitle, $x + 92, $y + 28)
        if ($mode -eq "matrix") {
            $body = "UI: $($items[$i].ui)`nVFX: $($items[$i].vfx)`nAudio: $($items[$i].audio)"
        } elseif ($mode -eq "access") {
            $body = "Color: $($items[$i].color)`nAccessibility: $($items[$i].accessibility)`nMix: $($items[$i].mix)"
        } else {
            $body = "$($items[$i].ui)`n$($items[$i].vfx)`n$($items[$i].audio)"
        }
        $rect = New-Object System.Drawing.RectangleF ($x + 26), ($y + 102), 350, 170
        $g.DrawString($body, $fontBody, $brushBody, $rect)
    }
    $bmp.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose()
    $bmp.Dispose()
}

Draw-PreviewSheet (Join-Path $renderRoot "V0135FP_feedback_system_contact_sheet.png") "V0.1.35 Feedback Polish Contact Sheet" $cueMatrix "contact"
Draw-PreviewSheet (Join-Path $renderRoot "V0135FP_ui_audio_vfx_matrix.png") "V0.1.35 UI + Audio + VFX Cue Matrix" $cueMatrix "matrix"
Draw-PreviewSheet (Join-Path $renderRoot "V0135FP_accessibility_readability_sheet.png") "V0.1.35 Accessibility + Readability Gates" $cueMatrix "access"

Write-Host "Generated V0.1.35 Feedback Polish staging bundle."
