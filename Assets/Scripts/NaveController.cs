using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NaveController : MonoBehaviour
{
    [Header("Movimiento directo (sin inercia)")]
    [SerializeField] private float velocidad = 7f;
    [SerializeField] private float limiteX = 8.5f;
    [SerializeField] private float limiteY = 5.5f;

    [Header("Disparo")]
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private float velocidadBala = 12f;
    [SerializeField] private float cadencia = 0.25f;

    private Rigidbody2D rb;
    private Vector2 input;
    private float siguienteDisparo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Sin inercia: nada de damping raro, nos movemos nosotros.
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
    }

    private void Update()
    {
        // Direccion segun flechas (o WASD). Diagonales incluidas.
        float x = 0f;
        float y = 0f;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) x -= 1f;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) x += 1f;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) y += 1f;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) y -= 1f;

        input = new Vector2(x, y);
        if (input.sqrMagnitude > 1f) input = input.normalized; // diagonal no mas rapida

        // Disparo con espacio (hacia la derecha, sentido del juego horizontal).
        if (Input.GetKey(KeyCode.Space) && Time.time >= siguienteDisparo)
        {
            Disparar();
            siguienteDisparo = Time.time + cadencia;
        }
    }

    private void FixedUpdate()
    {
        // Movimiento directo: se detiene en cuanto sueltas la tecla.
        Vector2 nuevaPos = rb.position + input * velocidad * Time.fixedDeltaTime;
        nuevaPos.x = Mathf.Clamp(nuevaPos.x, -limiteX, limiteX);
        nuevaPos.y = Mathf.Clamp(nuevaPos.y, -limiteY, limiteY);
        rb.MovePosition(nuevaPos);
    }

    private void Disparar()
    {
        if (balaPrefab == null) return;

        // La nave apunta a la derecha; la bala sale hacia la derecha.
        Vector3 punta = transform.position + Vector3.right * 0.6f;
        GameObject bala = Instantiate(balaPrefab, punta, Quaternion.identity);

        var rbBala = bala.GetComponent<Rigidbody2D>();
        if (rbBala != null)
        {
            rbBala.linearVelocity = Vector2.right * velocidadBala;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroide"))
        {
            var gm = Object.FindFirstObjectByType<GameManagerAsteroids>();
            if (gm != null) gm.NaveGolpeada();
        }
    }
}
