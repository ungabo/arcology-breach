# v0.1.36 Art Promotion Candidate Scorecard

Scores use `1` to `5`, where `5` is strongest or safest. Risk columns are scored as safety: `5` means low risk, `1` means high risk.

## Scorecard

| Candidate family | Visual fidelity | Unity import safety | Route-authority safety | Performance safety | Gameplay readability impact | Integration effort | Decision |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| Feedback polish cue package | 4 | 5 | 5 | 5 | 5 | 4 | Promote first as presentation-only recipes/assets. |
| Pressure cartridge + ammo pickup support | 4 | 4 | 4 | 5 | 5 | 4 | Promote first with existing pickup authority only. |
| Wall weapon display + ammo cabinet visual props | 4 | 4 | 4 | 4 | 4 | 4 | Promote first as non-authoritative visual props. |
| Pressure pistol component pack | 5 | 4 | 4 | 3 | 5 | 3 | Promote after first-person crop/material/LOD proof. |
| Steam scattergun component pack | 5 | 4 | 4 | 3 | 5 | 3 | Promote after muzzle/readability/performance proof. |
| Scrapper/Lancer/Bulwark enemy shells | 5 | 4 | 4 | 3 | 5 | 3 | Promote after rig socket, LOD, and combat readability proof. |
| Warden + Foundry Overseer elite shells | 5 | 4 | 3 | 2 | 4 | 2 | Hold for boss-flow and finale readability proof. |
| Level module setpieces targeted placements | 5 | 4 | 2 | 2 | 4 | 2 | Hold for route clearance, light, collision, and LOD proof. |
| Level module setpieces broad dressing | 5 | 3 | 1 | 1 | 3 | 1 | Staging-only until selective placements are proven. |
| Future lightning lance silhouette | 3 | 4 | 5 | 4 | 2 | 2 | Staging-only future exploration. |

## Family Notes

### Feedback Polish Cue Package

Promotion class: `ready-first`.

Strengths:

- Contains UI sprites, VFX recipes, short audio placeholders, cue matrix, import notes, and accessibility gates.
- Explicitly avoids scripts, scenes, prefabs, validators, build settings, and package changes.
- Strong positive impact on pickups, objectives, secrets, low-health, low-ammo, denied interaction, and route confirmation.

Primary risks:

- Cue IDs must attach to existing event/state owners only.
- Repeated denied/low-health/low-ammo cues can become noisy if throttling is skipped.
- Amber/green/red language must stay consistent across route, UI, VFX, and audio.

Promotion call:

- Promote first, but only as presentation assets and mapping guidance. Do not promote JSON files as runtime authority unless the main lane intentionally owns that conversion.

### Pressure Cartridge And Pickup Support

Promotion class: `ready-first`.

Strengths:

- Strong resource readability: standard, high-pressure, empty, and ruptured cartridge states are explicitly gated.
- Small import/performance footprint relative to weapons, enemies, and level modules.
- Useful for pickup, cabinet, and HUD icon language.

Primary risks:

- Pickup trigger sizing must remain larger than the visible mesh and owned by existing pickup systems.
- Red high-pressure state must not be confused with danger/locked route language.

Promotion call:

- Promote with feedback polish so pickup acquisition becomes visually and audibly legible early.

### Wall Display And Ammo Cabinet Visual Props

Promotion class: `ready-first`.

Strengths:

- Good armory identity and capability framing.
- Primitive collision guidance is already documented.
- State color mapping is explicit: green stocked/usable, amber low/charged, red empty/danger.

Primary risks:

- Ammo cabinet can imply vending interaction before interaction design is owned.
- Wall display hooks and backplate can accidentally block wall collision or pickup affordances.

Promotion call:

- Promote as visual shells only. Any interaction, vending, refill, or unlock logic remains staging-only until designed.

### Pressure Pistol Component Pack

Promotion class: `conditional-next`.

Strengths:

- Highest-confidence hero weapon visual candidate.
- Component pack includes LOD/collision guidance and material recipe context.
- Gauge/lamp readability supports first-person identity.

Primary risks:

- First-person crop may hide gauge/lamp or clutter the reticle.
- Decorative coils/rivets can overshoot hero weapon budgets if not reduced.
- Material replacement needs proof in project shaders to avoid mismatch or magenta failures.

Promotion call:

- Promote in Milestone 2 after a first-person proof sheet and import statistics.

### Steam Scattergun Component Pack

Promotion class: `conditional-next`.

Strengths:

- Strong class differentiation from pistol through triple barrel, pump, stock, soot, and larger silhouette.
- Useful for Level03 scattergun capability beat and wall display framing.

Primary risks:

- Heavy muzzle/flash/recoil treatment could hide crosshair, prompt, hazard, or enemy tells.
- Larger viewmodel has more first-person clearance risk.
- Soot pass must not hide muzzle direction.

Promotion call:

- Promote after pressure pistol or alongside it only if first-person crop and muzzle-direction reads are proven.

### Scrapper, Lancer, And Bulwark Enemy Shells

Promotion class: `conditional-next`.

Strengths:

- Clear family silhouettes: low fast saw-claw, tall lance/coil, wide shield wall.
- Emissive tell language is already separated: cyan/blue attack tells and amber/red weak lamps.
- Sockets, shutdown fragments, tool children, and LOD guidance are documented.

Primary risks:

- Visual shells can imply new weak-point, hitbox, or attack timing authority.
- Rigging and animation socket proof is not yet promotion-grade evidence.
- Multiple enemies on screen need triangle/material/shadow validation.

Promotion call:

- Promote common enemy art after rig socket proof and representative Level02/Level04 combat readability review.

### Warden And Foundry Overseer Elite

Promotion class: `hold`.

Strengths:

- High final-boss/finale value and strong elite silhouette language.
- Command lamps, crown coils, multi-tool body language, and shutdown fragments can significantly improve the final sequence.

Primary risks:

- Highest gameplay-readability coupling: Warden reveal, boss HUD, guardian lock, Warden defeat, final-hoist unlock, and final exit must remain coherent.
- Heavy/boss budget is more expensive and less forgiving.
- Elite weak lamps and coils could conflict with route/state colors if used too early.

Promotion call:

- Keep staging-only until the main lane has boss-flow proof and final-scene performance evidence.

### Level Module Setpieces

Promotion class: `hold-targeted`.

Strengths:

- Biggest environment identity leap: corridor bay, pressure door, vault door, pipe gallery, furnace alcove, catwalk rail, trim, gaslight, and dressing group.
- Unity-rendered proof sheets and material/LOD/collider guidance already exist.
- Strong level-specific recommendations are already documented for Level03, Level04, and Level05.

Primary risks:

- Largest route-authority risk: doors, lifts, hoists, transitions, pickups, enemy spawns, boss lanes, secrets, and final exit can be obstructed by geometry or dressing.
- Runtime lights, rivets, clamps, trims, and broad placement can exceed low/mid Windows budgets.
- Decorative amber/green/red lamps can compete with gameplay state language.

Promotion call:

- Do not promote as broad dressing. Start with a targeted placement proof in Level03 and Level04, then Level05 after route and performance evidence.

### Future Lightning Lance Silhouette

Promotion class: `staging-only`.

Strengths:

- Provides visual exploration and weapon family contrast.
- Low immediate risk if it remains reference-only.

Primary risks:

- No gameplay role, interaction design, first-person proof, or final integration target exists for v0.1.36.

Promotion call:

- Keep as future reference. Do not include in the next main-lane art promotion.
