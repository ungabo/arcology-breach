# Pressure Gauge Prototype Brief v0.1.11

Status: implemented prototype in `v0.1.11`, not final AAA art  
Owner lane: asset production / prop lookdev  
Write scope: `Documentation/AssetProduction/PressureGaugePrototype/`

## Purpose

Define the v0.1.11 wall pressure gauge prototype as a reusable Brassworks prop standard before any final-art promotion. The gauge should support corridor dressing, pressure gates, service lifts, routing valves, weapon feedback close-ups, and HUD/world readability studies without requiring Unity code, scene, or prefab edits in this pass.

This brief began as production alignment. The main lane then implemented a Unity-owned procedural prototype using the same component language in the Pressure Pistol viewmodel and Level01 pressure gate. It is verified as a promoted prototype, not approved as a shipping final mesh, final prefab, final material, texture, animation, VFX, or audio asset.

## Main-Lane Implementation Update

Implemented in `v0.1.11`:

- `Assets/_Project/Scripts/Utility/PressureGaugePrototype.cs` marker component.
- `Pressure Pistol Prototype Pressure Gauge` generated under the first-person viewmodel.
- `Pressure Gate Prototype Gauge` generated on the Level01 pressure gate.
- Validator coverage for marker metadata, required renderers, named parts, 16 tick marks, 12 bezel rivets, and material-role names.
- Full route audit and V0 build matrix pass for `Builds/Windows/v0.1.11/BrassworksBreach_v0.1.11.exe`.

## North-Star Art Direction

Target the established `Brassworks Breach` north star:

- Handmade analog machinery: brass, copper, riveted iron, cream enamel, ambered glass, soot, oil grime, steam staining, and mechanical fasteners.
- Readable industrial function: a player should understand "pressure state" from the silhouette, needle angle, colored danger band, pipe coupling, and mounting bracket before reading any label.
- Compact Victorian factory language: layered bezels, exposed screws, enamel face, black needle, copper feed pipe, stamped tick marks, and practical service hardware.
- Color logic: cream/black for neutral measurement, amber for attention, red/red-orange for danger or overpressure, green only for relieved/restored/service states.
- Wear pattern: oily fingerprints around screws, soot darkening near pipe ports, chipped enamel at rim edges, cloudy glass scratches, and tarnished brass high spots.

Avoid clean sci-fi displays, digital readouts, hologram styling, black chrome, neon strips, and decorative detail that makes the needle or state unreadable.

## Required Component Hierarchy And Named Parts

Future prefab or staged model should preserve these logical names so gates, valves, HUD studies, VFX, and animation can share expectations:

```text
PF_PROP_PressureGauge_Prototype_v011
  SM_Gauge_MountBackplate
  SM_Gauge_RivetedBracket
  SM_Gauge_OuterBezel_AgedBrass
  SM_Gauge_InnerRing_BlackenedIron
  SM_Gauge_Face_CreamEnamel
  SM_Gauge_TickMarks
  SM_Gauge_Numerals
  SM_Gauge_NeedlePivot
  SM_Gauge_Needle
  SM_Gauge_DangerBand_Red
  SM_Gauge_ServiceBand_Green
  SM_Gauge_Glass_AmberClouded
  SM_Gauge_GlassCracks_Optional
  SM_Gauge_Screws_Rim
  SM_Gauge_PipeSocket_Left
  SM_Gauge_PipeSocket_Right
  SM_Gauge_CopperFeedPipe_Optional
  SOCKET_GaugeNeedle
  SOCKET_GaugeSteamPuff
  SOCKET_GaugeSpark
  COL_Gauge_WallSimple
```

Minimum prototype read must include the bezel, face, needle, danger band, glass, screws, wall mount, and at least one pipe socket. Optional cracks and feed pipe may be omitted for lower-tier variants but should remain named if present.

## Material Roles

| Role | Intended material family | Notes |
| --- | --- | --- |
| Outer bezel / screws | Aged brass | Warm, tarnished edges; avoid polished gold. |
| Inner ring / bracket | Blackened riveted iron | Dark frame helps cream face and needle read. |
| Face | Cream enamel gauge | Chipped, slightly stained, high contrast tick marks. |
| Needle / tick marks | Black iron or dark enamel | Must remain readable under amber, red, and green lighting. |
| Danger band | Red/red-orange hazard enamel | Should read from gameplay distance without looking neon. |
| Service band | Muted green enamel | Use sparingly for relieved/restored variants. |
| Glass | Grimy amber glass | Transparent or faked gloss; keep glare from hiding the needle. |
| Pipe socket / feed pipe | Oxidized copper or aged brass | Adds pressure-system logic and directional context. |
| Dirt/wear | Oil, soot, scorch, chipped enamel | Use controlled grime at edges and ports, not full-surface noise. |

