using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler
{
    public RawImage imagenObjetivo; // RawImage en el que se cambiará la textura
    public Texture[] texturas; // Lista de texturas disponibles

    public int index;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("El mouse está sobre el botón");
        ActivarMetodo(); // Aquí puedes cambiar el índice según necesites
    }

    public void ActivarMetodo()
    {
        if (texturas.Length > 0 && index >= 0 && index < texturas.Length)
        {
            imagenObjetivo.texture = texturas[index];
            Debug.Log($"Textura cambiada a la de índice {index}");
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango o lista vacía.");
        }
    }
}
