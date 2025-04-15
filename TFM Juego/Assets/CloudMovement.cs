using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public bool isRotatingCloud = false; // Define si la nube rota o no
    private float moveSpeed;
    private float verticalSpeed;
    private float verticalAmplitude;
    private float rotationSpeed;

    private Vector3 startPosition;
    private float randomOffset; // Para desfasar el movimiento de cada nube

    void Start()
    {
        startPosition = transform.position;

        // Generar valores aleatorios en un rango más bajo para movimientos suaves
        moveSpeed = Random.Range(0.2f, 0.5f);
        verticalSpeed = Random.Range(0.2f, 0.5f);
        verticalAmplitude = Random.Range(0.05f, 0.15f);
        rotationSpeed = Random.Range(5f, 15f);

        // Generar un offset aleatorio para que no todas las nubes se muevan sincronizadas
        randomOffset = Random.Range(0f, Mathf.PI * 2);
    }

    void Update()
    {
        // Movimiento suave de izquierda a derecha y de arriba a abajo
        float offsetX = Mathf.Sin(Time.time * moveSpeed + randomOffset) * 0.5f;
        float offsetY = Mathf.Sin(Time.time * verticalSpeed + randomOffset) * verticalAmplitude;

        transform.position = startPosition + new Vector3(offsetX, offsetY, 0);

        // Si es una nube rotativa, rotar sobre el eje Y
        if (isRotatingCloud)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}
