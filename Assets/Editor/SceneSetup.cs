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
        cam.tag = "MainCamera";
        cameraObj.AddComponent<AudioListener>();
        cameraObj.AddComponent<CharacterController>();
        cameraObj.AddComponent<PlayerController>();
        
        // Add InputManager and PlayerInput
        InputManager inputMgr = cameraObj.AddComponent<InputManager>();
        PlayerInput playerInput = cameraObj.AddComponent<PlayerInput>();
        InputActionAsset actions = CreateInputActionsAsset();
        if (actions != null) playerInput.actions = actions;
        playerInput.defaultControlScheme = "Keyboard&Mouse";
        playerInput.defaultActionMap = "Player";

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
        WeaponBase[] weapons = CreateWeapons();
        foreach (var weapon in weapons)
        {
            wm.weapons.Add(weapon);
            weapon.gameObject.SetActive(false);
        }

        if (wm.weapons.Count > 0)
        {
            wm.EquipWeapon(0);
        }

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

    private static WeaponBase[] CreateWeapons()
    {
        WeaponBase[] weapons = new WeaponBase[2];

        // Create Pistol
        GameObject pistObj = new GameObject("Pistol");
        RangedWeapon pistol = pistObj.AddComponent<RangedWeapon>();
        LineRenderer lr = pistObj.GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.positionCount = 2;
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            Material tracerMat = new Material(Shader.Find("Unlit/Color"));
            tracerMat.color = Color.yellow;
            lr.sharedMaterial = tracerMat;
        }
        WeaponData pistolData = ScriptableObject.CreateInstance<WeaponData>();
        pistolData.weaponName = "Pistol";
        pistolData.type = WeaponType.Ranged;
        pistolData.damage = 10f;
        pistolData.range = 100f;
        pistolData.attackRate = 4f;
        pistolData.isAutomatic = false;
        pistol.data = pistolData;
        weapons[0] = pistol;

        // Create Knife
        GameObject knifeObj = new GameObject("Knife");
        MeleeWeapon knife = knifeObj.AddComponent<MeleeWeapon>();
        WeaponData knifeData = ScriptableObject.CreateInstance<WeaponData>();
        knifeData.weaponName = "Knife";
        knifeData.type = WeaponType.Melee;
        knifeData.damage = 20f;
        knifeData.range = 2f;
        knifeData.attackRate = 2f;
        knifeData.isAutomatic = false;
        knife.data = knifeData;
        weapons[1] = knife;

        return weapons;
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
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }
}