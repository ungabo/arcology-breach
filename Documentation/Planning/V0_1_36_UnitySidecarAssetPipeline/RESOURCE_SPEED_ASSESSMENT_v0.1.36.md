# V0.1.36 Resource And Speed Assessment

Purpose: define when parallel Unity sidecars are worth it, when they become drag, and how many lanes are practical on a single developer machine.

## Where Sidecars Help

Sidecars are most useful when the work is asset-heavy and has weak coupling to gameplay authority.

High-value cases:

- Lookdev: material tuning, silhouettes, color language, emissives, and prop readability.
- Prefab assembly: visual-only mesh/material/audio/VFX groupings with stable pivots and naming.
- Preview generation: contact sheets, turntables, lighting checks, and scale comparison scenes.
- Import quarantine: catching missing materials, bad scale, broken prefab references, and path collisions before the main project opens the pack.
- Artist iteration: one worker can churn through asset revisions while another keeps the primary game project open for gameplay, route, validator, and build work.

The major speed gain is not raw import speed. It is avoiding editor lock contention, broken primary-scene states, and project-wide churn while assets are still being shaped.

## Where Sidecars Slow Us Down

Sidecars hurt when a pack needs deep project coupling.

Poor sidecar candidates:

- Gameplay prefabs that require new scripts or modified runtime components.
- Scene placements tied to route blockers, spawn timing, objective logic, or build validation.
- Anything that changes tags, layers, input, packages, quality settings, render settings, physics settings, or build settings.
- Large shared material libraries if every lane edits the same material names.
- Addressables, AssetBundles, or runtime delivery systems before the primary project owns the pipeline.

Friction costs:

- Every sidecar has its own `Library/` import cache.
- First open/import can be slow and disk-heavy.
- Unity package dependency drift can make content look correct in the sidecar but fail in the primary project.
- More lanes create more review artifacts and more intake decisions for the main lane.
- Duplicate preview scenes can hide lighting or scale assumptions that differ from the game.

## Machine Limits

Practical constraints on a normal Windows workstation:

- RAM: each open Unity Editor can consume multiple GB after imports. Two open Editors plus IDE/browser is usually reasonable on 32 GB. Three can become unstable or paging-heavy if assets are texture-rich.
- CPU: imports, shader compilation, compression, and previews are bursty. Running heavy imports in multiple Editors at once can slow all lanes.
- Disk: every sidecar `Library/` can become many GB. The cost scales with imported textures, meshes, shader cache, and package cache.
- Disk I/O: simultaneous imports can saturate the drive and make the primary editor feel frozen.
- GPU/VRAM: multiple preview scenes, VFX, and high-resolution material previews can contend for VRAM.

Recommended local operating limit:

- 1 primary Unity Editor plus 1 sidecar Editor for active work is the safest default.
- 1 primary plus 2 sidecars is practical for focused asset-only lanes if imports are staggered.
- 1 primary plus 3 sidecars should be treated as temporary batch processing, not a daily default.
- More than 3 sidecar lanes on one workstation is usually counterproductive unless most lanes are closed and only exporting from batchmode.

## Library Cache Costs

Each sidecar project creates its own `Library/`, so the first import cost is paid per project. This is the biggest hidden cost of sidecars.

Mitigations:

- Keep sidecar dependencies minimal.
- Do not clone the full primary project into each sidecar.
- Use one sidecar per content family, not one sidecar per individual prop.
- Avoid high-resolution texture experiments until the pack has passed silhouette and layout review.
- Close inactive Editors and preserve sidecar `Library/` only for active lanes.
- Use clean throwaway import projects for validation, then delete them after report generation.

Do not share or copy `Library/` between projects as a pipeline guarantee. Unity caches are implementation details and can become stale or editor-version sensitive.

## Unity License And Session Risks

Risks to check before scaffolding:

- Unity license terms and seat/session behavior for multiple local Editor instances.
- Hub/account sign-in stability when opening several projects.
- Package Manager authentication if future packages require registry access.
- Asset Store license terms if paid assets are imported into sidecars.

Practical guidance:

- Multiple local projects are technically common, but the team should confirm licensing before making it a formal production lane.
- Do not run automated sidecar batch jobs under another worker's account.
- Keep sidecars asset-only so no build automation or distribution step depends on extra Unity sessions.

## Merge And Conflict Risks

Sidecars reduce conflict only if imports remain isolated.

Primary risks:

- Path collisions when two sidecars produce the same asset names.
- GUID collisions if `.meta` files are copied from duplicated projects or assets are force-copied incorrectly.
- Shared material names with different shader settings.
- Prefab references to sidecar-only preview assets.
- Hidden dependencies on sidecar packages or editor-only assets.
- Main-lane merge conflicts if multiple workers edit `Packages/manifest.json`, build settings, scenes, or status docs.

Controls:

- One package root per lane.
- Unique asset prefixes per lane.
- Manifest must list all dependencies and required primary changes.
- Import must be tested in a clean throwaway project.
- Primary intake happens in quarantine staging before final placement.
- No sidecar lane edits shared integration/status files.

## Practical Lane Count

Recommended v0.1.36 start:

- Start with 2 active sidecars.
- Add a third only if the first two produce import reports without primary-lane disruption.
- Keep at most 1 heavy import running at a time.
- Keep at most 1 sidecar focused on complex prefabs/large textures at a time.

Suggested capacity model:

| Lane count | Use case | Risk |
| --- | --- | --- |
| 1 sidecar | Prove pipeline with weapon/prop lookdev | Low |
| 2 sidecars | Weapon/prop plus mechanical enemy visuals | Manageable |
| 3 sidecars | Add level modules after import gates are stable | Medium |
| 4+ sidecars | Only if mostly closed/batch exports on strong hardware | High coordination overhead |

## Bottom Line

Sidecars are worth using for this project if they are treated as asset factories with hard import gates. They are not a way to parallelize primary-game integration. The fastest safe pattern is two sidecars producing isolated UPM-shaped visual packs while the main lane continues gameplay and scene work.
