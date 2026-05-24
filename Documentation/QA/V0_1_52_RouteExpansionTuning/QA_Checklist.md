# v0.1.52 Route Expansion Tuning QA Checklist

Use this checklist for any future v0.1.52/v0.1.53 route polish compile that touches Level02 pressure bypass, Level03 foundry gantry, or Level04 observatory pumpworks.

## Compile Entry Gate

- [ ] Project opens without script compile errors.
- [ ] Level02, Level03, and Level04 route-polish scenes load from the intended build index or test harness.
- [ ] No unrelated gameplay scenes, core scripts, or shared status docs are modified as part of the route-polish compile.
- [ ] v0.1.51 route additions remain present: Level02 pressure bypass, Level03 foundry gantry, Level04 observatory pumpworks.
- [ ] Route labels and objective labels use consistent naming and do not duplicate each other in the same view.

## Level02 - Pressure Bypass Acceptance Gates

- [ ] Bypass entrance label is readable before entering the route.
- [ ] First pressure hazard has a visible safe state and does not damage the player before its tell is visible.
- [ ] The route has at least one clear midpoint orientation cue.
- [ ] Rejoin point clearly communicates return to mainline.
- [ ] Optional secret, if added, is reachable through a readable pressure-system choice and returns to the route without dead-ending.
- [ ] No vent, pipe, pickup, or label blocks player movement or camera read on the critical path.
- [ ] Smoke pass completes from route entry to rejoin without blind damage, soft lock, or required backtracking confusion.

## Level03 - Foundry Gantry Acceptance Gates

- [ ] Every height transition communicates destination or function before commitment.
- [ ] Narrow gantry spans do not stack multiple high-threat sources without cover or lateral movement.
- [ ] Survivable drops and lethal drops are visually distinct.
- [ ] Foundry hazards have clear tells and safe preview angles.
- [ ] Pickups, interactables, and route signs are not competing in the same railing-edge sightline.
- [ ] A post-encounter breath pocket exists before the next precision traversal or hazard timing section.
- [ ] Smoke pass completes from gantry entrance to route rejoin without collision snagging, off-screen hazard damage, or encounter pileup.

## Level04 - Observatory Pumpworks Acceptance Gates

- [ ] Pump objective chain reads in order: intake/control, pump/primer, pressure/return, observatory/feed.
- [ ] Each pump interaction produces visible or audible feedback tied to the changed route state.
- [ ] Player gets a reorientation cue after each pump-state change.
- [ ] First puzzle read window is not interrupted by immediate combat or unclear hazard damage.
- [ ] Optional secret, if added, is signaled after a pump state change and reconnects before the next mandatory encounter.
- [ ] Observatory flavor props do not look like active pump objectives unless they are interactable.
- [ ] Smoke pass completes from first pump objective through observatory route rejoin without unclear objective state.

## Cross-Level Route Polish Gates

- [ ] Route entrance, midpoint, secret entrance, and rejoin screenshots are captured for all touched route segments.
- [ ] Route labels remain legible in combat, low-light, particle-heavy, and elevated views.
- [ ] Hazard color/tell language is distinct per level and consistent within each level.
- [ ] Secret placement rewards route mastery and does not obscure mandatory progression.
- [ ] Encounter density supports route pacing: teach, execute, breathe, combine.
- [ ] No new one-way drop, door, lift, or pump state creates a soft lock.
- [ ] Performance check covers the heaviest combined view in each level: labels, particles, animated machinery, enemies, and route geometry visible together.
- [ ] Readability check covers the busiest combined view in each level at normal player camera height.

## Compile Exit Gate

- [ ] All P0 recommendations in the JSON packet are either implemented or explicitly deferred in the active production tracker.
- [ ] All touched levels complete a start-to-rejoin route smoke pass.
- [ ] All optional secrets are verified as optional: route completion does not require collecting or discovering them.
- [ ] Screenshot evidence exists for before/after route readability on each touched level.
- [ ] Build/run notes identify any remaining performance or readability risk by level and route segment.
