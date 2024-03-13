using UnityEngine;
using UnityEngine.AI;

public class RagdollHealth : Health
{
    [SerializeField] private Ragdoller ragdoller;

    private NavMeshAgent agent;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }


    public override void HandleDeath() {
        if (ragdoller != null) {
            ragdoller.Ragdoll(transform);
            ragdoller.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
        }
    }

    public void TakeExplosionDamage(Vector3 explosionPoint, float forceAmount) {
        if (ragdoller != null) {
            ragdoller.Ragdoll(transform);
            ragdoller.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);

            Vector3 towardUs = (transform.position - explosionPoint).normalized;
            // TODO: adjust the force amount based on distance
            ragdoller.ApplyForce(towardUs, forceAmount);
        }
    }
}
