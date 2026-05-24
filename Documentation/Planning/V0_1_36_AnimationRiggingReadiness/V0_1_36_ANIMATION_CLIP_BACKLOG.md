# V0.1.36 Animation Clip Backlog

Purpose: define clip coverage for staged mechanical enemies and weapons before gameplay wiring. Clip names are recommendations, not required asset paths.

## Clip Naming

Recommended format:

`ANIM_[Family]_[StateOrAction]_[Variant]`

Examples:

- `ANIM_Scrapper_Idle_CombatLoop`
- `ANIM_Lancer_AttackTell_ChargeLance`
- `ANIM_PressurePistol_VM_Fire_Primary`

Use `VM` for first-person viewmodel clips and `WP` for world/pickup/display clips.

## Shared Enemy Coverage

Every enemy family should eventually have:

| Group | Minimum clips |
| --- | --- |
| Idle | combat idle, alert idle, damaged idle. |
| Move | start, loop, stop, turn left/right, short reposition. |
| Attack tells | readable windup, hold/charge, cancel/recover if applicable. |
| Attack execution | strike/fire/release, recoil/follow-through. |
| Hit reactions | front, back, left, right, weak-point hit, stagger/guard break if applicable. |
| Shutdown/death | quick shutdown, heavy collapse, weak-point burst, fragment settle. |
| Awareness | wake/boot, target acquire, lost-target reset. |

## Scrapper

Idle:

- `ANIM_Scrapper_Idle_CombatLoop`
- `ANIM_Scrapper_Idle_SawTwitch`
- `ANIM_Scrapper_Idle_DamagedSteamLeak`

Move:

- `ANIM_Scrapper_Move_StartLowScuttle`
- `ANIM_Scrapper_Move_LoopScuttle`
- `ANIM_Scrapper_Move_StopSkid`
- `ANIM_Scrapper_Move_Strafe_L`
- `ANIM_Scrapper_Move_Strafe_R`
- `ANIM_Scrapper_Move_TurnInPlace_90_L`
- `ANIM_Scrapper_Move_TurnInPlace_90_R`

Attack tells and attacks:

- `ANIM_Scrapper_AttackTell_SawRev`
- `ANIM_Scrapper_AttackTell_ClawRaise`
- `ANIM_Scrapper_Attack_SawSwipe`
- `ANIM_Scrapper_Attack_ClawSnap`
- `ANIM_Scrapper_Attack_LungeShort`
- `ANIM_Scrapper_Attack_RecoverOverextended`

Hit and shutdown:

- `ANIM_Scrapper_Hit_FrontLight`
- `ANIM_Scrapper_Hit_SideToolArm`
- `ANIM_Scrapper_Hit_WeakBoilerPop`
- `ANIM_Scrapper_Stagger_ToolJam`
- `ANIM_Scrapper_Shutdown_QuickFold`
- `ANIM_Scrapper_Shutdown_FragmentBurst`

## Lancer

Idle:

- `ANIM_Lancer_Idle_CombatLoop`
- `ANIM_Lancer_Idle_CoilPulse`
- `ANIM_Lancer_Idle_AimSearch`

Move:

- `ANIM_Lancer_Move_StartStalk`
- `ANIM_Lancer_Move_LoopStalk`
- `ANIM_Lancer_Move_StopBrace`
- `ANIM_Lancer_Move_Backstep`
- `ANIM_Lancer_Move_StrafeAim_L`
- `ANIM_Lancer_Move_StrafeAim_R`

Attack tells and firing:

- `ANIM_Lancer_AttackTell_LanceRaise`
- `ANIM_Lancer_AttackTell_CoilCharge_01`
- `ANIM_Lancer_AttackTell_CoilCharge_Hold`
- `ANIM_Lancer_Attack_FireBolt`
- `ANIM_Lancer_Attack_BraceRecoil`
- `ANIM_Lancer_Attack_CooldownVent`

Hit and shutdown:

- `ANIM_Lancer_Hit_FrontLight`
- `ANIM_Lancer_Hit_BackpackCoil`
- `ANIM_Lancer_Hit_MuzzleDeflect`
- `ANIM_Lancer_Stagger_LanceDip`
- `ANIM_Lancer_Shutdown_SpineCollapse`
- `ANIM_Lancer_Shutdown_CoilDischarge`

## Bulwark

