# Brassworks Breach - Parallel Platform Ports and VR Readiness Plan

Timestamp: 2026-05-23 21:31 -04:00

Owner: side-agent platform planning track

Write scope for this plan: documentation only. This plan is meant to run beside the main Windows implementation without editing Unity scenes, scripts, generated assets, or existing roadmaps.

## 1. Purpose

`Brassworks Breach` is currently a Windows-first steampunk FPS. The main team can keep building the playable Windows game while independent side agents prepare the platform constraints, asset budgets, input model, and verification additions needed for later Android, WebGL, SteamVR, and Meta Quest builds.

The goal is not to port the game immediately. The goal is to prevent Windows decisions from blocking future ports.

This plan covers:

- Windows mid/low gaming PC.
- Android phone.
- WebGL browser.
- SteamVR / PC VR.
- Meta Quest standalone VR.
- Shared input abstractions.
- VR comfort constraints.
- UI/HUD adaptations.
- Asset-quality tiers.
- Texture, mesh, LOD, memory, and storage budgets.
- Shader and material guidance.
- Build settings.
- Test matrix additions.
- Current Windows development risks and opportunities.

## 2. Current Assumptions

- Unity version: `6000.4.6f1`.
- Current primary platform: Windows x86_64 standalone.
- Current gameplay style: compact steampunk first-person dungeon crawler/shooter.
- Current visual north star: stylized brassworks, pressure machinery, gaslight, riveted iron, walnut, soot, copper, gauges, valves, and mechanical enemies.
- Windows remains the source-of-truth gameplay build until v1.0.
- Android, WebGL, SteamVR, and Meta Quest are deferred implementation targets, but their constraints should influence asset creation and architecture now.
- AAA aspiration should mean high craft, strong art direction, authored polish, and scalable production quality. It should not mean one-off unoptimized assets that cannot down-tier.

## 3. Parallel Development Boundaries

### Can Run Independently Now

These tracks can be delegated to other chats or side agents without waiting on main gameplay:

- Platform readiness documentation and budgets.
- Input abstraction design notes.
- UI/HUD adaptation design notes.
- Asset tier specifications and naming conventions.
- Texture trim-sheet and material-library planning.
- LOD naming and import preset recommendations.
- VR comfort requirement documentation.
- Test matrix expansion proposals.
- Store/platform risk tracking.
- Build profile checklist drafts.

### Should Wait For Main Gameplay

These tracks depend on the final shape of the Windows game:

- Actual Android touch implementation.
- Actual WebGL build pipeline.
- Actual OpenXR/XR rig implementation.
- Final level segmentation for mobile/browser/Quest.
- Final performance budgets based on real content.
- Final store compliance work.
- Final controller binding maps.

### Collision Rules For Side Agents

- Do not edit gameplay scripts unless explicitly assigned.
- Do not edit generated scenes unless assigned to the scene-build track.
- Do not overwrite asset catalogs owned by the main agent.
- Use new documentation files or append-only notes when possible.
- If a side agent produces generated assets, stage them in a clearly named incoming folder for review before integration.

## 4. Cross-Platform Architecture Requirements

These requirements should influence the Windows implementation now.

### Input

Use gameplay intents, not raw device actions, as the shared model.

Core gameplay intents:

- Move2D.
- Look2D.
- FirePrimary.
- FireAlternate.
- Interact.
- WeaponSlot1.
- WeaponSlot2.
- WeaponNext.
- WeaponPrevious.
- Pause.
- Restart.
- Confirm.
- Cancel.
- Settings.

Platform mappings:

| Intent | Windows KB/M | Android Phone | WebGL | SteamVR | Meta Quest |
| --- | --- | --- | --- | --- | --- |
| Move2D | WASD | virtual stick | WASD / gamepad | left thumbstick | left thumbstick |
| Look2D | mouse delta | swipe region | mouse pointer lock | headset + optional stick turn | headset + snap/smooth turn |
| FirePrimary | left mouse | fire button | left mouse / gamepad trigger | dominant trigger | dominant trigger |
| FireAlternate | right mouse | alternate button | right mouse / gamepad bumper | grip or secondary trigger | grip or secondary trigger |
| Interact | E / mouse over prompt | interact button | E / click | near-hand ray / hand proximity | near-hand ray / hand proximity |
| WeaponSwitch | 1/2 or wheel | radial/step button | 1/2 or wheel | wrist/weapon holster/step | wrist/weapon holster/step |
| Pause | Escape | pause button | Escape / UI button | menu button | menu button |

