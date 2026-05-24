# Brassworks Breach - v0.1.36 Narrative Encounter Integration Notes

Created: `2026-05-24`

## Integration Stance

This bundle is a handoff package for later integration. It should feed `v0.1.36+` scene, asset, and validation work without blocking `v0.1.35`.

Route authority remains with existing scenes, builders, objectives, validators, and tests. Narrative/setpiece integration should attach presentation to already-authoritative events wherever possible.

Safe integration examples:

- Add a visual state response to an existing Gear Key pickup.
- Add a red-to-green lamp flip to an existing Pressure Gate unlock.
- Add a short prop label near an existing Valve Wheel.
- Add ambient worker marks on walls outside collision-critical routes.
- Add nonblocking machine motion above or beside an existing arena.

Unsafe integration examples:

- Add a second route trigger because a sign implies a new path.
- Add a collider to a lore prop in a validated movement lane.
- Move existing pickups, enemies, lifts, hazards, or valves to satisfy a narrative beat.
- Make a lore plaque required for progression.
- Add combat spawns inside lift, prompt, secret, or hazard ownership volumes.

## Respect Current Route Authority

Current route facts to preserve:

- Level01: Gear Key opens Pressure Gate, then Service Lift transitions to Level02.
- Level02: Routing Valve unlocks Boilerheart lift, then Service Lift transitions to Level03.
- Level03: Boilerheart pressure valve unlocks Foundry lift and shuts down linked steam hazards, then Service Lift transitions to Level04.
- Level04: Emergency Hoist transitions to Level05.
- Level05: Governor Warden defeat unlocks Master Override Hoist and win state.

Integration rules:

- Narrative signage must describe existing mechanics only.
- Setpiece scripting must be downstream of existing objective events.
- New ambient scenes should be visual-only until a route owner assigns authority.
- Use amber for optional attention and red/green only when they match the current route state.
- Avoid adding new objective nouns unless they map to an existing mechanic or a future explicitly owned system.

## Low/Mid Windows PC Limits

Primary performance target remains Windows at 1080p, 60 FPS target, mid/low gaming PC.

Budget guidance for narrative/setpiece content:

| Category | Recommendation |
| --- | --- |
| Signs and lore plates | Atlas decals and shared plaque meshes. Prefer 512/1024 text atlases over many unique materials. |
| Ambient props | Group by family and combine static meshes where placement is dense. |
| Animated machinery | Use a few meaningful spinners/pistons per room, not every decorative gear. |
| Lights | Baked/static where possible. Runtime lights should reinforce objective state only. |
| VFX | Short-lived, pooled, and low-overdraw. Steam should clarify, not fog combat. |
| Audio | Small one-shot cues and loop layers with distance limits. Avoid stacking many constant loops in one room. |
| Text readability | Large, short labels; no long paragraphs requiring high-res unique textures. |

High-risk rooms:

- Level03 Boilerheart chamber: already has steam hazards, Bellows Node, weapon pickup, and valve state.
- Level04 foundry floor: heat hazards plus Bulwark pressure can become visually noisy.
- Level05 Warden arena: boss HUD, projectiles, hazards, and final lock lamps must remain readable.

## Android / WebGL / VR Compatibility

The narrative layer should scale down cleanly.

Android and WebGL:

- Favor reusable prop families and atlas signage.
- Keep signage text short enough to be readable at reduced texture resolution.
- Avoid relying on dense small props to communicate critical objectives.
- Provide simple material fallbacks for emissive lamps, gauges, and heat strips.
- Keep optional ambient animation non-authoritative so it can be disabled under mobile/browser quality tiers.

VR:

- No forced camera turns, surprise close-up flashes, or lore UI that locks the player in place.
- Setpiece reveals should be spatial and readable from stable head orientation.
- Signs need physical scale large enough for comfortable reading without leaning into walls.
- Do not place interactive prompts too low, too high, or inside tight corners.
- Avoid steam bursts directly at the camera/face height.
- Keep stairs/ramps and passage widths compatible with current no-jump/no-crouch assumptions.

## Narrative Asset Placement Rules

