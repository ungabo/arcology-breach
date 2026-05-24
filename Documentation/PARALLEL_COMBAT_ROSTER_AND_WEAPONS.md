# Parallel Combat Roster And Weapons Spec

Timestamp: 2026-05-23 21:31 -04:00

Purpose: define a bounded combat-production lane that can be developed in parallel with the main v0 implementation. This document is intentionally production-practical. It describes enemy and weapon intent, asset needs, animation/VFX/audio contracts, tuning fields, automated test hooks, VR constraints, and a safe implementation order.

Write scope for this side-agent pass: documentation only. No scene, script, prefab, material, generated asset, build, or roadmap file changes are assumed by this document.

## Combat Direction

Brassworks Breach should feel like a fast, readable first-person industrial breach through a failing steampunk pressure facility. Combat is built around short-range movement, clear mechanical tells, noisy pneumatic weapons, and enemies whose silhouettes explain their jobs before they attack.

Core rules:

- Every enemy attack needs a readable physical tell before damage.
- Every weapon needs a distinct first-person silhouette, sound, muzzle effect, hit effect, and reload/check state, even if reloads are not part of v0 gameplay.
- Damage readability should come from shape, light, smoke, sparks, pressure rings, and mechanical motion rather than UI text.
- Current v0 primitives should remain valid proxy prefabs until final models replace them.
- New combat content should be data-driven where possible so tests can verify tuning without scene-specific assumptions.
- Final Windows target assumes a mid-to-low gaming PC; assets should support LODs and material consolidation.
- Android, browser, and VR ports should be protected by early control, scale, and performance decisions even if those builds ship later.

## Current Roster Snapshot

### Player Weapons

| Weapon | Current Role | Production Intent | Keep From v0 | Needs Before Final |
| --- | --- | --- | --- | --- |
| Pressure Pistol | Starter precision sidearm | Reliable brass-and-walnut pneumatic pistol with single-shot pressure snap and short-range Pressure Burst alternate fire. | Data-driven definition, viewmodel, primary fire, alternate burst, ammo/cooldown tests. | Final model, reload/check animation, better recoil animation, refined burst VFX/audio, VR grip pose. |
| Steam Scattergun | Unlockable close-range breacher | Triple-barrel steam scatter weapon with pellet blast primary and pressure slug alternate fire. | Data-driven definition, Level03 pickup, weapon switching, pickup VFX/audio, dedicated blast and slug feedback. | Final model, shell/pump animation, reload/check state, stronger pickup presentation, pellet balance pass, VR two-hand pose. |

### Enemies And Machines

| Enemy | Current Role | Production Intent | Keep From v0 | Needs Before Final |
| --- | --- | --- | --- | --- |
| Scrapper | First melee chaser | Small cutter-arm maintenance automaton that pressures the player in corridors. | Chase behavior, attack windup, procedural body motion, hit/death VFX, smoke coverage. | Final model, cutter animation, pathing polish, attack spacing, hit reactions. |
| Lancer | Ranged pressure unit | Thin valve-rifle frame that anchors lanes with visible pressure bolts. | Projectile behavior, bolt trail VFX, data-driven definition, ranged smoke coverage. | Aim/fire animation, cover-lane placement rules, projectile telegraph polish, final model. |
| Bulwark | Heavy blocker | Furnace-plated hammer machine that forces repositioning and ammo commitment. | Data-driven definition, Level04/Level05 placement, heavy silhouette, combat smoke coverage. | Stomp/hammer tell, weak-point readability, stagger states, final armor model, low-frequency audio. |
| Bellows Node | Stationary support hazard | Pressure amplifier that pulses damage and overdrives nearby machines. | Pulse damage, Scrapper boost, pulse VFX/audio, boost VFX, validation, smoke coverage. | Multi-enemy boost rules, interrupt/shutdown state, clearer range ring, final model. |
| Governor Warden | Final guardian | Large pressure governor boss assembled from service frames, mixing stomp and pressure-bolt attacks. | Boss health HUD, projectile/stomp attacks, enrage, locked finale, shutdown VFX, smoke coverage. | Phase polish, arenas/cover rules, weak points, final model/animation, boss music/audio layers. |

## Weapon Family Spec

### Shared Weapon Requirements

All weapons should expose or preserve these fields in data/config:

- `displayName`
- `slotIndex`
- `primaryDamage`
- `primaryRange`
- `primaryCooldown`
- `primaryAmmoCost`
- `primarySpreadDegrees`
- `primaryProjectileCount`
- `primaryForce`
- `altDamage`
- `altRange`
- `altCooldown`
- `altAmmoCost`
- `altSpreadDegrees`
- `reloadSeconds` or `checkSeconds`
- `equipSeconds`
- `viewKick`
- `recoilReturnSpeed`
- `muzzleVfxId`
- `impactVfxId`
- `fireAudioCue`
- `altFireAudioCue`
- `emptyAudioCue`
- `pickupAudioCue`
- `pickupVfxId`
- `vrGripProfile`
- `testProbeId`

Shared first-person model requirements:

