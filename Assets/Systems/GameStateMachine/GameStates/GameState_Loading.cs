using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState_Loading : IState
{
    #region Singleton Instance

    private static readonly GameState_Loading instance = new GameState_Loading();

    public static GameState_Loading Instance = instance;

    #endregion

    #region Variables and References
    // references
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    UIManager uiManager => GameManager.Instance.UiManager;
    LevelManager levelManager => GameManager.Instance.LevelManager;

    // loading state
    private AsyncOperation loadingOperation;
    private string targetSceneName;
    private bool isLoading = false;
    private bool canActivateScene = false; 
    
    // smooth progress animation
    private float displayedProgress = 0f;
    private float progressAnimationSpeed = 0.5f; // control fill speed (lower = slower)

    #endregion

    public void EnterState()
    {
        Debug.Log("Entered Loading State");

        Cursor.visible = false;
        Time.timeScale = 1f;
        
        // Reset progress bar
        displayedProgress = 0f;
        canActivateScene = false;
        uiManager.ResetLoadingProgress();
        uiManager.EnableLoadScreen();
        
        isLoading = false;
        loadingOperation = null;
    }

    public void FixedUpdateState()
    {
    }

    public void UpdateState()
    {
        // Update progress bar if loading
        if (isLoading && loadingOperation != null)
        {
            float targetProgress;
            
            // If the async operation is done loading, target is 100%
            if (loadingOperation.progress >= 0.9f)
            {
                // Scene is ready to activate, but we'll wait for the progress bar to fill
                targetProgress = 1.0f;
            }
            else
            {
                targetProgress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            }

            // Smoothly animate the displayed progress toward the target progress
            displayedProgress = Mathf.MoveTowards(displayedProgress, targetProgress, progressAnimationSpeed * Time.deltaTime);
            
            uiManager.UpdateLoadingProgress(displayedProgress);
            Debug.Log($"Loading progress: {displayedProgress * 100f:F1}% (Target: {targetProgress * 100f:F1}%)");

            // Only allow scene activation when progress bar has reached 100%
            if (displayedProgress >= 1.0f && !canActivateScene)
            {
                canActivateScene = true;
                loadingOperation.allowSceneActivation = true;
                Debug.Log("Progress bar complete! Activating scene...");
            }

            // Check if loading is complete and displayed progress has reached 100%
            if (canActivateScene && loadingOperation.isDone)
            {
                isLoading = false;
                loadingOperation = null;
                canActivateScene = false;
            }
        }
    }

    public void LateUpdateState()
    {
    }

    public void ExitState()
    {
        Debug.Log("Exited Loading State");
        isLoading = false;
        canActivateScene = false;
        loadingOperation = null;
    }

    public void LoadScene(string sceneName)
    {
        if (isLoading)
        {
            Debug.LogWarning("Already loading a scene. Cannot load: " + sceneName);
            return;
        }

        targetSceneName = sceneName;
        isLoading = true;
        canActivateScene = false;
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;
        
        Debug.Log("Starting to load scene: " + sceneName);
    }

    public void LoadLevel(int levelNumber)
    {
        string levelName = $"Level {levelNumber:D2}";
        LoadScene(levelName);
    }
}
