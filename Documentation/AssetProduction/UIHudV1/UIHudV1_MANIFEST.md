# UIHudV1 Manifest

Generated: 2026-05-23T23:30:34-04:00

Scope: staged V1 HUD/UI art assets only. No gameplay scripts, Unity scenes, existing UI code, README, BUILD_STATUS, WORK_LEDGER, ConceptRenders, or active Bulwark/pressure-pistol files were modified.

## Visual Target

- Dark oil/iron instrument-panel base with worn brass trim.
- Cream enamel for readable gauge faces and labels.
- Red/orange for health, pressure danger, boss pressure, and denied states.
- Amber/brass for ammo, pressure hardware, interactable prompts, and useful machinery.
- Green reserved for success/restored/key-acquired states.

## Palette

| Token | Hex | Use |
| --- | --- | --- |
| `oil_black` | `#0c0a08` | Deep transparent-edge shadow and UI backdrops |
| `oil_panel` | `#17100b` | Main panel fill |
| `iron_dark` | `#211d19` | Dark metal plates and icon interiors |
| `brass` | `#b87928` | Primary brass trim |
| `brass_bright` | `#f0b95a` | Raised brass highlights |
| `brass_hot` | `#ffd17a` | High-readability brass highlights |
| `cream` | `#f2dfad` | Text and gauge enamel |
| `health_red` | `#d03424` | Health/boss pressure fill |
| `pressure_amber` | `#f0a33a` | Ammo/pressure fill |
| `danger_orange` | `#ff612e` | Warning/denied accents |
| `success_green` | `#4ddf72` | Acquired/unlocked/success state |
| `steam_cyan` | `#a6e7e3` | Secondary precision/steam reticle cue |

## Accessibility Contrast Notes

- Cream text on oil panel: 14.29:1, suitable for HUD labels.
- Hot brass on oil panel: 13.14:1, suitable for highlights and selected states.
- Pressure amber on oil black: 9.42:1, strong enough for fills/icons.
- Danger orange on oil black: 6.59:1, strong enough for warning accents.
- Health red on oil black: 3.95:1, acceptable for large fills but should not be the only critical signal.
- Success green on oil black: 11.4:1, strong enough for acquired/unlocked lamps.
- Danger, key, and prompt states include shape changes or iconography so the UI is not color-only.

## Unity Import Settings

- Texture Type: `Sprite (2D and UI)`.
- Sprite Mode: `Single`.
- Mesh Type: `Full Rect` for gauge frames, fills, panels, and sliced sprites; `Tight` is acceptable for standalone icons/reticles.
- sRGB: on for all color PNGs.
- Alpha Is Transparency: on.
- Mip Maps: off for UI.
- Filter Mode: `Bilinear`.
- Compression: `None` for source review; later platform variants can use high-quality compression after readability checks.
- Max Size: 1024 for HUD/menu panels and 256 or 512 for icons/reticles, matching source dimensions.

## Nine-Slice Notes

| Family | Suggested Border | Notes |
| --- | --- | --- |
| Health/ammo frames | L/R 34 px, T/B 22 px | Sliced, keep fill window center flexible. |
| Boss pressure frame | L/R 42 px, T/B 22 px | Sliced if width changes; current source supports 768 px top-center use. |
| Objective/prompt backplates | L/R 32-34 px, T/B 20-22 px | Sliced, leave icon/lamp socket fixed by layout. |
| Large menu panel | 48 px all sides | Sliced for main/pause/settings containers. |
| Menu buttons | L/R 20 px, T/B 16 px | Sliced state sprites: normal, hover, pressed. |
| Slider track | L/R 18 px, T/B 10 px | Sliced or fixed width. |
| Icons, lamps, reticles, dial | None | Fixed-size sprites. |

## Ready For Integration

