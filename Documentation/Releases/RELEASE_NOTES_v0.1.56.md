# Brassworks Breach - Release Notes v0.1.56

Generated: `2026-05-24 23:33 -04:00`

## Summary

`v0.1.56` is a playable Windows high-fidelity quarantine-import build. It brings the strongest accepted Set10/Set11 visual packages into the main Unity project as local package dependencies and places them across the five-level campaign as visual-only showcase dressing.

## Player-Facing Changes

- Level01-Level05 now include additional visual-only high-fidelity corridor dressing from Steam Corridor Dressing High Fidelity Set11, including gaslights, pipe runs, manifolds, valves, boiler pieces, handrails, and material samples.
- Mechanical Sentinel Hero Set10 now appears as visual-only modular enemy preview content in the campaign.
- Brassworks Door Mechanism Set10 now contributes selected visual-only pressure-door and mechanism showcase pieces.
- Steam Atmosphere VFX Set10 now contributes visual-only haze, steam, fog, and atmosphere samples.
- The import remains quarantined: sidecar packages do not own route collision, AI, damage, objectives, pickups, interactions, or gameplay lighting.
- The manual playtest index and generated QA/candidate packets now point at the v0.1.56 executable and Windows package.

## Verified Build

- Executable: `Builds/Windows/v0.1.56/BrassworksBreach_v0.1.56.exe`
- Package: `Builds/WindowsPackages/v0.1.56/BrassworksBreach_v0.1.56_Windows.zip`
- Package SHA-256: `0CAFFAEC3E709F435F8D88C0369E41D330F96A5C48A08C4E3CC57EFD80F147AD`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.56.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.56.md`
- Issue triage: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.56.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.56.md`

## Verification

- `V0_LEVEL_VALIDATION_PASS`
- `SIDECAR_QUARANTINE_IMPORT_PASS packages=24 assets=208`
- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- Full player smoke suite through runtime, auto-playthrough, combat, interaction, hazard, secret, pause, movement, balance, level-flow, audio, display, readability, gameplay feedback, and world-label readability.
- `V0_WINDOWS_PACKAGE_PASS`
- `V0_ROUTE_AUDIT_PASS`
- `V0_WINDOWS_QA_PACKET_PASS`
- `V0_WINDOWS_ISSUE_TRIAGE_PASS`
- `V0_WINDOWS_CANDIDATE_PASS`
- `V0_BUILD_MATRIX_PASS v0.1.56`

## Notes

This build is a visible density and presentation step, not a final-art claim. The imported high-fidelity sidecars are stronger than earlier swatch-level attempts, but still need deeper surface microdetail, stronger material calibration, and tighter lighting before final-art promotion.

New Set12 sidecar lanes for HUD ornamentation, corridor prop clusters, and industrial machinery completed after this build and are queued for v0.1.57 review.

Next-step directive: continue immediately with the next highest-impact unfinished task.
