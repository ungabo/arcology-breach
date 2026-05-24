# V0.1.36 Animation Rigging Readiness Asset Handoff Checklist

Purpose: give future modelers, riggers, animators, and Unity integrators a compact checklist before producing final rigs or clips.

## Enemy Handoff

For Scrapper, Lancer, Bulwark, Warden, and Foundry Overseer:

- Preserve `RIG_Root`, `RIG_Hips`, `RIG_SpineCage_01`, `RIG_HeadOrMask`, arm roots, and leg roots.
- Preserve family-specific tool sockets exactly as named in the planning standards.
- Keep weak-point sockets separate from furnace-eye sockets.
- Keep cyan charge/tell sockets separate from muzzle and weak-point sockets.
- Keep shutdown burst and shutdown fragment sockets available even if fragments are disabled at runtime.
- Export a neutral pose, combat idle pose, and socket visibility screenshot with each rig candidate.
- Provide a socket migration table if any final rig cannot preserve the recommended name.

## Weapon Handoff

For Pressure Pistol and Steam Scattergun:

- Preserve grip, trigger, muzzle, recoil pivot, reload, gauge, pickup/display, and VR hand sockets.
- Keep viewmodel pivot and pickup/display pivot distinct.
- Keep muzzle sockets stable during recoil and reload authoring.
- Export first-person alignment reference, world pickup reference, and wall-display reference.
- Provide reload path notes for pressure cells, shells, pumps, breeches, gauges, and coil resets.

## Clip Handoff

- Use the backlog naming format unless the integration owner assigns an asset-path convention.
- Identify any clip that expects root motion.
- Identify any clip that expects later Animation Events.
- Identify any procedural layer assumptions: recoil, aim offset, gauge flick, coil compression, pump slide, vent pulse, or hit-stop.
- Include timing notes for attack tells and firing frames, but do not make those notes gameplay authority.

## Review Packet Minimum

Each future rig/clip packet should include:

- Rig hierarchy screenshot or text export.
- Socket list.
- Clip list with loop/root-motion/additive flags.
- One neutral silhouette image per enemy or weapon.
- One combat/readability image per enemy or weapon.
- Notes on LOD compatibility and disabled decorative motion.
- Known risks before gameplay wiring.

