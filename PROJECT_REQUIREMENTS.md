# The EYE — Project Requirements Map

This document maps each graded requirement to where it is implemented in the
project, with file and line references.

> Game flow: **Main Menu → Bedroom (level-select hub) → sleep on the bed → Chase sequence.**
> (The old Act 1 walk-through level was cut; the bed now loads straight into the chase.)

---

## 1. Core Systems — Input, Rigidbody Physics, Character Movement

**Input handling**
- `Assets/Script/PlayerMovement.cs:22-23` — reads `Input.GetAxisRaw("Horizontal"/"Vertical")` for 8-directional movement.
- `Assets/Script/PlayerMovement.cs:46` — `LeftShift` sprint toggle.
- `Assets/Script/BedInteract.cs:18` — `E` key to sleep / interact.
- `Assets/Script/Act 2/LevelManager.cs:11` — `R` key to restart the level.

**Rigidbody2D physics simulation** (velocity applied in `FixedUpdate`, the correct physics step)
- `Assets/Script/PlayerMovement.cs:29-33` — `rb.velocity = moveTotal * speed`.
- `Assets/Script/Act 2/RunningSequence.cs:33-37` — auto-run physics during the chase.
- `Assets/Script/Act 2/EntityChasing.cs:26-30` — the chasing entity's movement.

**Character movement control**
- Free top-down movement with sprint in the bedroom hub (`PlayerMovement.cs`).
- Forced-forward "auto-runner" control during the chase (`RunningSequence.cs`, `xMove = 1`).

---

## 2. Environment & Interactions — Collision Detection / Handling

All interactions use Unity 2D trigger colliders with tag-based filtering (`CompareTag("Player")`):
- `Assets/Script/BedInteract.cs:57-73` — bed sleep zone (enter/exit prompt + sleep).
- `Assets/Script/Act 2/EntityChasing.cs:32-42` — death on contact with the entity → Game Over.
- `Assets/Script/Act 2/TrapTrigger.cs:17-30` — entering the trap zone triggers the spike drop.
- `Assets/Script/Act 2/KeyMinigameTrigger.cs:28-42` — entering reveals keys + activates the minigame.
- `Assets/Script/GoToAct2.cs:8-17`, `Interactable.cs`, `TilePuzzleTrigger.cs` — additional trigger interactions.

---

## 3. Game Logic — Object Spawning / Management

**Spawning** — `Assets/Script/Act 2/TrapTrigger.cs:21-28`
- When the player enters the trap, a loop instantiates **40 spike "drop" objects** at randomized
  positions (`Random.Range` on X and Y) via `Instantiate(drops, dropSpawn, Quaternion.identity)`.

**Lifecycle management** — `Assets/Script/Act 2/SpikeDespawn.cs`
- Each spawned spike manages its own life: removes its `Rigidbody2D` after a random freeze time and
  `Destroy`s itself after 4 seconds (`Invoke` timers) — preventing object buildup.

**State management**
- `Assets/Script/Act 2/KeyMinigame.cs` — enables/disables key objects and good/bad ending panels.
- `Assets/Script/Act 2/LevelManager.cs:17-35` — level restart with a persistent restart counter
  saved via `PlayerPrefs`.

---

## 4. Visual & Audio

**Visual / feedback ("juice")**
- `Assets/Script/CutsceneIntro.cs` — builds a full-screen cutscene overlay at runtime (fade → frames → fade).
- `Assets/Script/AnimationsScript/SceneFader.cs` — fade-to-black scene transitions.
- `Assets/Script/AnimationsScript/CameraShake.cs`, `HitStop.cs`, `Juice.cs` — screen shake / hit-stop game feel.
- `Assets/Script/PlayerAnimator.cs`, `SpriteLoop.cs`, `EyeUI.cs` — animation and UI visuals.

**Audio**
- `Assets/Script/Act 2/GameAudio.cs` / `AudioManager.cs` — audio manager (singleton) for SFX/music.
- Footstep SFX driven by movement (`PlayerMovement.cs:42`, `RunningSequence.cs:27`).
- Music + SFX assets in `Assets/Music/` (nightmare music, heartbeat, correct/wrong cues).
