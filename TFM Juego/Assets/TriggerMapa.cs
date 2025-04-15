using System.Collections;
using UnityEngine;

public class TriggerMapa : MonoBehaviour
{
    public GameObject tituloMapa1;
    public GameObject tituloMapa2;
    public GameObject tituloMapa3;
    public GameObject tituloMapa4;
    public GameObject tituloMapa5;

    public AudioSource soundtrackTutorial;
    public AudioSource soundtrack1;
    public AudioSource soundtrack2;
    public AudioSource soundtrack3;
    public AudioSource soundtrack4;
    public AudioSource soundtrack5;

    public AudioSource currentSoundtrack;
    public GameObject hud;

    private string ultimoTrigger = "";
    public bool inMap1;
    public bool inMap2;
    public bool inMap3;
    public bool inMap4;
    public bool inMap5;
    private void Start()
    {
        hud.SetActive(false);
        EsconderTodosLosTitulos();
        IniciarMusica();
    }

    private void EsconderTodosLosTitulos()
    {
        OcultarObjeto(tituloMapa1);
        OcultarObjeto(tituloMapa2);
        OcultarObjeto(tituloMapa3);
        OcultarObjeto(tituloMapa4);
        OcultarObjeto(tituloMapa5);
    }

    private void OcultarObjeto(GameObject obj)
    {
        if (obj != null)
        {
            CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>() ?? obj.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            obj.SetActive(false);
        }
    }

    private void IniciarMusica()
    {
        currentSoundtrack = soundtrackTutorial;
        if (currentSoundtrack != null)
        {
            currentSoundtrack.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.StartsWith("Map"))
        {
            if (ultimoTrigger == other.gameObject.tag) return; // Evita activar el mismo trigger repetidamente

            // Desactiva todos los títulos actuales
            DesactivarTitulos();

            // Guarda el nuevo trigger activo
            ultimoTrigger = other.gameObject.tag;

            // Activa el HUD
            hud.SetActive(true);

            // Activa el nuevo título
            StartCoroutine(MostrarTitulos(GetTituloMapaCorrespondiente(ultimoTrigger)));

            // Cambia la música
            CambiarMusica(GetSoundtrackCorrespondiente(ultimoTrigger));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ultimoTrigger)
        {
            ultimoTrigger = "";
        }
    }

    private GameObject GetTituloMapaCorrespondiente(string mapName)
    {
        ResetMapBools();

        return mapName switch
        {
            "Map1" => ActivarMapa(ref inMap1, tituloMapa1),
            "Map2" => ActivarMapa(ref inMap2, tituloMapa2),
            "Map3" => ActivarMapa(ref inMap3, tituloMapa3),
            "Map4" => ActivarMapa(ref inMap4, tituloMapa4),
            "Map5" => ActivarMapa(ref inMap5, tituloMapa5),
            _ => null,
        };
    }

    private void ResetMapBools()
    {
        inMap1 = false;
        inMap2 = false;
        inMap3 = false;
        inMap4 = false;
        inMap5 = false;
    }

    private GameObject ActivarMapa(ref bool mapBool, GameObject tituloMapa)
    {
        mapBool = true;
        return tituloMapa;
    }
    public void ActivarTituloCorrespondiente()
    {
        tituloMapa1.SetActive(inMap1);
        tituloMapa2.SetActive(inMap2);
        tituloMapa3.SetActive(inMap3);
        tituloMapa4.SetActive(inMap4);
        tituloMapa5.SetActive(inMap5);

        SetCanvasGroupAlpha(tituloMapa1, inMap1);
        SetCanvasGroupAlpha(tituloMapa2, inMap2);
        SetCanvasGroupAlpha(tituloMapa3, inMap3);
        SetCanvasGroupAlpha(tituloMapa4, inMap4);
        SetCanvasGroupAlpha(tituloMapa5, inMap5);
    }
    public void DesactivarTitulos()
    {
        tituloMapa1.SetActive(false);
        tituloMapa2.SetActive(false);
        tituloMapa3.SetActive(false);
        tituloMapa4.SetActive(false);
        tituloMapa5.SetActive(false);

        SetCanvasGroupAlpha(tituloMapa1, false);
        SetCanvasGroupAlpha(tituloMapa2, false);
        SetCanvasGroupAlpha(tituloMapa3, false);
        SetCanvasGroupAlpha(tituloMapa4, false);
        SetCanvasGroupAlpha(tituloMapa5, false);
    }


    private void SetCanvasGroupAlpha(GameObject tituloMapa, bool isActive)
    {
        if (tituloMapa.TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup.alpha = isActive ? 1f : 0f;
        }
    }


    private AudioSource GetSoundtrackCorrespondiente(string mapName)
    {
        return mapName switch
        {
            "Map1" => soundtrack1,
            "Map2" => soundtrack2,
            "Map3" => soundtrack3,
            "Map4" => soundtrack4,
            "Map5" => soundtrack5,
            _ => soundtrackTutorial,
        };
    }

    private void CambiarMusica(AudioSource nuevaMusica)
    {
        if (currentSoundtrack != nuevaMusica)
        {
            StartCoroutine(TransicionMusica(nuevaMusica));
        }
    }

    private IEnumerator TransicionMusica(AudioSource nuevaMusica)
    {
        if (currentSoundtrack != null)
        {
            for (float t = 1; t >= 0; t -= Time.deltaTime * 3)
            {
                currentSoundtrack.volume = t;
                yield return null;
            }
            currentSoundtrack.Stop();
        }

        currentSoundtrack = nuevaMusica;
        if (currentSoundtrack != null)
        {
            currentSoundtrack.Play();
            for (float t = 0; t <= 1; t += Time.deltaTime * 3)
            {
                currentSoundtrack.volume = t;
                yield return null;
            }
        }
    }
    public void ActivarMostrarTitulos()
    {
        if (inMap1)
            StartCoroutine(MostrarTitulos(tituloMapa1));
        else if (inMap2)
            StartCoroutine(MostrarTitulos(tituloMapa2));
        else if (inMap3)
            StartCoroutine(MostrarTitulos(tituloMapa3));
        else if (inMap4)
            StartCoroutine(MostrarTitulos(tituloMapa4));
        else if (inMap5)
            StartCoroutine(MostrarTitulos(tituloMapa5));
    }
    private IEnumerator MostrarTitulos(GameObject tituloMapa)
    {
        if (tituloMapa == null) yield break;

        yield return new WaitForSeconds(0.0f);
        yield return StartCoroutine(FadeIn(tituloMapa));

        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FadeOut(tituloMapa));
    }

    private IEnumerator FadeIn(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>() ?? obj.AddComponent<CanvasGroup>();
        obj.SetActive(true);

        for (float t = 0; t <= 1; t += Time.deltaTime * 5)
        {
            canvasGroup.alpha = t;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    private IEnumerator FadeOut(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null) yield break;

        for (float t = 1; t >= 0; t -= Time.deltaTime * 5)
        {
            canvasGroup.alpha = t;
            yield return null;
        }
        canvasGroup.alpha = 0;
        obj.SetActive(false);
    }
}
