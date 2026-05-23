# Arcology Breach - Local Asset Pack Review

Last updated: 2026-05-22 20:54 -04:00

## Purpose

The user has many Unity Asset Store packs installed locally and more available through the Unity account. This file tracks what has been reviewed, what is likely useful, and the import policy so the project can benefit from existing assets without becoming bloated or visually incoherent.

## Local Cache Reviewed

Local Asset Store cache path:

`C:\Users\Gabe\AppData\Roaming\Unity\Asset Store-5.x`

The cache contains downloaded `.unitypackage` files. Account-owned packs that are not downloaded may require the Unity Editor Package Manager or Asset Store login to inspect and install.

## Import Policy

- Import only milestone-sized subsets.
- Prefer small prototype kits, single props, skyboxes, VFX, and audio utilities before large demo scenes.
- Avoid importing complete projects into the main `Assets` tree unless they are isolated in a quarantine folder first.
- Keep all third-party content under clear vendor folders and document license/source.
- Re-skin and art-direct assets to the `Arcology Breach` cyberpunk language before using them as final content.
- Before committing, verify package imports did not add unnecessary sample scenes, docs, videos, or huge textures.
- Android, browser/WebGL, and VR compatibility must be checked before adopting any asset as core.

## High-Value Candidates

| Pack | Size | Use | Initial Decision |
| --- | ---: | --- | --- |
| Snaps Prototype Sci-Fi Industrial | 3.9 MB | Modular sci-fi blockout and corridor reference. | Strong candidate for v0.2/v0.3 layout prototyping. |
| Snaps Prototype Sci-Fi Military Base | 3.9 MB | Additional modular sci-fi greybox pieces. | Strong candidate for level-kit reference. |
| BlockOut Prototype Kit | 23.6 MB | General blockout shapes. | Useful for rapid level-map scale tests. |
| Space Robot Kyle | 9.9 MB | Humanoid robot reference or temporary enemy body. | Candidate for Scrapper/Lancer placeholder study. |
| Weapon Skins - FPS Microgame Add-Ons | 16.3 MB | Weapon material/skin reference. | Candidate for Pulse Pistol placeholder styling. |
| Fireballer - FPS Microgame Add-Ons | 12.4 MB | FPS weapon/FX reference. | Candidate for weapon/VFX study only. |
| Sniper Rifle - FPS Microgame Add-Ons | 0.4 MB | Weapon mesh reference. | Small candidate for weapon-scale study. |
| Unity Particle Pack 5x | 97.3 MB | Sparks, smoke, impact VFX. | Candidate for impact and machine death VFX. |
| Unity Particle Pack | 139.3 MB | Additional VFX samples. | Candidate, import selectively. |
| Volumetric Lines | 0.7 MB | Laser beams, scanlines, neon accents. | Strong candidate for cyberpunk targeting and signage. |
| Camera Shake FX | 28.1 MB | Weapon and damage feedback. | Candidate if current custom camera shake is insufficient. |
| Impact - Physics Interaction System | 21.9 MB | Hit feedback system. | Candidate for later physics-rich impacts, not v0.2 blocker. |
| Japanese Alley | 425.3 MB | Cyberpunk-adjacent urban alley art reference. | Potential visual reference, import only if needed. |
| NYC Block 6 | 428.5 MB | Urban block reference. | Potential distant city/reference, not current indoor slice. |
| Windridge City | 680.8 MB | City environment reference. | Defer due to size. |
| Skybox Series Free | 274.9 MB | Skybox backgrounds. | Candidate for later exterior/transit levels. |
| 8K Skybox Pack Free | 262.8 MB | Skybox backgrounds. | Candidate, but downscale for browser/mobile. |
| AllSky Free | 319.9 MB | Skybox set. | Candidate for Windows only; optimize variants. |
| VR Interaction Framework | 76.7 MB | VR interaction reference. | Defer, useful when VR branch begins. |
| Auto Hand - VR Physics Interaction | 18.1 MB | VR hand interaction reference. | Defer, useful for Meta/SteamVR evaluation. |
| Hurricane VR | 49.9 MB | VR physics interaction toolkit. | Defer, evaluate before VR prototype. |
| Oculus Integration | 377.4 MB | Meta/Oculus SDK integration. | Defer until Quest planning phase. |
| VRTK | 6.6 MB | VR toolkit reference. | Defer; may be outdated but useful for concepts. |
| VR Shooter Kit | 297.3 MB | VR shooter reference. | Defer, inspect only in isolated test project if needed. |

## Low-Fit or Deferred Packs

- Large nature, terrain, medieval, fantasy, food, race track, and non-cyberpunk packs should not be imported into the main project unless a later level specifically needs them.
- Full Unity demo projects are useful as references but should stay outside the game project unless a small, specific asset is selected.

## Next Review Steps

1. For v0.2/v0.3, inspect the two Snaps Prototype Sci-Fi packs and `Volumetric Lines` first.
2. If imported, place assets under a clearly named vendor folder and document the import in this file.
3. Update `AAA_ASSET_CATALOG.md` rows when a pack becomes the source of an asset or prototype.
4. Add any new work discovered during import to `WORK_LEDGER.md`.
