using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_GameOver : IState
{
    #region Singleton Instance

    private static readonly GameState_GameOver instance = new GameState_GameOver();

    public static GameState_GameOver Instance = instance;

    #endregion

    // references
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    UIManager uiManager => GameManager.Instance.UiManager;

    public void EnterState()
    {
        // Additional initialization logic for main menu state can be added here
        Debug.Log("Entered GameOver State");
        uiManager.EnableGameOver();
    }

    public void FixedUpdateState()
    {
        // Handle physics-related updates for main menu state here
        Debug.Log("GameOver State : Running fixed update");
    }

    public void UpdateState()
    {
        // Handle regular updates such as input handling and UI logic for main menu state here
    }

    public void LateUpdateState()
    {
        // Handle any late updates that need to occur after the main update logic for main menu state here
        Debug.Log("GameOver State : Running late update");
    }

    public void ExitState()
    {
        // Cleanup logic for exiting main menu state can be added here
        Debug.Log("Exited GameOver State");
    }
}