Design rules:

- Weapon firing should accept an aim origin and direction, not assume center-screen forever.
- Interactions should expose reusable `CanInteract`, `Interact`, prompt text, and usable distance concepts.
- Player movement should consume a platform movement provider, not direct keyboard state.
- Pause and pointer-lock behavior should be platform specific.
- Runtime tests should be able to inject intents without physical devices.

### Camera And Player Body

Flat-screen Windows camera:

- One camera, mouse-look, screen-center weapon ray.

Future touch/browser:

- Same camera family, different input provider.

Future VR:

- XR rig camera must own head pose.
- Weapon aim comes from controller pose, not camera center.
- Collision capsule should respect headset position without forcing camera motion.
- Head bob, camera shake, forced FOV effects, and hard rotations must have comfort-safe alternatives.

### UI Data Separation

HUD data should be separate from HUD presentation.

Shared data model:

- Health.
- Ammo by weapon.
- Current weapon.
- Gear key state.
- Objective text.
- Interact prompt.
- Boss health.
- Secret count.
- Damage warning.
- Pause/settings state.

Presentations:

- Windows/WebGL: screen-space brass HUD.
- Android: larger touch-safe screen-space HUD with reduced clutter.
- SteamVR/Quest: wrist, weapon-mounted, and world-space diegetic HUD.

### Asset Scalability

Every major asset should have:

- Source/high version for authoring.
- Runtime LOD0 for Windows.
- Runtime LOD1/LOD2 for distance.
- Mobile/Web/Quest reduced variant if needed.
- Material instance using shared shader families.
- Texture set that can be downscaled automatically.

## 5. Platform Targets

### Windows Mid/Low Gaming PC

Role:

- Primary development target.
- Source-of-truth gameplay and campaign implementation.
- Quality baseline for the full v1.0 release.

Hardware planning profile:

- CPU: older 4-core/6-core desktop CPU.
- GPU: GTX 1060 / GTX 1650 / RX 580 class or better.
- System RAM: 8 GB minimum, 16 GB recommended.
- VRAM: 4 GB practical budget.
- Display: 1080p primary.

Performance:

- 60 FPS target at 1080p.
- 30 FPS fallback only for low settings.
- Avoid combat spikes and asset streaming stalls.

Runtime budgets:

| Category | Target |
| --- | --- |
| Visible scene triangles | 250k to 500k typical, lower during heavy combat |
| Skinned enemies visible | 6 to 12 active, depending complexity |
| Draw calls / batches | Keep practical for 60 FPS on 4 GB VRAM GPUs |
| Realtime lights | Few, purposeful, mostly no realtime shadows |
| Texture memory | Target under 2 GB active scene VRAM |
| Audio memory | Mostly streamed/compressed for music/ambience, short clips compressed |
| Install size | Prefer under 8 GB for v1.0 unless content scope expands |

Texture guidance:

- Hero first-person weapons: 2048 base color/normal/ORM, 1024 fallback.
- Major enemies: 2048 for LOD0, 1024 fallback.
- Common props: 512 to 1024.
- Modular architecture: trim sheets at 1024 or 2048.
- Decals/signage: 512 to 1024, atlas where possible.

Mesh guidance:

- First-person weapon LOD0: 8k to 20k triangles.
- Enemy LOD0: 5k to 20k triangles depending role.
- Common prop LOD0: 300 to 3k triangles.
- Large modular wall/floor pieces: simple geometry plus trims.
- Use occlusion-friendly room construction.

Build settings:

- Windows x86_64.
- Default target frame rate 60.
- VSync off for automated tests; user-facing graphics option later.
- Prefer baked lighting and reflection probe discipline.
- Keep quality profiles data-driven.

Opportunity:

