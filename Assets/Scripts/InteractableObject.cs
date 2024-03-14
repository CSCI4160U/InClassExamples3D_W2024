using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] protected string activateText = "Right-click to interact";

    public virtual void Interact() {
        Debug.Log("Interacted with " + gameObject.name);
    }

    public virtual string GetInteractionText() { return activateText; }
}
