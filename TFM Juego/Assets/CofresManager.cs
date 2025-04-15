using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CofresManager : MonoBehaviour
{
    public GameObject Exclamacion; // Referencia al GameObject "Exclamacion"
    public CubeMovement cubeMovement; // Referencia al script CubeMovement

    public int monedas = 10; // Valor de monedas que se suma al destruir el cofre

    private bool playerEnTrigger = false; // Verifica si el jugador está dentro del trigger

    // Start is called before the first frame update
    void Start()
    {
        if (Exclamacion != null)
        {
            Exclamacion.SetActive(false); // Asegúrate de que el objeto esté desactivado al inicio
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Comprueba si el jugador está en el trigger y presiona la tecla "T"
        if (playerEnTrigger && Input.GetKeyDown(KeyCode.T))
        {
            Destroy(gameObject); // Destruye el cofre
            Exclamacion.SetActive(false); // Desactiva el GameObject "Exclamacion"

            if (cubeMovement != null)
            {
                cubeMovement.sumarMoneda(monedas); // Llama al método sumarMonedas
            }
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
