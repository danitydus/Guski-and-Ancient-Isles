using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Descanso : MonoBehaviour
{
    private bool enZonaDescanso = false;
    private bool enDescanso = false;
    public CubeMovement scriptCubeMovement;
    public Camera mainCamera;
    public Transform player;
    public CharacterController characterController;
    public float zoomInValue = 40f;
    public float normalZoom = 60f;
    public Vector3 offset;
    public AudioSource musicNormal;
    public AudioSource musicDescanso;
    public GameObject panelDescanso;
    public GameObject simboloAccion;
    public List<Transform> destinosTeleport;
    public int indice;
    public GameObject panelViajeRapido;
    public GameObject panelDescanso1;
    public GameObject panelConfirmacion;
    public GameObject pantallaCarga; // Pantalla de carga
    public GameObject cuerpo; // Nuevo objeto para ocultar y mostrar
    public GameObject textDescansar;
    public GameObject Hud;
    public TriggerMapa triggerMapa;
    public GameObject imageTeleport;
    public CameraFollow2D camera;
    public GameObject infoConfirmacion;

    private Vector3 posicionInicial = new Vector3(187.76358f, 42.5670547f, 0f);
    private Vector3 posicionConfirmacion = new Vector3(0f, 10f, 0f);
    public Vector2 targetSize = new Vector2(200f, 200f); // Tamaño final deseado
    private Vector2 initialSize;
    public RectTransform imageTeleportRect; // Referencia al RectTransform del objeto
    public float duration = 2f; // Tiempo total de la animación
    public float speed = 1f; // Velocidad de crecimiento
    public CheckpointHandler checkpointHandler;
    public List<GameObject> buttons; // Lista de botones
    public GameObject infoCollectionables;
    public void UpdateButtons()
    {
      

        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] != null)
            {
                buttons[i].SetActive(checkpointHandler.checkpoints[i]);
            }
        }
    }
    private void Start()
    {
        initialSize = imageTeleportRect.sizeDelta;

        if (panelDescanso != null) panelDescanso.SetActive(false);
        if (simboloAccion != null) simboloAccion.SetActive(false);
        if (characterController == null && player != null)
        {
            characterController = player.GetComponent<CharacterController>();
        }
        textDescansar.SetActive(false);
        ResetearImageTeleport();

    }
    public void ResetearImageTeleport()
    {
        imageTeleport.transform.SetParent(panelViajeRapido.transform);
        imageTeleport.GetComponent<RectTransform>().anchoredPosition = posicionInicial;
    }  
    public void ConfirmacionImageTeleport()
    {
        imageTeleport.transform.SetParent(panelConfirmacion.transform);
        imageTeleport.GetComponent<RectTransform>().anchoredPosition = posicionConfirmacion;
    }
    private void Update()
    {
        if (enZonaDescanso && Input.GetKeyDown(KeyCode.Y))
        {
            if (!enDescanso)
            {
                ActivarDescanso();
            }
            else
            {
                DesactivarDescanso();
            }
        }
    }

    private void ActivarDescanso()
    {
        triggerMapa.ActivarTituloCorrespondiente();
        infoCollectionables.SetActive(true);
        UpdateButtons();
        imageTeleportRect.sizeDelta = initialSize;
        textDescansar.SetActive(false);
        enDescanso = true;
        scriptCubeMovement.isChangingView = true;
        panelDescanso1.SetActive(true);
        panelViajeRapido.SetActive(false);
        panelConfirmacion.SetActive(false);
        Debug.Log("Juego pausado por descanso");

        mainCamera.fieldOfView = zoomInValue;
        mainCamera.transform.position = player.position + offset;

        if (musicNormal != null) triggerMapa.currentSoundtrack.Stop();
        if (musicDescanso != null) musicDescanso.Play();

        if (panelDescanso != null) panelDescanso.SetActive(true);
        if (cuerpo != null) cuerpo.SetActive(false); // Ocultar el cuerpo
        Hud.SetActive(false);
        ResetearImageTeleport();
    }

    public void DesactivarDescanso()
    {
        triggerMapa.DesactivarTitulos();

        textDescansar.SetActive(true);
        enDescanso = false;
        scriptCubeMovement.isChangingView = false;
        Debug.Log("Juego reanudado");

        mainCamera.fieldOfView = normalZoom;
        mainCamera.transform.position = player.position + offset;

        if (musicDescanso != null) musicDescanso.Stop();
        if (musicNormal != null) triggerMapa.currentSoundtrack.Play();

        if (panelDescanso != null) panelDescanso.SetActive(false);
        if (cuerpo != null) cuerpo.SetActive(true); // Mostrar el cuerpo
        Hud.SetActive(true);

    }

    public void ActivarCamaraMapa()
    {
        if (mainCamera != null) mainCamera.gameObject.SetActive(false);
    }

    public void DesactivarCamaraMapa()
    {
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Descanso"))
        {
            enZonaDescanso = true;
            textDescansar.SetActive(true);
            if (simboloAccion != null) simboloAccion.SetActive(true);
        }
    }
    private IEnumerator ResizeImage()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            imageTeleportRect.sizeDelta = Vector2.Lerp(initialSize, targetSize, t * speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        imageTeleportRect.sizeDelta = targetSize;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Descanso"))
        {
            enZonaDescanso = false;
            textDescansar.SetActive(false);

            if (simboloAccion != null) simboloAccion.SetActive(false);
        }
    }

    public void SetIndice(int indx)
    {
        indice = indx;
        panelConfirmacion.SetActive(true);
    }

    public void Teleport()
    {
        infoConfirmacion.SetActive(false);

        if (indice >= 0 && indice < destinosTeleport.Count)
        {
            StartCoroutine(ResizeImage());
            StartCoroutine(LoadingTeleport());
        }
        else
        {
            Debug.LogWarning("Índice de teleportación fuera de rango.");
        }
    }

    private IEnumerator LoadingTeleport()
    {
        if (cuerpo != null) cuerpo.SetActive(true); // Mostrar el cuerpo

        if (characterController != null)
        {
            player.position = destinosTeleport[indice].position;
        }
        if (characterController != null) characterController.enabled = false;

        yield return new WaitForSeconds(1.5f);
        panelConfirmacion.SetActive(false);
        infoConfirmacion.SetActive(true);
        DesactivarDescanso();

        if (characterController != null) characterController.enabled = true;

        if (pantallaCarga != null) pantallaCarga.SetActive(false);

        Debug.Log("Teletransportado a " + destinosTeleport[indice].position);
        textDescansar.SetActive(false);

    }

}
