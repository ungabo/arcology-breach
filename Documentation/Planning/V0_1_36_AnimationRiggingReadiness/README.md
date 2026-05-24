# V0.1.36 Animation And Rigging Readiness

Date: 2026-05-24
Package state: planning/staging bundle for later integration
Scope: rig sockets, animation backlog, Unity compatibility notes, and validation gates only

This bundle prepares the staged mechanical enemy and weapon families for a later animation pass without changing gameplay authority. It translates the v0.1.35 mechanical enemy pack and weapon arsenal into shared rig/socket standards, clip backlog names, import expectations, and acceptance gates.

## Owned Scope

- `Documentation/Planning/V0_1_36_AnimationRiggingReadiness/`
- `Documentation/AssetProduction/V0_1_36_AnimationRiggingReadiness/`

## Source Assumptions

- Scrapper, Lancer, Bulwark, Warden, and Foundry Overseer remain visual-only staged enemy families until a later gameplay integration task promotes them.
- Pressure Pistol and Steam Scattergun remain the primary weapon readiness targets.
- Future weapon silhouettes should inherit the same grip, muzzle, magazine/cell, pickup, wall-display, and VR hand-alignment conventions.
- Current route, combat, spawn, pickup, and validator authority remain outside this package.

## Files

- `V0_1_36_RIG_SOCKET_STANDARDS.md` - shared skeleton, socket, pivot, weak-point, weapon, and future silhouette naming rules.
- `V0_1_36_ANIMATION_CLIP_BACKLOG.md` - enemy and weapon clip backlog grouped by family and use case.
- `V0_1_36_UNITY_IMPLEMENTATION_NOTES.md` - Animator, AnimationClip, procedural motion, low/mid PC, and VR-readiness notes.
- `V0_1_36_ACCEPTANCE_GATES_AND_VALIDATION.md` - gates and targeted validation ideas that preserve readability and gameplay authority.
- Asset-production handoff files under `Documentation/AssetProduction/V0_1_36_AnimationRiggingReadiness/`.

## Guardrails

- No scripts.
- No scenes.
- No prefabs.
- No validators.
- No build settings.
- No package files.
- No shared status, ledger, release, or session files.
- No direct gameplay wiring.

