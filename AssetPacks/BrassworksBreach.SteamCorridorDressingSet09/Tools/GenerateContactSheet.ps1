param(
    [string]$RepoRoot
)

$ErrorActionPreference = "Stop"

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$packageRoot = Resolve-Path (Join-Path $scriptRoot "..")

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $packageRoot "..\..")).Path
}

$catalogPath = Join-Path $packageRoot "Runtime\Metadata\SCD09_SteamCorridorDressingSet09_Catalog_0.1.54-p001.json"
$outputDir = Join-Path $RepoRoot "Documentation\ConceptRenders\V0_1_54_SteamCorridorDressingSet09"
$outputPath = Join-Path $outputDir "SCD09_contact_sheet.png"

New-Item -ItemType Directory -Force -Path $outputDir | Out-Null

Add-Type -AssemblyName System.Drawing

$catalog = Get-Content -LiteralPath $catalogPath -Raw | ConvertFrom-Json
$pieces = @()
foreach ($family in $catalog.families) {
    foreach ($piece in $family.pieces) {
        $piece | Add-Member -NotePropertyName family -NotePropertyValue $family.name -Force
        $pieces += $piece
    }
}

$width = 1800
$height = 1420
$margin = 58
$titleHeight = 130
$columns = 4
$rows = 5
$gap = 24
$cardW = [int](($width - ($margin * 2) - ($gap * ($columns - 1))) / $columns)
$cardH = [int](($height - $titleHeight - $margin - ($gap * ($rows - 1))) / $rows)

$bitmap = New-Object System.Drawing.Bitmap $width, $height
$graphics = [System.Drawing.Graphics]::FromImage($bitmap)
$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$graphics.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::ClearTypeGridFit

$bg = [System.Drawing.Color]::FromArgb(24, 24, 22)
$paper = [System.Drawing.Color]::FromArgb(43, 40, 34)
$line = [System.Drawing.Color]::FromArgb(118, 98, 68)
$muted = [System.Drawing.Color]::FromArgb(178, 166, 137)
$ivory = [System.Drawing.Color]::FromArgb(225, 214, 178)
$brass = [System.Drawing.Color]::FromArgb(184, 132, 55)
$copper = [System.Drawing.Color]::FromArgb(168, 76, 44)
$iron = [System.Drawing.Color]::FromArgb(58, 61, 62)
$stone = [System.Drawing.Color]::FromArgb(32, 34, 33)
$amber = [System.Drawing.Color]::FromArgb(255, 160, 42)
$red = [System.Drawing.Color]::FromArgb(185, 26, 18)
$verdigris = [System.Drawing.Color]::FromArgb(46, 126, 111)
$rubber = [System.Drawing.Color]::FromArgb(12, 12, 11)

$brushBg = New-Object System.Drawing.SolidBrush $bg
$brushPaper = New-Object System.Drawing.SolidBrush $paper
$brushIvory = New-Object System.Drawing.SolidBrush $ivory
$brushMuted = New-Object System.Drawing.SolidBrush $muted
$brushBrass = New-Object System.Drawing.SolidBrush $brass
$brushCopper = New-Object System.Drawing.SolidBrush $copper
$brushIron = New-Object System.Drawing.SolidBrush $iron
$brushStone = New-Object System.Drawing.SolidBrush $stone
$brushAmber = New-Object System.Drawing.SolidBrush $amber
$brushRed = New-Object System.Drawing.SolidBrush $red
$brushVerdigris = New-Object System.Drawing.SolidBrush $verdigris
$brushRubber = New-Object System.Drawing.SolidBrush $rubber

$penLine = New-Object System.Drawing.Pen $line, 2
$penBrass = New-Object System.Drawing.Pen $brass, 8
$penCopper = New-Object System.Drawing.Pen $copper, 7
$penIron = New-Object System.Drawing.Pen $iron, 7
$penRubber = New-Object System.Drawing.Pen $rubber, 7
$penAmber = New-Object System.Drawing.Pen $amber, 5

