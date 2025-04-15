using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public bool isSimple;   // Movimiento horizontal
    public bool isVertical; // Movimiento vertical

    public float speed = 3f; // Velocidad de movimiento
    public float moveDuration = 2f; // Tiempo antes de cambiar de dirección
    private bool movingPositive = true; // Indica si se mueve en dirección positiva (derecha o arriba)
    private float moveTimer; // Temporizador para cambiar de dirección

    void Start()
    {
        moveTimer = moveDuration; // Inicializa el temporizador
    }

    void Update()
    {
        if (isSimple)
        {
            MoveHorizontally(); // Movimiento en X
        }
        else if (isVertical)
        {
            MoveVertically(); // Movimiento en Z
        }
    }

    void MoveHorizontally()
    {
        float movement = (movingPositive ? 1 : -1) * speed * Time.deltaTime;
        transform.position += new Vector3(movement, 0, 0); // Modifica la X

        UpdateDirection();
    }

    void MoveVertically()
    {
        float movement = (movingPositive ? 1 : -1) * speed * Time.deltaTime;
        transform.position += new Vector3(0, 0, movement); // Modifica la Z

        UpdateDirection();
    }

    void UpdateDirection()
    {
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            movingPositive = !movingPositive; // Cambia dirección
            moveTimer = moveDuration;
            Flip();
        }
    }

    void Flip()
    {
        // Solo voltea en el eje X si es horizontal
        if (isSimple)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1; // Invierte el sprite en X
            transform.localScale = newScale;
        }
    }
}
