using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private LayerMask enemymask;

    [SerializeField] private float range = 100f;
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private GameObject bulletHolePrefab;

    private void Update() {
        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    private void Shoot() {
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, range, enemymask)) {
            //Debug.Log("Ht an enemy: " + hit.collider.name);

            Health enemyHealth = hit.collider.GetComponent<Health>();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage(25);
                Animator enemyAnimator = hit.collider.GetComponent<Animator>();
                if (enemyAnimator != null) {
                    if (enemyHealth.IsDead()) {
                        enemyAnimator.SetBool("IsDead", true);
                        Rig interactRig = hit.collider.GetComponentInChildren<Rig>();
                        if (interactRig != null) {
                            interactRig.weight = 0f;
                        }
                    } else {
                        enemyAnimator.SetTrigger("Hit");
                    }
                }
            }
        } else if (Physics.Raycast(cam.position, cam.forward, out hit, range, wallMask)) {
            //Debug.Log("Hit a structure: " + hit.collider.name);
            if (bulletHolePrefab != null) {
                Instantiate(bulletHolePrefab, hit.point + (0.01f * hit.normal), Quaternion.LookRotation(-1 * hit.normal, hit.transform.up));
            }
        }

        gunAnimator.SetTrigger("Shoot");
    }
}
