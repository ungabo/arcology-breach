# Brassworks Breach - Release Notes v0.1.20

Generated: `2026-05-24 07:26 -04:00`

## Summary

`v0.1.20` is a modular environment-component promotion slice. No real manual route-triage notes were present, so the roadmap fallback was used: promote the next reusable Unity-owned steampunk route component into playable scenes.

## Changes

- Added `RivetedPressureDoorFramePrototype` runtime metadata.
- Added route-safe riveted pressure/vault door frame placements in Pipeworks Annex and Boilerheart Core.
- Built the component from Unity-generated geometry: blackened iron columns/lintel, aged brass ribs, pressure cylinders, animated central gear hub, cross braces, amber warning lamps, cream pressure gauge, and twenty visible rivets.
- Added validator coverage for promotion version, placement roles, named hierarchy, material roles, detail counts, and non-blocking collider footprint.
- Added production brief/status docs under `Documentation/AssetProduction/RivetedPressureDoorFramePrototype/`.
- Refreshed route-audit next-action sequencing for the post-door-frame path.

## Verification

- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.20.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.20.md`
- Issue triage packet: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.20.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.20.md`
- Executable: `Builds/Windows/v0.1.20/BrassworksBreach_v0.1.20.exe`
- Package: `Builds/WindowsPackages/v0.1.20/BrassworksBreach_v0.1.20_Windows.zip`
- Package SHA-256: `6D176B5F201FFA9A76D745E350E4700C8CBC5D40D6E3C53A75D4FAE548694498`

## Status

Full V0 matrix passed with `V0_BUILD_MATRIX_PASS`, including level validation, Windows build, packaged runtime tests, Windows package, QA packet, issue-triage packet, and candidate-readiness evidence.

Next-step directive: continue immediately with the next highest-impact unfinished task.
