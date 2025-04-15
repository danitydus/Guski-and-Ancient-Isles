using UnityEngine;

public class Nube : MonoBehaviour
{
    public float altura = 0.5f;
    public float velocidad = 5f;
    public bool withCoin = false;
    public CoinAnimation coin;

    private Vector3 posicionOriginal;
    private bool animando = false;
    public CubeMovement cubeMovement;
    private void Start()
    {
        posicionOriginal = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !animando)
        {
            StartCoroutine(SubirYBajar());
            if (withCoin && coin != null)
            {
                coin.IniciarMoneda();
                cubeMovement.sumarMoneda(1);
             }
        }
    }

    private System.Collections.IEnumerator SubirYBajar()
    {
        animando = true;
        Vector3 destino = posicionOriginal + Vector3.up * altura;
        while (Vector3.Distance(transform.position, destino) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
            yield return null;
        }
        while (Vector3.Distance(transform.position, posicionOriginal) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicionOriginal, velocidad * Time.deltaTime);
            yield return null;
        }
        transform.position = posicionOriginal;
        animando = false;
    }
}
