# V0.1.36 Acceptance Gates And Targeted Validation

Purpose: define what later integration should prove before animation and rigging readiness is treated as usable.

## Documentation Gates

- Every enemy family has root, torso, head/mask, tool, weak-point, furnace-eye, audio, and shutdown socket expectations.
- Pressure Pistol and Steam Scattergun have viewmodel, pickup/display, muzzle, reload, recoil, gauge, and VR hand socket expectations.
- Future weapon silhouettes have a minimum socket set that supports pickup, display, fire, reload, alt-fire, and VR hand alignment.
- Animation backlog covers idle, move, attack tell, attack/fire, hit reactions, shutdown/death, pickup/display idle, viewmodel fire, reload, and alt-fire.
- Unity notes explicitly preserve current gameplay, route, spawn, pickup, and validator authority.

## Later Asset Gates

Use these once actual rigged assets or animation clips are produced:

- Import produces no missing bones, missing materials, or broken socket transforms.
- Socket transforms retain exact names through prefab replacement.
- Enemy roots remain at floor contact center and weapons retain stable grip/recoil/pickup pivots.
- Scrapper reads as low fast saw-claw, Lancer as thin forward lance, Bulwark as shield wall, Warden as command tower, and Foundry Overseer as tri-tool miniboss from silhouette alone.
- Amber/red weak points and cyan/blue charge tells remain visually distinct in idle, tell, hit, and shutdown states.
- Viewmodel weapons keep muzzle alignment believable during fire, reload, and alt-fire.
- LOD1/LOD2 do not remove role-defining silhouettes or critical sockets.

## Combat Readability Gates

- Each attack tell changes silhouette before damage, projectile, or impact timing would occur in a later gameplay pass.
- Hit reactions should communicate impact direction or weak-point break without rotating the gameplay root unexpectedly.
- Shutdown clips must not look like attack windups. Lamps dim, tools drop, and pressure vents release.
- Bulwark shield animations must show whether the shield is braced, open, staggered, or broken.
- Lancer and Warden ranged tells must expose charge sequence through posture and cyan socket order, not only through sound or UI.
- Foundry Overseer tells must separate saw, hammer, back-lance, and crown-burst patterns.

## Gameplay Authority Gates

- No animation clip should be the only source of combat timing truth until the gameplay owner explicitly wires events.
- No root motion should move enemies through route blockers, doors, hazard lanes, or authored encounter constraints without integration review.
- No pickup/display idle animation should move a weapon outside existing pickup volume assumptions.
- No weapon viewmodel animation should change ammo, damage, reload, unlock, or alt-fire rules by itself.
- No rig socket should imply new interactables, weak-point rules, or destructibility unless a gameplay task owns those systems.

## Targeted Validation Ideas

These are small validation passes for the later integration owner, not commands required by this docs-only bundle.

1. Silhouette lineup review: place all five enemy families in neutral gray with no emissives and verify role identification at combat distance.
2. Tell distance review: play each attack tell at expected arena distance and confirm the tell is readable before any hypothetical hit frame.
3. Weak-point isolation review: toggle furnace eyes, weak lamps, and cyan charge sockets independently to catch confusing overlap.
4. Muzzle alignment review: freeze viewmodel fire frames and compare `SOCK_Muzzle` against the expected screen/crosshair direction.
5. Reload volume review: play reload clips near walls and route props to ensure weapon/hand arcs do not imply collision with route-critical geometry.
6. LOD animation review: switch LODs during idle, move, tell, and shutdown to verify no critical socket vanishes.
7. Low/mid PC sanity review: spawn representative enemy counts with idle/move/tell loops and confirm Animator layers, transform counts, and material instance churn stay modest.
8. VR hand preview review: align simple controller proxies to `SOCK_VR_Hand_R` and `SOCK_VR_Hand_L` for Pressure Pistol, Steam Scattergun, and one future silhouette.
9. Route authority review: confirm any root motion or display idle keeps actors and pickups inside existing route/pickup ownership.
10. Shutdown readability review: play death/shutdown in dim lighting and verify it cannot be mistaken for a charged attack.

## Stop Conditions

- A rig replacement removes or renames required sockets without a migration table.
- A tell relies only on tiny material flicker or gauge motion.
- A weak point is visually indistinguishable from a furnace eye or muzzle charge.
- A viewmodel clip moves the muzzle or reload hand so aggressively that gameplay feedback feels detached.
- Root motion is proposed before navigation and route constraints are reviewed.
- VR hand sockets are added by scaling or skewing desktop weapon proportions beyond the staged silhouette.

