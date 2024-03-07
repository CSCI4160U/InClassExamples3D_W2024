using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {
    Patrolling,
    Alerted,
    TargetVisible,
    Dead
}

public class EnemyStateMachine : MonoBehaviour
{

    [SerializeField] private EnemyState currentState = EnemyState.Patrolling;

    [Header("Patrolling")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int waypointIndex = 0;
    [SerializeField] private bool patrolLoop = true;
    [SerializeField] private float closeEnoughDistance = 1f;

    [Header("Alerted")]
    [SerializeField] private float lastAlertTime = 0f;
    [SerializeField] private float alertCooldown = 8f;
    [SerializeField] private Vector3 lastKnownTargetPosition;

    [Header("Target in Sight")]
    [SerializeField] private float lastShootTime = 0f;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private Transform target;

    private Animator animator = null;
    private NavMeshAgent agent = null;

    private void Start() {
        currentState = EnemyState.Patrolling;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if ((agent != null) && (waypoints.Length > 0)) {
            agent.SetDestination(waypoints[waypointIndex].position);
        }
    }

    public void Pause() {
        agent.enabled = false;
        animator.speed = 0f;
    }

    public void Resume() {
        agent.enabled = true;
        animator.speed = 1f;
    }

    public EnemyState GetState() {
        return currentState;
    }

    public void SetState(EnemyState state) {
        if (currentState == state) return;

        currentState = state;

        if (state == EnemyState.Patrolling) {
            // resume our patrol
            agent.enabled = true;
            waypointIndex = 0;
        } else if (state == EnemyState.Alerted) {
            // investigate the last known position
            agent.enabled = true;
            agent.SetDestination(lastKnownTargetPosition);

            // remember when we entered alerted state
            lastAlertTime = Time.time;
        } else if (state == EnemyState.TargetVisible) {
            // shoot at the target
            agent.enabled = false;
            animator.SetFloat("Forward", 0f);
            lastKnownTargetPosition = target.transform.position;
        } else if (state == EnemyState.Dead) {
            // disable the agent and play the death animation
            agent.enabled = false;
            animator.SetFloat("Forward", 0f);
            animator.SetBool("IsDead", true);
        }
    }

    public void SetTarget(Transform target) {
        this.target = target;
    }

    public void SetLastKnownTargetPosition(Vector3 targetPosition) {
        this.lastKnownTargetPosition = targetPosition;
    }

    void Update()
    {
        if (currentState == EnemyState.Dead) {
            return;
        } else if (currentState == EnemyState.Patrolling) {
            Patrol();
        } else if (currentState == EnemyState.Alerted) {
            // check if we have exceeded our timeout
            if (Time.time >= (lastAlertTime + alertCooldown)) {
                SetState(EnemyState.Patrolling);
                Patrol();
            } else {
                Alert();
            }
        } else if (currentState == EnemyState.TargetVisible) {
            Shoot();
        }
    }

    private void Patrol() {
        Vector3 destination = waypoints[waypointIndex].position;
        float distanceToDestination = Vector3.Distance(agent.transform.position, destination);
        if (distanceToDestination < closeEnoughDistance) {
            waypointIndex++;

            // loop if desired
            if (waypointIndex >= waypoints.Length) {
                if (patrolLoop) {
                    waypointIndex = 0;
                } else {
                    animator.SetFloat("Forward", 0f);
                    return;
                }
            }

            agent.SetDestination(waypoints[waypointIndex].position);
        }

        animator.SetFloat("Forward", agent.velocity.magnitude);
    }

    private void Alert() {
        float distanceToTarget = Vector3.Distance(agent.transform.position, lastKnownTargetPosition);
        if (distanceToTarget < closeEnoughDistance) {
            // stay here
            animator.SetFloat("Forward", 0f);

            // TODO: play look around animations
        } else {
            animator.SetFloat("Forward", agent.velocity.magnitude);
        }
    }

    private void Shoot() {
        // check if we've exceeded our cooldown
        if (Time.time >= (lastShootTime + shootCooldown)) {
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 0.15f);

            lastShootTime = Time.time;
            animator.SetTrigger("Shoot");

            // TODO: raycast to determine hit and damage
        }
    }
}
