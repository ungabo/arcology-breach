# Arcology Breach - Work Ledger

Status values: `backlog`, `ready`, `in-progress`, `blocked`, `review`, `verified`, `deferred`, `cut`.

## Active Milestone

Current target: `v0.2 Cyberpunk Combat Feel Slice`.

Primary goal: make the current Windows prototype feel good and point clearly at the cyberpunk identity before a large art pass.

## Current State

| ID | Task | Type | Priority | Status | Milestone | Verification |
| --- | --- | --- | --- | --- | --- | --- |
| CODE-001 | Build v0.0 FPS loop | code | P0 | verified | v0.0 | editor/build/runtime smoke |
| CODE-002 | Add v0.1 presentation feedback | code | P1 | verified | v0.1 | editor/build/runtime smoke |
| DOC-001 | Publish current project to GitHub | docs | P0 | verified | v0.1 | repo pushed |
| DOC-002 | Add production tracking docs | docs | P0 | verified | v0.1 | doc review |
| DOC-003 | Pivot identity to original cyberpunk game | docs | P0 | verified | v0.2 | editor/build/runtime smoke |
| DOC-004 | Add deferred Android, browser, and VR platform notes | docs | P1 | verified | v0.2 | doc review |
| DOC-005 | Add level map and transition planning | docs | P0 | verified | v0.2 | doc review |
| DOC-006 | Review locally cached Unity Asset Store packs | docs | P1 | verified | v0.2 | doc review |
| AUD-001 | Add first cyberpunk audio set | audio | P1 | verified | v0.2 | editor/build/runtime smoke |
| CODE-004 | Add Scrapper attack windup/readability | code | P1 | verified | v0.2 | editor/build/runtime smoke |
| CODE-005 | Improve access-shard/gate feedback | code | P1 | verified | v0.2 | editor/build/runtime smoke |
| TEST-002 | Add packaged objective auto-playthrough | test | P0 | verified | v0.2 | V0_AUTO_PLAYTHROUGH_PASS |
| CODE-004B | Improve Scrapper navigation and obstacle handling | code | P1 | verified | v0.2 | editor/build/runtime/autoplay smoke |
| TEST-003 | Add packaged combat smoke | test | P1 | verified | v0.2 | V0_COMBAT_SMOKE_PASS |

## Ready Next

| ID | Task | Type | Priority | Status | Milestone | Acceptance Criteria | Verification |
| --- | --- | --- | --- | --- | --- | --- | --- |
| TEST-001 | Manual Windows playthrough | test | P0 | ready | v0.2 | Complete start-to-exit, confirm death/restart, note tuning issues. | manual-playtest |
| CODE-003 | Tune movement/combat values | code | P0 | ready | v0.2 | Movement, enemy pressure, ammo, and health feel fair. | manual-playtest |
| LVL-002 | Confirm Aster Gate Intake scale and flow | level | P0 | ready | v0.2 | Gate is seen before shard, route loops cleanly, exit is readable, rooms fit scale rules. | manual-playtest |

## Backlog

