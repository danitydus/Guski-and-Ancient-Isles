using System.Collections;
using UnityEngine;

public class MenuIntro : MonoBehaviour
{
    public GameObject creatorName;
    public GameObject presenta;
    public GameObject menuDeJuego;
    public GameObject menuFinalReal;
    public GameObject pulsaBoton;
    public GameObject titulo; // Nuevo GameObject
    public GameObject menuText; // Contenedor del texto del menú que hará fade
    public float displayTime = 2f;
    public float fadeDuration = 1f;
    private bool menuFinalActivo = false;
    private Vector3 tituloOriginalScale;
    private Vector3 tituloTargetScale;
    private Vector3 tituloOriginalPosition;
    private Vector3 tituloTargetPosition;

    void Start()
    {
        creatorName.SetActive(false);
        presenta.SetActive(false);
        menuDeJuego.SetActive(false);
        menuFinalReal.SetActive(false);

        if (pulsaBoton != null)
        {
            pulsaBoton.SetActive(true);
            StartCoroutine(PulsaBotonFading());
        }

        if (titulo != null && menuDeJuego != null)
        {
            tituloOriginalScale = titulo.transform.localScale;
            tituloTargetScale = tituloOriginalScale * 0.6f;
            tituloOriginalPosition = titulo.transform.localPosition;
            tituloTargetPosition = tituloOriginalPosition + new Vector3(0, 50f, 0); // Pequeño desplazamiento hacia arriba
        }

        StartCoroutine(ShowIntroSequence());
    }

    void Update()
    {
        if (menuFinalActivo && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SwitchToFinalMenu());
        }
    }

    IEnumerator ShowIntroSequence()
    {
        yield return StartCoroutine(FadeIn(creatorName));
        yield return new WaitForSeconds(displayTime);
        yield return StartCoroutine(FadeOut(creatorName));

        yield return StartCoroutine(FadeIn(presenta));
        yield return new WaitForSeconds(displayTime);
        yield return StartCoroutine(FadeOut(presenta));

        yield return StartCoroutine(FadeIn(menuDeJuego));
        menuFinalActivo = true;
    }

    IEnumerator SwitchToFinalMenu()
    {
        menuFinalActivo = false;
        yield return StartCoroutine(FadeOut(menuText)); // Solo desaparece el texto del menú, no el título
        yield return StartCoroutine(AnimateTitulo());
        yield return StartCoroutine(FadeIn(menuFinalReal));
    }

    IEnumerator AnimateTitulo()
    {
        float duration = fadeDuration;
        float elapsed = 0;
        Vector3 startScale = titulo.transform.localScale;
        Vector3 startPosition = titulo.transform.localPosition;
        
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            titulo.transform.localScale = Vector3.Lerp(startScale, tituloTargetScale, t);
            titulo.transform.localPosition = Vector3.Lerp(startPosition, tituloTargetPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        titulo.transform.localScale = tituloTargetScale;
        titulo.transform.localPosition = tituloTargetPosition;
    }

    IEnumerator FadeIn(GameObject panel)
    {
        if (panel == titulo) yield break; // Evita que el título se afecte por fade
        
        panel.SetActive(true);
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>() ?? panel.AddComponent<CanvasGroup>();
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    IEnumerator FadeOut(GameObject panel)
    {
        if (panel == titulo) yield break; // Evita que el título se afecte por fade
        
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>() ?? panel.AddComponent<CanvasGroup>();
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = 1 - (t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
        panel.SetActive(false);
    }

    IEnumerator PulsaBotonFading()
    {
        CanvasGroup canvasGroup = pulsaBoton.GetComponent<CanvasGroup>() ?? pulsaBoton.AddComponent<CanvasGroup>();
        while (true)
        {
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                canvasGroup.alpha = t / fadeDuration;
                yield return null;
            }
            canvasGroup.alpha = 1;
            yield return new WaitForSeconds(1f);
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                canvasGroup.alpha = 1 - (t / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 0;
            yield return new WaitForSeconds(1f);
        }
    }
}
