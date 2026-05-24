# Brassworks Breach v0.1.37 Release Notes

Build date: `2026-05-24`

## Headline

`v0.1.37` improves first-person navigation readability by giving world labels billboard behavior, dark readability backplates, and high-contrast response.

## Player-Facing Changes

- Floating route, pickup, cache, and showcase labels now face the player camera during gameplay.
- World labels now sit on dark backing plates so brass/amber text stays legible against busy steampunk rooms.
- High-contrast mode now also affects world labels, increasing label size and switching text to white.
- Label backplates are presentation-only and do not add route-blocking colliders, rigidbodies, or gameplay authority.

## Production Changes

- Added `WorldLabelBillboard` for camera-facing world labels and high-contrast styling.
- Added `RuntimeWorldLabelReadabilityTest` and wired `-v0WorldLabelReadabilitySmoke` into the packaged Windows matrix.
- Extended editor validation to require world-label readability metadata, backplates, centered text, and no gameplay physics.
- Updated candidate-readiness checks to require `V0_WORLD_LABEL_READABILITY_PASS`.
- Kept completed sidecar package outputs isolated for separate package-review commits.

## Verification

- `V0_ROUTE_AUDIT_PASS`
- `V0_LEVEL_VALIDATION_PASS`
- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_WORLD_LABEL_READABILITY_PASS`
- `V0_BUILD_MATRIX_PASS v0.1.37`
- `V0_WINDOWS_PACKAGE_PASS`
- `V0_WINDOWS_QA_PACKET_PASS`
- `V0_WINDOWS_ISSUE_TRIAGE_PASS`
- `V0_WINDOWS_CANDIDATE_PASS`

Package SHA-256:

`B7FA153CA7129BA59AF8D306E1A294F5159945477AF40A65A03A5501B8FB9ADF`

Next-step directive: continue immediately with the next highest-impact unfinished task.
