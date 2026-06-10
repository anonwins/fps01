using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class SceneSetup
{
    [MenuItem("Tools/Setup Scene")]
    public static void SetupMainScene()
    {
        // Create camera
        GameObject cameraObj = new GameObject("PlayerCamera");
        Camera camera = cameraObj.AddComponent<Camera>();
        camera.tag = "MainCamera";
        cameraObj.AddComponent<AudioListener>();
        cameraObj.AddComponent<CharacterController>();
        cameraObj.AddComponent<PlayerController>();
        cameraObj.AddComponent<WeaponManager>();
        cameraObj.AddComponent<InputManager>();

        // Create WeaponHolder
        GameObject weaponHolder = new GameObject("WeaponHolder");
        weaponHolder.transform.SetParent(cameraObj.transform);
        weaponHolder.transform.localPosition = Vector3.zero;

        // Create Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Crosshair - create procedural texture
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

        // Assign to WeaponManager
        WeaponManager wm = cameraObj.GetComponent<WeaponManager>();
        wm.weaponNameText = weaponText;
        wm.weaponHolder = weaponHolder.transform;

        // Create World
        GameObject worldObj = new GameObject("World");
        worldObj.AddComponent<WorldGenerator>();

        // Create directional light
        GameObject lightObj = GameObject.Find("Directional Light");
        if (lightObj == null)
        {
            lightObj = new GameObject("Directional Light");
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
        }

        WorldGenerator wg = worldObj.GetComponent<WorldGenerator>();
        wg.directionalLight = lightObj.GetComponent<Light>();

        // Focus on camera
        Selection.activeGameObject = cameraObj;

        Debug.Log("Scene setup complete! Run Create Weapon Prefabs next.");
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

    [MenuItem("Tools/Create Weapon Prefabs")]
    public static void CreateWeaponPrefabs()
    {
        // Create Ranged Weapon - LineRenderer is auto-added by RequireComponent
        GameObject ranged = new GameObject("RangedWeapon");
        ranged.AddComponent<RangedWeapon>();
        LineRenderer lr = ranged.GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.positionCount = 2;
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            // Create material asset instead of using renderer.material
            Material tracerMat = new Material(Shader.Find("Unlit/Color"));
            tracerMat.color = Color.yellow;
            lr.sharedMaterial = tracerMat;
            AssetDatabase.CreateAsset(tracerMat, "Assets/TracerMaterial.mat");
        }

        // Create Melee Weapon
        GameObject melee = new GameObject("MeleeWeapon");
        melee.AddComponent<MeleeWeapon>();
        GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
        visual.name = "Visual";
        visual.transform.SetParent(melee.transform);
        visual.transform.localPosition = new Vector3(0, 0, 1f);
        visual.transform.localScale = new Vector3(0.1f, 0.3f, 0.05f);
        Object.DestroyImmediate(visual.GetComponent<Collider>());

        // Create WeaponData assets
        WeaponData pistol = ScriptableObject.CreateInstance<WeaponData>();
        pistol.weaponName = "Pistol";
        pistol.type = WeaponType.Ranged;
        pistol.damage = 10f;
        pistol.range = 100f;
        pistol.attackRate = 4f;
        pistol.isAutomatic = false;
        AssetDatabase.CreateAsset(pistol, "Assets/WeaponData_Pistol.asset");

        WeaponData knife = ScriptableObject.CreateInstance<WeaponData>();
        knife.weaponName = "Knife";
        knife.type = WeaponType.Melee;
        knife.damage = 20f;
        knife.range = 2f;
        knife.attackRate = 2f;
        knife.isAutomatic = false;
        AssetDatabase.CreateAsset(knife, "Assets/WeaponData_Knife.asset");

        // Assign weapon data
        ranged.GetComponent<RangedWeapon>().data = pistol;
        melee.GetComponent<MeleeWeapon>().data = knife;

        // Make prefabs
        string rangedPath = "Assets/Prefabs/RangedWeapon.prefab";
        string meleePath = "Assets/Prefabs/MeleeWeapon.prefab";

        PrefabUtility.SaveAsPrefabAsset(ranged, rangedPath);
        PrefabUtility.SaveAsPrefabAsset(melee, meleePath);

        Object.DestroyImmediate(ranged);
        Object.DestroyImmediate(melee);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Weapon prefabs created! Now run Finalize Scene.");
    }

    [MenuItem("Tools/Finalize Scene (Run after Create Weapon Prefabs)")]
    public static void FinalizeScene()
    {
        GameObject cameraObj = GameObject.Find("PlayerCamera");
        if (cameraObj == null) return;

        WeaponManager wm = cameraObj.GetComponent<WeaponManager>();
        if (wm == null) return;

        // Load weapon prefabs
        GameObject rangedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/RangedWeapon.prefab");
        GameObject meleePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MeleeWeapon.prefab");

        if (rangedPrefab != null && meleePrefab != null)
        {
            GameObject rangedInstance = (GameObject)PrefabUtility.InstantiatePrefab(rangedPrefab);
            GameObject meleeInstance = (GameObject)PrefabUtility.InstantiatePrefab(meleePrefab);

            wm.weapons.Add(rangedInstance.GetComponent<RangedWeapon>());
            wm.weapons.Add(meleeInstance.GetComponent<MeleeWeapon>());
            Debug.Log("Weapons assigned to WeaponManager!");
        }
    }
}