- Windows can carry the full art direction and widest content scope while proving mechanics before ports.

Risk:

- If Windows assets are created as huge one-off meshes/materials, every later platform will inherit expensive cleanup.

### Android Phone

Role:

- Future simplified portable version.
- Best treated as a curated campaign slice or optimized edition unless v1.0 content proves small enough.

Hardware planning profile:

- Mid-range Android phones.
- 4 GB to 8 GB device RAM class.
- Thermal throttling expected.
- Touch input primary, controller optional.

Performance:

- 30 FPS minimum.
- 60 FPS preferred on stronger devices.
- Keep sessions and level loads shorter than Windows.

Runtime budgets:

| Category | Target |
| --- | --- |
| App memory working set | Prefer under 1.5 GB, hard review above 2 GB |
| Texture memory | Prefer under 512 MB active scene |
| Build/download size | Prefer under 1.5 GB, lower if store strategy requires |
| Visible scene triangles | 75k to 150k typical |
| Active enemies | 3 to 6 typical |
| Realtime shadows | Avoid or severely limit |
| Particle counts | Low, pooled, short-lived |

Texture guidance:

- Hero weapons: 1024, 512 fallback.
- Enemies: 1024 for main, 512 fallback.
- Common props: 256 to 512.
- Architecture: 512 to 1024 trim sheets.
- Use ASTC compression where available.

Mesh guidance:

- Weapon LOD0: 4k to 8k triangles.
- Enemy LOD0: 3k to 8k triangles.
- Common prop LOD0: 150 to 1.5k triangles.
- Aggressive LOD distances.
- Merge static geometry where it reduces overhead without breaking culling.

Input:

- Virtual left stick.
- Swipe look region.
- Fire, alternate fire, interact, weapon switch, pause.
- Adjustable sensitivity.
- Optional aim assist and larger interact volumes.

UI/HUD:

- Larger buttons.
- Reduced HUD density.
- Touch-safe margins.
- Pause/settings accessible with one thumb.
- Objective text short and legible.

Build settings:

- Android build support installed later.
- IL2CPP.
- ARM64.
- Mobile-friendly render pipeline settings.
- ASTC texture compression target.
- Reduced quality profile.
- Disable expensive post-processing.

Opportunity:

- A strong mobile slice can expose the game to a wider audience if controls are tuned specifically for touch.

Risk:

- Fast classic FPS movement can feel poor on touch unless encounter layouts, aim assist, and enemy pressure are adjusted.

### WebGL Browser

Role:

- Future quick-access browser demo or simplified edition.
- Best used as a marketing/playable sample, not necessarily the complete v1.0 campaign.

Hardware planning profile:

- Desktop browsers first.
- Keyboard/mouse and pointer lock.
- Gamepad optional.
- Memory limits vary heavily by browser and machine.

Performance:

- 30 FPS minimum.
- 60 FPS preferred on desktop browsers.
- Fast initial load is critical.

Runtime budgets:

| Category | Target |
| --- | --- |
| Compressed initial download | Prefer under 300 MB for demo, lower is better |
| Runtime memory | Prefer under 1 GB, review above 1.5 GB |
| Texture memory | Prefer under 384 MB active scene |
| Visible scene triangles | 75k to 150k typical |
| Active enemies | 3 to 6 typical |
| Audio | Compressed, limited simultaneous voices |

Texture guidance:

- Hero weapons/enemies: 1024 maximum for demo.
- Common props: 256 to 512.
- Architecture: 512 to 1024 shared trims.
- Avoid excessive normal/ORM maps for tiny props.

Mesh guidance:

- Similar to Android, with extra care for startup payload.
- Prefer reusable modular sets.
- Avoid huge unique level meshes.

Input:

- Click-to-focus.
- Pointer lock onboarding.
- Escape behavior must handle browser pointer unlock plus in-game pause.
- Keyboard/mouse primary.
- Gamepad optional.

UI/HUD:

- Same layout family as Windows.
- Add explicit focus/pointer-lock state.
- Avoid tiny text that gets blurred by browser scaling.

Build settings:

