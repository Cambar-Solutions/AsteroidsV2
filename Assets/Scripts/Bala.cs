using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] private float vida = 2f;

    private void Start()
    {
        // La bala desaparece sola tras unos segundos.
        Destroy(gameObject, vida);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroide"))
        {
            var a = other.GetComponent<Asteroide>();
            if (a != null) a.Destruir();
            Destroy(gameObject);
        }
    }
}
