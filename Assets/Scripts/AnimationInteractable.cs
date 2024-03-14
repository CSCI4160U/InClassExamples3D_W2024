using UnityEngine;

public class AnimationInteractable : ToggleInteractable
{
    [SerializeField] private Animator[] animatorsOnActivate;
    [SerializeField] private Animator[] animatorsOnDeactivate;

    [SerializeField] private string[] toSetBooleanOnActivate;
    [SerializeField] private string[] toSetBooleanOnDeactivate;

    [SerializeField] private bool[] booleanValueOnActivate;
    [SerializeField] private bool[] booleanValueOnDeactivate;

    public override void OnActivate() {
        base.OnActivate();

        for (int i = 0; i < animatorsOnActivate.Length; ++i) {
            animatorsOnActivate[i].SetBool(toSetBooleanOnActivate[i], booleanValueOnActivate[i]);
        }
    }

    public override void OnDeactivate() {
        base.OnDeactivate();

        for (int i = 0; i < animatorsOnDeactivate.Length; ++i) {
            animatorsOnDeactivate[i].SetBool(toSetBooleanOnDeactivate[i], booleanValueOnDeactivate[i]);
        }
    }
}