- Put lore and signage on walls, benches, consoles, plinths, railings, and lintels.
- Keep floor clutter out of primary lanes unless it is flush and non-colliding.
- Preserve at least current route widths; do not reduce baffle, lift, hoist, or arena clearances.
- Do not hide pickups, prompt roots, boss silhouettes, hazard warning states, or projectile lanes.
- If a prop looks interactive but is not, either simplify it or label it as a noninteractive machine element.
- Use repeated visual grammar: worker chalk for secrets, municipal enamel signs for official route information, brass plaques for archives, red tags for hazards, green lamps for restored exits.

## Suggested Integration Order

1. Level01 objective signage and gate/key cause-effect, because it establishes the visual grammar cheaply.
2. Level02 Lancer lane and routing valve feedback, because it connects combat to infrastructure.
3. Level03 Scattergun/Bellows/Boilerheart linked-pressure reads, because this has the most mechanical storytelling value.
4. Level04 Bulwark cradle and foundry heat signage, because it gives the heavy enemy a story reason.
5. Level05 Warden lock lamps, core classification signage, and final override plate, because the finale needs the clearest state language.
6. Secret-language pass across existing secret spaces only after main route readability is stable.

## Targeted Validation Ideas

These are validation ideas for later owners. They do not require new tests in this docs-only bundle.

Objective readability:

- Start each level and confirm the first objective-facing sign or landmark is visible within the initial route cone.
- Trigger each existing objective event and screenshot before/after state color: red/amber/green must match route truth.
- Verify no optional lore text can be confused for a required objective.

Encounter readability:

- Level02: first Lancer bolt lane remains readable after pipe/signage dressing.
- Level03: Bellows pulse ring remains visible through steam/hazard effects.
- Level04: Bulwark silhouette remains readable against furnace glow.
- Level05: Warden boss bar, projectiles, lock lamps, and final hoist cue do not fight for the same screen space.

Route and collision:

- Run existing automated route playthrough after any scene placement.
- First-person manual smoke should brush all signage/setpiece zones to check snagging.
- Verify lift/hoist boarding footprints remain clear.
- Verify prompt radii for Gear Key, Valve Wheel, lore plaques, weapon pickup, and final hoist are not occluded.

Performance:

- Capture frame pacing in Level03, Level04, and Level05 after visual setpiece placement.
- Toggle quality tiers if available and confirm objective/lore essentials remain readable.
- Audit active lights and particle systems per room.

Platform readiness:

- Android/WebGL preview: check signage readability at lower texture sizes.
- VR preview: check no setpiece response forces head motion or emits face-height steam bursts.
- Controller/touch future pass: ensure objective and archive text can be displayed without tiny hover-only affordances.

## Acceptance Gates

Package gate:

- [x] Docs live only in the v0.1.36 NarrativeEncounterBundle planning/asset-production scopes.
- [x] No scripts, scenes, validators, build settings, packages, or shared status docs edited.
- [x] Beat sheet covers all five current levels.
- [x] Asset families are grouped for production rather than one-off prop requests.
- [x] Integration notes explicitly defer route authority to existing systems.

Narrative gate:

- [ ] Each level has a clear story question and a practical gameplay promise.
- [ ] Every proposed lore/signage beat is short enough for in-level use.
- [ ] Environmental storytelling supports play instead of interrupting combat.
- [ ] Enemy introductions explain mechanical function before combat escalation.
- [ ] Secret language reads as worker survival craft and remains optional.

Setpiece gate:

- [ ] Setpiece responses attach to existing authoritative objective events.
- [ ] No setpiece implies an unimplemented route, pickup, or interaction.
- [ ] Boss, hazard, enemy, pickup, and objective readability survive added dressing.
- [ ] Red/amber/green state language remains consistent across all five levels.

Performance and platform gate:

- [ ] Reusable signage and prop families use shared materials/atlases.
- [ ] Animated machinery is sparse, meaningful, and disable-friendly.
- [ ] Steam/VFX opacity and duration do not obscure combat on Windows or VR.
- [ ] Android/WebGL/VR reductions can remove optional ambience without breaking objectives.

Validation gate:

- [ ] Existing full route smoke remains green after any later integration.
- [ ] Objective state screenshots prove red-to-green transitions are legible.
- [ ] Encounter screenshots prove enemy silhouettes remain readable in dressed rooms.
- [ ] Manual first-person pass confirms no snagging or blocked prompts.
