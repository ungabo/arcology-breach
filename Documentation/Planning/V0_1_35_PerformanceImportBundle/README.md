# Brassworks Breach - v0.1.35 Performance + Import Readiness Bundle

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_35_PerformanceImportBundle/`

## Purpose

Define the performance, validation, and import-readiness rules for the `v0.1.35` staged weapon, enemy, level-module, and feedback packages. This is a documentation-only bundle intended to make the next art integration faster and safer on low/mid Windows PCs while preserving later Android, WebGL, SteamVR, and Meta paths.

This packet does not change scripts, scenes, validators, package files, build settings, or shared status docs.

## Packet Contents

- `PERFORMANCE_BUDGETS_v0.1.35.md` - low/mid Windows budgets plus deferred platform notes.
- `VALIDATION_GATES_v0.1.35.md` - targeted checks for import errors, shader/material failures, collider excess, missing previews/manifests, and route-authority mistakes.

Related asset-production packet:

- `Documentation/AssetProduction/V0_1_35_PerformanceImportBundle/IMPORT_RULES_v0.1.35.md`
- `Documentation/AssetProduction/V0_1_35_PerformanceImportBundle/INTEGRATION_CHECKLIST_v0.1.35.md`

## Top Rules

1. Import packages as visual staging first; gameplay authority remains with existing main-lane systems.
2. Prefer simple primitives, shared materials, and explicit LOD policy over dense one-off meshes.
3. Pink/magenta output, missing manifests, missing previews, or unauthorized colliders are hold/fail issues before prefab promotion.
4. Windows low PC readability and frame stability are the first gate; Android/WebGL/VR constraints must remain possible by design.
5. No staged bundle may introduce route, pickup, damage, prompt, objective, transition, boss-lock, save, or nav authority.