- WebGL build support installed later.
- Compression enabled.
- Decompression fallback plan if hosting constraints require it.
- Strip unused shader variants.
- Disable threading-dependent assumptions unless WebGL configuration supports them.
- Avoid platform APIs unavailable in browser.

Opportunity:

- WebGL can become a shareable demo with low friction.

Risk:

- Build size, memory spikes, and shader variant bloat can make the browser version fail before gameplay even starts.

### SteamVR / PC VR

Role:

- Future premium VR version for PC headsets.
- Can use higher fidelity than Quest, but frame timing is stricter than flat Windows.

Hardware planning profile:

- VR-capable gaming PC.
- OpenXR preferred.
- Headset refresh targets vary, but 90 FPS should be the preferred target.

Performance:

- 90 FPS preferred.
- 72/80 FPS fallback depending headset and settings.
- Avoid hitches, forced camera motion, and heavy transparent overdraw.

Runtime budgets:

| Category | Target |
| --- | --- |
| Frame time | About 11.1 ms for 90 FPS |
| Visible scene triangles | 200k to 400k typical, profile by headset |
| Active enemies | 4 to 8 typical, tuned for comfort/readability |
| Texture memory | Similar to Windows, but watch stereo cost |
| Realtime lights | Few, no careless shadows |
| Transparent VFX | Strictly limited near camera |

Texture guidance:

- Can use Windows texture tier for hero assets.
- Reduce noisy high-frequency surface detail that shimmers in headset.
- Prefer readable silhouettes and material contrast over tiny decals.

Mesh guidance:

- Proper scale is mandatory.
- Close-view props need clean geometry and normals.
- LOD popping is more noticeable in VR; use careful thresholds.
- Avoid tiny snag geometry around player movement spaces.

Input:

- Head pose controls camera.
- Controller pose controls weapon aim.
- Dominant-hand trigger for primary fire.
- Grip or secondary input for alternate fire.
- Interact through hand proximity or ray.
- Snap turn and optional smooth turn.
- Seated/standing support decision later.

Comfort:

- No forced head rotation.
- Camera shake disabled or comfort-scaled.
- Head bob disabled.
- Optional vignette/tunneling for smooth locomotion.
- Snap turn default.
- Teleport/dash fallback should be considered.
- Enemy melee should not crowd the player's face.
- Weapon recoil must not move the camera.

UI/HUD:

- Wrist-mounted objective/status option.
- Weapon-mounted ammo/gauge displays.
- World-space interaction prompts.
- Boss health as world-space/diegetic machinery gauge where feasible.
- Pause/settings as VR-safe panel.

Build settings:

- OpenXR packages added later on a branch.
- XR rig scene variant or rig injection.
- Single-pass instanced rendering where supported.
- MSAA likely useful; balance against cost.
- VR-specific quality profile.

Opportunity:

- The steampunk weapon/gauge fantasy maps naturally to VR hands, wrist gauges, and tactile valves.

Risk:

- Current flat-screen weapon code that assumes screen-center raycasts will become costly to unwind if not abstracted early.

### Meta Quest Standalone VR

Role:

- Future standalone VR edition for Meta Store.
- Similar asset constraints to Android, with stricter frame-rate and comfort demands.

Hardware planning profile:

- Standalone mobile VR headset.
- OpenXR/Meta tooling likely.
- Thermal, GPU, memory, and storage constraints are serious.

Performance:

- 72 FPS minimum planning target.
- Higher refresh modes only after profiling.
- Stable frame pacing is more important than visual density.

Runtime budgets:

| Category | Target |
| --- | --- |
| Frame time | About 13.9 ms for 72 FPS |
| App memory working set | Prefer under 1.5 GB |
| Texture memory | Prefer under 384 to 512 MB active scene |
| Build size | Prefer under 2 GB unless store/package strategy changes |
| Visible scene triangles | 75k to 150k typical |
| Active enemies | 3 to 5 typical |
| Realtime shadows | Avoid or use very selectively |
| Transparent VFX | Very limited, especially full-screen/near-camera |

Texture guidance:

- Hero weapons: 1024, use 512 fallback.
- Enemies: 512 to 1024.
- Common props: 256 to 512.
- Architecture: 512 shared trims.
- Use mobile compression and atlas planning.

