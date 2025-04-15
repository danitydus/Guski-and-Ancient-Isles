using UnityEngine;

public class IsleCam : MonoBehaviour
{
    public GameObject cam1; // C�mara que se activar�
    public GameObject cam2; // C�mara que se desactivar�

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IsleCam")) // Comprueba si el objeto que entra tiene la etiqueta "IsleCam"
        {
            if (cam1 != null && cam2 != null)
            {
                cam1.SetActive(true);
                cam2.SetActive(false);
            }
        }
    }
}
