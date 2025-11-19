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
        // Additional initialization logic for main menu state can be added here
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
        // Handle physics-related updates for main menu state here
        //Debug.Log("MainMenu State : Running fixed update");
    }

    public void UpdateState()
    {
        // Handle regular updates such as input handling and UI logic for main menu state here
    }

    public void LateUpdateState()
    {
        // Handle any late updates that need to occur after the main update logic for main menu state here
        //Debug.Log("MainMenu State : Running late update");
    }

    public void ExitState()
    {
        // Cleanup logic for exiting main menu state can be added here
        Debug.Log("Exited Boot Load State");
    }
}
