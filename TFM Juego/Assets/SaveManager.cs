using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public List<GameObject> monedasRecogidas = new List<GameObject>();
    public CubeMovement cubeMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Moneda1"))
        {
            monedasRecogidas.Add(other.gameObject);
        }
    }

    public void RestaurarMonedas()
    {
        foreach (GameObject Circle in monedasRecogidas)
        {
            if (Circle != null)
                Circle.SetActive(true);
            Circle.GetComponent<CoinAnimation>().RespawnCoin();
            
        }
        monedasRecogidas.Clear();
        cubeMovement.coinCount = 0;
    }

    public void DesactivarMonedas()
    {
        foreach (GameObject moneda in monedasRecogidas)
        {
            if (moneda != null)
                moneda.SetActive(false);
        }
    }
}
