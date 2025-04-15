using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GemManager : MonoBehaviour
{
    private bool[,] gemCollection = new bool[5, 4]; // 5 niveles, 4 gemas por nivel
    public RawImage[] gemImages; // Imágenes de gemas asignadas desde el Inspector
   
    public static GemManager Instance;
    public List<bool> gems1;
    public List<bool> gems2;
    public List<bool> gems3;
    public List<bool> gems4;
    public List<bool> gems5;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateAllGemImages(); // Asegurar que las imágenes reflejen correctamente el estado inicial
    }
    public bool AreAllGemsCollected(List<bool> gems)
    {
        if (gems == null || gems.Count == 0)
        {
            Debug.LogError("La lista de gemas es nula o vacía.");
            return false;
        }

        return gems.All(gem => gem);
    }
    public void CollectGem(int level, char gem)
    {
        if (level < 1 || level > 5)
        {
            Debug.LogError("Nivel fuera de rango (1-5)");
            return;
        }

        int gemIndex = GemToIndex(gem);
        if (gemIndex == -1)
        {
            Debug.LogError("Gema no válida (A, B, C o D)");
            return;
        }

        int arrayIndex = (level - 1) * 4 + gemIndex;
        gemCollection[level - 1, gemIndex] = true;
        UpdateGemImage(arrayIndex);
    }


    public bool IsGemCollected(int level, char gem)
    {

        //Actualizar lista por nivel
        if (level == 1)
        {
            if (gem == 'A')
            {
                gems1[0] = true;
            } if (gem == 'B')
            {
                gems1[1] = true;
            } if (gem == 'C')
            {
                gems1[2] = true;
            } if (gem == 'D')
            {
                gems1[3] = true;
            }
        }  if (level == 2)
        {
            if (gem == 'A')
            {
                gems2[0] = true;
            } if (gem == 'B')
            {
                gems2[1] = true;
            } if (gem == 'C')
            {
                gems2[2] = true;
            } if (gem == 'D')
            {
                gems2[3] = true;
            }
        }  if (level == 3)
        {
            if (gem == 'A')
            {
                gems3[0] = true;
            } if (gem == 'B')
            {
                gems3[1] = true;
            } if (gem == 'C')
            {
                gems3[2] = true;
            } if (gem == 'D')
            {
                gems3[3] = true;
            }
        }  if (level == 4)
        {
            if (gem == 'A')
            {
                gems4[0] = true;
            } if (gem == 'B')
            {
                gems4[1] = true;
            } if (gem == 'C')
            {
                gems4[2] = true;
            } if (gem == 'D')
            {
                gems4[3] = true;
            }
        }  if (level == 5)
        {
            if (gem == 'A')
            {
                gems5[0] = true;
            } if (gem == 'B')
            {
                gems5[1] = true;
            } if (gem == 'C')
            {
                gems5[2] = true;
            } if (gem == 'D')
            {
                gems5[3] = true;
            }
        }
        if (level < 1 || level > 5) return false;
        int gemIndex = GemToIndex(gem);
        if (gemIndex == -1) return false;
        return gemCollection[level - 1, gemIndex];


    }

    private int GemToIndex(char gem)
    {
        switch (gem)
        {
            case 'A': return 0;
            case 'B': return 1;
            case 'C': return 2;
            case 'D': return 3;
            default: return -1;
        }
    }

    private void UpdateGemImage(int index)
    {
        if (gemImages != null && index >= 0 && index < gemImages.Length && gemImages[index] != null)
        {
            Color color = gemImages[index].color;
            color.a = gemCollection[index / 4, index % 4] ? 1f : 0.45f; // Opacidad total si está recogida, casi invisible si no
            gemImages[index].color = color;
        }
    }

    private void UpdateAllGemImages()
    {
        for (int level = 0; level < 5; level++)
        {
            for (int gemIndex = 0; gemIndex < 4; gemIndex++)
            {
                int arrayIndex = level * 4 + gemIndex;
                UpdateGemImage(arrayIndex);
            }
        }
    }

    public void AnimateGem(int index)
    {
        if (gemImages != null && index >= 0 && index < gemImages.Length && gemImages[index] != null)
        {
            StartCoroutine(AnimateGemRoutine(gemImages[index]));
        }
    }
    public void AnimateAllGems()
    {
        if (gemImages == null) return;

        for (int i = 0; i < 4 && i < gemImages.Length; i++)
        {
            if (gemImages[i] != null)
            {
                StartCoroutine(AnimateGemRoutine(gemImages[i]));
            }
        }
    }

    private IEnumerator AnimateGemRoutine(RawImage gemImage)
    {
        float duration = 2f;
        float elapsed = 0f;
        Vector3 originalScale = gemImage.transform.localScale;

        while (elapsed < duration)
        {
            float scaleFactor = 1f + Mathf.Sin(elapsed * Mathf.PI * 4) * 0.2f; // Oscilación suave
            gemImage.transform.localScale = originalScale * scaleFactor;
            elapsed += Time.deltaTime;
            yield return null;
        }

        gemImage.transform.localScale = originalScale; // Restaurar tamaño original
    }
    public (int level1, int level2, int level3) GetGemsCollectedInEachLevel()
    {
        int level1 = gems1 != null ? gems1.Count(g => g) : 0;
        int level2 = gems2 != null ? gems2.Count(g => g) : 0;
        int level3 = gems3 != null ? gems3.Count(g => g) : 0;

        return (level1, level2, level3);
    }
}
