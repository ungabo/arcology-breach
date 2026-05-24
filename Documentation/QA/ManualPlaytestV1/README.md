# Manual Playtest V1 Route Sheets

Scope: `v0.1.35` Windows build, current V1 manual-playtest path.

Build to launch:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.1.35\BrassworksBreach_v0.1.35.exe`

Current generated QA packet:

`Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.35.md`

Current generated issue triage packet:

`Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.35.md`

Primary goal: give a human tester enough route, control, pass/fail, timing, and note structure to evaluate the current five-level run without asking for automation or Codex help.

## Windows Controls

| Action | Control |
| --- | --- |
| Look / aim | Mouse |
| Move | `W`, `A`, `S`, `D` |
| Fire current weapon | Left mouse |
| Alternate fire | Right mouse |
| Interact with gates, lifts, valves, plaques | `E` when the prompt appears |
| Equip Pressure Pistol | `1` |
| Equip Steam Scattergun | `2` after the Level03 pickup |
| Pause / resume menu | `Esc` |
| Restart after death or win | `R` when the end-state prompt appears |

No jump or crouch is expected or required in this build.

## Current Route

1. `Level01 - Brassworks Intake`: find the gear key, open the pressure gate, use the service lift.
2. `Level02 - Pipeworks Annex`: confirm the Boilerheart lift is locked, route pipe pressure at the valve, use the lift to Level03.
3. `Level03 - Boilerheart Core`: collect the Steam Scattergun, survive steam and the Bellows Node read, vent the Boilerheart pressure valve, use the foundry lift.
4. `Level04 - Furnace Foundry`: read steam and furnace heat hazards, fight mixed machines including the first Bulwark, use the emergency hoist.
5. `Level05 - Governor Core`: confirm the master override hoist is locked, defeat the Governor Warden, use the hoist to trigger win.

Current registered secrets:

- `Level01 - Secret - Intake Pressure Cache`
- `Level02 - Secret - Pipeworks Cartridge Cache`
- `Level04 - Secret - Foundry Coal Cache`

## Route Sheets

- [First-Run Route](01_FIRST_RUN_ROUTE.md)
- [Combat Feel Route](02_COMBAT_FEEL_ROUTE.md)
- [Secret-Hunt Route](03_SECRET_HUNT_ROUTE.md)
- [Hazard Route](04_HAZARD_ROUTE.md)
- [Boss Route](05_BOSS_ROUTE.md)
- [Accessibility And Readability Route](06_ACCESSIBILITY_READABILITY_ROUTE.md)
- [Test-Session Summary Template](SESSION_SUMMARY_TEMPLATE.md)
- [Issue-Log Template](ISSUE_LOG_TEMPLATE.md)

## Common Test Rules

- Start from a fresh launch unless a route sheet says otherwise.
- Record the build path and route sheet name at the top of every session note.
- If a tester is blocked for more than `2 minutes`, log where and why, then continue if a route forward is found.
- Do not turn a manual route sheet into a hard progression blocker. The goal is observation, not perfect execution.
- A pass means the tester can complete the intended task and can explain why it worked.
- A fail means the tester is blocked, confused for longer than the route target, misreads a threat/objective, or completes only by luck.

## Expected Current Limitations

- Automated smoke confirms the objective chain and regressions, but not human route readability or combat feel.
- Visuals are still mostly procedural prototype art unless an asset document marks a component as promoted or final.
- Enemy navigation uses simple side-steering, not a full NavMesh solution.
- Balance values have automated coverage but still need human feel tuning.
- AudioV1 is wired and mix-smoke-tested but still needs a human listen pass.
- Settings include sensitivity, master volume, flash intensity, resolution, fullscreen, and high-contrast readability.
- Windows is the only current playable target. Android, WebGL, PC VR, and Meta Quest are planned but deferred.
- Health and ammo persist across level transitions, but future weapon inventory and campaign flags still need expansion.
- There is no save system; boss and late-route retests require replaying the route unless a developer provides a special build later.
