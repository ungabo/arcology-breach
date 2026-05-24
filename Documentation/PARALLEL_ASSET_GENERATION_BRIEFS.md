# Brassworks Breach - Parallel Asset Generation Brief Library

Timestamp: 2026-05-23 21:38 -04:00

Side-agent scope: this document only. It is written so final asset generation can begin in parallel without changing Unity scenes, scripts, prefabs, materials, package imports, or existing roadmap/status files.

## North-Star Target

`Brassworks Breach` needs stylized steampunk silhouettes with high-fidelity PBR surface detail. Assets should look hand-built, pressure-driven, soot-stained, worn, and mechanical. The visual language is brass, copper, blackened iron, wet stone, soot brick, walnut, chipped cream enamel, warped glass, gauges, valves, rivets, flywheels, pipe bundles, furnace glow, steam, oil, and Victorian industrial machinery.

Do not generate cyberpunk assets. Avoid neon city styling, black chrome, holograms, clean sci-fi corridors, smooth robots, sleek electronic weapons, and generic futuristic decals.

The goal is AAA-quality presentation for the Windows build while preserving readable shapes and planned reductions for Android, WebGL, SteamVR, and Meta Quest.

## Shared Import Contract

Use this contract for every brief unless the brief overrides it.

| Field | Standard |
| --- | --- |
| Unity scale | `1 Unity unit = 1 meter` |
| Render pipeline target | URP-compatible PBR materials unless project settings require a different standard |
| Texture maps | `BaseColor`, `Normal`, packed `Mask` where practical, optional `Emission`, optional `Height` only for approved hero surfaces |
| Mask packing | Red = metallic, Green = roughness, Blue = ambient occlusion, Alpha = optional detail mask |
| Color space | sRGB for base color and emission color; linear for masks, normal, roughness, metallic, AO, height |
| Mesh transforms | Applied before export; no negative scale |
| Pivots | At placement or interaction point: floor/base center for props, grid edge for walls, hand grip for weapons, root footprint center for enemies |
| Collisions | Simple `COL` meshes, boxes, capsules, or triggers; never use high-detail render meshes for common gameplay collision |
| LODs | LOD0 for hero, LOD1 around 50 to 60 percent triangles, LOD2 around 20 to 30 percent, optional LOD3/impostor for large static pieces |
| Platform variants | Windows source quality plus Android/WebGL/Quest downscaled textures, simplified material count, reduced VFX/audio density |
| File formats | `fbx` for models, `png` or `tga` for textures, `wav` source audio plus Unity-compressed import, sprite sheets as `png` with metadata |
| Staging path | Do not import yet unless requested; intended future root is `Assets/_Project/Art/` |

## Naming Rules

Use `BBW` for Brassworks Breach World. Avoid spaces, vendor names, `final`, `new`, `copy`, or ambiguous abbreviations.

| Type | Pattern | Example |
| --- | --- | --- |
| Texture | `T_BBW_<Asset>_<Map>_<Size>` | `T_BBW_AgedBrass_BaseColor_2048` |
| Material | `Mat_BBW_<Asset>_<Variant>` | `Mat_BBW_AgedBrass_A` |
| Static mesh | `SM_BBW_<Category>_<Name>_<Variant>` | `SM_BBW_CorridorWall_4m_A` |
| Skeletal mesh | `SK_BBW_<Enemy>_<Variant>` | `SK_BBW_Scrapper_A` |
| Prefab | `PF_BBW_<Category>_<Name>_<Variant>` | `PF_BBW_PressureGate_4m_A` |
| Collision | `COL_BBW_<Name>_<Variant>` | `COL_BBW_CorridorWall_4m_A` |
| Animation | `AN_BBW_<Actor>_<State>_<Variant>` | `AN_BBW_Scrapper_AttackTell_A` |
| VFX | `VFX_BBW_<Purpose>_<Variant>` | `VFX_BBW_ScattergunBlast_A` |
| Audio | `AUD_BBW_<Family>_<Cue>_<Variant>` | `AUD_BBW_Scattergun_Fire_A` |

## First 10 Assets To Produce

These should be generated first because they unlock many downstream art tasks and can be accepted without waiting for new gameplay code.

1. `MAT-FOUND-001` - Aged brass PBR tileable.
2. `MAT-FOUND-002` - Riveted blackened iron PBR tileable.
3. `MAT-FOUND-003` - Soot brick and wet oil-stone PBR tileables.
4. `TRIM-ENV-001` - Brassworks industrial trim sheet for rivets, bands, panel lips, gauge rims, and pipe clamps.
5. `GEO-MOD-001` - 4m modular corridor wall/floor/ceiling test kit using the trim sheet.
6. `PROP-PIPE-001` - Reusable pipe/valve/gauge kit.
7. `DECAL-ENV-001` - Soot, oil, scorch, leaks, labels, arrows, and warning stamp decal atlas.
8. `WPN-001` - Pressure Pistol final first-person and world mesh package.
9. `WPN-002` - Steam Scattergun final first-person, world, and display-stand mesh package.
10. `ENEMY-001` - Scrapper final model, rig, attack-tell, and shutdown package.

## Brief Format

Each brief includes:

- Target use.
- Art direction.
- Required maps or files.
- LOD and collision notes.
- Platform variants.
- Unity naming.
- Acceptance criteria.
- Likely generation method.

## Material Tileables

### MAT-FOUND-001 - Aged Brass Tileable

