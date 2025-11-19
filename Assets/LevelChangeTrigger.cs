using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (GameManager.Instance != null && GameManager.Instance.LevelManager != null)
        {
            GameManager.Instance.LevelManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Fallback to direct loading if managers aren't available
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}

