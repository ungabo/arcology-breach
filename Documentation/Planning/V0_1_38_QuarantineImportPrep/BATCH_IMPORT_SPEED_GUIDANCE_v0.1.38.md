# V0.1.38 Batch Import Speed Guidance

Purpose: decide when batching sidecar imports increases speed and when it creates integration drag.

## Default Bias

Batch aggressively when packages are independent and already have clean static reports. Import one at a time only when packages are coupled to project settings, core scripts, render pipeline settings, or gameplay behavior.

## Batch Multiple Sidecars When

- Each package has a package-local manifest and no static errors.
- Each package can be copied into a separate quarantine root.
- Package names, prefixes, and `.meta` GUIDs are isolated.
- Dependencies are empty or already approved.
- The same Unity import/compile cycle can validate all of them.
- The assets support the same main-lane leap, such as weapon visuals plus enemy visuals plus level dressing.
- Rollback remains package-by-package delete-only.

Recommended batch groups:

- Weapons plus feedback-FX: good when improving player interaction feel.
- Mechanical enemies plus level kit: good when improving encounter readability.
- Weapons plus level kit: good when building a showcase route.
- Weapons, enemies, and level kit together: acceptable only after each package has a clean throwaway import.

## Import One Package At A Time When

- The package has placeholders or generated counts that do not match disk files.
- The package has any static errors.
- The package introduces scripts beyond editor-only generators.
- The package depends on shader, render pipeline, input, physics, tags, layers, or quality settings.
- The package is visually high-risk and needs focused review against the north-star steampunk art.
- A previous package from the same lane just failed quarantine.

## Compile Cadence

Use one Unity import/compile pass for a batch when:

- Static checks pass for all packages.
- File changes are limited to quarantine roots.
- The batch creates a visible player-facing leap.

Run a compile immediately after one package when:

- It includes scripts, asmdefs, shaders, VFX Graph assets, or unusual serialized assets.
- It is the first package of a new sidecar lane.
- It has warnings that could mask missing references.

## 15-Minute PM Speed Question

At each review ask:

```text
Is this the fastest safe development pace achievable with the resources currently available?
```

If no, choose one:

- Start another isolated sidecar lane.
- Combine clean packages into one quarantine import cycle.
- Send a package with placeholders back for generator/render completion.
- Add a small validation script or template to remove repeated manual review.
- Increase main-lane batch size before the next full player build.

Next-step directive: continue immediately with the next highest-impact unfinished task.