- Target use: shared metal for weapons, gauges, valve wheels, trim, pickup shells, service lift accents, and readable interactable affordances.
- Art direction: warm aged brass with worn bright edges, oily fingerprints, tiny dents, hand-polished contact points, darker grime in recesses, and no neon glow.
- Required maps or files: 2048 Windows `BaseColor`, `Normal`, `Mask`, optional `Height`; 1024 and 512 downscale exports; Unity material preset.
- LOD and collision notes: material only; create a 1m material preview cube/sphere in staging later, no gameplay collision.
- Platform variants: Windows 2048 source, Android/WebGL/Quest 1024 or 512, SteamVR 2048 for close inspectable objects.
- Unity naming: `T_BBW_AgedBrass_BaseColor_2048`, `T_BBW_AgedBrass_Normal_2048`, `T_BBW_AgedBrass_Mask_2048`, `Mat_BBW_AgedBrass_A`.
- Acceptance criteria: tiles cleanly at 1m and 2m scale, reads as brass in neutral and warm light, roughness variation visible but not noisy, downscaled 512 version still reads as worn brass.
- Likely generation method: Substance Designer, Material Maker, Blender procedural bake, or AI texture generation followed by manual map cleanup and normal/mask validation.

### MAT-FOUND-002 - Riveted Blackened Iron Tileable

- Target use: wall panels, gates, enemy armor, furnace machinery, heavy floor plates, Bulwark armor, Warden shell.
- Art direction: soot-dark iron with chipped edges, rivet rows, subtle heat staining, scrape marks, and oily grime; should feel heavy and old.
- Required maps or files: 2048 `BaseColor`, `Normal`, `Mask`, optional `Emission` variant for hot furnace seams; 1024 and 512 reductions.
- LOD and collision notes: material only; rivets may be normal-map detail for common surfaces, geometry only for hero trim.
- Platform variants: common 1024 on Windows modules, 512 on mobile/WebGL/Quest, 2048 only for hero gate or boss armor.
- Unity naming: `T_BBW_RivetedIron_BaseColor_2048`, `T_BBW_RivetedIron_Normal_2048`, `T_BBW_RivetedIron_Mask_2048`, `Mat_BBW_RivetedIron_A`.
- Acceptance criteria: rivet pattern aligns on modular panels, normals are not inverted, roughness prevents plastic shine, repeated corridors do not show obvious texture repetition at 4m spans.
- Likely generation method: procedural material with authored rivet masks and edge-wear pass.

### MAT-FOUND-003 - Soot Brick And Wet Oil-Stone Tileables

- Target use: dungeon walls, service corridors, intake floors, wet maintenance chambers, secret cache alcoves.
- Art direction: compact Victorian utility brick and rough dark stone; damp oil sheen in patches, soot buildup near ceilings, chipped mortar, and worn traffic paths.
- Required maps or files: separate brick and stone texture sets, each with 2048 `BaseColor`, `Normal`, `Mask`, optional puddle/detail mask; 1024 and 512 variants.
- LOD and collision notes: material only; surface relief should not imply blocking collision.
- Platform variants: Windows 2048 for primary tileables, Android/WebGL/Quest 1024 or 512, SteamVR 1024/2048 depending on close wall inspection.
- Unity naming: `Mat_BBW_SootBrick_A`, `Mat_BBW_OilStoneWet_A`, `T_BBW_SootBrick_*_2048`, `T_BBW_OilStoneWet_*_2048`.
- Acceptance criteria: loops cleanly, reads as dungeon industrial rather than medieval castle, wet areas do not look like glossy plastic, floor downscale keeps path readability.
- Likely generation method: procedural tileables with hand-authored grime masks and optional decal overlay set.

### MAT-FOUND-004 - Walnut, Enamel, Glass, Gasket, And Furnace Glow Materials

- Target use: weapon grips, tool handles, gauges, signs, lamps, seals, hoses, furnace eyes, heat vents, warning lenses.
- Art direction: dark scratched walnut; chipped cream enamel; slightly warped gauge glass; dark rubberized leather gasket; amber-orange furnace glow that feels gaslit and hot, not digital.
- Required maps or files: 1024 or 2048 material sets, emission mask for furnace/glass lamps, transparency settings for glass only, material presets.
- LOD and collision notes: material only; transparent glass should be reserved for gauges and lamps.
- Platform variants: glass and emission simplified on Android/WebGL/Quest; reduce transparent layers in VR.
- Unity naming: `Mat_BBW_WalnutDark_A`, `Mat_BBW_CreamEnamelChipped_A`, `Mat_BBW_GaugeGlassWarped_A`, `Mat_BBW_RubberGasket_A`, `Mat_BBW_FurnaceGlow_Amber_A`.
- Acceptance criteria: all five materials remain distinct under warm gaslight, enamel labels support readable decals, furnace glow does not bloom into combat silhouettes.
- Likely generation method: procedural materials plus hand-painted masks for chips and glow.

## Trim Sheets

### TRIM-ENV-001 - Brassworks Industrial Trim Sheet

- Target use: modular walls, doors, gate frames, service lifts, weapon ribs, pipe clamps, enemy armor seams, prop frames.
- Art direction: broad readable bands with rivets, beveled brass, black iron strips, cream label plates, red warning stripes, green service strips, gauge rims, and small repeatable screw heads.
- Required maps or files: 2048 trim sheet `BaseColor`, `Normal`, `Mask`, UV guide image, texel-density note, 1024/512 variants.
- LOD and collision notes: trim sheet only; matching low-poly mesh bevels should be optional, not required for mobile.
- Platform variants: 2048 Windows/SteamVR, 1024 Android/WebGL/Quest, shared material atlas where possible.
- Unity naming: `T_BBW_IndustrialTrim_BaseColor_2048`, `T_BBW_IndustrialTrim_Normal_2048`, `T_BBW_IndustrialTrim_Mask_2048`, `Mat_BBW_IndustrialTrim_A`.
- Acceptance criteria: works on straight and corner modules, includes enough non-repeating bands for 2m/4m/8m kit pieces, red/green bands match gameplay color language.
- Likely generation method: authored trim sheet in Blender/Substance with procedural normal baking.

