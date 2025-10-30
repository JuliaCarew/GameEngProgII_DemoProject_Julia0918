using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject pauseMenuUI;
    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject gameOverUI;

    private void Awake()
    {
        #region Singleton

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        #endregion

        DisableAllMenuUI();
    }

    public void DisableAllMenuUI()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (mainMenuUI != null) mainMenuUI.SetActive(false);
        if (gameplayUI != null) gameplayUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(false);
    }

    public void EnableMainMenu()
    {
        Time.timeScale = 0f;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        DisableAllMenuUI();
        mainMenuUI.SetActive(true);
    }

    public void EnablePause()
    {
        Time.timeScale = 0f;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        DisableAllMenuUI();
        pauseMenuUI.SetActive(true);
    }

    public void EnableGameplay()
    {
        Time.timeScale = 1f;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        DisableAllMenuUI();

        if (gameplayUI != null)
        {
            gameplayUI.SetActive(true);
        }
    }

    public void EnableGameOver()
    {
        Time.timeScale = 0f;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        DisableAllMenuUI();
        gameOverUI.SetActive(true);
    }

    public void ShowInteractionPrompt(string prompt)
    {
        // Implementation for showing interaction prompt on UI
        Debug.Log("Show Interaction Prompt: " + prompt);
    }   
}
