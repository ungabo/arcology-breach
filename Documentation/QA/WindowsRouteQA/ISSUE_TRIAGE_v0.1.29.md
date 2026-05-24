# Brassworks Breach - Windows Issue Triage Packet v0.1.29

Generated: `2026-05-24 10:17 -04:00`

## Source Artifacts

- Executable: `Builds/Windows/v0.1.29/BrassworksBreach_v0.1.29.exe`
- Package: `Builds/WindowsPackages/v0.1.29/BrassworksBreach_v0.1.29_Windows.zip`
- Package SHA-256: `8F8C6DB781ACD5EFA9D477BCE93001DCE5DAAB313A3EB885526CCBA196778CFF`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.29.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.29.md`

## Route Matrix Snapshot

| Scene | Core Route Objects | Enemies | Pickups | Hazards | Secrets | Transition / Exit | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Level01 | Gate:yes Lift:yes | S:4 L:0 B:0 N:0 W:0 | H:2 A:2 K:1 W:0 | Steam:0 Furnace:0 | 1 | to Level02 open | Plaques:1 Spawn->gate 22.5m Spawn->lift 34.6m |
| Level02 | Valve:yes Lift:yes | S:1 L:1 B:0 N:0 W:0 | H:2 A:2 K:0 W:0 | Steam:0 Furnace:0 | 1 | to Level03 locked | Plaques:1 Spawn->valve 20.6m Spawn->lift 23.2m |
| Level03 | Valve:yes Lift:yes | S:2 L:0 B:0 N:1 W:0 | H:1 A:1 K:0 W:1 | Steam:2 Furnace:0 | 0 | to Level04 locked | Plaques:1 Spawn->valve 18.3m Spawn->lift 24.3m |
| Level04 | Emergency lift:yes | S:2 L:1 B:1 N:0 W:0 | H:2 A:2 K:0 W:0 | Steam:2 Furnace:2 | 1 | to Level05 open | Plaques:1 Spawn->lift 28.3m |
| Level05 | Warden:yes Guardian lock:yes Exit:yes | S:1 L:1 B:1 N:0 W:1 | H:1 A:1 K:0 W:0 | Steam:1 Furnace:1 | 0 | final exit locked | Plaques:1 Warden->exit 4.5m Spawn->Warden 24.1m |

## Severity Rules

| Severity | Meaning | Required Action |
| --- | --- | --- |
| P0 | A tester cannot finish the intended route, cannot quit/restart, or hits a crash/hard hang. | Fix before a candidate can be called release-ready. |
| P1 | A tester can continue, but route, combat, hazard, boss, or accessibility clarity is materially confusing. | Fix or explicitly defer before v1.0. |
| P2 | The issue weakens polish, readability, or feel but does not block the route. | Batch into the next polish slice. |
| P3 | Cosmetic, copy, tuning preference, or future-platform note. | Track only if it supports the Windows v1 goal or deferred platform plans. |

## Issue Buckets

| Bucket | Default Severity | Capture When | Evidence To Record |
| --- | --- | --- | --- |
| ISSUE-WIN-ROUTE | P0/P1 | First-run route blockage, unclear objective, missing lock/key/valve/lift affordance | Route sheet, level, room/object, time blocked, expected cue, observed cue |
| ISSUE-WIN-COMBAT | P1/P2 | Enemy tell, weapon feedback, ammo pressure, death feedback, damage fairness | Enemy/weapon, encounter, health/ammo before/after, perceived cause of failure |
| ISSUE-WIN-SECRET | P2/P3 | Secret clue, reward clarity, discovery feedback, secret-stat confusion | Secret name, entry clue, discovery route, reward expectation |
| ISSUE-WIN-HAZARD | P1/P2 | Steam/furnace readability, damage timing, safe-lane clarity | Hazard type, warning cue, damage timing, safe route |
| ISSUE-WIN-BOSS | P1/P2 | Warden lock, boss HUD, arena readability, final hoist clarity | Fight phase, HUD state, lock message, exit state |
| ISSUE-WIN-ACCESS | P1/P2 | Settings, high contrast, flash intensity, prompt readability, text overflow | Resolution, fullscreen state, contrast state, affected UI text |
| ISSUE-WIN-AUDIO | P2/P3 | Mix level, missing cue, confusing cue identity, ambience fatigue | Cue name or scene, volume setting, competing sounds |

## Intake Template

Use one block per issue copied from a route sheet or playtest note.

```text
Issue ID: ISSUE-WIN-____-###
Severity: P0/P1/P2/P3
Bucket: route/combat/secret/hazard/boss/access/audio/performance/art
Build: v0.1.29
Route Sheet:
Level / Room:
Repro Steps:
Expected:
Actual:
Tester Impact:
Evidence:
Suggested Fix Slice:
Status: new/accepted/deferred/fixed/rejected
```

## Triage Flow

1. Copy raw tester notes from the manual sheet into the intake template.
2. Assign severity using the rules above, not personal taste.
3. Assign one issue bucket so fixes can batch cleanly.
4. If the issue affects route completion, candidate packaging, controls, quit/restart, or crash behavior, keep it P0 until verified fixed.
5. Convert accepted issues into WORK_LEDGER.md tasks or a later GitHub issue once the cluster is stable.
6. Re-run the full matrix after a fix changes gameplay, scene generation, settings, or distribution artifacts.

## Candidate Gate

- P0 count must be `0` for a release-ready Windows candidate.
- P1 issues need either a fix, an explicit defer note, or a documented reason they do not block v1.0.
- P2/P3 issues may batch into polish, art, audio, or platform-port follow-up lanes.
- Any new issue that contradicts automated route evidence should trigger a route-audit or smoke-test update.

Next-step directive: continue immediately with the next highest-impact unfinished task.
