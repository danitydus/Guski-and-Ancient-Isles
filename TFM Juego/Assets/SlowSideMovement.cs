using UnityEngine;

public class SlowSideMovement : MonoBehaviour
{
    public float speed = 0.1f; // Velocidad de movimiento
    private Vector3 direction = Vector3.right; // Direcci�n inicial del movimiento
    private float timeElapsed = 0f; // Tiempo transcurrido

    void Update()
    {
        // Actualizar el tiempo transcurrido
        timeElapsed += Time.deltaTime;

        // Mover el GameObject en la direcci�n actual
        transform.Translate(direction * speed * Time.deltaTime);

        // Cambiar de direcci�n cada 60 segundos
        if (timeElapsed >= 60f)
        {
            direction = -direction; // Invertir la direcci�n
            timeElapsed = 0f; // Reiniciar el contador de tiempo
        }
    }
}
