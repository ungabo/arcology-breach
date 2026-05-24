# Brassworks Breach v0.1.36 Distribution Readiness Plan

Generated: `2026-05-24`

## Purpose

This folder prepares the Windows-first distribution path for a later polished `v1.0` package. It is planning-only: no scripts, scenes, validators, build settings, packages, or shared status files were changed as part of this bundle.

## Bundle Files

- `WINDOWS_V1_DISTRIBUTION_CHECKLIST_v0.1.36.md` - Windows v1 package contents, install and launch flow, support docs, specs, accessibility, capture needs, credits/licensing placeholders, and release QA gates.
- `DEFERRED_STORE_READINESS_NOTES_v0.1.36.md` - Steam, Meta, Android, and WebGL readiness notes that keep Windows as the primary first-version target.
- `ASSET_LEGAL_LICENSE_TRACKING_v0.1.36.md` - tracking requirements for generated art, staged packs, Unity packages, audio placeholders, and final replacement assets.
- `BUILD_NUMBERING_RC_GUIDANCE_v0.1.36.md` - build-numbering and release-candidate rules for the `v0.1.x` to `v1.0` path.

## Windows-First Principle

`v1.0` should be treated as a Windows packaged game first, with store metadata and alternate platforms prepared only where they reduce later risk. Steam, Meta, Android, and WebGL work should remain deferred until the Windows candidate has stable packaging, support docs, legal manifests, capture assets, and repeatable QA evidence.

## Immediate Recommendations

- Promote one Windows package shape as canonical before changing any store-facing metadata.
- Require every candidate ZIP to include launcher, quickstart, README, support info, release index, checksum instructions, credits/license placeholders, and QA evidence references.
- Start asset/license traceability now, while generated and placeholder assets are still easy to identify.
- Do not assign `v1.0` until the final Windows candidate has passed automated smoke, manual route QA, install/extract checks, accessibility checks, and legal/package review.
