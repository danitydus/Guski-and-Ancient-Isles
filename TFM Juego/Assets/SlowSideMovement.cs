using UnityEngine;

public class SlowSideMovement : MonoBehaviour
{
    public float speed = 0.1f; // Velocidad de movimiento
    private Vector3 direction = Vector3.right; // Dirección inicial del movimiento
    private float timeElapsed = 0f; // Tiempo transcurrido

    void Update()
    {
        // Actualizar el tiempo transcurrido
        timeElapsed += Time.deltaTime;

        // Mover el GameObject en la dirección actual
        transform.Translate(direction * speed * Time.deltaTime);

        // Cambiar de dirección cada 60 segundos
        if (timeElapsed >= 60f)
        {
            direction = -direction; // Invertir la dirección
            timeElapsed = 0f; // Reiniciar el contador de tiempo
        }
    }
}
