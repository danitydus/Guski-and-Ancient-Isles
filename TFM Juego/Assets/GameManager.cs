using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("Panel to be shown during pause mode.")]
    public GameObject pauseMenuPanel;

    private bool isPaused = false;
    private float pauseToggleCooldown = 1f;
    private float lastPauseToggleTime = 0f;

    void Update()
    {
        // Usamos Time.unscaledTime en lugar de Time.time para el control del tiempo
        if (Input.GetKeyDown(KeyCode.P) && Time.unscaledTime >= lastPauseToggleTime + pauseToggleCooldown)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }

            lastPauseToggleTime = Time.unscaledTime; // Actualizamos el tiempo con Time.unscaledTime
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pausamos el juego
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true); // Mostramos el panel de pausa
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reanudamos el juego
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false); // Ocultamos el panel de pausa
        }
    }

    public void LoadScene(int sceneIndex)
    {
        Time.timeScale = 1f; // Aseguramos que el tiempo está en su estado normal antes de cambiar de escena
        SceneManager.LoadScene(sceneIndex);
    }
}
