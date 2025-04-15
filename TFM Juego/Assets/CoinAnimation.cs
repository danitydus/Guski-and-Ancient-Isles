using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
    public float rotationSpeed = 50f; // Velocidad de rotaci�n en grados por segundo
    public float scaleAmplitude = 0.05f; // Amplitud del cambio de escala
    public float scaleFrequency = 1f; // Frecuencia del cambio de escala

    public GameObject effectPrefab; // Prefab del efecto a instanciar
    public float effectDuration = 0.5f; // Tiempo que durar� el efecto antes de desaparecer

    public float moveSpeed = 10f; // Velocidad de movimiento hacia la esquina
    public float moveLerpSpeed = 5f; // Velocidad de interpolaci�n hacia el objetivo

    public float shrinkSpeed = 1f; // Velocidad de reducci�n de tama�o
    public float shrinkLerpSpeed = 0.5f; // Velocidad a la que se hace peque�a la moneda
    public float minScaleFactor = 0.7f; // Tama�o m�nimo al que puede reducirse la moneda

    private Vector3 initialScale;
    public bool collected = false;
    public Transform targetPosition; // Se asigna manualmente desde Unity
    private Vector3 initialPosition; // Cambiado a Vector3 para almacenar posici�n directamente

    void Start()
    {
        // Guardar la escala inicial de la moneda
        // Guardar la posici�n inicial de la moneda en la escena
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!collected)
        {
            // Rotaci�n continua de la moneda
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Escalado sutil como animaci�n de "respiraci�n"
        }
        else
        {
            // Movimiento hacia la posici�n objetivo con velocidad controlada
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, moveLerpSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            // Reducci�n progresiva de la escala con velocidad controlada

            // Si la moneda ha llegado a la posici�n objetivo, se destruye
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

        // Instanciar el efecto en la posici�n del GameObject
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, effectDuration);
    }
    public void RespawnCoin()
    {
        // Restablecer la posici�n de la moneda a la inicial
        transform.position = initialPosition;
        // Restablecer el estado de la moneda (no recogida)
        collected = false;
        // Reactivar el objeto si estaba desactivado
        gameObject.SetActive(true);
    }
}
