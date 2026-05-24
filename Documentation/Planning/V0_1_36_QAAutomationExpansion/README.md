# Brassworks Breach - v0.1.36 QA Automation Expansion Bundle

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_36_QAAutomationExpansion/`

## Purpose

This bundle proposes the next automated smoke expansion for gameplay/art milestone batches. It is docs-only planning for future main-lane implementation, with no script, scene, validator, package, build setting, or shared status changes.

The goal is to let larger milestone batches land with stronger automated evidence around gameplay feedback, staged asset imports, enemy readability, setpiece density, low/mid PC performance, and route authority.

## Packet Contents

- `SMOKE_EXPANSION_PLAN_v0.1.36.md` - proposed smoke lanes, data capture, pass markers, and escalation rules.
- `TEST_DATA_CONTRACTS_v0.1.36.md` - future log/event contracts and expected pass markers for implementation.
- `PRIORITIZATION_MATRIX_v0.1.36.md` - which tests should land in `v0.1.35`, `v0.1.36`, and later v1 stabilization.

Manual verification sheets live in:

- `Documentation/QA/V0_1_36_QAAutomationExpansion/MANUAL_VERIFICATION_SHEETS_v0.1.36.md`
- `Documentation/QA/V0_1_36_QAAutomationExpansion/QA_AUTOMATION_EXPANSION_PACKET_v0.1.36.md`

## Top Recommendations

1. Add route-authority drift checks first, because every ambitious content batch depends on the five-level route remaining trustworthy.
2. Add lightweight gameplay-feedback and enemy-readability markers before adding richer art/performance checks.
3. Treat staged asset import validation as a promotion gate: imported, visible, bounded, non-authoritative, performant, then eligible for authored gameplay use.
4. Keep manual sheets short and parallel-friendly so the user can keep developing while workers capture specific evidence.
5. Defer expensive v1 stabilization metrics until the smoke lanes have stable event names and consistent pass markers.

## Non-Goals

- No implementation of tests, validators, runners, scenes, or scripts.
- No new build matrix or release package definition.
- No changes to route authority.
- No edits to shared docs such as `WORK_LEDGER`, `BUILD_STATUS`, or `SESSION_LOG`.
