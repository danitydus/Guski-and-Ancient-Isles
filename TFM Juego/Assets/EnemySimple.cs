using UnityEngine;

public class EnemySimple : MonoBehaviour
{
    public Transform pointA; // Punto A al que el enemigo irá
    public Transform pointB; // Punto B al que el enemigo irá
    public float moveSpeed = 3f; // Velocidad de movimiento
    public float chaseDistance = 10f; // Distancia a la que empieza a perseguir al jugador
    public float attackDistance = 2f; // Distancia a la que ataca al jugador
    public Animator animator; // Referencia al Animator
    public Transform player; // Referencia al jugador

    public BoxCollider bodyCollider; // Referencia pública al BoxCollider del enemigo (lo arrastras desde el Inspector)
    public float heightOffset = -0.5f; // Diferencia de altura en la posición Y, modificable en el Inspector
    public float pointReachThreshold = 0.5f; // Umbral de distancia para cambiar al siguiente punto, modificable en el Inspector

    private Transform target; // El objetivo actual al que el enemigo se dirige
    private bool isChasing = false; // Indica si está persiguiendo al jugador
    private bool isBlocked = false; // Indica si el enemigo está bloqueado por un trigger (suelo, etc.)
    private float initialY; // Guardamos la posición inicial en Y
    public CubeMovement cubeMovement; // Referencia al script CubeMovement

    void Start()
    {
        target = pointA; // Inicializamos el objetivo con el punto A

        // Guardamos la posición inicial Y
        initialY = transform.position.y;

        // Ajustar la posición inicial del enemigo con el offset en Y
        Vector3 spawnPosition = transform.position;
        spawnPosition.y = initialY + heightOffset; // Aplica la diferencia de altura
        transform.position = spawnPosition; // Establece la nueva posición

        // Si el BoxCollider no se ha asignado, lo desactivamos inicialmente
        if (bodyCollider != null)
        {
            bodyCollider.enabled = false; // Inicialmente desactivamos el collider
        }
    }

    void Update()
    {
        // Si está bloqueado, no se mueve
        if (isBlocked)
            return;

        // Comprobar la distancia entre el enemigo y el jugador
        float distanceToPlayer = Vector3.Distance(new Vector3(player.position.x, 0, player.position.z),
                                                  new Vector3(transform.position.x, 0, transform.position.z));

        if (distanceToPlayer <= attackDistance)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseDistance && IsPlayerWithinPatrolArea())
        {
            // El jugador está dentro de la zona de patrullaje
            ChasePlayer();
        }
        else
        {
            // Si el jugador no está dentro del área de patrullaje, se mueve por el trayecto
            Patrol();
        }
    }

    void Patrol()
    {
        isChasing = false;

        // Activar trigger de movimiento en el Animator
        animator.SetTrigger("Move");

        // Mover hacia el objetivo actual sin rotar
        Vector3 direction = (target.position - transform.position).normalized;

        // Rotar el enemigo hacia el objetivo
        RotateTowards(target.position);

        // Mover el enemigo, manteniendo la posición Y original
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        newPosition.y = initialY + heightOffset; // Mantener la misma Y
        transform.position = newPosition;

        // Si ha llegado al punto de patrullaje (dentro del umbral), cambiar al siguiente punto
        if (Vector3.Distance(transform.position, target.position) <= pointReachThreshold)
        {
            target = (target == pointA) ? pointB : pointA; // Cambiar al siguiente punto
        }
    }

    void ChasePlayer()
    {
        isChasing = true;

        // Activar trigger de movimiento en el Animator
        animator.SetTrigger("Move");

        // Mover hacia el jugador sin rotar
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z); // Ignorar Y
        RotateTowards(targetPosition); // Rotar hacia el jugador
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        newPosition.y = initialY + heightOffset; // Mantener la misma Y
        transform.position = newPosition;
    }

    void AttackPlayer()
    {
        if (isChasing)
        {
            // Activar trigger de ataque en el Animator
            animator.SetTrigger("Attack");

            // Activar el collider solo mientras ataca
            if (bodyCollider != null)
            {
                bodyCollider.enabled = true; // Activar el collider al atacar
            }

            // Lógica de ataque (puedes agregar más aquí si es necesario)
        }
    }

    void StopChasing()
    {
        isChasing = false;

        // Reasignar objetivo al punto más cercano (A o B)
        target = Vector3.Distance(transform.position, pointA.position) < Vector3.Distance(transform.position, pointB.position)
                 ? pointA
                 : pointB;
    }

    // Verificar si el jugador está dentro de los límites del trayecto
    bool IsPlayerWithinPatrolArea()
    {
        // Comprobar si el jugador está dentro del área delimitada por pointA y pointB
        float distanceToPointA = Vector3.Distance(player.position, pointA.position);
        float distanceToPointB = Vector3.Distance(player.position, pointB.position);

        return distanceToPointA <= Vector3.Distance(pointA.position, pointB.position) &&
               distanceToPointB <= Vector3.Distance(pointA.position, pointB.position);
    }

    // Rotar al enemigo hacia el objetivo de forma suave
    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized; // Dirección hacia el objetivo
        Quaternion lookRotation = Quaternion.LookRotation(direction); // Crear rotación hacia el objetivo
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed); // Rotación suave
    }

    // Aquí se detiene al enemigo si entra en un trigger específico (como un "suelo" o zona bloqueada)
    void OnTriggerEnter(Collider other)
    {
        // Si el trigger es el "suelo" o área que detiene al enemigo
        if (other.CompareTag("Suelo"))
        {
            isBlocked = true; // Bloqueamos el movimiento del enemigo
            animator.SetTrigger("Idle"); // Cambiar animación si es necesario
        }
    }

    // Si el enemigo sale del área bloqueada (por ejemplo, un "suelo")
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Suelo"))
        {
            isBlocked = false; // Permitir que el enemigo vuelva a moverse
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Verifica si el objeto que entra en el trigger tiene el tag "Espada"
        if (other.CompareTag("Espada") && cubeMovement.isAttacking == true)
        {
           
            // Destruye este objeto (el yacimiento de gemas)
            Destroy(gameObject, 1f);
        }
    }
    // Desactivar el BoxCollider cuando el ataque termine
    void EndAttack()
    {
        if (bodyCollider != null)
        {
            bodyCollider.enabled = false; // Desactivar el collider al finalizar el ataque
        }
    }
}
