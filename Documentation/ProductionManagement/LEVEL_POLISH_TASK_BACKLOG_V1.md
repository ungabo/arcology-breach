# Brassworks Breach - V1 Level Polish Task Backlog

Scope: production-ready planning backlog for future implementation agents.

This file groups V1 level polish work into independent slices with lane ownership, file scopes, and validation expectations. It is intentionally practical: each slice should be implementable without guessing which systems or scenes it is allowed to touch.

Collaboration guardrail: this backlog is documentation only. It does not touch Unity scenes, scripts, status docs, work ledgers, asset staging, concept renders, or active Bulwark / pressure-pistol proof work.

## Lane Definitions

Parallel lane:
- Safe for future side agents if the slice stays inside the listed documentation, design, or asset-production scope.
- Does not edit generated scenes, gameplay scripts, level validator, scene builder, balance, or runtime tests.
- Can prepare briefs, blockouts, prop manifests, signage text, concept checklists, and validation plans.

Main integration lane:
- Must be owned by the agent integrating the playable build.
- Can touch generated scenes, `V0SceneBuilder`, `V0LevelValidator`, runtime smoke tests, balance files, and scene-specific gameplay objects.
- Requires validation in the same slice.

Blocked or coordinate-first lane:
- Must wait for active owners if it overlaps current work.
- At this snapshot, Level04/Bulwark-heavy work must coordinate with Anscombe's v0.0.93 Bulwark lane. Pressure-pistol proof render files must stay out of scope because Darwin owns that lane.

## Common Validation Baseline

For documentation-only or asset-brief slices:
- Markdown renders cleanly.
- File scope stays inside the listed docs or asset-production folders.
- No Unity scene, script, status, work ledger, README, or render-output file changes.

For main integration slices:
- Run the project scene rebuild path appropriate to the current workflow.
- Run `V0LevelValidator`.
- Run `Tools/RunV0BuildMatrix.ps1` after the slice is complete.
- Include targeted runtime smoke where relevant:
  - Route/lock changes: `RuntimeAutoPlaythroughTest`.
  - Secrets: `RuntimeSecretTest` plus auto-playthrough secret totals.
  - Hazards: `RuntimeHazardTest`.
  - Bellows Node: `RuntimeBellowsNodeTest`.
  - Bulwark: `RuntimeBulwarkCombatTest`.
  - Warden/finale: `RuntimeWardenCombatTest`.
  - Interactions/lore plaques: `RuntimeInteractionTest`.
  - Ranged combat: `RuntimeRangedCombatTest`.

## Parallel-Ready Slices

### P-01 Cross-Level Signage And Decal Text Sheet

Lane: Parallel.

Goal: define reusable route, lock, hazard, secret, and lore-adjacent signage before scene integration.

File scope:
- `Documentation/ProductionManagement/` new or existing planning docs.
- `Documentation/AssetProduction/` signage/decal briefs if an asset-production agent owns the output.

Implementation notes:
- Build a shared library of exact text strings for Intake, Pipeworks, Boilerheart, Foundry, and Governor.
- Separate readable objective labels from flavor signage.
- Preserve color language: amber for objectives, red-orange for danger/locked states, green for exits/success.
- Include small worker chalk phrases for secrets that do not look like main objective markers.

Validation:
- Every proposed sign maps to a level, room, and purpose.
- No sign text exceeds what can fit on a small enamel plate.
- Future integration can validate named sign props without inventing copy.

Parallelization:
- Can run alongside gameplay work.
- Must not edit scenes or `V0SceneBuilder`.

### P-02 Secret Cache Visual Language Brief

Lane: Parallel.

Goal: make current and future secrets feel fair without turning them into mandatory route labels.

File scope:
- `Documentation/ProductionManagement/`
- `Documentation/AssetProduction/` if prop/decal briefs are produced.

Implementation notes:
- Define shared clues: missing rivets, unusual green steam, cooler pipe color, worker chalk, misaligned plates, coal dust, mismatched labels.
- Define per-secret clue sets:
  - Level01 Intake Pressure Cache: service panel, warm pipe seam, chalk arrow.
  - Level02 Pipeworks Cartridge Cache: cold pipe, spare bolt marks, pipe-shadow recess.
  - Level04 Foundry Coal Cache: coal footprints, cooler quench pipe, sticky coal door mark.
  - Level03 optional Gauge Cache: gauge shutters or maintenance void, deferred.
  - Level05 optional Governor Clerk Void: mismatched clerk labels, deferred.

