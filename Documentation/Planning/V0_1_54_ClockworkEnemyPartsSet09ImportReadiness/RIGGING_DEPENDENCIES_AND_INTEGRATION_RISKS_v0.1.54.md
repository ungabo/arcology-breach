# CEPS09 Rigging Dependencies And Integration Risks

## Deferred Rigging Dependencies

- Final skeleton hierarchy per archetype: skitter quadruped root/leg chain, brute humanoid upper/lower limb chain, sentry rotary mount chain.
- Bind poses, skin weights, or rigid parent constraints for each approved part.
- IK targets for skitter legs, brute saw/claw arms, brute heavy feet, and sentry aiming pivots.
- Animation clips for idle, locomotion, attack windup, attack recovery, recoil, stagger, damaged-loop, and destruction.
- VFX sockets for steam vents, amber optic glow, furnace weak points, saw sparks, and pressure leaks.
- Main-lane ownership for hit volumes, weak points, colliders, AI behavior, audio emitters, and damage state swaps.

## Integration Risks

- Procedural meshes are lookdev geometry, not final optimized enemy production meshes.
- Transparent/emissive amber glass may need render-pipeline tuning after primary import.
- Socket transforms are named and placed for planning, but final rigging may need offset changes after animation blockout.
- Archetype preview prefabs communicate silhouette only and should not define gameplay scale or combat reach.
- Runtime textures are deterministic base maps; final PBR replacement can happen later if art direction demands higher fidelity.

## Mitigations

- Import into quarantine and compare against existing enemy scale references before promotion.
- Preserve the `SOCK_*` naming contract when creating final rig hierarchies.
- Promote only approved parts into gameplay assemblies; keep the sidecar visual-only.
- Run a main-lane component scan after any future promotion to catch accidental colliders, scripts, animators, or scene references.
