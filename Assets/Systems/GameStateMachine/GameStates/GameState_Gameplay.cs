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
        
        if (uiManager == null)
        {
            Debug.LogError("UIManager is null in GameState_Gameplay!");
            return;
        }
        
        if (playerController == null)
        {
            Debug.LogError("PlayerController is null in GameState_Gameplay!");
            return;
        }
        
        Debug.Log($"PlayerController reference: {playerController.name}");
        uiManager.EnableGameplay();
    }

    public void FixedUpdateState()
    {
        // Handle physics-related updates for gameplay state here
    }

    public void UpdateState()
    {
        // Handle regular updates such as input handling and game logic for gameplay state here
        
        if (playerController != null)
        {
            playerController.HandlePlayerMovement();
        }

        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            gameStateManager.Pause();
        }
    }

    public void LateUpdateState()
    {
        // Handle any late updates that need to occur after the main update logic for gameplay state here

        if (playerController != null)
        {
            playerController.HandlePlayerLook();
        }
    }

    public void ExitState()
    {
        // Cleanup logic for exiting gameplay state can be added here
        Debug.Log("Exited Gameplay State");
    }
}
