using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroide : MonoBehaviour
{
    [SerializeField] private float velocidadMin = 1.5f;
    [SerializeField] private float velocidadMax = 3.5f;
    [SerializeField] private float limiteIzquierdo = -11f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Se mueve hacia la izquierda con ligera deriva vertical y giro propio.
        float vx = -Random.Range(velocidadMin, velocidadMax);
        float vy = Random.Range(-1f, 1f);
        rb.linearVelocity = new Vector2(vx, vy);
        rb.angularVelocity = Random.Range(-90f, 90f);
    }

    private void Update()
    {
        // Si sale por la izquierda, se elimina.
        if (transform.position.x < limiteIzquierdo)
        {
            Destroy(gameObject);
        }
    }

    public void Destruir()
    {
        var gm = Object.FindFirstObjectByType<GameManagerAsteroids>();
        if (gm != null) gm.AsteroideDestruido();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var gm = Object.FindFirstObjectByType<GameManagerAsteroids>();
            if (gm != null) gm.NaveGolpeada();
        }
    }
}
