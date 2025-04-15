using System.Collections;
using UnityEngine;

public class CameraTravel : MonoBehaviour
{
    public Transform[] waypoints; // Puntos por los que viajará la cámara
    public float moveSpeed = 3f;  // Velocidad de movimiento
    public float rotateSpeed = 0.5f; // Velocidad de rotación más suave

    private int currentWaypointIndex = 0;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
            StartCoroutine(TravelRoutine());
        }
    }

    IEnumerator TravelRoutine()
    {
        while (true)
        {
            Transform target = waypoints[currentWaypointIndex];

            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                // Movimiento constante hacia el siguiente waypoint
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

                // Rotación mucho más suave y fluida hacia el siguiente waypoint
                Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f - Mathf.Exp(-rotateSpeed * Time.deltaTime));

                yield return null;
            }

            // Pasar al siguiente waypoint sin detenerse
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
