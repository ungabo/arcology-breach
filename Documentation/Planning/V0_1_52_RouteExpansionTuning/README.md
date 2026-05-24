# v0.1.52 Route Expansion Tuning Packet

Scope: production tuning recommendations for the v0.1.51 route additions in Level02 pressure bypass, Level03 foundry gantry, and Level04 observatory pumpworks.

Use this as a bounded implementation packet for v0.1.52/v0.1.53 route polish. Recommendations are grouped into batched slices so each pass can adjust a route family, its labels, nearby hazards, encounter pressure, and validation coverage together.

## Top 10 Recommendations

### Level02 - Pressure Bypass

#### P0 - RTE-052-01: Make the bypass read as the primary pressure-route alternative

Batch the bypass entrance, first decision point, and return-to-mainline marker into one readability pass. The current route fantasy needs players to understand "pressure bypass" before they commit, not after they are already in a side corridor.

- Add a consistent route label at the entrance, mid-route branch, and rejoin point: `Pressure Bypass`, `Manual Bleed`, `Mainline Rejoin`.
- Use the same local prop language at each label: pressure wheel, pipe stencil, warning stripe, or bypass placard.
- Keep the first bypass view clean enough that the exit/rejoin direction is visible within 2-3 seconds of entering.
- Avoid placing the first high-attention pickup in the same sightline as the navigation marker.

#### P0 - RTE-052-02: Re-time pressure hazards around player learning beats

Batch all pressure vent timing and telegraph placement in the bypass route. The goal is one readable teach beat, one execution beat, and one combined beat.

- First active vent should have a forgiving cycle and a clear idle/safe state.
- Second hazard should add timing pressure without adding a new rule.
- Final hazard can combine vent timing with route pressure, but must preserve a visible safe pocket.
- Validation target: no blind damage while rounding a corner at normal movement speed.

#### P1 - RTE-052-03: Place one secret as a pressure-system mastery reward

Add a single bounded secret that rewards reading the bypass system rather than wall-hugging.

- Put the secret off the safe-pocket branch after the player has seen at least two pressure tells.
- Gate it with a low-risk timing choice, alternate pressure wheel, or optional crawl/maintenance branch.
- Reward can be pickup, lore, shortcut peek, or ammo cache, but it should not be required for route completion.
- Keep the secret return path short and clearly reconnect it to the bypass route.

### Level03 - Foundry Gantry

#### P0 - RTE-052-04: Reduce gantry ambiguity at height transitions

Batch route labels, camera/sightline cleanup, and collision checks for all gantry level changes. Players should know whether a ramp/stair/elevator is progression, optional loot, or a return path before committing.

- Label vertical transitions with functional names: `Upper Gantry`, `Foundry Floor`, `Crane Return`, `Control Walkway`.
- Put progression labels before the climb/drop, not only at the destination.
- Align interactables and pickups away from railings where they can be mistaken for route markers.
- Confirm jump/drop boundaries communicate survivable versus non-survivable falls.

#### P0 - RTE-052-05: Tune encounter density to avoid crossfire pileups on narrow gantries

Batch enemy placement, spawn timing, and cover props along gantry chokepoints. The gantry route can feel intense without turning every narrow span into a forced damage lane.

- Cap active threats on narrow one-player-width spans to one forward pressure source plus one flank/ambient source.
- Move heavy or ranged pressure to wider platforms where the player has lateral choice.
- Reserve the strongest encounter for the platform after the route identity is established.
- Add a post-encounter breath pocket before any precision traversal or hazard read.

#### P1 - RTE-052-06: Strengthen foundry hazard contrast without increasing clutter

Batch readability treatment for molten, heat, crusher, or moving-machinery hazards used near the gantry.

- Each hazard family should have one clear tell: motion, light color, sound cue, or repeated prop silhouette.
- Do not stack two new hazard tells in the same first-combat view.
- Keep hot/danger surfaces visually distinct from warm background machinery.
- Add at least one safe preview angle for every hazard that can damage the player from below or off-screen.

### Level04 - Observatory Pumpworks

#### P0 - RTE-052-07: Clarify pumpworks objective chain and route labels

Batch objective signage, interaction labeling, and route-return messaging across the pumpworks. The pump route should read as a connected mechanical chain rather than separate decorative rooms.

- Label sequence anchors: `Intake Control`, `Pump Primer`, `Pressure Return`, `Observatory Feed`.
- Ensure every pump interaction has a visible downstream result or route-open feedback.
- After each pump step, provide one clear reorientation cue toward the next route segment.
- Keep optional astronomy/observatory flavor readable as context, not as competing objective language.

#### P1 - RTE-052-08: Tune pumpworks pacing from puzzle read to movement release

Batch the first pump puzzle, route traversal, and next encounter as one pacing pass.

- Give the first pump interaction a low-pressure read window.
- Follow a successful pump action with a short movement release: bridge, drained path, opened gate, or exposed maintenance line.
- Place combat after the route change is understood, not directly on top of the first changed-state view.
- Use a brief safe overlook to let players see what the pump changed.

#### P1 - RTE-052-09: Add one observatory-side secret with clear backtracking logic

Add a single optional route reward that uses pump state or observatory sightlines.

- Secret should become legible after a pump state changes, not before.
- Signal it from a higher or wider observatory view, then route players through pumpworks space to reach it.
- Avoid hiding it behind identical pipe clutter; use a distinct maintenance hatch, pressure gauge, or telescope-aligned marker.
- Return should reconnect before the next mandatory encounter.

### Cross-Level

#### P0 - RTE-052-10: Add route-polish validation instrumentation and risk checks

Batch a QA compile pass across Level02, Level03, and Level04 before adding more route content.

- Confirm route labels use a shared naming pattern and do not conflict with objective text.
- Run scene smoke passes for navigation, collision, hazard telegraphs, and encounter density after each level batch.
- Capture before/after screenshots of each new route entrance, route midpoint, secret entrance, and rejoin.
- Check performance/readability risk where steam, molten effects, pump animation, labels, and enemies are visible together.

## Recommended Implementation Slices

1. **Route Signage and Rejoin Slice:** Apply labels, rejoin markers, and first-sightline cleanup across all three levels.
2. **Hazard Readability Slice:** Tune pressure vents, foundry hazards, and pump state feedback with one validation pass per level.
3. **Encounter and Pacing Slice:** Adjust Level03 density and Level04 post-pump pacing, then re-smoke Level02 for bypass interruption risk.
4. **Secrets Slice:** Add one bounded secret per level only after base route readability passes.
5. **Compile QA Slice:** Run route acceptance gates, screenshot comparisons, and performance/readability spot checks before v0.1.52/v0.1.53 lock.
