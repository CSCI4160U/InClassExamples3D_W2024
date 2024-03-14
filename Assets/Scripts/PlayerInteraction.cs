using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private float range = 3f;

    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private LayerMask interactableMask;

    private void Update() {
        RaycastHit hit;
        InteractableObject interactableObject = null;

        if (Physics.Raycast(camera.position, camera.forward, out hit, range, interactableMask)) {
            interactableObject = hit.collider.GetComponent<InteractableObject>();
            if (interactableObject != null && interactionText != null) {
                interactionText.text = interactableObject.GetInteractionText();
            } else if (interactionText != null) {
                interactionText.text = "";
            }
        } else {
            if (interactionText != null) {
                interactionText.text = "";
            }
        }

        if (Input.GetButtonDown("Fire2") && interactableObject != null) {
            interactableObject.Interact();
        }
    }
}
