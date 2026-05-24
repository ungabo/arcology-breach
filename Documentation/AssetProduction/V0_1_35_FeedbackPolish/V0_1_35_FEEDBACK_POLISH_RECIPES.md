# V0.1.35 Feedback Polish Material, Sprite, Sound Recipes

## Brass UI Plates
- Base: aged brass with darker grime in seams and a subtle polished rim.
- Function: frames event class and separates UI from world noise.
- Unity notes: use one shared material variant for plates; reserve emissive only for lamps, not plate fill.

## Enamel Lamps
- Amber: impact, pending, objective, low-ammo early warning.
- Green: acquired, route confirmed, enemy death/clear state, secret positive reveal.
- Red: denied, low-health, empty, danger.
- Readability: every lamp state must also have icon shape, motion, or text support.

## Steam Puffs
- Use short puffs as punctuation, not atmosphere.
- Impact/pickup puffs should stay under 0.35s; death/secret puffs can last 0.6-0.8s.
- Keep center-screen alpha low and never obscure reticle or objective text.

## Pressure Sparks
- Amber sparks: weapon impact and enemy hit.
- Red sparks: denied interaction only, throttled.
- Keep bursts small: 2-6 particles, with larger events using longer audio/UI instead of more particles.

## Gauge Needle Motion
- Hit: tiny 3-5 degree nudge, settle in 0.10s.
- Objective: 20-30 degree sweep, settle in 0.22s.
- Low health: tremble, reduced-motion fallback is static cracked gauge.
- Low ammo: downward dip at threshold and final shot rim pulse.

## Audio Cue Intent
- Procedural WAVs in the staging folder are timing/mix placeholders, not final foley.
- Impacts use short brass ping/metal ticks under weapon fire.
- Death uses valve drop, clonk, steam release, protected from hit spam.
- Pickup and route cues are positive but brief.
- Denied cue is sharp and throttled so it reads without irritating repetition.
- Pause/settings uses only restrained focus ticks and no constant hiss.
