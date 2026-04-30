using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PearBiRPDemoSceneBuilder
{
    private const string ShaderName = "Pear/BiRP/Toon";

    [MenuItem("Tools/Pear/Create BiRP Demo Scene")]
    public static void CreateDemoScene()
    {
        Shader pearShader = Shader.Find(ShaderName);

        if (pearShader == null)
        {
            EditorUtility.DisplayDialog(
                "Pear BiRP Shader Missing",
                "Could not find Shader \"" + ShaderName + "\".\n\nMake sure PearToon_BiRP.shader is inside Assets/Pear/Runtime/Shaders and has compiled without errors.",
                "OK"
            );

            return;
        }

        CreateFolders();

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "PearBiRPDemo";

        CreateCamera();
        CreateDirectionalLight();
        CreateFloor();

        Material softAnime = CreateMaterial("M_Pear_BiRP_SoftAnime", pearShader);
        PearPresetApplier.ApplySoftAnime(softAnime);

        Material celShadow = CreateMaterial("M_Pear_BiRP_CelShadow", pearShader);
        PearPresetApplier.ApplyCelShadow(celShadow);

        Material glossyToy = CreateMaterial("M_Pear_BiRP_GlossyToy", pearShader);
        PearPresetApplier.ApplyGlossyToy(glossyToy);

        CreateDisplaySphere("Soft Anime", softAnime, new Vector3(-2.4f, 1.0f, 0f));
        CreateDisplaySphere("Cel Shadow", celShadow, new Vector3(0f, 1.0f, 0f));
        CreateDisplaySphere("Glossy Toy", glossyToy, new Vector3(2.4f, 1.0f, 0f));

        EditorSceneManager.SaveScene(scene, "Assets/Pear/Demo/Scenes/PearBiRPDemo.unity");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog(
            "Pear BiRP Demo Created",
            "Created Assets/Pear/Demo/Scenes/PearBiRPDemo.unity with three BiRP Pear materials.",
            "OK"
        );
    }

    private static void CreateFolders()
    {
        CreateFolderIfMissing("Assets/Pear");
        CreateFolderIfMissing("Assets/Pear/Demo");
        CreateFolderIfMissing("Assets/Pear/Demo/Scenes");
        CreateFolderIfMissing("Assets/Pear/Demo/Materials");
        CreateFolderIfMissing("Assets/Pear/Demo/Materials/BiRP");
    }

    private static void CreateFolderIfMissing(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
        {
            return;
        }

        string parent = System.IO.Path.GetDirectoryName(path).Replace("\\", "/");
        string folderName = System.IO.Path.GetFileName(path);

        AssetDatabase.CreateFolder(parent, folderName);
    }

    private static Material CreateMaterial(string materialName, Shader shader)
    {
        string path = "Assets/Pear/Demo/Materials/BiRP/" + materialName + ".mat";

        Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);

        if (existing != null)
        {
            existing.shader = shader;
            EditorUtility.SetDirty(existing);
            return existing;
        }

        Material material = new Material(shader)
        {
            name = materialName
        };

        AssetDatabase.CreateAsset(material, path);
        return material;
    }

    private static void CreateCamera()
    {
        GameObject cameraObject = new GameObject("Main Camera");
        Camera camera = cameraObject.AddComponent<Camera>();

        cameraObject.tag = "MainCamera";
        cameraObject.transform.position = new Vector3(0f, 2.2f, -6.5f);
        cameraObject.transform.rotation = Quaternion.Euler(15f, 0f, 0f);

        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.92f, 0.95f, 0.84f, 1f);
        camera.fieldOfView = 38f;
    }

    private static void CreateDirectionalLight()
    {
        GameObject lightObject = new GameObject("Directional Light");
        Light light = lightObject.AddComponent<Light>();

        light.type = LightType.Directional;
        light.color = new Color(1.0f, 0.96f, 0.82f, 1f);
        light.intensity = 1.15f;
        light.shadows = LightShadows.Soft;

        lightObject.transform.rotation = Quaternion.Euler(48f, -32f, 0f);
    }

    private static void CreateFloor()
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Pear Demo Floor";
        floor.transform.position = Vector3.zero;
        floor.transform.localScale = new Vector3(1.25f, 1f, 1.25f);

        Material floorMaterial = new Material(Shader.Find("Standard"));
        floorMaterial.name = "M_Pear_BiRP_DemoFloor";
        floorMaterial.color = new Color(0.86f, 0.91f, 0.74f, 1f);

        Renderer renderer = floor.GetComponent<Renderer>();
        renderer.sharedMaterial = floorMaterial;
    }

    private static void CreateDisplaySphere(string label, Material material, Vector3 position)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.name = label;
        sphere.transform.position = position;
        sphere.transform.localScale = Vector3.one * 1.45f;

        Renderer renderer = sphere.GetComponent<Renderer>();
        renderer.sharedMaterial = material;

        GameObject textObject = new GameObject(label + " Label");
        textObject.transform.position = new Vector3(position.x, 0.12f, position.z - 1.05f);
        textObject.transform.rotation = Quaternion.Euler(70f, 0f, 0f);

        TextMesh textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = label;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.fontSize = 42;
        textMesh.characterSize = 0.055f;
        textMesh.color = new Color(0.20f, 0.26f, 0.13f, 1f);
    }
}
