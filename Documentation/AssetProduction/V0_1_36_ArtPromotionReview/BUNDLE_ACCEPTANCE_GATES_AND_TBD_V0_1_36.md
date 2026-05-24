# v0.1.36 Art Promotion Acceptance Gates And TBD List

This file defines the acceptance gates for promoting staged `v0.1.35` art bundles into the main lane. It is documentation-only and does not modify import settings, scenes, scripts, validators, packages, builds, or shared status files.

## Universal Promotion Gates

Every promoted bundle must pass these gates before it leaves staging:

- Import into Unity without compile errors, missing prefab dependencies, magenta materials, broken references, or required external DCC files.
- Preserve the existing route-authority contract: no new owner for pickup, objective, transition, route-state, boss-lock, secret, damage, spawn, save, or final-exit behavior.
- Use primitive or existing-authority collision only. Mesh colliders, trigger colliders, nav changes, and physics objects are rejected unless separately owned and validated by the main lane.
- Preserve amber/red/green semantics: amber means attention/objective/charged, red means locked/hostile/unsafe/empty, green means restored/usable/safe/exit-ready.
- Preserve enemy tell and pickup readability in first-person play, not just concept sheets.
- Meet the `v0.1.35` performance budget intent: shared materials, LOD path, capped runtime lights, non-shadowing tiny detail, low overdraw VFX, short audio bursts, and no broad unique-material sprawl.
- Include explicit rollback/defer ownership for any promoted family that fails first-person review.

## Recommended Bundle Gates

### Feedback Polish Cue Package

Promotion target: Milestone 1.

Acceptance gates:

- All eleven cue IDs remain present across manifest, UI recipe, VFX recipe, and audio index.
- Cue IDs map to existing event/state hooks; no new gameplay event authority is introduced by art data.
- Low-health, low-ammo, denied interaction, objective update, route confirmation, pickup acquired, and secret discovery are distinguishable by shape/timing/audio, not color alone.
- Reduced-motion policy is preserved for tremble, pulsing, and lamp-chase effects.
- Denied, low-health, and low-ammo cues have repeat-throttle settings or equivalent design notes before runtime use.

TBD before final art:

- Final mix pass for cue loudness and priority against weapon, enemy, route, pickup, and pause sounds.
- Final UI atlas packing and shader/material replacement.
- Human readability screenshots in normal and readability-oriented display modes.

### Pressure Cartridge, Wall Display, And Ammo Cabinet Visual Props

Promotion target: Milestone 1.

Acceptance gates:

- Pickup trigger ownership remains with existing pickup systems; cartridge visuals never become pickup authority.
- Wall display backplate and hooks do not overlap existing wall blockers, prompt positions, or pickup roots.
- Ammo cabinet remains a visual shell unless a separate interaction design owns refill/vending behavior.
- Green/amber/red state materials match the feedback polish language.
- Collision is primitive-only and disabled where visual dressing is close to player path.

TBD before final art:

- Unity proof renders of props in the actual armory/pickup contexts.
- Final material pass for glass, brass, labels, and emissive state tabs.
- Interaction design brief if ammo vending/refill behavior becomes a real feature.

### Pressure Pistol And Steam Scattergun

Promotion target: Milestone 2.

Acceptance gates:

- First-person camera crop proof exists for idle, fire, pickup, switch, and reload/settle states where supported.
- Pressure pistol gauge/lamp and scattergun triple-barrel/muzzle direction stay readable without covering reticle, prompts, objective text, boss HUD, enemy tells, or hazard warnings.
- Decorative coils, rivets, labels, needles, and soot cards have an LOD/reduction path.
- Collision uses boxes/capsules only and never decorative mesh detail.
- Material replacement in project shaders is proven before prefab promotion.
- Import statistics are recorded for triangle count, material slots, collider count, texture dimensions, and runtime lights.

TBD before final art:

- Hand/socket alignment proof for both viewmodel weapons.
- Weapon feedback intensity review with feedback polish cues active.
- Muzzle flash/recoil/impact pass that does not weaken combat readability.
- Final audio/VFX binding to existing weapon events.

### Scrapper, Lancer, And Bulwark Enemy Shells

Promotion target: Milestone 2 after weapons or alongside a focused combat proof.

Acceptance gates:

- Rig socket placeholders are preserved and documented for hips, spine cage, tool hands, coil/backpack, weak lamp, and shutdown burst.
- Cyan/blue attack tells and amber/red weak lamps remain visible in Level02/Level04 representative combat backgrounds.
- Visual shells do not create hitbox, hurtbox, weak-point, nav, attack timing, spawn, or damage authority.
- LOD0/LOD1/LOD2 budgets are documented before broad use, with tiny rivets/coils/shutdown fragments reduced first.
- Shutdown fragments and hit/death visuals are distinct from windup, stagger, and temporary hit flash.

