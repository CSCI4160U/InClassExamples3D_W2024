using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform explosionPoint;
    [SerializeField] private int damage = 100;
    [SerializeField] private float forceAmount = 5000f;
    [SerializeField] private bool isExploded = false;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask barrelMask;

    private List<ExplodingBarrel> barrelsToExplode;

    private void Start() {
        barrelsToExplode = new List<ExplodingBarrel>();
    }

    public void Explode() {
        Instantiate(explosionPrefab, explosionPoint.position, Quaternion.identity);

        Destroy(transform.gameObject, 0.25f);

        // check for nearby players
        Collider[] hits = Physics.OverlapSphere(explosionPoint.position, explosionRadius, playerMask);
        for (int i = 0; i < hits.Length; i++) {
            Health playerHealth = hits[i].GetComponent<Health>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(damage);
            }
        }

        // check for nearby enemies
        hits = Physics.OverlapSphere(explosionPoint.position, explosionRadius, enemyMask);
        for (int i = 0; i < hits.Length; i++) {
            RagdollHealth enemyHealth = hits[i].GetComponent<RagdollHealth>();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage(damage);
                enemyHealth.TakeExplosionDamage(explosionPoint.position, forceAmount);
            }
        }

        // check for other barrels nearby
        isExploded = true;
        hits = Physics.OverlapSphere(explosionPoint.position, explosionRadius, barrelMask);
        barrelsToExplode.Clear();
        for (int i = 0; i < hits.Length; i++) {
            ExplodingBarrel barrel = hits[i].GetComponent<ExplodingBarrel>();
            if (!barrel.isExploded) {
                barrelsToExplode.Add(barrel);
            }
        }
        if (barrelsToExplode.Count > 0) {
            CascadeBarrels();
        }
    }

    public void CascadeBarrels() {
        foreach (ExplodingBarrel barrel in barrelsToExplode) {
            barrel.Explode();
        }
    }

    public void OnDrawGizmos() {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(explosionPoint.position, explosionRadius);
    }
}
