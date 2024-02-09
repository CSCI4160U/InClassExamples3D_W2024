using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] private int maxHP = 100;
    [SerializeField] [Range(0, 100)] private int hp = 100;
    [SerializeField] private bool isDead = false;

    public void TakeDamage(int damageAmount) {
        this.hp -= damageAmount;

        if (this.hp <= 0) {
            this.hp = 0;
            this.isDead = true;
        } else {
            this.isDead = false;
        }
    }

    public bool IsDead() { return this.isDead; }
}
