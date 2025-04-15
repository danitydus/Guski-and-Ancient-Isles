using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
    public float rotationSpeed = 50f; // Velocidad de rotación en grados por segundo
    public float scaleAmplitude = 0.05f; // Amplitud del cambio de escala
    public float scaleFrequency = 1f; // Frecuencia del cambio de escala

    public GameObject effectPrefab; // Prefab del efecto a instanciar
    public float effectDuration = 0.5f; // Tiempo que durará el efecto antes de desaparecer

    public float moveSpeed = 10f; // Velocidad de movimiento hacia la esquina
    public float moveLerpSpeed = 5f; // Velocidad de interpolación hacia el objetivo

    public float shrinkSpeed = 1f; // Velocidad de reducción de tamaño
    public float shrinkLerpSpeed = 0.5f; // Velocidad a la que se hace pequeña la moneda
    public float minScaleFactor = 0.7f; // Tamaño mínimo al que puede reducirse la moneda

    private Vector3 initialScale;
    public bool collected = false;
    public Transform targetPosition; // Se asigna manualmente desde Unity
    private Vector3 initialPosition; // Cambiado a Vector3 para almacenar posición directamente

    void Start()
    {
        // Guardar la escala inicial de la moneda
        // Guardar la posición inicial de la moneda en la escena
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!collected)
        {
            // Rotación continua de la moneda
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Escalado sutil como animación de "respiración"
        }
        else
        {
            // Movimiento hacia la posición objetivo con velocidad controlada
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, moveLerpSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            // Reducción progresiva de la escala con velocidad controlada

            // Si la moneda ha llegado a la posición objetivo, se destruye
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            IniciarMoneda();
        }
    }
    public void IniciarMoneda()
        {
        collected = true;

        // Instanciar el efecto en la posición del GameObject
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, effectDuration);
    }
    public void RespawnCoin()
    {
        // Restablecer la posición de la moneda a la inicial
        transform.position = initialPosition;
        // Restablecer el estado de la moneda (no recogida)
        collected = false;
        // Reactivar el objeto si estaba desactivado
        gameObject.SetActive(true);
    }
}