- `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_HealthGauge_Frame_512x96.png` - Drop-in health backplate/frame for lower-left HUD.
- `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_HealthGauge_Fill_Red_384x32.png` - Segmented health/pressure-fluid fill source.
- `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_PressureAmmoGauge_Frame_512x96.png` - Horizontal ammo gauge compatible with current HUD fill behavior.
- `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_PressureAmmoGauge_Fill_Amber_384x32.png` - Pressure-ammo fill source.
- `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_BossPressureGauge_Frame_768x96.png` - Top-center boss pressure gauge frame.
- `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_BossPressureGauge_Fill_Red_704x24.png` - Boss pressure fill compatible with current bossFillImage.
- `Assets/_Project/ArtStaging/UIHudV1/Panels/HUD_ObjectiveBackplate_640x72.png` - Objective HUD backplate with lamp socket.
- `Assets/_Project/ArtStaging/UIHudV1/Panels/HUD_PromptBackplate_640x80.png` - Interaction prompt panel with built-in E keycap slot.
- `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_BrassPanel_768x384.png` - Large main/pause/settings brass panel.
- `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_Header_768x96.png` - Header or title strip for menu UI.
- `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_Button_Normal_320x64.png` - Button state sprite: normal.
- `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_Button_Hover_320x64.png` - Button state sprite: hover.
- `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_Button_Pressed_320x64.png` - Button state sprite: pressed.
- `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_SliderTrack_360x40.png` - Slider track/backplate.
- `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_SliderHandle_48x48.png` - Valve-wheel slider handle.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/HUD_KeyLamp_Off_96x96.png` - Gear/objective lamp state: off.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/HUD_KeyLamp_On_96x96.png` - Gear/objective lamp state: on.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/HUD_KeyLamp_Denied_96x96.png` - Gear/objective lamp state: denied.
- `Assets/_Project/ArtStaging/UIHudV1/Reticles/RETICLE_BrassCrosshair_64x64.png` - Transparent reticle variant: brass_crosshair.
- `Assets/_Project/ArtStaging/UIHudV1/Reticles/RETICLE_PressurePinpoint_64x64.png` - Transparent reticle variant: pressure_pinpoint.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_InteractE_96x96.png` - Prompt icon for interact e.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_GearKey_96x96.png` - Prompt icon for gear key.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Valve_96x96.png` - Prompt icon for valve.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Lift_96x96.png` - Prompt icon for lift.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Ammo_96x96.png` - Prompt icon for ammo.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Health_96x96.png` - Prompt icon for health.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Warning_96x96.png` - Prompt icon for warning.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Secret_96x96.png` - Prompt icon for secret.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Pause_96x96.png` - Prompt icon for pause.
- `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_MouseRight_96x96.png` - Prompt icon for mouse right.

## Rough Or Requires Layout Work

- `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_PressureAmmoDial_Frame_256x256.png` - Radial ammo pressure dial concept. Requires UI script/layout work before integration.
- `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_PressureAmmoDial_FillArc_256x256.png` - Radial arc source for future pressure dial.
- `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_PressureAmmoDial_Needle_256x256.png` - Separate needle sprite for future pressure dial.
- `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_CornerCap_64x64.png` - Standalone corner cap for custom assembled panels.
- `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_EdgeHorizontal_256x32.png` - Optional horizontal panel rail.
- `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_EdgeVertical_32x256.png` - Optional vertical panel rail.

## Preview Files

- `Assets/_Project/ArtStaging/UIHudV1/Previews/PREVIEW_UIHudV1_ContactSheet.png` - Contact sheet for art review.
- `Assets/_Project/ArtStaging/UIHudV1/Previews/PREVIEW_UIHudV1_HUDMockup_1920x1080.png` - Composited HUD scale mockup.
- `Documentation/AssetProduction/UIHudV1/PREVIEW_UIHudV1_ContactSheet.png` - documentation copy.
- `Documentation/AssetProduction/UIHudV1/PREVIEW_UIHudV1_HUDMockup_1920x1080.png` - documentation copy.

## Full Asset Inventory

