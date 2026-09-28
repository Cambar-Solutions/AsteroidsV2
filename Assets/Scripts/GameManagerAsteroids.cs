using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * GameManagerAsteroids
 *
 * Puntaje por asteroides destruidos. Cuando un asteroide golpea
 * la nave, el juego se PAUSA (Time.timeScale = 0) y queda a la
 * espera de que el jugador pulse R para reiniciar.
 */
public class GameManagerAsteroids : MonoBehaviour
{
    private int puntaje;
    private bool juegoTerminado;

    private void Update()
    {
        // Reiniciar con R (funciona incluso con el juego pausado,
        // porque Update se sigue ejecutando aunque timeScale sea 0).
        if (juegoTerminado && Input.GetKeyDown(KeyCode.R))
        {
            Reiniciar();
        }
    }

    public void AsteroideDestruido()
    {
        if (juegoTerminado) return;
        puntaje += 10;
        Debug.Log("Asteroide destruido! Puntaje: " + puntaje);
    }

    public void NaveGolpeada()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        // Pausa total del juego.
        Time.timeScale = 0f;
        Debug.Log("GAME OVER! Puntaje final: " + puntaje + ".  Pulsa R para reiniciar.");
    }

    private void Reiniciar()
    {
        Time.timeScale = 1f;
        Scene actual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(actual.buildIndex);
    }
}