Mesh guidance:

- Weapon LOD0: 4k to 8k triangles.
- Enemy LOD0: 3k to 8k triangles.
- Common prop LOD0: 100 to 1k triangles.
- Strong LOD chain required.
- Simplify collision aggressively.

Input:

- Head pose camera.
- Motion-controller weapon aim.
- Snap turn default.
- Smooth locomotion optional with comfort vignette.
- Teleport/dash fallback recommended.
- Interactions must work at VR hand/controller distance.

Comfort:

- Highest comfort discipline of all target platforms.
- No forced camera motion.
- No intense full-screen damage flashes.
- Avoid flickering lights and dense steam clouds near the face.
- Keep interactables large and reachable.
- Keep enemy attacks readable from comfortable distance.

UI/HUD:

- Diegetic gauges.
- Wrist UI.
- Large readable world labels.
- Avoid flat HUD locked to face.
- Minimize text entry.

Build settings:

- Android/Quest toolchain installed later.
- ARM64.
- OpenXR/Meta configuration.
- Mobile VR quality profile.
- Fixed foveated rendering evaluation.
- Baked lighting.
- Shader variant stripping.
- Mobile-friendly post-processing only if budget allows.

Opportunity:

- The brass gauge and valve language can produce a distinctive VR interface without inventing a separate fantasy.

Risk:

- Quest is the harshest target: any Windows art pipeline that depends on high overdraw, many realtime lights, or giant unique textures will not survive.

## 6. Asset Quality Tiers

The asset pipeline should plan five tiers from the start.

| Tier | Purpose | Platforms | Notes |
| --- | --- | --- | --- |
| Source | DCC master/high poly | Not shipped | Sculpt/high-detail source, authoring only |
| Tier A | Full Windows/PC VR LOD0 | Windows, SteamVR | Highest runtime tier, still optimized |
| Tier B | Windows low/Web high | Windows low, WebGL | Reduced textures, reduced mesh detail |
| Tier C | Android/Quest | Android, Meta Quest | Mobile shaders, smaller textures, stronger LOD |
| Tier D | Distant/low LOD | All | Silhouette only, cheap materials |

Naming convention proposal:

- `AssetName_SRC`
- `AssetName_LOD0_PC`
- `AssetName_LOD1_PC`
- `AssetName_LOD0_Mobile`
- `AssetName_LOD1_Mobile`
- `AssetName_MAT_PC`
- `AssetName_MAT_Mobile`
- `AssetName_Tex_Atlas_PC`
- `AssetName_Tex_Atlas_Mobile`

Side-agent asset work should produce asset manifests before producing final imports.

## 7. Texture, Material, Mesh, And LOD Budgets

### Texture Budgets By Asset Type

| Asset Type | Windows / PC VR | Android | WebGL | Quest |
| --- | --- | --- | --- | --- |
| First-person weapon | 2048 | 1024 | 1024 | 1024 |
| Boss enemy | 2048 | 1024 | 1024 | 1024 |
| Standard enemy | 1024 to 2048 | 512 to 1024 | 512 to 1024 | 512 to 1024 |
| Common prop | 512 to 1024 | 256 to 512 | 256 to 512 | 256 to 512 |
| Architecture trim | 1024 to 2048 | 512 to 1024 | 512 to 1024 | 512 |
| Decal/signage | 512 to 1024 | 256 to 512 | 256 to 512 | 256 to 512 |
| VFX flipbook | 512 to 1024 | 256 to 512 | 256 to 512 | 256 to 512 |

Texture set rules:

- Prefer base color, normal, ORM packed map for PC tiers.
- Mobile/Web/Quest should use fewer maps when visual value is low.
- Pack occlusion/roughness/metallic where possible.
- Use trim sheets and atlases for brass/copper/iron/walnut/stone families.
- Avoid unique 4k textures in normal gameplay spaces.

### Mesh Budgets By Asset Type