$titleFont = New-Object System.Drawing.Font "Segoe UI", 30, ([System.Drawing.FontStyle]::Bold)
$subFont = New-Object System.Drawing.Font "Segoe UI", 12, ([System.Drawing.FontStyle]::Regular)
$familyFont = New-Object System.Drawing.Font "Segoe UI", 11, ([System.Drawing.FontStyle]::Bold)
$nameFont = New-Object System.Drawing.Font "Segoe UI", 13, ([System.Drawing.FontStyle]::Bold)
$idFont = New-Object System.Drawing.Font "Consolas", 8.5, ([System.Drawing.FontStyle]::Regular)

function Draw-Gauge([System.Drawing.Graphics]$g, [int]$cx, [int]$cy, [int]$r) {
    $g.FillEllipse($brushIvory, $cx - $r, $cy - $r, $r * 2, $r * 2)
    $g.DrawEllipse($penBrass, $cx - $r, $cy - $r, $r * 2, $r * 2)
    $needlePen = New-Object System.Drawing.Pen $red, 4
    $g.DrawLine($needlePen, $cx, $cy, $cx + [int]($r * 0.55), $cy - [int]($r * 0.35))
    $needlePen.Dispose()
}

function Draw-Icon([System.Drawing.Graphics]$g, [object]$piece, [int]$x, [int]$y, [int]$w, [int]$h) {
    $iconX = $x + 18
    $iconY = $y + 42
    $iconW = $w - 36
    $iconH = $h - 90
    $g.FillRectangle($brushStone, $iconX, $iconY, $iconW, $iconH)

    switch ($piece.family) {
        "Wall" {
            $g.FillRectangle($brushIron, $iconX + 14, $iconY + 18, $iconW - 28, $iconH - 36)
            $g.DrawLine($penBrass, $iconX + 26, $iconY + 44, $iconX + $iconW - 26, $iconY + 44)
            $g.DrawLine($penCopper, $iconX + 36, $iconY + 78, $iconX + $iconW - 38, $iconY + 78)
            if (($piece.tags -join ",").Contains("gauge")) { Draw-Gauge $g ($iconX + [int]($iconW * 0.68)) ($iconY + [int]($iconH * 0.56)) 26 }
            if (($piece.tags -join ",").Contains("gaslight")) { $g.FillEllipse($brushAmber, $iconX + [int]($iconW * 0.42), $iconY + 38, 52, 70); $g.DrawEllipse($penAmber, $iconX + [int]($iconW * 0.42), $iconY + 38, 52, 70) }
            if (($piece.tags -join ",").Contains("cable")) { $g.DrawLine($penRubber, $iconX + 26, $iconY + 105, $iconX + $iconW - 28, $iconY + 94) }
        }
        "Floor" {
            $g.FillRectangle($brushStone, $iconX + 10, $iconY + 28, $iconW - 20, $iconH - 56)
            for ($i = 0; $i -lt 6; $i++) {
                $sx = $iconX + 30 + ($i * 44)
                $g.FillRectangle($brushIron, $sx, $iconY + 44, 16, $iconH - 88)
            }
            if (($piece.tags -join ",").Contains("drain")) { $g.FillEllipse($brushIron, $iconX + [int]($iconW * 0.44), $iconY + 44, 62, 62); $g.DrawEllipse($penBrass, $iconX + [int]($iconW * 0.44), $iconY + 44, 62, 62) }
            if (($piece.tags -join ",").Contains("handrail")) { $g.DrawLine($penBrass, $iconX + 50, $iconY + 38, $iconX + $iconW - 52, $iconY + 38); $g.DrawLine($penBrass, $iconX + 62, $iconY + 38, $iconX + 62, $iconY + 118); $g.DrawLine($penBrass, $iconX + $iconW - 64, $iconY + 38, $iconX + $iconW - 64, $iconY + 118) }
        }
        "Ceiling" {
            $g.FillRectangle($brushIron, $iconX + 18, $iconY + 14, $iconW - 36, 28)
            $g.DrawLine($penBrass, $iconX + 26, $iconY + 70, $iconX + $iconW - 26, $iconY + 70)
            $g.DrawLine($penCopper, $iconX + 26, $iconY + 95, $iconX + $iconW - 50, $iconY + 95)
            if (($piece.tags -join ",").Contains("lamp")) { $g.FillEllipse($brushAmber, $iconX + [int]($iconW * 0.43), $iconY + 60, 58, 72); $g.DrawEllipse($penAmber, $iconX + [int]($iconW * 0.43), $iconY + 60, 58, 72) }
            if (($piece.tags -join ",").Contains("cable")) { $g.DrawLine($penRubber, $iconX + 26, $iconY + 118, $iconX + $iconW - 28, $iconY + 111) }
            if (($piece.tags -join ",").Contains("verdigris")) { $g.FillEllipse($brushVerdigris, $iconX + [int]($iconW * 0.35), $iconY + 104, 18, 34) }
        }
        "Doorway" {
            $g.FillRectangle($brushIron, $iconX + 40, $iconY + 22, $iconW - 80, 36)
            $g.FillRectangle($brushStone, $iconX + 64, $iconY + 58, $iconW - 128, $iconH - 78)
            $g.DrawLine($penBrass, $iconX + 46, $iconY + 65, $iconX + $iconW - 46, $iconY + 65)
            if (($piece.tags -join ",").Contains("gauge")) { Draw-Gauge $g ($iconX + [int]($iconW * 0.72)) ($iconY + [int]($iconH * 0.55)) 24 }
            if (($piece.tags -join ",").Contains("valve")) { $g.DrawEllipse($penAmber, $iconX + 92, $iconY + 92, 50, 50) }
            if (($piece.tags -join ",").Contains("threshold")) { $g.FillRectangle($brushBrass, $iconX + 36, $iconY + $iconH - 36, $iconW - 72, 16) }
        }
    }
}

