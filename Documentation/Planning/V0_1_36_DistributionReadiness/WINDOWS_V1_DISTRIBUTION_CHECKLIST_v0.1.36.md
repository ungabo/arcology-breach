# Windows v1 Distribution Checklist - v0.1.36 Planning

Generated: `2026-05-24`

## Distribution Target

Primary target: Windows ZIP package for `Brassworks Breach v1.0`.

Current readiness posture: prepare package policy and review gates only. Do not treat this document as evidence that a `v1.0` build exists.

## Canonical Package Contents

Required root contents for each Windows v1 candidate package:

- `BrassworksBreach_vX.Y.Z.exe`
- `BrassworksBreach_vX.Y.Z_Data/`
- `UnityPlayer.dll`
- `MonoBleedingEdge/`
- `LAUNCH_BRASSWORKS_BREACH.bat`
- `README_WINDOWS.txt`
- `QUICKSTART_WINDOWS.txt`
- `SUPPORT_INFO_WINDOWS.txt`
- `RELEASE_INDEX_WINDOWS.txt`
- `VERIFY_SHA256_WINDOWS.txt`
- `CREDITS_AND_LICENSES.txt`
- `ACCESSIBILITY_NOTES.txt`
- ZIP sidecar: `BrassworksBreach_vX.Y.Z_Windows.zip.sha256.txt`

Package rules:

- Ship the ZIP package, not the executable alone.
- Keep all Unity runtime folders beside the executable after extraction.
- Use a versioned package folder and file name such as `BrassworksBreach_v1.0.0_Windows.zip`.
- Include no editor-only folders, source-only documentation, raw asset production files, or test logs unless explicitly referenced by a release index.
- Keep support docs plain text in the shipped package so they are readable without web access.

## Install And Launch Flow

Expected user path:

1. Download `BrassworksBreach_v1.0.0_Windows.zip`.
2. Optional but recommended: compare the ZIP hash with the sidecar SHA-256 file.
3. Extract the ZIP to a writable folder outside the browser downloads preview.
4. Run `LAUNCH_BRASSWORKS_BREACH.bat` or `BrassworksBreach_v1.0.0.exe`.
5. If Windows SmartScreen appears, the support doc explains publisher/signing status and how to verify the package source.
6. First launch should reach the main playable flow without needing command-line flags, registry edits, extra runtimes, or Unity Editor installation.

Launch QA checks:

- Fresh extract launches from a short path such as `C:\Games\BrassworksBreach`.
- Fresh extract launches from a long path containing spaces.
- Launcher and direct executable paths both work.
- Missing-data-folder failure is understandable and points users back to extraction/package integrity.
- Antivirus or SmartScreen guidance is factual and does not pressure unsafe behavior.

## Support Docs

`README_WINDOWS.txt` should include:

- Game title, version, build date, and package type.
- One-paragraph description of the game.
- Controls.
- Objective summary.
- Minimum and recommended specs.
- Known limitations.
- Save/progression statement, if any.
- Support contact or issue-reporting link.
- Crash-report instructions.

`QUICKSTART_WINDOWS.txt` should include:

- Extract ZIP.
- Run launcher.
- Controls.
- Restart/pause instructions.
- Where to report problems.

`SUPPORT_INFO_WINDOWS.txt` should include:

- Exact package filename and version.
- Where logs are expected, if available.
- What to include in a bug report: OS version, CPU/GPU/RAM, display resolution, input device, steps, screenshot/video, and package hash.
- Known non-goals for v1: non-Windows platforms, mod support, VR, controller certification, cloud save, achievements, and storefront overlay integration unless added later.

## Specs

Minimum specs placeholder:

- OS: Windows 10 64-bit.
- CPU: modern 4-core desktop or laptop CPU.
- Memory: 8 GB RAM.
- GPU: DirectX 11 capable dedicated GPU or strong integrated GPU.
- Storage: final package size plus 2 GB free for extraction and logs.
- Input: keyboard and mouse.

Recommended specs placeholder:

