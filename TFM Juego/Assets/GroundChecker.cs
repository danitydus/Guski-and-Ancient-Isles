using UnityEngine;

public class CharacterGroundCheck : MonoBehaviour
{
    public float groundCheckDistance = 0.3f; // Aumentamos la distancia del raycast para detectar suelo antes
    public LayerMask groundLayer; // Capa del suelo
    public CubeMovement cubeMovement;
    public bool isGrounded;

    void Start()
    {
    }

    void Update()
    {
        // Raycast desde el centro del personaje hacia abajo, con una distancia ajustable
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        cubeMovement.isGrounded = isGrounded;

        Debug.Log("¿Está en el suelo?: " + isGrounded);
    }
}
