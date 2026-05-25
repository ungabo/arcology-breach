# Brassworks Breach - Release Notes v0.1.54

Generated: `2026-05-24 20:48 -04:00`

## Summary

`v0.1.54` is a playable Windows Set09 quarantine-import build. It brings Steam Corridor Dressing Set 09 and Clockwork Enemy Parts Set 09 into the main Unity package manifest as visual-only sidecar content, then places their corridor dressing, enemy previews, shared parts, and material swatches across the five-level campaign.

## Player-Facing Changes

- Level01-Level05 now include additional visual-only steampunk corridor dressing from Steam Corridor Dressing Set 09: gaslight sconces, pipe runs, gauge manifolds, steam vents, ceiling pipe clusters, floor grates, pressure-lock trim, and riveted doorway headers.
- Clockwork Enemy Parts Set 09 now appears as visual-only preview content in the campaign, including Skitter, Boiler Brute, Wall/Ceiling Sentry, shared gauge, shared gear, claw, and material swatch samples.
- The Set09 import is deliberately quarantined: imported package objects do not own route collision, AI, damage, lighting, objectives, pickups, or interactions.
- The manual playtest index and generated QA/candidate packets now point at the v0.1.54 executable and Windows package.

## Verified Build

- Executable: `Builds/Windows/v0.1.54/BrassworksBreach_v0.1.54.exe`
- Package: `Builds/WindowsPackages/v0.1.54/BrassworksBreach_v0.1.54_Windows.zip`
- Package SHA-256: `D15DE269B3754BD0E816526CA96E9F52C790A794001F773B48FE8342059EE89B`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.54.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.54.md`
- Issue triage: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.54.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.54.md`

## Verification

- `V0_LEVEL_VALIDATION_PASS`
- `SIDECAR_QUARANTINE_IMPORT_PASS packages=20 assets=177`
- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- Full player smoke suite through runtime, auto-playthrough, combat, interaction, hazard, secret, pause, movement, balance, level-flow, audio, display, readability, gameplay feedback, and world-label readability.
- `V0_WINDOWS_PACKAGE_PASS`
- `V0_ROUTE_AUDIT_PASS`
- `V0_WINDOWS_QA_PACKET_PASS`
- `V0_WINDOWS_ISSUE_TRIAGE_PASS`
- `V0_WINDOWS_CANDIDATE_PASS`
- `V0_BUILD_MATRIX_PASS v0.1.54`

## Notes

Room Material Set 10, Pressure Pistol Hero Set 10, Gaslight/Pipe Dressing Set 10, Door/Vault Set10, Pipe/Tank/Gauge Set10, Grime/Wetness Set10, and Corridor Assembly Lookdev Set10 are parallel side-lane outputs for future review. They are not promoted into this playable build.

Next-step directive: continue immediately with the next highest-impact unfinished task.
