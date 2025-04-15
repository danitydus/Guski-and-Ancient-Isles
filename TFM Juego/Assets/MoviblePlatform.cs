using UnityEngine;

public class MoviblePlatform : MonoBehaviour
{
    public Transform pointA; // Punto A
    public Transform pointB; // Punto B
    public float speed = 2f; // Velocidad de movimiento de la plataforma
    public bool CanMove = false; // Comienza como falso

    private Transform player; // Referencia al jugador
    private CharacterController playerController; // Referencia al CharacterController del jugador
    public GameObject Limits;
    void Start()
    {
        // Inicializamos la plataforma en el punto A
        transform.position = new Vector3(pointA.position.x, transform.position.y, pointA.position.z);
    }

    void Update()
    {
        if (CanMove)
        {
            MovePlatform();
        }

        // Si el jugador está en la plataforma, lo movemos junto con la plataforma
        if (player != null && playerController != null)
        {
            // Calculamos el desplazamiento de la plataforma en X y Z
            Vector3 platformMovement = transform.position - player.position;

            // Movemos al jugador solo en los ejes X y Z (sin afectar Y)
            playerController.Move(new Vector3(platformMovement.x, 0f, platformMovement.z));
        }
    }

    void MovePlatform()
    {
        // Usamos Mathf.PingPong para mover la plataforma entre A y B suavemente
        float pingPongValue = Mathf.PingPong(Time.time * speed, 1f);

        // Interpolamos la posición entre A y B en función de pingPongValue
        transform.position = Vector3.Lerp(pointA.position, pointB.position, pingPongValue);
    }

    // Detecta cuando el jugador entra en el trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Limits.SetActive(false);
            // Activar movimiento de la plataforma si no está activado
            if (!CanMove)
            {
                CanMove = true;
            }

            // Guardamos la referencia del jugador y su CharacterController
            player = other.transform;
            playerController = player.GetComponent<CharacterController>(); // Obtenemos el CharacterController del jugador
        }
    }

    // Detecta cuando el jugador sale del trigger
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Limits.SetActive(true);

            // Restauramos el CharacterController del jugador
            playerController = null;
            player = null;
        }
    }
}
