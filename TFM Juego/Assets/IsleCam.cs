using UnityEngine;

public class IsleCam : MonoBehaviour
{
    public GameObject cam1; // Cámara que se activará
    public GameObject cam2; // Cámara que se desactivará

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
