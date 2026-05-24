# Pressure Pistol Component Acceptance Gates

Status: active Unity component gate checklist  
Subject: pressure pistol only  
Source: `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`, lower-right pistol panel  
Companion plan: `Documentation/AssetProduction/HighFidelityLookdevRecovery/UnityPressurePistolProof/PRESSURE_PISTOL_COMPONENT_BUILD_PLAN.md`

## Gate Model

Each component moves through these states:

| State | Meaning |
| --- | --- |
| Not Started | No Unity proof exists. |
| In Proof | Component has a Unity-only render or temporary-scene test, but gates are not all passed. |
| Accepted For Assembly | Component passes this file's measurable gates and has an assembly note. |
| Needs Revision | Component is recognizable but fails one or more required checks. |
| Rejected For Composition | Component cannot be used in the full weapon until rebuilt or reworked. |

The full pressure pistol composition should not start until material calibration, gauge, coil, barrel/tank, muzzle, and grip/hand are accepted for assembly. Fasteners/plates and steam/smoke can be refined during integration, but each still needs its own proof before final review.

## Universal Gates

| Check | Pass Criteria |
| --- | --- |
| Unity-only lane | Component is developed, tested, and rendered in Unity. No Blender or external render tools are used. |
| Scope safety | No gameplay scenes, generated level scenes, player controllers, combat scripts, runtime managers, or build settings are changed for the component proof. |
| Source identity | Component is judged against the lower-right pressure pistol in the north-star concept, not against an invented generic steampunk gun. |
| Review render | Component proof includes at least one 1920x1080 Unity render or contact sheet. |
| Count ledger | Component proof records the relevant counts: ticks, turns, rings, straps, plates, fasteners, ports, smoke puffs, or material roles. |
| Material roles | Component uses the accepted material calibration roles unless a material board update is explicitly approved. |
| Assembly note | Component documents where it attaches to `PressurePistolRoot` and which other modules it must not obscure. |
| Honest label | Proof outputs remain labeled proof, component proof, or nonshipping until human review accepts the full weapon. |

## Required Work Order Gate

| Step | Pass Criteria |
| --- | --- |
| 1. Material swatches | Material board passes before component hero renders are judged. |
| 2. Gauge and coil | Gauge and coil each pass alone and at planned full-weapon scale. |
| 3. Barrel/tank/muzzle | Structural silhouette passes before grip/hand or steam are used to sell the image. |
| 4. Grip/hand | First-person grip/hand module passes with trigger and guard visible. |
| 5. Full composition | Full weapon uses accepted modules and fails back to a named component if the render does not pass. |

## Material Calibration Gate

| Check | Pass Criteria |
| --- | --- |
| Role count | At least 9 roles are visible: aged brass, blackened iron, dark pipe metal, hot copper, cream gauge face, glass, walnut or leather, soot/grime, smoke. |
| Brass vs copper | Brass and hot copper are visibly different in hue, roughness, and highlight behavior. |
| Iron vs pipe metal | Main blackened iron and darker pipe metal do not collapse into one flat black material. |
| Glass | Gauge glass has transparency or a specular highlight while preserving face readability. |
| Leather/wood | Grip material includes grain, creases, seams, or worn edge highlights. |
| Weathering | Tarnish, soot, scratches, and edge wear appear concentrated around seams, recesses, and contact zones. |
| Lighting compatibility | Swatches are tested under the same warm key, low cool fill, rim light, and fog exposure intended for the pistol proof. |
| Review outcome | Reviewer can identify each material role without labels at normal review size. |

## Gauge Module Gate

| Check | Pass Criteria |
| --- | --- |
| Main dial | One large round top-mounted pressure gauge is present. |
| Face | Cream face remains readable at planned full-weapon scale. |
| Tick marks | At least 24 tick marks are visible; target 28. |
| Needle | One red needle is visible and raised or separated from the face. |
| Bezel | Brass bezel reads as a separate rim with highlight. |
| Lens | Glass lens creates a highlight without hiding the dial. |
| Mounting | Gauge has a bracket, casing, or plate that physically attaches to the receiver/barrel frame. |
| Fasteners | At least 4 fasteners appear on the bezel, bracket, or mount. |
| Occlusion rule | Gauge is not hidden by muzzle parts, smoke, coil housing, or top valves in composition preview. |

## Coil Module Gate

| Check | Pass Criteria |
| --- | --- |
| Turns | At least 6 distinct coil turns are visible; target 8. |
| Material | Coil reads as hot copper, not aged brass or orange paint. |
| Housing | Coil sits inside a recessed window or framed cavity. |
| Backing | Dark backplate or shadow layer separates the coil from the body. |
| Frame | Window frame has at least 4 visible fasteners. |
| Heat accent | Emission or warm highlight is present but does not bloom the turns into one shape. |
| Scale preview | Coil remains readable at planned full-weapon scale in 1920x1080. |
| Placement | Coil is on the visible hero side and does not fight the gauge for silhouette space. |

## Barrel And Tank Module Gate

| Check | Pass Criteria |
| --- | --- |
| Main barrel | One blackened upper barrel is present and forms the weapon spine. |
| Lower tank | One separate lower pressure tank is present below the barrel. |
| Separation | Barrel and tank remain distinct at 1080p and in 3/4 view. |
| Barrel detail | Barrel has at least 3 caps, collars, seams, or bands. |
| Tank detail | Tank has capped ends plus at least 2 straps or brackets. |
| Frame logic | Brass rails, plates, or brackets visibly connect the two cylinders. |
| Fasteners | At least 8 fasteners appear on straps, collars, caps, or frame plates. |
| Material response | Barrel and tank have visible roughness, grime, and edge highlights instead of smooth flat cylinders. |
| Anchor readiness | Module leaves clear attachment points for gauge, coil, muzzle, grip, ports, and valves. |

