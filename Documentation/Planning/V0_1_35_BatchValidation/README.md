# Brassworks Breach - v0.1.35 Batch Validation Packet

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_35_BatchValidation/`

## Purpose

This packet defines acceptance gates and smoke guidance for the `v0.1.35` gameplay-systems batch. It is meant to validate a larger playable leap across weapon feedback, enemy feedback, pickups, objectives, secrets, pause/settings, and audio/VFX hooks while preserving the current route authority.

## Packet Contents

- `BATCH_VALIDATION_GATES_v0.1.35.md` - no-authority contract, lane-specific acceptance gates, route safety checks, and pass/fail rules.
- `TARGETED_SMOKE_COMMANDS_v0.1.35.md` - candidate smoke commands and expected pass markers before the full V0 matrix.

## Top Validation Gates

1. Route authority gate: feedback work must not create unauthorized colliders, triggers, nav blockers, route state, pickup state, damage, prompt, objective, save, transition, or boss-lock authority.
2. Batch breadth gate: the candidate must improve several gameplay systems together, not ship as a one-effect patch.
3. Player-juice gate: weapon, pickup, impact, and objective feedback must clarify state without hiding HUD, prompts, enemies, hazards, or route cues.
4. Enemy-feedback gate: hit/death feedback must improve confirmation while preserving attack tells, silhouettes, and boss unlock readability.
5. Settings gate: pause, display, readability, and audio settings must remain stable during and after gameplay events.
6. Validation gate: targeted smokes pass first; the existing full V0 matrix remains the release-candidate gate.

