using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)] // want this to run before anything else
public static class PerformBootload
{
    const string BOOTLOADER_SCENE_NAME = "BootLoader";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        // Check if the active scene is not the BootLoader scene
        if (SceneManager.GetActiveScene().name != BOOTLOADER_SCENE_NAME)
        {
            // Iterate through all loaded scenes
            for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                // Check if the BootLoader scene is already loaded
                var candidateScene = SceneManager.GetSceneAt(sceneIndex);

                // If it is, set it as the active scene and return
                if (candidateScene.name == BOOTLOADER_SCENE_NAME)
                {
                    //SceneManager.SetActiveScene(candidateScene);
                    return;
                }
            }

            Debug.Log("Loading BootLoader scene" + BOOTLOADER_SCENE_NAME);
            
            // Load the BootLoader scene additively
            SceneManager.LoadScene(BOOTLOADER_SCENE_NAME, LoadSceneMode.Additive);
        }
    }
}

public class BootLoader : MonoBehaviour
{
    public static BootLoader Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        Test();
    }

    public void Test()
    {
        Debug.Log("BootLoader is working!");
    }
}