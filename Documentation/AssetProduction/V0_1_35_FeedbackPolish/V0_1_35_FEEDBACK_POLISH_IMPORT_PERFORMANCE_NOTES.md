# V0.1.35 Feedback Polish Import Notes

## Unity Asset Import
- PNGs: import as Sprite (2D and UI), alpha enabled, 256 px source art.
- WAVs: import as mono SFX, no loop, normalize disabled if project audio policy allows manual gain staging.
- Materials: Standard shader proxy materials only; replace with project UI/VFX shader variants during final integration.
- JSON files: treat as staging data/reference, not runtime gameplay config unless an integrator intentionally maps them.

## Integration Boundaries
- This bundle intentionally contains no scripts, scenes, prefabs, validators, build settings, or package changes.
- Main gameplay systems should map cue IDs to existing event hooks rather than adding new event names.
- Suggested event IDs: weapon_impact, enemy_hit, enemy_death, pickup_acquired, secret_discovery, objective_update, denied_interaction, oute_confirmation, pause_settings_readability, low_health, low_ammo.

## Performance Notes
- UI icons are small transparent PNGs; pack into the project HUD atlas during integration.
- WAV placeholders are short mono files; final audio should keep similar durations to avoid UI sluggishness.
- VFX recipes favor low particle counts and short lifetimes to keep combat readable.
- Throttle denied/low-health/low-ammo repeat cues so feedback does not become a constant loop.
