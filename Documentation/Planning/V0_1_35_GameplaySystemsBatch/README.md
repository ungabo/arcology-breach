# Brassworks Breach - v0.1.35 Gameplay Systems Batch

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_35_GameplaySystemsBatch/`

## Purpose

This packet defines a bundled `v0.1.35` gameplay-systems milestone that the main lane can implement while asset teams continue art/audio production. The intent is one substantial playable leap: player juice, enemy feedback, pickup/readability cues, objectives, secrets, pause/settings polish, and audio/VFX hooks land together under one validation contract.

This is planning-only. Do not treat this packet as permission to edit scripts, scenes, builders, validators, packages, release notes, `WORK_LEDGER`, `BUILD_STATUS`, or `SESSION_LOG`.

## Packet Contents

- `V0_1_35_GAMEPLAY_SYSTEMS_BATCH_PLAN.md` - milestone shape, system slices, implementation order, and acceptance targets.
- `V0_1_35_DEPENDENCY_MAP.md` - main-lane-now work versus art/audio follow-up integration.

## Milestone Thesis

`v0.1.35` should make the current five-level route feel more responsive and understandable without moving route authority. It should not add new routes, new blockers, new collider/triggers, new enemy navigation rules, or new objective ownership. It should make existing play read better:

- Weapons communicate impact, state, and pickup value.
- Enemies communicate hit, stagger, death, attack tells, and boss state more clearly.
- Pickups are visible and differentiated under combat pressure.
- Objectives and route affordances explain what changed after key actions.
- Secrets reward discovery without stealing main-route authority.
- Pause, display, and readability settings remain usable inside the gameplay loop.
- Audio/VFX hooks exist as stable integration points even when final assets are not ready.

## Recommended Main-Lane Batch

Implement as a single candidate branch/slice with one compile-and-smoke push after the first coherent pass:

1. Add/route lightweight feedback events for weapon fire, impact, pickup, enemy hit, enemy death, objective completion, secret found, pause/resume, and settings changes.
2. Bind those events to existing primitive VFX/audio/UI affordances first, using temporary Unity-safe effects where final assets are missing.
3. Add route-readable objective and secret feedback around existing authority objects only.
4. Harden pause/settings flows against active combat, pickups, and route transitions.
5. Run targeted smokes from the validation packet before the full V0 matrix.

The key production constraint is simple: the batch may improve feedback around an authoritative object, but it must not create a second authority object.

