using UnityEngine;

public class CambiarColorHijos : MonoBehaviour
{
    public Color colorNuevo = Color.red;

    public void CambiarColor()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mr in renderers)
        {
            if (mr.material.HasProperty("_Color"))
            {
                mr.material.color = colorNuevo;
            }
        }
    }
}
