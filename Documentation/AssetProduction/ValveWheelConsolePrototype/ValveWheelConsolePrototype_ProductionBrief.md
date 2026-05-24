# Valve Wheel Console Prototype Production Brief

Status: Prepared for next asset/lookdev slice
Project: Brassworks Breach
Component folder: `Documentation/AssetProduction/ValveWheelConsolePrototype/`
Last updated: 2026-05-24

## Intent

Prepare a Unity-only production target for a steampunk wall console prototype that can be built from Unity primitives, procedural meshes, project-native materials, and in-editor lookdev passes. The console should read as a clear future interaction affordance in a brassworks industrial space without owning any gameplay state, puzzle logic, input bindings, animation authority, or build pipeline changes.

The visual north star is a brass and blackened iron wall console with a large valve wheel, pressure gauges, amber pilot lamps, exposed pipes, riveted plates, worn edges, oil grime, soot-dark recesses, and readable mechanical purpose.

## Scope

In scope:

- Wall-mounted console silhouette and readable asset breakdown.
- Unity primitive and procedural mesh construction plan.
- Material targets for brass, blackened iron, glass, amber emissive lamps, grime, oil, and pipe hardware.
- Lookdev and test-render expectations inside Unity.
- Acceptance criteria and validation checklist for a future implementation pass.

Out of scope:

- No changes to scenes, prefabs, scripts, gameplay systems, input, build scripts, release notes, or shared status docs.
- No external DCC authority such as Blender, Maya, Substance, or ZBrush for this prototype brief.
- No final gameplay interaction design beyond visual affordance.
- No authoritative puzzle state, damage state, objective state, or progression hooks.

## Player-Facing Read

At first glance, the asset should communicate: "this wall console controls pressure or flow, and the valve wheel is something the player may eventually operate." The valve wheel must be the primary focal point, with gauges and amber lamps providing secondary state cues. The asset should feel heavy, oily, hand-maintained, and mechanically plausible rather than decorative.

The future gameplay affordance should come from visual language only:

- The valve wheel faces the player and is sized for two-handed operation.
- The wheel has a slightly cleaner contact wear band on its rim and handles.
- Amber pilot lamps sit near the gauges as readable status indicators.
- Gauge needles have distinct resting poses but do not imply live gameplay values.
- Pipes visibly enter and leave the console, suggesting pressure flow.
- Mounting bolts, rivets, brackets, and grime reinforce that the unit belongs to the level.

## Unity-Only Construction Approach

The prototype should be achievable entirely in Unity using primitives, procedural meshes, editor-authored prefabs, project materials, and optional project-native decals or texture generators.

Recommended asset hierarchy:

```text
ValveWheelConsolePrototype
  Console_Backplate
  Console_RaisedPanel
  ValveWheel_Root
    ValveWheel_OuterRing
    ValveWheel_InnerHub
    ValveWheel_Spokes
    ValveWheel_GripHandles
    ValveWheel_AxleCap
  GaugeCluster_Root
    Gauge_Left
    Gauge_Right
    Gauge_Glass
    Gauge_Needles
  PilotLampCluster_Root
    Lamp_Amber_A
    Lamp_Amber_B
    Lamp_Housings
  Pipework_Root
    Pipe_Inlet
    Pipe_Outlet
    Pipe_Elbows
    Pipe_ValveCouplings
  Fasteners_Root
    Rivets
    Bolts
    Brackets
  Grime_Detail_Root
    OilStreaks
    SootPatches
    EdgeWearMasks
```

### Primitive and Procedural Mesh Plan

Backplate and raised panels:

- Use cubes scaled to thin wall plates with bevels where the local toolchain supports bevelled procedural meshes.
- If bevels are not available yet, use narrow darker trim strips around plate edges to avoid a flat rectangular read.
- Layer at least two plate depths: a broad wall backplate and a raised central control panel.
- Use small cylinders or low-poly capsule-like meshes for rivets and bolt heads.

Valve wheel:

- Use a procedural torus or ring mesh for the outer wheel. If a torus generator is not yet available, approximate with segmented cylinders arranged in a circle for the prototype pass.
- Use a central cylinder hub, a smaller axle cap, and 5 or 6 spokes built from cuboids or procedural rectangular prisms.
- Add three or four grip nubs/handles as short cylinders mounted around the ring.
- Keep the ring silhouette readable from 8 to 15 meters in Unity's Game view.
- Slightly offset the wheel forward from the panel so the hand-clearance read is obvious.

