using UnityEngine;

public class ActivatorInteractable : ToggleInteractable
{
    [SerializeField] private GameObject[] toEnableOnActivate;
    [SerializeField] private GameObject[] toEnableOnDeactivate;

    public override void OnActivate() {
        base.OnActivate();

        // disable all of the deactivate objects
        for (int i = 0; i < toEnableOnDeactivate.Length; i++) {
            toEnableOnDeactivate[i].SetActive(false);
        }

        // enable all of the activate objects
        for (int i = 0; i < toEnableOnActivate.Length; i++) {
            toEnableOnActivate[i].SetActive(true);
        }
    }

    public override void OnDeactivate() {
        base.OnDeactivate();

        // disable all of the activate objects
        for (int i = 0; i < toEnableOnActivate.Length; i++) {
            toEnableOnActivate[i].SetActive(false);
        }

        // enable all of the deactivate objects
        for (int i = 0; i < toEnableOnDeactivate.Length; i++) {
            toEnableOnDeactivate[i].SetActive(true);
        }
    }
}
