# Brassworks Breach - Batch Production Mode

Last updated: `2026-05-24 14:20 -04:00`

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

## Parallel Bundle Enforcement

Every new side-agent wave should start as a bundle spread, not a single work queue. A normal production wave should keep several of these lanes active at once when thread capacity allows:

- Weapon/prop arsenal bundle.
- Mechanical enemy bundle.
- Level module and setpiece bundle.
- Gameplay-systems and validation bundle.
- UI/audio/VFX bundle when the current milestone needs player feedback polish.

Side agents should own whole families with manifests, preview sheets, scale notes, material recipes, acceptance gates, and integration recommendations. A single isolated asset is acceptable only when it is a risky blocker, a failed proof recovery target, or a dependency that cannot be split cleanly.

If the subagent thread limit is reached, close completed or obsolete agents first, then immediately fill the open slots with the next independent bundle. Do not wait for every side bundle to finish before the main lane continues implementation.

## 15-Minute Speed Review

The PM loop now reviews production speed every 15 minutes. Each review should ask:

- Is this the fastest safe development pace achievable with the current machine, Unity install, repo state, and available subagent slots?
- Is the main lane blocked on side work, or can it keep integrating and testing a different slice?
- Are any agents doing serial single-asset work that should become a larger bundle?
- Are completed agents being closed quickly enough to refill capacity?
- Would a separate Unity sidecar project or importable asset-pack sandbox speed this batch without risking the primary project?

The answer should create action: redirect agents, spawn a new bundle, start/stop a sidecar lane, defer a risky import, or continue the main Unity integration.

## Unity Sidecar Asset-Pack Lanes

Separate Unity projects may be useful for asset-pack production when they can generate prefabs, materials, preview renders, manifests, and importable package outputs without touching primary game scenes, validators, build settings, or shared scripts.

Use sidecar Unity lanes for:

- Weapon/prop lookdev and prefab candidates.
- Mechanical enemy visual shells and socket/rigging proof.
- Level module and setpiece prefab kits.
- UI/audio/VFX feedback packages.
- Render-only concept and material proof sheets.

Avoid sidecar Unity lanes for:

- Core gameplay scripts that need current route/state authority.
- Generated scene integration.
- Build settings, packaging, QA candidate generation, or release docs.
- Work that depends on primary-scene object IDs or serialized references.

All sidecar output must come back through manifests, previews, import notes, and a primary-project import validation step before promotion.

## Batch Size Guidance

- Minimum acceptable batch: 4 to 6 related visible changes across at least two levels or one system plus supporting art/readability.
- Preferred milestone batch: 8 to 15 visible changes across multiple levels, asset families, or gameplay surfaces.
- Large leap batch: one gameplay system plus content, VFX/audio, validation, and route evidence, or a campaign-wide art/readability pass.

One-prop releases are reserved for focused blockers, regressions, or risky isolated fixes.

## Current Batch Direction

`v0.1.35` completed as the first large systems-plus-sidecar leap. The next batch should keep the same speed posture by finishing sidecar package gates and preparing the first safe quarantine imports without letting asset packages take gameplay or route authority.

- Sidecar gate remediation: package-local manifests, changelogs, import-smoke metadata, GUID collision notes, and zero blocking validator errors.
- Quarantine import prep: weapon/prop and mechanical enemy packages first, level kit after package metadata and import gates are clean.
- Main-lane follow-up: promote only small, route-safe visual references from sidecars until package import behavior is proven.
- QA/validation: require sidecar validator, quarantine import report, editor validation, route audit, and full matrix only when promoted content reaches playable scenes.

Expected verification: sidecar validator first, then targeted Unity import smoke for package lanes; reserve route audit/full V0 matrix for any main playable promotion.

## Subagent Directive

When assigning side agents:

- Give each agent a disjoint folder or file set.
- Ask for batch-ready implementation guidance, not isolated one-off release plans.
- Ask for acceptance gates that can validate multiple related assets at once.
- Ask for suggested targeted tests before the full matrix.
- Prefer assigning complete asset/system families to several agents at once over serializing individual assets.
- Keep code, generated scenes, build scripts, shared status docs, release docs, and Git operations out of side-agent write scope unless the parent lane explicitly owns integration.

## 15-Minute Review Loop

The `brassworks-breach-pm-continuation` heartbeat runs every 15 minutes and should perform a PM review before continuing:

- Check whether active work is batched enough to justify a versioned candidate.
- Stop one-prop drift unless a focused regression or blocker makes it necessary.
- Redirect side agents toward batch-ready plans, acceptance gates, material recipes, and placement maps.
- Decide whether separate Unity sidecar projects would speed asset-pack production for the current bundle.
- Use targeted tests while the batch is in progress.
- Run the full route/package/QA/candidate matrix only when the batch is coherent.
