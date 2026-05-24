# Audio V1 QA Listen Checklist

Use this checklist for manual audition before any future code or scene integration. Test first in a DAW or audio player, then in Unity once the clips are imported.

## Setup

- Confirm all WAVs are present under `Assets/_Project/ArtStaging/AudioV1/`.
- Confirm `AUDIO_V1_MANIFEST.json` and `AUDIO_V1_MANIFEST.md` match the generated files.
- Listen at a fixed reference volume, then again 6 dB lower.
- Compare against the procedural cue intent in `SteamworksAudio.cs` without editing that file.

## Global Checks

- No clipped, crunchy, or hard-limited transients beyond intentional placeholder grit.
- No leading silence that delays gameplay feedback.
- One-shot tails end cleanly and do not mask the next likely combat cue.
- Mono cues collapse correctly and do not feel phasey.
- Loudness hierarchy is readable: weapons and boss cues above interactions, pickups below weapons, ambience below all.

## Ambience Loops

- `AUDV1_AMB_BoilerRumble_loop.wav`: no obvious click at loop seam; low rumble does not feel like constant damage feedback.
- `AUDV1_AMB_SteamBed_loop.wav`: steam is soft enough for combat and HUD readability.
- `AUDV1_AMB_DistantMachinery_loop.wav`: motion feels industrial, not musical or comedic.
- `AUDV1_AMB_PipeKnocks_loop.wav`: knocks are sparse enough to avoid rhythm fatigue.
- `AUDV1_AMB_BrassworksMix_loop.wav`: preview mix should work as a temporary single ambience clip, but should not replace layer mixing for final.

## Combat Cues

- Pistol fire is short, sharp, and smaller than scattergun.
- Pressure Burst is wider than pistol primary and distinct from scattergun.
- Scattergun blast has clear low weight and immediate brass/steam identity.
- Scattergun slug reads narrower and more precise than scattergun primary.
- Empty click is audible but not punitive or startling.
- Machine hit reads as metal damage, not pickup or UI feedback.
- Machine shutdown has enough tail to sell defeat but does not block follow-up shots.

## Enemy And Boss Tells

- Scrapper tell communicates melee danger before damage.
- Lancer tell has a rising pressure character that is easy to locate spatially.
- Bulwark tell feels heavier and slower than Scrapper.
- Warden stomp tell is larger than Bulwark without becoming a full damage impact.
- Warden pressure-bolt tell is distinct from Lancer.
- Warden enrage and shutdown are boss-scale but leave room for music or ambience later.

## Pickups And Interactions

- Health pickup is gentler than ammo and gear key.
- Ammo pickup is quick and mechanical.
- Gear key pickup sounds objective-significant.
- Weapon pickup feels like an unlock, not a generic ammo tick.
- Gate denied is clear but not fatiguing under repeated failed interaction.
- Gate open and lift activate feel related but not identical.
- Valve turn and valve vented form a believable interaction pair.

## Hazards

- Steam hazard loop is localizable and does not mask enemy tells.
- Furnace warning loop communicates danger before active damage.
- Furnace heat pulse can be used on phase transition without sounding like a weapon.
- Bellows Node pulse reads as support-machine pressure rather than player weapon fire.

## Integration Smoke Listen

- In Level01 or a test scene, audition combat cues with current master volume at 0.55.
- Trigger one pickup immediately after a weapon shot and confirm both read.
- Trigger enemy tell plus player movement and confirm the tell cuts through ambience.
- Loop ambience for at least 3 minutes and note fatigue, seam, or masking problems.
- Test hazard loops at several distances if imported with spatial settings.

## Signoff Notes

Record issues with:

- File name.
- Timestamp or repeat condition.
- Problem type: seam, clipping, masking, loudness, duration, identity, spatialization.
- Suggested action: trim, retune, replace, split into layers, lower mixer volume, or regenerate.
