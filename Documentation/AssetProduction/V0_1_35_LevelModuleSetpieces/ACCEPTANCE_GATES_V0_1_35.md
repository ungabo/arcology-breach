# v0.1.35 Level Module Setpiece Acceptance Gates

## Package Gate

- [x] Generated in Unity batchmode, no Blender or external DCC.
- [x] Staged prefabs, materials, metadata, and preview sheets are inside owned package scope.
- [x] Documentation and render outputs are inside owned documentation scopes.
- [ ] Main integration lane reviews route clearances before any scene placement.
- [ ] Main integration lane decides whether staged proxies become final prefabs or remain lookdev references.

## Visual Gate

- [ ] Corridor kit reads as compact brassworks dungeon, not clean sci-fi.
- [ ] Pressure door and vault door read as gear/pressure machines from first-person approach distance.
- [ ] Pipe gallery supports valve objectives without implying extra interactables.
- [ ] Furnace alcove frames heat/hazard beats without washing out enemy silhouettes.
- [ ] Catwalk rail and trim kit add readable structure without becoming visual clutter.
- [ ] Lighting fixtures support amber gaslight mood without replacing gameplay-critical route colors.

## Route Gate

- [ ] No placement root overlaps transition, pickup, prompt, hazard, enemy spawn, boss, secret, or final-exit ownership volumes.
- [ ] Door apertures maintain at least `2.4m` readable height and route-specific width.
- [ ] Wall/ceiling dressing does not reduce player route clearance.
- [ ] Flush floor trim remains non-colliding or less than `0.03m` high.
- [ ] Rails are placed only at true edges/perimeters and do not snag the player.

## Windows Performance Gate

- [ ] Static batching or prefab combining is used for repeated trim and corridor shell pieces.
- [ ] Repeated rivets/clamps/lamp cage details use instancing or baked geometry where appropriate.
- [ ] LOD1 and LOD2 are authored before broad multi-level placement.
- [ ] Runtime point lights are budgeted by room; proof lights are not blindly copied into scenes.
- [ ] First-person smoke pass checks frame pacing in Level03, Level04, and Level05 first.
