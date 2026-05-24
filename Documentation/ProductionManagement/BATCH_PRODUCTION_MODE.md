# Brassworks Breach - Batch Production Mode

Last updated: `2026-05-24 12:46 -04:00`

## Why This Exists

The one-component-per-release cadence is reliable but too slow for the desired v1 scope. Beginning after `v0.1.32`, the main lane should batch several related improvements before creating a new packaged candidate.

This is now the default process. One-prop releases should happen only when the change is risky enough to isolate, blocks other work, or repairs a focused regression.

## Operating Mode

- Group related environment, gameplay-readability, UI, audio, and tuning work into coherent batches.
- Each compile should be a visible milestone leap. A player should be able to notice the difference without reading release notes.
- Use fast checks during implementation: compile, scene rebuild, level validation, and targeted player smoke tests.
- Run route audit plus the full package/QA/candidate matrix only after a coherent batch is ready.
- Keep side agents on disjoint prep work: briefs, acceptance gates, material recipes, route dressing plans, and future component specs.
- In every side-agent prompt, explicitly say that the requested output should support batched implementation and should not assume one component equals one release.
- Keep code, generated scene builder edits, generated scenes, build scripts, release docs, and Git pushes in the main lane.
- Use the project-local Unity toolchain only for render/lookdev work.

## Parallel Crew Rule

Do not run asset production as `build asset A, then asset B, then asset C`. Run multiple independent crews in parallel whenever the write scopes can be separated:

- Environment crew: route-safe modular dressing families, material recipes, and placement packs.
- Level crew: multi-level placement maps, route readability, encounter footprint checks, and density targets.
- Weapon/prop crew: staged pressure-pistol, scattergun, pickup, console, ammo, and display-stand improvements.
- Enemy/readability crew: staged enemy silhouettes, tell props, weak-point language, shutdown fragments, and material separation.
- QA/validation crew: acceptance gates, targeted smoke plans, manual-check sheets, and performance-risk notes.

The main lane integrates these outputs into Unity and owns shared code, generated scenes, validation scripts, builds, releases, commits, and pushes.

## Batch Size Guidance

- Minimum acceptable batch: 4 to 6 related visible changes across at least two levels or one system plus supporting art/readability.
- Preferred milestone batch: 8 to 15 visible changes across multiple levels, asset families, or gameplay surfaces.
- Large leap batch: one gameplay system plus content, VFX/audio, validation, and route evidence, or a campaign-wide art/readability pass.

One-prop releases are reserved for focused blockers, regressions, or risky isolated fixes.

## Suggested Next Batch

`v0.1.34` should keep the batch size aggressive by combining multiple parallel crew outputs into one playable polish leap:

- Weapon/prop staging: improve pressure-pistol, scattergun display, pickup, ammo, and console/readability silhouettes where safe.
- Enemy/readability staging: promote Scrapper, Lancer, Bulwark, and Warden tell/readability improvements that do not require final rigging.
- Level-density placement: add route-safe density where the v0.1.33 dressing pass proved clearances are stable.
- QA/validation: add batch-level acceptance gates for enemy/weapon readability and object authority preservation.
- Keep independent crews running in parallel; the main lane should integrate several families at once.

Expected verification: scene rebuild, level validation, targeted route smoke during development, then route audit and full V0 matrix when the batch is coherent.

## Subagent Directive

When assigning side agents:

- Give each agent a disjoint folder or file set.
- Ask for batch-ready implementation guidance, not isolated one-off release plans.
- Ask for acceptance gates that can validate multiple related assets at once.
- Ask for suggested targeted tests before the full matrix.
- Keep code, generated scenes, build scripts, shared status docs, release docs, and Git operations out of side-agent write scope unless the parent lane explicitly owns integration.

## 30-Minute Review Loop

The `brassworks-breach-pm-continuation` heartbeat runs every 30 minutes and should perform a PM review before continuing:

- Check whether active work is batched enough to justify a versioned candidate.
- Stop one-prop drift unless a focused regression or blocker makes it necessary.
- Redirect side agents toward batch-ready plans, acceptance gates, material recipes, and placement maps.
- Use targeted tests while the batch is in progress.
- Run the full route/package/QA/candidate matrix only when the batch is coherent.
