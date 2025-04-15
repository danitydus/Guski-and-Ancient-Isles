using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public float gravityScale = 1.0f; // Escala de gravedad personalizada
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Desactivar la gravedad predeterminada
    }

    void FixedUpdate()
    {
        // Aplicar una gravedad personalizada en el eje Y
        Vector3 customGravity = new Vector3(0, -9.81f * gravityScale, 0);
        rb.AddForce(customGravity, ForceMode.Acceleration);
    }
}
