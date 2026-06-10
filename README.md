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

### 4. Input Actions Asset
The `PlayerInputActions.inputactions` file is already configured. If recreating manually:

**Actions to add (click "+" for each):**
- MoveX (Value, Axis) - for A/D keys
- MoveY (Value, Axis) - for W/S keys
- Look (Value, Vector2)
- Jump (Button)
- Run (Button)
- Attack (Button)
- CycleWeapon (Value, Axis)

**Bind each action:**
- **MoveX**: `<Keyboard>/d` (positive), `<Keyboard>/a` (negative)
- **MoveY**: `<Keyboard>/w` (positive), `<Keyboard>/s` (negative)
- **Look**: `<Mouse>/delta`
- **Jump**: `<Keyboard>/space`
- **Run**: `<Keyboard>/leftShift`
- **Attack**: `<Mouse>/leftButton`
- **CycleWeapon**: `<Mouse>/scroll`

Save and close the editor.

### 5. Assign Input Actions to PlayerInput
1. Select "PlayerCamera" in Hierarchy
2. On PlayerInput component, find "Actions" field
3. Drag `PlayerInputActions` asset into it
4. Set "Default Control Scheme" to "Keyboard&Mouse"
5. Set "Default Action Map" to "Player"

### 6. Build the Game
1. Top menu: Build > Build Windows (Standalone)

## Controls
| Key | Action |
|-----|--------|
| W/A/S/D | Move |
| Mouse | Look |
| Space | Jump |
| LShift | Run |
| LMouse | Attack |
| Scroll | Cycle weapons |
| Escape | Unlock cursor |
