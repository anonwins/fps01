# FPS Prototype - Unity 6.x

## Setup Instructions

### 1. Prerequisites
- Unity Hub installed
- Unity 6.x installed via Hub
- This project folder

### 2. Open Project in Unity
1. Launch Unity Hub
2. Click "Open" button (NOT "Add")
3. Navigate to: `C:\Users\TSF\Desktop\fps01`
4. Select the folder and click "Select Folder"
5. Wait for Unity to finish importing (progress bar at bottom)

### 3. Enable New Input System
1. After project loads, go to top menu: Window > Package Manager
2. In Package Manager window (left sidebar):
   - Find "Input System" in Unity Registry
   - Click "Install" button (if not already installed)
3. Go to top menu: Edit > Project Settings
4. In Project Settings window (left sidebar), click "Player"
5. On right side, expand "Other Settings" section
6. Find "Active Input Handling" dropdown
7. Change from "Input Manager (Old)" to "Input System Package (New)"
8. Unity will show a restart prompt - click "Restart" or "Restart Later"

### 4. Run Setup Tools
1. After Unity restarts, look at top menu bar
2. Find "Tools" menu (between "Window" and "Help")
3. Click "Tools" > "Setup Scene"
4. Wait for Console to show "Scene setup complete!"
5. Click "Tools" > "Create Weapon Prefabs"
6. Click "Tools" > "Finalize Scene"

### 5. Save the Scene
1. Press Ctrl+S (or go to File > Save)
2. In save dialog, ensure "Scenes" folder is selected
3. Name: `Main`
4. Click "Save"

### 6. Build the Game
1. Go to top menu: Tools > Build > Build Windows (Standalone)
2. Or use File > Build Settings for more options

## Controls Summary
| Key | Action |
|-----|--------|
| W/A/S/D | Move |
| Mouse | Look around |
| Space | Jump |
| Left Shift | Run (faster movement) |
| Left Mouse | Attack (works for both melee and ranged weapons) |
| Mouse Scroll | Cycle weapons |
| Escape | Unlock mouse cursor |

## Project Structure
```
Assets/Scripts/Core/       # GameManager, InputManager
Assets/Scripts/Player/     # PlayerController
Assets/Scripts/Weapons/    # Weapon system
Assets/Scripts/Environment/ # WorldGenerator
Assets/Scripts/Utilities/  # IDamageable, ObjectPool
Assets/Editor/             # SceneSetup, BuildScript
```

## Adding More Weapons
1. Right-click in Assets > Create > Weapons > WeaponData
2. Configure: Name, Type, Damage, Range, Attack Rate
3. Create prefab, add Weapon component, assign data
4. Add to WeaponManager.weapons list in Inspector
