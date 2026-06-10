using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public static class SceneSetup
{
    [MenuItem("Tools/Setup Scene")]
    public static void SetupScene()
    {
        // Create a fresh scene (clears everything)
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        EditorSceneManager.SetActiveScene(newScene);

        // Create camera with all components
        GameObject cameraObj = new GameObject("PlayerCamera");
        Camera cam = cameraObj.AddComponent<Camera>();
        cameraObj.tag = "MainCamera";
        cameraObj.AddComponent<AudioListener>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.fieldOfView = 60f;
        cam.enabled = true;
        cameraObj.AddComponent<PlayerController>();

        // Add InputManager (auto-adds CharacterController and PlayerInput via RequireComponent)
        InputManager inputMgr = cameraObj.AddComponent<InputManager>();
        PlayerInput playerInput = cameraObj.GetComponent<PlayerInput>();
        InputActionAsset actions = CreateInputActionsAsset();
        if (actions != null && playerInput != null)
        {
            playerInput.actions = actions;
            playerInput.defaultControlScheme = "Keyboard&Mouse";
            playerInput.defaultActionMap = "Player";
        }

        // Add WeaponManager
        WeaponManager wm = cameraObj.AddComponent<WeaponManager>();

        // Create WeaponHolder
        GameObject weaponHolder = new GameObject("WeaponHolder");
        weaponHolder.transform.SetParent(cameraObj.transform);
        weaponHolder.transform.localPosition = Vector3.zero;
        wm.weaponHolder = weaponHolder.transform;

        // Create Canvas UI
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Crosshair
        Texture2D crosshairTexture = CreateCrosshairTexture();
        GameObject crosshairObj = new GameObject("Crosshair");
        Image crosshair = crosshairObj.AddComponent<Image>();
        crosshair.transform.SetParent(canvasObj.transform);
        crosshair.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        crosshair.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        crosshair.rectTransform.anchoredPosition = Vector2.zero;
        crosshair.rectTransform.sizeDelta = new Vector2(20, 20);
        crosshair.sprite = Sprite.Create(crosshairTexture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        crosshair.color = Color.white;

        // Weapon Name Text
        GameObject textObj = new GameObject("WeaponName");
        Text weaponText = textObj.AddComponent<Text>();
        weaponText.transform.SetParent(canvasObj.transform);
        weaponText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        weaponText.alignment = TextAnchor.MiddleRight;
        weaponText.rectTransform.anchorMin = new Vector2(1, 1);
        weaponText.rectTransform.anchorMax = new Vector2(1, 1);
        weaponText.rectTransform.anchoredPosition = new Vector2(-10, -10);
        weaponText.rectTransform.sizeDelta = new Vector2(200, 30);
        weaponText.color = Color.white;
        wm.weaponNameText = weaponText;

        // Create directional light
        GameObject lightObj = new GameObject("Directional Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.transform.rotation = Quaternion.Euler(50, -30, 0);

        // Create World
        GameObject worldObj = new GameObject("World");
        WorldGenerator wg = worldObj.AddComponent<WorldGenerator>();
        wg.directionalLight = light;

        // Create weapons
        CreateWeapon<RangedWeapon>("Pistol", WeaponType.Ranged, 10f, 100f, 4f, wm);
        CreateWeapon<MeleeWeapon>("Knife", WeaponType.Melee, 20f, 2f, 2f, wm);

        Selection.activeGameObject = cameraObj;
        EditorSceneManager.MarkSceneDirty(newScene);
        Debug.Log("Scene setup complete!");
    }

    private static InputActionAsset CreateInputActionsAsset()
    {
        string path = "Assets/PlayerInputActions.inputactions";
        var actions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(path);
        if (actions == null)
        {
            Debug.LogError("PlayerInputActions.inputactions not found at " + path);
        }
        return actions;
    }

    private static Texture2D CreateCrosshairTexture()
    {
        int size = 32;
        Texture2D tex = new Texture2D(size, size);
        Color32[] colors = new Color32[size * size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                int i = y * size + x;
                colors[i] = new Color32(255, 255, 255, 255);

                bool isHorizontal = y == size / 2;
                bool isVertical = x == size / 2;
                bool isDiagonal1 = (x == y && Mathf.Abs(x - size / 2) < 4);
                bool isDiagonal2 = (x + y == size - 1 && Mathf.Abs(x - size / 2) < 4);

                if (!(isHorizontal || isVertical || isDiagonal1 || isDiagonal2))
                {
                    colors[i] = new Color32(0, 0, 0, 0);
                }
            }
        }

        tex.SetPixels32(colors);
        tex.Apply();
        return tex;
    }

    [MenuItem("Build/Build Windows (Standalone)")]
    public static void BuildWindows()
    {
        string[] scenes = FindEnabledEditorScenes();
        string buildPath = "Builds/Windows/";
        System.IO.Directory.CreateDirectory(buildPath);
        BuildPipeline.BuildPlayer(scenes, buildPath + "FPSPrototype.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    private static string[] FindEnabledEditorScenes()
    {
        var scenePaths = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenePaths.Length; i++)
            scenePaths[i] = EditorBuildSettings.scenes[i].path;
        return scenePaths;
    }

    private static void CreateWeapon<T>(string name, WeaponType type, float damage, float range, float attackRate, WeaponManager wm) where T : WeaponBase
    {
        GameObject obj = new GameObject(name);
        T weapon = obj.AddComponent<T>();
        
        var data = ScriptableObject.CreateInstance<WeaponData>();
        data.weaponName = name;
        data.type = type;
        data.damage = damage;
        data.range = range;
        data.attackRate = attackRate;
        weapon.data = data;
        wm.weapons.Add(weapon);
    }
}