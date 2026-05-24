# Level Scale and Snap Spec v0.1.37

## Base Scale

- `1 Unity unit = 1 meter`
- Primary snap grid: `4m`
- Fine snap grid: `0.5m`
- Micro trim snap: `0.25m`, visual only

## Corridor Modules

| Item | Target |
| --- | --- |
| Corridor module length | `4m` |
| Visual shell width | `4m` |
| Minimum clear walk width | `3.2m` |
| Visual shell height | `3.2m` |
| Minimum clear walk height | `2.65m` |
| Corner footprint | `4m x 4m` |
| T-junction footprint | `8m x 4m`, with one 4m branch |

## Doors and Transitions

| Item | Target |
| --- | --- |
| Standard pressure-lock frame | `4.2m W x 3.2m H x 0.65m D` |
| Minimum aperture | `2.4m W x 2.6m H` |
| Boss/landmark vault door | `4.4m W x 3.6m H x 0.8m D` |
| Transition approach pad | `4m` clear in front of prompt/trigger |

## Navigation Clearance

- Player capsule assumption for blockout: `0.8m` diameter, `1.8m` height.
- Minimum hallway gameplay clearance: `3.2m` wide.
- Minimum objective console clearance: `1.5m` usable radius.
- Minimum enemy skirmish pocket: `6m` diameter.
- Preferred medium combat room: `8m to 10m` usable diameter.
- Boss/large mechanical enemy spaces: `12m+` usable span with at least two readable retreat lanes.

## Combat Space Recommendations

- Place bulky boilers, vault doors, and gauge walls on perimeters or ends of routes.
- Do not place rails or valve consoles inside enemy strafe lanes.
- Door frames can mark transitions, but center apertures must remain free of decorative pistons.
- Smoke anchors should face across walls or ceilings, not directly into the player's main aim/readability lane.

## Occlusion and Density Budget

| Category | Windows low/mid budget |
| --- | --- |
| Visible repeated corridor module triangles | Keep proxy module under `6k` until final art pass. |
| Materials per module | `1-4` preferred, `6` maximum for landmark modules. |
| Dynamic lights | `0` default; preview lights are not shipping lights. |
| Shadows | Disable on tiny rivets, gauges, slats, pipe clamps, and steam anchors. |
| Colliders | Primitive colliders only after main-lane route review. |
| Transparent steam | Avoid constant screen-filling overdraw; use short bursts and low-quality fallbacks. |

## Android and Web Simplification Notes

- Merge repeated rivets/slats into combined meshes or textures.
- Reduce material slots to shared iron, brass, copper, stone, and emissive recipes.
- Cap standard module textures at `512-1024` once texture assets exist.
- Replace heavy steam with simple low-alpha quads or event-only bursts.
- Avoid runtime mesh generation in player builds; generator is editor-only.

## Future VR Comfort Notes

- Avoid narrow, fast, blind `90-degree` turn chains without recovery spaces.
- Keep head-height bars, pistons, and pipes outside the headset comfort volume.
- Avoid large smoke bursts directly in front of the camera.
- Use strong shapes and lamps for route reads; do not rely on tiny gauge text.
- Keep moving doors and pressure locks slow, readable, and peripheral-safe.
