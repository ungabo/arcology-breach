# Pressure Pistol Hero Set12 Implementation Plan

## Scope

Set12 is a fresh Unity-only sidecar package for the Brassworks Breach pressure pistol hero weapon. It is built from procedural Unity meshes, procedural PNG texture maps, Unity materials, isolated prefabs, and generated concept renders.

## Component Targets

- PPH12_WoodLeatherGrip_Component: role `wooden_leather_grip`, 47 mesh parts, 47 renderers.
- PPH12_BrassTriggerGuard_Component: role `brass_trigger_guard`, 23 mesh parts, 23 renderers.
- PPH12_PressureCylinderBarrel_Component: role `pressure_cylinder_barrel`, 33 mesh parts, 33 renderers.
- PPH12_CopperCoilArray_Component: role `copper_coil_array`, 42 mesh parts, 42 renderers.
- PPH12_PressureGauge_Component: role `pressure_gauge`, 51 mesh parts, 51 renderers.
- PPH12_SideValveWheels_Component: role `side_valve_wheels`, 26 mesh parts, 26 renderers.
- PPH12_MuzzleCrownCogBrake_Component: role `muzzle_crown_cog_brake`, 18 mesh parts, 18 renderers.
- PPH12_LeatherGloveHandProxy_Component: role `leather_glove_hand_proxy`, 23 mesh parts, 23 renderers.
- PPH12_FullFirstPersonHeroPistol: role `full_assembled_first_person_hero_pistol`, 283 mesh parts, 283 renderers.

## Material Direction

- Aged brass uses oxide green/brown mottling, bright worn scratches, and medium roughness.
- Oxidized copper uses warm coil bands, teal oxidation, and heat staining.
- Dark blued iron uses blue-black pitting, edge wear, and oily low-sheen recesses.
- Warm amber glass uses radial glow, bubbles, and high smoothness.
- Walnut and leather use grain, cracks, wrap shadows, stitch marks, and grime in seams.

## Import Notes

The package is visual-only and can be imported as a local UPM file package when the main lane is ready. Runtime prefabs intentionally avoid colliders, rigidbodies, lights, cameras, audio, gameplay scripts, animation controllers, and scene references.

Validation status after generation: PASS.
