# Risk Matrix And Showcase Placements v0.1.49

| Risk | Mitigation |
| --- | --- |
| Visual upgrade is mistaken for gameplay enemy authority. | Prefabs contain visual groups only and no runtime scripts, colliders, AI, nav, animation clips, or animator controllers. |
| Weak-point markers imply damage behavior. | Markers are visual intent only; gameplay weak-point logic must remain in main-lane systems. |
| Pose proxies are treated as final animation. | The package includes pose proxies only and no animator-controller authority. |
| Materials require render-pipeline conversion. | Quarantine promotion should check material conversion before main-lane use. |
| Boss silhouettes exceed encounter scale. | Review in quarantine scene before any replacement. |

## Showcase Row

- `MEES05_Scrapper_A_IdleReadability_BoilerSaw`
- `MEES05_Scrapper_B_AttackWindup_FoldingSaw`
- `MEES05_Lancer_B_AttackWindup_PikeCharge`
- `MEES05_Bulwark_D_WeakPointMarked_FurnaceCore`
- `MEES05_Warden_A_IdleReadability_CommandHalo`
- `MEES05_BossPhase_E_Phase05_FinalSilhouette_CommandCrown`
