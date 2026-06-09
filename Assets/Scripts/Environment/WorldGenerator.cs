using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Ground")]
    public float groundSize = 100f;
    public Material groundMaterial;

    [Header("Sky")]
    public Material skyboxMaterial;
    public float skyboxExposure = 1.0f;

    [Header("Moon")]
    public float moonScale = 5f;
    public float moonDistance = 500f;
    public Material moonMaterial;

    [Header("Lighting")]
    public Light directionalLight;

    private void Start()
    {
        SetupGround();
        SetupSkybox();
        SetupMoon();
        SetupLighting();
    }

    private void SetupGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.transform.localScale = Vector3.one * (groundSize / 10f);
        ground.name = "Ground";

        if (groundMaterial != null)
        {
            ground.GetComponent<Renderer>().material = groundMaterial;
        }

        AddInvisibleWalls(groundSize);
    }

    private void AddInvisibleWalls(float size)
    {
        float wallHeight = 10f;
        float wallThickness = 1f;

        Vector3[] wallPositions = new Vector3[]
        {
            new Vector3(0, wallHeight/2, size/2),
            new Vector3(0, wallHeight/2, -size/2),
            new Vector3(size/2, wallHeight/2, 0),
            new Vector3(-size/2, wallHeight/2, 0)
        };

        Vector3[] wallScales = new Vector3[]
        {
            new Vector3(size, wallHeight, wallThickness),
            new Vector3(size, wallHeight, wallThickness),
            new Vector3(wallThickness, wallHeight, size),
            new Vector3(wallThickness, wallHeight, size)
        };

        for (int i = 0; i < wallPositions.Length; i++)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.position = wallPositions[i];
            wall.transform.localScale = wallScales[i];
            wall.GetComponent<Renderer>().enabled = false;
            wall.GetComponent<Collider>().isTrigger = false;
        }
    }

    private void SetupSkybox()
    {
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;
        }
    }

    private void SetupMoon()
    {
        GameObject moon = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        moon.name = "Moon";
        moon.transform.localScale = Vector3.one * moonScale;
        moon.transform.position = new Vector3(0, moonDistance, 0);

        if (moonMaterial != null)
        {
            moon.GetComponent<Renderer>().material = moonMaterial;
        }

        if (directionalLight != null)
        {
            directionalLight.transform.LookAt(moon.transform);
        }
    }

    private void SetupLighting()
    {
        if (directionalLight == null)
        {
            directionalLight = FindFirstObjectByType<Light>();
        }

        if (directionalLight != null)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.1f, 0.1f, 0.15f);
        }
    }
}