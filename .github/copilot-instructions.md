# Copilot Instructions for Penguin Deliveries Kinetic

## Project Overview
This is a Unity-based 2D game project. The main gameplay involves controlling a stationary cannon to aim and launch a projectile around planets with gravity to land the projectile on a finish point, with supporting systems for background scrolling, camera movement, projectiles, and game state management. The codebase is organized by Unity conventions, with most logic in the `Assets/` directory.

## Key Components
- **PlayerController.cs**: Handles player input, movement, and core gameplay logic for the cannon.
- **GameController.cs**: Manages game state, scoring, and high-level game events.
- **ProjectileController.cs**: Controls projectile behavior and interactions.
- **BackgroundScroller.cs / OffsetScroller.cs**: Implements parallax and background movement.
- **CameraController.cs**: Manages camera following and effects.
- **TrajectoryManager.cs**: Handles trajectory calculations for projectiles.
- **MainMenuController.cs**: Manages main menu UI and navigation.

## Directory Structure
- `Assets/` — Main Unity assets and scripts
  - `Scripts/` — (Minimal use; most scripts are in `Assets/` root)
  - `Prefabs/`, `Scenes/`, `Animations/`, `Audio/`, `Images/` — Standard Unity asset folders
- `ProjectSettings/`, `Packages/` — Unity project configuration

## Developer Workflows
- **Build**: Use Unity Editor's build menu. No custom build scripts detected.
- **Testing**: No automated test scripts found; manual playtesting in Unity Editor is standard.
- **Debugging**: Use Unity's Play mode and Inspector for debugging. Add `Debug.Log` statements in scripts for runtime output.

## Coding Patterns & Conventions
- Scripts are C# MonoBehaviour classes, attached to GameObjects in Unity scenes.
- Public fields are often used for Inspector assignment (avoid making everything public unless needed for Inspector or cross-script access).
- Use `Start()` for initialization and `Update()` for per-frame logic.
- Cross-component communication is typically via direct references or `FindObjectOfType<T>()`.
- Scene and asset references are managed via the Unity Editor, not hardcoded paths.

## Integration Points
- No external APIs or network calls detected.
- Uses Unity's built-in systems for input, physics, and rendering.
- Text rendering may use TextMesh Pro (see `TextMesh Pro/` folder).

## Examples
- To add a new gameplay mechanic, create a new script in `Assets/`, inherit from `MonoBehaviour`, and attach it to a relevant GameObject.
- To communicate between player and game state, use `GameController.Instance` or `FindObjectOfType<GameController>()`.

## Special Notes
- Follow Unity's best practices for performance (e.g., avoid heavy logic in `Update()` if possible).
- Keep serialized fields private with `[SerializeField]` unless Inspector access is needed.
- Use `.meta` files for asset tracking; do not delete them.

---
_Last updated: September 27, 2025_
