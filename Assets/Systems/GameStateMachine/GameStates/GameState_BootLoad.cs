using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_BootLoad : IState
{
    #region Singleton Instance

    private static readonly GameState_BootLoad instance = new GameState_BootLoad();

    public static GameState_BootLoad Instance = instance;

    #endregion

    // references
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    UIManager uiManager => GameManager.Instance.UiManager;

    public void EnterState()
    {
        Debug.Log("Entered Boot Load State");

        Cursor.visible = false;

        Time.timeScale = 0f;

        // if starting from the bootloader scene, go to the main menu scene
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "BootLoader")
        {
            GameManager.Instance.LevelManager.LoadScene("Main Menu");
        }

        // if not in main menu or botloader, assume we are in a gameplay scene
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Main Menu")
        {
            gameStateManager.SwitchToGameplayState();
        }

    }

    public void FixedUpdateState()
    {
    }

    public void UpdateState()
    {
    }

    public void LateUpdateState()
    {
    }

    public void ExitState()
    {
    }
}