| Asset Type | Windows / PC VR LOD0 | Android/Web/Quest LOD0 | LOD1 | LOD2 |
| --- | --- | --- | --- | --- |
| First-person weapon | 8k to 20k tris | 4k to 8k tris | 50 percent | 20 percent |
| Standard enemy | 5k to 15k tris | 3k to 8k tris | 50 percent | 20 percent |
| Boss enemy | 15k to 35k tris | 8k to 15k tris | 50 percent | 20 percent |
| Common prop | 300 to 3k tris | 100 to 1.5k tris | 40 percent | silhouette |
| Large machine prop | 3k to 15k tris | 1k to 5k tris | 50 percent | silhouette |
| Architecture module | 200 to 3k tris | 100 to 1.5k tris | optional | optional |

LOD rules:

- LOD0 should read well at gameplay range.
- LOD1 should preserve silhouette and remove internal bolts/tiny pipes.
- LOD2 should preserve mass and color only.
- VR LOD transitions need softer thresholds than flat screen.
- Enemies need stable silhouettes at distance more than tiny surface detail.

### Material Families

Core material families:

- Aged brass.
- Polished brass affordance.
- Oxidized copper.
- Riveted black iron.
- Oiled walnut.
- Soot brick.
- Wet stone.
- Cream enamel.
- Gauge glass.
- Furnace ceramic.
- Hot pressure glow.
- Steam vapor.

Material rules:

- Keep shader families shared.
- Prefer material variants over unique shaders.
- Use emission sparingly for readable affordances and danger states.
- Avoid complex layered shaders on mobile/Web/Quest.
- Avoid expensive transparent materials in dense steam scenes.
- Use simple alpha clipping for grates where possible instead of blended transparency.

## 8. Shader And Rendering Guidance

### Shared Direction

- Baked lighting first.
- Limited realtime lights.
- Clear color-coded affordances.
- Strong silhouette readability.
- Avoid shader variant explosion.
- Prefer stylized material response over physically expensive realism.

### Windows / PC VR

- Can support normal maps, ORM maps, emissive accents, light probes, and modest post-processing.
- Use screen-space effects only where value is obvious and scalable.
- Keep steam VFX pooled and short-lived.

### Android / WebGL / Quest

- Use mobile-friendly shader variants.
- Reduce or remove normal maps on small props.
- Avoid parallax, expensive subsurface, layered materials, and heavy refraction.
- Reduce transparent overdraw.
- Use baked emissive cues instead of many realtime lights.

### VR-Specific Rendering

- Avoid high-frequency texture shimmer.
- Avoid flickering emissives near the player.
- Avoid large full-screen flashes.
- Steam near face must be sparse and readable.
- Keep weapons visually rich but not visually noisy.

## 9. UI/HUD Adaptation Plan

### Shared HUD Data

Required display data:

- Health.
- Ammo.
- Current weapon.
- Alternate-fire readiness.
- Gear key / objective item state.
- Interact prompt.
- Objective.
- Boss health.
- Secret progress.
- Damage direction or warning.
- Pause/settings.

### Windows

- Compact brass screen-space HUD.
- Crosshair.
- Boss health gauge.
- Objective text short and persistent.

### Android

- Larger touch HUD.
- Virtual stick and fire/interact controls.
- Fewer decorative HUD elements.
- Bigger prompts.
- Optional auto-hide objective text.

### WebGL

- Similar to Windows HUD.
- Add pointer-lock/focus affordance.
- Escape handling must distinguish pointer unlock from pause.

### SteamVR / Quest

- No face-locked flat HUD as the only interface.
- Weapon-mounted ammo pressure gauge.
- Wrist objective/status display.
- World-space interact prompts.
- Boss state as diegetic core machinery gauge if possible.
- Pause menu placed comfortably in world space.

## 10. VR Comfort Requirements

VR comfort constraints must influence current Windows systems.

Required:

- No mandatory head bob in VR.
- No forced camera shake in VR.
- No forced camera rotation.
- No forced FOV kick.
- No full-screen flashing damage overlay in VR.
- Snap turn support.
- Optional smooth turn.
- Optional vignette for smooth locomotion.
- Teleport or dash fallback evaluation.
- Enemy melee range must avoid face clipping.
- All interactables should be reachable/readable at real scale.
- Text must be legible without leaning into walls.

