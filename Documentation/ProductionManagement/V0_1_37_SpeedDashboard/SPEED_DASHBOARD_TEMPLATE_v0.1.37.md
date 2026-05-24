# V0.1.37 Speed Dashboard Template

Purpose: every 15 minutes, force a production-speed decision instead of drifting into small isolated increments.

## Review Header

- Review timestamp:
- Reviewer:
- Current main-lane target:
- Current build target:
- Last successful compile/build:
- Last playable build:
- Last commit:
- Last push:

## Core Question

Is this the fastest safe development pace achievable with the resources currently available?

Decision: faster / same pace / slow down for integration safety

One-sentence reason:

## Active Workstream Snapshot

| Workstream | Owner / agent | Current batch | Status | ETA / next proof | Action |
| --- | --- | --- | --- | --- | --- |
| Main Unity lane |  |  | active / blocked / idle |  | continue / redirect / integrate |
| Weapon and prop sidecar |  |  | active / blocked / idle |  | continue / redirect / integrate |
| Mechanical enemy sidecar |  |  | active / blocked / idle |  | continue / redirect / integrate |
| Level module sidecar |  |  | active / blocked / idle |  | continue / redirect / integrate |
| Feedback UI/audio/VFX |  |  | active / blocked / idle |  | continue / redirect / integrate |
| QA / validation |  |  | active / blocked / idle |  | continue / redirect / integrate |
| Docs / release readiness |  |  | active / blocked / idle |  | continue / redirect / integrate |

## 15-Minute Speed Criteria

Active agents:

- Are enough independent lanes running for the available work?
- Is any agent idle while parallel-safe work exists?
- Is any agent blocked by unclear scope that can be resolved by a narrower package contract?

Blocked agents:

- Blocked owner:
- Block reason:
- Fastest safe unblock:
- Reassign / close / continue:

Batch size:

- Is the current batch large enough to produce a visible leap?
- Does the batch include too many coupled systems for safe import?
- Next bigger-safe batch candidate:

Compile cadence:

- Are we compiling too often for the size of change?
- Is there enough static/script validation to delay full Unity compile safely?
- Next compile trigger:

Asset sidecar throughput:

- Packages currently being produced:
- Packages ready for validation:
- Packages ready for quarantine:
- Packages rejected/deferred:
- Bottleneck:

Validation debt:

- Open unvalidated changes:
- Open QA reports:
- Known warnings accepted:
- Risks that must be tested before next build:

Integration readiness:

- Candidate package:
- Manifest present:
- Clean sidecar validation:
- Quarantine plan:
- Rollback path:
- Promotion decision:

## Acceleration Decision

Choose all that apply:

- Start another sidecar package lane.
- Increase main-lane batch size before the next compile.
- Merge multiple completed sidecar outputs into one quarantine import cycle.
- Convert stalled asset work into smaller component packs.
- Pause a lane that is generating low-quality or non-integrable output.
- Add a validation script/template to remove repeated manual checks.
- Run a targeted Unity compile now because integration risk is high.
- Defer full build until multiple gameplay-facing changes are in.

## Next 15-Minute Directive

Highest-impact next action:

Expected proof by next review:

Next-step directive: continue immediately with the next highest-impact unfinished task.
