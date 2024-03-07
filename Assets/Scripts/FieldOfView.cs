using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Vision")]
    [SerializeField] private Transform eye;
    [Range(0, 360)][SerializeField] private float fovAngle = 60f;
    [SerializeField] private float visionRadius = 10f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float targetScanDelay = 0.25f;

    [Header("State Management")]
    [SerializeField] private bool isAlerted = false;
    [SerializeField] private EnemyStateMachine stateMachine;

    void Start()
    {
        stateMachine = GetComponent<EnemyStateMachine>();

        StartCoroutine(KeepSearchingForTargets(targetScanDelay));
    }

    IEnumerator KeepSearchingForTargets(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            LookForTargets();
        }
    }

    private void LookForTargets() {
        // 1. any objects within our vision radius
        bool canSeeTarget = false;
        Collider[] targets = Physics.OverlapSphere(eye.position, visionRadius, targetLayer);

        for (int i = 0; i < targets.Length; i++) {
            Vector3 targetPos = targets[i].transform.position;
            targetPos.y += 1f;
            Vector3 targetDirection = (targetPos - eye.position).normalized;

            // 2. check if the object is within the FOV
            if (Vector3.Angle(eye.forward, targetDirection) < fovAngle) {
                // 3. check if we have a clear view of the object
                float distance = Vector3.Distance(eye.position, targetPos);

                if (!Physics.Raycast(eye.position, targetDirection, distance, wallLayer)) {
                    canSeeTarget = true;
                    isAlerted = true;

                    // update state machine
                    stateMachine.SetTarget(targets[i].transform);
                    stateMachine.SetState(EnemyState.TargetVisible);
                    return;
                }
            }

            if (!canSeeTarget && isAlerted) {
                stateMachine.SetState(EnemyState.Alerted);
                isAlerted = false;
            }
        }
    }

    public void OnDrawGizmos() {
        Gizmos.matrix = Matrix4x4.identity;

        Vector3 focusPos = eye.position + eye.forward * visionRadius;
        focusPos.y = eye.position.y;

        Gizmos.color = Color.green;
        if (stateMachine != null) {
            if (stateMachine.GetState() == EnemyState.TargetVisible) {
                Gizmos.color = Color.red;
            } else if (stateMachine.GetState() == EnemyState.Alerted) {
                Gizmos.color = Color.yellow;
            }
        }

        Gizmos.DrawWireSphere(eye.position, 0.2f);
        Gizmos.DrawWireSphere(eye.position, visionRadius);
        Gizmos.DrawLine(eye.position, focusPos);
    }
}
