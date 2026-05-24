# Brassworks Breach - v0.1.29 Route Triage Plan

Created: `2026-05-24T09:49:11-04:00`

Owned scope: `Documentation/Planning/V0_1_29_RouteTriage/`

## Purpose

Plan the next route-safe slice after the `v0.1.28` pressure tank rack promotion is committed. This packet is intentionally docs-only and should help the main lane decide whether `v0.1.29` should fix an accepted route issue or use the no-issue fallback.

## Inputs Reviewed

- `Documentation/VERSION_MICRO_ROADMAP.md`
- `Documentation/BUILD_STATUS.md`
- `Documentation/WORK_LEDGER.md`
- `Documentation/IMPLEMENTATION_TODO.md`
- `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.28.md`
- `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.28.md`
- `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.28.md`
- `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.28.md`

## Current Evidence

- The deterministic `v0.1.28` route audit reports no route-blocking scene composition issues across Level01 through Level05.
- The `v0.1.28` route matrix still shows the full five-level path: Level01 gate/lift, Level02 routing valve/lift, Level03 pressure valve/lift, Level04 emergency lift, and Level05 Warden-locked final exit.
- Candidate readiness exists for `v0.1.28`, including executable, package ZIP, QA packet, issue-triage packet, and full smoke marker coverage.
- At sidecar creation, `CANDIDATE_READINESS_v0.1.28.md` still marked release notes as pending until docs refresh. The main lane later refreshed candidate readiness so the release notes are present before the `v0.1.28` commit.
- `IMPLEMENTATION_TODO.md` still has open manual route-related work: manual Windows playthrough, Scrapper/readability review, and audio listen pass.

## Recommendation

Primary `v0.1.29` goal: run a route-triage conversion pass before adding more route-near geometry. If real manual playtest notes exist, convert accepted P0/P1/P2 notes into tracked tasks and fix only the highest-impact route blocker/confusion item in a narrow slice.

Preferred fallback if no route issue is found: do Windows release-distribution hardening, specifically a human-friendly package release index plus SHA-256 verification instructions generated into the Windows package and reflected in candidate readiness. This is the safest useful fallback because it improves the distributable without changing scene geometry, enemy placement, collision, route distances, or combat balance immediately after the pressure tank rack insertion.

Reserve the next modular gameplay prop promotion for the following art slice after `v0.1.29`, unless a route issue directly calls for a small visual readability prop.

## Main-Lane Sequence

1. Confirm `v0.1.28` is committed and pushed with release notes refreshed.
2. Search manual QA sheets and issue logs for new/accepted notes from the `v0.1.28` package.
3. If P0 or P1 route/confusion notes exist, choose one narrow fix with the smallest route blast radius.
4. If only P2/P3 notes exist, record them as tasks and choose whether the smallest one fits `v0.1.29`.
5. If no real notes exist, implement the release-distribution fallback: package release index plus checksum verification instructions.
6. Run route audit plus the full V0 matrix for any code, scene, package-tooling, or generated-package changes.
7. Refresh candidate readiness and record whether `v0.1.29` was `route_issue_fix` or `no_issue_distribution_fallback`.

## Acceptance Gates

- `v0.1.28` precondition: clean committed baseline, release notes present, and package/candidate evidence generated.
- Triage gate: no accepted P0/P1 manual issue remains untracked after the `v0.1.29` decision.
- Route-fix gate, if used: route audit still reports no blockers, affected level validation passes, and the full V0 matrix passes.
- Fallback gate, if used: Windows package contains a release index and checksum verification instructions, package manifest/candidate readiness link them, and full V0 matrix passes.
- Route-safety gate: no new colliders or route-near geometry are introduced unless tied to an accepted issue and validated by route audit plus level validation.
- Documentation gate: `BUILD_STATUS.md`, `WORK_LEDGER.md`, `VERSION_MICRO_ROADMAP.md`, `IMPLEMENTATION_TODO.md`, release notes, and session log are updated by the main lane after implementation.

## Verification Commands

Use the actual Unity path if it changes.

```powershell
git status -sb
```

```powershell
rg -n "Issue ID:|Status: new|Status: accepted|Severity: P0|Severity: P1|blocked|confusing|unreadable" Documentation/QA/ManualPlaytestV1 Documentation/QA/WindowsRouteQA
```

```powershell
.\Tools\RunV0RouteAudit.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -UnityPath 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe'
```

```powershell
.\Tools\RunV0BuildMatrix.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -UnityPath 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe'
```

```powershell
.\Tools\GenerateWindowsCandidateReadiness.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -LogPrefix '<next-log-prefix>'
```

```powershell
Get-FileHash -Algorithm SHA256 'D:\__MY APPS\Unity Doom\Builds\WindowsPackages\v0.1.29\BrassworksBreach_v0.1.29_Windows.zip'
```

```powershell
Get-Content 'Documentation\Planning\V0_1_29_RouteTriage\ROUTE_TRIAGE_STATUS.json' | ConvertFrom-Json | Out-Null
```

## Risk Notes

- Automated route audit is necessary but not a substitute for human route, combat, hazard, audio, and readability feel checks.
- `v0.1.28` candidate readiness currently notes pending release notes; starting `v0.1.29` before closing that loop can leave the evidence chain ambiguous.
- Additional decorative props can accidentally weaken readability even without colliders, so the immediate post-pressure-tank step should prefer triage or distribution polish.
- Package hardening can drift from the actual ZIP contents unless the candidate-readiness generator checks the new files inside the package.
- If a manual P0 appears, it should override the fallback and become the only `v0.1.29` implementation target.

## Fallback Definition

If no accepted route issue is found, implement `v0.1.29` as:

`Windows package release index and SHA-256 verification instructions`

Expected output:

- `RELEASE_INDEX_WINDOWS.txt` or equivalent included in the versioned package folder and ZIP.
- `VERIFY_SHA256_WINDOWS.txt` or equivalent included in the package folder and ZIP.
- Package manifest and candidate-readiness evidence updated to require both files.
- Release notes describe that the slice changed distribution clarity only, not gameplay route content.
