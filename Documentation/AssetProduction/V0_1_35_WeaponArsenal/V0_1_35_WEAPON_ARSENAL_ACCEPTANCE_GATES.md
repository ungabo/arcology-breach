# V0.1.35 Weapon Arsenal Acceptance Gates

## Import
- Import OBJ meshes from Assets/_Project/ArtStaging/V0_1_35_WeaponArsenal/Meshes/ with scale factor 1.0.
- Keep hierarchy/object names during import for component selection.
- Resolve materials through V0135WA_Brassworks_ArsenalPalette.mtl or replace with final Unity shader materials from the recipe JSON.

## Integration
- Pressure pistol and steam scattergun must fit first-person camera crops before final prefab conversion.
- Pickups require trigger volumes larger than visible geometry.
- Wall display and ammo cabinet must use primitive collision only.
- Red/green/amber status materials must map consistently: green stocked/usable, amber low/charged, red empty/danger.

## LOD And Collision
- LOD0: current staged mesh with decorative rivets/coils/gauges.
- LOD1: remove rivets, gauge needles, small coils, and soot cards.
- LOD2: merge to silhouette boxes/cylinders preserving weapon class readability.
- Collision: box/capsule primitives; never mesh collider for coils, pipes, lamps, rivets, or labels.

## Visual Proof
- Preview sheets live under Documentation/ConceptRenders/V0_1_35_WeaponArsenal/.
- Unity-only lookdev policy is preserved: the staging assets are Unity-readable and no Blender/DCC files are included.
