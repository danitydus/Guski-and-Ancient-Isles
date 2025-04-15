using UnityEngine;

public class CoinLoopingMotion : MonoBehaviour
{
    public float radius = 1f; // Radio del looping
    public float speed = 2f;  // Velocidad del movimiento
    public bool canLoop;
    public bool isSpecial;
    public Material material1;
    public Material material2;
    public Material material3;
    public ParticleSystem specialEffect;

    private Vector3 startPosition;
    private float timeOffset;
    private Renderer coinRenderer;

    void Start()
    {
        startPosition = transform.position;
        timeOffset = Random.Range(0f, Mathf.PI * 2); // Para que las monedas no vayan sincronizadas
        coinRenderer = GetComponent<Renderer>();

        if (isSpecial)
        {
            coinRenderer.material = material3;
            gameObject.tag = "Moneda5";
         
        }
        else if (canLoop)
        {
            coinRenderer.material = material2;
            gameObject.tag = "Moneda2";
        }
        else
        {
            coinRenderer.material = material1;
            gameObject.tag = "Moneda1";
        }
    }

    void Update()
    {
        if (canLoop)
        {
            float xOffset = Mathf.Sin((Time.time * speed) + timeOffset) * radius;
            float yOffset = Mathf.Cos((Time.time * speed) + timeOffset) * radius;
            transform.position = startPosition + new Vector3(xOffset, yOffset, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canLoop = false;
        }
    }
}
