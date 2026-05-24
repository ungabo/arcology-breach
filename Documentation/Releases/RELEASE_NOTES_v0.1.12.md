# Brassworks Breach v0.1.12 Release Notes

Status: verified Windows distribution polish slice  
Target build: `Builds/Windows/v0.1.12/BrassworksBreach_v0.1.12.exe`  
Package target: `Builds/WindowsPackages/v0.1.12/BrassworksBreach_v0.1.12_Windows.zip`
SHA-256: `869ABA999F99FD7226792AAF2550A64E77181BE20A1C16EB1F3BCD52BE2D90B6`

## Purpose

`v0.1.12` is a packaging and distribution-hardening slice. It does not change the game route or combat design. Its job is to make every verified Windows build easier to hand to a tester without guessing which files are needed.

## Expected Player-Facing Contents

- `BrassworksBreach_v0.1.12.exe`
- Unity runtime folders/files required beside the executable.
- `README_WINDOWS.txt` with launch instructions, controls, platform target, and verification markers.
- ZIP package plus SHA-256 hash and package manifest.

## Verification Target

- `V0_ROUTE_AUDIT_PASS`
- Full V0 build matrix.
- `V0_WINDOWS_PACKAGE_PASS`

Verified on `2026-05-24 04:57 -04:00`.

## Notes

- Windows remains the primary playable target.
- Android, WebGL/browser, SteamVR, and Meta Quest remain planned deferred ports.
- The current visual state is still verified prototype art unless an asset document explicitly says it has final-art approval.
