using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler
{
    public RawImage imagenObjetivo; // RawImage en el que se cambiar� la textura
    public Texture[] texturas; // Lista de texturas disponibles

    public int index;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("El mouse est� sobre el bot�n");
        ActivarMetodo(); // Aqu� puedes cambiar el �ndice seg�n necesites
    }

    public void ActivarMetodo()
    {
        if (texturas.Length > 0 && index >= 0 && index < texturas.Length)
        {
            imagenObjetivo.texture = texturas[index];
            Debug.Log($"Textura cambiada a la de �ndice {index}");
        }
        else
        {
            Debug.LogWarning("�ndice fuera de rango o lista vac�a.");
        }
    }
}