Validation:
- Each cache has one clue visible before discovery.
- Each cache is reachable without jump/crouch.
- Rewards are optional resources or lore, not required progression.

Parallelization:
- Can run alongside gameplay work.
- Scene placement is main integration later.

### P-03 Biome Dressing Kit Briefs

Lane: Parallel.

Goal: prepare prop and modular dressing requirements per level without touching live scene files.

File scope:
- `Documentation/AssetProduction/`
- Optional new planning files under `Documentation/ProductionManagement/`.

Implementation notes:
- Intake: soot brick, repair benches, small flywheels, work orders, service lift frame.
- Pipeworks: pipe-wall panels, baffles, gauge banks, cartridge crates, routing valve cluster.
- Boilerheart: boiler core, pressure valve catwalk props, scattergun display support, Bellows support pipes.
- Foundry: furnace row modules, slag gutters, quench tanks, hammer bay props, coal cache props.
- Governor: regulator pylons, logic drums, punch plates, clerk alcoves, master override hoist props.

Validation:
- Each kit includes collision guidance: visual-only clutter versus deliberate cover.
- Each kit includes LOD/static batching expectations.
- Asset names can later become validator names if integrated.

Parallelization:
- Safe in parallel while it remains a brief or staging-kit task.
- Importing or placing assets in scenes is main integration.

### P-04 Manual Playtest Route Sheets

Lane: Parallel.

Goal: create per-level playtest checklists for human route readability, independent of automated smoke.

File scope:
- `Documentation/ProductionManagement/`

Implementation notes:
- For each level, list first-time player questions:
  - Where do I go first?
  - What is locked?
  - What changed after the objective?
  - Did I understand the new enemy role?
  - Did hazards give warning before damage?
  - Did secrets feel fair after discovery?
- Include pass/fail language and note fields.
- Do not include stop points that block progress; this is observation guidance only.

Validation:
- A tester can run Level01-Level05 manually from the sheet.
- The sheet references exact level names and current objective chain.

Parallelization:
- Safe in parallel.
- Does not need scene ownership.

### P-05 Performance Hero-View Budget Notes

Lane: Parallel.

Goal: define the camera views most likely to stress performance before art dressing expands.

File scope:
- `Documentation/ProductionManagement/`
- `Documentation/AssetProduction/` if paired with art budgets.

Implementation notes:
- Level01: repair bay toward pressure gate and final room toward lift.
- Level02: condensate spine toward bridge/lift.
- Level03: Boilerheart ring toward core/valve/lift.
- Level04: furnace row toward Bulwark bay/hoist.
- Level05: Warden arena toward regulator core/hoist during shutdown VFX.
- Track expected risk: transparent steam, dynamic lights, moving machinery, repeated pipes, boss/heavy VFX.

Validation:
- Each level has one primary and one fallback hero view.
- Each view has a draw-call/particle/light risk note.
- Future integration can compare profiler output against the notes.

Parallelization:
- Safe in parallel as documentation.
- Profiling actual scenes is main integration.

## Main Integration Slices

### INT-01 Level01 Tutorial Loop Polish

Lane: Main integration.

Goal: make Brassworks Intake a polished first map without adding new systems.

File scope:
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Scenes/Level01.unity`
- Optional targeted updates to `RuntimeAutoPlaythroughTest` only if route coordinates change enough to break automation.

Implementation notes:
- Strengthen sightline from first combat room to pressure gate.
- Make key branch visibly loop back to gate.
- Improve service lift green directionality.
- Keep enemies Scrapper-only.
- Improve secret clue placement without accidental discovery in normal combat.
- Do not add lethal hazards.

Validation:
- Level validation requires named gate, key, lift, cover, archive plaque, secret cache, and updated route cue props.
- Auto-playthrough still completes key, gate, lift, transition to Level02.
- Secret smoke still finds/registers the Intake secret.
- Manual pass confirms gate is seen before key branch.

Parallelization:
- Cannot be parallel with other `V0SceneBuilder` edits.
- Can consume P-01 and P-02 outputs.

### INT-02 Level02 Pipeworks Loop And Lancer Read

Lane: Main integration.

Goal: expand Pipeworks Annex from narrow proof corridor into first ranged-combat production loop.

File scope:
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Scenes/Level02.unity`
- `Assets/_Project/Scripts/Utility/RuntimeAutoPlaythroughTest.cs` if lock/valve positions or transition timing change.
- `Assets/_Project/Scripts/Utility/RuntimeRangedCombatTest.cs` only if a new ranged-read smoke is added.

