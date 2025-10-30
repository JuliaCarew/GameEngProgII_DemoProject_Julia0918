using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Outline))]
public class BaseInteractible : MonoBehaviour, IInteractible
{
    [Header("Highlight Settings")]
    protected bool isFocused = false;
    protected Outline outline;

    public virtual void Awake()
    {
        if(gameObject.layer != LayerMask.NameToLayer("Interactible"))
            gameObject.layer = LayerMask.NameToLayer("Interactible");

        // initialize outline effect
        outline = GetComponent<Outline>();
        outline.OutlineColor = Color.black; // set outline color
        outline.OutlineWidth = 10; // set outline width
        outline.enabled = false; // disable outline by default
    }

    // Implementation for interaction
    public virtual void OnInteract()
    {
        Debug.Log("Interacted with: " + gameObject.name);
    }

    // Implementation for focus handling
    public void SetFocus(bool focus)
    {
        if (isFocused == focus) return;

        isFocused = focus;

        // enable or disable outline based on focus state
        if (focus)
            outline.enabled = true;
        else
            outline.enabled = false;
    }

    public string GetInteractionPrompt()
    {
        return "Interact"; // Default prompt
    }
}