Idle:

- `ANIM_Bulwark_Idle_GuardLoop`
- `ANIM_Bulwark_Idle_ShieldSettle`
- `ANIM_Bulwark_Idle_DamagedGuard`

Move:

- `ANIM_Bulwark_Move_StartHeavyStep`
- `ANIM_Bulwark_Move_LoopHeavyStep`
- `ANIM_Bulwark_Move_StopPlant`
- `ANIM_Bulwark_Move_ShieldAdvance`
- `ANIM_Bulwark_Move_TurnInPlace_90_L`
- `ANIM_Bulwark_Move_TurnInPlace_90_R`

Attack tells and attacks:

- `ANIM_Bulwark_AttackTell_HammerLift`
- `ANIM_Bulwark_AttackTell_ShieldBrace`
- `ANIM_Bulwark_Attack_HammerSlam`
- `ANIM_Bulwark_Attack_ShieldBash`
- `ANIM_Bulwark_Attack_GuardBreakOpen`
- `ANIM_Bulwark_Attack_RecoverShieldSet`

Hit and shutdown:

- `ANIM_Bulwark_Hit_ShieldSpark`
- `ANIM_Bulwark_Hit_FlankLamp_L`
- `ANIM_Bulwark_Hit_FlankLamp_R`
- `ANIM_Bulwark_Stagger_GuardBreak`
- `ANIM_Bulwark_Shutdown_KneeDrop`
- `ANIM_Bulwark_Shutdown_ShieldHingeBurst`

## Warden

Idle:

- `ANIM_Warden_Idle_CommandLoop`
- `ANIM_Warden_Idle_CrownCoilPulse`
- `ANIM_Warden_Idle_PointSurvey`

Move:

- `ANIM_Warden_Move_StartMeasuredStep`
- `ANIM_Warden_Move_LoopMeasuredStep`
- `ANIM_Warden_Move_StopCommandPose`
- `ANIM_Warden_Move_RotateCommand_90_L`
- `ANIM_Warden_Move_RotateCommand_90_R`

Attack tells and attacks:

- `ANIM_Warden_AttackTell_GavelRaise`
- `ANIM_Warden_AttackTell_TwinCoilCharge`
- `ANIM_Warden_AttackTell_PincerPoint`
- `ANIM_Warden_Attack_GavelStrike`
- `ANIM_Warden_Attack_CommandBolt`
- `ANIM_Warden_Attack_CalloutPulse`
- `ANIM_Warden_Attack_RecoverVent`

Hit and shutdown:

- `ANIM_Warden_Hit_CenterLamp`
- `ANIM_Warden_Hit_CrownCoil_L`
- `ANIM_Warden_Hit_CrownCoil_R`
- `ANIM_Warden_Stagger_CommandDisrupt`
- `ANIM_Warden_Shutdown_CageCrumple`
- `ANIM_Warden_Shutdown_CrownDischarge`

## Foundry Overseer

Idle:

- `ANIM_FoundryOverseer_Idle_BossLoop`
- `ANIM_FoundryOverseer_Idle_ToolSurvey`
- `ANIM_FoundryOverseer_Idle_PhaseHeatRise`

Move:

- `ANIM_FoundryOverseer_Move_StartBossStep`
- `ANIM_FoundryOverseer_Move_LoopBossStep`
- `ANIM_FoundryOverseer_Move_StopWeightSet`
- `ANIM_FoundryOverseer_Move_Pivot_90_L`
- `ANIM_FoundryOverseer_Move_Pivot_90_R`
- `ANIM_FoundryOverseer_Move_RepositionShort`

Attack tells and attacks:

- `ANIM_FoundryOverseer_AttackTell_SawWideRev`
- `ANIM_FoundryOverseer_AttackTell_HammerOverhead`
- `ANIM_FoundryOverseer_AttackTell_BackLanceDeploy`
- `ANIM_FoundryOverseer_AttackTell_CrownChargeSequence`
- `ANIM_FoundryOverseer_Attack_SawSweep`
- `ANIM_FoundryOverseer_Attack_HammerCrush`
- `ANIM_FoundryOverseer_Attack_BackLanceFire`
- `ANIM_FoundryOverseer_Attack_CrownBurst`
- `ANIM_FoundryOverseer_Attack_PhaseVentRecover`

Hit and shutdown:

