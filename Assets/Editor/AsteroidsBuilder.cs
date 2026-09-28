using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public static class AsteroidsBuilder
{
    [MenuItem("Asteroids/Reconstruir escena")]
    public static void Construir()
    {
        Directory.CreateDirectory("Assets/Sprites");
        Directory.CreateDirectory("Assets/Prefabs");

        // Sprites generados por codigo.
        Sprite triangulo = GuardarSprite(GenerarTriangulo(64), "Assets/Sprites/Triangulo.png");
        Sprite rombo = GuardarSprite(GenerarRombo(32), "Assets/Sprites/Rombo.png");
        Sprite circulo = GuardarSprite(GenerarCirculo(64), "Assets/Sprites/Circulo.png");

        // Prefab de BALA (circulo pequenio, trigger).
        var balaPrefab = CrearPrefabBala(circulo);
        // Prefab de ASTEROIDE (rombo gris, trigger).
        var asteroidePrefab = CrearPrefabAsteroide(rombo);

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Camara: espacio negro.
        var camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 6;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.08f, 0.03f, 0.12f);
        camGO.transform.position = new Vector3(0, 0, -10);
        camGO.tag = "MainCamera";

        // Estrellas decorativas (rombos, mas grandes y menos cantidad).
        var estrellas = new GameObject("Estrellas");
        for (int i = 0; i < 25; i++)
        {
            var e = new GameObject("Estrella");
            e.transform.SetParent(estrellas.transform);
            e.transform.position = new Vector3(Random.Range(-9f, 9f), Random.Range(-6f, 6f), 1f);
            float s = Random.Range(0.6f, 1.1f);
            e.transform.localScale = new Vector3(s, s, 1f);
            var sr = e.AddComponent<SpriteRenderer>();
            sr.sprite = rombo;
            sr.color = new Color(0.6f, 0.9f, 1f, Random.Range(0.35f, 0.8f));
            sr.sortingOrder = -10;
        }

        // NAVE: triangulo rojo. En modo horizontal arranca a la
        // izquierda apuntando hacia la derecha (rotada -90 en Z).
        var nave = new GameObject("Nave");
        nave.tag = "Player";
        nave.transform.position = new Vector3(-7f, 0f, 0f);
        nave.transform.rotation = Quaternion.Euler(0, 0, -90f);
        nave.transform.localScale = new Vector3(0.8f, 0.9f, 1f);
        var srNave = nave.AddComponent<SpriteRenderer>();
        srNave.sprite = triangulo;
        srNave.color = new Color(0.2f, 0.95f, 0.75f);
        srNave.sortingOrder = 5;
        var colNave = nave.AddComponent<PolygonCollider2D>();
        colNave.isTrigger = true;
        var rbNave = nave.AddComponent<Rigidbody2D>();
        rbNave.gravityScale = 0;
        rbNave.linearDamping = 0.5f;
        rbNave.angularDamping = 0.5f;
        var nc = nave.AddComponent<NaveController>();
        AsignarPrefab(nc, "balaPrefab", balaPrefab);

        // Detalle: triangulo azul chiquito dentro del rojo.
        var detalle = new GameObject("DetalleAzul");
        detalle.transform.SetParent(nave.transform, false);
        detalle.transform.localPosition = new Vector3(0f, 0.05f, 0f);
        detalle.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
        var srDet = detalle.AddComponent<SpriteRenderer>();
        srDet.sprite = triangulo;
        srDet.color = new Color(0.95f, 0.2f, 0.7f);
        srDet.sortingOrder = 6;

        // GameManager.
        var gm = new GameObject("GameManager");
        gm.AddComponent<GameManagerAsteroids>();

        // Spawner de asteroides.
        var sp = new GameObject("Spawner");
        var spc = sp.AddComponent<SpawnerAsteroides>();
        AsignarPrefab(spc, "asteroidePrefab", asteroidePrefab);

        EditorSceneManager.SaveScene(scene, "Assets/Scenes/Asteroids.unity");
        Debug.Log("ASTEROIDS: escena construida");
    }

    private static GameObject CrearPrefabBala(Sprite circulo)
    {
        var go = new GameObject("Bala");
        go.tag = "Bala";
        go.transform.localScale = new Vector3(0.15f, 0.15f, 1f);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = circulo;
        sr.color = new Color(0.4f, 1f, 1f);
        sr.sortingOrder = 4;
        var col = go.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        var rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        go.AddComponent<Bala>();

        string path = "Assets/Prefabs/Bala.prefab";
        var prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        return prefab;
    }

    private static GameObject CrearPrefabAsteroide(Sprite rombo)
    {
        var go = new GameObject("Asteroide");
        go.tag = "Asteroide";
        go.transform.localScale = Vector3.one;
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = rombo;
        sr.color = new Color(0.95f, 0.45f, 0.55f);
        sr.sortingOrder = 3;
        var col = go.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        var rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        go.AddComponent<Asteroide>();

        string path = "Assets/Prefabs/Asteroide.prefab";
        var prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        return prefab;
    }

    private static void AsignarPrefab(MonoBehaviour comp, string campo, GameObject prefab)
    {
        var so = new SerializedObject(comp);
        so.FindProperty(campo).objectReferenceValue = prefab;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    // ---- Generadores de textura ----

    private static Texture2D GenerarTriangulo(int size)
    {
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float fx = (float)x / (size - 1);
            float fy = (float)y / (size - 1);
            // Triangulo apuntando hacia arriba: ancho en la base (y=0), punta arriba.
            float mitadAncho = (1f - fy) * 0.5f;
            bool dentro = fx >= 0.5f - mitadAncho && fx <= 0.5f + mitadAncho;
            tex.SetPixel(x, y, dentro ? Color.white : Color.clear);
        }
        tex.Apply();
        return tex;
    }

    private static Texture2D GenerarRombo(int size)
    {
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        float c = (size - 1) / 2f;
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float d = Mathf.Abs(x - c) / c + Mathf.Abs(y - c) / c;
            tex.SetPixel(x, y, d <= 1f ? Color.white : Color.clear);
        }
        tex.Apply();
        return tex;
    }

    private static Texture2D GenerarCirculo(int size)
    {
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        float c = (size - 1) / 2f;
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float d = Mathf.Sqrt((x - c) * (x - c) + (y - c) * (y - c)) / c;
            tex.SetPixel(x, y, d <= 1f ? Color.white : Color.clear);
        }
        tex.Apply();
        return tex;
    }

    private static Sprite GuardarSprite(Texture2D tex, string path)
    {
        File.WriteAllBytes(path, tex.EncodeToPNG());
        AssetDatabase.ImportAsset(path);
        var importer = (TextureImporter)AssetImporter.GetAtPath(path);
        importer.textureType = TextureImporterType.Sprite;
        importer.spritePixelsPerUnit = 64;
        importer.filterMode = FilterMode.Bilinear;
        importer.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }
}