- Separate low-poly proxy and final render mesh.
- Single root transform named consistently for automated inspection.
- Muzzle socket.
- Ejection/vent socket.
- Optional off-hand socket for VR.
- Gauge, valve, pressure chamber, grip, trigger, and barrel elements separated enough for animation.
- LOD0/LOD1/LOD2 final meshes for Windows; simplified LOD for Android/browser.

Shared VFX requirements:

- Muzzle effect must identify the weapon within 0.2 seconds.
- Impact effect must distinguish metal hits from world hits when feasible.
- Alternate fire must not reuse the exact same primary VFX.
- Steam should dissipate fast enough that it does not block repeated shots in narrow corridors.
- VR effects need comfort-scaled opacity and no large screen-space flashes near the camera.

Shared audio requirements:

- Primary and alternate fire need different transient shapes.
- Empty click must be short and non-annoying.
- Pickup cue should be spatial in world, then optionally reinforced at player camera.
- Heavy weapons should use lower pressure resonance but avoid muddy low-end on small speakers.

### Pressure Pistol

Role intent:

- Always useful starter weapon.
- Encourages measured shots and movement.
- Primary fire is accurate and cheap.
- Alternate Pressure Burst is an emergency close-range dump with higher ammo cost.

Silhouette:

- Compact brass receiver.
- Walnut grip.
- Short pressure tube under barrel.
- Round gauge visible near rear.
- Crowned muzzle ring.
- Small side valve and pipe loop.

Attack tells and player feedback:

- Primary: quick valve snap, small muzzle pressure cone, brass spark impact.
- Alternate: visible pressure chamber dump, wider muzzle bloom, stronger hand kick.
- Empty: dry valve tick plus tiny gauge twitch.

Animation needs:

- Idle gauge tremble.
- Primary recoil kick and return.
- Alternate recoil with chamber vent.
- Equip raise.
- Optional reload/check: player cracks a valve, gauge rises, chamber locks.
- VR: one-hand dominant pose, optional off-hand steady point disabled by default.

Tuning fields:

- Primary damage: medium-low.
- Primary cooldown: short.
- Primary range: medium.
- Primary spread: low.
- Burst damage per pellet: low.
- Burst pellet count: 3-5.
- Burst range: short.
- Burst ammo cost: high enough to create a real tradeoff.

Prefab/model assets:

- `WPN_PressurePistol_View_Final`
- `WPN_PressurePistol_World_Final`
- `MAT_Brass_Worn`
- `MAT_Walnut_Dark`
- `MAT_Iron_Blackened`
- `VFX_Pistol_MuzzleSnap`
- `VFX_Pistol_BurstDump`
- `VFX_Pistol_ImpactSparks`
- `AUD_Pistol_Fire`
- `AUD_Pistol_Burst`
- `AUD_Pistol_Empty`

Automated test hooks:

- Spawn test Scrapper at known range.
- Verify primary ammo cost, cooldown, hit registration, hit VFX, and kill timing.
- Verify alternate consumes configured ammo and produces a different VFX/audio cue.
- Verify no primary shot is possible during cooldown.
- Verify empty audio triggers when ammo is insufficient.

VR considerations:

- Keep muzzle effects forward and away from eye plane.
- Avoid forced camera recoil; animate weapon model instead.
- Provide stable wrist angle for one-hand use.
- Do not require tiny iron-sight alignment for baseline play.

### Steam Scattergun

Role intent:

- Close-range problem solver.
- Strong against clustered small enemies and close heavy pressure.
- Slug alternate gives limited medium-range precision but should not replace the pistol.

Silhouette:

- Triple barrel cluster.
- Brass top rib.
- Walnut pump grip.
- Rear pressure coil.
- Valve wheel or rotating pressure cap.
- Shell rack on world pickup and optional viewmodel side.

Attack tells and player feedback:

- Primary: broad steam pressure ring, brass spark cone, heavy clack.
- Alternate slug: narrow pressure spear, sharper pipe whistle, brass collar flash.
- Pickup: stand/yoke/nameplate presentation, brass shell burst, pressure rise cue.

Animation needs:

- Equip lift with weight.
- Primary fire buck and pressure vent.
- Pump or shell-index motion after primary.
- Alternate slug chamber lock before fire.
- Idle coil vibration.
- VR: two-hand pose, optional pump interaction later; initial VR version can fire with dominant hand while support hand is tracked cosmetically.

Tuning fields:

- Pellet count.
- Pellet damage.
- Pellet spread.
- Primary range and falloff.
- Slug damage.
- Slug range.
- Slug cooldown.
- Ammo cost per mode.
- Pickup unlock flag.
- Auto-equip on pickup flag.

Prefab/model assets:

- `WPN_SteamScattergun_View_Final`
- `WPN_SteamScattergun_World_Final`
- `WPN_SteamScattergun_PickupStand_Final`
- `MAT_Brass_PolishedEdges`
- `MAT_Walnut_Oiled`
- `MAT_HeatStainedSteel`
- `VFX_Scattergun_BlastCone`
- `VFX_Scattergun_SlugSpear`
- `VFX_WeaponPickup_BrassSteam`
- `AUD_Scattergun_Primary`
- `AUD_Scattergun_Slug`
- `AUD_WeaponPickup`