| ID | Task | Type | Priority | Status | Milestone | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| ART-001 | Generate wet concrete and black chrome materials | art | P1 | backlog | v0.3 | See `AAA_ASSET_CATALOG.md`. |
| ART-002 | Generate access shard and lockdown gate visuals | art | P1 | backlog | v0.3 | Replace cube/gate placeholders. |
| ART-003 | Generate Scrapper enemy visual | art | P1 | backlog | v0.3 | Mechanical maintenance-frame melee enemy. |
| ART-004 | Generate Pulse Pistol visual | art | P1 | backlog | v0.3 | Replace blocky gun. |
| VFX-001 | Replace hit marker sphere with impact sparks | vfx | P1 | backlog | v0.3 | Wall and machine impact readability. |
| UI-001 | Replace text HUD with runner HUD | ui | P2 | backlog | v0.3 | Compact hacked-cyberdeck style. |
| LVL-001 | Rework Level01 into Aster Gate combat slice | level | P1 | backlog | v0.2 | Keep small, add better arena loops. |
| LVL-003 | Create LevelTransitionController | code | P2 | backlog | v0.4 | Load next scene from diegetic lift/data gate, preserve durable player state later. |
| LVL-004 | Draft top-down maps for first five campaign levels | level | P2 | backlog | v0.4 | Use `LEVEL_DESIGN_AND_MAPS.md` map ladder and template. |
| CODE-006 | Data-driven weapon definitions | code | P2 | backlog | v0.4 | Needed before more weapons. |
| CODE-007 | Data-driven enemy definitions | code | P2 | backlog | v0.4 | Needed before more enemy types. |
| TOOL-001 | Level validation checks | tool | P2 | backlog | v0.4 | Missing colliders, required objects, objective chain. |
| TEST-004 | Expand combat automation harness | test | P1 | backlog | v0.2 | Add player-damage, enemy attack windup, and ammo-empty scenarios. |
| PLAT-001 | Windows mid/low PC quality profile | platform | P1 | backlog | v0.4 | Quality settings and performance budgets. |
| PLAT-002 | Android port prototype plan | platform | P2 | deferred | post-Windows | Do not build until Windows game is complete. |
| PLAT-003 | Browser/WebGL port prototype plan | platform | P2 | deferred | post-Windows | Do not build until Windows game is complete. |
| PLAT-004 | SteamVR/OpenXR and Meta Quest port plan | platform | P2 | deferred | post-Windows | Keep architecture VR-compatible now; do not build VR yet. |

## Discovered Follow-Ups

| ID | Task | Source | Status | Notes |
| --- | --- | --- | --- | --- |
| DISC-001 | Decide when GitHub Issues should mirror this ledger | GitHub publish | backlog | Probably after v0.2 manual playtest. |
| DISC-002 | Decide whether local folder should remain `Unity Doom` or be renamed | rebrand | backlog | Repo and product name are becoming `Arcology Breach`; local path can stay for now. |
| DISC-003 | Inspect Unity account-only Asset Store packs in Package Manager | asset review | backlog | Local cache was reviewed; account-owned packs that are not downloaded need Editor/Package Manager access. |
| DISC-004 | Manual listen pass for procedural audio | audio | backlog | v0.0.2 smoke confirms cues exist, but a human needs to tune volume and tone in play. |
| DISC-005 | Manual readability pass for world labels | level | backlog | Verify in first-person that the TextMesh labels face the player and do not distract from combat. |

## Recently Verified

- `2026-05-22`: v0.1 editor smoke passed with `V0_SMOKE_TEST_PASS`.
- `2026-05-22`: v0.1 Windows build passed with `V0_WINDOWS_BUILD_PASS`.
- `2026-05-22`: v0.1 packaged runtime smoke passed with `V0_RUNTIME_SMOKE_PASS`.
- `2026-05-22`: Platform planning docs added for Windows mid/low PC, Android, browser/WebGL, and VR.
- `2026-05-22`: `v0.0.1` checkpoint build created at `Builds/Windows/v0.0.1/ArcologyBreach_v0.0.1.exe` and passed editor/build/runtime smoke.
- `2026-05-22`: Level scale/progression planning added in `LEVEL_DESIGN_AND_MAPS.md`.
- `2026-05-22`: Local Asset Store cache reviewed and candidate packs recorded in `ASSET_PACK_REVIEW.md`.
- `2026-05-22`: `v0.0.2` procedural audio checkpoint created at `Builds/Windows/v0.0.2/ArcologyBreach_v0.0.2.exe` and passed editor/build/runtime smoke.
- `2026-05-22`: `v0.0.3` combat/objective readability checkpoint created at `Builds/Windows/v0.0.3/ArcologyBreach_v0.0.3.exe` and passed editor/build/runtime smoke.
- `2026-05-22`: `v0.0.4` auto-playthrough/navigation checkpoint created at `Builds/Windows/v0.0.4/ArcologyBreach_v0.0.4.exe` and passed editor/build/runtime/auto-playthrough smoke.
- `2026-05-22`: `v0.0.5` combat automation checkpoint created at `Builds/Windows/v0.0.5/ArcologyBreach_v0.0.5.exe` and passed editor/build/runtime/auto-playthrough/combat smoke.