Gauges:

- Use cylinders for gauge housings, thin cylinders or discs for faces, and transparent glass discs slightly proud of the face.
- Gauge needles can be narrow triangles or slim cuboids pivoted at the lower center.
- Face markings may be procedural radial ticks, decal strips, or simple mesh ticks. For prototype validation, at least major ticks and one red danger arc are enough.
- Gauges should be angled or positioned so they catch highlights without becoming the primary focal point.

Pilot lamps:

- Use small cylinders or capsules for brass/iron lamp bezels.
- Use emissive amber material on the lens, with bloom enabled only if the active render pipeline and scene settings already support it.
- Lamps should be bright enough to read as powered but not so bright that they erase nearby metal detail.

Pipes:

- Use cylinders for straight pipe runs and procedural elbow meshes for bends when available.
- If procedural elbows are not available, use short cylinder segments at stepped angles for a prototype read.
- Include at least one pipe entering from below and one leaving laterally or upward.
- Add collars, flanges, or coupling rings at panel entry points.

Rivets and fasteners:

- Rivets should be repeated but not perfectly sterile. Vary scale and rotation subtly if the implementation path supports it.
- Use a higher density near brackets, pipe mounts, and panel seams.
- Keep fasteners large enough to survive the intended camera distance.

Oil grime and soot:

- Prefer project-native decals, vertex color masks, or procedural material masks.
- Concentrate grime below moving parts, under pipe joints, around the wheel axle, and along lower plate edges.
- Add blackened soot in recesses and where pipes meet the console.
- Use edge wear on the valve rim, spoke edges, raised panel corners, and bolt caps.

## Scale and Composition Targets

Recommended world scale:

- Overall width: 1.2 to 1.6 Unity units.
- Overall height: 1.6 to 2.1 Unity units.
- Overall depth from wall: 0.25 to 0.45 Unity units.
- Valve wheel diameter: 0.65 to 0.9 Unity units.
- Gauge diameter: 0.22 to 0.34 Unity units.
- Pilot lamp diameter: 0.08 to 0.14 Unity units.

Composition:

- The valve wheel should occupy roughly 35 to 45 percent of the visible console height.
- Gauges should sit above or beside the wheel, not hidden behind it.
- Lamps should form a small readable cluster near the gauges.
- Pipes should break the rectangular silhouette while staying wall-mounted and level-friendly.
- Avoid overly clean symmetry; the asset should feel engineered, repaired, and old.

## Material Targets

Use the active Unity render pipeline's standard lit material family unless the project already has a stronger house material workflow.

Brass:

- Base color: aged yellow brass, not bright gold.
- Metallic: high.
- Smoothness/roughness: medium, with polished wear on touched edges.
- Detail: darker tarnish in recesses, lighter rubbed highlights on rim, spokes, rivets, and panel corners.

Blackened iron:

- Base color: charcoal/blue-black iron with subtle brown oxidation.
- Metallic: medium to high.
- Smoothness/roughness: low to medium.
- Detail: chipped edges, soot-dark cavities, brighter exposed scratches on corners.

Amber lamp glass:

- Base color: warm amber/orange.
- Emission: low to medium amber, enough for status readability.
- Transparency: optional; do not rely on transparency if it creates sorting issues in the current pipeline.
- Detail: small bright core and darker rim can be faked with nested lens meshes.

Gauge glass:

- Base color: near-clear with faint blue/green tint.
- Smoothness: high.
- Metallic: none.
- Detail: use subtle reflection and thin highlight strips. Avoid making the gauge unreadable.

Gauge faces:

- Base color: aged ivory, stained metal, or dark enamel depending on project style.
- Markings: high contrast enough to read in Unity test renders.
- Needle: dark red, blackened steel, or brass with a clear silhouette.

Oil grime:

- Base color: dark brown to near-black.
- Smoothness: medium to high in streaks and pooled areas.
- Placement: beneath axle, pipe joints, lower rivets, and panel seams.

Rubber or gasket material:

- Base color: matte black or dark grey.
- Smoothness: low.
- Use around gauge rims, pipe couplings, or lamp housings only where it supports readability.

## Lookdev Lighting Targets