Automated test hooks:

- Verify Level03 pickup route unlocks weapon.
- Verify weapon switching preserves pistol and scattergun state.
- Verify primary and alternate use distinct cues.
- Verify slug can damage but does not incorrectly trigger pellet behavior.
- Verify pickup presentation pieces exist in scene validation.

VR considerations:

- Two-hand pose should not be required for accessibility.
- Pump action should be optional or assisted.
- Muzzle smoke opacity should be reduced in VR.
- Slug VFX should avoid fast full-screen streaks near camera.

### Rivet Launcher

Status: future weapon candidate.

Role intent:

- Medium-range precision weapon for armored machines.
- Slower than pistol, stronger against weak points.
- Useful for Bulwark and Warden phases.

Silhouette:

- Long rivet rail.
- Exposed feed cylinder.
- Brass pressure plunger.
- Iron shoulder brace.
- Rotating gauge cluster.

Attack tells and player feedback:

- Wind-up hiss before release.
- Rivet projectile with visible glowing hot tip.
- Metal pin impact with shower of small sparks.
- Weak-point hit should have a distinct sharper ring.

Animation needs:

- Charge or cocking state.
- Recoil and rail reset.
- Feed cylinder index.
- Optional manual reload pack.
- VR two-hand shoulder pose.

Tuning fields:

- Direct damage.
- Armor multiplier.
- Charge time.
- Projectile speed.
- Projectile gravity, if any.
- Weak-point multiplier.
- Ammo reserve and pickup amount.

Prefab/model assets:

- `WPN_RivetLauncher_View_Proxy`
- `WPN_RivetLauncher_World_Proxy`
- `VFX_Rivet_HotTrail`
- `VFX_Rivet_WeakPointImpact`
- `AUD_Rivet_Charge`
- `AUD_Rivet_Fire`

Automated test hooks:

- Verify charge cannot fire early unless design allows partial charge.
- Verify weak-point multiplier on tagged target.
- Verify projectile despawns and does not leak.
- Verify armor multiplier does not affect non-armored Scrappers.

VR considerations:

- Shoulder bracing is optional; do not require physical stock alignment.
- Charging should be hold-to-charge, release-to-fire, with comfort haptics later.

### Coil Harpoon

Status: future weapon candidate.

Role intent:

- Utility weapon for pulling levers, staggering small machines, and pinning a single target briefly.
- Avoid pure electronic fantasy; present as a spring-wound pressure harpoon with a brass return cable.

Silhouette:

- Short harpoon tube.
- Cable spool.
- Spring governor.
- Hooked brass/iron projectile.

Attack tells and player feedback:

- Spool whine before launch.
- Cable line visible for a short duration.
- Staggered enemy vents steam from joints.

Animation needs:

- Spool spin.
- Harpoon launch.
- Cable retract.
- Failed latch snapback.

Tuning fields:

- Stagger duration.
- Pull strength.
- Max latch range.
- Cooldown.
- Boss immunity or reduced effect.
- Utility target masks.

Prefab/model assets:

- `WPN_CoilHarpoon_View_Proxy`
- `WPN_CoilHarpoon_World_Proxy`
- `VFX_Harpoon_Cable`
- `VFX_Harpoon_LatchSteam`
- `AUD_Harpoon_Launch`
- `AUD_Harpoon_Retract`

Automated test hooks:

- Verify latchable targets respond.
- Verify non-latchable targets fail gracefully.
- Verify boss reduced effect.
- Verify cable VFX cleans up after hit and miss.

VR considerations:

- Pull effects must not move the player camera unexpectedly.
- Use enemy stagger rather than forced player tug in VR.

### Boiler Grenade

Status: future limited-use weapon candidate.

Role intent:

- Area denial and crowd control.
- Lets player flush Scrappers or interrupt support nodes.
- Should be powerful but scarce.

Silhouette:

- Palm-sized brass pressure pot.
- Red-orange overpressure glass slit.
- Pull valve ring.
- Riveted cap.

Attack tells and player feedback:

- Fuse hiss.
- Pressure pulse ring before blast.
- Smoke and sparks after detonation.

Animation needs:

- Pull valve.
- Throw.
- Bounce/spin.
- Overpressure pulse.
- VR: physical throw optional; button arc throw required.

Tuning fields:

- Fuse seconds.
- Blast radius.
- Inner/outer damage.
- Stagger strength.
- Max carried.
- Pickup count.

Prefab/model assets:

- `WPN_BoilerGrenade_World_Final`
- `VFX_Grenade_FuseSteam`
- `VFX_Grenade_PressureBlast`
- `AUD_Grenade_Fuse`
- `AUD_Grenade_Detonate`

Automated test hooks:

- Verify radius damage falloff.
- Verify line-of-sight handling if used.
- Verify fuse timing.
- Verify no player self-damage regression beyond tuned amount.

VR considerations:

- Use optional assisted throw arcs.
- Keep blast flash comfort-scaled.
- Avoid requiring floor pickup interactions during combat.

## Enemy Roster Spec

### Shared Enemy Requirements

