# Brassworks Breach - Windows Candidate Readiness v0.1.29

Generated: `2026-05-24 10:24 -04:00`

## Candidate Artifacts

- Executable: `Builds/Windows/v0.1.29/BrassworksBreach_v0.1.29.exe`
- Package: `Builds/WindowsPackages/v0.1.29/BrassworksBreach_v0.1.29_Windows.zip`
- Package SHA-256: `8F8C6DB781ACD5EFA9D477BCE93001DCE5DAAB313A3EB885526CCBA196778CFF`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.29.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.29.md`
- Issue triage packet: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.29.md`
- Release notes: `Documentation/Releases/RELEASE_NOTES_v0.1.29.md` (present)
- Package launcher: `Builds/WindowsPackages/v0.1.29/BrassworksBreach_v0.1.29_Windows/LAUNCH_BRASSWORKS_BREACH.bat`
- Package README: `Builds/WindowsPackages/v0.1.29/BrassworksBreach_v0.1.29_Windows/README_WINDOWS.txt`
- Package quickstart: `Builds/WindowsPackages/v0.1.29/BrassworksBreach_v0.1.29_Windows/QUICKSTART_WINDOWS.txt`
- Package support info: `Builds/WindowsPackages/v0.1.29/BrassworksBreach_v0.1.29_Windows/SUPPORT_INFO_WINDOWS.txt`
- Package release index: `Builds/WindowsPackages/v0.1.29/BrassworksBreach_v0.1.29_Windows/RELEASE_INDEX_WINDOWS.txt`
- Package checksum instructions: `Builds/WindowsPackages/v0.1.29/BrassworksBreach_v0.1.29_Windows/VERIFY_SHA256_WINDOWS.txt`
- Package SHA-256 sidecar: `Builds/WindowsPackages/v0.1.29/BrassworksBreach_v0.1.29_Windows.zip.sha256.txt`

## Automated Verification Markers

| Area | Marker | Log |
| --- | --- | --- |
| Scene rebuild | `V0 scenes rebuilt` | `v039-scene.log` |
| Level validation | `V0_LEVEL_VALIDATION_PASS` | `v039-level-validation.log` |
| Editor smoke | `V0_SMOKE_TEST_PASS` | `v039-smoke-test.log` |
| Windows build | `V0_WINDOWS_BUILD_PASS` | `v039-windows-build.log` |
| Runtime smoke | `V0_RUNTIME_SMOKE_PASS` | `v039-runtime-smoke.log` |
| Auto playthrough | `V0_AUTO_PLAYTHROUGH_PASS` | `v039-auto-playthrough.log` |
| Combat smoke | `V0_COMBAT_SMOKE_PASS` | `v039-combat-smoke.log` |
| Combat edge | `V0_COMBAT_EDGE_PASS` | `v039-combat-edge-smoke.log` |
| Combat scenario | `V0_COMBAT_SCENARIO_PASS` | `v039-combat-scenario-smoke.log` |
| Weapon switch | `V0_WEAPON_SWITCH_PASS` | `v039-weapon-switch-smoke.log` |
| Bellows Node | `V0_BELLOWS_NODE_PASS` | `v039-bellows-node-smoke.log` |
| Ranged combat | `V0_RANGED_COMBAT_PASS` | `v039-ranged-combat-smoke.log` |
| Bulwark combat | `V0_BULWARK_COMBAT_PASS` | `v039-bulwark-combat-smoke.log` |
| Warden combat | `V0_WARDEN_COMBAT_PASS` | `v039-warden-combat-smoke.log` |
| Interaction | `V0_INTERACTION_SMOKE_PASS` | `v039-interaction-smoke.log` |
| Hazards | `V0_HAZARD_PASS` | `v039-hazard-smoke.log` |
| Secrets | `V0_SECRET_PASS` | `v039-secret-smoke.log` |
| Pause flow | `V0_PAUSE_FLOW_PASS` | `v039-pause-flow.log` |
| Movement feel | `V0_MOVEMENT_FEEL_PASS` | `v039-movement-smoke.log` |
| Balance | `V0_BALANCE_ENVELOPE_PASS` | `v039-balance-smoke.log` |
| Level01 flow | `V0_LEVEL01_FLOW_PASS` | `v039-level01-flow-smoke.log` |
| Midgame flow | `V0_MIDGAME_FLOW_PASS` | `v039-midgame-flow-smoke.log` |
| Climax flow | `V0_CLIMAX_FLOW_PASS` | `v039-climax-flow-smoke.log` |
| Audio mix | `V0_AUDIO_MIX_PASS` | `v039-audio-mix-smoke.log` |
| Display settings | `V0_DISPLAY_SETTINGS_PASS` | `v039-display-settings-smoke.log` |
| Readability | `V0_READABILITY_SETTINGS_PASS` | `v039-readability-smoke.log` |

## Candidate Rules

- Ship only the ZIP package, not a loose executable alone.
- Keep the launcher, quickstart, README, support info, Data folder, UnityPlayer.dll, and MonoBleedingEdge folder together after extraction.
- Keep the SHA-256 hash with any shared package and use VERIFY_SHA256_WINDOWS.txt to compare it.
- Use RELEASE_INDEX_WINDOWS.txt as the package contents index before sharing a candidate.
- Use the QA packet as the manual route-test starting point.
- Treat this as a Windows candidate snapshot, not Android, WebGL, SteamVR, or Meta Quest readiness.
- Any manual blocker or confusion note should become a tracked task before a v1.0 release label.

## Status

Candidate readiness automation passed for this Windows build.

Next-step directive: continue immediately with the next highest-impact unfinished task.