TBD before final art:

- Rigging pass with separated torso, arms, legs, tools, lamps, coils, and shutdown fragments.
- Combat readability screenshots for Scrapper, Lancer, and Bulwark in their intended level contexts.
- Material/shadow/light budget proof with multiple enemies on screen.
- Animation timing review for tell visibility after final mesh replacement.

### Warden And Foundry Overseer Elite

Promotion target: Milestone 3 or later.

Acceptance gates:

- Warden reveal, boss HUD, guardian lock, Warden defeat, final-hoist unlock, and final-exit language remain one understandable chain.
- Command lamps, center lamps, crown coils, and bolt tells do not compete with green restored/final-exit state.
- Heavy/boss triangle, material, light, shadow, and particle budgets are recorded in a representative Level05 flow.
- Elite art remains visual-only until boss/miniboss behavior ownership is separately validated.

TBD before final art:

- Boss-flow proof renders from player approach, combat midrange, defeat, final-hoist unlock, and final-exit sightlines.
- Rigging/animation plan for command profile, pincer/gavel/saw/hammer/lance tools, coils, and shutdown.
- Final material balance so lamps and coils support HUD/state readability.
- Performance proof with finale environment dressing also present.

### Level Module Setpieces

Promotion target: Milestone 3 targeted placements only.

Acceptance gates:

- First promotion is selective, not broad dressing: Level03 Bellows/Scattergun and Level04 Furnace/Bulwark are the preferred proving grounds.
- Route clearances are proven for doors, lifts, hoists, transitions, pickups, prompts, hazards, enemy spawns, boss lanes, secrets, and final exit before placement expands.
- Door apertures and player-path widths preserve existing route standards; flush floor trim remains non-colliding or below snag height.
- Runtime proof lights are reviewed and either removed, baked, capped, or explicitly budgeted.
- LOD1/LOD2 or prefab-combine/static-batching strategy exists before repeated room-wide placement.
- Decorative route colors do not imply extra interactables, fake safe lanes, fake locked routes, or new objectives.

TBD before final art:

- First-person Unity proof renders inside actual Level03, Level04, and Level05 routes.
- Collision audit for catwalk rails, door frames, pipe galleries, furnace alcoves, trim, and gaslights.
- Material consolidation and draw-call reduction pass.
- Light budget pass separating proof lights from runtime lights.
- Interaction-design review for valve/objective-looking dressing that is not meant to be interactable.

## Staging-Only TBD List

Keep these assets out of main-lane promotion until the listed proof exists:

| Asset/group | Hold reason | Required proof |
| --- | --- | --- |
| Future Alt Lightning Lance Silhouette | Future exploration only; no v0.1.36 gameplay/readability target. | Weapon role brief, first-person silhouette proof, event/audio/VFX plan, collision/LOD plan. |
| Warden visual shell | High coupling to boss HUD, guardian lock, final-hoist, and final exit. | Full Level05 flow proof plus performance evidence. |
| Foundry Overseer Elite | Miniboss/elite behavior role not proven. | Encounter design, rigging plan, weak-point/tell policy, perf proof. |
| Broad level-module dressing group | Route and performance risk too high for blanket promotion. | Targeted placement proof, route audit, LOD/batching/light plan. |
| Furnace glow-heavy placements | Can wash out enemy silhouettes and hazard language. | Combat readability proof with enemies, hazards, and feedback active. |
| Valve/pipe/gauge-heavy dressing near objectives | May imply extra interactables. | Interaction-design review and player-facing affordance check. |
| Ammo cabinet interaction | Visual prop exists; vending/refill behavior is not owned. | Interaction design, authority owner, trigger/prompt validation, route smoke. |

## Batch Exit Criteria

### Milestone 1 Exit

- Feedback cues and small pickup/armory props are integrated as presentation-only assets.
- Existing route audit and targeted pickup/objective/secret/pause/readability smokes remain green.
- No cue or prop owns unauthorized collider, trigger, route, pickup, objective, secret, save, boss, damage, or transition authority.

### Milestone 2 Exit

- Pistol/scattergun first-person proof passes and does not weaken combat/objective readability.
- Scrapper/Lancer/Bulwark visual shell proof passes representative combat readability.
- Import stats are known for promoted weapons and enemies.

### Milestone 3 Exit

- Targeted Level03/Level04/Level05 setpiece placements pass first-person route clearance and performance review.
- Warden/finale art supports the final gameplay chain without competing state language.
- Any broad environment expansion is explicitly deferred until targeted placements prove stable.
