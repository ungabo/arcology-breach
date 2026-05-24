# Deferred Store Readiness Notes - v0.1.36 Planning

Generated: `2026-05-24`

## Priority Rule

Windows v1 distribution is the first packaging target. Steam, Meta, Android, and WebGL readiness should be prepared as research and metadata scaffolding only until the Windows package is stable, tested, licensed, documented, and capture-ready.

## Steam Readiness - Deferred But Worth Preparing

Prepare later:

- Steam app name, short description, long description, tags, genre, capsule art, screenshots, trailer, system requirements, supported languages, controller support status, EULA/privacy links if needed, and content descriptors.
- SteamPipe depot layout for Windows package contents.
- Achievement, cloud save, overlay, and controller support decisions.
- Store page visual standards and capsule art source files.
- Release branch strategy: private testing branch, review branch, release candidate branch, and public branch.

Keep deferred:

- Steam SDK integration.
- Achievements.
- Cloud saves.
- Overlay-specific UX.
- Steam Deck verification claims.
- Store page submission.

Windows-first guardrail:

- Steam copy and media should be sourced from the same Windows v1 candidate that passed package QA. Do not capture store assets from debug/editor sessions.

## Meta Readiness - Deferred

Potential future target: Meta Quest or Meta storefront research only.

Major unknowns:

- Whether the game is intended for VR, flat-screen PC distribution, or both.
- Comfort requirements, locomotion rules, input mapping, performance budget, and certification path.
- Asset scale, UI readability, interaction distance, and motion sickness implications.

Keep deferred:

- VR implementation.
- Meta SDK integration.
- Quest performance optimization.
- Store submission.
- Controller/hand tracking requirements.

Windows-first guardrail:

- Do not adjust Windows v1 controls, camera, pacing, or performance targets to satisfy a hypothetical VR path before the Windows package is complete.

## Android Readiness - Deferred

Potential future target: Android build research only.

Major unknowns:

- Touch controls and UI layout.
- Performance and thermal limits.
- Asset memory budget.
- Build size limits.
- Input redesign for weapon switching, alternate fire, pause, and interaction.
- Google Play listing, signing, privacy, and device compatibility.

Keep deferred:

- Android build settings.
- Mobile UI implementation.
- Touch input layer.
- Google Play services.
- APK/AAB packaging.

Windows-first guardrail:

- Do not simplify Windows v1 visuals, input, or encounter design solely for mobile feasibility. Track mobile adaptations separately.

## WebGL Readiness - Deferred

Potential future target: browser demo, not full v1 packaging.

Major unknowns:

- Asset memory and download size.
- Browser input capture and pointer lock.
- Audio autoplay rules.
- Save/progress limitations.
- Performance across browsers.
- Compression, hosting, and CDN strategy.

Keep deferred:

- WebGL build settings.
- Browser-specific UI.
- Hosting setup.
- Analytics.
- Web storefront claims.

Windows-first guardrail:

- WebGL should be considered a later demo or marketing slice unless the full Windows route can be preserved without major compromise.

## Shared Store Metadata Backlog

Collect once Windows v1 is stable:

- Final product title and executable/package naming.
- One-sentence pitch.
- Short description under typical store limits.
- Long description.
- Feature bullets based on shipped content only.
- Content warnings.
- Minimum/recommended specs with measured evidence.
- Supported input devices.
- Supported languages.
- Accessibility notes.
- Press kit screenshots and trailer.
- Logo, capsule, icon, and key art source files.
- Credits and license file.
- Support URL and contact.
- Privacy policy/EULA decision.

## Decision Gates Before Any Store Work Becomes Active

- Windows package passes release QA.
- Asset/legal manifest has no unresolved commercial-use blockers.
- Store screenshots and trailer can be captured from a release candidate.
- Game title, versioning, and support identity are locked.
- Platform-specific implementation work has an owner and does not block Windows v1.
