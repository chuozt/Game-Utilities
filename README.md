Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
All rights reserved.

Game Dev's utilities. Can act as a start-up package.

This includes:
- Design Pattern Fundamentals (Singleton, Observer)
- Custom Audio Manager: (Odin Inspector GUI supported)
  + Methods to play SFX and music
  + Supports multiple audio clips in a group. (e.g., "sfx_foot_step" group can include 2 different sounds)
  + Plays clips via enums of groups' names.
  + Can generate enum groups on the Inspector. (SoundKey.cs) (MusicKey) (group names are preferably snake_case, press "Generate", and they will turn into PascalCase inside the scripts)
- Custom Pool Manager: (Odin Inspector GUI supported)
  + Supports multiple prefabs in a group. (e.g., "vfx_fire_work" group can include 2 different prefabs)
  + Can generate enum groups on the Inspector. (PoolKey.cs)
- ProUtils.cs:
  + Currency Formatter
  + Time Formatter
  + UI Helper
  + DateTime Helper
  + String Helper
  + Number Transitor (Requires DOTween)
  + Spline Tweening (Requires DOTween & Unity's Spline)
- Scenes Selection HUD (ProUtils/Select Scenes)
- Pop-up system (In this case, I called Box(es))
- BoxLoading: Load async to another scene
