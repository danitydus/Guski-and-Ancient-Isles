using UnityEngine;

public class FloatingBarrel : MonoBehaviour
{
    public float floatStrength = 0.1f;  // Fuerza del movimiento vertical
    public float sideStrength = 0.05f;  // Fuerza del movimiento lateral
    public float speed = 2f;            // Velocidad del movimiento

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * speed) * floatStrength;
        float xOffset = Mathf.Sin(Time.time * speed * 0.5f) * sideStrength;

        transform.position = startPosition + new Vector3(xOffset, yOffset, 0);
    }
}
