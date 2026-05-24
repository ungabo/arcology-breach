# Brassworks Breach v0.1.35

Generated: `2026-05-24 14:20 -04:00`

## Release Focus

`v0.1.35` is a gameplay feedback systems leap. It adds a shared non-authoritative feedback layer so weapons, pickups, enemies, objectives, secrets, blocked routes, pause/settings, and boss beats can all trigger consistent player-facing feedback without moving gameplay authority into art or audio assets.

## Changes

- Added `GameplayFeedbackController` with v0.1.35 event taxonomy, counters, and stable hook names.
- Added `GameplayFeedbackPulseVfx` as a Unity-safe primitive fallback for world-space feedback pulses.
- Routed feedback events through weapon fire, impact, empty, and switch paths.
- Routed feedback events through pickup collection for health, ammo, gear key, and weapon pickup.
- Routed feedback events through Scrapper, Lancer, Bellows Node, Bulwark, and Governor Warden hit/death paths.
- Routed feedback events through objective completion, blocked route feedback, secret discovery, pause/resume, settings changes, and Warden phase changes.
- Added `RuntimeGameplayFeedbackTest` and included `-v0GameplayFeedbackSmoke` in the Windows matrix.
- Continued aggressive parallel production with Unity sidecar packages for weapons, mechanical enemies, Steamworks level kit, and integration gates.

## Artifacts

- Executable: `Builds/Windows/v0.1.35/BrassworksBreach_v0.1.35.exe`
- Package: `Builds/WindowsPackages/v0.1.35/BrassworksBreach_v0.1.35_Windows.zip`
- Package SHA-256: `DAD08132FA7F714DEFA73FFAE84AC337884C6D62E99EA48BC1CB210843F8E183`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.35.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.35.md`
- Issue triage: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.35.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.35.md`

## Verification

Full v0.1.35 smoke matrix passed with `V0_BUILD_MATRIX_PASS`; route audit, Windows package, QA packet, issue triage, and candidate readiness evidence were generated afterward.

Key markers:

- `V0_ROUTE_AUDIT_PASS`
- `V0_LEVEL_VALIDATION_PASS`
- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_GAMEPLAY_FEEDBACK_PASS`
- `V0_WINDOWS_PACKAGE_PASS`
- `V0_WINDOWS_QA_PACKET_PASS`
- `V0_WINDOWS_ISSUE_TRIAGE_PASS`
- `V0_WINDOWS_CANDIDATE_PASS`
- `V0_BUILD_MATRIX_PASS`

## Next

Continue with sidecar package-gate remediation, run the validator to zero blocking errors, then prepare the first quarantine imports for route-safe visual candidates.
