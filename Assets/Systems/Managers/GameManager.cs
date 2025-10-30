using UnityEngine;

// game manager must load first to initialize its references before sub-managers
[DefaultExecutionOrder(-100)]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Manager References (Auto-Assigned)")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private InteractibleManager interactibleManager;


    // public read-only accessors for other scripts to use the managers
    public InputManager InputManager => inputManager;
    public GameStateManager GameStateManager => gameStateManager;
    public PlayerController PlayerController => playerController;
    public UIManager UiManager => uiManager;
    public LevelManager LevelManager => levelManager;
    public InteractibleManager InteractibleManager => InteractibleManager;



    void Awake()
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

        // auto-assign manager references 
        inputManager ??= GetComponentInChildren<InputManager>();
        gameStateManager ??= GetComponentInChildren<GameStateManager>();
        playerController ??= GetComponentInChildren<PlayerController>();
        uiManager ??= GetComponentInChildren<UIManager>();
        levelManager ??= GetComponentInChildren<LevelManager>();
        interactibleManager ??= GetComponentInChildren<InteractibleManager>();

    }

    public void UpdatePlayerController(PlayerController newPlayerController)
    {
        playerController = newPlayerController;
    }
}