All enemy definitions should expose or preserve:

- `displayName`
- `archetype`
- `health`
- `armor`
- `moveSpeed`
- `turnSpeed`
- `acceleration`
- `detectionRadius`
- `loseSightSeconds`
- `attackRange`
- `preferredRange`
- `attackDamage`
- `attackCooldown`
- `windupSeconds`
- `activeDamageSeconds`
- `recoverySeconds`
- `projectileSpeed`
- `projectileSpreadDegrees`
- `staggerThreshold`
- `weakPointMultiplier`
- `deathVfxId`
- `hitVfxId`
- `attackTellVfxId`
- `attackAudioCue`
- `hurtAudioCue`
- `deathAudioCue`
- `navigationProfile`
- `vrComfortProfile`
- `testProbeId`

Shared model/prefab requirements:

- Root object name starts with enemy role.
- Visible forward direction marker in prefab.
- Center-mass hit collider.
- Optional weak-point collider.
- Attack origin socket.
- Head/eye/furnace glow socket.
- Left/right limb sockets for tell animation.
- VFX sockets for vents and sparks.
- LOD0/LOD1/LOD2 final meshes.
- Simplified collision capsule or compound colliders separate from visual mesh.

Shared animation requirements:

- Idle.
- Alert/acquire.
- Move.
- Windup.
- Attack active.
- Recover.
- Hit react.
- Stagger, if supported.
- Death/shutdown.
- Optional overdrive/boost loop.

Shared attack tell requirements:

- Red-orange pressure light or heat glow before damage.
- Physical motion that matches the attack source.
- Distinct audio pre-tell for dangerous attacks.
- Minimum readable windup target: 0.35 seconds for small enemies, longer for heavy/boss attacks.

Shared automated test hooks:

- Spawn enemy in isolated arena with deterministic player proxy.
- Verify attack windup exists before damage.
- Verify enemy can take damage and die.
- Verify configured hit/death VFX spawn.
- Verify audio cue IDs are valid.
- Verify enemy stops damaging after death.
- Verify scene validation can find required role objects in intended levels.

Shared VR considerations:

- Avoid enemies entering the camera body.
- Maintain comfortable minimum attack distance.
- Keep melee attacks from crossing the player's face plane.
- Use clear spatial audio so headset players can locate threats without extreme head turns.
- Boss scale should feel imposing without forcing the player to look straight up constantly.

### Scrapper

Role intent:

- Baseline melee pressure.
- Teaches movement, spacing, and attack tells.
- Works in groups of one to three.

Silhouette:

- Short, hunched machine.
- Boiler torso.
- Furnace eye.
- Piston legs.
- Cutter arms.
- Rear pressure tank.

Attack tells:

- Cutter arm lifts and glows red-orange.
- Short grinder spin-up.
- Forward lunge during active damage.
- Recovery exposes side/back briefly.

Audio/VFX cues:

- Idle: small piston ticks.
- Acquire: valve chirp.
- Windup: cutter whirr plus pressure hiss.
- Hit: brass sparks and steam pop.
- Death: shutdown whine, small steam burst, cutter drop.
- Boosted: overdrive ring, faster piston ticks.

Animation needs:

- Idle jitter.
- Scuttle run.
- Cutter windup.
- Slash/lunge.
- Hit flinch.
- Shutdown collapse.
- Boosted move loop.

Tuning fields:

- Health low.
- Speed medium-fast.
- Attack range short.
- Damage low-medium.
- Windup short but readable.
- Cooldown short.
- Stagger threshold low.
- Bellows boost speed/damage multipliers.

Prefab/model asset needs:

- `EN_Scrapper_Final`
- `EN_Scrapper_LOD1`
- `EN_Scrapper_LOD2`
- Cutter arm submeshes.
- Furnace eye emissive material.
- Pressure tank and piston materials.
- `VFX_Scrapper_CutterTell`
- `VFX_Scrapper_Shutdown`

Automated test hooks:

- Verify Scrapper damages only after windup.
- Verify boost state changes movement or attack values while active.
- Verify boost VFX attaches to Scrapper root.
- Verify player can kill with pistol within expected shot count.

VR considerations:

- Do not allow Scrapper to clip into headset space.
- Use ground-level audio and visible eye glow for tracking.
- Keep attack arc below face height.

### Lancer

Role intent:

- Ranged lane pressure.
- Punishes standing still.
- Combines with Scrappers by forcing movement into melee risk.

Silhouette:

- Tall, thin tripod or narrow biped.
- Long valve-rifle arm.
- Pressure cylinder along spine.
- Small glowing sight aperture.

Attack tells:

- Rifle arm raises and locks.
- Barrel pressure glow charges from brass to red-orange.
- Short aim hold before bolt release.
- Muzzle pressure ring at shot.

Audio/VFX cues:

- Idle: distant regulator clicks.
- Windup: rising pipe whistle.
- Fire: sharp pressure bolt snap.
- Projectile: visible glow core, steam puffs, sparks.
- Death: valve blowout, tripod collapse.

Animation needs:

