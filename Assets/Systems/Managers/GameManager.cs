using UnityEngine;

// game manager must load first to initialize its references before sub-managers
[DefaultExecutionOrder(-100)]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Manager References")]
    public InputManager inputManager;

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

        if (inputManager == null) inputManager = GetComponentInChildren<InputManager>();    
    }
}