| File | Size | Status | Integration | Nine-slice |
| --- | ---: | --- | --- | --- |
| `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_HealthGauge_Frame_512x96.png` | 512x96 | ready | ready | Suggested border: L/R 34 px, T/B 22 px. Use Sliced Image for flexible width. |
| `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_HealthGauge_Fill_Red_384x32.png` | 384x32 | ready | ready | Do not nine-slice; use Image.Type.Filled Horizontal, origin Left. |
| `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_PressureAmmoGauge_Frame_512x96.png` | 512x96 | ready | ready | Suggested border: L/R 34 px, T/B 22 px. Use Sliced Image for flexible width. |
| `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_PressureAmmoGauge_Fill_Amber_384x32.png` | 384x32 | ready | ready | Do not nine-slice; use Image.Type.Filled Horizontal, origin Left. |
| `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_PressureAmmoDial_Frame_256x256.png` | 256x256 | rough | review/rough | No nine-slice. Use as fixed-size dial face. |
| `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_PressureAmmoDial_FillArc_256x256.png` | 256x256 | rough | review/rough | No nine-slice. Use radial filled Image or mask. |
| `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_PressureAmmoDial_Needle_256x256.png` | 256x256 | rough | review/rough | No nine-slice. Rotate RectTransform around center. |
| `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_BossPressureGauge_Frame_768x96.png` | 768x96 | ready | ready | Suggested border: L/R 42 px, T/B 22 px. Use Sliced Image if boss names vary. |
| `Assets/_Project/ArtStaging/UIHudV1/Gauges/HUD_BossPressureGauge_Fill_Red_704x24.png` | 704x24 | ready | ready | Do not nine-slice; use Image.Type.Filled Horizontal, origin Left. |
| `Assets/_Project/ArtStaging/UIHudV1/Panels/HUD_ObjectiveBackplate_640x72.png` | 640x72 | ready | ready | Suggested border: L/R 32 px, T/B 20 px. Use Sliced Image. |
| `Assets/_Project/ArtStaging/UIHudV1/Panels/HUD_PromptBackplate_640x80.png` | 640x80 | ready | ready | Suggested border: L/R 34 px, T/B 22 px. Use Sliced Image. |
| `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_BrassPanel_768x384.png` | 768x384 | ready | ready | Suggested border: 48 px on all sides. |
| `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_Header_768x96.png` | 768x96 | ready | ready | Suggested border: L/R 48 px, T/B 22 px. |
| `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_Button_Normal_320x64.png` | 320x64 | ready | ready | Suggested border: L/R 20 px, T/B 16 px. |
| `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_Button_Hover_320x64.png` | 320x64 | ready | ready | Suggested border: L/R 20 px, T/B 16 px. |
| `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_Button_Pressed_320x64.png` | 320x64 | ready | ready | Suggested border: L/R 20 px, T/B 16 px. |
| `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_SliderTrack_360x40.png` | 360x40 | ready | ready | Suggested border: L/R 18 px, T/B 10 px. |
| `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_SliderHandle_48x48.png` | 48x48 | ready | ready | No nine-slice. |
| `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_CornerCap_64x64.png` | 64x64 | rough | review/rough | No nine-slice; use as decorative overlay. |
| `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_EdgeHorizontal_256x32.png` | 256x32 | rough | review/rough | Tile or stretch horizontally; border optional L/R 16 px. |
| `Assets/_Project/ArtStaging/UIHudV1/Panels/PANEL_Menu_EdgeVertical_32x256.png` | 32x256 | rough | review/rough | Tile or stretch vertically; border optional T/B 16 px. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/HUD_KeyLamp_Off_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed-size icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/HUD_KeyLamp_On_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed-size icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/HUD_KeyLamp_Denied_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed-size icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Reticles/RETICLE_BrassCrosshair_64x64.png` | 64x64 | ready | ready | No nine-slice. Fixed 64 px reticle. |
| `Assets/_Project/ArtStaging/UIHudV1/Reticles/RETICLE_PressurePinpoint_64x64.png` | 64x64 | ready | ready | No nine-slice. Fixed 64 px reticle. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_InteractE_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed 96 px icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_GearKey_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed 96 px icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Valve_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed 96 px icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Lift_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed 96 px icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Ammo_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed 96 px icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Health_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed 96 px icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Warning_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed 96 px icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Secret_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed 96 px icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_Pause_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed 96 px icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Icons/ICON_Prompt_MouseRight_96x96.png` | 96x96 | ready | ready | No nine-slice. Fixed 96 px icon. |
| `Assets/_Project/ArtStaging/UIHudV1/Previews/PREVIEW_UIHudV1_ContactSheet.png` | 1120x1880 | review | review/rough | None. |
| `Assets/_Project/ArtStaging/UIHudV1/Previews/PREVIEW_UIHudV1_HUDMockup_1920x1080.png` | 1920x1080 | review | review/rough | None. |

## Later Integration Tasks

- Import runtime sprites as Sprite (2D and UI), disable mip maps, preserve alpha, and keep source compression lossless until readability is checked in-game.
- Replace current solid-color HUD Image backplates/fills with these sprites in a dedicated integration branch; do not alter gameplay logic.
- Set horizontal fills to Image.Type Filled/Horizontal with Left origin; use frames as Sliced where manifest borders are supplied.
- If adopting the circular ammo dial, add layout and needle/arc driving separately because current HUDController drives horizontal fillAmount.
- Tune CanvasScaler reference resolution and anchored offsets against 16:9, 16:10, and ultrawide before committing final placements.
- Verify red/orange pressure warnings with color-blind filters; supplement danger states with icon shape and text, not color alone.
