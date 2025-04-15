using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform target; // Referencia al personaje
    public CharacterController targetController; // Referencia al CharacterController del personaje

    [Header("Ajustes Básicos")]
    public float offsetY = 0f; // Desplazamiento vertical
    public float offsetX = 3f; // Desplazamiento horizontal
    public float followSpeed = 5f; // Velocidad de seguimiento

    [Header("Límites de Movimiento")]
    public bool useBounds = false; // Activar/desactivar límites
    public Vector2 minBounds; // Límite inferior izquierdo
    public Vector2 maxBounds; // Límite superior derecho

    [Header("Configuración de Zoom")]
    public bool allowZoom = false; // Permitir zoom dinámico
    public float defaultZoom = 5f; // Tamaño ortográfico predeterminado
    public float zoomSpeed = 2f; // Velocidad de cambio de zoom
    public float minZoom = 3f; // Zoom mínimo
    public float maxZoom = 10f; // Zoom máximo

    [Header("Bloqueo de Ejes")]
    public bool lockVertical = false; // Bloquear movimiento vertical
    public bool lockHorizontal = false; // Bloquear movimiento horizontal

    [Header("Ajuste de Profundidad")]
    public float zOffset = -10f; // Distancia equilibrada en Z

    [Header("Teletransporte Inicial")]
    public float teleportDistanceThreshold = 5f; // Distancia mínima para teletransportarse

    private Camera cam;

    // Variable para hacer la transición más fluida
    private float smoothOffsetX;

    void Start()
    {
        cam = Camera.main;
        if (cam != null && cam.orthographic)
        {
            cam.orthographicSize = defaultZoom;
        }

        if (target != null)
        {
            Vector3 targetPosition = GetTargetPosition();
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance > teleportDistanceThreshold)
            {
                transform.position = targetPosition;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z + zOffset);
            }
        }

        // Inicializar smoothOffsetX con el valor actual de offsetX
        smoothOffsetX = offsetX;
    }

    void LateUpdate()
    {
        if (target != null && targetController != null)
        {
            // Obtener la velocidad en X del CharacterController
            float horizontalSpeed = targetController.velocity.x;

            // Ajustar el valor deseado de offsetX según la dirección del personaje
            float targetOffsetX = horizontalSpeed > 0 ? 4f : (horizontalSpeed < 0 ? -4f : offsetX);

            // Suavizar la transición de offsetX
            smoothOffsetX = Mathf.Lerp(smoothOffsetX, targetOffsetX, Time.deltaTime * followSpeed);

            Vector3 targetPosition = GetTargetPosition();
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            if (allowZoom)
            {
                float targetZoom = Mathf.Clamp(defaultZoom, minZoom, maxZoom);
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
            }
        }
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetPosition = transform.position;

        if (!lockHorizontal)
        {
            targetPosition.x = target.position.x + smoothOffsetX; // Usar smoothOffsetX para el movimiento horizontal
        }
        if (!lockVertical)
        {
            targetPosition.y = target.position.y + offsetY;
        }

        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
        }

        targetPosition.z = target.position.z + zOffset;
        return targetPosition;
    }

    public void SetBounds(Vector2 newMinBounds, Vector2 newMaxBounds)
    {
        minBounds = newMinBounds;
        maxBounds = newMaxBounds;
    }
}
