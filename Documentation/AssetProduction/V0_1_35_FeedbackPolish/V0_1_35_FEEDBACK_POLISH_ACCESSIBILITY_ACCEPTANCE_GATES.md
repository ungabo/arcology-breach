# V0.1.35 Feedback Polish Accessibility And Acceptance Gates

## Accessibility / Readability
- Never communicate state with color alone: pair amber/green/red with icon shape, motion, text token, or audio cadence.
- Pause/settings body text should target at least 18 px equivalent and 7:1 contrast on dark iron panels.
- Reduced-motion mode should disable gauge tremble, screen-edge pulsing, and lamp chase while preserving static icons and audio.
- Low-health feedback must not obscure the reticle or central enemies.
- Secret/objective panels need longer holds than combat pips so players can read them.

## Acceptance Gates
- All eleven cue IDs are present in manifest, UI recipe, VFX recipe coverage, and audio index.
- Each requested category has a PNG icon and WAV placeholder.
- Amber, green, and red meanings are consistent across UI, VFX, and audio documents.
- Denied interaction has repeat throttling guidance.
- Low-health and low-ammo are distinguishable by urgency, shape, and audio register.
- No files are required outside owned staging scopes for review.
