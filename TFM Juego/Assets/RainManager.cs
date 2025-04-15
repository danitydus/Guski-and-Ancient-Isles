using UnityEngine;

public class RainManager : MonoBehaviour
{
    public GameObject rain; // Asigna aquí el objeto lluvia en el inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rain"))
        {
            rain.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rain"))
        {
            rain.SetActive(false);
        }
    }
}
