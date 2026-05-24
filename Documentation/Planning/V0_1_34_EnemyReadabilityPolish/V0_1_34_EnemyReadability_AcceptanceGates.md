# V0.1.34 Enemy Readability Acceptance Gates

Date: 2026-05-24

## Global Gates

- [ ] Unity imports all five OBJ files without missing material errors.
- [ ] `ENEMY_V0134_ReadabilityPolishMaterials.mtl` is present and material names remain readable.
- [ ] No mesh colliders are generated during import.
- [ ] The package can be reviewed without rigging, animation, gameplay code, or scene builder changes.
- [ ] The PNG swatch sheet is visible in the Project window for quick reference.
- [ ] The manifest JSON in `Metadata/` matches the planning manifest.

## Silhouette Gates

- [ ] Scrapper reads as short, compact, hunched melee before color/material review.
- [ ] Lancer reads as tall, thin, ranged, and forward-facing before color/material review.
- [ ] Bulwark reads as broad, shielded, and defensive before color/material review.
- [ ] Warden reads as tall, caged, and command-oriented before color/material review.

## Tell Gates

- [ ] Scrapper keeps one circular cutter tell and one block hammer tell.
- [ ] Lancer keeps a clean uninterrupted +Z lance direction line.
- [ ] Lancer cyan rings read as pre-fire charge, not weak-point targets.
- [ ] Bulwark side weak lamps remain visible around the shield mass.
- [ ] Bulwark hammer tell remains visible past the shield edge.
- [ ] Warden overhead cyan coils read as command/charge language, not a second lance.

## Material Gates

- [ ] Dark silhouette material remains the dominant armor/silhouette value.
- [ ] Brass is used for readable edges, cage ribs, trims, and retainers.
- [ ] Copper is used for pressure transfer and weapon-barrel language.
- [ ] Cream enamel is limited to face/identity plates.
- [ ] Amber furnace eyes are distinct from gold weak-point lamps.
- [ ] Red tanks read as pressure vessels, not guaranteed pickups or explosive loot.
- [ ] Cyan/blue is reserved for Lancer and Warden bolt/command charge cues.
- [ ] Dim shutdown pieces read as inactive debris.

## Stop Conditions

Stop integration and return to staging if any of these happen:

- Any enemy loses identity in flat lighting.
- Weak-point lamps merge visually with furnace eyes, muzzle fire, or general decoration.
- The Lancer becomes bulky enough to read as melee.
- The Warden reads as only a taller Lancer.
- The Bulwark shield hides every readable damage affordance.
- Cyan/blue appears on Scrapper or Bulwark attack tells.
- Shutdown fragments read as pickups, ammo, or interactables.