### TRIM-ENV-002 - Pipe, Gauge, And Machinery Trim Sheet

- Target use: pipes, gauges, valve plates, boiler bands, pressure tanks, Bellows Node parts, Warden machinery.
- Art direction: circular gauge rims, valve tick marks, pipe bands, pressure labels, blackened bolts, polished hand-contact wear, and small enamel readouts.
- Required maps or files: 2048 trim sheet maps, gauge face sub-atlas, alpha/mask for labels if needed.
- LOD and collision notes: trim sheet only; gauge needles and valve handles can be separate mesh pieces.
- Platform variants: downscale labels must still read as shape/color even if tiny text becomes decorative.
- Unity naming: `T_BBW_MachineryTrim_BaseColor_2048`, `Mat_BBW_MachineryTrim_A`.
- Acceptance criteria: supports at least three gauge sizes, two valve wheel sizes, two pipe diameters, and one boiler band style.
- Likely generation method: vector-authored gauge/label layout converted to PBR atlas with normal/mask layers.

## Modular Walls, Floors, And Corridors

### GEO-MOD-001 - Corridor Wall/Floor/Ceiling Kit

- Target use: replace greybox corridors across Level01 to Level05 with snap-aligned steampunk dungeon modules.
- Art direction: soot brick base, riveted iron braces, copper pipe runs, brass trim, occasional service lamps, oil-dark stone floors, and compact utility geometry that preserves movement readability.
- Required maps or files: FBX meshes for 2m, 4m, 8m wall sections; floor slab, grate slab, ceiling pipe panel, inside/outside corner, arch frame; material assignments to `Mat_BBW_IndustrialTrim_A`, `Mat_BBW_SootBrick_A`, `Mat_BBW_OilStoneWet_A`; collision meshes; preview renders.
- LOD and collision notes: LOD0/LOD1/LOD2 for repeated pieces; simple box collision for walls/floors; pipe detail mostly non-blocking visual geometry.
- Platform variants: Windows modules with bevels and detail pipes; Android/WebGL/Quest variants remove small loose parts, combine meshes, and rely more on trim normals.
- Unity naming: `SM_BBW_CorridorWall_4m_A`, `SM_BBW_CorridorFloor_4m_A`, `SM_BBW_CeilingPipePanel_4m_A`, `PF_BBW_CorridorWall_4m_A`, `COL_BBW_CorridorWall_4m_A`.
- Acceptance criteria: snaps to a 1m or 2m grid, corridor widths stay playable at 3.5m to 5m, no z-fighting at corners, no silhouette clutter that hides enemies, baked-lighting UVs are clean.
- Likely generation method: Blender modular kit using trim-sheet UVs and procedural bevels, then export FBX with Unity scale validation.

### GEO-MOD-002 - Room, Corner, Cover, And Landmark Kit

- Target use: combat rooms, repair bays, foundry arenas, boss cover, landmark silhouettes, secret alcoves.
- Art direction: chunky industrial forms: boiler stacks, waist-high tool benches, pipe manifolds, furnace pillars, gear housings, pressure tanks, service alcoves, and riveted cover that looks functional.
- Required maps or files: FBX static meshes, trim-sheet UVs, LODs, simple collision meshes, optional emissive lamp material slots.
- LOD and collision notes: cover collision must match gameplay silhouette closely enough for raycasts and movement; decorative gauges/bolts should not block movement.
- Platform variants: combined static clusters for Android/WebGL; reduced transparent glass/emission for Quest.
- Unity naming: `SM_BBW_CoverBoiler_2m_A`, `SM_BBW_RepairBench_3m_A`, `SM_BBW_FurnacePillar_A`, `PF_BBW_CoverBoiler_2m_A`.
- Acceptance criteria: cover height and footprint are documented, pieces do not trap the player, landmark pieces are recognizable at 20m, LOD transitions preserve cover silhouettes.
- Likely generation method: Blender kitbash from approved material/trim library, with blockout collision first.

## Pipes, Valves, And Gauges

### PROP-PIPE-001 - Pipe Bundle Kit

- Target use: corridor dressing, route guidance, pressure machinery, hidden path clues, wall/ceiling/floor utility systems.
- Art direction: copper and black iron pipes with brass clamps, soot-dark joints, condensation streaks, occasional green service-lit route pipes and red danger pipes.
- Required maps or files: straight pipes in 1m/2m/4m/8m lengths, elbows, T-junctions, floor risers, wall clamps, pipe brackets, cap ends, simple collision options.
- LOD and collision notes: most pipes are non-blocking visual meshes; large floor pipes need simple capsule/box collision or explicit no-collision tags; LOD1/LOD2 for repeated bundles.
- Platform variants: use atlased materials and combined pipe bundles for mobile/WebGL/Quest; reduce small clamps.
- Unity naming: `SM_BBW_PipeStraight_4m_A`, `SM_BBW_PipeElbow_90_A`, `SM_BBW_PipeBundleWall_4m_A`, `PF_BBW_PipeBundleWall_4m_A`.
- Acceptance criteria: pipes align cleanly on grid, color-coded route cues are available, no accidental gameplay blockers, modules support visible pressure flow from room to room.
- Likely generation method: Blender procedural curves converted to mesh with shared trim/material slots.

### PROP-MECH-001 - Valve Wheel And Gauge Set

