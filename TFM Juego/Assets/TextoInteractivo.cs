using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextoInteractivo : MonoBehaviour
{
    [System.Serializable]
    public class TextoCompleto
    {
        public List<string> frases;
    }

    public List<TextoCompleto> textos;
    public TMP_Text textoPantalla;
    public float velocidadEscritura = 0.05f;
    public GameObject guski; // Arrastra aquí el objeto Guski en el inspector
    public AudioSource audioEscritura; // Arrastra aquí el AudioSource en el inspector

    private bool mostrandoTexto = false;
    private HashSet<string> triggersActivados = new HashSet<string>();

    private void Start()
    {
        if (guski != null) guski.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mostrandoTexto) return;

        if (other.CompareTag("Textos"))
        {
            string nombreTrigger = other.name;

            // Si ya se activó este trigger antes, ignorarlo
            if (triggersActivados.Contains(nombreTrigger)) return;

            if (nombreTrigger.StartsWith("Textos"))
            {
                string numeroStr = nombreTrigger.Substring(6);
                if (int.TryParse(numeroStr, out int numeroTrigger))
                {
                    int index = numeroTrigger - 1;
                    if (index >= 0 && index < textos.Count)
                    {
                        triggersActivados.Add(nombreTrigger); // Marcar como usado
                        StartCoroutine(MostrarTexto(textos[index].frases));
                    }
                }
            }
        }
    }

    IEnumerator MostrarTexto(List<string> frases)
    {
        mostrandoTexto = true;
        if (guski != null) guski.SetActive(true);

        foreach (string frase in frases)
        {
            textoPantalla.text = "";

            if (audioEscritura != null)
                audioEscritura.Play();

            foreach (char letra in frase)
            {
                textoPantalla.text += letra;
                yield return new WaitForSeconds(velocidadEscritura);
            }

            if (audioEscritura != null)
                audioEscritura.Stop();

            yield return new WaitForSeconds(1f);
        }

        textoPantalla.text = "";
        mostrandoTexto = false;
        if (guski != null) guski.SetActive(false);
    }
}
