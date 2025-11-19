using UnityEngine;

public class LevelTransitionInteractible : BaseInteractible
{
    [Header("Level Transition Settings")]
    [SerializeField] private string nextLevelName = "NextLevel";

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnInteract()
    {
        Debug.Log("Transitioning to next level: " + nextLevelName);
        
        // Use LevelManager to load the scene (which will use the Loading state)
        if (GameManager.Instance != null && GameManager.Instance.LevelManager != null)
        {
            GameManager.Instance.LevelManager.LoadScene(nextLevelName);
        }
        else
        {
            // Fallback to direct loading if managers aren't available
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
        }
    }

    public override string GetInteractionPrompt()
    {
        return "Proceed to next level";
    }
}