Test renders should be evaluated in Unity, not in external renderers.

Minimum test setup:

- One neutral three-quarter camera view at player eye height.
- One close-up camera on the wheel, gauges, and lamps.
- One low-angle view that catches rim highlights and pipe depth.
- A neutral wall or level-representative industrial wall behind the asset.
- Existing project lighting if available; otherwise a simple key, fill, and low warm practical light.

Lighting goals:

- Brass and blackened iron should separate clearly.
- Amber lamps should be visible but not clip into flat orange blobs.
- Gauge needles should remain legible.
- The wheel silhouette should remain readable against the backplate.
- Oil grime should add age without turning the whole asset uniformly dark.

## Acceptance Criteria

The future asset/lookdev implementation is acceptable when all of the following are true:

- The console is constructible in Unity from primitives, procedural meshes, project-native materials, and optional project-native decals.
- The large valve wheel is the primary visual affordance and reads as operable from normal gameplay distance.
- The asset includes a brass/blackened iron wall console, large valve wheel, pressure gauges, amber pilot lamps, pipes, rivets, and oil grime.
- The asset does not introduce gameplay authority, puzzle state, input handling, runtime interaction logic, objective logic, or build/release changes.
- The silhouette remains readable in Unity Game view at near, medium, and long inspection distances.
- Materials clearly distinguish brass, blackened iron, glass, amber lamps, and grime.
- Gauges and lamps imply possible future state without presenting authoritative live values.
- Pipes and mounting hardware make the console feel physically installed in the brassworks environment.
- Test renders show no missing materials, obvious z-fighting, unreadable gauge faces, clipped lamps, or floating components.
- The asset can be reviewed as a self-contained prototype before any gameplay integration begins.

## Validation Checklist

Use this checklist during the future implementation review:

- Folder and prefab naming are component-specific and do not collide with existing production assets.
- Asset uses Unity-native primitive/procedural construction for the prototype pass.
- No scripts are required for the static lookdev prototype.
- No scene, build, gameplay, release note, or shared status file is modified as part of this docs-only preparation slice.
- Valve wheel has clear depth from the wall and plausible hand clearance.
- Gauge needles, tick marks, and danger/readout zones are visible in Unity renders.
- Amber lamps have a controlled glow and maintain lens shape.
- Brass and blackened iron are visually distinct under the same lighting.
- Oil grime is concentrated around mechanically plausible leak and touch zones.
- Rivets and fasteners are present around panels, brackets, pipe mounts, and flanges.
- Pipes connect cleanly to the console and do not appear to float.
- Mesh intersections are hidden by collars, flanges, trim, or intentional overlap.
- No z-fighting is visible on layered plates, gauge glass, lamp lenses, or grime decals.
- Console silhouette remains readable against a dark industrial wall.
- The final review images are captured from Unity Game view or Scene view with project rendering active.

## Unity Test Render Evaluation

Evaluate test renders inside Unity using a small fixed capture set:

1. Hero three-quarter view: verifies silhouette, material separation, wheel affordance, and overall north-star match.
2. Orthographic-ish front view: verifies panel layout, gauge/lamp readability, symmetry/asymmetry balance, and fastener density.
3. Close-up detail view: verifies material wear, grime placement, gauge needles, lamp lenses, rivets, and pipe couplings.
4. Gameplay-distance view: verifies that the valve wheel and amber status lights still read at expected player distance.

For each render, record:

- Unity version and active render pipeline if known.
- Scene or test harness used.
- Camera distance and focal length or field of view.
- Lighting setup.
- Any material, mesh, or readability issues.

Render pass should fail review if:

- The valve wheel is not the strongest affordance.
- Brass reads as plastic or bright gold.
- Blackened iron and grime collapse into a single dark mass.
- Lamps bloom so strongly that nearby details are lost.
- Gauge faces cannot be understood as pressure gauges.
- Pipes, rivets, or brackets look detached from the console.
- The object implies active gameplay state that does not exist yet.

## Handoff Notes

The next production agent can use this brief to create a Unity prefab or lookdev scene later, but this slice intentionally stops at documentation. The safest next step is a static Unity prototype prefab with no runtime scripts, no trigger colliders, and no gameplay references. If future interaction is explored, it should be proposed separately and integrated through the existing gameplay architecture rather than authored inside the visual asset.
