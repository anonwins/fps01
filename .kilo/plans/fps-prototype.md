# FPS Game Prototype Plan

## Target: Unity 6.x (latest stable) + C#

### Prerequisites
- Install Unity Hub from unity.com
- Install Unity 6.x latest stable via Hub
- Create new 3D project named "FPSPrototype"
- Enable New Input System in project settings
- Enable ProBuilder (Window > Package Manager > ProBuilder)

---

## Architecture Decision: MonoBehaviour with ECS-Ready Patterns

**Rationale**: MonoBehaviour for rapid development while designing for future scalability:
- Use interfaces (IDamageable, IAttackable) for future ECS migration
- ScriptableObject-based configuration (data-driven design)
- Object pooling for performance-critical systems
- Clean separation: Data, Logic, Presentation layers

---

## Subagent Responsibilities

### Subagent 1: Core Systems
- GameManager.cs (singleton, game state)
- InputManager.cs (centralized input handling)
- .gitignore and Git setup
- BuildScript.cs (editor automation)

### Subagent 2: Player Controller
- PlayerController.cs (movement, physics, state machine)
- PlayerInput.cs (input component)
- InputActions.inputactions asset

### Subagent 3: Weapon System
- WeaponData.cs (ScriptableObject)
- WeaponBase.cs (abstract base class)
- WeaponManager.cs (cycling, equipping)
- RangedWeapon.cs (raycast, tracer)
- MeleeWeapon.cs (swing, hit detection)

### Subagent 4: Environment & UI
- WorldGenerator.cs (procedural ground)
- Skybox material/shader
- Crosshair UI and Canvas
- Moon and lighting setup

---

## Phase 1: Project Structure & Version Control

### 1.1 Git Setup
```
git init
# .gitignore will exclude:
# - Library/, Temp/, Obj/, Build/
# - *.csproj, *.unityproj
# - .userprefs, *.pidb, *.booproj
```

### 1.2 Folder Structure
Assets/
  Scripts/
    Core/
    Player/
    Weapons/
    Environment/
    Utilities/
  Prefabs/
  Scenes/
    Main.unity (main gameplay scene)
  Materials/
  Resources/
  Editor/
    BuildScript.cs (one-click build)

---

## Phase 2: First-Person Controller (Professional Grade)

### 2.1 PlayerController.cs
- CharacterController component (Unity standard for FPS)
- Physics-based movement with velocity vectors
- State machine pattern (Grounded, Airborne, Falling)
- Jump buffering and coyote time (game feel)

### 2.2 Configurable Parameters
- moveSpeed: 5f (walk)
- runMultiplier: 1.5f
- jumpForce: 7f
- gravityScale: 3f
- mouseSensitivity: 2f

---

## Phase 3: Environment (All Procedural)

### 3.1 Ground
- Plane primitive scaled to 100x100 units
- Checkerboard material using Shader Graph (procedural)
- Invisible walls to prevent falling off

### 3.2 Sky & Moon
- Gradient Skybox (Shader Graph or Procedural)
- Moon: Sphere with emissive material (bright spot in sky)
- Directional Light for moon/sun lighting

---

## Phase 4: Weapon System (Extensible)

### 4.1 WeaponData ScriptableObject
```csharp
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType type;
    public float damage;
    public float range;
    public float attackRate;
    public bool isAutomatic;
}
```

### 4.2 RangedWeapon
- Raycast from camera center (ScreenCenter = 0.5, 0.5)
- LineRenderer bullet tracer (object pooled)
- Layer-based hit detection

### 4.3 MeleeWeapon
- Procedural cuboid model in front of camera
- Quaternion swing animation
- Physics.OverlapSphere hit detection
- IDamageable interface

---

## Phase 5: Input System

Actions: Move, Look, Jump, Run, Attack, Melee, CycleWeapon

---

## Phase 6: UI System

- Canvas with Screen Space - Overlay
- Crosshair: Centered Image
- Weapon Panel: Top-right (current weapon)

---

## Future Features (NOT in scope - Late Future)
- Health System
- Enemy AI
- Armor System
- RPG Elements
- Items/Inventory
- Crafting
- Experience Points
- Bosses
- Skill Systems
- Weapon Upgrades
- Special Abilities
- Multiple Characters
- Story
- Multiplayer

---

## Implementation Order
1. Subagent 1: Git setup, GameManager, BuildScript
2. Subagent 2: PlayerController, Input system
3. Subagent 3: Weapon system (all weapons)
4. Subagent 4: Environment, UI, Crosshair
5. Integrate all components
6. Test and build
