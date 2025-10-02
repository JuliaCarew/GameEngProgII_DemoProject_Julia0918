using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_Pause : IState
{
    #region Singleton Instance

    private static readonly GameState_Pause instance = new GameState_Pause();

    public static GameState_Pause Instance = instance;

    #endregion

    // references
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    UIManager uiManager => GameManager.Instance.UiManager;

    public void EnterState()
    {
        // Additional initialization logic for main menu state can be added here
        Debug.Log("Entered Pause State");
        uiManager.EnablePause();
    }

    public void FixedUpdateState()
    {
        // Handle physics-related updates for main menu state here
        Debug.Log("Pause State : Running fixed update");
    }

    public void UpdateState()
    {
        // Handle regular updates such as input handling and UI logic for main menu state here
        Debug.Log("Pause State : Running update");

        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            gameStateManager.Resume();
        }
    }

    public void LateUpdateState()
    {
        // Handle any late updates that need to occur after the main update logic for main menu state here
        Debug.Log("Pause State : Running late update");
    }

    public void ExitState()
    {
        // Cleanup logic for exiting main menu state can be added here
        Debug.Log("Exited Pause State");
    }
}