- Target use: interactables, locked-object feedback, world storytelling, HUD/diegetic UI reference, Bellows Node/Warden machinery.
- Art direction: oversized readable valve silhouettes, cream enamel gauge faces, brass bezels, black needles, red danger zones, green restored-pressure zones, chipped labels.
- Required maps or files: small/medium/large valve wheels, wall gauges in three sizes, gauge face texture atlas, optional needle mesh, interaction socket marker, LODs.
- LOD and collision notes: interactable valve uses trigger/collider around wheel; gauges usually no collision; valve spokes must remain readable in LOD1.
- Platform variants: Android/WebGL/Quest use simplified wheel spokes and lower-res gauge atlas; VR keeps larger physical dimensions for close interaction.
- Unity naming: `SM_BBW_ValveWheel_M_A`, `SM_BBW_PressureGauge_M_A`, `PF_BBW_ValveInteractable_M_A`, `T_BBW_GaugeFaces_BaseColor_1024`.
- Acceptance criteria: player can identify valve/gauge purpose at normal corridor distance, red/green pressure states are obvious, optional text is decorative rather than required.
- Likely generation method: Blender meshes plus vector gauge-face atlas and PBR material assignment.

## Signage And Decals

### DECAL-ENV-001 - Industrial Decal Atlas

- Target use: path guidance, warning language, lock hints, secret clues, oil/scorch/scratch layering, work orders, archive plaques.
- Art direction: chipped enamel signs, stamped brass tags, soot smears, oil leaks, hand-painted worker arrows, red pressure warnings, green service lift markers, amber gear-key markers.
- Required maps or files: atlas `BaseColor`, alpha/mask, normal where useful, decal placement guide, plain-text source list for labels.
- LOD and collision notes: decals have no collision; use large shapes and icons so gameplay meaning survives downscale.
- Platform variants: 2048 Windows atlas, 1024 WebGL/Android/Quest; group decals to reduce material count.
- Unity naming: `T_BBW_IndustrialDecals_BaseColor_2048`, `T_BBW_IndustrialDecals_Mask_2048`, `Mat_BBW_IndustrialDecal_A`.
- Acceptance criteria: includes at least 24 decals: oil, soot, scorch, leaks, scratches, rivet stains, red warning arrows, green lift arrows, amber gear markers, pressure lock labels, archive plaque frames, and secret clue marks.
- Likely generation method: vector/painted atlas with hand-authored alpha and procedural grime overlay.

### DECAL-SIGN-001 - Readable Sign And Plaque Set

- Target use: level wayfinding, lore plaques, warning panels, service lift destination plates, pressure gate instructions.
- Art direction: compact Victorian industrial typography on enamel/brass plates; text should be short, not lore-heavy, with icons and color doing most of the work.
- Required maps or files: sign atlas, individual text source, optional mesh plates, normal/mask maps, font/license note.
- LOD and collision notes: signs may be flat meshes or decals with no collision; VR variants require larger text or icon-first design.
- Platform variants: mobile/WebGL use simplified icon signs and fewer unique text plates; VR uses larger plaque geometry and less tiny text.
- Unity naming: `T_BBW_ServiceSigns_BaseColor_2048`, `SM_BBW_EnamelSign_Rect_A`, `PF_BBW_ServiceSign_Lift_A`.
- Acceptance criteria: major route signs are readable at 3m on Windows, icon meaning remains clear at 512 texture size, no cyberpunk UI language.
- Likely generation method: vector design and decal atlas bake.

## Pickups And Gameplay Props

### PROP-PICKUP-001 - Gear Key Final Package

- Target use: key objective pickup for locks and pressure gates.
- Art direction: brass clockwork key with gear teeth, central hub, thick shaft, readable bit, amber lamp glow, and worn edge polish.
- Required maps or files: world pickup mesh, plinth optional mesh, pickup VFX socket, audio cue reference, textures/material assignments, LODs, trigger/collision bounds.
- LOD and collision notes: trigger collider larger than mesh; LODs preserve gear/key silhouette; plinth collision simple cylinder/box.
- Platform variants: reduce spoke detail and use normal-map teeth for mobile/WebGL; VR scale should feel hand-sized and readable.
- Unity naming: `SM_BBW_GearKey_A`, `PF_BBW_GearKeyPickup_A`, `COL_BBW_GearKeyPickup_A`.
- Acceptance criteria: reads as a key without text, amber objective color is obvious, pickup silhouette remains readable while bobbing/spinning.
- Likely generation method: Blender modeled mesh with trim/material library and procedural pickup VFX later.

### PROP-PICKUP-002 - Health, Ammo, And Resource Pickup Family

- Target use: health vial, pressure cartridges, rivet bundles, boiler caps, secret cache rewards.
- Art direction: brass-and-glass medical vial with red fluid; bundled pressure cartridges with brass caps and iron nozzles; rivet bundle as tool/ammo; boiler caps as chunky pressure charges.
- Required maps or files: individual meshes, material assignments, icon/silhouette thumbnails, trigger bounds, LODs, pickup VFX socket.
- LOD and collision notes: trigger colliders only; LOD1 preserves color and broad shape; tiny straps/labels can collapse in LOD2.
- Platform variants: mobile/WebGL use combined atlas and simplified cartridges; VR keeps pickups physically readable and not too small.
- Unity naming: `PF_BBW_HealthVial_A`, `PF_BBW_PressureCartridgePack_A`, `PF_BBW_RivetBundle_A`, `PF_BBW_BoilerCapAmmo_A`.
- Acceptance criteria: health and ammo are unmistakably different at 5m, secret rewards feel desirable, no pickup requires reading text to understand.
- Likely generation method: Blender prop modeling plus shared PBR materials and simple atlas.

### PROP-CACHE-001 - Secret Cache Container Set

- Target use: optional reward containers and suspicious secret alcove dressing.
- Art direction: worker-maintained pressure lockboxes, coal bins, brass-latched tool chests, hidden pipe access panels, chalk marks from the Locked Shift.
- Required maps or files: closed/open mesh variants, hinge/latch pieces, simple collision, optional open animation, decal clue set.
- LOD and collision notes: box collision; open lid animation optional; LODs preserve silhouette and secret clue markings.
- Platform variants: mobile/WebGL use static open/closed states and lower-resolution decals.
- Unity naming: `SM_BBW_SecretCache_Lockbox_A`, `PF_BBW_SecretCache_Lockbox_A`, `AN_BBW_SecretCache_Open_A`.
- Acceptance criteria: looks optional and rewarding, not mandatory objective; clue language is consistent across levels.
- Likely generation method: Blender hard-surface prop set plus decal atlas.

