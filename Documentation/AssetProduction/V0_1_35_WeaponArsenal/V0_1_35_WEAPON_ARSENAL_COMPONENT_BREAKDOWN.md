# V0.1.35 Weapon Arsenal Component Breakdown

Visual target: heavy brassworks/steampunk, designed as integration-ready staging geometry rather than concept-only sketches. All assets are proxy meshes with named OBJ objects for Unity selection, material replacement, socketing, and LOD cleanup.

## Pressure Pistol Final Component Pack
- Coils: 	op_recoil_coil_01..03 define the spring/pressure silhouette above the receiver.
- Dials/gauges: cream_pressure_gauge_disc_left, lack_gauge_rim_left, gauge_needle_left.
- Barrels: arrel_heat_stained_main_tube, arrel_blackened_inner_bore, soot card at muzzle.
- Grip/frame: walnut grip, blackened trigger guard, split brass receiver frame.
- Valves/rivets/lamps: copper chamber, amber pressure lamp, polished rivet row.
- Wear/soot: oil_soot_muzzle_wear_card, polished rivets as edge-wear proxies.

## Steam Scattergun Final Component Pack
- Coils/lines: left/right copper bypass coils and lower steam expansion tank.
- Dials/gauges: rear cream pressure dial with brass rim.
- Barrels: triple heat-stained barrel cluster with soot smear at muzzle.
- Grip/stock: ribbed pump grip, walnut shoulder stock, brass butt plate.
- Frames/plates: blackened receiver box with brass side plates.
- Wear/soot: muzzle smear and polished side rivets.

## Pressure Cartridge Family
- Five variants: pistol short cell, scattergun slug canister, ruptured empty, redband high-pressure, display cutaway.
- Shared components: copper body, brass cap, blackened base, front label band.
- Gameplay readability: red band for dangerous/high value, cream label for standard, rack rail for cabinet dressing.

## Wall Weapon Display Frame
- Brass frame rails, iron backplate, paired weapon hooks, cream labels, amber lamps, polished screws.
- Intended to stage mounted weapons without requiring scene edits in this batch.

## Ammo Cabinet / Vending Prop
- Iron shell, brass front frame, green stock window, amber low-pressure window, red empty flag, coin valve wheel, gauge, side pressure pipes, soot floor panel.
- Recommended sockets: pickup_spawn_front, coin_valve_interact, status_window_state.

## Future Alt Lightning Lance Silhouette
- Alternate future weapon direction only: long brass spine, twin arc prongs, amber accumulator, copper induction coil, route green charge window, red overload switch.
- Purpose: widen v0.1.35 arsenal silhouette range without changing current weapon definitions.
