# Door Vault Set 10

Unity-only visual sidecar package for the north-star steampunk corridor door and vault assembly.

Assets are visual-only: no runtime scripts, colliders, rigidbodies, lights, cameras, audio, animation controllers, scenes, or gameplay authority are saved in prefabs. The only script is the editor-only generator under `Editor`.

Preview PNGs and the contact sheet are generated to `Documentation/ConceptRenders/V0_1_55_DoorVaultSet10`.

## Contents

- `Runtime/Prefabs`: standalone modules and one candidate assembly.
- `Runtime/Materials`: brass, iron, gunmetal, amber glass, gauge enamel, red pressure paint, verdigris pipe, and oil-dark gasket materials.
- `Runtime/Textures`: generated base texture PNGs used by the materials.
- `Runtime/Meshes`: reusable procedural Unity mesh assets.
- `Runtime/Metadata`: normalized manifest copy.
- `Documentation~/Manifest`: import-facing normalized manifest copy.

## Rebuild

Open the validation project or a quarantine project with this package referenced, then run:

`Brassworks Breach/Sidecars/Door Vault Set 10/Generate Assets And Renders`
