using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource[] canciones; // Lista de canciones
    private AudioSource currentSong; // Canción actual

    private CubeMovement cubeMovementScript; // Referencia al script CubeMovement
    private AudioLowPassFilter lowPassFilter; // Filtro de paso bajo
    // AudioManager.cs


    public AudioClip coinSound;
    public AudioClip gemSound;
    public AudioSource audioSource;

    public void PlayCoinSound()
    {
        audioSource.PlayOneShot(coinSound);
    }

    public void PlayGemSound()
    {
        audioSource.PlayOneShot(gemSound);
    }

void Start()
    {
        // Encuentra el script CubeMovement
        cubeMovementScript = FindObjectOfType<CubeMovement>();

        // Configuración inicial
        SetInitialMusic();
    }

  

    void SetInitialMusic()
    {
        if (canciones.Length > 0)
        {
            currentSong = canciones[0];
            currentSong.Play();
        }
    }


}