## Service Lifts And Pressure Gates

### GEO-GATE-001 - Pressure Gate Package

- Target use: locked progression gates, pressure-denial feedback, large mechanical route changes.
- Art direction: heavy riveted slab gate with gear-driven rails, red locked lamps, green restored-pressure lamps, large socket, pressure cylinders, chain or rack movement.
- Required maps or files: gate frame, moving slab, side rails, gear wheel, socket, gauge, lamp meshes, LODs, collision for closed/open states, animation clips, VFX/audio sockets.
- LOD and collision notes: closed gate collision is simple blocker; open state clears player path fully; moving parts need separate pivots; LODs preserve red/green lamp positions.
- Platform variants: mobile/WebGL reduce moving gear detail and emissive count; VR avoids fast near-face motion and heavy screen shake.
- Unity naming: `PF_BBW_PressureGate_4m_A`, `SM_BBW_PressureGate_Frame_4m_A`, `AN_BBW_PressureGate_Open_A`, `COL_BBW_PressureGate_4m_A`.
- Acceptance criteria: locked/open state is obvious from 10m, player cannot snag on threshold, animation reads as gear/pressure-driven, state colors follow red danger/green service language.
- Likely generation method: Blender modular mechanical assembly with authored pivot hierarchy and simple animation.

### GEO-LIFT-001 - Service Lift Package

- Target use: level exits, transitions, safe-zone landmarks, future vertical movement.
- Art direction: brass/iron cage lift with green service lamps, pulley drum, cable, gauge panel, chain rails, worn floor grate, and destination plate.
- Required maps or files: lift cage, platform, call box, pulley, chain/cable pieces, gate/door optional, destination signs, LODs, collision, activation animation, VFX/audio sockets.
- LOD and collision notes: floor/player collision simple and stable; side rails block player if used; LODs keep green service lamps visible.
- Platform variants: mobile/WebGL use static chain/cable detail and baked lighting; VR transition should support fade and stable forward orientation.
- Unity naming: `PF_BBW_ServiceLift_A`, `SM_BBW_ServiceLift_Cage_A`, `AN_BBW_ServiceLift_Activate_A`, `COL_BBW_ServiceLift_A`.
- Acceptance criteria: unmistakable as exit/progression object, safe color language is clear, collision has no edge snagging, destination sign can be swapped per level.
- Likely generation method: Blender kit assembly from pipe/trim/material library with simple activation animation.

## Weapons

### WPN-001 - Pressure Pistol Final Package

- Target use: starter first-person weapon, world pickup/reference prop, VR one-hand weapon.
- Art direction: compact brass-and-walnut pneumatic sidearm with short barrel, pressure tube, gauge near rear, crowned muzzle, side valve, pipe loop, worn iron receiver, and visible mechanical trigger.
- Required maps or files: first-person mesh, world mesh, low-detail pickup mesh, material set, LODs, grip socket, muzzle socket, vent socket, optional chamber/gauge needle bones, animation clips for idle/fire/burst/equip/check/dry-fire, preview renders.
- LOD and collision notes: first-person mesh does not need distance LOD but needs separated animatable parts; world mesh LOD0/LOD1/LOD2; trigger collider only for pickup/world use.
- Platform variants: Windows/SteamVR 2048 hero texture; Android/WebGL/Quest 1024 atlas and simplified gauge/valve detail; VR grip pose around handle with muzzle kept away from eye plane.
- Unity naming: `SM_BBW_PressurePistol_View_A`, `SM_BBW_PressurePistol_World_A`, `PF_BBW_PressurePistol_View_A`, `PF_BBW_PressurePistol_Pickup_A`, `AN_BBW_PressurePistol_Fire_A`.
- Acceptance criteria: silhouette reads in lower-right first-person view, gauge and pressure tube are visible, muzzle socket aligns with current projectile/VFX expectations, alternate fire can visibly vent pressure without reusing primary animation exactly.
- Likely generation method: authored Blender hard-surface model with trim/material library, baked normals, and simple rig/animation.

### WPN-002 - Steam Scattergun Final Package

- Target use: close-range breaching weapon, Level03 pickup, first-person viewmodel, VR two-hand weapon.
- Art direction: triple barrel cluster, brass top rib, walnut pump grip, rear pressure coil, valve wheel or pressure cap, shell rack, heavy clackable mechanism, display-stand pickup silhouette.
- Required maps or files: first-person mesh, world mesh, display stand mesh, shell rack pieces, LODs, material set, muzzle sockets for pellet cone and slug, vent socket, pump/coil/gauge animatable pieces, animation clips for idle/primary/pump/slug/equip/check/dry-fire/pickup presentation.
- LOD and collision notes: world pickup trigger collider only; display stand simple collision; LODs preserve triple-barrel silhouette and shell rack.
- Platform variants: Windows/SteamVR 2048 hero texture; Android/WebGL/Quest 1024, fewer loose shells, simplified coil; VR support-hand socket optional but documented.
- Unity naming: `SM_BBW_SteamScattergun_View_A`, `SM_BBW_SteamScattergun_World_A`, `SM_BBW_SteamScattergun_DisplayStand_A`, `PF_BBW_SteamScattergun_Pickup_A`, `AN_BBW_SteamScattergun_PrimaryFire_A`, `AN_BBW_SteamScattergun_SlugFire_A`.
- Acceptance criteria: distinct from Pressure Pistol at a glance, primary and slug modes have separate animation/VFX sockets, pickup stand reads as valuable optional power, does not block excessive screen space.
- Likely generation method: Blender authored model, baked high-to-low detail, hand-authored first-person animation, separate lower-detail pickup/display mesh.

