using UnityEngine;

public interface IInteractible
{
    void OnInteract();
    void SetFocus(bool focused);
    string GetInteractionPrompt();
}
