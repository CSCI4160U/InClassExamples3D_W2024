using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float alertDistance = 10f;
    [SerializeField] private float attackDistance = 0.6f;

    private Animator animator = null;
    private NavMeshAgent agent = null;

    private void Awake() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if (animator == null) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude < attackDistance) {
            // we are close enough to attack
            agent.enabled = false;
            animator.SetFloat("Forward", 0f);
            animator.SetInteger("AttackNum", Random.Range(1, 6));
            animator.SetTrigger("Attack");

            // face the target
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.1f);
        } else if (direction.magnitude < alertDistance) {
            // move toward the target
            agent.enabled = true;
            agent.SetDestination(target.position);
            animator.SetFloat("Forward", agent.velocity.magnitude);
        } else {
            Debug.Log("Distance to target: " + direction.magnitude);
        }
    }

    public void OnDrawGizmos() {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, alertDistance);
    }
}