### WPN-003 - Rivet Launcher Candidate Package

- Target use: future precision/armor weapon for Bulwark and Warden pressure phases.
- Art direction: compact industrial rivet tool converted into a weapon: rivet magazine drum, pressure ram, brass gauge, iron muzzle sleeve, heavy walnut shoulder/hand brace.
- Required maps or files: concept sheet, first-person and world mesh plan, LOD plan, sockets, animation list, tuning placeholder notes.
- LOD and collision notes: not needed for current scenes, but model should preserve magazine/ram silhouette.
- Platform variants: keep material count low and avoid complex transparent gauges.
- Unity naming: `SM_BBW_RivetLauncher_View_A`, `PF_BBW_RivetLauncher_Pickup_A`.
- Acceptance criteria: visually explains slow precision impact and does not overlap Scattergun silhouette.
- Likely generation method: concept plus later Blender hard-surface model after mechanics are approved.

## Mechanical Enemies

### ENEMY-001 - Scrapper Final Package

- Target use: baseline melee enemy and core readability standard for all machine threats.
- Art direction: small maintenance automaton with cutter arms, hunched boiler torso, piston elbows, furnace eye, chipped brass faceplate, oil leaks, black iron legs, and clear attack windup shape.
- Required maps or files: design sheet front/side/back, skeletal mesh, LOD0/LOD1/LOD2, material set, rig, collider plan, nav footprint, animations for idle/chase/attack tell/attack release/hit/stagger/shutdown, VFX sockets, audio socket notes.
- LOD and collision notes: hit colliders use capsule/box regions; cutter arms should not be tiny weak points unless gameplay supports it; LODs preserve cutter silhouette and glowing eye.
- Platform variants: Windows/SteamVR 2048 or 1024 textures; Android/WebGL/Quest 1024 or 512, simplified pistons/gears, lower animation bone count if needed.
- Unity naming: `SK_BBW_Scrapper_A`, `PF_BBW_Scrapper_A`, `AN_BBW_Scrapper_AttackTell_A`, `COL_BBW_Scrapper_Hitbox_A`.
- Acceptance criteria: role reads as melee within 1 second, attack tell visible in corridors, shutdown pose clearly dead, collision matches targetable mass, no organic monster language.
- Likely generation method: Blender/Maya mechanical rig with authored animations and PBR texture bake.

### ENEMY-002 - Lancer Final Package

- Target use: ranged pressure unit for lanes, bridges, and mixed encounters.
- Art direction: tall thin valve-rifle security frame with tripod or narrow legs, long pressure lance, charging gauge, shoulder boiler, red-orange warning glow before fire, and visible recoil valve.
- Required maps or files: skeletal mesh, LODs, rifle/lance socket, projectile muzzle socket, charge lamp material, animations for idle/aim/charge/fire/recover/hit/shutdown.
- LOD and collision notes: collider body narrow but fair; weapon barrel must remain visible in LOD1; projectile socket alignment critical.
- Platform variants: simplify thin mechanical rods for mobile/WebGL; VR keeps charge tell large and slow enough to read.
- Unity naming: `SK_BBW_Lancer_A`, `PF_BBW_Lancer_A`, `AN_BBW_Lancer_Charge_A`, `AN_BBW_Lancer_Fire_A`.
- Acceptance criteria: ranged role reads at 15m, charge tell is distinct from idle, pressure bolt origin matches barrel, shutdown does not leave misleading active glow.
- Likely generation method: authored mechanical rig and animation, with emissive material state variants.

### ENEMY-003 - Bulwark Final Package

- Target use: heavy blocker and arena repositioning threat.
- Art direction: furnace-plated riot machine with thick boiler belly, hammer or shield arms, heat-stained iron plates, glowing furnace seams, pressure vents, and slow dangerous windups.
- Required maps or files: skeletal mesh, armor material set, LODs, hitbox/collider plan, animations for idle/advance/windup/slam/stagger/hit/shutdown, VFX sockets for vents and furnace core.
- LOD and collision notes: wide simple colliders; LODs preserve heavy mass and windup arm silhouette; avoid small weak spots unless gameplay explicitly uses them.
- Platform variants: mobile/WebGL reduce armor plate count, use normal maps for rivets, lower VFX emission; VR avoids huge near-player opacity bursts.
- Unity naming: `SK_BBW_Bulwark_A`, `PF_BBW_Bulwark_A`, `AN_BBW_Bulwark_SlamTell_A`, `AN_BBW_Bulwark_Shutdown_A`.
- Acceptance criteria: heavy role reads instantly, attack windup gives clear response time, player can read alive/stagger/dead states, material feels furnace-hot without sci-fi glow.
- Likely generation method: Blender/Maya rigged hard-surface model with authored slam/stagger animation.

### ENEMY-004 - Bellows Node Final Package

- Target use: stationary support hazard that pulses pressure and boosts nearby machines.
- Art direction: bolted wall/floor machine with accordion bellows, pressure tanks, brass manifold, rotating governor, pulse gauge, red-orange danger ring, steam exhaust stacks.
- Required maps or files: static or lightly rigged mesh, LODs, pulse animation, shutdown animation/state, range-ring VFX socket, boost output socket, material states for idle/pulse/shutdown.
- LOD and collision notes: collision should match central machine body, not pulse radius; LODs keep bellows and pulse gauge readable.
- Platform variants: mobile/WebGL/Quest use lower-density bellows folds and simple pulse sprite; VR pulse ring should be readable on floor/world, not screen-space.
- Unity naming: `SM_BBW_BellowsNode_A` or `SK_BBW_BellowsNode_A`, `PF_BBW_BellowsNode_A`, `AN_BBW_BellowsNode_Pulse_A`.
- Acceptance criteria: support role is visible before damage, pulse timing can sync to VFX/audio, shutdown clearly disables it, no player-snagging collision.
- Likely generation method: Blender model with simple animated bellows parts and VFX socket layout.

