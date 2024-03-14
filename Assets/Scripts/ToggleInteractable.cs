using System.Collections;
using UnityEngine;

public class ToggleInteractable : InteractableObject
{
    [SerializeField] protected string deactivateText = "Right-click to deactivate";
    [SerializeField] private bool isActive = false;

    [SerializeField] private protected AudioClip activateAudioClip;
    [SerializeField] private protected AudioClip deactivateAudioClip;

    [SerializeField] protected bool autoDeactivate = false;
    [SerializeField] protected float autoDeactivateDelay = 8f;

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public override void Interact() {
        base.Interact();

        isActive = !isActive;

        if (isActive) {
            this.OnActivate();

            if (autoDeactivate) {
                // start a coroutine to deactivate after the delay
                StartCoroutine(DeactivateAfterDelay(autoDeactivateDelay));
            }
        } else {
            this.OnDeactivate();
        }
    }

    public virtual void OnActivate() {
        if (audioSource != null && activateAudioClip != null) {
            audioSource.PlayOneShot(activateAudioClip);
        }
    }

    public virtual void OnDeactivate() {
        if (audioSource != null && deactivateAudioClip != null) {
            audioSource.PlayOneShot(deactivateAudioClip);
        }
    }

    public IEnumerator DeactivateAfterDelay(float delay) {
        float timePassed = 0f;

        do {
            timePassed += Time.deltaTime;
            yield return null;
        } while (timePassed < delay);

        isActive = false;
        this.OnDeactivate();

        yield return null;
    }

    public override string GetInteractionText() {
        if (isActive) {
            return deactivateText;
        }
        else {
            return activateText;            
        }
    }
}
