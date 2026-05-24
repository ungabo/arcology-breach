# V0.1.48 Gameplay Promotion Planning QA Checklist

Purpose: verify this packet is docs-only, parseable, and useful for future gameplay-promotion slices.

## Scope Checks

| Check | Status | Evidence |
| --- | --- | --- |
| Writes limited to allowed planning root | Pass | `Documentation/Planning/V0_1_48_GameplayPromotionPlan/` |
| Writes limited to allowed QA root | Pass | `Documentation/QA/V0_1_48_GameplayPromotionPlan/` |
| No code, package, scene, shared status doc, or git history edits required | Pass | Packet is Markdown and JSON only |
| Imported package list sourced from active manifest | Pass | `Packages/manifest.json` imports 15 sidecar packages |
| v0.1.45 quarantine boundary retained | Pass | Release notes state sidecars remain visual-review content |
| Showcase authority retained | Pass | `V0SceneBuilder` places quarantine showcases and strips presentation physics |

## Content Checks

| Check | Status | Evidence |
| --- | --- | --- |
| Promotion matrix groups all imported sidecars | Pass | `PROMOTION_MATRIX_v0.1.48.md` includes all 15 imported sidecar packages |
| Risk/authority matrix states visual-only constraints | Pass | `RISK_AUTHORITY_MATRIX_v0.1.48.md` |
| Next 5 milestone batches recommended | Pass | `MILESTONE_BATCHES_AFTER_v0.1.47.md` |
| Future v0.1.46 packets handled as context, not imported baseline | Pass | Deferred note in milestone packet |
| JSON review summary parses cleanly | Pass | `ConvertFrom-Json` returned `JSON_PARSE_PASS` |
| New docs contain no trailing whitespace | Pass | Trailing whitespace scan returned `TRAILING_WHITESPACE_PASS` |

## Future Slice QA Gates

Before any sidecar graduates out of quarantine, the owning implementation slice should add evidence for:

- Collider proxy review and movement/stuck-point route pass.
- Objective interaction radius, prompt, completion, and softlock checks.
- VFX socket trigger matrix and particle lifetime/performance checks.
- Lighting readability and performance captures.
- Enemy visual-to-hitbox overlay review and combat route tests.
- Weapon muzzle origin, FOV clipping, recoil/reload timing, and pickup balance tests.
- Audio mixer routing, settings coverage, cooldowns, and runtime mix validation.