- `ANIM_FoundryOverseer_Hit_FurnaceLamp`
- `ANIM_FoundryOverseer_Hit_CenterApronLamp`
- `ANIM_FoundryOverseer_Hit_ToolInterrupt`
- `ANIM_FoundryOverseer_Stagger_PhaseBreak`
- `ANIM_FoundryOverseer_Shutdown_BossCollapse`
- `ANIM_FoundryOverseer_Shutdown_MultiBurst`
- `ANIM_FoundryOverseer_Shutdown_CrownToolSettle`

## Pressure Pistol

World/pickup/display:

- `ANIM_PressurePistol_WP_Idle_PickupHover`
- `ANIM_PressurePistol_WP_Idle_WallDisplay`
- `ANIM_PressurePistol_WP_Inspect_Turntable`
- `ANIM_PressurePistol_WP_PickupSnap`

Viewmodel:

- `ANIM_PressurePistol_VM_Idle_Breathe`
- `ANIM_PressurePistol_VM_Equip_Draw`
- `ANIM_PressurePistol_VM_Fire_Primary`
- `ANIM_PressurePistol_VM_Fire_Charged`
- `ANIM_PressurePistol_VM_Reload_CellOut`
- `ANIM_PressurePistol_VM_Reload_CellIn`
- `ANIM_PressurePistol_VM_Reload_GaugeSettle`
- `ANIM_PressurePistol_VM_AltFire_PressureDumpTell`
- `ANIM_PressurePistol_VM_AltFire_PressureDumpRelease`
- `ANIM_PressurePistol_VM_HitOrLower`

Procedural-friendly subclips:

- `ANIM_PressurePistol_Add_RecoilKick`
- `ANIM_PressurePistol_Add_CoilCompress`
- `ANIM_PressurePistol_Add_GaugeNeedleFlick`
- `ANIM_PressurePistol_Add_MuzzleVent`

## Steam Scattergun

World/pickup/display:

- `ANIM_SteamScattergun_WP_Idle_PickupHover`
- `ANIM_SteamScattergun_WP_Idle_WallDisplay`
- `ANIM_SteamScattergun_WP_Inspect_Turntable`
- `ANIM_SteamScattergun_WP_PickupSnap`

Viewmodel:

- `ANIM_SteamScattergun_VM_Idle_HeavyBreathe`
- `ANIM_SteamScattergun_VM_Equip_HeavyDraw`
- `ANIM_SteamScattergun_VM_Fire_PrimaryBlast`
- `ANIM_SteamScattergun_VM_Fire_AltSlug`
- `ANIM_SteamScattergun_VM_Pump_Cycle`
- `ANIM_SteamScattergun_VM_Reload_OpenBreech`
- `ANIM_SteamScattergun_VM_Reload_InsertShell`
- `ANIM_SteamScattergun_VM_Reload_CloseBreech`
- `ANIM_SteamScattergun_VM_AltFire_SlugLoadTell`
- `ANIM_SteamScattergun_VM_AltFire_SlugRelease`
- `ANIM_SteamScattergun_VM_HitOrLower`

Procedural-friendly subclips:

- `ANIM_SteamScattergun_Add_RecoilHeavyKick`
- `ANIM_SteamScattergun_Add_PumpGripSlide`
- `ANIM_SteamScattergun_Add_TankPressurePulse`
- `ANIM_SteamScattergun_Add_MuzzleSteamVent`

## Future Weapon Silhouettes

Minimum backlog for new weapon silhouettes:

- `ANIM_[Weapon]_WP_Idle_PickupHover`
- `ANIM_[Weapon]_WP_Idle_WallDisplay`
- `ANIM_[Weapon]_WP_Inspect_Turntable`
- `ANIM_[Weapon]_VM_Idle`
- `ANIM_[Weapon]_VM_Equip`
- `ANIM_[Weapon]_VM_Fire_Primary`
- `ANIM_[Weapon]_VM_Reload_Primary`
- `ANIM_[Weapon]_VM_AltFire_Tell`
- `ANIM_[Weapon]_VM_AltFire_Release`
- `ANIM_[Weapon]_Add_Recoil`
- `ANIM_[Weapon]_Add_StatusGaugeOrCoil`

Future weapons should not require new Animator architecture just to show in pickup, wall display, first-person fire, reload, alt-fire, and inspection contexts.

