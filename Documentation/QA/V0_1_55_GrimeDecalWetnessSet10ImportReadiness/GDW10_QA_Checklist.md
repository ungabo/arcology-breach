# GDW10 QA Checklist

## Static Validation

- [x] Package root is isolated to AssetPacks/BrassworksBreach.GrimeDecalWetnessSet10.
- [x] Documentation is isolated to the four assigned GDW10 documentation roots.
- [x] package.json exists and parses as JSON.
- [x] Normalized manifest exists in runtime metadata and package documentation.
- [x] 32 visual-only prefab assets generated.
- [x] 16 transparent Standard materials generated.
- [x] 48 PNG material textures generated.
- [x] 32 preview PNGs and two contact sheet copies generated.
- [x] Prefabs use Unity built-in Quad mesh fileID 10210 only.
- [x] Prefabs contain no Collider, Rigidbody, MonoBehaviour, Light, ReflectionProbe, AudioSource, Animation, or Animator components.
- [x] Package contains no .cs, .unity, .anim, .controller, .fbx, .obj, .blend, audio, or external DCC files.
- [x] Materials and previews are view/material only.

## Manual Import Review

- [ ] Open quarantine Unity project after import.
- [ ] Verify alpha blending and sorting are acceptable against the room shell.
- [ ] Verify floor helpers do not visually fight with gameplay collision surfaces.
- [ ] Verify final room read matches concept: soot, grime, dampness, wet floor reflections, edge wear, oil/water pooling, and masonry breakup.