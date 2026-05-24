# Brassworks Breach v0.1.36 Distribution Readiness Bundle

Generated: `2026-05-24`

## Scope

This release-readiness bundle prepares the future Windows v1 distribution path. It does not implement packaging changes, edit build settings, alter scenes, touch scripts, or update shared status files.

## Produced Planning Artifacts

- `Documentation/Planning/V0_1_36_DistributionReadiness/README.md`
- `Documentation/Planning/V0_1_36_DistributionReadiness/WINDOWS_V1_DISTRIBUTION_CHECKLIST_v0.1.36.md`
- `Documentation/Planning/V0_1_36_DistributionReadiness/DEFERRED_STORE_READINESS_NOTES_v0.1.36.md`
- `Documentation/Planning/V0_1_36_DistributionReadiness/ASSET_LEGAL_LICENSE_TRACKING_v0.1.36.md`
- `Documentation/Planning/V0_1_36_DistributionReadiness/BUILD_NUMBERING_RC_GUIDANCE_v0.1.36.md`

## Windows v1 Readiness Summary

The Windows v1 package should become a versioned ZIP with launcher, executable, Unity runtime folders, package index, checksum sidecar, quickstart, support info, accessibility notes, and credits/license text. A release candidate should not be promoted to `v1.0.0` until automated QA, manual route QA, clean extraction/launch, support-doc review, asset/legal review, capture approval, and known-issue triage are complete.

## Deferred Platform Summary

Steam, Meta, Android, and WebGL remain deferred. Their metadata and risk notes can be prepared, but platform SDK work, store submission, mobile controls, VR work, WebGL hosting, achievements, cloud saves, and certification-specific implementation should not displace Windows v1 readiness.

## Legal And Asset Summary

The bundle calls for a manifest-level record of generated art, staged asset packs, Unity packages, audio placeholders, fonts, code libraries, final assets, and store media rights. Any asset with unknown commercial-use, redistribution, attribution, placeholder, or store-media rights should block v1 until resolved or replaced.

## Numbering Summary

Continue `v0.1.x` for active production leaps. Use `v1.0.0-rc.N` only after feature scope, final assets, package shape, support docs, legal review, automated QA, and manual QA are ready for release-candidate treatment. Promote to `v1.0.0` only with final package naming and regenerated hash evidence.

## Top Recommendations

- Make the Windows ZIP package structure canonical before investing in store-specific implementation.
- Start asset/license tracking immediately so generated and placeholder content does not become release-week archaeology.
- Capture screenshots and trailer footage only from a release candidate or a build explicitly approved for marketing.
- Keep non-Windows platforms in documented deferral until Windows v1 has a stable package and QA path.
- Treat `v1.0.0` as a release approval event, not just the next build number.
