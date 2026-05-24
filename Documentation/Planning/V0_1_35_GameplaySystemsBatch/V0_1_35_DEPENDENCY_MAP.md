# Brassworks Breach - v0.1.35 Dependency Map

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_35_GameplaySystemsBatch/`

## Purpose

Separate work the main lane can implement immediately from follow-up art/audio integration, so `v0.1.35` can advance while asset teams continue production.

## Main Lane Can Implement Now

| Lane | Main-lane-now work | Validation expectation |
| --- | --- | --- |
| Weapon feedback | Placeholder muzzle flash, impact puff, recoil/camera impulse if already supported, dry-fire cue, pickup/switch confirmation. | `V0_WEAPON_SWITCH_PASS`, `V0_COMBAT_SCENARIO_PASS`, `V0_AUDIO_MIX_PASS`. |
| Enemy hit/death feedback | Shared hit flash/shutdown profile using current enemy health/death paths; per-class tuning values. | `V0_COMBAT_SMOKE_PASS`, `V0_COMBAT_EDGE_PASS`, enemy-specific combat smokes. |
| Pickup readability | Collection burst/audio/HUD confirmation on existing health, ammo, key, and weapon pickups. | `V0_INTERACTION_SMOKE_PASS`, `V0_WEAPON_SWITCH_PASS`, flow smokes. |
| Objective affordance | Locked rejection, valve complete, lift restored, boss unlock, and final exit feedback on existing route-authority objects. | `V0_AUTO_PLAYTHROUGH_PASS`, flow smokes, route audit. |
| Secret feedback | Discovery confirmation and optional clue-state treatment using existing secret definitions only. | `V0_SECRET_PASS`, route audit. |
| Pause/settings polish | Pause/resume hardening, display/readability feedback, audio mix continuity. | `V0_PAUSE_FLOW_PASS`, `V0_DISPLAY_SETTINGS_PASS`, `V0_READABILITY_SETTINGS_PASS`, `V0_AUDIO_MIX_PASS`. |
| Audio/VFX hooks | Stable hook names and placeholder fallback bindings for all batch events. | Missing final art/audio cannot fail gameplay route validation. |

## Follow-Up Art/Audio Integration

| Lane | Asset-team follow-up | Main-lane contract |
| --- | --- | --- |
| Weapon feedback | Final muzzle flashes, casing/steam puffs, impact decals, weapon-specific fire/empty/switch sounds. | Asset swaps bind to existing hooks; no gameplay logic moves into assets. |
| Enemy feedback | Enemy-specific hit sparks, shutdown fragments, death audio, boss phase/reveal VFX. | Effects remain children/attachments of existing enemy authority roots; no new hitboxes. |
| Pickups | Final health/ammo/key/weapon pickup burst VFX and pickup stingers. | Effects do not obscure pickup prompt, pickup root, or route object. |
| Objectives | Final gate/valve/lift/hoist state lamps, sounders, pressure vent effects, objective UI polish. | Final art follows amber/red/green route language and binds to existing state changes. |
| Secrets | Secret found flourish, clue glow, cache ambience. | Secret cue stays optional and does not introduce new discovery volumes. |
| Pause/settings | UI skins, menu sounds, mix snapshots, readability preview polish. | Settings remain reversible and smoke-testable without final UI art. |

## Cross-Team Handoff Rules

- Assets may replace placeholder presentation but may not own route, pickup, damage, objective, prompt, save, transition, or nav authority.
- Final VFX must have a no-op fallback or safe missing-reference behavior.
- Final audio must be mix-safe: no cue should mask enemy tells, low-health warnings, objective feedback, or pause/settings confirmation.
- Any asset that needs a collider for rendering or interaction must be explicitly escalated to the main lane and rejected from feedback-only integration until validated.
- Art/audio follow-up should be merged after the gameplay hook packet passes targeted smokes, not during initial route-authority debugging.

## Dependency Risks

| Risk | Why it matters | Mitigation |
| --- | --- | --- |
| Feedback becomes gameplay authority | Missing VFX/audio could break player understanding or validation. | Keep authority in current scripts/objects; hooks are presentation subscribers. |
| Color language conflicts | New effects can confuse locked/safe/hazard states. | Reserve amber/red/green meanings and require readability review. |
| VFX masks enemy tells | Juice can make combat feel worse if it hides windups/projectiles. | Validate with combat edge, ranged, Bulwark, and Warden smokes plus human review. |
| Pause changes destabilize runtime state | Time scale, cursor, audio, and input are easy to desync. | Run pause/display/readability/audio smokes before full matrix. |
| Secret cues pull players off route | Strong optional cues can disrupt first-run route readability. | Keep secret clues lower priority than objective cues and verify route audit/flow smokes. |

