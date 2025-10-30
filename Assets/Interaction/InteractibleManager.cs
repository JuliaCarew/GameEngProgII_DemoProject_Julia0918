using Unity.VisualScripting;
using UnityEngine;

public class InteractibleManager : MonoBehaviour
{
    private InputManager inputManager => GameManager.Instance.InputManager;

    [Header("Interaction Settings")]
    private LayerMask interactibleLayer;
    [SerializeField] private float interactionRange = 3f;

    private IInteractible currentFocusedInteractible;

    private Transform cameraRoot;

    private void Start()
    {
        interactibleLayer = LayerMask.GetMask("Interactible");
        cameraRoot = GameManager.Instance.PlayerController.CameraRoot;
    }

    private void Update()
    {
        HandleInteractionDetection(); // later, move to GameStates
    }

    private void HandleInteractionDetection()
    {
        var Ray = new Ray(cameraRoot.position, cameraRoot.forward);

        if (Physics.Raycast(Ray, out RaycastHit hitInfo, interactionRange, interactibleLayer))
        {
            // debug log to put out name if object targeting
            //Debug.Log("Focused on: " + hitInfo.collider.gameObject.name + " interactible at distance: " + hitInfo.distance);

            // get interactible component from the object hit
            IInteractible interactible = hitInfo.collider.GetComponent<IInteractible>();
            if (interactible != null)
            {
                // if it's a new interactible, update focus
                if (currentFocusedInteractible != interactible)
                {
                    // remove focus from previous interactible
                    if (currentFocusedInteractible != null)
                        currentFocusedInteractible.SetFocus(false);

                    // set focus to new interactible
                    currentFocusedInteractible = interactible;
                    currentFocusedInteractible.SetFocus(true);

                    //Debug.Log("New interactible focused: " + hitInfo.collider.gameObject.name);

                    // get prompt text to show interactible UI
                    string prompt = currentFocusedInteractible.GetInteractionPrompt();
                    GameManager.Instance.UiManager.ShowInteractionPrompt(prompt);
                }
            }
        }
        else if (currentFocusedInteractible != null)
        {
            // no interactible in focus, clear current focus
            currentFocusedInteractible.SetFocus(false);
            currentFocusedInteractible = null;
        }
    }
    
    private void HandleInteractInput()
    {
            if (currentFocusedInteractible != null)
            {
                currentFocusedInteractible.OnInteract();
            }
    }

    private void OnEnable()
    {
        inputManager.InteractInputEvent += HandleInteractInput;
    }

    private void OnDestroy()
    {
        inputManager.InteractInputEvent -= HandleInteractInput;
    }

}
