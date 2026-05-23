# Platform Target - Windows

## Role

Windows is the primary development and quality target for `Arcology Breach`.

Assume a mid-to-low level gaming PC, not a high-end showcase machine.

## Target Hardware Profile

Baseline planning target:

- CPU: older 4-core/6-core desktop CPU.
- GPU: entry/mid gaming GPU roughly GTX 1060 / GTX 1650 / RX 580 class or better.
- RAM: 8 GB system memory minimum, 16 GB recommended.
- VRAM: 4 GB target budget.
- Storage: keep builds reasonable; avoid huge uncompressed asset payloads.
- Display: 1080p primary.

## Performance Targets

- 60 FPS target at 1080p.
- 30 FPS minimum fallback only if absolutely necessary.
- Avoid frame spikes during combat.
- Keep enemy counts tuned for readability and performance.

## Content Budget Guidelines

These are starting budgets, not final technical limits:

- Texture sizes: mostly 1024 or 2048 for hero assets, 512/1024 for common props.
- Materials: limit unique materials per room; favor atlases/trim sheets.
- Lights: prefer baked/static lighting where possible; keep realtime lights purposeful.
- VFX: short-lived, readable, pooled where needed.
- Audio: compressed, short clips for combat events.
- Meshes: stylized efficiency over dense geometry.

## Rendering Direction

Prioritize:

- Strong silhouettes.
- Stylized neon lighting.
- Baked or mostly static light setups.
- Limited post-processing.
- Clear color coding.

Avoid:

- Heavy transparent overdraw everywhere.
- Excessive realtime shadows.
- Complex shader stacks that cannot scale down for Android/browser/VR.

## Input

Primary:

- Keyboard/mouse.

Keep future support in mind:

- Gamepad.
- VR controllers.
- Touch controls for Android.

Do not hard-code gameplay around only keyboard/mouse semantics. Route input through a future abstraction when systems expand.

## Build Notes

Current build method:

`V0SceneBuilder.BuildWindowsV0`

Current executable:

`Builds/Windows/<version>/ArcologyBreach_<version>.exe`

## Verification

Required for every major change:

- Unity editor smoke.
- Windows build.
- Packaged runtime smoke.

Required before milestone completion:

- Manual Windows playthrough.
- Basic performance observation in a visible build.