- Idle aim scan.
- Strafe/reposition.
- Aim windup.
- Rifle recoil.
- Reload/pressure recharge.
- Hit flinch.
- Shutdown collapse.

Tuning fields:

- Health low-medium.
- Preferred range medium.
- Projectile speed medium-high.
- Fire cooldown medium.
- Aim windup readable.
- Spread low.
- Minimum range retreat behavior.

Prefab/model asset needs:

- `EN_Lancer_Final`
- Valve-rifle arm.
- Back pressure cylinder.
- Sight aperture emissive.
- Projectile prefab.
- `VFX_Lancer_AimTell`
- `VFX_PressureBolt_Trail`
- `AUD_Lancer_Charge`
- `AUD_Lancer_Fire`

Automated test hooks:

- Verify projectile spawns from correct socket.
- Verify projectile VFX exists.
- Verify player damage occurs on projectile hit, not on windup.
- Verify Lancer respects cooldown.
- Verify Lancer can be killed by pistol and scattergun.

VR considerations:

- Projectile should be visible in headset without being a bright face flash.
- Avoid excessive projectile speed for comfort.
- Spatial windup audio helps players locate off-screen Lancers.

### Bulwark

Role intent:

- Heavy area blocker.
- Forces weapon switching or careful kiting.
- Protects keys, lifts, and foundry lanes.

Silhouette:

- Large furnace-plated torso.
- Hammer arms.
- Heavy piston legs.
- Belly furnace glow.
- Reinforced shoulder boilers.

Attack tells:

- Hammer raises high and vents steam.
- Belly furnace glow intensifies.
- Foot plant before slam.
- Ground pressure ring marks danger radius.

Audio/VFX cues:

- Idle: low boiler thump.
- Windup: furnace roar plus hydraulic lift.
- Slam: metal impact, dust/steam ring.
- Hit: armor spark ricochet with lower damage feedback.
- Weak-point hit: sharper crack and brighter vent.
- Death: large shutdown burst, pressure release, armor plate drop.

Animation needs:

- Heavy idle.
- Slow advance.
- Hammer windup.
- Slam active.
- Recovery kneel or overheat window.
- Stagger on weak-point damage.
- Death collapse.

Tuning fields:

- Health high.
- Armor high.
- Weak-point multiplier.
- Speed slow.
- Attack range medium-short.
- Slam radius.
- Windup long.
- Recovery long.
- Stagger threshold high.

Prefab/model asset needs:

- `EN_Bulwark_Final`
- Furnace torso mesh.
- Hammer arm meshes.
- Weak-point gauge or rear boiler.
- Ground slam VFX.
- Heavy shutdown VFX.
- Armor hit material response.
- Low-frequency audio set.

Automated test hooks:

- Verify heavy enemy survives expected pistol shots.
- Verify scattergun is effective at close range but not instant-kill unless tuned.
- Verify slam damage is delayed until active window.
- Verify death VFX uses heavy scale.
- Verify weak-point multiplier if implemented.

VR considerations:

- Heavy slam camera shake should be optional or reduced in VR.
- Ground pressure ring should be visible without looking straight down.
- Keep Bulwark attack reach away from headset plane.

### Bellows Node

Role intent:

- Stationary support pressure source.
- Creates local priority targets.
- Makes already-known enemies more dangerous without adding navigation complexity.

Silhouette:

- Anchored bellows machine.
- Vertical pressure bladder.
- Brass ribs.
- Rotating valve crown.
- Floor pipe cluster.

Attack/support tells:

- Bellows inhale before pulse.
- Range ring or floor pipes glow red-orange.
- Boosted enemies receive visible overdrive state.
- Shutdown releases a final harmless vent.

Audio/VFX cues:

- Idle: slow breathing bellows.
- Windup: intake whoosh.
- Pulse: radial pressure thump.
- Boost: fast valve ticks on affected enemies.
- Death/shutdown: bladder deflates, valve rattle.

Animation needs:

- Bellows inhale/exhale loop.
- Pulse expansion.
- Valve crown spin.
- Damage state collapse.
- Boost beam/ring optional.

Tuning fields:

- Health medium.
- Pulse interval.
- Pulse radius.
- Pulse damage.
- Boost radius.
- Boost duration.
- Boost affected archetypes.
- Boost damage/speed/cooldown multipliers.
- Shutdown disable state.

Prefab/model asset needs:

- `EN_BellowsNode_Final`
- Bellows bladder mesh.
- Valve crown mesh.
- Floor pipe mesh set.
- Pulse radius VFX.
- Boost state VFX.
- Shutdown vent VFX.

Automated test hooks:

- Verify pulse damages player only on pulse, not continuously.
- Verify boost applies to intended enemies within radius.
- Verify boost expires.
- Verify node shutdown removes boost.
- Verify pulse audio/VFX cue IDs are valid.

VR considerations:

- Pulse ring must be readable in peripheral vision.
- Avoid strong inward/outward camera effects.
- Provide spatial low-frequency cue but do not rely on sub-bass.

### Governor Warden

Role intent:

- Final guardian that combines learned mechanics.
- Tests movement, target prioritization, and weapon switching.
- Should feel like a pressure governor failing under load, not a magical monster.

