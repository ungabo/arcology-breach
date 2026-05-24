# V0.1.36 Unity Implementation Notes

Purpose: make the rigging and animation backlog compatible with later Unity integration while preserving current gameplay and route authority.

## Animator Strategy

- Keep enemy visual Animator components below gameplay-controlled roots. Navigation, route authority, hit boxes, spawn logic, and combat scripts should remain on their existing owners when integration happens.
- Prefer one Animator Controller family for mechanical enemies only after the clip set proves state parity. Until then, use per-family controllers or override controllers to avoid forcing boss, shield, and ranged behavior into the same graph.
- Use explicit states for major readable tells: idle, locomotion, attack tell, attack release, hit reaction, stagger, and shutdown.
- Keep attack tell clips long enough to be read at combat distance before any future damage event or projectile spawn is fired.
- Avoid baking gameplay events into clips in this planning stage. Later Animation Events may be added by the integration owner, but the source clips should remain readable if played without events.

## AnimationClip Import Notes

- Enemy locomotion may be authored in-place first. Root motion should be opt-in only after navigation authority is reviewed.
- Mechanical enemies should support additive upper-body layers for aim, tool windup, recoil, and hit flinch.
- Weapon viewmodel clips should be authored around a stable camera-relative pivot, not a world pickup pivot.
- Gauge needles, coils, pumps, vents, shield hinges, and tool heads should be separate child transforms so simple procedural motion can layer over authored clips.
- Use AnimationClip curves for non-gameplay visual properties only: glow intensity, gauge needle rotation, coil compression, small vent flaps, or display idle sway.
- Do not require humanoid retargeting. These are mechanical rigs with family-specific proportions.

## Procedural Motion Compatibility

Leave room for:

- Procedural weapon recoil layered on `SOCK_RecoilPivot`.
- Procedural muzzle climb and recovery on the viewmodel weapon root.
- Procedural pump, coil compression, and gauge bounce as additive child motion.
- IK or aim offsets on Lancer, Warden, and Foundry Overseer ranged/tell sockets.
- Low-amplitude idle sway on pickup/display weapons without changing pickup authority.
- Hit-stop or stagger overlays that do not move the gameplay root.

Recommended separation:

| Motion | Authored clip | Procedural layer |
| --- | --- | --- |
| Enemy locomotion | gait, body weight, foot timing | small foot spark, servo jitter, hit-stop overlay. |
| Attack tell | posture, tool raise, coil charge sequence | glow pulse, audio source timing, muzzle aim offset. |
| Weapon fire | hand pose, trigger, base recoil | camera kick, muzzle flash, smoke, gauge needle flick. |
| Reload | hand path, cell/shell timing | tiny gauge settle, coil reset, pickup idle resume. |
| Shutdown | collapse, tool drop | fragment burst, steam vent, lamp dim. |

## Low/Mid Windows PC Performance

Targets should favor readability and stable frame time over high bone counts.

- Keep enemy mechanical rigs lean: broad torso, head/mask, two or three limb segments per appendage, separate tool heads, and only necessary decorative child transforms.
- Prefer transform animation over high-density skinned deformation for pistons, cages, coils, and shield parts.
- Use LOD-friendly animation: LOD1 can keep major bones and disable tiny decorative jiggle; LOD2 can freeze gauges, coil micro-motion, and fragment children.
- Avoid per-enemy Animator graphs with excessive layers, expensive IK, or always-on procedural scripts when several enemies are active.
- Batch visual-only glow or vent changes through material instances carefully during integration; do not create runaway unique materials for every idle pulse.
- Keep shutdown fragments pooled or disabled by default if later implemented. The readiness sockets should exist without forcing runtime fragment simulations.

## Future VR Hand Alignment

The weapon standards should support later VR without committing the current desktop build to VR behavior.

- Preserve `SOCK_VR_Hand_R` and `SOCK_VR_Hand_L` on every future-ready weapon.
- Keep `SOCK_Grip_Main` and `SOCK_Grip_Support` distinct from VR sockets; desktop viewmodel hands, pickup displays, and VR controller alignment have different needs.
- Avoid scaling weapon meshes to fit viewmodel hands in a way that breaks real-world-ish grip spacing.
- Keep trigger, pump, breech, and pressure-cell sockets reachable from plausible hand positions.
- For Steam Scattergun, the support hand should align with pump travel, not only a static forward grip.
- For Pressure Pistol, support hand can remain optional, but the left-hand socket should still exist for inspection, two-hand aiming, or accessibility modes.

## Readability Compatibility

- Amber/red weak-point sockets must stay separate from furnace-eye identity sockets.
- Cyan/blue charge tell sockets must stay separate from weak-point sockets.
- Attack tells must expose a pose silhouette change, not only emissive material changes.
- Shutdown/death clips should dim identity lamps and weak lamps distinctly so players can tell "dead" from "charging."
- Weapon firing and reload clips should preserve muzzle direction and HUD/readability assumptions. Do not move the muzzle so far during fire that traces, VFX, or crosshair reads feel detached.

