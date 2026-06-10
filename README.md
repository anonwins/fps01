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

### 4. Create Input Actions Asset
1. In Project window (bottom-left panel showing Assets folder), right-click on empty space
2. Select "Create" > "Input Actions"
3. Name the file: `PlayerInputActions` (press Enter to confirm)

### 5. Configure Input Actions (PlayerInputActions file)
1. Double-click the `PlayerInputActions` file you just created
2. This opens the Input Actions editor
3. On left side, under "Action Maps", click the "+" button
4. A new action map appears called "New Action Map" - rename to `Player`
5. Click on "Player" to select it
6. On right side, click "Add Action" button (+) three times to add:
   - `Move` (set Action Type to "Value", Control Type to "Vector2")
   - `Look` (set Action Type to "Value", Control Type to "Vector2")
   - `Jump` (set Action Type to "Button", Control Type to "Button")
   - `Run` (set Action Type to "Button", Control Type to "Button")
   - `Attack` (set Action Type to "Button", Control Type to "Button")
   - `Melee` (set Action Type to "Button", Control Type to "Button")
   - `CycleWeapon` (set Action Type to "Value", Control Type to "Axis")

### 6. Bind Controls to Actions
In the Input Actions editor:
1. Click on `Move` action
2. Under "Bindings", click "+" > "2D Vector Composite" > name it "WASD"
3. For each sub-binding:
   - Up: Click "+" > "Keyboard" > Select "w" or "Up Arrow"
   - Down: Click "+" > "Keyboard" > Select "s" or "Down Arrow"
   - Left: Click "+" > "Keyboard" > Select "a" or "Left Arrow"
   - Right: Click "+" > "Keyboard" > Select "d" or "Right Arrow"

4. Click `Look` action > "+" > "Mouse" > Select "Mouse Delta"
5. Click `Jump` action > "+" > "Keyboard" > Select "Space"
6. Click `Run` action > "+" > "Keyboard" > Select "Left Shift"
7. Click `Attack` action > "+" > "Mouse" > Select "Left Button"
8. Click `Melee` action > "+" > "Mouse" > Select "Right Button"
9. Click `CycleWeapon` action > "+" > "Mouse" > Select "Scroll/Y"

10. Close the Input Actions editor and save when prompted

### 7. Run Setup Tools (Recommended)
1. After saving, look at top menu bar
2. Find "Tools" menu (between "Window" and "Help")
3. Click "Tools" > "Setup Scene"
4. Wait for Console to show "Scene setup complete!"
5. Click "Tools" > "Create Weapon Prefabs"
6. Click "Tools" > "Finalize Scene"

### 8. Save the Scene
1. Press Ctrl+S (or go to File > Save)
2. In save dialog, ensure "Scenes" folder is selected
3. Name: `Main`
4. Click "Save"

### 9. Build the Game
1. Go to top menu: Tools > Build > Build Windows (Standalone)
2. Or use File > Build Settings for more options

## Controls Summary
| Key | Action |
|-----|--------|
| W/A/S/D | Move |
| Mouse | Look around |
| Space | Jump |
| Left Shift | Run (faster movement) |
| Left Mouse | Shoot (Pistol) |
| Right Mouse | Melee (Knife) |
| Mouse Scroll | Cycle weapons |
| Escape | Unlock mouse cursor |

## Project Structure
```
Assets/Scripts/Core/       # GameManager, InputManager
Assets/Scripts/Player/     # PlayerController
Assets/Scripts/Weapons/    # Weapon system (all files here)
Assets/Scripts/Environment/ # WorldGenerator
Assets/Scripts/Utilities/ # IDamageable, ObjectPool
Assets/Editor/            # SceneSetup, BuildScript (editor tools)
Assets/Prefabs/           # Created by tools
Assets/Scenes/            # Main.unity scene
```

## Adding More Weapons
1. Right-click in Assets > Create > Weapons > WeaponData
2. Configure in Inspector:
   - Weapon Name: "New Weapon"
   - Type: Melee or Ranged
   - Damage: 10
   - Range: 100
   - Attack Rate: 1
3. Create empty prefab: Right-click Assets/Prefabs > Create > Empty
4. Add Weapon component (RangedWeapon or MeleeWeapon)
5. Assign WeaponData to the component
6. Add the weapon to WeaponManager.weapons list in Inspector