Silhouette:

- Tall central governor frame.
- Rotating pressure crown.
- Furnace heart.
- Heavy arms or stabilizer pylons.
- Integrated pressure cannon.
- Exposed weak-point regulators during phase windows.

Attack tells:

- Stomp: leg/arm lift, floor pressure ring, furnace flare.
- Pressure bolt: cannon aligns, crown locks, barrel glow charges.
- Enrage: crown spins faster, furnace heart overbright, vent rhythm changes.
- Shutdown: heart flicker, valves pop, pressure ring expands outward.

Audio/VFX cues:

- Idle: layered governor rotation and boiler rumble.
- Phase change: alarm bell, rising vent pitch.
- Stomp: heavy metal impact, steam wave.
- Bolt: deep charge plus sharp release.
- Hit: armor clang or weak-point crack.
- Death: cascading valve failure, large steam/spark sequence.

Animation needs:

- Idle crown rotation.
- Tracking/turning.
- Stomp windup and recovery.
- Cannon aim/fire.
- Enrage transition.
- Weak-point exposure.
- Multi-part shutdown.

Tuning fields:

- Total health.
- Phase thresholds.
- Stomp damage/radius/windup.
- Bolt damage/speed/cooldown.
- Summon/support flags, if any.
- Enrage multipliers.
- Weak-point exposure windows.
- Arena lock/unlock state.
- Boss HUD id.

Prefab/model asset needs:

- `BOSS_GovernorWarden_Final`
- Crown assembly.
- Furnace heart.
- Pressure cannon.
- Stomp limb.
- Weak-point regulator meshes.
- Boss hit/death VFX.
- Boss audio layers.
- Boss HUD portrait/icon optional.

Automated test hooks:

- Verify boss HUD appears and updates after damage.
- Verify finale lock stays locked while Warden is alive.
- Verify finale unlocks after Warden death.
- Verify stomp and bolt damage use separate attack windows.
- Verify enrage triggers at configured threshold.
- Verify shutdown VFX exists and cleans up.

VR considerations:

- Boss height should not require constant neck extension.
- Stomp rings and cannon tells should be visible at normal standing gaze.
- Avoid forced cinematic camera control.
- Large VFX must be depth-stable and not screen-space overwhelming.

## Future Mechanical Enemy Candidates

These enemies are optional and should be added only after current roster readability is strong.

| Enemy | Role | Silhouette | Primary Tell | Main Asset Needs | Test Hook |
| --- | --- | --- | --- | --- | --- |
| Gearmite Swarm | Low-health nuisance group | Small rolling gear bodies with tiny legs | Gear chatter speeds up before leap | Small mesh variants, simple swarm material, tiny spark death | Verify group spawn count, low damage, cleanup |
| Soot Surgeon | Repair/support unit | Tall tool-rack automaton with oil canister | Extends repair arm with greenish brass glow | Repair beam VFX, tool-arm animation | Verify it heals enemies, not player, and stops on death |
| Rivet Drummer | Suppression ranged unit | Drum-fed rivet frame on squat legs | Drum spins and barrel line glows | Burst projectile VFX, drum animation | Verify burst count/spread/cooldown |
| Chain Hauler | Area denial puller | Winch torso with chain spool | Chain hook winds back before launch | Chain projectile, spool animation | Verify pull/stagger rules and boss immunity |
| Furnace Mender | Heavy support | Shielded boiler cart with repair vents | Side vents open before armor buff | Armor-buff VFX, shield plates | Verify armor buff applies/ends correctly |
| Regulator Twin | Mini-boss pair | Two linked governor frames with shared pipe | One charges while the other vents | Linked health UI, pipe tether VFX | Verify shared phase logic and tether cleanup |

Future enemy implementation rules:

- Add one new behavior family at a time.
- Avoid adding multiple new projectile types in the same slice unless shared infrastructure already exists.
- Each enemy must ship with scene validation, isolated combat smoke, and at least one level placement rule.
- Final art can be developed in parallel, but integration should use proxy prefabs until test hooks pass.

## Combat Asset Pack Plan

The final combat asset pack can be produced in parallel as long as it targets stable contracts rather than unstable scene layouts.

### Materials

| Material | Usage | Notes |
| --- | --- | --- |
| `MAT_Brass_Worn` | Weapons, valves, enemy trim | Warm brass with dark grime in creases; primary steampunk identity material. |
| `MAT_Copper_Aged` | Pipes, coils, pressure tubes | Oxidized edges, restrained green patina only where readable. |
| `MAT_Iron_Blackened` | Enemy frames, weapon barrels | Dark iron, soot, worn edges. |
| `MAT_Walnut_Dark` | Weapon grips, stock pieces | Oiled dark wood, visible grain, not fantasy-clean. |
| `MAT_HeatStainedSteel` | Barrels, furnace parts | Blue/brown heat stain near vents and muzzles. |
| `MAT_Furnace_Emissive` | Enemy eyes, furnace bellies | Red-orange danger language. |
| `MAT_PressureGlass` | Gauges, sight slits | Slightly grimy glass, readable needle markings. |
| `MAT_Oil_Soot` | Decals, grime masks | Used sparingly to avoid muddy readability. |

