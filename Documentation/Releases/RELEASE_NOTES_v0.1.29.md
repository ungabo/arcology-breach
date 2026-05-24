# Brassworks Breach - Release Notes v0.1.29

Generated: `2026-05-24 10:18 -04:00`

## Summary

`v0.1.29` is a Windows distribution-hardening slice. No accepted manual route-triage issues were present, so the v0.1.29 fallback path was used: make the packaged Windows candidate easier to inspect and verify before sharing.

## Changes

- Added `RELEASE_INDEX_WINDOWS.txt` to the Windows package folder and ZIP.
- Added `VERIFY_SHA256_WINDOWS.txt` to the Windows package folder and ZIP.
- Added package-manifest fields for the release index, checksum instructions, and generated SHA-256 sidecar.
- Expanded candidate-readiness automation so it requires those package files on disk and inside the ZIP.
- Refreshed route-audit next-action sequencing for the post-distribution-hardening path.
- Added the v0.1.30 asset-queue planning packet, selecting `ServiceLiftCallBoxPrototype` as the next route-safe Unity-owned prop slice.

## Verification

- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.29.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.29.md`
- Issue triage packet: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.29.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.29.md`
- Executable: `Builds/Windows/v0.1.29/BrassworksBreach_v0.1.29.exe`
- Package: `Builds/WindowsPackages/v0.1.29/BrassworksBreach_v0.1.29_Windows.zip`
- Package SHA-256: `8F8C6DB781ACD5EFA9D477BCE93001DCE5DAAB313A3EB885526CCBA196778CFF`

## Status

Full V0 matrix passed with `V0_BUILD_MATRIX_PASS`, including route audit, scene rebuild, level validation, Windows build, packaged runtime tests, Windows package, QA packet, issue-triage packet, and candidate-readiness evidence.

Next-step directive: continue immediately with the next highest-impact unfinished task.
