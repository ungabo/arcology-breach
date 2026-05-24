# Brassworks Breach - v0.1.36 QA Automation Prioritization Matrix

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_36_QAAutomationExpansion/`

## Purpose

Prioritize which QA automation expansions should be added in `v0.1.35`, `v0.1.36`, and later v1 stabilization.

## Priority Bands

- `P0`: protects route, build viability, or milestone promotion.
- `P1`: protects player clarity and batch quality.
- `P2`: protects scale, polish, or future release confidence.

## Version Placement Matrix

| Test | Priority | v0.1.35 | v0.1.36 | v1 Stabilization | Reason |
| --- | --- | --- | --- | --- | --- |
| Route authority owner drift | P0 | Add minimal digest check | Expand per route step | Preserve as release gate | Prevents feedback/art batches from stealing route authority. |
| Five-level route chain marker | P0 | Keep existing route audit as baseline | Add explicit `QA_ROUTE_AUTHORITY_PASS` marker | Keep in all candidate builds | Makes route health searchable and comparable. |
| Unauthorized collider/trigger scan for presentation roots | P0 | Add for feedback roots if cheap | Expand to staged imports and setpieces | Keep as import promotion gate | Prevents non-gameplay art from blocking or altering play. |
| Gameplay feedback state-delta snapshot | P1 | Add weapon/pickup smoke snapshot | Expand objective, secret, pause/resume | Add regression thresholds | Confirms feedback improved clarity without changing rules. |
| Pickup/objective/secret event distinction | P1 | Add manual/automation naming contract | Implement smoke count summary | Maintain as UX clarity gate | Stops mixed feedback language from confusing route state. |
| Enemy tell/hit/death marker order | P1 | Manual spot check only | Automate for five enemy classes | Add timing thresholds | Ensures readability survives bigger combat/art batches. |
| Warden boss-chain order | P0 | Keep final route manual check | Automate reveal-to-exit chain | Preserve as boss release gate | Level05 remains the highest-risk route sequence. |
| Staged asset import visibility/material/reference scan | P1 | Document contract | Implement for promoted imports | Expand to asset catalog history | Protects asset promotion quality. |
| Asset bounds vs route-critical volumes | P1 | Manual spot check | Automate overlap summaries | Add density trend reports | Prevents staged art from causing route clutter. |
| Setpiece density clearance sampling | P1 | Defer except manual notes | Add key choke-point samples | Expand to all density zones | Needed as level dressing accelerates. |
| Prompt/hazard/enemy occlusion checks | P1 | Manual sheets | Add targeted smoke markers | Add screenshot/computer-vision assist if useful | Automation catches obvious cases; humans still judge clarity. |
| Low PC frame sample | P1 | Defer unless perf regression appears | Add rough route sample | Stabilize with fixed hardware/profile | Avoids art batches outrunning target machines. |
| Mid PC frame sample | P2 | Defer | Add if LowPC lane is stable | Stabilize with trend reporting | Useful for optimization trend, less urgent than route safety. |
| Hitch cluster reporting | P2 | Defer | Add for transitions and Warden reveal | Expand route-wide | More valuable once sample windows are stable. |
| Manual sheet run-ID contract | P1 | Add immediately | Require with new smoke lanes | Keep for all release candidates | Lets parallel workers attach evidence without stopping development. |
| Full v1 release QA dashboard | P2 | Defer | Defer | Implement after markers stabilize | Dashboard before stable markers creates churn. |

## Recommended v0.1.35 Additions

The `v0.1.35` window should stay narrow and protect the current gameplay feedback push:

- Route authority owner drift digest.
- Searchable route pass marker.
- Unauthorized collider/trigger scan for feedback roots.
- Gameplay feedback state-delta snapshot for weapons and pickups.
- Manual sheet run-ID convention.

Exit criteria:

- Existing route audit still passes.
- New smoke output can be searched by marker.
- No feedback-only object introduces authority.

## Recommended v0.1.36 Additions

The `v0.1.36` window should broaden into art, density, readability, and performance:

- Staged asset import visibility/material/reference scan.
- Asset bounds against route-critical volumes.
- Enemy tell/hit/death marker order for all current enemy classes.
- Warden reveal-to-final-exit chain marker.
- Setpiece density clearance sampling at key choke points.
- Low PC route sample and hot spot output.

Exit criteria:

- All six proposed lane markers can be emitted by future main-lane automation.
- Manual sheets can link to automation run IDs.
- HOLD/FAIL output identifies the exact scene and route step.

## Recommended v1 Stabilization

The v1 stabilization phase should harden trend and release confidence:

- Preserve all `v0.1.36` markers as release-candidate gates.
- Add stable hardware/profile baselines for LowPC and MidPC.
- Add density trends by scene and route segment.
- Add timing thresholds for enemy tells, damage, deaths, and boss-chain events.
- Add release dashboard only after marker names and failure codes are stable.

## Do Not Prioritize Yet

- Screenshot vision-based readability scoring before event markers are stable.
- Full performance dashboard before target hardware profiles are locked.
- Automated subjective "fun" scoring.
- Broad scene mutation or auto-fix tooling.
- Any smoke that requires presentation objects to own gameplay authority.
