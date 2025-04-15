using UnityEngine;

public class GemDeposit : MonoBehaviour
{
    public int monedas = 10; // Valor de monedas que se suma al destruir el cofre

    // Este método se ejecuta cuando otro objeto entra en el trigger
    public CubeMovement cubeMovement; // Referencia al script CubeMovement

    private void OnTriggerStay(Collider other)
    {
        // Verifica si el objeto que entra en el trigger tiene el tag "Espada"
        if (other.CompareTag("Espada") && cubeMovement.isAttacking == true)
        {
            if (cubeMovement != null)
            {
                cubeMovement.sumarMoneda(monedas); // Llama al método sumarMonedas
            }
            // Destruye este objeto (el yacimiento de gemas)
            Destroy(gameObject);
        }
    }
}