- OS: Windows 11 64-bit.
- CPU: recent 6-core desktop or laptop CPU.
- Memory: 16 GB RAM.
- GPU: midrange DirectX 12 capable dedicated GPU.
- Storage: SSD with 4 GB free.
- Display: 1920x1080 or higher.
- Input: keyboard and mouse.

Spec validation before v1:

- Confirm minimum specs on real hardware or clearly label them as estimated.
- Capture average and worst-case FPS on minimum and recommended profiles.
- Confirm startup time, memory peak, and package size.
- Document graphics quality defaults and any display settings exposed to players.

## Accessibility Notes

Required package-facing notes:

- Keyboard and mouse controls list.
- Pause/restart behavior.
- Readability options currently supported.
- Audio/visual reliance notes for enemy tells, hazards, pickups, and objectives.
- Photosensitivity warning if flashing, high contrast, muzzle flashes, sparks, furnace heat, or rapid screen effects remain.
- No claim of full accessibility compliance unless tested against an explicit standard.

Pre-v1 accessibility checks:

- Essential gameplay information should not rely only on color.
- Enemy attack tells should have readable visual timing and, where possible, audio support.
- UI text should remain legible at common desktop resolutions.
- Pause menu should be reachable during normal play.
- Restart after death/win should be clear.
- Input remapping, controller support, subtitle support, and colorblind modes should be listed as either implemented, planned, or explicitly unsupported for v1.

## Screenshots And Trailer Needs

Screenshot capture list:

- First-person pressure-pistol combat with readable enemy silhouette.
- Steam Scattergun unlock or use moment.
- Brassworks corridor with clear route signage and lighting.
- Steam/furnace hazard encounter.
- Secret cache discovery.
- Boilerheart valve interaction.
- Governor Core finale or Warden confrontation.
- HUD readable during a representative combat beat.

Screenshot requirements:

- Capture at 1920x1080 minimum, with no debug overlays.
- Include a mix of combat, exploration, objective interaction, and environmental spectacle.
- Avoid screenshots that show placeholder art, broken lighting, editor gizmos, or unreadable UI.

Trailer needs:

- 30-60 second Windows v1 trailer.
- Show movement, shooting, alternate fire, enemy tells, hazards, objective progression, and finale stakes.
- Include only final or release-acceptable audio.
- Avoid unlicensed music, temporary SFX, debug UI, unfinished menus, and non-final platform claims.

## Credits And Licensing Placeholders

`CREDITS_AND_LICENSES.txt` should reserve sections for:

- Project title and copyright owner.
- Development credits.
- Unity engine acknowledgment.
- Unity package credits and licenses.
- Third-party art packages.
- Third-party audio packages.
- Fonts.
- Generated art/audio disclosure, if retained.
- Open-source code libraries, if any.
- Special thanks.

No asset should be treated as shippable until its license source, allowed use, attribution requirement, modification permission, redistribution permission, and commercial-use status are recorded.

## Release QA Gates

Automated gates:

- Scene rebuild passes.
- Level validation passes.
- Editor smoke passes.
- Windows build passes.
- Windows package generation passes.
- Runtime smoke passes from packaged executable.
- Auto-playthrough passes.
- Combat, hazard, interaction, weapon-switch, pause-flow, audio mix, readability, and display settings smoke tests pass.
- Candidate readiness packet exists with package hash and artifact list.

Manual gates:

- Clean-machine ZIP extraction and launch.
- Main route completed by a human tester.
- Death/restart and win/restart flows checked.
- All support docs reviewed for accuracy.
- Screenshots and trailer captures approved as release-facing.
- Credits/license placeholders resolved or explicitly blocked.
- Known issues triaged into ship blockers, acceptable limitations, and post-v1 tasks.

Hard blockers for v1:

- Package cannot launch from fresh extract.
- Main route cannot be completed.
- Missing or inaccurate license rights for shipped assets.
- Support docs describe features that are not present.
- Store/capture assets show placeholder content that is not intended for release.
- Version number, package name, executable name, and release notes disagree.
