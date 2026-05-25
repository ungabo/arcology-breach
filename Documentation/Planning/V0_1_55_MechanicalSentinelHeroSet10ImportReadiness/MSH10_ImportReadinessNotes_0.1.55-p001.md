# MSH10 Import Readiness Notes

## Status

`STATIC READY WITH LIMITATIONS`

## Safe Import Rules

- Import as a local Unity package only after visual review.
- Keep all prefabs visual-only until main-lane gameplay ownership is assigned.
- Do not add colliders, AI scripts, hitboxes, animation controllers, lights, audio, damage, route collision, or objective behavior from this package during quarantine import.
- Use `MSH10_MechanicalSentinelHeroAssembly` as the first hero visual shell candidate.
- Use named `SOCKET_` transforms for future rig/animation planning.

## Recommended Next Work

- Review the contact sheet and hero previews against the north-star mechanical monster.
- If accepted visually, integrate only as a quarantined showcase object first.
- Assign later rigging/animation work to a separate lane after the visual hierarchy is accepted.
