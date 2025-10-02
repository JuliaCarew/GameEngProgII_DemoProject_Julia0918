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

    // public read-only accessors for other scripts to use the managers
    public InputManager InputManager => inputManager;
    public GameStateManager GameStateManager => gameStateManager;
    public PlayerController PlayerController => playerController;
    public UIManager UiManager => uiManager;

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

        // auto-assign manager references if not set in inspector
        inputManager ??= GetComponentInChildren<InputManager>();
        gameStateManager ??= GetComponentInChildren<GameStateManager>();
        playerController ??= GetComponentInChildren<PlayerController>();
        uiManager ??= GetComponentInChildren<UIManager>();
    }
}
