# Audio V1 Integration Plan

Implementation status: `v0.0.99` completed the first gameplay integration pass using Option A below. `SteamworksAudio` now prefers staged AudioV1 WAV ambience and cue clips for every `SteamworksAudioCue`, while retaining procedural fallback. Remaining work is human listen/mix tuning, import/compression tuning, and later per-emitter ambience/hazard placement.

## Import

1. Import WAVs from `Assets/_Project/ArtStaging/AudioV1/`.
2. Apply import settings from `AUDIO_V1_MANIFEST.md` or `AUDIO_V1_MANIFEST.json`.
3. Keep source WAVs unedited; make Unity compression/import changes through `.meta` or AudioImporter settings in the integration pass.
4. Keep ambience loops stereo/non-spatial unless a room-specific diegetic source is needed.
5. Keep combat and interaction one-shots mono so they can be spatialized when played at world positions.

## Routing

| Route | Clips | Suggested Mixer Bus |
| --- | --- | --- |
| Weapon one-shots | Pistol, burst, scattergun, slug, empty click | SFX/Weapons |
| Enemy tells and impacts | Scrapper, Lancer, Bulwark, machine hit/death | SFX/Enemies |
| Boss cues | Warden tell/enrage/shutdown | SFX/Boss |
| Pickups | Health, ammo, gear key, weapon pickup | SFX/Pickups |
| World interactions | Gate, lift, valve | SFX/World |
| Hazards | Steam, furnace, Bellows Node | SFX/Hazards |
| Ambience | Boiler, steam, machinery, knocks, preview mix | Ambience |
| Player feedback | Player hurt | SFX/Player |

## Code Hook Options

Option A: Keep `SteamworksAudioCue` as the stable cue contract and allow `SteamworksAudio` to prefer imported `AudioClip` references when present, falling back to procedural generation.

Option B: Add a separate audio library ScriptableObject that maps cue names to clips, then have `SteamworksAudio` resolve from the library before procedural fallback.

Option C: Stage per-prefab AudioSources for hazards, ambience emitters, and boss cues, while keeping global one-shots in `SteamworksAudio`.

Current path: Option A is implemented for `v0.0.99` because it is low-risk and keeps the existing cue contract stable. Recommended later path remains Option B for richer one-shot libraries plus per-emitter AudioSources for ambience and hazards.

## Scene And Prefab Placement Notes

- Ambience layers can be tested on a single scene-level 2D AudioSource stack before per-room mixing.
- Steam hazard loops should live on hazard emitters with range-limited 3D rolloff.
- Furnace warning loops should start during warning phase and stop or duck during safe phase.
- Furnace heat pulse should fire once at active phase transition or damage tick, not loop constantly.
- Gate open and lift activate should sync to existing VFX timing and mechanical motion.
- Valve turn can play during interact animation; valve vented should play on objective completion.
- Boss cues should originate from the Warden body or attack socket, not the player camera.

## Verification

- Confirm every mapped cue loads and plays in editor.
- Confirm existing smoke tests still pass after any future integration.
- Add or update runtime smoke checks only in the future code lane, not in this staging lane.
- Run manual listen pass with master volume at 0.55 and then at 1.0.
- Verify ambience and hazard loops continue cleanly for at least 3 minutes.
- Check repeated pickups and denied gates for fatigue.
- Check weapon/enemy tell masking during active combat.

## Platform Notes

- Windows: use source-quality imports first, then tune compression.
- Android/WebGL: reduce ambience layer count, lower Vorbis quality, and consider shorter loops.
- VR: keep localizable threats spatial but avoid over-wide 2D hazard loops that feel attached to the player head.
- Browser: prefer the preview ambience mix or fewer layers if download size becomes an issue.