Recommended:

- Comfort preset: Comfortable, Standard, Intense.
- Dominant hand setting.
- Seated/standing calibration decision.
- Weapon angle calibration.
- Height recenter option.

## 11. Build Settings Checklist By Platform

### Windows

- Target: Windows x86_64.
- Scripting backend: current project default, review before v1.
- Quality profile: Windows mid/low.
- Target frame rate: 60.
- Fullscreen/windowed options later.
- Graphics options: resolution, quality tier, sensitivity, audio volume.

### Android

- Target: Android ARM64.
- Scripting backend: IL2CPP.
- Texture compression: ASTC preferred.
- Quality profile: Android phone.
- Target frame rate: 30 default, 60 option if stable.
- Touch controls enabled.
- Controller support optional.

### WebGL

- Target: WebGL.
- Compression enabled.
- Shader stripping required.
- Quality profile: WebGL browser.
- Pointer lock flow required.
- Initial content payload minimized.

### SteamVR

- Target: Windows x86_64 with OpenXR.
- Quality profile: PC VR.
- XR rig enabled.
- Single-pass instanced rendering where supported.
- Snap turn default.
- Comfort options available.

### Meta Quest

- Target: Android ARM64 with OpenXR/Meta tooling.
- Quality profile: Meta Quest.
- Texture compression: ASTC.
- Fixed foveated rendering evaluation.
- Baked lighting.
- Strict shader stripping.
- Stable 72 FPS target.

## 12. Memory And Storage Budget Summary

| Platform | Runtime Memory Target | Texture Memory Target | Download/Install Target |
| --- | --- | --- | --- |
| Windows mid/low | Fits 8 GB system RAM, 4 GB VRAM | Under 2 GB active scene VRAM | Prefer under 8 GB |
| Android phone | Under 1.5 GB preferred | Under 512 MB active scene | Prefer under 1.5 GB |
| WebGL browser | Under 1 GB preferred | Under 384 MB active scene | Prefer under 300 MB for demo |
| SteamVR PC | Similar to Windows, stricter frame time | Under 2 GB active scene VRAM | Similar to Windows |
| Meta Quest | Under 1.5 GB preferred | 384 to 512 MB active scene | Prefer under 2 GB |

Budget enforcement ideas:

- Add asset audit script later for texture max size and import compression.
- Add scene budget report later for active renderers/material count/lights.
- Add LOD coverage report later.
- Add build-size report per platform.
- Add memory smoke profiles once platform builds exist.

## 13. Test Matrix Additions

Current Windows matrix should remain the main gate. Add new tests in layers.

### Now: Editor/Windows Tests That Protect Future Ports

- Input-intent smoke: verify gameplay can be driven by intent injection, not only direct keyboard/mouse.
- HUD-data smoke: verify HUD state exists independently from screen-space presentation.
- Interactable contract smoke: verify interactables expose prompt, range, and interaction result.
- Asset-budget report: list textures above target size, meshes without LODs, materials using expensive shader families.
- Scene scale audit: verify doors, corridors, interactables, enemies, and weapon positions stay plausible for VR scale.
- Camera comfort audit: list active camera shake, FOV kicks, head bob, and full-screen flashes.

### Android Future Matrix

- Build APK/AAB smoke.
- Launch on physical mid-range phone.
- Touch input smoke.
- UI safe-area smoke.
- Thermal 10-minute combat loop.
- Memory capture after level load and after combat.
- Load-time measurement.

### WebGL Future Matrix

- WebGL build smoke.
- Local hosted browser launch.
- Pointer lock smoke.
- Escape/pause smoke.
- Memory/load-time check.
- Chrome and Edge smoke.
- Gamepad smoke if enabled.

### SteamVR Future Matrix

- OpenXR build smoke.
- XR rig launch smoke.
- Controller aim/fire smoke.
- Interact smoke.
- Snap turn/smooth locomotion smoke.
- Comfort options smoke.
- 10-minute frame timing pass.

### Meta Quest Future Matrix

