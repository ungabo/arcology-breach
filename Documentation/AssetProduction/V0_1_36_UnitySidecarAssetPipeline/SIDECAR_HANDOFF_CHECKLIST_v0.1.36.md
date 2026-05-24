# V0.1.36 Sidecar Handoff Checklist

Use this checklist before asking the primary Unity lane to review a sidecar asset pack.

## Package Boundary

- One package root.
- Unique lane prefix on asset names.
- No direct replacement of existing primary assets.
- No scattered imports into generic folders.
- No primary scenes, scripts, validators, project settings, package files, build settings, or generated builds.

## Manifest

- Pack ID, version, build ID, Unity version, and owner lane are filled in.
- Export artifacts and checksums are listed.
- Asset counts are filled in.
- Dependencies are listed.
- Required primary changes are listed or explicitly empty.
- Path collision and GUID collision checks are marked.
- Rollback path is documented.

## Clean Import

- Pack imports into a clean throwaway project.
- Console has no blocking errors.
- Runtime prefabs instantiate.
- No missing scripts, materials, meshes, textures, audio, or VFX references.
- Preview scene/sample remains preview-only.

## Visual/Content Proof

- Contact sheet or screenshots are attached.
- Scale, pivot, and naming rules are documented.
- LOD/collision/material guidance is included where relevant.
- Known risks and deferred issues are listed.

## Primary Review Request

- Ask for quarantine import review, not direct promotion.
- Include manifest, import report, screenshots/contact sheet, and rollback notes.
- State whether the intended decision is promote, defer, or reject.
