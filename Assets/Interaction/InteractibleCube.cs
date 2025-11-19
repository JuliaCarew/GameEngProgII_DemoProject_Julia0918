using UnityEngine;

public class InteractibleCube : BaseInteractible
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void OnInteract()
    {
        Debug.Log("You have interacted with the cube: " + gameObject.name);

        gameObject.SetActive(false); // deactivate the cube upon interaction
    }

    public override string GetInteractionPrompt()
    {
        return "Interact with E";
    }
}
