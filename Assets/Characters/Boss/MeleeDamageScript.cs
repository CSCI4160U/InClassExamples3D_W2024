using UnityEngine;

public class MeleeDamageScript : MonoBehaviour
{
    [SerializeField] private int damageInflicted = 5;

    public void OnTriggerEnter(Collider other) {
        Debug.Log("Melee damage on " + other.gameObject.name);

        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null) {
            targetHealth.TakeDamage(damageInflicted);
        }
    }
}
