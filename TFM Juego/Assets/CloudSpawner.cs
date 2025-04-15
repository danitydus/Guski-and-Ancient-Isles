using System.Collections;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab;  // Prefab de la nube
    public int cloudCount = 100;    // N�mero de nubes a generar
    public float distanceBetweenClouds = 20f;  // Distancia entre las nubes

    private void Start()
    {
        SpawnClouds();
    }

    void SpawnClouds()
    {
        Vector3 startingPosition = transform.position;  // Posici�n inicial donde empezar� a generar las nubes
        float initialX = startingPosition.x;  // Usar la X de la posici�n inicial
        float initialZ = startingPosition.z;  // Usar la Z de la posici�n inicial

        // Generar las nubes en un patr�n controlado
        for (int i = 0; i < cloudCount; i++)
        {
            // Calcular la posici�n de cada nube con desplazamiento en X y Z
            float xPosition = initialX + (i % 10) * distanceBetweenClouds;  // Espaciado en X (10 columnas)
            float zPosition = initialZ + (i / 10) * distanceBetweenClouds;  // Espaciado en Z (filas de 10)

            // Calcular la altura aleatoria entre 60 y 90
            float yPosition = Random.Range(60f, 90f);

            // Crear la posici�n de la nube
            Vector3 cloudPosition = new Vector3(xPosition, yPosition, zPosition);

            // Instanciar la nube en la posici�n calculada
            Instantiate(cloudPrefab, cloudPosition, Quaternion.identity);
        }
    }
}
