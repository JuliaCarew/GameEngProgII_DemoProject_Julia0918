using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState_Loading : IState
{
    #region Singleton Instance

    private static readonly GameState_Loading instance = new GameState_Loading();

    public static GameState_Loading Instance = instance;

    #endregion

    // references
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    UIManager uiManager => GameManager.Instance.UiManager;
    LevelManager levelManager => GameManager.Instance.LevelManager;

    // loading state
    private AsyncOperation loadingOperation;
    private string targetSceneName;
    private bool isLoading = false;

    public void EnterState()
    {
        Debug.Log("Entered Loading State");

        Cursor.visible = false;
        Time.timeScale = 1f;
        
        // Reset progress bar
        uiManager.ResetLoadingProgress();
        uiManager.EnableLoadScreen();
        
        isLoading = false;
        loadingOperation = null;
    }

    public void FixedUpdateState()
    {
        // Handle physics-related updates for loading state here
    }

    public void UpdateState()
    {
        // Update progress bar if loading
        if (isLoading && loadingOperation != null)
        {
            float progress;
            if (loadingOperation.isDone)
            {
                // Loading is complete, set progress to 100%
                progress = 1.0f;
                isLoading = false;
                loadingOperation = null;
            }
            else
            {
                progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            }

            uiManager.UpdateLoadingProgress(progress);
            // TODO: change progress to update slowly over time
            Debug.Log($"Loading progress: {progress * 100f}%");
        }
    }

    public void LateUpdateState()
    {
        // Handle any late updates that need to occur after the main update logic for loading state here
    }

    public void ExitState()
    {
        Debug.Log("Exited Loading State");
        isLoading = false;
        loadingOperation = null;
    }

    // Public method to start loading a scene
    public void LoadScene(string sceneName)
    {
        if (isLoading)
        {
            Debug.LogWarning("Already loading a scene. Cannot load: " + sceneName);
            return;
        }

        targetSceneName = sceneName;
        isLoading = true;
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = true;
        
        Debug.Log("Starting to load scene: " + sceneName);
    }

    // Public method to start loading a level by number
    public void LoadLevel(int levelNumber)
    {
        string levelName = $"Level {levelNumber:D2}";
        LoadScene(levelName);
    }
}