Implementation notes:
- Add arrival lift, baffle corridor, condensate spine, locked lift lobby, routing valve gallery, Lancer bridge, resource side room, cartridge cache.
- Show the Boilerheart lift locked before routing valve completion.
- Give the first Lancer a longer sightline with cover breaks.
- Keep damaging hazards out of this level.
- Keep the current Pipeworks secret, but make clueing subtler.

Validation:
- Level validation requires routing valve, valve wheel, vented lamp, locked lift, Lancer, secret cache, pipework dressing, and route signage.
- Auto-playthrough verifies locked lift rejection, valve completion, objective update, transition to Level03.
- Ranged combat smoke still verifies Lancer projectile readability.
- Manual pass confirms player can break line of sight without snagging.

Parallelization:
- Main integration only.
- Can run after P-01 signage and P-02 secret brief are available.

### INT-03 Level03 Boilerheart Ring Cohesion

Lane: Main integration.

Goal: turn Level03 into a readable ring loop around scattergun, Bellows Node, steam hazards, valve, and foundry lift.

File scope:
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Scenes/Level03.unity`
- `Assets/_Project/Scripts/Utility/RuntimeAutoPlaythroughTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeHazardTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeWeaponSwitchTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeBellowsNodeTest.cs`

Implementation notes:
- Build a central Boilerheart Ring with side branches.
- Place scattergun pickup before the Bellows Node teaching spike.
- Create a simple scattergun trial encounter.
- Give Bellows Node a separate chamber with a visible pulse radius.
- Place pressure valve so it visually connects to hazards and foundry lift.
- Make steam shutdown after valve completion visible along the return route.

Validation:
- Level validation requires scattergun pickup visuals/cues, Bellows Node visuals, steam hazards, pressure valve, foundry lift, and archive plaque.
- Weapon-switch smoke still verifies pickup, persistence, fire, and alternate fire.
- Bellows smoke verifies pulse damage, boost, boost VFX, and destruction.
- Hazard smoke verifies steam damage and VFX.
- Auto-playthrough verifies locked lift, valve completion, linked hazard shutdown, transition to Level04.
- Manual pass confirms scattergun read occurs before Bellows read.

Parallelization:
- Main integration only.
- Can consume P-03 Boilerheart kit brief.

### INT-04 Level03 Bellows Node Spatial Readability

Lane: Main integration.

Goal: polish Bellows Node support-machine readability without changing its core behavior.

File scope:
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Scenes/Level03.unity`
- `Assets/_Project/Scripts/Utility/RuntimeBellowsNodeTest.cs`
- Possible `Assets/_Project/Scripts/Utility/BellowsNodePulseVfx.cs` only if VFX behavior must expose additional visible pieces.

Implementation notes:
- Add floor ring or pressure-line radius marker.
- Add pipes/warnings pointing from Bellows Node to boosted Scrapper lane.
- Keep one nearby Scrapper boost target.
- Avoid adding Lancer support in this slice.

Validation:
- Bellows smoke still passes.
- Level validation requires named Bellows readability pieces.
- Manual pass confirms the player can identify pulse radius before taking repeat damage.

Parallelization:
- Main integration because it touches Level03 and validation.
- Can be done independently from Level04/Level05 work if `V0SceneBuilder` is not otherwise owned.

### INT-05 Level04 Furnace Foundry Arena Rebuild

Lane: Coordinate-first main integration.

Goal: make Furnace Foundry the first true hazard-combat showcase and Bulwark reveal.

File scope:
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Scenes/Level04.unity`
- `Assets/_Project/Scripts/Utility/RuntimeHazardTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeBulwarkCombatTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeAutoPlaythroughTest.cs` if transition coordinates or required conditions change.

Implementation notes:
- Coordinate with active Bulwark lane before touching any Bulwark scene placement or validation.
- Expand into arrival hoist, furnace row, side loop, gantry lane, Bulwark hammer bay, coal cache, emergency hoist.
- Teach furnace heat timing before Bulwark.
- Put Bulwark in a wide mostly flat bay.
- Keep Lancer support outside the first Bulwark read unless arena width supports it.
- Keep coal cache reachable without crossing active lethal heat.

Validation:
- Level validation requires foundry furnace row, steam hazards, furnace heat hazards, Bulwark, hoist, secret cache, archive plaque, and named cover/readability props.
- Hazard smoke verifies steam and heat damage/VFX.
- Bulwark smoke verifies attack tell before damage and kill/death behavior.
- Auto-playthrough still transitions Level04 to Level05.
- Manual pass confirms heat damage source is distinguishable from Bulwark damage.

Parallelization:
- Must not run in parallel with active Bulwark gameplay edits.
- Can consume P-02, P-03, and P-05 outputs.

### INT-06 Level04 Cooling Regulator Objective Candidate

Lane: Main integration, optional after INT-05.

Goal: add a simple foundry objective only if Level04 needs more structure after the arena rebuild.

File scope:
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Scenes/Level04.unity`
- `Assets/_Project/Scripts/Utility/RuntimeAutoPlaythroughTest.cs`
- Possible reusable objective script only if existing `SteamValveObjective` cannot represent the regulator cleanly.

