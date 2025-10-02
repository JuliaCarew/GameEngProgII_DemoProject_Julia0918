using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_Gameplay : IState
{
    #region Singleton Instance

    private static readonly GameState_Gameplay instance = new GameState_Gameplay();

    public static GameState_Gameplay Instance = instance;

    #endregion

    // references
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    UIManager uiManager => GameManager.Instance.UiManager;

    PlayerController playerController => gameManager.PlayerController;

    public void EnterState()
    {
        // Additional initialization logic for gameplay state can be added here
        Debug.Log("Entered Gameplay State");
        uiManager.EnableGameplay();
    }

    public void FixedUpdateState()
    {
        // Handle physics-related updates for gameplay state here
        Debug.Log("Gameplay State : Running fixed update");
    }

    public void UpdateState()
    {
        // Handle regular updates such as input handling and game logic for gameplay state here
        Debug.Log("Gameplay State : Running update");
        playerController.HandlePlayerMovement();

        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            gameStateManager.Pause();
        }
    }

    public void LateUpdateState()
    {
        // Handle any late updates that need to occur after the main update logic for gameplay state here
        Debug.Log("Gameplay State : Running late update");

        playerController.HandlePlayerLook();
    }

    public void ExitState()
    {
        // Cleanup logic for exiting gameplay state can be added here
        Debug.Log("Exited Gameplay State");
    }
}
