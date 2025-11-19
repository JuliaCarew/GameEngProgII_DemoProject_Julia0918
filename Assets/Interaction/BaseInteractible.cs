using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Outline))]
public class BaseInteractible : MonoBehaviour, IInteractible
{
    [Header("Highlight Settings")]
    protected bool isFocused = false;
    protected Outline outline;

    [Header("World Space Text")]
    [SerializeField] protected TextMeshPro worldSpaceText;
    [SerializeField] protected Vector3 textOffset = new Vector3(0, 2f, 0); // Offset above the object
    [SerializeField] protected bool faceCamera = true; // Make text face the camera 

    public virtual void Awake()
    {
        if(gameObject.layer != LayerMask.NameToLayer("Interactible"))
            gameObject.layer = LayerMask.NameToLayer("Interactible");

        // initialize outline effect
        outline = GetComponent<Outline>();
        outline.OutlineColor = Color.black; // set outline color
        outline.OutlineWidth = 10; // set outline width
        outline.enabled = false; // disable outline by default

        // Setup world space text if not assigned
        if (worldSpaceText == null)
        {
            SetupWorldSpaceText();
        }
        else
        {
            // Position the text 
            worldSpaceText.transform.SetParent(transform);
            worldSpaceText.transform.localPosition = textOffset;
        }

        // Hide text by default
        if (worldSpaceText != null)
        {
            worldSpaceText.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        // Make text face the camera if enabled
        if (faceCamera && worldSpaceText != null && worldSpaceText.gameObject.activeSelf && Camera.main != null)
        {
            worldSpaceText.transform.LookAt(worldSpaceText.transform.position + Camera.main.transform.rotation * Vector3.forward,
                Camera.main.transform.rotation * Vector3.up);
        }
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

        // Update world space text visibility and content
        if (worldSpaceText != null)
        {
            worldSpaceText.gameObject.SetActive(focus);
            if (focus)
            {
                worldSpaceText.text = GetInteractionPrompt();
            }
        }
    }

    public virtual string GetInteractionPrompt()
    {
        return "Interact with E"; // Default prompt
    }

    // Setup world space text component
    protected virtual void SetupWorldSpaceText()
    {
        // Create a new GameObject for the text
        GameObject textObject = new GameObject("InteractionText");
        textObject.transform.SetParent(transform);
        textObject.transform.localPosition = textOffset;

        // Add TextMeshPro component
        worldSpaceText = textObject.AddComponent<TextMeshPro>();
        
        // Configure text settings
        worldSpaceText.text = GetInteractionPrompt();
        worldSpaceText.fontSize = 3f;
        worldSpaceText.alignment = TextAlignmentOptions.Center;
        worldSpaceText.color = Color.white;
        
        // Set initial rotation
        worldSpaceText.transform.rotation = Quaternion.identity;
    }
}
