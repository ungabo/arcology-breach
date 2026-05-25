# Pipe Tank Gauge Set 10 Import Readiness Plan

- Keep package isolated as `com.brassworks.sidecar.pipe-tank-gauge-set10` with no runtime dependencies.
- Import through local package reference or package manager tarball when integration lane is ready.
- Validate prefabs by dragging into a neutral corridor scene and checking scale, material separation, and absence of physics/gameplay components.
- Use `Runtime/Previews/PTG10_PREVIEW_contact-sheet.png` for quick visual triage.
