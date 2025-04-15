using System.Collections;
using UnityEngine;

public class Gema : MonoBehaviour
{
    public AudioSource musicaActual;
    public AudioSource musicaGema;
    public AudioSource musicaTodasLasGemas;
    public Camera camara;
    public Camera camara2;
    public GameObject textGema;
    public Vector3 offsetCamara = new Vector3(0, 1.5f, -2f);
    public Vector3 offsetCamara2 = new Vector3(2f, 1.5f, 0f); // Nuevo offset específico para camara2
    private CharacterController playerController;
    private Transform playerTransform;
    private bool controlDesactivado = false;
    private CameraFollow2D camaraScript;
    public int level;
    public char gemType;
    public GemManager gemManager;
    public ParticleSystem specialEffect;
    private bool esperaDesactivacion = false;
    public TriggerMapa triggerMapa;
    public int index;

    private Vector3 camaraPosOriginal;
    private Camera camaraActiva;

    private void Start()
    {
        textGema.SetActive(false);
        camaraScript = camara.GetComponent<CameraFollow2D>();

        if (specialEffect != null)
        {
            ParticleSystem effectInstance = Instantiate(specialEffect, transform.position, Quaternion.identity, transform);
            var main = effectInstance.main;
            main.startSize = new ParticleSystem.MinMaxCurve(main.startSize.constant * 0.005f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<CharacterController>();
            playerTransform = other.transform;

            if (playerController != null)
            {
                esperaDesactivacion = true;
            }

            GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(EfectoGema());
            gemManager.CollectGem(level, gemType);
        }
    }

    private void Update()
    {
        if (esperaDesactivacion && playerController != null && playerController.isGrounded)
        {
            playerController.enabled = false;
            controlDesactivado = true;
            esperaDesactivacion = false;
        }

        if (controlDesactivado)
        {
            playerTransform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private IEnumerator EfectoGema()
    {
        gemManager.IsGemCollected(level, gemType);

        if (camaraScript != null)
        {
            camaraScript.enabled = false;
        }

        musicaActual.Stop();
        musicaGema.Play();

        camaraActiva = GetCamaraActiva();
        camaraPosOriginal = camaraActiva.transform.position;

        Vector3 destinoZoom;

        if (camaraActiva == camara2)
        {
            destinoZoom = playerTransform.position + offsetCamara2;
        }
        else
        {
            destinoZoom = playerTransform.position + offsetCamara + new Vector3(2f, 0, 0);
        }

        yield return StartCoroutine(ZoomCamara(destinoZoom, 1f));

        triggerMapa.tituloMapa1.SetActive(false);
        triggerMapa.tituloMapa2.SetActive(false);
        triggerMapa.tituloMapa3.SetActive(false);
        triggerMapa.tituloMapa4.SetActive(false);
        triggerMapa.tituloMapa5.SetActive(false);

        textGema.SetActive(true);
        yield return new WaitForSeconds(5f);
        textGema.SetActive(false);

        musicaActual.Play();
        musicaGema.Stop();

        yield return StartCoroutine(ZoomCamara(camaraPosOriginal, 1f));

        if (playerController != null)
        {
            playerController.enabled = true;
            controlDesactivado = false;
        }

        if (camaraScript != null)
        {
            camaraScript.enabled = true;
        }

        triggerMapa.ActivarMostrarTitulos();
        gemManager.AnimateGem(index);

        if ((triggerMapa.inMap1 && gemManager.AreAllGemsCollected(gemManager.gems1)) ||
            (triggerMapa.inMap2 && gemManager.AreAllGemsCollected(gemManager.gems2)) ||
            (triggerMapa.inMap3 && gemManager.AreAllGemsCollected(gemManager.gems3)) ||
            (triggerMapa.inMap4 && gemManager.AreAllGemsCollected(gemManager.gems4)) ||
            (triggerMapa.inMap5 && gemManager.AreAllGemsCollected(gemManager.gems5)))
        {
            gemManager.AnimateAllGems();
            StartCoroutine(SongPlay());
        }

        gameObject.SetActive(false);
    }

    private IEnumerator ZoomCamara(Vector3 destino, float duracion)
    {
        float tiempo = 0f;
        Vector3 inicio = camaraActiva.transform.position;

        while (tiempo < duracion)
        {
            camaraActiva.transform.position = Vector3.Lerp(inicio, destino, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        camaraActiva.transform.position = destino;
    }

    private Camera GetCamaraActiva()
    {
        if (camara.gameObject.activeInHierarchy)
            return camara;
        else if (camara2.gameObject.activeInHierarchy)
            return camara2;
        else
            return camara; // Fallback
    }

    private IEnumerator SongPlay()
    {
        musicaActual.Stop();
        musicaTodasLasGemas.Play();
        yield return new WaitForSeconds(5f);
        musicaTodasLasGemas.Stop();
        musicaActual.Play();
    }
}
