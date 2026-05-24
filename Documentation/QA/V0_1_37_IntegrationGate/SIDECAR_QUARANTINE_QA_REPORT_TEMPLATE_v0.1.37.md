# V0.1.37 Sidecar Quarantine QA Report Template

Purpose: record whether a sidecar package is safe to move from isolated asset production into primary-project quarantine and then live production.

## Report Header

- Report timestamp:
- Reviewer:
- Package:
- Version:
- Build ID:
- Sidecar project:
- Source artifact:
- Manifest:
- Import method:
- Unity editor version:
- Primary project branch/commit:

## Static Validation

- Validator command:
- Validator result: pass / fail
- Errors:
- Warnings:
- Accepted warnings:
- Required fixes:

## Quarantine Import

- Quarantine root:
- Files imported:
- Files changed outside quarantine:
- Existing assets overwritten: yes / no
- Rollback tested: yes / no
- Rollback path:

Console result:

| Severity | Count | Notes |
| --- | ---: | --- |
| Errors |  |  |
| Warnings |  |  |
| Info |  |  |

## Preview Render Verification

- Preview scene or setup:
- Contact sheet path:
- Render path:
- Lighting setup:
- Camera distance:
- Materials visible: pass / fail
- Scale/readability: pass / fail
- First-person usability if applicable: pass / fail / not applicable
- Missing references: pass / fail

Visual notes:

- Silhouette:
- Brass/iron/leather/glass material read:
- Steam/glow/gauge readability:
- Damage/wear/readability:
- Deviation from north-star concept:

## Prefab Verification

| Prefab | Opens in prefab mode | Instantiates | Missing scripts | Missing refs | Pivot/scale ok | Decision |
| --- | --- | --- | --- | --- | --- | --- |
|  | pass / fail | pass / fail | yes / no | yes / no | yes / no | promote / defer / reject |

## Content-Specific Checks

Weapon/prop:

- Muzzle/grip/display pivots:
- Pickup scale:
- Gauge/coil readability:
- Collider recommendation:

Mechanical enemy:

- Silhouette role:
- Weak-point readability:
- Socket naming:
- LOD readiness:

Level module:

- Grid fit:
- Doorway/floor scale:
- Collision guidance:
- Route-risk notes:

Feedback UI/audio/VFX:

- Event readiness:
- Audio loudness notes:
- Icon readability:
- Accessibility risk:

## Decision

Decision: promote / defer / reject

Reason:

Required fixes:

Follow-up owner:

Next validation needed:

Next-step directive: continue immediately with the next highest-impact unfinished task.