Implementation notes:
- Prefer reusing existing interactable valve/objective patterns.
- Hoist starts blocked by heat surge or pressure state.
- Cooling regulator lowers surge near exit and powers hoist green.
- Do not add this if it makes Level04 compete with Level03's valve identity.

Validation:
- Auto-playthrough verifies locked hoist rejection, regulator completion, objective update, transition to Level05.
- Level validation requires regulator object, prompt, signal states, and hoist lock link.
- Hazard smoke still passes.

Parallelization:
- Main integration only.
- Depends on INT-05 route shape.

### INT-07 Level05 Governor Core Boss Arena Rebuild

Lane: Main integration.

Goal: turn Governor Core from a validated finale lane into a readable boss arena.

File scope:
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Scenes/Level05.unity`
- `Assets/_Project/Scripts/Utility/RuntimeWardenCombatTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeAutoPlaythroughTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeHazardTest.cs` if hazard placement changes materially.

Implementation notes:
- Add arrival hoist, pressure chapel approach, core ring, west/east regulator arms, Warden arena, master override hoist.
- Keep Warden reveal centered and front-facing.
- Place cover pylons with enough spacing to preserve movement.
- Keep hazards on edges or pre-boss lanes, not in the Warden's main stomp lane.
- Keep the final hoist visible and locked before Warden defeat.

Validation:
- Level validation requires Warden, guardian objective, boss HUD wiring, final hoist, lock signals, regulator landmark, hazards, archive plaque, and cover props.
- Warden combat smoke verifies boss health HUD, damage feedback, death, and shutdown VFX.
- Auto-playthrough verifies hoist locked before Warden defeat, Warden defeat unlocks hoist, final win fires, secret totals persist.
- Manual pass confirms damage source clarity during boss fight.

Parallelization:
- Main integration only.
- Can consume P-05 performance notes and P-03 Governor kit brief.

### INT-08 Objective Device Feedback Cohesion

Lane: Main integration.

Goal: make valves, gates, lifts, hoists, and lock releases consistently readable across all levels.

File scope:
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Scenes/Level01.unity` through `Assets/_Project/Scenes/Level05.unity`
- `Assets/_Project/Scripts/Utility/RuntimeAutoPlaythroughTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeInteractionTest.cs`
- Existing VFX/audio utility scripts only if current hooks cannot express the needed feedback.

Implementation notes:
- Standardize red locked, amber objective, green restored/exit language.
- Ensure each lock has a local signal and a route signal.
- Add state-change props only when validation can require them by name.
- Do not change objective chain semantics in this slice.

Validation:
- Auto-playthrough verifies every current lock/unlock transition.
- Interaction smoke still verifies lore plaque and prompt behavior.
- Level validation requires named device feedback pieces per level.
- Manual pass confirms a player can tell what changed after interacting without relying only on HUD text.

Parallelization:
- Main integration because it touches shared generated scenes.
- Can be split by level only if each level owns separate files and `V0SceneBuilder` ownership is coordinated.

### INT-09 Secret Cache Readability Integration

Lane: Main integration.

Goal: apply the secret visual language to current registered secrets.

