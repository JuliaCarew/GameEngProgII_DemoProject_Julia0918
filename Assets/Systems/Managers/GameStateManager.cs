using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [Header("Debug (read only)")]
    [SerializeField] private string lastActiveState;
    [SerializeField] private string currentActiveState;

    // private variables to store state information
    private IState currentState;
    private IState lastState;

    // instantiat all possible states
    private GameState_MainMenu gameState_MainMenu = GameState_MainMenu.Instance;
    private GameState_Gameplay gameState_Gameplay = GameState_Gameplay.Instance;
    private GameState_Pause gameState_pause = GameState_Pause.Instance;
    private GameState_GameOver gameState_gameOver = GameState_GameOver.Instance;


    private void Start()
    {
        // set initial state to mainmenu 
        currentState = gameState_MainMenu;
        currentActiveState = currentState.ToString();
        currentState.EnterState();
    }


    #region State Machine Update Calls

    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }
    private void Update()
    {
        currentState.UpdateState();
    }
    private void LateUpdate()
    {
        currentState.LateUpdateState();
    }

    #endregion

    private void SwitchState(IState newState)
    {
        lastState = currentState; // store the current state as the last state
        lastActiveState = lastState.ToString();
        currentState?.ExitState(); // exit the current state

        currentState = newState; // switch to the new state
        currentActiveState = currentState.ToString();
        currentState.EnterState(); // enter the new state
    }

    #region Menu Buttons

    public void Pause()
    {
        if (currentState != gameState_Gameplay)
            return;

        if (currentState == gameState_Gameplay)
        {
            SwitchState(gameState_pause);
            return;
        }
    }

    public void Resume()
    {
        if (currentState != gameState_pause)
            return;

        if (currentState == gameState_pause)
        {
            SwitchState(gameState_Gameplay);
            return;
        }
    }

    public void MainMenu() => SwitchState(gameState_MainMenu);
    
    public void Play()
    {
        // Load Level 1 - LevelManager
        LevelManager.Instance.LoadLevel(1);
    }
    
    public void SwitchToGameplayState() => SwitchState(gameState_Gameplay);
    public void Quit() => Application.Quit();
    public void GameOver() => SwitchState(gameState_gameOver); 

    #endregion
}