Recommended runtime material budget for a small repeated prop is 2 to 3 slots: metal hardware, enamel face/marks, glass. A hero close-up variant may justify a fourth detail/decal slot if it demonstrably improves readability.

## Validation Gates

Use `accepted-with-notes` at most until an in-engine proof exists. Final acceptance should reference the shared Unity asset gates.

| Gate ID | Check | Prototype pass measure |
| --- | --- | --- |
| COM-01 | North-star role named | Declares prop role as pressure-system gauge for corridor, gate, lift, valve, weapon, or HUD study. |
| COM-02 | Unity proof | Passed for procedural generated-scene prototype through level validation and full build matrix; still needs human visual screenshot review before final art promotion. |
| COM-03 | Style match | Brass/copper/iron/enamel/glass/steam-pressure language, no clean sci-fi. |
| COM-04 | Material vocabulary | Uses approved aged brass, blackened iron, cream enamel gauge, amber glass, copper, oil/soot families. |
| COM-05 | Texture import notes | Must specify sRGB color maps, normal import, ORM linear data, mipmaps, and platform overrides. |
| COM-06 | Geometry budget | Repeated wall prop target: 800 to 2,500 tris LOD0; hero/close-up target: 2,500 to 5,000 tris LOD0. |
| COM-07 | LOD plan | Repeated usage needs LOD0/LOD1/LOD2; needle silhouette must survive LOD1. |
| COM-08 | Collision plan | Simple wall collision only; no render-mesh collision and no snagging screws/pipes. |
| COM-09 | Lighting behavior | Needle and danger band must read under warm amber, red warning, green service, and dark corridor lighting. |
| COM-10 | Performance note | Platform reduction notes present for Windows, Android, WebGL, and VR. |
| COM-11 | Naming/versioning | Uses `PF_PROP_PressureGauge_Prototype_v011` family naming and does not overwrite active prefabs/materials. |
| COM-12 | Dependencies listed | Names material families, sockets, optional VFX hooks, collision, and any future animation hook. |

Required proof before promotion:

- Neutral prefab/contact screenshot.
- Player-height corridor screenshot at 6m to 10m.
- Close screenshot proving needle, tick marks, glass, and band readability.
- One overpressure/red state and one service/green or neutral state if the asset supports variants.
- Triangle count, material slot count, texture sizes, LOD status, collision status, and platform tier notes.

## Platform Notes

### Windows

- Baseline quality target: 1080p, 60 FPS, mid/low gaming PC.
- Allow 1024 or 2048 texture set for hero close-up use; repeated corridor gauges should usually fit 512 to 1024.
- Keep glass affordable. Prefer simple transparent material or faked highlight if many gauges are visible.
- Needles, tick marks, and red danger band must read from normal FPS movement distance.

### Android

- Plan 512 texture fallback, merged material slots, simplified glass, and lower-poly LOD1/LOD2 for repeated corridor use.
- Avoid tiny numerals as critical gameplay text; rely on needle angle and color bands.
- Reduce any steam puff or spark VFX linked to the gauge.

### WebGL

- Favor atlas-friendly face/marks and shared metal/glass materials to reduce download and memory footprint.
- Avoid expensive transparency stacks; use opaque glass highlight decals where possible.
- Keep unique variants limited: neutral, danger, and service states should share texture space when practical.

### VR

- Gauge scale must feel physical at arm's length and remain readable without forcing the player close to a wall.
- Avoid high-frequency tick shimmer, tiny critical text, and bright glare near the face.
- Collision must be simple and comfortable for close inspection; pipes/screws should not create hand or head snag points.
- Future diegetic/wrist/weapon gauge variants should preserve the same needle, color, and material language.

## Blockers Before Final AAA Promotion

- Unity generated-scene proof exists for the procedural prototype, but no final authored mesh or reusable prefab package exists yet.
- No final import settings beyond the runtime marker script `.meta` exist for an authored gauge asset package.
- No measured triangle count, material slot count, LOD chain, or collision mesh has been produced.
- No authored texture set or verified PBR material binding exists for this specific prototype.
- No state animation, needle driving logic, VFX hookups, or audio cues have been integrated.
- No platform-specific import overrides or runtime quality tiers have been verified.
- No human art-review signoff against the north-star concept has been recorded.

The asset has moved from `briefed` to `implemented prototype` in generated gameplay scenes. It can move toward final AAA promotion only after authored mesh/material production, prefab ownership, platform tiering, screenshot/art review, and final acceptance-gate evidence are complete.