File scope:
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Scenes/Level01.unity`
- `Assets/_Project/Scenes/Level02.unity`
- `Assets/_Project/Scenes/Level04.unity`
- `Assets/_Project/Scripts/Utility/RuntimeSecretTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeAutoPlaythroughTest.cs`

Implementation notes:
- Add clue props before each secret trigger.
- Make trigger volumes avoid accidental discovery during normal combat.
- Preserve current secret IDs unless there is a deliberate migration plan.
- Do not add Level03 or Level05 secrets in this slice.

Validation:
- Runtime secret smoke passes.
- Auto-playthrough still verifies expected total registered secrets through final win.
- Level validation requires named clue props for each current secret.
- Manual pass confirms every secret reads as fair after discovery.

Parallelization:
- Main integration because it touches multiple generated scenes and secret validation.
- Can follow P-02.

### INT-10 Hazard Audio And Timing Readability

Lane: Main integration.

Goal: make steam and furnace hazards readable by sound and timing, not only visible puffs/plates.

File scope:
- `Assets/_Project/Scripts/Utility/SteamworksAudio.cs`
- `Assets/_Project/Scripts/Utility/SteamHazardVfx.cs`
- `Assets/_Project/Scripts/Utility/FurnaceHeatHazardVfx.cs`
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Scenes/Level03.unity`
- `Assets/_Project/Scenes/Level04.unity`
- `Assets/_Project/Scenes/Level05.unity`
- `Assets/_Project/Scripts/Utility/RuntimeHazardTest.cs`

Implementation notes:
- Add warning/active audio cues for furnace heat.
- Add stronger pre-damage steam cue if current puffs are not enough in manual play.
- Keep hazards avoidable through lateral movement.
- Ensure cues do not mask enemy attack tells.

Validation:
- Hazard smoke verifies damage and visible VFX.
- Add smoke coverage for audio cue presence only if the audio API can expose it cleanly.
- Manual pass verifies the player can identify when a lane is unsafe before damage.

Parallelization:
- Main integration because it touches shared audio/VFX scripts and scenes.
- Should not run while another agent owns `SteamworksAudio.cs`.

### INT-11 Route Cohesion Pass After Level Expansions

Lane: Main integration.

Goal: make the five-level route feel like one authored run after individual level polish slices land.

File scope:
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Scenes/Level01.unity` through `Assets/_Project/Scenes/Level05.unity`
- `Assets/_Project/Scripts/Utility/RuntimeAutoPlaythroughTest.cs`
- Any targeted runtime smoke whose assumptions changed.

Implementation notes:
- Review health/ammo carryover across levels.
- Check objective text handoff at every lift/hoist.
- Check first spawn orientation in each level.
- Check enemy learning order from Scrapper to Lancer to Bellows to Bulwark to Warden.
- Check secrets total and win message after route completion.

Validation:
- Full V0 matrix passes.
- Manual Windows playthrough recommended.
- Auto-playthrough verifies all transitions and objective state updates.
- Runtime performance profile should be reviewed on the heaviest views.

Parallelization:
- Must stay in main integration.
- Should run after INT-01 through INT-09, or after any smaller subset that materially changes route flow.

## Suggested Dependency Order

1. Parallel: P-01, P-02, P-03, P-04, P-05 can run immediately if they stay inside their scopes.
2. Main: INT-01 Level01 tutorial loop.
3. Main: INT-02 Level02 ranged/valve loop.
4. Main: INT-03 Level03 Boilerheart ring.
5. Main: INT-04 Bellows readability if it is not absorbed into INT-03.
6. Coordinate-first main: INT-05 Level04 foundry arena after Bulwark lane ownership is clear.
7. Optional main: INT-06 cooling regulator only after Level04 route shape is stable.
8. Main: INT-07 Level05 boss arena.
9. Main: INT-08 objective feedback or INT-09 secret readability can run when `V0SceneBuilder` ownership is available.
10. Main: INT-10 hazard audio/readability after hazard placement stabilizes.
11. Main: INT-11 route cohesion after major scene shape changes.

## Work That Must Not Be Parallelized

- Two agents editing `Assets/_Project/Editor/V0SceneBuilder.cs` at the same time.
- Two agents editing `Assets/_Project/Editor/V0LevelValidator.cs` at the same time.
- Any scene edit to the same `LevelXX.unity` file by multiple agents.
- Bulwark placement, Bulwark validation, or Bulwark combat smoke while the active Bulwark lane owns those files.
- Warden arena, Warden combat smoke, and final hoist changes split across separate agents.
- Objective chain changes without auto-playthrough changes in the same integration slice.
- Secret ID or total changes without secret smoke and auto-playthrough updates.

## Definition Of Done For A V1 Level Polish Slice

- The route is playable from that level's spawn to its exit or win device.
- The main objective can be understood from world composition and HUD text.
- New or moved gameplay objects are validated by name.
- Runtime smoke covers any changed lock, enemy role, hazard, secret, or transition.
- Manual play confirms no required jump, crouch, narrow snag, or forced fast turn.
- Art dressing improves orientation instead of hiding gameplay signals.
- Performance risk is noted for the level's heaviest hero view.
