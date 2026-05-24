# Pressure Pistol Acceptance Gates

Use these gates before calling any pressure-pistol output "closer," "high fidelity," or "acceptable visual direction." A planning image may pass planning gates while failing render gates.

## Gate 0: Scope And Label

| Check | Pass Criteria | Current Status |
| --- | --- | --- |
| Write scope | New/changed files remain in `Documentation/AssetProduction/HighFidelityLookdevRecovery/`, `Documentation/ConceptRenders/`, or optional isolated staging. | Open until final status check |
| Subject focus | Output is pressure-pistol-only; corridor and enemy are not active proof targets. | Complete |
| Honest label | Planning images say planning/reference; proof renders say proof/nonshipping until accepted. | Complete for Recovery02 planning sheet |
| No gameplay edits | No gameplay scripts, gameplay scenes, build docs, generated scenes, or previous HFLD files are edited. | Open until final status check |

## Gate 1: Component Count

| Component | Minimum For Proof Render |
| --- | --- |
| Main blackened barrel | 1 large upper cylinder with caps/collars/seams |
| Lower pressure tank | 1 separate lower cylinder with brackets and capped ends |
| Top pressure gauge | 1 readable gauge with brass bezel, cream face, red needle, glass highlight, tick marks |
| Coil window | 1 exposed window/housing with at least 6 visible coil turns |
| Muzzle assembly | 1 nested muzzle/nozzle stack with at least 3 rings or stepped forms |
| Trigger and guard | 1 readable trigger plus guard/loop |
| Grip/glove area | 1 leather grip or gloved hand mass visible in lower-right frame |
| Steam/pressure ports | At least 2 visible ports/sockets |
| Top valves/caps | At least 2 small top details |
| Plates/brackets | At least 8 distinct plates, straps, brackets, or mount elements |
| Fasteners | At least 60 visible screws/rivets/bolts |

## Gate 2: Material And Texture Detail

| Check | Pass Criteria |
| --- | --- |
| Material slot budget | Production-minded proof uses no more than 6 material slots on the gun body, while still showing 8 visual roles through masks/texture regions. |
| Visual material roles | Blackened iron, aged brass, darker pipe metal, hot copper coil, cream gauge face, glass highlight, dark leather, soot/grime are all visible. |
| Texture maps | Credible proof includes base color, normal/bump, roughness, metallic, and AO or equivalent procedural channels. |
| Detail scale | Rivets, tick marks, coil edges, leather wrinkles, seams, scratches, and grime remain readable at 1920x1080. |
| Edge highlights | Barrel caps, gauge rim, coil housing, lower tank, brackets, and muzzle all catch bevel/specular highlights. |
| Weathering | Brass has tarnish and polished edges; iron has scratches and dark roughness breakup; leather has creases and worn highlights. |

## Gate 3: Camera And Composition

| Check | Pass Criteria |
| --- | --- |
| Camera angle | 3/4 first-person angle, not orthographic side view. |
| Muzzle direction | Muzzle points left or left-forward, with visible foreshortening. |
| Gauge readability | Gauge remains unobstructed and readable at normal review size. |
| Coil readability | Coil window remains unobstructed and visually hot/warm. |
| Hand scale | Leather glove/grip area anchors lower-right frame and communicates first-person scale. |
| Frame occupancy | Gun occupies 55-80% of image width and 45-70% of image height. |
| Background | Dark smoky background supports silhouette without becoming the subject. |

## Gate 4: Lighting And Contrast

| Check | Pass Criteria |
| --- | --- |
| Key light | Warm amber key from upper-left/front-left, approximately 1800K-2600K visual feel. |
| Fill | Low cool/dark fill preserves blackened barrel and lower tank silhouette. |
| Rim/specular | Highlights visible on gauge rim, barrel caps, coil housing, muzzle, lower tank, and brass frame. |
| Coil/gauge accents | Coil is warm copper/hot accent; gauge face remains cream, not over-bloomed. |
| Contrast | Render has deep shadows, bright highlight islands, and readable midtone metal forms. |
| Steam | Any steam comes from visible ports and does not cover the gauge, coil, or silhouette. |

## Gate 5: Resolution And File Checks

| Check | Pass Criteria |
| --- | --- |
| Planning sheet | At least 1536x1024. |
| Hero proof render | At least 1920x1080. |
| File naming | Use `HFLDR` or `Recovery` and include `pressure_pistol`; include `planning` when not a proof. |
| Output location | `Documentation/ConceptRenders/`. |
| Render notes | Proof render has a companion note naming tool, source assets, material setup, camera angle, light setup, and known gaps. |

## Gate 6: Human Review

Pass only if reviewers can answer yes to all:

- Does this read as the same pressure pistol as the source concept when labels are removed?
- Is it obviously closer to the source than Batch01?
- Does the silhouette feel chunky, layered, first-person, and steampunk-mechanical?
- Do the gauge, coil window, blackened barrel, aged brass frame, lower tank, leather grip/glove, rivets, grime, and ports all read?
- Do the materials feel dimensional and worn rather than flat graphic fills?
- Is the lighting dramatic and warm without hiding essential details?

## Current Gate Status

- Gate 0: mostly complete; final scope check still required after edits.
- Gate 1: defined, not satisfied by any current render.
- Gate 2: defined, not satisfied by any current render.
- Gate 3: defined, not satisfied by Batch01; target breakdown complete.
- Gate 4: defined, not satisfied by Batch01.
- Gate 5: planning sheet complete; hero proof render open.
- Gate 6: open.