### ENEMY-005 - Governor Warden Final Package

- Target use: Level05 boss guardian and climax enemy.
- Art direction: large assembled pressure governor machine with brass crown regulator, furnace heart, rotating pressure cannon, heavy service-frame legs, red/orange enrage glow, green shutdown relief vents, and readable boss landmarks.
- Required maps or files: design sheet, skeletal mesh, LOD0/LOD1/LOD2/optional LOD3, boss material states, rig, collider plan, animations for idle/stomp tell/stomp release/pressure bolt/enrage/hit/shutdown, sockets for cannon, feet, vents, furnace heart, boss death burst.
- LOD and collision notes: boss hitbox simple and fair; stomp area separate from mesh; LODs preserve crown/cannon/heart silhouette; shutdown pose cannot block exit route unexpectedly.
- Platform variants: Windows/SteamVR 2048 textures; Android/WebGL/Quest 1024/512 and fewer moving secondary gears; VR requires conservative scale and comfort-safe VFX.
- Unity naming: `SK_BBW_GovernorWarden_A`, `PF_BBW_GovernorWarden_A`, `AN_BBW_GovernorWarden_StompTell_A`, `AN_BBW_GovernorWarden_Enrage_A`, `AN_BBW_GovernorWarden_Shutdown_A`.
- Acceptance criteria: boss reads as unique final guardian from 25m, attacks are physically readable, enrage state is clear but not blinding, exit objective remains visible after defeat.
- Likely generation method: authored boss model/rig in Blender/Maya with staged concept approval before final bake.

### ENEMY-006 - Boiler Tick Future Scout

- Target use: optional small pressure scout for swarm encounters and early variety.
- Art direction: squat clockwork pressure tank with four or six small legs, gauge eye, skittering pistons, rupture valve, and amber/red pressure state.
- Required maps or files: concept sheet, small rigged mesh, LODs, scuttle/idle/rupture/shutdown animations, compact collider plan.
- LOD and collision notes: collider larger than tiny legs for fair targeting; LODs preserve tank body and state color.
- Platform variants: low bone count and simple silhouette for all platforms.
- Unity naming: `SK_BBW_BoilerTick_A`, `PF_BBW_BoilerTick_A`.
- Acceptance criteria: distinct from Scrapper, readable as fast/small, no tiny target frustration.
- Likely generation method: quick Blender rig after core roster finalizes.

## VFX Sprites And Flipbooks

### VFX-SET-001 - Steam, Smoke, Sparks, Heat, And Pressure Atlas

- Target use: shared VFX sprites for weapons, impacts, hazards, pickups, gates, lifts, machine hit/death, Bellows Node, and Warden.
- Art direction: warm white steam, soot smoke, brass sparks, oil flecks, heat shimmer masks, pressure rings, furnace embers, and short-lived industrial bursts.
- Required maps or files: sprite atlas or flipbook sheets for steam puffs, spark streaks, smoke wisps, pressure rings, heat masks, oil flecks, ember bursts; alpha channels; normal/distortion masks if used; low/medium/high density notes.
- LOD and collision notes: VFX have no collision; particle count tiers required; effects must be pooled-friendly and short-lived.
- Platform variants: Windows medium/high density, Android/WebGL/Quest low density and smaller atlases, VR lower opacity and no full-screen flashes.
- Unity naming: `T_BBW_VFX_SteamPuffs_8x8_1024`, `T_BBW_VFX_Sparks_1024`, `T_BBW_VFX_PressureRings_1024`, `VFX_BBW_SteamVent_Burst_A`.
- Acceptance criteria: alpha edges clean, readable over dark and warm backgrounds, does not obscure first-person view during repeated fire, low tier still communicates gameplay state.
- Likely generation method: procedural VFX texture generation, Blender/Houdini/EmberGen flipbooks, or AI sprite generation with alpha cleanup and Unity particle staging later.

### VFX-SET-002 - Weapon And Pickup Effect Families

- Target use: Pressure Pistol muzzle/impact/burst, Scattergun blast/slug/pickup, future Rivet Launcher, health/ammo/key pickups.
- Art direction: compact pressure snaps, brass spark cones, steam collars, visible slug spear, amber gear-key glint, red health pulse, brass cartridge pressure bloom.
- Required maps or files: named VFX prefab specs, sprites from shared atlas, color ramp textures, socket alignment notes, low/medium/high tier settings.
- LOD and collision notes: no collision; first-person VFX constrained to avoid screen blockage; impact decals optional.
- Platform variants: reduced particle count and opacity on Android/WebGL/Quest/VR; no expensive distortion unless approved.
- Unity naming: `VFX_BBW_Pistol_MuzzleSnap_A`, `VFX_BBW_Pistol_PressureBurst_A`, `VFX_BBW_Scattergun_BlastCone_A`, `VFX_BBW_Scattergun_SlugSpear_A`, `VFX_BBW_Pickup_GearKey_A`.
- Acceptance criteria: each weapon mode reads differently within 0.2 seconds, pickup effects match item color language, repeated fire remains readable.
- Likely generation method: Unity particle prefabs later using generated atlas, with sprite/flipbook production now.

### VFX-SET-003 - Enemy And Environment Effect Families

