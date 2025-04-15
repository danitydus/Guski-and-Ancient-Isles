using UnityEngine;

public class ZonaBloqueada : MonoBehaviour
{
    public GameObject Bloqueo;
    public GameObject InteraccionText;
    public int monedasRequeridas;
    public AudioSource sonidoApertura;
    public AudioSource sonidoInsuficiente;
    private bool enZona = false;
    public CubeMovement cubeMovement;
    private bool isOpen;
    public void Start()
    {
        InteraccionText.SetActive(false);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            InteraccionText.SetActive(true);
            enZona = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteraccionText.SetActive(false);
            enZona = false;
        }
    }

    private void Update()
    {
        if (enZona && Input.GetKeyDown(KeyCode.E))
        {
            if (cubeMovement.coinCount >= monedasRequeridas)
            {
                Destroy(Bloqueo);
                sonidoApertura.Play();
                cubeMovement.coinCount -= monedasRequeridas;
                isOpen = true;
            }
            else
            {
                sonidoInsuficiente.Play();
            }
        }
    }
}

public static class Jugador
{
    public static int monedas = 0;
}