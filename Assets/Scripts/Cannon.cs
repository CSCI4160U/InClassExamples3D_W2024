using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Projectile")]

    [Tooltip("The object to be spawned")]
    [SerializeField] private Rigidbody projectile;

    [Space]

    [Header("Initial Position and Velocity")]

    [SerializeField] [Range(0, 100)] private float launchVelocity;
    [SerializeField] private Transform launchPoint;

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            Fire();
        }
    }

    public void Fire() {
        if (launchPoint != null) {
            var newProjectile = Instantiate(projectile, launchPoint.position, Quaternion.identity);
            Debug.Log("Projectile: " + newProjectile);
            newProjectile.velocity = launchPoint.forward * launchVelocity;
        }
    }
}
