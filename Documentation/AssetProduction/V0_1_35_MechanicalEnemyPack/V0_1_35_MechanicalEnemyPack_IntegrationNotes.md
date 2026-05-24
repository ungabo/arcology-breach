# v0.1.35 Mechanical Enemy Pack

Unity-only staging package for the steampunk/brassworks enemy direction. The pack contains integration-ready proxy meshes, prefab assemblies, material recipes, sockets, shutdown fragments, and rendered proof sheets.

## Families

- Scrapper: 1.55 m low fast profile, left saw, right claw, crouched piston legs.
- Lancer: 1.90 m narrow spire profile, forward lance, cyan backpack coils.
- Bulwark: 2.15 m shield-wall profile, heavy boiler cage, shield plate, steam hammer.
- Warden: 2.35 m command profile, twin charge coils, pincer/gavel arms.
- Foundry Overseer Elite: 2.75 m miniboss silhouette with saw, hammer, back lance, crown coils.

## Rigging Sockets

Socket placeholders are named with `SOCK_` and should be preserved when replacing proxies with final rigged meshes. Key sockets: hips, spine cage, tool hands, coil/backpack, weak lamp, and shutdown burst.

## Animation Readiness

Keep torso cages as stable root masses, arms/legs as separated piston bars, tools as child transforms, and coils/lamps as independent children for readable anticipation, damage, and shutdown animation. Shutdown fragments are staged near the feet/back for burst or crumble effects.

## LOD And Collider Guidance

LOD0 keeps all rivets, coils, tags, weak lamps, and shutdown fragments. LOD1 may remove every other rivet and simplify coil stacks. LOD2 should collapse tools to broad silhouette meshes while preserving cyan tell and amber weak lamp cards. Use one capsule or box for body collision, separate simple boxes/spheres for shield, hammer/saw/lance damage windows, and one small sphere trigger around each weak-point lamp.

## Material Recipes

- Aged brass: warm metallic brass, medium smoothness, soot masks on concave seams.
- Blackened iron: dark high-metal body metal with low-mid smoothness, dry edge wear.
- Oily leather: dark brown low-metal grips, higher smoothness, uneven oil shine.
- Grimy glass: green-grey transparent read for lenses and lamp covers.
- Amber furnace lamps: emissive orange weak-point heat language.
- Cyan/blue bolt tells: emissive charge and ranged-attack readability.
- Soot/wear: almost-black grime for cavities, feet, shutdown fragments, and silhouettes.
- Hazard trims: yellow enamel/paint for shield and miniboss danger accents.

## Acceptance Gates

- Five prefabs import under `Assets/_Project/ArtStaging/V0_1_35_MechanicalEnemyPack/Prefabs` with no gameplay references.
- Material count covers the eight required recipes plus weak/shutdown/tag utility materials.
- Each family has torso cage, limbs, a role-defining tool, weak-point lamps, shutdown fragments, and a silhouette tag.
- Preview sheets exist under `Documentation/ConceptRenders/V0_1_35_MechanicalEnemyPack/` and are generated from Unity, not external DCC.
