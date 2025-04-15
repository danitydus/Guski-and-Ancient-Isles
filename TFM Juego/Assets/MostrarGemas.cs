using TMPro; // Asegúrate de tener esto para usar TextMeshPro
using UnityEngine;

public class MostrarGemas : MonoBehaviour
{
    public TriggerMapa triggerMapa;
    public GemManager gemManager;
    public TextMeshProUGUI textoGemas;

    void Update()
    {
        if (triggerMapa == null || gemManager == null || textoGemas == null) return;

        var (level1, level2, level3) = gemManager.GetGemsCollectedInEachLevel();

        if (triggerMapa.inMap1)
        {
            textoGemas.text = "" + level1.ToString();
        }
        else if (triggerMapa.inMap2)
        {
            textoGemas.text = "" + level2.ToString();
        }
        else if (triggerMapa.inMap3)
        {
            textoGemas.text = "" + level3.ToString();
        }
        else
        {
            textoGemas.text = "";
        }
    }
}