## Muzzle Module Gate

| Check | Pass Criteria |
| --- | --- |
| Ring stack | At least 3 nested rings or stepped forms are visible; target 5 to 6. |
| Aperture | Dark central nozzle or aperture is visible. |
| Depth | Muzzle reads as left-forward 3/4 depth, not a flat side-view disk. |
| Attachment | Muzzle has at least 2 braces, clamps, or fasteners tying it to the barrel. |
| Material balance | Brass highlights support the form without turning the muzzle into a bright gold plug. |
| Scale | Muzzle remains compact enough that the pistol does not read as a rifle. |
| Occlusion rule | Muzzle does not hide the gauge, coil window, or barrel/tank separation in the hero camera. |

## Grip And Hand Module Gate

| Check | Pass Criteria |
| --- | --- |
| Frame anchor | Grip or hand mass anchors the lower-right of the hero frame. |
| Trigger | Trigger is visible as a separate piece. |
| Guard | Trigger guard is visible and not merged with the hand or tank. |
| Material | Walnut, leather, or glove material shows grain, seams, creases, or worn highlights. |
| Receiver mount | Grip attaches through a visible plate, bracket, or frame connection. |
| Scale | Hand/glove scale supports first-person perspective without covering key weapon details. |
| Detail count | At least 4 fasteners, stitches, seams, or plate details appear in the grip/hand module. |
| Occlusion rule | Grip/hand does not hide the lower pressure tank or trigger area in the final camera. |

## Fasteners And Plates Gate

| Check | Pass Criteria |
| --- | --- |
| Full fastener count | Full assembled weapon has at least 60 visible fasteners. |
| Full plate count | Full assembled weapon has at least 8 distinct plates, straps, brackets, or rails; target 24+ for a denser Recovery04-style read. |
| Variation | Fasteners include at least 3 size or silhouette variants. |
| Placement logic | Fasteners land on seams, brackets, collars, plate corners, mounts, or strap ends. |
| Key zones | Gauge, coil frame, barrel collars, lower tank straps, muzzle mount, and grip mount all have construction detail. |
| Perspective | Fasteners follow the component's perspective and do not float as screen-space dots. |
| Readability | Added plates do not cover gauge face, coil turns, muzzle aperture, tank separation, trigger, or grip silhouette. |

## Steam And Smoke Gate

| Check | Pass Criteria |
| --- | --- |
| Port origins | At least 2 visible pressure ports, vents, or sockets exist before steam is added. |
| Source logic | Every major plume originates from a visible port, valve, seam, or nozzle. |
| Material | Smoke/steam uses soft transparent Unity material or particle settings, not opaque gray geometry. |
| Non-obscuring | Steam does not cover the gauge, coil, muzzle aperture, trigger, or grip silhouette enough to harm readability. |
| Depth | Plumes create foreground/background depth instead of flat screen-space stickers. |
| Lighting | Smoke picks up warm/cool scene lighting subtly without becoming the brightest subject. |
| Failure rule | If smoke makes an accepted hard-surface component unreadable, the smoke pass fails. |

## Full Composition Gate

| Check | Pass Criteria |
| --- | --- |
| Accepted parts only | Full weapon uses modules that have passed their component gates, except for explicitly marked temporary placeholders. |
| Camera | First-person 3/4 camera with muzzle pointing left or left-forward. |
| Grip anchor | Grip/hand mass anchors the lower-right frame. |
| Top gauge | Gauge remains readable with cream face, red needle, brass bezel, lens highlight, and tick marks. |
| Coil window | Coil remains readable with at least 6 visible turns and warm copper/hot accent. |
| Barrel/tank | Main barrel and lower pressure tank remain separate cylinders. |
| Muzzle | Muzzle stack shows nested depth and central aperture. |
| Trigger area | Trigger and guard are visible. |
| Construction density | At least 60 fasteners and at least 8 plates/brackets/straps are visible. |
| Material roles | Blackened iron, aged brass, dark pipe metal, hot copper, cream gauge face, glass, walnut/leather, soot/grime, and smoke are visible. |
| Lighting | Warm amber key, low cool fill, and rim/specular highlights preserve dark mood while keeping identity details readable. |
| Frame occupancy | Weapon fills the hero frame without cutting off essential gauge, muzzle, tank, coil, or grip information. If occupancy exceeds roughly 85 percent width or touches the top edge, review camera scale and crop. |
| Human review | Reviewer says it is recognizably closer to the north-star pressure pistol than the failed earlier blockout/proof. |

## Component Failure Taxonomy

Use the most specific failure label possible:

| Failure Label | Meaning |
| --- | --- |
| Silhouette/camera failure | The part only reads from a flat side view or breaks the first-person 3/4 pose. |
| Component-density failure | Counts are too low or details are present but not visually meaningful. |
| Material-response failure | The part has the right color names but not credible Unity material behavior. |
| Gauge/coil readability failure | The two hero details cannot be read at planned full-weapon scale. |
| Assembly-anchor failure | The part has no believable way to attach to the weapon body. |
| Lighting/contrast failure | The component exists but gets lost under the intended lighting. |
| Occlusion failure | Steam, plates, hand, muzzle, or other modules hide a required identity detail. |
| North-star identity failure | The component is technically built but no longer resembles the source pressure pistol. |

## Promotion Rule

A component is accepted for assembly only when all required gates for that component pass and the proof notes name the anchors, material roles, counts, and known gaps. A full weapon render cannot be called successful if any major identity component is failing or hidden.
