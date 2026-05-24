# Asset Legal And License Tracking Needs - v0.1.36 Planning

Generated: `2026-05-24`

## Purpose

This document defines the tracking needed before a Windows v1 package can be called release-ready. It does not validate any current asset license; it defines the evidence that must be gathered.

## Required Asset Manifest Fields

Track each shipped asset or asset family with:

- Asset ID.
- In-game path or package path.
- Category: generated art, staged pack, Unity package, audio placeholder, final art, final audio, font, code library, or documentation/media.
- Source.
- Author/vendor/tool.
- Creation or acquisition date.
- License name and version.
- Commercial use allowed: yes/no/unknown.
- Redistribution in game package allowed: yes/no/unknown.
- Modification allowed: yes/no/unknown.
- Attribution required: yes/no/unknown.
- Attribution text.
- Proof location: invoice, license file, store URL, model card, generation prompt record, or internal source note.
- Replacement status: final, placeholder, needs replacement, blocked.
- Reviewer and review date.

## Generated Art

Needs:

- Prompt/source records for any generated bitmap, concept sheet, texture, icon, capsule, or marketing asset that might ship.
- Tool/provider name and license terms at the time of generation.
- Confirmation that commercial use and redistribution are allowed.
- Human review that output does not imitate a protected living artist, franchise, trademark, or recognizable third-party work.
- Separation between concept-only generated art and shipped/generated runtime assets.

Release rule:

- Concept-only generated art may inform direction, but no generated asset should ship or appear in store media without an explicit manifest row and commercial-use review.

## Staged Asset Packs

Needs:

- Original marketplace/source URL.
- Purchase or license proof.
- Allowed platforms.
- Redistribution rules for compiled game packages.
- Modification rules.
- Attribution text.
- Third-party dependencies bundled inside the pack.
- Confirmation that editor-only/demo/sample content is excluded from shipping package unless intentionally used.

Release rule:

- Staged packs should be reduced to the used, licensed runtime subset before v1 packaging.

## Unity Packages

Needs:

- Package name, version, publisher, and source.
- Unity Asset Store, Package Manager, custom, or open-source origin.
- Runtime vs editor-only classification.
- License file location.
- Attribution requirements.
- Whether package code/assets are included in the final player build.

Release rule:

- Editor-only packages can remain in the project but should not be described as shipped runtime dependencies unless they are included in the player.

## Audio Placeholders

Needs:

- Distinguish temporary procedural/placeholders from final shipped audio.
- Track source, generation method, license, and replacement owner.
- Identify any sounds that resemble stock library placeholders and require replacement.
- Mark final mix approval status.

Release rule:

- No placeholder audio should ship unless intentionally accepted as final and licensed for commercial redistribution.

## Future Final Assets

Needs:

- Commission or internal creation record.
- Work-for-hire or assignment terms, if external.
- License scope across Windows, Steam, trailers, screenshots, social media, press kits, and future ports.
- Layered source file ownership.
- Attribution and moral-rights requirements where applicable.

Release rule:

- Final assets should arrive with legal metadata at import time, not during release week.

## Credits And Licenses Output

Before v1, generate or manually assemble:

- `CREDITS_AND_LICENSES.txt` for the shipped Windows package.
- Internal asset manifest with proof links.
- Store/media rights checklist for screenshots, trailer, key art, capsule art, and logo.
- Known exclusions list for assets that must not appear in package or marketing captures.

## Blocker Categories

- `BLOCKER_LICENSE_UNKNOWN`: source or license cannot be proven.
- `BLOCKER_COMMERCIAL_USE`: commercial use not allowed or unclear.
- `BLOCKER_REDISTRIBUTION`: redistribution in a game package not allowed or unclear.
- `BLOCKER_ATTRIBUTION`: required attribution is missing.
- `BLOCKER_PLACEHOLDER`: asset is temporary and not approved for release.
- `BLOCKER_STORE_MEDIA`: asset can ship in-game but cannot be used in store media.

## Review Cadence

- Update asset/legal tracking before each release candidate.
- Re-review any asset touched by visual replacement, audio replacement, package upgrade, or marketing capture.
- Treat legal review as a v1 gate, not a post-release cleanup task.
