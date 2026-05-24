# v0.1.36 Final Art Promotion Candidate Review Bundle

Created: `2026-05-24`

Owned scope:

- `Documentation/Planning/V0_1_36_ArtPromotionReview/`
- `Documentation/AssetProduction/V0_1_36_ArtPromotionReview/`

This bundle reviews the staged `v0.1.35` art packages for promotion into the main lane. It is documentation-only and does not claim gameplay, route, validator, scene, script, build, or release authority.

## Reviewed Source Packages

| Family | Source package | Evidence reviewed |
| --- | --- | --- |
| Feedback polish | `Documentation/AssetProduction/V0_1_35_FeedbackPolish/` | Manifest, UI/audio/VFX recipes, accessibility gates, import/performance notes, concept render sheets. |
| Weapon arsenal | `Documentation/AssetProduction/V0_1_35_WeaponArsenal/` | Manifest, component breakdown, material recipes, acceptance gates, scale/collision/LOD notes, concept render sheets. |
| Mechanical enemy pack | `Documentation/AssetProduction/V0_1_35_MechanicalEnemyPack/` | Manifest, integration notes, Unity proof contact sheets, material/tell conventions. |
| Level module setpieces | `Documentation/AssetProduction/V0_1_35_LevelModuleSetpieces/` | README, material/LOD/collider guidance, acceptance gates, Unity proof renders, placement-planning context. |
| Performance/import envelope | `Documentation/Planning/V0_1_35_PerformanceImportBundle/` | Performance budgets, import rules, validation gates. |
| Gameplay validation envelope | `Documentation/Planning/V0_1_35_BatchValidation/` | Route-authority contract, feedback breadth gates, pass/fail rules. |

## Top Recommendation

Promote in three milestone batches, starting with presentation-only feedback and narrow weapon/pickup props before broad environment placement or enemy gameplay replacement.

| Batch | Recommendation | Families | Why first |
| --- | --- | --- | --- |
| Batch A | Main-lane promotion candidate | Feedback polish cue package; pressure cartridges; wall weapon display; ammo cabinet visual shell. | Highest readability value, lowest route-authority risk, small import footprint, no scene geometry dependency. |
| Batch B | Conditional promotion after proof renders and fit checks | Pressure pistol component pack; steam scattergun component pack; Scrapper/Lancer/Bulwark visual enemy shells. | Big visual leap, but requires first-person crop, socket, hit/death readability, and performance proof in representative combat. |
| Batch C | Staging-only until route/performance proof | Level module setpieces; Warden/Foundry Overseer elite shells; future lightning lance silhouette. | Largest route/performance surface and highest chance of implying gameplay that is not yet proven. |

## Promotion Decision Summary

### Ready For First Main-Lane Promotion

- `V0_1_35_FeedbackPolish`: promote as cue recipes, icons, short placeholder audio, and readability policy only. Map to existing event hooks and preserve the no-authority contract.
- `V0135WA_003 Pressure Cartridge Family`: promote as pickup visual variants and UI/icon source after trigger volumes stay owned by existing pickup systems.
- `V0135WA_004 Wall Weapon Display Frame`: promote as a non-blocking armory/pickup framing prop.
- `V0135WA_005 Ammo Cabinet / Vending Prop`: promote as visual shell only, without vending interaction authority.

### Needs More Proof Before Main-Lane Promotion

- `V0135WA_001 Pressure Pistol Final Component Pack`: strong candidate, but needs first-person camera crop proof, hand/socket proof, material replacement proof, and triangle/material stats.
- `V0135WA_002 Steam Scattergun Final Component Pack`: strong candidate, but needs muzzle-direction readability, recoil/flash compatibility, viewmodel clearance, and LOD proof.
- Scrapper, Lancer, and Bulwark enemy visual shells: good silhouette/readability candidates, but need rig socket, damage-window, tell/readability, and LOD evidence inside combat spaces.

### Keep Staging-Only For Now

- `V0135WA_006 Future Alt Lightning Lance Silhouette`: explicitly future exploration; useful reference, not a v0.1.36 promotion asset.
- Warden and Foundry Overseer Elite visual shells: high-value finale art, but should wait for boss-flow proof so visual lamps/coils do not conflict with Warden HUD, guardian lock, defeat, final-hoist unlock, or final exit language.
- Level module setpieces as broad multi-level placement: excellent kit direction, but route clearance, draw-call/light budgets, collision policy, LODs, and first-person smoke screenshots remain open gates.

## Milestone Promotion Order

### Milestone 1: Feedback And Pickup Readability

Promote feedback polish plus small weapon-support props. Keep all assets presentation-only and attach to existing state owners.

Acceptance emphasis:

- All eleven feedback cue IDs map to existing hooks.
- Low-health, low-ammo, denied, objective, route, pickup, and secret feedback remain distinct by shape/audio/timing, not color alone.
- Pickup props use primitive/non-authoritative colliders only where existing systems already own trigger authority.
- No visual/audio element blocks prompts, pickup roots, transition volumes, enemy tells, or route-state objects.

### Milestone 2: Viewmodel Weapons And Common Enemy Silhouettes

Promote pressure pistol/scattergun final component packs after first-person proof. In the same or following batch, promote Scrapper/Lancer/Bulwark as visual shells once rigging and tell sockets are proven.

Acceptance emphasis:

- Weapon viewmodel crops preserve gauge/lamp class reads without covering reticle, prompts, objective text, enemy tells, or hazard warnings.
- Weapon/pickup LOD and primitive collision rules are documented with import stats.
- Enemy weak-point lamps, cyan charge tells, shutdown fragments, and silhouette tags remain readable in Level02, Level03, and Level04 representative spaces.
- Enemy visual children do not add hitbox, hurtbox, navigation, attack timing, spawn, damage, or weak-point authority.

### Milestone 3: Environment Modules And Finale/Elite Art

Promote selected level modules only as targeted setpiece swaps for high-value beats, not as blanket dressing. Promote Warden/elite art after final-flow proof.

Acceptance emphasis:

- First placements target Level03 Bellows/Scattergun and Level04 Furnace/Bulwark spaces first, then Level05 Warden/Core Ring.
- Route clearance screenshots prove doors, lifts, hoists, boss lane, secrets, pickups, and final exit are unobstructed.
- Decorative lights are budgeted; proof lights are not copied wholesale.
- Warden/elite lamps and coils support, rather than compete with, boss HUD, guardian lock, defeat, final-hoist unlock, and restored-exit language.

## Files In This Bundle

- `Documentation/Planning/V0_1_36_ArtPromotionReview/README_V0_1_36_ART_PROMOTION_REVIEW.md`
- `Documentation/Planning/V0_1_36_ArtPromotionReview/CANDIDATE_SCORECARD_V0_1_36.md`
- `Documentation/AssetProduction/V0_1_36_ArtPromotionReview/BUNDLE_ACCEPTANCE_GATES_AND_TBD_V0_1_36.md`
