using System.Collections;
using UnityEngine;

public class CloudBehavior : MonoBehaviour
{
    public float shrinkDuration = 1.0f; // Duraci�n del efecto de desaparici�n
    public float shrinkAmount = 0.5f;  // Cantidad que la nube se hunde hacia abajo antes de desaparecer

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isDisappearing = false; // Controla si el efecto ya est� en ejecuci�n

    void Start()
    {
        // Guardar el tama�o y posici�n inicial de la nube
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Verificar si el objeto que toca la nube tiene el tag "Player" y si el efecto no ha comenzado
        if (hit.gameObject.CompareTag("Player") && !isDisappearing)
        {
            isDisappearing = true; // Marcar que el efecto ha comenzado
            StartCoroutine(DisappearEffect());
        }
    }

    IEnumerator DisappearEffect()
    {
        Vector3 targetScale = Vector3.zero; // La nube se hace completamente peque�a
        Vector3 targetPosition = originalPosition - new Vector3(0, shrinkAmount, 0); // La nube baja ligeramente

        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            // Interpolar el tama�o y la posici�n durante el tiempo
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / shrinkDuration);
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / shrinkDuration);

            elapsedTime += Time.deltaTime;
            yield return null; // Esperar hasta el siguiente frame
        }

        // Asegurarse de que los valores finales sean los esperados
        transform.localScale = targetScale;
        transform.position = targetPosition;

        // Opcional: Destruir el objeto o desactivarlo
        Destroy(gameObject);
    }
}
