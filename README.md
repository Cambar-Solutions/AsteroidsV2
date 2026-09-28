# Asteroids V2 (Unity 2D)

Shooter espacial horizontal hecho en Unity **6000.6.0f1**.
Variante con tonos neón (nave verde-cian, asteroides rosados, balas cian, fondo morado).

## Controles
- **Flechas / WASD:** mover la nave (8 direcciones, control directo sin inercia).
- **Espacio:** disparar.
- **R:** reiniciar cuando el juego se pausa tras un impacto.

## Mecánica
- La nave (triángulo verde-cian con detalle magenta) se mueve libremente por la pantalla.
- Los asteroides (rombos rosados, todos del mismo tamaño) entran por la derecha en gran cantidad.
- Dispara para destruirlos y sumar puntos (se ven en la Console).
- Si un asteroide toca la nave, el juego se **pausa**; pulsa **R** para reiniciar.
- Estrellas decorativas (rombos grandes) de fondo.

## Cómo abrir y correr
1. Clonar el repo.
2. Abrir la carpeta del proyecto desde **Unity Hub** (versión 6000.6.0f1).
3. Abrir la escena `Assets/Scenes/Asteroids.unity`.
4. Pulsar **Play**.

> La carpeta `Library/` no se incluye (se regenera automáticamente al abrir el proyecto en Unity).

## Estructura
- `Assets/Scripts/` — NaveController, Bala, Asteroide, SpawnerAsteroides, GameManagerAsteroids.
- `Assets/Editor/AsteroidsBuilder.cs` — genera sprites (triángulo/rombo/círculo) y arma la escena.
- `Assets/Scenes/Asteroids.unity` — escena jugable.
