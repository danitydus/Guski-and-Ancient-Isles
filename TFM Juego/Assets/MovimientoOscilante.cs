using UnityEngine;

public class MovimientoOscilante : MonoBehaviour
{
    public float velocidad = 2f; // Velocidad de movimiento
    private Vector3 puntoInicial;
    private bool moviendoDerecha = true;
    private float distanciaRecorrida = 0f;

    void Start()
    {
        // Guardar la posición inicial del objeto
        puntoInicial = transform.position;
    }

    void Update()
    {
        // Calcular el desplazamiento basado en el tiempo
        float movimiento = velocidad * Time.deltaTime;

        if (moviendoDerecha)
        {
            // Mover a la derecha
            if (distanciaRecorrida < 4f)
            {
                transform.position += new Vector3(movimiento, 0, 0);
                distanciaRecorrida += movimiento;
            }
            else
            {
                moviendoDerecha = false;
                distanciaRecorrida = 0f; // Reiniciar la distancia cuando cambiamos de dirección
            }
        }
        else
        {
            // Mover a la izquierda
            if (distanciaRecorrida < 4f)
            {
                transform.position -= new Vector3(movimiento, 0, 0);
                distanciaRecorrida += movimiento;
            }
            else
            {
                moviendoDerecha = true;
                distanciaRecorrida = 0f; // Reiniciar la distancia cuando cambiamos de dirección
            }
        }
    }
}
