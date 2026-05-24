# Brassworks Breach Enemy Animation Proxy Set 01

Version: `v0.1.44-p001`

This package is a self-contained Unity sidecar for visual-only enemy animation pose proxies. It is not gameplay content: no AI, damage, movement, colliders, nav obstacles, rigging, autonomous audio, or runtime scripts are included.

## Contents

- `Runtime/Prefabs`: generated visual candidate prefabs.
- `Runtime/Materials`: generated steampunk material library.
- `Runtime/Meshes`: reusable procedural mesh accents used by the prefabs.
- `Runtime/AnimationClips`: package-local placeholder timing labels, not authoritative gameplay animation.
- `Runtime/Metadata`: runtime catalog JSON for the proxy batch.
- `Editor`: Unity editor generator and batch validation entry points.
- `Documentation~/Manifest`: package-local sidecar manifest.
- `Samples~/PreviewScene`: preview-scene notes.
- `ValidationProject~`: package-local Unity project for isolated generation and smoke validation.

## Generation Menu

After referencing this package in a Unity project, run:

- `Brassworks Breach/Sidecars/Enemy Animation Proxy Set 01/Generate Package`
- `Brassworks Breach/Sidecars/Enemy Animation Proxy Set 01/Render Preview PNGs`

Batch entry point:

`BrassworksBreach.EnemyAnimationProxySet01.Editor.EnemyAnimationProxySet01Generator.GenerateValidateAndQuit`

## Visual Families

- Scrapper / Ashcan: idle brace, windup coil, saw lunge, and recover drag proxies.
- Lancer / Pressure Spindle: aim line, charge step, thrust peak, and recoil vent proxies.
- Bulwark / Gatehammer: guard set, hammer raise, slam impact, and stagger-open proxies.
- Warden / Governor: command idle, signal raise, order pulse, and shutdown drop proxies.

Every prefab includes named visual part groups for `chassis`, `boiler`, `lens`, `saw_limb`, `pressure_lines`, `rivets`, `warning_lamps`, `smoke_stacks`, and `armor_plates`, plus package-local socket markers under `sockets_visual_markers`.

## Scope Rules

- Visual-only prefabs and generated assets only.
- No runtime assembly and no gameplay components.
- Unity primitives and procedural meshes only.
- Preview PNGs are rendered by Unity from generated prefabs.
- Primary project manifests and gameplay scenes are intentionally untouched.
