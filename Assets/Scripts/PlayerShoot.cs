using UnityEngine;

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
            Debug.Log("Hit an enemy: " + hit.collider.name);

            Health enemyHealth = hit.collider.GetComponent<Health>();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage(25);
                if (enemyHealth.IsDead()) {
                    Animator enemyAnimator = hit.collider.GetComponent<Animator>();
                    if (enemyAnimator != null) {
                        enemyAnimator.SetBool("IsDead", true);
                    }
                }
            }
        } else if (Physics.Raycast(cam.position, cam.forward, out hit, range, wallMask)) {
            Debug.Log("Hit a structure: " + hit.collider.name);
            if (bulletHolePrefab != null) {
                Instantiate(bulletHolePrefab, hit.point + (0.01f * hit.normal), Quaternion.LookRotation(-1 * hit.normal, hit.transform.up));
            }
        }

        gunAnimator.SetTrigger("Shoot");
    }
}
