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

        // Crosshair
        GameObject crosshairObj = new GameObject("Crosshair");
        Image crosshair = crosshairObj.AddComponent<Image>();
        crosshair.transform.SetParent(canvasObj.transform);
        crosshair.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        crosshair.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        crosshair.rectTransform.anchoredPosition = Vector2.zero;
        crosshair.rectTransform.sizeDelta = new Vector2(20, 20);
        crosshair.color = Color.white;

        // Weapon Name Text
        GameObject textObj = new GameObject("WeaponName");
        Text weaponText = textObj.AddComponent<Text>();
        weaponText.transform.SetParent(canvasObj.transform);
        weaponText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
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
            lightObj.AddComponent<AudioListener>(); // Remove this duplicate
            DestroyImmediate(lightObj.GetComponent<AudioListener>());
        }

        WorldGenerator wg = worldObj.GetComponent<WorldGenerator>();
        wg.directionalLight = lightObj.GetComponent<Light>();

        // Focus on camera
        Selection.activeGameObject = cameraObj;

        Debug.Log("Scene setup complete! Assign weapon prefabs to WeaponManager.weapons");
    }

    [MenuItem("Tools/Create Weapon Prefabs")]
    public static void CreateWeaponPrefabs()
    {
        // Create Ranged Weapon
        GameObject ranged = new GameObject("RangedWeapon");
        ranged.AddComponent<RangedWeapon>();
        LineRenderer lr = ranged.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = Color.yellow;

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
        PrefabUtility.SaveAsPrefabAsset(ranged, "Assets/Prefabs/RangedWeapon.prefab");
        PrefabUtility.SaveAsPrefabAsset(melee, "Assets/Prefabs/MeleeWeapon.prefab");

        Object.DestroyImmediate(ranged);
        Object.DestroyImmediate(melee);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Weapon prefabs created!");
    }
}