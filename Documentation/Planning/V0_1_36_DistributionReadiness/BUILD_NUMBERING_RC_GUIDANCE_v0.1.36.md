# Build Numbering And Release-Candidate Guidance - v0.1.36 Planning

Generated: `2026-05-24`

## Current Path

The project is using rapid `v0.1.x` candidate leaps for production batches. Continue that cadence while features, assets, and distribution rules are still changing. Reserve `v1.0.0` for the first polished Windows release candidate that survives package, QA, support-doc, and legal review.

## Version Roles

`v0.1.x`:

- Active production and integration milestones.
- May include placeholder assets, incomplete store metadata, or deferred legal rows.
- Must be honest in release notes about readiness level.

`v0.9.x`:

- Optional stabilization lane if the project needs a visible pre-v1 freeze.
- Should focus on release blockers, performance, UX, package polish, and final asset replacement.

`v1.0.0-rc.N`:

- Release candidate naming for internally shared Windows v1 candidates.
- Should use the same package shape intended for final.
- No new features except fixes for release blockers.

`v1.0.0`:

- First public Windows v1 package.
- Requires final package docs, legal/credits review, QA gates, and store/capture approval.

## Recommended Numbering Rules

- Keep internal milestone docs in the existing `v0.1.x` format until the release freeze.
- Use package folders that match the executable and release notes exactly.
- Use `v1.0.0-rc.1`, `v1.0.0-rc.2`, and so on for release-candidate ZIPs.
- Promote an RC to `v1.0.0` only by rebuilding or repackaging with final naming and updated hashes.
- Never rename an old ZIP to a new version without regenerating checksum evidence.
- Do not reuse a version number after sharing a package externally.

## RC Entry Criteria

Before creating `v1.0.0-rc.1`:

- Feature scope is frozen.
- Final or release-approved art/audio is in place.
- Windows package contents match the canonical checklist.
- Support docs are present and accurate.
- Asset/legal manifest has no unresolved blockers.
- Screenshots and trailer can be captured from the candidate.
- Automated smoke matrix passes.
- Known issues are triaged.

## RC Exit Criteria

Before promoting to final `v1.0.0`:

- RC package hash is recorded.
- Clean-machine install/extract/launch passes.
- Main route manual QA passes.
- Accessibility notes are reviewed.
- Credits and license file is complete.
- Minimum and recommended specs are validated or clearly labeled.
- Store/media captures are approved.
- No ship blockers remain.

## Patch Version Guidance After v1

Use `v1.0.1`, `v1.0.2`, and so on for post-release Windows patches.

Patch releases should include:

- Fixed issues.
- Known issues.
- Package hash.
- Compatibility note for saves/settings, if applicable.
- QA scope, especially if the patch touches progression, combat, input, display, audio, or packaging.

## Branch/Artifact Naming Recommendation

Suggested artifact names:

- `BrassworksBreach_v1.0.0-rc.1_Windows.zip`
- `BrassworksBreach_v1.0.0_Windows.zip`
- `CANDIDATE_READINESS_v1.0.0-rc.1.md`
- `RELEASE_NOTES_v1.0.0.md`

Suggested status labels:

- `planning`
- `candidate-built`
- `automated-qa-pass`
- `manual-qa-pass`
- `legal-pass`
- `capture-approved`
- `release-approved`
- `shipped`
