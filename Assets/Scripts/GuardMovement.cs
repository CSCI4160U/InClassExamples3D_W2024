using UnityEngine;
using UnityEngine.AI;

public class GuardMovement : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float closeEnoughDistance = 1f;
    [SerializeField] private bool loop = false;

    private Animator animator;
    private NavMeshAgent agent;
    private int waypointIndex = 0;
    private bool patrolling = true;

    private void Awake() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        if ((agent != null) && (waypoints.Length > 0)) {
            agent.SetDestination(waypoints[waypointIndex].position);
        }
    }

    private void Update() {
        if (agent == null) return;

        if (!patrolling) {
            return;
        }

        float distanceToWaypoint = Vector3.Distance(agent.transform.position,
                                          waypoints[waypointIndex].position);
        if (distanceToWaypoint < closeEnoughDistance) {
            // go to the next waypoint
            waypointIndex++;

            if (waypointIndex >= waypoints.Length) {
                // no more waypoints
                if (loop) {
                    waypointIndex = 0;
                } else {
                    patrolling = false;
                    animator.SetFloat("Forward", 0f);
                    return;
                }
            } else {
                // there is a next waypoint

            }

            agent.SetDestination(waypoints[waypointIndex].position);
        }

        animator.SetFloat("Forward", agent.velocity.magnitude);
    }
}
