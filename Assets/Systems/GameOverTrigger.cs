using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) gameStateManager.GameOver();
    }
}
