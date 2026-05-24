# Enemy Readability Batch Art Notes

Date: 2026-05-24
Package state: staged art/readability payload for upcoming integration.

## Batch Readability Strategy

Each enemy gets the same layered readability structure so future integration can compare them as a set:

1. Silhouette: a distinct height/width/weapon profile visible before materials matter.
2. Furnace identity: paired hot amber eyes in a soot or visor cavity.
3. Pressure language: red pressure tanks, brass collars, copper pressure rods, and dark hoses.
4. Attack tell: cutter, hammer, shield, or blue bolt geometry called out as named mesh groups.
5. Weak point: amber lamp glass separated from furnace eyes so damage affordance does not collapse into face read.
6. Shutdown read: detached fragment kits with dim metal, broken lenses, tank caps, and enemy-specific tell pieces.
7. Material separation: chassis, trim, pipes, face plate, lamps, tanks, tells, and fragments are split into named materials.

## Scrapper

Silhouette target: compact hunched melee worker-machine with low center of mass and oversized tool arms.

- Furnace eyes sit in a cream mask over a soot visor slit.
- The chest weak-point lamp is centered on the boiler, separate from the eyes.
- Twin rear pressure tanks make the back profile readable when it turns or retreats.
- Right arm carries a cutter wheel with hazard teeth and an amber hub.
- Left arm carries a hammer head with a heavy striker face.
- Shutdown fragments include mask, furnace glass, lamp lens, tank cap, chassis shard, cutter tooth, and hammer face.

Integration note: keep the cutter/hammer side asymmetry. If the rig mirrors the arms later, preserve one circular cutter read and one block hammer read.

## Lancer

Silhouette target: tall ranged automaton with a thin body and an unmistakable forward lance line.

- Furnace eyes are smaller and higher than Scrapper to keep the head narrow.
- The sternum weak lamp should remain visible behind or under the weapon brace.
- The back pressure tank reinforces ranged-charge pressure without adding front clutter.
- Blue bolt rings run along the lance and should be the primary pre-fire telegraph.
- The muzzle blue core is separated from the weak lamp so the player can distinguish "about to fire" from "shoot here."
- Shutdown fragments include bolt coil and muzzle sleeve pieces.

Integration note: the lance should keep a long uninterrupted +Z direction line. Avoid bulky hand or shoulder additions that make it read like another melee enemy.

## Bulwark

Silhouette target: broad defender with shield-door mass and obvious frontal denial.

- Shield plate is the first read: large blackened iron slab with brass rims and hazard enamel marks.
- Furnace eyes are tucked into a brow above the shield, so the creature still has a face.
- Weak-point lamps are placed left/right on the shield face to invite flanking or timing reads depending on future combat design.
- Shoulder pressure tanks widen the upper profile without becoming explosive pickups.
- Hammer tell sits on one side so a slam windup can read past the shield mass.
- Shutdown fragments include shield hinge and hammer face pieces.

Integration note: if the final gameplay wants a front-shield rule, this package keeps the side weak lamps physically separate enough to promote those targets without authoring the rule here.

## Warden

Silhouette target: tall command unit with a cage/tower body and overhead bolt language.

- Vertical cage ribs and brass rings make the torso read as a governor/control unit.
- Furnace eyes are housed in a command face plate.
- Central weak-point lamp is placed on the tower body, not in the head.
- Crown pressure tanks and overhead bolt coils create a top-heavy command silhouette.
- Blue coil rings should become the charge/readability language for command or bolt effects.
- Shutdown fragments include cage rib and bolt coil pieces.

Integration note: preserve the height and crown read. The Warden should not collapse into a tall Lancer; the cage, crown tanks, and overhead charge spine are the differentiators.

## Material Separation Notes

- Blackened iron should stay dominant on the core silhouette.
- Aged brass should be trim, retainers, bands, cage ribs, and readable mechanical structure.
- Copper should imply pressure transfer and movement.
- Cream enamel should stay localized to face/identity plates.
- Amber furnace eyes should read as identity/alert, while weak-point lamps should read as damage affordance.
- Blue should be reserved for bolt or ranged-charge tells.
- Red pressure tanks should be visible but not framed like pickups or guaranteed explosion targets.
- Hazard enamel should mark active tool danger: cutter teeth, hammer faces, shield warnings.

## Animation And VFX Handoff Targets

This package does not author animation or VFX, but the mesh groups are named for future sockets and timing:

- Furnace eyes: idle glow, alert brighten, shutdown dim.
- Weak-point lamps: damage flare, break flash, dim state.
- Pressure tanks: vent puffs, overpressure shake, shutdown cap pop.
- Cutter tell: spin blur, spark arc, windup flash.
- Hammer tell: raised windup pose, striker glow, impact shake.
- Bolt tell: charge rings brighten in sequence, muzzle flare, post-fire dim.
- Shutdown fragments: breakaway pieces, dim glass, pressure cap ejection.

## Review Gate

The batch is acceptable for integration prep when each enemy can be identified from silhouette alone, then confirmed by material/tell language at a glance:

- Scrapper: short/hunched, cutter plus hammer.
- Lancer: tall/thin, long lance plus blue bolt rings.
- Bulwark: broad/shielded, hammer and side lamps.
- Warden: tall/caged, crown tanks and overhead bolt coils.
