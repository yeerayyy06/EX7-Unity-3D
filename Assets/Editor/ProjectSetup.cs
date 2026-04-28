// ============================================================
// SCRIPT D'EDITOR - Configura tot el projecte automàticament
// Menú Unity: Naus3D → Configurar Projecte 3D
// ============================================================
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public static class ProjectSetup
{
    [MenuItem("Naus3D/Configurar Projecte 3D")]
    public static void Setup()
    {
        if (!EditorUtility.DisplayDialog("Naus 3D - Configurar Projecte",
            "Això crearà automàticament:\n\n" +
            "• 3 escenes: EscenaInici, EscenaJoc, EscenaResultats\n" +
            "• 8 prefabs: naus, projectils, objecte vida, explosió\n" +
            "• Materials de colors per a cada objecte\n" +
            "• Totes les connexions de scripts i UI\n\n" +
            "IMPORTANT: Primer ves a Window > TextMeshPro > Import TMP Essential Resources\n\n" +
            "Continuar?", "Sí, configura", "Cancel·lar"))
            return;

        // Folders
        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Prefabs");
        EnsureFolder("Assets/Materials");
        EnsureFolder("Assets/Audio");

        // Tags
        AddTag("NauJugador");
        AddTag("Enemic");
        AddTag("ProjectilJugador");
        AddTag("ProjectilEnemic");

        // Materials
        var matJugador  = Mat("NauJugadorMat",         new Color(0.2f, 0.4f, 1f));
        var matEnemic   = Mat("NauEnemicMat",           new Color(0.9f, 0.1f, 0.1f));
        var matEspecial = Mat("NauEspecialMat",         new Color(1f,   0.85f, 0f));
        var matVida     = Mat("ObjecteVidaMat",         new Color(0f,   0.85f, 0.2f));
        var matProjJ    = Mat("ProjectilJugadorMat",    Color.cyan);
        var matProjE    = Mat("ProjectilEnemicMat",     new Color(1f, 0.4f, 0f));
        var matBlanc    = Mat("ExplosioMat",            Color.white);

        AssetDatabase.Refresh();

        // Prefabs (per ordre de dependències)
        var explosio     = PrefabExplosio(matBlanc);
        var projJugador  = PrefabProjectil("ProjectilJugador",        matProjJ, typeof(ProjectilJugador),        "ProjectilJugador");
        var projEnemic   = PrefabProjectil("ProjectilEnemic",         matProjE, typeof(ProjectilEnemic),         "ProjectilEnemic");
        var projEspPref  = PrefabProjectil("ProjectilEnemicEspecial", matProjE, typeof(ProjectilEnemicEspecial), "ProjectilEnemic");
        var objecteVida  = PrefabObjecteVida(matVida);
        var nauEnemic    = PrefabNauEnemic(matEnemic,   explosio, projEnemic);
        var nauEspecial  = PrefabNauEspecial(matEspecial, explosio, projEspPref);
        var nauJugador   = PrefabNauJugador(matJugador,  explosio, projJugador);

        // Escenes
        ScenaInici();
        ScenaJoc(nauJugador, nauEnemic, nauEspecial, objecteVida);
        ScenaResultats();

        // Build Settings
        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene("Assets/Scenes/EscenaInici.unity",     true),
            new EditorBuildSettingsScene("Assets/Scenes/EscenaJoc.unity",       true),
            new EditorBuildSettingsScene("Assets/Scenes/EscenaResultats.unity", true),
        };

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Naus 3D",
            "Projecte configurat correctament!\n\n" +
            "Escenes creades a Assets/Scenes/\n" +
            "Prefabs creats a Assets/Prefabs/\n\n" +
            "Pendent:\n" +
            "• Afegir una música .mp3 a Assets/Audio/\n" +
            "• Arrossegar-la al camp '_musicaFons' del GameManager\n" +
            "• Substituir els cubs per models Blender (opcional)",
            "D'acord");
    }

    // ═══════════════════════════ HELPERS ════════════════════════════

    static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;
        string parent = System.IO.Path.GetDirectoryName(path).Replace('\\', '/');
        string child  = System.IO.Path.GetFileName(path);
        AssetDatabase.CreateFolder(parent, child);
    }

    static void AddTag(string tag)
    {
        var so   = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        var tags = so.FindProperty("tags");
        for (int i = 0; i < tags.arraySize; i++)
            if (tags.GetArrayElementAtIndex(i).stringValue == tag) return;
        tags.arraySize++;
        tags.GetArrayElementAtIndex(tags.arraySize - 1).stringValue = tag;
        so.ApplyModifiedProperties();
    }

    static Material Mat(string name, Color color)
    {
        string path = "Assets/Materials/" + name + ".mat";
        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat != null) return mat;
        mat = new Material(Shader.Find("Standard")) { color = color };
        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }

    static void SetKinematicRigidbody(GameObject go)
    {
        var rb = go.AddComponent<Rigidbody>();
        rb.useGravity  = false;
        rb.isKinematic = true;
    }

    static GameObject SavePrefab(GameObject go, string name)
    {
        var prefab = PrefabUtility.SaveAsPrefabAsset(go, "Assets/Prefabs/" + name + ".prefab");
        Object.DestroyImmediate(go);
        return prefab;
    }

    // ═══════════════════════════ PREFABS ════════════════════════════

    static GameObject PrefabExplosio(Material mat)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = "Explosio";
        go.transform.localScale = Vector3.one * 0.8f;
        Object.DestroyImmediate(go.GetComponent<SphereCollider>());
        go.GetComponent<MeshRenderer>().sharedMaterial = mat;
        go.AddComponent<Explosio>();
        return SavePrefab(go, "Explosio");
    }

    static GameObject PrefabProjectil(string name, Material mat, System.Type script, string tag)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = name;
        go.tag  = tag;
        go.transform.localScale = Vector3.one * 0.3f;
        go.GetComponent<MeshRenderer>().sharedMaterial = mat;
        go.GetComponent<SphereCollider>().isTrigger = true;
        SetKinematicRigidbody(go);
        go.AddComponent(script);
        return SavePrefab(go, name);
    }

    static GameObject PrefabObjecteVida(Material mat)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "ObjecteVida";
        go.transform.localScale = Vector3.one * 0.6f;
        go.GetComponent<MeshRenderer>().sharedMaterial = mat;
        go.GetComponent<BoxCollider>().isTrigger = true;
        SetKinematicRigidbody(go);
        go.AddComponent<ObjecteVida>();
        return SavePrefab(go, "ObjecteVida");
    }

    static GameObject PrefabNauEnemic(Material mat, GameObject explosio, GameObject projEnemic)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "NauEnemic";
        go.tag  = "Enemic";
        go.transform.localScale = new Vector3(1f, 0.25f, 1.5f);
        go.GetComponent<MeshRenderer>().sharedMaterial = mat;
        go.GetComponent<BoxCollider>().isTrigger = true;
        SetKinematicRigidbody(go);
        go.AddComponent<NauEnemic>()._ExplosioPrefab = explosio;
        go.AddComponent<GeneradorProjectilEnemic>()._ProjectilEnemicPrefab = projEnemic;
        return SavePrefab(go, "NauEnemic");
    }

    static GameObject PrefabNauEspecial(Material mat, GameObject explosio, GameObject projEsp)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "NauEnemicEspecial";
        go.tag  = "Enemic";
        go.transform.localScale = new Vector3(1.5f, 0.25f, 2f);
        go.GetComponent<MeshRenderer>().sharedMaterial = mat;
        go.GetComponent<BoxCollider>().isTrigger = true;
        SetKinematicRigidbody(go);
        var s = go.AddComponent<NauEnemicEspecial>();
        s._ExplosioPrefab        = explosio;
        s._ProjectilEnemicPrefab = projEsp;
        return SavePrefab(go, "NauEnemicEspecial");
    }

    static GameObject PrefabNauJugador(Material mat, GameObject explosio, GameObject projJug)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "NauJugador";
        go.tag  = "NauJugador";
        go.transform.localScale = new Vector3(1f, 0.25f, 1.5f);
        go.GetComponent<MeshRenderer>().sharedMaterial = mat;
        go.GetComponent<BoxCollider>().isTrigger = true;
        SetKinematicRigidbody(go);
        go.AddComponent<NauJugador>()._ExplosioPrefab = explosio;
        go.AddComponent<GeneradorProjectils>()._ProjectilPrefab = projJug;
        return SavePrefab(go, "NauJugador");
    }

    // ═══════════════════════════ UI HELPERS ═════════════════════════

    static void SetupCamera(Camera cam)
    {
        cam.transform.position = new Vector3(0, 0, -15);
        cam.transform.rotation = Quaternion.identity;
        cam.backgroundColor    = new Color(0.02f, 0.02f, 0.1f);
        cam.clearFlags         = CameraClearFlags.SolidColor;
        cam.fieldOfView        = 60f;
    }

    static GameObject CreateCanvas()
    {
        var go     = new GameObject("Canvas");
        var canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = go.AddComponent<CanvasScaler>();
        scaler.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        go.AddComponent<GraphicRaycaster>();
        return go;
    }

    static TextMeshProUGUI TMPText(string name, string text, Transform parent,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pos, Vector2 size, int fs = 36)
    {
        var go  = new GameObject(name);
        go.transform.SetParent(parent, false);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text      = text;
        tmp.fontSize  = fs;
        tmp.color     = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin        = anchorMin;
        rt.anchorMax        = anchorMax;
        rt.pivot            = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        rt.sizeDelta        = size;
        return tmp;
    }

    static Button CreateButton(string name, string label, Transform parent, Vector2 pos, Vector2 size)
    {
        var go  = new GameObject(name);
        go.transform.SetParent(parent, false);
        var img = go.AddComponent<Image>();
        img.color = new Color(0.15f, 0.45f, 1f);
        var btn = go.AddComponent<Button>();
        var rt  = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot            = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        rt.sizeDelta        = size;

        // Text label
        var txt = new GameObject("Text");
        txt.transform.SetParent(go.transform, false);
        var tmp = txt.AddComponent<TextMeshProUGUI>();
        tmp.text      = label;
        tmp.fontSize  = 30;
        tmp.color     = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        var trt = txt.GetComponent<RectTransform>();
        trt.anchorMin = Vector2.zero;
        trt.anchorMax = Vector2.one;
        trt.offsetMin = trt.offsetMax = Vector2.zero;
        return btn;
    }

    // ═══════════════════════════ ESCENES ════════════════════════════

    // ── EscenaInici ──────────────────────────────────────────────────
    static void ScenaInici()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        SetupCamera(Camera.main);

        var canvas = CreateCanvas();

        TMPText("TitolJoc", "NAUS 3D", canvas.transform,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0, 160), new Vector2(600, 110), 80);

        TMPText("SubtitolJoc", "EX7 - Unity 3D", canvas.transform,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0, 80), new Vector2(500, 60), 36);

        var btn = CreateButton("BotoJugar", "JUGAR", canvas.transform,
            new Vector2(0, -20), new Vector2(250, 70));

        var pantallaInici = canvas.AddComponent<PantallaInici>();
        UnityEventTools.AddPersistentListener(btn.onClick,
            new UnityAction(pantallaInici.AnarAPantallaJoc));

        EditorSceneManager.SaveScene(scene, "Assets/Scenes/EscenaInici.unity");
    }

    // ── EscenaJoc ────────────────────────────────────────────────────
    static void ScenaJoc(GameObject nauJugPrefab, GameObject nauEnemPrefab,
                         GameObject nauEspPrefab, GameObject vidaPrefab)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        SetupCamera(Camera.main);

        // Canvas
        var canvas = CreateCanvas();

        var tmpPunts = TMPText("TextPunts", "Punts: 0", canvas.transform,
            new Vector2(0, 1), new Vector2(0, 1),
            new Vector2(110, -30), new Vector2(200, 50), 28);
        tmpPunts.gameObject.AddComponent<TextPuntsJugador>();

        var tmpVides = TMPText("TextVides", "Vides: 3", canvas.transform,
            new Vector2(1, 1), new Vector2(1, 1),
            new Vector2(-110, -30), new Vector2(200, 50), 28);
        tmpVides.gameObject.AddComponent<TextVidesJugador>();

        var botoJugar = CreateButton("BotoJugar", "JUGAR", canvas.transform,
            new Vector2(0, 0), new Vector2(250, 70));

        // NauJugador instanciada del prefab
        var nauJugGO = (GameObject)PrefabUtility.InstantiatePrefab(nauJugPrefab);
        nauJugGO.name = "NauJugador";
        nauJugGO.transform.position = new Vector3(0, -4, 0);

        // GeneradorEnemics
        var genEnemGO = new GameObject("GeneradorEnemics");
        var genEnem   = genEnemGO.AddComponent<GeneradorEnemics>();
        genEnem._NauEnemicPrefab         = nauEnemPrefab;
        genEnem._NauEnemicEspecialPrefab = nauEspPrefab;

        // GeneradorObjectesVida
        var genVidaGO = new GameObject("GeneradorObjectesVida");
        var genVida   = genVidaGO.AddComponent<GeneradorObjectesVida>();
        genVida._ObjecteVidaPrefab = vidaPrefab;

        // GameManager
        var gmGO = new GameObject("GameManager");
        var gm   = gmGO.AddComponent<GameManager>();
        gm._botoJugar             = botoJugar.gameObject;
        gm._nauJugador            = nauJugGO;
        gm._generadorEnemics      = genEnemGO;
        gm._textPuntsJugador      = tmpPunts.gameObject;
        gm._textVidesJugador      = tmpVides.gameObject;
        gm._generadorObjectesVida = genVidaGO;

        UnityEventTools.AddPersistentListener(botoJugar.onClick,
            new UnityAction(gm.PassarAEstatJugant));

        // Estat inicial: ocultar jugador i textos
        nauJugGO.SetActive(false);
        tmpPunts.gameObject.SetActive(false);
        tmpVides.gameObject.SetActive(false);

        EditorSceneManager.SaveScene(scene, "Assets/Scenes/EscenaJoc.unity");
    }

    // ── EscenaResultats ──────────────────────────────────────────────
    static void ScenaResultats()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        SetupCamera(Camera.main);

        var canvas = CreateCanvas();

        TMPText("TitolResultats", "RESULTATS", canvas.transform,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0, 200), new Vector2(500, 90), 64);

        var tmpPunts = TMPText("TextPuntsAconseguits", "Punts: 0", canvas.transform,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0, 80), new Vector2(450, 60), 40);

        var tmpVides = TMPText("TextVidesAgafades", "Vides agafades: 0", canvas.transform,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0, 10), new Vector2(450, 60), 40);

        var btn = CreateButton("BotoTornarInici", "TORNAR A L'INICI", canvas.transform,
            new Vector2(0, -100), new Vector2(320, 70));

        // PantallaResultats
        var pr = canvas.AddComponent<PantallaResultats>();
        var so = new SerializedObject(pr);
        so.FindProperty("puntsAconseguits").objectReferenceValue = tmpPunts;
        so.FindProperty("videsAgafades").objectReferenceValue    = tmpVides;
        so.ApplyModifiedProperties();

        UnityEventTools.AddPersistentListener(btn.onClick,
            new UnityAction(pr.TornarAInici));

        EditorSceneManager.SaveScene(scene, "Assets/Scenes/EscenaResultats.unity");
    }
}
