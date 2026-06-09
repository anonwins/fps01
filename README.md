# FPS Prototype - Unity 6.x

## Setup Instructions

### 1. Prerequisites
- Unity Hub installed
- Unity 6.x installed via Hub
- This project folder

### 2. Open Project in Unity
1. Open Unity Hub
2. Click "Open" and select this folder
3. Unity will import the project

### 3. Enable New Input System
1. Window > Package Manager
2. Find "Input System" and install
3. Edit > Project Settings > Player > Other Settings
4. Set "Active Input Handling" to "Input System Package (New)"

### 4. Create Input Actions Asset
1. Right-click in Assets > Create > Input Actions
2. Name it "PlayerInputActions"
3. Add Action Map: "Player" 
4. Add Actions:
   - Move (Value, Vector2) - WASD composite
   - Look (Value, Vector2) - Mouse delta
   - Jump (Button, Button) - Space
   - Run (Button, Button) - Left Shift
   - Attack (Button, Button) - Left Mouse
   - Melee (Button, Button) - Right Mouse
   - CycleWeapon (Value, Axis) - Scroll/Click

### 5. Run Setup Tools
1. Tools > Setup Scene
2. Tools > Create Weapon Prefabs
3. Tools > Finalize Scene

### 6. Build
- Tools > Build > Build Windows (Standalone)

## Controls
- WASD: Move | Mouse: Look | Space: Jump | LShift: Run
- LMouse: Shoot (Pistol) | RMouse: Melee (Knife)
- Scroll: Cycle weapons | Escape: Unlock cursor

## Project Structure
```
Assets/Scripts/Core/      # GameManager, InputManager
Assets/Scripts/Player/    # PlayerController
Assets/Scripts/Weapons/   # WeaponData, WeaponBase, RangedWeapon, MeleeWeapon, WeaponManager
Assets/Scripts/Environment/# WorldGenerator
Assets/Scripts/Utilities/ # IDamageable, ObjectPool
Assets/Prefabs/           # RangedWeapon, MeleeWeapon
Assets/Editor/            # SceneSetup, BuildScript
```

## To Add Weapons
1. Create WeaponData asset (right-click > Weapons > WeaponData)
2. Configure: name, type, damage, range, attackRate
3. Create prefab with appropriate Weapon component
4. Add to WeaponManager.weapons list