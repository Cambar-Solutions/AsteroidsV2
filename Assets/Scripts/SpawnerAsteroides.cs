using UnityEngine;

public class SpawnerAsteroides : MonoBehaviour
{
    [SerializeField] private GameObject asteroidePrefab;
    [SerializeField] private float intervalo = 0.35f;
    [SerializeField] private float rangoY = 5f;
    [SerializeField] private float xSpawn = 10f; // aparece a la derecha, fuera de pantalla

    private float siguiente;

    private void Update()
    {
        if (asteroidePrefab == null) return;

        if (Time.time >= siguiente)
        {
            Generar();
            siguiente = Time.time + intervalo;
        }
    }

    private void Generar()
    {
        float y = Random.Range(-rangoY, rangoY);
        Vector3 pos = new Vector3(xSpawn, y, 0f);
        float escala = 2.5f;

        GameObject a = Instantiate(asteroidePrefab, pos, Quaternion.identity);
        a.transform.localScale = new Vector3(escala, escala, 1f);
    }
}
