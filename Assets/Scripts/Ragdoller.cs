using UnityEngine;

public class Ragdoller : MonoBehaviour
{
    [SerializeField] private Rigidbody mainBody;

    public void Ragdoll(Transform newTransform) {
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;

        // TODO: Move all of the armature components too
    }

    public void ApplyForce(Vector3 forceDirection, float forceAmount) {
        if ((mainBody != null) && (forceAmount > 0f)) {
            mainBody.AddForce(forceDirection * forceAmount);
        }
    }
}