### Texture Sets

| Set | Maps | Target |
| --- | --- | --- |
| Weapons 2K | Albedo, normal, metallic, roughness, emission mask | Windows final. |
| Enemies 2K/4K by size | Albedo, normal, metallic, roughness, emission mask, packed grime | Windows final, 4K only for boss if needed. |
| Mobile/Web 1K | Albedo, normal, packed ORM | Android/browser fallback. |
| VR Optimized | Albedo, normal, packed ORM, reduced emissive flicker | Steam/Meta VR comfort builds. |

### Model Production Units

Suggested independent model tasks:

- Pressure Pistol final view/world model.
- Steam Scattergun final view/world/pickup stand model.
- Shared weapon gauge/valve/screw/pipe kit.
- Scrapper final model and cutter arms.
- Lancer final model and valve-rifle assembly.
- Bulwark final model and weak-point assembly.
- Bellows Node final model and floor pipe kit.
- Governor Warden final boss model.
- Shared mechanical limb kit for future enemies.
- Shared projectile/VFX mesh kit: rivets, rings, spark cards, steam cards.

Model constraints:

- Match Unity meter scale.
- Keep pivot/root orientation consistent with current prefabs.
- Separate moving parts.
- Provide collision proxy suggestions.
- Keep UVs clean for material atlasing.
- Include LODs or mark where LODs are required.
- Avoid tiny details that only read in screenshots but disappear in FPS motion.

## Tuning And Balance Method

Combat balance should progress from deterministic to experiential:

1. Automated invariants first: damage, cooldown, ammo, death, VFX/audio presence.
2. Solo enemy tests second: each role can threaten and be defeated.
3. Pair tests third: Scrapper + Lancer, Scrapper + Bellows Node, Bulwark + Lancer.
4. Encounter tests fourth: level-specific placement and route pressure.
5. Manual feel pass last: pacing, readability, ammo economy, difficulty.

Suggested tuning targets:

- Starter Scrapper should die quickly enough to teach shooting, not attrition.
- Lancer should miss or be dodgeable if player moves when the tell appears.
- Bulwark should be slow enough to kite but dangerous if ignored.
- Bellows Node should be worth prioritizing but not mandatory in every room.
- Warden should reuse learned tells, then combine them under pressure.

## Automated Test Coverage Map

| Feature | Minimum Automated Coverage |
| --- | --- |
| Pressure Pistol primary | Hit, damage, cooldown, ammo, impact VFX/audio. |
| Pressure Pistol burst | Ammo cost, spread behavior, distinct VFX/audio, cooldown. |
| Steam Scattergun pickup | Unlock state, pickup VFX/audio, weapon switch, persistence across level transition. |
| Steam Scattergun primary | Pellet count, close-range damage, distinct blast VFX/audio. |
| Steam Scattergun slug | Distinct cue, range behavior, ammo cost, non-pellet damage path. |
| Scrapper | Windup-before-damage, chase, death, boost state, hit/death VFX. |
| Lancer | Charge tell, projectile spawn, projectile damage, projectile VFX cleanup. |
| Bulwark | Heavy health, delayed slam damage, death VFX scale, optional weak-point logic. |
| Bellows Node | Pulse interval, pulse damage, boost apply/expire, shutdown cleanup. |
| Governor Warden | HUD, phase/enrage, stomp, bolt, finale lock/unlock, shutdown VFX. |
| Future enemies | Isolated role test plus one mixed encounter test before level integration. |

Test naming should remain explicit and grep-friendly, for example:

- `V0_PRESSURE_PISTOL_PRIMARY_PASS`
- `V0_PRESSURE_PISTOL_BURST_PASS`
- `V0_STEAM_SCATTERGUN_PASS`
- `V0_SCRAPPER_TELL_PASS`
- `V0_LANCER_PROJECTILE_PASS`
- `V0_BULWARK_SLAM_PASS`
- `V0_BELLOWS_NODE_SUPPORT_PASS`
- `V0_GOVERNOR_WARDEN_PHASE_PASS`

## VR Compatibility Rules

These decisions should be made now, even before VR builds start:

- Never couple weapon recoil to forced camera rotation.
- Keep all combat-relevant information readable through world geometry, enemy motion, spatial audio, and hand models.
- Avoid required rapid 180-degree turns.
- Avoid enemies attacking from inside the player's personal space.
- Provide optional snap turn, smooth turn, and seated-height assumptions later.
- Keep large VFX depth-stable and avoid full-screen flashes.
- Do not require exact two-hand weapon alignment for baseline firing.
- Preserve separate weapon roots and sockets so hand poses can be layered later.
- Keep boss attacks readable at normal head angle.

## Safe Parallel Implementation Order

This order is designed so another chat/process can work independently without breaking main v0 development.

1. Combat data contract audit
   - Document current weapon/enemy definition fields.
   - Identify missing fields from this spec.
   - Do not change gameplay yet.

2. Test hook naming pass
   - Add or document grep-friendly pass markers for combat systems.
   - Keep tests deterministic.
   - No asset dependency required.

