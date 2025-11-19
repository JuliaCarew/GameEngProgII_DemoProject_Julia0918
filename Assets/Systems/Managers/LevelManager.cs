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

            // switch to Gameplay state (exiting loading state)
            gameStateManager.SwitchToGameplayState();

            // move player to spawnpoint
            MovePlayerToSpawnPoint();
        }
        else if (scene.name == "Main Menu")
        {
            // switch to Main Menu state (exiting loading state)
            gameStateManager.MainMenu();
        }
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

    public void LoadScene(string sceneName)
    {
        // Switch to loading state first, then let it handle the async loading
        gameStateManager.SwitchToLoadingState();
        GameState_Loading.Instance.LoadScene(sceneName);
    }
    
    public void LoadScene(int sceneIndex)
    {
        // Load scene by build index
        string sceneName = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Extract scene name from path
            string sceneNameOnly = System.IO.Path.GetFileNameWithoutExtension(sceneName);
            LoadScene(sceneNameOnly);
        }
        else
        {
            Debug.LogError($"Scene at index {sceneIndex} not found in build settings!");
        }
    }
    
    public void LoadLevel(int levelNumber)
    {
        string levelName = $"Level {levelNumber:D2}"; 
        LoadScene(levelName);
    }
}
