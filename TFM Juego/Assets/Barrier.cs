using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameObject Exclamacion; // Referencia al GameObject "Exclamacion"
    public CubeMovement cubeMovement; // Referencia al script CubeMovement

    private bool playerEnTrigger = false; // Verifica si el jugador est� dentro del trigger
    public int numeroDeGemas;
    // Start is called before the first frame update
    void Start()
    {
        if (Exclamacion != null)
        {
            Exclamacion.SetActive(false); // Aseg�rate de que el objeto est� desactivado al inicio
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Comprueba si el jugador est� en el trigger y presiona la tecla "T"
        if (playerEnTrigger && Input.GetKeyDown(KeyCode.T) && cubeMovement.gemCount >= numeroDeGemas)
        {
            Destroy(gameObject); // Destruye la barrera
            Exclamacion.SetActive(false); // Desactiva el GameObject "Exclamacion"

          
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que entra al trigger es el jugador
        if (other.CompareTag("Player"))
        {
            playerEnTrigger = true;
            if (Exclamacion != null)
            {
                Exclamacion.SetActive(true); // Activa el GameObject "Exclamacion"
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Comprueba si el objeto que sale del trigger es el jugador
        if (other.CompareTag("Player"))
        {
            playerEnTrigger = false;
            if (Exclamacion != null)
            {
                Exclamacion.SetActive(false); // Desactiva el GameObject "Exclamacion"
            }
        }
    }
}
