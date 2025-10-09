using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    // references
    private GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    private PlayerController playerController => GameManager.Instance.PlayerController;

    // set gameplay scenes by name
    private readonly string[] gameplayScenes = { "Level 01", "Level 02" };

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
    }

    void Start() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Level Manager: scene Loaded - {scene.name}");

        bool isGameplayScene = IsGameplayScene(scene.name);

        if (isGameplayScene)
        {
            // find the PlayerController first when scene loads
            PlayerController scenePlayer = FindFirstObjectByType<PlayerController>();
            if (scenePlayer != null)
                GameManager.Instance.UpdatePlayerController(scenePlayer);

            // switch to Gameplay state
            gameStateManager.SwitchToGameplayState();

            // move player to spawnpoint
            MovePlayerToSpawnPoint();
        }
        else if (scene.name == "Main Menu")
            gameStateManager.MainMenu(); // switch to Main Menu state
    }

    private bool IsGameplayScene(string sceneName)
    {
        // gameplay scenes = any scene named in the array
        foreach (string gameplayScene in gameplayScenes)
        {
            if (sceneName == gameplayScene)
                return true;
        }
        return false;
    }

    public void MovePlayerToSpawnPoint()
    {
        PlayerSpawnpoint spawnPoint = FindFirstObjectByType<PlayerSpawnpoint>();
        playerController.MovePlayerToSpawnPosition(spawnPoint.transform);
    }

    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void LoadLevel(int levelNumber)
    {
        string levelName = $"Level {levelNumber:D2}"; 
        LoadScene(levelName);
    }
}