$graphics.FillRectangle($brushBg, 0, 0, $width, $height)
$graphics.DrawString("Steam Corridor Dressing Set 09", $titleFont, $brushIvory, $margin, 34)
$graphics.DrawString("Deterministic catalog contact sheet - wall, floor, ceiling, and doorway dressing families", $subFont, $brushMuted, $margin + 4, 82)
$graphics.DrawString("Source: Runtime/Metadata/SCD09_SteamCorridorDressingSet09_Catalog_0.1.54-p001.json", $subFont, $brushMuted, $margin + 4, 106)

for ($i = 0; $i -lt $pieces.Count; $i++) {
    $row = [math]::Floor($i / $columns)
    $col = $i % $columns
    $x = $margin + ($col * ($cardW + $gap))
    $y = $titleHeight + ($row * ($cardH + $gap))
    $piece = $pieces[$i]

    $graphics.FillRectangle($brushPaper, $x, $y, $cardW, $cardH)
    $graphics.DrawRectangle($penLine, $x, $y, $cardW, $cardH)

    $familyColor = switch ($piece.family) {
        "Wall" { $brass }
        "Floor" { $copper }
        "Ceiling" { $verdigris }
        "Doorway" { $amber }
    }
    $familyBrush = New-Object System.Drawing.SolidBrush $familyColor
    $graphics.FillRectangle($familyBrush, $x, $y, $cardW, 9)
    $graphics.DrawString($piece.family.ToUpperInvariant(), $familyFont, $familyBrush, $x + 16, $y + 16)
    $familyBrush.Dispose()

    Draw-Icon $graphics $piece $x $y $cardW $cardH
    $graphics.DrawString($piece.displayName, $nameFont, $brushIvory, $x + 16, $y + $cardH - 44)
    $graphics.DrawString($piece.id, $idFont, $brushMuted, $x + 16, $y + $cardH - 22)
}

$bitmap.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)

$graphics.Dispose()
$bitmap.Dispose()
$titleFont.Dispose()
$subFont.Dispose()
$familyFont.Dispose()
$nameFont.Dispose()
$idFont.Dispose()
$brushBg.Dispose()
$brushPaper.Dispose()
$brushIvory.Dispose()
$brushMuted.Dispose()
$brushBrass.Dispose()
$brushCopper.Dispose()
$brushIron.Dispose()
$brushStone.Dispose()
$brushAmber.Dispose()
$brushRed.Dispose()
$brushVerdigris.Dispose()
$brushRubber.Dispose()
$penLine.Dispose()
$penBrass.Dispose()
$penCopper.Dispose()
$penIron.Dispose()
$penRubber.Dispose()
$penAmber.Dispose()

Write-Output $outputPath
