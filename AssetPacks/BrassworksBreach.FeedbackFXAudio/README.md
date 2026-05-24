# Brassworks Breach Feedback FX Audio Sidecar

Version: `0.1.38-p001`

This package is a Unity-only sidecar asset factory for presentation feedback tied to the v0.1.35 `GameplayFeedbackController` taxonomy. It generates isolated prefab, material, mesh, metadata, preview, and short WAV placeholder cue candidates without referencing gameplay scripts from the main project.

## Event Coverage

- `WeaponFired`
- `WeaponImpact`
- `WeaponEmpty`
- `WeaponSwitched`
- `PickupCollected`
- `EnemyHit`
- `EnemyDeath`
- `ObjectiveUpdated`
- `ObjectiveCompleted`
- `RouteBlocked`
- `SecretFound`
- `PauseOpened`
- `PauseClosed`
- `SettingChanged`
- `BossPhaseChanged`

## Unity Menu Items

After importing this package into a throwaway or quarantine Unity project:

- `Brassworks Breach/Sidecars/Feedback FX Audio/Generate v0.1.38 Package`
- `Brassworks Breach/Sidecars/Feedback FX Audio/Render v0.1.38 Contact Sheets`
- `Brassworks Breach/Sidecars/Feedback FX Audio/Generate and Render v0.1.38`

Generated runtime assets are presentation-only. The prefabs include package-local identity metadata and named sockets, but they do not call or require any main-game scripts.

## Quarantine Import Notes

Use this package as a local UPM package first. Promote individual prefabs or audio cues into the main game only after confirming event names, mix levels, object scale, visual readability, and no GUID/path collisions.