3. Current weapon final-art contracts
   - Produce model sheets or placeholder final prefab specs for Pressure Pistol and Steam Scattergun.
   - Keep pivots, sockets, material IDs, and LOD expectations explicit.
   - Do not integrate final art until model scale is verified.

4. Current enemy final-art contracts
   - Produce model sheets or prefab specs for Scrapper, Lancer, Bulwark, Bellows Node, and Warden.
   - Include collider, weak-point, attack-origin, and VFX socket requirements.

5. Scrapper readability pass
   - Improve attack tell and hit/death clarity.
   - Add/expand tests for windup-before-damage.
   - This is the best first gameplay implementation slice because it improves the baseline enemy without requiring new systems.

6. Lancer projectile readability pass
   - Improve charge tell and projectile readability.
   - Verify projectile cleanup and player dodge timing.

7. Pressure Pistol polish pass
   - Improve recoil, burst identity, impact feedback, and audio separation.
   - Maintain existing tuning until tests are stable.

8. Steam Scattergun polish pass
   - Improve pump/slug identity, pickup presentation, and close-range balance.
   - Verify weapon switching and unlock persistence.

9. Bulwark heavy pass
   - Add clearer slam tell, recovery window, and weak-point hook.
   - Tune with scattergun and future rivet launcher in mind.

10. Bellows Node support pass
    - Expand support behavior beyond Scrappers only if tests remain deterministic.
    - Add clear shutdown/disable state.

11. Governor Warden phase pass
    - Refine boss phase thresholds and attack sequencing.
    - Keep finale lock/unlock tests as non-negotiable.

12. First future weapon prototype
    - Recommended: Rivet Launcher because it naturally supports heavy/weak-point gameplay.
    - Add as a fully tested isolated weapon before level placement.

13. First future enemy prototype
    - Recommended: Soot Surgeon or Rivet Drummer.
    - Avoid swarm behavior until navigation and cleanup tests are strong.

14. Combat asset pack integration
    - Replace proxy models one role at a time.
    - Run scene validation after each replacement.
    - Keep proxy fallback prefabs until final art has passed performance checks.

15. Platform reduction pass
    - Define Android/browser/VR asset variants.
    - Reduce material count and VFX density.
    - Validate that combat tells still read after simplification.

## Parallel Work Packages

These can be handed to independent chat instances safely.

| Package | Write Scope | Output | Dependency Risk |
| --- | --- | --- | --- |
| Weapon Art Contract Pack | New documentation or asset-planning files only | Detailed model sheets, socket lists, material IDs for Pressure Pistol and Steam Scattergun | Low |
| Enemy Art Contract Pack | New documentation or asset-planning files only | Final model requirements for current five machine roles | Low |
| Combat Test Plan | New documentation or test-design file only | Deterministic combat test matrix and pass marker list | Low |
| VFX/Audio Cue Catalog | New documentation or generated placeholder asset list only | Cue names, timing, event triggers, priority levels | Low |
| Future Weapon Concepts | New documentation only | Rivet Launcher, Coil Harpoon, Boiler Grenade specs with implementation slices | Low |
| Future Enemy Concepts | New documentation only | Optional roster candidates with role limits and tests | Low |
| Final Asset Pack Production | Generated assets in a separate reviewed asset-import folder | Textures, materials, model exports, LOD notes | Medium; needs import standards before touching Unity scenes |
| VR Combat Adaptation | Documentation first, later separate VR input branch | Comfort rules, hand poses, interaction assumptions | Medium; should wait for stable weapon roots |

Recommended side-agent rule: each parallel process should own a narrow write scope, record timestamps, and never touch Unity scene files unless explicitly assigned scene integration.

## Top Production Risks

1. Readability drift
   - Final art may become ornate and less readable than primitives.
   - Mitigation: preserve strong silhouettes, emissive tells, and automated visual object checks.

2. Test brittleness
   - Combat tests can fail if they depend on exact animation timing without tolerance.
   - Mitigation: test state transitions and event markers, not frame-perfect visuals.

3. Performance creep
   - Smoke, sparks, emissives, and detailed mechanical meshes can overload low-end targets.
   - Mitigation: LODs, pooled VFX, material atlases, and platform VFX budgets.

4. VR discomfort
   - Camera recoil, close melee, large flashes, and forced look angles can hurt VR comfort.
   - Mitigation: model recoil only, maintain personal space, scale VFX, and keep boss tells at normal gaze.

5. Scope expansion
   - New enemy/weapon ideas can outrun the core game.
   - Mitigation: finish current roster readability before adding future roles.

## Immediate Top 5 Combat-System Priorities

1. Lock the data contract for weapons and enemies so side-agents can build art and tests against stable fields.
2. Improve Scrapper attack readability because it is the baseline melee lesson for every later encounter.
3. Polish Pressure Pistol and Steam Scattergun identity so primary/alternate fire modes are unmistakable by silhouette, VFX, and audio.
4. Harden Lancer/Bulwark/Bellows Node automated tests around attack windows, support states, and VFX/audio cue presence.
5. Define final prefab socket/material/LOD standards before importing any AAA asset pack into Unity.
