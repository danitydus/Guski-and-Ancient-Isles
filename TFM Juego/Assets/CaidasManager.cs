using UnityEngine;
using System.Collections; // Para usar corrutinas

public class CaidasManager : MonoBehaviour
{
    private Vector3 lastGroundedPosition;
    private CharacterController characterController;
    public CubeMovement cubeMovement; // Referencia al script CubeMovement
    public GameObject cuerpo;
    public ParticleSystem splashEffect; // Efecto de chapoteo

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cubeMovement = GetComponent<CubeMovement>(); // Obtenemos la referencia al script CubeMovement

        // Inicializamos la posición inicial como el punto de respawn.
        lastGroundedPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si toca un trigger llamado "MiniCheck", guardamos la posición del personaje
        if (other.CompareTag("MiniCheck"))
        {
            lastGroundedPosition = transform.position;
        }

        // Si toca el trigger llamado "Muerte", lo teletransportamos al último punto válido.
        if (other.CompareTag("Muerte"))
        {
            if (splashEffect != null)
            {
                Instantiate(splashEffect, transform.position, Quaternion.identity);
            }
            StartCoroutine(HandleDeath()); // Iniciamos la corrutina para manejar la muerte
            cubeMovement.PerderVidas(); // Llamamos al método para reducir las vidas
        }
    }

    private IEnumerator HandleDeath()
    {
        // Desactivamos el CharacterController para evitar colisiones.
        characterController.enabled = false;

        // Desactivamos el cuerpo del personaje.
        if (cuerpo != null)
        {
            cuerpo.SetActive(false);
        }

        yield return new WaitForSeconds(2f); // Esperamos 2 segundos

        if (cubeMovement.lives > 0)
        {
            // Reaparecemos al último punto válido
            transform.position = lastGroundedPosition;

            // Reactivamos el cuerpo del personaje
            if (cuerpo != null)
            {
                cuerpo.SetActive(true);
            }

            // Reactivamos el CharacterController.
            characterController.enabled = true;
        }
    }
}
