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
        pauseMenuUI.SetActive(false);
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(false);
    }

    public void EnableMainMenu()
    {
        Time.timeScale = 0f;
        UnityEngine.Cursor.visible = true;

        DisableAllMenuUI();
        mainMenuUI.SetActive(true);
    }

    public void EnablePause()
    {
        Time.timeScale = 0f;
        UnityEngine.Cursor.visible = true;

        DisableAllMenuUI();
        pauseMenuUI.SetActive(true);
    }

    public void EnableGameplay()
    {
        Time.timeScale = 1f;
        UnityEngine.Cursor.visible = false;

        DisableAllMenuUI();
        gameplayUI.SetActive(true);
    }

    public void EnableGameOver()
    {
        Time.timeScale = 0f;
        UnityEngine.Cursor.visible = true;

        DisableAllMenuUI();
        gameOverUI.SetActive(true);
    }

}