- Quest build/install smoke.
- Headset launch smoke.
- Controller aim/fire smoke.
- Comfort settings smoke.
- 10-minute thermal/frame pacing pass.
- Memory capture.
- Store-readiness checklist later.

## 14. Risks That Should Influence Current Windows Development

### Risk 1: Screen-Center Weapon Assumptions

If weapons permanently assume screen-center aim, VR controller aiming will require a rewrite. Use an aim provider abstraction soon.

### Risk 2: HUD Coupled To Desktop Canvas

If gameplay state only exists inside a Windows HUD component, Android and VR UI will duplicate logic. Keep HUD state separate from presentation.

### Risk 3: Unbounded AAA Asset Detail

High-detail steampunk assets are welcome, but every asset needs LODs, texture tiers, and shared material families. One-off giant assets will slow all ports.

### Risk 4: Camera Effects That Break VR

Head bob, recoil camera kick, full-screen flashes, FOV pulses, and forced camera motion should be optional or presentation-layer-only.

### Risk 5: Dense Transparent Steam

Steam is core to the style, but transparent overdraw is costly on WebGL, mobile, and VR. Steam VFX need tiered particle counts and mobile variants.

### Risk 6: Large Monolithic Levels

Large levels may work on Windows but fail on WebGL/mobile/Quest memory. Build levels from segments that can be simplified, culled, or split later.

### Risk 7: Tiny Text And Micro-Props

Tiny gauge text, small buttons, and delicate prop detail will not work equally across phone, browser, and VR. Use readable shapes and optional close-detail.

### Risk 8: Store-Specific Late Surprises

SteamVR and Meta Quest will need comfort, performance, and input compliance. Early comfort requirements reduce late rework.

## 15. Opportunities For Parallel Agents

### Platform Budget Agent

Deliverables:

- Asset budget spreadsheet or markdown table.
- Per-platform texture size recommendations.
- Per-platform mesh/LOD recommendations.
- Audit checklist for imported assets.

No dependency on current gameplay.

### Input Architecture Agent

Deliverables:

- Input intent schema.
- Mapping table for Windows, touch, WebGL, gamepad, VR controllers.
- Test injection proposal.
- Migration plan from direct input reads.

Depends only on current controls list.

### VR Comfort Agent

Deliverables:

- Comfort settings specification.
- Locomotion decision matrix.
- VR interaction scale guide.
- List of current systems to make comfort-toggleable.

No dependency on final levels.

### UI Adaptation Agent

Deliverables:

- HUD data model.
- Windows, Android, WebGL, and VR HUD variants.
- Touch layout wire notes.
- Wrist/weapon/world-space VR UI notes.

Depends on current HUD data, but not final art.

### Asset Pipeline Agent

Deliverables:

- Steampunk material library plan.
- Trim-sheet plan.
- LOD naming/import rules.
- Texture packing rules.
- Incoming asset review checklist.

Can start immediately.

### Build/Test Matrix Agent

Deliverables:

- Proposed platform build commands.
- Future CI/test matrix outline.
- Smoke test pass-marker naming scheme.
- Device test checklist.

Can start before actual platform builds.

## 16. Near-Term Recommendations To Main Windows Track

1. Introduce or document a gameplay-intent input boundary before adding many more weapons.
2. Keep weapon hit logic separate from camera and viewmodel presentation.
3. Keep HUD state separate from screen-space brass HUD rendering.
4. Add a lightweight asset budget audit before importing large asset packs.
5. Require LOD and texture-tier notes for every new AAA-quality asset.
6. Avoid hard-coded forced camera motion in weapon, damage, or hazard feedback.
7. Keep interactables large, readable, and prompt-driven.
8. Build levels from modular pieces that can be split or simplified.
9. Keep steam VFX tiered and pooled.
10. Use the steampunk north-star style, but author assets for scalability from day one.

## 17. Done Criteria For This Side Plan

This document is complete when it gives the main project:

- Clear target constraints for Windows, Android, WebGL, SteamVR, and Meta Quest.
- Actionable architecture rules for input, UI, camera, interactions, and assets.
- Asset budget tables that can guide independent asset-pack generation.
- Test matrix additions for future platform verification.
- A list of platform risks that should shape Windows development now.

