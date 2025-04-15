using UnityEngine;

public class CameraFollowZ : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform target;

    [Header("Ajustes Básicos")]
    public float offsetX = 0f;
    public float offsetZ = 0f;
    public float offsetY = 0f;
    public float followSpeed = 5f;
    public float teleportThreshold = 10f;

    [Header("Límites de Movimiento")]
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    [Header("Configuración de Zoom")]
    public bool allowZoom = false;
    public float defaultZoom = 5f;
    public float zoomSpeed = 2f;
    public float minZoom = 3f;
    public float maxZoom = 10f;

    [Header("Bloqueo de Ejes")]
    public bool lockVertical = false;
    public bool lockDepth = false;

    [Header("Control Externo")]
    public bool zoomBloqueadoExternamente = false;

    private Camera cam;
    private float lastZPosition;
    private float targetZoom;

    void Start()
    {
        cam = Camera.main;
        if (cam != null && cam.orthographic)
        {
            targetZoom = defaultZoom;
            cam.orthographicSize = targetZoom;
        }

        if (target != null)
        {
            lastZPosition = target.position.z;
            Vector3 targetPosition = new Vector3(
                target.position.x + offsetX,
                target.position.y + offsetY,
                target.position.z + offsetZ
            );
            if (Vector3.Distance(transform.position, targetPosition) > teleportThreshold)
            {
                transform.position = targetPosition;
            }
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            float currentZPosition = target.position.z;
            if (currentZPosition > lastZPosition)
                offsetZ = 3.5f;
            else if (currentZPosition < lastZPosition)
                offsetZ = -3.5f;
            lastZPosition = currentZPosition;

            Vector3 targetPosition = new Vector3(
                target.position.x + offsetX,
                target.position.y + offsetY,
                target.position.z + offsetZ
            );

            if (Vector3.Distance(transform.position, targetPosition) > teleportThreshold)
            {
                transform.position = targetPosition;
            }
            else
            {
                if (!lockVertical)
                    targetPosition.y = target.position.y + offsetY;
                if (!lockDepth)
                    targetPosition.z = target.position.z + offsetZ;

                if (useBounds)
                {
                    targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
                    targetPosition.z = Mathf.Clamp(targetPosition.z, minBounds.x, maxBounds.x);
                }

                transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            }

            if (allowZoom)
            {
                if (!zoomBloqueadoExternamente)
                {
                    targetZoom = Mathf.Clamp(defaultZoom, minZoom, maxZoom);
                }

                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
            }
        }
    }

    public void SetZoom(float newZoom)
    {
        zoomBloqueadoExternamente = true;
        targetZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
    }

    public void LiberarZoom()
    {
        zoomBloqueadoExternamente = false;
    }
}