- Target use: Scrapper hit/death, Lancer charge, Bulwark slam, Bellows pulse/boost, Warden enrage/shutdown, steam hazards, furnace heat fields, gate/lift activation.
- Art direction: mechanical pressure pulses, hot vents, directional sparks, oil flecks, furnace embers, floor pressure rings, service-green relief bursts.
- Required maps or files: sprite/flipbook atlas additions, effect timing charts, socket requirements per enemy/prop, platform density tiers.
- LOD and collision notes: VFX area must match gameplay hazard radius where applicable; avoid misleading rings when damage is inactive.
- Platform variants: low-opacity VR, low-overdraw mobile/WebGL, no large transparent stacks in narrow corridors.
- Unity naming: `VFX_BBW_MachineHit_Sparks_A`, `VFX_BBW_BellowsNode_Pulse_A`, `VFX_BBW_Warden_Shutdown_A`, `VFX_BBW_Gate_Open_A`, `VFX_BBW_Lift_Activate_A`.
- Acceptance criteria: effect state matches gameplay state, tells are visible before damage, shutdown/death does not hide remaining enemies, color language remains consistent.
- Likely generation method: generated flipbooks plus later Unity particle prefab implementation.

## Audio Families

### AUD-SET-001 - Weapons Audio Family

- Target use: Pressure Pistol, Steam Scattergun, future Rivet Launcher, empty clicks, pickup acquisition, weapon switch/equip.
- Art direction: pneumatic snaps, brass latch clacks, valve ticks, steam vents, pipe resonance, short mechanical transients, and tactile low-mid impacts without muddy bass.
- Required maps or files: dry source WAV layers, mixed Unity-ready WAV clips, metadata sheet with cue names, loop flags if any, loudness notes, mono/stereo designation.
- LOD and collision notes: audio has no collision; one-shots should be mono/spatial except UI reinforcements; keep tails short.
- Platform variants: Windows/SteamVR higher quality Vorbis, Android/WebGL/Quest shorter/compressed clips, optional layer reductions.
- Unity naming: `AUD_BBW_Pistol_Fire_A`, `AUD_BBW_Pistol_Burst_A`, `AUD_BBW_Scattergun_Fire_A`, `AUD_BBW_Scattergun_Slug_A`, `AUD_BBW_Weapon_Pickup_A`, `AUD_BBW_Weapon_EmptyClick_A`.
- Acceptance criteria: each weapon mode is identifiable by sound alone, empty click is clear but not irritating, pickup cue feels rewarding and mechanical, clips avoid clipping and excessive low end.
- Likely generation method: layered procedural synthesis plus Foley-style metal/air source recordings if available.

### AUD-SET-002 - Enemy Audio Family

- Target use: Scrapper, Lancer, Bulwark, Bellows Node, Governor Warden, future Boiler Tick.
- Art direction: servo ticks, piston steps, cutter scrape, valve charge, furnace groan, hammer slam, pressure pulse, shutdown whine, metal collapse.
- Required maps or files: idle loops where needed, footstep/mechanical motion one-shots, attack tell, attack release, hurt, shutdown, enrage, boost, metadata sheet.
- LOD and collision notes: spatial mono one-shots; enemy loops must be short and not phase badly with multiple enemies.
- Platform variants: reduce simultaneous layers and clip length on mobile/WebGL/Quest; VR spatial cues must support locating threats without being harsh.
- Unity naming: `AUD_BBW_Scrapper_AttackTell_A`, `AUD_BBW_Lancer_Charge_A`, `AUD_BBW_Bulwark_Slam_A`, `AUD_BBW_BellowsNode_Pulse_A`, `AUD_BBW_Warden_Enrage_A`.
- Acceptance criteria: attack tells are audible and distinct, shutdown state is clear, repeated enemy groups do not become noisy mush, boss remains powerful without masking player feedback.
- Likely generation method: procedural mechanical layers, edited metal Foley, pitch/variation exports.

### AUD-SET-003 - Environment, Interactable, UI, And Ambience Audio Family

- Target use: gates, lifts, valves, gear keys, secret caches, hazards, ambience, HUD/menu instrument tones.
- Art direction: boiler-room bed, distant gears, pipe knocks, furnace rumble, steam leaks, green service chime, warning buzzer, brass gauge tick, archive plaque tick.
- Required maps or files: looping ambience beds, one-shot interactables, UI ticks, hazard loops, metadata sheet with mixer routing suggestions.
- LOD and collision notes: ambience can be stereo/non-spatial; interactables mono/spatial; loops need clean loop points.
- Platform variants: downmixed or shorter ambience loops for Android/WebGL/Quest; VR spatial loops should not feel like they are inside the player head.
- Unity naming: `AUD_BBW_Ambience_BoilerRoom_A`, `AUD_BBW_Gate_Open_A`, `AUD_BBW_Gate_Denied_A`, `AUD_BBW_Lift_Activate_A`, `AUD_BBW_Valve_Turn_A`, `AUD_BBW_UI_GaugeTick_A`.
- Acceptance criteria: interactables have clear state feedback, ambience supports steampunk identity without masking combat, loop seams are inaudible, warning sounds are useful but not fatiguing.
- Likely generation method: layered procedural audio, edited recordings, and loop polishing in an audio editor.

## Import Review Checklist For Generated Assets

Before any asset from this library is imported into the main Unity project, record:

- Asset ID and version.
- Source method and license status.
- Source file path outside the active project or staged import path if approved.
- Target Unity names.
- Texture sizes and platform overrides.
- Material count.
- Triangle count and LOD counts.
- Collision type.
- Pivot and scale validation.
- Screenshot or turntable reference.
- Acceptance status: `briefed`, `generated`, `staged`, `accepted`, `integrated`, `rework`, or `deferred`.

## Parallel Work Notes

This library intentionally starts with reusable foundations before hero assets. The material and trim work can run while gameplay development continues because those assets define the surface language for every later model. Weapon and enemy hero packages can start with concepts, silhouette blockouts, sockets, and rig plans before final mechanics are complete, as long as source files remain staged and do not replace active Unity assets without an integration pass.
