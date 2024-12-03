using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Asigna aqu� el men� de pausa en el inspector
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Detecta la tecla Escape
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Detiene el tiempo
        isPaused = true;
        pauseMenuUI.SetActive(true); // Activa el men� de pausa
        Cursor.lockState = CursorLockMode.None; // Libera el cursor para el men�
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Restaura el tiempo
        isPaused = false;
        pauseMenuUI.SetActive(false); // Desactiva el men� de pausa
        Cursor.lockState = CursorLockMode.Locked; // Vuelve a ocultar el cursor
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1; // Asegura que el tiempo est� restaurado antes de salir
       
        SceneManager.LoadScene("Menu");
    }
}